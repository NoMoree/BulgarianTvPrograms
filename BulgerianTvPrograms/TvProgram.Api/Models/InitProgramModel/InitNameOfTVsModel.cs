using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TvProgram.Api.Models.InitProgramModel
{
    [DataContract(Name = "tv")]
    public class InitNameOfTVsModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "programName")]
        public string Name { get; set; }

        [DataMember(Name = "logo")]
        public string Logo { get; set; }

        //[DataMember(Name = "lastUpdatedId")]
        //public int LastUpdatedId { get; set; }
    }
}