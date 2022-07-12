import { timesheetActionTypes } from '../../constants/actionTypes';
import { timesheetAPIConfig, RequestPayload, assignmentAPIConfig, contractAPIConfig, visitAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData, CreateData, DeleteData } from '../../services/api/baseApiService';
import moment, { locale } from 'moment';
import {
    ShowLoader,
    HideLoader,
    UpdateCurrentPage,
    UpdateCurrentModule,
    FetchEmailTemplate,
    UpdateInteractionMode,
    RemoveDocumentsFromDB
} from '../../common/commonAction';
import {
    FetchTechSpecRateSchedules,
    FetchAssignmentAdditionalExpensesForTimesheet
} from '../../actions/timesheet/timesheetTechSpecsAccountsAction';
import {
    getlocalizeData,
    parseValdiationMessage,
    isEmptyReturnDefault,
    isEmptyOrUndefine,
    isEmpty,
    FilterSave,
    deepCopy,
    getNestedObject,
    isUndefined
} from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import arrayUtil from '../../utils/arrayUtil';
import { SuccessAlert, ValidationAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import { HandleMenuAction } from '../../components/sideMenu/sideMenuAction';
import { FetchExpenseType } from '../../common/masterData/masterDataActions';
import {
    FetchProjectClientNotifications,
    FetchCustomerContact
} from '../project/clientNotificationAction';
import { FetchReferencetypes } from './timesheetReferenceAction';
import { 
    isOperatorCompany,
    isCoordinatorCompany
} from '../../selectors/timesheetSelector';
import { applicationConstants } from '../../constants/appConstants';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';
import { securitymodule } from '../../constants/securityConstant';
import { moduleViewAllRights_Modified } from '../../utils/permissionUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchAssignmentTimesheets: (payload) => ({
        type: timesheetActionTypes.FETCH_ASSIGNMENT_TIMESHEETS,
        data: payload
    }),
    AssignmentTimesheetStatus: (payload) => ({
        type: timesheetActionTypes.SELECTED_TIMESHEET_STATUS,
        data: payload
    }),
    GetSelectedTimesheet: (payload) => ({
        type: timesheetActionTypes.SELECTED_TIMESHEET,
        data: payload
    }),
    FetchTimesheetSearchDataSuccess: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_SEARCH_SUCCESS,
        data: payload
    }),
    FetchTimesheetSearchDataError: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_SEARCH_ERROR,
        data: payload
    }),
    FetchAssignmentToAddTimesheet: (payload) => ({
        type: timesheetActionTypes.FETCH_ASSIGNMENT_TO_ADD_TIMESHEET,
        data: payload
    }),
    ClearTimesheetSearchResults: () => ({
        type: timesheetActionTypes.CLEAR_TIMESHEET_SEARCH_RESULTS,
    }),
    FetchTimesheetDetailSuccess: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_DETAIL_SUCCESS,
        data: payload
    }),
    UpdateTimesheetStatus: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_STATUS,
        data: payload
    }),
    CreateNewTimesheet: (payload) => ({
        type: timesheetActionTypes.CREATE_NEW_TIMESHEET,
        data: payload
    }),
    SelectedTimesheetTechSpecs: (payload) => ({
        type: timesheetActionTypes.SELECTED_TIMESHEET_TECHNICAL_SPECIALISTS,
        data: payload
    }),
    UpdateShowAllRates: (payload) => ({
        type: timesheetActionTypes.UPDATE_SHOW_ALL_RATES,
        data: payload
    }),
    CustomerReportingNotificationContants: (payload) => ({
        type: timesheetActionTypes.CUSTOMER_REPORTING_NOTIFICATION_CONTANT,
        data: payload
    }),
    FetchTimesheetValidationData: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_VALIDATION_DATA,
        data: payload
    }),
    ClearTimesheetCalendarData: (payload) => ({
        type: timesheetActionTypes.CLEAR_CALENDAR_DATA,
        data: payload
    }),
    FetchRateSchedules: (payload) => ({
        type: timesheetActionTypes.ERROR_FETCHING_TIMESHEET_CONTRACT_SCHEDULES,
        data: payload
    }),
    TimesheetButtonDisable: () => ({
        type: timesheetActionTypes.TIMESHEET_BUTTON_DISABLE,
        data: true
    }),
    ClearTimesheetDetails: () => ({
        type: timesheetActionTypes.CLEAR_TIMESHEET_DETAILS      
    }),
    SaveValidTimesheetCalendarDataForSave: (payload) => ({
        type: timesheetActionTypes.TIMESHEET_VALID_CALENDAR_DATA_SAVE,   
        data: payload     
    }),
    UpdateTimesheetExchangeRates: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_EXCHANGE_RATES,
        data: payload
    }),
    FetchTimesheetStatus: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_STATUS,
        data: payload
    }),
    FetchUnusedReason: (payload) => ({
        type: timesheetActionTypes.FETCH_UNUSED_REASON,
        data: payload
    }),
};

export const ClearTimesheetDetails = (payload) => (dispatch) => {
    dispatch(actions.ClearTimesheetDetails());
};

export const GetSelectedTimesheet = (payload) => (dispatch) => {
    dispatch(actions.GetSelectedTimesheet(payload));
};

export const FetchAssignmentTimesheets = () => async (dispatch, getstate) => {
    const assignmentId = getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo && getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;
    if (assignmentId) {
        dispatch(FetchTimesheetSearchResults({
            assignmentId: assignmentId
        }));
    }
    // const assignmentTimesheets = timesheetAPIConfig.timesheets+timesheetAPIConfig.GetTimesheets;
    // if (assignmentId) {
    //     const params = {
    //         TimesheetAssignmentId: assignmentId
    //     };
    //     const requestPayload = new RequestPayload(params);
    //     const response = await FetchData(assignmentTimesheets, requestPayload)
    //         .catch(error => {
    //             IntertekToaster(localConstant.errorMessages.ASSIGNMENT_TIMESHEET_FETCH_FAILED, 'dangerToast assignmentTimesheet');
    //         });
    //     if (response && response.code === "1") {
    //         const res = {
    //             responseResult: response.result,
    //             //selectedValue:status
    //         };
    //         dispatch(actions.FetchAssignmentTimesheets(res.responseResult));
    //         //dispatch(actions.AssignmentTimesheetStatus(res.selectedValue));
    //     }
    //     else {
    //         IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast assignemntSomthingWrong');
    //     }
    // }
};

