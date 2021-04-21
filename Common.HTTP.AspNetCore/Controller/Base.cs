using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Raique.Common.HTTP.Hooks;
using Raique.Microservices.Authenticate.Protocols;
using System.Threading.Tasks;

namespace Raique.Common.HTTP.AspNetCore.Controller
{
    [AppActionFilter]
    public abstract class Base : ControllerBase, IActionController
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        protected Base(ITokenRepository tokenRepository, IUserRepository userRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public async Task DoActionExecuted(ActionExecutedContext context)
        {
            AfterAction _afterAction = new AfterAction(HttpAfterActionMessage.CreateFromContext(context));
            await _afterAction.Execute();
        }
        
        public async Task DoActionExecuting(ActionExecutingContext context)
        {
            BeforeAction beforeAction = new BeforeAction(
                HttpBeforeActionMessage.CreateFromContext(context),
                _tokenRepository,
                _userRepository);
            await beforeAction.Execute();
        }
    }
}
