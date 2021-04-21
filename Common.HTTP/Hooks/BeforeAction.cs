using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.Hooks
{
    public class BeforeAction
    {
        private readonly IBeforeActionMessage _message;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public BeforeAction(IBeforeActionMessage message, ITokenRepository tokenRepository, IUserRepository userRepository)
        {
            _message = message;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task Execute()
        {
            try
            {
                Device = ReadHeader(DeviceHeader);
                if (String.IsNullOrWhiteSpace(Device))
                {
                    SendBadRequestResponse(Properties.Resources.UninformedDeviceMessage);
                }
                else
                {
                    App = ReadHeader(AppHeader);
                    if (String.IsNullOrWhiteSpace(App))
                    {
                        SendBadRequestResponse(Properties.Resources.UninformedAppMessage);
                    }
                    else
                    {
                        CurrentUser = null;
                        string token = ReadHeader(TokenHeader);
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            var userId = await _tokenRepository.GetUserIdByToken(Device, token);
                            CurrentUser = await _userRepository.GetById(userId);
                            CurrentUser.ClearSecurityInfo();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SendInternalServerErrorResponse(ex);
            }
        }

        protected virtual string ReadHeader(string headerName)
        {
            return _message.Headers.Any(f => f.Key == headerName) ?
                _message.Headers.Where(f => f.Key == headerName).First().Value.First() : String.Empty;
        }

        protected string Device { get; private set; }

        protected string App { get; private set; }

        protected virtual string DeviceHeader => "rqe_device";

        protected virtual string AppHeader => "rqe_app";

        protected virtual string TokenHeader => "rqe_token";

        protected virtual void SendBadRequestResponse(string message)
        {
            _message.CreateResponse(HttpStatusCode.BadRequest, message);
        }
        protected virtual void SendInternalServerErrorResponse(Exception ex)
        {
            _message.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }

        protected User CurrentUser { get; private set; }
    }
}
