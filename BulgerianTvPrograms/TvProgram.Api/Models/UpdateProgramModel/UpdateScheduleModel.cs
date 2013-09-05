using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models.UpdateProgramModel
{
    [DataContract(Name="schedule")]
    public class UpdateScheduleModel
    {
        //[DataMember(Name = "programId")]
        //public int ProgramId { get; set; }

        [DataMember(Name = "dataId")]
        public int DateId { get; set; }

        [DataMember(Name = "shows")]
        public IEnumerable<UpdateShowModel> Shows { get; set; }
    }
}