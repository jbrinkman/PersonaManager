using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAG.Modules.PersonaManager.Services.ViewModels
{
    public class PersonaListViewModel
    {
        public PersonaListViewModel(string addUrl, bool editMode)
        {
            AddUrl = addUrl;
            EditMode = editMode;
        }

        [JsonProperty("personas")]
        public List<PersonaViewModel> Personas { get; set; }

        [JsonProperty("addUrl")]
        public string AddUrl { get; set; }

        [JsonProperty("editMode")]
        public bool EditMode { get; set; }


    }
}