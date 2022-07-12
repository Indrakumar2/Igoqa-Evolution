import { processApiRequest } from '../../../services/api/defaultServiceApi';
import { dashBoardActionTypes } from '../../../constants/actionTypes';
import { homeAPIConfig,RequestPayload } from '../../../apiConfig/apiConfig';
import { AppDashBoardRoutes } from '../../../routes/routesConfig';
import { FetchData,CreateData } from '../../../services/api/baseApiService';
import { applicationConstants } from '../../../constants/appConstants';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { ShowLoader,HideLoader } from '../../../common/commonAction';
import axios from 'axios';
import { getlocalizeData,isEmpty ,parseValdiationMessage,isEmptyOrUndefine,isUndefined } from '../../../utils/commonUtils';
import { StringFormat } from '../../../utils/stringUtil';
import { FetchMySearchData,ClearMySearchData } from '../../../grm/actions/techSpec/mySearchActions';
import { FetchTechSpecMytaskData,ClearMyTasksData } from '../../../grm/actions/techSpec/techSpecMytaskActions';
import { DashboardDocumentationDetails } from '../../../actions/dashboard/documentationAction';
import moment from 'moment';
import { getTSVisible } from '../../../common/selector';

const localConstant = getlocalizeData();

const actions = {
    FetchActiveAssignments: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_ASSIGNMENST_DATA,
            data: payload
        };
    },
    FetchInActiveAssignments: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_IN_ACTIVE_ASSIGNMENST_DATA,
            data: payload
        };
    },
    FetchVisitStatus: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_VISIT_STATUS_APPROVAL,
            data: payload
        };
    },
    FetchTimesheetPendingAproval: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_TIMESHEET_PENDING_APROVAL,
            data: payload
        };
    },
    FetchContractsNearExpiry: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_CONTRACTS_NEAR_EXPIRY,
            data: payload
        };
    },
    FetchDocumentAproval: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_DOCUMENT_APROVAL,
            data: "FETCH_DOCUMENT_APROVAL"
        };
    },
    FetchBudgetMonetary: () => {
        return {
            type: dashBoardActionTypes.FETCH_BUDGET_MONETARY,
            data: "FETCH_BUDGET_MONETARY"
        };
    },
    FetchBudgetHours: () => {
        return {
            type: dashBoardActionTypes.FETCH_BUDGET_HOURS,
            data: "FETCH_BUDGET_HOURS"
        };
    },
    FetchDocumentApproval: (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_DOCUMENTATION_APPROVAL,
            data: payload
        };
    }, 
    SelectedDocumentToApprove: (payload) => {
        return {
            type: dashBoardActionTypes.SELECTED_DOCUMENT_TO_APPROVE,
            data: payload
        };
    },
    ToggleAllCoordinator: (payload) => {
        return {
            type: dashBoardActionTypes.TOGGLE_ALL_COORDINATOR,
            data: payload
        };
    },
    FetchDashboardCount:(payload) =>{
        return{
            type:dashBoardActionTypes.FETCH_DASHBOARD_COUNT,
            data:payload
        };
    },
    ClearDashboardReducer:(payload) =>{
        return{
            type:dashBoardActionTypes.CLEAR_DASHBOARD_REDUCER,
            data:payload
        };
    },
    RejectDocumentApproval: (payload) => {
        return {
            type: 'Reject_Document_Approval',
            data: payload
        };
    },
    BudgetPropertyChange: (payload) => {
        return {
            type:dashBoardActionTypes.BUDGET_PROPERTY_CHANGE,
            data:payload
        };
    },
    FetchBudget : (payload) => {
        return{
            type:dashBoardActionTypes.FETCH_BUDGET,
            data:payload
        };
    },
    FetchDocumentType:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_DOCUMENT_APPROVAL_TYPE,
            data:payload
        };
    },
    FetchCustomerName:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_CUSTOMER_NAME,
            data:payload
        };
    },
    FetchContractForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_CONTRACT_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    FetchProjectForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_PROJECT_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    FetchAssignmentForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_ASSIGNMENT_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    FetchSupplierPOForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_SUPPLIERPO_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    FetchTimesheetForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_TIMESHEET_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    FetchVisitForDocumentApproval:(payload)=>{
        return{
            type:dashBoardActionTypes.FETCH_VISIT_FOR_DOC_APPROVAL,
            data:payload
        };
    },
    GetTechSpecDashboardCompanyMessage:(payload)=>{
        return{
            type:dashBoardActionTypes.GET_TECH_SPEC_DASHBOARD_MEASSAGE,
            data:payload
        };
    },
    MyTaskPropertyChange: (payload) => {
        return {
            type:dashBoardActionTypes.MYTASK_PROPERTY_CHANGE,
            data:payload
        };
    },
    /*
    Added For home dashboard my task and my search count not refreshing
    */
    MySearchPropertyChange:(payload) =>{
        return {
            type:dashBoardActionTypes.MYSEARCH_PROPERTY_CHANGE,
            data:payload
        };
    },    
};

