namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.RegularExpressions;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;
    using static Boardgames.Data.DataConstraints;
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper helper = new();
            const string xmlRoot = "Creators";

            ICollection<Creator> creatorsToImport = new List<Creator>();
            ImportCreatorDto[] creators = helper.Deserialize<ImportCreatorDto[]>(xmlString, xmlRoot);

            foreach (var creatorDto in creators)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                ICollection<Boardgame> boardgamesToImport = new List<Boardgame>();

                foreach (ImportCreatorBoardgameDto boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame newBoardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics
                    };

                    boardgamesToImport.Add(newBoardgame);
                }

                Creator newCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                    Boardgames = boardgamesToImport
                };

                creatorsToImport.Add(newCreator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creatorDto.FirstName, creatorDto.LastName, boardgamesToImport.Count));
            }

            context.Creators.AddRange(creatorsToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellerDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;
            ICollection<Seller> sellersToImport = new List<Seller>();

            var existingBoardgameIds = new HashSet<int>(context.Boardgames.Select(b => b.Id));
            foreach (var sellerDto in sellerDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<BoardgameSeller> boardgamesToImport = new HashSet<BoardgameSeller>();

                foreach (var boardgameId in sellerDto.Boardgames)
                {

                    if (existingBoardgameIds.Contains(boardgameId))
                    {
                        if (!boardgamesToImport.Any(bg => bg.BoardgameId == boardgameId))
                        {
                            BoardgameSeller newBoardgame = new BoardgameSeller()
                            {
                                BoardgameId = boardgameId
                            };

                            boardgamesToImport.Add(newBoardgame);
                        }
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                Seller newSeller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                    BoardgamesSellers = boardgamesToImport,
                };

                sellersToImport.Add(newSeller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, newSeller.Name, newSeller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellersToImport);
            context.SaveChanges();
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
