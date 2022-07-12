namespace Evolution.Logging.Interfaces
{
    public interface IDataLogger<T>
    {
        void LogToDb(string errorCode, string message, string objectType, object args);
    }
}
