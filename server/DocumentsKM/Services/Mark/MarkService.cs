using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class MarkService : IMarkService
    {
        private IMarkRepo _repository;
        private readonly ISubnodeService _subnodeService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public MarkService(
            IMarkRepo markRepo,
            ISubnodeService subnodeService,
            IDepartmentService departmentService,
            IEmployeeService employeeService)
        {
            _repository = markRepo;
            _subnodeService = subnodeService;
            _departmentService = departmentService;
            _employeeService = employeeService;
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            return _repository.GetAllBySubnodeId(subnodeId);
        }

        public Mark GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Create(
            Mark mark,
            int subnodeId,
            int departmentNumber,
            int mainBuilderId,
            int? chiefSpecialistId,
            int? groupLeaderId)

        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = _subnodeService.GetById(subnodeId);
            if (subnode == null)
                throw new ArgumentNullException(nameof(subnode));
            mark.Subnode = subnode;
            var department = _departmentService.GetByNumber(departmentNumber);
            if (department == null)
                throw new ArgumentNullException(nameof(department));
            mark.Department = department;
            var mainBuilder = _employeeService.GetById(mainBuilderId);
            if (mainBuilder == null)
                throw new ArgumentNullException(nameof(mainBuilder));
            mark.MainBuilder = mainBuilder;
            if (chiefSpecialistId != null)
            {
                var chiefSpecialist = _employeeService.GetById(chiefSpecialistId.GetValueOrDefault());
                if (chiefSpecialist == null)
                    throw new ArgumentNullException(nameof(chiefSpecialist));
                mark.ChiefSpecialist = chiefSpecialist;
            }
            if (groupLeaderId != null)
            {
                var groupLeader = _employeeService.GetById(groupLeaderId.GetValueOrDefault());
                if (groupLeader == null)
                    throw new ArgumentNullException(nameof(groupLeader));
                mark.GroupLeader = groupLeader;
            }
            _repository.Create(mark);
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
            if (mark.Code != null)
                foundMark.Code = mark.Code;
            if (mark.Name != null)
                foundMark.Name = mark.Name;
            if (mark.SubnodeId != null)
            {
                var subnode = _subnodeService.GetById(mark.SubnodeId.GetValueOrDefault());
                if (subnode == null)
                    throw new ArgumentNullException(nameof(subnode));
                foundMark.Subnode = subnode;
            }
            if (mark.DepartmentNumber != null)
            {
                var department = _departmentService.GetByNumber(mark.DepartmentNumber.GetValueOrDefault());
                if (department == null)
                    throw new ArgumentNullException(nameof(department));
                foundMark.Department = department;
            }
            if (mark.MainBuilderId != null)
            {
                 var mainBuilder = _employeeService.GetById(mark.MainBuilderId.GetValueOrDefault());
                if (mainBuilder == null)
                    throw new ArgumentNullException(nameof(mainBuilder));
                foundMark.MainBuilder = mainBuilder;
            }
            // Nullable section
            if (mark.ChiefSpecialistId != null)
            {
                int chiefSpecialistId = mark.ChiefSpecialistId.GetValueOrDefault();
                if (chiefSpecialistId == -1)
                    foundMark.ChiefSpecialist = null;
                else
                {
                    var chiefSpecialist = _employeeService.GetById(chiefSpecialistId);
                    if (chiefSpecialist == null)
                        throw new ArgumentNullException(nameof(chiefSpecialist));
                    foundMark.ChiefSpecialist = chiefSpecialist;
                }
            }
            if (mark.GroupLeaderId != null)
            {
                int groupLeaderId = mark.GroupLeaderId.GetValueOrDefault();
                if (groupLeaderId == -1)
                    foundMark.GroupLeader = null;
                else
                {
                    var groupLeader = _employeeService.GetById(groupLeaderId);
                    if (groupLeader == null)
                        throw new ArgumentNullException(nameof(groupLeader));
                    foundMark.GroupLeader = groupLeader;
                }
            }
            _repository.Update(foundMark);
        }

        public void UpdateApprovals(
            int id,
            MarkApprovalsRequest mark)
        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var foundMark = _repository.GetById(id);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            if (mark.ApprovalSpecialist1Id != null)
            {
                int approvalSpecialist1Id = mark.ApprovalSpecialist1Id.GetValueOrDefault();
                if (approvalSpecialist1Id == -1)
                    foundMark.ApprovalSpecialist1 = null;
                else
                {
                    var approvalSpecialist1 = _employeeService.GetById(approvalSpecialist1Id);
                    if (approvalSpecialist1 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist1));
                    foundMark.ApprovalSpecialist1 = approvalSpecialist1;
                }
            }
            if (mark.ApprovalSpecialist2Id != null)
            {
                int approvalSpecialist2Id = mark.ApprovalSpecialist2Id.GetValueOrDefault();
                if (approvalSpecialist2Id == -1)
                    foundMark.ApprovalSpecialist2 = null;
                else
                {
                    var approvalSpecialist2 = _employeeService.GetById(approvalSpecialist2Id);
                    if (approvalSpecialist2 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist2));
                    foundMark.ApprovalSpecialist2 = approvalSpecialist2;
                }
            }
            if (mark.ApprovalSpecialist3Id != null)
            {
                int approvalSpecialist3Id = mark.ApprovalSpecialist3Id.GetValueOrDefault();
                if (approvalSpecialist3Id == -1)
                    foundMark.ApprovalSpecialist3 = null;
                else
                {
                    var approvalSpecialist3 = _employeeService.GetById(approvalSpecialist3Id);
                    if (approvalSpecialist3 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist3));
                    foundMark.ApprovalSpecialist3 = approvalSpecialist3;
                }
            }
            if (mark.ApprovalSpecialist4Id != null)
            {
                int approvalSpecialist4Id = mark.ApprovalSpecialist4Id.GetValueOrDefault();
                if (approvalSpecialist4Id == -1)
                    foundMark.ApprovalSpecialist4 = null;
                else
                {
                    var approvalSpecialist4 = _employeeService.GetById(approvalSpecialist4Id);
                    if (approvalSpecialist4 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist4));
                    foundMark.ApprovalSpecialist4 = approvalSpecialist4;
                }
            }
            if (mark.ApprovalSpecialist5Id != null)
            {
                int approvalSpecialist5Id = mark.ApprovalSpecialist5Id.GetValueOrDefault();
                if (approvalSpecialist5Id == -1)
                    foundMark.ApprovalSpecialist5 = null;
                else
                {
                    var approvalSpecialist5 = _employeeService.GetById(approvalSpecialist5Id);
                    if (approvalSpecialist5 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist5));
                    foundMark.ApprovalSpecialist5 = approvalSpecialist5;
                }
            }
            if (mark.ApprovalSpecialist6Id != null)
            {
                int approvalSpecialist6Id = mark.ApprovalSpecialist6Id.GetValueOrDefault();
                if (approvalSpecialist6Id == -1)
                    foundMark.ApprovalSpecialist6 = null;
                else
                {
                    var approvalSpecialist6 = _employeeService.GetById(approvalSpecialist6Id);
                    if (approvalSpecialist6 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist6));
                    foundMark.ApprovalSpecialist6 = approvalSpecialist6;
                }
            }
            if (mark.ApprovalSpecialist7Id != null)
            {
                int approvalSpecialist7Id = mark.ApprovalSpecialist7Id.GetValueOrDefault();
                if (approvalSpecialist7Id == -1)
                    foundMark.ApprovalSpecialist7 = null;
                else
                {
                    var approvalSpecialist7 = _employeeService.GetById(approvalSpecialist7Id);
                    if (approvalSpecialist7 == null)
                        throw new ArgumentNullException(nameof(approvalSpecialist7));
                    foundMark.ApprovalSpecialist7 = approvalSpecialist7;
                }
            }
            _repository.Update(foundMark);
        }
    }
}
