using System.Collections.Generic;
using Utilities.DataModel;

namespace Utilities
{
    public class AddTariffViewModel
    {
        public Tariff NewItem { get; set; }
        public IEnumerable<UtilityType> UtilityTypes { get; set; }


        public AddTariffViewModel(UtilityDataModel model)
        {
            NewItem = new Tariff();
            UtilityTypes = model.UtilityTypes;
        }
    }
}
