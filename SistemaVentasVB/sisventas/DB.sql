

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


-- PRODUCTO

alter table producto 
add imagen image
go

create proc mostrar_producto 
as
select producto.idproducto, producto.idcategoria, categoria.nombre_categoria,
	   producto.nombre, producto.descripcion, producto.stock, producto.precio_compra,
	   producto.precio_venta, producto.fecha_vencimiento
from producto
inner join categoria
on producto.idcategoria = categoria.idcategoria
order by producto.idcategoria desc
go

create proc insertar_producto
@idcategoria integer,
@nombre varchar(50),
@descripcion varchar(50),
@stock decimal(18,2),
@precio_compra decimal(18,2),
@precio_venta decimal(18,2),
@fecha_vencimiento date,
@imagen image
as
insert into producto(idcategoria,nombre,descripcion,stock,precio_compra,precio_venta,fecha_vencimiento,imagen) 
values(@idcategoria,@nombre,@descripcion,@stock,@precio_compra,@precio_venta,@fecha_vencimiento,@imagen)
go

create proc editar_producto
@idproducto integer,
@idcategoria integer,
@nombre varchar(50),
@descripcion varchar(50),
@stock decimal(18,2),
@precio_compra decimal(18,2),
@precio_venta decimal(18,2),
@fecha_vencimiento date,
@imagen image
as
update producto
set idcategoria = @idcategoria,
nombre = @nombre,
descripcion = @descripcion,
stock = @stock,
precio_compra = @precio_compra,
precio_venta = @precio_venta,
fecha_vencimiento = @fecha_vencimiento,
imagen = @imagen
where idproducto = @idproducto
go



create proc eliminar_producto
@idproducto integer
as
delete from producto where idproducto=@idproducto
go

-- VENTA
select * from ventas
go

create proc insertar_venta
@idcliente as integer,
@fecha_venta as date,
@tipo_documento as varchar(50),
@num_documento as varchar(50)
as
insert into ventas (idcliente, fecha_venta, tipo_documento, num_documento)
values(@idcliente, @fecha_venta, @tipo_documento, @num_documento)
go

create proc editar_venta
@idventa as integer,
@idcliente as integer,
@fecha_venta as date,
@tipo_documento as varchar(50),
@num_documento as varchar(50)
as
update ventas 
set idcliente=@idcliente,fecha_venta=@fecha_venta,tipo_documento=@tipo_documento,
	num_documento=@num_documento 
where idventa = @idventa
go

create proc eliminar_venta
@idventa as integer 
as
delete from ventas where idventa=@idventa
go


create proc mostrar_venta
as
select 
ventas.idventa, ventas.idcliente, cliente.apellidos, cliente.dni, ventas.fecha_venta, 
ventas.tipo_documento, ventas.num_documento
from ventas
inner join cliente
on ventas.idcliente = cliente.idcliente
go

-- DETALLE VENTA
select * from detalle_venta
go

create proc insertar_detalle_venta
@idventa as integer,
@idproducto as integer,
@cantidad as decimal(18,2),
@precio_unitario as decimal(18,2)
as
insert into detalle_venta(idventa,idproducto,cantidad,precio_unitario)
values(@idventa,@idproducto,@cantidad,@precio_unitario)
go

create proc editar_detalle_venta
@oddetalle_venta as integer,
@idventa as integer,
@idproducto as integer,
@cantidad as decimal(18,2),
@precio_unitario as decimal(18,2)
as
update detalle_venta 
set idventa=@idventa,idproducto=@idproducto,cantidad=@cantidad,precio_unitario=@precio_unitario
where iddetalle_venta=iddetalle_venta
go

create proc eliminar_detalle_venta
@iddetalle_venta as integer
as
delete from detalle_venta 
where iddetalle_venta=@iddetalle_venta
go

create proc mostrar_detalle_venta
as
select * from detalle_venta order by iddetalle_venta desc
go

create proc aumentar_stock
@idproducto as integer,
@cantidad as decimal(18,2)
as
update producto set stock=stock+@cantidad
where idproducto=@idproducto
go

create proc disminuir_stock
@idproducto as integer,
@cantidad as decimal(18,2)
as
update producto set stock=stock-@cantidad
where idproducto=@idproducto
go
















