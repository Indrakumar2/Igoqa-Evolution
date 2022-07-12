import { FetchData, PostData, CreateData, DeleteData } from '../../services/api/baseApiService';
import {
    contractAPIConfig,
    projectAPIConfig,
    RequestPayload
} from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {
    projectActionTypes,
    contractActionTypes
} from '../../constants/actionTypes';
import { FetchCustomerContact } from '../project/clientNotificationAction';
import { ShowLoader, HideLoader ,SetCurrentPageMode,UpdateInteractionMode,UpdateCurrentPage,UpdateCurrentModule } from '../../common/commonAction';
import { getlocalizeData, isEmpty, formatToDecimal,parseValdiationMessage,FilterSave, isUndefined,getNestedObject } from '../../utils/commonUtils';
import { SuccessAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import {
    FetchProjectCompanyDivision,
    FetchProjectCompanyOffices,
    FetchProjectMICoordinator,
    FetchProjectCompanyCostCenter
} from './generalDetailsAction';
import { EditProject } from '../../components/sideMenu/sideMenuAction';
import moment from 'moment';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
import { UpdateSelectedCompany } from '../../components/appLayout/appLayoutActions';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';
import { isNullOrUndefined } from 'util';
import { FetchCustomerDocumentsofProject } from './documentAction';
import { FetchProjectDocumentsofContracts } from '../contracts/documentAction';

const localConstant = getlocalizeData();

const actions = {
    FetchProjectContractInfo: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_CONTRACT_INFO,
        data: payload
    }),
    FetchProjectDetailSuccess: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_DETAIL_SUCCESS,
        data: payload
    }),
    FetchProjectDetailError: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_DETAIL_ERROR,
        data: payload
    }),
    SaveSelectedProjectNumber: (payload) => ({
        type: projectActionTypes.SAVE_SELECTED_PROJECT_NUMBER,
        data: payload
    }),
    UpdateBtnEnableStatus: (payload) => ({
        type: projectActionTypes.UPDATE_BTN_ENABLE_STATUS,
        data: payload
    }),
    // UpdateInteractionMode: (payload) => ({
    //     type: projectActionTypes.UPDATE_INTERACTION_MODE_PROJECT,
    //     data: payload
    // }),
    GetSelectedCustomerName: (payload) => ({
        type: contractActionTypes.CONTRACT_SELECTED_CUSTOMER_CODE,
        data: payload
    }),
    UpdateProjectMode: (payload) => ({
        type: projectActionTypes.UPDATE_PROJECT_MODE,
        data: payload
    })
};

export const FetchContractForProjectCreation = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    //const contractNumber = getstate().RootContractReducer.ContractDetailReducer.selectedCustomerData.contractNumber;
    const contractNumber = data.contractNumber;
    const fetchUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;
    const params = {
        'contractNumber': decodeURIComponent(contractNumber),
        'IsInvoiceDetailRequired': true
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_CONTRACT_FOR_PROJECT_CREATION, 'dangerToast conActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        const contractInfo=Object.assign({},response.ContractInfo);
        dispatch(FetchProjectContractInfo(response));
        dispatch(actions.GetSelectedCustomerName(contractInfo));  
        dispatch(FetchProjectCompanyDivision(contractInfo.contractHoldingCompanyCode));
        dispatch(FetchProjectCompanyOffices(contractInfo.contractHoldingCompanyCode));
        // dispatch(FetchProjectCoordinator());
        const miCompanyCodes = [ contractInfo.contractHoldingCompanyCode ];
        dispatch(FetchProjectMICoordinator(miCompanyCodes));
        dispatch(actions.UpdateBtnEnableStatus());
        dispatch(HideLoader());
        dispatch(SetCurrentPageMode(null));
        dispatch(UpdateCurrentPage(localConstant.sideMenu.CREATE_PROJECT));
        dispatch(UpdateCurrentModule(localConstant.moduleName.PROJECT));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_CONTRACT_FOR_PROJECT_CREATION, 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
    }
};

