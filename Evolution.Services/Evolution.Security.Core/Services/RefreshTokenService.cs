using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using System;

namespace Evolution.Security.Core.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IAppLogger<RefreshTokenService> _logger = null; 
        private readonly IRefreshTokenRepository _repository = null; 

        #region Constructor
        public RefreshTokenService(IRefreshTokenRepository repository, IAppLogger<RefreshTokenService> logger)
        { 
            this._repository = repository;
            this._logger = logger; 
        }
        #endregion

        public bool isValidToken(string token, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return false; 
                return _repository.Exists(x => x.Username == userName && x.AccessToken == token).Result; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { token , userName });
                return false;
            }
        }
    }
}
