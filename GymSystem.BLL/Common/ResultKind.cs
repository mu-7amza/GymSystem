using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public enum ResultKind
    {
        OK,
        NoFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
