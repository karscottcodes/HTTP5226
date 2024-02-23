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

namespace PP_HTTP5226.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Provides all TEAM information
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all TEAM information in the database
        /// <example>
        /// GET: api/TeamData/ListTeams
        /// </example>
        [HttpGet]
        public IEnumerable<TeamDto> ListTeams()
        {
            List<Team> Teams = db.Teams.ToList();
            List<TeamDto> TeamDtos = new List<TeamDto>();

            Teams.ForEach(t => TeamDtos.Add(new TeamDto()
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName
            }));

            return TeamDtos;
        }

        /// <summary>
        /// Provides all TEAM information, associated with a Team ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all TEAM information in the database, associated with a Team Id
        /// <param = "TeamId">TeamId</param>
        /// <example>
        /// GET: api/TeamData/FindTeam/5
        /// </example>
        [ResponseType(typeof(Team))]
        [HttpGet]
        public IHttpActionResult FindTeam(int id)
        {
            Team Team = db.Teams.Find(id);
            if(Team == null)
            {
                return NotFound();
            }
            TeamDto TeamDto = new TeamDto()
            {
                TeamId = Team.TeamId,
                TeamName = Team.TeamName
            }; 

            return Ok(TeamDto);
        }

        /// <summary>
        /// Updates TEAM information based on TEAMid
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: updates TEAM information in the database based on teamid
        /// <param = "teamId">TeamId</param>
        /// <example>
        /// POST: api/TeamData/UpdateTeam/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeam(int id, Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.TeamId)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        /// <summary>
        /// Provides all TEAM information associated with a PLAYER
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all TEAM information in the database, that is associated with a specific PALYER ID
        /// </returns>
        /// <param name="id">Player ID</param>
        /// <example>
        /// GET: api/TeamData/ListTeamsForPlayer/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TeamDto))]

        public IHttpActionResult ListTeamsForPlayer(int id)
        {
            List<Team> Teams = db.Teams.Include(t => t.Players).Where(t => t.TeamId == id).ToList();
            List<TeamDto> TeamDtos = new List<TeamDto>();

            Teams.ForEach(t => TeamDtos.Add(new TeamDto()
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName
            }));

            return Ok(TeamDtos);

        }

        // POST: api/TeamData/AddTeam
        // Adds new team to db based on form data
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult AddTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = team.TeamId }, team);
        }

        // POST: api/TeamData/DeleteTeam/5
        // Delete team based on team id
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult DeleteTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            db.SaveChanges();

            return Ok(team);
        }

        // GET: api/TeamData/ListPlayersByTeam
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




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamId == id) > 0;
        }
    }
}
