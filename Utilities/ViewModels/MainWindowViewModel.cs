using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Utilities.DataModel;

namespace Utilities
{
    class MainWindowViewModel
    {
        public ObservableCollection<Check> Checks { get; set; }
        private ListCollectionView _listCollectionView;
        public Check SelectedCheck { get; set; }

        public MainWindowViewModel()
        {
            Checks = new ObservableCollection<Check>();
            _listCollectionView = CollectionViewSource.GetDefaultView(Checks) as ListCollectionView;
            if (_listCollectionView != null)
            {
                _listCollectionView.IsLiveSorting = true;
                _listCollectionView.CustomSort = new
                        CaseInsensitiveComparer(CultureInfo.InvariantCulture);
            }
        }
    }
}
