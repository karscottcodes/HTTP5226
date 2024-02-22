using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PP_HTTP5226.Models;
using PP_HTTP5226.Models.ViewModels;

namespace PP_HTTP5226.Controllers
{
    public class TeamController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client = new HttpClient();

        static TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }

        // GET: Team/List
        // Objective: Webpage To List Teams in System
        public ActionResult List()
        {

            string url = "TeamData/ListTeams"; //SET URL

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TeamDto> Teams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            return View(Teams);
        }

        //GET: Team/Details/5
        public ActionResult Details(int id)
        {

            string url = "TeamData/FindTeam/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;

            //DISPLAY INFO ABOUT PLAYERS RELATED TO THIS TEAM

            string playerUrl = "PlayerData/ListPlayersForTeam/" + id;
            HttpResponseMessage playerResponse = client.GetAsync(playerUrl).Result;
            IEnumerable<PlayerDto> RelatedPlayers = playerResponse.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            //DISPLAY INFO ABOUT PLAYERS available TO THIS TEAM

            string availableUrl = "PlayerData/ListPlayersAvailableForTeam/" + id;
            HttpResponseMessage availableResponse = client.GetAsync(availableUrl).Result;
            IEnumerable<PlayerDto> AvailablePlayers = availableResponse.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            DetailsTeam ViewModel = new DetailsTeam
            {
                SelectedTeam = SelectedTeam,
                RelatedPlayers = RelatedPlayers,
                AvailablePlayers = AvailablePlayers
            };

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }
        //GET: Team/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            return View(SelectedTeam);
        }

        //POST: Team/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "TeamData/DeleteTeam/" + id;
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

        //GET Team/New
        public ActionResult New()
        {
            return View();
        }

        //POST: Team/Create
        [HttpPost]
        public ActionResult Create(Team Team)
        {

            Debug.WriteLine("The JSON Payload is: ");
            Debug.WriteLine(Team.TeamName);
            // OBJECTIVE: Add New Team into db using the existing API
            // curl -H "Content-Type:application/json" -d @team.json https://localhost:{port}/api/teamdata/addteam

            string url = "TeamData/AddTeam/";

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

        //GET: Team/Edit/5

        public ActionResult Edit(int id)
        {
            UpdateTeam ViewModel = new UpdateTeam();

            //Existing Teams
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            ViewModel.SelectedTeam = SelectedTeam;

            //Existing Players
            string url2 = "PlayerData/ListPlayers";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            if (response2.IsSuccessStatusCode)
            {
                IEnumerable<PlayerDto> PlayerChoice = response2.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;
                ViewModel.PlayerChoice = PlayerChoice;
            }


            //Existing Results
            string url3 = "ResultData/ListResults";
            HttpResponseMessage response3 = client.GetAsync(url3).Result;
            if (response3.IsSuccessStatusCode)
            {
                IEnumerable<ResultDto> ResultChoice = response2.Content.ReadAsAsync<IEnumerable<ResultDto>>().Result;
                ViewModel.ResultChoice = ResultChoice;
            }


            return View(ViewModel);

        }
    }
}