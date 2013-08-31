using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TvProgram.Api.Models;

namespace TvProgram.Api.Models.InitProgramModel
{
    [DataContract(Name="tv")]
    public class InitTvProgramModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "schedule")]
        public IEnumerable<InitProgramScheduleModel> Schedule { get; set; }
    }
}