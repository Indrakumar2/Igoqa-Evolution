import { assignmentAPIConfig, RequestPayload ,customerAPIConfig } from '../../apiConfig/apiConfig';
import { assignmentsActionTypes } from '../../constants/actionTypes';
import { FetchData, PostData, CreateData, DeleteData } from '../../services/api/baseApiService';
import { ShowLoader, HideLoader, UpdateInteractionMode,UpdateCurrentPage,UpdateCurrentModule,IsARSMasterDataLoaded } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, 
        getlocalizeData, 
        isEmptyReturnDefault,
        FilterSave , 
        formatToDecimal, 
        parseValdiationMessage,
        getNestedObject,
        isEmptyOrUndefine, 
        deepCopy, 
        fetchCoordinatorDetails } from '../../utils/commonUtils';
import { FetchGeneralDetails } from '../assignment/generalDetailsActions';
import { StringFormat } from '../../utils/stringUtil';
import moment from 'moment';
import { SuccessAlert, ValidationAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import { HandleMenuAction } from '../../components/sideMenu/sideMenuAction';
import { FetchSupplierContacts,FetchSubsuppliers,FetchSubsupplierContacts,
         AddSubSupplierInformation, FetchAssignmentTechSpec } from './supplierInformationAction';
import { clearARSSearchDetails,SetDefaultPreAssignmentName } from './arsSearchAction';
import { clearPreAssignmentDetails } from '../../grm/actions/techSpec/preAssignmentAction';
import { ClearMyTasksData } from '../../grm/actions/techSpec/techSpecMytaskActions';
import arrayUtil from '../../utils/arrayUtil';
import { AddProjectAssignmentReference, FetchReferencetypes } from './assignmentReferenceActions';
import { required } from '../../utils/validator';
import { FetchCity, FetchState, ClearSubCategory, ClearServices } from '../../common/masterData/masterDataActions';
import { isTimesheet, isVisit } from '../../selectors/assignmentSelector';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
import { isSettlingTypeMargin } from '../../selectors/assignmentSelector';
import { handleAssignedSpecialistTaxonomyLOV, ClearAssignedSpecialist , FetchPaySchedule } from './assignedSpecialistsAction';
import { isInterCompanyDiscountChanged, ClearContractHoldingDiscounts } from './assignmentICDiscountsAction';
import { securitymodule } from '../../constants/securityConstant';
import { ClearAssignmentRateSchedule , FetchContractScheduleName } from './contractRateScheduleActions';
import { ClearAdditionalExpenses } from './assignmentAdditionalExpensesAction';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';
import { isNullOrUndefined } from 'util';
import { moduleViewAllRights_Modified } from '../../utils/permissionUtil';
import { FetchVisits } from '../visit/visitAction';
import { isUndefined } from 'lodash';
 
const localConstant = getlocalizeData();
const actions = {
    FetchAssignmentSearchDataSuccess: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_SEARCH_SUCCESS,
        data: payload
    }),
    FetchAssignmentSearchDataError: (error) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_SEARCH_ERROR,
        data: error
    }),
    FetchAssignmentStatusSuccess: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_STATUS_SUCCESS,
        data: payload
    }),
    FetchAssignmentStatusError: (error) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_STATUS_ERROR,
        data: error
    }),
    FetchAssignmentType: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_TYPE,
        data: payload
    }),
    FetchAssignmentLifeCycle: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_LIFE_CYCLE,
        data: payload
    }),
    FetchAssignmentProjectInfo: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_INFO,
        data: payload
    }),
    FetchAssignmentProjectDateInfoEditMode: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_DATE_INFO_EDITMODE,
        data: payload
    }),
    ClearAssignmentSearchResults: () => ({
        type: assignmentsActionTypes.CLEAR_ASSIGNMENT_SEARCH_RESULTS
    }),
    SaveSelectedAssignmentId: (payload) => ({
        type: assignmentsActionTypes.SAVE_SELECTED_ASSIGNMENT_ID,
        data: payload
    }),
    ClearGridFormDatas:(payload)=>
    ({
        type:assignmentsActionTypes.CLEAR_GRID_FORM_DATAS,
        data:payload

    }),
    FetchAssignmentDetailSuccess: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_DETAIL_SUCCESS,
        data: payload
    }),
    FetchAssignmentDetailError: (error) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_DETAIL_ERROR,
        data: error
    }),
    FetchVisitStatus: (payload) => ({
        type: assignmentsActionTypes.FETCH_VISIT_STATUS,
        data: payload
    }),
    FetchProjectAssignmentReferenceTypes: (payload) => ({
        type: assignmentsActionTypes.FETCH_REFERENCE_TYPES,
        data: payload
    }),
    AssignmentHasFinalVisit :(payload) =>({
        type: assignmentsActionTypes.IS_ASSIGNMENT_HAS_FINAL_VISIT,
        data: payload
    }),
    UpdateCopyAssignmentInitialState :(payload) => ({ // D - 631
        type: assignmentsActionTypes.UPDATE_COPY_ASSIGNMENT_INITIAL_STATE,
        data: payload
    }),
    ClearTaxonomyOldValues :(payload) => ({
        type: assignmentsActionTypes.CLEAR_TAXONOMY_OLD_VALUES,
        data: payload
    }),
    ClearAssignmentDetails :(payload) => ({
        type: assignmentsActionTypes.CLEAR_ASSIGNMENT_DETAILS,
        data: payload
    }),
    FetchReviewAndModerationProcess: (payload) => ({
        type: assignmentsActionTypes.FETCH_REVIEW_AND_MODERATION_PROCESS,
        data: payload
    }),
    AssignmentSaveButtonDisable: (payload) => ({
        type: assignmentsActionTypes.ASSIGNMENT_SAVE_BUTTON_DISABLE,
        data: payload
    }),
    ClearAssignmentData: () => ({
        type: assignmentsActionTypes.CLEAR_ASSIGNMENT_MASTER_DATA
    }),
    SelectedTechSpec: (payload) => ({
        type: assignmentsActionTypes.SELECTED_TECH_SPEC,
        data: payload
    }),
    IsInternalAssignment: (payload) => ({
        type: assignmentsActionTypes.IS_INTERNAL_ASSIGNMENT,
        data: payload
    })
};

export const IsInternalAssignment = (payload) => (dispatch) => {
    dispatch(actions.IsInternalAssignment(payload));
};

export const SelectedTechSpec = (payload) => async (dispatch) => {
    dispatch(actions.SelectedTechSpec(payload));
};

export const ClearAssignmentData = () => (dispatch) => {
    dispatch(actions.ClearAssignmentData());
 };

export const ClearAssignmentDetails =() =>(dispatch,getstate)=>{
    dispatch(actions.ClearAssignmentDetails());
};
//Selected Assignment Create Vist and TimeSheet 
export const FetchAssignmentSearchDataSuccess =(data) =>(dispatch,getstate)=>{
    dispatch(actions.FetchAssignmentSearchDataSuccess(data));
};

/** Fetch GeneralDetails Related Master Data */
export const FetchGeneralDetailsMasterData = () => (dispatch) => {
    dispatch(FetchAssignmentStatus());
    dispatch(FetchAssignmentType());
    dispatch(FetchAssignmentLifeCycle());
    dispatch(FetchReviewAndModerationProcess());
    // dispatch(FetchVisitStatus());
};
export const FetchProjectAssignmentReferenceTypes = (isAddAssignment) => async (dispatch, getstate) => { 
    const state = getstate();
    const projectNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentProjectNumber;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const url = customerAPIConfig.projects + projectNumber + assignmentAPIConfig.assignmnetReferenceTypes;
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.assignments.ERROR_FETCHING_REFERENCE_TYPES, 'dangerToast fetchAssignmentWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const referenceTypes = response.result.length > 0 ?
            response.result.filter(x => x.isVisibleToAssignment === true) : [];
        dispatch(actions.FetchProjectAssignmentReferenceTypes(referenceTypes));
        if (response.result) {
            if (isAddAssignment) {
                dispatch(AddProjAssignmentReference(response.result));
            }
        }
    }
};

