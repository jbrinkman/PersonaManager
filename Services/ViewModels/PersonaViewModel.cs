/*
' Copyright (c) 2016 Joe Brinkman
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using TAG.Modules.PersonaManager.Components;
using Newtonsoft.Json;

namespace TAG.Modules.PersonaManager.Services.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PersonaViewModel
    {
        public PersonaViewModel(Persona t)
        {
            Id = t.Id;
            Name = t.Name;
            Description = t.Description;
            AssignedUser = t.AssignedUserId;
        }

        public PersonaViewModel(Persona t, string editUrl)
        {
            Id = t.Id;
            Name = t.Name;
            Description = t.Description;
            AssignedUser = t.AssignedUserId;
            EditUrl = editUrl;
        }


        public PersonaViewModel() { }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignedUser")]
        public int AssignedUser { get; set; }

        [JsonProperty("editUrl")]
        public string EditUrl { get; }
    }
}