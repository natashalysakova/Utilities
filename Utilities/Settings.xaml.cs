using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Utilities.DataModel;

namespace Utilities
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly UtilityDataModel dataModel;
        private UtilityDataModel _newDataModel;

        private SettingsViewModel viewModel;
        public Settings(UtilityDataModel dataModel)
        {
            this.dataModel = dataModel;
            viewModel = new SettingsViewModel(this.dataModel);
            DataContext = viewModel;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUtilityType window = new AddUtilityType(dataModel);
            window.ShowDialog();
            AddNewUtilityTypeToViewModel();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddTariff window = new AddTariff(dataModel);
            window.ShowDialog();

            AddNewTariffToViewModel();
        }

        private void AddNewTariffToViewModel()
        {
            foreach (var item in dataModel.Tarifs)
            {
                if (!viewModel.Tariffs.Contains(item))
                {
                    viewModel.Tariffs.Add(item);
                }
            }
        }

        private void AddNewUtilityTypeToViewModel()
        {
            foreach (var item in dataModel.UtilityTypes)
            {
                if (!viewModel.UtilityTypes.Contains(item))
                {
                    viewModel.UtilityTypes.Add(item);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (_newDataModel != null)
                SaveDataModel(_newDataModel);
            else if (dataModel != null)
                SaveDataModel(dataModel);

            DialogResult = true;
            Close();
        }

        private void SaveDataModel(UtilityDataModel model)
        {
            foreach (var item in model.Tarifs)
            {
                foreach (var item2 in model.UtilityTypes)
                {
                    if (item.Type.Id == item2.Id)
                    {
                        item.Update(item2);
                        break;
                    }
                }
            }

            foreach (var item in model.Checks)
            {
                foreach (var item2 in model.Tarifs)
                {
                    foreach (var item3 in item.Records)
                    {
                        if (item3.Tariff.Id == item2.Id)
                        {
                            item3.Update(item2);
                            break;
                        }
                    }
                }
                item.ReCalculate();
            }

            FileManager.SaveDefault(model);

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var item = (UtilityType)(sender as Button).DataContext;
            if (item != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete utility type {item.Name} {item.Units}?",
    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {

                    dataModel.UtilityTypes.Remove(item);
                    viewModel.UtilityTypes.Remove(item);
                }
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var item = (Tariff)(sender as Button).DataContext;
            if (item != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete tarif {item.Type.Name} {item.StartDate} - {item.EndDate}?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    dataModel.Tarifs.Remove(item);
                    viewModel.Tariffs.Remove(item);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "json|*.json",
                AddExtension = true,
                OverwritePrompt = true,
                DefaultExt = ".mp4"
            };

            if (dialog.ShowDialog().Value)
            {
                viewModel.SavePath = dialog.FileName;
                _newDataModel = FileManager.LoadDefault();
            }
        }
    }

    public class SettingsViewModel : ActiveItem
    {
        public SettingsViewModel(UtilityDataModel model)
        {
            UtilityTypes = new ObservableCollection<UtilityType>();
            Tariffs = new ObservableCollection<Tariff>();
            _utilityListCollectionView = CollectionViewSource.GetDefaultView(UtilityTypes) as ListCollectionView;
            _tarifsListCollectionView = CollectionViewSource.GetDefaultView(Tariffs) as ListCollectionView;
            if (_utilityListCollectionView != null)
            {
                _utilityListCollectionView.IsLiveSorting = true;
                _utilityListCollectionView.CustomSort = new
                        CaseInsensitiveComparer(CultureInfo.InvariantCulture);
            }
            if (_tarifsListCollectionView != null)
            {
                _tarifsListCollectionView.IsLiveSorting = true;
                _tarifsListCollectionView.CustomSort = new
                        CaseInsensitiveComparer(CultureInfo.InvariantCulture);
            }

            //UtilityTypes.AddRange(model.UtilityTypes)
            if (model != null)
            {
                foreach (var item in model.UtilityTypes)
                {
                    UtilityTypes.Add(item);
                }
                //Tariffs.AddRange(model.Tarifs);
                foreach (var item in model?.Tarifs)
                {
                    Tariffs.Add(item);
                }
            }

            //DeleteTariff = 
        }

        public ObservableCollection<UtilityType> UtilityTypes { get; set; }
        public ObservableCollection<Tariff> Tariffs { get; set; }
        private ListCollectionView _utilityListCollectionView;
        private ListCollectionView _tarifsListCollectionView;

        public string SavePath
        {
            get { return Properties.Settings.Default.SavePath; }
            set { Properties.Settings.Default.SavePath = value; Properties.Settings.Default.Save(); OnPropertyChanged(nameof(SavePath)); }
        }


        public ICommand Delete { get; set; }

    }


    public static class Extentions
    {
        //public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        //{
        //    foreach (var item in items)
        //    {
        //        collection.Add(item);
        //    }
        //}
    }

}
