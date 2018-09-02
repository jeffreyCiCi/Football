using System;
using System.Collections.Generic;
using System.Text;
using FootballCore.Models;

namespace FootballCore.Interfaces
{
    public interface IFileParser
    {
        bool IsFilePathValid { get; }
        bool IsColValid { get; }
        List<ResultData> ResultList { get; }

        bool LoadFile(string filePathName);
    }
}
