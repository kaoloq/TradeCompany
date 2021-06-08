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
using TradeCompany_BLL.DataAccess;
using TradeCompany_BLL.Models;
using TradeCompany_UI.Interfaces;

namespace TradeCompany_UI
{
    /// <summary>
    /// Interaction logic for CertainSupply.xaml
    /// </summary>
    public partial class CertainSupply : Page, IProductAddable
    {
        private Frame _frame;
        private SupplysDataAccess _supplysDataAccess;
        public SupplyModel SupplyModel { get; set; }
        public CertainSupply(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
            _supplysDataAccess = new SupplysDataAccess();
        }

        public CertainSupply(Frame frame, int id)
        {
            _frame = frame;
            InitializeComponent();
            _supplysDataAccess = new SupplysDataAccess();
            SupplyModel = _supplysDataAccess.GetSupplyModelByID(id);
            if (SupplyModel is null)
            {

            }
            else
            {
                dgSupplyList.ItemsSource = SupplyModel.SupplyListModel;
                SupplysDate.SelectedDate = SupplyModel.DateTime;
            }
        }

        public void AddProductToCollection(int productID, string productName, string productMeasureUnit, List<ProductGroupModel> productGroupModels)
        {
            SupplyListModel supplyListModel = new SupplyListModel();
            supplyListModel.ProductID = productID;
            supplyListModel.ProductMeasureUnit = productMeasureUnit;
            supplyListModel.ProductName = productName;
            supplyListModel.ProductGroups = productGroupModels;
            SupplyModel.SupplyListModel.Add(supplyListModel);
            dgSupplyList.Items.Refresh();
        }

        private void SupplysDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplyModel.DateTime = (DateTime)SupplysDate.SelectedDate;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new ProductCatalog(_frame, this);

        }

        private void PotentialClients_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgSupplys_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
        }

        private void dgSupplyList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }
    }
}