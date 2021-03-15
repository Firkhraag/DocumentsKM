using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DocumentsKM.Services
{
    public class FetchService : IHostedService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FetchService(
            IHttpClientFactory clientFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _clientFactory = clientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task OnGet()
        {
            Log.Information("Start fetching");
            var url = "";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var departments = await JsonSerializer.DeserializeAsync<IEnumerable<DepartmentFetched>>(responseStream);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
                    
                    departmentService.UpdateAll(departments.ToList());
                }
            }
            else
                Log.Fatal("Error while fetching the data");
            Log.Information("End fetching");
        }


        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // return OnGet();
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading;
// using System.Threading.Tasks;
// using DocumentsKM.Dtos;
// using DocumentsKM.Services;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using NCrontab;
// using Serilog;

// public class FetchService : IHostedService
// {
//     private readonly CrontabSchedule _crontabSchedule;
//     private DateTime _nextRun;

//     // Cron time string format
//     // Minute Hour DayOfTheMonth Month DayOfTheWeek

//     // // Daily at 1 am
//     // private const string Schedule = "0 0 1 * * *";
//     // Each minute
//     private const string Schedule = "1 * * * * *";

//     private readonly IHttpClientFactory _clientFactory;
//     private readonly IServiceScopeFactory _serviceScopeFactory;

//     public FetchService(
//         IHttpClientFactory clientFactory,
//         IServiceScopeFactory serviceScopeFactory)
//     {
//         _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions{IncludingSeconds = true});
//         _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);

//         _clientFactory = clientFactory;
//         _serviceScopeFactory = serviceScopeFactory;
//     }

//     public async Task OnGet()
//     {
//         Log.Information("Start fetching");
//         var url = "";
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Add("Accept", "application/json");

//         var client = _clientFactory.CreateClient();

//         var response = await client.SendAsync(request);
//         if (response.IsSuccessStatusCode)
//         {
//             using var responseStream = await response.Content.ReadAsStreamAsync();
//             var departments = await JsonSerializer.DeserializeAsync<IEnumerable<DepartmentFetched>>(responseStream);
//             using (var scope = _serviceScopeFactory.CreateScope())
//             {
//                 var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
                
//                 departmentService.UpdateAll(departments.ToList());
//             }
//         }
//         else
//             Log.Fatal("Error while fetching the data");
//         Log.Information("End fetching");
//     }

//     public virtual Task StartAsync(CancellationToken cancellationToken)
//     {
//         Task.Run(async () =>
//         {
//             while (!cancellationToken.IsCancellationRequested)
//             {
//                 await Task.Delay(UntilNextExecution(), cancellationToken);
//                 await OnGet();
//                 _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
//             }
//         }, cancellationToken);
//         return Task.CompletedTask;
//     }

//     private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

//     public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
// }
