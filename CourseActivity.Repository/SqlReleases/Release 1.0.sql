IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190501141917_initial')
BEGIN
    CREATE TABLE [CourseUploads] (
        [Id] int NOT NULL IDENTITY,
        [DateCreated] datetime2 NOT NULL,
        [LastUpdated] datetime2 NULL,
        [CourseId] nvarchar(max) NULL,
        [CSVData] nvarchar(max) NULL,
        CONSTRAINT [PK_CourseUploads] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190501141917_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190501141917_initial', N'2.1.11-servicing-32099');
END;

GO

