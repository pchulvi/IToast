using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using IToast.Models;
using System.Threading.Tasks;

namespace IToast.Controllers
{
    /// <summary>
    /// Controller of the IToast
    /// </summary>
    public class ToastersController : ApiController
    {
        private IToastContext db = new IToastContext();

        // GET: api/Toasters
        /// <summary>
        /// Gets Toaster info
        /// </summary>
        /// <returns></returns>
        public IQueryable<Toaster> GetToasters()
        {
            return db.Toasters;
        }

        // POST: api/Toasters?numToasts=2
        /// <summary>
        /// Adds toasts to the toaster. The maximum number of toasts is 2
        /// </summary>
        /// <param name="numToasts">Number of toasts</param>
        [HttpPost]
        public void InsertToasts(int numToasts)
        {
            if (numToasts > 2) throw new Exception("The maximum number of toasts is 2.");

            Toaster toaster = db.Toasters.FirstOrDefault();
            toaster.NumToasts = numToasts;

            db.Entry(toaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // POST: api/Toasters?status=1
        /// <summary>
        /// Turns on/off the toaster. The conditions to turn on the toaster are:
        /// 1.There must be toasts in the toaster
        /// 2.A time interval must be set
        /// </summary>
        /// <param name="status">State of the toaster</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Toaster))]
        public async Task<IHttpActionResult> Toast(Status status)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            PantryController pantry = new PantryController();
            
            if (toaster.Status == status) return StatusCode(HttpStatusCode.NoContent);
            
            toaster.Status = status;
            
            db.Entry(toaster).State = EntityState.Modified;

            switch (toaster.Status)
            {
                case Status.On:
                    if (toaster.NumToasts > 0 && pantry.HasBread())
                    {
                        //pantry.GetBreads(numToasts);
                        toaster.ToastsMade += toaster.NumToasts;
                        toaster.TimeStart = DateTime.Now.ToString();
                        toaster.TimeEnd = DateTime.Now.AddSeconds(toaster.Time).ToString();
                    }
                    else
                        throw new Exception("No bread in the Toaster.");
                    break;

                default:
                    toaster.NumToasts = 0;
                    toaster.Profile = Profile.NoProfile;
                    toaster.TimeStart = new DateTime().ToString();
                    toaster.TimeEnd = new DateTime().ToString();
                    break;
            }

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

        /// <summary>
        /// Toasts with a specific number of toasts and time 
        /// </summary>
        /// <param name="numToasts"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Toaster))]
        public async Task<IHttpActionResult> Toast(int numToasts, int time)
        {
            try
            {
                InsertToasts(numToasts);
                SetTime(time);
                await (Toast(Status.On));

                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="numToasts"></param>
        /// <param name="profile"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Toaster))]
        public async Task<IHttpActionResult> Toast(int numToasts, Profile profile, Database date)
        {
            try
            {
                //Debe existir un proceso que reciba la fecha/hora y que ejecute el tostado

                //SetProfile(profile);
                //InsertToasts(numToasts);
                //await Toast(Status.On);

                return StatusCode(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Toasters?interval=01/01/2018 10:00:00
        /// <summary>
        /// Returns whether the toaster is toasting or not
        /// </summary>
        /// <param name="interval">Current dateTime interval</param>
        /// <returns>True = toasting | False = not toasting</returns>
        [HttpGet]
        [ResponseType(typeof(Toaster))]
        public Boolean IsToasting(DateTime interval)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();

            int s = interval.CompareTo(DateTime.Parse(toaster.TimeStart));
            int e = interval.CompareTo(DateTime.Parse(toaster.TimeEnd));

            return (toaster.Status == Status.On && (s + e == 0 ? true : false));
        }

        /// <summary>
        /// Gets remaining time of the Toaster to be finished
        /// </summary>
        /// <returns>Number of seconds remaining</returns>
        [HttpPost]
        public int TimeRemaining()
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            if (DateTime.Parse(toaster.TimeEnd) > DateTime.Now)
            {
                DateTime endTime = DateTime.Parse(toaster.TimeEnd);
                return ((endTime - DateTime.Now).Minutes * 60) + (endTime - DateTime.Now).Seconds;
            }
            else
                return 0;
        }

        // POST: api/Toasters?time=10
        /// <summary>
        /// Sets the toasting time of the toaster
        /// </summary>
        /// <param name="time">Number of seconds</param>
        /// <returns></returns>
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult SetTime(int time)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            toaster.Time = time;
            toaster.Profile = Profile.NoProfile;

            db.Entry(toaster).State = EntityState.Modified;

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
        /// <summary>
        /// Sets the current profile of the toaster
        /// </summary>
        /// <param name="profile">NoProfile = 0 | Low = 1 | Normal = 2 | High = 3 | Burnt = 4</param>
        /// <returns></returns>
        [ResponseType(typeof(Toaster))]
        public IHttpActionResult SetProfile(Profile profile)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();
            toaster.Profile = profile;

            switch (profile)
            {
                case Profile.NoProfile:
                    toaster.Time = 0;
                    break;
                case Profile.Low:
                    toaster.Time = 90;
                    break;
                case Profile.Normal:
                    toaster.Time = 180;
                    break;
                case Profile.High:
                    toaster.Time = 360;
                    break;
                case Profile.Burnt:
                    toaster.Time = 600;
                    break;

                default:
                    throw new Exception("Profile error.");
            }

            db.Entry(toaster).State = EntityState.Modified;

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
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ToasterExists(int id)
        {
            return db.Toasters.Count(e => e.Id == id) > 0;
        }

     }
}