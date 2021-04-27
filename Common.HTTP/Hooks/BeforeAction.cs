using Raique.Common.Controller;
using Raique.Common.HTTP.Hooks.Exceptions;
using Raique.Core.Exceptions;
using Raique.Library;
using Raique.Microservices.Authenticate.Domain;
using Raique.Microservices.Authenticate.Protocols;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.Hooks
{
    public class BeforeAction : BaseAction
    {
        private readonly IBeforeActionMessage _message;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IController _controller;

        public BeforeAction(IController controller, IBeforeActionMessage message, ITokenRepository tokenRepository, IUserRepository userRepository)
        {
            _controller = controller;
            _message = message;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task Execute()
        {
            try
            {
                ReadDevice();
                ReadApp();
                await ReadUser();
            }
            catch (Exception ex)
            {
                LogException(HttpStatusCode.InternalServerError, GenerateLogCode(), ex);
                CreateResponseFromException(ex);
            }
        }

        private void ReadDevice()
        {
            _controller.Device = ReadHeader(DeviceHeader);
            UninformedDeviceException.Creator.ThrowIfTrue((_controller.DeviceRequired) && (String.IsNullOrWhiteSpace(_controller.Device)));
        }

        private void ReadApp()
        {
            _controller.AppKey = ReadHeader(AppHeader);
            UninformedAppException.Creator.ThrowIfTrue((_controller.AppRequired) && (String.IsNullOrWhiteSpace(_controller.AppKey)));
        }

        private async Task ReadUser()
        {
            _controller.CurrentUser = CreateInvalidUser();
            string token = ReadHeader(TokenHeader);
            if (!string.IsNullOrWhiteSpace(token))
            {
                _controller.Token = token;
                await ReadUserByTokenFromRepository(token);
            }
            InvalidUserException.Creator.ThrowIfTrue((_controller.UserRequired) && (!_controller.CurrentUser.IsValid()));
        }

        private async Task ReadUserByTokenFromRepository(string token)
        {
            var userId = await _tokenRepository.GetUserIdByToken(_controller.Device, token);
            if (userId > 0)
            {
                _controller.CurrentUser = await _userRepository.GetById(userId);
                _controller.CurrentUser.ClearSecurityInfo();
            }
        }

        private User CreateInvalidUser() => User.Invalid<User>();

        private void CreateResponseFromException(Exception exception)
        {
            if (exception is IBusinessException)
            {
                SendBadRequestResponse(exception.Message);
            }
            else
            {
                SendInternalServerErrorResponse(exception);
            }
        }

        protected virtual string ReadHeader(string headerName)
        {
            return _message.Headers.Any(f => f.Key == headerName) ?
                _message.Headers.Where(f => f.Key == headerName).First().Value.First() : String.Empty;
        }

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
    }
}
