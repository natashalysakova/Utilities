using System;
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
}
