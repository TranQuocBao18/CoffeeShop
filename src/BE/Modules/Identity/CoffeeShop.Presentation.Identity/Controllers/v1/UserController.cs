using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure.Shared.Wrapper;
using CoffeeShop.Model.Dto.Identity.Responses;
using CoffeeShop.Presentation.Shared.Controllers;
using CoffeeShop.Presentation.Shared.Permissions;
using CoffeeShop.Service.Identity.UseCases.Identity.Queries;
using CoffeeShop.Service.Identity.UseCases.Permissions.Queries;
using CoffeeShop.Service.Identity.UseCases.Roles.Queries;
using CoffeeShop.Service.Identity.UseCases.Users.Commands;
using CoffeeShop.Service.Identity.UseCases.Users.Queries;
using CoffeeShop.Shared.Attributes;
using CoffeeShop.Shared.Auth;

namespace CoffeeShop.Presentation.Identity.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public UserController(ISecurityContextAccessor securityContextAccessor)
        {
            _securityContextAccessor = securityContextAccessor;
        }


        // GET api/<controller>/profile
        [HttpGet("profile")]
        [HasPermissionFunctions(ApplicationPermissions.User.VIEW)]
        [ProducesResponseType(typeof(Response<ProfileResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProfileAsync()
        {
            var userId = _securityContextAccessor.UserId;
            return Ok(await Mediator.Send(new GetProfileQuery() { Id = userId }));
        }

        // GET api/<controller>/roles
        [HttpGet("roles")]
        [HasPermissionFunctions(ApplicationPermissions.User.VIEW)]
        public async Task<IActionResult> RolesAsync()
        {
            var userId = _securityContextAccessor.UserId;
            return Ok(await Mediator.Send(new GetRolesByUserIdQuery() { UserId = userId }));
        }

        // GET api/<controller>/permissions
        [HttpGet("permissions")]
        [HasPermissionFunctions(ApplicationPermissions.User.CREATE)]
        [ProducesResponseType(typeof(Response<IReadOnlyList<PermissionsResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PermissionsAsync()
        {
            var userId = _securityContextAccessor.UserId;
            return Ok(await Mediator.Send(new GetPermissionsByUserIdQuery() { UserId = userId }));
        }

        // GET: api/<controller>
        [HttpGet]
        [HasPermissionFunctions(ApplicationPermissions.User.VIEW)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<UsersResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetUsersParameter filter)
        {
            return Ok(await Mediator.Send(new GetUsersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // POST api/<controller>/search
        [HttpPost("search")]
        [HasPermissionFunctions(ApplicationPermissions.User.VIEW)]
        [ProducesResponseType(typeof(PagedResponse<IReadOnlyList<UsersResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(SearchUsersParameter search)
        {
            return Ok(await Mediator.Send(new GetUsersQuery() { PageSize = search.PageSize, PageNumber = search.PageNumber, Search = search.Search }));
        }

        // GET api/<controller>/<id>
        [HttpGet("{id}")]
        [HasPermissionFunctions(ApplicationPermissions.User.VIEW)]
        [ProducesResponseType(typeof(Response<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [HasPermissionFunctions(ApplicationPermissions.User.CREATE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(UpsertUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/<id>
        [HttpPut()]
        [HasPermissionFunctions(ApplicationPermissions.User.UPDATE)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(UpsertUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/<id>
        [HttpDelete("{id}")]
        [HasPermissionFunctions(ApplicationPermissions.User.DELETE)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }

        // POST api/<controller>
        [HttpPost("resetpassword")]
        [HasPermissionFunctions(ApplicationPermissions.User.UPDATE)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // POST api/<controller>
        [HttpPost("changepassword")]
        [HasPermissionFunctions(ApplicationPermissions.User.UPDATE)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}