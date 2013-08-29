using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TvProgram.Api.Models
{
    public class DateModel
    {
        private readonly string name;
        private readonly DateTime date;

        public DateModel(string name, DateTime date)
        {
            this.name = name;
            this.date = date;
        }

        public string Name { get { return name; } }
        public DateTime GetDate { get { return date; } }
        public string GetDateSiteFromat()
        {
            var output = date.Date.ToString("s").Split('T')[0];
            return output;
        }
    }
}