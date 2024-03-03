using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedUserId { get; set; }
        public int DomainId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
