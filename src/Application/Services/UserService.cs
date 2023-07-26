using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepositorio;

        public UserService(IUserRepository usersRepositorio)
        {
            _usersRepositorio = usersRepositorio;
        }
        public async Task DeleteAsync(int Id)
        {
            await _usersRepositorio.DeleteAsync(Id);
        }

        public async Task<List<User>> GetAllAsync(int Page, int PageSize)
        {
            return await _usersRepositorio.GetAllAsync(Page, PageSize);
        }

        public async Task<User> GetGuidAsync(string guid)
        {
            return await _usersRepositorio.GetGuidAsync(guid);
        }

        public async Task<User> GetIdAsync(int id)
        {
            return await _usersRepositorio.GetIdAsync(id);
        }

        public async Task<User> InsertAsync(User entity)
        {
            return await _usersRepositorio.InsertAsync(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            await _usersRepositorio.UpdateAsync(entity);
        }

        public async Task<List<User>> WhereAsync(Expression<Func<User, bool>> expression)
        {
            return await _usersRepositorio.WhereAsync(expression);
        }
    }
}
