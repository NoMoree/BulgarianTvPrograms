using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.DataLayer;
using CodeFirst.Model;
using TvProgram.Api.Models;

namespace TvProgram.Api.Controllers
{
    public class DbUpdateController : BaseApiController
    {
        //GetListOfShows
        [HttpGet]
        public HttpResponseMessage InitOrUpdate()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    InitOrUpdateToday();

                    var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                    return response;
                });
            return responseMsg;
        }

        private ProgramTvContext InitOrUpdateToday()
        {
            var context = new ProgramTvContext();

            using (context)
            {
                var dbMeta = context.DbMetadata.FirstOrDefault();

                if (dbMeta == null)
                {
                    dbMeta = new CodeFirst.Model.DbMetadata();
                    context.DbMetadata.Add(dbMeta);
                }

                #region If new Init DB data
                
                if (dbMeta.LastUpdate.Year != DateTime.Now.Year)
                {
                    var addDay = InitOrUpdateDays();
                    var addTvPrograms = InitOrUpdateTvPrograms();
                    var initSchedule = InitSchedule();
                    
                    dbMeta.LastUpdate = DateTime.Now;
                    context.SaveChanges();
                }
                
                #endregion
                
                #region Else Update DB data If not it was not UPDATED TODAY
                
                else if (dbMeta.LastUpdate.DayOfYear != DateTime.Now.DayOfYear)
                {
                    var addDay = InitOrUpdateDays();
                    var addTvPrograms = InitOrUpdateTvPrograms();
                    var initSchedule = UpdateSchedulePrivate();
                    
                    dbMeta.LastUpdate = DateTime.Now;
                    context.SaveChanges();
                }

                #endregion
            }
            return context;
        }

        [HttpGet]
        //[ActionName("UpdateTvProg")]
        public HttpResponseMessage UpdateTvProg()
        {
            return InitOrUpdateTvPrograms();
        }
        [HttpGet]
        //[ActionName("UpdateTvProg")]
        public HttpResponseMessage UpdateDay()
        {
            return InitOrUpdateDays();
        }

        [HttpGet]
        public HttpResponseMessage UpdateSchedule()
        {
            return UpdateSchedulePrivate();
        }
        [HttpGet]






        public HttpResponseMessage InitSchedule()
        {
            return InitSchedulePrivate();
        }

        private HttpResponseMessage InitOrUpdateTvPrograms()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        var tvPrograms = context.TvPrograms;

                        string main = GetMainHtml();
                        var allPrograms = GetListOfPrograms(main);
                        List<TvProgramModel> programs = new List<TvProgramModel>();
                        for (int i = 0; i < 20; i++)
                        {
                            programs.Add(allPrograms[i]);
                        }

                        var models =
                            (from tv in tvPrograms
                             select new TvProgramModel()//tv.ProgramId, tv.Name)
                             {
                                 Name = tv.Name,
                                 ProgramId = tv.ProgramId
                             });
                        bool isAdded;
                        foreach (var program in programs)
                        {
                            isAdded = false;
                            foreach (var model in models)
                            {
                                if (program.Name == model.Name)
                                {
                                    if (program.ProgramId == model.ProgramId)
                                    {
                                        isAdded = true;
                                        break;
                                    }
                                    else
                                    {
                                        //TODO UpdateProgramChange
                                        //model.ProgramId = program.ProgramId;
                                        //context.SaveChanges();
                                        break;
                                    }
                                }
                            }
                            if (!isAdded)
                            {
                                var newProgram = new CodeFirst.Model.TvProgram()
                                {
                                    Name = program.Name,
                                    ProgramId = program.ProgramId
                                    //LastUpdatedDate = DateTime.Now
                                };
                                tvPrograms.Add(newProgram);
                                context.SaveChanges();

                            }
                        }


                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                        return response;
                    }
                });
            return responseMsg;
        }
        private HttpResponseMessage InitOrUpdateDays(string main = "")
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        var dayContext = context.Day;

                        if (main == "")
                        {
                            main = GetMainHtml();

                        }
                        var days = GetListOfDays(main);
                        //List<TvProgramModel> programs = new List<TvProgramModel>();
                        //for (int i = 0; i < 20; i++)
                        //{
                        //    programs.Add(allPrograms[i]);
                        //}

                        var models =
                            (from day in dayContext
                             select new DayModel()//tv.ProgramId, tv.Name)
                             {
                                 Name = day.Name,
                                 Date = day.Date
                             });
                        bool isAdded;
                        foreach (var day in days)
                        {
                            isAdded = false;
                            foreach (var model in models)
                            {
                                if (day.Name == model.Name)
                                {
                                    isAdded = true;
                                    break;
                                }
                            }
                            if (!isAdded)
                            {
                                var newDay = new CodeFirst.Model.Day()
                                {
                                    Name = day.Name,
                                    Date = day.Date
                                };
                                dayContext.Add(newDay);
                                context.SaveChanges();
                            }
                        }


                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                        return response;
                    }
                });
            return responseMsg;
        }

        private HttpResponseMessage InitSchedulePrivate()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        //var dayContext = context.ProgramSchedules;
                        var tvPrograms = context.TvPrograms;
                        var days = context.Day;

                        var allDays =
                           (from day in days
                            select new DayModel()
                            {
                                Id = day.Id,
                                Name = day.Name,
                                Date = day.Date
                            }).ToList();


                        int programsCount = context.TvPrograms.Count();
                        var allDaysCount = allDays.Count();
                        for (int i = 1; i < programsCount + 1; i++)
                        {
                            for (int thisDay = 1; thisDay < allDaysCount + 1; thisDay++)
                            {

                                var thisProgram = tvPrograms.FirstOrDefault(tv => tv.Id == i);

                                string schedulePage = GetShowsHtml(
                                    thisProgram.ProgramId,
                                    allDays[thisDay - 1].GetDateSiteFromat());

                                var newShows = GetListOfShows(schedulePage);

                                if (newShows == null || newShows.Count < 3)
                                {
                                    break;
                                }

                                var newSchedule = new CodeFirst.Model.ProgramSchedule()
                                {
                                    Day = days.FirstOrDefault(day => day.Id == thisDay)
                                };
                                thisProgram.Days.Add(newSchedule);
                                context.SaveChanges();


                                foreach (var show in newShows)
                                {
                                    var showForAdding = new CodeFirst.Model.Show()
                                    {
                                        Name = show.Name,
                                        StarAt = show.StartAt,
                                        TvProgram = thisProgram,
                                        Day = newSchedule
                                    };
                                    thisProgram.LastUpdatedDate = newSchedule.Day;
                                    newSchedule.Shows.Add(showForAdding);
                                    context.SaveChanges();
                                }
                            }
                        }
                        #region asd
                        //foreach (var lastUpdate in programsLastUpdatedDateId)
                        //{

                        //}


                        //var allInformationFromDB = (from tv in tvPrograms
                        //                            select new CheckHelperModel()//tv.ProgramId, tv.Name)
                        //                            {
                        //                                Id = tv.Id,
                        //                                ProgramId = tv.ProgramId,
                        //                                Days = (from day in tv.Days
                        //                                        where day.Date < DateTime.Now
                        //                                        select new DataHelperModel()
                        //                                        {
                        //                                            GetDate = day.Date,
                        //                                            Count = day.Shows.Count()
                        //                                        })
                        //                            });

                        //foreach (CheckHelperModel program in allInformationFromDB)
                        //{

                        //}


                        //.Where(day => day.Date >DateTime.Now)
                        //day => day.Date < DateTime.Now)
                        //select new DataHelperModel()
                        //{
                        //    GetDate = day.Date,
                        //    Count = day.Shows.Count()
                        //})

                        //foreach (var item in collection)
                        //{

                        //}
                        //var models =
                        //    (from day in dayContext
                        //     select new ProgramScheduleModel()//tv.ProgramId, tv.Name)
                        //     {
                        //         Name = day.Name,
                        //         GetDate = day.Date
                        //     });
                        //bool isAdded;
                        //foreach (var day in days)
                        //{
                        //    isAdded = false;
                        //    foreach (var model in models)
                        //    {
                        //        if (day.Name == model.Name)
                        //        {
                        //            isAdded = true;
                        //            break;
                        //        }
                        //    }
                        //    if (!isAdded)
                        //    {
                        //        var newDay = new CodeFirst.Model.ProgramSchedule()
                        //        {
                        //            Name = day.Name,
                        //            Date = day.GetDate
                        //        };
                        //        dayContext.Add(newDay);
                        //        context.SaveChanges();
                        //    }
                        //} 
                        #endregion


                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                        return response;
                    }
                });
            return responseMsg;
        }
        private HttpResponseMessage UpdateSchedulePrivate()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        //var dayContext = context.ProgramSchedules;
                        var tvPrograms = context.TvPrograms;
                        var days = context.Day;


                        #region all

                        //List<TvProgramModel> programs = new List<TvProgramModel>();
                        //for (int i = 0; i < 20; i++)
                        //{
                        //    programs.Add(allPrograms[i]);
                        //}
                        //var allInformationFromDB = (from tv in tvPrograms
                        //            select new TvProgramModel()//tv.ProgramId, tv.Name)
                        //            {
                        //                Name = tv.Name,
                        //                ProgramId = tv.ProgramId,
                        //                ProgramSchedules = (from day in tv.ProgramSchedules
                        //                        select new ProgramScheduleModel()
                        //                        {
                        //                            Name = day.Name,
                        //                            GetDate = day.Date,
                        //                            Shows = (from show in day.Shows
                        //                                    select new ShowModel()
                        //                                    {
                        //                                        Name = show.Name,
                        //                                        StartAt = show.StarAt
                        //                                    })
                        //                       })
                        //            }); 


                        //var programs = (from tv in tvPrograms
                        //                select tv);
                        //new TvProgramModel()
                        //{

                        //});
                        #endregion
                        var programsLastUpdatedDateId = (from tv in tvPrograms
                                                         orderby tv.Id
                                                         //where tv.LastUpdatedDate !=null
                                                         select tv.LastUpdatedDate.Id
                                                         ).ToList();


                        //int n = 10;

                        int lastDayId = days.Count();

                        var nLastDaysId =
                           (from day in days
                            orderby day.Id
                            //where day.Id > (days.Last().Id - n)
                            where day.Id > (lastDayId - 3)
                            select new DayModel()
                                                {
                                                    Id = day.Id,
                                                    Name = day.Name,
                                                    Date = day.Date
                                                }
                                            ).ToList();

                        //select tv.LastUpdatedDate.Id).ToList();
                        int programsCount = programsLastUpdatedDateId.Count;
                        for (int i = 1; i < programsCount + 1; i++)
                        {
                            if (programsLastUpdatedDateId[i - 1] < lastDayId)
                            {
                                for (int thisDay = programsLastUpdatedDateId[i - 1]; thisDay < lastDayId; thisDay++)
                                {
                                    var thisProgram = tvPrograms.FirstOrDefault(tv => tv.Id == i);

                                    string schedulePage = GetShowsHtml(thisProgram.ProgramId,
                                        nLastDaysId[lastDayId - thisDay].GetDateSiteFromat());

                                    var newShows = GetListOfShows(schedulePage);

                                    if (newShows == null)
                                    {
                                        break;
                                    }

                                    var newSchedule = new CodeFirst.Model.ProgramSchedule()
                                                    {
                                                        Day = days.FirstOrDefault(usr => usr.Id == thisDay)
                                                    };
                                    thisProgram.Days.Add(newSchedule);
                                    context.SaveChanges();


                                    foreach (var show in newShows)
                                    {
                                        var showForAdding = new CodeFirst.Model.Show()
                                        {
                                            Name = show.Name,
                                            StarAt = show.StartAt,
                                            TvProgram = thisProgram,
                                            Day = newSchedule
                                        };
                                        thisProgram.LastUpdatedDate = newSchedule.Day;
                                        newSchedule.Shows.Add(showForAdding);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }

                        //foreach (var lastUpdate in programsLastUpdatedDateId)
                        //{

                        //}


                        //var allInformationFromDB = (from tv in tvPrograms
                        //                            select new CheckHelperModel()//tv.ProgramId, tv.Name)
                        //                            {
                        //                                Id = tv.Id,
                        //                                ProgramId = tv.ProgramId,
                        //                                Days = (from day in tv.Days
                        //                                        where day.Date < DateTime.Now
                        //                                        select new DataHelperModel()
                        //                                        {
                        //                                            GetDate = day.Date,
                        //                                            Count = day.Shows.Count()
                        //                                        })
                        //                            });

                        //foreach (CheckHelperModel program in allInformationFromDB)
                        //{

                        //}

                        #region asd
                        //.Where(day => day.Date >DateTime.Now)
                        //day => day.Date < DateTime.Now)
                        //select new DataHelperModel()
                        //{
                        //    GetDate = day.Date,
                        //    Count = day.Shows.Count()
                        //})

                        //foreach (var item in collection)
                        //{

                        //}
                        //var models =
                        //    (from day in dayContext
                        //     select new ProgramScheduleModel()//tv.ProgramId, tv.Name)
                        //     {
                        //         Name = day.Name,
                        //         GetDate = day.Date
                        //     });
                        //bool isAdded;
                        //foreach (var day in days)
                        //{
                        //    isAdded = false;
                        //    foreach (var model in models)
                        //    {
                        //        if (day.Name == model.Name)
                        //        {
                        //            isAdded = true;
                        //            break;
                        //        }
                        //    }
                        //    if (!isAdded)
                        //    {
                        //        var newDay = new CodeFirst.Model.ProgramSchedule()
                        //        {
                        //            Name = day.Name,
                        //            Date = day.GetDate
                        //        };
                        //        dayContext.Add(newDay);
                        //        context.SaveChanges();
                        //    }
                        //} 
                        #endregion


                        var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                        return response;
                    }
                });
            return responseMsg;
        }

        //Ok


        // [HttpPost]
        ////[ActionName("Update")]
        //private HttpResponseMessage UpDate(PostModel model,
        //    [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        //{
        //    var responseMsg = this.PerformOperationAndHandleExceptions(
        //        () =>
        //        {
        //            var context = new BloggingContext();
        //            using (context)
        //            {
        //                #region Validate User (SessionKey)
        //                Validate(sessionKey, name: "SessionKey", exact: SessionKeyLength);

        //                var user = context.Users.FirstOrDefault(
        //                    usr => usr.SessionKey == sessionKey);
        //                if (user == null)
        //                {
        //                    throw new InvalidOperationException("Not existing username with this SessionKey");
        //                }
        //                #endregion
        //                #region AddNewUser and GenerateSessionKey
        //                var newPost = new Post()
        //                {
        //                    Title = model.Title,
        //                    Text = model.Text,
        //                    PostDate = DateTime.Now,
        //                    PostedBy = user
        //                };
        //                context.Posts.Add(newPost);
        //                context.SaveChanges();

        //                ICollection<string> inputTags = new List<string>();
        //                foreach (var tag in model.Tags)
        //                {
        //                    if (!inputTags.Contains(tag))
        //                    {
        //                        inputTags.Add(tag.ToLower());
        //                    }
        //                }

        //                ICollection<TagModel> allTags = new List<TagModel>();
        //                foreach (var input in inputTags)
        //                {
        //                    bool isAdded = false;
        //                    foreach (var postTags in newPost.Tags)
        //                    {
        //                        if (postTags.Name == input)
        //                        {
        //                            isAdded = true;
        //                        }
        //                    }
        //                    if (!isAdded)
        //                    {
        //                        var tagEntity = context.Tags.FirstOrDefault(
        //                            t => t.Name == input);

        //                        if (tagEntity != null)
        //                        {
        //                            tagEntity.Posts.Add(newPost);
        //                            context.SaveChanges();
        //                            newPost.Tags.Add(tagEntity);
        //                        }
        //                        else
        //                        {
        //                            tagEntity = new Tag()
        //                                            {
        //                                                Name = input
        //                                            };
        //                            tagEntity.Posts.Add(newPost);

        //                            context.Tags.Add(tagEntity);
        //                            context.SaveChanges();
        //                            newPost.Tags.Add(tagEntity);
        //                        }
        //                    }
        //                }
        //                context.SaveChanges();
        //                #endregion
        //                #region return postResponseModel to response
        //                var postResponseModel = new PostResponseModel()
        //                {
        //                    Title = newPost.Title,
        //                    Id = newPost.Id
        //                };

        //                var response =
        //                    this.Request.CreateResponse(HttpStatusCode.Created,
        //                                    postResponseModel);
        //                #endregion
        //                return response;
        //            }
        //        });
        //    return responseMsg;
        //}



        [HttpGet]
        [ActionName("GetPrograms")]
        public IQueryable<TvProgramModel> GetAll()
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
                         ProgramId = tv.ProgramId,

                     });
                return model.OrderBy(t => t.Name);
            });

            //return responseMsg;
        }
    }
}
