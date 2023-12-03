create table tbl_color(
	color_id int primary key not null identity(1,1),
	color_name nvarchar(255) not null
);

create table tbl_customer(
	cust_id int primary key not null identity(1,1),
	cust_name nvarchar(100) not null,
	cust_lname nvarchar(255) not null,
	cust_email varchar(100) not null,
	cust_phone varchar(50) not null,
	cust_address text not null,
	cust_city nvarchar(100) not null,
	cust_sate nvarchar(100) not null,
	cust_zip nvarchar(30) not null,
	cust_datetime varchar(100) not null,
	cust_status int not null
);

create table tbl_user_customer(
	cust_id int not null,
	user_id int not null,
	constraint fk_cust_user foreign key (cust_id) 
	references tbl_customer(cust_id),
	constraint fk_user_cust foreign key (user_id) 
	references tbl_user(user_id)
);

create table tbl_user(
	user_id int not null primary key identity(1,1),
	user_name nvarchar(255) not null,
	user_email varchar(255) not null,
	user_phone varchar(100) not null,
	user_pass varchar(255) not null,
	user_photo varchar(255) not null,
	user_role varchar(30) not null,
	user_status varchar(10) not null
);



create table tbl_cust_message(
	cust_msg_id int primary key not null identity(1,1),
	subject_msg nvarchar(255) not null,
	msg text not null,
	cust_id int not null
);

create table tbl_faq(
	faq_id int primary key not null identity(1,1),
	faq_title nvarchar(255),
	faq_content text not null
);

create table tbl_top_category(
	tcat_id int primary key not null identity(1,1),
	tcat_name nvarchar(255) not null,
	show_on_menu int
);
create table tbl_mid_category(
	mcat_id int primary key not null identity(1,1),
	mcat_name nvarchar(255) not null,
	tcat_id int,
	constraint fk_tcat_id
	foreign key (tcat_id) references tbl_top_category (tcat_id)
);

create table tbl_end_category(
	ecat_id int primary key not null identity(1,1),
	ecat_name nvarchar(255) not null,
	mcat_id int,
	constraint fk_mcat_id
	foreign key (mcat_id) references tbl_mid_category (mcat_id)
);

create table tbl_product_pic(
	pic_id int primary key not null identity(1,1),
	pic_name varchar(255) not null,
	product_id int,
	constraint fk_product_id
	foreign key (product_id) references tbl_product(product_id)
);

create table tbl_product(
	product_id int primary key not null identity(1,1),
	product_name nvarchar(255) not null,
	product_pic varchar(255) not null,
	color nvarchar(100) not null,
	stock_quantity int not null,
	ecat_id int not null,
	product_des text,
	product_status nvarchar(100),
	constraint fk_ecat_id foreign key (ecat_id) references tbl_end_category(ecat_id)
); 

create table tbl_cart(
	cart_id int primary key not null identity(1,1),
	cart_date varchar(100) not null,
	total_price varchar(255) not null,
	cust_id int not null,
	constraint fk_cust_id foreign key (cust_id) references tbl_customer(cust_id)
);

create table tbl_cart_detail(
	cart_detail_id int primary key not null identity(1,1),
	cart_id int not null,
	product_id int not null,
	product_name nvarchar(255) not null,
	product_pic varchar(255) not null,
	color nvarchar(100) not null,
	quantity int not null,
	constraint fk_cart_id foreign key (cart_id) references tbl_cart(cart_id),
	constraint fk_productC_id foreign key (product_id) references tbl_product(product_id)
);

create table tbl_order(
	order_id int primary key not null identity(1,1),
	cart_id int not null,
	cust_id int not null,
	cust_name nvarchar(255) not null,
	shiping_address nvarchar(255) not null,
	shiping_city nvarchar(100) not null,
	total_price varchar(255) not null,
	order_status nvarchar(255) not null,
	constraint fk_cartO_id foreign key (cart_id) references tbl_cart(cart_id),
	constraint fk_custO_id foreign key (cust_id) references tbl_customer(cust_id)
);

create table tbl_promotion(
	promo_id int primary key not null identity(1,1),
	promo_name nvarchar(255) not null,
	promo_discount int not null,
	promo_sdate varchar(255) not null,
	promo_edate varchar(255) not null
);

create table tbl_products_promotion(
	promo_id int not null,
	product_id int not null,
	constraint fk_promo_id foreign key (promo_id)
	references tbl_promotion(promo_id),
	constraint fk_pro2_id foreign key (product_id)
	references tbl_product(product_id)
);

create table tbl_poster(
	post_id int primary key not null identity(1,1),
	post_pic varchar(255) not null
);

insert into tbl_product values
('testing1','1','blue',100,2,'testing','Active'),
('testing2','1','blue',100,2,'testing','Active'),
('testing3','1','blue',100,2,'testing','Active'),
('testing4','1','blue',100,2,'testing','Active'),
('testing5','1','blue',100,2,'testing','Active'),
('testing6','1','blue',100,2,'testing','Active'),
('testing7','1','blue',100,2,'testing','Active'),
('testing8','1','blue',100,2,'testing','Active'),
('testing9','1','blue',100,2,'testing','Active'),
('testing10','1','blue',100,2,'testing','Active'),
('testing11','1','blue',100,2,'testing','Active'),
('testing12','1','blue',100,2,'testing','Active'),
('testing13','1','blue',100,2,'testing','Active'),
('testing14','1','blue',100,2,'testing','Active'),
('testing15','1','blue',100,2,'testing','Active'),
('testing16','1','blue',100,2,'testing','Active'),
('testing17','1','blue',100,2,'testing','Active'),
('testing18','1','blue',100,2,'testing','Active'),
('testing19','1','blue',100,2,'testing','Active'),
('testing20','1','blue',100,2,'testing','Active'),
('testing21','1','blue',100,2,'testing','Active'),
('testing22','1','blue',100,2,'testing','Active'),
('testing23','1','blue',100,2,'testing','Active'),
('testing24','1','blue',100,2,'testing','Active'),
('testing25','1','blue',100,2,'testing','Active'),
('testing26','1','blue',100,2,'testing','Active'),
('testing27','1','blue',100,2,'testing','Active'),
('testing28','1','blue',100,2,'testing','Active'),
('testing29','1','blue',100,2,'testing','Active'),
('testing30','1','blue',100,2,'testing','Active')