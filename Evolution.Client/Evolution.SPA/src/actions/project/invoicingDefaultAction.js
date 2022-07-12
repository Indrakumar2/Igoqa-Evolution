import { FetchData } from '../../services/api/baseApiService';
import { projectActionTypes } from '../../constants/actionTypes';
import { masterData, contractAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty ,parseValdiationMessage, isEmptyReturnDefault } from '../../utils/commonUtils';
const localConstant = getlocalizeData();

const actions = {
    FetchCustomerContractContact: (payload) => ({
        type: projectActionTypes.invoicingDefaults.PROJECT_CUSTOMER_CONTACT,
        data: payload
    }),
    FetchTaxes: (payload) => ({
        type: projectActionTypes.invoicingDefaults.PROJECT_FETCH_TAXES,
        data: payload
    }),
    FetchCustomerContractAddress: (payload) => ({
        type: projectActionTypes.invoicingDefaults.PROJECT_CUSTOMER_CONTRACT_ADDRESS,
        data: payload
    }),
    FetchCustomerTax :(payload)=>({
        type:projectActionTypes.invoicingDefaults.FETCH_CUSTOMER_TAX,
        data:payload
    }),
    FetchInvoiceRemittanceandFooterText: (payload) => ({
        type: projectActionTypes.invoicingDefaults.PROJECT_INVOICE_REMITTANCE_FOOTER_TEXT,
        data: payload
    }),
    // FetchProjectAttachmentType: (payload) => ({
    //     type: projectActionTypes.invoicingDefaults.PROJECT_ASSIGNMENT_TYPE,
    //     data: payload
    // }),
    AddInvoiceDefault: (payload) => ({
        type: projectActionTypes.invoicingDefaults.ADD_PROJECT_INVOICE_DEFAULT,
        data: payload
    }),
    UpdateInvoiceDefault: (payload) => ({
        type: projectActionTypes.invoicingDefaults.UPDATE_PROJECT_INVOICE_DEFAULT,
        data: payload
    }),
    DeleteInvoiceDefault: (payload) => ({
        type: projectActionTypes.invoicingDefaults.DELETE_PROJECT_INVOICE_DEFAULT,
        data: payload
    }),
    AddAttachmentTypes: (payload) => ({
        type: projectActionTypes.invoicingDefaults.ADD_PROJECT_ATTACHMENT_TYPES,
        data: payload
    }),
    UpdateAttachmentTypes: (payload) => ({
        type: projectActionTypes.invoicingDefaults.UPDATE_PROJECT_ATTACHMENT_TYPES,
        data: payload
    }),
    DeleteAttachmentTypes: (payload) => ({
        type: projectActionTypes.invoicingDefaults.DELETE_PROJECT_ATTACHMENT_TYPES,
        data: payload
    }),
    AddUpdateInvoicingDefaults: (payload) => ({
        type: projectActionTypes.invoicingDefaults.ADD_UPDATE_PROJECT_INVOICING_DEFAULTS,
        data: payload
    })
};

/**
 *  Fetching the required api's at once.
 */
export const FetchInvoicingDefaults = (companyCode) => (dispatch) => {
    dispatch(FetchCustomerContractContact());
    dispatch(FetchCustomerContractAddress());
    dispatch(FetchInvoiceRemittanceandFooterText(companyCode));
    dispatch(FetchTaxes(companyCode));
};
/**
 * Fetch Bot Invoice Invoice Remittance and Footer.
 */
export const FetchInvoiceRemittanceandFooterText = (companyCode) => async (dispatch, getstate) => {
    if (!isEmpty(getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo)) {
        const selectedCompany = companyCode ? companyCode :getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo.contractHoldingCompanyCode;
        const invoiceDetails = contractAPIConfig.contractBaseUrl + contractAPIConfig.companies + selectedCompany + contractAPIConfig.invoicedetail;
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(invoiceDetails, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.INVOICE_FOOTER_AND_REMITTANCE_NOT_FETCHED, 'dangerToast invDefActInvRemandFotTxt');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchInvoiceRemittanceandFooterText(response.result));
        }
        else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast InvoiceRemittanceWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.INVOICE_FOOTER_AND_REMITTANCE_NOT_FETCHED, 'dangerToast invDefActInvRemandFotTxtsmtWrng');
        }
    }
};

/**
 * Fetch Customer Contract Contact
 */
export const FetchCustomerContractContact = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo)) {
        const selectedCustomer = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo.contractCustomerCode;
        const customerContractContact = contractAPIConfig.contractBaseUrl + contractAPIConfig.customers + selectedCustomer + contractAPIConfig.contacts;
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(customerContractContact, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACT_CONTACT_NOT_FETCHED, 'dangerToast invDefActCustCntCont');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchCustomerContractContact(response.result));
        }
        else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast CustCntrctContactWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACT_CONTACT_NOT_FETCHED, 'dangerToast invDefActCustCntContSmtWrng');
        }
    }
};

/**
 * Fetch taxes
 */
export const FetchTaxes = (companyCode) => async (dispatch, getstate) => {
    const projectInfo = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo;
    if (!isEmpty(projectInfo) && projectInfo.contractHoldingCompanyCode) {
        const selectedCompany = companyCode ? companyCode : projectInfo.contractHoldingCompanyCode;
        const salesTax = contractAPIConfig.contractBaseUrl + contractAPIConfig.companies + selectedCompany + contractAPIConfig.taxes;
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(salesTax, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.SALES_TAX_NOT_FETCHED, 'dangerToast invDefActTax');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchTaxes(response.result));
        }
        else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchTaxesWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.SALES_TAX_NOT_FETCHED, 'dangerToast invDefActTaxSmtWrng');
        }
    }
};

