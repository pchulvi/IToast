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

        
        /// GET: HowManyBreads
        /// <summary>
        /// How many breads are in our pantry
        /// </summary>
        /// <returns>A number of breads (as int)</returns>
        
        [Route("api/pantry/breads/howmany")]
        [HttpGet]
        public int HowManyBreads()
        {
            
            Pantry pantry = db.Pantries.FirstOrDefault();

            return pantry.NumberOfBreads;
            
        }


        /// <summary>
        /// Update the number of breads in our pantry
        /// </summary>
        /// <param name="nBreads">Number of breads</param>
        [Route("api/pantry/breads/{nBreads}")]
        [HttpPut]
        public void PutBreads(int nBreads)
        {
            Pantry pantry = db.Pantries.FirstOrDefault();
            pantry.NumberOfBreads = nBreads;

            db.SaveChanges();
        }

        /// <summary>
        /// Get a number of breads for the toaster
        /// </summary>
        /// <param name="nBreads">Number of breads. It can't be more than 2 breads in toaster</param>
        
        [Route("api/pantry/breads/{nBreads}")]
        [HttpGet]
        public int GetBreads(int nBreads)
        {
            //if (nBreads < 1) throw new Exception("The number of breads can't be 0 or less 0.");
            if (nBreads > 2) throw new Exception("You can't get more than 2 breads at same time.");


            Pantry pantry = db.Pantries.FirstOrDefault();
            pantry.NumberOfBreads = pantry.NumberOfBreads - nBreads;

            if (pantry.NumberOfBreads < 0) throw new Exception(String.Format("Insufficient breads for toasting. There are {0} breads now in pantry.", pantry.NumberOfBreads));

            db.SaveChanges();

            return nBreads;
        }

        /// <summary>
        /// Is there bread in pantry?
        /// </summary>
        /// <returns>Boolean</returns>
        [Route("api/pantry/hasbread")]
        [HttpGet]
        public bool HasBread()
        {
            return this.HowManyBreads() > 0;
        }

        /// <summary>
        /// Actual status of our pantry: 0 = Empty, 1 = AlmostEmpy, 2 = Normal or 3 = Full
        /// </summary>
        /// <returns>PantryStatus Object</returns>
        [Route("api/pantry/status")]
        [HttpGet]
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
        [Route("api/pantry/breads/buy/{nBreads}")]
        [HttpPost]
        public void BuyToSupermarket(int nBreads)
        {
            int breads= new SuperMarketController().SellBread(nBreads);

            int howmanybreadsNow = this.HowManyBreads();
            this.PutBreads(howmanybreadsNow + breads);
        }


    }
}
