using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_HTTP5226.Models
{
    public class ResultTeam
    {
        [Key]
        public int ResultTeamId { get; set; }

        [ForeignKey("Result")]
        public int ResultId { get; set; }
        public Result Result { get; set; }

        [ForeignKey("Team1")]
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Score must be a positive integer.")] //Data Validation - # must be in range of 0-MaxInt)
        public int Team1Score { get; set; }

        [ForeignKey("Team2")]
        public int Team2Id { get; set; }
        public Team Team2 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Score must be a positive integer.")] //Data Validation - # must be in range of 0-MaxInt)
        public int Team2Score { get; set; }
    }
}