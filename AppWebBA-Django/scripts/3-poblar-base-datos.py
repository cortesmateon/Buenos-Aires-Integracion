from django.db import connection

SP_OBTENER_EQUIPOS_EN_BODEGA = """
CREATE PROCEDURE [dbo].[SP_OBTENER_EQUIPOS_EN_BODEGA]
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    p.idprod,
    p.nomprod,
    p.descprod,
    p.precio, 
    p.imagen, 
    COUNT(s.idprod) AS cantidad, 
    CASE 
        WHEN COUNT(s.idprod) = 0 
        THEN 'AGOTADO' 
        ELSE 'DISPONIBLE' 
    END AS disponibilidad
FROM
    Producto p
    LEFT JOIN (SELECT * FROM StockProducto WHERE nrofac IS NULL) s on p.idprod = s.idprod
GROUP BY
    p.idprod,
    p.nomprod,
    p.descprod,
    p.precio,
    p.imagen
ORDER BY p.idprod


END
"""

SP_OBTENER_PRODUCTOS = """
CREATE PROCEDURE [dbo].[SP_OBTENER_PRODUCTOS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Producto
END
"""

SP_OBTENER_SOLICITUDES_DE_SERVICIO = """
CREATE PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_DE_SERVICIO]
	@tipousu VARCHAR(50),
	@rut     VARCHAR(50)
AS
BEGIN
	/*
		Ejemplos de ejecucion del procedimiento:
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '6666-6'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '7777-7'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Técnico', '8888-8'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Cliente', '1111-1'
		EXEC SP_OBTENER_SOLICITUDES_DE_SERVICIO 'Todos', ''
	*/

	SET NOCOUNT ON;

	IF (@tipousu = 'Técnico')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			sol.fechavisita, 
			'10:00:00'                                  AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		WHERE
			pertec.rut = @rut
		ORDER BY 
			usucli.first_name

	IF (@tipousu = 'Cliente')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			sol.fechavisita, 
			'10:00:00'                                  AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		WHERE
			percli.rut = @rut
		ORDER BY 
			usutec.first_name

	IF (@tipousu = 'Todos')
		SELECT 
			sol.nrosol, 
			usucli.first_name + ' '  + usucli.last_name AS nomcli, 
			sol.tiposol,
			sol.fechavisita, 
			'10:00:00'                                  AS hora,
			usutec.first_name + ' '  + usutec.last_name AS nomtec,
			sol.descsol       + ': ' + fac.descfac      AS descser,
			sol.estadosol
		FROM 
			SolicitudServicio sol 
			INNER JOIN Factura       fac    ON sol.nrofac     = fac.nrofac
			INNER JOIN PerfilUsuario percli ON fac.rutcli     = percli.rut
			INNER JOIN PerfilUsuario pertec ON sol.ruttec     = pertec.rut
			INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
			INNER JOIN auth_user     usutec ON pertec.user_id =  usutec.id
		ORDER BY
			usucli.first_name,
			usutec.first_name

END
"""

SP_OBTENER_TODOS_LOS_USUARIOS = """
CREATE PROCEDURE [dbo].[SP_OBTENER_TODOS_LOS_USUARIOS]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		*
	FROM 
		PerfilUsuario per
		INNER JOIN auth_user usu ON per.user_id = usu.id
END
"""

SP_RESERVAR_EQUIPO_ANWO = """
CREATE PROCEDURE [dbo].[SP_RESERVAR_EQUIPO_ANWO]
    @nroserieanwo NVARCHAR(100),
	@reservado NVARCHAR(1)
AS
BEGIN
	/*
		Ejemplos de ejecucion del procedimiento:
		
        EXEC SP_RESERVAR_EQUIPO_ANWO 'A9', 'S'
        SELECT * FROM AnwoStockProducto WHERE nroserieanwo = 'A9' --DEBE ENTREGAR UNA FILA CON reservado = 'S'

		EXEC SP_RESERVAR_EQUIPO_ANWO 'A9', 'N'
        SELECT * FROM AnwoStockProducto WHERE nroserieanwo = 'A9' --DEBE ENTREGAR UNA FILA CON reservado = 'N'
	*/

    SET NOCOUNT ON;

    UPDATE AnwoStockProducto
    SET reservado = @reservado
    WHERE nroserieanwo = @nroserieanwo;
END
"""


