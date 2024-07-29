namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Creators";

            var creatorsToExport = context.Creators
                .Where(cr => cr.Boardgames.Any())
                .Select(dto => new CreatorExportDto()
                {
                    BoardgamesCount = dto.Boardgames.Count,
                    CreatorName = dto.FirstName + " " + dto.LastName,
                    Boardgames = dto.Boardgames
                    .OrderBy(bg=>bg.Name)
                    .Select(bg => new CreatorBoardGameExportDto()
                    {
                        BoardgameName = bg.Name,
                        BoardgameYearPublished = bg.YearPublished,
                    })
                    .ToArray()
                })
                .OrderByDescending(cr => cr.BoardgamesCount)
                .ThenBy(cr=>cr.CreatorName)
                .ToList();

            return xmlHelper.Serialize(creatorsToExport, xmlRoot);
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellersWithMostBoardgames = context.Sellers
                .Where(s => s.BoardgamesSellers.Any())
                .Where(s => s.BoardgamesSellers.Any(yp => yp.Boardgame.YearPublished >= year))
                .Where(s => s.BoardgamesSellers.Any(r => r.Boardgame.Rating <= rating))
                .Select(s => new SellerWithMostBoardgamesExportDto()
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers  
                    .Where(bg=>bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating)
                    .Select(bg => new BoardgameExportDto()
                    {
                        Name = bg.Boardgame.Name,
                        Rating = bg.Boardgame.Rating,
                        Mechanics = bg.Boardgame.Mechanics,
                        Category = bg.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(bg=>bg.Rating)
                    .ThenBy(bg => bg.Name)
                    .ToArray()   
                })
                .OrderByDescending(s=>s.Boardgames.Length)
                .ThenBy(s=>s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellersWithMostBoardgames, Formatting.Indented);
        }
    }
}