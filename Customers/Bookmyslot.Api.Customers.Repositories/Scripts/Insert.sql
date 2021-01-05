 INSERT INTO [Bookmyslot].[dbo].[Customer]( 
		FirstName, MiddleName, LastName, Gender, Email
	)
OUTPUT inserted.Email
VALUES

('FA', 'MA', 'LA','Male', 'a@gmail.com'),
('FB', 'MB', 'LB','Female', 'b@gmail.com'),
('FC', 'MC', 'LC','Male', 'c@gmail.com')