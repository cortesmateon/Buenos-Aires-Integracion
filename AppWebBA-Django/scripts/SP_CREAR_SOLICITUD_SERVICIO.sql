USE [base_datos]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (OBJECT_ID('SP_CREAR_SOLICITUD_SERVICIO', 'P') IS NOT NULL) DROP PROCEDURE [dbo].[SP_CREAR_SOLICITUD_SERVICIO]
GO

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
GO


GO
