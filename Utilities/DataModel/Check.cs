﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Data;

namespace Utilities.DataModel
{
    public class Check : ActiveItem, IComparable
    {
        private DateTime _date;
        private decimal sum;

        public event EventHandler DateChanged;

        [JsonConstructor]
        public Check(int id, DateTime date)
        {
            Records = new ObservableCollection<Record>();
            _listCollectionView = CollectionViewSource.GetDefaultView(Records) as ListCollectionView;
            if (_listCollectionView != null)
            {
                _listCollectionView.IsLiveSorting = true;
                _listCollectionView.CustomSort = new
                        CaseInsensitiveComparer(CultureInfo.InvariantCulture);
            }

            Id = id;
            Date = date;
        }

        public int Id { get; set; }

        public DateTime Date
        {
            get => _date; set
            {
                _date = value;
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
}