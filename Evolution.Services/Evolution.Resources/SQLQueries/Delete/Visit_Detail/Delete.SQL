--DELETE from visit.VisitDocument where VisitId={0};
--DELETE from [techspecialist].[TechnicalSpecialistCalendar] where CalendarRefCode={0};
DELETE from visit.VisitNote where VisitId={0};
UPDATE common.Document set IsDeleted=1 where ModuleRefCode={0} and ModuleCode='VST';
DELETE from visit.VisitReference where VisitId={0};
DELETE from visit.VisitSupplierPerformance where VisitId={0}
DELETE TTAC from visit.VisitTechnicalSpecialistAccountItemConsumable TTAC
INNER JOIN visit.VisitTechnicalSpecialist TT on TT.Id = TTAC.VisitTechnicalSpecialistId
WHERE TT.VisitId = {0};
DELETE TTAE from visit.VisitTechnicalSpecialistAccountItemExpense TTAE INNER JOIN visit.VisitTechnicalSpecialist TT on TT.Id = TTAE.VisitTechnicalSpeciallistId
WHERE TT.VisitId = {0};
DELETE TTAT from visit.VisitTechnicalSpecialistAccountItemTime TTAT INNER JOIN visit.VisitTechnicalSpecialist TT on TT.Id = TTAT.VisitTechnicalSpeciallistId
WHERE TT.VisitId = {0};
DELETE TTSAT from visit.VisitTechnicalSpecialistAccountItemTravel TTSAT INNER JOIN visit.VisitTechnicalSpecialist TT on TT.Id = TTSAT.VisitTechnicalSpecialistId
WHERE TT.VisitId = {0};
DELETE from visit.VisitTechnicalSpecialist where VisitId = {0};
DELETE from visit.VisitInterCompanyDiscount where VisitId = {0};
DELETE from visit.Visit where id = {0};