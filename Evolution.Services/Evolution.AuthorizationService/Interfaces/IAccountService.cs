using Evolution.AuthorizationService.Models;
using Evolution.Common.Models.Responses;

namespace Evolution.AuthorizationService.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Authenticate User and return tokens
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="requestedIp"></param>
        /// <param name="clientCode"></param>
        /// <param name="audienceCode"></param>
        /// <returns></returns>
        Response SignIn(string username, string password, string requestedIp, string clientCode, string audienceCode);

        Response SignInByQuestionAnswer(UserQuestionAnswer userQuestionAnswer, string requestedIp, string clientCode, string audienceCode);

        /// <summary>
        /// ill renew the access token if refresh token is valid.
        /// </summary>
        /// <param name="token">refresh token</param>
        /// <param name="userName">User name which has generated refresh token other was it will token renw will failed.</param>
        /// <param name="requestedIp">Ip from which older token was generated</param>
        /// <returns></returns>
        Response RefreshAccessToken(string token, string userName, string requestedIp, string clientCode, string audienceCode);

        /// <summary>
        /// Remove Token from system
        /// </summary>
        /// <param name="token">refresh token</param>
        /// <param name="userName">User name which has generated refresh token other was it will token renw will failed.</param>
        /// <param name="requestedIp">Ip from which older token was generated</param>
        /// <returns></returns>
        Response RevokeRefreshToken(string token, string userName, string requestedIp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="requestedIp"></param>
        /// <returns></returns>
        Response CancelAccessToken(string token, string userName, string requestedIp);

        Response GetUserQuestion(string applicatName, string userLogonName, string userEmail);

        Response ValidateUserAnswer(UserQuestionAnswer userQuestionAnswer);

        Response SendPassword(UserQuestionAnswer userQuestionAnswer);

        Response resetPassword(User user);
    }
}
