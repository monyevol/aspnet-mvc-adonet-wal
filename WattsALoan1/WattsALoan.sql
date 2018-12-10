CREATE SCHEMA HumanResources;
GO
CREATE SCHEMA Management;
GO

CREATE TABLE HumanResources.Employees
(
	EmployeeID	    INT IDENTITY(1, 1),
	EmployeeNumber  NVARCHAR(10) UNIQUE,
	FirstName	    NVARCHAR(20),
	LastName        NVARCHAR(20),
	EmploymentTitle NVARCHAR(50),
	CONSTRAINT PK_Employees PRIMARY KEY(EmployeeID)
);
GO
CREATE TABLE Management.LoanContracts
(
	LoanContractID	  INT IDENTITY(1, 1),
	LoanNumber        INT UNIQUE NOT NULL,
	DateAllocated	  DATE,
	EmployeeID	      INT,
	CustomerFirstName NVARCHAR(20),
	CustomerLastName  NVARCHAR(20),
	LoanType		  NVARCHAR(20) DEFAULT N'Personal Loan',
	LoanAmount		  DECIMAL(8, 2),
	InterestRate	  DECIMAL(8, 2),
	[Periods]	      SMALLINT,
	MonthlyPayment	  DECIMAL(8, 2),
	FutureValue	      DECIMAL(8, 2),
	InterestAmount	  DECIMAL(8, 2),
	PaymentStartDate  DATE,
	CONSTRAINT FK_LoanProcessors FOREIGN KEY(EmployeeID) REFERENCES HumanResources.Employees(EmployeeID),
	CONSTRAINT PK_LoansContracts PRIMARY KEY(LoanContractID)
);
GO
CREATE TABLE Management.Payments
(
	PaymentID	   INT IDENTITY(1, 1),
	ReceiptNumber  INTEGER,
	PaymentDate	   DATE,
	EmployeeID     INT,
	LoanContractID INT,
	PaymentAmount  DECIMAL(8, 2),
	Balance		   DECIMAL(8, 2),
	CONSTRAINT FK_PaymentsReceivers FOREIGN KEY(EmployeeID) REFERENCES HumanResources.Employees(EmployeeID),
	CONSTRAINT FK_LoansPayments FOREIGN KEY(LoanContractID) REFERENCES Management.LoanContracts(LoanContractID),
	CONSTRAINT PK_Payments PRIMARY KEY(PaymentID)
);
GO
/*
CREATE VIEW Management.LoansContracts
WITH SCHEMABINDING
AS
	SELECT LoanNumber,
		   DateAllocated,
		   EmployeeNumber + N': ' + FirstName + N' ' + LastName AS [Processed By],
		   CustomerFirstName + N' ' + CustomerLastName AS Customer,
		   LoanType,
		   LoanAmount,
		   InterestRate,
		   [Periods],
		   MonthlyPayment,
		   FutureValue,
		   InterestAmount,
		   PaymentStartDate 
	FROM Management.LoanContracts INNER JOIN HumanResources.Employees
	ON Management.LoanContracts.EmployeeID LIKE HumanResources.Employees.EmployeeID;
GO
CREATE VIEW Management.LoanPayment
WITH SCHEMABINDING
AS
	SELECT ReceiptNumber,
		   EmployeeID,
		   LoanContractID,
		   PaymentDate,
		   PaymentAmount,
		   Balance
	FROM   Management.Payments;
GO

CREATE VIEW Management.PaymentsSummary
AS
	SELECT LoanNumber,
		   HumanResources.Employees.EmployeeNumber + N': ' + HumanResources.Employees.FirstName + N' ' + HumanResources.Employees.LastName AS [Received By],
		   PaymentDate,
		   Customer = CustomerFirstName + N' ' + CustomerLastName,
		   LoanType,
		   LoanAmount,
		   PaymentAmount,
		   Balance
	FROM Management.Payments
		INNER JOIN HumanResources.Employees ON Payments.EmployeeID LIKE HumanResources.Employees.EmployeeID
		INNER JOIN Management.LoanContracts ON Management.LoanContracts.LoanContractID LIKE Management.Payments.LoanContractID;
GO
*/