﻿CREATE PROCEDURE [TradeCompany_DataBase].[GetOrderByClientID]
	@ID int
AS
	SELECT O.ID, O.ClientsID, A.Address, O.DateTime, O.Comment
	from TradeCompany_DataBase.Orders as O
	left join TradeCompany_DataBase.Addresses as A on A.ClientID = o.AddressID
	where ClientsID = @ID
	order by o.DateTime desc