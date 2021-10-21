using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modix.Data.Users
{
    public interface IUsersRepositorySynchronizer
    {
        ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken);
    }

    internal class UsersRepositorySynchronizer
        : IUsersRepositorySynchronizer
    {
        public UsersRepositorySynchronizer()
            => _asyncMutex = new();

        public ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
            => _asyncMutex.LockAsync(cancellationToken);

        private readonly AsyncMutex _asyncMutex;
    }
}