/*
    Fetch Active Assignments with updated code with header passing and token validation
*/
export const FetchActiveAssignments = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if(state.dashboardReducer.isActiveAssignmentsLoaded){
        dispatch(HideLoader()); 
        return false;
    }  
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const params = {
        'assignmentOperatingCompanyCode': selectedCompany,
        'assignmentStatus' : 'P'
    };
    if (!allCoOrdinator) {
        params.AssignmentContractHoldingCompanyCoordinator = loginUser;
        params.AssignmentContractHoldingCompanyCoordinatorSamAcctName=state.appLayoutReducer.username;
    }
    const requestPayload = new RequestPayload(params);
 
    const response = await FetchData(homeAPIConfig.assignments, requestPayload)
        .catch(error => {          
            // console.error(error); // To show the error details in console
            // IntertekToaster('Assignment fetch is not working','dangerToast fetchAssignmentWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());          
        });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code === "1") {
                dispatch(actions.FetchActiveAssignments(response.result));
            }   
            else if(response && response.code && (response.code === "11" || response.code === "41")){
                IntertekToaster(parseValdiationMessage(response), 'warningToast fetchactiveassignment');
           
            } 
            dispatch(HideLoader());
        } else {
            IntertekToaster(localConstant.validationMessage.ACTIVE_ASSIGNMENT_FETCH_WENT_WRONG, 'dangerToast fetchactiveassignmentwentwrong');
            dispatch(HideLoader());
        }
};

export const FetchInActiveAssignments = () => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if(state.dashboardReducer.isInactiveAssignmentLoaded){
        dispatch(HideLoader());
        return false;
    } 
    const url = "";
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const params = {
        'assignmentOperatingCompanyCode': selectedCompany
    };
    if (!allCoOrdinator) {
        params.AssignmentContractHoldingCompanyCoordinator = loginUser;
        params.AssignmentContractHoldingCompanyCoordinatorSamAcctName=state.appLayoutReducer.username;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.inActivessignments, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Error fetching Assignment, API not responding','dangerToast fetchAssignmentWrongError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
        if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response && response.code === "1") {
            dispatch(actions.FetchInActiveAssignments(response.result));
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchinactiveassignment');
        }
    dispatch(HideLoader());
}
else{ 
    IntertekToaster(localConstant.validationMessage.INACTIVE_ASSIGNMENT_FETCH_WENT_WRONG, 'dangerToast fetchinactiveassignmentwentwrong');                     
    dispatch(HideLoader());
}
};

