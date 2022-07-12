using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace Evolution.ILearn
    {
        public class ILearn
        {
            static string PathStorage = "F:\\ILearn\\";
            public static void Main(string[] args)
            {
                
                string sqlConnection = sqlConnection = "Data Source=10.10.11.130;Initial Catalog=Evolution2DEC;User ID=otr;Password=otrmi";
                DataTable ftpData = getFtpDataForToday();
                string Respoese = bulkCopyInsert(sqlConnection, ftpData);
            }

            public static DataTable getFtpDataForToday()
            {
                    DataTable finalData = new DataTable();
                    string ftpDownloadResult = string.Empty;
                    try
                    {
                            WebRequest request = WebRequest.Create("ftp://ftp.intertek.csod.com/Reports/GRM/");
                            request.Method = WebRequestMethods.Ftp.ListDirectory;
                            request.Credentials = new NetworkCredential("intertek", "r5hIrV1T");
                            using (var response = (FtpWebResponse)request.GetResponse())
                            {
                                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                                List<string> directories = new List<string>();
                                string line = streamReader.ReadLine();
                                DataSet final = new DataSet();
                                while (!string.IsNullOrEmpty(line))
                                {
                                    directories.Add(line);//Adding all FTP  Text files to Directory object
                                    line = streamReader.ReadLine();
                                }
                                directories.Reverse();
                                streamReader.Close();
                                WebClient request2 = new WebClient();
                                request2.Credentials = new NetworkCredential("intertek", "r5hIrV1T");
                                //byte[] newFileData = request2.DownloadData("ftp://ftp.intertek.csod.com/Reports/GRM/" + directories[0]);
                                var filteredData = request2.DownloadData("ftp://ftp.intertek.csod.com/Reports/GRM/" + directories[0]).Skip(65);//Remove heading and Take only the Table like content
                                var finalFilterdString = System.Text.Encoding.UTF8.GetString(filteredData.ToArray());
                                byte[] byteArray = Encoding.UTF8.GetBytes(finalFilterdString);
                                MemoryStream stream = new MemoryStream(byteArray);
                                  string finalPath = PathStorage + directories[0];//Static Path - one Day Path
                                //if(!(System.IO.File.Exists(finalPath)))
                                //{
                                    using (var fileStream = new FileStream(finalPath, FileMode.Create, FileAccess.Write))
                                    {
                                        stream.CopyTo(fileStream);
                                    }
                                    finalData = ConvertToDataTable(finalPath, 5);
                                    if ((System.IO.File.Exists(finalPath)))
                                    {
                                        System.IO.File.Delete(finalPath);
                                    }
                               // }
                               
                            }
                      }
                    catch (Exception ex)
                    {
                        //Write exception Logs here
                    }
                   return finalData;
               }
            public static DataTable ConvertToDataTable(string filePath, int numberOfColumns)
            {
                DataTable tbl = new DataTable();
                tbl.TableName = "ILearnTest";
                var lines2 = System.IO.File.ReadAllLines(filePath);
                int headers = 0;

            //var logList = new List<string>(lines2);
            //         foreach (string line in lines2)
            //         {
            //                 var eachData = line.Split('\t');

            //         }


            foreach (string line in lines2)
            {
                var eachData = line.Split('\t');
                if (headers == 0)
                {
                    for (int cIndex = 0; cIndex < 6; cIndex++)
                    {
                        tbl.Columns.Add(new DataColumn(eachData[cIndex].Replace(" ", "_")));
                    }
                }
                DataRow dr = tbl.NewRow();
                for (int rIndex = 0; rIndex < 6; rIndex++)
                {
                    dr[rIndex] = eachData[rIndex];
                }
                if (headers != 0)
                {
                    tbl.Rows.Add(dr);
                }
                headers++;
            }
            return tbl;

            }
            public static string bulkCopyInsert(string sqlConnection,DataTable ftpData)
            {
                try
                {
                    using (var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity))
                    {
                        foreach (DataColumn col in ftpData.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }

                        bulkCopy.BulkCopyTimeout = 600;
                        bulkCopy.DestinationTableName = "ILearnTest";
                        bulkCopy.WriteToServer(ftpData);
                    }
                }
                catch(Exception ex)
                {
                   //Write exception Logs here
                }
            return "Success";
            }
        }
        
    }
