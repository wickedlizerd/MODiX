using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Users
{
    [Table("GuildUsers", Schema = "Users")]
    internal class GuildUserEntity
    {
        public ulong GuildId { get; init; }

        [ForeignKey(nameof(User))]
        public ulong UserId { get; init; }

        public DateTimeOffset FirstSeen { get; init; }

        public DateTimeOffset LastSeen { get; set; }

        public UserEntity User { get; init; }
            = null!;
    }

    internal class GuildUserEntityTypeConfiguration
        : IEntityTypeConfiguration<GuildUserEntity>
    {
        public void Configure(EntityTypeBuilder<GuildUserEntity> entityBuilder)
        {
            entityBuilder
                .HasKey(x => new { x.GuildId, x.UserId });

            entityBuilder
                .Property(x => x.GuildId)
                .HasConversion<long>();

            entityBuilder
                .Property(x => x.UserId)
                .HasConversion<long>();

            entityBuilder
                .Property(x => x.FirstSeen)
                .HasConversion(DateTimeOffsetValueConverter.Default);

            entityBuilder
                .Property(x => x.LastSeen)
                .HasConversion(DateTimeOffsetValueConverter.Default);
        }
    }
}
