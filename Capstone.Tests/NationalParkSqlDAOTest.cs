using Capstone.DAO;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class NationalParkSqlDAOTest
    {
        #region Test Prep

        protected static string _connectionString  = "Server=.\\SQLEXPRESS;Database=npcampgroundtest;Trusted_Connection=True;";

        private TransactionScope transaction;

        /// <summary>
        /// Begins the transaction for each test.
        /// </summary>
        [TestInitialize]
        public void BeginTestTranaction()
        {
            transaction = new TransactionScope();
        }

        /// <summary>
        /// Closes out the test transaction and removes the data it uses.
        /// </summary>
        [TestCleanup]
        public void RollbackTestTranaction()
        {
            transaction.Dispose();
        }

        #endregion

        #region Campground Tests

        [TestMethod]
        public void GetCampgroundsTest()
        {
            NationalParkSqlDAO npsd = new NationalParkSqlDAO(_connectionString);

            Park park = new Park
            {
                Name = "TestPark",
                Location = "Testa",
                Description = "Nothing",
                EstablishDate = Convert.ToDateTime("01/01/2000"),
                Area = 1,
                AnnualVisitors = 5
            };

            int parkId = npsd.CreatePark(park);

            park.ParkId = parkId;

            Campground camp = new Campground
            {
                Name = "TestCamp",
                DailyFee = 10,
                ClosingMthNumber = 4,
                OpeningMthNumber = 2,
                ParkId = parkId
            };

            int campId = npsd.CreateCamground(camp);

            var result = npsd.GetCampgrounds(park);

            Assert.AreEqual(1, result.Count);
        }

        #endregion

        #region Park Tests

        [TestMethod]
        public void GetParksTest()
        {
            NationalParkSqlDAO npsd = new NationalParkSqlDAO(_connectionString);

            Park park = new Park
            {
                Name = "TestPark",
                Location = "Testa",
                Description = "Nothing",
                EstablishDate = Convert.ToDateTime("01/01/2000"),
                Area = 1,
                AnnualVisitors = 5
            };

            int parkId = npsd.CreatePark(park);

            park.ParkId = parkId;

            var result = npsd.GetParks();

            Assert.AreEqual(1, result.Count);
        }

        #endregion

        #region Site Tests

        [TestMethod]
        public void GetAvailableSitesTest()
        {
            NationalParkSqlDAO npsd = new NationalParkSqlDAO(_connectionString);

            Park park = new Park
            {
                Name = "TestPark",
                Location = "Testa",
                Description = "Nothing",
                EstablishDate = Convert.ToDateTime("01/01/2000"),
                Area = 1,
                AnnualVisitors = 5
            };

            int parkId = npsd.CreatePark(park);

            park.ParkId = parkId;

            Campground camp = new Campground
            {
                Name = "TestCamp",
                DailyFee = 10,
                ClosingMthNumber = 12,
                OpeningMthNumber = 2,
                ParkId = parkId
            };

            int campId = npsd.CreateCamground(camp);
            camp.CampgroundId = campId;

            Site site = new Site
            {
                Accessible = true,
                CampgroundId = campId,
                MaxOccupancy = 25,
                MaxRvLength = 0,
                SiteNumber = 29,
                Utilities = false
            };

            int siteId = npsd.CreateSite(site);
            site.SiteId = siteId;

            var result = npsd.GetAvailableSites(camp, 1, true, 25, false, Convert.ToDateTime("10/10/2025"), Convert.ToDateTime("11/11/2025"));

            Assert.AreEqual(0, result.Count);
        }

        #endregion
    }
}