/** Add Project Assignment Reference to the Assignment Detail*/ // D - 631
export const AddProjAssignmentReference = (assignmentReference) => (dispatch,getstate) => {
    const state = getstate();
    if(assignmentReference && Array.isArray(assignmentReference)){
        const referenceType = Object.assign([],assignmentReference);
        const assignmentReferenceType = referenceType.filter(x => x.isVisibleToAssignment === true);
        if (assignmentReferenceType.length > 0) {
            for (let i = 0; i < assignmentReferenceType.length; i++) {
                const defaultRefernceTypes = {
                    assignmentReferenceTypeAddUniqueId:Math.floor(Math.random() * 99) - 100,
                    assignmentReferenceTypeId:null,
                    recordStatus:'N',
                    assignmentId:state.rootAssignmentReducer.assignmentDetail.AssignmentInfo ?
                    state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId : null,
                    modifiedBy:state.appLayoutReducer.loginUser,
                    referenceType:assignmentReferenceType[i].referenceType,
                    referenceValueValidation:localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE
                };
                dispatch(AddProjectAssignmentReference(defaultRefernceTypes));
            }
        }
    }
};
/**
 * method will fetch project for assignment creation
 * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc
 */
export const FetchProjectForAssignmentCreation = (supplierPoInfo, isFetchLookValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state =getstate();
    const projectNo = state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo;   
    const assignmentDetailData = Object.assign({}, state.rootAssignmentReducer.assignmentDetail);
    const assignmentValidationData = Object.assign({}, state.rootAssignmentReducer.dataToValidateAssignment);
    // const projectInfoUrl = projectAPIConfig.projectSearch;
    const projectInfoUrl = assignmentAPIConfig.projectAssignmentCreation; // Optimization
    if(!required(projectNo)){
        const params = {
            'projectNumber': projectNo
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(projectInfoUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast AssActDataNotFound');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                if (response.result && response.result.length > 0) {
                    const businessUnit = isEmptyReturnDefault(state.masterDataReducer.businessUnit);
                    const assignmentData = FetchAssignmentProjectInfo(response.result[0], assignmentDetailData, assignmentValidationData, businessUnit);
                    if(!isEmpty(supplierPoInfo) && supplierPoInfo.supplierPOId){
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierPurchaseOrderId=supplierPoInfo.supplierPOId;
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierPurchaseOrderNumber=supplierPoInfo.supplierPONumber;
                        // assignmentData.assignmentData.AssignmentSubSuppliers=supplierPOData.SupplierPOSubSupplier;
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierId=supplierPoInfo.supplierPOMainSupplierId;
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierName=supplierPoInfo.supplierPOMainSupplierName; //Scenario 159 fixes
                        dispatch(FetchSupplierInfoData(assignmentData.assignmentData));
                    }
                    else{
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierPurchaseOrderId="";
                        assignmentData.assignmentData.AssignmentInfo.assignmentSupplierPurchaseOrderNumber="";
                    }
                    dispatch(actions.FetchAssignmentProjectInfo(assignmentData));
                    dispatch(CheckAddAssignmentReference(true));
                    dispatch(FetchGeneralDetails(assignmentData.assignmentData.AssignmentInfo, isFetchLookValues));
                }
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssignmentSearchResultsErr');
            }
            else {
                IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast FetchAssignmentSearchResultsErr');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast FetchAssignmentSearchResultsErr');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast FetchAssignmentSearchResultsErr');
    }
    dispatch(HideLoader());
};

export const FetchProjectForAssignmentEdit= (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    // const projectInfoUrl = projectAPIConfig.projectSearch;
    const projectInfoUrl = assignmentAPIConfig.projectAssignmentCreation; // Optimization
    const params = {
        'projectNumber': data
    };

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectInfoUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast AssActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            if (response.result && response.result.length > 0) {
             const  assignmentValidationData = {
                    "fromDate": response.result[0].projectStartDate,
                    "toDate": response.result[0].projectEndDate,
                    "projectBudgetValue":response.result[0].projectBudgetValue,
                    "projectBudgetUnit":response.result[0].projectBudgetHoursUnit,
                    "projectBudgetWarning":response.result[0].projectBudgetWarning,
                    "projectBudgetHoursWarning":response.result[0].projectBudgetHoursWarning,
                    "isProjectForNewFacility":response.result[0].isProjectForNewFacility,    //Added for Assignment Lifecycle Validation
                    "projectClientReportingRequirements": response.result[0].projectClientReportingRequirement, // ITK D-781
                };
                dispatch(actions.FetchAssignmentProjectDateInfoEditMode(assignmentValidationData));
            }
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssignmentSearchResultsErr');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast FetchAssignmentSearchResultsErr');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_ASSIGNMENT_CREATION, 'dangerToast FetchAssignmentSearchResultsErr');
    }
    dispatch(HideLoader());
};

const FetchAssignmentProjectInfo = (data, assignmentDetailData, assignmentValidationData, businessUnits) =>{
    const assignmentData = assignmentDetailData;
    const isMargin = isSettlingTypeMargin({
        projectReportParams: businessUnits,
        businessUnit: data.projectType
    });
    if (data) {
        
        assignmentData.AssignmentInfo = {
            "assignmentId": null,
            "assignmentBudgetCurrency": data.projectBudgetCurrency,
            // "assignmentBudgetValue": data.projectBudgetValue,
            // "assignmentBudgetHours": data.projectBudgetHoursUnit,
            // "assignmentBudgetWarning": data.projectBudgetWarning,
            // "assignmentBudgetHoursWarning": data.projectBudgetHoursWarning,
            "assignmentBudgetValue":"00.00",
            "assignmentBudgetHours":"00.00",
            "assignmentBudgetWarning":data.projectBudgetWarning,
            "assignmentBudgetHoursWarning":data.projectBudgetHoursWarning,
            "assignmentInvoicedToDate": data.projectInvoicedToDate,
            "assignmentUninvoicedToDate": data.projectUninvoicedToDate,
            "assignmentHoursInvoicedToDate": data.projectHoursInvoicedToDate,
            "assignmentHoursUninvoicedToDate": data.projectHoursUninvoicedToDate,
            "assignmentRemainingBudgetValue": data.projectRemainingBudgetValue,
            "assignmentRemainingBudgetHours": data.projectRemainingBudgetHours,
            "assignmentCustomerCode": data.contractCustomerCode,
            "assignmentCustomerName": data.contractCustomerName,
            "assignmentContractHoldingCompanyCode": data.contractHoldingCompanyCode,
            "assignmentContractHoldingCompany": data.contractHoldingCompanyName,
            "assignmentOperatingCompanyCode": data.contractHoldingCompanyCode,
            "assignmentOperatingCompany": data.contractHoldingCompanyName,
            "assignmentContractNumber": data.contractNumber,
            "assignmentProjectNumber": data.projectNumber,
            "clientReportingRequirements": data.projectClientReportingRequirement,
            "assignmentContractHoldingCompanyCoordinator": data.projectCoordinatorName,
            "assignmentContractHoldingCompanyCoordinatorCode": data.projectCoordinatorCode,
            "assignmentOperatingCompanyCoordinator": data.projectCoordinatorName,
            "assignmentOperatingCompanyCoordinatorCode": data.projectCoordinatorCode,
            "assignmentCustomerAssigmentContact": data.projectCustomerContact,
            "assignmentCompanyAddress": data.projectCustomerContactAddress,
            "recordStatus": "N",
            "assignmentProjectWorkFlow": data.workFlowType,
            "assignmentProjectType": data.projectType, // used in IC Discounts
            /**To-do for TS selection weathear ARS or with in the Evo2 */
            "assignmentBusinessUnit":data.projectType,
            "assignmentProjectBusinessUnit":data.projectType,
            "assignmentCustomerProjectName": data.customerProjectName,
            /**To-do for TS selection weathear ARS or with in the Evo2 */
            
            "assignmentType": ((data.workFlowType.trim() === "V" || data.workFlowType.trim() === "S") ? "A" : "R"),
            "assignmentStatus": "P",

            "assignmentHostCompanyCode": (isMargin ? data.contractHoldingCompanyCode : null), // ITK D - 712 & 715
            "assignmentHostCompany": (isMargin ? data.contractHoldingCompanyName : null), // ITK D - 712 & 715

            // "assignmentHostCompanyCode": ((data.workFlowType.trim() === "M") ? data.contractHoldingCompanyCode : null),
            // "assignmentHostCompany": ((data.workFlowType.trim() === "M") ? data.contractHoldingCompanyName : null),
            "assignmentParentContractCompany": data.assignmentParentContractCompany,
            "assignmentParentContractCompanyCode": data.assignmentParentContractCompanyCode,
            "assignmentParentContractDiscount": data.assignmentParentContractDiscount,
        };
        assignmentData.AssignmentInstructions = {
            "interCompanyInstructions": data.projectAssignmentOperationNotes,
            "technicalSpecialistInstructions": data.projectAssignmentOperationNotes,
            "assignmentId":null,
            "recordStatus": "N"
        };
        assignmentValidationData = {
            "fromDate": data.projectStartDate,
            "toDate": data.projectEndDate,
            "projectBudgetValue":data.projectBudgetValue,
            "projectBudgetUnit":data.projectBudgetHoursUnit,
            "projectBudgetWarning":data.projectBudgetWarning,
            "projectBudgetHoursWarning":data.projectBudgetHoursWarning,
            "isProjectForNewFacility":data.isProjectForNewFacility   //Added for Assignment Lifecycle Validation
        };
        let ocDiscount =100;
        if( !isNaN(Number(data.assignmentParentContractDiscount))){
            ocDiscount = 100 - data.assignmentParentContractDiscount;
        }
        assignmentData.AssignmentInterCompanyDiscounts = {
            "assignmentOperatingCompanyDiscount":ocDiscount && parseFloat(ocDiscount).toFixed(2),
            "assignmentOperatingCompanyCode": data.contractHoldingCompanyCode,
            "assignmentOperatingCompanyName": data.contractHoldingCompanyName,
            "assignmentParentContractCompany": data.assignmentParentContractCompany,
            "assignmentParentContractCompanyCode": data.assignmentParentContractCompanyCode,
            "parentContractHoldingCompanyDiscount": data.assignmentParentContractDiscount,
            "assignmentId":null,
            "recordStatus":"N"
        };

        assignmentData.AssignmentSubSuppliers = null;
        assignmentData.AssignmentSubSupplierTechnicalSpecialist = null;
        assignmentData.AssignmentContractSchedules = null;
        assignmentData.AssignmentReferences = null;
        assignmentData.AssignmentTechnicalSpecialists = null;
        assignmentData.AssignmentAdditionalExpenses = null;
         
        assignmentData.AssignmentNotes = null;
        assignmentData.AssignmentDocuments = null;
        assignmentData.AssignmentContributionCalculators = null;
        assignmentData.AssignmentTaxonomy = null;
    }
    return { assignmentData, assignmentValidationData };
};

