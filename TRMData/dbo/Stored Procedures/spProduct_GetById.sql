CREATE PROCEDURE [dbo].[spProduct_GetById]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInstock, IsTaxable
	FROM [DBO].[Product]
	WHERE Id = @id;
END