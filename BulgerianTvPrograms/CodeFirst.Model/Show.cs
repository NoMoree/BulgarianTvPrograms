using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeFirst.Model
{
    public class Show
    {
        public int Id { get; set; }
        //[System.ComponentModel.DataAnnotations.
        public virtual ProgramSchedule Day { get; set; }

        public string Name { get; set; }
        public string StarAt { get; set; }


        //[Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[System.ComponentModel.DataAnnotations.Schema.ForeignKey("Day"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public virtual ProgramSchedule Day { get; set; }

        //public virtual TvProgram TvProgram { get; set; }
    }
}
