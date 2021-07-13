using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Remora.Discord.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Users
{
    [Table("GuildUsers", Schema = "Users")]
    internal class GuildUserEntity
    {
        [Required]
        public Snowflake GuildId { get; init; }

        [Required]
        [ForeignKey(nameof(User))]
        public Snowflake UserId { get; init; }

        [Required]
        public DateTimeOffset FirstSeen { get; init; }

        [Required]
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
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.UserId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.FirstSeen)
                .HasConversion(DateTimeOffsetValueConverter.Default);

            entityBuilder
                .Property(x => x.LastSeen)
                .HasConversion(DateTimeOffsetValueConverter.Default);
        }
    }
}
