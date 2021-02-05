using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Serilog;

namespace DocumentsKM.Services
{
    public class DepartmentCollectorService : IHostedService
    {
        private readonly ISubscriberService _subscriberService;
        private readonly IDepartmentRepo _departmentRepo;

        public DepartmentCollectorService(
            ISubscriberService subscriberService,
            IDepartmentRepo departmentRepo)
        {
            _subscriberService = subscriberService;
            _departmentRepo = departmentRepo;
        }

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
            if (headers.ContainsKey("entity") && headers.ContainsKey("method"))
            {
                if (headers["entity"].ToString() == "department" && headers["entity"].ToString() == "add")
                {
                    var department = JsonSerializer.Deserialize<Department>(message);
                    if (_departmentRepo.GetById(department.Id) == null)
                        _departmentRepo.Add(department);
                    else
                        Log.Information("[AMQP] Department already exists");
                }
            }
            return true;
        }
    }
}
