using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FootballApi.Models
{
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class League:Auditable
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; } =  new HashSet<Team>();
    }
}
