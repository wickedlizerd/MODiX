using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Auditing
{
    [Table("AuditedActionTypes", Schema = "Auditing")]
    internal class AuditedActionTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }

        [Required]
        [ForeignKey(nameof(Category))]
        public AuditedActionCategory CategoryId { get; init; }

        [Required]
        public string Name { get; init; }
            = null!;

        public AuditedActionCategoryEntity Category { get; init; }
            = null!;
    }

    internal class AuditedActionTypeEntityTypeConfiguration
        : IEntityTypeConfiguration<AuditedActionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<AuditedActionTypeEntity> entityBuilder)
        {
            entityBuilder
                .Property(x => x.Id);

            entityBuilder
                .Property(x => x.CategoryId)
                .HasConversion<int>();

            entityBuilder
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
