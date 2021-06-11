using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Users
{
    [Table("Users", Schema = "Users")]
    internal class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; init; }

        public DateTimeOffset FirstSeen { get; init; }

        public DateTimeOffset LastSeen { get; set; }
    }

    internal class UserEntityTypeConfiguration
        : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Id)
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
