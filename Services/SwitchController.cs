using System.Net.Http;
using System.Web.Http;
using TAG.Modules.PersonaManager.Components;
using DotNetNuke.Common;
using DotNetNuke.Web.Api;
using DotNetNuke.Security;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Log.EventLog;
using DNNUserController = DotNetNuke.Entities.Users.UserController;
using System.Web;

namespace TAG.Modules.PersonaManager.Services
{
    [SupportedModules("PersonaManager")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
    public class SwitchController : DnnApiController
    {
        private readonly IPersonaRepository _repository;

        public SwitchController(IPersonaRepository repository)
        {
            Requires.NotNull(repository);

            this._repository = repository;
        }

        public SwitchController() : this(PersonaRepository.Instance) { }

        [HttpGet]
        public HttpResponseMessage Switch(int id)
        {
            ProcessLogin(id);
            return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
        }

        private void ProcessLogin(int newUserId)
        {
            var currentUser = DNNUserController.Instance.GetCurrentUserInfo();

            //Log event
            var objEventLog = new EventLogController();
            objEventLog.AddLog("Username", currentUser.Username, PortalSettings, currentUser.UserID, EventLogController.EventLogType.USER_IMPERSONATED);

            //Remove user from cache
            DataCache.ClearUserCache(PortalSettings.PortalId, currentUser.Username);

            var objPortalSecurity = new PortalSecurity();
            objPortalSecurity.SignOut();

            var newUser = DNNUserController.Instance.GetUser(PortalSettings.PortalId, newUserId);

            DNNUserController.UserLogin(newUser.PortalID, newUser, PortalSettings.PortalName, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], false);
        }


    }
}
