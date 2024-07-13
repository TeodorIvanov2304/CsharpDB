using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NewtonsoftJsonDemo.Models;


//Newtonsoft JSON

//Serialize:
Dog dog = new Dog()
{
    DogName = "Garo Garov",
    DogAge = 23,
    Weight = 100.1
};
//Use settings


var settings = new JsonSerializerSettings()
{

};
string data = JsonConvert.SerializeObject(dog);
Console.WriteLine(data);


Dog? dog1 = JsonConvert.DeserializeObject<Dog>(data);

//Change case style to SnakeCase
DefaultContractResolver contractResolver =
 new DefaultContractResolver()
 {
     NamingStrategy = new SnakeCaseNamingStrategy()
 };

var settings2 = new JsonSerializerSettings()
{
    ContractResolver = contractResolver,
};

var template = new
{
    FullName = string.Empty,
    Age = 0,
    Height = 0,
    Weight = 0.0
};
string data1 = JsonConvert.SerializeObject(dog1, settings);
Console.WriteLine(data1);


Console.WriteLine("************************************************");
//JObject
string dataForJObject = JsonConvert.SerializeObject(dog, settings2);
JObject pr = JObject.Parse(dataForJObject);

Console.WriteLine(pr.SelectToken("dog_name"));
