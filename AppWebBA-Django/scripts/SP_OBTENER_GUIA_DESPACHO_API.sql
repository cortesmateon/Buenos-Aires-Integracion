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
