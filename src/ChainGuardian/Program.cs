using Newtonsoft.Json;

public class Person
{
    public string Name { get; set; } = default!;
    public int Age { get; set; }
    public string Email { get; set; } = default!;
}

public class Program
{
    public static void Main(string[] args)
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com"
        };

        // Serialize the person object to JSON
        string json = JsonConvert.SerializeObject(person);
        Console.WriteLine("Serialized JSON:");
        Console.WriteLine(json);
    }
}