export const FetchTimesheetSearchResults = (data) => async (dispatch, getstate) => {
    
    dispatch(ShowLoader());
    dispatch(actions.FetchTimesheetSearchDataSuccess([]));

    const searchUrl = timesheetAPIConfig.searchTimesheets;

    const params = {};
    if(getstate().CommonReducer.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE) {
        params.modulename = "TIME";  
    }
    if (data.timesheetCustomerName) {
        params.timesheetCustomerName = data.timesheetCustomerName;
        params.CustomerId = data.timesheetCustomerId;
    }
    if (data.customerCode) {
        params.timesheetCustomerCode = data.customerCode;
    }
    if (data.timesheetAssignmentNumber) {
        params.timesheetAssignmentNumber = parseInt(data.timesheetAssignmentNumber);
    }
    if (data.timesheetProjectNumber) {
        params.timesheetProjectNumber = parseInt(data.timesheetProjectNumber);
    }
    if (data.timesheetContractNumber) {
        params.timesheetContractNumber = data.timesheetContractNumber;
    }

    if (data.timesheetEndDate) {        
        params.timesheetEndDate = data.timesheetEndDate;
    }

    if (data.timesheetStartDate) {
        params.timesheetStartDate = data.timesheetStartDate;
    }
    if (data.timesheetContractHolderCompanyCode) {
        params.timesheetContractHolderCompanyCode = data.timesheetContractHolderCompanyCode;
    }

    if (data.timesheetOperatingCompanyCode) {
        params.timesheetOperatingCompanyCode = data.timesheetOperatingCompanyCode;
    }
    if (data.timesheetContractHolderCompany) {
        // params.timesheetContractHolderCompany = data.timesheetContractHolderCompany;
        params.ContractCompanyId = data.timesheetContractHolderCompany;
    }

    if (data.timesheetOperatingCompany) {
        // params.timesheetOperatingCompany = data.timesheetOperatingCompany;
        params.OperatingCompanyId = data.timesheetOperatingCompany;
    }

    if (data.timesheetContractHolderCoordinator) {
        // params.timesheetContractHolderCoordinatorCode = data.timesheetContractHolderCoordinator;
        params.ContractCoordinatorId = data.timesheetContractHolderCoordinator;
    }

    if (data.timesheetOperatingCompanyCoordinator) {
        // params.timesheetOperatingCompanyCoordinatorCode = data.timesheetOperatingCompanyCoordinator;
        params.OperatingCoordinatorId = data.timesheetOperatingCompanyCoordinator;
    }

    if (data.technicalSpecialistName) {
        params.technicalSpecialistName = data.technicalSpecialistName;
    }

    if (data.timesheetStatus) {
        params.timesheetStatus = data.timesheetStatus;
    }
    if (data.unusedReason) {
        params.unusedReason = data.unusedReason;
    }
    if (data.timesheetDescription) {
        params.timesheetDescription = data.timesheetDescription;
    }

    if (data.timesheetDocumentId) {
        params.timesheetDocumentId = data.timesheetDocumentId;
    }
    if (data.documentSearchText) {
        params.documentSearchText = data.documentSearchText;
    }
    if (!isEmpty(data.searchDocumentType )) {
        params.searchDocumentType= data.searchDocumentType;
    }
    if (data.assignmentId) {
        params.timesheetAssignmentId = data.assignmentId;
    }

    if (data.timesheetCategory) {
        params.categoryId = data.timesheetCategory;
    }
    if (data.timesheetSubCategory) {
        params.subCategoryId = data.timesheetSubCategory;
    }
    if (data.timesheetService) {
        params.serviceId = data.timesheetService;
    }

    //search optimization
    if(data.OffSet){
        params.OffSet = data.OffSet;
    }
    //search optimization
    if (data.timesheetSearchTotalCount) {
        params.TotalCount = data.timesheetSearchTotalCount;
    }
    if (data.isExport) //Added for Export to CSV
        params.isExport = data.isExport;

    params.isOnlyViewTimesheet = data.isOnlyViewTimesheet;  
    params.loggedInCompanyCode = data.loggedInCompanyCode;
    params.loggedInCompanyId = data.loggedInCompanyId;

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(searchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchTimesheetSearchDataError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_SEARCH_RESULTS, 'dangerToast FetchAssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code && response.code === "1") {
        dispatch(actions.FetchTimesheetSearchDataSuccess(response.result));
        dispatch(HideLoader());
        return response;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_SEARCH_RESULTS, 'dangerToast FetchAssignmentSearchResultsErr');
    }
    dispatch(HideLoader());
};
 /**
 * method will fetch assignment details for timesheet creation
  * @param {*} assignmentId id to fetch assignment details for timesheet creation
  * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc 
  */
