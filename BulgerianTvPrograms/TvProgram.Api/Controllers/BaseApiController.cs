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

        private static void TotatlUpdate(List<TvProgramModel> programs, List<DayModel> dates)
        {
            string dnesTvShow;
            var showsForDay = new List<ShowModel>();
            foreach (var program in programs)
            {
                Console.WriteLine(program.Name);
                foreach (var day in dates)
                {
                    dnesTvShow = GetShowsHtml(program.ProgramId, day.GetDateSiteFromat());
                    showsForDay = GetListOfShows(dnesTvShow);

                    foreach (var show in showsForDay)
                    {
                        //dates
                        //Console.WriteLine("        {0} -  {1}", show.Time, show.Name);
                    }
                }
            }
        }

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
