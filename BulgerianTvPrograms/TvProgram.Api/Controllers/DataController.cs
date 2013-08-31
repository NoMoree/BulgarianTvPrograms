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
    public class DataController : BaseApiController
    {
        [HttpGet]
        [ActionName("AllProgramSchedule")]
        public IQueryable<TvProgramModel> GetAllProgramSchedule()
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                InitOrUpdateToday();

                var context = new ProgramTvContext();
                var tvPrograms = context.TvPrograms;

                var model =
                    (from tv in tvPrograms
                     select new TvProgramModel()//tv.ProgramId, tv.Name)
                     {
                         Id = tv.Id,                    //can be remove for optimation
                         Name = tv.Name,
                         ProgramId = tv.ProgramId,      //can be remove for optimation
                         Days = (from schedule in tv.Days
                                 select new ProgramScheduleModel()
                                {
                                    DateId = schedule.Day.Id,
                                    Shows = (from show in schedule.Shows
                                             select new ShowModel()
                                             {
                                                 Name = show.Name,
                                                 StartAt = show.StarAt,
                                                 TvProgramId = tv.Id,
                                                 DateId = schedule.Day.Id
                                             })
                                })
                     });
                return model;//.OrderBy(t => t.Name);
            });

            return responseMsg;
        }

        [HttpGet]
        [ActionName("UpdateProgramSchedule")]
        public IQueryable<UpdateScheduleModel> UpdateProgramSchedule(IList<UpdateScheduleHelper> helperModel)
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();
                var tvPrograms = context.TvPrograms;

                #region hide not used
                //var model =
                //    (from tv in tvPrograms
                //     //select new TvProgramModel()//tv.ProgramId, tv.Name)
                //     //{
                //         //Name = tv.Name,            //to remove
                //         //ProgramId = tv.ProgramId,
                //         //Days = 
                //             (from schedule in tv.Days
                //                 where schedule.Day.Id > helperModel[tv.ProgramId].LastUpdatedDate
                //                 //select new ProgramScheduleModel()
                //                 //{
                //                 //    DateId = schedule.Day.Id,
                //                 //    Shows = 
                //                 (from show in schedule.Shows
                //                              select new ShowModel()
                //                              {
                //                                  Name = show.Name,
                //                                  StartAt = show.StarAt,
                //                                  TvProgramId = tv.Id,
                //                                  DateId = schedule.Day.Id
                //                              })
                //                 //})}
                //                     )
                //                     );
                //var allProgramIdForSearch = new List<int>();
                //foreach (var model in helperModel)
                //{
                //    allProgramIdForSearch.Add(model.Id);
                //}

                //            //IQueryable<CodeFirst.Model.TvProgram> allWithThatId = List<CodeFirst.Model.TvProgram>();
                //            foreach (var tv in tvPrograms)
                //{
                //     var temp = helperModel.Where((item) => (item.Id == tv.Id));
                //     allWithThatId.Add(temp);
                //}

                //var models2 = this.GetAll(sessionKey)
                //.Where(p => p.Title.Contains(keyword) 
                #endregion

                var model =
                    (from tv in tvPrograms
                     from schedule in tv.Days
                     where schedule.Day.Id > helperModel[tv.ProgramId].LastUpdatedDate
                     select new UpdateScheduleModel()
                     {
                         ProgramId = tv.Id,
                         DateId = schedule.Day.Id,
                         Shows = (from show in schedule.Shows
                                  select new ShowModel()
                                    {
                                        Name = show.Name,
                                        StartAt = show.StarAt,
                                        TvProgramId = tv.Id,        //can be remove for optimation
                                        DateId = schedule.Day.Id    //can be remove for optimation
                                    })
                     });

                return model;//.OrderBy(t => t.Name);
            });

            return responseMsg;
        }

        [HttpGet]
        [ActionName("InitUserPrograms")]
        public IQueryable<InitTvProgramModel> InitUserPrograms()
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();
                using (context)
                {
                    var tvPrograms = context.TvPrograms;

                    var today = DateTime.Now.AddDays(1);
                                                    
                    var yesterday=DateTime.Now.AddDays(-1);

                    var model =
                        (from tv in tvPrograms
                         select new InitTvProgramModel()//tv.ProgramId, tv.Name)
                         {
                             Id = tv.Id,
                             Name = tv.Name,
                             LastUpdate = DateTime.Now,
                             Schedule = (from day in tv.Days
                                         select new ProgramScheduleModel()
                                         {
                                             DateId = day.Day.Id,
                                             Shows = (from show in day.Shows
                                                      //where day.Day.Date.Month == DateTime.Now.Month
                                                      //where day.Day.Date.Year == DateTime.Now.Year
                                                      //where day.Day.Date.Day == DateTime.Now.Day
                                                      //where day.Day.Date < today
                                                      //where day.Day.Date> yesterday
                                                      select new ShowModel()
                                                      {
                                                          DateId = day.Day.Id,
                                                          Name = show.Name,
                                                          StartAt = show.StarAt,
                                                          TvProgramId = tv.Id
                                                      })
                                         })
                         }).AsQueryable();

                    return model;//.OrderBy(t => t.Name);
                }
            });
        }
    }
}
