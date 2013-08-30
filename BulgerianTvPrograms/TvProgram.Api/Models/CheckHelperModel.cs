using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models
{
    public class CheckHelperModel
    {
        public int Id { get; set; }

        public int ProgramId { get; set; }

        public IEnumerable<DataHelperModel> Days { get; set; }
    }
}