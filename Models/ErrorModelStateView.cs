using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebIdentityApi.Models
{
    public class ErrorModelStateView
    {
        public string Code { get; set; }
        public List<string> Errors { get; set; }
    }
}