using GymSystem.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        // HireDate => CreatedAt

        public Specilaities Specilaities { get; set; }

        #region Relations
        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion
    }
}
