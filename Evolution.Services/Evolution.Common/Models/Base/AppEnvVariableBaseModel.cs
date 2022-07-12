namespace Evolution.Common.Models.Base
{
    public class AppEnvVariableBaseModel
    {
        public string SQLServer { get; set; }
        public string SQLDatabase { get; set; }
        public string SQLUser { get; set; }
        public string SQLPassword { get; set; }
        public string SQLMaxPoolSize { get; set; }
        public string SQLConnectionTimeout { get; set; }

        public string MongoDbIp { get; set; }
        public string MongoDbPort { get; set; }
        public string MongoDbName { get; set; }
        public string MongoSyncTypes { get; set; } 

        public int TaskRunIntervalInMinute { get; set; }

        public string DocumentTitle { get; set; }
        public string ModifiedBy { get; set; }

        public string SecurityAppName { get; set; }
        public string GoogleApiKey { get; set; }
        public string ApplicationURL { get; set; }
        public string ApplicationGatewayURL { get; set; }
        public string ExtranetURL { get; set; }
        public string ResourceExtranetURL { get; set; } //DEF 1388 #2 fix
        public bool IsIpValidationRequire { get; set; }
        public int LockUserAfterFailedAttemptCount { get; set; } = 3;
        public int TempLockDurationInMinutes { get; set; } = 5;
        public string Evolution2_SPA_Client_Audience_code { get; set; }
        public string FtpAddress { get; set; }//ILearn
        public string FTPUserName { get; set; }//ILearn
        public string FTPPassword { get; set; }//ILearn
        public string TempServerPath { get; set; }//ILearn
        public int ILearnIntervalInMinute { get; set; }//ILearn
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
        public int VisitRecordSize { get; set; }
        public int TimesheetRecordSize { get; set; }
        public int AssignmentRecordSize { get; set; }
        public int ProjectRecordSize { get; set; }
        public int SupplierPORecordSize { get; set; }
        public int ChunkSize { get; set; } 
    }
}
