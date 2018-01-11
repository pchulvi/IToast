using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IToast.Models;

namespace IToast.Controllers
{
    public class PantryController : ApiController
    {

        private IToastContext db = new IToastContext();


        /// <summary>
        /// Nos dice de cuántos panes dispone la despensa
        /// </summary>
        /// <returns></returns>
        public int HowManyBreads()
        {
            Pantry pantry = db.Pantry.First();

            return pantry.NumberOfBreads;
            
        }

        /// <summary>
        /// Actualiza el número de panes existentes
        /// </summary>
        /// 
        public void PutBreads(int nBreads)
        {
            Pantry pantry = db.Pantry.First();
            pantry.NumberOfBreads = nBreads;

            db.SaveChanges();
        }

        public bool HasBread()
        {
            return this.HowManyBreads() > 0;
        }

        public PantryStatus GetStatus()
        {
            int howManyBreads = this.HowManyBreads();

            if (howManyBreads == 0) return PantryStatus.Empty;
            if (howManyBreads <= 10) return PantryStatus.AlmostEmpty;
            if (howManyBreads > 90) return PantryStatus.Full;

            return PantryStatus.Normal;
            

        }

        public void AskToSupermarket(int nBreads)
        {

        }


    }
}
