using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Utilities
{
    /// <summary>
    /// Interaction logic for AddTariff.xaml
    /// </summary>
    public partial class AddTariff : Window
    {
        private readonly UtilityDataModel model;
        AddTariffViewModel viewModel;
        public AddTariff(UtilityDataModel model)
        {
            viewModel = new AddTariffViewModel(model);
            DataContext = viewModel;
            InitializeComponent();
            this.model = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            model.Tarifs.Add(viewModel.NewItem);
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBlock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var canParse = decimal.TryParse(e.Text, out _);
            var endsWithDot = e.Text.EndsWith('.');
            e.Handled = !canParse && !endsWithDot;
        }
    }


    public class AddTariffViewModel
    {
        public Tariff NewItem { get; set; }
        public IEnumerable<UtilityType> UtilityTypes { get; set; }


        public AddTariffViewModel(UtilityDataModel model)
        {
            NewItem = new Tariff();
            UtilityTypes = model.UtilityTypes;
        }
    }

    public abstract class ActiveItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