export const FetchContractForProjectEdit = (data) => async (dispatch, getstate) => {
    const apiUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contractSearch;
    const params = {
        'contractNumber': data,
        'IsInvoiceDetailRequired': true
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_CONTRACT_FOR_PROJECT_CREATION, 'dangerToast conActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if(response.result && Array.isArray(response.result) && response.result.length > 0)
            dispatch(actions.GetSelectedCustomerName(response.result[0]));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_CONTRACT_FOR_PROJECT_CREATION, 'dangerToast conActDataSomethingWrong');
    }
};
export const FetchProjectContractInfo = (data) => (dispatch, getstate) => {
    dispatch(actions.FetchProjectContractInfo({}));
    const projectData = Object.assign({}, getstate().RootProjectReducer.ProjectDetailReducer.projectDetail);
    if (data && data.ContractInfo) {
        projectData["ProjectInfo"] = {};
        projectData["ProjectInvoiceAttachments"] = [];
        projectData["ProjectInvoiceReferences"] = [];
        projectData["ProjectDocuments"] = null;
        projectData["ProjectNotes"] = null;
        projectData["ProjectNotifications"] = null;
        projectData.ProjectInfo.projectStatus = "O";
        projectData.ProjectInfo.projectStartDate = '';
        projectData.ProjectInfo.projectBudgetHoursUnit = '00.00';
        projectData.ProjectInfo.projectClientReportingRequirement = data.ContractInfo.contractClientReportingRequirement;
        projectData.ProjectInfo.projectAssignmentOperationNotes = data.ContractInfo.contractOperationalNote;
        projectData.ProjectInfo.projectBudgetWarning =data.ContractInfo.contractBudgetMonetaryWarning;
        projectData.ProjectInfo.projectBudgetHoursWarning =data.ContractInfo.contractBudgetHoursWarning;
        projectData.ProjectInfo.projectBudgetValue='00.00';
        projectData.ProjectInfo.projectInvoicePaymentTerms = data.ContractInfo.contractInvoicePaymentTerms;
        projectData.ProjectInfo.projectCustomerContact = data.ContractInfo.contractCustomerContact;
        projectData.ProjectInfo.projectCustomerContactAddress = data.ContractInfo.contractCustomerContactAddress;
        projectData.ProjectInfo.projectCustomerInvoiceContact = data.ContractInfo.contractCustomerInvoiceContact;
        projectData.ProjectInfo.projectCustomerInvoiceAddress = data.ContractInfo.contractCustomerInvoiceAddress;
        projectData.ProjectInfo.projectInvoiceRemittanceIdentifier = data.ContractInfo.contractInvoiceRemittanceIdentifier;
        projectData.ProjectInfo.projectBudgetCurrency = data.ContractInfo.contractInvoicingCurrency;
        projectData.ProjectInfo.projectInvoicingCurrency = data.ContractInfo.contractInvoicingCurrency;
        projectData.ProjectInfo.projectInvoiceGrouping = data.ContractInfo.contractInvoiceGrouping;
        projectData.ProjectInfo.projectInvoiceInstructionNotes = data.ContractInfo.contractInvoiceInstructionNotes;
        projectData.ProjectInfo.projectInvoiceFreeText = data.ContractInfo.contractInvoiceFreeText;
        projectData.ProjectInfo.contractNumber = data.ContractInfo.contractNumber;
        projectData.ProjectInfo.customerContractNumber = data.ContractInfo.customerContractNumber;
        projectData.ProjectInfo.contractHoldingCompanyCode = data.ContractInfo.contractHoldingCompanyCode;
        projectData.ProjectInfo.contractHoldingCompanyName = data.ContractInfo.contractHoldingCompanyName;
        projectData.ProjectInfo.contractCustomerCode = data.ContractInfo.contractCustomerCode;
        projectData.ProjectInfo.contractCustomerName = data.ContractInfo.contractCustomerName;
        projectData.ProjectInfo.contractType = data.ContractInfo.contractType;
        projectData.ProjectInfo.recordStatus = "N";
        projectData.ProjectInfo.assignmentParentContractCompanyCode=data.ContractInfo.parentCompanyCode;  //Added for Sanity Defect 86
        if (data.ContractInvoiceAttachments) {
            let obj = {};
            data.ContractInvoiceAttachments.map(attachment => {
                obj.projectInvoiceAttachmentId = Math.floor(Math.random() * (Math.pow(10, 5)));
                obj.documentType = attachment.documentType;
                obj.recordStatus = "N";
                obj.modifiedBy = attachment.modifiedBy;
                projectData.ProjectInvoiceAttachments.push(obj);
                obj = {};
            });
        }
        if (data.ContractInvoiceReferences) {
            let obj = {};
            data.ContractInvoiceReferences.map(ref => {
                obj.projectInvoiceReferenceTypeId = Math.floor(Math.random() * (Math.pow(10, 5)));
                obj.referenceType = ref.referenceType;
                obj.displayOrder = ref.displayOrder;
                obj.isVisibleToAssignment = ref.isVisibleToAssignment;
                obj.isVisibleToVisit = ref.isVisibleToVisit;
                obj.isVisibleToTimesheet = ref.isVisibleToTimesheet;
                obj.recordStatus = "N";
                obj.modifiedBy = ref.modifiedBy;
                projectData.ProjectInvoiceReferences.push(obj);
                obj = {};
            });
        }

        // ITK D-590 - Starts
        if(data.ContractInfo.isChildContract && !isEmpty(data.ContractInfo.parentContractInvoicingDetails) && data.ContractInfo.parentContractInvoicingDetails.isParentContractInvoiceUsed){
            projectData.ProjectInfo.projectSalesTax = data.ContractInfo.parentContractInvoicingDetails.parentContractSalesTax;
            projectData.ProjectInfo.projectWithHoldingTax = data.ContractInfo.parentContractInvoicingDetails.parentContractWithHoldingTax;
            projectData.ProjectInfo.projectInvoiceRemittanceIdentifier = data.ContractInfo.parentContractInvoicingDetails.parentContractInvoiceRemittanceIdentifier;
            projectData.ProjectInfo.projectInvoiceFooterIdentifier = data.ContractInfo.parentContractInvoicingDetails.parentContractInvoiceFooterIdentifier; 
            projectData.ProjectInfo.parentCompanyCode = data.ContractInfo.parentCompanyCode;
        }
        else if(data.ContractInfo.isChildContract && data.ContractInfo.contractHoldingCompanyCode !== data.ContractInfo.parentCompanyCode){ //Added for D-1164
            projectData.ProjectInfo.projectSalesTax = '';
            projectData.ProjectInfo.projectWithHoldingTax = '';
            projectData.ProjectInfo.projectInvoiceRemittanceIdentifier = '';
            projectData.ProjectInfo.projectInvoiceFooterIdentifier = ''; 
        }//Added for D-1164-End
        else {
            projectData.ProjectInfo.projectSalesTax = data.ContractInfo.contractSalesTax;
            projectData.ProjectInfo.projectWithHoldingTax = data.ContractInfo.contractWithHoldingTax;
            projectData.ProjectInfo.projectInvoiceRemittanceIdentifier = data.ContractInfo.contractInvoiceRemittanceIdentifier;
            projectData.ProjectInfo.projectInvoiceFooterIdentifier = data.ContractInfo.contractInvoiceFooterIdentifier; 
        }
        // ITK D-590 - Ends

        dispatch(actions.FetchProjectContractInfo(projectData));

        // const selectedCompany = getstate().appLayoutReducer.selectedCompany;
        // if(projectData.ProjectInfo.contractHoldingCompanyCode === selectedCompany){
        //     dispatch(actions.UpdateInteractionMode(false));
        // }
        // else{
        //     dispatch(actions.UpdateInteractionMode(true));
        // }
        dispatch(UpdateInteractionMode(projectData.ProjectInfo.contractHoldingCompanyCode));
    }
};

