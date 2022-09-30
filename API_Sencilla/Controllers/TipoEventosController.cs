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
    public class TipoEventosController : ApiController
    {
        private EventoEntities db = new EventoEntities();

        // GET: api/TipoEventos
        public IQueryable<TipoEvento> GetTipoEvento()
        {
            return db.TipoEvento;
        }

        // GET: api/TipoEventos/5
        [ResponseType(typeof(TipoEvento))]
        public async Task<IHttpActionResult> GetTipoEvento(int id)
        {
            TipoEvento tipoEvento = await db.TipoEvento.FindAsync(id);
            if (tipoEvento == null)
            {
                return NotFound();
            }

            return Ok(tipoEvento);
        }

        // PUT: api/TipoEventos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTipoEvento(int id, TipoEvento tipoEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoEvento.ID)
            {
                return BadRequest();
            }

            db.Entry(tipoEvento).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEventoExists(id))
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

        // POST: api/TipoEventos
        [ResponseType(typeof(TipoEvento))]
        public async Task<IHttpActionResult> PostTipoEvento(TipoEvento tipoEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoEvento.Add(tipoEvento);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tipoEvento.ID }, tipoEvento);
        }

        // DELETE: api/TipoEventos/5
        [ResponseType(typeof(TipoEvento))]
        public async Task<IHttpActionResult> DeleteTipoEvento(int id)
        {
            TipoEvento tipoEvento = await db.TipoEvento.FindAsync(id);
            if (tipoEvento == null)
            {
                return NotFound();
            }

            db.TipoEvento.Remove(tipoEvento);
            await db.SaveChangesAsync();

            return Ok(tipoEvento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoEventoExists(int id)
        {
            return db.TipoEvento.Count(e => e.ID == id) > 0;
        }
    }
}