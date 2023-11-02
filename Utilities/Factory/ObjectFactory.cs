using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

        public Check CreateCheck(Check check)
        {
            if(check == null)
                return CreateCheck();

            var newCheck =  new Check(check.Id, DateTime.Now) { Records = new System.Collections.ObjectModel.ObservableCollection<Record>(), Sum = check.Sum };
            foreach (var item in check.Records)
            {
                newCheck.Records.Add(
                    new Record(item.Tariff, item.Measure, item.PreviousValue)
                    {
                        Cost = item.Cost,
                        Meters = item.Meters,
                        PreviousValue = item.PreviousValue,
                    });
            }

            return newCheck;
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
