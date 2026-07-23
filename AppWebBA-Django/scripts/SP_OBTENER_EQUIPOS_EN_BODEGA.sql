USE [base_datos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (OBJECT_ID('SP_OBTENER_EQUIPOS_EN_BODEGA', 'P') IS NOT NULL) DROP PROCEDURE [dbo].[SP_OBTENER_EQUIPOS_EN_BODEGA]
GO
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
GO