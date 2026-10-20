using System;
using System.Collections.Generic;
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

namespace Bayanov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();

            var currentServices = BayanovAutoserviceEntities.GetContext().Service.ToList();
            ServiceListView.ItemsSource = currentServices;

            ComboType.SelectedIndex = 0;

            UpdateServices();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            var currenrServices = BayanovAutoserviceEntities.GetContext().Service.ToList();
            if(ComboType.SelectedIndex == 0)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.0 && p.Discount <= 0.100)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.0 && p.Discount < 0.05)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.05 && p.Discount < 0.15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.15 && p.Discount < 0.30)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.30 && p.Discount < 0.70)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currenrServices = currenrServices.Where(p => (p.Discount >= 0.70 && p.Discount < 0.100)).ToList();
            }

            currenrServices = currenrServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currenrServices = currenrServices.OrderByDescending(p => p.Cost).ToList();
            }

            if (RButtonUp.IsChecked.Value)
            {
                currenrServices = currenrServices.OrderBy(p => p.Cost).ToList();
            }

            ServiceListView.ItemsSource = currenrServices;
        }
    }
}
