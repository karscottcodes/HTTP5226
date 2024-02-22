using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PP_HTTP5226.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ResultDate { get; set; }

        public string ResultNote { get; set; }

        [ForeignKey("Team1")]
        public int Team1Id {  get; set; }
        public Team Team1 { get; set; }

        public int Team1Score {  get; set; }

        [ForeignKey("Team2")]
        public int Team2Id { get; set; }
        public Team Team2 {  get; set; }
        public int Team2Score {  get; set; }

    }

   

    //Simplified Result Class (Data-Transfer Object)
    public class ResultDto
    {
        public int ResultId { get; set; }
        public string ResultNote { get; set; }

        public DateTime ResultDate { get; set; }

        public string Team1Name {  get; set; }
        public int Team1Score { get; set; }
        public string Team2Name {  get; set; }
        public int Team2Score { get; set; }
    }
}