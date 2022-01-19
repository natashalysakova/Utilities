﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Utilities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UtilityDataModel model;
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(model);
            if(window.ShowDialog().Value)
            {
                ReloadModel();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            //Properties.Settings.Default.Reset();
#endif

            if (string.IsNullOrEmpty(Properties.Settings.Default.SavePath))
            {
                Settings window = new Settings(model);
                if (window.ShowDialog().Value)
                {
                    ReloadModel();
                }
            }
            else
                ReloadModel();
        }

        private void ReloadModel()
        {
            try
            {
                model = FileManager.LoadDefault();
            }
            catch (Exception)
            {
                model = new UtilityDataModel();

            }

            model.CheckAdded += Model_CheckAdded;
            RefillChecks();
            CheckList.SelectedIndex = CheckList.Items.Count - 1;
            CheckList.ScrollIntoView(CheckList.SelectedItem);
        }

        private void RefillChecks()
        {
            viewModel.Checks.Clear();
            //viewModel.Checks.AddRange(model.Checks);
            foreach (var item in model.Checks)
            {
                viewModel.Checks.Add(item);
            }
        }

        private void Model_CheckAdded(Check check)
        {
            viewModel.Checks.Add(check);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                FileManager.SaveDefault(model);
            }
            catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddCheck window = new AddCheck(model);
            window.ShowDialog();

            FileManager.SaveDefault(model);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((sender as Button).DataContext as Check).ReCalculate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedCheck != null)
            {
                Export window = new Export(viewModel.SelectedCheck);
                window.ShowDialog();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedCheck == null)
                return;

            if (MessageBox.Show($"Are you sure you want to delete check from {viewModel.SelectedCheck.Date.ToString("dd.MM.yyyy")}?",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                model.Checks.Remove(viewModel.SelectedCheck);
                viewModel.Checks.Remove(viewModel.SelectedCheck);
                viewModel.SelectedCheck = null;
                FileManager.SaveDefault(model);
            }
        }
    }

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