DELETE FROM contract.ContractNote WHERE ContractId = {0};
UPDATE common.Document set isDeleted=1 WHERE ModuleRefCode = {1} and ModuleCode='CNT';
DELETE CR from[contract].[ContractRate][CR]
INNER JOIN[contract].[ContractSchedule] CS on Cs.Id = CR.ContractScheduleId
WHERE CS.ContractId = {0};
DELETE FROM contract.ContractSchedule WHERE ContractId = {0};
DELETE FROM contract.ContractInvoiceAttachment WHERE ContractId = {0};
DELETE FROM contract.ContractInvoiceReference WHERE ContractId = {0};
DELETE FROM contract.ContractMessage WHERE ContractId = {0};
DELETE FROM contract.ContractExchangeRate WHERE ContractId = {0};
DELETE FROM contract.Contract WHERE id = {0};