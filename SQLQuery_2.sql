-- //1 Position
-- //2 Account
-- //3 Employee
-- //4 Product
-- //5 ProductType
-- //6 Supplier
-- //7 ImportBill
-- //8 ImportBillInfo
-- //9 PaymentVoucher
-- //10 ReceiptVoucher
-- //11 Invoice
-- //12 InvoiceInfo
-- //13 Incident

-- //1 Positionx
-- //2 Accountx
-- //3 Employeex
-- //4 Product
-- //5 ProductType
-- //6 Supplierx
-- //7 ImportBill
-- //8 ImportBillInfo
-- //9 PaymentVoucher
-- //10 ReceiptVoucher
-- //11 Invoice
-- //12 InvoiceInfo
-- //13 Incident

CREATE TABLE Position (
    idPosition INT,
    namePosition NVARCHAR(100),
    salary REAL,
    standardWorkDays REAL,
    isDelete BIT,
    CONSTRAINT PK_Position PRIMARY KEY(idPosition)
)
CREATE TABLE Account (
    idAccount INT,
    idEmployee INT,
    username NVARCHAR(100),
    password NVARCHAR(1000),
    isDelete BIT,
    CONSTRAINT PK_Account PRIMARY KEY(idAccount)
)

CREATE TABLE Employee (
    idEmployee INT,
    idPosition INT,
    name NVARCHAR(100),
    dateOfBirth SMALLDATETIME,
    dateStartWorking SMALLDATETIME,
    address NVARCHAR(500),
    phoneNumber NVARCHAR(10),
    gender BIT,
    isDelete BIT,
    CONSTRAINT PK_Employee PRIMARY KEY(idEmployee)
)

CREATE TABLE ProductType (
    idProductType INT,
    nameProductType NVARCHAR(100),
    profitPercentage REAL,
    isDelete BIT,
    CONSTRAINT PK_Good PRIMARY KEY(idProductType)
)

CREATE TABLE Product (
    idProduct INT,
    idProductType INT,
    nameProduct NVARCHAR(100),
    description NVARCHAR(1000),
    price REAL,
    status BIT,
    image VARCHAR(MAX),
    isDelelte BIT,
    CONSTRAINT PK_GoodType PRIMARY KEY(idProduct)
)

CREATE TABLE Supplier (
    idSupplier INT,
    nameSupplier NVARCHAR(100),
    addressSupplier NVARCHAR(200),
    phoneSupplier NVARCHAR(100),
    isDelete BIT,
    CONSTRAINT PK_Supplier PRIMARY KEY(idSupplier)
)

CREATE TABLE ImportBill (
    idImportBill INT,
    idEmployee INT,
    idSupplier INT,
    date SMALLDATETIME,
    totalMoney REAL,
    note NVARCHAR(1000),
    status BIT,
    isDelete BIT,
    CONSTRAINT PK_ImportBill PRIMARY KEY(idImportBill)
)

CREATE TABLE ImportBillInfo (
    idImportBillInfo INT,
    idImportBill INT,
    idMaterial INT,
    quantity INT,
    unit NVARCHAR(50),
    price REAL,
    intoMoney REAL,
    CONSTRAINT PK_ImportBillInfo PRIMARY KEY(idImportBillInfo)
)

CREATE TABLE PaymentVoucher (
    idPaymentVoucher INT,
    idEmployee INT,
    idImportBill INT,
    date SMALLDATETIME,
    totalMoney REAL,
    note NVARCHAR(100),
    isDelete BIT,
    CONSTRAINT PK_PaymentVoucher PRIMARY KEY(idPaymentVoucher)
)

CREATE TABLE ReceiptVoucher (
    idReceiptVoucher INT,
    idEmployee INT,
    date SMALLDATETIME,
    totalMoney REAL,
    note NVARCHAR(100),
    isDelete BIT,
    CONSTRAINT PK_ReceiptVoucher PRIMARY KEY(idReceiptVoucher)
)


CREATE TABLE Invoice (
    idInvoice INT,
    idEmployee INT,
    date SMALLDATETIME,
    totalMoney REAL,
    moneyCustomer REAL,
    tableNumber INT,
    status BIT,
    isDelete BIT,
    CONSTRAINT PK_Invoice PRIMARY KEY(idInvoice)
)

