using Microsoft.Azure.Cosmos;

class ChangeFeedTest
{
    public static async Task Test()
    {
        using var _ = Globals.ActivitySource.StartActivity("change-feed-test-activity");

        var sourceContainer = Globals.Client.GetContainer("tino-db", "change-feed-test");
        var leaseContainer = Globals.Client.GetContainer("tino-db", "change-feed-lease");

        var changeFeedProcessor = sourceContainer
            .GetChangeFeedProcessorBuilder<SqlManagedInstance>("tinoChangeFeedProcessor", HandleChangesAsync)
            .WithInstanceName($"tinoChangeFeedProcessorInstance")
            .WithLeaseContainer(leaseContainer)
            .Build();

        await changeFeedProcessor.StartAsync();

        Console.WriteLine("Change Feed Processor started, press Enter to stop...");
        Console.ReadLine();

        await changeFeedProcessor.StopAsync();
    }

    static async Task HandleChangesAsync(
        ChangeFeedProcessorContext context,
        IReadOnlyCollection<SqlManagedInstance> changes,
        CancellationToken cancellationToken)
    {
        foreach (var instance in changes)
        {
            Console.WriteLine($"{instance.Id} changed. Name: {instance.Name}, Monthly Cost: {instance.MonthlyCost}");
        }
    }
}
