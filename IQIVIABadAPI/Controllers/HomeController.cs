using IQVIABadAPI.Logging;
using IQVIABadAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
/// <summary>
/// Home Controller class
/// </summary>
namespace IQVIABadAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Solution()
        {
            return View();
        }
        /// <summary>
        /// Gets all the tweets from the api https://badapi.iqvia.io
        /// </summary>
        /// <returns>Final JsonResult with all the tweets between the dates</returns>
        public ActionResult LoadTweets()
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();//Create a webclient object

                DateTime startDate = new DateTime(2016, 1, 1);
                DateTime endDate = new DateTime(2017, 12, 31, 23, 59, 59);

                //Contains the list of result set returned by the API
                List<TwitterModel> lstTwitterModel = new List<TwitterModel>();

                //Get the first set of results
                string str = wc.DownloadString("https://badapi.iqvia.io/api/v1/Tweets?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy-MM-dd"));

                //Loop until the data returned is empty
                while (str != "[]")
                {
                    //Get the list for the dates
                    str = wc.DownloadString("https://badapi.iqvia.io/api/v1/Tweets?startDate=" + startDate.ToString("yyyy-MM-dd") + "&endDate=" + endDate.ToString("yyyy-MM-dd"));

                    //Add the result to the final list
                    lstTwitterModel.AddRange(JsonConvert.DeserializeObject<List<TwitterModel>>(str));

                    //Set the start date as the last records date
                    startDate = lstTwitterModel[lstTwitterModel.Count - 1].stamp;
                }

                return new JsonResult() { Data = lstTwitterModel, MaxJsonLength = Int32.MaxValue };
            }
            catch(Exception ex)
            {
                Log4NetLogger.LogError(ex.ToString(), ex, this.ToString());
                ViewBag.Message = "Error has occured. Please contact administrator.";
                return new JsonResult();
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}