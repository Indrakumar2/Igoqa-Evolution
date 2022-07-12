import { visitActionTypes } from '../../constants/actionTypes';
import { masterData, visitAPIConfig, RequestPayload,customerAPIConfig, assignmentAPIConfig, supplierAPIConfig, contractAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData, CreateData, DeleteData } from '../../services/api/baseApiService';
import {
    getlocalizeData,
    parseValdiationMessage,
    isEmptyReturnDefault,
    isEmptyOrUndefine,
    isEmpty,
    FilterSave,
    getNestedObject,
    isUndefined
} from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import {
    ShowLoader,
    HideLoader,
    UpdateCurrentPage,
    UpdateCurrentModule,
    FetchEmailTemplate,
    UpdateInteractionMode,
    RemoveDocumentsFromDB
} from '../../common/commonAction';
import { SuccessAlert, ValidationAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import arrayUtil from '../../utils/arrayUtil';
import moment from 'moment';
import { HandleMenuAction } from '../../components/sideMenu/sideMenuAction';
import { FetchAssignmentAdditionalExpensesForVisit } from '../../actions/visit/technicalSpcialistAction';
import { FetchInterCompanyDiscounts } from '../../actions/visit/interCompanyDiscountsAction';
import {
    FetchProjectClientNotifications,
    FetchCustomerContact
   } from '../project/clientNotificationAction';
   import  { FetchRateSchedule } from '../../actions/contracts/rateScheduleAction';
import { 
    isOperatorCompany,
    isCoordinatorCompany
} from '../../selectors/visitSelector';
import { applicationConstants } from '../../constants/appConstants';
import {
    RemoveVisitTechnicalSpecialist
} from '../../actions/visit/generalDetailsAction';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';
import { securitymodule } from '../../constants/securityConstant';
import { FetchAssignmentDocuments } from '../visit/documentsAction';
import { moduleViewAllRights_Modified } from '../../utils/permissionUtil';
import { processMICoordinators } from '../../utils/commonUtils';
import { required } from '../../utils/validator';

const localConstant = getlocalizeData();

const actions = {
    FetchAssignmentVisits: (payload) => ({
        type: visitActionTypes.FETCH_ASSIGNMENT_VISITS,
        data: payload
    }),
    FetchAssignmentSpecificVisits: (payload) => ({
        type: visitActionTypes.FETCH_ASSIGNMENT_SPECIFIC_VISITS,
        data: payload
    }),
    AssignmentVisitStatus:(payload)=>({
        type:visitActionTypes.SELECTED_VISIT_STATUS,
        data:payload
    }),
    GetSelectedVisit: (payload) => ({
        type: visitActionTypes.GET_SELECTED_VISIT,
        data: payload
    }),
    FetchAssignmentToAddVisit: (payload) => ({
        type: visitActionTypes.FETCH_ASSIGNMENT_TO_ADD_VISIT,
        data: payload
    }),
    FetchContractHoldingCoordinator: (payload) => ({
        type: visitActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR,
        data: payload
    }),
    FetchOperatingCoordinator: (payload) => ({
        type: visitActionTypes.FETCH_OPERATING_COORDINATOR,
        data: payload
    }),
    UpdateVisitStartDate: (payload) => ({
        type: visitActionTypes.UPDATE_START_DATE,
        data: payload
    }),
    UpdateVisitEndDate: (payload) => ({
        type: visitActionTypes.UPDATE_END_DATE,
        data: payload
    }),
    SaveVisitDeatils: (payload) => ({
        type: visitActionTypes.SAVE_VISIT_DETAILS,
        data: payload
    }),
    ClearVisitSearchResults: () => ({
        type: visitActionTypes.CLEAR_VISIT_SEARCH_RESULTS,
    }),
    FetchVisitDetailSuccess: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_DETAIL_SUCCESS,
        data: payload
    }),
    UpdateVisitStatus: (payload) =>({
        type: visitActionTypes.UPDATE_VISIT_STATUS,
        data: payload
    }),
    FetchReferencetypes: (payload) => ({
        type: visitActionTypes.FETCH_REFERENCE_TYPES,
        data: payload
    }),
    FetchVisitStatus: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_STATUS,
        data: payload
    }),
    FetchUnusedReason: (payload) => ({
        type: visitActionTypes.FETCH_UNUSED_REASON,
        data: payload
    }),
    FetchSupplierList: (payload) => ({
        type: visitActionTypes.FETCH_SUPPLIER_LIST,
        data: payload
    }),
    FetchSubsuppliers: (payload) => ({
        type: visitActionTypes.FETCH_SUB_SUPPLIER_LIST,
        data: payload
    }),
    FetchSubsuppliersForVisit: (payload) => ({
        type: visitActionTypes.FETCH_SUBSUPPLIERS_FOR_VISIT,
        data: payload
    }),
    FetchTechnicalSpecialistList: (payload) => ({
        type: visitActionTypes.FETCH_TECHNICAL_SPECIALIST_LIST,
        data: payload
    }),
    FetchVisitValidationData: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_VALIDATION_DATA,
        data: payload
    }),
    FetchTechSpecRateDefault: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
        data: payload
    }),
    CreateNewVisit:(payload) => ({
        type: visitActionTypes.CREATE_NEW_VISIT,
        data: payload
    }),
    AddVisitReference: (payload) => ({
        type: visitActionTypes.ADD_VISIT_REFERENCE,
        data: payload
    }),
    UpdateShowAllRates: (payload) => ({
        type: visitActionTypes.UPDATE_SHOW_ALL_RATES,
        data: payload
    }),
    CustomerReportingNotificationContants: (payload) =>({
        type: visitActionTypes.CUSTOMER_REPORTING_NOTIFICATION_CONTANT,
        data: payload
    }),
    FetchSupplierSearchList: (payload) => ({
        type: visitActionTypes.FETCH_SUPPLIER_LIST_FOR_SEARCH,
        data: payload
    }),
    ClearSupplierSearchList: () => ({
        type: visitActionTypes.CLEAR_SUPPLIER_LIST_FOR_SEARCH
    }),
    ClearCalendarData: () => ({
        type: visitActionTypes.CLEAR_CALENDAR_DATA
    }),
    FetchRateSchedules: (payload) => ({
        type: visitActionTypes.ERROR_FETCHING_VISIT_CONTRACT_SCHEDULES,
        data: payload
    }),
    AddUpdateGeneralDetails: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_DETAILS,
        data: payload
    }),
    VisitButtonDisable: () => ({
        type: visitActionTypes.VISIT_BUTTON_DISABLE,
        data: true
    }),
    ClearVisitDetails: () => ({
        type: visitActionTypes.CLEAR_VISIT_DETAILS,        
    }),
    SaveValidCalendarDataForSave: (payload) => ({
        type: visitActionTypes.VISIT_VALID_CALENDAR_DATA_SAVE,   
        data: payload     
    }),
    AddAttachedDocuments: (payload) => ({
        type: visitActionTypes.ADD_ATTACHED_DOCUMENTS,   
        data: payload     
    }),
    UpdateVisitExchangeRates: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_EXCHANGE_RATES,
        data: payload
    })
};

export const GetSelectedVisit = (payload) => (dispatch) => {
    dispatch(actions.GetSelectedVisit(payload));
};

export const UpdateVisitStartDate = (data) => async (dispatch, getstate) => {
    dispatch(actions.UpdateVisitStartDate(data));
};

export const UpdateVisitEndDate = (data) => async (dispatch, getstate) => {
    dispatch(actions.UpdateVisitEndDate(data));
};

export const FetchAssignmentVisits = () => async (dispatch, getstate) => {
    const assignmentId = getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;
    let assignmentVisits = [];
    if (assignmentId) {
        assignmentVisits = dispatch(FetchVisits({
            assignmentId: assignmentId
        }));
    }
    return assignmentVisits;
};

