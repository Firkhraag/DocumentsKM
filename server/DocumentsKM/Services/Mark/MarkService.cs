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
        private readonly ISubnodeRepo _subnodeRepo;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IEmployeeRepo _employeeRepo;

        private readonly ISpecificationService _specificationService;

        public MarkService(
            IMarkRepo markRepo,
            ISubnodeRepo subnodeRepo,
            IDepartmentRepo departmentRepo,
            IEmployeeRepo employeeRepo,
            ISpecificationService specificationService)
        {
            _repository = markRepo;
            _subnodeRepo = subnodeRepo;
            _departmentRepo = departmentRepo;
            _employeeRepo = employeeRepo;

            _specificationService = specificationService;
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
            mark.Subnode = subnode;
            var department = _departmentRepo.GetById(departmentId);
            if (department == null)
                throw new ArgumentNullException(nameof(department));
            mark.Department = department;
            var mainBuilder = _employeeRepo.GetById(mainBuilderId);
            if (mainBuilder == null)
                throw new ArgumentNullException(nameof(mainBuilder));
            mark.MainBuilder = mainBuilder;
            if (chiefSpecialistId != null)
            {
                var chiefSpecialist = _employeeRepo.GetById(chiefSpecialistId.GetValueOrDefault());
                if (chiefSpecialist == null)
                    throw new ArgumentNullException(nameof(chiefSpecialist));
                mark.ChiefSpecialist = chiefSpecialist;
            }
            if (groupLeaderId != null)
            {
                var groupLeader = _employeeRepo.GetById(groupLeaderId.GetValueOrDefault());
                if (groupLeader == null)
                    throw new ArgumentNullException(nameof(groupLeader));
                mark.GroupLeader = groupLeader;
            }
            
            _repository.Add(mark);
            _specificationService.Create(mark.Id);
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
                var subnode = _subnodeRepo.GetById(mark.SubnodeId.GetValueOrDefault());
                if (subnode == null)
                    throw new ArgumentNullException(nameof(subnode));
                foundMark.Subnode = subnode;
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
                    var chiefSpecialist = _employeeRepo.GetById(chiefSpecialistId);
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
                    var groupLeader = _employeeRepo.GetById(groupLeaderId);
                    if (groupLeader == null)
                        throw new ArgumentNullException(nameof(groupLeader));
                    foundMark.GroupLeader = groupLeader;
                }
            }
            _repository.Update(foundMark);
        }
    }
}
