CREATE DATABASE OPENXMLTesting
USE [OPENXMLTesting]
GO

/****** Object:  Table [dbo].[Certificado]    Script Date: 24/01/2015 01:14:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Certificado](
	[RFC] [varchar](20) NULL,
	[ValidezObligaciones] [varchar](3) NULL,
	[EstatusCertificado] [varchar](2) NULL,
	[noCertificado] [varchar](20) NULL,
	[FechaInicio] [varchar](50) NULL,
	[FechaFinal] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



CREATE PROCEDURE SP_XML_TO_SLQ
@filePath VARCHAR(300)
AS
BEGIN
	DECLARE @xmlDoc XML

	IF (@filePath = 'FILE1')
		BEGIN
			SELECT @xmlDoc = BulkColumn
			FROM OPENROWSET (
				BULK  'C:\Users\ACER\Documents\GitHub\LCO\A1Limpio.xml', SINGLE_CLOB 
				) AS xmlData
		END
	ELSE IF (@filePath = 'FILE2')
		BEGIN
			SELECT @xmlDoc = BulkColumn
			FROM OPENROWSET (
				BULK  'C:\Users\ACER\Documents\GitHub\LCO\A2Limpio.xml', SINGLE_CLOB 
				) AS xmlData
		END
	ELSE IF (@filePath = 'FILE3')
		BEGIN
			SELECT @xmlDoc = BulkColumn
			FROM OPENROWSET (
				BULK  'C:\Users\ACER\Documents\GitHub\LCO\A3Limpio.xml', SINGLE_CLOB 
				) AS xmlData
		END
	ELSE
		BEGIN
			SELECT @xmlDoc = BulkColumn
			FROM OPENROWSET (
				BULK  'C:\Users\ACER\Documents\GitHub\LCO\A4Limpio.xml', SINGLE_CLOB 
				) AS xmlData
		END

		;WITH XMLNAMESPACES(N'http://www.w3.org/2001/XMLSchema-instance' as xsi, 'http:/www.sat.gob.mx/cfd/LCO' as lco)

	INSERT Certificado(RFC, ValidezObligaciones, EstatusCertificado, noCertificado, FechaInicio, FechaFinal)
	SELECT Contribuyente.value('@RFC', 'VARCHAR(20)') as RFC,
		   Certificado.value('@ValidezObligaciones', 'VARCHAR(3)') as ValidezObligaciones,
		   Certificado.value('@EstatusCertificado', 'VARCHAR(2)') as EstatusCertificado,
		   Certificado.value('@noCertificado', 'VARCHAR(20)') as noCertificado,
		   Certificado.value('@FechaInicio', 'VARCHAR(50)') as FechaInicio,
		   Certificado.value('@FechaFinal', 'VARCHAR(50)') as FechaFinal
	FROM @xmlDoc.nodes('lco:LCO/lco:Contribuyente') as x1(Contribuyente)
	CROSS APPLY x1.Contribuyente.nodes('lco:Certificado') AS x2(Certificado)
END