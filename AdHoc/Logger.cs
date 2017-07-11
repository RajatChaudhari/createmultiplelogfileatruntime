using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository;
using log4net.Core;
using log4net.Config;

namespace Entities
{
    public class Logger
    {
        ILog logger;
        static List<string> filenames= new List<string>();
        public Logger()
        {
            log4net.Config.BasicConfigurator.Configure();
            logger = LogManager.GetLogger("ErrorLog");
        }

        public Logger(string fileName)
        {
            ILoggerRepository repository;
            if ( filenames.Contains(fileName))
            {
                repository = LogManager.GetRepository("Repository_" + fileName);
            }
            else
            {
                filenames.Add(fileName);
                repository = LogManager.CreateRepository("Repository_" + fileName);
            }

            log4net.Appender.IAppender appender = CreateFileAppender("appender_" + fileName,fileName + "_log");

            BasicConfigurator.Configure(repository, appender);
            logger = LogManager.GetLogger(repository.Name, "logger_" + fileName);
            AddAppender(fileName, appender);
            
            
            //SetLevel(fileName, fileName);

        }

        public void LogError(string error)
        {
            logger.Error(error);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void SetLevel(string loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            log4net.Repository.Hierarchy.Logger l = (log4net.Repository.Hierarchy.Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }
        public void AddAppender(string Name, log4net.Appender.IAppender appender)
        {

            log4net.ILog log = log4net.LogManager.GetLogger("logger_" + Name);
            log4net.Repository.Hierarchy.Logger l = (log4net.Repository.Hierarchy.Logger)log.Logger;
            l.Repository.Configured = true;
            l.AddAppender(appender);
        }

        public log4net.Appender.IAppender CreateFileAppender(string name, string fileName)
        {
            log4net.Appender.FileAppender appender = new
          log4net.Appender.FileAppender();
            appender.Name = name;

            appender.File = "D:\\CapitaCodeBase\\Devon\\EYPP\\CensusImport\\logs\\" + fileName + ".txt";
            appender.AppendToFile = true;

            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
            layout.ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();
            return appender;
        }


    }
}
