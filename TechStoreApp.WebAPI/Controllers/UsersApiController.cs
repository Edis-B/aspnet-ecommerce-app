using CinemaApp.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TechStoreApp.Services.Data.Interfaces;
using TechStoreApp.Web.ViewModels.ApiViewModels.Users;

namespace TechStoreApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersApiController : ControllerBase
    {
        private const string action = "[action]";
        private readonly IProfileService profileService;
        public UsersApiController(IProfileService _profileService)
        {
            profileService = _profileService;
        }

        [AdminCookieOnly]
        [HttpGet(action)]
        [ProducesResponseType((typeof(IEnumerable<UserDetailsApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await profileService.GetAllUsersAsync();

            return Ok(users);
        }

        [AdminCookieOnly]
        [HttpPost(action)]
        [ProducesResponseType((typeof(IEnumerable<UserDetailsApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            if (userId == null || roleName == null)
            {
                return NotFound();
            }

            var users = await profileService.AssignRoleAssignRoleAsync(userId, roleName);

            if (users.Succeeded == false) 
            { 
                return BadRequest();
            }

            return Ok($"User with ID: {userId}, has been added to Role: {roleName}");
        }

        [AdminCookieOnly]
        [HttpPost(action)]
        [ProducesResponseType((typeof(IEnumerable<UserDetailsApiViewModel>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            if (userId == null || roleName == null)
            {
                return NotFound();
            }

            var users = await profileService.RemoveRoleRemoveRoleAsync(userId, roleName);

            if (users.Succeeded == false)
            {
                return BadRequest();
            }

            return Ok($"User with ID: {userId}, has been removed from Role: {roleName}");
        }
    }
}
