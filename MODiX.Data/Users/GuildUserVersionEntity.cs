using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Remora.Discord.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Users
{
    [Table("GuildUserVersions", Schema = "Users")]
    internal class GuildUserVersionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        [Required]
        public Snowflake GuildId { get; init; }

        [Required]
        public Snowflake UserId { get; init; }

        [Required]
        public DateTimeOffset Created { get; init; }

        public string? Nickname { get; init; }

        public long? PreviousVersionId { get; set; }

        public long? NextVersionId { get; set; }

        public GuildUserEntity GuildUser { get; init; }
            = null!;

        public GuildUserVersionEntity? PreviousVersion { get; set; }

        public GuildUserVersionEntity? NextVersion { get; set; }
    }

    internal class GuildUserVersionEntityTypeConfiguration
        : IEntityTypeConfiguration<GuildUserVersionEntity>
    {
        public void Configure(EntityTypeBuilder<GuildUserVersionEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.GuildId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.UserId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.Created)
                .HasConversion(DateTimeOffsetValueConverter.Default);

            entityBuilder
                .HasOne(x => x.GuildUser)
                .WithMany()
                .HasForeignKey(x => new { x.GuildId, x.UserId });

            entityBuilder
                .HasOne(x => x.PreviousVersion)
                .WithOne()
                .HasForeignKey<GuildUserVersionEntity>(x => x.PreviousVersionId);

            entityBuilder
                .HasOne(x => x.NextVersion)
                .WithOne()
                .HasForeignKey<GuildUserVersionEntity>(x => x.NextVersionId);
        }
    }
}
