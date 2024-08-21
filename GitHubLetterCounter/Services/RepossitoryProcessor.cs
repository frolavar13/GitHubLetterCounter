using GitHubLetterCounter.Interfaces;
using GitHubLetterCounter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Services
{
    public class RepositoryProcessor : IRepositoryProcessor
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IFileProcessor _fileProcessor;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(4);

        public RepositoryProcessor(IHttpClientWrapper httpClientWrapper, IFileProcessor fileProcessor)
        {
            _httpClientWrapper = httpClientWrapper;
            _fileProcessor = fileProcessor;
        }

        //Processes the received repository returning the number of occurrences of each letter
        public async Task<Dictionary<char, int>> ProcessRepositoryAsync(string repositoryUrl)
        {
            //This extensions should not be written here, they should be in a config file
            // or given by the user in a final version
            
            var extensions = new List<string>()
            {
                ".js",
                ".ts"
            };
            var letterCounts = new Dictionary<char, int>();

            // Obtains every file url from the repository that ends with something from the extensions list
            var fileUrls = await ExtractFileUrlsAsync(repositoryUrl, extensions);

            var tasks = new List<Task<Dictionary<char, int>>>();

            foreach (var fileUrl in fileUrls)
            {
                //Process the files in parallel using 4 threads
                tasks.Add(Task.Run(async () =>
                {
                    //Control the number of threads with a semaphore
                    await _semaphore.WaitAsync();
                    try
                    {
                        //Obtain and process the file content
                        var fileContent = await _httpClientWrapper.GetStringAsync(fileUrl);
                        return _fileProcessor.ProcessFileContent(fileContent);
                    }
                    catch (Exception ex)
                    {
                        //Handle posible exception
                        Logger.Error($"Unexpected error while processing file: {fileUrl} -> {ex.Message}");
                        return new Dictionary<char, int>();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }));
            }

            //Wait for every task to be completed
            var results = await Task.WhenAll(tasks);

            //Combine the results
            foreach (var result in results)
            {
                foreach (var kvp in result)
                {
                    if (letterCounts.ContainsKey(kvp.Key))
                    {
                        letterCounts[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        letterCounts[kvp.Key] = kvp.Value;
                    }
                }
            }

            return letterCounts;
        }

        //Returns a list with every file that ends with the specified extensions in the repository
        private async Task<List<string>> ExtractFileUrlsAsync(string repositoryUrl, List<string> extensions)
        {
            var fileUrls = new List<string>();

            // Obtain the directory content
            var content = await _httpClientWrapper.GetStringAsync(repositoryUrl);
            var items = JsonSerializer.Deserialize<List<RepositoryItem>>(content);

            foreach (var item in items)
            {
                if (item.type == "file" && (extensions.Any(x => item.name.EndsWith(x))))
                {
                    //Add the file if ends with any extension included
                    fileUrls.Add(item.download_url);
                }
                else if (item.type == "dir")
                {
                    //Recursively search if it is a directory
                    var subdirectoryFileUrls = await ExtractFileUrlsAsync(item.url, extensions);
                    fileUrls.AddRange(subdirectoryFileUrls);
                }
            }

            return fileUrls;
        }

    }

}
