using SaveEyes.DB;
using SaveEyes.Properties;
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

namespace SaveEyes
{
    /// <summary>
    /// Interaction logic for AgentsList.xaml
    /// </summary>
    public partial class AgentsList : Page
    {
        public List<Agent> Agents { get; set; }
        public List<Agent> FilteredAgents { get; set; }

        public List<AgentType> AgentTypes { get; set; }
        public Dictionary<string, int> Sortings { get; set; }

        public AgentsList()
        {
            InitializeComponent();


            Agents = DBConnection.connection.Agent.ToList();
            FilteredAgents = Agents.ToList();

            AgentTypes = DBConnection.connection.AgentType.ToList();

            AgentTypes.Insert(0, new AgentType() { Title = "Все типы" });

            Sortings = new Dictionary<string, int>
            {
                { "Без сортировки", 1},
                { "Наименование по возрастанию", 2},
                { "Наименование по убыванию", 3 },
                { "Приоритет по возрастанию", 4 },
                { "Приоритет по убыванию", 5 },
                { "Размер скидки по возрастанию", 6},
                { "Размер скидки по убыванию", 7 }
            };

            this.DataContext = this;
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void lvAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new AgentPage(lvAgents.SelectedItem as Agent, false));
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            var searchingText = tbSearch.Text.ToLower();
            var sorting = Sortings[cbSorting.SelectedItem as string];
            var agentType = cbType.SelectedItem as AgentType;

            if (sorting == null || agentType == null)
                return;

            FilteredAgents = Agents.FindAll(agent => agent.Title.ToLower().Contains(searchingText));

            if (agentType.ID != 0)
                FilteredAgents = FilteredAgents.FindAll(agent => agent.AgentType == agentType);

            switch (sorting)
            {
                case 1:
                    FilteredAgents = FilteredAgents.OrderBy(agent => agent.ID).ToList();
                    break;

                case 2:
                    FilteredAgents = FilteredAgents.OrderBy(agent => agent.Title).ToList();
                    break;

                case 3:
                    FilteredAgents = FilteredAgents.OrderByDescending(agent => agent.Title).ToList();
                    break;

                case 4:
                    FilteredAgents = FilteredAgents.OrderBy(agent => agent.Priority).ToList();
                    break;

                case 5:
                    FilteredAgents = FilteredAgents.OrderByDescending(agent => agent.Priority).ToList();
                    break;

                case 6:
                    FilteredAgents = FilteredAgents.OrderBy(agent => agent.Discount).ToList();
                    break;

                case 7:
                    FilteredAgents = FilteredAgents.OrderByDescending(agent => agent.Discount).ToList();
                    break;

                default:
                    break;
            }


            lvAgents.ItemsSource = FilteredAgents;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AgentPage(lvAgents.SelectedItem as Agent, true));
        }
    }
}
