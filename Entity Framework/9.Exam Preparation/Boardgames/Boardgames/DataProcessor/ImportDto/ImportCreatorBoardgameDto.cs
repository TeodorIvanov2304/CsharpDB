using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportCreatorBoardgameDto
    {
        [Required]
        [MinLength(BoardGameNameMinLength)]
        [MaxLength(BoardGameNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [Range(BoardGameRatingMinValue, BoardGameRatingMaxValue)]
        [XmlElement("Rating")]
        public double Rating { get; set; }

        [Required]
        [Range(BoardGameYearPublishedMinValue, BoardGameYearPublishedMaxValue)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }

        [Required]
        [XmlElement("CategoryType")]
        [Range(BoardGameCategoryTypeMinValue, BoardGameCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; }
    }
}