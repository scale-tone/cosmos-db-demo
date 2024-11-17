using Microsoft.Azure.Cosmos;

class TransactionTest
{
    static Container Container = Globals.Client.GetContainer("tino-db", "transaction-test");

    public static async Task Test()
    {
        using var _ = Globals.ActivitySource.StartActivity("transaction-test-activity");

        var customerId = Guid.Parse("9EEE32E9-B475-4AE2-8E7A-D99AA3628EC2");

        var batch = Container
            .CreateTransactionalBatch(new PartitionKey(customerId.ToString()))
            .UpsertItem(new SqlManagedInstance
            {
                Id = "TinoTestSql1",
                Name = "TinoTestSql1",
                CustomerId = customerId,
            })
            .UpsertItem(new SqlManagedInstance
            {
                Id = "TinoTestSql2",
                Name = "TinoTestSql2",
                CustomerId = customerId,
            })
            .UpsertItem(new SqlManagedInstance
            {
                Id = "TinoTestSql3",
                Name = "TinoTestSql3",
                CustomerId = customerId,
            });

        using var transactionResult = await batch.ExecuteAsync();

        Console.WriteLine($"Succeeded?: {transactionResult.IsSuccessStatusCode}");
    }
}
