using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ControllerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        internal void ValidateUser(string userId)
        {
            if (userId != ((User)_httpContextAccessor.HttpContext.Items["User"]).Id)
            {
                throw new DomainServicesException("Current user has no permissions to do this action.");
            }
        }

        internal User GetCurrentUser()
        {
            return (User)_httpContextAccessor.HttpContext.Items["User"];
        }
    }
}