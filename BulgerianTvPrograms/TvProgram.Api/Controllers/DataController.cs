using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.DataLayer;
using TvProgram.Api.Models;
using TvProgram.Api.Models.InitProgramModel;
using TvProgram.Api.Models.UpdateProgramModel;

namespace TvProgram.Api.Controllers
{
    public class DataController : BaseApiController
    {
        // not tested       after     add to Init region
        [HttpPost]
        public List<UpdateScheduleModel> PostProgramSchedule(IList<UpdateScheduleHelper> helperModel)
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();
                var tvPrograms = context.TvPrograms;

                var outPutModel = new List<UpdateScheduleModel>();

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
                
                foreach (var program in helperModel)
                {
                    if (program.Id >20 || program.Id <1)
                    {
                        throw new ArgumentException("The Id is incorect");
                    }
                    var tempModels =
                    (from tv in tvPrograms
                     where tv.Id == program.Id
                     from schedule in tv.Days
                     where schedule.Day.Id > program.LastUpdatedDate
                     select new UpdateScheduleModel()
                     {
                         ProgramId = tv.Id,
                         DateId = schedule.Day.Id,
                         Shows = (from show in schedule.Shows
                                  select new UpdateShowModel()
                                  {
                                      Name = show.Name,
                                      StartAt = show.StarAt
                                      //,
                                      //TvProgramId = tv.Id,        //can be remove for optimation
                                      //DateId = schedule.Day.Id    //can be remove for optimation
                                  })
                     }).ToList();

                    foreach (var model in tempModels)
                    {
                        outPutModel.Add(model);
                    }
                }
                

                return outPutModel;//.OrderBy(t => t.Name);
            });

            return responseMsg;
        }


#region Init :       DAYS   PROGRAMS
        [HttpGet]
        public IQueryable<InitNameOfTVsModel> InitNameOfTVsModel()
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();
                var tvPrograms = context.TvPrograms;

                var model =
                    (from tv in tvPrograms
                     select new InitNameOfTVsModel()
                     {
                         Id = tv.Id,
                         Name = tv.Name,
                         LastUpdate = DateTime.Now
                     });

                return model;//.OrderBy(t => t.Name);

            });
            return responseMsg;
        }


        [HttpGet]
        public IQueryable<InitTvProgramModel> InitPrograms()
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();

                var tvPrograms = context.TvPrograms;

                var today = DateTime.Now;
                var yesterday = DateTime.Now.AddDays(-1);

                var model =
                    (from tv in tvPrograms
                     select new InitTvProgramModel()
                     {
                         Id = tv.Id,
                         Schedule = (from day in tv.Days
                                     where day.Day.Date < today
                                     where day.Day.Date > yesterday
                                     select new InitProgramScheduleModel()
                                     {
                                         DateId = day.Day.Id,
                                         Shows = (from show in day.Shows
                                                  orderby show.Id
                                                  select new InitShowModel()
                                                  {
                                                      Name = show.Name,
                                                      StartAt = show.StarAt
                                                  })
                                     }                                     )
                     });

                return model;//.OrderBy(t => t.Name);

            });



            return responseMsg;
        }

        [HttpGet]
        public IQueryable<DayModel> InitDays()
        {
            var responseMsg = PerformOperationAndHandleExceptions(() =>
            {
                var context = new ProgramTvContext();

                var days = context.Day;

                var today = DateTime.Now;

                var yesterday = DateTime.Now.AddDays(-1);

                var model =
                    (from day in days
                     select new DayModel()//tv.ProgramId, tv.Name)
                     {
                         Id = day.Id,
                         Name = day.Name,
                         Date = day.Date
                     });

                return model;//.OrderBy(t => t.Name);

            });



            return responseMsg;
        } 
	#endregion

        #region GetAllProgramSchedule  to big operation for initialise
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

        #endregion
        #region allready added
        //[HttpGet]
        //public IQueryable<InitTvProgramModel> UpdatAllProgramsss()
        //{
        //    var responseMsg = PerformOperationAndHandleExceptions(() =>
        //    {
        //        var context = new ProgramTvContext();

        //        var tvPrograms = context.TvPrograms;

        //        var today = DateTime.Now;
        //        var yesterday = DateTime.Now.AddDays(-1);

        //        var model =
        //            (from tv in tvPrograms
        //             select new InitTvProgramModel()
        //             {
        //                 Id = tv.Id,
        //                 Name = tv.Name,
        //                 LastUpdate = DateTime.Now,
        //                 Schedule = (from day in tv.Days
        //                             where day.Day.Date < today
        //                             where day.Day.Date > yesterday
        //                             select new InitProgramScheduleModel()
        //                             {
        //                                 DateId = day.Day.Id,
        //                                 Shows = (from show in day.Shows
        //                                          orderby show.Id
        //                                          select new InitShowModel()
        //                                          {
        //                                              Name = show.Name,
        //                                              StartAt = show.StarAt
        //                                          })
        //                             })
        //             });

        //        return model;//.OrderBy(t => t.Name);

        //    });



        //    return responseMsg;
        //} 
        #endregion

    }
}
