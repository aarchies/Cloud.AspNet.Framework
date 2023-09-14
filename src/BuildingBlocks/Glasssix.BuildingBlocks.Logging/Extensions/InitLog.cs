using Serilog;
using System;

namespace Glasssix.BuildingBlocks.Logging.Extensions
{
    public static class InitLog
    {
        public static void InitializationLog()
        {
            Log.Debug($"Initialization Load Dotnet Framework Options ...");

            string initLog = $"  ___ _               _         _            _  _     _     ___                                  _   \r\n / __| |__ _ ________(_)_ __   /_\\   ____ __| \\| |___| |_  | __| _ __ _ _ __  _____ __ _____ _ _| |__\r\n| (_ | / _` (_-<_-<_-< \\ \\ /  / _ \\ (_-< '_ \\ .` / -_)  _| | _| '_/ _` | '  \\/ -_) V  V / _ \\ '_| / /\r\n \\___|_\\__,_/__/__/__/_/_\\_\\ /_/ \\_\\/__/ .__/_|\\_\\___|\\__| |_||_| \\__,_|_|_|_\\___|\\_/\\_/\\___/_| |_\\_\\\r\n                                       |_|                                                           \r\n";
            // Log.Debug(initLog);
            Console.WriteLine(initLog);
        }
    }
}