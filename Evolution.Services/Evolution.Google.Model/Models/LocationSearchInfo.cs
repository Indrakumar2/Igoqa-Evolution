using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Google.Model.Models
{
    public class LocationSearchInfo
    {
        public LocationSearchInfo(){ }

        public LocationSearchInfo(string country, string state, string city, string zip)
        {
            this.Country = country;
            this.State = state;
            this.City = city;
            this.Zip = zip;
        }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string GoogleAddress { get; set; }
        
        public string Address { get; set; }

        public string Zip { get; set; }

        public bool IsPartiallyMatched { get; set; }
    }
}
