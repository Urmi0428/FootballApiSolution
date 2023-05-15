using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    [ModelMetadataType(typeof(TeamMetaData))]
    public class TeamDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        //added navigation properties
        public string LeagueID { get; set; }
        public LeagueDTO League { get; set; }

        public string LeagueName { get; set; }

        public int? Playertotal { get; set; } = null;

        public ICollection<PlayerDTO> Players { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Name[0] == 'X' || Name[0] == 'F' || Name[0] == 'S')
            {
                yield return new ValidationResult("Team names are not allowed to start with the letters X, F, or S.", new[] { "Name" });
            }
        }
    }
}
