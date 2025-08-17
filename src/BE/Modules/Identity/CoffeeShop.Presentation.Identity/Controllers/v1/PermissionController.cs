using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Presentation.Shared.Controllers;
using CoffeeShop.Service.Identity.UseCases.Permissions.Queries;
using CoffeeShop.Shared.Auth;

namespace CoffeeShop.Presentation.Identity.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PermissionController : BaseApiController
    {
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public PermissionController(ISecurityContextAccessor securityContextAccessor)
        {
            _securityContextAccessor = securityContextAccessor;
        }

        // GET: api/<controller>
        [HttpGet]
        //[HasPermissionFunctions(IdentityPermissions.Account.VIEW_LIST)]
        [ProducesResponseType(typeof(Response<IReadOnlyList<PermissionsResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetPermissionsByUserIdQuery
            {
                UserId = _securityContextAccessor.UserId,
            }));
        }
    }
}