using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using System.Linq;

namespace DocumentsKM.Services
{
    public class ArchiveMarkService : IArchiveMarkService
    {
        private readonly IArchiveMarkRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IArchiveDepartmentRepo _archiveDepartmentRepo;
        private readonly IDepartmentRepo _departmentRepo;

        public ArchiveMarkService(
            IArchiveMarkRepo archiveMarkRepo,
            IMarkRepo markRepo,
            IArchiveDepartmentRepo archiveDepartmentRepo,
            IDepartmentRepo departmentRepo)
        {
            _repository = archiveMarkRepo;
            _markRepo = markRepo;
            _archiveDepartmentRepo = archiveDepartmentRepo;
            _departmentRepo = departmentRepo;
        }

        public IEnumerable<ArchiveMark> GetAllBySubnodeId(int subnodeId)
        {
            var markCodes = _markRepo.GetAllBySubnodeId(subnodeId).Select(v => v.Code);
            var archiveMarks =  _repository.GetAllBySubnodeId(subnodeId).Where(v => !markCodes.Contains(v.Code));
            foreach (var archiveMark in archiveMarks)
            {
                var archiveDepartment = _archiveDepartmentRepo.GetById(archiveMark.DepartmentId);
                archiveMark.Department = _departmentRepo.GetByCode(archiveDepartment.Code);
            }
            return archiveMarks;
        }
    }
}
