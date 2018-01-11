﻿using System;
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

        // GET: api/Toasters?status=1
        /// <summary>
        /// Starts | Stops the toaster
        /// </summary>
        /// <param name="status">State of the toaster</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Toaster))]
        public async Task<IHttpActionResult> Toast(Status status)
        {
            Toaster toaster = db.Toasters.FirstOrDefault();

            if (toaster.Status == status) return StatusCode(HttpStatusCode.NoContent);
            
            toaster.Status = status;
            
            db.Entry(toaster).State = EntityState.Modified;

            switch (toaster.Status)
            {
                case Status.On:
                    toaster.ToastsMade += 1;
                    toaster.TimeStart = DateTime.Now.ToString();
                    toaster.TimeEnd = DateTime.Now.AddSeconds(toaster.Time).ToString();
                    break;

                default:
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

        // GET: api/Toasters?interval=01/01/2018 10:00:00
        /// <summary>
        /// Returns whether the toaster is toasting or not
        /// </summary>
        /// <param name="interval">Current dateTime interval</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns>True | False</returns>
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