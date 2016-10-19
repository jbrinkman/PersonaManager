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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Collections;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;

namespace TAG.Modules.PersonaManager.Components
{
    public class PersonaRepository : ServiceLocator<IPersonaRepository, PersonaRepository>, IPersonaRepository
    {

        protected override Func<IPersonaRepository> GetFactory()
        {
            return () => new PersonaRepository();
        }

        public int AddPersona(Persona t)
        {
            Requires.NotNull(t);
            Requires.PropertyNotNegative(t, "ModuleId");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Persona>();
                rep.Insert(t);
            }
            return t.Id;
        }

        public void DeletePersona(int personaId, int moduleId)
        {
            Requires.NotNegative("personaId", personaId);
            Requires.NotNegative("moduleId", moduleId);

            var t = GetPersona(personaId, moduleId);
            DeletePersona(t);
        }

        public void DeletePersona(Persona t)
        {
            Requires.NotNull(t);
            Requires.PropertyNotNegative(t, "Id");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Persona>();
                rep.Delete(t);
            }
        }

        public Persona GetPersona(int personaId, int moduleId)
        {
            Requires.NotNegative("personaId", personaId);
            Requires.NotNegative("moduleId", moduleId);

            Persona t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Persona>();
                t = rep.GetById(personaId, moduleId);
            }
            return t;
        }

        public IQueryable<Persona> GetPersonas(int moduleId)
        {
            Requires.NotNegative("moduleId", moduleId);

            IQueryable<Persona> t = null;

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Persona>();

                t = rep.Get(moduleId).AsQueryable();
            }

            return t;
        }

        public void UpdatePersona(Persona t)
        {
            Requires.NotNull(t);
            Requires.PropertyNotNegative(t, "Id");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Persona>();
                rep.Update(t);
            }
        }
    }
}