export const FetchAssignmentSearchResults = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchAssignmentSearchDataSuccess(null));
    const assignmentSearchUrl = assignmentAPIConfig.assignment;
    const params = {};
    if (!isEmpty(getstate().CommonReducer.currentPage) &&  
        getstate().CommonReducer.currentPage === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE)    
    {
        params.moduleName = "ASGMNT"; /*TODO- hard coding to be removed*/
    }
    if (data.customerName) {
        params.assignmentCustomerName = data.customerName;
    }
    if (data.assignmentNumber) {
        params.assignmentNumber = data.assignmentNumber;
    }
    if (data.projectNumber) {
        params.assignmentProjectNumber = data.projectNumber;
    }
    if (data.supplierPurchaseOrderNumber) {
        params.assignmentSupplierPurchaseOrderNumber = data.supplierPurchaseOrderNumber;
    }
    if (data.contractHoldingCompanyCode) {
        params.assignmentContractHoldingCompanyCode = data.contractHoldingCompanyCode;
    }

    if (data.operatingCompanyCode) {
        params.assignmentOperatingCompanyCode = data.operatingCompanyCode;
    }

    if (data.contractHoldingCompanyCoordinator) {
        params.assignmentContractHoldingCompanyCoordinator = data.contractHoldingCompanyCoordinator;
    }

    if (data.ContractHoldingCoordinator) {
        params.assignmentContractHoldingCompanyCoordinator = data.ContractHoldingCoordinator;
    }

    if (data.OperatingCompanyCoordinator) {
        params.assignmentOperatingCompanyCoordinator = data.OperatingCompanyCoordinator;
    }

    if (data.technicalSpecialistName) {
        params.technicalSpecialistName = data.technicalSpecialistName;
    }

    if (data.assignmentStatus) {
        params.assignmentStatus = data.assignmentStatus;
    }
    if (data.assignmentReference) {
        params.assignmentReference = data.assignmentReference;
    }

    if (data.assignmentDocumentId) {
        params.assignmentDocumentId = data.assignmentDocumentId;
    }
    if (data.searchDocumentType) {
        params.searchDocumentType = data.searchDocumentType;
    }
    if (data.documentSearchText) {
        params.documentSearchText = data.documentSearchText;
    }
    if(data.assignmentSupplierPurchaseOrderId) {
        params.AssignmentSupplierPurchaseOrderId = data.assignmentSupplierPurchaseOrderId;
    }
    if(data.workFlowTypeIn){
        params.workFlowTypeIn = data.workFlowTypeIn;
    }
    params.isOnlyViewAssignment = data.isOnlyViewAssignment;  
    params.loggedInCompanyCode = data.loggedInCompanyCode;     
    params.loggedInCompanyId = data.loggedInCompanyId; 
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentSearchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchAssignmentSearchDataError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetcssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchAssignmentSearchDataSuccess(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssigntSearchResultsErr');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetignmentSearchResultsErr');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetignmentSearchResultsErr');
    }
    dispatch(HideLoader());
   
};

export const FetchAssignmentSearchResultswithLoadMore = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchAssignmentSearchDataSuccess(null));
    const assignmentSearchUrl = assignmentAPIConfig.assignment;

    const params = {};
    if (data.customerName) {
        params.assignmentCustomerName = data.customerName;
        params.assignmentCustomerId = data.assignmentCustomerId;
    }
    if (data.assignmentNumber) {
        params.assignmentNumber = data.assignmentNumber;
    }
    if (data.projectNumber) {
        params.assignmentProjectNumber = data.projectNumber;
    }
    if (data.supplierPurchaseOrderNumber) {
        params.assignmentSupplierPurchaseOrderNumber = data.supplierPurchaseOrderNumber;
    }
    if (data.contractHoldingCompanyCode) {
        // params.assignmentContractHoldingCompanyCode = data.contractHoldingCompanyCode;
        params.AssignmentContractHoldingCompanyId  = data.contractHoldingCompanyCode;
    }

    if (data.operatingCompanyCode) {
        // params.assignmentOperatingCompanyCode = data.operatingCompanyCode;
        params.AssignmentOperatingCompanyId = data.operatingCompanyCode;
    }

    if (data.contractHoldingCompanyCoordinator) {
        params.assignmentContractHoldingCompanyCoordinator = data.contractHoldingCompanyCoordinator;
    }

    if (data.ContractHoldingCoordinator) {
        params.assignmentContractHoldingCompanyCoordinator = data.ContractHoldingCoordinator;
    }

    if (data.OperatingCompanyCoordinator) {
        params.assignmentOperatingCompanyCoordinator = data.OperatingCompanyCoordinator;
    }

    if (data.technicalSpecialistName) {
        params.technicalSpecialistName = data.technicalSpecialistName;
    }

    if (data.assignmentStatus) {
        params.assignmentStatus = data.assignmentStatus;
    }
    if (data.assignmentReference) {
        params.assignmentReference = data.assignmentReference;
    }

    if (data.assignmentDocumentId) {
        params.assignmentDocumentId = data.assignmentDocumentId;
    }
    if (data.searchDocumentType) {
        params.searchDocumentType = data.searchDocumentType;
    }
    if (data.documentSearchText) {
        params.documentSearchText = data.documentSearchText;
    }
    if(data.assignmentSupplierPurchaseOrderId) {
        params.AssignmentSupplierPurchaseOrderId = data.assignmentSupplierPurchaseOrderId;
    }
    if(data.workFlowTypeIn){
        params.workFlowTypeIn = data.workFlowTypeIn;
    }
    if(data.assignmentCategory){
        params.categoryId = data.assignmentCategory;
    }
    if(data.assignmentSubCategory){
        params.subCategoryId = data.assignmentSubCategory;
    }
    if(data.assignmentService){
        params.serviceId = data.assignmentService;
    }
    if(data.materialDescription){
        params.materialDescription = data.materialDescription;
    }
    //search optimization
    if(data.offSet){
        params.OffSet = data.offSet;
    }
    //search optimization
    if (data.assignmentSearchTotalCount) {
        params.TotalCount = data.assignmentSearchTotalCount;
    }
    if (data.isExport) //Added for Export to CSV
        params.isExport = data.isExport;

    params.isOnlyViewAssignment = data.isOnlyViewAssignment;  
    params.loggedInCompanyCode = data.loggedInCompanyCode;     
    params.loggedInCompanyId = data.loggedInCompanyId; 
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentSearchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchAssignmentSearchDataError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetcssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchAssignmentSearchDataSuccess(response.result));
            dispatch(HideLoader());
            return response;
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssigntSearchResultsErr');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetignmentSearchResultsErr');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SEARCH_RESULT, 'dangerToast FetignmentSearchResultsErr');
    }
    dispatch(HideLoader());
   
};

