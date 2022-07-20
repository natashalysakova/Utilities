using System.Collections.Generic;
using Utilities.DataModel;
using Utilities.Factory;

namespace Utilities
{
    public class AddTariffViewModel
    {
        public Tariff NewItem { get; set; }
        public IEnumerable<UtilityType> UtilityTypes { get; set; }


        public AddTariffViewModel(UtilityDataModel model)
        {
            NewItem = new ObjectFactory(model).CreateTarif();
            UtilityTypes = model.UtilityTypes;
        }
    }
}
