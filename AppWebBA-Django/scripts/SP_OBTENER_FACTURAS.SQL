CREATE PROCEDURE SP_OBTENER_FACTURAS
AS
BEGIN
    SELECT nrofac, fechafac, descfac, monto, rutcli, idprod
    FROM Factura;
END
