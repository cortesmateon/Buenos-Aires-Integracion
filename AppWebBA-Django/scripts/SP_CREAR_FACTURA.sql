USE [base_datos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (OBJECT_ID('SP_CREAR_FACTURA', 'P') IS NOT NULL) DROP PROCEDURE [dbo].[SP_CREAR_FACTURA]
GO

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
GO
