import { FetchData, PostData,MultiDocumentDownload } from '../../services/api/baseApiService';
import { RequestPayload, masterData,customerAPIConfig , contractAPIConfig, EvolutionReportsAPIConfig,GrmAPIConfig }  from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { reportActionTypes } from '../../constants/actionTypes';
import { getlocalizeData, parseValdiationMessage, isEmpty } from '../../utils/commonUtils';
import { sortCurrency } from '../../utils/currencyUtil';
import { IsGRMMasterDataFetched,IsARSMasterDataLoaded,ShowLoader,HideLoader,DownloadDocument } from '../../common/commonAction';
import { ChargeRates } from '../../actions/contracts/rateScheduleAction';
import { result } from 'lodash';
import React from 'react'; 
import DownloadZipCVS from '../../common/commonAction';

const localConstant = getlocalizeData();

const actions = {
    FetchCustomerList: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_CUSTOMER,
        data: payload
    }),
    FetchCustomerContracts: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_CUSTOMERCONTRACTS,
        data: payload
    }),
    FetchApprovedUnApprovedVistCustomer: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_APPROVEDUNAPPROVEDVISIT,
        data: payload
    }),
    FetchUnApprovedVistCustomer: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_UNAPPROVEDVISIT,
        data: payload
    }),
    FetchKPICustomer: (payload) => ({
        type: reportActionTypes.REPORTS_KPI_CUSTOMER,
        data: payload
    }),
    FetchKPIVisitTimesheetContract: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_KPICONTRACTS,
        data: payload
    }),
    FetchApprovedUnApprovedVistContract: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_APPROVEDUNAPPROVEDCUSTOMERCONTRACTS,
        data: payload
    }),FetchUnApprovedVistContract: (payload) => ({
        type: reportActionTypes.REPORTS_FETCH_UNAPPROVED_CONTRACTS,
        data: payload
    }),
    FetchContractProjects:(payload) => ({
        type:reportActionTypes.REPORT_FETCH_CONTRACTPROJECTS,
        data: payload
    }),
    FetchKPIVisitTimesheetProject:(payload) => ({
        type:reportActionTypes.REPORT_FETCH_CONTRACTPROJECTS,
        data: payload
    }),
    FetchCustomerBasedOnCoordinators:(payload)=>({
        type: reportActionTypes.FETCH_CUSTOMERS_BASED_ON_COORDINATORS,
        data:payload
    }),
    ClearCustomerList: (payload) => ({
        type: reportActionTypes.CLEAR_CUSTOMER,
        data: payload
    }),
    ClearCustomerContracts: (payload) => ({
        type: reportActionTypes.CLEAR_CUSTOMERCONTRACTS,
        data: payload
    }),
    ClearContractProjects: (payload) => ({
        type: reportActionTypes.CLEAR_CONTRACTPROJECTS,
        data: payload
    }),
    ClearServerReports: (payload) => ({
        type: reportActionTypes.CLEAR_SERVER_REPORTS,
        data: payload
    }),
    FetchServerReportData: (payload) => ({
        type: reportActionTypes.FETCH_SERVER_REPORT_DATA,
        data: payload
    }), 
    GetAndExportTechnicalSpeclaistCV : (payload) => ({
        type: reportActionTypes.DOWNLOAD_EXPORT_CV_AS_ZIP,
        data: payload
    }),
    DownloadServerFile: (payload) => ({
        type: reportActionTypes.DOWNLOAD_SERVER_FILE_DATA,
        data: payload
    }),
};
 
export const FetchCustomerList = (data) => async (dispatch) => {
    let apiUrl =  customerAPIConfig.customerDetails + '?';
    apiUrl += "Active=YES ";
    const params = {
        customerCode:data,
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCustomerList(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchCustomerContracts = (data) => async (dispatch, getstate) => {
    const customerContractData =customerAPIConfig.custContracts ;
    const params = {
        contractCustomerCode: data,
     };
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(customerContractData, requestPayload)
        .catch(error => {
            dispatch(HideLoader());
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast projectAssignment');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchCustomerContracts(response.result));
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
        dispatch(HideLoader());
        IntertekToaster(parseValdiationMessage(response), 'dangerToast custContractWentWrong');
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.customer.FETCH_CUSTOMER_CONTRACT_API_FAILED, 'dangerToast assignemntSomthingWrong');
    }
};
export const FetchContractProjects = (data) => async (dispatch, getstate) => {  
  
    const projectDataUrl = EvolutionReportsAPIConfig.ProjectBasedOnStatus;
    const params = data;
    if(!isEmpty(params.contractNumber)){
        dispatch(ShowLoader());
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(projectDataUrl, requestPayload)
            .catch(error => {     
                dispatch(HideLoader());
                // console.error(error); // To show the error details in console    
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast projectAssignment');
            });
        if (response && response.code === "1") {
            dispatch(HideLoader());
            dispatch(actions.FetchContractProjects(response.result));
            return response.result;
        }
        else {        
            dispatch(HideLoader());
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast assignemntSomthingWrong');
        }
    }
};

export const FetchApprovedUnApprovedVistCustomer = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetCustomers;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
             // To show the error details in console
            dispatch(HideLoader());
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchApprovedUnApprovedVistCustomer(response.result));
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchKPICustomer = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetKPICustomers;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader());
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchKPICustomer(response.result));
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchKPIVisitTimesheetContract = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetKPIContracts;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader());
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchKPIVisitTimesheetContract(response.result));
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchKPIVisitTimesheetProject = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetKPIProjects;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader());
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchKPIVisitTimesheetProject(response.result));
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

