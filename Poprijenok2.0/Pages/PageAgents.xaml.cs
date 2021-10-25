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
    /// Логика взаимодействия для PageAgents.xaml
    /// </summary>
    public partial class PageAgents : Page
    {
        public PageAgents()
        {
            InitializeComponent();

            var currentAgents = Poprijenok2Entities.GetContext().Agent.ToList();
            var types = Poprijenok2Entities.GetContext().AgentType.ToList();

            List<String> sort = new List<String>();
            sort.Add("Сортировка");
            sort.Add("Наименование");
            sort.Add("Размер скидки");
            sort.Add("Приоритет");

            types.Insert(0, new AgentType
            {
                Title = "Все типы"
            });

            lvAgents.ItemsSource = currentAgents;

            cbTypes.ItemsSource = types;
            cbTypes.DisplayMemberPath = "Title";
            cbTypes.SelectedValuePath = "ID";
            cbTypes.SelectedIndex = 0;
           
            cbSort.ItemsSource = sort;
            cbSort.SelectedIndex = 0;

            tbFinder.Text = "Поиск по названию";
        }
        /// <summary>
        /// Вспомогательный метод для поиска, фильтрации и сортировки агентов
        /// </summary>
        private void UpdateAgents()
        {
            var currentAgents = Poprijenok2Entities.GetContext().Agent.ToList();

            if (cbTypes.SelectedIndex > 0)
            {
                currentAgents = currentAgents.Where(a => a.AgentTypeID == int.Parse(cbTypes.SelectedValue.ToString())).ToList();
            }

            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        if (rbAsc.IsChecked == true)
                        {
                            currentAgents = currentAgents.OrderBy(a => a.Title).ToList();
                        }
                        else
                            currentAgents = currentAgents.OrderByDescending(a => a.Title).ToList();
                        break;
                    case 2:
                        break;
                    case 3:
                        if (rbAsc.IsChecked == true)
                        {
                            currentAgents = currentAgents.OrderBy(a => a.Priority).ToList();
                        }
                        else
                            currentAgents = currentAgents.OrderByDescending(a => a.Priority).ToList();
                        break;
                }
            }

            if (tbFinder.Text == "Поиск по названию")
            {
                tbFinder.Text = "";
            }

            currentAgents = currentAgents.Where(a => a.Title.ToLower().Contains(tbFinder.Text.ToLower())).ToList();
            lvAgents.ItemsSource = currentAgents.ToList();
        }
        /// <summary>
        /// Открытие окна добавления агента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit(null));
        }
        /// <summary>
        /// Обновление списка агентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Poprijenok2Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(a => a.Reload());
                lvAgents.ItemsSource = Poprijenok2Entities.GetContext().Agent.ToList();
            }
        }
        /// <summary>
        /// Открытие окна Изменения данных выбранного агента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit((sender as Button).DataContext as Agent));
        }
        /// <summary>
        /// Поиск по названию агента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }
        /// <summary>
        /// Сортировка агентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
        /// <summary>
        /// Фильтрация агентов по их типу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
        /// <summary>
        /// Очистка поля Поиска от значения по-умолчанию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFinder_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbFinder.Text == "Поиск по названию")
            {
                tbFinder.Text = "";
            }
        }
        /// <summary>
        /// Изменение типа сортировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
