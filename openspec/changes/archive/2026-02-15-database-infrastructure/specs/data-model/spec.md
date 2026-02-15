## ADDED Requirements

### Requirement: Conversation Entity

The system SHALL define a `Conversation` entity inheriting from `Entity` in `Features/Chat/`.

#### Scenario: Conversation properties

- **WHEN** a Conversation entity is created
- **THEN** it SHALL include `Title` (string), `UserId` (Guid, FK to User)
- **AND** it SHALL have a navigation property to its `User` and a collection of `Message` entities

### Requirement: Message Entity

The system SHALL define a `Message` entity inheriting from `Entity` in `Features/Chat/`.

#### Scenario: Message properties

- **WHEN** a Message entity is created
- **THEN** it SHALL include `Content` (string), `Role` (MessageRole enum), `ConversationId` (Guid, FK to Conversation), `Timestamp` (DateTime)
- **AND** `MessageRole` SHALL be an enum with values `User` and `Assistant`

### Requirement: CachedQuery Entity

The system SHALL define a `CachedQuery` entity inheriting from `Entity` in `Features/FootballData/`.

#### Scenario: CachedQuery properties

- **WHEN** a CachedQuery entity is created
- **THEN** it SHALL include `QueryHash` (string), `Endpoint` (string), `Params` (string, JSONB), `ResponseData` (string, JSONB), `ExpiresAt` (DateTime)

### Requirement: User Entity Configuration

The system SHALL define `UserConfiguration` implementing `IEntityTypeConfiguration<User>` in `Features/UserManagement/`.

#### Scenario: User configuration

- **WHEN** the User entity is configured
- **THEN** it SHALL have a unique index on `Email`
- **AND** `EncryptedApiKey` SHALL have a max length of 512
- **AND** `Version` SHALL be configured as a concurrency token (row version)
- **AND** audit fields (`CreatedBy`, `CreatedAtUtc`) SHALL be configured as required

### Requirement: Conversation Entity Configuration

The system SHALL define `ConversationConfiguration` extending `EntityConfiguration<Conversation>` in `Features/Chat/`.

#### Scenario: Conversation configuration

- **WHEN** the Conversation entity is configured
- **THEN** it SHALL have a foreign key from `UserId` to `User` with cascade delete
- **AND** `Title` SHALL have a max length of 200

### Requirement: Message Entity Configuration

The system SHALL define `MessageConfiguration` extending `EntityConfiguration<Message>` in `Features/Chat/`.

#### Scenario: Message configuration

- **WHEN** the Message entity is configured
- **THEN** it SHALL have a foreign key from `ConversationId` to `Conversation` with cascade delete
- **AND** `Role` SHALL be stored as a string via `HasConversion<string>()`
- **AND** it SHALL have a composite index on `ConversationId` and `Timestamp`

### Requirement: CachedQuery Entity Configuration

The system SHALL define `CachedQueryConfiguration` extending `EntityConfiguration<CachedQuery>` in `Features/FootballData/`.

#### Scenario: CachedQuery configuration

- **WHEN** the CachedQuery entity is configured
- **THEN** it SHALL have a unique index on `QueryHash`
- **AND** `Params` and `ResponseData` SHALL use JSONB column type
- **AND** it SHALL have an index on `ExpiresAt`

### Requirement: DbContext DbSet Properties

The system SHALL register DbSet properties on `GolMetricsDbContext` for all domain entities.

#### Scenario: DbSet registration

- **WHEN** the DbContext is used
- **THEN** it SHALL expose `DbSet<Conversation>`, `DbSet<Message>`, and `DbSet<CachedQuery>` properties
