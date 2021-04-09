using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Threading.Tasks;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class CreateApp
    {
        public static async Task<string> Execute(IAppRepository repository, string appName) =>
            await new CreateApp(repository, appName).Do();

        private readonly string _appName;
        private readonly IAppRepository _repository;
        private CreateApp(IAppRepository repository, string appName)
        {
            _appName = appName;
            _repository = repository;
            ValidateInstance();
        }
        private void ValidateInstance()
        {
            _repository.ThrowIfNull("repository");
            _appName.ThrowIfIsNullOrEmpty("appName");
        }

        private async Task<string> Do()
        {
            if (!(await _repository.GetByName(_appName)).IsValid())
            {
                string key = Guid.NewGuid().ToString();
                await _repository.Create(_appName, key);
                return key;
            }
            else
            {
                throw AppAlreadExistsException.Create(_appName);
            }
        }
    }
}
