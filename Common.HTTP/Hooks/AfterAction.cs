using System.Net;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.Hooks
{
    public class AfterAction : BaseAction
    {
        private readonly IAfterActionMessage _message;

        public AfterAction(IAfterActionMessage message)
        {
            _message = message;
        }

        public async Task Execute()
        {
            await Task.Run(() =>
            {
                if (_message.Exception != null)
                {
                    string code = GenerateLogCode();
                    if (_message.Exception is Core.Exceptions.IBusinessException)
                    {
                        CreateReponseAndClearException(HttpStatusCode.BadRequest, code);
                    }
                    else if (_message.Exception is Core.Exceptions.IPermissionException)
                    {
                        CreateReponseAndClearException(HttpStatusCode.Forbidden, code);
                    }
                    else
                    {
                        CreateReponseAndClearException(HttpStatusCode.InternalServerError, code);
                    }
                }
            });
        }

        private void CreateReponseAndClearException(HttpStatusCode statusCode, string code)
        {
            _message.CreateResponse(statusCode, $"{_message.Exception.Message} ({code})");
            LogException(statusCode, code, _message.Exception);
            _message.ClearException();
        }

        

        
    }
}
