using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.Logger;

public class FileLogger:LoggerServiceBase
{
    private readonly IConfiguration _configuration;

    public FileLogger(IConfiguration configuration)
    {
        _configuration = configuration;
        FileLogConfiguration logConfig =
            configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>()
            ?? throw new Exception(SerilogMessages.NullOptionsMessage);

        string logFilePath = string.Format(format:"{0}{1}",arg0:Directory.GetCurrentDirectory()+logConfig.FolderPath,arg1:".txt");
        Logger  = new LoggerConfiguration().WriteTo.File(
            logFilePath
            ,rollingInterval:RollingInterval.Day //Hergün yeni Dosya oluştur.
            ,retainedFileCountLimit:null //Eski Dosyaları Silmek için Limit var mı Hayır;
            ,fileSizeLimitBytes:5000000 // dosya Mac Boyut
            ,outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Excepiton}"
            ).CreateLogger();
    }
}
