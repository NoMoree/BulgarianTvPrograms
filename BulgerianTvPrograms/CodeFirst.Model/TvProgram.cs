using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class TvProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int ProgramId { get; set; }
        public virtual Day LastUpdatedDate { get; set; }

        public virtual ICollection<ProgramSchedule> Days { get; set; }

        public TvProgram()
        {
            this.Days = new HashSet<ProgramSchedule>();
        }
    }
}
