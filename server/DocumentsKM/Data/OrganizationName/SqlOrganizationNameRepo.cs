using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlOrganizationNameRepo : IOrganizationNameRepo
    {
        private readonly ApplicationContext _context;

        public SqlOrganizationNameRepo(ApplicationContext context)
        {
            _context = context;
        }

        public OrganizationName Get()
        {
            return _context.OrganizationName.SingleOrDefault();
        }
    }
}
