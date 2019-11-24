using Capstone.DAO;
using Capstone.Models;
using Security.BusinessLogic;
using Security.DAO;
using Security.Exceptions;
using Security.Models;
using Security.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Menus
{
    public class Menu
    {
        #region Properties & Constructor

        private NationalParkSqlDAO Npsd { get; set; }
        private UserSecurityDAO Dao { get; set; }
        private UserManager Mgr { get; set; }

        public Menu(string connectionString)
        {
            Npsd = new NationalParkSqlDAO(connectionString);
            Dao = new UserSecurityDAO(connectionString);
            Mgr = new UserManager(Dao);
        }

        #endregion

        #region Main Menu

        /// <summary>
        /// Runs the menus for the program.
        /// </summary>
        public void MainMenu()
        {
            LoginScreen();

            if (Mgr.IsAuthenticated)
            {
                NationalParksMenu();
                Mgr.LogoutUser();
            }
        }

        #endregion

        #region National Parks Menus

        /// <summary>
        /// Main menu for 
        /// </summary>
        private void NationalParksMenu()
        {

            bool exit = false;

            while (!exit)
            {
                var parks = Npsd.GetParks();

                Console.Clear();

                PrintColoredMessage($"Welcome to national parks, {Mgr.User.FirstName}!", ConsoleColor.Yellow);
                Console.WriteLine();
                Console.WriteLine($"Select a Park for further details:");

                foreach (var park in parks)
                {
                    Console.WriteLine($"{park.Key}) {park.Value.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("H) Show user history");
                Console.WriteLine("Q) Quit");
                string userSelection = Console.ReadLine();

                if (userSelection == "Q" || userSelection == "q" 
                    || userSelection == "Quit" || userSelection == "quit")
                {
                    exit = true;
                }
                else if (userSelection == "H" || userSelection == "h"
                         || userSelection == "History" || userSelection == "history")
                {
                    ShowUserHistoryMenu();
                }
                else if (int.TryParse(userSelection, out int selectionNumber))
                {
                    try
                    {
                        ParkMenu(parks[selectionNumber]);
                    }
                    catch (KeyNotFoundException)
                    {
                        PrintMessageToScreen("Invalid selection, please try again...");
                    }
                }
                else
                {
                    PrintMessageToScreen("Invalid selection, please try again...");
                }
            }
        }

        /// <summary>
        /// Displays information about the park and menu for further options.
        /// </summary>
        /// <param name="park"></param>
        private void ParkMenu(Park park)
        {
            const ConsoleKey VIEW_CAMP_KEY = ConsoleKey.D1;
            const ConsoleKey SEARCH_RESERVATION_KEY = ConsoleKey.D2;
            const ConsoleKey UPCOMING_RESERVATION_KEY = ConsoleKey.D3;
            const ConsoleKey RETURN_KEY = ConsoleKey.Q;

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintColoredMessage($"{park.Name} National Park Location: {park.Location}", ConsoleColor.Yellow);
                Console.WriteLine($"Established: {park.EstablishDate.ToShortDateString()}");
                Console.WriteLine($"Area: {park.Area}");
                Console.WriteLine($"Annual Visitors: {park.AnnualVisitors}");
                Console.WriteLine();
                Console.WriteLine(park.Description);
                Console.WriteLine();
                Console.WriteLine("Select a Command: ");
                Console.WriteLine($"{VIEW_CAMP_KEY.ToString().Substring(1)}) View Campgrounds");
                Console.WriteLine($"{SEARCH_RESERVATION_KEY.ToString().Substring(1)}) Search for Reservation");
                Console.WriteLine($"{UPCOMING_RESERVATION_KEY.ToString().Substring(1)}) See upcoming reservations");
                Console.WriteLine($"{RETURN_KEY}) Return to Previous Screen");

                var userSelection = Console.ReadKey(true).Key;

                if (userSelection == VIEW_CAMP_KEY)
                {
                    ViewCampground(park);
                }
                else if (userSelection == SEARCH_RESERVATION_KEY)
                {
                    ParkWideSearchAskInputsMenu(park);
                }
                else if (userSelection == UPCOMING_RESERVATION_KEY)
                {
                    ShowUpcomingReservationsMenu(park);
                }
                else if (userSelection == RETURN_KEY)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Displays the history of reservations for the current user.
        /// </summary>
        private void ShowUserHistoryMenu()
        {
            const ConsoleKey RETURN_KEY = ConsoleKey.Q;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                PrintColoredMessage($"Reservation user history for {Mgr.User.Username}:", ConsoleColor.Yellow);

                var userHisory = Npsd.GetUserReservationHistory(Mgr.User.Id);

                Console.WriteLine();
                PrintColoredMessage("Reservation ID".PadRight(15) + "Name".PadRight(40) + "Arrival Date".PadRight(15) + 
                                    "Departure Date".PadRight(15), ConsoleColor.Yellow);

                foreach (var reservation in userHisory)
                {
                    Console.WriteLine($"{reservation.ReservationId.ToString().PadRight(15)}" +
                                      $"{reservation.Name.PadRight(40)}{reservation.ToDate.ToShortDateString().ToString().PadRight(15)}" +
                                      $"{reservation.FromDate.ToShortDateString()}");
                }

                Console.WriteLine();
                Console.WriteLine($"{RETURN_KEY}) Return to prior menu");
                var userSelection = Console.ReadKey(true).Key;

                if (userSelection == RETURN_KEY)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Displays the upcoming reservations for a park for the user.
        /// </summary>
        /// <param name="park"></param>
        private void ShowUpcomingReservationsMenu(Park park)
        {
            const ConsoleKey QUIT_KEY = ConsoleKey.Q;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                PrintColoredMessage($"Here are the upcoming reservations for the next 30 days at {park.Name}: ", ConsoleColor.Yellow);

                var reservations = Npsd.GetUpcomingParkReservations(park);

                Console.WriteLine();
                PrintColoredMessage("Name".PadRight(40) + "Arrival Date".PadRight(15) + "Departure Date", ConsoleColor.Yellow);

                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"{reservation.Name.PadRight(40)}{reservation.ToDate.ToShortDateString().ToString().PadRight(15)}{reservation.FromDate.ToShortDateString()}");
                }

                Console.WriteLine();
                Console.WriteLine($"{QUIT_KEY}) Return to prior menu");

                var userSelection = Console.ReadKey(true).Key;

                if (userSelection == QUIT_KEY)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// View all the campgrounds for that park.
        /// </summary>
        /// <param name="park"></param>
        private void ViewCampground(Park park)
        {
            const ConsoleKey SEARCH_RESERVATION_KEY = ConsoleKey.D1;
            const ConsoleKey RETURN_KEY = ConsoleKey.D2;
            var camps = Npsd.GetCampgrounds(park);

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("   Name".PadRight(39) + "Open".PadRight(11) + "Close".PadRight(11) + "Daily Fee");

                foreach (var camp in camps)
                {
                    Console.WriteLine($"{camp.Key}) {camp.Value.Name.PadRight(35)} {camp.Value.OpeningMonth.PadRight(10)}" +
                        $" {camp.Value.ClosingMonth.PadRight(10)} {camp.Value.DailyFee:c}");
                }

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine($"{SEARCH_RESERVATION_KEY.ToString().Substring(1)}) Search for Available Reservation");
                Console.WriteLine($"{RETURN_KEY.ToString().Substring(1)}) Return to Previous Screen");

                var userSelection = Console.ReadKey(true).Key;

                if (userSelection == SEARCH_RESERVATION_KEY)
                {
                    Console.WriteLine();

                    int campSelected = ParseUserInputRequired("Which campground (enter 0 to cancel)? ");

                    if (campSelected != 0 && camps.ContainsKey(campSelected))
                    {
                        DateTime arrivalDate = ParseUserDateRequired("What is the arrival date (MM/DD/YYYY)? ");
                        DateTime departureDate = ParseUserDateRequired("What is the departure date (MM/DD/YYYY)? ");

                        if (arrivalDate > DateTime.Now && departureDate > arrivalDate)
                        {
                            if (camps[campSelected].CampOpenInDateRange(arrivalDate, departureDate))
                            {
                                int occupancy = ParseUserInputRequired("Number of occupants? ");
                                bool accessible = ParseUserBoolRequired("Accessiblity requirements (true, false): ");
                                int rvLength = ParseUserInputRequired("RV Length requirements (0 if no RV): ");
                                bool utility = ParseUserBoolRequired("Utility hookup Required (true, false)? ");

                                SearchCampgroundForAvailableReservationMenu(camps[campSelected], occupancy, accessible, rvLength, utility, arrivalDate, departureDate);
                            }
                            else
                            {
                                PrintMessageToScreen("Sorry the campground is closed in that date range. Please try a different date range.");
                            }
                        }
                        else
                        {
                            PrintMessageToScreen("Invalid date entry, please try again...");
                        }
                    }
                    else if (campSelected != 0 && !camps.ContainsKey(campSelected))
                    {
                        PrintMessageToScreen("Invalid camp selection, please try again...");
                    }
                }
                else if (userSelection == RETURN_KEY)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Asks the user for the inputs required for a park wide search.
        /// </summary>
        /// <param name="park"></param>
        private void ParkWideSearchAskInputsMenu(Park park)
        {
            var camps = Npsd.GetCampgrounds(park);

            Console.WriteLine();

            DateTime arrivalDate = ParseUserDateRequired("What is the arrival date (MM/DD/YYYY)? ");
            DateTime departureDate = ParseUserDateRequired("What is the departure date (MM/DD/YYYY)? ");

            if (arrivalDate > DateTime.Now && departureDate > arrivalDate)
            {
                int occupancy = ParseUserInputRequired("Number of occupants? ");
                bool accessible = ParseUserBoolRequired("Accessiblity requirements (true, false): ");
                int rvLength = ParseUserInputRequired("RV Length requirements (0 if no RV): ");
                bool utility = ParseUserBoolRequired("Utility hookup Required (true, false)? ");

                SearchParkWideForAvailableReservationsMenu(park, occupancy, accessible, rvLength, utility, arrivalDate, departureDate);
            }
            else
            {
                PrintMessageToScreen("Invalid date entry, please try again...");
            }
        }

        /// <summary>
        /// Takes inputs from the user to display a menu for available reservations to select from.
        /// </summary>
        /// <param name="camp"></param>
        /// <param name="occupancy"></param>
        /// <param name="accessible"></param>
        /// <param name="rvLength"></param>
        /// <param name="utility"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="departureDate"></param>
        private void SearchCampgroundForAvailableReservationMenu(Campground camp, int occupancy, bool accessible, int rvLength, 
                                                            bool utility, DateTime arrivalDate, DateTime departureDate)
        {
            const ConsoleKey MAKE_RESERVATION_KEY = ConsoleKey.D1;
            const ConsoleKey QUIT_KEY = ConsoleKey.Q;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                var sites = Npsd.GetAvailableSites(camp, occupancy, accessible, rvLength, utility, arrivalDate, departureDate);

                Console.WriteLine("Site No.".PadRight(20) + "Max Occup.".PadRight(20) + "Accessible?".PadRight(20) + "Max RV Length".PadRight(20)
                                    + "Utility".PadRight(20) + "Cost");

                foreach (var site in sites)
                {
                    Console.WriteLine($"{site.Value.SiteNumber.ToString().PadRight(20)}{site.Value.MaxOccupancy.ToString().PadRight(20)}" +
                                        $"{site.Value.Accessible.ToString().PadRight(20)} {site.Value.MaxRvLength.ToString().PadRight(20)}" +
                                        $"{site.Value.Utilities.ToString().PadRight(20)}{camp.CalculateFees(arrivalDate, departureDate):c}");
                }

                if (sites.Count == 0)
                {
                    Console.WriteLine("Sorry, there aren't any available sites with your requirements.");
                }

                Console.WriteLine();
                Console.WriteLine("Available Options:");
                Console.WriteLine($"{MAKE_RESERVATION_KEY.ToString().Substring(1)}) Make Reservation.");
                Console.WriteLine($"{QUIT_KEY}) Return to prior menu.");

                ConsoleKey userSelection = Console.ReadKey(true).Key;

                Console.WriteLine();

                if (userSelection == MAKE_RESERVATION_KEY && sites.Count > 0)
                {
                    int siteSelection = ParseUserInputRequired("Which site should be reserved (enter 0 to cancel)? ");

                    if (siteSelection != 0 && sites.ContainsKey(siteSelection))
                    {
                        string reservationName = AskUserForInputRequired("What name should the reservation be made under? ");

                        try
                        {
                            int reservationId = Npsd.MakeReservation(Mgr.User.Id, sites[siteSelection].SiteId, reservationName, arrivalDate, departureDate);

                            PrintMessageToScreen($"The reservation has been made and the confirmation id is {reservationId}");
                        }
                        catch
                        {
                            PrintMessageToScreen("Failed in making a reservation, please try again.");
                        }
                    }
                    else if (siteSelection != 0 && !sites.ContainsKey(siteSelection))
                    {
                        PrintMessageToScreen("Invalid site selection, please try again...");
                    }
                }
                else if (userSelection == QUIT_KEY)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Takes inputs from the user to display a menu for available reservations parkwide to select from.
        /// </summary>
        /// <param name="park"></param>
        /// <param name="occupancy"></param>
        /// <param name="accessible"></param>
        /// <param name="rvLength"></param>
        /// <param name="utility"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="departureDate"></param>
        private void SearchParkWideForAvailableReservationsMenu(Park park, int occupancy, bool accessible, int rvLength,
                                                            bool utility, DateTime arrivalDate, DateTime departureDate)
        {
            const ConsoleKey MAKE_RESERVATION_KEY = ConsoleKey.D1;
            const ConsoleKey QUIT_KEY = ConsoleKey.Q;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                var sites = Npsd.GetAvailableSitesParkWide(park, occupancy, accessible, rvLength, utility, arrivalDate, departureDate);

                Console.WriteLine("Campground".PadRight(20) + "Site No.".PadRight(20) + "Max Occup.".PadRight(20) + "Accessible?".PadRight(20) + "Max RV Length".PadRight(20)
                                    + "Utility".PadRight(20) + "Cost");

                foreach (var site in sites)
                {
                    Campground camp = Npsd.GetCampgroundForSite(site.Value.SiteId);

                    Console.WriteLine($"{camp.Name.PadRight(20)}{site.Value.SiteNumber.ToString().PadRight(20)}{site.Value.MaxOccupancy.ToString().PadRight(20)}" +
                                        $"{site.Value.Accessible.ToString().PadRight(20)} {site.Value.MaxRvLength.ToString().PadRight(20)}" +
                                        $"{site.Value.Utilities.ToString().PadRight(20)}{camp.CalculateFees(arrivalDate, departureDate):c}");
                }

                if (sites.Count == 0)
                {
                    Console.WriteLine("Sorry, there aren't any available sites with your requirements.");
                }

                Console.WriteLine();
                Console.WriteLine("Available Options:");
                Console.WriteLine($"{MAKE_RESERVATION_KEY.ToString().Substring(1)}) Make Reservation.");
                Console.WriteLine($"{QUIT_KEY}) Return to prior menu.");

                ConsoleKey userSelection = Console.ReadKey(true).Key;

                Console.WriteLine();

                if (userSelection == MAKE_RESERVATION_KEY && sites.Count > 0)
                {
                    int siteSelection = ParseUserInputRequired("Which site should be reserved (enter 0 to cancel)? ");

                    if (siteSelection != 0 && sites.ContainsKey(siteSelection))
                    {
                        string reservationName = AskUserForInputRequired("What name should the reservation be made under? ");

                        try
                        {
                            int reservationId = Npsd.MakeReservation(Mgr.User.Id, sites[siteSelection].SiteId, reservationName, arrivalDate, departureDate);

                            PrintMessageToScreen($"The reservation has been made and the confirmation id is {reservationId}");
                        }
                        catch
                        {
                            PrintMessageToScreen("Failed in making a reservation, please try again.");
                        }
                    }
                    else if (siteSelection != 0 && !sites.ContainsKey(siteSelection))
                    {
                        PrintMessageToScreen("Invalid site selection, please try again...");
                    }
                }
                else if (userSelection == QUIT_KEY)
                {
                    exit = true;
                }
            }
        }

        #endregion

        #region Login Menus

        /// <summary>
        /// Initial menu for user login/registration.
        /// </summary>
        private void LoginScreen()
        {
            const ConsoleKey LOGIN_KEY = ConsoleKey.D1;
            const ConsoleKey REGISTRATION_KEY = ConsoleKey.D2;
            const ConsoleKey QUIT_KEY = ConsoleKey.Q;

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintColoredMessage("Welcome to the National Park Campsite Reservation Service!",
                    ConsoleColor.Yellow);
                Console.WriteLine();
                Console.WriteLine("Please select from the following menu...");
                Console.WriteLine();

                Console.WriteLine($"{LOGIN_KEY.ToString().Substring(1)}) Login");
                Console.WriteLine($"{REGISTRATION_KEY.ToString().Substring(1)}) Register user");
                Console.WriteLine($"{QUIT_KEY}) Quit");


                var keySelection = Console.ReadKey(true).Key;
                if (keySelection == LOGIN_KEY)
                {
                    LoginMenu();
                }
                else if (keySelection == REGISTRATION_KEY)
                {
                    RegisterUserMenu();
                }
                else if (keySelection == QUIT_KEY)
                {
                    exit = true;
                }
                else
                {
                    PrintMessageToScreen("Invalid selection, please try again...");
                }

                if (Mgr.IsAuthenticated)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Menu for user to log-in.
        /// </summary>
        private void LoginMenu()
        {
            Console.Clear();

            try
            {
                string username = AskUserForInputRequired("Please enter your username: ");
                string password = HideUserPassword("Please enter your password: ");

                Mgr.LoginUser(username, password);
                Console.WriteLine("Login Successful!");
            }
            catch (UserDoesNotExistException)
            {
                Console.WriteLine("User does not exist.");
            }
            catch (PasswordMatchException)
            {
                Console.WriteLine("Password does not match.");
            }

            Console.ReadKey(true);
        }

        /// <summary>
        /// Menu for user registration.
        /// </summary>
        private void RegisterUserMenu()
        {
            Console.Clear();

            User user = new User
            {
                FirstName = AskUserForInputRequired("Please enter your first name: "),
                LastName = AskUserForInputRequired("Please enter your last name: "),
                Email = AskUserForInputRequired("Please enter your email: "),
                Username = AskUserForInputRequired("Please enter your username: "),
                Password = HideUserPassword("Please enter your password: "),
                ConfirmPassword = HideUserPassword("Please confirm your password: ")
            };

            Mgr.RegisterUser(user);

            PrintMessageToScreen("Registration Successful!");
        }

        #endregion

        #region Console Methods

        /// <summary>
        /// Asks the user for their input and returns it as a string.
        /// The input is required, so user must input something to continue. 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string AskUserForInputRequired(string message)
        {
            string userInput = "";

            while(userInput == "")
            {
                Console.Write(message);
                userInput = Console.ReadLine();
                if (userInput == "")
                {
                    Console.Write("Field Required, please try again.");
                    Console.ReadKey(true);
                    
                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }

            return userInput;
        }

        /// <summary>
        /// Hides user input when typing in password.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string HideUserPassword(string message)
        {
            Console.Write(message);

            string password = "";
            ConsoleKeyInfo charEntered = Console.ReadKey(true);
            while (charEntered.Key != ConsoleKey.Enter)
            {
                if (charEntered.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += charEntered.KeyChar;
                }
                else if (charEntered.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // Remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // Get the location of the cursor
                        int position = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(position - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(position - 1, Console.CursorTop);
                    }
                }
                    charEntered = Console.ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        /// <summary>
        /// Parses user input into an input into an int. Required, so user must enter valid input to continue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int ParseUserInputRequired(string message)
        {
            int userInput = 0;
            bool parsed = false;

            while (!parsed)
            {
                if (int.TryParse(AskUserForInputRequired(message), out int number))
                {
                    userInput = number;
                    parsed = true;
                }
                else
                {
                    Console.Write("Invalid selection. Please enter as numeric value.");
                    Console.ReadKey(true);

                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }

            return userInput;
        }

        /// <summary>
        /// Parses user input into a bool. Required, so user must enter valid input to continue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ParseUserBoolRequired(string message)
        {
            bool inputBool = false;
            bool parsed = false;

            while (!parsed)
            {
                if (bool.TryParse(AskUserForInputRequired(message), out inputBool))
                {
                    parsed = true;
                }
                else
                {
                    Console.Write("Invalid selection. Please enter as numeric value.");
                    Console.ReadKey(true);

                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }

            return inputBool;
        }

        /// <summary>
        /// Parses user input into an input into a date. Required, so user must enter valid input to continue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DateTime ParseUserDateRequired(string message)
        {
            DateTime userInput = DateTime.Now;
            bool parsed = false;

            while (!parsed)
            {
                if (DateTime.TryParse(AskUserForInputRequired(message), out DateTime date))
                {
                    userInput = date;
                    parsed = true;
                }
                else
                {
                    Console.Write("Invalid selection. Please enter date in MM/DD/YYYY format.");
                    Console.ReadKey(true);

                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);

                }
            }

            return userInput;
        }

        /// <summary>
        /// Prints a message to the console, in a chosen font color.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="forColor"></param>
        public static void PrintColoredMessage(string message, ConsoleColor forColor)
        {
            ConsoleColor currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = forColor;
            Console.WriteLine(message);
            Console.ForegroundColor = currentForeground;
        }

        /// <summary>
        /// Prints a message to the console, in a chosen font/background color.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="forColor"></param>
        /// <param name="bacColor"></param>
        public static void PrintColoredMessage(string message,
                           ConsoleColor forColor,ConsoleColor bacColor)
        {
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = forColor;
            Console.BackgroundColor = bacColor;
            Console.WriteLine(message);
            Console.BackgroundColor = currentBackground;
            Console.ForegroundColor = currentForeground;
        }

        /// <summary>
        /// Clears the current console line.
        /// </summary>
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>
        /// Adds a new line, prints a message to the screen and wait for user.
        /// </summary>
        /// <param name="message"></param>
        public static void PrintMessageToScreen(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadKey(true);
        }

        #endregion
    }
}
