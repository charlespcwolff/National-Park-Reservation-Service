using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        private enum Months
        {
            January = 1,
            February = 2,
            March = 3, 
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpeningMthNumber { get; set; }
        public int ClosingMthNumber { get; set; }
        public string OpeningMonth
        {
            get
            {
                return ConvertIntMonthToString(OpeningMthNumber);
            }
        }
        public string ClosingMonth
        {
            get
            {
                return ConvertIntMonthToString(ClosingMthNumber);
            }
        }
        public decimal DailyFee { get; set; }

        /// <summary>
        /// Convertes a month into its name from a int value.
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string ConvertIntMonthToString(int month)
        {
            string name = "";

            if (Enum.TryParse<Months>(month.ToString(), out Months monthName))
            {
                name = monthName.ToString();
            }
            else
            {
                throw new Exception();
            }

            return name;
        }

        /// <summary>
        /// Returns whether the campground is open in a given date range.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool CampOpenInDateRange(DateTime startDate, DateTime endDate)
        {
            bool isOpen = false;

            if (startDate.Month >= OpeningMthNumber && endDate.Month <= ClosingMthNumber)
            {
                isOpen = true;
            }

            return isOpen;
        }

        /// <summary>
        /// Returns total fees for the campground, based on a date range.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal CalculateFees(DateTime startDate, DateTime endDate)
        {
            double totalDays = (endDate - startDate).TotalDays;

            return DailyFee * (decimal)totalDays;
        }
    }
}
