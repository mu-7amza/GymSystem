using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = default!;

        #region Relations
        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion


    }
}
