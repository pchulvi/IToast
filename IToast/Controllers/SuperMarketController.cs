using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IToast.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SuperMarketController : ApiController
    {
        /// <summary>
        /// Action of selling bread, from Supermarket (see api/pantry/breads/buy/nBreads)
        /// This method only returns a number of breads. 
        /// </summary>
        /// <param name="nBreads">Number of breads. It can't be more than 60 breads</param>
        /// <returns></returns>
        public int SellBread(int nBreads)
        {
            //if (nBreads < 1) throw new Exception("I can't sell less 1 bread");
            if (nBreads > 60) throw new Exception("I can't sell more than 60 breads");
            return nBreads;
        }
    }
}
