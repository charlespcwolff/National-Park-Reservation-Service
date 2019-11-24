using Security.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.BusinessLogic
{
    /// <summary>
    /// Handles the user authorization
    /// </summary>
    public class Authorization
    {
        /// <summary>
        /// The available user roles
        /// </summary>
        public enum eRole
        {
            Unknown = 0,
            Administrator = 1,
            StandardUser = 2
        }

        /// <summary>
        /// The user to manage permissions for
        /// </summary>
        public UserItem User { get; set; }

        public Authorization(UserItem user)
        {
            User = user;
        }

        /// <summary>
        /// The name of the user's role
        /// </summary>
        private eRole RoleName
        {
            get
            {
                return User != null ? (eRole)User.RoleId : eRole.Unknown;
            }
        }

        /// <summary>
        /// Specifies if the user has administrator permissions
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return RoleName == eRole.Administrator;
            }
        }

        /// <summary>
        /// Specifies if the user has StandardUser permissions
        /// </summary>
        public bool IsStandardUser
        {
            get
            {
                return RoleName == eRole.StandardUser;
            }
        }

        /// <summary>
        /// Specifies if the user has unknown permissions
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return RoleName == eRole.Unknown;
            }
        }
    }
}
