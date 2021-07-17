using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Modix.Data.Users
{
    public interface IUsersRepository
    {
        Task MergeAsync(
            UserMergeModel      model,
            CancellationToken   cancellationToken);

        Task MergeAsync(
            IEnumerable<UserMergeModel> models,
            CancellationToken           cancellationToken);
    }

    internal class UsersRepository
        : IUsersRepository
    {
        public UsersRepository(
            ILogger<UsersRepository>    logger,
            ModixDbContext              modixDbContext,
            ITransactionScopeFactory    transactionScopeFactory)
        {
            _logger                     = logger;
            _modixDbContext             = modixDbContext;
            _transactionScopeFactory    = transactionScopeFactory;
        }

        public async Task MergeAsync(
            UserMergeModel      model,
            CancellationToken   cancellationToken)
        {
            UsersLogMessages.UserMerging(_logger, model);

            using var transactionScope = _transactionScopeFactory.CreateScope();

            await MergeInternalAsync(model, cancellationToken);

            await _modixDbContext.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();

            UsersLogMessages.UserMerged(_logger, model);
        }

        public async Task MergeAsync(
            IEnumerable<UserMergeModel> models,
            CancellationToken           cancellationToken)
        {
            UsersLogMessages.UsersMerging(_logger);

            using var transactionScope = _transactionScopeFactory.CreateScope();

            foreach(var model in models)
                await MergeInternalAsync(model, cancellationToken);

            await _modixDbContext.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();

            UsersLogMessages.UsersMerged(_logger);
        }

        private async Task MergeInternalAsync(
            UserMergeModel      model,
            CancellationToken   cancellationToken)
        {
            UsersLogMessages.UserRetrieving(_logger, model.UserId);
            var user = await _modixDbContext.FindAsync<UserEntity?>(new object[] { model.UserId }, cancellationToken);
            if (user is not null)
                UsersLogMessages.UserRetrieved(_logger, model.UserId);
            else
            {
                UsersLogMessages.UserNotFound(_logger, model.UserId);
                user = new UserEntity()
                {
                    Id          = model.UserId,
                    FirstSeen   = model.Timestamp
                };

                UsersLogMessages.UserCreating(_logger, user);
                await _modixDbContext.AddAsync(user, cancellationToken);
                UsersLogMessages.UserCreated(_logger, user);
            }
            user.LastSeen = model.Timestamp;

            UsersLogMessages.UserCurrentVersionRetrieving(_logger, model.UserId);
            var currentUserVersion = await _modixDbContext.Set<UserVersionEntity>()
                .AsQueryable()
                .Where(uv => (uv.UserId == model.UserId) && (uv.NextVersionId == null))
                .FirstOrDefaultAsync(cancellationToken);
            if ((currentUserVersion is not null)
                    && (!model.Username.IsSpecified         || (model.Username.Value        == currentUserVersion.Username))
                    && (!model.Discriminator.IsSpecified    || (model.Discriminator.Value   == currentUserVersion.Discriminator))
                    && (!model.AvatarHash.IsSpecified       || (model.AvatarHash.Value      == currentUserVersion.AvatarHash)))
                UsersLogMessages.UserCurrentVersionUpToDate(_logger, model.UserId, currentUserVersion.Id);
            else
            {
                UsersLogMessages.UserCurrentVersionOutOfDate(_logger, model.UserId, currentUserVersion?.Id);
                var newUserVersion = new UserVersionEntity()
                {
                    UserId              = model.UserId,
                    Created             = model.Timestamp,
                    Username            = model.Username.IsSpecified
                        ? model.Username.Value
                        : currentUserVersion?.Username ?? "UNKNOWN",
                    Discriminator       = model.Discriminator.IsSpecified
                        ? model.Discriminator.Value
                        : currentUserVersion?.Discriminator ?? 0,
                    AvatarHash          = model.AvatarHash.IsSpecified
                        ? model.AvatarHash.Value
                        : currentUserVersion?.AvatarHash,
                    PreviousVersionId   = currentUserVersion?.Id
                };

                UsersLogMessages.UserVersionCreating(_logger, newUserVersion);
                await _modixDbContext.AddAsync(newUserVersion, cancellationToken);
                if (currentUserVersion is not null)
                    currentUserVersion.NextVersion = newUserVersion;
                UsersLogMessages.UserVersionCreated(_logger, newUserVersion);
            }

            UsersLogMessages.GuildUserRetrieving(_logger, model.GuildId, model.UserId);
            var guildUser = await _modixDbContext.FindAsync<GuildUserEntity?>(new object[] { model.GuildId, model.UserId }, cancellationToken);
            if (guildUser is not null)
                UsersLogMessages.GuildUserRetrieved(_logger, model.GuildId, model.UserId);
            else
            {
                UsersLogMessages.GuildUserNotFound(_logger, model.GuildId, model.UserId);
                guildUser = new GuildUserEntity()
                {
                    GuildId     = model.GuildId,
                    UserId      = model.UserId,
                    FirstSeen   = model.Timestamp
                };

                UsersLogMessages.GuildUserCreating(_logger, guildUser);
                await _modixDbContext.AddAsync(guildUser, cancellationToken);
                UsersLogMessages.GuildUserCreated(_logger, guildUser);
            }
            guildUser.LastSeen = model.Timestamp;

            UsersLogMessages.GuildUserCurrentVersionRetrieving(_logger, model.GuildId, model.UserId);
            var currentGuildUserVersion = await _modixDbContext.Set<GuildUserVersionEntity>()
                .Where(guv => (guv.GuildId == model.GuildId) && (guv.UserId == model.UserId) && (guv.NextVersionId == null))
                .FirstOrDefaultAsync(cancellationToken);
            if ((currentGuildUserVersion is not null)
                    && (!model.Nickname.IsSpecified || (model.Nickname.Value == currentGuildUserVersion.Nickname)))
                UsersLogMessages.GuildUserCurrentVersionUpToDate(_logger, model.GuildId, model.UserId, currentGuildUserVersion.Id);
            else
            {
                UsersLogMessages.GuildUserCurrentVersionOutOfDate(_logger, model.GuildId, model.UserId, currentGuildUserVersion?.Id);
                var newGuildUserVersion = new GuildUserVersionEntity()
                {
                    GuildId             = model.GuildId,
                    UserId              = model.UserId,
                    Created             = model.Timestamp,
                    Nickname            = model.Nickname.IsSpecified
                        ? model.Nickname.Value
                        : currentGuildUserVersion?.Nickname,
                    PreviousVersionId   = currentGuildUserVersion?.Id
                };

                UsersLogMessages.GuildUserVersionCreating(_logger, newGuildUserVersion);
                await _modixDbContext.AddAsync(newGuildUserVersion, cancellationToken);
                if (currentGuildUserVersion is not null)
                    currentGuildUserVersion.NextVersion = newGuildUserVersion;
                UsersLogMessages.GuildUserVersionCreated(_logger, newGuildUserVersion);
            }
        }

        private readonly ILogger                    _logger;
        private readonly ModixDbContext             _modixDbContext;
        private readonly ITransactionScopeFactory   _transactionScopeFactory;
    }
}
