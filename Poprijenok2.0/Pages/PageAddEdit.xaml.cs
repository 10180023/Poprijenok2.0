using Microsoft.Win32;
using Poprijenok2._0.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Poprijenok2._0.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        private Agent _currentAgent = new Agent();
        public static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        public PageAddEdit(Agent selectedAgent)
        {
            InitializeComponent();
            if (selectedAgent != null)
            {
                _currentAgent = selectedAgent;
            }

            Logo.Source = new BitmapImage(new Uri($"{projectDirectory}/Poprijenok2.0/{_currentAgent.Logo}"));

            DataContext = _currentAgent;
            cbAgentType.ItemsSource = Poprijenok2Entities.GetContext().AgentType.ToList();
        }
        /// <summary>
        /// Проверка корректного ввода данных во все поля и добавленре записи в талицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите название агента");
            if (_currentAgent.AgentType == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите директора агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите email агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Logo))
                _currentAgent.Logo = null;
            if (_currentAgent.Priority.ToString() == "0")
                errors.AppendLine("Укажите приоритет агента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_currentAgent.ID == 0)
            {
                Poprijenok2Entities.GetContext().Agent.Add(_currentAgent);
            }

            try
            {
                Poprijenok2Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация добавлена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        ///  Отмена внесения изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
        /// <summary>
        /// Удаление записи из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить агента?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Poprijenok2Entities.GetContext().Agent.Remove(_currentAgent);
                    Poprijenok2Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно удалить несуществующего агента", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        /// <summary>
        /// Запрет на ввод не чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Digits_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Выбор логотипа для агента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".png",
                FileName = "picture.png",
                Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG",
                InitialDirectory = projectDirectory + @"\Poprijenok2.0\agents\"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                Logo.Source = new BitmapImage(new Uri(fileName));
                tbLogo.Text = "agents" + Path.GetFileName(fileName);
                File.Copy(fileName, $"{projectDirectory}/Poprijenok2.0{tbLogo.Text}");
            }
        }
    }
}
