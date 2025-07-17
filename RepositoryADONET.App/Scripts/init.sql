create database [RepositoryADONET.App]
go

use [RepositoryADONET.App]
go

create table Customer
(
	Id int not null identity(1, 1),
	[Name] varchar(100) not null,
	[Email] varchar(100) not null
)
go

insert into Customer([Name], [Email]) Values('Misael C. Homem', 'misael@email.com')
go

select * from Customer
go