/** Fetch Assignment Selected ID Info **/
export const FetchAssignmentsDetailInfo = (data) => async (dispatch, getState) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchAssignmentSearchDataSuccess(null));
    const params = {
        isVisitTimesheet:!isEmptyOrUndefine(data) && data.isVisitTimesheet != "undefined" ? data.isVisitTimesheet : false
    };
    const assignmentId = parseInt(data.assignmentId);
    const assignmentsInfoUrl = assignmentAPIConfig.assignmentsInfo + assignmentId;
    const requestPayload = new RequestPayload(params);
    let assigResponse;
    const response = await FetchData(assignmentsInfoUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchAssignmentSearchDataError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SELECTED_INFO, 'dangerToast FetcssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
        assigResponse = false;
    });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchAssignmentSearchDataSuccess(response.result));
            assigResponse = response.result && response.result.length > 0 ? response.result[0] : {};
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssigntSearchResultsErr');
            assigResponse = false;
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SELECTED_INFO, 'dangerToast FetignmentSearchResultsErr');
            assigResponse = false;
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_SELECTED_INFO, 'dangerToast FetignmentSearchResultsErr');
        assigResponse = false;
    }
    dispatch(HideLoader());
    return assigResponse;
};

/** Fetch Assignment status action */
export const FetchAssignmentStatus = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootAssignmentReducer.assignmentStatus)) {
        return;
    }
    const assignmentStatusUrl = assignmentAPIConfig.assignmentStatus;
    const isActive=true;
    const params = {
        isActive:isActive  //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentStatusUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(actions.FetchAssignmentStatusError(error));
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchAssignmentStatusSuccess(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assActassignStatusErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_STATUS, 'dangerToast assActassignStatusErrMsg1');            
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_STATUS, 'dangerToast assActassignStatusErrMsg1');
    }
};

/** Fetch Assignment Type action */
export const FetchAssignmentType = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootAssignmentReducer.assignmentType)) {
        return;
    }
    const assignmentTypeUrl = assignmentAPIConfig.assignmentType;
    const isActive=true;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentTypeUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchAssignmentType(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assActassignTypeErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg1');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg1');
    }
};

/** Fetch Visit Staus action */
export const FetchVisitStatus = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootAssignmentReducer.visitStatus)) {
        return;
    }
    const assignmentVisitUrl = assignmentAPIConfig.visitStatus;
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
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assActassignVisitStatErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};

/** Fetch Assignment Life Cycles action */
export const FetchAssignmentLifeCycle = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootAssignmentReducer.assignmentLifeCycle)) {
        return;
    }
    const isActive=true;
    const assignmentLifeCycleUrl = assignmentAPIConfig.assignmentLifeCycle;
    const params = {
        isActive:isActive  //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentLifeCycleUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_LIFE_CYCLE, 'dangerToast assActassignLifeCycleErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result,'name','asc');
            dispatch(actions.FetchAssignmentLifeCycle(result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assActassignLifeCycleErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_LIFE_CYCLE, 'dangerToast assActassignLifeCycleErrMsg1');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_LIFE_CYCLE, 'dangerToast assActassignLifeCycleErrMsg1');
    }
};

export const ClearSearchCustomerList = () => (dispatch) => {
    dispatch(actions.ClearSearchCustomerList());

};

export const ClearAssignmentSearchResults = () => (dispatch, getstate) => {
    dispatch(actions.ClearAssignmentSearchResults());
};
export const SaveSelectedAssignmentId = (selectedAssignmentData) => (dispatch) => {
    dispatch(actions.SaveSelectedAssignmentId(selectedAssignmentData));
};

export const ClearGridFormDatas=()=>(dispatch,getstate)=>
{
dispatch(actions.ClearGridFormDatas());
};

