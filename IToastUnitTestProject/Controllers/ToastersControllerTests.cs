using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IToast.Controllers.Tests
{
    [TestClass()]
    public class ToastersControllerTests
    {
        [TestMethod()]
        public void GetToastersTest()
        {
            var controller = new ToastersController();
            Assert.IsTrue(controller.GetToasters().Count() > 0, "There must be at least a toaster.");
        }

        [TestMethod()]
        public void GetCurrentStatusTest()
        {
            var controller = new ToastersController();
            if(controller.GetCurrentStatus() == Models.Status.On) controller.Toast(Models.Status.Off);

            Assert.IsTrue(controller.GetCurrentStatus() == Models.Status.Off, "Toaster is ON");
        }

        [TestMethod()]
        public void Toast_OnTest()
        {
            var controller = new ToastersController();
            if(controller.GetCurrentStatus() == Models.Status.On) controller.Toast(Models.Status.Off);
            controller.SetToasts(2);
            Assert.IsTrue(controller.Toast(Models.Status.On) == Models.Status.On, "The toaster status is Off");
        }

        [TestMethod()]
        public void Toast_OffTest()
        {
            var controller = new ToastersController();
            if(controller.GetCurrentStatus() == Models.Status.Off) controller.Toast(Models.Status.On);
            Assert.IsTrue(controller.Toast(Models.Status.Off) == Models.Status.Off, "The toaster status is On");
        }

        [TestMethod()]
        public void Toast_NumToastsAndTimeTest()
        {
            var controller = new ToastersController();
            if (controller.GetCurrentStatus() == Models.Status.On) controller.Toast(Models.Status.Off);

            Assert.IsTrue(controller.Toast(2, 90) == Models.Status.On, "The toaster status is Off");
        }

        //[TestMethod()]
        //public void IsToastingTest()
        //{
        //    var controller = new ToastersController();
        //    if (controller.GetCurrentStatus() == Models.Status.Off) controller.Toast(2, 600);

        //    var dateToTest = System.DateTime.Now.AddMinutes(2);
            
        //    Assert.IsTrue(controller.IsToasting(System.DateTime.Parse(dateToTest.ToString())), "Toaster is Not toasting");
        //}

        [TestMethod()]
        public void TimeRemainingTest()
        {
            var controller = new ToastersController();
            if (controller.GetCurrentStatus() == Models.Status.Off) controller.Toast(2, 600);

            Assert.IsTrue(controller.TimeRemaining() > 0, "Time remaining is 0");
        }

        [TestMethod()]
        public void HowManyToastsMadeTest()
        {
            var controller = new ToastersController();
            Assert.IsTrue(controller.HowManyToastsMade() > 0, "There are no toasts made");
        }

        [TestMethod()]
        public void SetTimeTest()
        {
            var controller = new ToastersController();
            Assert.IsTrue(controller.SetTime(90) == 90, "SetTime is not working");
        }

        [TestMethod()]
        public void SetProfileTest()
        {
            var controller = new ToastersController();
            Assert.IsTrue(controller.SetProfile(Models.Profile.Low) == Models.Profile.Low, "Set Profile is wrong");
        }

        [TestMethod()]
        public void SetToastsTest()
        {
            var controller = new ToastersController();
            Assert.IsTrue(controller.SetToasts(2) == 2, "SetToasts not working");
        }
    }
}