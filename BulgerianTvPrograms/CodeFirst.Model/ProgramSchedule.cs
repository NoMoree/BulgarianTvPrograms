using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class ProgramSchedule
    {
        public int Id { get; set; }

        public virtual Day Day { get; set; }
        
        public virtual ICollection<Show> Shows { get; set; }

        public ProgramSchedule()
        {
            this.Shows = new HashSet<Show>();
        }
    }
}
