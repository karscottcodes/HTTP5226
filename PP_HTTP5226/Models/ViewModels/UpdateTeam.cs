using PP_HTTP5226.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP_HTTP5226.Models.ViewModels
{
    public class UpdateTeam
    {
        //Existing Result Info
        public TeamDto SelectedTeam { get; set; }

        //Player Choice
        public IEnumerable<PlayerDto> PlayerChoice { get; set; }

        //Result Choice
        public IEnumerable<ResultDto> ResultChoice { get; set; }
    }
}