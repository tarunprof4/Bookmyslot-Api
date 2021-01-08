 INSERT INTO [Bookmyslot].[dbo].[Customer]( 
		FirstName,  LastName, Gender, Email, Id
	)
OUTPUT inserted.Email
VALUES

('FA',  'LA','Male', 'a@gmail.com' , '142b8af166f349c79f861a707021b415'),
('FB', 'LB','Female', 'b@gmail.com', 'ec50e126e8c842bea85b86b954b71626'),
('FC',  'LC','Male', 'c@gmail.com', 'd4572c45fc894833a7969af389f94669'),
('FD', 'LD','FeMale', 'd@gmail.com', 'ea3fd505edc04fca8ece1347a889055f'),
('FE', 'LE','Male', 'e@gmail.com', '1a2d9ae0f60a466a92a9f18b52a2549c'),
('FF',  'LF','FeMale', 'f@gmail.com', 'b66217ed3c0f49fdafb9c9d20dad12f4'),
('FG',  'LG','Male', 'g@gmail.com', '10a5b1d6d1a7497eb4b59bf95e0793a2'),
('FH',  'LH','FeMale', 'h@gmail.com', 'a2904aacc0ed41f48629ba66b75350ff'),
('FI',  'LI','Male', 'i@gmail.com', '97477a7207884e4cabc1349d83d0ca8f'),
('FJ',  'LJ','FeMale', 'j@gmail.com', '763c9c80f5ae4e21a44723e082cdfbc4'),
('FK',  'LK','Male', 'k@gmail.com', 'd6a5ceed74f3435cabf27757d9179b56')