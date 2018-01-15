using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IToast.Controllers;
using IToast.Models;
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

        [TestMethod]
        public void GetBreads_ShouldTwoBreadsMinus()
        {
            var controller = new IToast.Controllers.PantryController();

            controller.PutBreads(50);

            int toasts = 2;

            int howManyBreadsBeforeToasting = controller.HowManyBreads();

            controller.GetBreads(toasts);

            int howManyBreadsAfterToasting = controller.HowManyBreads();

            Assert.AreEqual(howManyBreadsBeforeToasting - howManyBreadsAfterToasting, toasts);
        }
        
        [TestMethod]
        public void HasBread_ShouldHaveBread()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(0);
            controller.BuyToSupermarket(20);

            Assert.IsTrue(controller.HasBread(), "Pantry has bread");

        }

        [TestMethod]
        public void GetStatus_ShouldBeEmptyStatusPantry()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(0);

            Assert.IsTrue(controller.GetStatus() == PantryStatus.Empty, "Pantry empty of breads");

        }

        [TestMethod]
        public void GetStatus_ShouldBeAlmostEmptyStatusPantry()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(6);

            Assert.IsTrue(controller.GetStatus() == PantryStatus.AlmostEmpty, "Pantry almost empty of breads");
        }

        [TestMethod]
        public void GetStatus_ShouldBeNormalStatusPantry()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(49);

            Assert.IsTrue(controller.GetStatus() == PantryStatus.Normal, "Pantry normal number of breads");
        }

        [TestMethod]
        public void GetStatus_ShouldBeFullStatusPantry()
        {
            var controller = new IToast.Controllers.PantryController();
            controller.PutBreads(91);

            Assert.IsTrue(controller.GetStatus() == PantryStatus.Full, "Pantry full of breads");
        }

        [TestMethod]
        public void BuyToSupermarket()
        {
            var controller = new IToast.Controllers.PantryController();

            int breadsToBuy = 20;

            int howManyBreadsBeforeBuying = controller.HowManyBreads();

            controller.BuyToSupermarket(breadsToBuy);

            int howManyBreadsAfterBuying = controller.HowManyBreads();

            Assert.AreEqual(howManyBreadsAfterBuying - howManyBreadsBeforeBuying, breadsToBuy, "Buying "+breadsToBuy+" breads correctly");

        }
    }
}
