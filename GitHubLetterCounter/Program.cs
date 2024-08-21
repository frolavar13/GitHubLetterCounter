using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GitHubLetterCounter.Interfaces;
using GitHubLetterCounter.Services;
using Microsoft.Extensions.DependencyInjection;

// Program.cs
class Program
{
    //TO DO: Include unit tests

    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IHttpClientWrapper, HttpClientWrapper>()
            .AddSingleton<IFileProcessor, TextFileProcessor>()
            .AddSingleton<IRepositoryProcessor, RepositoryProcessor>()
            .BuildServiceProvider();

        Logger.Info("Application started");

        var repositoryProcessor = serviceProvider.GetService<IRepositoryProcessor>();

        //lodash repository url
        string repositoryUrl = "https://api.github.com/repos/lodash/lodash/contents"; 
        var letterCounts = await repositoryProcessor.ProcessRepositoryAsync(repositoryUrl);

        //Show the results
        foreach (var letter in letterCounts.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"Letter: {letter.Key}, Count: {letter.Value}");
        }

        Logger.Info("Application finished");
    }
}
