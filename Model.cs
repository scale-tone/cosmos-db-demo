
using Newtonsoft.Json;
using System.Text.RegularExpressions;

class SqlManagedInstance
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public Guid CustomerId { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public int MonthlyCost { get; set; }

    //    public DateTimeOffset LastAccessedAt { get; set; }

    public List<SqlManagedDb> Databases { get; set; } = [];

    public int MaxDbSize => this.Databases.Count == 0 ? 0 : this.Databases.Max(d => d.Size);

    public string AddressString { get; set; } = null!;

/*
    private AddressRecord _addressRecord = null!;
    public AddressRecord AddressRecord 
    { 
        get 
        {
            return this._addressRecord ??= AddressRecord.Parse(this.AddressString);
        }
        set 
        {
            this._addressRecord = value;
        } 
    }
*/
}

class SqlManagedDb
{
    public string Name { get; set; } = null!;

    public int Size { get; set; }

    public List<string> Tables { get; set; } = [];
}





record AddressRecord(string StreetAddress, int ZipCode, string City, string Country)
{
    static readonly Regex AddressRegex = new Regex("([\\w\\s]+), (\\d+) (\\w+), (\\w+)", RegexOptions.Compiled);

    public static AddressRecord Parse(string str)
    {
        if (str == null)
        {
            return null!;
        }

        var match = AddressRegex.Match(str);
        if (!match.Success)
        {
            return null!;
        }

        return new AddressRecord(match.Groups[1].Value, int.Parse(match.Groups[2].Value), match.Groups[3].Value, match.Groups[4].Value);
    }
}
