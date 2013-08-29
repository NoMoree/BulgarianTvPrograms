using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models
{
    public class ShowModel
    {
        private readonly string name;
        private readonly string time;

        public ShowModel(string name, string time)
        {
            this.name = name;
            this.time = time;
        }

        public string Name { get { return name; } }
        public string Time { get { return time; } }
    }
}