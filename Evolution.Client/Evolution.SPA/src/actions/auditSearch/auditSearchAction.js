import { RequestPayload ,masterData ,assignmentAPIConfig,homeAPIConfig,companyAPIConfig,customerAPIConfig
,projectAPIConfig ,
contractAPIConfig,
supplierAPIConfig,supplierPOApiConfig,GrmAPIConfig,
auditAPIConfig,timesheetAPIConfig,
visitAPIConfig
} from '../../apiConfig/apiConfig';
import {  FetchData ,PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty,parseValdiationMessage } from '../../utils/commonUtils';
import { getlocalizeData } from '../../utils/commonUtils';
import { auditSearchActionTypes } from '../../constants/actionTypes';
import { ShowLoader, HideLoader } from '../../common/commonAction';

const localConstant = getlocalizeData();

const actions = {
    FetchModuleName:(payload)=>(
        {
             type:auditSearchActionTypes.FETCH_MODULE_DATA_FOR_AUDITSEARCH,
             data:payload
         }
     ),
     FetchSubModuleName:(payload)=>(
         {
              type:auditSearchActionTypes.FETCH_SUB_MODULE_DATA,
              data:payload
          }
      ),
    FetchParentSubModuleGridData:(payload)=>(
        {
            type: auditSearchActionTypes.FETCH_PARENT_SUB_GRID_DATA,
            data: payload
        }
    ),
    ClearParentData:(payload)=>(
        {
            type:auditSearchActionTypes.CLEAR_PARENT_GRID_DATA,
            data:payload
        }
    ),
    ClearMouduleName:(payload)=>(
        {
            type:auditSearchActionTypes.CLEAR_MODULE_NAME,
            data:payload
        }
    ),
   
    FetchAuditSearchUnquieId:(payload)=>({
         type:auditSearchActionTypes.FETCH_AUDIT_SEARCH_UNIQUE_ID,
         data:payload
     }),
     
    FetchAuditSearchData:(payload)=>({
        type:auditSearchActionTypes.FETCH_AUDIT_SEARCH_DATA,
        data:payload
    }),

    FetchAuditSearchLogData:(payload)=>({
        type:auditSearchActionTypes.FETCH_AUDIT_SEARCH_LOG_DATA,
        data:payload
    })

};

