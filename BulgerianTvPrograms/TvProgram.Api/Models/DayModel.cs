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
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "getDate")]
        public DateTime GetDate { get; set; }

        //[DataMember(Name = "GetDateSiteFromat")]
        public string GetDateSiteFromat()
        {
            var output = GetDate.Date.ToString("s").Split('T')[0];
            return output;
        }

        [DataMember(Name = "shows")]
        public IEnumerable<ShowModel> Shows { get; set; }
    }
}