/**
 * Fetch the customer contract address
 */
export const FetchCustomerContractAddress = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo)) {
        const selectedCustomer = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo.contractCustomerCode;
        const customerContractAddress = contractAPIConfig.contractBaseUrl + contractAPIConfig.customers + selectedCustomer + contractAPIConfig.addresses;
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(customerContractAddress, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACTOR_ADDRESS_NOT_FETCHED, 'dangerToast invDefActCustContAddress');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchCustomerContractAddress(response.result));
        }
        else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast CustCntrctAddressErr');
        }
        else {
            IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACTOR_ADDRESS_NOT_FETCHED, 'dangerToast invDefActCustContAddressSmtWrng');
        }
    }
};

/**
 * Fetching project Attachment types
 */
// export const FetchProjectAttachmentType = () => async (dispatch) => {
//     const documentTypeMasterData = masterData.baseUrl + masterData.documentType;
//     const params = {
//         'moduleName': 'Project'
//     };
//     const requestPayload = new RequestPayload(params);
//     const response = await FetchData(documentTypeMasterData, requestPayload)
//         .catch(error => {
//             IntertekToaster(localConstant.errorMessages.ATTACHMENT_TYPE_DATA_NOT_FETCHED, 'dangerToast invDefProjActDocTypeMstrDt');
//         });
//     if (response && response.code === "1") {
//         dispatch(actions.FetchProjectAttachmentType(response.result));
//     }
//     else {
//         IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefProjActDocTypeMstrDtSmtWrng');
//     }
// };

/**
 * Add Default Invoice Reference
 */
export const AddInvoiceDefault = (data) => (dispatch) => {
    //const invoiceReferenceDefault = []; //Changes for D1015
    // data.forEach((iteratedValue,index)=>{
    //     if(iteratedValue.displayOrder !== index+1){
    //        // iteratedValue.displayOrder = index+1;
    //         if(iteratedValue.recordStatus !== "N" && iteratedValue.recordStatus !== "D"){
    //             iteratedValue.recordStatus = "M";
    //         }
    //     }
    //     invoiceReferenceDefault.push(iteratedValue);
    // });
    dispatch(actions.AddInvoiceDefault(data));
};

/**
 * Update Default Invoice Reference
 */
export const UpdateInvoiceDefault = (data, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, editedRowData, data);
    const index = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceReferences.findIndex(defaultInvoice => defaultInvoice.projectInvoiceReferenceTypeId === editedRow.projectInvoiceReferenceTypeId);
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceReferences);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateInvoiceDefault(newState));
    }
};

/**
 * Delete Default Invoice Reference
 */
export const DeleteInvoiceDefault = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = [];
    const defaultState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceReferences);
    let displayIndex = 1;
    if(defaultState){
        const invoiceReference = [
            ...defaultState
        ];
        invoiceReference.sort((a, b) => a.displayOrder - b.displayOrder);
        invoiceReference.map((item) => { newState.push(item); });
    }
    data.map(row => {
        newState.map((defaultInvoice, defaultIndex) => {
            if (defaultInvoice.projectInvoiceReferenceTypeId === row.projectInvoiceReferenceTypeId) {
                const index = newState.findIndex(value => (value.projectInvoiceReferenceTypeId === row.projectInvoiceReferenceTypeId));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }  //Changes for D1015 - Changing displayOrder after deleting reference
            else if(defaultInvoice.projectInvoiceReferenceTypeId !== row.projectInvoiceReferenceTypeId && defaultInvoice.recordStatus !== 'D'){
                if(defaultInvoice.displayOrder !== displayIndex){
                    newState[defaultIndex].displayOrder = displayIndex;
                    if (defaultInvoice.recordStatus !== "N") {
                        newState[defaultIndex].recordStatus = "M";
                    }
                }
                displayIndex = displayIndex+1;
            }
        });
        displayIndex = 1;
    });
    dispatch(actions.DeleteInvoiceDefault(newState));
};

/**
 * Add Default Invoice Attachment Types
 */
export const AddAttachmentTypes = (data) => (dispatch) => {
    dispatch(actions.AddAttachmentTypes(data));
};

/**
 * Update Default Invoice Attachment Types
 */
export const UpdateAttachmentTypes = (data, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, editedRowData, data);
    const index = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceAttachments.findIndex(attachment => attachment.projectInvoiceAttachmentId === editedRow.projectInvoiceAttachmentId);
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceAttachments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateAttachmentTypes(newState));
    }
};

/**
 * Delete Default Invoice Attachment Types 
 */
export const DeleteAttachmentTypes = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceAttachments);
    data.map(row => {
        newState.map(attachment => {
            if (attachment.projectInvoiceAttachmentId === row.projectInvoiceAttachmentId) {
                const index = newState.findIndex(value => (value.projectInvoiceAttachmentId === row.projectInvoiceAttachmentId));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteAttachmentTypes(newState));
};

/**
 * Update the modified data of the invoicing default. 
 */
export const AddUpdateInvoicingDefaults = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = Object.assign({}, state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo, data);
    dispatch(actions.AddUpdateInvoicingDefaults(modifiedData));
};