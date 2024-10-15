CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);



CREATE TABLE "Companies" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Companies" PRIMARY KEY AUTOINCREMENT,
    "CompanyName" TEXT NOT NULL
);

CREATE TABLE "Projects" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "CompanyId" INTEGER NOT NULL,
    "Company" TEXT NOT NULL,
    "DivisionId" INTEGER NOT NULL,
    "Division" TEXT NOT NULL,
    "BrandId" INTEGER NOT NULL,
    "Brand" TEXT NOT NULL,
    "Brief" TEXT NOT NULL,
    "References" TEXT NOT NULL,
    "Category" TEXT NULL,
    "Size" TEXT NULL,
    "Code" TEXT NOT NULL,
    "CreateDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL
);

CREATE TABLE "Role" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Role" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);

CREATE TABLE "Divisions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Divisions" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "CompanyId" INTEGER NOT NULL,
    CONSTRAINT "FK_Divisions_Companies_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_RoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_RoleClaims_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Brands" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Brands" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "DivisionId" INTEGER NOT NULL,
    CONSTRAINT "FK_Brands_Divisions_DivisionId" FOREIGN KEY ("DivisionId") REFERENCES "Divisions" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Brands_DivisionId" ON "Brands" ("DivisionId");

CREATE INDEX "IX_Divisions_CompanyId" ON "Divisions" ("CompanyId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "Role" ("NormalizedName");

CREATE INDEX "IX_RoleClaims_RoleId" ON "RoleClaims" ("RoleId");

CREATE INDEX "IX_UserClaims_UserId" ON "UserClaims" ("UserId");

CREATE INDEX "IX_UserLogins_UserId" ON "UserLogins" ("UserId");

CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "Users" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "Users" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240725084523_changed DB to sqlite', '8.0.7');





ALTER TABLE "Projects" ADD "BriefFile" TEXT NULL;

ALTER TABLE "Projects" ADD "ReferenceFile" TEXT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240725092841_added brief and referene file', '8.0.7');





CREATE TABLE "ef_temp_Projects" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY AUTOINCREMENT,
    "Brand" TEXT NULL,
    "BrandId" INTEGER NULL,
    "Brief" TEXT NULL,
    "BriefFile" TEXT NULL,
    "Category" TEXT NULL,
    "Code" TEXT NOT NULL,
    "Company" TEXT NULL,
    "CompanyId" INTEGER NULL,
    "CreateDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Division" TEXT NULL,
    "DivisionId" INTEGER NULL,
    "Name" TEXT NOT NULL,
    "ReferenceFile" TEXT NULL,
    "References" TEXT NULL,
    "Size" TEXT NULL
);

INSERT INTO "ef_temp_Projects" ("Id", "Brand", "BrandId", "Brief", "BriefFile", "Category", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "Name", "ReferenceFile", "References", "Size")
SELECT "Id", "Brand", "BrandId", "Brief", "BriefFile", "Category", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "Name", "ReferenceFile", "References", "Size"
FROM "Projects";



PRAGMA foreign_keys = 0;



DROP TABLE "Projects";

ALTER TABLE "ef_temp_Projects" RENAME TO "Projects";



PRAGMA foreign_keys = 1;



INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240805114844_changed to nullable', '8.0.7');





CREATE TABLE "ProjectReferences" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ProjectReferences" PRIMARY KEY AUTOINCREMENT,
    "File" TEXT NOT NULL,
    "ProjectId" INTEGER NOT NULL,
    CONSTRAINT "FK_ProjectReferences_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ProjectReferences_ProjectId" ON "ProjectReferences" ("ProjectId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240806154813_added project reference table', '8.0.7');





CREATE TABLE "Documents" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Documents" PRIMARY KEY AUTOINCREMENT,
    "Type" TEXT NOT NULL,
    "File" TEXT NOT NULL,
    "ProjectId" INTEGER NOT NULL,
    CONSTRAINT "FK_Documents_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Documents_ProjectId" ON "Documents" ("ProjectId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240806161333_added document table', '8.0.7');





DROP TABLE "ProjectReferences";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240806161828_removed project reference table', '8.0.7');





ALTER TABLE "Documents" RENAME COLUMN "File" TO "FileName";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240806165106_renamed column file to filename', '8.0.7');





ALTER TABLE "Documents" ADD "UniqueFileName" TEXT NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240807160452_added uniquefilenamefield in document tabe', '8.0.7');





CREATE TABLE "ef_temp_Projects" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY AUTOINCREMENT,
    "Brand" TEXT NULL,
    "BrandId" INTEGER NULL,
    "Brief" TEXT NULL,
    "Category" TEXT NULL,
    "Code" TEXT NOT NULL,
    "Company" TEXT NULL,
    "CompanyId" INTEGER NULL,
    "CreateDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Division" TEXT NULL,
    "DivisionId" INTEGER NULL,
    "Name" TEXT NOT NULL,
    "References" TEXT NULL,
    "Size" TEXT NULL
);

INSERT INTO "ef_temp_Projects" ("Id", "Brand", "BrandId", "Brief", "Category", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "Name", "References", "Size")
SELECT "Id", "Brand", "BrandId", "Brief", "Category", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "Name", "References", "Size"
FROM "Projects";



PRAGMA foreign_keys = 0;



DROP TABLE "Projects";

ALTER TABLE "ef_temp_Projects" RENAME TO "Projects";



PRAGMA foreign_keys = 1;



INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240808071036_removed documents from project table', '8.0.7');





ALTER TABLE "Projects" ADD "IsCompleted" INTEGER NOT NULL DEFAULT 0;

CREATE TABLE "ef_temp_Projects" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY AUTOINCREMENT,
    "Brand" TEXT NULL,
    "BrandId" INTEGER NULL,
    "Brief" TEXT NULL,
    "Code" TEXT NOT NULL,
    "Company" TEXT NULL,
    "CompanyId" INTEGER NULL,
    "CreateDate" TEXT NOT NULL,
    "CreatedBy" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Division" TEXT NULL,
    "DivisionId" INTEGER NULL,
    "IsCompleted" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "References" TEXT NULL
);

INSERT INTO "ef_temp_Projects" ("Id", "Brand", "BrandId", "Brief", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "IsCompleted", "Name", "References")
SELECT "Id", "Brand", "BrandId", "Brief", "Code", "Company", "CompanyId", "CreateDate", "CreatedBy", "Description", "Division", "DivisionId", "IsCompleted", "Name", "References"
FROM "Projects";



PRAGMA foreign_keys = 0;



DROP TABLE "Projects";

ALTER TABLE "ef_temp_Projects" RENAME TO "Projects";



PRAGMA foreign_keys = 1;



INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240808083411_removed size and category from project and added iscompleted', '8.0.7');





CREATE TABLE "ProjectTasks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ProjectTasks" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Category" TEXT NULL,
    "Size" TEXT NULL,
    "ProjectId" INTEGER NOT NULL,
    CONSTRAINT "FK_ProjectTasks_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ProjectTasks_ProjectId" ON "ProjectTasks" ("ProjectId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240808095554_added projecttask table', '8.0.7');



