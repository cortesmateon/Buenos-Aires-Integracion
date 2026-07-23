USE [base_datos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (OBJECT_ID('SP_CREAR_GUIA_DESPACHO', 'P') IS NOT NULL) DROP PROCEDURE [dbo].[SP_CREAR_GUIA_DESPACHO]
GO


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
GO



GO
