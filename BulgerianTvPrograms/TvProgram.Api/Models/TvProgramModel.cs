using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models
{
    public class TvProgramModel
    {
        private readonly string name;
        private readonly int internetId;

        public TvProgramModel(int internetId, string name)
        {
            this.internetId = internetId;
            this.name = name;
        }

        public string Name { get { return name; } }
        public int InternetId { get { return internetId; } }
    }

}