CREATE TABLE InvoiceInfo (
    idInvoiceInfo INT,
    idInvoice INT,
    idProduct INT,
    quantity INT,
    price REAL,
    intoMoney REAL,
    CONSTRAINT PK_InvoiceInfo PRIMARY KEY(idInvoiceInfo)
)

CREATE TABLE Incident (
    idIncident INT,
    idEmployee INT,
    date SMALLDATETIME,
    description NVARCHAR(1000),
    status BIT,
    pay BIT,
    totalMoney REAL,
    isDelete BIT,
    CONSTRAINT PK_Incident PRIMARY KEY(idIncident)
)

CREATE TABLE Material (
    idMaterial INT,
    nameMaterail INT,
    type NVARCHAR(100),
    countUnit NVARCHAR(100),
    quantity INT,
    purchasePrice REAL,
    image NVARCHAR(MAX),
    status BIT,
    isDelelte BIT,
    CONSTRAINT PK_Material PRIMARY KEY(idMaterial)
)
-- //1 Position
-- //2 Account
-- //3 Employee
-- //4 Product
-- //5 ProductType
-- //6 Supplier
-- //7 ImportBill
-- //8 ImportBillInfo
-- //9 PaymentVoucher
-- //10 ReceiptVoucher
-- //11 Invoice
-- //12 InvoiceInfo
-- //13 Incident
-- //14 Material

-- DROP TABLE Position
-- DROP TABLE Account
-- DROP TABLE Employee
-- DROP TABLE Product 
-- DROP TABLE ProductType 
-- DROP TABLE Supplier 
-- DROP TABLE ImportBill 
-- DROP TABLE ImportBillInfo 
-- DROP TABLE PaymentVoucher 
-- DROP TABLE ReceiptVoucher 
-- DROP TABLE Invoice 
-- DROP TABLE InvoiceInfo 
-- DROP TABLE Incident 
-- DROP TABLE Material 

-- ALTER TABLE Persons DROP CONSTRAINT UC_Person; 
-- ALTER TABLE Account DROP CONSTRAINT FK_Account_Employee; 
-- ALTER TABLE ImportBill DROP CONSTRAINT FK_ImportBill_Employee; 
-- ALTER TABLE PaymentVoucher DROP CONSTRAINT FK_PaymentVoucher_Employee; 
-- ALTER TABLE ReceiptVoucher DROP CONSTRAINT FK_ReceiptVoucher_Employee; 
-- ALTER TABLE Invoice DROP CONSTRAINT FK_Invoice_Employee; 
-- ALTER TABLE Incident DROP CONSTRAINT FK_Incident_Employee; 

ALTER TABLE Account ADD CONSTRAINT FK_Account_Employee FOREIGN KEY(idAccount) REFERENCES Employee(idEmployee)

ALTER TABLE Employee ADD CONSTRAINT FK_Employee_Position FOREIGN KEY(idPosition) REFERENCES Position(idPosition)

ALTER TABLE Product ADD CONSTRAINT FK_Product_ProductType FOREIGN KEY(idProductType) REFERENCES ProductType(idProductType)

ALTER TABLE ImportBill ADD CONSTRAINT FK_ImportBill_Employee FOREIGN KEY(idEmployee) REFERENCES Employee(idEmployee)
ALTER TABLE ImportBill ADD CONSTRAINT FK_ImportBill_Supplier FOREIGN KEY(idSupplier) REFERENCES Supplier(idSupplier)

ALTER TABLE ImportBillInfo ADD CONSTRAINT FK_ImportBillInfo_ImportBill FOREIGN KEY(idImportBill) REFERENCES ImportBill(idImportBill)
ALTER TABLE ImportBillInfo ADD CONSTRAINT FK_ImportBillInfo_Material FOREIGN KEY(idMaterial) REFERENCES Material(idMaterial)

