using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Benefit
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        
        static System.Timers.Timer timer = new System.Timers.Timer();
        static System.Timers.Timer timer2 = new System.Timers.Timer();
        string path = null;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            timer.Interval = 1000;
            timer.Elapsed += tm_Tick;
            
            path = Server.MapPath("/");

            timer2.Interval = 1000;
            timer2.Elapsed += tm2_Tick;
           StartTimer();
        }

        private void tm2_Tick(object sender, ElapsedEventArgs e)
        {
            EndTimer();
            Console.Beep(300, 1000);
            Console.WriteLine("its timer 2");
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now.ToString("HH:mm tt") == "00:00")
            {
                //Benefit.Models.GetInfoFromSite.InsertGamesParWeek(path);
            }
        }

        private void tm_Tick(object sender, ElapsedEventArgs e)
        {
          EndTimer();
            Console.Beep(300, 1000);
            //Benefit.Models.GetInfoFromSite.InsertPlayers(path);
            //Benefit.Models.GetInfoFromSite.InsertIndex(path);
            //Benefit.Models.GetInfoFromSite.InsertStatistic(path);
            //Console.Beep(300, 1000);
            // Benefit.Models.BenefitSystem.GetLazyTrainees(path);
        }

        public static void StartTimer()
        {
            timer.Enabled = true;
            timer2.Enabled = true;
        }

        public static void EndTimer()
        {
            timer.Enabled = false;
            timer2.Enabled = false;
        }
    }

}
