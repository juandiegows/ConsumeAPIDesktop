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
using System.Web.Http.Description;
using API_Sencilla.Models;

namespace API_Sencilla.Controllers
{
    public class EventosController : ApiController
    {
        private EventoEntities db = new EventoEntities();

        // GET: api/Eventos
        public IQueryable<Evento> GetEvento()
        {
            return db.Evento;
        }

        // GET: api/Eventos/5
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> GetEvento(int id)
        {
            Evento evento = await db.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        // PUT: api/Eventos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvento(int id, Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != evento.ID)
            {
                return BadRequest();
            }

            db.Entry(evento).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        // POST: api/Eventos
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> PostEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Evento.Add(evento);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = evento.ID }, evento);
        }

        // DELETE: api/Eventos/5
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> DeleteEvento(int id)
        {
            Evento evento = await db.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            db.Evento.Remove(evento);
            await db.SaveChangesAsync();

            return Ok(evento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventoExists(int id)
        {
            return db.Evento.Count(e => e.ID == id) > 0;
        }
    }
}