export const FetchAssignmentForTimesheetCreation = (assignmentId, isFetchLookValues) => (dispatch, getstate) => {  
    let timesheetDetail = {};
    const state = getstate(),
        assignemtList = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentList);
        
    if (!assignmentId)
        assignmentId = state.rootAssignmentReducer.selectedAssignmentId;
    const assignment = arrayUtil.FilterGetObject(assignemtList, 'assignmentId', assignmentId);

    if (!isEmptyOrUndefine(assignment)) {
        timesheetDetail = {
            TimesheetInfo: {
                assignmentCreatedDate: assignment.assignmentCreatedDate,
                customerProjectName: assignment.assignmentCustomerProjectName,
                assignmentId: assignment.assignmentId,
                timesheetId: null,
                timesheetAssignmentNumber: assignment.assignmentNumber,
                timesheetAssignmentId: assignment.assignmentId,
                timesheetContractCompanyCode: assignment.assignmentContractHoldingCompanyCode,
                timesheetContractCompany: assignment.assignmentContractHoldingCompany,
                timesheetContractCoordinator: assignment.assignmentContractHoldingCompanyCoordinator,
                timesheetContractCoordinatorCode: assignment.assignmentContractHoldingCompanyCoordinatorCode,
                isContractHoldingCompanyActive: assignment.isContractHoldingCompanyActive,
                timesheetCustomerCode: assignment.assignmentCustomerCode,
                timesheetCustomerName: assignment.assignmentCustomerName,
                timesheetContractNumber: assignment.assignmentContractNumber,
                timesheetOperatingCompanyCode: assignment.assignmentOperatingCompanyCode,
                timesheetOperatingCompany: assignment.assignmentOperatingCompany,
                timesheetOperatingCoordinator: assignment.assignmentOperatingCompanyCoordinator,
                timesheetOperatingCoordinatorCode: assignment.assignmentOperatingCompanyCoordinatorCode,
                timesheetProjectNumber: assignment.assignmentProjectNumber,
                assignmentTechSpecialists: assignment.techSpecialists,
                assignmentClientReportingRequirements: assignment.clientReportingRequirements,
                assignmentProjectBusinessUnit: assignment.assignmentProjectBusinessUnit,
                timesheetStatus: 'N',
                projectInvoiceInstructionNotes: assignment.projectInvoiceInstructionNotes,
                assignmentProjectWorkFlow: assignment.assignmentProjectWorkFlow,
                assignmentStatus:assignment.assignmentStatus,
                isVisitOnPopUp:assignment.isVisitOnPopUp
            },
            TimesheetTechnicalSpecialists: null,
            TimesheetTechnicalSpecialistConsumables: null,
            TimesheetTechnicalSpecialistExpenses: null,
            TimesheetTechnicalSpecialistTimes: null,
            TimesheetTechnicalSpecialistTravels: null,
            TimesheetReferences: null,
            TimesheetDocuments: null,
            TimesheetNotes: null,
        };
        dispatch(actions.FetchAssignmentToAddTimesheet(timesheetDetail));
        dispatch(GetTimesheetInitialData(timesheetDetail.TimesheetInfo,isFetchLookValues));
        dispatch(UpdateCurrentPage(localConstant.timesheet.CREATE_TIMESHEET_MODE));
        dispatch(FetchTimesheetStatus());
        if(timesheetDetail && timesheetDetail.TimesheetInfo && !(isOperatorCompany(timesheetDetail.TimesheetInfo.timesheetOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
            || isCoordinatorCompany(timesheetDetail.TimesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany))) {
            dispatch(UpdateInteractionMode(timesheetDetail.TimesheetInfo.timesheetContractCompanyCode));
        }
        if(!timesheetDetail.TimesheetInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));
        dispatch(UpdateCurrentModule('timesheet'));
    } else {
        if (!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo)) {
            dispatch(CreateNewTimesheet());
        }
    }
};

export const ClearTimesheetSearchResults = () => (dispatch, getstate) => {
    dispatch(actions.ClearTimesheetSearchResults());
};
 /**
 * method will fetch timesheet details
  * @param {*} timesheetId id to fetch timesheet details
  * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc 
  */
export const FetchTimesheetDetail = (timesheetId,isFetchLookValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!timesheetId) {
        timesheetId = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo ?
            state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetId :
            state.rootTimesheetReducer.selectedTimesheetId;
    }
    if (!timesheetId) {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchTimesheetDetail');
        dispatch(HideLoader());
        return false;
    }
    const url = StringFormat(timesheetAPIConfig.detail, timesheetId);
    const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchTimesheetDetailError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return error;
        });
        
        //646 Fixes
        if (!isEmpty(response) && isEmpty(response.TimesheetInfo)){
            dispatch(HideLoader());
            dispatch(ChangeDataAvailableStatus(true));
            return response;
        }

    if (!isEmpty(response) && !isEmpty(response.TimesheetInfo)) {
        // ITK D - 1131 #2.1 initial sort for line items.
        if(response.TimesheetTechnicalSpecialistTimes){
            response.TimesheetTechnicalSpecialistTimes = arrayUtil.sort(response.TimesheetTechnicalSpecialistTimes,'expenseDate','desc');
            response.TimesheetTechnicalSpecialistTimes = arrayUtil.sort(response.TimesheetTechnicalSpecialistTimes,'chargeExpenseType','asc');
        }
        if(response.TimesheetTechnicalSpecialistExpenses){
            response.TimesheetTechnicalSpecialistExpenses = arrayUtil.sort(response.TimesheetTechnicalSpecialistExpenses,'expenseDate','desc');
            response.TimesheetTechnicalSpecialistExpenses = arrayUtil.sort(response.TimesheetTechnicalSpecialistExpenses,'chargeExpenseType','asc');
        }
        if(response.TimesheetTechnicalSpecialistTravels){
            response.TimesheetTechnicalSpecialistTravels = arrayUtil.sort(response.TimesheetTechnicalSpecialistTravels,'expenseDate','desc');
            response.TimesheetTechnicalSpecialistTravels = arrayUtil.sort(response.TimesheetTechnicalSpecialistTravels,'chargeExpenseType','asc');
        }
        if(response.TimesheetTechnicalSpecialistConsumables){
            response.TimesheetTechnicalSpecialistConsumables = arrayUtil.sort(response.TimesheetTechnicalSpecialistConsumables,'expenseDate','desc');
            response.TimesheetTechnicalSpecialistConsumables = arrayUtil.sort(response.TimesheetTechnicalSpecialistConsumables,'chargeExpenseType','asc');
        }
        dispatch(actions.FetchTimesheetDetailSuccess(response));
        dispatch(GetTimesheetInitialData(response.TimesheetInfo,isFetchLookValues));
        dispatch(UpdateCurrentPage(localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE));     
        dispatch(FetchTimesheetStatus()); 
        dispatch(FetchUnusedReason()); 
        if(!(isOperatorCompany(response.TimesheetInfo.timesheetOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
            || isCoordinatorCompany(response.TimesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany))) {
            dispatch(UpdateInteractionMode(response.TimesheetInfo.timesheetContractCompanyCode));
        }
        if(!response.TimesheetInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));
        const { timesheetContractCompanyCode, timesheetOperatingCompanyCode, isContractHoldingCompanyActive } = response.TimesheetInfo;
        const isMandatoryView = !isContractHoldingCompanyActive;

        // const isViewAllTimesheet = isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies).length > 0;
        const isViewAllTimesheet = moduleViewAllRights_Modified(securitymodule.TIMESHEET, state.masterDataReducer.viewAllRightsCompanies);
        // dispatch(UpdateInteractionMode([ timesheetContractCompanyCode,timesheetOperatingCompanyCode ],securitymodule.TIMESHEET,isViewAllTimesheet,isMandatoryView));
        dispatch(UpdateInteractionMode([ timesheetContractCompanyCode,timesheetOperatingCompanyCode ],isViewAllTimesheet,isMandatoryView));
        dispatch(UpdateCurrentModule('timesheet'));
        
    };
    dispatch(HideLoader());
    return response;
};

