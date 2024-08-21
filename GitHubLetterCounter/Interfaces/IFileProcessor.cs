using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Interfaces
{
    //Interface to define file processing functions needed
    public interface IFileProcessor
    {
        Dictionary<char, int> ProcessFileContent(string content);
    }
}
