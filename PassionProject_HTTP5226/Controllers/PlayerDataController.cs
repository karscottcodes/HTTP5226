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
using PassionProject_HTTP5226.Models;
using System.Diagnostics;

namespace PassionProject_HTTP5226.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PlayerData/ListPlayers
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
                TeamName = p.Teams.TeamName
            }));

            return PlayerDtos;
        }

        // GET: api/PlayerData/FindPlayer/5
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
            };
            if (Player == null)
            {
                return NotFound();
            }

            return Ok(PlayerDto);
        }

        // POST: api/PlayerData/UpdatePlayer/5
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