/**
* Fetch assignment detail
*/
export const FetchAssignmentDetail = (assignmentId, isCopyAssignment,isFetchLookValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!assignmentId)
        assignmentId = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;
    if (!assignmentId) {
        IntertekToaster("Failed to Fetch Assignment", 'dangerToast FetchBusinessUnit');
    }
    const url = StringFormat(assignmentAPIConfig.assignmentDetail, assignmentId);
    dispatch(FetchVisits({ assignmentId: assignmentId }));
    const params = { 'IsInvoiceDetailRequired': true };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(actions.FetchAssignmentDetailError(error));
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_ASSIGNMENT_DETAIL, 'dangerToast FetchBusinessUnit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return error;
        });

        //646 Fixes
        if (!isEmpty(response) && isEmpty(response.AssignmentInfo)){
            dispatch(HideLoader());
            dispatch(ChangeDataAvailableStatus(true));
            return response;
        }

    if (!isEmpty(response) && !isEmpty(response.AssignmentInfo)) {
        response.AssignmentInfo.assignmentInvoicedToDate = formatToDecimal(response.AssignmentInfo.assignmentInvoicedToDate,2);
        response.AssignmentInfo.assignmentUninvoicedToDate = formatToDecimal(response.AssignmentInfo.assignmentUninvoicedToDate,2);
        response.AssignmentInfo.assignmentRemainingBudgetValue = formatToDecimal(response.AssignmentInfo.assignmentRemainingBudgetValue,2);
        response.AssignmentInfo.assignmentHoursInvoicedToDate = formatToDecimal(response.AssignmentInfo.assignmentHoursInvoicedToDate,2);
        response.AssignmentInfo.assignmentHoursUninvoicedToDate = formatToDecimal(response.AssignmentInfo.assignmentHoursUninvoicedToDate,2);
        response.AssignmentInfo.assignmentRemainingBudgetHours = formatToDecimal(response.AssignmentInfo.assignmentRemainingBudgetHours,2);
        response.AssignmentInfo.assignmentBudgetValue = formatToDecimal(response.AssignmentInfo.assignmentBudgetValue,2);
        response.AssignmentInfo.assignmentBudgetHours = formatToDecimal(response.AssignmentInfo.assignmentBudgetHours,2);
        response.AssignmentInfo.assignmentBudgetWarning = response.AssignmentInfo.assignmentBudgetWarning !== null ? response.AssignmentInfo.assignmentBudgetWarning : 0;
        response.AssignmentInfo.assignmentBudgetHoursWarning = response.AssignmentInfo.assignmentBudgetHoursWarning !== null ? response.AssignmentInfo.assignmentBudgetHoursWarning : 0; //Changes for D1383
        if(response.AssignmentInterCompanyDiscounts)
        {
            response.AssignmentInterCompanyDiscounts.assignmentOperatingCompanyDiscount=response.AssignmentInterCompanyDiscounts.assignmentOperatingCompanyDiscount && response.AssignmentInterCompanyDiscounts.assignmentOperatingCompanyDiscount.toFixed(2);
            response.AssignmentInterCompanyDiscounts.assignmentHostcompanyDiscount=response.AssignmentInterCompanyDiscounts.assignmentHostcompanyDiscount && response.AssignmentInterCompanyDiscounts.assignmentHostcompanyDiscount.toFixed(2);
            response.AssignmentInterCompanyDiscounts.assignmentContractHoldingCompanyDiscount = response.AssignmentInterCompanyDiscounts.assignmentContractHoldingCompanyDiscount && response.AssignmentInterCompanyDiscounts.assignmentContractHoldingCompanyDiscount.toFixed(2);
            response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany1_Discount=response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany1_Discount && response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany1_Discount.toFixed(2);
            response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany2_Discount=response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany2_Discount && response.AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany2_Discount.toFixed(2);
        }
        //D578
        if(response.AssignmentTechnicalSpecialists){
            response.AssignmentTechnicalSpecialists.forEach(itratedValue =>{
                if(itratedValue.profileStatus !== undefined && itratedValue.profileStatus === localConstant.assignments.INACTIVE){
                    itratedValue.InActiveDangerTag = true;
                }
            });
        }

        if (Array.isArray(response.AssignmentAdditionalExpenses) && response.AssignmentAdditionalExpenses.length > 0) {
            response.AssignmentAdditionalExpenses.forEach(expanse => {
                expanse.totalUnit = formatToDecimal(expanse.totalUnit, 2);
                expanse.perUnitRate = formatToDecimal(expanse.perUnitRate, 4);
            });
        }
        if(!isCopyAssignment){
            dispatch(FetchSupplierInfoData(response));
        }
        dispatch(actions.FetchAssignmentDetailSuccess(response));
        dispatch(FetchAssignmentTechSpec(response.AssignmentTechnicalSpecialists));
        dispatch(actions.IsInternalAssignment(response.AssignmentInfo.isInternalAssignment));
        //set isFetchLookValues to true when we fetch Assignment initially
        //on save, cancel we dont need to fetch the data that already exists
        if(isFetchLookValues === true || isFetchLookValues === undefined){
            dispatch(FetchGeneralDetails(response.AssignmentInfo, isFetchLookValues));
            //dispatch(FetchProjectAssignmentReferenceTypes());
            dispatch(CheckAddAssignmentReference(false));
            dispatch(FetchProjectForAssignmentEdit(response.AssignmentInfo.assignmentProjectNumber));
        }
        const { assignmentContractHoldingCompanyCode, assignmentOperatingCompanyCode, isContractHoldingCompanyActive } = response.AssignmentInfo;
        const isMandatoryView = !isContractHoldingCompanyActive;

        // const isViewAllAssignment = isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies).length > 0;
        const isViewAllAssignment = moduleViewAllRights_Modified(securitymodule.ASSIGNMENT, state.masterDataReducer.viewAllRightsCompanies);
        dispatch(UpdateInteractionMode([ assignmentContractHoldingCompanyCode,assignmentOperatingCompanyCode ], isViewAllAssignment,isMandatoryView));
        // dispatch(UpdateInteractionMode([ assignmentContractHoldingCompanyCode,assignmentOperatingCompanyCode ],securitymodule.ASSIGNMENT,isViewAllAssignment,isMandatoryView));
        dispatch(HideLoader());
    };
    return response;
};

export const AssignmentHasFinalVisit = (status) => (dispatch,getstate)=>{
    dispatch(actions.AssignmentHasFinalVisit(status));
};
// /** Fetch Supplier Inforamtion related Data */
// export const FetchSupplierInfoData = (data) => (dispatch,getstate) => {
//     const state=getstate();
//     const supplierPOList=isEmptyReturnDefault(state.rootAssignmentReducer.supplierPO);
//     if(data && data.AssignmentInfo && data.AssignmentInfo.assignmentSupplierPurchaseOrderId){
//         const supplierPOId = data.AssignmentInfo.assignmentSupplierPurchaseOrderId;
//         if(isEmptyReturnDefault(data.AssignmentSubSuppliers).length>0){
//             const supplierPOObj = arrayUtil.FilterGetObject(supplierPOList, "supplierPOId", parseInt(supplierPOId));
//             if(supplierPOObj){
//             dispatch(FetchSupplierContacts(supplierPOObj.supplierPOMainSupplierId));
//             }
//             dispatch(FetchSubsuppliers(supplierPOId));
//         }
//     }
// };
/** Fetch Supplier Inforamtion related Data */
export const FetchSupplierInfoData = (data) => async (dispatch,getstate) => {
    const supplierPurchaseOrderId = getNestedObject(getstate().rootAssignmentReducer.assignmentDetail,
        [ "AssignmentInfo", "assignmentSupplierPurchaseOrderId" ]);
    // if (isEmptyReturnDefault(data.AssignmentSubSuppliers).length > 0) {
        if (data && data.AssignmentInfo && data.AssignmentInfo.assignmentSupplierPurchaseOrderId) {
            const supplierPOId = data.AssignmentInfo.assignmentSupplierPurchaseOrderId;
            if(supplierPurchaseOrderId !== supplierPOId){
                dispatch(FetchSupplierContacts(data.AssignmentInfo.assignmentSupplierId));
            }
            if(!data.AssignmentInfo.assignmentId){
                const mainSupplierData = {};
                mainSupplierData["assignmentSubSupplierId"]= null;
                mainSupplierData["assignmentId"]= null;
                mainSupplierData["subSupplierId"]= null;
                mainSupplierData["supplierName"]= null;
                mainSupplierData["supplierContactId"]= null;
                mainSupplierData["supplierContactName"]= null;
                mainSupplierData["mainSupplierContactName"]= null;
                mainSupplierData["mainSupplierContactId"]= null;
                mainSupplierData["isSubSupplierFirstVisit"]= null;
                mainSupplierData["assignmentSubSupplierTS"]= [];
                mainSupplierData["recordStatus"]= "N";
                mainSupplierData["mainSupplierId"]= data.AssignmentInfo.assignmentSupplierId;
                const response = await dispatch(FetchSubsuppliers(supplierPOId,data));
                if (response) {
                    mainSupplierData['isMainSupplierFirstVisit'] = true;
                    dispatch(AddSubSupplierInformation(mainSupplierData, null,true));
                }
                else {
                    mainSupplierData['isMainSupplierFirstVisit'] = false;
                    dispatch(AddSubSupplierInformation(mainSupplierData, null,true));
                }
            }
            else{
                // supplierPOId and Assignment detail as parameter.
                dispatch(FetchSubsuppliers(supplierPOId,data));
            }
        }
    // }
};

/**
 * Save Assignment Detail
 */
