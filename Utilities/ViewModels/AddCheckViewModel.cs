using System;
using System.Linq;
using Utilities.DataModel;

namespace Utilities
{
    public class AddCheckViewModel : IDisposable
    {
        private readonly UtilityDataModel model;

        public AddCheckViewModel(UtilityDataModel model)
        {
            //Records = new ObservableCollection<Record>();
            NewCheck = new Check(DateTime.Now);
            NewCheck.DateChanged += NewCheck_DateChanged;
            this.model = model;

            FillRecords();
        }

        private void NewCheck_DateChanged(object? sender, EventArgs e)
        {
            FillRecords();
        }

        private void FillRecords()
        {
            //Records.Clear();
            NewCheck.Records.Clear();
            var date = NewCheck.Date;
            var tarifs = model.Tarifs.Where(x => (date >= x.StartDate && x.EndDate is null) || (date >= x.StartDate && date <x.EndDate)).OrderByDescending(x=>x.Type.Order);
            foreach (var tarif in tarifs)
                NewCheck.Records.Add(new Record(tarif, 0, model.FindLatestRecordMetterForUtilityTypeBeforeDate(tarif.Type, date)));
        }

        public void Dispose()
        {
            NewCheck.DateChanged -= NewCheck_DateChanged;
        }

        public Check NewCheck { get; set; }
        //public ObservableCollection<Record> Records { get; set; }
    }
}
