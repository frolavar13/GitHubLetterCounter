using GitHubLetterCounter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Services
{
    public class TextFileProcessor : IFileProcessor
    {
        //Process the file content received returning the number of occurrences of each letter
        public Dictionary<char, int> ProcessFileContent(string content)
        {
            var letterCounts = new Dictionary<char, int>();

            foreach (var c in content)
            {
                if (char.IsLetter(c))
                {
                    var lowerChar = char.ToLower(c);
                    if (letterCounts.ContainsKey(lowerChar))
                    {
                        letterCounts[lowerChar]++;
                    }
                    else
                    {
                        letterCounts[lowerChar] = 1;
                    }
                }
            }

            return letterCounts;
        }
    }

}
