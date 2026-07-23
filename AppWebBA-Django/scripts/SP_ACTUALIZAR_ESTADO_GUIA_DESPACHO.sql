CREATE OR ALTER PROCEDURE SP_ACTUALIZAR_ESTADO_GUIA_DESPACHO
@nrogd int,
@estadogd NVARCHAR(50)
AS
BEGIN
    UPDATE GuiaDespacho 
	SET estadogd = @estadogd 
    WHERE nrogd=@nrogd
END