export const FetchContractHoldingCoordinator = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if(data){
    const selectedCompany = data;
    const coordinatorUrl = masterData.miCoordinatorsStatus;
    const param = [ selectedCompany ];
    const requestPayload = new RequestPayload(param);
    if (param) {
        const response = await PostData(coordinatorUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast HoldingCompanyCoordinatorValPreAssign');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                const processedMICoordinators = processMICoordinators(response.result);
                dispatch(actions.FetchContractHoldingCoordinator(arrayUtil.sort(processedMICoordinators, "displayName", "asc")));
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast HoldingCompanyCoordinatorSWWValPreAssign');
        }
    }
    else {
        dispatch(actions.FetchContractHoldingCoordinator([]));
    }
}
else{
    dispatch(actions.FetchContractHoldingCoordinator([]));
}
dispatch(HideLoader());
};

/** Fetch Coordinator for Operating Company action - pre assignment */
export const FetchOperatingCoordinator = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if(data){
    const selectedCompany = data;
    const coordinatorUrl = masterData.miCoordinatorsStatus;
    const param = [ selectedCompany ];
    const requestPayload = new RequestPayload(param);
    if (param) {
        const response = await PostData(coordinatorUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast OperatingCoordinatorValPreAssign');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                const processedMICoordinators = processMICoordinators(response.result);
                dispatch(actions.FetchOperatingCoordinator(arrayUtil.sort(processedMICoordinators, "displayName", "asc")));
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast OperatingCoordinatorSWWValPreAssign');
        }
    }
    else {
        dispatch(actions.FetchOperatingCoordinator([]));
    }
}
else{
    dispatch(actions.FetchOperatingCoordinator([]));
}
    dispatch(HideLoader());
};

export const FetchVisits = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const assignmentVisits = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetSearchVisits;
    const params = {};
    if(getstate().CommonReducer.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE) {
        params.modulename = "VST";  
    }
    if (data.customerName) {
        params.CustomerName = data.customerName;
        params.CustomerId = data.customerId;
    }
    if(data.contractNo) {
        params.ContractNumber = data.contractNo;
    }
    if (data.assignmentNo) {
        params.AssignmentNubmer = data.assignmentNo;
    }
    if (data.contractHoldingCompany) {
        // params.ContractHoldingCompanyCode = data.contractHoldingCompany;
        params.ContractCompanyId = data.contractHoldingCompany;
    }
    if (data.customerNo) {
        params.CustomerNumber = data.customerNo;
    }
    if (data.reportNo) {
        params.ReportNumber = data.reportNo;
    }
    if (data.chCoordinator) {
        // params.CHCoordinatorCode = data.chCoordinator;
        params.ContractCoordinatorId = data.chCoordinator;
    }
    if (data.customerContarctNo) {
        params.CustomerContractNumber = data.customerContarctNo;
    }
    if (data.eariliestvisitDate) {
        params.FromDate = data.eariliestvisitDate;
    }
    if (data.operatingCompany) {
        // params.OperatingCompanyCode = data.operatingCompany;
        params.OperatingCompanyId = data.operatingCompany;
    }
    if (data.ProjectNo) {
        params.ProjectNumber = data.ProjectNo;
    }
    if (data.lastvisitdate) {
        params.ToDate = data.lastvisitdate;
    }
    if (data.ocCoordinatorName) {
        // params.OCCoordinatorCode = data.ocCoordinatorName;
        params.OperatingCoordinatorId = data.ocCoordinatorName;
    }
    if (data.customerProjectName) {
        params.CustomerProjectName = data.customerProjectName;
    }
    if (data.supplierPoNo) {
        params.SupplierPONumber = data.supplierPoNo;
    }
    if(data.supplierSubSupplier) {
        params.SupplierSubSupplier = data.supplierSubSupplier;
        params.SupplierId=data.supplierId;
    }
    if(data.technicalSpecialist) {
        params.TechnicalSpecialist = data.technicalSpecialist;
    }
    if (data.searchDocumentType) {
        params.SearchDocumentType = data.searchDocumentType;
    }
    if (data.searchText) {
        params.DocumentSearchText = data.searchText;
    }
    if (data.notificationRef) {
        params.NotificationReference = data.notificationRef;
    }
    if(data.assignmentId){
        params.assignmentId = data.assignmentId;
    }
    params.isOnlyViewVisit = data.isOnlyViewVisit;  
    params.loggedInCompanyCode = data.loggedInCompanyCode;
    params.loggedInCompanyId = data.loggedInCompanyId;

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentVisits, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_VISITS_FETCH_FAILED, 'dangerToast assignmentvisit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const res = {
            responseResult:response.result,
            //selectedValue:status
        };
        dispatch(actions.FetchAssignmentVisits(res.responseResult));
        //dispatch(actions.AssignmentVisitStatus(res.selectedValue));
        dispatch(HideLoader());
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'warningToast FetchVisitAPI');
    }

    dispatch(HideLoader());
};

//Create for visit search page
export const FetchVisitsForSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());

    const assignmentVisits = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetSearchVisits;

    const params = {};
    if (data.customerName) {
        params.CustomerName = data.customerName;
        params.CustomerId = data.customerId;
    }
    if(data.contractNo) {
        params.ContractNumber = data.contractNo;
    }
    if (data.assignmentNo) {
        params.AssignmentNubmer = data.assignmentNo;
    }
    if (data.contractHoldingCompany) {
        // params.ContractHoldingCompanyCode = data.contractHoldingCompany;
        params.ContractCompanyId = data.contractHoldingCompany;
    }
    if (data.customerNo) {
        params.CustomerNumber = data.customerNo;
    }
    if (data.reportNo) {
        params.ReportNumber = data.reportNo;
    }
    if (data.chCoordinator) {
        // params.CHCoordinatorCode = data.chCoordinator;
        params.ContractCoordinatorId = data.chCoordinator;
    }
    if (data.customerContarctNo) {
        params.CustomerContractNumber = data.customerContarctNo;
    }
    if (data.eariliestvisitDate) {
        params.FromDate = data.eariliestvisitDate;
    }
    if (data.operatingCompany) {
        // params.OperatingCompanyCode = data.operatingCompany;
        params.OperatingCompanyId = data.operatingCompany;
    }
    if (data.ProjectNo) {
        params.ProjectNumber = data.ProjectNo;
    }
    if (data.lastvisitdate) {
        params.ToDate = data.lastvisitdate;
    }
    if (data.ocCoordinatorName) {
        // params.OCCoordinatorCode = data.ocCoordinatorName;
        params.OperatingCoordinatorId = data.ocCoordinatorName;
    }
    if (data.customerProjectName) {
        params.CustomerProjectName = data.customerProjectName;
    }
    if (data.supplierPoNo) {
        params.SupplierPONumber = data.supplierPoNo;
    }
    if(data.supplierSubSupplier) {
        params.SupplierSubSupplier = data.supplierSubSupplier;
        params.SupplierId=data.supplierId;
    }
    if(data.technicalSpecialist) {
        params.TechnicalSpecialist = data.technicalSpecialist;
    }
    if (data.searchDocumentType) {
        params.SearchDocumentType = data.searchDocumentType;
    }
    if (data.searchText) {
        params.DocumentSearchText = data.searchText;
    }
    if (data.notificationRef) {
        params.NotificationReference = data.notificationRef;
    }
    if(data.assignmentId){
        params.assignmentId = data.assignmentId;
    }
    if(data.visitCategory){
        params.categoryId = data.visitCategory;
    }
    if(data.visitSubCategory){
        params.subCategoryId = data.visitSubCategory;
    }
    if(data.visitService){
        params.serviceId = data.visitService;
    }
    if(data.materialDescription){
        params.materialDescription = data.materialDescription;
    }
    //search optimization
    if(data.OffSet){
        params.OffSet = data.OffSet;
    }
    //search optimization
    if (data.visitSearchTotalCount) {
        params.TotalCount = data.visitSearchTotalCount;
    }
    if (data.isExport) //Added for Export to CSV
        params.isExport = data.isExport;

    params.isOnlyViewVisit = data.isOnlyViewVisit;  
    params.loggedInCompanyCode = data.loggedInCompanyCode;
    params.loggedInCompanyId = data.loggedInCompanyId;
    
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentVisits, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_VISITS_FETCH_FAILED, 'dangerToast assignmentvisit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const res = {
            responseResult:response.result,
            //selectedValue:status
        };
        dispatch(actions.FetchAssignmentVisits(res.responseResult));
        //dispatch(actions.AssignmentVisitStatus(res.selectedValue));
        dispatch(HideLoader());
        return response;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'warningToast FetchVisitAPI');
    }

    dispatch(HideLoader());
};

 /**
 * method will fetch assignment details for visit creation
  * @param {*} assignmentId id to fetch assignment details for visit creation
  * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc 
  */
