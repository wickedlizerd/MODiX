using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Modix.Data.Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auditing");

            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.EnsureSchema(
                name: "Permissions");

            migrationBuilder.CreateTable(
                name: "AuditedActionCategories",
                schema: "Auditing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditedActionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionCategories",
                schema: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditedActionTypes",
                schema: "Auditing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditedActionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditedActionTypes_AuditedActionCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Permissions",
                        principalTable: "PermissionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildUsers",
                schema: "Users",
                columns: table => new
                {
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUsers", x => new { x.GuildId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GuildUsers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVersions",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<int>(type: "integer", nullable: false),
                    AvatarHash = table.Column<string>(type: "text", nullable: true),
                    PreviousVersionId = table.Column<long>(type: "bigint", nullable: true),
                    NextVersionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVersions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVersions_UserVersions_NextVersionId",
                        column: x => x.NextVersionId,
                        principalSchema: "Users",
                        principalTable: "UserVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserVersions_UserVersions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalSchema: "Users",
                        principalTable: "UserVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditedActions",
                schema: "Auditing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    Performed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PerformedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditedActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditedActions_AuditedActionTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditedActions_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalSchema: "Users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildUserVersions",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    PreviousVersionId = table.Column<long>(type: "bigint", nullable: true),
                    NextVersionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUserVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildUserVersions_GuildUsers_GuildId_UserId",
                        columns: x => new { x.GuildId, x.UserId },
                        principalSchema: "Users",
                        principalTable: "GuildUsers",
                        principalColumns: new[] { "GuildId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildUserVersions_GuildUserVersions_NextVersionId",
                        column: x => x.NextVersionId,
                        principalSchema: "Users",
                        principalTable: "GuildUserVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildUserVersions_GuildUserVersions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalSchema: "Users",
                        principalTable: "GuildUserVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildPermissionMappingEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionId = table.Column<int>(type: "integer", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    GuildPermission = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: false),
                    DeletionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildPermissionMappingEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildPermissionMappingEntity_AuditedActions_CreationId",
                        column: x => x.CreationId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildPermissionMappingEntity_AuditedActions_DeletionId",
                        column: x => x.DeletionId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildPermissionMappingEntity_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Permissions",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionMappingEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionId = table.Column<int>(type: "integer", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: false),
                    DeletionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionMappingEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissionMappingEntity_AuditedActions_CreationId",
                        column: x => x.CreationId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionMappingEntity_AuditedActions_DeletionId",
                        column: x => x.DeletionId,
                        principalSchema: "Auditing",
                        principalTable: "AuditedActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissionMappingEntity_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Permissions",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Auditing",
                table: "AuditedActionCategories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 16777216, "Administration" });

            migrationBuilder.InsertData(
                schema: "Permissions",
                table: "PermissionCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 16777216, "Permissions related to administration of the application", "Administration" });

            migrationBuilder.InsertData(
                schema: "Auditing",
                table: "AuditedActionTypes",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[] { 16777216, 16777216, "Administration" });

            migrationBuilder.InsertData(
                schema: "Permissions",
                table: "Permissions",
                columns: new[] { "Id", "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 16843008, 16777216, "Allows reading of application permissions information", "PermissionsRead" },
                    { 16843264, 16777216, "Allows editing of application permissions assignments", "PermissionsEdit" },
                    { 16908544, 16777216, "Allows reading of application diagnostics information", "DiagnosticsRead" },
                    { 16908800, 16777216, "Allows execution of application diagnostics tests", "DiagnosticsExecute" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditedActionCategories_Name",
                schema: "Auditing",
                table: "AuditedActionCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditedActions_PerformedById",
                schema: "Auditing",
                table: "AuditedActions",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditedActions_TypeId",
                schema: "Auditing",
                table: "AuditedActions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditedActionTypes_CategoryId",
                schema: "Auditing",
                table: "AuditedActionTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditedActionTypes_Name",
                schema: "Auditing",
                table: "AuditedActionTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildPermissionMappingEntity_CreationId",
                table: "GuildPermissionMappingEntity",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildPermissionMappingEntity_DeletionId",
                table: "GuildPermissionMappingEntity",
                column: "DeletionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildPermissionMappingEntity_PermissionId",
                table: "GuildPermissionMappingEntity",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildUsers_UserId",
                schema: "Users",
                table: "GuildUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildUserVersions_GuildId_UserId",
                schema: "Users",
                table: "GuildUserVersions",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildUserVersions_NextVersionId",
                schema: "Users",
                table: "GuildUserVersions",
                column: "NextVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildUserVersions_PreviousVersionId",
                schema: "Users",
                table: "GuildUserVersions",
                column: "PreviousVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategories_Name",
                schema: "Permissions",
                table: "PermissionCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CategoryId_Name",
                schema: "Permissions",
                table: "Permissions",
                columns: new[] { "CategoryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMappingEntity_CreationId",
                table: "RolePermissionMappingEntity",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMappingEntity_DeletionId",
                table: "RolePermissionMappingEntity",
                column: "DeletionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMappingEntity_PermissionId",
                table: "RolePermissionMappingEntity",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVersions_NextVersionId",
                schema: "Users",
                table: "UserVersions",
                column: "NextVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserVersions_PreviousVersionId",
                schema: "Users",
                table: "UserVersions",
                column: "PreviousVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserVersions_UserId",
                schema: "Users",
                table: "UserVersions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildPermissionMappingEntity");

            migrationBuilder.DropTable(
                name: "GuildUserVersions",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "RolePermissionMappingEntity");

            migrationBuilder.DropTable(
                name: "UserVersions",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "GuildUsers",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "AuditedActions",
                schema: "Auditing");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Permissions");

            migrationBuilder.DropTable(
                name: "AuditedActionTypes",
                schema: "Auditing");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "PermissionCategories",
                schema: "Permissions");

            migrationBuilder.DropTable(
                name: "AuditedActionCategories",
                schema: "Auditing");
        }
    }
}
