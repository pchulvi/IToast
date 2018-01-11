using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using IToast.Models;
using System.Threading.Tasks;
using System.Threading;

namespace IToast.Controllers
{
    public class ToastersController : ApiController
    {
        private IToastContext db = new IToastContext();

        // GET: api/Toasters
        /// <summary>
        /// GetToasters
        /// </summary>
        /// <returns></returns>
        public IQueryable<Toaster> GetToasters()
        {
            return db.Toasters;
        }

        // GET: api/Toasters/5
        /// <summary>
        /// GetToaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult GetToaster(int id)
        {
            Toaster toaster = db.Toasters.Find(id);
            if (toaster == null)
            {
                return NotFound();
            }

            return Ok(toaster);
        }

        // PUT: api/Toasters/5
        /// <summary>
        /// PutToaster
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toaster"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToaster(int id, Toaster toaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toaster.Id)
            {
                return BadRequest();
            }

            db.Entry(toaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToasterExists(id))
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

        // POST: api/Toasters
        /// <summary>
        /// PostToaster
        /// </summary>
        /// <param name="toaster"></param>
        /// <returns></returns>
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult PostToaster(Toaster toaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Toasters.Add(toaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = toaster.Id }, toaster);
        }

        // DELETE: api/Toasters/5
        /// <summary>
        /// DeleteToaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult DeleteToaster(int id)
        {
            Toaster toaster = db.Toasters.Find(id);
            if (toaster == null)
            {
                return NotFound();
            }

            db.Toasters.Remove(toaster);
            db.SaveChanges();

            return Ok(toaster);
        }

        // GET: api/Toasters?status=1&time=0
        /// <summary>
        /// Starts | Stops the toaster
        /// </summary>
        /// <param name="status"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Toaster))]
        public async Task<IHttpActionResult> Toaster(Status status, int time = 0)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            toaster.Status = status;
            toaster.Time = time;
            
            db.Entry(toaster).State = EntityState.Modified;

            switch (toaster.Status)
            {
                case Status.On:
                    toaster.TimeStart = DateTime.Now.ToShortTimeString();
                    toaster.TimeEnd = DateTime.Now.AddSeconds(time).ToShortTimeString();
                    break;

                default:
                    toaster.Profile = Profile.NoProfile;
                    toaster.TimeStart = new DateTime().ToShortTimeString();
                    toaster.TimeEnd = new DateTime().ToShortTimeString();
                    break;
            }

            //Save data
            db.Entry(toaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return StatusCode(HttpStatusCode.OK);
        }

        // GET
        [HttpGet]
        public Boolean isToasting(DateTime interval)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            return (toaster.Status == Status.On
                && (interval.CompareTo(toaster.TimeStart) >= 0
                && interval.CompareTo(toaster.TimeEnd) <= 0));
        }

        // POST: api/Toasters?time=10
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult SetTime(int time)
        {
            db.Toasters.FirstOrDefault().Time = time;
            db.Toasters.FirstOrDefault().Profile = Profile.NoProfile;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Toasters?profile=1
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult SetProfile(Profile profile)
        {
            db.Toasters.FirstOrDefault().Profile = profile;

            switch (profile)
            {
                case Profile.NoProfile:
                    db.Toasters.FirstOrDefault().Time = 0;
                    break;
                case Profile.Low:
                    db.Toasters.FirstOrDefault().Time = 90;
                    break;
                case Profile.Normal:
                    db.Toasters.FirstOrDefault().Time = 180;
                    break;
                case Profile.High:
                    db.Toasters.FirstOrDefault().Time = 360;
                    break;
                case Profile.Burnt:
                    db.Toasters.FirstOrDefault().Time = 600;
                    break;

                default:
                    throw new Exception("Profile error.");
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// TOasterExists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ToasterExists(int id)
        {
            return db.Toasters.Count(e => e.Id == id) > 0;
        }

     }
}