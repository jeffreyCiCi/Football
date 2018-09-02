using System;
using System.Collections.Generic;
using FootballCore.Models;
using FootballCore.Interfaces;

namespace DomainServices.BusinessLayer
{
    public class FootballService
    {
        private IFileParser _fileParser;

        public bool IsFilePathValid { get { return _fileParser.IsFilePathValid; } }
        public bool IsFileColValid { get { return _fileParser.IsColValid; } }

        public FootballService(IFileParser parser)
        {
            _fileParser = parser;
        }

        public bool LoadFile(string FilePathName)
        {
            return _fileParser.LoadFile(FilePathName);
        }

        public string GetTeamWithMinGap()
        {
            List<ResultData> list = _fileParser.ResultList;

            if(list != null && list.Count>0)
            {
                ResultData minGapTeam = new ResultData() { F=0, A=1000};
                foreach(var t in list)
                {
                    if(Math.Abs(t.F-t.A) < Math.Abs( minGapTeam.F-minGapTeam.A))
                    {
                        minGapTeam = t;
                    }
                }

                return minGapTeam.TeamName;
            }
            else
            {
                return "";
            }
        }
    }
}
