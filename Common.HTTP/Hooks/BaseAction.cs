using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Raique.Common.HTTP.Hooks
{
    public abstract class BaseAction
    {
        protected void LogException(HttpStatusCode statusCode, string code, Exception exception)
        {
            
            string detail = (exception is Core.Exceptions.IDetailedException) ?
                (exception as Core.Exceptions.IDetailedException).GetDetail() : string.Empty;
            Log($"{LogIdentifierByStatusCode(statusCode)} - Detalhe: {detail} - {exception} ({code})");
            
        }
        protected string GenerateLogCode() => Library.StringUtilities.GenerateRandom(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
        protected void Log(string message) => 
            Common.Log.FileLog.InternalLog.Log(message);
        private string LogIdentifierByStatusCode(HttpStatusCode statusCode) =>
            (statusCode == HttpStatusCode.BadRequest) ?
            "Erro de Negócio" : (statusCode == HttpStatusCode.Forbidden) ?
            "Erro de permissão" : "Erro Interno";
    }
}
