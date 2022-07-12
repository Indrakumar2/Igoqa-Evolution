using Evolution.AuthorizationService.Interfaces;
using Evolution.AuthorizationService.Models;
using Evolution.AuthorizationService.Models.Tokens;
using Evolution.Common.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.AuthorizationService.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IHttpContextAccessor _accessor;
        private readonly IAccountService _accountService;
        private readonly string _requestIp = string.Empty;
        private readonly string _clientCode = string.Empty;
        private readonly string _audienceCode = string.Empty;

        public AccountController(IAccountService accountService, IHttpContextAccessor accessor)
        {
            _accountService = accountService;
            _accessor = accessor;
            _requestIp = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            _clientCode = _accessor.HttpContext.Request.Headers["client_code"].ToString();
            _audienceCode = _accessor.HttpContext.Request.Headers["client_aud_code"].ToString();
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public Response SignIn([FromBody] TokenRequest request)
            => _accountService.SignIn(request?.Username?.Trim(), request?.Password?.Trim(), _requestIp, _clientCode, _audienceCode);

        [HttpPost("authenticate/question")]
        [AllowAnonymous]
        public Response SignInByQuestionAnswer([FromBody] UserQuestionAnswer userQuestionAnswer)
           => _accountService.SignInByQuestionAnswer(userQuestionAnswer, _requestIp, _clientCode, _audienceCode);

        [HttpPost("token/renew")]
        public Response RefreshAccessToken([FromBody] RenewTokenRequest request)
            => _accountService.RefreshAccessToken(request?.Token, request?.UserName, _requestIp, _clientCode, _audienceCode);

        [HttpPost("token/refresh/revoke")]
        public Response RevokeRefreshToken([FromBody] RevokeToken request)
            => _accountService.RevokeRefreshToken(request?.Token, request?.UserName, _requestIp);

        [HttpPost("token/access/revoke")]
        public Response CancelAccessToken([FromBody] RevokeToken request)
            => _accountService.CancelAccessToken(request?.Token, request?.UserName, _requestIp);

        [HttpGet("security/question")]
        public Response UserSecurityQuestion([FromQuery] string applicatName, [FromQuery] string userLogonName, [FromQuery] string userEmail)
            => _accountService.GetUserQuestion(applicatName, userLogonName, userEmail);

        [HttpPost("security/answer/validate")]
        public Response ValidateUserAnswer([FromBody]UserQuestionAnswer userQuestionAnswer)
            => _accountService.ValidateUserAnswer(userQuestionAnswer);

        [HttpPost("security/user/sendpassword")]
        public Response SendUserPassword([FromBody]UserQuestionAnswer userQuestionAnswer)
           => _accountService.SendPassword(userQuestionAnswer);
        
        [HttpPost("authenticate/resetpassword")]
        [AllowAnonymous]
        public Response resetPassword([FromBody] User user)
           => _accountService.resetPassword(user);
    }
}
