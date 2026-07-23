CREATE PROCEDURE SP_OBTENER_GUIA_DESPACHO
AS
BEGIN
    SELECT nrogd, estadogd,nrofac,idprod
    FROM GuiaDespacho;
END