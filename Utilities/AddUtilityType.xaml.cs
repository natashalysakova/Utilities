using System.Collections.ObjectModel;
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
    /// Interaction logic for AddUtilityType.xaml
    /// </summary>
    public partial class AddUtilityType : Window
    {
        AddUtilityViewModel viewModel;
        public AddUtilityType(UtilityDataModel dataModel)
        {
            DataModel = dataModel;
            viewModel = new AddUtilityViewModel(dataModel.UtilityTypes.Any() ? dataModel.UtilityTypes.Max(x => x.Id + 1) : 0 );
            DataContext = viewModel;

            InitializeComponent();

        }

        public UtilityDataModel DataModel { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataModel.UtilityTypes.Add(viewModel.NewItem);
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
