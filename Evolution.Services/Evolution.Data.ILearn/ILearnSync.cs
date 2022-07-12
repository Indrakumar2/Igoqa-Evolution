using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.ILearn.Domain.Interfaces;
using Evolution.ILearn.Domain.Models;
using Evolution.Logging.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Globalization;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Evolution.Data.ILearn
{
    public class ILearnSync
    {
        private readonly AppEnvVariableBaseModel _syncSetting = null;
        private readonly IAppLogger<IlearnData> _logger = null;
        private readonly ILearnInterface _ILearnService = null;



        private bool _isSyncInProgress = false;

        public ILearnSync(IOptions<AppEnvVariableBaseModel> syncSetting, IAppLogger<IlearnData> logger, ILearnInterface ILearnService)
        {
            _syncSetting = syncSetting.Value;
            _logger = logger;
            _ILearnService = ILearnService;
        }

        public void PrformIlearnSyncOperation(IConfigurationRoot _configuration)
        {
            if (!this._isSyncInProgress)
            {
                try
                {
                    this.StartLearnSyncData(_configuration);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                }
                finally
                {
                    this._isSyncInProgress = false;
                }
            }

        }
        private void StartLearnSyncData(IConfigurationRoot _configuration)
        {
            this._isSyncInProgress = true;
            var firstInsertSync = StartILearnSyncDataInsert(_configuration);
            if (firstInsertSync.Description.ToString() == ResponseType.Success.ToString())
            {
                var AddCompantancyData = _ILearnService.AddCompantancyData();
                var InternalTraining = _ILearnService.AddInternalTrainingData();
                var AddCertificateData = _ILearnService.AddCertificateData();
                var AddTrainingData = _ILearnService.AddTrainingData();
            }
        }

        private Response StartILearnSyncDataInsert(IConfigurationRoot _configuration)
        {
            string value = _configuration.GetConnectionString("isLocal");
            string isSFTP = _configuration.GetConnectionString("isSFTP");
            IList<ILearnData> response = null;
            if (value == "true")
            {
                response = StartILearnSyncTempGet(_configuration);
            }
            else
            {
                if(isSFTP == "true")
                {
                    response = startILearnSyncGetSFTP(_configuration);
                }
                else
                {
                    response = StartILearnSyncGet(_configuration);
                }
            }
             
            var finaldata = response?.ToList();
            Response responsedata = null;
            List<LearnData> LearnData = new List<LearnData>();
            if (finaldata?.Count > 0)
            {
                finaldata?.ForEach(x =>
                {
                    LearnData tempData = new LearnData();
                    tempData.GRM_ePin_ID = x.GRM_ePin_ID;
                    tempData.Training_Object_ID = x.Training_Object_ID;
                    tempData.Transcript_Score = x.Transcript_Score;
                    tempData.Training_Title = x.Training_Title;
                    tempData.Transcript_Completed_Date = x.Transcript_Completed_Date;
                    tempData.Training_Hours = x.Training_Hours;
                    tempData.IsILearn = true;
                    LearnData.Add(tempData);
                });
            }
            if (LearnData.Count > 0)
            {
                responsedata = _ILearnService.Save(LearnData);//Save Data in ILearnData table in DB
            }
            return responsedata;
        }
        private IList<ILearnData> startILearnSyncGetSFTP(IConfigurationRoot _configuration)
        {
            string host = _configuration.GetConnectionString("SFTPAddress");
            var port = Convert.ToInt32(_configuration.GetConnectionString("SFTPPort"));  
            string username = _configuration.GetConnectionString("SFTPUserName");
            string password = _configuration.GetConnectionString("SFTPPassword"); 

            string remoteDirectory = _configuration.GetConnectionString("SFTPRemoteDirectory");
            string localDirectory = _configuration.GetConnectionString("TempServerPath");
            IList<ILearnData> finaldata = null;
            try
            {
                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    var sftpFiles = sftp.ListDirectory(remoteDirectory).ToList();
                    sftpFiles.Reverse();

                    if (sftpFiles != null)
                    {
                        string remoteFileName = sftpFiles[0].Name;
                        if (!sftpFiles[0].Name.StartsWith("."))
                        {
                            using (Stream fileStream = File.OpenWrite(localDirectory + remoteFileName))
                            {
                                sftp.DownloadFile(remoteDirectory + remoteFileName, fileStream);
                            }
                        }
                        finaldata = ConvertPathToList(localDirectory + remoteFileName);
                        if ((System.IO.File.Exists(localDirectory + remoteFileName)))
                        {
                            System.IO.File.Delete(localDirectory + remoteFileName);//Remove immediatly as this is not required after conversion
                        }
                    }

                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
               
            }
            return finaldata;
        }

        private IList<ILearnData> StartILearnSyncGet(IConfigurationRoot _configuration)
        {
            string PathStorage = _configuration.GetConnectionString("TempServerPath");
            string ftp_Address = _configuration.GetConnectionString("FtpAddress");
            string ftp_UserName = _configuration.GetConnectionString("FTPUserName");
            string ftp_Password = _configuration.GetConnectionString("FTPPassword");
            //string PathStorage = appEnvVariableBaseModel.TempServerPath;
            Exception exception = null;
            this._isSyncInProgress = true;
            IList<ILearnData> finaldata = null;
            try
            {
                WebRequest request = WebRequest.Create(ftp_Address);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftp_UserName, ftp_Password);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    List<string> directories = new List<string>();
                    string line = streamReader.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        directories.Add(line);//Adding all FTP  Text files to Directory object
                        line = streamReader.ReadLine();
                    }
                    directories.Reverse();
                    streamReader.Close();
                    WebClient request2 = new WebClient();
                    request2.Credentials = new NetworkCredential(ftp_UserName, ftp_Password);
                    var filteredData = request2.DownloadData(ftp_Address + directories[0]).Skip(65);//Remove heading and Take only the Table like content
                    var finalFilterdString = System.Text.Encoding.UTF8.GetString(filteredData.ToArray());
                    byte[] byteArray = Encoding.UTF8.GetBytes(finalFilterdString);
                    MemoryStream stream = new MemoryStream(byteArray);
                    string finalPath = PathStorage + directories[0];//Static Path - one Day Path
                    using (var fileStream = new FileStream(finalPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                    finaldata = ConvertPathToList(finalPath);
                    if ((System.IO.File.Exists(finalPath)))
                    {
                        System.IO.File.Delete(finalPath);//Remove immediatly as this is not required after conversion
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return finaldata;
        }

        private IList<ILearnData> StartILearnSyncTempGet(IConfigurationRoot _configuration)
        {
            string folderPath = _configuration.GetConnectionString("TempServerPath");
            string finalPath = @"" + folderPath + "\test1.txt";
            IList<ILearnData> finaldata = ConvertPathToList(finalPath);
            return finaldata;
        }

        private IList<ILearnData> ConvertPathToList(string filePath)
        {
            int headers = 0;
            List<ILearnData> domainData = new List<ILearnData>();
            List<ILearnData> filterData = new List<ILearnData>();
            Exception exception = null;
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    var lines2 = System.IO.File.ReadAllLines(filePath);

                    var logList = new List<string>(lines2);
                    foreach (string line in logList)
                    {
                        try
                        {
                            var eachData = line.Split('\t');
                            if (headers >= 4)
                            {
                                ILearnData obj = new ILearnData();
                                obj.Training_Object_ID = eachData[0];
                                obj.Training_Title = eachData[1];
                                // DateTime dt;
                                // DateTime.TryParse(eachData[2], out dt); //Commented for D1172
                                CultureInfo provider = CultureInfo.InvariantCulture; //Added for D1172
                                obj.Transcript_Completed_Date = DateTime.Parse(eachData[2], provider); //Added for D1172 (Date Convertion mm/dd/YYYY to dd/mm/yyyy)
                                obj.GRM_ePin_ID = eachData[3].Equals("") ? 0 : Convert.ToInt64(eachData[3]?.Split('.')?[0]?.Trim());
                                obj.Transcript_Score = eachData[4];
                                obj.Training_Hours = eachData[5];
                                domainData.Add(obj);
                            }
                            headers++;
                        }
                        catch (Exception ex2)
                        {
                            exception = ex2;
                            _logger.LogError(ResponseType.Exception.ToId(), ex2.ToFullString(), null);
                        }
                    }
                    DateTime getBeforeDate = DateTime.Now.AddDays(-1);///Get only one day before Data
                    getBeforeDate = getBeforeDate.Date;
                    filterData = domainData.Where(x => x.Transcript_Completed_Date >= getBeforeDate).ToList(); //For Sarah Review Changes
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            // return domainData; //for patch work need to remove
            return filterData;
        }
    }
}
