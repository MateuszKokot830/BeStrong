namespace Application.Interfaces
{
    public interface INLoggerService
    {
        void Info(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception ex);
    }
}