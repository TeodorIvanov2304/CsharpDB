namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Cadastre.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Districts";

            ImportDistrictDto[] districts = xmlHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, xmlRoot);
            ICollection<District> districtsToImport = new List<District>();


            foreach (ImportDistrictDto districtDto in districts)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (districtsToImport.Any(dn => dn.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Property> propertiesToImport = new List<Property>();

                foreach (var propertyDto in districtDto.Properties)
                {
                    if (!IsValid(propertyDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property newProperty = new Property()
                    {
                        PropertyIdentifier = propertyDto.PropertyIdentifier,
                        Area = propertyDto.Area,
                        Details = propertyDto.Details,
                        Address = propertyDto.Address,
                        DateOfAcquisition = DateTime.ParseExact(propertyDto.DateOfAcquisition,"dd/MM/yyyy", CultureInfo.InvariantCulture)
                    };

                    if (propertiesToImport.Any(pi => pi.PropertyIdentifier == newProperty.PropertyIdentifier || pi.Address == newProperty.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    propertiesToImport.Add(newProperty);
                    
                }

                District newDistrict = new District() 
                {   
                    
                    Name = districtDto.Name,
                    Region = districtDto.Region,
                    PostalCode = districtDto.PotalCode,
                    Properties = propertiesToImport
                };

                districtsToImport.Add(newDistrict);
                sb.AppendLine(String.Format(SuccessfullyImportedDistrict, newDistrict.Name, newDistrict.Properties.Count));
            }

            dbContext.Districts.AddRange(districtsToImport);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new();
            var citizensDtos = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);
            ICollection<Citizen> citizensToImport = new List<Citizen>();

            foreach (var citizenDto in citizensDtos)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (citizenDto.MaritalStatus != "Unmarried"
                    && citizenDto.MaritalStatus != "Married"
                    && citizenDto.MaritalStatus != "Divorced"
                    && citizenDto.MaritalStatus != "Widowed")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<int> propertiesToImport = new();

                Citizen newCitizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = DateTime.ParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citizenDto.MaritalStatus)
                };

                foreach (var id in citizenDto.Properties)
                {
                    PropertyCitizen newPropertyCitizen = new PropertyCitizen() 
                    {
                        CitizenId = newCitizen.Id,
                        PropertyId = id
                    };

                    newCitizen.PropertiesCitizens.Add(newPropertyCitizen);
                }

                

                citizensToImport.Add(newCitizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, newCitizen.FirstName, newCitizen.LastName, newCitizen.PropertiesCitizens.Count));
            }

            dbContext.Citizens.AddRange(citizensToImport);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