export const FetchAssignmentForVisitCreation = (assignmentId,isFetchLookValues) => async (dispatch, getstate) => {
    let visitDetails = {};
    const state = getstate(),
        assignemtList = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentList);
  
    if (!assignmentId){
        assignmentId = state.rootAssignmentReducer.selectedAssignmentId;       
    }
        const assignment = arrayUtil.FilterGetObject(assignemtList, 'assignmentId', assignmentId);
   
    if (!isEmptyOrUndefine(assignment)) {
        visitDetails = {
            VisitInfo: {
                visitAssignmentCreatedDate: assignment.assignmentCreatedDate,
                visitCustomerProjectName: assignment.assignmentCustomerProjectName,
                // //assignmentId: assignment.assignmentId,
                //visitId: null,
                visitAssignmentNumber: assignment.assignmentNumber,
                visitAssignmentId: assignment.assignmentId,
                visitSupplierPOId: assignment.assignmentSupplierPurchaseOrderId,
                supplierId: assignment.assignmentSupplierId,
                visitSupplierPONumber: assignment.assignmentSupplierPurchaseOrderNumber,
                visitContractCompanyCode: assignment.assignmentContractHoldingCompanyCode,
                visitContractCompany: assignment.assignmentContractHoldingCompany,
                visitContractCoordinator: assignment.assignmentContractHoldingCompanyCoordinator,
                isContractHoldingCompanyActive: assignment.isContractHoldingCompanyActive,
                visitContractNumber: assignment.assignmentContractNumber,
                visitCustomerCode: assignment.assignmentCustomerCode,
                visitCustomerName: assignment.assignmentCustomerName,
                visitOperatingCompanyCode: assignment.assignmentOperatingCompanyCode,
                visitOperatingCompany: assignment.assignmentOperatingCompany,
                visitOperatingCompanyCoordinator: assignment.assignmentOperatingCompanyCoordinator,
                visitOperatingCompanyCoordinatorCode: assignment.assignmentOperatingCompanyCoordinatorCode,
                visitContractCoordinatorCode: assignment.assignmentContractHoldingCompanyCoordinatorCode,
                visitProjectNumber: assignment.assignmentProjectNumber,
                assignmentProjectBusinessUnit:assignment.assignmentProjectBusinessUnit,
                workflowType:assignment.assignmentProjectWorkFlow,
                techSpecialists: [],
                projectInvoiceInstructionNotes:assignment.projectInvoiceInstructionNotes,
                assignmentClientReportingRequirements:assignment.clientReportingRequirements,
                isExtranetSummaryReportVisible:assignment.isExtranetSummaryReportVisible,
                isVisitOnPopUp:assignment.isVisitOnPopUp
            },
            // VisitTechnicalSpecialists: null,
            // VisitTechnicalSpecialistConsumables: null,
            // VisitTechnicalSpecialistExpenses: null,
            // VisitTechnicalSpecialistTimes: null,
            // VisitTechnicalSpecialistTravels: null,
             VisitReferences: null,
            // VisitDocuments: null,
            // VisitNotes: null,
            // VisitSupplierPerformances: null
        };        
        dispatch(actions.FetchAssignmentToAddVisit(visitDetails));
        dispatch(UpdateCurrentPage(localConstant.visit.CREATE_VISIT_MODE));        
        if(visitDetails && visitDetails.VisitInfo && !(isOperatorCompany(visitDetails.VisitInfo.visitOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
            || isCoordinatorCompany(visitDetails.VisitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany))) {
                dispatch(UpdateInteractionMode(visitDetails.VisitInfo.visitContractCompanyCode));
        }
        if(!visitDetails.VisitInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));        
        dispatch(UpdateCurrentModule('visit'));
        dispatch(FetchVisitStatus());
        dispatch(FetchUnusedReason());        
 
        //set isFetchLookValues to true when we fetch visit initially
        //on save, cancel we dont need to fetch data (like dropdowns, rateschedules,Techspecs) that already exists
        if (isFetchLookValues === true || isFetchLookValues === undefined) {
            await dispatch(FetchTechnicalSpecialistList(true));            
            await dispatch(FetchSubsuppliers(true));
            await dispatch(FetchTechSpecRateDefault());
            await dispatch(FetchAssignmentAdditionalExpensesForVisit(assignmentId));    
            await dispatch(FetchInterCompanyDiscounts(true));        
        }
    } else {
        if (!isEmptyOrUndefine(state.rootVisitReducer.visitDetails.VisitInfo)) {
            dispatch(CreateNewVisit());
        }
    }
    
};

export const ClearVisitSearchResults = () => (dispatch, getstate) => {
    dispatch(actions.ClearVisitSearchResults());
};

 /**
 * method will fetch vist details
  * @param {*} visitId id to fetch visit details
  * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc 
  */
