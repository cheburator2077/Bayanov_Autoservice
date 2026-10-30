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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;

            var _currentClient = BayanovAutoserviceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentService.DurationInSeconds == 0)
                errors.AppendLine("Укажите длительность услуги");

            if (_currentService.DurationInSeconds > 240)
                errors.AppendLine("Длительность не может быть больше 240 минут");

            if (_currentService.DurationInSeconds < 0)
                errors.AppendLine("Длительность не может быть меньше 0");

            if (_currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ServiceID == 0)
                BayanovAutoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                BayanovAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length < 3 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int checkHour, checkMin;
                int.TryParse(start[0], out checkHour);
                int.TryParse(start[1], out checkMin);

                if (checkHour > 23 || checkMin > 59)
                {
                    MessageBox.Show("Введено некорректное время");
                    TBStart.Text = "";
                    return;
                }

                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = 0;
                if (!int.TryParse(start[1], out startMin)) return;

                int sum = startHour + startMin + _currentService.DurationInSeconds;

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                if (EndHour > 23)
                {
                    EndHour -= 24;
                }
                string strEndHour = (EndHour < 10) ? "0" + EndHour.ToString() : EndHour.ToString();
                string strEndMin = (EndMin < 10) ? "0" + EndMin.ToString() : EndMin.ToString();
                s = strEndHour + ":" + strEndMin;
                TBEnd.Text = s;
            }
        }
    }
}
