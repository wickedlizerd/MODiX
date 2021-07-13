using System;
using System.ComponentModel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modix.Data.Permissions
{
    public enum PermissionCategory
    {
        [Description("Permissions related to administration of the application")]
        Administration = 0x01000000,
    }

    internal class PermissionCategoryDataConfiguration
        : IEntityTypeConfiguration<PermissionCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionCategoryEntity> entityBuilder)
        {
            foreach (var (category, description) in EnumEx.EnumerateValuesWithDescriptions<PermissionCategory>())
                entityBuilder.HasData(new PermissionCategoryEntity()
                {
                    Id          = category,
                    Name        = category.ToString(),
                    Description = description
                });
        }
    }
}
