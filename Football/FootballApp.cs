using System;
using FootballCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using DomainServices.BusinessLayer;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Football
{
    public class FootballApp
    {
        public ServiceProvider ServiceProvider { get; private set; }

        private string _defaultFilePath;

        public FootballApp()
        {
            try
            {
                // Load config files
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                _defaultFilePath = configuration["FootballFile:DefaultFilePath"];

                // Dependency Injection
                ServiceProvider = new ServiceCollection()
                    .AddTransient<IFileParser, CsvFileParser>()
                    .BuildServiceProvider();      
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public bool ExtractInfoFromFile(string filePath = "")
        {
            if(string.IsNullOrEmpty(filePath))
            {
                filePath = _defaultFilePath;
            }

            try
            {
                // Get registered service
                FootballService footballService = new FootballService(ServiceProvider.GetService<IFileParser>());

                // Try to read & parse file
                bool ret = footballService.LoadFile(filePath);

                // Validate file, and produce output
                if (ret && footballService.IsFilePathValid)
                {
                    if (footballService.IsFileColValid)
                    {
                        Console.WriteLine("The file columns are valid.");
                        Console.WriteLine("The team with the smallest difference in ‘for’ and ‘against’ goals is: " 
                            + footballService.GetTeamWithMinGap());

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("The file columns are not valid.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Cannot open file.");
                    return false;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
