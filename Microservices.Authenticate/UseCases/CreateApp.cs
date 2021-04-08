using Raique.Library;
using Raique.Microservices.Authenticate.Exceptions;
using Raique.Microservices.Authenticate.Protocols;
using System;

namespace Raique.Microservices.Authenticate.UseCases
{
    public class CreateApp
    {
        public static string Execute(IAppRepository repository, string appName) =>
            new CreateApp(repository, appName).Do();

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

        private string Do()
        {
            if (!_repository.GetByName(_appName).IsValid())
            {
                string key = Guid.NewGuid().ToString();
                _repository.Create(_appName, key);
                return key;
            }
            else
            {
                throw AppAlreadExistsException.Create(_appName);
            }
        }
    }
}
