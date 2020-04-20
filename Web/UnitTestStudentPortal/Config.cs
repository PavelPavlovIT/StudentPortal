using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTestStudentPortal
{
    public static class Config
    {
        public static IConfiguration configuration;
        static Config()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
        }

        public static string Get(string name)
        {
            string appSettings = configuration[name];
            return appSettings;
        }
    }
}
