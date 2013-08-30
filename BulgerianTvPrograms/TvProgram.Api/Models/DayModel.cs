using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models
{
    [DataContract]
    public class DayModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "getDate")]
        public DateTime Date { get; set; }

        public string GetDateSiteFromat()
        {
            var output = Date.Date.ToString("s").Split('T')[0];
            return output;
        }
    }
}