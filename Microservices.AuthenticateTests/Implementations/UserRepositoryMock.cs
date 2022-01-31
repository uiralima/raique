using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using Raique.Microservices.Authenticate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.Implementations
{
    public class UserRepositoryMock : IUserRepository
    {
        private static List<User> db = new List<User>();

        public static Func<UserRepositoryMock> CreateRepository = () => new UserRepositoryMock();
        public int CreateCount { get; private set; }
        public int GetByKeyToAppCount { get; private set; }
        public int GetByIdCount { get; private set; }
        public int ChangePasswordCount { get; private set; }
        public int ChangePasswordByCodeCount { get; private set; }
        

        private UserRepositoryMock()
        {
            CreateCount = 0;
            GetByKeyToAppCount = 0;
            GetByIdCount = 0;
            ChangePasswordCount = 0;
            ChangePasswordByCodeCount = 0;
        }

        public async Task<int> Create(User user)
        {
            return await Task.Run(() =>
            {
                CreateCount++;
                user.UserId = db.Count + 2;
                db.Add(user);
                return user.UserId;
            });
        }

        public async Task<User> GetByKeyToApp(string key, string appKey)
        {
            return await Task.Run(() =>
            {
                GetByKeyToAppCount++;
                if (db.Any(f => f.Key == key && f.AppKey == appKey))
                {
                    return db.Where(f => f.Key == key && f.AppKey == appKey).First();
                }
                else if (key == "USUARIO" && appKey == "CHAVE")
                {
                    return new User
                    {
                        AppKey = "CHAVE",
                        Key = "USUARIO",
                        CheckKey = "CHAVE_DE_SEGURANÇA",
                        Password = PasswordCreator.Create("SENHA", "CHAVE_DE_SEGURANÇA"),
                        UserId = 1
                    };
                }
                else
                {
                    return User.Invalid<User>();
                }
            });
        }

        public async Task<User> GetById(int userId)
        {
            return await Task.Run(() =>
            {
                GetByIdCount++;
                if (db.Any(f => f.UserId == userId))
                {
                    return db.Where(f => f.UserId == userId).First();
                }
                else if (userId == 1)
                {
                    return new User
                    {
                        AppKey = "CHAVE",
                        Key = "USUARIO",
                        CheckKey = "CHAVE_DE_SEGURANÇA",
                        Password = PasswordCreator.Create("SENHA", "CHAVE_DE_SEGURANÇA"),
                        UserId = 1
                    };
                }
                else
                {
                    return User.Invalid<User>();
                }
            });
        }

        public async Task ChangePassword(int userId, string password)
        {
            await Task.Run(() =>
            {
                ChangePasswordCount++;
            });
        }

        public async Task<bool> ChangePasswordByCode(string userName, string code, string password)
        {
            return await Task.Run(() =>
            {
                ChangePasswordByCodeCount++;
                return true;
            });
        }
    }
}
