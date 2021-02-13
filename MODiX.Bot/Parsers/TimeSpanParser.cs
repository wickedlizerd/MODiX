using System;
using System.Threading;
using System.Threading.Tasks;

using Modix.Bot.Extensions.Remora.Results;

using Remora.Commands.Parsers;
using Remora.Results;

namespace Modix.Bot.Parsers
{
    public class TimeSpanParser
        : AbstractTypeParser<TimeSpan>
    {
        public override ValueTask<Result<TimeSpan>> TryParse(string value, CancellationToken ct)
            => new(TimeSpan.TryParse(value, out var result)
                ? result
                : new ParsingError<TimeSpan>(value));
    }
}