export const ClearParentData = () => async (dispatch, getState) => {
    dispatch(actions.ClearParentData());
};
export const ClearMouduleName = () => async (dispatch, getState) => {
    dispatch(actions.ClearMouduleName());
};
// export const FetchAuditSearchUnquieId = (data) => async (dispatch, getState) => {
//     let url = "";
//     const params = {};
//     if (data.moduleName === "Assignment" && data.selectType === "ProjectNumber" && !isEmpty(data.columnValue)) {
//         url = assignmentAPIConfig.assignment;
//         params.assignmentProjectNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Assignment" && data.selectType === "ProjectAssignment" && !isEmpty(data.columnValue)) {
//         const value = data.columnValue;
//         const projectNumber = value.split("-")[0];
//         const assignmentNumber = value.split("-")[1];
//         url = assignmentAPIConfig.assignment;
//         params.assignmentProjectNumber = projectNumber;
//         params.assignmentNumber = assignmentNumber;
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Contract" && data.selectType === "ContractNumber" && !isEmpty(data.columnValue)) {
//         // url = homeAPIConfig.homeBaseURL + homeAPIConfig.contract;
//         url=contractAPIConfig.contractBaseUrl + contractAPIConfig.contractSearch ;
//         params.contractNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Contract" && data.selectType === "CustomerContractNumber" && !isEmpty(data.columnValue)) {
//         // url = homeAPIConfig.homeBaseURL + homeAPIConfig.contract;
//        url= contractAPIConfig.contractBaseUrl + contractAPIConfig.contractSearch ;
//         params.customerContractNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Company" && data.selectType === "Name" && !isEmpty(data.columnValue)) {
//         url = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails;
//         params.companyName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Company" && data.selectType === "CompanyCode" && !isEmpty(data.columnValue)) {
//         url = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails;
//         params.companyCode = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Customer" && data.selectType === "Name" && !isEmpty(data.columnValue)) {
//         url = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;
//         params.customerName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Customer" && data.selectType === "ParentName" && !isEmpty(data.columnValue)) {
//         url = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;
//         params.parentCompanyName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Customer" && data.selectType === "CustomerCode" && !isEmpty(data.columnValue)) {
//         url = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;
//         params.customerCode = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Document Library" && data.selectType === "Name" && !isEmpty(data.columnValue)) {
//          url = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;/** need to be changed */
//         params.customerCode = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Project" && data.selectType === "CustomerProjectNumber" && !isEmpty(data.columnValue)) {
//         url = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;
//         params.customerProjectNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Project" && data.selectType === "ProjectNumber" && !isEmpty(data.columnValue)) {
//         url = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;
//         params.projectNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Project" && data.selectType === "CustomerProjectName" && !isEmpty(data.columnValue)) {
//         url = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;
//         params.customerProjectName= data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Project" && data.selectType === "ContractNumber" && !isEmpty(data.columnValue)) {
//         url = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;
//         params.contractNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "Supplier" && data.selectType === "Name" && !isEmpty(data.columnValue)) {
//         url = supplierAPIConfig.supplierBaseUrl + supplierAPIConfig.supplierSearch;
//         params.supplierName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//      if (data.moduleName === "SupplierPO" && data.selectType === "Number" && !isEmpty(data.columnValue)) {
//         url = supplierPOApiConfig.supplierPOSearch;
//         params.supplierPONumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "SupplierPO" && data.selectType === "ProjectNumber" && !isEmpty(data.columnValue)) {
//         url = supplierPOApiConfig.supplierPOSearch;
//         params.SupplierPoProjectNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "SupplierPO" && data.selectType === "CustomerProjectName" && !isEmpty(data.columnValue)) {
//         url = supplierPOApiConfig.supplierPOSearch;
//         params.supplierPOCustomerProjectName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "TechnicalSpecialist" && data.selectType === "LastName" && !isEmpty(data.columnValue)) {
//         url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.search;
//         params.lastName = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if (data.moduleName === "TechnicalSpecialist" && data.selectType === "PIN" && !isEmpty(data.columnValue)) {
//         url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.search;
//         params.epin = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if(data.moduleName === "Visit" && data.selectType === "ReportNumber" && !isEmpty(data.columnValue)){
//         url=visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetSearchVisits;
//         params.ReportNumber = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if(data.moduleName === "Visit" && data.selectType === "JobReferenceNumber" && !isEmpty(data.columnValue)){
//         url=visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetSearchVisits;
//         params.ReportNumber = data.columnValue.trim(); /** need to discuss */
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if(data.moduleName === "Visit" && data.selectType === "ProjectAssignment" && !isEmpty(data.columnValue)){
//         url=visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetSearchVisits;
//         const value = data.columnValue;
//         const projectNumber = value.split("-")[0];
//         const assignmentNumber = value.split("-")[1];
//         params.assignmentProjectNumber = projectNumber;
//         params.assignmentNumber = assignmentNumber;
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
        
//     }
//     if(data.moduleName === "Timesheet" && data.selectType === "TimesheetDescription" && !isEmpty(data.columnValue)){
//         url=timesheetAPIConfig.searchTimesheets;
//         params.timesheetDescription = data.columnValue.trim();
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if(data.moduleName === "Timesheet" && data.selectType === "JobReferenceNumber" && !isEmpty(data.columnValue)){
//         url=timesheetAPIConfig.searchTimesheets;
//         params.timesheetId = data.columnValue.trim(); /**need to discuss */
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     if(data.moduleName === "Timesheet" && data.selectType === "ProjectAssignment" && !isEmpty(data.columnValue)){
//         url=timesheetAPIConfig.searchTimesheets;
//         const value = data.columnValue;
//         const projectNumber = value.split("-")[0];
//         const assignmentNumber = value.split("-")[1];
//         params.assignmentProjectNumber = projectNumber;
//         params.assignmentNumber = assignmentNumber;
//         params.fromDate=data.fromDate;
//         params.toDate=data.toDate;
//     }
//     const requestPayload = new RequestPayload(params);
//     const response = await FetchData(url, requestPayload).catch(error => {
//         IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, "dangerToast" );
//     }
//     );
//     if (!isEmpty(response)) {
//         if (response.code == 1) {
//             dispatch(actions.FetchAuditSearchUnquieId(response.result));
//            return response.result;
//         } else {
//             IntertekToaster(parseValdiationMessage(response), 'dangerToast');
//         }
//     }
// };
export const FetchModuleName = () => async (dispatch) => {
    const Url = auditAPIConfig.auditBaseUrl + auditAPIConfig.module;
    const requestPayload = new RequestPayload();
    const response = await FetchData(Url, requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console          
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        }); 
        
    if (response && response.code == "1") { 
        dispatch(actions.FetchModuleName(response.result));  
        return response.result;
    }
    else {        
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};
export const FetchSubModuleName = (data) => async (dispatch) => {
const Url = auditAPIConfig.auditBaseUrl + auditAPIConfig.module;
    let response = {};
    if (isEmpty(data)) {
        dispatch(actions.FetchSubModuleName());
    }
    else {
        const  params = {  "ModuleName":data  };
        const requestPayload = new RequestPayload(params);
        response = await FetchData(Url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchCity');
            });
        if (response && response.code == "1") {
            dispatch(actions.FetchSubModuleName(response.result));
        }
        else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
        }
    }
    return response;
};
export const FetchAuditSearchData= (data) => async (dispatch, getState) => {
    dispatch(ShowLoader());
    const url = auditAPIConfig.auditBaseUrl + auditAPIConfig.audit;
    const params = {
        "SearchReference":data.unquieId,
        "auditModuleName":data.auditModuleName,
        "auditModuleId":data.auditModuleId,
        "selectType":data.selectType,
        "fromDate":data.fromDate,
        "toDate":data.toDate
    };
    const requestPayload = new RequestPayload(params);
    const response = await PostData(url, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, "dangerToast" );
    }
    );
    if (!isEmpty(response)) {
        if (response.code == 1) {
          dispatch(actions.FetchAuditSearchData(response.result));
          dispatch(HideLoader());
        } else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        }
    }
    dispatch(HideLoader());
};

