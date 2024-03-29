﻿using System;
using System.Linq;
using Utilities.DataModel;
using Utilities.Factory;

namespace Utilities
{
    public class AddCheckViewModel : IDisposable
    {
        private readonly UtilityDataModel model;
        public Mode Mode { get;  }

        public AddCheckViewModel(UtilityDataModel model, Check check)
        {
            Mode = check is null ? Mode.Add : Mode.Edit;
            //NewCheck = check is null  ?  new ObjectFactory(model).CreateCheck() : check;
            NewCheck = new ObjectFactory(model).CreateCheck(check);
            this.model = model;
            NewCheck.DateChanged += NewCheck_DateChanged;
            FillRecords();
        }

        private void NewCheck_DateChanged(object? sender, EventArgs e)
        {
            FillRecords();
        }

        private void FillRecords()
        {
            //Records.Clear();
            if(Mode == Mode.Add)
            {
                NewCheck.Records.Clear();
                var date = NewCheck.Date;
                var tarifs = model.Tarifs.Where(x => (date >= x.StartDate && x.EndDate is null) || (date >= x.StartDate && date < x.EndDate)).OrderByDescending(x => x.Type.Order);
                foreach (var tarif in tarifs)
                    NewCheck.Records.Add(new Record(tarif, 0, model.FindLatestRecordMetterForUtilityTypeBeforeDate(tarif.Type, date)));
            }
        }

        public void Dispose()
        {
            NewCheck.DateChanged -= NewCheck_DateChanged;
        }

        public Check NewCheck { get; set; }
        public string WindowTitle { get { return Mode == Mode.Add ? "Добавить счёт" : "Редактировать счёт"; } }
    }

    public enum Mode
    {
        Add, Edit
    }
}
