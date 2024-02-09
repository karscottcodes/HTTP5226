using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PassionProject_HTTP5226.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ResultDate { get; set; }

        public string ResultNote { get; set; }

        public ICollection<ResultTeam> ResultTeams { get; set; }

    }

    //Simplified Result Class (Data-Transfer Object)
    public class ResultDto
    {
        public int ResultId { get; set; }
        public string ResultNote { get; set; }

        public DateTime ResultDate { get; set; }
    }
}