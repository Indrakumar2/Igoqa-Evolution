import { customerActionTypes } from '../../../constants/actionTypes';
import { companyActionTypes } from '../../../constants/actionTypes';//chandra
import { processApiRequest } from '../../../services/api/defaultServiceApi';
import { customerAPIConfig, RequestPayload, masterData } from '../../../apiConfig/apiConfig';
//import { FetchCity, FetchState } from '../../viewComponents/company/companyAction';
import { SuccessAlert, FailureAlert } from './alertAction';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { FetchData, PostData } from '../../../services/api/baseApiService';
import { getlocalizeData, isEmpty, parseValdiationMessage, FilterSave, isUndefined, isEmptyOrUndefine } from '../../../utils/commonUtils';
import { ReplaceString } from '../../../utils/stringUtil';
import { RemoveDocumentsFromDB, ShowLoader, HideLoader, SetCurrentPageMode, UpdateCurrentPage, UpdateCurrentModule } from '../../../common/commonAction';
import { UpdateSelectedCompany } from '../../appLayout/appLayoutActions';
import { FetchProjectList } from '../../../actions/project/projectSearchAction';
import { isArray } from 'util';
import { FetchCity, FetchCityId, FetchState, FetchStateId } from '../../../common/masterData/masterDataActions';

const localConstant = getlocalizeData();
const actions = {
    FetchCustomerDetail: (payload) => ({
        type: customerActionTypes.FETCH_CUSTOMER_DETAIL,
        data: payload
    }),

    // general details actions
    FetchCustomerList: (payload) => ({
        type: customerActionTypes.FETCH_CUSTOMER_LIST,
        data: payload
    }),

    AddCustomerAddress: (payload) => ({
        type: customerActionTypes.ADD_CUSTOMER_ADDRESS,
        data: payload
    }),
    AddCustomerContact: (payload) => ({
        type: customerActionTypes.ADD_CUSTOMER_CONTACT,
        data: payload
    }),
    DeleteCustomerAddress: (payload) => ({
        type: customerActionTypes.DELETE_CUSTOMER_ADDRESS,
        data: payload
    }),
    DeleteCustomerContact: (payload) => ({
        type: customerActionTypes.DELETE_CUSTOMER_CONTACT,
        data: payload
    }),
    EditAddressReference: (payload) => ({
        type: customerActionTypes.EDIT_ADDRESS_REFERENCE,
        data: payload
    }),
    EditStateAddressReference: (payload) => ({
        type: customerActionTypes.EDIT_STATE_ADDRESS_REFERENCE,
        data: payload
    }),
    EditContactReference: (payload) => ({
        type: customerActionTypes.EDIT_CONTACT_REFERENCE,
        data: payload
    }),
    UpdateAddressReference: (payload) => ({
        type: customerActionTypes.UPDATE_ADDRESS_REFERENCE,
        data: payload
    }),
    UpdateContactReference: (payload) => ({
        type: customerActionTypes.UPDATE_CONTACT_REFERENCE,
        data: payload
    }),

    //Assignment actions
    FetchAssignmentReference: (payload) => ({
        type: customerActionTypes.FETCH_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    FetchAccountReference: (payload) => ({
        type: customerActionTypes.FETCH_ACCOUNT_REFERENCE,
        data: payload
    }),
    AddAssignmentReference: (payload) => ({
        type: customerActionTypes.ADD_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    AddAccountReference: (payload) => ({
        type: customerActionTypes.ADD_ACCOUNT_REFERENCE,
        data: payload
    }),
    DeleteAssignmentReference: (payload) => ({
        type: customerActionTypes.DELETE_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    DeleteAccountReference: (payload) => ({
        type: customerActionTypes.DELETE_ACCOUNT_REFERENCE,
        data: payload
    }),
    EditAssignmentReference: (payload) => ({
        type: customerActionTypes.EDIT_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    EditAccountReference: (payload) => ({
        type: customerActionTypes.EDIT_ACCOUNT_REFERENCE,
        data: payload
    }),
    UpdateAssignmentReference: (payload) => ({
        type: customerActionTypes.UPDATE_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    UpdateAccountReference: (payload) => ({
        type: customerActionTypes.UPDATE_ACCOUNT_REFERENCE,
        data: payload
    }),
    GetSelectedCustomerCode: (payload) => ({
        type: customerActionTypes.SELECTED_CUSTOMER_CODE,
        data: payload
    }),
    AddNotesDetails: (payload) => ({
        type: customerActionTypes.ADD_NOTES_DETAILS,
        data: payload
    }),
    EditNotesDetails: (payload) => ({
        type: customerActionTypes.EDIT_NOTES_DETAILS,
        data: payload
    }),
    //contracts
    FetchCustomerContracts: (payload) => ({
        type: customerActionTypes.FETCH_CONTRACT_OF_CUSTOMER,
        data: payload
    }),
    GetSelectedContractNumber: (payload) => ({
        type: customerActionTypes.SELECTED_CONTRACT_NUMBER,
        data: payload
    }),
    ShowButtonHandler: () => ({
        type: customerActionTypes.SHOWBUTTON
    }),
    FetchAssignmentRefTypes: (payload) => ({
        type: customerActionTypes.FETCH_ASSIGNMENT_REF_TYPES,
        data: payload
    }),
    FetchMasterDocumentTypes: (payload) => ({
        type: customerActionTypes.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    AddDocumentDetails: (payload) => ({
        type: customerActionTypes.ADD_DOCUMENTS_DETAILS,
        data: payload
    }),
    CopyDocumentDetails: (payload) => ({
        type: customerActionTypes.COPY_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteDocumentDetails: (payload) => ({
        type: customerActionTypes.DELETE_DOCUMENTS_DETAILS,
        data: payload
    }),
    UpdateDocumentDetails: (payload) => ({
        type: customerActionTypes.UPDATE_DOCUMENTS_DETAILS,
        data: payload
    }),
    EditDocumentDetails: (payload) => ({
        type: customerActionTypes.EDIT_CUSTOMER_DOCUMENTS_DETAILS,
        data: payload
    }),
    //POST ACTION
    SaveCustomerDetails: (payload) => ({
        type: customerActionTypes.SAVE_CUTOMER_DETAILS,
        data: payload
    }),
    SetLoader: (payload) => ({
        type: customerActionTypes.SET_LOADER,
        data: payload
    }),
    RemoveLoader: (payload) => ({
        type: customerActionTypes.REMOVE_LOADER,
        data: payload
    }),
    ClearSearchData: () => ({
        type: customerActionTypes.CLEAR_SEARCH_DATA
    }),
    ClearGridFormSearchData: () =>
    ({
        type: customerActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS
    }),
    CustomerUpdatedAction: (payload) => ({
        type: customerActionTypes.CUSTOMER_UPDATED,
        data: payload
    }),
    DisplayDocuments: (payload) => ({
        type: customerActionTypes.DISPLAY_DOCUMENTS,
        data: payload
    }),
    PasteDocumentUploadData: (payload) => ({
        data: payload
    }),
    FetchCustomerContractDocuments: (payload) => ({
        type: customerActionTypes.CUSTOMER_CONTRACT_DOCUMENTS,
        data: payload
    }),
    FetchCustomerProjectDocuments: (payload) => ({
        type: customerActionTypes.CUSTOMER_PROJECT_DOCUMENTS,
        data: payload
    }),
    FetchCustomerProjects: (payload) => ({
        type: customerActionTypes.FETCH_CUSTOMER_PROJECTS,
        data: payload
    }),

    ClearStateCityData: (payload) => ({
        type: customerActionTypes.CLEAR_STATE_CITY_DATA,
        data: payload
    }),

    ClearMasterStateCityData: (payload) => ({
        type: companyActionTypes.CLEAR_STATE_CITY_DATA,
        data: payload
    }),

    ClearMasterCityData: (payload) => ({
        type: companyActionTypes.CLEAR_CITY_DATA,
        data: payload
    }),
    ClearCityData: (payload) => ({
        type: customerActionTypes.CLEAR_CITY_DATA,
        data: payload
    }),

    DisplayFullAddress: (payload) => ({
        type: customerActionTypes.DISPLAY_FULL_ADDRESS,
        data: payload
    }),
    ExtranetUserAccountModalState: (payload) => ({
        type: customerActionTypes.IS_EXTRANET_USER_ACCOUNT_MODAL,
        data: payload
    }),
    AddExtranetUser: (payload) => ({
        type: customerActionTypes.ADD_EXTRANET_USER,
        data: payload
    }),
    AssignExtranetUserToContact: (payload) => ({
        type: customerActionTypes.ASSIGN_EXTRANET_USER_TO_CONTACT,
        data: payload
    }),
    DeactivateExtranetUserAccount: (payload) => ({
        type: customerActionTypes.DEACTIVATE_EXTRANET_USER_ACCOUNT,
        data: payload
    }),
    ExtranetUserAccountsList: (payload) => ({
        type: customerActionTypes.POPULATE_EXTRANET_USERS_LIST,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: customerActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

/**
 * Customer Updated Check 
 */

export const SetLoader = (data) => (dispatch, getstate) => {
    dispatch(actions.SetLoader(false));
};

export const RemoveLoader = (data) => (dispatch, getstate) => {
    dispatch(actions.RemoveLoader(true));
};
export const FetchCustomerProjectDocuments = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const customerInfo = Object.assign({}, state.CustomerReducer.customerDetail.Detail);
    const customerId = customerInfo.customerId;
    const apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.projects + customerAPIConfig.documents + customerAPIConfig.getProjectDocuments + customerId;
    const requestPayload = new RequestPayload();
    if (customerId) {
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.customer.FETCH_PROJECT_CONTRACT_DOC_FAILED, 'wariningToast customerProjectDocFail');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {
            if (response.code == 1) {
                dispatch(actions.FetchCustomerProjectDocuments(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast customerProjectDocFail');
            }
            else {
                IntertekToaster(localConstant.customer.FETCH_PROJECT_CONTRACT_DOC_FAILED, 'wariningToast customerProjectDocFail');
            }
        }
    };
};
export const ClearCustomerDocument = () => async (dispatch, getstate) => {
    const state = getstate();
    const customerDetails = Object.assign({}, state.CustomerReducer.customerDetail.Detail);
    const documentData = state.CustomerReducer.customerDetail.Documents;
    const customerCode = customerDetails.customerCode;
    const deleteUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDocuments + customerCode;
    let response = null;
    if (!isEmpty(documentData) && !isUndefined(customerCode)) {
        response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response;
};
export const CancelCustomerDetail = () => async (dispatch, getstate) => {
    const documentData = getstate().CustomerReducer.customerDetail.Documents;
    const customerCode = getstate().CustomerReducer.customerDetail.Detail.customerCode;
    const deleteUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDocuments + customerCode;
    if (!isEmpty(documentData)) {
        const res = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    dispatch(FetchCustomerDetail({ "customerCode": encodeURIComponent(customerCode) }));
    dispatch(FetchCustomerContractDocuments());
    dispatch(FetchCustomerProjectDocuments());
};
export const FetchCustomerDetail = (data) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    let selectedCustomer = getstate().CustomerReducer.selectedCustomerCode;
    let selectedCompany = getstate().appLayoutReducer.selectedCompany;
    if (data && data.selectedCompany) {
        selectedCompany = data.selectedCompany;
        dispatch(UpdateSelectedCompany({ companyCode: selectedCompany }));
    }
    if (data && data.customerCode && isEmpty(selectedCustomer)) {
        selectedCustomer = data.customerCode;
    }
    const apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customer + customerAPIConfig.customerDetail + "?customerCode=" + encodeURIComponent(selectedCustomer);
    //const apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customer +selectedCustomer+"/"+ customerAPIConfig.customerDetail;
    //dispatch(SetLoader());

    processApiRequest(apiUrl, {
        method: "GET",
        headers: { 'Content-Type': 'text/plain' }
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isEmptyOrUndefine(response.data.Detail)) {
            const userList = [];
            dispatch(actions.FetchCustomerDetail(response.data));
            if (isArray(response.data.Addresses)) {

                response.data.Addresses.forEach(element => {
                    if (isArray(element.Contacts)) {
                        element.Contacts.forEach(item => {
                            if (item.UserInfo) {//Added IsActive Check for Sanity Defect -125 /** 08-12-2020 : Removed IsActive Condition for ITK D1432 */
                                userList.push(item.UserInfo);
                            }
                        });
                    }
                });
            }
            dispatch(actions.ExtranetUserAccountsList(userList));
            dispatch(RemoveLoader());
            /**
             * toggle customer updated v~alue to false after customer fetch done
             */
            dispatch(actions.CustomerUpdatedAction(false));
        } else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast CustActCustDet');
        }
        dispatch(HideLoader());
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast CustActCustDeterr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
};

//To-Do: Need to change the logic 
//for temporary purpose the logic is written.
export const FetchCustomerList = (data) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCustomerList([]));
    const searchData = data;
    let apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails + '?';
    if (searchData.customerCode !== undefined && searchData.customerCode !== '') {
        apiUrl += "customerCode=" + encodeURIComponent(searchData.customerCode) + '&';
    }
    if (searchData.parentCompanyName !== undefined && searchData.parentCompanyName !== '') {
        apiUrl += "parentCompanyName=" + searchData.parentCompanyName + '&';
    }
    if (searchData.customerName !== undefined && searchData.customerName !== '') {
        apiUrl += "customerName=" + searchData.customerName + '&';
    }
    if (searchData.operatingCountry !== undefined) {
        apiUrl += "operatingCountry=" + searchData.operatingCountry + '&';
    }
    if (searchData.searchDocumentType !== undefined) {
        apiUrl += "searchDocumentType=" + searchData.searchDocumentType + '&';
    }
    if (searchData.documentSearchText !== undefined && searchData.documentSearchText !== '') {
        const searchText = ReplaceString(searchData.documentSearchText, '+', '%2B');
        apiUrl += "documentSearchText=" + searchText + '&';
    }
    apiUrl = apiUrl.slice(0, -1);
    processApiRequest(apiUrl, {
        method: "GET"
    }).then(response => {
        if (response.data) {
            if (response.data.code == 1) {
                dispatch(actions.FetchCustomerList(response.data.result));
                dispatch(HideLoader());
            }
            else if (response && response.data.code && (response.data.code === "11" || response.data.code === "41" || response.data.code === "31")) {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast CustActCustListWentWrong');
            }
        }
        else {
            IntertekToaster(localConstant.customer.FETCH_CUSTOMER_LIST_FAILED, 'dangerToast CustActCustListResultErr');
            dispatch(HideLoader());
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast CustActCustListErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
};
// operations
export const AddCustomerAddress = (data) => (dispatch) => {
    dispatch(actions.AddCustomerAddress(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const DeleteCustomerAddress = (data) => (dispatch) => {
    dispatch(actions.DeleteCustomerAddress(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const AddCustomerContact = (data) => (dispatch) => {
    dispatch(actions.AddCustomerContact(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const DeleteCustomerContact = (data) => (dispatch) => {
    dispatch(actions.DeleteCustomerContact(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

//assignment
export const FetchAssignmentRefTypes = () => (dispatch, getstate) => {
    const assignmentRefTypesUrl = masterData.baseUrl + masterData.assignmentReference;
    processApiRequest(assignmentRefTypesUrl, {
        method: "GET"
    }).then(response => {
        if (response.data) {
            if (response.data.code == 1) {
                dispatch(actions.FetchAssignmentRefTypes(response.data.result));
            }
            else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast CustActAssignRefTypWentWrong');
            }
        }
        else {
            IntertekToaster(localConstant.customer.FETCH_ASSIGNMENT_REF_FAILED, 'dangerToast CustActAssignRefTyp');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast CustActAssignRefTypErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

//documentTypes Master data
export const FetchMasterDocumentTypes = () => (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=customer";
    processApiRequest(documentTypesMasterDataUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchMasterDocumentTypes(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast CustActMasDocTyp');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast CustActMasDocTypErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};
//Fetch customer related contract documents
export const FetchCustomerContractDocuments = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const customerInfo = Object.assign({}, state.CustomerReducer.customerDetail.Detail);
    const customerId = customerInfo.customerId;
    const customercontractdocumentDataUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerContractDocuments + customerId;
    const requestPayload = new RequestPayload();
    if (customerId) {
        const response = await FetchData(customercontractdocumentDataUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.customer.FETCH_CUSTOMER_CONTRACT_FAILED, 'wariningToast customerContractDocFail');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {
            if (response.code == 1) {
                dispatch(actions.FetchCustomerContractDocuments(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast customerContractDocFail');
            }
            else {
                IntertekToaster(localConstant.customer.FETCH_CUSTOMER_CONTRACT_FAILED, 'wariningToast customerContractDocFail');
            }
        }
    };
};

export const AddAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddAssignmentReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const AddAccountReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddAccountReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const EditAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.EditAssignmentReference(data));
};
export const EditAccountReference = (data) => (dispatch, getstate) => {
    dispatch(actions.EditAccountReference(data));
};
export const EditAddressReference = (data) => (dispatch, getstate) => {
    dispatch(actions.EditAddressReference(data));
    // dispatch(FetchState(data.Country));
    // dispatch(FetchCity(data.County));
    dispatch(FetchStateId(data.CountryId));
    dispatch(FetchCityId(data.StateId));
    dispatch(DisplayFullAddress(data.Address));
};
export const EditStateAddressReference = (data) => (dispatch, getstate) => {
    dispatch(actions.EditStateAddressReference(data));
};
export const EditContactReference = (data) => (dispatch, getstate) => {
    dispatch(actions.EditContactReference(data));
};
export const UpdateAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateAssignmentReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const UpdateAddressReference = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateAddressReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const UpdateContactReference = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateContactReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const UpdateAccountReference = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateAccountReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const DeleteAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.DeleteAssignmentReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const DeleteAccountReference = (data) => (dispatch, getstate) => {
    dispatch(actions.DeleteAccountReference(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const GetSelectedCustomerCode = (data) => (dispatch, getstate) => {
    dispatch(actions.GetSelectedCustomerCode(data));
    dispatch(SetCurrentPageMode());
    dispatch(UpdateCurrentPage(localConstant.customer.EDIT_VIEW_CUSTOMER));
    dispatch(UpdateCurrentModule(localConstant.contract.CUSTOMER));
};
export const AddNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddNotesDetails(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const EditNotesDetails = (editedData) => (dispatch, getstate) => { //D661 issue8
    const state = getstate();
    const index = state.CustomerReducer.customerDetail.Notes.findIndex(iteratedValue => iteratedValue.customerNoteId === editedData.customerNoteId);
    const newState = Object.assign([], state.CustomerReducer.customerDetail.Notes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditNotesDetails(newState));
        dispatch(actions.CustomerUpdatedAction(true));
    }
};
export const AddDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddDocumentDetails(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const CopyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.CopyDocumentDetails(data));
    dispatch(actions.CustomerUpdatedAction(true));
};
export const DeleteDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.DeleteDocumentDetails(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const ClearStateCityData = (data) => (dispatch) => {
    dispatch(actions.ClearStateCityData(Object.assign(data, { City: '', County: '' })));
};
export const ClearMasterStateCityData = () => (dispatch) => {
    dispatch(actions.ClearMasterStateCityData());
};
export const ClearCityData = (data) => (dispatch) => {
    dispatch(actions.ClearCityData(Object.assign(data, { City: '' })));
};
export const ClearMasterCityData = () => (dispatch) => {
    dispatch(actions.ClearMasterCityData());
};

export const UpdateDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if (editedData) {
        const editedRow = Object.assign({}, editedData, data);
        const index = state.CustomerReducer.customerDetail.Documents.findIndex(document => document.id === editedRow.id);
        const newState = Object.assign([], state.CustomerReducer.customerDetail.Documents);
        newState[index] = editedRow;
        if (index >= 0) {
            dispatch(actions.UpdateDocumentDetails(newState));
            dispatch(actions.CustomerUpdatedAction(true));
        }
    }
    else {
        dispatch(actions.UpdateDocumentDetails(data));
        dispatch(actions.CustomerUpdatedAction(true));
    }
};
export const EditDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.EditDocumentDetails(data));
};

export const DisplayFullAddress = (data) => (dispatch) => {
    dispatch(actions.DisplayFullAddress(data));
};
//Projects
export const FetchCustomerProjects = (status) => async (dispatch, getstate) => {
    const data = {
        responseResult: null,
        selectedValue: status
    };
    if (status === '') {
        const data = {
            responseResult: [],
            selectedValue: status
        };
        dispatch(actions.FetchCustomerProjects(data));
        return false;
    }
    dispatch(actions.FetchCustomerProjects(data));
    const customerCode = getstate().CustomerReducer.selectedCustomerCode;
    const customerProjectData = customerAPIConfig.custBaseUrl + customerAPIConfig.custProjectDetail + customerCode;
    let params;
    if (status === "all") {
        params = {
            customerCode: customerCode
        };
    } else {
        params = {
            customerCode: customerCode,
            contractStatus: status
        };
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(customerProjectData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.customer.FETCH_CUSTOMER_PROJECTS_API_FAILED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const data = {
            responseResult: response.result,
            selectedValue: status
        };
        dispatch(actions.FetchCustomerProjects(data));
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast assignemntWentWrong');
    }
    else {
        IntertekToaster(localConstant.customer.FETCH_CUSTOMER_PROJECTS_API_FAILED, 'dangerToast assignemntSomthingWrong');
    }
};
//contracts
export const FetchCustomerContracts = (status) => async (dispatch, getstate) => {
    const data = {
        responseResult: null,
        selectedValue: status
    };
    if (status === '') {
        const data = {
            responseResult: [],
            selectedValue: status
        };
        dispatch(actions.FetchCustomerContracts(data));
        return false;
    }
    dispatch(actions.FetchCustomerContracts(data));
    const customerCode = getstate().CustomerReducer.selectedCustomerCode;
    const customerContractData = customerAPIConfig.custBaseUrl + customerAPIConfig.custContractDetail + customerCode;
    let params;
    if (status === "all") {
        params = {
            customerCode: customerCode
        };
    } else {
        params = {
            customerCode: customerCode,
            contractStatus: status
        };
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(customerContractData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.customer.FETCH_CUSTOMER_CONTRACT_API_FAILED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const data = {
            responseResult: response.result,
            selectedValue: status
        };
        dispatch(actions.FetchCustomerContracts(data));
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast custContractWentWrong');
    }
    else {
        IntertekToaster(localConstant.customer.FETCH_CUSTOMER_CONTRACT_API_FAILED, 'dangerToast assignemntSomthingWrong');
    }
};
export const GetSelectedContractNumber = (data) => (dispatch, getstate) => {
    dispatch(actions.GetSelectedContractNumber(data));
};

export const ShowButtonHandler = () => (dispatch) => {
    dispatch(actions.ShowButtonHandler());
};

export const ClearSearchData = () => (dispatch) => {
    dispatch(actions.ClearSearchData());
};
export const DisplayDocuments = (data) => (dispatch) => {
    dispatch(actions.DisplayDocuments(data));
};

export const ClearGridFormSearchData = () => (dispatch) => {
    dispatch(actions.ClearGridFormSearchData());
};
//POST CUSTOMER DATA
export const SaveCustomerDetails = () => (dispatch, getstate) => {
    // dispatch(SetLoader());
    dispatch(ShowLoader());
    const customerDataToPost = [];

    const customerData = getstate().CustomerReducer.customerDetail;

    if (customerData.Documents) {
        customerData.Documents.map(row => {
            if (!isEmpty(row.status)) {
                row.status = row.status.trim();
            }

            if (row.recordStatus === "N") {
                row.id = 0;
            }
        });
    }
    customerDataToPost.push(customerData);
    const postCustomerUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;
    // let filteredCustomerObject = FilterSave(customerDataToPost,[]);
    // filteredCustomerObject = Object.keys(filteredCustomerObject).map(function(key) {
    //     return [ Number(key), filteredCustomerObject[key] ];
    //   });
    processApiRequest(postCustomerUrl, {
        method: "PUT",
        data: customerDataToPost,
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Acces-Control-Allow-Origin": "*"
        }
    }).then(response => {
        if (response.status === 200) {
            if (response.data.code == 1) {
                dispatch(actions.SaveCustomerDetails());
                dispatch(SuccessAlert(response.data, "Customer"));
                dispatch(FetchCustomerDetail());
                dispatch(HideLoader());
            }
            else if (response && response.data.code && (response.data.code === "11" || response.data.code === "41" || response.data.code === "31")) {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast CustomerSaveWentWrong');
            }
            else {
                dispatch(FailureAlert(response.data, "Customer"));
            }
            dispatch(HideLoader());
        }
        else {
            dispatch(HideLoader());
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast CustActCustSaveCustomerErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

};

export const ExtranetUserAccountModalState = (data) => (dispatch, getstate) => {
    if (data) {
        const projectSearchFilter = {
            contractCustomerCode: getstate().CustomerReducer.selectedCustomerCode,
            contractHoldingCompanyCode: getstate().appLayoutReducer.selectedCompany
        };
        dispatch(FetchProjectList(projectSearchFilter));
    }
    dispatch(actions.ExtranetUserAccountModalState(data));
};

export const AddExtranetUser = (data) => (dispatch) => {
    dispatch(actions.AddExtranetUser(data));
};

export const AssignExtranetUserToContact = (data, prj) => (dispatch) => {
    dispatch(actions.AssignExtranetUserToContact({ UserInfo: data, AuthourisedProjects: prj }));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const DeactivateExtranetUserAccount = (data) => (dispatch) => {
    dispatch(actions.DeactivateExtranetUserAccount(data));
    dispatch(actions.CustomerUpdatedAction(true));
};

export const ExtranetUserAccountsList = (data) => (dispatch) => {
    dispatch(actions.ExtranetUserAccountsList(data));
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};
