using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raique.Microservices.AuthenticateTests.Implementations
{
    class AppRepositoryMock : IAppRepository
    {
        public static Func<AppRepositoryMock> CreateRepository = () => new AppRepositoryMock();

        private static List<App> db = new List<App>();
        public int CreateCount { get; private set; }
        public int GetByKeyCount { get; private set; }
        public int GetByNameCount { get; private set; }

        AppRepositoryMock()
        {
            CreateCount = 0;
            GetByNameCount = 0;
            GetByKeyCount = 0;
        }

        public async Task<int> Create(string name, string key)
        {
            return await Task.Run(() =>
            {
                int id = db.Count + 2;
                db.Add(new App
                {
                    AppId = id,
                    Key = key,
                    Name = name
                });
                CreateCount++;
                return id;
            });
        }

        public async Task<App> GetByName(string appName)
        {
            return await Task.Run(() =>
            {
                GetByNameCount++;
                if (db.Any(f => f.Name == appName))
                {
                    return db.Where(f => f.Name == appName).First();
                }
                else
                {
                    return App.Invalid<App>();
                }
            });
        }

        public async Task<App> GetByKey(string key)
        {
            return await Task.Run(() =>
            {
                GetByKeyCount++;
                if (db.Any(f => f.Key == key))
                {
                    return db.Where(f => f.Key == key).First();
                }
                else if (key == "Chave")
                {
                    return new App
                    {
                        AppId = 1,
                        Key = "Chave",
                        Name = "Mock"
                    };
                }
                else
                {
                    return App.Invalid<App>();
                }
            });
        }
    }
}
