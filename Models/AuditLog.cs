using System;

namespace WebIdentityApi.Models
{
    public class AuditLog
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime TimeStamp { get; set; }
        public object DataBefore { get; set; }
        public object DataAfter { get; set; }
        public string SearchKeyword { get; set; }
    }
}