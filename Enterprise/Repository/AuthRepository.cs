using Data.Dto;
using Data.Enum;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;

namespace Enterprise.Repository
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        IdentityFactoryOptions<UserManager<IdentityUser>> options = new IdentityFactoryOptions<UserManager<IdentityUser>>();

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }


        public async Task<IdentityResult> RegisterCustomerAsync(UserModel userModel, AppRole appRole = AppRole.customer)
        {
            var addToRoleResult = new IdentityResult();
            // Add generate username to the model
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.EmailAddress,
                EmailConfirmed = true,
                PhoneNumber = userModel.PhoneNumber,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            //Role-user combination is managed as Contact-Role combination in ContactRole table
            //Create Role Admin if it does not exist
            var _role = await _roleManager.FindByNameAsync(appRole.ToString());
            if (_role == null)
            {
                _role = new IdentityRole(appRole.ToString());
                var roleresult = await _roleManager.CreateAsync(_role);
            }
            var rolesForUser = await _userManager.GetRolesAsync(user.Id);
            if (!rolesForUser.Contains(_role.Name))
            {
                addToRoleResult = await _userManager.AddToRoleAsync(user.Id, _role.Name);
            }

            return addToRoleResult;
        }

        public bool IsUserExistsByUsername(string Username)
        {
            // if gerenated username exists in the db then generate UNIQUE Username
            IdentityUser isUserExists = _userManager.FindByName(Username);
            if (isUserExists != null)
                return true;
            else
                return false;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityRole> FindRole(string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);

            return role;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}