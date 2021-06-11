using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Users
{
    [Table("UserVersions", Schema = "Users")]
    internal class UserVersionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        [ForeignKey(nameof(User))]
        public ulong UserId { get; init; }

        [Required]
        public string Username { get; init; }
            = null!;

        public ushort Discriminator { get; init; }

        public string? AvatarHash { get; init; }

        public long? PreviousVersionId { get; set; }

        public long? NextVersionId { get; set; }

        public DateTimeOffset Created { get; init; }

        public UserEntity User { get; init; }
            = null!;

        public UserVersionEntity? PreviousVersion { get; set; }

        public UserVersionEntity? NextVersion { get; set; }
    }

    internal class UserVersionEntityTypeConfiguration
        : IEntityTypeConfiguration<UserVersionEntity>
    {
        public void Configure(EntityTypeBuilder<UserVersionEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.UserId)
                .HasConversion<long>();

            entityBuilder
                .Property(x => x.Created)
                .HasConversion(DateTimeOffsetValueConverter.Default);

            entityBuilder
                .HasOne(x => x.PreviousVersion)
                .WithOne()
                .HasForeignKey<UserVersionEntity>(x => x.PreviousVersionId);

            entityBuilder
                .HasOne(x => x.NextVersion)
                .WithOne()
                .HasForeignKey<UserVersionEntity>(x => x.NextVersionId);
        }
    }
}
