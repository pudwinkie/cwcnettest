//
// bittorrent File Creator
// Author: http://www.cnblogs.com/thomasnet
// Email: cnblogreply@gmail.com
// License: GPL v3, http://www.gnu.org/copyleft/gpl.html
//

using System;
using System.Collections.Generic;
using System.Text;
using BenCode;
using System.IO;

namespace BtCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            InputParameter parameter;
            try
            {
                parameter = ParseUserInput(args);
                VerifyInput(parameter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            TorrentFileCreator.OnProgress += WriteProgress;
            if (!string.IsNullOrEmpty(parameter.fileName))
            {
                TorrentFileCreator.EncodeFile(parameter.fileName,
                    parameter.announceUrl);
            }
            else
            {
                TorrentFileCreator.EncodeDirectory(parameter.directoryPath,
                    parameter.announceUrl);
            }
            Console.WriteLine();
            Console.WriteLine("All done!");
        }

        static void ShowWelcomeMessage()
        {
            Console.WriteLine("===A C# tool to create torrent hash===");
            Console.WriteLine("Licensed under GPL v3, http://www.gnu.org/copyleft/gpl.html");
        }

        static InputParameter ParseUserInput(string[] args)
        {
            InputParameter parameter = new InputParameter();
            if (args.Length % 2 == 1)
            {
                ThrowInvalidInputException();
            }

            for(int i=0; i<args.Length; i+=2)
            {
                switch (args[i].ToLower())
                {
                    case "-a":
                        parameter.announceUrl = args[i + 1];
                        break;
                    case "-f":
                        parameter.fileName = args[i + 1];
                        break;
                    case "-d":
                        parameter.directoryPath = args[i + 1];
                        break;
                }
            }
            return parameter;
        }

        static void VerifyInput(InputParameter parameter)
        {
            bool valid = true;
            // account url not null
            if (string.IsNullOrEmpty(parameter.announceUrl))
            {
                valid = false;
            }

            // enocde file or directory
            if (string.IsNullOrEmpty(parameter.fileName)&&
                string.IsNullOrEmpty(parameter.directoryPath))
            {
                valid = false;
            }

            // filename should exist
            if (!string.IsNullOrEmpty(parameter.fileName) && 
                !File.Exists(parameter.fileName))
            {
                valid = false;
            }

            // directory should exist
            if (!string.IsNullOrEmpty(parameter.directoryPath) &&
                !Directory.Exists(parameter.directoryPath))
            {
                valid = false;
            }

            if (valid == false)
                ThrowInvalidInputException();
        }

        static void ThrowInvalidInputException()
        {
            string encodeFileExample = "btcreator -a http://tracker.domain.com:8080/announce -f filename";
            string encodeDirectoryExample = "btcreator -a http://tracker.domain.com:8080/announce -d directory"; ;
            throw new Exception(
                string.Format("Invalid parameter!\nEncode a file: {0}\nEncode a directory: {1}",
                encodeFileExample, encodeDirectoryExample));
        }

        static void WriteProgress(UInt64 numByteRead)
        {
            Console.Write("#");
        }
    }

    class InputParameter
    {
        public string announceUrl;
        public string fileName;
        public string directoryPath;
    }
}
