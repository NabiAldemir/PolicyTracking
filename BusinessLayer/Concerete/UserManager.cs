using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PolicyTracking.Models;

namespace BusinessLayer.Concerete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<AppUser> Get(Expression<Func<AppUser, bool>> filter)
        {
            return _userDal.Get(filter);
        }

        public List<AppUser> GetList()
        {
            return _userDal.GetListAll();
        }

        public List<AppUser> GetListAll(Expression<Func<AppUser, bool>> filter)
        {
            return _userDal.GetListAll(filter);
        }

        public void TAdd(AppUser t)
        {
            _userDal.Insert(t);
        }

        public void TDelete(AppUser t)
        {
            _userDal.Delete(t);
        }

        public AppUser TGetById(int id)
        {
            return _userDal.GetById(id);
        }
        public void TUpdate(AppUser t)
        {
            _userDal.Update(t);
        }

        public Task<List<string>> GetUserRolesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UserUpdateDTO t)
        {
            return _userDal.UpdateUser(t);
        }

        public Task AddUser(UserSignUpViewModel p)
        {
            return _userDal.AddUser(p);
        }

        public Task DeleteUser(int id)
        {
            return _userDal.DeleteUser(id);
        }

        public Task<bool> AnyAsync(Expression<Func<AppUser, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _userDal.AnyAsync(predicate, cancellationToken);
        }

        public Task UpdateUserPassword(UserUpdateDTO model, Dictionary<string, object?> oldData, Dictionary<string, object?> newData)
        {
            return _userDal.UpdateUserPassword(model, oldData, newData);
        }
    }
}
