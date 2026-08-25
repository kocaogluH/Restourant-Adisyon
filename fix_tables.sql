CREATE TABLE category (
    catID int primary key identity,
    catName varchar(50)
);

CREATE TABLE products (
    pID int primary key identity,
    pName varchar(50),
    pPrice float,
    CategoryID int,
    pImage image
);

CREATE TABLE tables (
    tID int primary key identity,
    tName varchar(50)
);

CREATE TABLE users (
    userID int primary key identity,
    username varchar(50),
    upass varchar(50),
    uName varchar(50),
    uphone varchar(50),
    uRole varchar(50)
);

INSERT INTO users (username, upass, uName, uRole) VALUES ('admin', '123', 'Administrator', 'Admin');
