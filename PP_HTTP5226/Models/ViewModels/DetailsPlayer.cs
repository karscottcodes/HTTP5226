using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PP_HTTP5226.Models;

namespace PP_HTTP5226.Models.ViewModels
{
    public class DetailsPlayer
    {
        public PlayerDto SelectedPlayer { get; set; }
        public IEnumerable<TeamDto> RelatedTeams { get; set; }
    }
}