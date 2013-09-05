using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models.UpdateProgramModel
{
    public class MultUpdateScheduleModel
    {
        public int ProgramId { get; set; }

        public int DateId { get; set; }

        public IEnumerable<UpdateShowModel> Shows { get; set; }
    }
}