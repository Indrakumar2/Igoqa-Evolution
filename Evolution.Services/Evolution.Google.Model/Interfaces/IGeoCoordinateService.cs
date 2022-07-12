using Evolution.Google.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Google.Model.Interfaces
{
    public interface IGeoCoordinateService
    {
        LocationSearchResult GetLocationCoordinate(LocationSearchInfo searchModel);
        LocationSearchResult GetLocationCoordinate(string address);
        
    }
}
