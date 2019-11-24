using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models.Database
{
    /// <summary>
    /// Database model base class which covers the primary key
    /// </summary>
    public class BaseItem
    {
        /// <summary>
        /// Used as the default for the primary key to denote this object is not from the database
        /// </summary>
        public const int InvalidId = -1;

        /// <summary>
        /// Primary key database tables
        /// </summary>
        public int Id { get; set; } = InvalidId;
    }
}
