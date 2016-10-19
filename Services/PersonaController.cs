using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TAG.Modules.PersonaManager.Components;
using TAG.Modules.PersonaManager.Services.ViewModels;
using DotNetNuke.Common;
using DotNetNuke.Web.Api;
using DotNetNuke.Security;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;
using DNNUserController = DotNetNuke.Entities.Users.UserController;
using DotNetNuke.Entities.Modules;

namespace TAG.Modules.PersonaManager.Services
{
    [SupportedModules("PersonaManager")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
    public class PersonaController : DnnApiController
    {
        private readonly IPersonaRepository _repository;

        public PersonaController(IPersonaRepository repository)
        {
            Requires.NotNull(repository);

            this._repository = repository;
        }

        public PersonaController() : this(PersonaRepository.Instance) { }

        public HttpResponseMessage Delete(int Id)
        {
            var persona = _repository.GetPersona(Id, ActiveModule.ModuleID);

            _repository.DeletePersona(persona);

            return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Get(int Id)
        {
            var persona = new PersonaViewModel(_repository.GetPersona(Id, ActiveModule.ModuleID));

            return Request.CreateResponse(persona);
        }

        public HttpResponseMessage GetList()
        {
            var personaList = new PersonaListViewModel(GetEditUrl(-1), Globals.IsEditMode());

            List<PersonaViewModel> personas;

            if (Globals.IsEditMode())
            {
                personas = _repository.GetPersonas(ActiveModule.ModuleID)
                       .Select(persona => new PersonaViewModel(persona, GetEditUrl(persona.Id)))
                       .ToList();
            }
            else
            {
                personas = _repository.GetPersonas(ActiveModule.ModuleID)
                       .Select(persona => new PersonaViewModel(persona, ""))
                       .ToList();
            }
            personaList.Personas = personas;

            return Request.CreateResponse(personaList);
        }

        protected string GetEditUrl(int id)
        {
            //javascript: dnnModal.show('http://evoq-demo.dnndev.me/Test-Page/ctl/Edit/mid/805?popUp=true',/*showReturn*/false, 550, 950, true, '')

            string editUrl = id > 0 ? 
                Globals.NavigateURL("Edit", string.Format("mid={0}", ActiveModule.ModuleID), string.Format("tid={0}", id)) :
                Globals.NavigateURL("Edit", string.Format("mid={0}", ActiveModule.ModuleID));

            if (PortalSettings.EnablePopUps)
            {
                editUrl = UrlUtils.PopUpUrl(editUrl, PortalSettings, false, false, 550, 950);
            }
            return editUrl;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Upsert(PersonaViewModel persona)
        {
            if (persona.Id > 0)
            {
                var t = Update(persona);
                return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
            }
            else
            {
                var t = Create(persona);
                return Request.CreateResponse(t.Id);
            }

        }

        private Persona Create(PersonaViewModel persona)
        {
            Persona t = new Persona
            {
                Name = persona.Name,
                Description = persona.Description,
                AssignedUserId = persona.AssignedUser,
                ModuleId = ActiveModule.ModuleID,
                CreatedByUserId = UserInfo.UserID,
                LastModifiedByUserId = UserInfo.UserID,
                CreatedOnDate = DateTime.UtcNow,
                LastModifiedOnDate = DateTime.UtcNow
            };
            _repository.AddPersona(t);

            return t;
        }

        private Persona Update(PersonaViewModel persona)
        {

            var t = _repository.GetPersona(persona.Id, ActiveModule.ModuleID);
            if (t != null)
            {
                t.Name = persona.Name;
                t.Description = persona.Description;
                t.AssignedUserId = persona.AssignedUser;
                t.LastModifiedByUserId = UserInfo.UserID;
                t.LastModifiedOnDate = DateTime.UtcNow;
            }
            _repository.UpdatePersona(t);

            return t;
        }

    }
}
