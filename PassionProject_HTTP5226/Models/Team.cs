using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PassionProject_HTTP5226.Models
{
    public class Team
    {
        [Key] public int TeamId {  get; set; }
        public string TeamName {  get; set; }
        
        //Many Players Associated With A Team
        public ICollection<Player> Players { get; set; }

        //Many Results Associated With A Team
        //public ICollection<Result> Results {  get; set; }
    }

    //Simplified Team Class (Data-Transfer Object)
    public class TeamDto
    {
        public int TeamId { get; set; }

        [JsonProperty("TeamName")]
        public string TeamName { get; set; }
    }
}