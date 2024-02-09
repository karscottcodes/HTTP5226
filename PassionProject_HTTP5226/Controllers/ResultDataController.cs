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

namespace PassionProject_HTTP5226.Controllers
{
    public class ResultDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ResultData/ListResults
        [HttpGet]
        public IEnumerable<ResultDto> ListResults()
        {
            List<Result> Results = db.Results.ToList();
            List<ResultDto> ResultDtos = new List<ResultDto>();

            Results.ForEach(r => ResultDtos.Add(new ResultDto()
            {
                ResultId = r.ResultId,
                ResultDate = r.ResultDate,
                ResultNote = r.ResultNote
            }));

            return ResultDtos;
        }

        // GET: api/ResultData/ListResult/5
        [ResponseType(typeof(Result))]
        [HttpGet]
        public IHttpActionResult FindResult(int id)
        {
            Result Result = db.Results.Find(id);
            ResultDto ResultDto = new ResultDto()
            {
                ResultId = Result.ResultId,
                ResultNote = Result.ResultNote,
                ResultDate = Result.ResultDate

            };
            if (Result == null)
            {
                return NotFound();
            }

            return Ok(ResultDto);
        }

        // POST: api/ResultData/UpdateResult/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateResult(int id, Result result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != result.ResultId)
            {
                return BadRequest();
            }

            db.Entry(result).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
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

        // POST: api/ResultData/AddResult
        [ResponseType(typeof(Result))]
        [HttpPost]
        public IHttpActionResult AddResult(Result result)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Results.Add(result);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = result.ResultId }, result);
        }

        // POST: api/ResultData/DeletePlayer/5
        [ResponseType(typeof(Result))]
        [HttpPost]
        public IHttpActionResult DeleteResult(int id)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            db.Results.Remove(result);
            db.SaveChanges();

            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResultExists(int id)
        {
            return db.Results.Count(r => r.ResultId == id) > 0;
        }
    }
}