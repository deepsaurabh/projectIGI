using Data;
using Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;

namespace API
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

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {

            // Once Auth service integrated this condition must be removed from here.
            if (string.IsNullOrEmpty(userModel.FirstName) && string.IsNullOrEmpty(userModel.LastName))
            {
                userModel.FirstName = userModel.UserName;
                //userModel.LastName = "Lastname";
            }

            // Add generate username to the model
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityResult> RegisterUserAsync(string Username, string Password, string roleName = "Employer")
        {
            var addToRoleResult = new IdentityResult();
            // Add generate username to the model
            IdentityUser user = new IdentityUser
            {
                UserName = Username,
            };

            var result = await _userManager.CreateAsync(user, Password);

            //Role-user combination is managed as Contact-Role combination in ContactRole table
            //Create Role Admin if it does not exist
            //var _role = await _roleManager.FindByNameAsync(roleName);
            //if (_role == null)
            //{
            //    _role = new IdentityRole(roleName);
            //    var roleresult = await _roleManager.CreateAsync(_role);
            //}
            //var rolesForUser = await _userManager.GetRolesAsync(user.Id);
            //if (!rolesForUser.Contains(_role.Name))
            //{
            //    addToRoleResult = await _userManager.AddToRoleAsync(user.Id, _role.Name);
            //}

            return addToRoleResult;
        }

        public bool RegisterUser(string Username, string Password, string roleName = "Employer")
        {

            // Add generate username to the model
            IdentityUser user = new IdentityUser
            {
                UserName = Username,
            };

            var result = _userManager.Create(user, Password);

            //Create Role Admin if it does not exist
            //var _role = _roleManager.FindByName(roleName);
            //if (_role == null)
            //{
            //    _role = new IdentityRole(roleName);
            //    var roleresult = _roleManager.Create(_role);
            //}
            //var rolesForUser = _userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(_role.Name))
            //{
            //    var addToRoleResult = _userManager.AddToRole(user.Id, _role.Name);
            //}

            return result.Succeeded;
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


        public string GenerateEmailConfirmationToken(string userName)
        {
            string token = string.Empty;

            IdentityUser user = _userManager.FindByName(userName);

            token = _userManager.GenerateEmailConfirmationToken(user.Id);

            return token;
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


        public bool UpdateFirstTimeUser(string userName, string password)
        {
            string passwordChangeToken = string.Empty;
            IdentityUser isUserExists = _userManager.FindByName(userName);
            if (isUserExists != null)
            {
                var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("EPSemploymentPortal");

                if (provider != null)
                {
                    _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser, string>(provider.Create("PR"));
                }

                var code = _userManager.GeneratePasswordResetToken(isUserExists.Id);
                IdentityResult resultResetPassword = _userManager.ResetPassword(isUserExists.Id, code, password);
                return resultResetPassword.Succeeded;
            }
            else
                return false;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}