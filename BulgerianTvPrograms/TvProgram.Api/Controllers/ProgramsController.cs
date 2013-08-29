using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
                return new List<TvProgramModel>().AsQueryable();
            });
        }
    }
}
