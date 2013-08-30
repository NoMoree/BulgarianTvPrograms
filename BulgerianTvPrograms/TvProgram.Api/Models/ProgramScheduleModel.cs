using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models
{
    [DataContract]
    public class ProgramScheduleModel
    {
        [DataMember(Name = "dateId")]
        public int DateId { get; set; }

        [DataMember(Name = "shows")]
        public IEnumerable<ShowModel> Shows { get; set; }
    }
}