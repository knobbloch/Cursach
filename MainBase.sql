USE MainBase;

CREATE TABLE Fact (
	Datea DATETIME,    
    Named CHAR (30), 
    Sum INT,
    Category INT DEFAULT 1 REFERENCES Category (Id) ON DELETE SET DEFAULT, 
    Card INT DEFAULT 1 REFERENCES Card (Id) ON DELETE SET DEFAULT,
    ExpenditureIncome BIT (2) NOT NULL
);

CREATE TABLE Card (
	Id INT Primary Key NOT NULL,
    Balance INT NOT NUll,
    Named CHAR (30), 
    CardNumber BIT (16),
    Bank CHAR (30), 
	IsDelete BIT (2) NOT NULL
);

CREATE TABLE Category (
	Id INT Primary Key NOT NULL,
    Named CHAR (30), 
    Picture INT, 
    ExpenditureIncome BIT (2) NOT NULL
);

CREATE TABLE Plan (
	BeginDate DATE NOT NULL, 
    EndDate DATE NOT NULL,
    Named CHAR (30), 
    Category INT DEFAULT 1 REFERENCES Category (Id) ON DELETE SET DEFAULT, 
    Sum INT,
    ExpenditureIncome BIT (2) NOT NULL
);



