using Microsoft.Azure.Cosmos;

class SchemaTest
{
    static Container Container = Globals.Client.GetContainer("tino-db", "schema-test");

    public static async Task Test()
    {
        using var _ = Globals.ActivitySource.StartActivity("schema-test-activity");

        var sqlInstance = new SqlManagedInstance
        {
            Id = "tino-sql-123",
            CustomerId = Guid.Empty,
            Name = $"TinoTestSql1",
            MonthlyCost = Random.Shared.Next(0, 1000),
            AddressString = "Dronning Eufemias gate 71, 0194 Oslo, Norway"
        };

        var response = await Container.UpsertItemAsync(sqlInstance);

//        Console.WriteLine($"Street Address: {response.Resource.AddressRecord.StreetAddress}");
    }
}
