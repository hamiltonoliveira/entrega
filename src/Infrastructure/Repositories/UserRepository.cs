using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        protected readonly DataContext _db;
        public UserRepository(DataContext db)
        {
            _db = db;
        }
        public async Task DeleteAsync(int Id)
        {
            try
            {
                var user = await _db.User.FindAsync(Id);

                if (user != null)
                {
                    //user.SetDeletar();
                    _db.User.Update(user);
                    await _db.CommitAsync();
                }
            }
            finally
            {
                Dispose();
            }
        }

        public async Task<List<User>> GetAllAsync(int Page, int PageSize)
        {
            if (Page == 0)
                Page = 1;
            var retorna = (from cust in _db.User orderby cust.UserName select cust).Where(x => x.UserName != null)
            .Skip(Page - 1).Take(PageSize).ToListAsync();
            return retorna.Result;
        }

        public async Task<User> GetGuidAsync(string guid)
        {
            return await _db.User.FindAsync(guid);
        }

        public async Task<User> GetIdAsync(int id)
        {
            return await _db.User.FindAsync(id);
        }

        public async Task<User> InsertAsync(User entity)
        {
            try
            {
                var verificaUsers = _db.User.SingleOrDefault(x => x.UserName == entity.UserName);

                if (verificaUsers == null)
                {
                    _db.User.Add(entity);
                    await _db.CommitAsync();
                }
                return entity;
            }
            finally
            {
                //Dispose();
            }
        }
        public async Task UpdateAsync(User entity)
        {
            try
            {
                var recebe = _db.User.FindAsync(entity.Id);
                if (recebe != null)
                {
                    _db.Update(entity);
                    _db.Entry(entity).State = EntityState.Modified;
                    await _db.CommitAsync();
                }
            }
            finally { Dispose(); }
        }

        public async Task<List<User>> WhereAsync(Expression<Func<User, bool>> expression)
        {
            return await _db.Set<User>().Where(expression).ToListAsync();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
