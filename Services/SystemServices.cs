using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using WebIdentityApi.Data;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class SystemServices : ISystemServices
    {
        private readonly ILogger<SystemServices> _logger;
        private readonly ApplicationDbContext _context;
        public SystemServices(ILogger<SystemServices> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // public async Task Log(string tableName, string action, object dataBefore, object dataAfter, User userHandle)
        // {
        //     var handle = new ActionDetail
        //     {
        //         HandleTable = tableName,
        //         Action = action,
        //         DataBefore = JsonSerializer.Serialize(dataBefore),
        //         DataAfter = JsonSerializer.Serialize(dataAfter),
        //         HandleBy = userHandle,
        //         UserHandle = userHandle.FullName,
        //         HandleAt = DateTime.Now
        //     };
        //     await _context.ActionDetails.AddAsync(handle);
        //     await _context.SaveChangesAsync();

        //     _logger.LogInformation("Logged action: {Action} on table {TableName} by {user}", action, tableName, userHandle.FullName);
        // }

        public async Task<AuditLog> CreateLog(string action, object dataBefore, object dataAfter, User userHandle, string searchKeyword)
        {
            var auditLog = new AuditLog()
            {
                UserId = userHandle.Id,
                UserName = userHandle.FullName,
                Action = action,
                DataBefore = dataBefore,
                DataAfter = dataAfter,
                SearchKeyword = searchKeyword,
                TimeStamp = DateTime.Now
            };
            return auditLog;
        }
    }
}