using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Models
{
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class LeagueDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int? Teamtotal { get; set; } = null;

        public virtual ICollection<TeamDTO> Teams { get; set; }
    }
}
