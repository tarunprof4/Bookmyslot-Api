 INSERT INTO [Bookmyslot].[dbo].[Customer]( 
		FirstName, MiddleName, LastName, Gender, Email
	)
OUTPUT inserted.Email
VALUES

('FA', 'MA', 'LA','Male', 'a@gmail.com'),
('FB', 'MB', 'LB','Female', 'b@gmail.com'),
('FC', 'MC', 'LC','Male', 'c@gmail.com'),
('FD', 'MD', 'LD','FeMale', 'd@gmail.com'),
('FE', 'ME', 'LE','Male', 'e@gmail.com'),
('FF', 'MF', 'LF','FeMale', 'f@gmail.com'),
('FG', 'MG', 'LG','Male', 'g@gmail.com'),
('FH', 'MH', 'LH','FeMale', 'h@gmail.com'),
('FI', 'MI', 'LI','Male', 'i@gmail.com'),
('FJ', 'MJ', 'LJ','FeMale', 'j@gmail.com'),
('FK', 'MK', 'LK','Male', 'k@gmail.com')