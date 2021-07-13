using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Remora.Discord.Core;

using Modix.Data.Users;

namespace Modix.Data.Auditing
{
    [Table("AuditedActions", Schema = "Auditing")]
    internal class AuditedActionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        [Required]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; init; }

        [Required]
        public DateTimeOffset Performed { get; init; }

        [ForeignKey(nameof(PerformedBy))]
        public Snowflake? PerformedById { get; init; }

        public AuditedActionTypeEntity Type { get; init; }
            = null!;

        public UserEntity? PerformedBy { get; init; }
    }

    internal class AuditedActionEntityTypeConfiguration
        : IEntityTypeConfiguration<AuditedActionEntity>
    {
        public void Configure(EntityTypeBuilder<AuditedActionEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Performed);

            entityBuilder
                .Property(x => x.PerformedById)
                .HasConversion(SnowflakeValueConverter.Default);
        }
    }
}