export const FetchVisitStatus = () => async(dispatch, getstate) => {
    //if FetchVisitStatus Data is already loaded do nothing, otherwise fetch data
    dispatch(ShowLoader());
    const state = getstate();
    if(state.dashboardReducer.isVisitStatusAprovalsLoaded){
        dispatch(HideLoader());
        return false;
    }    

    const url="";
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const futureDays = state.dashboardReducer.futureDays;
    const params = {
        'visitOperatingCompanyCode': selectedCompany
    };
    if((allCoOrdinator) && (futureDays!=='')){  
        params.VisitFutureDays = futureDays;
                                                                    
    }
    else if((!allCoOrdinator)  && (futureDays!=='') && (futureDays!==7)){

        params.VisitOperatingCompanyCoordinator = loginUser;
        params.VisitOperatingCompanyCoordinatorSamAcctName=state.appLayoutReducer.username;
        params.VisitFutureDays = futureDays;
    }
    else if((futureDays==='') && (allCoOrdinator)){
      
        params.visitOperatingCompanyCode = selectedCompany;
    }
    else{
     
        params.VisitOperatingCompanyCoordinator = loginUser;
        params.VisitOperatingCompanyCoordinatorSamAcctName=state.appLayoutReducer.username;
        params.VisitFutureDays = futureDays;
    }

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.visitStatus, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.VISIT_STATUS_WENT_WRONG,'dangerToast visitStatusError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
    if (response && response.code === "1") {
        dispatch(actions.FetchVisitStatus(response.result));
        dispatch(HideLoader());
    }
    else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast visitStatusError');
      //  IntertekToaster(localConstant.errorMessages.VISIT_STATUS_WENT_WRONG,'dangerToast visitStatusError');
        dispatch(HideLoader());
    }
    else{
        IntertekToaster(localConstant.errorMessages.VISIT_STATUS_WENT_WRONG,'dangerToast visitStatusError1');
        dispatch(HideLoader());
    }
    };

export const FetchTimesheetPendingAproval = () => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if(state.dashboardReducer.isTimeSheetPendingAprovalLoaded){
        dispatch(HideLoader());
        return false;
    }else{

        const url = "";
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const futureDays = state.dashboardReducer.futureDays;
    const params = {
        'timesheetOperatingCompanyCode': selectedCompany
    };

    if((allCoOrdinator) && (futureDays!=='')){
        params.timesheetFutureDays = futureDays;
    }
    else if((!allCoOrdinator)  && (futureDays!=='')){
        params.timesheetOperatingCoordinator = loginUser; 
        params.timesheetOperatingCoordinatorSamAcctName=state.appLayoutReducer.username;
        params.timesheetFutureDays = futureDays;
    }
    else if((futureDays==='') && (allCoOrdinator)){
        params.timesheetOperatingCompanyCode = selectedCompany;
    }
    else{
        params.timesheetOperatingCoordinator = loginUser;
        params.timesheetOperatingCoordinatorSamAcctName=state.appLayoutReducer.username;
    }
    
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.timesheet, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.TIMESHEET_PENDING_APPROVAL_WENT_WRONG,'dangerToast FetchTimesheetPendingAprovalError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
    if (response && response.code === "1") {
        dispatch(actions.FetchTimesheetPendingAproval(response.result));
        dispatch(HideLoader());
    }
    else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast FetchTimesheetPendingAprovalError');
      //  IntertekToaster(localConstant.errorMessages.TIMESHEET_PENDING_APPROVAL_WENT_WRONG,'dangerToast FetchTimesheetPendingAprovalError');
        dispatch(HideLoader());
    }
    else{
        IntertekToaster(localConstant.errorMessages.TIMESHEET_PENDING_APPROVAL_WENT_WRONG,'dangerToast FetchTimesheetPendingAprovalError1');
        dispatch(HideLoader());
    }
    }
   
};

export const FetchContractsNearExpiry = () => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
        const params = {
            'contractStatus': 'o',
            'contractFutureDays': 90,
            contractHoldingCompanyCode:selectedCompany
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(homeAPIConfig.contract, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.CONTRACT_NEAR_EXPIRY,'dangerToast FetchContractsNearExpiryError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchContractsNearExpiry(response.result));
            dispatch(HideLoader());
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast FetchContractsNearExpiryError1');
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.errorMessages.CONTRACT_NEAR_EXPIRY,'dangerToast FetchContractsNearExpiryError');
            dispatch(HideLoader());
        }
};

export const FetchDocumentAproval = (data) => (dispatch, getstate) => {

    dispatch(actions.FetchDocumentAproval(data));
};

export const FetchBudgetMonetary = (data) => async(dispatch, getstate) => {
    return await FetchData(homeAPIConfig.assignments, requestPayload);
};

