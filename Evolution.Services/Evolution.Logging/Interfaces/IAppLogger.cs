namespace Evolution.Logging.Interfaces
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T">App Controller/Service Class</typeparam>
    public interface IAppLogger<T>
    {
        void LogInformation(string infoCode, string message, params object[] args);
        void LogWarning(string warningCode, string message, params object[] args);
        void LogError(string errorCode, string message, params object[] args);
        void LogCritical(string errorCode, string message, params object[] args);
        void LogTrace(string errorCode, string message, params object[] args);
        void LogDebug(string errorCode, string message, params object[] args);
    }
}