export const FetchAuditEventData= (data) => async (dispatch, getState) => {
    dispatch(ShowLoader());
    const url = auditAPIConfig.auditBaseUrl + auditAPIConfig.auditEvent;
    const params = {
        "SearchReference":data.unquieId,
        "auditModuleName":data.auditModuleName,
        "auditModuleId":data.auditModuleId,
        "selectType":data.selectType,
        "fromDate":data.fromDate,
        "toDate":data.toDate
    };
    const requestPayload = new RequestPayload(params);
    const response = await PostData(url, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, "dangerToast" );
    }
    );
    if (!isEmpty(response)) {
        if (response.code == 1) {
          dispatch(actions.FetchAuditSearchData(response.result));
          dispatch(HideLoader());
        } else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        }
    }
    dispatch(HideLoader());
};

export const FetchAuditLogData= (data) => async (dispatch, getState) => {
    dispatch(ShowLoader());
    const url = auditAPIConfig.auditBaseUrl + auditAPIConfig.auditLog;
    const params = {
        "logId":data
    };
    const requestPayload = new RequestPayload(params);
    const response = await PostData(url, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, "dangerToast" );
    }
    );
    if (!isEmpty(response)) {
        if (response.code == 1) {
          dispatch(actions.FetchAuditSearchLogData(response.result));
          dispatch(HideLoader());
          return response.result;
        } else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        }
    }
    dispatch(HideLoader());
};