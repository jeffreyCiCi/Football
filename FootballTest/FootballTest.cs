using System;
using Xunit;
using FootballCore.Models;
using DomainServices.BusinessLayer;
using Infrastructure.Repositories;
using FootballCore.Interfaces;
using Football;

namespace FootballTest
{
    public class FootballTest
    {
        [Fact]
        public void Test1_Success()
        {
            FootballApp app = new FootballApp();
            Assert.True(app.ExtractInfoFromFile());
        }

        [Fact]
        public void Test2_InvalidFilePath()
        {
            FootballApp app = new FootballApp();
            string filePath = @"C:\football.csv";
            Assert.False(app.ExtractInfoFromFile(filePath));
        }

    }
}
