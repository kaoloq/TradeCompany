﻿CREATE PROCEDURE [TradeCompany_DataBase].[GetProductsByParams]
	@FromStockAmount float,
	@ToStockAmount float,
	@FromWholesalePrice float,
	@ToWholesalePrice float,
	@FromRetailPrice float,
	@ToRetailPrice float,
	@MinDateTime DateTime,
	@MaxDateTime DateTime
AS
	select P.ID, P.Name, P.StockAmount, MU.Name as [MeasureUnitName], P.WholesalePrice, P.RetailPrice, P.LastSupplyDate
	from [TradeCompany_DataBase].Products as P
	left join [TradeCompany_DataBase].[Product_ProductGroups] as P_PG on P.ID = P_PG.ProductID
	left join [TradeCompany_DataBase].[ProductGroups] as PG on PG.ID = P_PG.ProductGroupID
	left join [TradeCompany_DataBase].MeasureUnits as MU on MU.ID = P.MeasureUnit
where
	(@FromStockAmount IS NULL OR P.StockAmount >= @FromStockAmount) AND
	(@ToStockAmount IS NULL OR P.StockAmount <= @ToStockAmount) AND
	(@FromWholesalePrice IS NULL OR P.WholesalePrice >= @FromWholesalePrice) AND
	(@ToWholesalePrice IS NULL OR P.WholesalePrice <= @ToWholesalePrice) AND
	(@FromRetailPrice IS NULL OR P.RetailPrice >= @FromRetailPrice) AND
	(@ToRetailPrice IS NULL OR P.RetailPrice <= @ToRetailPrice) AND
	(@MinDateTime IS NULL OR P.LastSupplyDate >= @MinDateTime) AND
	(@MaxDateTime IS NULL OR P.LastSupplyDate <= @MaxDateTime) 
