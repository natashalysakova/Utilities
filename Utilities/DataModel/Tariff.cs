using System;

namespace Utilities.DataModel
{
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
}