using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PP_HTTP5226.Models.ViewModels;
using PP_HTTP5226.Models;
using System.Data.Entity;

namespace PP_HTTP5226.Controllers
{
    public class ResultController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();

        static ResultController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }

        // GET: Result/List
        // Objective: Webpage To List Results in System
        //curl https://localhost:44300/api/Resultdata/listResults
        public ActionResult List()
        {

            string url = "ResultData/ListResults"; //SET URL

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ResultDto> Results = response.Content.ReadAsAsync<IEnumerable<ResultDto>>().Result;

            //foreach(ResultDto Result in Results)
            //{
            //    Debug.WriteLine(Result.ResultName);
            //}

            return View(Results);
        }

        // GET: Result/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var resultWithTeams = (from result in db.Results
                                       where result.ResultId == id
                                       select result)
                                       .Include(r => r.Team1)  // Include Team1 entity
                                       .Include(r => r.Team2)  // Include Team2 entity
                                       .SingleOrDefault(); // Retrieve a single result matching the ID

                if (resultWithTeams == null)
                {
                    return HttpNotFound();
                }

                return View(resultWithTeams);
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET Result/New
        public ActionResult New()
        {
            string url = "TeamData/ListTeams"; //SET URL
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamChoice = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            NewResult ViewModel = new NewResult
            {
                TeamChoice = TeamChoice
            };

            return View(ViewModel);
        }

        //POST: Result/Create
        [HttpPost]
        public ActionResult Create(Result Result)
        {

            //Debug.WriteLine("The JSON Payload is: ");
            //Debug.WriteLine(Result.ResultName);
            // OBJECTIVE: Add New Result into db using the existing API
            // curl -H "Content-Type:application/json" -d @Result.json https://localhost:{port}/api/Resultdata/addResult

            string url = "ResultData/AddResult/";

            string jsonpayload = jss.Serialize(Result);

            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //GET: Result/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ResultData/FindResult/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ResultDto SelectedResult = response.Content.ReadAsAsync<ResultDto>().Result;
            return View(SelectedResult);
        }

        //POST: Result/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "ResultData/DeleteResult" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Result/Edit/5

        public ActionResult Edit(int id)
        {
            UpdateResult ViewModel = new UpdateResult();

            //Existing Result
            string url = "ResultData/FindResult/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ResultDto SelectedResult = response.Content.ReadAsAsync<ResultDto>().Result;
            ViewModel.SelectedResult = SelectedResult;

            //Existing Teams
            string url2 = "TeamData/ListTeams";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            IEnumerable<TeamDto> TeamChoice = response2.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            ViewModel.TeamChoice = TeamChoice;

            return View(ViewModel);

        }

        //POST: Result/Update/5
        [HttpPost]
        public ActionResult Update(int id, Result Result)
        {
            string url = "ResultData/UpdateResult/" + id;
            string jsonpayload = jss.Serialize(Result);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

    }
}