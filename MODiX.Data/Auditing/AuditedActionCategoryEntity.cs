using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Auditing
{
    [Table("AuditedActionCategories", Schema = "Auditing")]
    internal class AuditedActionCategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public AuditedActionCategory Id { get; init; }

        [Required]
        public string Name { get; init; }
            = null!;
    }

    internal class AuditedActionCategoryEntityTypeConfiguration
        : IEntityTypeConfiguration<AuditedActionCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<AuditedActionCategoryEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Id)
                .HasConversion<int>();

            entityBuilder
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
