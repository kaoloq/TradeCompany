﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TradeCompany_BLL;
using TradeCompany_BLL.Models;
using TradeCompany_DAL;
using TradeCompany_DAL.DTOs;

namespace TradeCompany_UI
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        private UINavi _uiNavi;
        private OrderDataAccess _orderDataAccess;
        private List<OrderModel> _orderModels;
        public Orders(Page previousPage = null)
        {
            InitializeComponent();
            _uiNavi = UINavi.GetUINavi();
            _orderDataAccess = new OrderDataAccess();
            _orderModels = _orderDataAccess.GetOrderModelsByParams();
            dgOrders.ItemsSource = _orderModels;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ClientFiltr.Text = null;
            AddressFiltr.Text = null;
            MinDate.SelectedDate = null;
            MaxDate.SelectedDate = null;
            FilterOrders();
        }

        public void FilterOrders()
        {
            string client = null;
            string address = null;
            if (ClientFiltr.Text != "")
            {
                client = ClientFiltr.Text;
            }
            if (AddressFiltr.Text != "")
            {
                address = AddressFiltr.Text;
            }
            DateTime? maxDate = null;

            if (MaxDate.SelectedDate != null)
            {
                DateTime dateTimeTmp = (DateTime)MaxDate.SelectedDate;
                dateTimeTmp = dateTimeTmp.AddDays(1);
                dateTimeTmp = dateTimeTmp.AddMilliseconds(-2);
                maxDate = (DateTime?)dateTimeTmp;
            }
            List<OrderModel> orderModels = _orderDataAccess.GetOrderModelsByParams(client, MinDate.SelectedDate, maxDate, address);
            dgOrders.ItemsSource = orderModels;
        }


        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            _uiNavi.GoToThePage(new SpecificOrder(this));
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<OrderModel> orderModels = _orderDataAccess.SearchOrderModels(SearchBox.Text);
            dgOrders.ItemsSource = orderModels;
        }

        private void ClientFiltr_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterOrders();
        }

        private void AddressFiltr_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterOrders();
        }

        private void MinDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOrders();
        }

        private void MaxDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOrders();
        }

        private void dgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgOrders.CurrentItem != null)
            {
                OrderModel crntModel = (OrderModel)dgOrders.CurrentItem;
                _uiNavi.GoToThePage(new SpecificOrder(crntModel.ID,this));
            }
        }
    }
}
