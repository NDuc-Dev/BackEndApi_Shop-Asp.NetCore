using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebIdentityApi.Models
{
    public class ErrorViewForModelState
    {
        public bool Success { get; set; }
        public ErrorModelStateView Error { get; set; }
    }
}