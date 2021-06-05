﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TradeCompany_UI
{
    /// <summary>
    /// Interaction logic for ProductCatalog.xaml
    /// </summary>
    public partial class ProductCatalog : Page
    {
        private ProductsDataAccess _products;
        private string _filtrByText;
        private int? _filtrByGategory;
        private float? _filtrFromStockAmount;
        private float? _filtrToStockAmount;
        private float? _filtrFromWholesalePrice;
        private float? _filtrToWholesalePrice;
        private float? _filtrFromRetailPrice;
        private float? _filtrToRetailPrice;
        private DateTime? _filtrMinDateTime;
        private DateTime? _filtrMaxDateTime;
        

        public ProductCatalog()
        {
            InitializeComponent();
            _products = new ProductsDataAccess();
            dgProductCatalog.ItemsSource = _products.GetAllProducts();

            List<ProductGroupModel> productGroupName = _products.GetAllGroups();
            ProductGroupSelect.Items.Add("Категория");
            for (int i = 0; i < productGroupName.Count; i++)
            {
                ProductGroupSelect.Items.Add(productGroupName[i].Name);
            }

            
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductSearch_TextChange(object sender, TextChangedEventArgs e)
        {
            if (ProductSearch.Text == "")
            {
                _filtrByText = null;
                dgProductCatalog.ItemsSource = _products.GetAllProductsByAllParams(_filtrByText, _filtrByGategory, _filtrFromStockAmount, _filtrToStockAmount, _filtrFromWholesalePrice, _filtrToWholesalePrice, _filtrFromRetailPrice, _filtrToRetailPrice, _filtrMinDateTime, _filtrMaxDateTime);
            }
            else
            {
                _filtrByText = ProductSearch.Text;
                dgProductCatalog.ItemsSource = _products.GetAllProductsByAllParams(_filtrByText, _filtrByGategory, _filtrFromStockAmount, _filtrToStockAmount, _filtrFromWholesalePrice, _filtrToWholesalePrice, _filtrFromRetailPrice, _filtrToRetailPrice, _filtrMinDateTime, _filtrMaxDateTime);
            }
        }


        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
           
            dgProductCatalog.ItemsSource = _products.GetAllProductsByAllParams(_filtrByText, _filtrByGategory, _filtrFromStockAmount, _filtrToStockAmount, _filtrFromWholesalePrice, _filtrToWholesalePrice, _filtrFromRetailPrice, _filtrToRetailPrice, _filtrMinDateTime, _filtrMaxDateTime);
        }


        private void ProductGroupSelect_DropDownClosed(object sender, EventArgs e)
        {
            if (ProductGroupSelect.Text == "Категория")
            {
                _filtrByGategory = null;
            }
            else
            {
                List<ProductGroupModel> productsOfGroup = _products.GetAllGroups();
                for (int i = 0; i < productsOfGroup.Count; i++)
                {
                    if (productsOfGroup[i].Name == ProductGroupSelect.Text)
                    {
                        _filtrByGategory = productsOfGroup[i].ID;
                    }
                }
            }
        }

        private void FromStockAmount_TextChange(object sender, TextChangedEventArgs e)
        {
            _filtrFromStockAmount = InputValidation(_filtrFromStockAmount, FromStockAmount);    
        }

        private void ToStockAmount_TextChange(object sender, TextChangedEventArgs e)
        {
            _filtrToStockAmount = InputValidation(_filtrToStockAmount, ToStockAmount);
        }

        private void FromPrice_TextChange(object sender, TextChangedEventArgs e)
        {
            if (RadioButtonRetailPrice.IsChecked == true)
            {
                SetUpRetailPrice();
                NullifyWholesalePrices();
            }
            if (RadioButtonWholesalePrice.IsChecked == true)
            {
                SetUpWholesalePrice();
                NullifyRetailPrices();
            }
        }

        private void ToPrice_TextChange(object sender, TextChangedEventArgs e)
        {
            if (RadioButtonRetailPrice.IsChecked == true)
            {
                SetUpRetailPrice();
                NullifyWholesalePrices();
            }
            if (RadioButtonWholesalePrice.IsChecked == true)
            {
                SetUpWholesalePrice();
                NullifyRetailPrices();
            }
        }

        private void RadioButtonRetailPrice_Checked(object sender, RoutedEventArgs e)
        {
            SetUpRetailPrice();
            NullifyWholesalePrices();

        }

        private void RadioButtonWholesalePrice_Checked(object sender, RoutedEventArgs e)
        {
            SetUpWholesalePrice();
            NullifyRetailPrices();
        }

        private void DateFrom_SelectedDateChange(object sender, SelectionChangedEventArgs e)
        {
            if (DateFrom.SelectedDate > DateUntil.SelectedDate)
            {
                DateFrom.SelectedDate = null;
                DateUntil.SelectedDate = null;
                MessageBox.Show("Неверный выбор даты");
            }
            _filtrMinDateTime = DateFrom.SelectedDate;
        }

        private void DateUntil_SelectedDateChange(object sender, SelectionChangedEventArgs e)
        {
            if (DateFrom.SelectedDate > DateUntil.SelectedDate)
            {
                DateFrom.SelectedDate = null;
                DateUntil.SelectedDate = null;
                MessageBox.Show("Неверный выбор даты");
            }
            _filtrMaxDateTime = DateUntil.SelectedDate;
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            FromStockAmount.Text = "";
            ToStockAmount.Text = "";
            FromPrice.Text = "";
            ToPrice.Text = "";
            RadioButtonRetailPrice.IsChecked = true;
            ProductSearch.Text = "";
            ProductGroupSelect.SelectedItem = ProductGroupSelect.Items[0];
            DateFrom.SelectedDate = null;
            DateUntil.SelectedDate = null;
            _filtrByText = null;
            _filtrByGategory = null;
            _filtrFromStockAmount = null;
            _filtrToStockAmount = null;
            _filtrFromWholesalePrice = null;
            _filtrToWholesalePrice = null;
            _filtrFromRetailPrice = null;
            _filtrToRetailPrice = null;
            _filtrMinDateTime = null;
            _filtrMaxDateTime = null;
            dgProductCatalog.ItemsSource = _products.GetAllProducts();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private float? InputValidation(float? filtr, TextBox textbox)
        {
            textbox.Text = Regex.Replace(textbox.Text, @"[.]+", ",");
            textbox.Text = Regex.Replace(textbox.Text, @"[^0-9,.]+", "");
            textbox.SelectionStart = textbox.Text.Length;
            if (textbox.Text == "")
            {
                filtr = null;
            }
            else
            {
                try
                {
                    filtr = (float)Convert.ToDouble(textbox.Text);
                }
                catch (FormatException ex)
                {
                    textbox.Text = "";
                    MessageBox.Show("Неверный ввод");
                }
            }

            return filtr;
        }  
        
        private void NullifyWholesalePrices()
        {
            _filtrToWholesalePrice = null;
            _filtrFromWholesalePrice = null;
        }

        private void NullifyRetailPrices()
        {
            _filtrToRetailPrice = null;
            _filtrFromRetailPrice = null;
        }

        private void SetUpWholesalePrice()
        {
            _filtrFromWholesalePrice = InputValidation(_filtrFromWholesalePrice, FromPrice);
            _filtrToWholesalePrice = InputValidation(_filtrToWholesalePrice, ToPrice);
        }

        private void SetUpRetailPrice()
        {
            _filtrFromRetailPrice = InputValidation(_filtrFromRetailPrice, FromPrice);
            _filtrToRetailPrice = InputValidation(_filtrToRetailPrice, ToPrice);
        }

        
    }
}