using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TMS.Tests.Common
{
    public static class Utils
    {
        public static void EnvFileReader(string pathToFile)
        {
            Regex GetEnvReg = new Regex(@"^([^=\n\t\r ]+) *= *([^=\n\t\r ]+) *$");
            string[] lines = File.ReadAllLines(pathToFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Match match = GetEnvReg.Match(line);
                    if (match.Success)
                    {
                        Environment.SetEnvironmentVariable(match.Groups[1].Value, match.Groups[2].Value);
                    }
                }
            }
        }
    }
}
