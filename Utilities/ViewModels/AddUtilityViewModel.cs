using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.DataModel;
using Utilities.Factory;

namespace Utilities
{
    public class AddUtilityViewModel
    {
        public IEnumerable<string> UtilitiyTypes { get; set; }
        public UtilityType NewItem { get; set; }

        public AddUtilityViewModel(UtilityDataModel model)
        {
            UtilitiyTypes = Enum.GetValues(typeof(Utility)).OfType<Utility>().Select(x => x.ToString()).ToList();
            NewItem = new ObjectFactory(model).CreateUilityType();
        }
    }
}
