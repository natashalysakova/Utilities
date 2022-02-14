using System;

namespace Utilities.DataModel
{
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
}