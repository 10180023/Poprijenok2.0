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
        }

        private void UpdateAgents()
        {
            var currentAgents = Poprijenok2Entities.GetContext().Agent.ToList();

            if (cbTypes.SelectedIndex > 0)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeID == int.Parse(cbTypes.SelectedValue.ToString())).ToList();
            }

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(tbFinder.Text.ToLower())).ToList();
            lvAgents.ItemsSource = currentAgents.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Poprijenok2Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                lvAgents.ItemsSource = Poprijenok2Entities.GetContext().Agent.ToList();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit((sender as Button).DataContext as Agent));
        }

        private void tbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
