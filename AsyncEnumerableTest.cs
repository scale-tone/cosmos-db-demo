
static class AsyncEnumerableTest
{
    static IEnumerable<string> Produce()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return i.ToString();
        }
    }

    public static void Consume()
    {
        foreach (var item in Produce())
        {
            Console.WriteLine(item);
        }

        // OR

        var en = Produce().GetEnumerator();
        while (en.MoveNext())
        {
            Console.WriteLine(en.Current);
        }
    }




    static async IAsyncEnumerable<string> ProduceAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Yield();

            yield return i.ToString();
        }
    }

    public static async Task ConsumeAsync()
    {
        await foreach (var item in ProduceAsync())
        {
            Console.WriteLine(item);
        }

        // OR

        var en = ProduceAsync().GetAsyncEnumerator();
        while (await en.MoveNextAsync())
        {
            Console.WriteLine(en.Current);
        }
    }
}
