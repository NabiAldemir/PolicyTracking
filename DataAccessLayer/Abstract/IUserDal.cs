using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using PolicyTracking.Models;

namespace DataAccessLayer.Abstract
{
    public interface IUserDal:IGenericDal<AppUser>
    {
        public Task DeleteUser(int id);
        public Task AddUser(UserSignUpViewModel p);
        public Task UpdateUser(UserUpdateDTO t);
        Task<bool> AnyAsync(Expression<Func<AppUser , bool>> predicate, CancellationToken cancellationToken = default);
        public Task UpdateUserPassword(UserUpdateDTO model, Dictionary<string, object?> oldData, Dictionary<string, object?> newData);
    }
}
