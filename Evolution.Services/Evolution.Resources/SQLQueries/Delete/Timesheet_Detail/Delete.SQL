--DELETE from timesheet.TimesheetDocument where TimesheetId={0};
--DELETE from [techspecialist].[TechnicalSpecialistCalendar] where CalendarRefCode={0};
BEGIN TRANSACTION
DELETE from timesheet.TimesheetNote where TimesheetId={0};
UPDATE common.Document set IsDeleted=1 where ModuleCode='TIME' and ModuleRefCode=CONVERT(VARCHAR(50),{0});
DELETE from timesheet.TimesheetReference where TimesheetId={0};
DELETE TTAC from timesheet.TimesheetTechnicalSpecialistAccountItemConsumable TTAC
INNER JOIN timesheet.TimesheetTechnicalSpecialist TT on TT.Id = TTAC.TimesheetTechnicalSpecialistId
WHERE TT.TimesheetId = {0};
DELETE TTAE from timesheet.TimesheetTechnicalSpecialistAccountItemExpense TTAE INNER JOIN timesheet.TimesheetTechnicalSpecialist TT on TT.Id = TTAE.TimesheetTechnicalSpeciallistId
WHERE TT.TimesheetId = {0};
DELETE TTAT from timesheet.TimesheetTechnicalSpecialistAccountItemTime TTAT INNER JOIN timesheet.TimesheetTechnicalSpecialist TT on TT.Id = TTAT.TimesheetTechnicalSpeciallistId
WHERE TT.TimesheetId = {0};
DELETE TTSAT from timesheet.TimesheetTechnicalSpecialistAccountItemTravel TTSAT INNER JOIN timesheet.TimesheetTechnicalSpecialist TT on TT.Id = TTSAT.TimesheetTechnicalSpecialistId
WHERE TT.TimesheetId = {0};
DELETE from timesheet.TimesheetTechnicalSpecialist where TimesheetId={0};
DELETE FROM timesheet.TimesheetInterCompanyDiscount where TimesheetId={0};
DELETE from timesheet.Timesheet where id={0};

IF(@@ERROR > 0)
BEGIN
    Rollback Transaction
END
ELSE
BEGIN
   Commit Transaction
END