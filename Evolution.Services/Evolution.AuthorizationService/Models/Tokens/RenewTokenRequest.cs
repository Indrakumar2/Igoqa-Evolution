using Evolution.Common.Helpers;
using System;

namespace Evolution.AuthorizationService.Models.Tokens
{
    public class RenewTokenRequest
    {
        public string Token { get; set; }

        public string UserName { get; set; }

        private string _userTimeZoneOffset = null;
        public string UserTimeZoneOffset
        {
            get
            {
                return string.IsNullOrEmpty(_userTimeZoneOffset) ?
                        DateTimeHelper.ValidateTimeZoneAndReturnDefault(string.Empty).GetUtcOffset(DateTime.Now).ToString() :
                        _userTimeZoneOffset;
            }
            set
            {
                _userTimeZoneOffset = value;
            }
        }
    }
}
