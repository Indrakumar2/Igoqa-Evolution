using Evolution.Common.Helpers;
using System;

namespace Evolution.AuthorizationService.Models.Tokens
{
    public class TokenRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        private string _userTimeZoneOffset;
        public string UserTimeZoneOffset
        {
            get
            {
                return string.IsNullOrEmpty(_userTimeZoneOffset) ?
                        DateTimeHelper.ValidateTimeZoneAndReturnDefault("").GetUtcOffset(DateTime.Now).ToString() :
                        _userTimeZoneOffset;
            }
            set
            {
                _userTimeZoneOffset = value;
            }
        }
    }
}
