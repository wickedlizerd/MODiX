using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Core;

using Modix.Data.Auditing;

namespace Modix.Data.Permissions
{
    internal class GuildPermissionMappingEntity
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
        public DiscordPermission GuildPermission { get; init; }

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

    internal class GuildPermissionMappingEntityTypeConfiguration
        : IEntityTypeConfiguration<GuildPermissionMappingEntity>
    {
        public void Configure(EntityTypeBuilder<GuildPermissionMappingEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.GuildId)
                .HasConversion(SnowflakeValueConverter.Default);

            entityBuilder
                .Property(x => x.GuildPermission)
                .HasConversion<string>();

            entityBuilder
                .Property(x => x.Type)
                .HasConversion<string>();
        }
    }
}
