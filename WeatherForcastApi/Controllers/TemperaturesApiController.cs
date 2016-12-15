using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WeatherForcastApi.Controllers;
using WeatherForcastApi.Hubs;
using WeatherForcastApi.Models;

namespace WeatherForcastApi.Controllers
{
    [EnableCors("*", "*", "GET,POST,PUT")]
    public class TemperaturesApiController : ApiControllerWithHub<TemperatureHub>
    {
        private TemperatureEntitesEntities db = new TemperatureEntitesEntities();

        // GET: api/TemperaturesApi
        public IQueryable<Temperature> GetTemperatures()
        {
            return db.Temperatures;
        }

        // GET: api/TemperaturesApi/5
        [ResponseType(typeof(Temperature))]
        public async Task<IHttpActionResult> GetTemperature(int id)
        {
            Temperature temperature = await db.Temperatures.FindAsync(id);
            if (temperature == null)
            {
                return NotFound();
            }

            return Ok(temperature);
        }

        // PUT: api/TemperaturesApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTemperature(int id, Temperature temperature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != temperature.Id)
            {
                return BadRequest();
            }

            db.Entry(temperature).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                Hub.Clients.All.reciveTemperature(temperature);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemperatureExists(id))
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

        // POST: api/TemperaturesApi
        [ResponseType(typeof(Temperature))]
        public async Task<IHttpActionResult> PostTemperature(Temperature temperature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Temperatures.Add(temperature);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = temperature.Id }, temperature);
        }

        // DELETE: api/TemperaturesApi/5
        [ResponseType(typeof(Temperature))]
        public async Task<IHttpActionResult> DeleteTemperature(int id)
        {
            Temperature temperature = await db.Temperatures.FindAsync(id);
            if (temperature == null)
            {
                return NotFound();
            }

            db.Temperatures.Remove(temperature);
            await db.SaveChangesAsync();

            return Ok(temperature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TemperatureExists(int id)
        {
            return db.Temperatures.Count(e => e.Id == id) > 0;
        }
    }
}