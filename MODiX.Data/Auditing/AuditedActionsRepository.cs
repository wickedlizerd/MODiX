using System;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.Core;

namespace Modix.Data.Auditing
{
    public interface IAuditedActionsRepository
    {
        Task<long> CreateAsync<TActionType>(
                TActionType         type,
                DateTimeOffset      performed,
                Snowflake?          performedById,
                CancellationToken   cancellationToken)
            where TActionType : struct, IConvertible;
    }

    internal class AuditedActionsRepository
        : IAuditedActionsRepository
    {
        public AuditedActionsRepository(ModixDbContext modixDbContext)
            => _modixDbContext = modixDbContext;

        public async Task<long> CreateAsync<TActionType>(
                TActionType         type,
                DateTimeOffset      performed,
                Snowflake?          performedById,
                CancellationToken   cancellationToken)
            where TActionType : struct, IConvertible
        {
            var action = new AuditedActionEntity()
            {
                TypeId          = type.ToInt32(null),
                Performed       = performed,
                PerformedById   = performedById
            };
            await _modixDbContext.AddAsync(action, cancellationToken);

            await _modixDbContext.SaveChangesAsync(cancellationToken);

            return action.Id;
        }

        private readonly ModixDbContext _modixDbContext;
    }
}
