using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentsKM.Services
{
    public class DataCollectorService : IHostedService
    {
        private readonly ISubscriberService _subscriberService;
        // private readonly IDepartmentRepo _departmentRepo;
        public IServiceScopeFactory _serviceScopeFactory;
        public DataCollectorService(
            IServiceScopeFactory serviceScopeFactory,
            ISubscriberService subscriberService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _subscriberService = subscriberService;
        }

        // public DataCollectorService(
        //     ISubscriberService subscriberService,
        //     IDepartmentRepo departmentRepo)
        // {
        //     _subscriberService = subscriberService;
        //     _departmentRepo = departmentRepo;
        // }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriberService.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            Log.Information($"[AMQP] Message: {message}");
            if (headers.ContainsKey("entity") && headers.ContainsKey("method"))
            {
                if (headers["entity"].ToString() == "department" && headers["entity"].ToString() == "add")
                {
                    var department = JsonSerializer.Deserialize<Department>(message);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var departmentRepo = scope.ServiceProvider.GetRequiredService<IDepartmentRepo>();
                        
                        if (departmentRepo.GetById(department.Id) == null)
                        departmentRepo.Add(department);
                        else
                            Log.Information($"[AMQP] Department with id {department.Id} already exists");
                    }
                }
            }
            return true;
        }
    }
}
