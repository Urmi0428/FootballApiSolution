using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FootballApi.Models
{
    [ModelMetadataType(typeof(PlayerMetaData))]
    public class PlayerDTO
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Jersey { get; set; }

        public DateTime DOB { get; set; }

        public double FeePaid { get; set; }

        public string EMail { get; set; }

        //concurrency added
        public byte[] RowVersion { get; set; }

        public int? Teamtotal { get; set; } = null;

        
        public virtual ICollection<TeamDTO> Teams { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Today.Year - DOB.Year
                    - ((DateTime.Today.Month < DOB.Month || (DateTime.Today.Month == DOB.Month && DateTime.Today.Day < DOB.Day) ? 1 : 0)) > 10 && FeePaid < 120.0)
            {
                yield return new ValidationResult("Players over 10 years old must pay a Fee of at least $120.", new[] { "FeePaid" });
            }
        }
    }
}
