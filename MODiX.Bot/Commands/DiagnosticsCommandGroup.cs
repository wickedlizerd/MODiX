using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
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

namespace Modix.Bot.Commands
{
    public class DiagnosticsCommandGroup
        : CommandGroup
    {
        public const string CancelButtonEmoji
            = "❌";

        public DiagnosticsCommandGroup(
            IReactionButtonFactory buttonFactory,
            IDiscordRestChannelAPI channelApi,
            IDiagnosticsService diagnosticsService,
            ICommandContext context)
        {
            _buttonFactory = buttonFactory;
            _channelApi = channelApi;
            _context = context;
            _diagnosticsService = diagnosticsService;
        }

        [Command("echo")]
        public async Task<OperationResult> EchoAsync(string value)
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: value);
            return result.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(result);
        }

        [Command("delay")]
        public async Task<OperationResult> DelayAsync(TimeSpan duration)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var messageCreationResult = await _channelApi.CreateMessageAsync(
                channelID:  _context.ChannelID,
                content:    $"Delaying for {duration.Humanize()}...");
            if (!messageCreationResult.IsSuccess)
                return OperationResult.FromError(messageCreationResult);

            var whenCancelledSource = new TaskCompletionSource();

            var buttonCreationResult = await _buttonFactory.CreateAsync(
                channelId:      _context.ChannelID,
                messageId:      messageCreationResult.Entity.ID,
                emoji:          "❌",
                onClickedAsync: _ =>
                {
                    whenCancelledSource.SetResult();
                    return Task.FromResult(OperationResult.FromSuccess());
                });
            if (!buttonCreationResult.IsSuccess)
                return OperationResult.FromError(buttonCreationResult);

            using var button = buttonCreationResult.Control;

            stopwatch.Stop();
            var remaining = duration - stopwatch.Elapsed;
            if (remaining > TimeSpan.Zero)
            {
                try
                {
                    await Task.WhenAny(
                        Task.Delay(remaining),
                        whenCancelledSource.Task);
                }
                catch (TaskCanceledException) { }
            }

            var editMessageResult = await _channelApi.EditMessageAsync(
                channelID:  _context.ChannelID,
                messageID:  messageCreationResult.Entity.ID,
                content:    $"Delay ({duration.Humanize()}) {(whenCancelledSource.Task.IsCompleted ? "cancelled" : "completed")}");
            if (!editMessageResult.IsSuccess)
                return OperationResult.FromError(editMessageResult);

            var destroyResult = await button.DestroyAsync();
            return destroyResult.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(destroyResult);
        }

        [Command("ping")]
        public async Task<OperationResult> PingAsync()
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: "Pong!");
            return result.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(result);
        }

        [Command("pingtest")]
        public async Task<OperationResult> PingTestAsync()
        {
            if (_diagnosticsService.PingTestEndpointCount == 0)
            {
                var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: "No endpoints configured for ping testing");
                return result.IsSuccess
                    ? OperationResult.FromSuccess()
                    : OperationResult.FromError(result);
            }

            var createMessageResult = await _channelApi.CreateMessageAsync(_context.ChannelID, content: $"Pinging {_diagnosticsService.PingTestEndpointCount} endpoints...");
            if (!createMessageResult.IsSuccess)
                return OperationResult.FromError(createMessageResult);

            var pingTestOutcomes = await _diagnosticsService.PerformPingTestAsync(CancellationToken.None);

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
                Title:          $"Pong! Checked {pingTestOutcomes.Count} endpoints",
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
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(editMessageResult);

            string FormatOutcome(PingTestOutcome outcome)
                => outcome switch
                {
                    _ when (outcome.Latency >= TimeSpan.Zero)   => FormatLatency(outcome.Latency.Value.TotalMilliseconds),
                    _                                           => outcome.Status.ToString().Humanize() + " ⚠️"
                };

            string FormatLatency(double latency)
                => $"{latency: 0}ms " + latency switch
                {
                    _ when (latency > 300)  => "💔",
                    _ when (latency > 100)  => "💛", // Yellow heart
                    _                       => "💚" // Green heart
                };

            Optional<Color> GetLatencyColor(double? latency)
                => latency switch
                {
                    _ when (latency is null)    => Color.Red,
                    _ when (latency > 300)      => Color.Red,
                    _ when (latency > 100)      => Color.Gold,
                    _ when (latency <= 100)     => Color.Green,
                    _                           => default
                };
        }

        private readonly IReactionButtonFactory _buttonFactory;
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _context;
        private readonly IDiagnosticsService _diagnosticsService;
    }
}
