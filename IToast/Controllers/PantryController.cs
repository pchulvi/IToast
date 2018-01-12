using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IToast.Models;

namespace IToast.Controllers
{
    /// <summary>
    /// PantryController Class
    /// </summary>
    public class PantryController : ApiController
    {

        private IToastContext db = new IToastContext();


        /// <summary>
        /// How many breads are in our pantry
        /// </summary>
        /// <returns></returns>
        public int HowManyBreads()
        {
            Pantry pantry = db.Pantries.FirstOrDefault();

            return pantry.NumberOfBreads;
            
        }






        /// <summary>
        /// Update the number of breads in our pantry
        /// </summary>
        /// <param name="nBreads">Number of breads</param>

        public void PutBreads(int nBreads)
        {
            Pantry pantry = db.Pantries.FirstOrDefault();
            pantry.NumberOfBreads = nBreads;

            db.SaveChanges();
        }

        /// <summary>
        /// Get a number of breads for the toaster
        /// </summary>
        /// <param name="nBreads">Number of breads</param>
        public void GetBreads(int nBreads)
        {
            if (nBreads < 1) throw new Exception("The number of breads can't be 0 or less 0.");
            if (nBreads > 2) throw new Exception("You can't get more than 2 breads at same time.");

            Pantry pantry = db.Pantries.FirstOrDefault();
            pantry.NumberOfBreads = pantry.NumberOfBreads - nBreads;

            db.SaveChanges();
        }

        /// <summary>
        /// Is there bread in pantry?
        /// </summary>
        /// <returns></returns>
        public bool HasBread()
        {
            return this.HowManyBreads() > 0;
        }

        /// <summary>
        /// Actual status of our pantry: Empty, AlmostEmpy, Normal or Full
        /// </summary>
        /// <returns></returns>
        public PantryStatus GetStatus()
        {
            int howManyBreads = this.HowManyBreads();

            if (howManyBreads == 0) return PantryStatus.Empty;
            if (howManyBreads <= 10) return PantryStatus.AlmostEmpty;
            if (howManyBreads > 90) return PantryStatus.Full;

            return PantryStatus.Normal;
            

        }

        /// <summary>
        /// Buy breads in Supermarket
        /// </summary>
        /// <param name="nBreads"></param>
        public void AskToSupermarket(int nBreads)
        {
            int breads= new SuperMarketController().SellBread(nBreads);

            this.PutBreads(breads);
        }


    }
}