const GetTimesheetInitialData = (data, isFetchLookValues) => (dispatch, getstate) => {
    //set isFetchLookValues to true when we fetch visit initially
    //on save, cancel we dont need to fetch data (like dropdowns, rateschedules,Techspecs) that already exists
    if (isFetchLookValues === true || isFetchLookValues === undefined) {
        dispatch(FetchExpenseType());
        dispatch(FetchAssignmentAdditionalExpensesForTimesheet(data.timesheetAssignmentId));
        dispatch(FetchTechSpecRateSchedules());
        dispatch(FetchTimesheetValidationData(data.timesheetAssignmentId));
        //dispatch(FetchRateSchedules());
    }
    if (!isEmptyOrUndefine(data.techSpecialists)) {
        const techSpecs = data.techSpecialists && data.techSpecialists.map(eachItem => {
            return { value: eachItem.pin, label: eachItem.fullName };
        });
        dispatch(actions.SelectedTimesheetTechSpecs(techSpecs));
    }
};

/**
 * Save Timesheet Detail
 */
export const SaveTimesheetDetails = (data, isRejectSave) => async (dispatch, getstate) => {
    const state = getstate();
    let timesheetData = deepCopy(state.rootTimesheetReducer.timesheetDetail);
    let timesheetCalendarData = state.rootTimesheetReducer.timesheetCalendarData;
    // let timesheetData = state.rootTimesheetReducer.timesheetDetail;
    const currentPage = state.CommonReducer.currentPage;

    if (data.timesheetStatus && data.timesheetStatus !== timesheetData.TimesheetInfo.timesheetStatus) {
        timesheetData.TimesheetInfo = Object.assign({}, timesheetData.TimesheetInfo, data);
    }

    if (!isEmpty(timesheetData.TimesheetInfo.timesheetStartDate)) {
        timesheetData.TimesheetInfo.timesheetStartDate = moment(timesheetData.TimesheetInfo.timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(timesheetData.TimesheetInfo.timesheetEndDate)) {
        timesheetData.TimesheetInfo.timesheetEndDate = moment(timesheetData.TimesheetInfo.timesheetEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(timesheetData.TimesheetInfo.timesheetExpectedCompleteDate)) {
        timesheetData.TimesheetInfo.timesheetExpectedCompleteDate = moment(timesheetData.TimesheetInfo.timesheetExpectedCompleteDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    //Commented the code for CR8
    // let updateTimesheetStatus = false;
    // if(timesheetData.TimesheetInfo.timesheetStatus === 'N') {
    //     if(timesheetData.TimesheetTechnicalSpecialistTimes && timesheetData.TimesheetTechnicalSpecialistTimes !== null && timesheetData.TimesheetTechnicalSpecialistTimes.length > 0) {            
    //         updateTimesheetStatus = (timesheetData.TimesheetTechnicalSpecialistTimes.filter(x => x.recordStatus === "D" && (x.chargeTotalUnit > 0 
    //                 && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;                        
    //     }
    //     if(timesheetData.TimesheetTechnicalSpecialistExpenses && timesheetData.TimesheetTechnicalSpecialistExpenses !== null && timesheetData.TimesheetTechnicalSpecialistExpenses.length > 0 && updateTimesheetStatus === false) {            
    //         updateTimesheetStatus = (timesheetData.TimesheetTechnicalSpecialistExpenses.filter(x => x.recordStatus === "D" && (x.chargeUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }
    //     if(timesheetData.TimesheetTechnicalSpecialistTravels && timesheetData.TimesheetTechnicalSpecialistTravels !== null && timesheetData.TimesheetTechnicalSpecialistTravels.length > 0 && updateTimesheetStatus === false) {            
    //         updateTimesheetStatus = (timesheetData.TimesheetTechnicalSpecialistTravels.filter(x => x.recordStatus === "D" && (x.chargeUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }
    //     if(timesheetData.TimesheetTechnicalSpecialistConsumables && timesheetData.TimesheetTechnicalSpecialistConsumables !== null && timesheetData.TimesheetTechnicalSpecialistConsumables.length > 0 && updateTimesheetStatus === false) {            
    //         updateTimesheetStatus = (timesheetData.TimesheetTechnicalSpecialistConsumables.filter(x => x.recordStatus === "D" && (x.chargeUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }

    //     if(updateTimesheetStatus === true) {
    //         timesheetData.TimesheetInfo.timesheetStatus = 'C';
    //     }
    // }
    if(!isEmptyOrUndefine(timesheetData.TimesheetTechnicalSpecialistExpenses)) {
        timesheetData.TimesheetTechnicalSpecialistExpenses.forEach(row => {
            if(row.recordStatus !== "D" && ((isEmptyOrUndefine(row.chargeRateExchange) || row.chargeRateExchange === null) || (row.chargeRateExchange === 0 && row.recordStatus === null))) {
                row.chargeRateExchange = "0";
                row.recordStatus = row.recordStatus === "N" ? "N" : "M";
            }
            if(row.recordStatus !== "D" && ((isEmptyOrUndefine(row.payRateExchange) || row.payRateExchange === null) || (row.payRateExchange === 0 && row.recordStatus === null))) {
                row.payRateExchange = "0";
                row.recordStatus = row.recordStatus === "N" ? "N" : "M";
            }
            //IGO Defect 966
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
    //IGO Defect 966
    if(!isEmptyOrUndefine(timesheetData.TimesheetTechnicalSpecialistTimes)) {
        timesheetData.TimesheetTechnicalSpecialistTimes.forEach(row => {            
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
    //IGO Defect 966
    if(!isEmptyOrUndefine(timesheetData.TimesheetTechnicalSpecialistTravels)) {
        timesheetData.TimesheetTechnicalSpecialistTravels.forEach(row => {            
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
    //IGO Defect 966
    if(!isEmptyOrUndefine(timesheetData.TimesheetTechnicalSpecialistConsumables)) {
        timesheetData.TimesheetTechnicalSpecialistConsumables.forEach(row => {            
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
      //Defect ID 702 - customer visible switch can Enable while document Upload 
     // As per discussion with Sumit and Bhavithira, Documnt Type 'Report - Inspection' Based customer Visible Switch On only for timesheetStatus 'A' 
     if(!isEmptyOrUndefine(timesheetData.TimesheetDocuments)) {
        timesheetData.TimesheetDocuments.forEach(row => {
            if(row.documentType === applicationConstants.customerVisibleAprovedStauts.ReportInspection && timesheetData.TimesheetInfo.timesheetStatus === 'A' ) {
                row.isVisibleToCustomer = true;
            }
        });
    }
    
    let saveResponse = {};
    timesheetCalendarData=state.rootTimesheetReducer.validCalendarData;
    if (currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
        timesheetData.TimesheetInfo.recordStatus = "N";
        timesheetData.TimesheetInfo.actionByUser = state.appLayoutReducer.username;
        timesheetData.TimesheetInfo.isAuditUpdate = true;
        if (timesheetCalendarData)
            timesheetData.TechnicalSpecialistCalendarList = timesheetCalendarData;

        //Rames : To-do  for Timesheet fetch await
        saveResponse = await dispatch(CreateTimesheetDetails(timesheetData, isRejectSave));
        if (saveResponse.code === "1" && saveResponse.result && saveResponse.result.timesheetId) {
            saveResponse = await dispatch(FetchTimesheetDetail(saveResponse.result.timesheetId, false));
        }
    }
    if (currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
        timesheetData.TimesheetInfo.recordStatus = "M";
        timesheetData.TimesheetInfo.modifiedBy = state.appLayoutReducer.username;
        timesheetData.TimesheetInfo.isAuditUpdate = true;
        if (timesheetCalendarData)
            timesheetData.TechnicalSpecialistCalendarList = timesheetCalendarData;
        timesheetData = FilterSave(timesheetData);
        saveResponse = await dispatch(EditTimesheetDetails(timesheetData, isRejectSave));
    }
    return saveResponse;
};

/**Create Timesheet Details */
export const CreateTimesheetDetails = (payload, isRejectSave) => async (dispatch) => {
    dispatch(ShowLoader());
    const postUrl = StringFormat(timesheetAPIConfig.detail, 0);
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_POST_ERROR, 'dangerToast CreateTimesheetError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (response) {
        if (response.code === "1" && response.result) {
            if(!isRejectSave) dispatch(CreateAlert(response.result.timesheetId, "Timesheet"));
            dispatch(SaveAssignmentAdditionalExpenses());
            dispatch(ClearTimesheetCalendarData());
            dispatch(HandleMenuAction({ currentPage: localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE, currentModule: "timesheet" }));
            dispatch(HideLoader());
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
            dispatch(HideLoader());
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Timesheet"));
            dispatch(HideLoader());
        }
    }
    else {
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Timesheet"));
        dispatch(HideLoader());
    }
    return response;
};

/**Update Timesheet Details */
export const EditTimesheetDetails = (payload, isRejectSave) => async (dispatch, getstate) => {  
    dispatch(ShowLoader());
    const timesheetId = getstate().rootTimesheetReducer.selectedTimesheetId;
    const putUrl = StringFormat(timesheetAPIConfig.detail, timesheetId);
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_UPDATE_ERROR, 'dangerToast AssActUpdtAssDetUpdtErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if (response.code === "1") {
            dispatch(actions.TimesheetButtonDisable());      
            dispatch(ClearTimesheetCalendarData());
            dispatch(SaveAssignmentAdditionalExpenses());
            await dispatch(FetchTimesheetDetail(response.result.timesheetId,false));
            if(!isRejectSave) dispatch(SuccessAlert(response.result.timesheetId, "Timesheet"));
            const timesheetStatus = getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetStatus;
                if(timesheetStatus === "E"){
                    dispatch(UpdateInteractionMode(true));
                }    
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {
            if(!isRejectSave) IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
        }
        else {
            if(!isRejectSave) dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Timesheet"));
        }
    }
    dispatch(HideLoader());
    return response;
};

/**
 * Cancel Create timesheet Details
 */
export const CancelCreateTimesheetDetails = () => (dispatch, getstate) => {
    const isFetchLookUpData = false;
    dispatch(FetchReferencetypes());
    dispatch(FetchAssignmentForTimesheetCreation(undefined,isFetchLookUpData));
    dispatch(actions.SelectedTimesheetTechSpecs([])); //Changes for Live D650
};

/**
 * Cancel Edit timesheet Details
 */
export const CancelEditTimesheetDetails = () => async (dispatch, getstate) => {
    const timesheetId = getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetId;
    const isFetchLookUpData = false;
    dispatch(FetchReferencetypes());
    return await dispatch(FetchTimesheetDetail(timesheetId,isFetchLookUpData));
};

export const CancelEditTimesheetDetailsDocument = () => async (dispatch, getstate) => {
    const state = getstate();
    const TimesheetInfo = Object.assign({}, state.rootTimesheetReducer.timesheetDetail.TimesheetInfo);
    const timesheetId = TimesheetInfo.timesheetId;
    const documentData = state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments;
    const deleteUrl =  StringFormat(timesheetAPIConfig.timesheetDocumentDelete, timesheetId);
    let response = null;
    if (!isEmpty(documentData) && !isUndefined(timesheetId)) {
        response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    } 
    return response;  
};

/**
 * Delete existing Timesheet
 */
export const DeleteTimesheet = (timesheetId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!timesheetId)
        timesheetId = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetId;

    if (!timesheetId) {
        IntertekToaster(localConstant.errorMessages.TIMESHEET_DELETE_ERROR, 'dangerToast DeleteTimesheetError');
    }

    const timesheetDetail = state.rootTimesheetReducer.timesheetDetail;
    timesheetDetail.TimesheetInfo.recordStatus = "D";
    const deleteUrl = StringFormat(timesheetAPIConfig.detail, timesheetId);
    const params = {
        data: timesheetDetail,
    };
    const requestPayload = new RequestPayload(params);
    const response = await DeleteData(deleteUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TIMESHEET_DELETE_ERROR, 'dangerToast DeleteTimesheetError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response && response.code && response.code === "1") {
        dispatch(HideLoader());
        return response;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'warningToast DeleteTimesheetError');
        dispatch(HideLoader());
        return false;
    }
    else {
        IntertekToaster(localConstant.errorMessages.TIMESHEET_DELETE_ERROR, 'warningToast DeleteTimesheetError');
        dispatch(HideLoader());
        return false;
    }
};

/**Update Timesheet Details */
export const UpdateTimesheetStatus = (statusObj) => async (dispatch, getstate) => {
    const state = getstate();
    let timesheetData = state.rootTimesheetReducer.timesheetDetail;
    const timesheetInfo = timesheetData.TimesheetInfo;
    if(!statusObj.isValidationRequired) timesheetData.TimesheetInfo = Object.assign({}, timesheetInfo, statusObj.status); 
    if (!isEmpty(timesheetInfo.timesheetStartDate)) {
        timesheetInfo.timesheetStartDate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(timesheetInfo.timesheetEndDate)) {
        timesheetInfo.timesheetEndDate = moment(timesheetInfo.timesheetEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(timesheetInfo.timesheetExpectedCompleteDate)) {
        timesheetInfo.timesheetExpectedCompleteDate = moment(timesheetInfo.timesheetExpectedCompleteDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(statusObj.rejectionDate)) {
        statusObj.rejectionDate = moment(statusObj.rejectionDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    timesheetData.TimesheetInfo.recordStatus = "M";
    timesheetData.TimesheetInfo.modifiedBy = timesheetData.TimesheetInfo.actionByUser = state.appLayoutReducer.username;
    if(!isEmptyOrUndefine(timesheetData.TimesheetTechnicalSpecialistExpenses)) {
        timesheetData.TimesheetTechnicalSpecialistExpenses.forEach(row => {
            if(row.recordStatus !== "D" && ((isEmptyOrUndefine(row.chargeRateExchange) || row.chargeRateExchange === null) || (row.chargeRateExchange === 0 && row.recordStatus === null))) {
                row.chargeRateExchange = "0";
                row.recordStatus = row.recordStatus === "N" ? "N" : "M";
            }
            if(row.recordStatus !== "D" && ((isEmptyOrUndefine(row.payRateExchange) || row.payRateExchange === null) || (row.payRateExchange === 0 && row.recordStatus === null))) {
                row.payRateExchange = "0";
                row.recordStatus = row.recordStatus === "N" ? "N" : "M";
            }
        });
    }
    timesheetData = FilterSave(timesheetData);
    dispatch(ShowLoader());
    const timesheetId = timesheetData.TimesheetInfo.timesheetId;
    if(!timesheetId){
        dispatch(HideLoader());
        return;
    }
    let data = timesheetData;
    let putUrl = StringFormat(timesheetAPIConfig.detail, timesheetId);
    if (statusObj.isFromApproveOrReject === 'approve') {
        timesheetData.TimesheetDocuments = state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments;        
        putUrl = StringFormat(timesheetAPIConfig.approve, timesheetId);
        data = {
            TimesheetDetail:timesheetData,
            isValidationRequired:statusObj.isValidationRequired, //Bugid 563
            emailContent: (statusObj && statusObj.isSuccesAlert && statusObj.emailContent ? statusObj.emailContent : ''),
            isProcessNotification: statusObj.isSuccesAlert ? true : false,
            attachments: statusObj.attachments,
            toAddress: statusObj.toAddress ? statusObj.toAddress : '',
            emailSubject: statusObj.emailSubject ? statusObj.emailSubject : '',
            isAuditUpdate: statusObj.isAuditUpdate
        };
    }
    if (statusObj.isFromApproveOrReject === 'reject') {
        putUrl = StringFormat(timesheetAPIConfig.reject, timesheetId);
        if(statusObj.timesheetNotes) {
            if(state.rootTimesheetReducer.timesheetDetail.TimesheetNotes) {
                timesheetData.TimesheetNotes = [
                    statusObj.timesheetNotes,
                    ...state.rootTimesheetReducer.timesheetDetail.TimesheetNotes
                ];
            } else {
                timesheetData.TimesheetNotes = [ statusObj.timesheetNotes ];
            }
        }
        data = {
            TimesheetDetail:timesheetData, 
            reasonForRejection: statusObj.reasonForRejection,
            rejectionDate: statusObj.rejectionDate
        };
    }
    const requestPayload = new RequestPayload(data);
    let returnReponse = false;
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_UPDATE_ERROR, 'dangerToast AssActUpdtAssDetUpdtErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return error;
        });
    if (response) {
        if (response.code === "1") {
            if(statusObj.isSuccesAlert) dispatch(SuccessAlert(response.result.timesheetId, "Timesheet"));
            // await dispatch(FetchTimesheetGeneralDetail(timesheetId, true));
            await dispatch(FetchTimesheetDetail(timesheetId));
            returnReponse = true;
        }
        //Bug FIx 563 Start
        else if(response.code && response.code === "41"){
           const validationMessages = isEmptyReturnDefault(response.result);  
           let tsHasMultiplebookingValidation = false;
           if(validationMessages.length > 0){
            tsHasMultiplebookingValidation = ("techSpecName" in validationMessages[0] || "resourceAdditionalInfos" in validationMessages[0]);
           }
            if(statusObj.isFromApproveOrReject === 'approve' 
            && ("A" === statusObj.status.timesheetStatus || "O" === statusObj.status.timesheetStatus) && tsHasMultiplebookingValidation){
                returnReponse = validationMessages;  
            } else{
                IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
            }
        }//Bug FIx 563 End
        else if (response.code && response.code === "11") {
            // else if (response && response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Timesheet"));
        }
    }
    dispatch(HideLoader());
    return returnReponse;
};

export const CreateNewTimesheet = () => (dispatch, getstate) => {
    let timesheetDetail = {};
    const state = getstate(),
        timesheetInfo = isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo, 'object');

    if (!isEmptyOrUndefine(timesheetInfo)) {
        if(Array.isArray(timesheetInfo.assignmentTechSpecialists) 
        && timesheetInfo.assignmentTechSpecialists.length > 0){
            timesheetInfo.assignmentTechSpecialists.forEach(item =>{
                item.timesheetId = null;
            });
        }
        timesheetDetail = {
            TimesheetInfo: {
                assignmentCreatedDate: timesheetInfo.assignmentCreatedDate,
                customerProjectName: timesheetInfo.customerProjectName,
                timesheetCustomerName: timesheetInfo.timesheetCustomerName,
                timesheetId: null,
                timesheetNumber: null,
                timesheetStatus: 'N',
                timesheetAssignmentNumber: timesheetInfo.timesheetAssignmentNumber,
                timesheetAssignmentId: timesheetInfo.timesheetAssignmentId,
                timesheetContractCompanyCode: timesheetInfo.timesheetContractCompanyCode,
                timesheetContractCompany: timesheetInfo.timesheetContractCompany,
                timesheetContractCoordinator: timesheetInfo.timesheetContractCoordinator,
                timesheetContractCoordinatorCode: timesheetInfo.timesheetContractCoordinatorCode,
                isContractHoldingCompanyActive: timesheetInfo.isContractHoldingCompanyActive,
                timesheetCustomerCode: timesheetInfo.timesheetCustomerCode,
                timesheetContractNumber: timesheetInfo.timesheetContractNumber,
                timesheetOperatingCompanyCode: timesheetInfo.timesheetOperatingCompanyCode,
                timesheetOperatingCompany: timesheetInfo.timesheetOperatingCompany,
                timesheetOperatingCoordinator: timesheetInfo.timesheetOperatingCoordinator,
                timesheetOperatingCoordinatorCode: timesheetInfo.timesheetOperatingCoordinatorCode,
                timesheetProjectNumber: timesheetInfo.timesheetProjectNumber,
                assignmentTechSpecialists: timesheetInfo.assignmentTechSpecialists,
                assignmentClientReportingRequirements: timesheetInfo.assignmentClientReportingRequirements,
                assignmentProjectBusinessUnit: timesheetInfo.assignmentProjectBusinessUnit,
                projectInvoiceInstructionNotes: timesheetInfo.projectInvoiceInstructionNotes,
                assignmentProjectWorkFlow: timesheetInfo.assignmentProjectWorkFlow,
                assignmentStatus:timesheetInfo.assignmentStatus,
                isVisitOnPopUp:timesheetInfo.isVisitOnPopUp
            },
            TimesheetTechnicalSpecialists: null,
            TimesheetTechnicalSpecialistConsumables: null,
            TimesheetTechnicalSpecialistExpenses: null,
            TimesheetTechnicalSpecialistTimes: null,
            TimesheetTechnicalSpecialistTravels: null,
            TimesheetReferences: null,
            TimesheetDocuments: null,
            TimesheetNotes: null,
        };
        dispatch(HandleMenuAction({ currentPage: localConstant.timesheet.CREATE_TIMESHEET_MODE, currentModule: "timesheet" }));
        dispatch(actions.CreateNewTimesheet(timesheetDetail));
        dispatch(GetTimesheetInitialData(timesheetDetail.TimesheetInfo,false));
    }
    dispatch(UpdateCurrentPage(localConstant.timesheet.CREATE_TIMESHEET_MODE));
    if(timesheetDetail && timesheetDetail.TimesheetInfo && !(isOperatorCompany(timesheetDetail.TimesheetInfo.timesheetOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
        || isCoordinatorCompany(timesheetDetail.TimesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany))) {
            dispatch(UpdateInteractionMode(timesheetDetail.TimesheetInfo.timesheetContractCompanyCode));
    }
    if(!timesheetDetail.TimesheetInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));
    dispatch(UpdateCurrentModule('timesheet'));
};

export const SaveAssignmentAdditionalExpenses = () => async (dispatch, getstate) => {
    const state = getstate();
    const assignmentUnLinkedExpenses = isEmptyReturnDefault(state.rootTimesheetReducer.assignmentUnLinkedExpenses)
        .filter(item => {
            return item.recordStatus === 'M';
        });

    if (assignmentUnLinkedExpenses.length === 0) {
        return false;
    }
    const assignmentId = getNestedObject(state.rootTimesheetReducer.timesheetDetail, [
        "TimesheetInfo", "timesheetAssignmentId"
    ]);
    if (!assignmentId) {
        return false;
    }
    const url = StringFormat(assignmentAPIConfig.additionalExpenses, assignmentId);
    const response = await CreateData(url, { data: assignmentUnLinkedExpenses })
        .catch(error => {
            // console.error(error); // To show the error details in console
            const err = error;
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            return false;
        });
    if (!isEmptyOrUndefine(response)) {
        if (response.code === "1") {
            return response.result;
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {            
            return false;
        }
        else {            
            return false;
        }
    }
};

export const ProcessApprovalEmailNotification = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const customerCode = getNestedObject(state.rootTimesheetReducer.timesheetDetail, [
        'TimesheetInfo', 'timesheetCustomerCode'
    ]);
    const projectNo = getNestedObject(state.rootTimesheetReducer.timesheetDetail, [
        'TimesheetInfo', 'timesheetProjectNumber'
    ]);
    const companyCode = getNestedObject(state.rootTimesheetReducer.timesheetDetail, [
        "TimesheetInfo", 'timesheetContractCompanyCode'
    ]);
    if (!projectNo || !customerCode) {
        return false;
    }
    const projectClientNotifications = await dispatch(FetchProjectClientNotifications(projectNo, false));
    // const isSendCustomerReportingNotification = projectClientNotifications && projectClientNotifications.find(notify => {
    //     return notify.isSendCustomerReportingNotification = true;
    // });
    const notificationsArr = projectClientNotifications && projectClientNotifications.filter(notify => {
        return notify.isSendCustomerReportingNotification === true;
    });    
    if (Array.isArray(notificationsArr) && notificationsArr.length > 0) {
        //get the customer contacts to send emails
        const customerContacts = await dispatch(FetchCustomerContact(customerCode));
        const notifyCustomerContacts = customerContacts.filter(function (el) {
            return notificationsArr.some(function (f) {
                return f.customerContact === el.contactPersonName && !isEmptyOrUndefine(el.email);
            });
        });
          //Fetch Customer reporting notification Email Template
          const templateResponse =  await dispatch(FetchEmailTemplate({
            emailKey:'EmailCustomerReportingNotification',
            companyMessageType:6,
            companyCode:companyCode
        }));
        dispatch(actions.CustomerReportingNotificationContants(notifyCustomerContacts));
        dispatch(HideLoader());
        return true;
    } else {
        dispatch(HideLoader());
        //Doesn't show approval email notification popup 
        return false;
    }
};

export const UpdateShowAllRates = (data) => async (dispatch, getstate) => {
    dispatch(actions.UpdateShowAllRates(data));
};

export const SendCustomerReportingNotification = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const timesheetId = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetId;
    const url = StringFormat(timesheetAPIConfig.customerReportNotification, timesheetId);
    const requestData = {
        timesheetInfo: state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,
        ...data
    };
    const requestPayload = new RequestPayload(requestData);
    const response = await PostData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(parseValdiationMessage(response), 'warningToast SendCustomerReportingNotification');
            return error;
        });
    if (!isEmptyOrUndefine(response)) {
        if (response.code === "1") {
            return response.result;
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SendCustomerReportingNotification');
        }
        else {
            IntertekToaster(localConstant.commonConstants.SOMETHING_WENT_WRONG, 'warningToast SendCustomerReportingNotification');
        }
    }
    return response;
};

export const FetchTimesheetValidationData = (assignmentId) => async (dispatch, getstate) => {
    const url = timesheetAPIConfig.GetTimesheetValidationData;
    const param = {
        TimesheetAssignmentId: assignmentId
    };
    dispatch(ShowLoader());
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchTimesheetValidationData(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
    }
    dispatch(HideLoader());
};

export const ClearTimesheetCalendarData = () => (dispatch, getstate) => {
    dispatch(actions.ClearTimesheetCalendarData());
};

export const FetchRateSchedules = () => async (dispatch, getstate) => {
    const state = getstate();
    let contractNumber;
    if(!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetContractNumber)){
        contractNumber = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetContractNumber;
        }
        if (isEmptyOrUndefine(contractNumber)) {
            return false;
        }
        const params = {
            ContractNumber:contractNumber
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(contractAPIConfig.contracts + contractAPIConfig.rateSchedule, requestPayload)
            .catch(error => {     
                // console.error(error); // To show the error details in console       
                // IntertekToaster(localConstant.timesheet.ERROR_FETCHING_TIMESHEET_CONTRACT_SCHEDULES,'dangerToast fetchContractScheduleWentWrong');        
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
            if (response && response.code === "1" && !isEmptyOrUndefine(response.result) && !isEmptyOrUndefine(state.masterDataReducer.currencyMasterData)) {
                   const masterCurrency = state.masterDataReducer.currencyMasterData.filter(function (el) {
                        return response.result.some(function (f) {
                            return f.chargeCurrency === el.code;
                        });
                    });
                    dispatch(actions.FetchRateSchedules(masterCurrency));
            }
};

export const SaveValidTimesheetCalendarDataForSave = (data) => async (dispatch, getstate) => {
    dispatch(actions.SaveValidTimesheetCalendarDataForSave(data));
};

export const UpdateTimesheetExchangeRates = (data) => async (dispatch) => {
    await dispatch(actions.UpdateTimesheetExchangeRates(data));
};

export const FetchTimesheetStatus = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootTimesheetReducer.timesheetStatus)) {
        return;
    }
    const timesheetStatusUrl = visitAPIConfig.visitStatus;
    const response = await FetchData(timesheetStatusUrl, {})
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchTimesheetStatus(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_TIMESHEET_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};
export const FetchUnusedReason = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootVisitReducer.unusedReason)) {
        return;
    }
    const assignmentVisitUrl = visitAPIConfig.unusedReason;
    const response = await FetchData(assignmentVisitUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchUnusedReason(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_UNUSED_REASON, 'dangerToast assActassignVisitStatErrMsg1');
    }
};