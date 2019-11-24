using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAO
{
    public class NationalParkSqlDAO
    {
        #region Properties & Constructor

        private string ConnectionString { get; }
        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";

        public NationalParkSqlDAO(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }

        #endregion

        #region Campground

        /// <summary>
        /// Gets an alphabetically sorted dictionary of all campgrounds for a park from the database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Campground> GetCampgrounds(Park park)
        {
            var campList = new Dictionary<int, Campground>();

            const string sql = "SELECT * FROM campground " +
                               "JOIN park on park.park_id = campground.park_id " +
                               "WHERE park.park_id = @park_id " +
                               "ORDER BY campground.name;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@park_id", park.ParkId);

                SqlDataReader reader = cmd.ExecuteReader();

                int campgroundCount = 0;

                while (reader.Read())
                {
                    campgroundCount++;
                    campList.Add(campgroundCount, GetCampgroundFromReader(reader));
                }
            }

            return campList;
        }

        /// <summary>
        /// Adds a campground to the database. Returns the ID it created.
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public int CreateCamground(Campground camp)
        {
            int result = 0;

            const string sql = "INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                               "VALUES (@park_id, @name, @open_from_mm, @open_to_mm, @daily_fee);";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);

                cmd.Parameters.AddWithValue("@park_id", camp.ParkId);
                cmd.Parameters.AddWithValue("@name", camp.Name);
                cmd.Parameters.AddWithValue("@open_from_mm", camp.OpeningMthNumber);
                cmd.Parameters.AddWithValue("@open_to_mm", camp.ClosingMthNumber);
                cmd.Parameters.AddWithValue("@daily_fee", camp.DailyFee);

                result = (int)cmd.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Creates a campground object from a sql reader line.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            var camp = new Campground
            {
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                ParkId = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                OpeningMthNumber = Convert.ToInt32(reader["open_from_mm"]),
                ClosingMthNumber = Convert.ToInt32(reader["open_to_mm"]),
                DailyFee = Convert.ToDecimal(reader["daily_fee"])
            };

            return camp;
        }

        #endregion

        #region Park

        /// <summary>
        /// Gets an alphabetically sorted dictionary of all parks from the database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Park> GetParks()
        {
            var parkList = new Dictionary<int, Park>();

            const string sql = "SELECT * From park ORDER BY park.name;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                int parkCount = 0;

                while (reader.Read())
                {
                    parkCount++;
                    parkList.Add(parkCount, GetParkFromReader(reader));
                }
            }

            return parkList;
        }

        /// <summary>
        /// Creates a park object from a sql reader line.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Park GetParkFromReader(SqlDataReader reader)
        {
            var park = new Park
            {
                ParkId = Convert.ToInt32(reader["park_id"]),
                Name = Convert.ToString(reader["name"]),
                Location = Convert.ToString(reader["location"]),
                EstablishDate = Convert.ToDateTime(reader["establish_date"]),
                Area = Convert.ToInt32(reader["area"]),
                AnnualVisitors = Convert.ToInt32(reader["visitors"]),
                Description = Convert.ToString(reader["description"])
            };

            return park;
        }

        /// <summary>
        /// Adds a park to the database. Returns the ID it created.
        /// </summary>
        /// <param name="park"></param>
        /// <returns></returns>
        public int CreatePark(Park park)
        {
            int result = 0;

            const string sql = "INSERT INTO park (name, location, establish_date, area, visitors, description) " +
                               "VALUES (@name, @location, @establish_date, @area, @visitors, @description);";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);

                cmd.Parameters.AddWithValue("@name", park.Name);
                cmd.Parameters.AddWithValue("@location", park.Location);
                cmd.Parameters.AddWithValue("@establish_date", park.EstablishDate);
                cmd.Parameters.AddWithValue("@area", park.Area);
                cmd.Parameters.AddWithValue("@visitors", park.AnnualVisitors);
                cmd.Parameters.AddWithValue("@description", park.Description);

                result = (int)cmd.ExecuteScalar();
            }

            return result;
        }

        #endregion

        #region Site

        /// <summary>
        /// Gets an numerically sorted list of all sites for a park from the database.
        /// </summary>
        /// <param name="campId"></param>
        /// <returns></returns>
        public Dictionary<int, Site> GetAvailableSites(Campground camp, int occupancy, bool accessible, 
                                   int rvLength, bool utilities, DateTime startDate, DateTime endDate)
        {
            var siteList = new Dictionary<int, Site>();

            const string sql = "SELECT TOP 5 site.site_id, site.campground_id, site.site_number, site.max_occupancy, " +
                               "accessible, max_rv_length, utilities, campground.daily_fee FROM site " +
                               "JOIN campground ON campground.campground_id = site.campground_id " +
                               "LEFT JOIN reservation ON reservation.site_id = site.site_id " +
                               "WHERE campground.campground_id = @campground_id AND site.max_occupancy >= @occupancy " +
                               "AND site.accessible >= @accessible AND site.max_rv_length >= @rv_length AND site.utilities >= @utilities " +
                               "AND NOT ((reservation.from_date BETWEEN @start_date AND @end_date) " +
                               "AND NOT (reservation.to_date BETWEEN @start_date AND @end_date)) " +
                               "GROUP BY site.site_id, site.campground_id, site.site_number, site.max_occupancy, site.accessible, " +
                               "site.max_rv_length, site.utilities, campground.daily_fee ORDER BY site.site_id;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@campground_id", camp.CampgroundId);
                cmd.Parameters.AddWithValue("@occupancy", occupancy);
                cmd.Parameters.AddWithValue("@accessible", accessible);
                cmd.Parameters.AddWithValue("@rv_length", rvLength);
                cmd.Parameters.AddWithValue("@utilities", utilities);
                cmd.Parameters.AddWithValue("@start_date", startDate);
                cmd.Parameters.AddWithValue("@end_date", endDate);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var site = GetSiteFromReader(reader);
                    siteList.Add(site.SiteNumber, site);
                }
            }

            return siteList;
        }

        /// <summary>
        /// Gets an numerically sorted list of all sites for a park from the database.
        /// </summary>
        /// <param name="campId"></param>
        /// <returns></returns>
        public Dictionary<int, Site> GetAvailableSitesParkWide(Park park, int occupancy, bool accessible,
                                   int rvLength, bool utilities, DateTime startDate, DateTime endDate)
        {
            var siteList = new Dictionary<int, Site>();

            const string sql = "SELECT TOP 5 park.park_id, site.site_id, site.campground_id, campground.name, site.site_number, " +
                               "site.max_occupancy, accessible, max_rv_length, utilities, campground.daily_fee FROM site " +
                               "JOIN campground ON campground.campground_id = site.campground_id " +
                               "JOIN park ON park.park_id = campground.park_id LEFT " +
                               "JOIN reservation ON reservation.site_id = site.site_id " +
                               "WHERE park.park_id = @park_id AND site.max_occupancy >= @occupancy AND site.accessible >= @accessible " +
                               "AND site.max_rv_length >= @rv_length AND site.utilities >= @utilities " +
                               "AND NOT ((reservation.from_date BETWEEN @start_date AND @end_date) " +
                               "AND NOT (reservation.to_date BETWEEN @start_date AND @end_date)) " +
                               "AND @start_month >= campground.open_from_mm AND @end_month <= campground.open_to_mm " +
                               "GROUP BY park.park_id, campground.name, site.site_id, site.campground_id, site.site_number, site.max_occupancy, " +
                               "site.accessible, site.max_rv_length, site.utilities, campground.daily_fee ORDER BY site.site_id;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@park_id", park.ParkId);
                cmd.Parameters.AddWithValue("@occupancy", occupancy);
                cmd.Parameters.AddWithValue("@accessible", accessible);
                cmd.Parameters.AddWithValue("@rv_length", rvLength);
                cmd.Parameters.AddWithValue("@utilities", utilities);
                cmd.Parameters.AddWithValue("@start_date", startDate);
                cmd.Parameters.AddWithValue("@end_date", endDate);
                cmd.Parameters.AddWithValue("@start_month", startDate.Month);
                cmd.Parameters.AddWithValue("@end_month", endDate.Month);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var site = GetSiteFromReader(reader);
                    siteList.Add(site.SiteNumber, site);
                }
            }

            return siteList;
        }

        /// <summary>
        /// Retrieves the campground for a site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Campground GetCampgroundForSite(int siteId)
        {
            Campground campground = null;

            const string sql = "SELECT * FROM campground " +
                               "JOIN site ON site.campground_id = campground.campground_id " +
                               "WHERE site.site_id = @site_id;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@site_id", siteId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campground = GetCampgroundFromReader(reader);
                }
            }

            return campground;
        }

        /// <summary>
        /// Creates a site from an sql reader line.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Site GetSiteFromReader(SqlDataReader reader)
        {
            var site = new Site
            {
                SiteId = Convert.ToInt32(reader["site_id"]),
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                SiteNumber = Convert.ToInt32(reader["site_number"]),
                MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                Accessible = Convert.ToBoolean(reader["accessible"]),
                MaxRvLength = Convert.ToInt32(reader["max_rv_length"]),
                Utilities = Convert.ToBoolean(reader["utilities"])
            };

            return site;
        }

        /// <summary>
        /// Adds a site to the database. Returns the ID it created.
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int CreateSite(Site site)
        {
            int result = 0;

            const string sql = "INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) " +
                               "VALUES (@campground_id, @site_number, @max_occupancy, @accessible, @max_rv_length, @utilities);";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);

                cmd.Parameters.AddWithValue("@campground_id", site.CampgroundId);
                cmd.Parameters.AddWithValue("@site_number", site.SiteId);
                cmd.Parameters.AddWithValue("@max_occupancy", site.MaxOccupancy);
                cmd.Parameters.AddWithValue("@accessible", site.Accessible);
                cmd.Parameters.AddWithValue("@max_rv_length", site.MaxRvLength);
                cmd.Parameters.AddWithValue("@utilities", site.Utilities);

                result = (int)cmd.ExecuteScalar();
            }

            return result;
        }

        #endregion

        #region Reservation

        /// <summary>
        /// Retrieves a list of upcoming reservations for a park.
        /// </summary>
        /// <param name="park"></param>
        /// <returns></returns>
        public List<Reservation> GetUpcomingParkReservations(Park park)
        {
            List<Reservation> upcomingReservations = new List<Reservation>();

            const string sql = "SELECT * FROM reservation " +
                               "JOIN site ON site.site_id = reservation.site_id " +
                               "JOIN campground ON campground.campground_id = site.campground_id " +
                               "WHERE reservation.from_date BETWEEN @nowDate AND @30_later AND campground.park_id = 1;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nowDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@30_later", DateTime.Now.AddDays(30));

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    upcomingReservations.Add(GetReservationFromReader(reader));
                }
            }

            return upcomingReservations;
        }

        /// <summary>
        /// Retrieves a list of the reservation history for a user, sorted by start dates.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Reservation> GetUserReservationHistory(int userId)
        {
            List<Reservation> upcomingReservations = new List<Reservation>();

            const string sql = "SELECT * FROM reservation " +
                               "JOIN UserReservation ON UserReservation.ReservationId = reservation.reservation_id " +
                               "WHERE UserReservation.UserId = @user_id ORDER BY reservation.from_date;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    upcomingReservations.Add(GetReservationFromReader(reader));
                }
            }

            return upcomingReservations;
        }

        /// <summary>
        /// Creates a reservation from an sql reader line.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Reservation GetReservationFromReader(SqlDataReader reader)
        {
            var reservation = new Reservation
            {
                ReservationId = Convert.ToInt32(reader["reservation_id"]),
                SiteId = Convert.ToInt32(reader["site_id"]),
                Name = Convert.ToString(reader["name"]),
                FromDate = Convert.ToDateTime(reader["from_date"]),
                ToDate = Convert.ToDateTime(reader["to_date"]),
                CreateDate = Convert.ToDateTime(reader["create_date"])
            };

            return reservation;
        }

        /// <summary>
        /// Makes a reservation by adding to both the reservation and user reservation tables.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="reservationName"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="departureDate"></param>
        /// <returns></returns>
        public int MakeReservation(int userId, int siteId, string reservationName, DateTime arrivalDate, DateTime departureDate)
        {
            int reservationId = 0;

            const string reservationSql = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                                          "VALUES (@site_id, @name, @from_date, @to_date, @create_date); ";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(reservationSql + _getLastIdSQL, conn);

                cmd.Parameters.AddWithValue("@site_id", siteId);
                cmd.Parameters.AddWithValue("@name", reservationName);
                cmd.Parameters.AddWithValue("@from_date", arrivalDate);
                cmd.Parameters.AddWithValue("@to_date", departureDate);
                cmd.Parameters.AddWithValue("@create_date", DateTime.Now);

                reservationId = (int)cmd.ExecuteScalar();
            }

            const string addUserSql = "INSERT INTO UserReservation(UserId, ReservationId) " +
                                      "VALUES (@UserId, @reservationID); ";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(addUserSql + _getLastIdSQL, conn);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@reservationID", reservationId);

                cmd.ExecuteScalar();
            }

            return reservationId;
        }

        #endregion
    }
}
