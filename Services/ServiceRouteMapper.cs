
using DotNetNuke.Web.Api;
using System.Web.Http;

namespace TAG.Modules.PersonaManager.Services
{

    /// <summary>
    /// The ServiceRouteMapper tells the DNN Web API Framework what routes this module uses
    /// </summary>
    public class ServiceRouteMapper : IServiceRouteMapper
    {
        /// <summary>
        /// RegisterRoutes is used to register the module's routes
        /// </summary>
        /// <param name="mapRouteManager"></param>
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "PersonaManager",
                routeName: "default",
                url: "{controller}/{Id}",
                defaults: new { Id = RouteParameter.Optional },
                namespaces: new[] { "TAG.Modules.PersonaManager.Services" });
        }
    }
}