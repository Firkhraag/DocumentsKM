using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class DefaultValuesService : IDefaultValuesService
    {
        private readonly IDefaultValuesRepo _repository;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public DefaultValuesService(
            IDefaultValuesRepo defaultValuesRepo,
            IDepartmentRepo departmentRepo,
            IEmployeeRepo employeeRepo)
        {
            _repository = defaultValuesRepo;
            _departmentRepo = departmentRepo;
            _employeeRepo = employeeRepo;
        }

        public DefaultValues GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId);
        }

        public void Update(
            int userId,
            DefaultValuesUpdateRequest defaultValues)
        {
            if (defaultValues == null)
                throw new ArgumentNullException(nameof(defaultValues));
            var foundDefaultValues = _repository.GetByUserId(userId);
            if (foundDefaultValues == null)
                throw new ArgumentNullException(nameof(foundDefaultValues));

            if (defaultValues.DepartmentId != null)
            {
                int departmentId = defaultValues.DepartmentId.GetValueOrDefault();
                if (departmentId == -1)
                    foundDefaultValues.DepartmentId = null;
                else
                {
                    var department = _departmentRepo.GetById(departmentId);
                    if (department == null)
                        throw new ArgumentNullException(nameof(department));
                    foundDefaultValues.Department = department;
                }
            }
            if (defaultValues.CreatorId != null)
            {
                int creatorId = defaultValues.CreatorId.GetValueOrDefault();
                if (creatorId == -1)
                    foundDefaultValues.CreatorId = null;
                else
                {
                    var creator = _employeeRepo.GetById(creatorId);
                    if (creator == null)
                        throw new ArgumentNullException(nameof(creator));
                    foundDefaultValues.Creator = creator;
                }
            }
            if (defaultValues.InspectorId != null)
            {
                int inspectorId = defaultValues.InspectorId.GetValueOrDefault();
                if (inspectorId == -1)
                    foundDefaultValues.InspectorId = null;
                else
                {
                    var inspector = _employeeRepo.GetById(inspectorId);
                    if (inspector == null)
                        throw new ArgumentNullException(nameof(inspector));
                    foundDefaultValues.Inspector = inspector;
                }
            }
            if (defaultValues.NormContrId != null)
            {
                int normContrId = defaultValues.NormContrId.GetValueOrDefault();
                if (normContrId == -1)
                    foundDefaultValues.NormContrId = null;
                else
                {
                    var normContr = _employeeRepo.GetById(normContrId);
                    if (normContr == null)
                        throw new ArgumentNullException(nameof(normContr));
                    foundDefaultValues.NormContr = normContr;
                }
            }

            _repository.Update(foundDefaultValues);
        }
    }
}