export const FetchVisitDetail = (visitId,isFetchLookValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!visitId) {
        //Commenting this code because it is having issue -- need to check with prathap
        // visitId = state.rootVisitReducer.visitDetails.VisitInfo ?
        //           state.rootVisitReducer.visitDetails.VisitInfo.visitId :
        //           state.rootVisitReducer.selectedVisitData.visitId;

        visitId = state.rootVisitReducer.selectedVisitData.visitId;
    }
    if (!visitId) {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_VISIT_DETAIL, 'dangerToast FetchVisitDetail');
        dispatch(HideLoader());
        return;
    }

    const url = StringFormat(visitAPIConfig.Detail, visitId);
    const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_VISIT_DETAIL, 'dangerToast FetchVisitDetailError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return error;
        });

        //646 Fixes
        if (!isEmpty(response) && isEmpty(response.VisitInfo)){
            dispatch(HideLoader());
            dispatch(ChangeDataAvailableStatus(true));
            return response;
        }

    if (!isEmpty(response) && !isEmpty(response.VisitInfo)) {
        // ITK D - 1131 #2.1 initial sort for line items.
        if(response.VisitTechnicalSpecialistTimes){
            response.VisitTechnicalSpecialistTimes = arrayUtil.sort(response.VisitTechnicalSpecialistTimes,'expenseDate','desc');
            response.VisitTechnicalSpecialistTimes = arrayUtil.sort(response.VisitTechnicalSpecialistTimes,'chargeExpenseType','asc');
        }
        if(response.VisitTechnicalSpecialistExpenses){
            response.VisitTechnicalSpecialistExpenses = arrayUtil.sort(response.VisitTechnicalSpecialistExpenses,'expenseDate','desc');
            response.VisitTechnicalSpecialistExpenses = arrayUtil.sort(response.VisitTechnicalSpecialistExpenses,'chargeExpenseType','asc');
        }
        if(response.VisitTechnicalSpecialistTravels){
            response.VisitTechnicalSpecialistTravels = arrayUtil.sort(response.VisitTechnicalSpecialistTravels,'expenseDate','desc');
            response.VisitTechnicalSpecialistTravels = arrayUtil.sort(response.VisitTechnicalSpecialistTravels,'chargeExpenseType','asc');
        }
        if(response.VisitTechnicalSpecialistConsumables){
            response.VisitTechnicalSpecialistConsumables = arrayUtil.sort(response.VisitTechnicalSpecialistConsumables,'expenseDate','desc');
            response.VisitTechnicalSpecialistConsumables = arrayUtil.sort(response.VisitTechnicalSpecialistConsumables,'chargeExpenseType','asc');
        }
        dispatch(actions.FetchVisitDetailSuccess(response));
        dispatch(FetchAssignmentDocuments(false));
        dispatch(FetchVisitStatus());
        dispatch(FetchUnusedReason());        
        if (isFetchLookValues === true || isFetchLookValues === undefined) {
            await dispatch(FetchTechnicalSpecialistList(false));            
            await dispatch(FetchSubsuppliers(false));
            await dispatch(FetchTechSpecRateDefault());
            await dispatch(FetchAssignmentAdditionalExpensesForVisit());
        }
        // dispatch(ChangeDataAvailableStatus(true));
    }
    dispatch(HideLoader());
    dispatch(UpdateCurrentPage(localConstant.visit.EDIT_VIEW_VISIT_MODE));
    dispatch(UpdateCurrentModule('visit'));    
    if(response && response.VisitInfo && !(isOperatorCompany(response.VisitInfo.visitOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
        || isCoordinatorCompany(response.VisitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany))){
            dispatch(UpdateInteractionMode(response.VisitInfo.visitContractCompanyCode));
    }
    if(response && response.VisitInfo && !response.VisitInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));
    const { visitContractCompanyCode, visitOperatingCompanyCode, isContractHoldingCompanyActive } = response.VisitInfo;
        const isMandatoryView = !isContractHoldingCompanyActive;
        // const isViewAllVisit = isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies).length > 0;
        const isViewAllVisit = moduleViewAllRights_Modified(securitymodule.VISIT, state.masterDataReducer.viewAllRightsCompanies);
        dispatch(UpdateInteractionMode([ visitContractCompanyCode,visitOperatingCompanyCode ],isViewAllVisit,isMandatoryView));
        // dispatch(UpdateInteractionMode([ visitContractCompanyCode,visitOperatingCompanyCode ],securitymodule.VISIT,isViewAllVisit,isMandatoryView));
    return response;
};

