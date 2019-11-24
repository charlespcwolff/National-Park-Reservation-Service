using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundTest
    {
        [TestMethod]
        public void DailyFeeConvertIntMonthToStringTest()
        {
            Campground campground = new Campground();

            campground.OpeningMthNumber = 1;
            campground.ClosingMthNumber = 2;

            Assert.AreEqual("January", campground.OpeningMonth, "Should print out the correct name for the month.");
            Assert.AreEqual("February", campground.ClosingMonth, "Should print out the correct name for the month.");
        }

        [TestMethod]
        public void CampOpenInDateRangeTest()
        {
            Campground campground = new Campground();

            campground.OpeningMthNumber = 5;
            campground.ClosingMthNumber = 9;

            bool result = campground.CampOpenInDateRange(DateTime.Parse("04/09/2019"), DateTime.Parse("10/09/2019"));
            Assert.IsFalse(result, "Date should be out of acceptable range.");
        }

        [TestMethod]
        public void CalculateFeesTest()
        {
            Campground campground = new Campground();
            

            campground.DailyFee = 35.00M;

            decimal result = campground.CalculateFees(DateTime.Parse("05/10/2019"), DateTime.Parse("05/11/2019"));
            Assert.AreEqual(35, result);

            decimal result1 = campground.CalculateFees(DateTime.Parse("05/10/2019"), DateTime.Parse("05/20/2019"));
            Assert.AreEqual(350, result1);

        }
    }
}
