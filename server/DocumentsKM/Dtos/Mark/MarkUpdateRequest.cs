using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkUpdateRequest
    {
        public Int16? SubnodeId { get; set; }
        [MaxLength(40)]
        public string Code { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public Int16? DepartmentId { get; set; }
        public Int32? ChiefSpecialistId { get; set; }
        public Int32? GroupLeaderId { get; set; }
        public Int32? MainBuilderId { get; set; }

        public MarkUpdateRequest()
        {
            SubnodeId = null;
            Code = null;
            Name = null;
            DepartmentId = null;
            ChiefSpecialistId = null;
            GroupLeaderId = null;
            MainBuilderId = null;
        }
    }
}
