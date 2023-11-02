using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace Utilities.DataModel
{
    [Serializable]
    public class UtilityDataModel
    {
        public event CheckAddedHandler? CheckAdded;
        public event CheckAddedHandler? CheckUpdated;

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
            check.Id = Checks.Max(x => x.Id) + 1;
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
                var rec = tmp.Single(x => x.Measure == tmp.Max(x => x.Measure));
                return rec is null ? 0 : rec.Meters;
            }

            return 0;
        }

        internal void AddTarifs(Tariff newItem)
        {
            newItem.Id = Tarifs.Max(x => x.Id) + 1;
            Tarifs.Add(newItem);
        }

        internal void UpdateCheck(Check check)
        {
            var original = Checks.SingleOrDefault(x => x.Id == check.Id);

            if (original == null)
            {
                return;
            }

            original.Date = check.Date;
            original.Sum = check.Sum;

            foreach (var item in original.Records)
            {
                var tmpRec = check.Records.SingleOrDefault(x => x.Tariff == item.Tariff);

                if (tmpRec == null) {
                    continue;
                }

                item.Cost = tmpRec.Cost;
                item.Measure = tmpRec.Measure;
                item.Meters = tmpRec.Meters;

            }

            CheckUpdated?.Invoke(check);
        }
    }
}