using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;
using DocumentsKM.Helpers;

namespace DocumentsKM.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepo _repository;
        private readonly ISubnodeRepo _subnodeRepo;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEstimateTaskRepo _estimateTaskRepo;
        private readonly ISpecificationService _specificationService;
        private readonly IMarkGeneralDataPointService _markGeneralDataPointService;

        public MarkService(
            IMarkRepo markRepo,
            ISubnodeRepo subnodeRepo,
            IDepartmentRepo departmentRepo,
            IEmployeeRepo employeeRepo,
            IEstimateTaskRepo estimateTaskRepo,
            ISpecificationService specificationService,
            IMarkGeneralDataPointService markGeneralDataPointService)
        {
            _repository = markRepo;
            _subnodeRepo = subnodeRepo;
            _departmentRepo = departmentRepo;
            _employeeRepo = employeeRepo;
            _estimateTaskRepo = estimateTaskRepo;
            _specificationService = specificationService;
            _markGeneralDataPointService = markGeneralDataPointService;
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            return _repository.GetAllBySubnodeId(subnodeId);
        }

        public Mark GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string GetNewMarkCode(int subnodeId){
            var codes = _repository.GetAllBySubnodeId(subnodeId).Select(
                v => v.Code.Substring(2));
            if (codes.Count() == 0)
                return "КМ1";
            var newNum = 1;
            foreach (var code in codes)
            {
                Int16.Parse(code);
                var n = string.Empty;
                for (int i = 0; i < code.Length; i++)
                    if (Char.IsDigit(code[i]))
                        n += code[i];

                if (n.Length > 0)
                {
                    var v = int.Parse(n);
                    if (v >= newNum)
                        newNum = v + 1;
                }
            }
            return "КМ" + newNum.ToString();
        }

        public void Create(
            Mark mark,
            int userId,
            int subnodeId,
            int departmentId,
            int mainBuilderId,
            int? chiefSpecialistId,
            int? groupLeaderId)
        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = _subnodeRepo.GetById(subnodeId);
            if (subnode == null)
                throw new ArgumentNullException(nameof(subnode));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                subnode.Id, mark.Code);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            mark.Subnode = subnode;
            var department = _departmentRepo.GetById(departmentId);
            if (department == null)
                throw new ArgumentNullException(nameof(department));
            mark.Department = department;
            var mainBuilder = _employeeRepo.GetById(mainBuilderId);
            if (mainBuilder == null)
                throw new ArgumentNullException(nameof(mainBuilder));
            if (mainBuilder.Department.Id != departmentId)
                throw new ConflictException(nameof(departmentId));
            mark.MainBuilder = mainBuilder;
            if (chiefSpecialistId != null)
            {
                var chiefSpecialist = _employeeRepo.GetById(
                    chiefSpecialistId.GetValueOrDefault());
                if (chiefSpecialist == null)
                    throw new ArgumentNullException(nameof(chiefSpecialist));
                if (chiefSpecialist.Department.Id != departmentId)
                    throw new ConflictException(nameof(departmentId));
                mark.ChiefSpecialist = chiefSpecialist;
            }
            if (groupLeaderId != null)
            {
                var groupLeader = _employeeRepo.GetById(groupLeaderId.GetValueOrDefault());
                if (groupLeader == null)
                    throw new ArgumentNullException(nameof(groupLeader));
                if (groupLeader.Department.Id != departmentId)
                    throw new ConflictException(nameof(departmentId));
                mark.GroupLeader = groupLeader;
            }
            
            _repository.Add(mark);
            _specificationService.Create(mark.Id);

            _estimateTaskRepo.Add(new EstimateTask
            {
                Mark = mark,
                TaskText = "Разработать сметную документацию к чертежам " + MarkHelper.MakeMarkName(
                    subnode.Node.Project.BaseSeries, subnode.Node.Code, subnode.Code, mark.Code
                ) + "\nСостав и объемы работ:",
            });

            _markGeneralDataPointService.AddDefaultPoints(userId, mark);
        }

        public void Update(
            int id,
            MarkUpdateRequest mark)
        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var foundMark = _repository.GetById(id);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            if (mark.Name != null)
                foundMark.Name = mark.Name;
            
            if ((mark.Code != null) && (mark.SubnodeId != null))
            {
                foundMark.Code = mark.Code;

                var subnode = _subnodeRepo.GetById(mark.SubnodeId.GetValueOrDefault());
                if (subnode == null)
                    throw new ArgumentNullException(nameof(subnode));
                foundMark.Subnode = subnode;

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    subnode.Id, mark.Code);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));
            }
            else if (mark.Code != null)
            {
                foundMark.Code = mark.Code;

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundMark.Subnode.Id, mark.Code);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));
            }
            else if (mark.SubnodeId != null)
            {
                var subnode = _subnodeRepo.GetById(mark.SubnodeId.GetValueOrDefault());
                if (subnode == null)
                    throw new ArgumentNullException(nameof(subnode));
                foundMark.Subnode = subnode;

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    subnode.Id, foundMark.Code);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));
            }

            if (mark.DepartmentId != null)
            {
                var department = _departmentRepo.GetById(mark.DepartmentId.GetValueOrDefault());
                if (department == null)
                    throw new ArgumentNullException(nameof(department));
                foundMark.Department = department;
            }
            if (mark.MainBuilderId != null)
            {
                var mainBuilder = _employeeRepo.GetById(mark.MainBuilderId.GetValueOrDefault());
                if (mainBuilder == null)
                    throw new ArgumentNullException(nameof(mainBuilder));
                if (mainBuilder.Department.Id != foundMark.Department.Id)
                        throw new ConflictException("departmentId");
                foundMark.MainBuilder = mainBuilder;
            }
            // Nullable section
            if (mark.ChiefSpecialistId != null)
            {
                int chiefSpecialistId = mark.ChiefSpecialistId.GetValueOrDefault();
                if (chiefSpecialistId == -1)
                    foundMark.ChiefSpecialistId = null;
                else
                {
                    var chiefSpecialist = _employeeRepo.GetById(chiefSpecialistId);
                    if (chiefSpecialist == null)
                        throw new ArgumentNullException(nameof(chiefSpecialist));
                    if (chiefSpecialist.Department.Id != foundMark.Department.Id)
                        throw new ConflictException("departmentId");
                    foundMark.ChiefSpecialist = chiefSpecialist;
                }
            }
            if (mark.GroupLeaderId != null)
            {
                int groupLeaderId = mark.GroupLeaderId.GetValueOrDefault();
                if (groupLeaderId == -1)
                    foundMark.GroupLeaderId = null;
                else
                {
                    var groupLeader = _employeeRepo.GetById(groupLeaderId);
                    if (groupLeader == null)
                        throw new ArgumentNullException(nameof(groupLeader));
                    if (groupLeader.Department.Id != foundMark.Department.Id)
                        throw new ConflictException("departmentId");
                    foundMark.GroupLeader = groupLeader;
                }
            }
            foundMark.EditedDate = DateTime.Now;
            _repository.Update(foundMark);
        }

        public void UpdateIssueDate(Mark mark)
        {
            mark.IssueDate = DateTime.Now;
            mark.EditedDate = DateTime.Now;
            _repository.Update(mark);
        }

        public void UpdateIssueDate(Mark mark, DateTime date)
        {
            mark.IssueDate = date;
            mark.EditedDate = DateTime.Now;
            _repository.Update(mark);
        }
    }
}
