import { supplierPOActionTypes } from '../../constants/actionTypes';
import { supplierPOApiConfig, RequestPayload,projectAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData, DeleteData, PostData, CreateData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty,formatToDecimal,parseValdiationMessage,deepCopy,FilterSave, isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader,SetCurrentPageMode } from '../../common/commonAction';
import { SuccessAlert, ValidationAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import { StringFormat } from '../../utils/stringUtil';
import moment from 'moment';
import { RemoveDocumentsFromDB,UpdateCurrentModule,UpdateCurrentPage } from '../../common/commonAction';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';
import { FetchVisitDocOfSupplierPo } from '../../actions/supplierPO/supplierPODocumentAction';

const localConstant = getlocalizeData();

const actions = {
    FetchSupplierPoData: (payload) => ({
        type: supplierPOActionTypes.FETCH_SUPPLIER_PO_DATA,
        data: payload
    }),    
    ClearSupplierPOData: () => ({
        type: supplierPOActionTypes.CLEAR_SUPPLIER_PO_DATA,
    }),
    AddSubSupplier: (payload) => ({
        type: supplierPOActionTypes.ADD_SUB_SUPPLIER,
        data: payload
    }),
    UpdateSubSupplier: (payload) => ({
        type: supplierPOActionTypes.UPDATE_SUB_SUPPLIER,
        data: payload
    }),
    DeleteSubSupplier: (payload) => ({
        type: supplierPOActionTypes.DELETE_SUB_SUPPLIER,
        data: payload
    }),
    updateSupplierDetails: (payload) => ({
        type: supplierPOActionTypes.UPDATE_SUPPLIER_DETAILS,
        data: payload
    }),
    saveProjectDetailsForSupplier: (payload) => ({
        type:supplierPOActionTypes.SAVE_PROJECT_DETAILS_FOR_SUPPLIER,
        data: payload
    }),
    // Added For ITK D-456 -Starts
    SetSupplierPOViewMode: (payload) => ({
        type:supplierPOActionTypes.SUPPLIER_PO_VIEW_ACCESS,
        data: payload
    }),
    // Added For ITK D-456 End
    
};

/**
 * Fetch supplierPo data action.
 */
export const FetchSupplierPoData = (data) => async (dispatch, getstate) => { 
    const state = getstate();
    dispatch(ShowLoader());
    let res = null;
  //  dispatch(actions.FetchSupplierPoData({}));    
    if (data && data.supplierPOId) {
        const supplierUrl = StringFormat(supplierPOApiConfig.supplierPODetails, data.supplierPOId);
        const params = { 'IsInvoiceDetailRequired': true };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(supplierUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.SUPPLER_PO_FETCH_VALIDATION, 'warningToast supplierPoFetchVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {

            //646 Fixes
            if (isEmpty(response.SupplierPOInfo)) {
                dispatch(HideLoader());
                dispatch(ChangeDataAvailableStatus(true));
                return response;
            }

            if(response.SupplierPOInfo){
                response.SupplierPOInfo.supplierPOBudget = formatToDecimal(response.SupplierPOInfo.supplierPOBudget,2);
                response.SupplierPOInfo.supplierPOBudgetHours = formatToDecimal(response.SupplierPOInfo.supplierPOBudgetHours,2);
                response.SupplierPOInfo.supplierPOBudgetInvoicedToDate = formatToDecimal(response.SupplierPOInfo.supplierPOBudgetInvoicedToDate,2);
                response.SupplierPOInfo.supplierPOBudgetUninvoicedToDate = formatToDecimal(response.SupplierPOInfo.supplierPOBudgetUninvoicedToDate,2);
                response.SupplierPOInfo.supplierPORemainingBudgetValue = formatToDecimal(response.SupplierPOInfo.supplierPORemainingBudgetValue,2);
                response.SupplierPOInfo.supplierPOBudgetHoursInvoicedToDate = formatToDecimal(response.SupplierPOInfo.supplierPOBudgetHoursInvoicedToDate,2);
                response.SupplierPOInfo.supplierPOBudgetHoursUnInvoicedToDate = formatToDecimal(response.SupplierPOInfo.supplierPOBudgetHoursUnInvoicedToDate,2);
                response.SupplierPOInfo.supplierPORemainingBudgetHours = formatToDecimal(response.SupplierPOInfo.supplierPORemainingBudgetHours,2);
                response.SupplierPOInfo.oldSupplierPONumber=response.SupplierPOInfo.supplierPONumber; //added for checking duplicates of supplierPO
            }

            // dispatch(HandleMenuAction({ 
            //     currentPage: localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE,
            //     currentModule:'supplierpo'
            //  }));
            dispatch(UpdateCurrentModule(localConstant.moduleName.SUPPLIER_PO));
            dispatch(UpdateCurrentPage(localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE));
            res = await dispatch(actions.FetchSupplierPoData(response)); //Added for D932
            (response.SupplierPOInfo && response.SupplierPOInfo.isSupplierPOCompanyActive) ?
                dispatch(SetCurrentPageMode()) : 
                    dispatch(SetCurrentPageMode(localConstant.commonConstants.VIEW));
            //Added for D-479 issue 1
            // if(state.appLayoutReducer.selectedCompany !== response.SupplierPOInfo.supplierPOCompanyCode){
            //     dispatch(SetCurrentPageMode(localConstant.commonConstants.VIEW));
            // }
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierPoApiFailure');
        }
    }
    else {
        //Changes for D-149 and D-623 issue1
        const supplierPoInfoData = isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo,'object');
        if(supplierPoInfoData.supplierPOId){
            dispatch(UpdateCurrentModule(localConstant.moduleName.SUPPLIER_PO));
            dispatch(UpdateCurrentPage(localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE));
            supplierPoInfoData.isSupplierPOCompanyActive ?
                dispatch(SetCurrentPageMode()) :
                    dispatch(SetCurrentPageMode(localConstant.commonConstants.VIEW));
            // if(state.appLayoutReducer.selectedCompany !== state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo.supplierPOCompanyCode){
            //     dispatch(SetCurrentPageMode(localConstant.commonConstants.VIEW));
            // }
        } else{
            dispatch(UpdateCurrentModule(localConstant.moduleName.SUPPLIER_PO));
            dispatch(UpdateCurrentPage(localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE));
        }
    }
    dispatch(HideLoader());
    return res;
};

