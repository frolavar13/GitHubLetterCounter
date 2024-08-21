using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Interfaces
{
    //Interface to define repository processing functions needed
    public interface IRepositoryProcessor
    {
        Task<Dictionary<char, int>> ProcessRepositoryAsync(string repositoryUrl);
    }
}
