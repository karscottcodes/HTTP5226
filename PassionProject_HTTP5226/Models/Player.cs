using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject_HTTP5226.Models
{
    public class Player
    {
        [Key] public int PlayerId { get; set; }

        public string PlayerName {  get; set; }
        public DateTime PlayerDob { get; set; }
        public DateTime PlayerJoin {  get; set; }

        //A Player Can Belong To One Team
        [ForeignKey("Teams")]
        public int TeamId {  get; set; }
        public virtual Team Teams {  get; set; }

        //A Player Can Have Many Results
        //public ICollection<Result> Results {  get; set; }
    }

    //Simplified Player Class (Data-Transfer Object)
    public class PlayerDto
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        [Column(TypeName ="datetime2")] public DateTime PlayerDob { get; set; }

        [Column(TypeName = "datetime2")] public DateTime PlayerJoin { get; set; }
        public string TeamName { get; set; }
    }
}