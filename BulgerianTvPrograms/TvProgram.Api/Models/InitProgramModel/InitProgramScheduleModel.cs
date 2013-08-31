using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models.InitProgramModel
{
    [DataContract(Name="schedule")]
    public class InitProgramScheduleModel
    {
        [DataMember(Name = "dataId")]
        public int DateId { get; set; }

        [DataMember(Name = "show")]
        public IEnumerable<InitShowModel> Shows { get; set; }
    }
}