//XML string
using System.Xml.Linq;
using System.Xml.Serialization;
using XMLDemo;

string xmlString = @"<?xml version=""1.0""?>
<library name=""Developer's Library"">
    <book>
        <title>Professional C# and .NET</title>
        <author>Christian Nagel</author>
        <isbn>978-0-470-50225-9</isbn>
    </book>
    <book>
        <title>Teach Yourself XML in 10 Minutes</title>
        <author>Andrew H. Watt</author>
        <isbn>978-0-672-32471-0</isbn>
    </book>
</library>";


//Create XML Document
XDocument doc =  XDocument.Parse(xmlString);

//Show Root values
Console.WriteLine(doc.Root.Value);

//Show the fist title
Console.WriteLine(doc.Root
    .Descendants()
    .First()
    .Elements()
    .First().Value);

//Show the first author
Console.WriteLine(doc.Root
    .Descendants()
    .First()
    .Elements()
    .First(a=>a.Name == "author").Value);

//Add new element
doc.Root
    .Descendants()
    .First()
    .SetElementValue("issueDate", "2024-07-25");
//Save the new element
doc.Save("test.xml");

//Set attribute
doc.Root
    .Descendants()
    .First()
    .SetAttributeValue("issueDate", "2024-07-25");

doc.Save("test.xml");

//Loading from file
//XDocument xmlDoc = XDocument.Load("../../books.xml");

//Create XML Document
XDocument document = new();
document.Add(
    new XElement("class",
        new XElement("student",
        new XElement("name", "Stamo"),
        new XAttribute("grade", "C#")),
        new XElement("student",
        new XElement("name", "Pehso"),
        new XAttribute("grade", "cigulka")))
    );

document.Save("students.xml");

//Create XML Document 2
XDocument doc2 = new();
doc2.Add(new XElement("cars"));
doc2.Root.Add(new XElement("car"));
XElement car = doc2.Root.Elements().First();
car.Add(new XElement("make", "Renault"));
car.Add(new XElement("model", "Megan"));
doc2.Save("cars.xml",SaveOptions.None);


//Print all elements on all levels recursively 
Console.WriteLine("Print all elements on all levels recursively ");
int level = 0;
PrintStructure(doc.Elements(), level);


//Serializing classes

var family = new Family()
{
    FamilyName = "Petrovi",
    Members = new Person[]
    {
        new Person
        {
            Name = "Qvor",
            Age = 20
        },
        new Person
        {
            Name = "Penka",
            Age = 10
        }
    }
};

XmlSerializer serializer = new XmlSerializer(typeof(Family), new XmlRootAttribute("Family"));
using (StreamWriter writer = new StreamWriter("family.xml"))
{
    serializer.Serialize(writer, family);
};

    void PrintStructure(IEnumerable<XElement> elements, int level)
    {
        int newLevel = ++level;

        //Bottom
        if (elements.Count() == 0)
        {
            return;
        }

        foreach (var element in elements)
        {
            Console.WriteLine($"{new string(' ', newLevel * 2)} {element.Name}");
            PrintStructure(elements.Elements(), newLevel);
        }
    }
