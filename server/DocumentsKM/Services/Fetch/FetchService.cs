using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using DocumentsKM.Helpers;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NCrontab;
using Serilog;

public class FetchService : IHostedService
{
    private readonly CrontabSchedule _crontabSchedule;
    private DateTime _nextRun;

    // Cron time string format
    // Second Minute Hour DayOfTheMonth Month DayOfTheWeek

    // Daily at 1 am
    private const string Schedule = "0 0 1 * * *";

    private readonly IHttpClientFactory _clientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AppSettings _appSettings;

    public FetchService(
        IHttpClientFactory clientFactory,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AppSettings> appSettings)
    {
        _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions{IncludingSeconds = true});
        _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);

        _clientFactory = clientFactory;
        _serviceScopeFactory = serviceScopeFactory;
        _appSettings = appSettings.Value;
    }

    public async Task OnGet()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            // Personnel
            string baseUrl = _appSettings.PersonnelUrl;
            var client = _clientFactory.CreateClient();

            Log.Information("Fetching departments");
            var departmentUrl = baseUrl + "department";
            var departmentRequest = new HttpRequestMessage(HttpMethod.Get, departmentUrl);
            departmentRequest.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(departmentRequest);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var departments = await JsonSerializer.DeserializeAsync<IEnumerable<DepartmentFetched>>(
                    responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
                departmentService.UpdateAll(departments.ToList());
            }
            else
                Log.Fatal("Error while fetching departments");
            Log.Information("Departments were fetched successfully");

            Log.Information("Fetching posts");
            var postUrl = baseUrl + "post";
            var postRequest = new HttpRequestMessage(HttpMethod.Get, postUrl);
            postRequest.Headers.Add("Accept", "application/json");
            response = await client.SendAsync(postRequest);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var positions = await JsonSerializer.DeserializeAsync<IEnumerable<Position>>(
                    responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                var positionService = scope.ServiceProvider.GetRequiredService<IPositionService>();
                positionService.UpdateAll(positions.ToList());
            }
            else
                Log.Fatal("Error while fetching posts");
            Log.Information("Posts were fetched successfully");

            Log.Information("Fetching staff");
            var staffUrl = baseUrl + "staff";
            var staffRequest = new HttpRequestMessage(HttpMethod.Get, staffUrl);
            staffRequest.Headers.Add("Accept", "application/json");
            response = await client.SendAsync(staffRequest);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var employees = await JsonSerializer.DeserializeAsync<IEnumerable<EmployeeFetched>>(
                    responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
                employeeService.UpdateAll(employees.ToList());
            }
            else
                Log.Fatal("Error while fetching staff");
            Log.Information("Staff was fetched successfully");
        }
    }

    // DEBUG
    // public async Task OnTest()
    // {
    //     using (var scope = _serviceScopeFactory.CreateScope())
    //     {
    //         string baseUrl = _appSettings.PersonnelUrl;
    //         var client = _clientFactory.CreateClient();

    //         Log.Information("Fetching departments");
    //         var departmentUrl = baseUrl + "department";
    //         var departmentRequest = new HttpRequestMessage(HttpMethod.Get, departmentUrl);
    //         departmentRequest.Headers.Add("Accept", "application/json");
    //         var response = await client.SendAsync(departmentRequest);
    //         if (response.IsSuccessStatusCode)
    //         {
    //             using var responseStream = await response.Content.ReadAsStreamAsync();
    //             var departments = await JsonSerializer.DeserializeAsync<IEnumerable<DepartmentFetched>>(
    //                 responseStream,
    //                 new JsonSerializerOptions
    //                 {
    //                     PropertyNameCaseInsensitive = true,
    //                 });
    //             var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
    //             departmentService.UpdateAll(departments.ToList());
    //         }
    //         else
    //             Log.Fatal("Error while fetching departments");
    //         Log.Information("Departments were fetched successfully");
    //     }
    // }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(UntilNextExecution(), cancellationToken);
                await OnGet();
                _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            }
        }, cancellationToken);

        return Task.CompletedTask;
        // return OnTest();
    }

    private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