//FetchUnApprovedVistContract
export const FetchUnApprovedVistContract = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetUnApprovedContracts;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader);
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader);
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader);
        dispatch(actions.FetchUnApprovedVistContract(response.result));
    }
    else {
        dispatch(HideLoader);
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchApprovedUnApprovedVistContract = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetContracts;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader);
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader);
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader);
        dispatch(actions.FetchApprovedUnApprovedVistContract(response.result));
    }
    else {
        dispatch(HideLoader);
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchUnApprovedVistCustomer = (data) => async(dispatch,getstate) =>{
    const apiUrl =  customerAPIConfig.visitTimesheetUnapprovedCustomers;
    const params = data;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await FetchData(apiUrl,requestPayload)
        .catch(error => {
            dispatch(HideLoader()); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        dispatch(actions.FetchUnApprovedVistCustomer(response.result));
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchCustomerBasedOnCoordinators = (data) => async(dispatch,getstate) => {
    const projectCoordinatorUrl =customerAPIConfig.customerCoordinator;
    const param=data;
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(projectCoordinatorUrl,requestPayload)
        .catch(error=>{
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG,'wariningToast projectCoordinatorVal');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                dispatch(actions.FetchCustomerBasedOnCoordinators(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCoWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
        }
};
export const ClearCustomerList = () => (dispatch) => {
    dispatch(actions.ClearCustomerList());  
};

export const ClearCustomerContracts = () => (dispatch) => {
    dispatch(actions.ClearCustomerContracts());
};

export const ClearContractProjects = () => (dispatch) => {
    dispatch(actions.ClearContractProjects());
};

export const ClearServerReports = () => (dispatch) =>{
    //serverReportData
    dispatch(actions.ClearServerReports());
};

function IconImage() {
    return (
        <i className="zmdi zmdi-refresh zmdi-hc-lg"></i>
    );
  }

export const GetAndExportTechnicalSpeclaistCV = (rowData) =>(dispatch,props) => {
    const exportCVFrom = rowData.reportType;
    const reportParam= JSON.parse(rowData.reportParam);
    const epins = reportParam.epins;
    const requestPayload = new RequestPayload(epins);
    const reportID =rowData.id;
    const url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.DocumentApi + GrmAPIConfig.exportCVasZIP + "?isChevron="+false+"&exportCVFrom="+exportCVFrom+"&reportid="+reportID;
    const response =  MultiDocumentDownload(url,requestPayload)
    .catch(error => {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (response) {
        const type= response.headers['content-type'];
         const headerval = response.headers['content-disposition'];
            let filename = headerval.split(';')[1].split('=')[1].replace('"', '').replace('"', '');
           // let filename = "SearchCVExportTask";
            filename = decodeURIComponent(filename);
            const ua = window.navigator.userAgent;
            const msie = ua.indexOf(".NET ");
            if(msie>0) {
                window.navigator.msSaveBlob(new Blob([ response.data ]), filename);
            }
            else{
                
            const blob = new Blob([ response.data ],{ type: type, encoding: 'UTF-8' });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.remove();
            }
          //  dispatch(ShowLoader(false));
    } 
  };

export const FetchServerReportData = (data) => async(dispatch,props) => {
    const reportUrl = customerAPIConfig.serverDownloadReport+'?astrUserName='+data.loggedInUser+'&aintReportType='+data.reportType;
    const requestPayload = new RequestPayload();
    dispatch(ShowLoader());
    const response = await FetchData(reportUrl,requestPayload)
    .catch(error=>{
        dispatch(HideLoader());
        // IntertekToaster(localConstant.validationMessage.SERVER_REPORT_DATA_FAILURE,'warningToast serverReportDataFails');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if(response && !isEmpty(response.result)){
        if(response.code === '1'){
            dispatch(actions.FetchServerReportData(response.result));
        }
        else{
            dispatch(actions.FetchServerReportData([]));
        }
        dispatch(HideLoader());
    }
    else{
        dispatch(actions.FetchServerReportData([]));
        dispatch(HideLoader());
   }
};

export const DeleteReportFiles = (data) => async(dispatch,props) => {
    const reportUrl = customerAPIConfig.deleteReportFiles;
    dispatch(ShowLoader());
    const deleteParams = data.deleteParams;
    const fetchParams = {
        loggedInUser: data.loggedInUser,
        reportType: data.reportType
    };
    const requestPayload = new RequestPayload(deleteParams);
    const response = await PostData(reportUrl,requestPayload)
    .catch(error=>{
        dispatch(HideLoader());
        // IntertekToaster(localConstant.validationMessage.SERVER_REPORT_DATA_FAILURE,'warningToast serverReportDataFails');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if(response && !isEmpty(response.result)){
        if(response.code === '1' && response.result === 'Deleted Successfully'){
           IntertekToaster(response.result,'successToast serverReportDataFails');
           dispatch(FetchServerReportData(fetchParams));
        }
        else{
            dispatch(actions.FetchServerReportData([]));
        }
        dispatch(HideLoader());
    }
    else{
        dispatch(actions.FetchServerReportData([]));
        dispatch(HideLoader());
   }
};

export const PrintSSRSReport = (data, contentType, filename) => async(dispatch) => {
    const reportUrl = EvolutionReportsAPIConfig.reportBaseUrl + EvolutionReportsAPIConfig.EvolutionReportBaseUrl;
    const params = data;
    const reportName = filename;
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await PostData(reportUrl,requestPayload)
    .catch(error=>{
        dispatch(HideLoader());
        // IntertekToaster(localConstant.validationMessage.SERVER_REPORT_DATA_FAILURE,'warningToast serverReportDataFails');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (response) {
        const _finalResponse = response;
        if(_finalResponse && _finalResponse.code && (_finalResponse.code == 200 || _finalResponse.code =="1") ){
            const _data = _finalResponse.data ==undefined? _finalResponse.result:_finalResponse.data;
            const binaryString = window.atob(_data);
            const binaryLen = binaryString.length;
            const bytes = new Uint8Array(binaryLen);
            for (let i = 0; i < binaryLen; i++) {
                const ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }

            const blob = new Blob([ bytes ], { type: contentType });
            if (navigator.appVersion.toString().indexOf('.NET') > 0) {
                window.navigator.msSaveBlob(blob, reportName);
            }
            else {
                const fileURL = URL.createObjectURL(blob);
                window.open(fileURL);
            }
            dispatch(HideLoader());
        }
        else{
            if (_finalResponse && _finalResponse.message) {
                IntertekToaster(_finalResponse.message, 'warningToast ssrs_report');
                dispatch(HideLoader());
            }
            else{
                IntertekToaster(localConstant.commonConstants.ERROR_MSG, 'dangerToast ssrs_report');
                dispatch(HideLoader());
            }
        }
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.commonConstants.ERROR_MSG, 'dangerToast ssrs_report');
    }
};

export const DownloadServerFile = (data) => async(dispatch) => {
    const reportUrl = customerAPIConfig.downloadServerFile;
    const params = {
        filename: data.filename,
        isDelete: data.isDelete,
    };
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await PostData(reportUrl,requestPayload)
    .catch(error=>{
        dispatch(HideLoader());
        // IntertekToaster(localConstant.validationMessage.SERVER_REPORT_DATA_FAILURE,'warningToast serverReportDataFails');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if(!isEmpty(response)){
        if (response.code == 200) {
            if (response.message && response.data) {
                const filenameAndType = response.message.split('||');
                let reportFileName = filenameAndType[0];
                let newreportFileName='';
                 if (reportFileName.includes('.zip.zip')) {
                    newreportFileName = reportFileName.replace(".zip.zip", ".zip");
                    reportFileName=newreportFileName;
                }
                if(reportFileName.includes('.zip')){
                    const newfilename=data.filename.substr(data.filename.lastIndexOf("\\")+1);
                    reportFileName=newfilename;
                }
                const contentType = filenameAndType[1];
                const _data = response.data;
                const binaryString = window.atob(_data);
                const binaryLen = binaryString.length;
                const bytes = new Uint8Array(binaryLen);
                for (let i = 0; i < binaryLen; i++) {
                    const ascii = binaryString.charCodeAt(i);
                    bytes[i] = ascii;
                }

                const blob = new Blob([ bytes ], { type: contentType });
                if (navigator.appVersion.toString().indexOf('.NET') > 0) {
                    window.navigator.msSaveBlob(blob, reportFileName);
                }
                else {
                    const link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = reportFileName;
                    link.click();
                }
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.DOWNLOAD_SERVER_REPORT_FAILURE,'dangerToast ReportFetchWentWrong');
        }
        dispatch(HideLoader());
    }
    else{
       IntertekToaster(localConstant.validationMessage.DOWNLOAD_SERVER_REPORT_FAILURE, 'dangerToast ReportFetchWentWrong');
       dispatch(HideLoader());
   }
};
