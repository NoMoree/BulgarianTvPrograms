using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public HttpResponseMessage Drop()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    Database.SetInitializer(new DropCreateDatabaseAlways<ProgramTvContext>());
                    //Database.SetInitializer<ProgramTvContext>(new System.Data.Entity.Dr .DropCreateDatabaseAlways<ProgramTvContext>());
                    //InitOrUpdateDays();
                    InitOrUpdateTvPrograms();

                    var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                    return response;
                });
            return responseMsg;
        }

        [HttpGet]
        public HttpResponseMessage InitOrUpdate()
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var response =
                            this.Request.CreateResponse(HttpStatusCode.OK);

                    return response;
                });
            return responseMsg;
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



       
    }
}
