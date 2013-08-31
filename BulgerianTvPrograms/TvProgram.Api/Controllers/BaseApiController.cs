using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.DataLayer;
using TvProgram.Api.Models;

namespace TvProgram.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected const string SessionKey = "hTe*$h3krbxeobewfAU0Et4w8Wbp$gqPZoPOiQ(ldpRXUbJu6z";

        protected T PerformOperationAndHandleExceptions<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(errResponse);
            }
        }
        protected void InitOrUpdateToday()
        {
            var context = new ProgramTvContext();

            using (context)
            {
                var dbMeta = context.DbMetadata.First();

                if (dbMeta == null)
                {
                    InitOrUpdateDays();
                    InitOrUpdateTvPrograms();
                    InitSchedulePrivate();



                    var newDbMeta = new CodeFirst.Model.DbMetadata()
                    {
                        LastUpdate = DateTime.Now,
                        OnProgramIdChange=DateTime.Now
                    };

                    context.DbMetadata.Add(newDbMeta);
                    context.SaveChanges();
                }

                //#region If new Init DB data
                //if (dbMeta.LastUpdate.Year != DateTime.Now.Year)
                //{

                //}
                //#endregion

                #region Else Update DB data If not it was not UPDATED TODAY
                else if (dbMeta.LastUpdate.DayOfYear != DateTime.Now.DayOfYear)
                {
                    InitOrUpdateDays();
                    InitOrUpdateTvPrograms();
                    UpdateSchedulePrivate();

                    dbMeta.LastUpdate = DateTime.Now;
                    context.SaveChanges();
                }
                #endregion
            }
            //return context;
        }

        protected HttpResponseMessage InitOrUpdateTvPrograms()
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
        protected HttpResponseMessage InitOrUpdateDays(string main = "")
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

        protected HttpResponseMessage InitSchedulePrivate()
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
                                    newSchedule.Shows.Add(showForAdding);
                                    context.SaveChanges();
                                }
                                thisProgram.LastUpdatedDate = newSchedule.Day;

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
        protected HttpResponseMessage UpdateSchedulePrivate()
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


        //private static void TotatlUpdate(List<TvProgramModel> programs, List<DayModel> dates)
        //{
        //    string dnesTvShow;
        //    var showsForDay = new List<ShowModel>();
        //    foreach (var program in programs)
        //    {
        //        Console.WriteLine(program.Name);
        //        foreach (var day in dates)
        //        {
        //            dnesTvShow = GetShowsHtml(program.ProgramId, day.GetDateSiteFromat());
        //            showsForDay = GetListOfShows(dnesTvShow);

        //            foreach (var show in showsForDay)
        //            {
        //                //dates
        //                //Console.WriteLine("        {0} -  {1}", show.Time, show.Name);
        //            }
        //        }
        //    }
        //}

        protected string GetMainHtml()
        {
            return Get("http://www.dnes.bg/tv.php");
        }



        private static string Get(string url, IDictionary<string, string> headers = null)//, string mediaType = "application/json")
        {
            HttpClient client = new HttpClient();
            // "http://www.dnes.bg/tv.php";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Method = HttpMethod.Get;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = client.SendAsync(request).Result;
            var contentString = response.Content.ReadAsStringAsync().Result;

            return contentString;
        }
        protected static string GetShowsHtml(int programId, string dayName)
        {
            return Get("http://www.dnes.bg/tv.php?tv=" + programId + "&date=" + dayName);
        }


        protected static List<ShowModel> GetListOfShows(string contentString)
        {
            var startIndexOfSelectTv = contentString.IndexOf("<div class=\"pad bProgram\">");
            startIndexOfSelectTv = contentString.IndexOf("info\">") + 6;

            var endIndexOfSelectTVs = contentString
                .IndexOf("<div class=\"c\"></div>", startIndexOfSelectTv) - 28;

            //var stringOfAllPrograms = contentString.Substring(
            //    startIndexOfSelectTv,
            //    endIndexOfSelectTVs - startIndexOfSelectTv)
            //    .Replace('\n', ' ')
            //    .Split(new string[] 
            //            { 
            //                "</div> \t </div> <div class=\"b5 tv_line\">  \t<div class=\"info\">"
            //            },
            //            StringSplitOptions.RemoveEmptyEntries
            //        );

            if (endIndexOfSelectTVs <0)
            {
                return null;
            }

            var stringOfAllPrograms = contentString.Substring(
                startIndexOfSelectTv,
                endIndexOfSelectTVs - startIndexOfSelectTv)
                .Replace('\n', ' ')
                .Split(new string[] 
                        { 
                            "</div> \t </div> <div class=\"b5 tv_line\">  \t<div class=\"info\">"
                        },
                        StringSplitOptions.RemoveEmptyEntries
                    );


            var shows = new List<ShowModel>();

            string[] tempString = new string[2];
            foreach (var program in stringOfAllPrograms)
            {
                tempString = program.Split(new string[] { "</div> \t<div class=\"ttl\">" }, StringSplitOptions.None);
                shows.Add(new ShowModel()
                    {
                        Name = tempString[1],
                        StartAt = tempString[0]
                    });
            }

            return shows;
        }

        protected static List<DayModel> GetListOfDays(string contentString)//, string mediaType = "application/json")
        {
            #region old
            //var startIndexOfSelectTv = contentString.IndexOf("<select name=\"date\" style=\"font-size:11px\">");// +45;
            //startIndexOfSelectTv = contentString.IndexOf("<option value=\"", startIndexOfSelectTv) + 15;// +45;


            //var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;

            //var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv,
            //                                                    endIndexOfSelectTVs - startIndexOfSelectTv)/*.Replace("\\n", "")*/.Replace(" selected", "").Replace("</option>", "").Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);


            //var dates = new List<ProgramScheduleModel>();

            //string[] tempProg = new string[2];
            //foreach (var date in stringOfAllPrograms)
            //{
            //    tempProg = date.Split(new string[] { "\">" }, StringSplitOptions.None);
            //    dates.Add(new ProgramScheduleModel()
            //        {
            //            Name = tempProg[1],
            //            GetDate = DateTime.Parse(tempProg[0])
            //        });
            //} 
            #endregion
            var startIndexOfSelectTv = contentString.IndexOf("<select name=\"date\" style=\"font-size:11px\">");// +45;
            startIndexOfSelectTv = contentString.IndexOf("<option value=\"", startIndexOfSelectTv) + 15;// +45;


            var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;

            var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv,
                                                                endIndexOfSelectTVs - startIndexOfSelectTv)/*.Replace("\\n", "")*/.Replace(" selected", "").Replace("</option>", "").Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);


            var dates = new List<DayModel>();
            


            string[] tempProg = new string[2];
            foreach (var date in stringOfAllPrograms)
            {
                tempProg = date.Split(new string[] { "\">" }, StringSplitOptions.None);
                dates.Add(new DayModel()
                {
                    Name = tempProg[1],
                    Date = DateTime.Parse(tempProg[0])
                });
            }

            return dates;
        }

        protected static List<TvProgramModel> GetListOfPrograms(string contentString)
        {
            var startIndexOfSelectTv = contentString.IndexOf("Всички</option>") + 17;
            var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;

            var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv,
                                                                endIndexOfSelectTVs - startIndexOfSelectTv).Replace("\\n", "").Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);


            var programs = new List<TvProgramModel>();

            string[] tempProg = new string[2];
            foreach (var program in stringOfAllPrograms)
            {
                tempProg = program.Split(new string[] { "\">" }, StringSplitOptions.None);
                programs.Add(new TvProgramModel()
                    {
                        ProgramId = int.Parse(tempProg[0]),
                        Name = tempProg[1]
                    });
            }

            return programs;
        }
    }
}
