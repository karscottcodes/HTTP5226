using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PP_HTTP5226.Models;
using System.Diagnostics;

namespace PP_HTTP5226.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Provides all PLAYER information
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all PLAYER information in the database
        /// <example>
        /// GET: api/PlayerData/ListPlayers
        /// </example>
        [HttpGet]
        public IEnumerable<PlayerDto> ListPlayers()
        {
            List<Player> Players = db.Players.ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerId = p.PlayerId,
                PlayerName = p.PlayerName,
                PlayerDob = p.PlayerDob,
                PlayerJoin = p.PlayerJoin,
                TeamName = p.TeamName
            }));

            return PlayerDtos;
        }

        /// <summary>
        /// Provides all PLAYER information
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all PLAYER information in the database
        /// <param = "PlayerId">PlayerId</param>
        /// <example>
        /// GET: api/PlayerData/FindPlayer/5
        /// </example>
        [ResponseType(typeof(Player))]
        [HttpGet]
        public IHttpActionResult FindPlayer(int id)
        {
            Player Player = db.Players.Find(id);
            PlayerDto PlayerDto = new PlayerDto()
            {
                PlayerId = Player.PlayerId,
                PlayerName = Player.PlayerName,
                PlayerDob = Player.PlayerDob,
                PlayerJoin = Player.PlayerJoin,
                TeamId = Player.TeamId,
                TeamName = Player.TeamName
            };
            if (Player == null)
            {
                return NotFound();
            }

            Debug.WriteLine("PlayerDto" + PlayerDto);


            return Ok(PlayerDto);
        }

        /// <summary>
        /// Updates PLAYER information based on playerid
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: updates PLAYER information in the database based on playerid
        /// <param = "PlayerId">PlayerId</param>
        /// <example>
        /// GET: api/PlayerData/UpdatePlayer/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            db.Entry(player).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PlayerData/AddPlayer
        // Add new player to db based on form data
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Players.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.PlayerId }, player);
        }

        // POST: api/PlayerData/DeletePlayer/5
        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult DeletePlayer(int id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            db.Players.Remove(player);
            db.SaveChanges();

            return Ok(player);
        }

        /// <summary>
        /// Provides all PLAYER information associated with a TEAM
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all PLAYER information in the database, that is associated with a specific TEAM ID
        /// </returns>
        /// <param name="id">Team ID</param>
        /// <example>
        /// GET: api/PlayerData/ListPlayersForTeam/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]

        public IHttpActionResult ListPlayersForTeam(int id)
        {
            List<Player> Players = db.Players.Include(p => p.Teams).Where(p => p.TeamId == id).ToList();
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            Debug.WriteLine("PlayerDto" + Players);

            Players.ForEach(p => PlayerDtos.Add(new PlayerDto()
            {
                PlayerId = p.PlayerId,
                PlayerName = p.PlayerName,
                PlayerDob = p.PlayerDob,
                PlayerJoin = p.PlayerJoin,
                TeamId = p.TeamId,
                TeamName = p.TeamName

            }));



            return Ok(PlayerDtos);

        }

        /// <summary>
        /// Returns PLAYERS in the system not ON A particular TEAM.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all players in the database not on a particular team
        /// </returns>
        /// <param name="id">Team Primary Key</param>
        /// <example>
        /// GET: api/PlayerData/ListPlayersAvailableForTeam/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult ListPlayersAvailableForTeam(int id)
        {
            List<Player> Players = db.Players.ToList(); // Get all players from the database
            List<PlayerDto> PlayerDtos = new List<PlayerDto>();

            foreach (var player in Players)
            {
                // Check if the player does not belong to the specified team
                if (player.TeamId != id)
                {
                    PlayerDtos.Add(new PlayerDto()
                    {
                        PlayerId = player.PlayerId,
                        PlayerName = player.PlayerName
                    });
                }
            }

            return Ok(PlayerDtos);
        }






        // GET: api/TeamData/ListResultsByTeam
        //[HttpGet]
        //public IEnumerable<TeamDto> ListTeams()
        //{
        //    List<Team> Teams = db.Teams.ToList();
        //    List<TeamDto> TeamDtos = new List<TeamDto>();

        //    Teams.ForEach(t => TeamDtos.Add(new TeamDto()
        //    {
        //        TeamId = t.TeamId,
        //        TeamName = t.TeamName
        //    }));

        //    return TeamDtos;
        //}

        /// <summary>
        /// Associates a particular TEAM with a particular PLAYER
        /// </summary>
        /// <param name="TeamId">The Team ID primary key</param>
        /// <param name="PlayerId">The Player ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/PlayerData/AssociatePlayerWithTeam/9/1
        /// </example>
        [HttpPost]
        [Route("api/PlayerData/AssociatePlayerWithTeam/{PlayerId}/{TeamId}")]
        [Authorize]
        public IHttpActionResult AssociatePlayerWithTeam(int PlayerId, int TeamId)
        {

            Player SelectedPlayer = db.Players.Include(p => p.TeamId).Where(p => p.PlayerId == PlayerId).FirstOrDefault();
            Team SelectedTeam = db.Teams.Find(TeamId);

            if (SelectedPlayer == null || SelectedTeam == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input player id is: " + PlayerId);
            Debug.WriteLine("selected player name is: " + SelectedPlayer.PlayerName);
            Debug.WriteLine("input team id is: " + TeamId);
            Debug.WriteLine("selected team name is: " + SelectedTeam.TeamName);


            SelectedTeam.Players.Add(SelectedPlayer);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular Team and a particular Player
        /// </summary>
        /// <param name="PlayerId">The Player ID primary key</param>
        /// <param name="TeamId">The Team ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/PlayerData/UnAssociatePlayerWithTeam/9/1
        /// </example>
        [HttpPost]
        [Route("api/PlayerData/UnAssociatePlayerWithTeam/{PlayerId}/{TeamId}")]
        [Authorize]
        public IHttpActionResult UnAssociatePlayerWithTeam(int PlayerId, int TeamId)
        {

            Player SelectedPlayer = db.Players.Include(p => p.Teams).Where(p => p.PlayerId == PlayerId).FirstOrDefault();
            Team SelectedTeam = db.Teams.Find(TeamId);

            if (SelectedPlayer == null || SelectedTeam == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input Player id is: " + PlayerId);
            Debug.WriteLine("selected Player name is: " + SelectedPlayer.PlayerName);
            Debug.WriteLine("input Team id is: " + TeamId);
            Debug.WriteLine("selected Team name is: " + SelectedTeam.TeamName);


            SelectedTeam.Players.Remove(SelectedPlayer);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return db.Players.Count(e => e.PlayerId == id) > 0;
        }
    }
}