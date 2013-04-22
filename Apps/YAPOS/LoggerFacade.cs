using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace YAPOS
{
    public class LoggerFacade : ILoggerFacade
    {
        
        public void Log(string message, Category category, Priority priority)
        {
            Logger.Write(message, category.ToString(), (int)priority);
        }

    }
}
