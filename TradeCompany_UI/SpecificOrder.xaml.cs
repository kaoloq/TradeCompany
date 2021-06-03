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

namespace TradeCompany_UI
{
    /// <summary>
    /// Interaction logic for SpecificOrder.xaml
    /// </summary>
    public partial class SpecificOrder : Page
    {
        private InformationAboutOrderList informationAboutOrderList;
        private int orderId;

        public SpecificOrder(int id)
        {
            InitializeComponent();
            orderId = id;
            var connectionString = @"Persist Security Info=False;User ID=DevEd;Password=qqq!11;Initial Catalog=Sandbox.Test;Server=80.78.240.16";
            informationAboutOrderList = new InformationAboutOrderList(connectionString);
        }

        private void dgSpecificOrder_Loaded(object sender, RoutedEventArgs e)
        {
            var productsForOrder = informationAboutOrderList.GetProductsForOrderByOrderId(orderId);
            // либо сделать маппер для ui либо сразу использовать productsForOrder
            foreach (var product in productsForOrder)
            {

            }
        }
    }
}