using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PassionProject_HTTP5226.Models;
using System.Web.Script.Serialization;
using PassionProject_HTTP5226.Models.ViewModels;

namespace PassionProject_HTTP5226.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376/api/");
        }

        // GET: Player/List
        // Objective: Webpage To List Players in System
        //curl https://localhost:44376/api/playerdata/listplayers
        public ActionResult List()
        {

            string url = "PlayerData/ListPlayers"; //SET URL

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PlayerDto> Players = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            //foreach(PlayerDto player in Players)
            //{
            //    Debug.WriteLine(player.PlayerName);
            //}

            return View(Players);
        }

        //GET: Player/Details/5
        public ActionResult Details(int id)
        {

            string url = "PlayerData/FindPlayer/"+id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;

            return View(SelectedPlayer);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET Player/New
        public ActionResult New()
        {
            string url = "TeamData/ListTeams"; //SET URL
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamChoice = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            //foreach (TeamDto Team in Teams)
            //{
            //    Debug.WriteLine(Team.TeamName);
            //}

            return View(TeamChoice);
        }

        //POST: Player/Create
        [HttpPost]
        public ActionResult Create(Player player)
        {

            //Debug.WriteLine("The JSON Payload is: ");
            //Debug.WriteLine(player.PlayerName);
            // OBJECTIVE: Add New Player into db using the existing API
            // curl -H "Content-Type:application/json" -d @player.json https://localhost:{port}/api/playerdata/addplayer

            string url = "PlayerData/AddPlayer/";

            string jsonpayload = jss.Serialize(player);

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

        //GET: Player/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PlayerData/FindPlayer/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            return View(SelectedPlayer);
        }

        //POST: Player/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "PlayerData/DeletePlayer"+id;
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

        //GET: Player/Edit/5

        public ActionResult Edit(int id)
        {
            UpdatePlayer ViewModel = new UpdatePlayer();

            //Existing Player
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            ViewModel.SelectedPlayer = SelectedPlayer;

            //Existing Teams
            string url2 = "TeamData/ListTeams";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            IEnumerable<TeamDto> TeamChoice = response2.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            ViewModel.TeamChoice = TeamChoice;

            return View(ViewModel);

        }

        //POST: Player/Update/5
        [HttpPost]
        public ActionResult Update(int id, Player player)
        {
            string url = "PlayerData/UpdatePlayer/" + id;
            string jsonpayload = jss.Serialize(player);
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