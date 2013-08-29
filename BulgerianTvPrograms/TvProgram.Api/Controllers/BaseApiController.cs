using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        private static void TotatlUpdate(List<TvProgramModel> programs, List<DateModel> dates)
        {
            string dnesTvShow;
            var showsForDay = new List<ShowModel>();
            foreach (var program in programs)
            {
                Console.WriteLine(program.Name);
                foreach (var day in dates)
                {
                    dnesTvShow = GetShow(program.InternetId, day.GetDateSiteFromat());
                    showsForDay = GetListOfShows(dnesTvShow);

                    foreach (var show in showsForDay)
                    {
                        //dates
                        //Console.WriteLine("        {0} -  {1}", show.Time, show.Name);
                    }
                }
            }
        }

        protected string GetMain()
        {
            return Get("http://www.dnes.bg/tv.php");
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
                shows.Add(new ShowModel(tempString[1], tempString[0]));
            }

            return shows;
        }



        public static string Get(string url, IDictionary<string, string> headers = null)//, string mediaType = "application/json")
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

        public static List<DateModel> GetListOfDates(string contentString)//, string mediaType = "application/json")
        {
            var startIndexOfSelectTv = contentString.IndexOf("<select name=\"date\" style=\"font-size:11px\">");// +45;
            startIndexOfSelectTv = contentString.IndexOf("<option value=\"", startIndexOfSelectTv) + 15;// +45;


            var endIndexOfSelectTVs = contentString.IndexOf("</select>", startIndexOfSelectTv) - 3;

            var stringOfAllPrograms = contentString.Substring(startIndexOfSelectTv,
                                                                endIndexOfSelectTVs - startIndexOfSelectTv)/*.Replace("\\n", "")*/.Replace(" selected", "").Replace("</option>", "").Split(new string[] { "<option value=\"" }, StringSplitOptions.RemoveEmptyEntries);


            var dates = new List<DateModel>();

            string[] tempProg = new string[2];
            foreach (var date in stringOfAllPrograms)
            {
                tempProg = date.Split(new string[] { "\">" }, StringSplitOptions.None);
                dates.Add(new DateModel(tempProg[1], DateTime.Parse(tempProg[0])));
            }

            return dates;
        }

        private static string GetShow(int programId, string dayName)
        {
            return Get("http://www.dnes.bg/tv.php?tv=" + programId + "&date=" + dayName);
        }

        public static List<TvProgramModel> GetListOfPrograms(string contentString)
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
                programs.Add(new TvProgramModel(int.Parse(tempProg[0]), tempProg[1]));
            }

            return programs;
        }


        //public static void Validate(string value, string name = "Name", bool canBeNull = false, string validate = null, int min = int.MinValue, int max = int.MaxValue, int exact = int.MinValue, bool isNumber = false)
        //{
        //    if (!canBeNull)
        //    {
        //        if (value == null)
        //        {
        //            throw new ArgumentNullException(name + " can not be null");
        //        }
        //    }

        //    #region isNumberOrString => MinMaxExact
        //    if (!isNumber)
        //    {
        //        if (exact != int.MinValue)
        //        {
        //            if (exact != value.Length)
        //            {
        //                throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be exact {1} characters long", name, exact));
        //            }
        //        }
        //        else
        //        {
        //            if (min != int.MinValue)
        //            {
        //                if (min > value.Length)
        //                {
        //                    throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be at least {1} characters long", name, min));
        //                }
        //            }
        //            if (max != int.MaxValue)
        //            {
        //                if (max < value.Length)
        //                {
        //                    throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be less then {1} characters long", name, max));
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (exact != int.MinValue)
        //        {
        //            if (exact != int.Parse(value))
        //            {
        //                throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be exact number {1}", name, exact));
        //            }
        //        }
        //        else
        //        {
        //            if (min != int.MinValue)
        //            {
        //                if (min > int.Parse(value))
        //                {
        //                    throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be at bigger then {1}", name, min));
        //                }
        //            }
        //            if (max != int.MaxValue)
        //            {
        //                if (max < int.Parse(value))
        //                {
        //                    throw new ArgumentOutOfRangeException(string.Format(
        //                        "{0} must be at smaller then {1}", name, max));
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    if (validate != null)
        //    {
        //        if (value.Any(ch => !validate.Contains(ch)))
        //        {
        //            throw new ArgumentOutOfRangeException(
        //               string.Format("{0} must contain only:  {1}", name, validate));
        //        }
        //    }


        //}

    }
}
