using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Presentation.Shared.Controllers;
using CoffeeShop.Presentation.Shared.Permissions;
using CoffeeShop.Service.Identity.UseCases.Groups.Commands;
using CoffeeShop.Service.Identity.UseCases.Groups.Queries;
using CoffeeShop.Shared.Attributes;

namespace CoffeeShop.Presentation.Identity.Controllers.v1
{
    [ApiVersion("1.0")]
    public class GroupController : BaseApiController
    {
        // POST api/<controller>
        [HttpPost]
        [HasPermissionFunctions(ApplicationPermissions.Group.CREATE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(UpsertGroupCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // POST api/<controller>
        [HttpDelete("{GroupId}")]
        [HasPermissionFunctions(ApplicationPermissions.Group.DELETE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid groupId)
        {
            return Ok(await Mediator.Send(new DeleteGroupCommand
            {
                Id = groupId,
            }));
        }

        // POST api/<controller>
        [HttpPost("clone")]
        [HasPermissionFunctions(ApplicationPermissions.Group.CREATE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CloneGroup(CloneGroupCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // POST api/<controller>
        [HttpPut]
        [HasPermissionFunctions(ApplicationPermissions.Group.UPDATE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(UpsertGroupCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // GET: api/<controller>
        [HttpGet]
        [HasPermissionFunctions(ApplicationPermissions.Group.VIEW)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<RolesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetGroupsQuery()));
        }

        // GET: api/<controller>/permissions
        [HttpGet("{GroupId}")]
        [HasPermissionFunctions(ApplicationPermissions.Group.VIEW)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<RolesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissions([FromRoute] Guid groupId)
        {
            return Ok(await Mediator.Send(new GetGroupByIdQuery
            {
                Id = groupId,
            }));
        }

        // GET: api/<controller>/permissions
        [HttpPost("permissions")]
        [HasPermissionFunctions(ApplicationPermissions.Group.UPDATE)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<RolesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GrantPermissions(GrantPermissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}