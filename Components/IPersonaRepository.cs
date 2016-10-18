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

using System.Linq;
using DotNetNuke.Collections;

namespace TAG.Modules.PersonaManager.Components
{
    public interface IPersonaRepository
    {

        int AddPersona(Persona t);

        void DeletePersona(int personaId, int moduleId);

        void DeletePersona(Persona t);

        Persona GetPersona(int personaId, int moduleId);

        IQueryable<Persona> GetPersonas(int moduleId);

        void UpdatePersona(Persona t);
    }
}