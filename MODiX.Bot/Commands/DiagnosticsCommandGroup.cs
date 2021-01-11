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
using Remora.Discord.Gateway.Results;
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
            IDiscordRestChannelAPI channelApi,
            IDiagnosticsService diagnosticsService,
            IMessageDialogFactory dialogFactory,
            ICommandContext context)
        {
            _channelApi = channelApi;
            _context = context;
            _diagnosticsService = diagnosticsService;
            _dialogFactory = dialogFactory;
        }

        [Command("echo")]
        public async Task<IResult> EchoAsync(string value)
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: value);
            return result.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(result);
        }

        [Command("delay")]
        public async Task<IResult> DelayAsync(TimeSpan duration)
        {
            //var createMessageResult = await _channelApi.CreateMessageAsync(_context.ChannelID, content: $"Delaying for {duration}...");
            //if (!createMessageResult.IsSuccess)
            //    return EventResponseResult.FromError(createMessageResult);

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            //using var cancellationTokenSource = new CancellationTokenSource();

            //using var _1 = _reactionAddDelegator.ObserveWith(gatewayEvent =>
            //{
            //    if (gatewayEvent is not null)
            //        CheckForCancelReaction(gatewayEvent.MessageID, gatewayEvent.UserID, gatewayEvent.Emoji);
            //});
            //using var _2 = _reactionRemoveDelegator.ObserveWith(gatewayEvent =>
            //{
            //    if (gatewayEvent is not null)
            //        CheckForCancelReaction(gatewayEvent.MessageID, gatewayEvent.UserID, gatewayEvent.Emoji);
            //});

            //var createReactionResult = await _channelApi.CreateReactionAsync(
            //    channelID:  createMessageResult.Entity.ChannelID,
            //    messageID:  createMessageResult.Entity.ID,
            //    emoji:      CancelButtonEmoji);
            //if (!createReactionResult.IsSuccess)
            //    return EventResponseResult.FromError(createReactionResult);

            //stopwatch.Stop();
            //var remaining = duration - stopwatch.Elapsed;
            //try
            //{
            //    if (remaining > TimeSpan.Zero)
            //        await Task.Delay(remaining, cancellationTokenSource.Token);
            //}
            //catch (TaskCanceledException) { }

            //var editMessageResult = await _channelApi.EditMessageAsync(
            //    channelID:  createMessageResult.Entity.ChannelID,
            //    messageID:  createMessageResult.Entity.ID,
            //    content:    $"{duration} delay {(cancellationTokenSource.IsCancellationRequested ? "cancelled" : "completed")}");
            //if (!editMessageResult.IsSuccess)
            //    return EventResponseResult.FromError(editMessageResult);

            //var clearReactionsResult = await _channelApi.DeleteAllReactionsForEmojiAsync(
            //    channelID:  createMessageResult.Entity.ChannelID,
            //    messageID:  createMessageResult.Entity.ID,
            //    emoji:      CancelButtonEmoji);
            //return clearReactionsResult.IsSuccess
            //    ? EventResponseResult.FromSuccess()
            //    : EventResponseResult.FromError(clearReactionsResult);

            //void CheckForCancelReaction(Snowflake messageId, Snowflake userId, IPartialEmoji emoji)
            //{
            //    if ((messageId.Value == createMessageResult.Entity.ID.Value)
            //            && (userId.Value != createMessageResult.Entity.Author.ID.Value)
            //            && emoji.Name.HasValue
            //            && (emoji.Name.Value == CancelButtonEmoji))
            //        cancellationTokenSource.Cancel();
            //}

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var whenCancelledSource = new TaskCompletionSource();

            var dialogCreationResult = await _dialogFactory.CreateAsync(
                buttons:    Enumerable.Empty<ReactionButtonOptions>()
                    .Append(new(
                        emoji:          "❌",
                        onClickedAsync: _ =>
                        {
                            whenCancelledSource.SetResult();
                            return Task.FromResult(OperationResult.FromSuccess());
                        })),
                channelId:  _context.ChannelID,
                content:    $"Delaying for {duration.Humanize()}...");
            if (!dialogCreationResult.IsSuccess)
                return OperationResult.FromError(dialogCreationResult);

            using var dialog = dialogCreationResult.Control;

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

            var updateResult = await dialog.UpdateAsync(
                content: $"Delay ({duration.Humanize()}) {(whenCancelledSource.Task.IsCompleted ? "cancelled" : "completed")}");
            if (!updateResult.IsSuccess)
                return OperationResult.FromError(updateResult);

            var destroyResult = await dialog.DestroyAsync();
            return destroyResult.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(destroyResult);
        }

        [Command("ping")]
        public async Task<IResult> PingAsync()
        {
            var result = await _channelApi.CreateMessageAsync(_context.ChannelID, content: "Pong!");
            return result.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(result);
        }

        [Command("pingtest")]
        public async Task<IResult> PingTestAsync()
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
                return EventResponseResult.FromError(createMessageResult);

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

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _context;
        private readonly IDiagnosticsService _diagnosticsService;
        private readonly IMessageDialogFactory _dialogFactory;
    }
}