SP_CREAR_FACTURA = """
CREATE PROCEDURE [dbo].SP_CREAR_FACTURA
    @nrofac INT,
    @fechafac DATE,
    @descfac NVARCHAR(300),
    @monto INT,
    @rutcli NVARCHAR(20),
    @idprod INT
AS
BEGIN
        -- Insertar la nueva factura
        INSERT INTO Factura (nrofac, fechafac, descfac, monto, rutcli, idprod)
        VALUES (@nrofac, @fechafac, @descfac, @monto, @rutcli, @idprod);
        
END;
"""

SP_CREAR_GUIA_DESPACHO = """
CREATE PROCEDURE [dbo].SP_CREAR_GUIA_DESPACHO
    @nrogd INT,
    @estadogd NVARCHAR(20),
    @nrofac INT,
    @idprod INT

AS
BEGIN
    INSERT INTO GuiaDespacho (nrogd,estadogd, nrofac, idprod)
    VALUES (@nrogd,@estadogd, @nrofac, @idprod);
    
END;
"""

SP_CREAR_SOLICITUD_SERVICIO = """
CREATE PROCEDURE [dbo].SP_CREAR_SOLICITUD_SERVICIO
    @nrosol INT,
    @tiposol NVARCHAR(50),
    @fechavisita DATE,
    @descsol NVARCHAR(200),
    @estadosol NVARCHAR(200),
    @nrofac INT,
    @ruttec NVARCHAR(15)
AS
BEGIN

    -- Crear la solicitud de servicio
    INSERT INTO SolicitudServicio (nrosol, tiposol, fechavisita, descsol, estadosol, nrofac,ruttec)
    VALUES (@nrosol,@tiposol, @fechavisita, @descsol, @estadosol, @nrofac, @ruttec);

END;
"""

SP_OBTENER_FACTURAS = """
CREATE PROCEDURE SP_OBTENER_FACTURAS
AS
BEGIN
    SELECT nrofac, fechafac, descfac, monto, rutcli, idprod
    FROM Factura;
END
"""

SP_OBTENER_GUIA_DESPACHO = """
CREATE PROCEDURE SP_OBTENER_GUIA_DESPACHO
AS
BEGIN
    SELECT nrogd, estadogd,nrofac,idprod
    FROM GuiaDespacho;
END
"""

SP_OBTENER_SOLICITUDES_PARA_MIS_COMPRAS = """
CREATE PROCEDURE SP_OBTENER_SOLICITUDES_PARA_MIS_COMPRAS
AS
BEGIN
    SELECT nrosol, tiposol, fechavisita, descsol, estadosol, nrofac,ruttec
    FROM SolicitudServicio;
END
"""

SP_OBTENER_GUIA_DESPACHO_API = """
CREATE PROCEDURE SP_OBTENER_GUIA_DESPACHO_API
AS
BEGIN
    SELECT g.nrogd AS nrogd, p.descprod AS descprod, g.estadogd AS estadogd, f.nrofac AS nrofac, usucli.first_name +' '+ usucli.last_name AS Cliente
    FROM Factura f JOIN GuiaDespacho g
    on(f.nrofac=g.nrofac)
    JOIN Producto p 
    on(p.idprod=g.idprod)
    INNER JOIN PerfilUsuario percli ON f.rutcli = percli.rut
	INNER JOIN auth_user     usucli ON percli.user_id =  usucli.id
;
END
"""

