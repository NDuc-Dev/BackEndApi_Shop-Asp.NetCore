using System;

namespace WebIdentityApi.Models
{
    public class AuditLog
    {
        public string ActorId { get; set; }
        public string ActorName { get; set; }
        public string Action { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AffectedTable { get; set; }
        public string ObjId { get; set; }
        public string Exception { get; set; }
    }
}