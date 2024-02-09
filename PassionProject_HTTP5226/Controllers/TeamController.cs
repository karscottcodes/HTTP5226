using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PassionProject_HTTP5226.Models;
using System.Web.Script.Serialization;

namespace PassionProject_HTTP5226.Controllers
{
    public class TeamController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private HttpClient client = new HttpClient();
        // GET: Team/List
        // Objective: Webpage To List Teams in System
        public ActionResult List()
        {

            string url = "https://localhost:44376/api/TeamData/ListTeams"; //SET URL

            HttpResponseMessage response = client.GetAsync(url).Result;

            List<TeamDto> Teams = response.Content.ReadAsAsync<List<TeamDto>>().Result;

            foreach(TeamDto Team in Teams)
            {
                Debug.WriteLine(Team.TeamName);
                Debug.WriteLine(Team.TeamId);
            }

            return View(Teams);
        }

        //GET: Team/Details/5
        public ActionResult Details(int id)
        {

            string url = "https://localhost:44376/api/TeamData/FindTeam/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;

            return View(SelectedTeam);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET Team/New
        public ActionResult New()
        {
            string url = "https://localhost:44376/api/TeamData/ListTeams"; //SET URL
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<TeamDto> Teams = response.Content.ReadAsAsync<List<TeamDto>>().Result;

            foreach (TeamDto Team in Teams)
            {
                Debug.WriteLine(Team.TeamName);
            }

            return View(Teams);
        }

        //POST: Team/Create
        [HttpPost]
        public ActionResult Create(Team Team)
        {

            Debug.WriteLine("The JSON Payload is: ");
            Debug.WriteLine(Team.TeamName);
            // OBJECTIVE: Add New Team into db using the existing API
            // curl -H "Content-Type:application/json" -d @team.json https://localhost:{port}/api/teamdata/addteam

            string url = "https://localhost:44376/api/TeamData/AddTeam/";

            string jsonpayload = jss.Serialize(Team);

            Debug.WriteLine(jsonpayload);

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