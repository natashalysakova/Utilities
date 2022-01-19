using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Data;

namespace Utilities
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
            var latest = checksWithUtility.OrderBy(x => x.Date).LastOrDefault()?.Records.SingleOrDefault(x => x.Tariff.Type.Name == type.Name);
            return latest is null ? 0 : latest.Meters;
        }
    }

    public class Check : ActiveItem, IComparable
    {
        private DateTime date;
        private decimal sum;

        public event EventHandler DateChanged;

        public Check()
        {

        }
        public Check(DateTime date)
        {
            Records = new ObservableCollection<Record>();
            _listCollectionView = CollectionViewSource.GetDefaultView(Records) as ListCollectionView;
            if (_listCollectionView != null)
            {
                _listCollectionView.IsLiveSorting = true;
                _listCollectionView.CustomSort = new
                        CaseInsensitiveComparer(CultureInfo.InvariantCulture);
            }

            Date = date;
        }

        public DateTime Date
        {
            get => date; set
            {
                date = value;
                DateChanged?.Invoke(this, new EventArgs());
            }
        }

        //internal void AddRecords(ObservableCollection<Record> records)
        //{
        //    foreach (var item in records)
        //    {
        //        Records.Add(item);
        //    }

        //    ReCalculate();
        //}

        public ObservableCollection<Record> Records { get; set; }
        [JsonIgnore]
        private ListCollectionView _listCollectionView;

        public decimal Sum { get => sum; set { sum = value; OnPropertyChanged(nameof(Sum)); } }


        public void ReCalculate()
        {
            Sum = Records.Sum(x => x.Cost);
        }

        //public int CompareTo(Check? other)
        //{
        //    if (other == null)
        //        return Date.CompareTo(other?.Date);
        //    else
        //        return -1;
        //}

        public int CompareTo(object? other)
        {
            if (other != null)
                return Date.CompareTo(((Check)other).Date);
            else
                return -1;
        }
    }

    public class Record : ActiveItem, IComparable
    {
        public decimal PreviousValue { get; set; }
        private decimal measure;
        private decimal meters;
        private decimal cost;

        public Record()
        {

        }
        public Record(Tariff tariff, decimal measure, decimal previousValue)
        {
            Tariff = tariff;
            Measure = measure;
            if (!tariff.Type.UseMesures)
                Measure = 1;

            PreviousValue = previousValue;

        }
        public Tariff Tariff { get; set; }
        public decimal Measure { get => measure; set { measure = value; ReCalculate(); } }
        public decimal Meters { get => meters; set { meters = value; ReCalculate(); } }

        public decimal Cost { get => cost; set { cost = value; OnPropertyChanged(nameof(Cost)); } }

        private void ReCalculate()
        {
            if (Tariff.Type.UseMeters)
            {
                measure = meters - PreviousValue;
                OnPropertyChanged(nameof(Measure));
            }

            Cost = Measure * Tariff.Cost;
        }

        public int CompareTo(object? other)
        {
            if (other != null)
                return Tariff.Type.Name.CompareTo(((Record)other).Tariff.Type.Name);
            else
                return -1;
        }

        internal void Update(Tariff item2)
        {
            Tariff.StartDate = item2.StartDate;
            Tariff.EndDate = item2.EndDate;
            Tariff.Cost = item2.Cost;
            Tariff.IsActive = item2.IsActive;
            Tariff.Update(item2.Type);

            ReCalculate();
        }
    }

    public class Tariff : IComparable
    {

        public Tariff() : this(0)
        {
        }

        public Tariff(int id)
        {
            IsActive = true;
            StartDate = DateTime.Now;
            Id = id;
        }
        public Tariff(UtilityType type, DateTime sartDate, DateTime? endDate, bool isActive, decimal cost)
        {
            Type = type;
            StartDate = sartDate;
            EndDate = endDate;
            IsActive = isActive;
            Cost = cost;
        }

        public int Id { get; set; }
        public UtilityType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; }

        public int CompareTo(object? other)
        {
            if (other == null)
                return -1;

            if (Type.Id == ((Tariff)other).Type.Id && IsActive == ((Tariff)other).IsActive)
                return 0;

            if (Type.Order.CompareTo(((Tariff)other).Type.Order) == 1 && IsActive.CompareTo(((Tariff)other).IsActive) == 1)
                return 1;
            if (Type.Order.CompareTo(((Tariff)other).Type.Order) == -1 && IsActive.CompareTo(((Tariff)other).IsActive) == -1)
                return -1;
            if (Type.Order.CompareTo(((Tariff)other).Type.Order) == 1 && IsActive.CompareTo(((Tariff)other).IsActive) == -1)
                return 1;
            if (Type.Order.CompareTo(((Tariff)other).Type.Order) == -1 && IsActive.CompareTo(((Tariff)other).IsActive) == 1)
                return -1;

            return 1;
        }

        public bool IsActiveAtDate(DateTime date)
        {
            return date > StartDate && date < EndDate;
        }

        internal void Update(UtilityType item2)
        {
            this.Type.Name = item2.Name;
            this.Type.UseMeters = item2.UseMeters;
            this.Type.Units = item2.Units;
            this.Type.Order = item2.Order;
            this.Type.UseMesures = item2.UseMesures;
        }
    }

    public class UtilityType : IComparable
    {
        [Obsolete("use only for design mode", true)]
        public UtilityType()
        {

        }

        public UtilityType(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Utility Type { get; set; }
        public string Units { get; set; }
        public int Order { get; set; }
        public bool UseMesures { get; set; }
        public bool UseMeters { get; set; }

        public int CompareTo(object? other)
        {
            if (other != null)
                return Order.CompareTo(((UtilityType)other).Order);
            else
                return -1;
        }
    }

    public enum Utility
    {
        HotWater = 0,
        ColdWater = 1,
        Heating = 2,
        Electricity = 3,
        TrashService = 4,
        Security = 5,
        Internet = 6,
        Other = 7
    }

}