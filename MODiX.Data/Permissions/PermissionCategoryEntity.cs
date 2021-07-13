using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Permissions
{
    [Table("PermissionCategories", Schema = "Permissions")]
    internal class PermissionCategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public PermissionCategory Id { get; init; }

        [Required]
        public string Name { get; init; }
            = null!;

        [Required]
        public string Description { get; init; }
            = null!;

        public ICollection<PermissionEntity> Permissions { get; init; }
            = null!;
    }

    internal class PermissionCategoryEntityTypeConifugration
        : IEntityTypeConfiguration<PermissionCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionCategoryEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Id)
                .HasConversion<int>();

            entityBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityBuilder
                .Property(x => x.Description);
        }
    }
}
