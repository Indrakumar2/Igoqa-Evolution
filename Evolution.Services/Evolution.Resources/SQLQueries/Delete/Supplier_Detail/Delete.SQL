DELETE FROM [supplier].[SupplierNote] WHERE SupplierId = {0};
UPDATE common.Document set isDeleted=1 WHERE ModuleRefCode = {0} and ModuleCode='SUP';
DELETE FROM [supplier].[SupplierContact] WHERE SupplierId = {0};
DELETE FROM [supplier].[Supplier] WHERE id = {0};
