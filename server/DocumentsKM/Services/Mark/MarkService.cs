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
        private readonly INodeRepo _nodeRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEstimateTaskRepo _estimateTaskRepo;
        private readonly ISpecificationService _specificationService;
        private readonly IMarkGeneralDataPointService _markGeneralDataPointService;

        public MarkService(
            IMarkRepo markRepo,
            ISubnodeRepo subnodeRepo,
            INodeRepo nodeRepo,
            IProjectRepo projectRepo,
            IDepartmentRepo departmentRepo,
            IEmployeeRepo employeeRepo,
            IEstimateTaskRepo estimateTaskRepo,
            ISpecificationService specificationService,
            IMarkGeneralDataPointService markGeneralDataPointService)
        {
            _repository = markRepo;
            _subnodeRepo = subnodeRepo;
            _nodeRepo = nodeRepo;
            _projectRepo = projectRepo;
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

        public IEnumerable<Mark> GetAllByIds(List<int> ids)
        {
            return _repository.GetAllByIds(ids);
        }

        public Mark GetById(int id)
        {
            return _repository.GetById(id);
        }

        public (Mark, int, int, int) GetByIdWithParentIds(int id)
        {
            var mark = _repository.GetById(id);
            if (mark == null)
                return (null, 0, 0, 0);
            var subnodeId = mark.SubnodeId;
            var subnode = _subnodeRepo.GetById(subnodeId);
            var nodeId = subnode.NodeId;
            var node = _nodeRepo.GetById(nodeId);
            var projectId = node.ProjectId;
            return (mark, subnodeId, subnodeId, projectId);
        }

        public string GetNewMarkCode(int subnodeId){
            var codes = _repository.GetAllBySubnodeId(subnodeId).Select(
                v => v.Code.Substring(2));
            if (codes.Count() == 0)
                return "КМ1";
            var newNum = 1;
            foreach (var code in codes)
            {
                var n = string.Empty;
                for (int i = 0; i < code.Length; i++)
                    if (Char.IsDigit(code[i]))
                        n += code[i];
                    else
                        break;

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
            int? chiefSpecialistId,
            int? groupLeaderId,
            int? normContrId)
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

            mark.SubnodeId = subnodeId;

            var node = _nodeRepo.GetById(subnode.NodeId);
            mark.ChiefEngineerName = node.ChiefEngineerName;

            var project = _projectRepo.GetById(node.ProjectId);

            mark.Designation = MarkHelper.MakeMarkName(
                project.BaseSeries, node.Code, subnode.Code, mark.Code);

            (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                project.Name, node.Name, subnode.Name, mark.Name, project.Bias);
            mark.ComplexName = complexName;
            mark.ObjectName = objectName;

            var department = _departmentRepo.GetById(departmentId);
            if (department == null)
                throw new ArgumentNullException(nameof(department));
            mark.Department = department;

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
            if (normContrId != null)
            {
                var normContr = _employeeRepo.GetById(
                    normContrId.GetValueOrDefault());
                if (normContr == null)
                    throw new ArgumentNullException(nameof(normContr));
                if (normContr.Department.Id != departmentId)
                    throw new ConflictException(nameof(departmentId));
                mark.NormContr = normContr;
            }

            mark.EditedDate = DateTime.Now;
            
            _repository.Add(mark);
            _specificationService.Create(mark.Id);

            _estimateTaskRepo.Add(new EstimateTask
            {
                Mark = mark,
                TaskText = "Разработать сметную документацию к чертежам " + mark.Designation + "\nСостав и объемы работ:",
            });

            _markGeneralDataPointService.AddDefaultPoints(userId, mark);
        }

        public string Update(
            int id,
            MarkUpdateRequest mark)
        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var foundMark = _repository.GetById(id);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            Subnode subnode = null;
            Node node = null;
            Project project = null;
            var archiveWasFetched = false;

            if (mark.Name != null)
            {
                foundMark.Name = mark.Name;

                subnode = _subnodeRepo.GetById(foundMark.SubnodeId);
                node = _nodeRepo.GetById(subnode.NodeId);
                project = _projectRepo.GetById(node.ProjectId);

                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name, project.Bias);
                foundMark.ComplexName = complexName;
                foundMark.ObjectName = objectName;

                archiveWasFetched = true;
            }

            var designation = foundMark.Designation;
            if (mark.Code != null)
            {
                foundMark.Code = mark.Code;

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundMark.SubnodeId, mark.Code);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));

                if (!archiveWasFetched)
                {
                    subnode = _subnodeRepo.GetById(foundMark.SubnodeId);
                    node = _nodeRepo.GetById(subnode.NodeId);
                    project = _projectRepo.GetById(node.ProjectId);
                }

                designation = MarkHelper.MakeMarkName(project.BaseSeries, node.Code, subnode.Code, mark.Code);
                foundMark.Designation = designation;
            }

            if (mark.DepartmentId != null)
            {
                var department = _departmentRepo.GetById(mark.DepartmentId.GetValueOrDefault());
                if (department == null)
                    throw new ArgumentNullException(nameof(department));
                foundMark.Department = department;
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
            if (mark.NormContrId != null)
            {
                int normContrId = mark.NormContrId.GetValueOrDefault();
                if (normContrId == -1)
                    foundMark.NormContrId = null;
                else
                {
                    var normContr = _employeeRepo.GetById(normContrId);
                    if (normContr == null)
                        throw new ArgumentNullException(nameof(normContr));
                    if (normContr.Department.Id != foundMark.Department.Id)
                        throw new ConflictException("departmentId");
                    foundMark.NormContr = normContr;
                }
            }
            foundMark.EditedDate = DateTime.Now;
            _repository.Update(foundMark);

            return designation;
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
