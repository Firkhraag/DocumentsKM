using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;

namespace DocumentsKM.Services
{
    public class MarkService : IMarkService
    {
        private IMarkRepo _repository;
        private readonly ISubnodeService _subnodeService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public MarkService(
            IMarkRepo MarkRepo,
            ISubnodeService subnodeService,
            IDepartmentService departmentService,
            IEmployeeService employeeService)
        {
            _repository = MarkRepo;
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
            int chiefSpecialistId,
            int groupLeaderId)

        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            validateMark(
                mark,
                subnodeId,
                departmentNumber,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId);
            _repository.Create(mark);
        }

        public void Update(
            Mark mark,
            int subnodeId,
            int departmentNumber,
            int mainBuilderId,
            int chiefSpecialistId,
            int groupLeaderId)
        {
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var foundMark = _repository.GetById(mark.Id);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            foundMark.Code = mark.Code;
            foundMark.Name = mark.Name;
            validateMark(
                foundMark,
                subnodeId,
                departmentNumber,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId);
            _repository.Update(foundMark);
        }

        private void validateMark(
            Mark mark,
            int subnodeId,
            int departmentNumber,
            int mainBuilderId,
            int chiefSpecialistId,
            int groupLeaderId)
        {
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
            if (chiefSpecialistId != -1)
            {
                var chiefSpecialist = _employeeService.GetById(chiefSpecialistId);
                if (chiefSpecialist == null)
                    throw new ArgumentNullException(nameof(chiefSpecialist));
                mark.ChiefSpecialist = chiefSpecialist;
            }
            if (groupLeaderId != -1)
            {
                var groupLeader = _employeeService.GetById(groupLeaderId);
                if (groupLeader == null)
                    throw new ArgumentNullException(nameof(groupLeader));
                mark.GroupLeader = groupLeader;
            }
        }

        // public void Update(Mark mark)
        // {
        //     if (mark == null)
        //     {
        //         throw new ArgumentNullException(nameof(mark));
        //     }
        //     _repository.Update(mark);
        // }
    }
}
