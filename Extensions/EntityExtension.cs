using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;

namespace WebIdentityApi.Extensions
{
    public static class EntityExtension
    {
        public static async Task<bool> IsExistsAsync<T>(this ApplicationDbContext context, string key, object value) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equalExpression = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            return await context.Set<T>().AnyAsync(lambda);
        }

    }
}