using System.Threading;
using System.Threading.Tasks;

namespace Modix.Business.Messaging
{
    public interface INotificationHandler { }

    public interface INotificationHandler<TNotification>
            : INotificationHandler
        where TNotification : notnull
    {
        Task HandleNotificationAsync(
            TNotification notification,
            CancellationToken cancellationToken);
    }
}
