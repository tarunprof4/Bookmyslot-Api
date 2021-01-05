 INSERT INTO [Bookmyslot].[dbo].[Slot] ( 
		Id, Title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, TimeZone, SlotDate
	)
OUTPUT inserted.Id
VALUES
	('f05206be-973c-4a2f-bc85-35e80bcc6260','a','a@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-20 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6261','aOne','a@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-21 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6262','atwo','a@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-22 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6263','athree','a@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-23 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6264','b','b@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-24 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6265','bone','b@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-25 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6266','btwo','b@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-26 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6267','c','c@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-27 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6268','cone','c@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-28 00:00:00.000'),
	('f05206be-973c-4a2f-bc85-35e80bcc6269','ctwo','c@gmail.com','10:00:00', '11:00:00', 0,'India Standard Time', '2021-03-29 00:00:00.000')
	