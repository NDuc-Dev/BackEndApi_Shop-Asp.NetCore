using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public void Log(string tableName, string action, object dataBefore, object dataAfter, User userHandle)
        {
            var handle = new ActionDetail
            {
                HandleTable = tableName,
                Action = action,
                DataBefore = JsonConvert.SerializeObject(dataBefore),
                DataAfter = JsonConvert.SerializeObject(dataAfter),
                HandleBy = userHandle,
                UserHandle = userHandle.FullName,
                HandleAt = DateTime.Now
            };
            _context.ActionDetails.Add(handle);
            _context.SaveChanges();

            _logger.LogInformation("Logged action: {Action} on table {TableName} by {user}", action, tableName, userHandle.FullName);
        }
    }
}