namespace FootballApi.Models
{
    public class PlayerTeam 
    {

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }  
}
