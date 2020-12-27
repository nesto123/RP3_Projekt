SELECT IdItem_FK, Price FROM dbo.Happyhour WHERE Deleted=@deleted

select * from dbo.Happyhour

INSERT INTO dbo.Happyhour(IdItem_FK,From_,Untill,  Pastprice)  VALUES(1,convert(datetime,'26.12.2020. 22:00:00',5),convert(datetime,'26.12.2020. 23:00:00',5), 2)

INSERT INTO dbo.Happyhour(IdItem_FK,From_,Untill,  Pastprice)  VALUES(1,convert(datetime2,'2018-06-23 07:30:20',5),convert(datetime2,'2018-06-23 07:30:20',5), 2)

INSERT INTO dbo.Happyhour(IdItem_FK,From_,Untill,  Pastprice)  VALUES(3,'2018-06-23 23:30:20','2018-06-23 07:30:20', 2)