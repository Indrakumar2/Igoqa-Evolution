DELETE FROM [supplier].[SupplierPurchaseOrderNote] WHERE SupplierPurchaseOrderId = {0};
UPDATE common.Document set isDeleted=1 WHERE ModuleRefCode = {0} and ModuleCode='SUPPO';
DELETE FROM [supplier].[SupplierPurchaseOrderSubSupplier] WHERE SupplierPurchaseOrderId = {0};
DELETE FROM supplier.SupplierPurchaseOrder  WHERE id = {0};
