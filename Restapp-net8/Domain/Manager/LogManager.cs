using NLog;
using NLog.Config;
using NLog.Targets;
using Serilog;
using static Domain.Manager.Enums;

namespace Domain.Manager
{
    public class LogManager
    {
        private Serilog.Core.Logger SeriLogger { get; set; }
        private NLog.Logger NLogger { get; set; }
        private enumLogType EnumLogType { get; set; }
        private enumLogOutputType EnumLogOutputType { get; set; }
        private enumLogLevel EnumLogLevel { get; set; }
        private string FullPathFileName { get; set; }

        private LogManager(enumLogType enumLogType, enumLogOutputType enumLogOutputType, enumLogLevel enumLogLevel, string fullPathFileName = null)
        {
            this.EnumLogType = enumLogType;
            this.EnumLogOutputType = enumLogOutputType;
            this.EnumLogLevel = enumLogLevel;
            this.FullPathFileName = fullPathFileName;

            switch (enumLogType)
            {
                case enumLogType.SERILOG:
                    if (enumLogOutputType == enumLogOutputType.CONSOLE)
                    {
                        this.SeriLogger = GetSerilogConsoleConfiguration();
                    }
                    else
                    {
                        this.SeriLogger = GetSerilogFileConfiguration(fullPathFileName);
                    }

                    break;
                case enumLogType.NLOG:
                    if (enumLogOutputType == enumLogOutputType.CONSOLE)
                    {
                        this.NLogger = GetNLogConsoleConfiguration();
                    }
                    else
                    {
                        this.NLogger = GetNLogFileConfiguration(fullPathFileName);
                    }
                    break;
            }
        }

        public static LogManager GetInstance(enumLogType enumLogType, enumLogOutputType enumLogOutputType, enumLogLevel enumLogLevel, string fullPathFileName = null)
        {
            return new LogManager(enumLogType, enumLogOutputType, enumLogLevel, fullPathFileName);
        }

        public Serilog.Core.Logger GetSerilogFileConfiguration(string fullPathFileName)
        {
            Serilog.Core.Logger logger = null;
            try
            {
                logger = GetLogConfigurationLevel()
                    .WriteTo.File(fullPathFileName,
                    retainedFileCountLimit: null,
                    fileSizeLimitBytes: null)
                    .CreateLogger();
            }
            catch (Exception e)
            {
                throw e;
            }
            return logger;
        }

        public Serilog.Core.Logger GetSerilogConsoleConfiguration()
        {
            Serilog.Core.Logger logger = null;
            try
            {
               logger = GetLogConfigurationLevel().WriteTo
                    .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
            }
            catch (Exception e)
            {
                throw e;
            }
            return logger;
        }

        private LoggerConfiguration GetLogConfigurationLevel()
        {
            LoggerConfiguration configuration = new LoggerConfiguration();
            switch (EnumLogLevel)
            {
                case enumLogLevel.DEBUG:
                    configuration.MinimumLevel.Debug();
                    break;
                case enumLogLevel.INFO:
                    configuration.MinimumLevel.Information();
                    break;
                case enumLogLevel.WARNING:
                    configuration.MinimumLevel.Warning();
                    break;
                case enumLogLevel.ERROR:
                    configuration.MinimumLevel.Error();
                    break;
                default:
                    break;
            }
            return configuration;
        }

        public NLog.Logger GetNLogFileConfiguration(string fullPathFileName)
        {
            NLog.Logger logger = null;
            try
            {
                LoggingConfiguration config = new LoggingConfiguration();
                FileTarget fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);
                // Step 3. Set target properties
                fileTarget.FileName = fullPathFileName;
                fileTarget.Layout = "${date:format=yyyy MM dd HHmmss} ${uppercase:${level}} ${message}";
                fileTarget.LineEnding = LineEndingMode.Default;
                fileTarget.AutoFlush = true;
                fileTarget.KeepFileOpen = false;
                fileTarget.ConcurrentWrites = true;
                fileTarget.ArchiveFileName = "${date:format=yyyyMMddHH}_{#####}.log";
                fileTarget.ArchiveEvery = FileArchivePeriod.Day;
                fileTarget.ArchiveNumbering = ArchiveNumberingMode.Sequence;
                fileTarget.MaxArchiveFiles = 720;

                // Step 4. Define rules
                LoggingRule rule = GetLoggingRule(fileTarget);
                config.LoggingRules.Add(rule);
                // Step 5. Activate the configuration
                NLog.LogManager.Configuration = config;
                logger = NLog.LogManager.GetCurrentClassLogger();
            }
            catch (Exception e)
            {
                throw e;
            }
            return logger;
        }

        public NLog.Logger GetNLogConsoleConfiguration()
        {
            NLog.Logger logger = null;
            try
            {
                LoggingConfiguration config = new LoggingConfiguration();
                ConsoleTarget consoleTarget = new ConsoleTarget();
                config.AddTarget("file", consoleTarget);
                // Step 3. Set target properties
                consoleTarget.Layout = "${date:format=yyyy MM dd HHmmss} ${uppercase:${level}} ${message}";
                consoleTarget.AutoFlush = true;
                // Step 4. Define rules
                LoggingRule rule = GetLoggingRule(consoleTarget);
                config.LoggingRules.Add(rule);
                // Step 5. Activate the configuration
                NLog.LogManager.Configuration = config;
                logger = NLog.LogManager.GetCurrentClassLogger();
            }
            catch (Exception e)
            {
                throw e;
            }
            return logger;
        }

        private LoggingRule GetLoggingRule(Target target)
        {
            LoggingRule rule = null;
            switch (EnumLogLevel)
            {
                case enumLogLevel.DEBUG:
                    rule = new LoggingRule("*", LogLevel.Debug, target);
                    break;
                case enumLogLevel.INFO:
                    rule = new LoggingRule("*", LogLevel.Info, target);
                    break;
                case enumLogLevel.WARNING:
                    rule = new LoggingRule("*", LogLevel.Warn, target);
                    break;
                case enumLogLevel.ERROR:
                    rule = new LoggingRule("*", LogLevel.Error, target);
                    break;
                default:
                    break;
            }
            return rule;
        }

        public void WriteDebug(string message)
        {
            switch (this.EnumLogType)
            {
                case enumLogType.NLOG:
                    NLogger.Debug(message);
                    break;
                case enumLogType.SERILOG:
                    SeriLogger.Debug(message);
                    break;
            }
        }

        public void WriteInformation(string message)
        {
            switch (this.EnumLogType)
            {
                case enumLogType.NLOG:
                    NLogger.Info(message);
                    break;
                case enumLogType.SERILOG:
                    SeriLogger.Information(message);
                    break;
            }
        }

        public void WriteWarning(string message)
        {
            switch (this.EnumLogType)
            {
                case enumLogType.NLOG:
                    NLogger.Warn(message);
                    break;
                case enumLogType.SERILOG:
                    SeriLogger.Warning(message);
                    break;
            }
        }

        public void WriteError(string message)
        {
            switch (this.EnumLogType)
            {
                case enumLogType.NLOG:
                    NLogger.Error(message);
                    break;
                case enumLogType.SERILOG:
                    SeriLogger.Error(message);
                    break;
            }
        }

    }
}