export const SaveSelectedProjectNumber = (projectNumber) => (dispatch) => {
    dispatch(actions.SaveSelectedProjectNumber(projectNumber));
};

/**
* Fetch project detail
*/
export const FetchProjectDetail = (data, isFetchLookUpValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchProjectDetailSuccess({}));
    let selectedCompany = getstate().appLayoutReducer.selectedCompany;
    if(data.selectedCompany){
        selectedCompany = data.selectedCompany;
        dispatch(UpdateSelectedCompany({ companyCode:selectedCompany }));
    }
    if (!data.projectNumber) {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROJECT_DETAIL, 'dangerToast FetchBusinessUnit');
    }
    dispatch(actions.SaveSelectedProjectNumber(data.projectNumber));
    const businessUnitUrl = `/${ projectAPIConfig.projectSearch }/${ data.projectNumber }${ projectAPIConfig.detail }`; ///projects/{projectNumber}/detail
    const params = { 'IsInvoiceDetailRequired': true };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(businessUnitUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(actions.FetchProjectDetailError(error));
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROJECT_DETAIL, 'dangerToast FetchBusinessUnit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });

        //646 Fixes
        if (!isEmpty(response) && isEmpty(response.ProjectInfo)){
            dispatch(HideLoader());
            dispatch(ChangeDataAvailableStatus(true));
            return response;
        }

    if (!isEmpty(response) && !isEmpty(response.ProjectInfo)) {

        response.ProjectInfo.projectBudgetValue = formatToDecimal(response.ProjectInfo.projectBudgetValue,2);
        response.ProjectInfo.projectBudgetHoursUnit = formatToDecimal(response.ProjectInfo.projectBudgetHoursUnit,2);
        response.ProjectInfo.projectInvoicedToDate =formatToDecimal(response.ProjectInfo.projectInvoicedToDate,2);
        response.ProjectInfo.projectUninvoicedToDate = formatToDecimal(response.ProjectInfo.projectUninvoicedToDate,2);
        response.ProjectInfo.projectRemainingBudgetValue = formatToDecimal(response.ProjectInfo.projectRemainingBudgetValue,2);
        response.ProjectInfo.projectHoursInvoicedToDate = formatToDecimal(response.ProjectInfo.projectHoursInvoicedToDate,2);
        response.ProjectInfo.projectHoursUninvoicedToDate = formatToDecimal(response.ProjectInfo.projectHoursUninvoicedToDate,2);
        response.ProjectInfo.projectRemainingBudgetHours = formatToDecimal(response.ProjectInfo.projectRemainingBudgetHours,2);
        
        dispatch(actions.FetchProjectDetailSuccess(response));
        if(isFetchLookUpValues === undefined || isFetchLookUpValues === true){
            dispatch(FetchContractForProjectEdit(response.ProjectInfo.contractNumber));
            dispatch(FetchProjectCompanyDivision(response.ProjectInfo.contractHoldingCompanyCode));
            dispatch(FetchProjectCompanyOffices(response.ProjectInfo.contractHoldingCompanyCode));
            // dispatch(FetchProjectCoordinator());
            const miCompanyCodes = [ response.ProjectInfo.contractHoldingCompanyCode ];
            dispatch(FetchProjectMICoordinator(miCompanyCodes));
            dispatch(FetchProjectCompanyCostCenter(response.ProjectInfo.companyDivision));
            dispatch(FetchCustomerContact());
        }
        // if(response.ProjectInfo.contractHoldingCompanyCode === selectedCompany){
        //     dispatch(actions.UpdateInteractionMode(false));
        //     dispatch(SetCurrentPageMode());
        // }
        // else{
        //     dispatch(actions.UpdateInteractionMode(true));
        //     dispatch(SetCurrentPageMode("View"));
        // }
        dispatch(UpdateInteractionMode(response.ProjectInfo.contractHoldingCompanyCode,false,!response.ProjectInfo.isContractHoldingCompanyActive)); //D-1290
        dispatch(HideLoader());
        //  redirection for home page on the basis(A project is Fetched for updating or Viewing the data)
       // if (getstate().RootProjectReducer.ProjectDetailReducer.projectMode === "") {
        dispatch(UpdateCurrentPage(localConstant.sideMenu.EDIT_VIEW_PROJECT));
        dispatch(UpdateCurrentModule(localConstant.moduleName.PROJECT));
        dispatch(UpdateProjectMode(localConstant.project.EDITPROJECT));
        //};
    }
    else{
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROJECT_DETAIL, 'dangerToast FetchProjectDetailFail');
        dispatch(HideLoader());
    }
};
/**
 * Cancel Create Project Details
 */
