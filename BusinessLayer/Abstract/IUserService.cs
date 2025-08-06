using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using PolicyTracking.Models;

namespace BusinessLayer.Abstract
{
    public interface IUserService:IGenericService<AppUser>
    {
        Task<List<string>> GetUserRolesAsync(ClaimsPrincipal user);
        public Task UpdateUser(UserUpdateDTO t);
        public Task AddUser(UserSignUpViewModel p);
        public Task DeleteUser(int id);
        Task<bool> AnyAsync(Expression<Func<AppUser, bool>> predicate, CancellationToken cancellationToken = default);
        public Task UpdateUserPassword(UserUpdateDTO model, Dictionary<string, object?> oldData, Dictionary<string, object?> newData);
    }
}