export const SaveVisitDetails = () => async (dispatch, getstate) => {  
    const state = getstate();
    let visitData = state.rootVisitReducer.visitDetails;
    let visitCalendarData = state.rootVisitReducer.visitCalendarData;
    const currentPage = state.CommonReducer.currentPage;
    const VisitInterCompanyDiscounts=JSON.stringify(visitData.VisitInfo);
    if (!isEmpty(visitData.VisitInfo.visitStartDate)) {
        visitData.VisitInfo.visitStartDate = moment(visitData.VisitInfo.visitStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(visitData.VisitInfo.visitEndDate)) {
        visitData.VisitInfo.visitEndDate = moment(visitData.VisitInfo.visitEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(visitData.VisitInfo.visitReportSentToCustomerDate)) {
        visitData.VisitInfo.visitReportSentToCustomerDate = moment(visitData.VisitInfo.visitReportSentToCustomerDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(visitData.VisitInfo.visitExpectedCompleteDate)) {
        visitData.VisitInfo.visitExpectedCompleteDate = moment(visitData.VisitInfo.visitExpectedCompleteDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    
    //Commented the code for CR8
    // let updateVisitStatus = false;
    // if(visitData.VisitInfo.visitStatus === 'Q' || visitData.VisitInfo.visitStatus === 'T' ||
    //         visitData.VisitInfo.visitStatus === 'U') {
    //     if(visitData.VisitTechnicalSpecialistTimes && visitData.VisitTechnicalSpecialistTimes !== null && visitData.VisitTechnicalSpecialistTimes.length > 0) {            
    //         updateVisitStatus = (visitData.VisitTechnicalSpecialistTimes.filter(x => x.recordStatus !== "D" && (x.chargeTotalUnit > 0 
    //                 && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;                        
    //     }
    //     if(visitData.VisitTechnicalSpecialistExpenses && visitData.VisitTechnicalSpecialistExpenses !== null && visitData.VisitTechnicalSpecialistExpenses.length > 0 && updateVisitStatus === false) {            
    //         updateVisitStatus = (visitData.VisitTechnicalSpecialistExpenses.filter(x => x.recordStatus !== "D" && (x.chargeUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }
    //     if(visitData.VisitTechnicalSpecialistTravels && visitData.VisitTechnicalSpecialistTravels !== null && visitData.VisitTechnicalSpecialistTravels.length > 0 && updateVisitStatus === false) {            
    //         updateVisitStatus = (visitData.VisitTechnicalSpecialistTravels.filter(x => x.recordStatus !== "D" && (x.chargeTotalUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }
    //     if(visitData.VisitTechnicalSpecialistConsumables && visitData.VisitTechnicalSpecialistConsumables !== null && visitData.VisitTechnicalSpecialistConsumables.length > 0 && updateVisitStatus === false) {            
    //         updateVisitStatus = (visitData.VisitTechnicalSpecialistConsumables.filter(x => x.recordStatus !== "D" && (x.chargeTotalUnit > 0 
    //             && x.chargeRate > 0) || (x.payUnit > 0 && x.payRate > 0)).length) > 0 ? true : false;
    //     }

    //     if(updateVisitStatus === true) {
    //         visitData.VisitInfo.visitStatus = 'C';
    //     }
    // }
    const subSupplierList = state.rootVisitReducer.subSupplierList;    
    if (!isEmptyOrUndefine(subSupplierList)) {
        subSupplierList.forEach(supplierData => {                
            if((visitData && visitData.VisitInfo && visitData.VisitInfo.supplierId)
                && (supplierData.mainSupplierId === visitData.VisitInfo.supplierId || supplierData.subSupplierId === visitData.VisitInfo.supplierId)) {
                visitData.VisitInfo.visitSupplier = (supplierData.supplierType === "M" ? supplierData.mainSupplierName : supplierData.subSupplierName);
            }                                  
        });
    }
    if(!isEmptyOrUndefine(visitData.VisitTechnicalSpecialistTimes)) {
        visitData.VisitTechnicalSpecialistTimes.forEach(row => {
            row.chargeWorkUnit = (isNaN(row.chargeWorkUnit)
                ? (isEmptyOrUndefine(row.chargeWorkUnit) ? null : row.chargeWorkUnit)
                : (row.chargeWorkUnit ? row.chargeWorkUnit :null));
            row.chargeTravelUnit = (isNaN(row.chargeTravelUnit)
                ? (isEmptyOrUndefine(row.chargeTravelUnit) ? null : row.chargeTravelUnit)
                : (row.chargeTravelUnit ? row.chargeTravelUnit : null));
            row.chargeWaitUnit = (isNaN(row.chargeWaitUnit)
                ? (isEmptyOrUndefine(row.chargeWaitUnit) ? null : row.chargeWaitUnit)
                : (row.chargeWaitUnit ? row.chargeWaitUnit : null));
            row.chargeReportUnit = (isNaN(row.chargeReportUnit)
                ? (isEmptyOrUndefine(row.chargeReportUnit) ? null : row.chargeReportUnit)
                : (row.chargeReportUnit ? row.chargeReportUnit : null));
            //IGO Defect 966
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
    if(!isEmptyOrUndefine(visitData.VisitTechnicalSpecialistExpenses)) {
        visitData.VisitTechnicalSpecialistExpenses.forEach(row => {
            if(isEmptyOrUndefine(row.payRateTax)) row.payRateTax = "0";
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
    if(!isEmptyOrUndefine(visitData.VisitTechnicalSpecialistTravels)) {
        visitData.VisitTechnicalSpecialistTravels.forEach(row => {            
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
    //IGO Defect 966
    if(!isEmptyOrUndefine(visitData.VisitTechnicalSpecialistConsumables)) {
        visitData.VisitTechnicalSpecialistConsumables.forEach(row => {            
            if(row.payUnit == 0 && row.payRate > 0) {
                row.payRate = 0;
            }
        });
    }
      //Defect ID 702 - customer visible switch can Enable while document Upload 
     // As per discussion with Sumit and Bhavithira, Documnt Type 'Report - Inspection' Based customer Visible Switch On only for visitStaus 'A' 
    if(!isEmptyOrUndefine(visitData.VisitDocuments)) {
            visitData.VisitDocuments.forEach(row => {
        if(row.documentType === applicationConstants.customerVisibleAprovedStauts.ReportInspection && visitData.VisitInfo.visitStatus === 'A' ) {
                    row.isVisibleToCustomer = true;
                }
            });
        }

    let saveResponse = {};
    visitCalendarData = state.rootVisitReducer.validCalendarData;
    if (currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
        visitData.VisitInfo.recordStatus = "M";
        visitData.VisitInfo.modifiedBy = state.appLayoutReducer.username;
        visitData.VisitInfo.actionByUser = state.appLayoutReducer.username;
        visitData = FilterSave(visitData);
        if (visitCalendarData){
            visitData.technicalSpecialistCalendarList = visitCalendarData;
        }
        saveResponse = await dispatch(UpdateVisitDetails(visitData));
    } else {
        visitData.VisitInfo.recordStatus = "N";
        visitData.VisitInfo.modifiedBy = state.appLayoutReducer.username;
        visitData.VisitInfo.actionByUser = state.appLayoutReducer.username;
        if (visitCalendarData){
            visitData.TechnicalSpecialistCalendarList = visitCalendarData;
        }
        //Rames : To-do  for visit fetch await
        saveResponse = await dispatch(CreateVisitDetails(visitData));
        if (saveResponse.code === "1" && saveResponse.result && saveResponse.result.visitId) {
            saveResponse = await dispatch(FetchVisitDetail(saveResponse.result.visitId,false));
        }
    }
    return true;
};

export const CreateVisitDetails = (payload) => async (dispatch) => {
    dispatch(ShowLoader());
    const postContractUrl = StringFormat(visitAPIConfig.Detail, 0);
    //const postContractUrl = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.Detail;
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postContractUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.VISIT_POST_ERROR, 'dangerToast CreateVisitError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if (response.code === "1") {
            if (response.result) {                
                dispatch(CreateAlert(response.result.visitId, "Visit"));
                dispatch(HideLoader());
                dispatch(SaveAssignmentAdditionalExpenses());
                dispatch(ClearCalendarData());
                dispatch(HandleMenuAction({ currentPage: localConstant.visit.EDIT_VIEW_VISIT_MODE, currentModule: "visit" }));                
            } else {
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'warningToast AssActCreateAssPostErr');
                dispatch(HideLoader());
            }
        }
        else if (response.code === "11" || response.code === "41") {
            if (response.validationMessages.length > 0) {
                response.validationMessages.forEach(result => {
                    if (result.messages.length > 0) {
                        result.messages.forEach(valMessage => {
                            dispatch(ValidationAlert(valMessage.message, "Visit"));
                            dispatch(HideLoader());
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Visit"));
                        dispatch(HideLoader());
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.forEach(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "Visit"));
                        dispatch(HideLoader());
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Visit"));
                dispatch(HideLoader());
            }
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Visit"));
            dispatch(HideLoader());
        }
    }
    
    return response;
    //dispatch(HideLoader());
};

export const UpdateVisitDetails = (payload) => async (dispatch, getstate) => {    
    dispatch(ShowLoader());
    let visitId = getstate().rootVisitReducer.selectedVisitData.visitId;
    if (!visitId) {
        visitId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitId;
    }
    
    if (!visitId) {
        dispatch(HideLoader());
        return;
    }
    const postContractUrl = StringFormat(visitAPIConfig.Detail, visitId);
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(postContractUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.VISIT_UPDATE_ERROR, 'dangerToast AssActUpdtAssDetUpdtErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (response) {
            if (response.code === "1") {   
                dispatch(HideLoader());
                dispatch(actions.VisitButtonDisable());
                dispatch(SaveAssignmentAdditionalExpenses());
                dispatch(ClearCalendarData());
                const fetchResponse = await dispatch(FetchVisitDetail(response.result.visitId,false));                
                dispatch(SuccessAlert(response.result.visitId, "Visit")); 
                const visitStatus = getstate().rootVisitReducer.visitDetails.VisitInfo.visitStatus;
                if(visitStatus === "D"){
                    dispatch(UpdateInteractionMode(true));
                }                 
                if(fetchResponse)
                    return response;
                else
                    return response;
            }
            else if (response.code && (response.code === "11" || response.code === "41")) {
                IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
                return response;
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Visit"));
                return response;
            }
        }
    dispatch(HideLoader());
};

export const CancelCreateVisitDetails = () => (dispatch) => {
    const isFetchLookUpData = false;
    dispatch(FetchAssignmentForVisitCreation(undefined,isFetchLookUpData));
    dispatch(FetchAssignmentDocuments());
    dispatch(FetchReferencetypes());
};
export const ClearVisitDetails = () => (dispatch) => {   
    dispatch(actions.ClearVisitDetails());
};

/**
 * Cancel Edit Visit Details
 */
export const CancelEditVisitDetails = () => async (dispatch, getstate) => {
    const visitId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitId;
    const isFetchLookUpData = false;
    await dispatch(CancelEditVisitUploadDocument());
    dispatch(FetchAssignmentDocuments());
    dispatch(FetchReferencetypes());
    return await dispatch(FetchVisitDetail(visitId,isFetchLookUpData));
};

export const CancelEditVisitUploadDocument = () => async (dispatch, getstate) => {
    const state = getstate();
    const VisitInfo = Object.assign({},state.rootVisitReducer.visitDetails.VisitInfo);
    const visitId = VisitInfo.visitId;
    const documentData = state.rootVisitReducer.visitDetails.VisitDocuments;
    const deleteUrl = StringFormat(visitAPIConfig.visitDocumentsDelete, visitId);
    let response=null;
    if (!isEmpty(documentData)&&!isUndefined(visitId)) {
        response= await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response;
};

/**
 * Delete existing Visit
 */
export const DeleteVisit = (visitId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!visitId)
        visitId = state.rootVisitReducer.visitDetails.VisitInfo.visitId;

    if (!visitId) {
        IntertekToaster(localConstant.errorMessages.VISIT_DELETE_ERROR, 'dangerToast DeleteVisitError');
        return;
    }

    const visitDetail = state.rootVisitReducer.visitDetails;
    visitDetail.VisitInfo.recordStatus = "D";
    //const deleteUrl = StringFormat(visitAPIConfig.visitDetail, visitId);
    const deleteUrl = StringFormat(visitAPIConfig.Detail, visitId);
    const params = {
        data: visitDetail,
    };
    const requestPayload = new RequestPayload(params);
    const response = await DeleteData(deleteUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.VISIT_DELETE_ERROR, 'dangerToast DeleteVisitError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response && response.code && response.code === "1") {
        dispatch(HideLoader());
        return response;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'warningToast DeleteVisitError');
    }
    else {
        IntertekToaster(localConstant.errorMessages.VISIT_DELETE_ERROR, 'warningToast DeleteVisitError');
    }
    dispatch(HideLoader());
};

/**Update Visit Details */
export const UpdateVisitStatus = (statusObj) => async (dispatch, getstate) => {

    dispatch(ShowLoader());
    const state = getstate();
    let visitData = state.rootVisitReducer.visitDetails;
    if(!statusObj.isValidationRequired) visitData.VisitInfo = Object.assign({}, visitData.VisitInfo, statusObj.status);
    if (!isEmpty(visitData.VisitInfo.visitStartDate)) {
        visitData.VisitInfo.visitStartDate = moment(visitData.VisitInfo.visitStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(visitData.VisitInfo.visitEndDate)) {
        visitData.VisitInfo.visitEndDate = moment(visitData.VisitInfo.visitEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(visitData.VisitInfo.visitReportSentToCustomerDate)) {
        visitData.VisitInfo.visitReportSentToCustomerDate = moment(visitData.VisitInfo.visitReportSentToCustomerDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (statusObj.dateSentToCustomer){ //&& isEmpty(visitData.VisitInfo.visitReportSentToCustomerDate)) {
        visitData.VisitInfo.visitReportSentToCustomerDate = statusObj.dateSentToCustomer;
    }
    if (!isEmpty(visitData.VisitInfo.visitExpectedCompleteDate)) {
        visitData.VisitInfo.visitExpectedCompleteDate = moment(visitData.VisitInfo.visitExpectedCompleteDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(statusObj.rejectionDate)) {
        statusObj.rejectionDate = moment(statusObj.rejectionDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    //For Bug Id 563 -Start
    if(Array.isArray(visitData.VisitTechnicalSpecialists) && visitData.VisitTechnicalSpecialists.length >0){
        visitData.VisitInfo.techSpecialists = visitData.VisitTechnicalSpecialists.map(eachTs =>{
        return {
            pin:eachTs.pin,
            technicalSpecialistName: eachTs.technicalSpecialistName,
            visitId:eachTs.visitId
        };
    });
    }
    //For Bug Id 563 -END
    visitData.VisitInfo.recordStatus = "M";
    visitData.VisitInfo.modifiedBy = visitData.VisitInfo.actionByUser = state.appLayoutReducer.username;
    if(!isEmptyOrUndefine(visitData.VisitTechnicalSpecialistExpenses)) {
        visitData.VisitTechnicalSpecialistExpenses.forEach(row => {
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
    visitData = FilterSave(visitData);
    const visitId = visitData.VisitInfo.visitId;
    if(!visitId){
        dispatch(HideLoader());
        return;
    }    
    let data = visitData,returnResponse = false;
    let putUrl =  StringFormat(visitAPIConfig.Detail, visitId);
    if(statusObj.isFromApproveOrReject === 'approve'){
        visitData.VisitDocuments = state.rootVisitReducer.visitDetails.VisitDocuments;      
        putUrl = StringFormat(visitAPIConfig.approve, visitId);     
        data = {
            visitDetail: visitData,
            isValidationRequired: statusObj.isValidationRequired, //For Bug Id 563,
            emailContent: (statusObj && statusObj.isSuccesAlert && statusObj.emailContent ? statusObj.emailContent : ''),
            isProcessNotification: statusObj.isSuccesAlert ? true : false,
            attachments: statusObj.attachments,
            toAddress: statusObj.toAddress ? statusObj.toAddress : '',
            emailSubject: statusObj.emailSubject ? statusObj.emailSubject : ''
        };
    }
    if(statusObj.isFromApproveOrReject === 'reject'){
        visitData.VisitTechnicalSpecialists = state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists;
        putUrl = StringFormat(visitAPIConfig.reject, visitId);
        if(statusObj.visitNotes) {
            if(state.rootVisitReducer.visitDetails.VisitNotes) {
                visitData.VisitNotes = [
                    statusObj.visitNotes,
                    ...state.rootVisitReducer.visitDetails.VisitNotes
                ];
            } else {
                visitData.VisitNotes = [ statusObj.visitNotes ];
            }
        }
        
        data = {
            visitDetail:visitData,
            reasonForRejection: statusObj.reasonForRejection,
            rejectionDate: statusObj.rejectionDate
        };
    }
    const requestPayload = new RequestPayload(data);
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast AssActUpdtAssDetUpdtErr');
            dispatch(HideLoader());
            return error;
        });
        if (response) {
            if (response.code === "1") {
                if(statusObj.isSuccesAlert) dispatch(SuccessAlert(response.result.visitId, "Visit"));           
                dispatch(HideLoader());                
                await dispatch(FetchVisitDetail(response.result.visitId));                
                //dispatch(actions.UpdateVisitStatus(visitStatus));
                returnResponse =  true;
            }
            else if(response.code && response.code === "41"){
                const validationMessages = isEmptyReturnDefault(response.result); 
                let tsHasMultipleBookingValidation =false;
                if(validationMessages.length > 0){
                    tsHasMultipleBookingValidation = ("techSpecName" in validationMessages[0] || "resourceAdditionalInfos" in validationMessages[0]);
                }
                 if(statusObj.isFromApproveOrReject === 'approve' 
                 && ("A" === statusObj.status.visitStatus || "O" === statusObj.status.visitStatus) && tsHasMultipleBookingValidation){
                    returnResponse = validationMessages;  
                 } else{
                     IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
                 }
             }
            else if (response.code && response.code === "11") {
                // else if (response && response.code && (response.code === "11" || response.code === "41")) {
                IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Visit"));
            }
        }
    dispatch(HideLoader());
    return returnResponse;
};

/** Fetch Dropdown data lists */
export const FetchReferencetypes = (isNewVisit) => async (dispatch, getstate) => {    
    const params = {};
    let projectNumber = 0;
    const state = getstate();
    if(isNewVisit) {
        if(!isEmptyOrUndefine(state.rootVisitReducer.visitDetails) &&
                !isEmptyOrUndefine(state.rootVisitReducer.visitDetails.VisitInfo)) {
                    projectNumber = state.rootVisitReducer.visitDetails.VisitInfo.visitProjectNumber;
        }
    } else {
        projectNumber = state.rootVisitReducer.selectedVisitData.visitProjectNumber;
    }

    if(!projectNumber && state.rootVisitReducer.visitDetails && state.rootVisitReducer.visitDetails.VisitInfo &&
        state.rootVisitReducer.visitDetails.VisitInfo.visitProjectNumber) {
            projectNumber = state.rootVisitReducer.visitDetails.VisitInfo.visitProjectNumber;
    }

    if(projectNumber != undefined && projectNumber !== 0) {
        const requestPayload = new RequestPayload(params);
        const url = customerAPIConfig.projects + projectNumber + visitAPIConfig.visitReferenceTypes;
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.assignments.ERROR_FETCHING_REFERENCE_TYPES, 'dangerToast fetchAssignmentWrong');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchReferencetypes(response.result));
            if(isNewVisit === true) {
                if (!isEmptyOrUndefine(response.result)) {
                let referenceItems = response.result.filter(x => x.isVisibleToVisit === true);
                const refTypesAlreadyExisits = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitReferences);
                referenceItems = referenceItems.filter(function (el) {
                        return !refTypesAlreadyExisits.some(function (f) {
                            return f.referenceType === el.referenceType;
                        });
                    });
                    referenceItems.forEach(referenceData => {
                        if (referenceData.isVisibleToVisit) {
                            const updatedData = {};
                            updatedData["referenceType"] = referenceData.referenceType;
                            updatedData["referenceValue"] = "TBA";
                            updatedData["visitReferenceTypeAddUniqueId"] = Math.floor(Math.random() * 99) - 100;
                            updatedData["visitReferenceId"] = null;
                            updatedData["recordStatus"] = 'N';
                            //this.props.actions.AddVisitReference(this.updatedData);
                            dispatch(actions.AddVisitReference(updatedData));
                        }
                    });
                }
            }
        }
    }
};

export const FetchVisitStatus = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootVisitReducer.visitStatus)) {
        return;
    }
    const assignmentVisitUrl = visitAPIConfig.visitStatus;
    const response = await FetchData(assignmentVisitUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchVisitStatus(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};

export const FetchUnusedReason = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootVisitReducer.unusedReason)) {
        return;
    }
    const assignmentVisitUrl = visitAPIConfig.unusedReason;
    const response = await FetchData(assignmentVisitUrl, {})
        .catch(error => {
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

export const FetchTechnicalSpecialistList = (isNewVisit) => async (dispatch, getstate) => {
    // if (!isEmpty(getstate().rootVisitReducer.technicalSpecialistList)) {
    //     return;
    // }    
    
    let assignmentId = 0;
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
                !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }

    if(!assignmentId) assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;

    const techSpecialistUrl = visitAPIConfig.visitBaseUrl + visitAPIConfig.assignment + assignmentId + visitAPIConfig.technicalSpecialistList;
    const param = {
        AssignmentId: assignmentId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(techSpecialistUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchTechnicalSpecialistList(response.result));
           return response;
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};

export const FetchSupplierList = (isNewVisit) => async (dispatch, getstate) => {
    let selectedsupplierPOId = 0;    
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
            !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            selectedsupplierPOId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitSupplierPOId;
        }
    } else {
        selectedsupplierPOId = getstate().rootVisitReducer.selectedVisitData.visitSupplierPOId;
    }

    if(!selectedsupplierPOId) selectedsupplierPOId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitSupplierPOId;

    const url = assignmentAPIConfig.mainSupplier;
    const param = {
        SupplierPOId: selectedsupplierPOId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.result)) {
        if (response.code === "1") {
            // dispatch(actions.FetchMainSupplierName(response.result[0].supplierPOMainSupplierName));
            // dispatch(actions.FetchMainSupplierId(response.result[0].supplierPOMainSupplierId));
            dispatch(actions.FetchSupplierList(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {
            //To-Do: show appropriate messages
        }
    }
};

export const FetchSubsuppliers =(isNewVisit) => async (dispatch, getstate) => {
    let assignmentId = 0;
    let supplierPOId = 0;
    supplierPOId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitSupplierPOId;
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
                !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }
    
    if(!assignmentId) assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
    
    const url  = StringFormat(visitAPIConfig.subSupplierVisit, assignmentId);
    dispatch(actions.FetchSubsuppliers([]));
    dispatch(FetchSubsuppliersForVisit(supplierPOId));
    //const url  = StringFormat(assignmentAPIConfig.subSuppliers, selectedsupplierPOId);
    const response = await FetchData(url, {})
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.result)) {        
        if (response.code === "1") {
            dispatch(actions.FetchSubsuppliers(response.result));            
            if(!isEmptyOrUndefine(response.result) && response.result.length > 0) {                
                
                if(isNewVisit) {
                    response.result.forEach(item => {
                        if(item.isMainSupplierFirstVisit || item.isSubSupplierFirstVisit) {
                            const visitInfo = getstate().rootVisitReducer.visitDetails.VisitInfo;
                            visitInfo.supplierId =  (item.supplierType === "M" ? item.mainSupplierId : item.subSupplierId);
                            dispatch(actions.AddUpdateGeneralDetails(visitInfo));  
                        }
                    });
                } else {
                    response.result.forEach(item => {
                        const visitInfo = getstate().rootVisitReducer.visitDetails.VisitInfo;
                        if(item.isDeleted && ((item.supplierType === "M" && visitInfo.supplierId === item.mainSupplierId)
                            || (item.supplierType === "S" && visitInfo.supplierId === item.subSupplierId))
                            && (item.assignmentSubSupplierTS && item.assignmentSubSupplierTS.filter(x => x.isDeleted === false).length > 0) === false) 
                        {
                            //visitInfo.supplierId = null; Duplicate items making supplier id has null
                            dispatch(actions.AddUpdateGeneralDetails(visitInfo)); 
                        }
                        // HOT fix 777 - Commented the code because deleted Supplier TS is not showing in visit
                        // item.assignmentSubSupplierTS && item.assignmentSubSupplierTS.forEach(subSupplierTS => {
                        //     if(subSupplierTS.isDeleted) {
                        //         const visitTechnicalSpecialists = isEmptyReturnDefault(getstate().rootVisitReducer.visitDetails.VisitTechnicalSpecialists);
                        //         const deleteTechSpec = visitTechnicalSpecialists && visitTechnicalSpecialists.filter(x => x.pin === subSupplierTS.epin);
                        //         if(deleteTechSpec && deleteTechSpec.length > 0) dispatch(RemoveVisitTechnicalSpecialist(deleteTechSpec[0]));
                        //     }
                        // });
                    });
                }
            }
        }
    }
};

export const FetchSubsuppliersForVisit =(selectedsupplierPOId) => async (dispatch,getstate) =>{
    const state = getstate();
    if(!required(selectedsupplierPOId)){
        const url  = StringFormat(assignmentAPIConfig.subSuppliers, selectedsupplierPOId);
        const response = await FetchData(url, {})
        .catch(error => {
            console.error(error);
            IntertekToaster(localConstant.assignments.ERROR_FETCHING_SUB_SUPPLIER, 'dangerToast subSupplierErr');
        });
        if (!isEmpty(response)) {
            if(response.result && response.result.length === 0){
                dispatch(actions.FetchSubsuppliersForVisit([]));
                return true;
            }
            else{
                dispatch(actions.FetchSubsuppliersForVisit(response.result));
                return false;
            }
        }
    }
    else{
        dispatch(actions.FetchSubsuppliersForVisit([]));
        return false;
    }
};

export const FetchVisitValidationData = (isNewVisit) => async (dispatch, getstate) => {    
    let assignmentId = 0;
    let visitId = 0;
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
                !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
            visitId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
        visitId = getstate().rootVisitReducer.selectedVisitData.visitId;
    }

    if(!assignmentId) assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
    if(!visitId) visitId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitId;
    dispatch(ShowLoader());
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetVisitValidationData;
    const param = {
        VisitAssignmentId: assignmentId,
        VisitId: visitId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            return false;
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchVisitValidationData(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
        return false;
    }
    dispatch(HideLoader());
    return true;
};

export const FetchTechSpecRateDefault = () => async (dispatch, getstate) => {
    const state = getstate();
    let assignmentId ;
    if(state.rootVisitReducer.visitDetails.VisitInfo &&
        state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId){
            assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    if(!assignmentId){
        assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId;
    }
    dispatch(ShowLoader());
    const url = StringFormat(visitAPIConfig.TechSpecRateSchedule, assignmentId);

    const params = { };

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //dispatch(HideLoader());
            //IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_TECHSPEC_RATE_SCHEDULES, 'dangerToast fetchTimesheetWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (response && "1" === response.code) {
        dispatch(actions.FetchTechSpecRateDefault(response.result));
    }
    dispatch(HideLoader());
};

export const CreateNewVisit = () => (dispatch, getstate) => {
    let visitDetails = {};
    const state = getstate(),
    visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo,'object');

    if (!isEmptyOrUndefine(visitInfo)) {
        visitDetails = {
            VisitInfo: {
                visitAssignmentCreatedDate: visitInfo.visitAssignmentCreatedDate,
                visitCustomerProjectName: visitInfo.visitCustomerProjectName,
                visitAssignmentNumber: visitInfo.visitAssignmentNumber,
                visitAssignmentId: visitInfo.visitAssignmentId,
                visitSupplierPOId: visitInfo.visitSupplierPOId,
                supplierId: visitInfo.supplierId,
                visitSupplierPONumber: visitInfo.visitSupplierPONumber,
                visitContractCompanyCode: visitInfo.visitContractCompanyCode,
                visitContractCompany: visitInfo.visitContractCompany,
                visitContractCoordinator: visitInfo.visitContractCoordinator,
                isContractHoldingCompanyActive: visitInfo.isContractHoldingCompanyActive,
                visitContractNumber: visitInfo.visitContractNumber,
                visitCustomerCode: visitInfo.visitCustomerCode,
                visitCustomerName: visitInfo.visitCustomerName,
                visitOperatingCompanyCode: visitInfo.visitOperatingCompanyCode,
                visitOperatingCompany: visitInfo.visitOperatingCompany,
                visitOperatingCompanyCoordinator: visitInfo.visitOperatingCompanyCoordinator,
                visitProjectNumber: visitInfo.visitProjectNumber,
                assignmentProjectBusinessUnit:visitInfo.assignmentProjectBusinessUnit,
                workflowType:visitInfo.workflowType,
                techSpecialists: [],
                projectInvoiceInstructionNotes:visitInfo.projectInvoiceInstructionNotes,
                assignmentClientReportingRequirements:visitInfo.assignmentClientReportingRequirements,
                isExtranetSummaryReportVisible:visitInfo.isExtranetSummaryReportVisible,
                isVisitOnPopUp:visitInfo.isVisitOnPopUp
            },
            VisitReferences: null,
        };
    }         
    const subSupplierList = state.rootVisitReducer.subSupplierList;
    if(!isEmptyOrUndefine(subSupplierList) && subSupplierList.length > 0) {                
        let supplierId = -1;
        subSupplierList.forEach(item => {
            if(item.isSubSupplierFirstVisit) supplierId = item.subSupplierSupplierId;
        });                
        if(supplierId !== -1) {
            visitDetails.visitInfo.supplierId = supplierId;            
        }        
    }        
    dispatch(actions.FetchAssignmentToAddVisit(visitDetails));
    dispatch(FetchAssignmentAdditionalExpensesForVisit(visitInfo.visitAssignmentId));
    dispatch(UpdateCurrentPage(localConstant.visit.CREATE_VISIT_MODE));
    if(visitDetails && visitDetails.VisitInfo && !(isOperatorCompany(visitDetails.VisitInfo.visitOperatingCompanyCode,state.appLayoutReducer.selectedCompany)
        || isCoordinatorCompany(visitDetails.VisitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany))) {
        dispatch(UpdateInteractionMode(visitDetails.VisitInfo.visitContractCompanyCode));
    }
    if(!visitDetails.VisitInfo.isContractHoldingCompanyActive) dispatch(UpdateInteractionMode(true));
    dispatch(UpdateCurrentModule('visit'));
};

export const SaveAssignmentAdditionalExpenses = () => async (dispatch, getstate) => {
    const state = getstate();
    const assignmentUnLinkedExpenses = isEmptyReturnDefault(state.rootVisitReducer.assignmentUnLinkedExpenses)
        .filter(item => {
            return item.recordStatus === 'M';
        });

    if (assignmentUnLinkedExpenses.length === 0) {
        return false;
    }
    const assignmentId = getNestedObject(state.rootVisitReducer.visitDetails, [ "VisitInfo", "visitAssignmentId" ]);
    if (!assignmentId) {
        return false;
    }
    const url = StringFormat(assignmentAPIConfig.additionalExpenses, assignmentId);
    const response = await CreateData(url, { data:assignmentUnLinkedExpenses })
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            const err =error;
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

export const UpdateShowAllRates = (data) => async (dispatch, getstate) => {
    dispatch(actions.UpdateShowAllRates(data));
};

export const ProcessApprovalEmailNotification = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const customerCode = getNestedObject(state.rootVisitReducer.visitDetails, [ "VisitInfo",  'visitCustomerCode' ]);
    const companyCode = getNestedObject(state.rootVisitReducer.visitDetails, [ "VisitInfo",  'visitContractCompanyCode' ]);
    const projectNo = getNestedObject(state.rootVisitReducer.visitDetails, [ "VisitInfo",  'visitProjectNumber' ]);
    if (!projectNo || !customerCode) {
        return false;
    }
    const projectClientNotifications = await dispatch(FetchProjectClientNotifications(projectNo));
    const notificationsArr = projectClientNotifications && projectClientNotifications.filter(notify => {
        return notify.isSendCustomerReportingNotification === true;
    });
    if (Array.isArray(notificationsArr) && notificationsArr.length > 0) {
        //get the customer contacts to send emails
        const customerContacts = await dispatch(FetchCustomerContact(customerCode));
        const notifyCustomerContacts = customerContacts.filter(function (el) {
            return notificationsArr.some(function (f) {
                return f.customerContact === el.contactPersonName  && !isEmptyOrUndefine(el.email);
            });
        });
        // // Fetch Customer reporting notification Email Template
        const templateResponse = await dispatch(FetchEmailTemplate({
            emailKey:'EmailCustomerReportingNotification',
            CompanyMessageType:6,
            companyCode:companyCode
        }));
        dispatch(actions.CustomerReportingNotificationContants(notifyCustomerContacts));
        dispatch(HideLoader());
        return true;
    } else {
        //Doesn't show approval email notification popup
        dispatch(HideLoader());
        return false;
    }
};

export const SendCustomerReportingNotification = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const visitId = state.rootVisitReducer.visitDetails.VisitInfo.visitId;
    const url = StringFormat(visitAPIConfig.customerReportNotification, visitId);
    const requestData = { visitInfo:state.rootVisitReducer.visitDetails.VisitInfo,
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

export const FetchSupplierSearchList = (data) => async (dispatch) => {
    dispatch(ShowLoader());
    const supplierSearchUrl = supplierAPIConfig.supplierBaseUrl + supplierAPIConfig.supplierSearch;
    const params = data;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'warningToast supplierSearchFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.FetchSupplierSearchList(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast supplierSearchFetchVal');
        }
        else {
            IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'dangerToast fetchSupplierSearchFailure');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierSearchFailure');
    }
    dispatch(HideLoader());
};

export const ClearSupplierSearchList = (data) => (dispatch, getstate) => {
    dispatch(actions.ClearSupplierSearchList());
};

export const ClearCalendarData = () => (dispatch, getstate) => {
    dispatch(actions.ClearCalendarData());
};

export const FetchRateSchedules = () => async (dispatch, getstate) => {
    const state = getstate();
    let contractNumber;
    if(!isEmptyOrUndefine(state.rootVisitReducer.visitDetails.VisitInfo.visitContractNumber)){
        contractNumber = state.rootVisitReducer.visitDetails.VisitInfo.visitContractNumber;
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
                // IntertekToaster(localConstant.visit.ERROR_FETCHING_VISIT_CONTRACT_SCHEDULES,'dangerToast fetchContractScheduleWentWrong');        
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

export const SaveValidCalendarDataForSave = (data) => async (dispatch, getstate) => {
    dispatch(actions.SaveValidCalendarDataForSave(data));
};

export const AddAttachedDocuments = (data) => async (dispatch, getstate) => {
    await dispatch(actions.AddAttachedDocuments(data));
};

export const UpdateVisitExchangeRates = (data) => async (dispatch) => {
    await dispatch(actions.UpdateVisitExchangeRates(data));
};