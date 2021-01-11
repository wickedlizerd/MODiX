using System;
using System.Threading;
using System.Threading.Tasks;

using Remora.Commands.Parsers;
using Remora.Results;

namespace Modix.Bot.Parsers
{
    public class TimeSpanParser
        : AbstractTypeParser<TimeSpan>
    {
        public override ValueTask<RetrieveEntityResult<TimeSpan>> TryParse(string value, CancellationToken ct)
            => new(TimeSpan.TryParse(value, out var result)
                ? RetrieveEntityResult<TimeSpan>.FromSuccess(result)
                : RetrieveEntityResult<TimeSpan>.FromError("Invalid time-span format"));
    }
}
