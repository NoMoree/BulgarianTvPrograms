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
        //[ActionName("UpdateTvProg")]
        public HttpResponseMessage UpdateTvProg()
        {
            return UpdateTvPrograms();
        }

        [HttpGet]
        //[ActionName("UpdateTvProg")]
        public HttpResponseMessage UpdateDay()
        {
            return UpdateDays();
        }

        private HttpResponseMessage UpdateShows()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        var dayContext = context.Days;
                        var tvPrograms = context.TvPrograms;


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
                        //                Days = (from day in tv.Days
                        //                        select new DayModel()
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
                        #endregion

                        var allInformationFromDB = (from tv in tvPrograms
                                                    select new CheckHelperModel()//tv.ProgramId, tv.Name)
                                                    {
                                                        Id = tv.Id,
                                                        ProgramId = tv.ProgramId,
                                                        Days = (from day in tv.Days
                                                                where day.Date < DateTime.Now
                                                                select new DataHelperModel()
                                                                {
                                                                    GetDate = day.Date,
                                                                    Count = day.Shows.Count()
                                                                })
                                                    });
                        
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
                        //     select new DayModel()//tv.ProgramId, tv.Name)
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
                        //        var newDay = new CodeFirst.Model.Day()
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

        private HttpResponseMessage UpdateTvPrograms()
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
                                        //model.ProgramId = program.ProgramId;
                                        //context.SaveChanges();
                                    }
                                }
                            }
                            if (!isAdded)
                            {
                                var newProgram = new CodeFirst.Model.TvProgram()
                                {
                                    Name = program.Name,
                                    ProgramId = program.ProgramId,
                                    LastUpdatedDate = DateTime.Now
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

        private HttpResponseMessage UpdateDays(string main = "")
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var context = new ProgramTvContext();
                    using (context)
                    {
                        var dayContext = context.Days;

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
                                 GetDate = day.Date
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
                                    Date = day.GetDate
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
                         ProgramId = tv.ProgramId
                     });
                return model.OrderBy(t => t.Name);
            });

            //return responseMsg;
        }
    }
}
