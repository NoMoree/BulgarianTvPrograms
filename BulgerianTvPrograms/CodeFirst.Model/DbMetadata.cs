using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class DbMetadata
    {
        [Key]
        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime OnProgramIdChange { get; set; }

    }
}
