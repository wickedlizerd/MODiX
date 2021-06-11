using System;

namespace Modix.Business.Users.Tracking
{
    internal static class UserTrackingDefaults
    {
        public static readonly TimeSpan DefaultCacheTimeout
            = TimeSpan.FromMinutes(5);
    }
}
