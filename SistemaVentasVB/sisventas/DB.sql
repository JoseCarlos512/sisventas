

CREATE DATABASE dbventas

GO

USE dbventas;

GO


CREATE TABLE producto(
	idproducto int primary key identity,
	idcategoria int,
	nombre varchar(50),
	descripcion varchar(50),
	stock decimal(18,2),
	precio_compra decimal(18,2),
	precio_venta decimal(18,2),
	fecha_vencimiento date,
	fecha_registro datetime default getdate()
)

GO



CREATE TABLE categoria(
	idcategoria int primary key identity, 
	nombre_categoria varchar(50),
	fecha_registro datetime default getdate()
)

GO


CREATE TABLE cliente(
	idcliente int primary key identity, 
	nombre varchar(50),
	apellidos varchar(50),
	direccion varchar(100),
	telefono varchar(9),
	dni varchar(8),
	fecha_registro datetime default getdate()
)

GO


CREATE TABLE ventas(
	idventa int primary key identity, 
	idcliente int,
	fecha_venta date,
	tipo_documento varchar(50),
	num_documento varchar(50),
	fecha_registro datetime default getdate()
)


GO


CREATE TABLE detalle_venta(
	iddetalle_venta int primary key identity, 
	idventa int,
	idproducto int,
	cantidad decimal(18,2),
	precio_unitario decimal(18,2),
	fecha_registro datetime default getdate()
)



GO

CREATE TABLE usuario(
	idusuario int primary key identity, 
	nombre varchar(50),
	apellidos varchar(50),
	dni varchar(8),
	direccion varchar(100),
	telefono varchar(9),
	login varchar(50),
	password varchar(50),
	fecha_registro datetime default getdate()
)
GO


-- CREAR PROCEDURE MOSTRAR CLIENTE
create proc mostrar_cliente
as
select * from cliente order by idcliente desc

go

-- CREAR PROCEDURE INSERT CLIENTE
create proc insertar_cliente
@nombre varchar(50),
@apellidos varchar(50),
@direccion varchar(100),
@telefono varchar(10),
@dni varchar(8)
as
insert into cliente (nombre, apellidos, direccion, telefono, dni)
values (@nombre, @apellidos, @direccion, @telefono, @dni)

go

-- CREAR PROCEDURE EDITAR
create proc editar_cliente
@idcliente integer,
@nombre varchar(50),
@apellidos varchar(50),
@direccion varchar(100),
@telefono varchar(9),
@dni varchar(8)
as
update cliente set 
nombre = @nombre,
apellidos = @apellidos,
direccion = @direccion,
telefono = @telefono,
dni = @dni
where idcliente = @idcliente

go


-- PROCEDURE ELIMINAR CLIENTE
create proc eliminar_cliente
@idcliente integer
as
delete from cliente where idcliente=@idcliente
go


-- PROCEDURES DE UN CRUD CATEGORIA
create proc mostrar_categoria
as
select * from categoria order by idcategoria desc
go

create proc insertar_categoria
@nombre_categoria varchar(50)
as
insert into categoria(nombre_categoria) values(@nombre_categoria)
go

create proc editar_categoria
@idcategoria integer,
@nombre_categoria varchar(50)
as
update categoria set nombre_categoria = @nombre_categoria
where idcategoria = @idcategoria
go

create proc eliminar_categoria
@idcategoria integer
as
delete from categoria where idcategoria=@idcategoria
go

