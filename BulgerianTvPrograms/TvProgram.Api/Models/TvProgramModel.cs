using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models
{
    public class TvProgramModel
    {
        //private readonly string name;
        //private readonly int programId;

        //public TvProgramModel(int programId, string name)
        //{
        //    this.ProgramId = programId;
        //    this.Name = name;
        //}

        //public TvProgramModel()
        //{
        //}

        public string Name { get; set; }
        public int ProgramId { get; set; }
    }

}