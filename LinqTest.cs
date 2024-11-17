
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

static class LinqTest
{
    static Container Container = Globals.Client.GetContainer("tino-db", "linq-test");




    public static void Test()
    {
        using var _ = Globals.ActivitySource.StartActivity("linq-test-activity");

        var query = Container.GetItemLinqQueryable<SqlManagedInstance>(allowSynchronousQueryExecution: true);

        var resultList = query.ToList();

        Console.WriteLine(resultList.SelectMany(i => i.Databases).Count());
    }





    public static async Task TestAsync()
    {
        using var _ = Globals.ActivitySource.StartActivity("linq-async-test-activity");

        var query = Container.GetItemLinqQueryable<SqlManagedInstance>();

        using var feed = query.ToFeedIterator();

        int c = 0;
        while (feed.HasMoreResults)
        {
            using var __ = Globals.ActivitySource.StartActivity("batch-read-activity");

            foreach (var item in await feed.ReadNextAsync())
            {
                c++;
            }
        }

        Console.WriteLine($"Items read: {c}");
    }


    public static async Task ProjectionTestAsync()
    {
        using var _ = Globals.ActivitySource.StartActivity("projection-test-activity");

        var query = Container.GetItemLinqQueryable<SqlManagedInstance>()
            .Select(i => i.Name);

        using var feed = query.ToFeedIterator();

        int c = 0;
        while (feed.HasMoreResults)
        {
            using var __ = Globals.ActivitySource.StartActivity("batch-read");

            foreach (var item in await feed.ReadNextAsync())
            {
                c++;
            }
        }

        Console.WriteLine($"Items read: {c}");
    }



    public static void AdvancedQueryTest()
    {
        using var _ = Globals.ActivitySource.StartActivity("advanced-query-test-activity");

/*
        var query = Container.GetItemLinqQueryable<SqlManagedInstance>(true)
            .SelectMany(i => i.Databases.Select(d => new { instance = i, database = d }))
            .OrderBy(r => r.database.Size)
            .Take(5)
            .Select(r => r.instance.Name + " " + r.database.Size);


        var query = Container.GetItemLinqQueryable<SqlManagedInstance>(true)
            .OrderByDescending(i => i.MaxDbSize)
            .Take(5)
            .Select(r => r.Name + " " + r.MaxDbSize.ToString());

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
*/

        var query = Container.GetItemLinqQueryable<SqlManagedInstance>(true)
            .OrderByDescending(i => i.MaxDbSize)
            .Take(5);

        foreach (var item in query)
        {
            Console.WriteLine(item.Name);
        }
    }




    public static async Task IAsyncEnumerableTest()
    {
        using var _ = Globals.ActivitySource.StartActivity("iasyncenumerable-test-activity");

        var query = Container.GetItemLinqQueryable<SqlManagedInstance>();

        int c = 0;
        await foreach (var item in query.ToAsyncEnumerable())
        {
            c++;
        }

        Console.WriteLine($"Items read: {c}");
    }






    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IQueryable<T> query)
    {
        using var feed = query.ToFeedIterator();
        while (feed.HasMoreResults)
        {
            foreach (var item in await feed.ReadNextAsync())
            {
                yield return item;
            }
        }
    }

    public static async Task CreateMany()
    {
        for (int x = 0; x < 1000; x++)
        {
            var managedInstance = new SqlManagedInstance
            {
                Name = $"TinoTestSql{x}"
            };

            for (int i = 0; i < 5; i++)
            {
                var managedDb = new SqlManagedDb
                {
                    Name = $"TinoTestDb{i}",
                    Size = Random.Shared.Next(1, 100)
                };

                for (int j = 0; j < 5; j++)
                {
                    managedDb.Tables.Add($"TinoTestTable{j}");
                }

                managedInstance.Databases.Add(managedDb);
            }

            await Container.UpsertItemAsync(managedInstance);
        }
    }
}
