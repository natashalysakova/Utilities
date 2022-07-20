using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataModel;

namespace Utilities.Factory
{
    internal class ObjectFactory
    {
        private readonly UtilityDataModel model;

        public ObjectFactory(UtilityDataModel model)
        {
            this.model = model;
        }

        public Check CreateCheck()
        {
            int id = model.Checks.Any() ? model.Checks.Max(x => x.Id) + 1 : 0;
            return new Check(id, DateTime.Now);
        }

        public Tariff CreateTarif()
        {
            int id = model.Tarifs.Any() ? model.Tarifs.Max(x => x.Id) + 1 : 0;
            return new Tariff(id);
        }

        public UtilityType CreateUilityType()
        {
            int id = model.UtilityTypes.Any() ?  model.UtilityTypes.Max(x => x.Id) + 1 : 0;
            return new UtilityType(id);
        }
    }
}
