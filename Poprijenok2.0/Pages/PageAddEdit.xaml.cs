using Poprijenok2._0.Model;
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

namespace Poprijenok2._0.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        private Agent _curruntAgent = new Agent();

        public PageAddEdit(Agent selectedAgent)
        {
            InitializeComponent();
            if (selectedAgent != null)
            {
                _curruntAgent = selectedAgent;
            }
            DataContext = _curruntAgent;
            cbAgentType.ItemsSource = Poprijenok2Entities.GetContext().AgentType.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_curruntAgent.Title))
                errors.AppendLine("Укажите название агента");
            if (_curruntAgent.AgentType == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.DirectorName))
                errors.AppendLine("Укажите директора агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.Email))
                errors.AppendLine("Укажите email агента");
            if (string.IsNullOrWhiteSpace(_curruntAgent.Logo))
                _curruntAgent.Logo = null;
            if (_curruntAgent.Priority.ToString() == "0")
                errors.AppendLine("Укажите приоритет агента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_curruntAgent.ID == 0)
            {
                Poprijenok2Entities.GetContext().Agent.Add(_curruntAgent);
            }

            try
            {
                Poprijenok2Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация добавлена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить агента?", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Poprijenok2Entities.GetContext().Agent.Remove(_curruntAgent);
                    Poprijenok2Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
