﻿CREATE PROCEDURE [TradeCompany_DataBase].[AddClient]
	@Name nvarchar(255),
	@INN int,
	@E_mail nvarchar(100),
	@Phone nvarchar(50),
	@Comment nvarchar(500),
	@CorporateBody binary,
	@Type binary,
	@LastOrderDate datetime,
	@ContactPerson nvarchar(255)
AS
	insert into Clients (Name, INN, E_Mail, Phone, Type, CorporateBody, LastOrderDate, Comment)
	values(@Name, @INN, @E_mail, @Phone, @Type, @CorporateBody, @LastOrderDate, @Comment)
