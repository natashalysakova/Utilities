using System;

namespace Utilities.DataModel
{
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
}