SP_ACTUALIZAR_ESTADO_GUIA_DESPACHO="""
CREATE OR ALTER PROCEDURE SP_ACTUALIZAR_ESTADO_GUIA_DESPACHO
@nrogd int,
@estadogd NVARCHAR(50)
AS
BEGIN
    UPDATE GuiaDespacho 
	SET estadogd = @estadogd 
    WHERE nrogd=@nrogd
END
"""

def exec_sql(query):
    with connection.cursor() as cursor:
        cursor.execute(query)

def run():

    # Poblar tabla Producto

    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (1,N'Aire Wifi 9000 btu',  N'Enfría 9-16 m2',     299990,'images/0001.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (2,N'Split Inv 12000 btu', N'Frío/Calor AR12',    450000,'images/0002.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (3,N'Anwo VP',             N'9000 Virus Protect', 288990,'images/0003.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (4,N'Anwo Portátil',       N'12000 btu f/c',      131990,'images/0004.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (5,N'GPORT-14',            N'Anwo 14000 btu',     399990,'images/0005.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (6,N'Kendal 12000',        N'Climat 22-24 m2',    335990,'images/0006.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (7,N'Kendal Wifi 4000',    N'Climat 32-34 m2',    335990,'images/0006.png');")
    exec_sql("INSERT INTO dbo.Producto (idprod, nomprod, descprod, precio, imagen) VALUES (8,N'Anwo 12000',          N'Climat 42-44 m2',    335990,'images/0006.png');")

    # Poblar tabla PerfilUsuario

    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'1234-5',  N'Superusuario',  N'La Florida',  1);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'1111-1',  N'Cliente',       N'La Florida',  2);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'2222-2',  N'Cliente',       N'Santiago',    3);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'3333-3',  N'Cliente',       N'Providencia', 4);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'4444-4',  N'Cliente',       N'Las Condes',  5);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'5555-5',  N'Cliente',       N'Maipú',       6);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'6666-6',  N'Técnico',       N'Cerro Navia', 7);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'7777-7',  N'Técnico',       N'Peñalolén',   8);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'8888-8',  N'Técnico',       N'La Reina',    9);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'9999-9',  N'Bodeguero',     N'La Florida',  10);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'0000-0',  N'Administrador', N'La Reina',    11);")
    exec_sql("INSERT INTO dbo.PerfilUsuario (rut, tipousu, dirusu, user_id) VALUES (N'1234-0',  N'Vendedor', N'La Reina',    12);")

    # Poblar tabla Factura

    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (1, N'1111-1', 1, CAST('20220223' AS datetime), N'Aire Wifi 9000 btu',  25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (2, N'2222-2', 2, CAST('20220224' AS datetime), N'Split Inv 12000 btu', 450000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (3, N'3333-3', 4, CAST('20220303' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (4, N'4444-4', 4, CAST('20220308' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (5, N'5555-5', 4, CAST('20220315' AS datetime), N'Anwo Portátil',       25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (6, N'1111-1', 5, CAST('20220327' AS datetime), N'Mantención',          25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (7, N'1111-1', 5, CAST('20220403' AS datetime), N'GPORT-14',            25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (8, N'1111-1', 3, CAST('20220408' AS datetime), N'Anwo VP',             25000);")
    exec_sql("INSERT INTO dbo.Factura (nrofac, rutcli, idprod, fechafac, descfac, monto) VALUES (9, N'1111-1', 5, CAST('20220415' AS datetime), N'GPORT-14',            25000);")

    # Poblar tabla GuiaDespacho

    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (11, 1, 1, N'Entregado');")
    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (22, 2, 2, N'Depachado');")
    exec_sql("INSERT INTO dbo.GuiaDespacho (nrogd, nrofac, idprod, estadogd) VALUES (88, 8, 3, N'EnBodega');")

    # Poblar tabla SolicitudServicio

    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (111, 1, N'Instalación', CAST('20220302 09:00' AS datetime), N'6666-6', N'Instalar equipo', N'Cerrada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (222, 2, N'Instalación', CAST('20220303 10:00' AS datetime), N'6666-6', N'Instalar equipo', N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (333, 3, N'Mantención',  CAST('20220310 15:00' AS datetime), N'6666-6', N'Cambiar enchufe', N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (444, 4, N'Mantención',  CAST('20220315 09:00' AS datetime), N'6666-6', N'Limpiar filtro',  N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (555, 5, N'Reparación',  CAST('20220322 17:00' AS datetime), N'6666-6', N'Reparar equipo',  N'Aceptada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (666, 6, N'Mantención',  CAST('20220403 11:00' AS datetime), N'7777-7', N'Limpiar filtro',  N'Cerrada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (777, 7, N'Reparación',  CAST('20220410 16:00' AS datetime), N'6666-6', N'Reparar aire',    N'Modificada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (888, 8, N'Instalación', CAST('20220415 10:00' AS datetime), N'7777-7', N'Instalar equipo', N'Modificada');")
    exec_sql("INSERT INTO dbo.SolicitudServicio (nrosol, nrofac, tiposol, fechavisita, ruttec, descsol, estadosol) VALUES (999, 9, N'Reparación',  CAST('20220422 18:00' AS datetime), N'8888-8', N'Enchufe quemado', N'Aceptada');")

    # Poblar tabla StockProducto

    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (11111, 1, 1);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (22222, 2, 2);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (88888, 3, 8);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90001, 1, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90002, 3, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90003, 4, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90004, 6, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90005, 1, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90006, 3, null);")
    exec_sql("INSERT INTO dbo.StockProducto (idstock, idprod, nrofac) VALUES (90007, 4, null);")

    # Poblar tabla AnwoStockProducto

    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A1', 'Aire Anwo 111',  '500000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A2', 'Aire Anwo 222',  '400000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A3', 'Aire Anwo 333',  '300000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A4', 'Aire Anwo 444',  '200000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A5', 'Aire Anwo 555',  '500000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A6', 'Aire Anwo 666',  '400000', 'N');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A7', 'Aire Anwo 777',  '300000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A8', 'Aire Anwo 888',  '200000', 'S');")
    exec_sql("INSERT INTO dbo.ANWOSTOCKPRODUCTO (NROSERIEANWO, NOMPRODANWO, PRECIOANWO, RESERVADO) VALUES ('A9', 'Aire Anwo 999',  '500000', 'N');")

try:
    exec_sql(SP_OBTENER_EQUIPOS_EN_BODEGA)
except:
    pass

try:
    exec_sql(SP_OBTENER_PRODUCTOS)
except:
    pass

try:
    exec_sql(SP_OBTENER_SOLICITUDES_DE_SERVICIO)
except:
    pass
    
try:
    exec_sql(SP_OBTENER_TODOS_LOS_USUARIOS)
except:
    pass
    
try:
    exec_sql(SP_RESERVAR_EQUIPO_ANWO)
except:
    pass

try:
    exec_sql(SP_CREAR_GUIA_DESPACHO)
except:
    pass

try:
    exec_sql(SP_CREAR_FACTURA)
except:
    pass

try:
    exec_sql(SP_CREAR_SOLICITUD_SERVICIO)
except:
    pass

try:
    exec_sql(SP_OBTENER_FACTURAS)
except:
    pass

try:
    exec_sql(SP_OBTENER_GUIA_DESPACHO)
except:
    pass

try:
    exec_sql(SP_OBTENER_SOLICITUDES_PARA_MIS_COMPRAS)
except:
    pass


try:
    exec_sql(SP_OBTENER_GUIA_DESPACHO_API)
except:
    pass


try:
    exec_sql(SP_ACTUALIZAR_ESTADO_GUIA_DESPACHO)
except:
    pass