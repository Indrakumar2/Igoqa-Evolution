using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Google.Model.Enums
{
    public enum GoogleSearchStatus
    {
        Ok = -1,
        Unknown = 0,
        InvalidRequest = 1,
        ZeroResults = 2,
        OverQueryLimit = 3,
        RequestDenied = 4,
        NotFound = 5,
        MaxWayPointsExceeded = 6,
    }
}
