using System;
using System.Collections.Generic;
using System.IO;
using FootballCore.Models;
using FootballCore.Interfaces;

namespace Infrastructure.Repositories
{
    public class CsvFileParser : IFileParser
    {
        public bool IsFilePathValid { get; private set; }
        public bool IsColValid { get; private set; }
        public List<ResultData> ResultList { get; private set; }

        private readonly List<string> headerList = new List<string> { "Team", "P", "W", "L", "D", "F", "-", "A", "Pts" };

        public CsvFileParser()
        {
            IsFilePathValid = false;
            IsColValid = false;
        }

        public bool LoadFile(string filePathName)
        {
            string line;
            bool bFirstLine = true;
            try
            {
                using (FileStream fileStream = new FileStream(filePathName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        List<string> columnData = new List<string>();   // used to temporarily store parsed data from each line
                        ResultList = new List<ResultData>();
                        // File can open means file name and path are valid.
                        IsFilePathValid = true;

                        int badValue = -1;          // assume all value should be above 0;


                        while ((line = reader.ReadLine()) != null)
                        {
                            if(line.Trim().Length > 0 )
                            {
                                // if this is the first line (header line)
                                if(bFirstLine)
                                {
                                    // validate header
                                    List<string> headersFromFile = new List<string>(line.Split(','));

                                    if(headersFromFile.Count != headerList.Count)
                                    {
                                        // if headers are not valid, then exit loop
                                        break;
                                    }

                                    // check if each header text is correct
                                    for(int i=0; i<headersFromFile.Count; i++)
                                    {
                                        if(headersFromFile[i] != headerList[i])
                                        {
                                            // if header not match, then exit loop
                                            break;
                                        }
                                    }

                                    // when it comes to here, it means headers are valid
                                    IsColValid = true;
                                    bFirstLine = false; 
                                }
                                // if this is a data line
                                else
                                {
                                    // process data
                                    columnData.AddRange(line.Split(','));

                                    // if number of columns is not right, then skip this line
                                    if(columnData.Count != headerList.Count)
                                    {
                                        continue;
                                    }

                                    // Else convert the data into object 
                                    int idx = 1;    // skip the first column: TeamName

                                    //  "Team", "P", "W", "L", "D", "F", "-", "A", "Pts" 
                                    if (!int.TryParse(columnData[idx++], out int _p))
                                        _p = badValue;
                                    if (!int.TryParse(columnData[idx++], out int _w))
                                        _w = badValue;
                                    if (!int.TryParse(columnData[idx++], out int _l))
                                        _l = badValue;
                                    if (!int.TryParse(columnData[idx++], out int _d))
                                        _d = badValue;
                                    if (!int.TryParse(columnData[idx++], out int _f))
                                        _f = badValue;
                                    // skip "-" column
                                    if (!int.TryParse(columnData[++idx], out int _a))
                                        _a = badValue;
                                    if (!int.TryParse(columnData[++idx], out int _pts))
                                        _pts = badValue;

                                    ResultList.Add(new ResultData
                                    {
                                        TeamName = columnData[0],
                                        P = _p,
                                        W = _w,
                                        L = _l,
                                        D = _d,
                                        F = _f,
                                        A = _a,
                                        Pts = _pts
                                    });

                                    columnData.Clear();
                                }

                            }
                        }
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Exception when loading file", e);
            }
        }
    }
}
