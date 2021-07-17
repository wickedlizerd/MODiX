using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

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
        public AuditedActionsRepository(
            ILogger<AuditedActionsRepository>   logger,
            ModixDbContext                      modixDbContext)
        {
            _logger         = logger;
            _modixDbContext = modixDbContext;
        }

        public async Task<long> CreateAsync<TActionType>(
                TActionType         type,
                DateTimeOffset      performed,
                Snowflake?          performedById,
                CancellationToken   cancellationToken)
            where TActionType : struct, IConvertible
        {
            AuditingLogMessages.ActionCreating(_logger, type, performed, performedById);

            var action = new AuditedActionEntity()
            {
                TypeId          = type.ToInt32(null),
                Performed       = performed,
                PerformedById   = performedById
            };
            AuditingLogMessages.ActionInserting(_logger);
            await _modixDbContext.AddAsync(action, cancellationToken);

            AuditingLogMessages.ActionSaving(_logger);
            await _modixDbContext.SaveChangesAsync(cancellationToken);

            AuditingLogMessages.ActionCreated(_logger, action.Id, type, performed, performedById);
            return action.Id;
        }

        private readonly ILogger        _logger;
        private readonly ModixDbContext _modixDbContext;
    }
}
