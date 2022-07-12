DELETE FROM [project].[ProjectNote] WHERE ProjectId = ({0}); 
UPDATE common.Document set isDeleted=1 WHERE ModuleRefCode = ({0}) and ModuleCode='PRJ';
DELETE FROM [project].[ProjectClientNotification] WHERE ProjectId = ({0}); 
DELETE FROM [project].[ProjectInvoiceAttachment] WHERE ProjectId = ({0}); 
DELETE FROM [project].[ProjectInvoiceAssignmentReference] WHERE ProjectId = ({0}); 
DELETE FROM [project].[ProjectMessage] WHERE ProjectId = ({0}); 
DELETE FROM [project].[Project] WHERE id = ({0}); 