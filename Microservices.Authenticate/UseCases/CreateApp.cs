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
        private readonly Func<Exception> AppAlreadExistsExceptionCreator;
        private CreateApp(IAppRepository repository, string appName)
        {
            _appName = appName;
            _repository = repository;
            ValidateInstance();
            AppAlreadExistsExceptionCreator = () => AppAlreadExistsException.Create(_appName);
        }
        private void ValidateInstance()
        {
            _repository.ThrowIfNull("repository");
            _appName.ThrowIfIsNullOrEmpty("appName");
        }
        private async Task<string> Do()
        {
            AppAlreadExistsExceptionCreator.ThrowIfTrue(await AlreadExistsAppWithAppName());
            string key = CreateRandomKey();
            await _repository.Create(_appName, key);
            return key;
        }

        private async Task<bool> AlreadExistsAppWithAppName() => (await _repository.GetByName(_appName)).IsValid();
        private string CreateRandomKey() => Guid.NewGuid().ToString();
    }
}
