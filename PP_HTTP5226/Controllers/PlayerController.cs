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
using System.Web.Http;

namespace PP_HTTP5226.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }

        // GET: Player/List
        // Objective: Webpage To List Players in System
        //curl https://localhost:44300/api/playerdata/listplayers
        public ActionResult List()
        {

            string url = "PlayerData/ListPlayers"; //SET URL

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PlayerDto> Players = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;

            return View(Players);
        }

        //GET: Player/Details/5
        public ActionResult Details(int id)
        {

            DetailsPlayer ViewModel = new DetailsPlayer();

            string url = "PlayerData/FindPlayer/"+id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            ViewModel.SelectedPlayer = SelectedPlayer;

            string teamUrl = "TeamData/ListTeamsForPlayer" + id;
            HttpResponseMessage teamResponse = client.GetAsync(teamUrl).Result;
            IEnumerable<TeamDto> RelatedTeams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            ViewModel.RelatedTeams = RelatedTeams;

            return View(ViewModel);
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

            NewPlayer ViewModel = new NewPlayer
            {
                TeamChoice = TeamChoice
            };

            return View(ViewModel);
        }

        //POST: Player/Create
        [System.Web.Mvc.HttpPost]
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
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDto SelectedPlayer = response.Content.ReadAsAsync<PlayerDto>().Result;
            return View(SelectedPlayer);
        }

        //POST: Player/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "PlayerData/DeletePlayer/" + id;
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
        [System.Web.Mvc.HttpPost]
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

        //POST:Player/Associate/{PlayerId}
        [System.Web.Mvc.HttpPost]
        public ActionResult Associate(int id, int TeamId)
        {
            //call our api to associate player with team
            string url = "playerdata/AssociatePlayerWithTeam/" + id + "/" + TeamId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: Player/UnAssociate/{id}/{TeamId}
        [System.Web.Mvc.HttpGet]
        public ActionResult UnAssociate(int id, int TeamId)
        {

            //call our api to associate player with team
            string url = "playerdata/unassociateplayerwithteam/" + id + "/" + TeamId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

    }
}