ALTER TABLE PaymentVoucher ADD CONSTRAINT FK_PaymentVoucher_Employee FOREIGN KEY(idEmployee) REFERENCES Employee(idEmployee)
ALTER TABLE PaymentVoucher ADD CONSTRAINT FK_PaymentVoucher_ImportBill FOREIGN KEY(idImportBill) REFERENCES ImportBill(idImportBill)

ALTER TABLE ReceiptVoucher ADD CONSTRAINT FK_ReceiptVoucher_Employee FOREIGN KEY(idEmployee) REFERENCES Employee(idEmployee)

ALTER TABLE Invoice ADD CONSTRAINT FK_Invoice_Employee FOREIGN KEY(idEmployee) REFERENCES Employee(idEmployee)

ALTER TABLE InvoiceInfo ADD CONSTRAINT FK_InvoiceInfo_Invoice FOREIGN KEY(idInvoice) REFERENCES Invoice(idInvoice)
ALTER TABLE InvoiceInfo ADD CONSTRAINT FK_InvoiceInfo_Product FOREIGN KEY(idProduct) REFERENCES Product(idProduct)

ALTER TABLE Incident ADD CONSTRAINT FK_Incident_Employee FOREIGN KEY(idEmployee) REFERENCES Employee(idEmployee)

ALTER TABLE Employee ADD CONSTRAINT FK_Employee_Position FOREIGN KEY(idPosition) REFERENCES Position(idPosition)

ALTER TABLE Product ADD CONSTRAINT FK_Product_ProductType FOREIGN KEY(idProductType) REFERENCES ProductType(idProductType)

ALTER TABLE Employee DROP CONSTRAINT FK_Employee_Position; 

INSERT INTO Position VALUES ('1','Accoutant','7500000','30','0')
SET DATEFORMAT dmy
INSERT INTO Employee VALUES ('1','1','Phat','21/09/2001','21/09/2021','1','0')


UPDATE Employee SET isDelete = '0' , name = 'Thinh' WHERE idEmployee = 2

SELECT * FROM Employee WHERE isDelete = 0 AND idEmployee != 0

SELECT * FROM [Position]

SELECT * FROM Supplier
INSERT INTO Supplier VALUES ('1','TT','Ha Noi','0783249260','0')

SELECT COUNT(*) FROM Supplier

SELECT COUNT(*) FROM ImportBill Where idSupplier=1
SELECT SUM(totalMoney) FROM ImportBill Where idSupplier=1
SELECT SUM(totalMoney) FROM ImportBill
SELECT MAX(idEmployee) FROM Employee

UPDATE Employee SET 
idPosition='1' , 
name='Thinh' , 
dateOfBirth='2001/08/21' , 
dateStartWorking='2021/08/21' , 
gender= '1' WHERE idEmployee=1

SELECT * FROM Employee WHERE idEmployee != 0 AND isDelete = 0

SET DATEFORMAT dmy
INSERT INTO Employee VALUES ('1','1','Phat','21/09/2001','21/09/2021','Duy Phuoc','0783249260','1','0')

SET DATEFORMAT dmy
INSERT INTO ReceiptVoucher VALUES ('1','1','21/09/2001','100000','No','0')
SELECT * FROM ReceiptVoucher
SELECT * FROM Employee
SELECT * FROM Incident
Delete from ReceiptVoucher

SET DATEFORMAT dmy
INSERT INTO ImportBill VALUES ('1','1','1','1/12/2001','100000','No','0','0')
SET DATEFORMAT dmy
INSERT INTO PaymentVoucher VALUES ('1','1','1','21/09/2001','100000','No','0')

SELECT * FROM PaymentVoucher

SELECT * FROM Account
SELECT * FROM [Position]
INSERT INTO Position VALUES ('1','Manager','7500000','30','0')
INSERT INTO Position VALUES ('2','Accountant','7500000','30','0')
INSERT INTO Position VALUES ('3','Bartender','7500000','30','0')
INSERT INTO Account VALUES ('1','1','thanhphat219','c4ca4238a0b923820dcc509a6f75849b','0')

SELECT * FROM Account WHERE username='thanhphat219' AND password='c4ca4238a0b923820dcc509a6f75849b' AND isDelete=0 AND idAccount != 0

SELECT * FROM Incident 