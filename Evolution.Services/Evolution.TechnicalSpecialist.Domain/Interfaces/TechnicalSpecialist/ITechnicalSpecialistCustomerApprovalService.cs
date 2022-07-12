using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCustomerApprovalService
    {

        Response Get(TechnicalSpecialistCustomerApprovalInfo searchModel);

        Response Get(IList<int> Ids);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> CustomerName);

        Response Add(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                           bool commitChange = true,
                           bool isValidationRequire = true);

        Response Add(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                          
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.TechnicalSpecialistCustomers> Dbcustomers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
           ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustApprInfos,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCutomerApprovlInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                               ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomer,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist,
                                               bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCustomerApprovalInfo> tsCustomerApprovalInfos,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCustomerName,
                                    ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                    ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumber,
                                    ref IList<DbModel.TechnicalSpecialistCustomerApproval> dbTsCustomerApprovalInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndCustomerNameNotExists,
                                    ref IList<ValidationMessage> validationMessages);
    }
}
