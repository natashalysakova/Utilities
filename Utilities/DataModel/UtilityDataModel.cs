using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.DataModel
{
    [Serializable]
    public class UtilityDataModel
    {
        public event CheckAddedHandler? CheckAdded;
        public delegate void CheckAddedHandler(Check check);

        public UtilityDataModel()
        {

            Tarifs = new List<Tariff>();
            Checks = new List<Check>();
            UtilityTypes = new List<UtilityType>();
        }
        public List<UtilityType> UtilityTypes { get; set; }
        public List<Tariff> Tarifs { get; set; }
        public List<Check> Checks { get; set; }

        public void AddCheck(Check check)
        {
            Checks.Add(check);
            CheckAdded?.Invoke(check);
        }

        internal decimal FindLatestRecordMetterForUtilityTypeBeforeDate(UtilityType type, DateTime date)
        {
            var checksWithUtility = new List<Check>();
            foreach (var check in Checks)
            {
                if (date < check.Date)
                    continue;

                foreach (var record in check.Records)
                {
                    if (record.Tariff.Type.Name == type.Name)
                    {
                        checksWithUtility.Add(check);
                        break;
                    }

                }
            }
            var latest = checksWithUtility.OrderBy(x => x.Date).LastOrDefault();
            if (latest != null)
            {
                var tmp = latest.Records.Where(x => x.Tariff.Type.Name == type.Name);
                var rec = tmp.Single(x => x.Measure == tmp.Max(x=>x.Measure));
                return rec is null ? 0 : rec.Meters;
            }

            return 0;
        }
    }
}