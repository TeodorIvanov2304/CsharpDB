using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime dateToCompare = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var propertiesWithOwners = dbContext.Properties
                .Where(p => DateTime.Compare(p.DateOfAcquisition, dateToCompare) >= 0)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new PropertyExportDto
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = p.PropertiesCitizens.Select(o => new OwnerExportDto
                    {
                        LastName = o.Citizen.LastName,
                        MaritalStatus = o.Citizen.MaritalStatus.ToString()
                    })
                .OrderBy(o => o.LastName)
                .ToArray()
                })
            .ToArray();



            return JsonConvert.SerializeObject(propertiesWithOwners, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var propertiesWithDistrict = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new PropertyWithDistrictExportDto()
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy")
                })
                .ToArray();


            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Properties";
            return xmlHelper.Serialize(propertiesWithDistrict, xmlRoot);
        }
    }
}
