using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using TAG.Modules.PersonaManager.Services.ViewModels;
using DotNetNuke.Web.Api;
using DotNetNuke.Security;
using DotNetNuke.Entities.Users;
using DNNUserController = DotNetNuke.Entities.Users.UserController;

namespace TAG.Modules.PersonaManager.Services
{
    [SupportedModules("PersonaManager")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    public class UserController : DnnApiController
    {
        public UserController() { }

        public HttpResponseMessage GetList()
        {
            var userList = DNNUserController.GetUsers(this.PortalSettings.PortalId);
            var hostUserList = DNNUserController.GetUsers(false, true, -1);

            userList.AddRange(hostUserList);

            var users = userList.Cast<UserInfo>().ToList()
                   .Select(user => new UserViewModel(user))
                   .OrderBy(user => user.Name)
                   .ToList();

            return Request.CreateResponse(users);
        }
    }
}
