create database E_Commerce_App
use E_Commerce_App

--- E-Commerce Application ----

--- table 1

create table Customers(
Customer_ID int primary key identity(1001,1),
Customer_Name varchar(50),
Email varchar(100),
Password varchar(50))

--- table 2

create  table Products(
Product_ID int primary key identity(2501,1),
Product_Name varchar(50),
Product_Price decimal(10,2),
Description varchar(50),
Stock_Quant int )

--- table 3

create table Cart(
Cart_ID int primary key identity(3001,1),
Customer_ID int foreign key references customers(customer_ID),
Product_ID int foreign key references products(product_Id),
Quantity int)

--- table 4

create table Orders(
Order_ID int primary key identity(501,1),
Customer_ID int foreign key references customers(customer_ID),
Order_Date date,
Total_price decimal(10,2),
Shipping_access varchar(50))

--- table 5

create table Order_items(
Order_Items_ID int primary key identity(41,1),
Order_ID int foreign key references orders(Order_ID),
Product_ID int foreign key references Products(Product_ID),
OD_Quantity int)



select * from Customers
select * from Products
select * from Cart
select * from Orders
select * from Order_items




