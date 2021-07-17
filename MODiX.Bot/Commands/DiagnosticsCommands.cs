using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
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
            IDiscordRestChannelAPI channelApi,
            IDiagnosticsManager diagnosticsManager,
            IDiagnosticsService diagnosticsService,
            ICommandContext context)
        {
            _channelApi = channelApi;
            _context = context;
            _diagnosticsManager = diagnosticsManager;
            _diagnosticsService = diagnosticsService;
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
        public Task<IResult> DelayAsync(TimeSpan duration)
            => Task.FromException<IResult>(new NotImplementedException());

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
                embeds:     new[] { embed });
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
        public Task<IResult> ServerClockAsync()
            => Task.FromException<IResult>(new NotImplementedException());

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _context;
        private readonly IDiagnosticsManager _diagnosticsManager;
        private readonly IDiagnosticsService _diagnosticsService;
    }
}
