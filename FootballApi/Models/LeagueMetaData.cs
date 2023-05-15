using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    public class LeagueMetaData
    {
        [Required(ErrorMessage = "ID is required for League")]
        [RegularExpression("^[A-Z][A-Z]$")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Id code should be 2 characters only")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Name is required for League")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name should be in length between 1 to 100")]
        public string Name { get; set; }
    }
}