export const BudgetPropertyChange = (data) => (dispatch,getstate) => {
    dispatch(actions.BudgetPropertyChange(data));
};

export const FetchContractBudget = (data) => (dispatch, getstate) => { 
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const contractStatus = state.dashboardReducer.budgetContractStatus;
    const showMyAssignmentsOnly = state.dashboardReducer.showMyAssignmentsOnly;
    const params = {
        'companyCode':selectedCompany,
        'userName':loginUser,
        'contractStatus':contractStatus,
        'showMyAssignmentsOnly':showMyAssignmentsOnly
    };
    const requestPayload = new RequestPayload(params);
    return axios.get(homeAPIConfig.contractBudget, {
        params: requestPayload.data,
    });
};

export const FetchProjectBudget = (data) => (dispatch, getstate) => {
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const contractStatus = state.dashboardReducer.budgetContractStatus;
    const showMyAssignmentsOnly = state.dashboardReducer.showMyAssignmentsOnly;
    const params = {
        'companyCode':selectedCompany,
        'userName':loginUser,
        'contractStatus':contractStatus,
        'showMyAssignmentsOnly':showMyAssignmentsOnly
    };
    const requestPayload = new RequestPayload(params);
    return axios.get(homeAPIConfig.projectBudget, {
        params: requestPayload.data,
    });
};

export const FetchAssignmentBudget = (data) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = state.appLayoutReducer.loginUser;
    const contractStatus = state.dashboardReducer.budgetContractStatus;
    const showMyAssignmentsOnly = state.dashboardReducer.showMyAssignmentsOnly;
    const params = {
        'companyCode':selectedCompany,
        'userName':loginUser,
        'contractStatus':contractStatus,
        'showMyAssignmentsOnly':showMyAssignmentsOnly
    };
    const requestPayload = new RequestPayload(params);
    return axios.get(homeAPIConfig.assignmentBudget, {
        params: requestPayload.data,
    });
};

export const FetchBudget = (budgetType) => async (dispatch,getstate) =>{
    dispatch(ShowLoader());
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = localStorage.getItem(applicationConstants.Authentication.USER_NAME,);
    const contractStatus = state.dashboardReducer.budgetContractStatus;
    const showMyAssignmentsOnly = state.dashboardReducer.showMyAssignmentsOnly;
    const params = {
        'companyCode':selectedCompany,
        'userName':loginUser,
        'contractStatus':contractStatus,
        'isMyAssignmentOnly':showMyAssignmentsOnly,
    };
    if(budgetType === 'budgetMonetary'){
        params.IsOverBudgetValue = true;
    }
    if(budgetType === 'budgetHour'){
        params.IsOverBudgetHour = true;
    }

    const requestPayload = new RequestPayload(params);
 
    const response = await FetchData(homeAPIConfig.fetchBudget, requestPayload)
        .catch(error => {            
            // console.error(error); // To show the error details in console
            // if(budgetType === 'budgetMonetary'){
            //     IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_BUDGET_MONETARY,'dangerToast fetchAssignmentWrong');
            // }
            // if(budgetType === 'budgetHour'){
            //     IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_BUDGET_HOUR,'dangerToast fetchAssignmentWrong');
            // }
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());          
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchBudget(response.result));           
            dispatch(HideLoader());
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast FetchContractsNearExpiryError12');
            dispatch(HideLoader());
        }
        else{
            if(budgetType === 'budgetMonetary'){
                IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_BUDGET_MONETARY,'dangerToast fetchAssignmentWrong');
            }
            if(budgetType === 'budgetHour'){
                IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_BUDGET_HOUR,'dangerToast fetchAssignmentWrong');
            }
            dispatch(HideLoader());
        }
};

export const FetchDocumentApproval = () => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if(state.dashboardReducer.isDocumentAprovalLoaded){
        dispatch(HideLoader()); 
        return false;
    }  
    const loginUser = state.appLayoutReducer.username;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const params={};
    params.companyCode=state.appLayoutReducer.selectedCompany;
    if (!allCoOrdinator) {
        params.coordinatorName = loginUser;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.documentsForApproval, requestPayload)
      .catch(error => {  
        // console.error(error); // To show the error details in console          
            // IntertekToaster(localConstant.errorMessages.DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());          
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchDocumentApproval(response.result));           
            dispatch(HideLoader());
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchdocapproval');
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.errorMessages.DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            dispatch(HideLoader());
        }
       
};

