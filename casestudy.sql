create database Ecom_App;

use Ecom_App;

create table customers(
customer_id int not null identity(1,1) primary key,
name varchar(50) not null,
email varchar(100) not null,
password varchar(25) not null
);

create table products(
product_id int not null identity(1,1) primary key,
name varchar(50) not null,
price decimal not null,
description varchar(255) not null,
stockQuantity int not null
);


Create table Cart(
cart_id int not null identity(1,1) primary key,
customer_id int not null,
product_id int not null,
Quantity int not null
Constraint fk_customers_cart foreign key(customer_id) references customers(customer_id) on delete cascade on update cascade,
Constraint fk_products_cart foreign key(product_id) references products(product_id) on delete cascade on update cascade
);

Create table Orders(
order_id int not null identity(1,1) primary key,
customer_id int not null,
order_date datetime not null,
total_price decimal not null,
shipping_address text not null,
Constraint fk_customers_orders foreign key(customer_id) references customers(customer_id) on delete cascade on update cascade
);

Create table Order_items(
order_item_id int not null identity(1,1) primary key,
order_id int not null,
product_id int not null,
Quantity int not null,
Constraint fk_products_orderitems foreign key(product_id) references products(product_id) on delete cascade on update cascade,
Constraint fk_orders_orderitems foreign key(order_id) references orders(order_id) on delete cascade on update cascade
);