//clear Supplier PO
export const ClearSupplierPOData = () => async (dispatch) => {
    dispatch(actions.ClearSupplierPOData());
};

/**
 * Cancel Create Supplier PO Data
 */
export const CancelCreateSupplierPODetails = () => async (dispatch, getstate) => {    
    let storeSupplierPOData = {};
    let res = null;
    const supplierPOInfo = Object.assign({},getstate().rootSupplierPOReducer.supplierPOData.SupplierPOInfo);
    dispatch(actions.FetchSupplierPoData({}));
    storeSupplierPOData = {
        "supplierPOCustomerName": supplierPOInfo.supplierPOCustomerName,
        "supplierPOProjectNumber": supplierPOInfo.supplierPOProjectNumber,
        "supplierPOStatus": supplierPOInfo.supplierPOStatus,
        "supplierPOCustomerProjectName": supplierPOInfo.supplierPOCustomerProjectName,
        "supplierPOCustomerCode": supplierPOInfo.supplierPOCustomerCode,
        "supplierPOId": supplierPOInfo.supplierPOId,
        "supplierPOBudgetHoursWarning":supplierPOInfo.oldSupplierPOBudgetHoursWarning,
        "supplierPOBudgetWarning":supplierPOInfo.oldSupplierPOBudgetWarning,
        "oldSupplierPOBudgetHoursWarning":supplierPOInfo.oldSupplierPOBudgetHoursWarning,
        "oldSupplierPOBudgetWarning":supplierPOInfo.oldSupplierPOBudgetWarning,
        "supplierPOBudget": '',
        "supplierPOBudgetHours": '00.00',
        "supplierPOMainSupplierName":"",
        "supplierPOMainSupplierAddress":""
    };
    res = await dispatch(actions.FetchSupplierPoData({ "SupplierPOInfo" : storeSupplierPOData }));
    dispatch(FetchVisitDocOfSupplierPo());
    return res; //Added for D932
};

