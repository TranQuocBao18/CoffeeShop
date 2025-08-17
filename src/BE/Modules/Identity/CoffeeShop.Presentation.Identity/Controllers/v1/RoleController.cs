using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Presentation.Shared.Controllers;
using CoffeeShop.Presentation.Shared.Permissions;
using CoffeeShop.Service.Identity.UseCases.Roles.Queries;
using CoffeeShop.Shared.Attributes;

namespace CoffeeShop.Presentation.Identity.Controllers.v1
{
    [ApiVersion("1.0")]
    public class RoleController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [HasPermissionFunctions(ApplicationPermissions.Role.VIEW)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<RolesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetRolesQuery()));
        }
    }
}