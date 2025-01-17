﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCompany_BLL.Models;
using TradeCompany_DAL;
using TradeCompany_DAL.DTOs;

namespace TradeCompany_BLL.DataAccess
{
        public class StatisticsDataAccess
        {

        private StatisticsData _statistecsData = new StatisticsData(@"Persist Security Info=False;User ID=DevEd;Password=qqq!11;Initial Catalog=Sandbox.Test;Server=80.78.240.16");
        private MapsDTOtoModel _mapDTOtoModel = new MapsDTOtoModel();


        public List<StatisticsGroupsModel> GetStatisticsProducts(FilterGroupModel filter)
        {
            List<StatisticsGroupsDTO> statisticsProductsDTOs = _statistecsData.GetStatisticsProducts(filter.MinDateSupply, filter.MaxDateSupply, filter.MinDateOrder, filter.MaxDateOrder, filter.MinAmount, filter.MaxAmount, filter.MinSum, filter.MaxSum, filter.PeriodFor, filter.PeriodUntil);
            List<StatisticsGroupsModel> clinetsModel = _mapDTOtoModel.MapStatisticsGroursDTOToStatisticsGroursModel(statisticsProductsDTOs);
            return clinetsModel;
        }

        public List<StatisticsProductModel>  GetStatisticsProductsByGroupID(int id, FilterGroupModel filter)
        {
            List<StatisticsProductDTO> statisticsProductsDTOs = _statistecsData.GetStatisticsProductsByGroupID(id, filter.MinDateSupply, filter.MaxDateSupply, filter.MinDateOrder, filter.MaxDateOrder, filter.MinAmount, filter.MaxAmount, filter.MinSum, filter.MaxSum, filter.PeriodFor, filter.PeriodUntil);
            List<StatisticsProductModel> clinetsModel = _mapDTOtoModel.MapStatisticsProductsDTOToStatisticsProductsModel(statisticsProductsDTOs);
            return clinetsModel;
        }
    }
}
