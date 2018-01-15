using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IToast.Controllers;
using System.Threading.Tasks;

namespace IToastUnitTestProject
{
    [TestClass]
    public class PantryControllerTest
    {
        [TestMethod]
        public void HowManyBreads_ShouldReturnMoreThanOneBread()
        {
            var controller = new IToast.Controllers.PantryController();

            Assert.IsTrue(controller.HowManyBreads() > 0,"True, has more almost one bread");

        }

        [TestMethod]
        public void PutBreads_ShouldReturn50BreadsInPantry()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(50);

            int howmanybreads = controller.HowManyBreads();

            Assert.IsTrue(howmanybreads == 50, "There are 50 breads in pantry");

        }

      

        
        // public void GetBreads(int nBreads)
        // public bool HasBread()
        //public PantryStatus GetStatus()
        // public void BuyToSupermarket(int nBreads)
    }
}
