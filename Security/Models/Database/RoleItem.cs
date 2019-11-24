using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models.Database
{
    /// <summary>
    /// Database model for the role table
    /// </summary>
    public class RoleItem : BaseItem
    {
        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Makes a deep copy of this class
        /// </summary>
        /// <returns>Deep copy of the role item</returns>
        public RoleItem Clone()
        {
            RoleItem item = new RoleItem();
            item.Id = this.Id;
            item.Name = this.Name;
            return item;
        }
    }
}


