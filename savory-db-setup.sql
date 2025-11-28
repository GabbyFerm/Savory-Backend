IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [AvatarColor] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [Ingredients] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Ingredients] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [Recipes] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Instructions] nvarchar(max) NOT NULL,
        [PrepTime] int NOT NULL,
        [CookTime] int NOT NULL,
        [Servings] int NOT NULL,
        [ImagePath] nvarchar(max) NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Recipes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Recipes_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Recipes_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE TABLE [RecipeIngredients] (
        [RecipeId] uniqueidentifier NOT NULL,
        [IngredientId] uniqueidentifier NOT NULL,
        [Quantity] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_RecipeIngredients] PRIMARY KEY ([RecipeId], [IngredientId]),
        CONSTRAINT [FK_RecipeIngredients_Ingredients_IngredientId] FOREIGN KEY ([IngredientId]) REFERENCES [Ingredients] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RecipeIngredients_Recipes_RecipeId] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [CreatedAt], [Name], [UpdatedAt])
    VALUES (''1cc14a78-acfd-45fd-9f62-019d11fbd84a'', ''2025-11-27T14:52:00.0468977Z'', N''Beverage'', NULL),
    (''3eadde6e-6553-466f-944d-4619638bdcde'', ''2025-11-27T14:52:00.0468962Z'', N''Breakfast'', NULL),
    (''4065924a-9276-449b-a4a8-3613d6902e6d'', ''2025-11-27T14:52:00.0468973Z'', N''Dinner'', NULL),
    (''45960150-22bc-4373-9774-936d8649e3be'', ''2025-11-27T14:52:00.0468974Z'', N''Dessert'', NULL),
    (''5ee86808-027e-4821-9dbc-c7e7e474c68f'', ''2025-11-27T14:52:00.0468971Z'', N''Lunch'', NULL),
    (''703194bf-433f-42fb-ba97-fd7bf2da79c7'', ''2025-11-27T14:52:00.0468976Z'', N''Snack'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'Unit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Ingredients]'))
        SET IDENTITY_INSERT [Ingredients] ON;
    EXEC(N'INSERT INTO [Ingredients] ([Id], [CreatedAt], [Name], [Unit], [UpdatedAt])
    VALUES (''0031fc49-bffa-43bf-a3ac-186747d5a30d'', ''2025-11-27T14:52:00.0469154Z'', N''Milk'', N''ml'', NULL),
    (''033f85a3-ed49-4904-84b5-d3ad3e7cf64b'', ''2025-11-27T14:52:00.0469137Z'', N''Butter'', N''g'', NULL),
    (''1479d39f-1c79-45fd-afa2-bb1b310d3c44'', ''2025-11-27T14:52:00.0469143Z'', N''Tomato'', N''pcs'', NULL),
    (''1f79f995-5c70-458a-9652-1fa1f67b04fe'', ''2025-11-27T14:52:00.0469141Z'', N''Garlic'', N''cloves'', NULL),
    (''20e34178-e4b5-437d-b433-d9f5401c28fc'', ''2025-11-27T14:52:00.0469134Z'', N''Pasta'', N''g'', NULL),
    (''2410c70c-b047-490d-bbef-cb1747976866'', ''2025-11-27T14:52:00.0469138Z'', N''Olive Oil'', N''ml'', NULL),
    (''4e4a0dcc-121f-4154-a54d-a58a968bd629'', ''2025-11-27T14:52:00.0469148Z'', N''Eggs'', N''pcs'', NULL),
    (''59774212-5dfe-4c3e-851b-542c25d58070'', ''2025-11-27T14:52:00.0469150Z'', N''Sugar'', N''g'', NULL),
    (''5c36a30f-7d23-4980-af67-15568ad028c9'', ''2025-11-27T14:52:00.0469151Z'', N''Salt'', N''g'', NULL),
    (''71392fb4-bce4-45b2-87e9-1d8bdcc5f964'', ''2025-11-27T14:52:00.0469136Z'', N''Rice'', N''g'', NULL),
    (''7572461c-25dd-4fed-beb6-ad24d56a496e'', ''2025-11-27T14:52:00.0469140Z'', N''Parmesan'', N''g'', NULL),
    (''775b8a55-790d-4d73-8bd3-366924668c08'', ''2025-11-27T14:52:00.0469146Z'', N''Chicken Breast'', N''g'', NULL),
    (''874c853a-6c8b-43ff-9472-ad0348a5e57c'', ''2025-11-27T14:52:00.0469142Z'', N''Onion'', N''pcs'', NULL),
    (''8a48d6f1-152e-4615-b58d-2f70343aa0ef'', ''2025-11-27T14:52:00.0469153Z'', N''Black Pepper'', N''g'', NULL),
    (''cca0d0c1-06c6-4d97-aac7-76dfb24cf3ab'', ''2025-11-27T14:52:00.0469149Z'', N''Flour'', N''g'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Name', N'Unit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Ingredients]'))
        SET IDENTITY_INSERT [Ingredients] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_RecipeIngredients_IngredientId] ON [RecipeIngredients] ([IngredientId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_Recipes_CategoryId] ON [Recipes] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    CREATE INDEX [IX_Recipes_UserId] ON [Recipes] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251127145200_SeedIngredients'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251127145200_SeedIngredients', N'8.0.22');
END;
GO

COMMIT;
GO

