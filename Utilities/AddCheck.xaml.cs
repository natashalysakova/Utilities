using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddCheck.xaml
    /// </summary>
    public partial class AddCheck : Window
    {
        private readonly UtilityDataModel model;
        private readonly AddCheckViewModel viewModel;
        public AddCheck(UtilityDataModel model, Check selected = default(Check))
        {
            this.model = model;
            viewModel = new AddCheckViewModel(model, selected);
            DataContext = viewModel;
            InitializeComponent();
            
        }

        public void Save()
        {
            var check = viewModel.NewCheck;
            viewModel.NewCheck.ReCalculate();
            //check.AddRecords(viewModel.Records);
            model.AddCheck(check);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Dispose();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(e.Text == "." || e.Text == ",")
            {
                e.Handled = false;
            }
            else
            {
                var canParse = decimal.TryParse(e.Text, out _);
                //var endsWithDot = e.Text.EndsWith('.');
                e.Handled = !canParse;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.NewCheck.ReCalculate();
        }

        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            viewModel.NewCheck.ReCalculate();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            viewModel.NewCheck.ReCalculate();

        }
    }
}