/**
 * Cancel Edit Supplier PO Data 
 */
export const CancelEditSupplierPODetails = () => async (dispatch, getstate) => {    
    let res = null;
    const supplierPOId = getstate().rootSupplierPOReducer.supplierPOData.SupplierPOInfo.supplierPOId;
    const documentData = getstate().rootSupplierPOReducer.supplierPOData.SupplierPODocuments;
    const deleteUrl = supplierPOApiConfig.supplierPODocuments + supplierPOId;
    if (!isEmpty(documentData)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    res = await dispatch(FetchSupplierPoData({ "supplierPOId":supplierPOId }));
    dispatch(FetchVisitDocOfSupplierPo());
    return res;
};

//To Update Sub-Supplier grid data
export const UpdateSubSupplier = (data) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, data);
    const newState = Object.assign([], state.rootSupplierPOReducer.supplierPOData.SupplierPOSubSupplier);
    let index = -1;
    if (editedRow.recordStatus === "N") {
        index = newState.findIndex(subSupplier =>
            subSupplier.subSupplierUniqueIdentifier === editedRow.subSupplierUniqueIdentifier);
    }
    else {
        index = newState.findIndex(subSupplier =>
            subSupplier.subSupplierId === editedRow.subSupplierId);
    }
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateSubSupplier(newState));
    }
};
//To add new Sub-Supplier to grid
export const AddSubSupplier = (data) => (dispatch) => {
    dispatch(actions.AddSubSupplier(data));
};

//To delete subsupplier
export const DeleteSubSupplier = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootSupplierPOReducer.supplierPOData.SupplierPOSubSupplier);
    data && data.forEach(row => {
        newState.forEach((iteratedValue, i) => {
            if (iteratedValue.subSupplierId === row.subSupplierId) {
                if (row.recordStatus !== "N") {
                    newState[i].recordStatus = "D";
                }
                else {
                    const index = newState.findIndex(value => (value.subSupplierUniqueIdentifier === row.subSupplierUniqueIdentifier));
                    if (index >= 0)
                        newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteSubSupplier(newState));
};

//To Update supplier general data
export const updateSupplierDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const SupplierPOInfo = state.rootSupplierPOReducer.supplierPOData ? state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo : {};

    const updatedProjectInfo = Object.assign({}, SupplierPOInfo, data);
    dispatch(actions.updateSupplierDetails(updatedProjectInfo));
};

/**
 * Save Supplier PO Detail
 */
export const SaveSupplierPoData = () => (dispatch, getstate) => {
    const supplierData = getstate().rootSupplierPOReducer.supplierPOData;
    const supplierPOMode = getstate().CommonReducer.currentPage;

    if(supplierData.SupplierPOInfo){
        if (!isEmpty(supplierData.SupplierPOInfo.supplierPODeliveryDate)) {
            supplierData.SupplierPOInfo.supplierPODeliveryDate = moment(supplierData.SupplierPOInfo.supplierPODeliveryDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }
    
        if (!isEmpty(supplierData.SupplierPOInfo.supplierPOCompletedDate)) {
            supplierData.SupplierPOInfo.supplierPOCompletedDate = moment(supplierData.SupplierPOInfo.supplierPOCompletedDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }
    
        if (supplierPOMode === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE) {
            supplierData.SupplierPOInfo.recordStatus = "N";
            dispatch(CreateSupplierPODetails(supplierData));
        }
        if (supplierPOMode === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE) {
            supplierData.SupplierPOInfo.recordStatus = "M";
           // const FilteredSupplierPOInfo = FilterSave(supplierData);
            dispatch(UpdateSupplierPODetails(supplierData));
        }    
    }
    else{
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,"dangerToast SuppPOActCreatSuppDet");
    }
};

/**
 * Create New Supplier PO
 */
export const CreateSupplierPODetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const postUrl = StringFormat(supplierPOApiConfig.supplierPODetails, 0);
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.SUPPLIER_PO_POST_ERROR, 'dangerToast SuppPOActCreatSuppDet');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if (response.code == "1") {
            dispatch(CreateAlert(response.result, "Supplier PO"));
            // dispatch(HandleMenuAction({ 
            //     currentPage: localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE,
            //     currentModule:'supplierpo'
            //  }));
            dispatch(FetchSupplierPoData({ supplierPOId:response.result }));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast SuppPOActCreatSuppDet');
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
        }
    }
    else{
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
    }
    dispatch(HideLoader());
};

/**
 * Update the exisiting project details 
 */
export const UpdateSupplierPODetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const supplierPOId = getstate().rootSupplierPOReducer.supplierPOData.SupplierPOInfo.supplierPOId;
    const putUrl = StringFormat(supplierPOApiConfig.supplierPODetails, supplierPOId);
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(putUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.SUPPLIER_PO_UPDATE_ERROR, 'dangerToast SuppPoActUpdateSuppPODet');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if (response.code == 1) {
            dispatch(SuccessAlert(response.result, "Supplier PO"));
            dispatch(FetchSupplierPoData({ supplierPOId:response.result }));            
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast SuppPoActUpdateSuppPODet');
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
        }
    }
    else{
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
    }
    dispatch(HideLoader());
};

