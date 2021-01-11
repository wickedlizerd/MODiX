using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remora.Results
{
    public class AggregateResult
        : IResult
    {
        public static AggregateResult FromResults(IEnumerable<IResult> results)
            => new AggregateResult(results.ToArray());

        public AggregateResult(IReadOnlyList<IResult> innerResults)
            : base()
        {
            InnerResults = innerResults;

            var errorCount = 0;
            var errorReasonBuilder = new StringBuilder();
            foreach(var result in InnerResults)
            {
                if (!result.IsSuccess)
                {
                    ++errorCount;
                    if (errorReasonBuilder.Length > 0)
                        errorReasonBuilder.Append(Environment.NewLine);
                    errorReasonBuilder.Append(result.ErrorReason);
                }
            }
            if (errorCount > 1)
            {
                errorReasonBuilder.Insert(0, Environment.NewLine);
                errorReasonBuilder.Insert(0, "Multiple errors occurred:");
            }

            IsSuccess = (errorCount == 0);
            ErrorReason = errorReasonBuilder.ToString();
        }

        public IReadOnlyList<IResult> InnerResults { get; }

        public bool IsSuccess { get; }

        public string ErrorReason { get; }

        public Exception? Exception
            => null;
    }
}
