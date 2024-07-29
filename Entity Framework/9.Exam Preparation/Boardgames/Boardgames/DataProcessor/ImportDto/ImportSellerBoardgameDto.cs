using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerBoardgameDto
    {
        [Required]
        public int Id { get; set; }
    }
}