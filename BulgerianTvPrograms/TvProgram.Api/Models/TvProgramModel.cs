using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models
{
    [DataContract]
    public class TvProgramModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "programId")]
        public int ProgramId { get; set; }

        [DataMember(Name = "days")]
        public IEnumerable<DayModel> Days { get; set; }
    }

}