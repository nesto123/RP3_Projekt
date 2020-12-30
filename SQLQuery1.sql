SELECT * FROM [User] WHERE Deleted = 0

UPDATE [User] SET Deleted = 1 WHERE Id=5

INSERT INTO [User] (Username,Password) VALUES ('b','b')

Select Id from [Storage] where Deleted=0

Select * from [Receipts] where  '2020-12-27 22:05:14'<Time and Time< '2020-12-28 23:05:14' and Deleted = 0


Select sum(RI.Amount) as amount from [Receipts_item] RI Where RI.Id_receiptFK in (Select Id from [Receipts] where  '2020-12-27 22:05:14'<Time and Time< '2020-12-28 23:05:14' and Deleted = 0
)
/*Upit za količinu itema u vremenu*/
Select sum(RI.Amount) as amount, AVG(R.Discount) as avgdiscount, sum(R.Total) as total, ST.Item from [Receipts_item] RI, [Receipts] R, [Storage] ST Where RI.Id_receiptFK = R.Id AND '2020-12-27 22:05:14'<Time and Time< '2020-12-30 23:05:14' and R.Deleted = 0 AND ST.Id = RI.Id_itemFK group by ST.Item

/*racuni*/