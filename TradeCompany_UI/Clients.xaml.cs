﻿using System;
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
using TradeCompany_BLL;
using TradeCompany_BLL.Models;
using TradeCompany_UI.Interfaces;

namespace TradeCompany_UI
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {

        private ClientsDataAccess _clientsData;
        private UINavi _uiNavi;
        private Page _previosPage;

        public Clients(Page previosPage = null)
        {
            InitializeComponent();
            InitializeComponent();
            _uiNavi = UINavi.GetUINavi();
            _previosPage = previosPage;
            _clientsData = new ClientsDataAccess();
            dgClientsTable.ItemsSource = _clientsData.GetClients();
        }
        public void UpdateDG()
        {
            dgClientsTable.ItemsSource = _clientsData.GetClients();

        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            MapsDTOtoModel map = new MapsDTOtoModel();
            dgClientsTable.ItemsSource = _clientsData.GetClients();
        }

        private void dgAllClientsTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textInput(object sender, TextChangedEventArgs e)
        {
            MapsDTOtoModel map = new MapsDTOtoModel();
            if (ClientFiltr.Text == "")
            {
                dgClientsTable.ItemsSource = _clientsData.GetClients();
            }
            else
            {
                dgClientsTable.ItemsSource = _clientsData.GetClientsBySearch(ClientFiltr.Text);
            }
        }

        private void ClientsFiltr(object sender, RoutedEventArgs e)
        {
            int? person = null;
            int? sale = null;
            if (CheckBoxF.IsChecked != CheckBoxU.IsChecked)
            {
                if (CheckBoxF.IsChecked == true)
                {
                    person = 1;
                }
                else
                {
                    person = 0;
                }
            }
            if (CheckBoxOpt.IsChecked != CheckBoxRetail.IsChecked)
            {
                if (CheckBoxOpt.IsChecked == true)
                {
                    sale = 1;
                }
                else
                {
                    sale = 0;
                }
            }
            DateTime? maxDate = null;
            if (MaxDate.SelectedDate != null)
            {
                DateTime timeTmp = (DateTime)MaxDate.SelectedDate;
                timeTmp = timeTmp.AddDays(1);
                timeTmp = timeTmp.AddMilliseconds(-1);
                maxDate = (DateTime?)timeTmp;
            }
            dgClientsTable.ItemsSource = _clientsData.GetClientsByParam(person, sale, MinDate.SelectedDate, maxDate);
        }

        private void ButtonFiltr_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxF.IsChecked = false;
            CheckBoxU.IsChecked = false;
            CheckBoxOpt.IsChecked = false;
            CheckBoxRetail.IsChecked = false;
            MinDate.SelectedDate = null;
            MaxDate.SelectedDate = null;
            ClientsFiltr(sender, e);
        }


        private void AddNewClient(object sender, RoutedEventArgs e)
        {
            _uiNavi.GoToThePage(new OneClient(this));
        }


        private void dgClientsTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            ClientBaseModel item = (ClientBaseModel)dg.CurrentItem;
            if (_previosPage is IClientAddable)
            {
                ClientBaseModel clientBaseModel = (ClientBaseModel)dgClientsTable.SelectedItem;
                IClientAddable clientAddable = (IClientAddable)_previosPage;
                clientAddable.AddClientToOrder(clientBaseModel);
                _uiNavi.GoToThePage(_previosPage);
            }
            else
            {
                if (item != null)
                {
                    int id = item.ID;
                    _uiNavi.GoToThePage(new OneClient(id, this));
                }
            }
        }
    }
}
