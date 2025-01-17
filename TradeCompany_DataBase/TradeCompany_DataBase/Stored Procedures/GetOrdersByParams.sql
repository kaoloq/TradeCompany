﻿CREATE PROCEDURE [TradeCompany_DataBase].[GetOrdersByParams]
	@Client nvarchar(255),
	@MinDateTime DateTime,
	@MaxDateTime DateTime,
	@Address nvarchar(255)
AS
	SELECT o.ID, o.ClientsID, o.DateTime, o.AddressID, o.Comment, A.Address as Address,
	ol.ID, ol.OrderID, ol.ProductID, ol.Amount, ol.Price,
	c.ID, c.Name, c.Type, c.Phone,
	p.id, p.Name, m.Name as MeasureUnitName from [TradeCompany_DataBase].[Orders] as O
	left join [TradeCompany_DataBase].OrderLists as OL on o.ID = OL.OrderID
	left join [TradeCompany_DataBase].Clients as C on o.ClientsID = C.ID
	left join [TradeCompany_DataBase].Products as P on ol.ProductID = P.ID
	left join [TradeCompany_DataBase].MeasureUnits as M on m.ID = P.MeasureUnit
	left join [TradeCompany_DataBase].Addresses as A on o.AddressID = A.ID
	Where 
	O.IsDeleted = 0 and
	(@Client IS NULL OR c.Name LIKE '%'+@Client+'%') AND
	(@Address IS NULL OR A.Address LIKE '%'+@Address+'%') AND
	(@MinDateTime IS NULL OR O.DateTime >= @MinDateTime) AND
	(@MaxDateTime IS NULL OR O.DateTime <= @MaxDateTime) 
	order by o.DateTime desc, o.ID desc