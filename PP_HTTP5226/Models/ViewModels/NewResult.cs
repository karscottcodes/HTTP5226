using PP_HTTP5226.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP_HTTP5226.Models.ViewModels
{
    public class NewResult
    {
        public IEnumerable<ResultDto> Result { get; set; }
        //Team Choice
        public IEnumerable<TeamDto> TeamChoice { get; set; }
    }
}