/**
 * Delete existing SupplierPO
 */
export const DeleteSupplierPoData = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const supplierData = getstate().rootSupplierPOReducer.supplierPOData;
    supplierData.SupplierPOInfo.recordStatus = "D";
    const supplierPOId = getstate().rootSupplierPOReducer.supplierPOData.SupplierPOInfo.supplierPOId;
    const deleteUrl = StringFormat(supplierPOApiConfig.supplierPODetails, supplierPOId);
    const params = {
        data: supplierData,
    };
    const requestPayload = new RequestPayload(params);
    const response = await DeleteData(deleteUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.SUPPLIER_PO_DELETE_ERROR, 'dangerToast SuppPOActDelSuppPO');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        if (response.code == 1) {
            dispatch(HideLoader());
            return response;
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast SuppPOActDelSuppPO');
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
        }
    }
    else{
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
    }
    dispatch(HideLoader());
};
export const FetchProjectDetailForSupplier = (data) => async (dispatch, getstate) => {
    const state =getstate();
    const projectNo = state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo;
    const params = {
        'projectNumber': projectNo
    };
    const projectInfoUrl = projectAPIConfig.projectSearch;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectInfoUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROJECT_DETAIL, 'dangerToast FetchBusinessUnit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
      
    if (!isEmpty(response)) {
        dispatch(actions.saveProjectDetailsForSupplier(response.result));
        return true;
    } else if ( response && response.messages.length > 0) {
        response.messages.forEach(result => {
            if (result.message.length > 0) {
                dispatch(ValidationAlert(result.message, "SupplierPo"));
            }
        });
    }
    else {
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "SupplierPo"));
    }

};

//Added for SupplierPODuplication warning
export const SupplierPODuplicateName = (data) => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const duplicateSupplier = [];
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const supplierPOInfo=state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo;
    const supplierPOSearchUrl = supplierPOApiConfig.supplierPOSearch;
    data.supplierPOCompanyCode = selectedCompany;
    const requestPayload = new RequestPayload(data);
    const response = await FetchData(supplierPOSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO_DATA, 'dangerToast FetchSupplierPOSearchResultsErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (!isEmpty(response.result) && (response.code == 1)) {
            if (response.result && response.result.length > 0) {
                response.result.forEach(iteratedValue => {
                    if(supplierPOInfo.supplierPONumber === iteratedValue.supplierPONumber){
                            duplicateSupplier.push(iteratedValue);
                    }
                });
                dispatch(HideLoader());
                return duplicateSupplier;
            }
            else{
                return false;
            }
        } else if (isEmpty(response.result) && (response.code == 1)) {
            return false;
        }
        else if (response.code == 11||response.code == 41 ||response.code == 31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SupplierPOduplicate');
        }
        else {
                IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierPOSearchCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierPOSearchFailure');
    }
    dispatch(HideLoader());
};
// Added For ITK D-456 -Starts
export const SetSupplierPOViewMode = (data) => (dispatch) => {
    dispatch(actions.SetSupplierPOViewMode(data));
};
// Added For ITK D-456 -End