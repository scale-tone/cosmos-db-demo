using Microsoft.Azure.Cosmos;

class PatchTest
{
    static Container Container = Globals.Client.GetContainer("tino-db", "patch-test");

    public static async Task Test()
    {
        using var _ = Globals.ActivitySource.StartActivity("patch-test-activity");

        var sqlInstance = new SqlManagedInstance
        {
            Id = "tino-sql-123",
            CustomerId = Guid.Empty,
            Name = $"TinoTestSql1",
            MonthlyCost = 1000,
        };

        sqlInstance.Databases.Add(new SqlManagedDb
        {
            Name = "master",
            Size = 1
        });

        await Container.UpsertItemAsync(sqlInstance);

        var patchResult = await Container.PatchItemAsync<SqlManagedInstance>(
            sqlInstance.Id,
            new PartitionKey(sqlInstance.CustomerId.ToString()),

            patchOperations: [

                PatchOperation.Set($"/Databases/0/Size", 123),

                PatchOperation.Increment($"/MonthlyCost", 234)
            ]
        );

        Console.WriteLine($"New monthly cost: {patchResult.Resource.MonthlyCost}");
        Console.WriteLine($"New master size: {patchResult.Resource.Databases[0].Size}");
    }
}
