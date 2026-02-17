namespace GolMetrics.API.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "Integration";
}