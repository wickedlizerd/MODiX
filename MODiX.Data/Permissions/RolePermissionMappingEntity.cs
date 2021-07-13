using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Remora.Discord.Core;

using Modix.Data.Auditing;

namespace Modix.Data.Permissions
{
    internal class RolePermissionMappingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        [Required]
        [ForeignKey(nameof(Permission))]
        public int PermissionId { get; init; }

        [Required]
        public Snowflake GuildId { get; init; }

        [Required]
        public Snowflake RoleId { get; init; }

        [Required]
        public PermissionMappingType Type { get; init; }

        [Required]
        [ForeignKey(nameof(Creation))]
        public long CreationId { get; init; }

        [ForeignKey(nameof(Deletion))]
        public long? DeletionId { get; set; }

        public PermissionEntity Permission { get; init; }
            = null!;

        public AuditedActionEntity Creation { get; init; }
            = null!;

        public AuditedActionEntity? Deletion { get; set; }
    }

    internal class RolePermissionMappingEntityTypeConfiguration
        : IEntityTypeConfiguration<RolePermissionMappingEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionMappingEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.GuildId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.RoleId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.Type)
                .HasConversion<string>();
        }
    }
}
