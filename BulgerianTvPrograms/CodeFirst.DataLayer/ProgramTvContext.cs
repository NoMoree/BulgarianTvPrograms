using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Model;

namespace CodeFirst.DataLayer
{
    public class ProgramTvContext: DbContext
    {
        //public ProgramTvContext()
        //    : base("BloggingDB")
        //{
        //}

        public DbSet<TvProgram> TvPrograms { get; set; }
        public DbSet<ProgramSchedule> ProgramSchedules { get; set; }
        public DbSet<Show> Shows { get; set; }

        public DbSet<Day> Day { get; set; }
        public DbSet<DbMetadata> DbMetadata { get; set; }

    }
}
