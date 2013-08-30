using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string StarAt { get; set; }

        public virtual ProgramSchedule Day { get; set; }

        public virtual TvProgram TvProgram { get; set; }
    }
}
