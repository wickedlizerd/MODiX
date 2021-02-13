using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

using Humanizer;

using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Core;
using Remora.Results;

using Modix.Bot.Controls;
using Modix.Business.Diagnostics;
using Modix.Web.Protocol.Diagnostics;

namespace Modix.Bot.Commands
{
    public class DiagnosticsCommands
        : CommandGroup
    {
        public const string CancelButtonEmoji
            = "❌";

        public DiagnosticsCommands(
            IReactionButtonFactory buttonFactory,
            IDiscordRestChannelAPI channelApi,
            IDiagnosticsManager diagnosticsManager,
            IDiagnosticsService diagnosticsService,
            IMessageDialogFactory dialogFactory,
            ICommandContext context)
        {
            _buttonFactory = buttonFactory;
            _channelApi = channelApi;
            _context = context;
            _diagnosticsManager = diagnosticsManager;
            _diagnosticsService = diagnosticsService;
            _dialogFactory = dialogFactory;
        }

        [Command("echo")]
        public async Task<IResult> EchoAsync([Greedy]string value)
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: value);
            return result.IsSuccess
                ? Result.FromSuccess()
                : Result.FromError(result.Error);
        }

        [Command("delay")]
        public async Task<IResult> DelayAsync(TimeSpan duration)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var messageCreationResult = await _channelApi.CreateMessageAsync(
                channelID:  _context.ChannelID,
                content:    $"Delaying for {duration.Humanize()}...");
            if (!messageCreationResult.IsSuccess)
                return Result.FromError(messageCreationResult);

            await using var button = await _buttonFactory.CreateAsync(
                guildId:            (_context is MessageContext messageContext) && messageContext.Message.GuildID.HasValue
                    ? messageContext.Message.GuildID.Value
                    : null as Snowflake?,
                channelId:  _context.ChannelID,
                messageId:  messageCreationResult.Entity.ID,
                emojiName:  "❌");

            stopwatch.Stop();
            var wasCancelled = false;
            var remaining = duration - stopwatch.Elapsed;
            if (remaining > TimeSpan.Zero)
                await Task.WhenAny(
                    Task.Delay(remaining),
                    button.Clicked
                        .Take(1)
                        .Do(_ => wasCancelled = true)
                        .ToTask());

            var editMessageResult = await _channelApi.EditMessageAsync(
                channelID:  _context.ChannelID,
                messageID:  messageCreationResult.Entity.ID,
                content:    $"Delay ({duration.Humanize()}) {(wasCancelled ? "cancelled" : "completed")}");
            return editMessageResult.IsSuccess
                ? Result.FromSuccess()
                : Result.FromError(editMessageResult);
        }

        [Command("ping")]
        public async Task<IResult> PingAsync()
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: "Pong!");
            return result.IsSuccess
                ? Result.FromSuccess()
                : Result.FromError(result);
        }

        [Command("pingtest")]
        public async Task<IResult> PingTestAsync()
        {
            if (_diagnosticsService.PingTestEndpointNames.Length == 0)
            {
                var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: "No endpoints configured for ping testing");
                return result.IsSuccess
                    ? Result.FromSuccess()
                    : Result.FromError(result);
            }

            var createMessageResult = await _channelApi.CreateMessageAsync(_context.ChannelID, content: $"Pinging {_diagnosticsService.PingTestEndpointNames.Length} endpoints...");
            if (!createMessageResult.IsSuccess)
                return Result.FromError(createMessageResult);

            var pingTestOutcomes = await _diagnosticsService.PerformPingTest()
                .ToArrayAsync();

            var embedFields = pingTestOutcomes
                .Select(outcome => new EmbedField(
                    Name:       outcome.EndpointName,
                    Value:      "📶 " + FormatOutcome(outcome),
                    IsInline:   true))
                .ToList();

            var averageLatency = pingTestOutcomes
                .Select(result => result.Latency?.TotalMilliseconds)
                .Where(latency => latency.HasValue)
                .Average();

            if (averageLatency.HasValue)
                embedFields.Add(new EmbedField(
                    Name:       "Average",
                    Value:      "📈" + FormatLatency(averageLatency.Value),
                    IsInline:   true));

            var embed = new Embed(
                Title:          $"Pong! Checked {pingTestOutcomes.Length} endpoints",
                Type:           EmbedType.Rich,
                Description:    (embedFields.Count == 0)
                    ? "No endpoints configured"
                    : default(Optional<string>),
                Timestamp:      DateTimeOffset.UtcNow,
                Colour:         GetLatencyColor(averageLatency),
                Fields:         embedFields);

            var editMessageResult = await _channelApi.EditMessageAsync(
                channelID:  createMessageResult.Entity.ChannelID,
                messageID:  createMessageResult.Entity.ID,
                content:    "",
                embed:      embed);
            return editMessageResult.IsSuccess
                ? Result.FromSuccess()
                : Result.FromError(editMessageResult);

            string FormatOutcome(PingTestOutcome outcome)
                => outcome switch
                {
                    _ when outcome.Latency >= TimeSpan.Zero => FormatLatency(outcome.Latency.Value.TotalMilliseconds),
                    _                                       => outcome.Status.ToString().Humanize() + " ⚠️"
                };

            string FormatLatency(double latency)
                => $"{latency: 0}ms " + latency switch
                {
                    _ when latency > 300    => "💔",
                    _ when latency > 100    => "💛", // Yellow heart
                    _                       => "💚" // Green heart
                };

            Optional<Color> GetLatencyColor(double? latency)
                => latency switch
                {
                    _ when latency is null  => Color.Red,
                    _ when latency > 300    => Color.Red,
                    _ when latency > 100    => Color.Gold,
                    _ when latency <= 100   => Color.Green,
                    _                       => default
                };
        }

        [Command("serverclock")]
        public async Task<IResult> ServerClockAsync()
        {
            await using var dialog = await _dialogFactory.CreateAsync(
                guildId:            (_context is MessageContext messageContext) && messageContext.Message.GuildID.HasValue
                    ? messageContext.Message.GuildID.Value
                    : null as Snowflake?,
                channelId:          _context.ChannelID,
                buttonEmojiNames:   new[] { "❌" },
                content:            RenderServerTime(await _diagnosticsManager.Now.Take(1)));

            await _diagnosticsManager.Now
                .Sample(TimeSpan.FromSeconds(5))
                .SelectMany(now => dialog.UpdateAsync(content: RenderServerTime(now)))
                .TakeUntil(dialog.ButtonClicked);

            return Result.FromSuccess();

            static string RenderServerTime(DateTimeOffset serverTime)
                => $"Server Time: {serverTime:yyyy-MMM-dd hh:mm:ss tt K}";
        }

        private readonly IReactionButtonFactory _buttonFactory;
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _context;
        private readonly IDiagnosticsManager _diagnosticsManager;
        private readonly IDiagnosticsService _diagnosticsService;
        private readonly IMessageDialogFactory _dialogFactory;
    }
}
