using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class TvProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int SiteId { get; set; }

        public virtual ICollection<Day> Days { get; set; }

        public TvProgram()
        {
            this.Days = new HashSet<Day>();
        }
    }
}