export const SelectedDocumentToApprove = (payload) =>async (dispatch, getstate) => {
    dispatch(actions.SelectedDocumentToApprove(payload));
    const documentTypeModuleName = payload.moduleName === 'Timesheet' ? 'Visit' : payload.moduleName;
    dispatch(FetchDocumentType(documentTypeModuleName));
    dispatch(FetchCustomerName(payload.moduleCode,payload.moduleRefCode));
};

export const UpdateSelectedRecord=(payload)=>(dispatch,getstate)=>{
    const state = getstate();
    const selectedDocument=state.dashboardReducer.selectedDocumentToApprove;
    const immutatedData=Object.assign({},selectedDocument,payload);
    dispatch(actions.SelectedDocumentToApprove(immutatedData));
};

export const FetchDocumentType=(data)=> async (dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params = {
        'ModuleName': data,
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.documentType, requestPayload)
      .catch(error => {  
        // console.error(error); // To show the error details in console         
            // IntertekToaster(localConstant.errorMessages.DOCUMENT_TYPE_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());          
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchDocumentType(response.result));           
            dispatch(HideLoader());
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchdoctype');
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.errorMessages.DOCUMENT_TYPE_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            dispatch(HideLoader());
        }
};

export const FetchCustomerName =(moduleCode,moduleRefCode)=>async (dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params = {
        'moduleCode':moduleCode,
        'moduleRefCode': moduleRefCode,
    };
    const url = homeAPIConfig.homeBaseURL + homeAPIConfig.documentApproval +  homeAPIConfig.getCustomerName;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
      .catch(error => {   
        // console.error(error); // To show the error details in console         
            // IntertekToaster(localConstant.errorMessages.CUSTOMER_FETCH_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());          
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchCustomerName(response.result));   
            dispatch(FetchContractForDocumentApproval());        
            dispatch(HideLoader());
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchcustormername');
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.errorMessages.CUSTOMER_FETCH_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
            dispatch(HideLoader());  

        }
};

