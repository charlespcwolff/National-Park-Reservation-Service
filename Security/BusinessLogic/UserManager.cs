using Security.DAO;
using Security.Exceptions;
using Security.Models;
using Security.Models.Database;
using System;

namespace Security.BusinessLogic
{
    /// <summary>
    /// Used to manage user registration, login, and authorization
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Holds the user security dao interface
        /// </summary>
        private IUserSecurityDAO _db = null;

        private const string LoginFailure = "Either the username or the password is invalid.";

        /// <summary>
        /// Used for getting user authorization information
        /// </summary>
        public Authorization Permission { get; private set; }

        /// <summary>
        /// The user to manage permissions for
        /// </summary>
        public UserItem User
        {
            get
            {
                return Permission.User;
            }

            set
            {
                Permission.User = value;
            }
        }

        /// <summary>
        /// Constructor that requires the security DAO
        /// </summary>
        /// <param name="db">Interface for the user security dao</param>
        public UserManager(IUserSecurityDAO db)
        {
            _db = db;
            Permission = new Authorization(null);
        }

        /// <summary>
        /// Returns true if the program has an authenticated user
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        /// <summary>
        /// Adds a new user to the program system
        /// </summary>
        /// <param name="userModel">Model that contains all the user information</param>
        public void RegisterUser(User userModel)
        {
            UserItem userItem = null;
            try
            {
                userItem = _db.GetUserItem(userModel.Username);
            }
            catch (Exception)
            {
            }

            if (userItem != null)
            {
                throw new UserExistsException("The username is already taken.");
            }

            if (userModel.Password != userModel.ConfirmPassword)
            {
                throw new PasswordMatchException("The password and confirm password do not match.");
            }

            Authentication auth = new Authentication(userModel.Password);
            UserItem newUser = new UserItem()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username,
                Salt = auth.Salt,
                Hash = auth.Hash,
                RoleId = (int)Authorization.eRole.StandardUser
            };

            _db.AddUserItem(newUser);
            LoginUser(newUser.Username, userModel.Password);
        }

        /// <summary>
        /// Logs a user into the program and throws exceptions on any failures
        /// </summary>
        /// <param name="username">The username of the user to authenicate</param>
        /// <param name="password">The password of the user to authenicate</param>
        public void LoginUser(string username, string password)
        {
            UserItem user = null;

            try
            {
                user = _db.GetUserItem(username);
            }
            catch (Exception)
            {
                throw new UserDoesNotExistException(LoginFailure);
            }

            Authentication auth = new Authentication(password, user.Salt);
            if (!auth.Verify(user.Hash))
            {
                throw new PasswordMatchException(LoginFailure);
            }

            Permission = new Authorization(user);
        }

        /// <summary>
        /// Logs the current user out of the program
        /// </summary>
        public void LogoutUser()
        {
            Permission = new Authorization(null);
        }                
    }
}
