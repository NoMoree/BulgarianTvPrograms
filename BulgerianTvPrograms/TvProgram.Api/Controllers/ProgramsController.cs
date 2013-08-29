using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.DataLayer;
using TvProgram.Api.Models;

namespace TvProgram.Api.Controllers
{
    public class ProgramsController : BaseApiController
    {
        [HttpGet]
        private IQueryable<TvProgramModel> GetAll()
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();
                var tvPrograms = context.TvPrograms;

                var model =
                    (from tv in tvPrograms
                     select new TvProgramModel()//tv.ProgramId, tv.Name)
                     {
                         Name = tv.Name,
                         ProgramId = tv.ProgramId
                     }
                     );
                return model;
            });

            //return responseMsg;
        }
    }
}
