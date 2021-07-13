using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modix.Data.Auditing;

namespace Modix.Data.Administration
{
    public enum AdministrationActionType
    {
        PermissionsEdit = 0x010000
    }

    internal class AdministrationActionCategoryDataConfiguration
        : IEntityTypeConfiguration<AuditedActionTypeEntity>
    {
        public void Configure(EntityTypeBuilder<AuditedActionTypeEntity> entityBuilder)
        {
            foreach (var type in EnumEx.EnumerateValues<AuditedActionCategory>())
                entityBuilder.HasData(new AuditedActionTypeEntity()
                {
                    Id          = (int)type,
                    CategoryId  = AuditedActionCategory.Administration,
                    Name        = type.ToString()
                });
        }
    }
}
