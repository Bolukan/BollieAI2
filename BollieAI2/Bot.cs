using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using BollieAI2.Services;

namespace BollieAI2
{
    /// <summary>
    /// Provides a class that reads and parses the input
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// The parser
        /// </summary>
        private Parser parser;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        public Bot()
        {
            parser = new Parser();
        }
        
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run(bool debug)
        {
            if (debug)
            {
                foreach (var line in File.ReadAllLines(@"Z:\BollieAI2\549c84eb4b5ab240b714a5f7.txt"))
                {
                    parser.Parse(line);
                }
                Console.ReadLine();
            }
        
            while (true)
            {
                /* Normalize the input:
                 * 1) Trim leading and trailing whitespaces
                 * 2) Replace all whitespaces with a regular space
                 * */
                String line = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    Regex.Replace(line, "\\s+", " ");
                    //Let the parser deal with it
                    parser.Parse(line);
                }
            }
        }
    }
}