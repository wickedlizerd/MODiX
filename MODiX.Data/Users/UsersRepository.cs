using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.EntityFrameworkCore;

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
            ModixDbContext              modixDbContext,
            ITransactionScopeFactory    transactionScopeFactory)
        {
            _modixDbContext             = modixDbContext;
            _transactionScopeFactory    = transactionScopeFactory;
        }

        public async Task MergeAsync(
            UserMergeModel      model,
            CancellationToken   cancellationToken)
        {
            using var transactionScope = _transactionScopeFactory.CreateScope();

            await MergeInternalAsync(model, cancellationToken);

            await _modixDbContext.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();
        }

        public async Task MergeAsync(
            IEnumerable<UserMergeModel> models,
            CancellationToken           cancellationToken)
        {
            using var transactionScope = _transactionScopeFactory.CreateScope();

            foreach(var model in models)
                await MergeInternalAsync(model, cancellationToken);

            await _modixDbContext.SaveChangesAsync(cancellationToken);

            transactionScope.Complete();
        }

        private async Task MergeInternalAsync(
            UserMergeModel      model,
            CancellationToken   cancellationToken)
        {
            var user = await _modixDbContext.FindAsync<UserEntity?>(new object[] { model.UserId }, cancellationToken);
            if (user is null)
            {
                user = new UserEntity()
                {
                    Id          = model.UserId,
                    FirstSeen   = model.Timestamp
                };
                await _modixDbContext.AddAsync(user, cancellationToken);
            }
            user.LastSeen = model.Timestamp;

            var currentUserVersion = await _modixDbContext.Set<UserVersionEntity>()
                .AsQueryable()
                .Where(uv => (uv.UserId == model.UserId) && (uv.NextVersionId == null))
                .FirstOrDefaultAsync(cancellationToken);
            if ((currentUserVersion is null)
                || (model.Username.IsSpecified && (model.Username.Value != currentUserVersion.Username))
                || (model.Discriminator.IsSpecified && (model.Discriminator.Value != currentUserVersion.Discriminator))
                || (model.AvatarHash.IsSpecified && (model.AvatarHash.Value != currentUserVersion.AvatarHash)))
            {
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
                await _modixDbContext.AddAsync(newUserVersion, cancellationToken);
                if (currentUserVersion is not null)
                    currentUserVersion.NextVersion = newUserVersion;
            }

            var guildUser = await _modixDbContext.FindAsync<GuildUserEntity?>(new object[] { model.GuildId, model.UserId }, cancellationToken);
            if (guildUser is null)
            {
                guildUser = new GuildUserEntity()
                {
                    GuildId     = model.GuildId,
                    UserId      = model.UserId,
                    FirstSeen   = model.Timestamp
                };
                await _modixDbContext.AddAsync(guildUser, cancellationToken);
            }
            guildUser.LastSeen = model.Timestamp;

            var currentGuildUserVersion = await _modixDbContext.Set<GuildUserVersionEntity>()
                .Where(guv => (guv.GuildId == model.GuildId) && (guv.UserId == model.UserId) && (guv.NextVersionId == null))
                .FirstOrDefaultAsync(cancellationToken);
            if ((currentGuildUserVersion is null)
                || (model.Nickname.IsSpecified && (model.Nickname.Value != currentGuildUserVersion.Nickname)))
            {
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
                await _modixDbContext.AddAsync(newGuildUserVersion, cancellationToken);
                if (currentGuildUserVersion is not null)
                    currentGuildUserVersion.NextVersion = newGuildUserVersion;
            }
        }

        private readonly ModixDbContext             _modixDbContext;
        private readonly ITransactionScopeFactory   _transactionScopeFactory;
    }
}
