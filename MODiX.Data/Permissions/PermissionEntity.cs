using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Permissions
{
    [Table("Permissions", Schema = "Permissions")]
    internal class PermissionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }

        [Required]
        [ForeignKey(nameof(Category))]
        public PermissionCategory CategoryId { get; init; }

        [Required]
        public string Name { get; init; }
            = null!;

        [Required]
        public string Description { get; init; }
            = null!;

        public PermissionCategoryEntity Category { get; init; }
            = null!;
    }

    internal class PermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Id);

            entityBuilder
                .Property(x => x.CategoryId)
                .HasConversion<int>();

            entityBuilder
                .HasIndex(x => new { x.CategoryId, x.Name })
                .IsUnique();

            entityBuilder
                .Property(x => x.Description);
        }
    }
}
