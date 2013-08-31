using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models
{
    //[DataContract]
    public class UpdateScheduleHelper
    {
        //[DataMember(Name="id")]
        public int Id { get; set; }

        //[DataMember(Name = "lastUpdatedDate")]
        public int LastUpdatedDate { get; set; }
    }
}