using JsonDemo.Models;
using System.Text.Json;

//Install-Package newtonsoft.json

Person person = new Person()
{
    FullName = "Garo Garov",
    Age = 23,
    Height = 190,
    Weight = 100.1d
};

JsonSerializerOptions options = new JsonSerializerOptions()
{   
    //Sort using pretty printing
    WriteIndented = true,
    //Use CamelCase
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    

};

//Serialize
string data = JsonSerializer.Serialize(person, options);
Console.WriteLine(data);

//Deserialize
Person? person1 = JsonSerializer.Deserialize<Person>(data, options);

Console.WriteLine($"{person1.FullName} is {person1.Age} years old and is {person1.Height} cm high!");




