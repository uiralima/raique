using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.Hooks
{
    public class AfterAction
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
                    string code = Raique.Library.StringUtilities.GenerateRandom(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                    if (_message.Exception is Raique.Core.Exceptions.IBusinessException)
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
            Log($"{LogIdentifierByStatusCode(statusCode)} - {_message.Exception.ToString()} ({code})");
            _message.ClearException();
        }

        private string LogIdentifierByStatusCode(HttpStatusCode statusCode) => 
            (statusCode == HttpStatusCode.BadRequest) ? 
            "Erro de Negócio" : (statusCode == HttpStatusCode.Forbidden) ? 
            "Erro de permissão" : "Erro Interno";

        private void Log(string message)
        {
            
        }
    }
}
