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
    namePosititon NVARCHAR(100),
    salary REAL,
    standardWorkDays REAL,
    isDelete BIT,
    CONSTRAINT PK_Position PRIMARY KEY(idPosition)
)

CREATE TABLE Account (
    idAccount INT,
    idEmployee INT,
    username NVARCHAR(100),
    password NVARCHAR(100),
    isDelete BIT,
    CONSTRAINT PK_Account PRIMARY KEY(idAccount)
)

CREATE TABLE Employee (
    idEmployee INT,
    idPosition INT,
    name NVARCHAR(100),
    dateOfBirth SMALLDATETIME,
    dateStartWorking SMALLDATETIME,
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
    quantity INT,
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
    emailSupplier NVARCHAR(100),
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
    idMateial INT,
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
    idGood INT,
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
    quantity INT,
    price REAL,
    status BIT,
    isDelelte BIT,
    CONSTRAINT PK_Material PRIMARY KEY(idMaterial)
)
-- DROP TABLE Account
-- DROP TABLE Employee
-- DROP TABLE GoodType
-- DROP TABLE Position 
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

ALTER TABLE Account ADD CONSTRAINT FK_Account_Employee FOREIGN KEY(idAccount) REFERENCES Account(idAccount)

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

