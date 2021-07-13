using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Auditing
{
    public enum AuditedActionCategory
    {
        Administration = 0x01000000,
    }

    internal class AdministrationActionCategoryDataConfiguration
        : IEntityTypeConfiguration<AuditedActionCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<AuditedActionCategoryEntity> entityBuilder)
        {
            foreach (var category in EnumEx.EnumerateValues<AuditedActionCategory>())
                entityBuilder.HasData(new AuditedActionCategoryEntity()
                {
                    Id      = category,
                    Name    = category.ToString()
                });
        }
    }
}
