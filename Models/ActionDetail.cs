using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class ActionDetail
    {
        public int id { get; set; }
        public string HandleByUserId { get; set; }
        public User HandleBy { get; set; }
        public string UserHandle { get; set; }
        public string HandleTable { get; set; }
        public string Action { get; set; }
        public string DataBefore { get; set; }
        public string DataAfter { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HandleAt { get; set; }
    }
}