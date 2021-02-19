using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using Serilog;

namespace DocumentsKM.Services
{
    public class EstimateTaskService : IEstimateTaskService
    {
        private readonly IEstimateTaskRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public EstimateTaskService(
            IEstimateTaskRepo estimateTaskRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo)
        {
            _repository = estimateTaskRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
        }

        public EstimateTask GetByMarkId(int markId)
        {
            return _repository.GetByMarkId(markId);
        }

        public void Update(
            int markId,
            EstimateTaskUpdateRequest estimateTask)
        {
            if (estimateTask == null)
                throw new ArgumentNullException(nameof(estimateTask));
            var foundEstimateTask = _repository.GetByMarkId(markId);
            if (foundEstimateTask == null)
                throw new ArgumentNullException(nameof(foundEstimateTask));

            if (estimateTask.TaskText != null)
                foundEstimateTask.TaskText = estimateTask.TaskText;
            if (estimateTask.AdditionalText != null)
                foundEstimateTask.AdditionalText = estimateTask.AdditionalText;
            if (estimateTask.ApprovalEmployeeId != null)
            {
                int approvalEmployeeId = estimateTask.ApprovalEmployeeId.GetValueOrDefault();
                if (approvalEmployeeId == -1)
                    foundEstimateTask.ApprovalEmployeeId = null;
                else
                {
                    var approvalEmployee = _employeeRepo.GetById(approvalEmployeeId);
                    if (approvalEmployee == null)
                        throw new ArgumentNullException(nameof(approvalEmployee));
                    foundEstimateTask.ApprovalEmployee = approvalEmployee;
                }
            }
            _repository.Update(foundEstimateTask);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