export const SaveAssignmentDetails = () => (dispatch, getstate) => {
    const state =getstate();
    let assignmentData = state.rootAssignmentReducer.assignmentDetail;
    const contractHoldingCompanyCoordinators = state.rootAssignmentReducer.contractHoldingCompanyCoordinators;
    const operatingCompanyCoordinators = state.rootAssignmentReducer.operatingCompanyCoordinators;
    const ARSDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    const techList = state.RootTechSpecReducer.TechSpecDetailReducer.techspecList;
    const assignmentMode = state.CommonReducer.currentPage;

    // Added on 20 Jan 2021- Basis francina mail
    assignmentData.AssignmentInfo.preAssignmentId=!isEmpty(state.rootAssignmentReducer.selectedPreAssignment) ?
    state.rootAssignmentReducer.selectedPreAssignment.id : null;

    //Adding Coordinator e-mail Id's
    let assignmentContractCompanyCoordinatorEmail=null;
    if (assignmentData.AssignmentInfo && assignmentData.AssignmentInfo.assignmentContractHoldingCompanyCoordinatorCode) {
        assignmentContractCompanyCoordinatorEmail = fetchCoordinatorDetails(assignmentData.AssignmentInfo.assignmentContractHoldingCompanyCoordinatorCode,
            contractHoldingCompanyCoordinators, "username", "email");
        assignmentData.AssignmentInfo.assignmentContractCompanyCoordinatorEmail =  assignmentContractCompanyCoordinatorEmail.length > 0 ? assignmentContractCompanyCoordinatorEmail[0] : "";
    }
    let assignmentOperatingCompanyCoordinatorEmail=null;
    if(assignmentData.AssignmentInfo && assignmentData.AssignmentInfo.assignmentOperatingCompanyCoordinatorCode){
        assignmentOperatingCompanyCoordinatorEmail = fetchCoordinatorDetails(assignmentData.AssignmentInfo.assignmentOperatingCompanyCoordinatorCode,
            operatingCompanyCoordinators, "username", "email");
        assignmentData.AssignmentInfo.assignmentOperatingCompanyCoordinatorEmail = assignmentOperatingCompanyCoordinatorEmail.length > 0 ? assignmentOperatingCompanyCoordinatorEmail[0] : "";
    }
    if(isUndefined(assignmentData.AssignmentInfo.isInternalAssignment)){
        assignmentData.AssignmentInfo.isInternalAssignment = false;
    }
    /** Save code for ARS Search - Starts*/
    if(!isEmpty(ARSDetails)){
        if(ARSDetails.id && ARSDetails.id !== 0){
            ARSDetails.recordStatus = 'M';
        }
        else{
            ARSDetails.recordStatus = 'N';
        }
        ARSDetails.assignmentId = assignmentData.AssignmentInfo && assignmentData.AssignmentInfo.id;
        ARSDetails.searchParameter.preAssignmentId=assignmentData.AssignmentInfo && assignmentData.AssignmentInfo.preAssignmentId;
    }

    if(ARSDetails.searchParameter && ARSDetails.searchParameter.assignedResourceInfos 
        && ARSDetails.searchParameter.assignedResourceInfos.length > 0){
        const ARSData = deepCopy(isEmptyReturnDefault(ARSDetails.searchParameter.assignedResourceInfos));
        ARSData.forEach((x) => {
            techList.forEach(y => {
                if (y.supplierLocationId.includes(x.supplierId)) {
                    x.assignedTechSpec.forEach((z, index) => {
                        y.resourceSearchTechspecInfos.forEach(x1 => {
                            if (z.epin == x1.epin && x1.isSelected === false) {
                                x.assignedTechSpec.splice(index, 1);
                            }
                        });
                    });
                }
            });
        });
    ARSDetails.searchParameter.assignedResourceInfos = ARSData;
}

    assignmentData.ResourceSearch = ARSDetails;
    /** Save code for ARS Search - Ends*/

    /** ITK D - 491 - Starts */
    const assignmentSubSupplier = deepCopy(isEmptyReturnDefault(assignmentData.AssignmentSubSuppliers));
    assignmentSubSupplier.forEach(x => {
        const assignmentSubSuppleirTS = getNestedObject(x,[ 'assignmentSubSupplierTS' ]);
        if (Array.isArray(assignmentSubSuppleirTS) && assignmentSubSuppleirTS.length > 0) {
            assignmentSubSuppleirTS.forEach(y => {
                if (y.isAssignedToThisSubSupplier === null) {
                    y.isAssignedToThisSubSupplier = true;
                }
                else if (y.isAssignedToThisSubSupplier === false && y.recordStatus == null && techList !== undefined && techList.length > 0) {
                    const mainSupp = techList.findIndex(x => x.supplierInfo !== null && x.supplierInfo.length > 0 && x.supplierInfo[0].supplierType == "Supplier");
                    if (techList[mainSupp] !== undefined) {
                        techList[mainSupp].resourceSearchTechspecInfos.forEach(x1 => {
                            if (y.epin == x1.epin) {
                                if (x1.isSelected == false) {
                                    y.recordStatus = 'D';
                                }
                            }
                        });
                    }
                }
            });
        }
    });
    assignmentData.AssignmentSubSuppliers = assignmentSubSupplier;
    /** ITK D - 491 - Ends */
    if (!isEmpty(assignmentData.AssignmentInfo.visitFromDate)) {
        assignmentData.AssignmentInfo.fromDate = moment(assignmentData.AssignmentInfo.visitFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        assignmentData.AssignmentInfo.visitFromDate = moment(assignmentData.AssignmentInfo.visitFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    if (!isEmpty(assignmentData.AssignmentInfo.visitToDate)) {
        assignmentData.AssignmentInfo.toDate = moment(assignmentData.AssignmentInfo.visitToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        assignmentData.AssignmentInfo.visitToDate = moment(assignmentData.AssignmentInfo.visitToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isEmpty(assignmentData.AssignmentInfo.timesheetFromDate)) {
        assignmentData.AssignmentInfo.fromDate = moment(assignmentData.AssignmentInfo.timesheetFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        assignmentData.AssignmentInfo.timesheetFromDate = moment(assignmentData.AssignmentInfo.timesheetFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    if (!isEmpty(assignmentData.AssignmentInfo.timesheetToDate)) {
        assignmentData.AssignmentInfo.toDate = moment(assignmentData.AssignmentInfo.timesheetToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        assignmentData.AssignmentInfo.timesheetToDate = moment(assignmentData.AssignmentInfo.timesheetToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isNullOrUndefined(assignmentData.AssignmentInfo.assignmentParentContractDiscount)) {
        assignmentData.AssignmentInfo.assignmentParentContractDiscount = parseFloat(assignmentData.AssignmentInfo.assignmentParentContractDiscount).toFixed(2);
    }
    // if(assignmentData && Array.isArray(assignmentData.AssignmentSubSuppliers) && assignmentData.AssignmentSubSuppliers.length>0 )
    //     assignmentData.AssignmentSubSuppliers.forEach((items,index)=>
    //         {
    //             // if(required(items.isPartofAssignment))
    //             // {
    //             //     assignmentData.AssignmentSubSuppliers.splice(index,1);
    //             // }

    //         });//MS-TS CR
    
    if (assignmentMode === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
        assignmentData.AssignmentInfo.recordStatus = "N";
        assignmentData.AssignmentInfo.createdBy = state.appLayoutReducer.loginUser;
        // ITK D 700
        if(!isEmpty(ARSDetails)){
            if(ARSDetails.searchAction == "PLO" || ARSDetails.searchAction == "OPR")
                assignmentData.AssignmentInfo.isOverrideOrPLO = true; 
        }
        // assignmentData = FilterSave(assignmentData);
        dispatch(CreateAssignmentDetails(assignmentData));
    }
    if (assignmentMode === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE) {
        assignmentData.AssignmentInfo.recordStatus = "M";
        assignmentData.AssignmentInfo.modifiedBy = state.appLayoutReducer.loginUser;
        if(!isEmpty(ARSDetails) && (ARSDetails.searchAction == "PLO" || ARSDetails.searchAction == "OPR") && isEmpty(assignmentData.AssignmentTechnicalSpecialists)){
            assignmentData.AssignmentInfo.isOverrideOrPLO = true; 
        }
        assignmentData = FilterSave(assignmentData);
        dispatch(UpdateAssignmentDetails(assignmentData));
    }

};

/**Create Assignment Details */
export const CreateAssignmentDetails = (payload) => async (dispatch) => {
    dispatch(ShowLoader());
    const postUrl = StringFormat(assignmentAPIConfig.assignmentDetail, 0);
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_POST_ERROR, 'dangerToast AssActCreateAssPostErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code === "1") {
            if (response.result) {
                dispatch(CreateAlert(response.result, "Assignment"));
                dispatch(FetchAssignmentDetail(response.result,false,false));
                /** Code for ARS Search data clear after save */
                dispatch(ClearARSSearchData());
                /** Code to clear Validation Properties in Store */
                dispatch(ClearTaxonomyOldValues());
                dispatch(HandleMenuAction({ currentPage: localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE, currentModule: "assignment" }));
            }else{
                IntertekToaster(localConstant.errorMessages.ASSIGNMENT_POST_ERROR, 'dangerToast AssActCreateAssPostErr'); 
                dispatch(HideLoader());
            }
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
            dispatch(HideLoader());
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Assignment"));
            dispatch(HideLoader());
        }
    }
    // dispatch(HideLoader());
};

/**Update Assignment Details */
export const UpdateAssignmentDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const assignmentId = getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;
    const putUrl = StringFormat(assignmentAPIConfig.assignmentDetail, assignmentId);
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_UPDATE_ERROR, 'dangerToast AssActUpdtAssDetUpdtErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            // dispatch(FetchAssignmentDetail(assignmentId));
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code === "1") {
            dispatch(SuccessAlert(response.result, "Assignment"));
            dispatch(FetchAssignmentDetail(assignmentId,false,false));
            /** Code for ARS Search data clear after save */
            dispatch(ClearARSSearchData());
             /** Code to clear Validation Properties in Store */
             dispatch(ClearTaxonomyOldValues());
            // dispatch(HandleMenuAction({ currentPage: localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE }));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
            dispatch(HideLoader());
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Assignment"));
            dispatch(HideLoader());
        }
    }
    // dispatch(HideLoader());
};
 
/**
 * Cancel Create assignment Details
 */
export const CancelCreateAssignmentDetails = (supplierPoInfo) => (dispatch, getstate) => {
    const isFetchLookValues = true;  
    dispatch(FetchSupplierContacts(supplierPoInfo.supplierPOMainSupplierId));
    dispatch(FetchSubsuppliers());
    dispatch(FetchSubsupplierContacts());
    dispatch(isInterCompanyDiscountChanged(false));
    dispatch(FetchProjectForAssignmentCreation(supplierPoInfo,isFetchLookValues));
    dispatch(FetchContractScheduleName()); 
    dispatch(FetchReferencetypes());
    dispatch(FetchPaySchedule());
    dispatch(ClearARSSearchData());
};

/**
 * Cancel Edit assignment Details
 */
export const CancelEditAssignmentDetails = (isCopyAssignment) => async (dispatch, getstate) => {
    const documentData = getstate().rootAssignmentReducer.assignmentDetail.AssignmentDocuments;
    const assignmentId = getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;
    const supplierId = getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentSupplierId;
    const deleteUrl = assignmentAPIConfig.assignmentDocuments + assignmentId;
    if (!isEmpty(documentData)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    } 
    dispatch(FetchSupplierContacts(supplierId));
    dispatch(FetchSubsuppliers());
    dispatch(FetchSubsupplierContacts());   
    dispatch(ClearTaxonomyOldValues());
    dispatch(isInterCompanyDiscountChanged(false));
    dispatch(ClearARSSearchData());
    dispatch(FetchContractScheduleName()); 
    dispatch(FetchPaySchedule());
    dispatch(FetchGeneralDetails(getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo, true));
    dispatch(FetchReferencetypes());
    dispatch(FetchProjectForAssignmentEdit(getstate().rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentProjectNumber));
   return await dispatch(FetchAssignmentDetail(assignmentId,isCopyAssignment,false));
};

/**
 * clear taxonomy, lifecycle and supplier po old values 
 */
export const ClearTaxonomyOldValues = () => (dispatch) => {
    dispatch(actions.ClearTaxonomyOldValues());
};

/**
 * Delete existing Assignment
 */
export const DeleteAssignment = (assignmentId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (!assignmentId )
        assignmentId = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId;

    if (!assignmentId) {
        IntertekToaster("Failed to Delete Assignment", 'dangerToast FetchBusinessUnit');
    }

    const assignmentDetail = state.rootAssignmentReducer.assignmentDetail;
    assignmentDetail.AssignmentInfo.recordStatus = "D";
    const deleteUrl = StringFormat(assignmentAPIConfig.assignmentDetail, assignmentId);
    const params = {
        data: assignmentDetail,
    };
    const requestPayload = new RequestPayload(params);
    const response = await DeleteData(deleteUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.PROJECT_DELETE_ERROR, 'dangerToast ProjActDelDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code == 1) {
            dispatch(HideLoader());
            return response;
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
        }
        else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
        }
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
    }
    dispatch(HideLoader());
};

export const copyAssignment = ()=>(dispatch,getState)=>{
    const state = getState();
    const AssignmentInterCompanyDiscounts = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInterCompanyDiscounts, "object");
    const AssignmentInstructions = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInstructions,"object");
    const assignmentData={};
    const data={};
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    const assignmentValidationData = isEmptyReturnDefault(state.rootAssignmentReducer.dataToValidateAssignment,'object');
    assignmentData.AssignmentInfo = { ...state.rootAssignmentReducer.assignmentDetail.AssignmentInfo , ...{ 
        "assignmentId": null,
        "assignmentNumber":null,
        "assignmentCreatedDate":null,
        "assignmentStatus": "P",
        "isAssignmentCompleted" : false,
        "assignmentBudgetValue": "00.00",
        "assignmentBudgetCurrency": null,
        "assignmentBudgetHours": "00.00",
        "assignmentBudgetWarning": assignmentValidationData.projectBudgetWarning,
        "assignmentBudgetHoursWarning": assignmentValidationData.projectBudgetHoursWarning,
        "assignmentInvoicedToDate": 0,
		"assignmentUninvoicedToDate": 0,
		"assignmentHoursInvoicedToDate": 0,
		"assignmentHoursUninvoicedToDate": 0,
        "recordStatus": "N",
        "assignmentReference":null,
        "assignmentLifecycle":null,
        "assignmentReviewAndModerationProcess":null,
        "assignmentSupplierPurchaseOrderId": null,
        "assignmentSupplierPurchaseOrderNumber": null,
        "assignmentSupplierId": null,
        "assignmentSupplierName": null,
        "resourceSearchId":null,
        "arsTaskType":null,
       "visitFromDate":null,
       "visitToDate":null,
       "visitStatus":isVisit(workflowType)?'T':null,
       "timesheetStatus":isTimesheet(workflowType)?'N':null,
       "timesheetFromDate":null,
       "timesheetToDate":null,
        "assignmentSupplierPoMaterial": null,
        "assignmentLastVisitDate": null,
        "assignmentFirstVisitDate": null,
        "assignmentExpectedCompleteDate":null,
        "assignmentPercentageCompleted":null,
        "clientReportingRequirements": assignmentValidationData.projectClientReportingRequirements,
        "updateCount": null,
        "lastModification": null,
        "modifiedBy": null,
        "eventId": null,
        "actionByUser": null,
        "userTypes": null,
        "isFirstVisit":false,
        "assignmentOperatingCompanyCode": assignmentInfo.isOperatingCompanyActive ? assignmentInfo.assignmentOperatingCompanyCode : null,
        "assignmentOperatingCompany": assignmentInfo.isOperatingCompanyActive ? assignmentInfo.assignmentOperatingCompany : null,
        "isCopyAssignment" : true
        }
    };
    const pCHDiscount = AssignmentInterCompanyDiscounts.parentContractHoldingCompanyDiscount;
    const oCDiscount = pCHDiscount ? isNaN(parseFloat(pCHDiscount)) ? parseFloat(100).toFixed(2) : parseFloat(100).toFixed(2) - parseFloat(pCHDiscount).toFixed(2):parseFloat(100).toFixed(2);
    assignmentData.AssignmentInterCompanyDiscounts = {        
        "assignmentOperatingCompanyCode": AssignmentInterCompanyDiscounts.assignmentOperatingCompanyCode,
        "assignmentOperatingCompanyName": AssignmentInterCompanyDiscounts.assignmentOperatingCompany,
        "assignmentContractHoldingCompanyName": AssignmentInterCompanyDiscounts.assignmentContractHoldingCompanyName,
        "assignmentContractHoldingCompanyCode": AssignmentInterCompanyDiscounts.assignmentContractHoldingCompanyCode,
        "assignmentHostcompanyName":  AssignmentInterCompanyDiscounts.assignmentOperatingCompany,
        "assignmentHostcompanyCode": AssignmentInterCompanyDiscounts.assignmentOperatingCompanyCode,
        "parentContractHoldingCompanyName": AssignmentInterCompanyDiscounts.parentContractHoldingCompanyName,
        "parentContractHoldingCompanyCode": AssignmentInterCompanyDiscounts.parentContractHoldingCompanyCode,
        // "assignmentAdditionalIntercompany1_Name": AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany1_Name,
        // "assignmentAdditionalIntercompany1_Code": AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany1_Code,
        // "assignmentAdditionalIntercompany2_Name": AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany2_Name,
        // "assignmentAdditionalIntercompany2_Code": AssignmentInterCompanyDiscounts.assignmentAdditionalIntercompany2_Code,
        "assignmentAdditionalIntercompany1_Name": null,
        "assignmentAdditionalIntercompany1_Code": null,
        "assignmentAdditionalIntercompany2_Name": null,
        "assignmentAdditionalIntercompany2_Code": null,
        "recordStatus": "N",
        "assignmentId":null,
        "parentContractHoldingCompanyDiscount": AssignmentInterCompanyDiscounts.parentContractHoldingCompanyDiscount,
        "parentContractHoldingCompanyDescription": AssignmentInterCompanyDiscounts.parentContractHoldingCompanyDescription,
        "assignmentContractHoldingCompanyDiscount": null,
        "assignmentContractHoldingCompanyDescription": null,
        "assignmentOperatingCompanyDiscount": oCDiscount,
        "assignmentAdditionalIntercompany1_Discount": null,
        "assignmentAdditionalIntercompany1_Description": null,
        "assignmentAdditionalIntercompany2_Discount": null,
        "assignmentAdditionAlIntercompany2_Description": null,
        "assignmentHostcompanyDiscount": null,
        "assignmentHostcompanyDescription": null
    };
    //Assignment Instruction of Original Assignment will get copied.
    if(AssignmentInstructions && (!required(AssignmentInstructions.interCompanyInstructions) || !required(AssignmentInstructions.technicalSpecialistInstructions))){
        assignmentData.AssignmentInstructions = {
            "interCompanyInstructions": AssignmentInstructions.interCompanyInstructions,
            "technicalSpecialistInstructions": AssignmentInstructions.technicalSpecialistInstructions,
            "assignmentId":null,
            "recordStatus": "N"
        };
    }
    assignmentData.AssignmentSubSuppliers = null;
    assignmentData.AssignmentTaxonomy=null;
    assignmentData.AssignmentContractSchedules = null;
    assignmentData.AssignmentReferences = null;  
    assignmentData.AssignmentTechnicalSpecialists = null;
    assignmentData.AssignmentTechnicalSpecialistChargeRate = null;
    assignmentData.AssignmentAdditionalExpenses = null;
    assignmentData.AssignmentNotes= null;
    assignmentData.AssignmentDocuments= null;
    assignmentData.AssignmentContributionCalculators = null;
    dispatch(actions.FetchAssignmentProjectInfo({ assignmentData, assignmentValidationData }));
    dispatch(actions.UpdateCopyAssignmentInitialState(assignmentData)); // D - 631
    if (isTimesheet(workflowType)) {
        const assignmentInfo = assignmentData.AssignmentInfo;
        if (!isEmptyOrUndefine(assignmentInfo)
            && !isEmptyOrUndefine(assignmentInfo.workLocationCountry)) {
            dispatch(FetchState(assignmentInfo.workLocationCountry));
        }
        if (!isEmptyOrUndefine(assignmentInfo)
            && !isEmptyOrUndefine(assignmentInfo.workLocationCounty)) {
            dispatch(FetchCity(assignmentInfo.workLocationCounty));
        }
    }
    dispatch(FetchSupplierContacts());
    dispatch(FetchSubsuppliers());
    dispatch(FetchSubsupplierContacts());
    // dispatch(FetchProjectAssignmentReferenceTypes(true));
    dispatch(CheckAddAssignmentReference(true));
    dispatch(UpdateCurrentPage(localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE));
    dispatch(UpdateCurrentModule('assignment'));
};

/** Clear ARS Related datas */
export const ClearARSSearchData = () => (dispatch,getstate)=>{
    dispatch(ClearMyTasksData());
    dispatch(clearPreAssignmentDetails());
    dispatch(clearARSSearchDetails());
    dispatch(SetDefaultPreAssignmentName());
    //dispatch(FetchTechSpecMytaskData());
};

/** Revert copy assignment changes */ // D - 631
export const cancelCopyAssignment = () => (dispatch,getstate) => {
    const state = getstate();
    const copyAssignmentData = Object.assign({},state.rootAssignmentReducer.copyAssignmentInitialState);
    const assignmentInfo = copyAssignmentData.AssignmentInfo;
    dispatch(actions.FetchAssignmentProjectInfo({ assignmentData : copyAssignmentData }));
    //const assignmentReferenceType = isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentReferenceTypes);
    // if(assignmentReferenceType && assignmentReferenceType.length > 0)
    //     dispatch(AddProjAssignmentReference(assignmentReferenceType));
    // else
    //     dispatch(FetchProjectAssignmentReferenceTypes(true));
    dispatch(handleAssignedSpecialistTaxonomyLOV({}));
    dispatch(CheckAddAssignmentReference(true));
    dispatch(FetchSupplierContacts());
    dispatch(FetchSubsuppliers());
    dispatch(FetchSubsupplierContacts());
    dispatch(isInterCompanyDiscountChanged(false));
    if (!isEmptyOrUndefine(assignmentInfo) 
    && !isEmptyOrUndefine(assignmentInfo.workLocationCountry)){
        dispatch(FetchState(assignmentInfo.workLocationCountry));
    }
    if (!isEmptyOrUndefine(assignmentInfo) 
    && !isEmptyOrUndefine(assignmentInfo.workLocationCounty)) {
        dispatch(FetchCity(assignmentInfo.workLocationCounty));
    }
};

/**
 * This action get call on assignment module get unmounts.
 * Call respective clearing actions to make the store to initial state.
 */
export const OnCancelUploadDocument = () => async (dispatch,getstate) => {
    const state = getstate();
    const AssignmentInfo = Object.assign({},state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const documentData = getstate().rootAssignmentReducer.assignmentDetail.AssignmentDocuments;
    const assignmentId = AssignmentInfo.assignmentId;
    const deleteUrl = assignmentAPIConfig.assignmentDocuments + assignmentId;
    if (!isEmpty(documentData) && !required(assignmentId)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    }    
};
export const OnAssignmentUnmount = () => async (dispatch,getstate) => {
    dispatch(IsARSMasterDataLoaded(false));
    dispatch(isInterCompanyDiscountChanged(false));
    dispatch(ClearSubCategory());
    dispatch(ClearServices());
};

const CheckAddAssignmentReference = (isAddAssignment) => (dispatch,getstate) => {
    const state = getstate();
    const assignmentReferenceType = isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentReferenceTypes);
    if(assignmentReferenceType && assignmentReferenceType.length > 0)
        dispatch(AddProjAssignmentReference(assignmentReferenceType));
    else
        dispatch(FetchProjectAssignmentReferenceTypes(isAddAssignment));
};

const arraymove = (arr, fromIndex, toIndex) => {
    const element = arr[fromIndex];
    arr.splice(fromIndex, 1);
    arr.splice(toIndex, 0, element);
    return arr;
};

/** Fetch Assignment Type action */
export const FetchReviewAndModerationProcess = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootAssignmentReducer.reviewAndModerationProcess)) {
        return;
    }
    const assignmentReviewAndModerationProcessURL = assignmentAPIConfig.assignmentReviewAndModerationProcess;
    const isActive=true;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .`
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentReviewAndModerationProcessURL, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_REVIEW_AND_MODERATION_PROCESS, 'dangerToast assActassignTypeErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            arraymove(response.result,1,0);
            dispatch(actions.FetchReviewAndModerationProcess(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assActassignTypeErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_REVIEW_AND_MODERATION_PROCESS, 'dangerToast assActassignTypeErrMsg1');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg1');
    }
};

// Clear IC Dependent data on change of Operating Company
export const ClearICDependentData = () => (dispatch) => {
    dispatch(ClearAssignmentRateSchedule());
    dispatch(ClearAssignedSpecialist());
    dispatch(ClearAdditionalExpenses());
    dispatch(ClearContractHoldingDiscounts(null));
};

export const AssignmentSaveButtonDisable = (data) => (dispatch) => {
    dispatch(actions.AssignmentSaveButtonDisable(data));
};
