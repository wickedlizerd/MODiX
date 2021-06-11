using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ulong GuildId { get; init; }

        public ulong UserId { get; init; }

        public string? Nickname { get; init; }

        public long? PreviousVersionId { get; set; }

        public long? NextVersionId { get; set; }

        public DateTimeOffset Created { get; init; }

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
                .HasConversion<long>();

            entityBuilder
                .Property(x => x.UserId)
                .HasConversion<long>();

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