export const CancelCreateProjectDetails = (result) => (dispatch, getstate) => {
    dispatch(FetchContractForProjectCreation(result));
    dispatch(FetchCustomerDocumentsofProject());
    dispatch(FetchProjectDocumentsofContracts());
};

export const ClearDocumentDetails = () => async (dispatch, getstate) => {
    const state = getstate();
    const ProjectInfo = Object.assign({},state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo); 
    const documentData = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments;
    const selectedProjectNo = ProjectInfo.projectNumber;
    const deleteUrl = projectAPIConfig.baseUrl + projectAPIConfig.projectDocuments + selectedProjectNo;
    let response = null;
    if (!isEmpty(documentData)&& !isUndefined(selectedProjectNo)) {
        response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response;
};
/**
 * Cancel Edit Project Details
 */
export const CancelEditProjectDetails = () => async (dispatch, getstate) => {
    const selectedProjectNo = getstate().RootProjectReducer.ProjectDetailReducer.selectedProjectNo;
    const documentData = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments;
    const deleteUrl = projectAPIConfig.baseUrl + projectAPIConfig.projectDocuments + selectedProjectNo;
    if (!isEmpty(documentData)) {
        const res = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }    
    dispatch(FetchProjectDetail({ projectNumber:selectedProjectNo }, true));
    dispatch(FetchCustomerDocumentsofProject());
    dispatch(FetchProjectDocumentsofContracts());
};
/**
 * Save Project Detail
 */
export const SaveProjectDetails = () => (dispatch, getstate) => {
    const projectData = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail;
    const projectMode = getstate().RootProjectReducer.ProjectDetailReducer.projectMode;

    if (projectData.ProjectInvoiceAttachments) {
        projectData.ProjectInvoiceAttachments.forEach(row => {
            if (row.recordStatus == "N") {
                row.projectInvoiceAttachmentId = 0;
            }
        });
    }
    if (projectData.ProjectInvoiceReferences) {
        projectData.ProjectInvoiceReferences.forEach((row,index) => {
            if (row.recordStatus == "N") {
                row.projectInvoiceReferenceTypeId = 0;
            }
            // if (row.displayOrder - 1 !== index) { //Changes for D1015
            //        if (row.recordStatus !== "N" && row.recordStatus !== "D") {
            //            row.recordStatus = "M";
            //        }
            //    }
        });
    }
    if (projectData.ProjectNotifications) {
        const CustomerDirectReportingEmail =[];
        projectData.ProjectNotifications.forEach(row => {
            if (row.recordStatus == "N") {
                row.projectClientNotificationId = 0;
            }
            const customerContact =getstate().RootProjectReducer.ProjectDetailReducer.customerContact;
            const projectCustomerContact = customerContact && customerContact.filter(x=>x.contactPersonName === row.customerContact);
            if(row.isSendCustomerDirectReportingNotification ===true && projectCustomerContact[0] && !isEmpty(projectCustomerContact[0].email))
                CustomerDirectReportingEmail.push(projectCustomerContact[0].email);
        });
        const emailAddress = CustomerDirectReportingEmail.join(';');
        projectData.ProjectInfo.customerDirectReportingEmailAddress=emailAddress;
    };
    if (projectData.ProjectDocuments) {
        projectData.ProjectDocuments.forEach(row => {
            if (!isEmpty(row.status)) {
                row.status = row.status.trim();
            }
            if (row.recordStatus == "N") {
                row.id = 0;
            }
        });
    };
    if (projectData.ProjectNotes) {
        projectData.ProjectNotes.forEach(row => {
            if (row.recordStatus == "N") {
                row.projectNoteId = 0;
            }
        });
    };
    if (!isEmpty(projectData.ProjectInfo.projectStartDate)) {
        projectData.ProjectInfo.projectStartDate = moment(projectData.ProjectInfo.projectStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    if (!isEmpty(projectData.ProjectInfo.projectEndDate)) {
        projectData.ProjectInfo.projectEndDate = moment(projectData.ProjectInfo.projectEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }
    if (!isNullOrUndefined(projectData.ProjectInfo.assignmentParentContractDiscount)) {
        projectData.ProjectInfo.assignmentParentContractDiscount = parseFloat(projectData.ProjectInfo.assignmentParentContractDiscount).toFixed(2);
    }
    if (projectMode === "createProject") {
        dispatch(CreateProjectDetails(projectData));
    }
    if (projectMode === "editProject") {
        projectData.ProjectInfo.recordStatus = "M";
        const filterdProjectData = FilterSave(projectData);
        dispatch(UpdateProjectDetails(filterdProjectData));
    }

};

/**
 * Create New Project
 */
export const CreateProjectDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const postUrl = projectAPIConfig.projectSearch + '/0' + projectAPIConfig.detail;
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast ProjActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code == "1" && !isEmpty(response.result)) {
            dispatch(actions.SaveSelectedProjectNumber(response.result.projectNumber));
            dispatch(CreateAlert(response.result, "Project"));
            dispatch(FetchProjectDetail(response.result,false));
            dispatch(EditProject({
                "currentModule":"project",
                 "currentPage":"editViewProject", //refer from locale constants
              }));
            dispatch(actions.UpdateBtnEnableStatus());
        }
        else if (response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast CreateProjectWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast CreateProjectError');
        }
    }
    dispatch(HideLoader());
};

/**
 * Update the exisiting project details 
 */
export const UpdateProjectDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const projectNo = getstate().RootProjectReducer.ProjectDetailReducer.selectedProjectNo;
    const putUrl = projectAPIConfig.projectSearch + '/' + projectNo + projectAPIConfig.detail;
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.PROJECT_UPDATE_ERROR, 'dangerToast ProjActUpdtDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code == 1) {
            dispatch(SuccessAlert(response.result, "Project"));
            if(response.result){
                dispatch(FetchProjectDetail(response.result,false));
            }
            dispatch(actions.UpdateBtnEnableStatus());
        }
        else if (response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast UpdateProjectWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.PROJECT_UPDATE_ERROR, 'dangerToast ProjActUpdtDataError');
        }
    }
    dispatch(HideLoader());
};

/**
 * Delete existing project
 */
export const DeleteProjectDetails = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const projectData = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail;
    projectData.ProjectInfo.recordStatus = "D";
    const projectNo = getstate().RootProjectReducer.ProjectDetailReducer.selectedProjectNo;
    const deleteUrl = projectAPIConfig.projectSearch + '/' + projectNo + projectAPIConfig.detail;
    const params = {
        data: projectData,
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
        else if (response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast DeleteProjectWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.PROJECT_DELETE_ERROR, 'dangerToast ProjectDeleteError');
        }
    }
    dispatch(HideLoader());
};

export const UpdateProjectMode = (payload) => (dispatch,getstate) =>{
    dispatch(actions.UpdateProjectMode(payload));
};