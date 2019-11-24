using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models.Database
{
    /// <summary>
    /// Database model for the user table
    /// </summary>
    public class UserItem : BaseItem
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's username, must be unique
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's email address, must be uniqe
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hash value of the user's password
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Salt used to create the hash
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Foreign key to the role table
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Makes a deep copy of this class
        /// </summary>
        /// <returns>Deep copy of the user item</returns>
        public UserItem Clone()
        {
            UserItem item = new UserItem();
            item.Id = this.Id;
            item.FirstName = this.FirstName;
            item.LastName = this.LastName;
            item.Username = this.Username;
            item.Email = this.Email;
            item.Hash = this.Hash;
            item.Salt = this.Salt;
            item.RoleId = this.RoleId;
            return item;
        }
    }
}
