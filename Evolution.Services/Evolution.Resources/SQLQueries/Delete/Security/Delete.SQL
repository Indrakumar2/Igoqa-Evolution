DELETE FROM [security].[UserRole] WHERE UserId IN ( SELECT Id FROM [security].[User] WHERE SAMAccountName IN({0}))
DELETE FROM [security].[UserType] WHERE UserId IN ( SELECT Id FROM [security].[User] WHERE SAMAccountName IN({0}))
DELETE FROM [security].[User] WHERE SAMAccountName IN({1})