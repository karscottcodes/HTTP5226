using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP_HTTP5226.Models.ViewModels
{
    public class DetailsTeam
    {
        public TeamDto SelectedTeam { get; set; }
        public IEnumerable<PlayerDto> RelatedPlayers {  get; set; }

        public IEnumerable<PlayerDto> AvailablePlayers { get; set; }
    }
}