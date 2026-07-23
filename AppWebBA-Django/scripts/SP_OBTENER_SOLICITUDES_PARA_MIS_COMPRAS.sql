CREATE PROCEDURE SP_OBTENER_SOLICITUDES_PARA_MIS_COMPRAS
AS
BEGIN
    SELECT nrosol, tiposol, fechavisita, descsol, estadosol, nrofac,ruttec
    FROM SolicitudServicio;
END
