using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models
{
    [DataContract]
    public class ShowModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "startAt")]
        public string StartAt { get; set; }

        [DataMember(Name = "tvProgramId")]
        public int TvProgramId { get; set; }

        [DataMember(Name = "dateId")]
        public int DateId { get; set; }
    }
}