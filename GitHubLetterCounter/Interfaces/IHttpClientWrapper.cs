using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Interfaces
{
    //Interface to define http functions needed
    public interface IHttpClientWrapper
    {
        Task<string> GetStringAsync(string url);
    }
}
