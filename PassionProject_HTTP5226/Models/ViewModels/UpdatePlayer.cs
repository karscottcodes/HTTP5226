using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_HTTP5226.Models.ViewModels
{
    public class UpdatePlayer
    {
        //Existing Player Info
        public PlayerDto SelectedPlayer {  get; set; }

        //Team Choice
        public IEnumerable<TeamDto> TeamChoice { get; set; }
    }
}