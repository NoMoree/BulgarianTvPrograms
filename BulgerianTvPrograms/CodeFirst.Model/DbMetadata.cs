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

        public int LastUpdate { get; set; }

        public int OnProgramIdChange { get; set; }

    }
}
