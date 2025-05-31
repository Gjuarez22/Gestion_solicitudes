USE GestionS
GO

CREATE TABLE Rol(
    id int identity(1,1) primary key,
    nombre varchar(100)

)

ALTER TABLE Usuario
add idrol int

ALTER TABLE Usuario
add constraint fk_rol
FOREIGN KEY(idrol)
REFERENCES ROL(id)

INSERT INTO rol(nombre)values('Admin'),('user');
GO

INSERT INTO Usuario(usuario,clave,nomnbre,idRol)
VALUES
('admin','0000','Adminsitrador',1),
('usuario','0000','Usuario',2);
GO