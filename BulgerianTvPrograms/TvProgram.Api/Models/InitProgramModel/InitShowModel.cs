using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models.InitProgramModel
{
    [DataContract(Name="show")]
    public class InitShowModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "startAt")]
        public string StartAt { get; set; }
    }
}