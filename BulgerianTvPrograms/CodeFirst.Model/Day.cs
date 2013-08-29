using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Day
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Show> Shows { get; set; }

        public Day()
        {
            this.Shows = new HashSet<Show>();
        }
    }
}
