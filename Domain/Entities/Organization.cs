using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public Organization? ParentOrganization { get; set; }
        public int? ParentOrganizationId { get; set; }

    }
}
