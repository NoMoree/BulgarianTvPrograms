﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TvProgram.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region Data
            #region Data/InitDays RDY
            config.Routes.MapHttpRoute(
                       name: "InitDays",
                       routeTemplate: "api/Data/InitDays",
                       defaults: new
                       {
                           controller = "Data",
                           action = "InitDays"
                       }
                   ); 
            #endregion
            #region Data/InitPrograms RDY
            config.Routes.MapHttpRoute(
                       name: "InitPrograms",
                       routeTemplate: "api/Data/{action}",
                       defaults: new
                       {
                           controller = "Data",
                           action = "InitPrograms"
                       }
                   ); 
            #endregion

            #region Data/InitNameOfTVsModel RDY
            config.Routes.MapHttpRoute(
                       name: "InitNameOfTVsModel",
                       routeTemplate: "api/Data/{action}",
                       defaults: new
                       {
                           controller = "Data",
                           action = "InitNameOfTVsModel"
                       }
                   ); 
            #endregion

            #region Data/AllProgramSchedule         FOR DEL         to big operation for initialise
            config.Routes.MapHttpRoute(
                       name: "AllProgramSchedule",
                       routeTemplate: "api/Data/{action}",
                       defaults: new
                       {
                           controller = "Data",
                           action = "AllProgramSchedule"
                       }
                   ); 
            #endregion

            #region Data/UpdateProgramSchedule
            config.Routes.MapHttpRoute(
                       name: "PostProgramSchedule",
                       routeTemplate: "api/Data/{action}",
                       defaults: new
                       {
                           controller = "Data",
                           action = "PostProgramSchedule"
                       }
                   ); 
            #endregion 
            #endregion

            //stoped
            #region DbUpdate           Not Ment to be public

            ///*
            //http://localhost:52807/api/DbUpdate/UpdateDay
            //http://localhost:52807/api/DbUpdate/UpdateTvProg
            //http://localhost:52807/api/DbUpdate/InitSchedule
            // * 
            //http://localhost:52807/api/DbUpdate/UpdateSchedule
            //http://localhost:52807/api/DbUpdate/
            //http://localhost:52807/api/DbUpdate/
             
            //*/
            //#region DbUpdate/DbUpdate    //public for naw
            //config.Routes.MapHttpRoute(
            //                    name: "Drop",      
            //                    routeTemplate: "api/DbUpdate/{action}",
            //                    defaults: new
            //                    {
            //                        controller = "DbUpdate",
            //                        action = "Drop"
            //                    }
            //                ); 
            //#endregion

            //#region DbUpdate/InitOrUpdate    //public for naw
            //config.Routes.MapHttpRoute(
            //                    name: "InitOrUpdate",      
            //                    routeTemplate: "api/DbUpdate/{action}",
            //                    defaults: new
            //                    {
            //                        controller = "DbUpdate",
            //                        action = "InitOrUpdate"
            //                    }
            //                ); 
            //#endregion

            //#region DbUpdate/UpdateDay   //public for naw
            //config.Routes.MapHttpRoute(
            //                    name: "UpdateDays",
            //                    routeTemplate: "api/DbUpdate/{action}",
            //                    defaults: new
            //                    {
            //                        controller = "DbUpdate",
            //                        action = "UpdateDay"
            //                    }
            //                ); 
            //#endregion
            //#region DbUpdate/UpdateTvProg    //public for naw
            //config.Routes.MapHttpRoute(
            //            name: "UpdatePrograms(20)",
            //            routeTemplate: "api/DbUpdate/{action}",
            //            defaults: new
            //            {
            //                controller = "DbUpdate",
            //                action = "UpdateTvProg"
            //            }
            //        ); 
            //#endregion
            //#region DbUpdate/InitSchedule    //public for naw
            //config.Routes.MapHttpRoute(
            //                    name: "InitSchedule",
            //                    routeTemplate: "api/DbUpdate/{action}",
            //                    defaults: new
            //                    {
            //                        controller = "DbUpdate",
            //                        action = "InitSchedule"
            //                    }
            //                );  
            //#endregion
            //#region DbUpdate/UpdateSchedule  //public for naw
            //config.Routes.MapHttpRoute(
            //           name: "UpdateSchedule",
            //           routeTemplate: "api/DbUpdate/{action}",
            //           defaults: new
            //           {
            //               controller = "DbUpdate",
            //               action = "UpdateSchedule"
            //           }
            //       ); 
            //#endregion

            
            #endregion







            #region DefaultApi + useless info
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            #endregion
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
