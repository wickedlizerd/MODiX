using Remora.Results;

namespace Modix.Data
{
    public record DataNotFoundError
        : ResultError
    {
        public DataNotFoundError(string dataDescription)
            : base($"Data not found: {dataDescription}") { }
    }
}