export const FetchContractForDocumentApproval =() => async(dispatch,getstate)=>{
    dispatch(ShowLoader());
    const state = getstate();
    const contractCustomerCode=state.dashboardReducer.selectedDocumentCustomer;
    const params={};
    if(!isEmpty(contractCustomerCode)){
            params['contractCustomerCode']=contractCustomerCode[0];
            // params['contractStatus'] ='O';
            params['contractHoldingCompanyCode'] = state.appLayoutReducer.selectedCompany; //D-1260
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.contract, requestPayload)
    .catch(error => {     
        // console.error(error); // To show the error details in console       
        //   IntertekToaster(localConstant.errorMessages.CONTRACT_FOR_DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchContractForDocumentApproval(response.result));           
          dispatch(HideLoader());
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchcontractfordocapproval');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.CONTRACT_FOR_DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        dispatch(HideLoader()); 
      }
};

export const FetchProjectForDocumentApproval=(payload)=> async (dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params = {
        'ContractNumber':payload
        // 'ProjectStatus':'O'
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.project, requestPayload)
    .catch(error => {  
        // console.error(error); // To show the error details in console          
        //   IntertekToaster(localConstant.errorMessages.PROJECT_FOR_DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchProjectForDocumentApproval(response.result));           
          dispatch(HideLoader());
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchprojectfordocaprval');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.PROJECT_FOR_DOCUMENT_APPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        dispatch(HideLoader());    
      }
};

export const FetchAssignmentForDocumentApproval=(payload)=> async (dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params={
        'AssignmentProjectNumber':payload,
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.assignmentDocumentApproval, requestPayload)
    .catch(error => {   
        // console.error(error); // To show the error details in console         
        //   IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchAssignmentForDocumentApproval(response.result));           
          dispatch(HideLoader());
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchassignmentfordocapproval');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        dispatch(HideLoader());  
      }
};

export const FetchSupplierPOForDocumentApproval=(payload)=>async(dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params={
        'SupplierPOProjectNumber':payload,
        'ProjectStatus':'O'
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(homeAPIConfig.supplierPO, requestPayload)
    .catch(error => {   
        // console.error(error); // To show the error details in console         
        //   IntertekToaster(localConstant.errorMessages.FETCH_SUPPLIERPO_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchSupplierPOForDocumentApproval(response.result));
          dispatch(HideLoader());                     
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchsupplierpo');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.FETCH_SUPPLIERPO_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        dispatch(HideLoader());      
      }
};

export const FetchTimesheetForDocumentApproval=(payload)=>async(dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params={
        'TimesheetAssignmentId':payload
    };
    const url = homeAPIConfig.homeBaseURL + homeAPIConfig.timesheet + homeAPIConfig.getTimeSheetDocumentApproval;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
    .catch(error => {   
        // console.error(error); // To show the error details in console         
        //   IntertekToaster(localConstant.errorMessages.FETCH_TIMESHEET_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchTimesheetForDocumentApproval(response.result));  
          dispatch(HideLoader());                   
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchtimesheetfordocapproval');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.FETCH_TIMESHEET_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        dispatch(HideLoader());         
      }
};

export const FetchVisitForDocumentApproval=(payload)=>async(dispatch,getstate)=>{
    dispatch(ShowLoader());
    const params={
        'VisitAssignmentId':payload
    };
    const url= homeAPIConfig.homeBaseURL + homeAPIConfig.visitStatus + homeAPIConfig.getVisitDocumentApproval;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
    .catch(error => {  
        // console.error(error); // To show the error details in console          
        //   IntertekToaster(localConstant.errorMessages.FETCH_VISIT_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });
      if (response && response.code === "1") {
          dispatch(actions.FetchVisitForDocumentApproval(response.result));           
          dispatch(HideLoader());          
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchvisitfordocapproval');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.FETCH_VISIT_FOR_DOCUMENTAPPROVAL_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong'); 
        dispatch(HideLoader());    
      }
};

export const RejectDocument = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    dispatch(actions.SelectedDocumentToApprove(payload));
    if(!isEmpty(payload)){
        payload.recordStatus='M';
        payload.status="R";
        payload.isForApproval=false;
        payload.isVisibleToTS=getTSVisible({ docTypes:state.masterDataReducer.documentTypeMasterData, 
            docTypeName : payload.documentType,
            moduleName : payload.moduleName === 'Timesheet' ? 'Visit' : payload.moduleName });
    }
    const documentPayload = [];
    documentPayload.push(payload);
    const requestPayload = new RequestPayload(documentPayload);
    const response = await CreateData(homeAPIConfig.documentApproval, requestPayload)
    .catch(error => {      
        // console.error(error); // To show the error details in console      
        //   IntertekToaster(localConstant.errorMessages.REJECT_DOCUMENT_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });

      if (response && response.code === "1") {     
          IntertekToaster('Document Deleted successfully', 'successToast TimeoffRequestSubmission');
          dispatch(HideLoader());
          const documentApproval=Object.assign([],state.dashboardReducer.documentApproval);
          const index = documentApproval.findIndex(iteratedValue =>(iteratedValue.id===payload.id));
          if(index >= 0){
            documentApproval[index] = payload;
            dispatch(actions.FetchDocumentApproval(documentApproval));
            dispatch(FetchDashboardCount());
        }
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast rejectdocument');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.REJECT_DOCUMENT_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        dispatch(HideLoader());     
      }
};

export const ApproveDocument =(payload) => async (dispatch,getstate)=>{
    dispatch(ShowLoader());
    const state = getstate();
    if(!isEmpty(payload)){
        payload.recordStatus='M';
        payload.status="C"; 
        payload.isForApproval=false; 
        payload.approvedBy=localStorage.getItem(applicationConstants.Authentication.USER_NAME);
        payload.approvalDate=moment();
        payload.isVisibleToTS=getTSVisible({ docTypes:state.masterDataReducer.documentTypeMasterData, 
                                            docTypeName : payload.documentType,
                                            moduleName : payload.moduleName === 'Timesheet' ? 'Visit' : payload.moduleName });
    }
    const documentPayload = [];
    documentPayload.push(payload);
    const requestPayload = new RequestPayload(documentPayload);
    const response = await CreateData(homeAPIConfig.documentApproval, requestPayload)
    .catch(error => {  
        // console.error(error); // To show the error details in console          
        //   IntertekToaster(localConstant.errorMessages.APPROVE_DOCUMENT_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          dispatch(HideLoader());          
      });

      if (response && response.code === "1") {  
        IntertekToaster('Document Approved successfully', 'successToast TimeoffRequestSubmission');
          dispatch(HideLoader());
          const documentApproval=Object.assign([],state.dashboardReducer.documentApproval);
          const index = documentApproval.findIndex(iteratedValue =>(iteratedValue.id===payload.id));
          if(index >= 0){
            documentApproval[index] = payload;
            dispatch(actions.FetchDocumentApproval(documentApproval));
            dispatch(FetchDashboardCount());
        }
      }
      else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast approvedoc');
        dispatch(HideLoader());
    }
      else{
        IntertekToaster(localConstant.errorMessages.APPROVE_DOCUMENT_WENT_WRONG,'dangerToast fetchDocumentApprovalWrong');
        dispatch(HideLoader());       
      }
};

export const ToggleAllCoordinator = (data) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(ClearMySearchData());
    dispatch(ClearMyTasksData());
    dispatch(actions.ToggleAllCoordinator(data));
};

export const Dashboardrefresh = (data) => (dispatch, getstate) => {
    switch (data) {
        case AppDashBoardRoutes.assignments: {
            dispatch(FetchActiveAssignments());
        }
            break;
        case AppDashBoardRoutes.inactiveassignments: {
            dispatch(FetchInActiveAssignments());
        }
            break;
        case AppDashBoardRoutes.visitStatus: {
            dispatch(FetchVisitStatus());
        }
            break;
        case AppDashBoardRoutes.timesheet: {
            dispatch(FetchTimesheetPendingAproval());
        }
            break;
        case AppDashBoardRoutes.contractsNearExpiry: {
            dispatch(FetchContractsNearExpiry());
        }
            break;
        case AppDashBoardRoutes.budgetMonetary: {
            dispatch(FetchBudget('budgetMonetary'));
        }
            break;
        case AppDashBoardRoutes.BudgetHours: {
            dispatch(FetchBudget('budgetHour'));
        }
            break;
        case AppDashBoardRoutes.documentAproval: {
            dispatch(FetchDocumentApproval());
        }
            break;
        case AppDashBoardRoutes.mysearch: {
            dispatch(FetchMySearchData());
        }
            break;
        case AppDashBoardRoutes.mytasks: {
            dispatch(FetchTechSpecMytaskData());
        }
            break;
        case AppDashBoardRoutes.documentation: {
            dispatch(DashboardDocumentationDetails());
        }
            break;
    }
};

/**
 * 
 */
export const FetchDashboardCount = () => (dispatch,getstate)=>{  //changes happened for home dashboard my task and my search count not refreshing
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const loginUser = encodeURIComponent(state.appLayoutReducer.loginUser); //Added for D1302 French Letter Decode Fix
    const samAccountName = state.appLayoutReducer.username;
    const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const futureDays = state.dashboardReducer.futureDays;
    const moduleCode = 'ASGMNT,CNT,PRJ,SUPPO,VST,TIME,TS';   
    let url = "";
    if(allCoOrdinator){
        url = homeAPIConfig.homeBaseURL + homeAPIConfig.dashBoardCount + '&assignmentOperatingCompanyCode='+selectedCompany+'&visitOperatingCompanyCode='+selectedCompany+'&VisitFutureDays='+futureDays+'&timesheetOperatingCompanyCode='+selectedCompany+'&timesheetFutureDays='+futureDays+'&CHCompanyCode='+selectedCompany+'&contractStatus=o&contractFutureDays=90&ContractHoldingCompanyCode='+selectedCompany+'&userName='+samAccountName+'&companyCode='+selectedCompany+'&assignedTo='+samAccountName+'&IsForApproval=true&status=C&ModuleCode='+moduleCode+'&CompanyCode='+selectedCompany;
    }else{
        url = homeAPIConfig.homeBaseURL + homeAPIConfig.dashBoardCount + '&assignmentOperatingCompanyCode=' + selectedCompany + '&AssignmentContractHoldingCompanyCoordinator=' + loginUser + '&visitOperatingCompanyCode=' + selectedCompany + '&VisitOperatingCompanyCoordinator=' + loginUser + '&VisitFutureDays=' + futureDays + '&timesheetOperatingCompanyCode=' + selectedCompany + '&TimesheetOperatingCoordinator=' + loginUser + '&timesheetFutureDays=' + futureDays + '&CHCompanyCode=' + selectedCompany + '&contractStatus=o&contractFutureDays=90&ContractHoldingCompanyCode=' + selectedCompany + '&IsForApproval=true&status=C&CoordinatorName=' + samAccountName + '&userName=' + samAccountName + '&companyCode=' + selectedCompany + '&assignedTo=' + samAccountName + '&ModuleCode=' + moduleCode + '&assignmentContractHoldingCompanyCoordinatorSamAcctName=' + state.appLayoutReducer.username + '&timesheetOperatingCoordinatorSamAcctName=' + state.appLayoutReducer.username + '&visitOperatingCompanyCoordinatorSamAcctName=' + state.appLayoutReducer.username + '&CompanyCode=' + selectedCompany;
    }
    processApiRequest(url, {
        method: 'get'
    })
    .then(response => {
        if (response&&response.status==200) {
            let value=response.data;  
            //D363 CR change
            if(getstate().RootTechSpecReducer.TechSpecDetailReducer.isMyTasksLoaded){
                const count={ 'MyTaskCount':getstate().dashboardReducer.count.MyTaskCount,
                              'MySearchCount':getstate().dashboardReducer.count.MySearchCount };
                value=Object.assign({},value,count);
            }
                dispatch(actions.FetchDashboardCount(value));
        }    
        else{
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast DashboardCountError');
        }  
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error+' Failed to show dashboard tabs counts','dangerToast DashboardCountError');        
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

//Clear Dashboard Reducer

export const ClearDashboardReducer = () => (dispatch,getstate)=>{
    dispatch(ClearMySearchData());
    dispatch(ClearMyTasksData());
    dispatch(actions.ClearDashboardReducer());
};
export const RejectDocumentApproval = (data) => (dispatch) => {
    dispatch(actions.RejectDocumentApproval(data));
};

//Fetch Technical Specialist Dashboard Message 
export const GetTechSpecDashboardCompanyMessage =()=> async (dispatch)=>{
    dispatch(ShowLoader());
    const ccode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));
    const url=  StringFormat(homeAPIConfig.techSpechDashboardMessage,ccode);
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.EXTRANET_RESOURCE_COMPANY_WENT_WRONG, 'dangerToast getTechSpecDashboardCompanyMessage');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (response && response.code === "1") {
        dispatch(actions.GetTechSpecDashboardCompanyMessage(response.result.techSpecialistExtranetComment));
        dispatch(HideLoader());
    }
    else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchtechspecdashboardcompmessage');
        dispatch(HideLoader());
    }
    else{
        IntertekToaster(localConstant.errorMessages.EXTRANET_RESOURCE_COMPANY_WENT_WRONG, 'dangerToast getTechSpecDashboardCompanyMessage');
        dispatch(HideLoader());
    }
    
};

export const MyTaskPropertyChange = (data) => (dispatch) => {
    dispatch(actions.MyTaskPropertyChange(data));    
    return true;
};
 /*
    Added For home dashboard my task and my search count not refreshing
*/
export const MySearchPropertyChange = (data) => (dispatch) =>{
    dispatch(actions.MySearchPropertyChange(data));    
    return true;
};
