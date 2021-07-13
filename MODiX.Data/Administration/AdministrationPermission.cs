using System;
using System.ComponentModel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modix.Data.Permissions;

namespace Modix.Data.Administration
{
    public enum AdministrationPermission
    {
        [Description("Allows reading of application permissions information")]
        PermissionsRead     = PermissionCategory.Administration + 0x010100,

        [Description("Allows editing of application permissions assignments")]
        PermissionsEdit     = PermissionCategory.Administration + 0x010200,

        [Description("Allows reading of application diagnostics information")]
        DiagnosticsRead     = PermissionCategory.Administration + 0x020100,

        [Description("Allows execution of application diagnostics tests")]
        DiagnosticsExecute  = PermissionCategory.Administration + 0x020200
    }

    internal class AdministrationPermissionDataConfiguration
        : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> entityBuilder)
        {
            foreach (var (value, description) in EnumEx.EnumerateValuesWithDescriptions<AdministrationPermission>())
                entityBuilder.HasData(new PermissionEntity()
                {
                    CategoryId      = PermissionCategory.Administration,
                    Id    = (int)value,
                    Name            = value.ToString(),
                    Description     = description
                });
        }
    }
}
