DELETE  from assignment.AssignmentNote where AssignmentId={0};
UPDATE common.Document set IsDeleted=1 where ModuleRefCode={0} and ModuleCode='ASGMNT';
DELETE  from assignment.AssignmentContractSchedule where AssignmentId={0};
DELETE  from assignment.AssignmentTaxonomy where AssignmentId={0};
DELETE  from assignment.AssignmentReference where AssignmentId={0};
DELETE  from assignment.AssignmentAdditionalExpense where AssignmentId={0};
DELETE ASUB from assignment.AssignmentSubSupplierTechnicalSpecialist ASUB
INNER JOIN assignment.AssignmentSubSupplier ASS on ASS.Id = ASUB.AssignmentSubSupplierId
WHERE ASS.AssignmentId = {0};
DELETE  from assignment.AssignmentSubSupplier where AssignmentId={0};
DELETE ATS from assignment.AssignmentTechnicalSpecialistSchedule ATS
INNER JOIN assignment.AssignmentTechnicalSpecialist AT on AT.Id = ATS.AssignmentTechnicalSpecialistId
WHERE AT.AssignmentId = {0};
DELETE  from assignment.AssignmentTechnicalSpecialist where AssignmentId ={0};
DELETE  from assignment.AssignmentMessage where AssignmentId ={0};
DELETE  from assignment.Assignment where id={0};
