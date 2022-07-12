import { FetchData } from '../../services/api/baseApiService';
import { contractAPIConfig } from '../../apiConfig/apiConfig';
import { contractActionTypes } from '../../constants/actionTypes';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData } from '../../utils/commonUtils';
import { required } from '../../utils/validator';
const localConstant = getlocalizeData();
const actions = {
    FetchCustomerContractContact: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.SHOW_CUSTOMER_CONTACT,
        data: payload
    }),
    FetchTaxes: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.FETCH_TAXES,
        data: payload
    }),
    FetchTaxesOnRefresh: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.FETCH_TAXES,
        data: payload
    }),
    FetchCustomerContractAddress: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.SHOW_CUSTOMER_CONTRACT_ADDRESS,
        data: payload
    }),
    FetchInvoiceRemittanceandFooterText: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.SHOW_INVOICE_REMITTANCE_FOOTER_TEXT,
        data: payload
    }),
    InvoiceReferenceModalState: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_REFERENCE_MODAL_OPEN,
        data: payload
    }),
    InvoiceReferenceEditCheck: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_REFERENCE_EDIT,
        data: payload
    }),
    InvoiceAttachmentTypesModalState: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_ATTACHMENT_TYPES_MODAL_OPEN,
        data: payload
    }),
    InvoiceAttachmentTypesEditCheck: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_ATTACHMENT_TYPES_EDIT,
        data: payload
    }),
    AddNewInvoiceDefault: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.ADD_NEW_INVOICE_DEFAULT,
        data: payload
    }),
    DeleteInvoiceDefault: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.DELETE_INVOICE_DEFAULT,
        data: payload
    }),
    EditInvoiceDefault: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.EDIT_INVOICE_DEFAULT,
        data: payload
    }),
    UpdateInvoiceDefault: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.UPDATE_INVOICE_DEFAULT,
        data: payload
    }),
    AddAttachmentTypes: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.ADD_ATTACHMENT_TYPES,
        data: payload
    }),
    DeleteAttachmentTypes: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.DELETE_ATTACHMENT_TYPES,
        data: payload
    }),
    EditAttachmentTypes: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.EDIT_ATTACHMENT_TYPES,
        data: payload
    }),
    UpdateAttachmentTypes: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.UPDATE_ATTACHMENT_TYPES,
        data: payload
    }),
    AddUpdateInvoicingDefaults: (payload) => ({
        type: contractActionTypes.invoiceDefaultActionTypes.ADD_UPDATE_INVOICING_DEFAULTS,
        data: payload
    })
};

export const FetchInvoicingDefaults = () => (dispatch) => {
    dispatch(FetchCustomerContractContact());
    dispatch(FetchCustomerContractAddress());
    dispatch(FetchInvoiceRemittanceandFooterText());
    dispatch(FetchTaxes());
};

export const FetchInvoiceRemittanceandFooterText = () => async (dispatch, getstate) => { 
    const contractInfo = getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo;
    if(contractInfo && contractInfo.contractHoldingCompanyCode){
        const selectedCompany = contractInfo.contractHoldingCompanyCode;
        const invoiceDetails = contractAPIConfig.contractBaseUrl + contractAPIConfig.companies + selectedCompany + contractAPIConfig.invoicedetail;
        const response = await FetchData(invoiceDetails, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Invoice Footer and Remittance not fetched', 'dangerToast invDefActInvRemandFotTxt');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchInvoiceRemittanceandFooterText(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActInvRemandFotTxtsmtWrng');
        }
    }
};

export const FetchCustomerContractContact = () => async (dispatch, getstate) => {
    //Changes for Defect 882 -Starts
    const state=getstate();
    const contractInfo=state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo;
    const contractCustomerCode= contractInfo && required(contractInfo.contractCustomerCode) ?
        state.RootContractReducer.ContractDetailReducer.customerName.customerCode :
        contractInfo.contractCustomerCode;
    if (contractCustomerCode) {
        const selectedCustomer = contractCustomerCode;
    //Changes for Defect 882 -End
        const customerContractContact = contractAPIConfig.contractBaseUrl + contractAPIConfig.customers + selectedCustomer + contractAPIConfig.contacts;
        const response = await FetchData(customerContractContact, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Customer Contract Contact not fetched', 'dangerToast invDefActCustCntCont');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchCustomerContractContact(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActCustCntContSmtWrng');
        }
    }
   
};
export const FetchTaxesOnRefresh = () => async (dispatch, getstate) => {
    const selectedCompany= getstate().appLayoutReducer.selectedCompany;
    if (selectedCompany) {
        const salesTax = contractAPIConfig.contractBaseUrl + contractAPIConfig.companies + selectedCompany + contractAPIConfig.taxes;
        const response = await FetchData(salesTax, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Sales tax not fetched', 'dangerToast invDefActTax');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchTaxes(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActTaxSmtWrng');
        }
    }
};

export const FetchTaxes = () => async (dispatch, getstate) => {
    const contractInfo = getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo;
    if(contractInfo && contractInfo.contractHoldingCompanyCode){     
    const selectedCompany = contractInfo.contractHoldingCompanyCode;    
    const salesTax = contractAPIConfig.contractBaseUrl + contractAPIConfig.companies + selectedCompany + contractAPIConfig.taxes;
    const response = await FetchData(salesTax, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Sales tax not fetched', 'dangerToast invDefActTax');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTaxes(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActTaxSmtWrng');
    }
}
};

export const FetchCustomerContractAddress = () => async (dispatch, getstate) => {
    //Changes for Defect 882 -Starts
    const state=getstate();
    const contractInfo=state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo;
    const contractCustomerCode= contractInfo && required(contractInfo.contractCustomerCode) ?
        state.RootContractReducer.ContractDetailReducer.customerName.customerCode :
        contractInfo.contractCustomerCode;
    if (contractCustomerCode) {
        const selectedCustomer = contractCustomerCode;
    //Changes for Defect 882 -End
        const customerContractAddress = contractAPIConfig.contractBaseUrl + contractAPIConfig.customers + selectedCustomer + contractAPIConfig.addresses;
        const response = await FetchData(customerContractAddress, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Customer Contract Address not fetched', 'dangerToast invDefActCustContAddress');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchCustomerContractAddress(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActCustContAddressSmtWrng');
        }
    }
};

export const InvoiceReferenceModalState = (data) => (dispatch, getstate) => {
    dispatch(actions.InvoiceReferenceModalState(data));
};

export const InvoiceReferenceEditCheck = (data) => (dispatch, getstate) => {
    dispatch(actions.InvoiceReferenceEditCheck(data));
};
export const InvoiceAttachmentTypesModalState = (data) => (dispatch, getstate) => {
    dispatch(actions.InvoiceAttachmentTypesModalState(data));
};

export const InvoiceAttachmentTypesEditCheck = (data) => (dispatch, getstate) => {
    dispatch(actions.InvoiceAttachmentTypesEditCheck(data));
};

export const AddNewInvoiceDefault = (data) => (dispatch) => {
    // const invoiceReferenceDefault = [];  //Changes for D1015
    // data.map((iteratedValue,index)=>{
    //     if(iteratedValue.displayOrder !== index+1){
    //        // iteratedValue.displayOrder = index+1;
    //         if(iteratedValue.recordStatus !== "N" && iteratedValue.recordStatus !== "D"){
    //             iteratedValue.recordStatus = "M";
    //         }
    //     }
    //     invoiceReferenceDefault.push(iteratedValue);
    // });
    dispatch(actions.AddNewInvoiceDefault(data));
};

export const EditInvoiceDefault = (data) => (dispatch) => {
    dispatch(actions.EditInvoiceDefault(data));
};

export const UpdateInvoiceDefault = (data) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, state.RootContractReducer.ContractDetailReducer.editInvoiceReferences, data);
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences.findIndex(defaultInvoice => defaultInvoice.contractInvoiceReferenceTypeId === editedRow.contractInvoiceReferenceTypeId);
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateInvoiceDefault(newState));
    }
};

export const DeleteInvoiceDefault = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = [];
    const defaultState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences);
    let displayIndex = 1;
    if(defaultState){
        const invoiceReference = [
            ...defaultState
        ];
        invoiceReference.sort((a, b) => a.displayOrder - b.displayOrder);
        invoiceReference.map((item) => { newState.push(item); });
    }
    data.map(row => {
        newState.map((defaultInvoice,defaultIndex) => {
            if (defaultInvoice.contractInvoiceReferenceTypeId === row.contractInvoiceReferenceTypeId) {
                const index = newState.findIndex(value => (value.contractInvoiceReferenceTypeId === row.contractInvoiceReferenceTypeId));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }  //Changes for D1015 - Changing displayOrder after deleting reference
            else if(defaultInvoice.contractInvoiceReferenceTypeId !== row.contractInvoiceReferenceTypeId && defaultInvoice.recordStatus !== 'D'){
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

export const AddAttachmentTypes = (data) => (dispatch) => {
    dispatch(actions.AddAttachmentTypes(data));
};

export const EditAttachmentTypes = (data) => (dispatch) => {
    dispatch(actions.EditAttachmentTypes(data));
};

export const UpdateAttachmentTypes = (data) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, state.RootContractReducer.ContractDetailReducer.editAttachmentTypes, data);
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments.findIndex(attachment => attachment.contractInvoiceAttachmentId === editedRow.contractInvoiceAttachmentId);
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateAttachmentTypes(newState));
    }
};

export const DeleteAttachmentTypes = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments);
    data.map(row => {
        newState.map(attachment => {
            if (attachment.contractInvoiceAttachmentId === row.contractInvoiceAttachmentId) {
                const index = newState.findIndex(value => (value.contractInvoiceAttachmentId === row.contractInvoiceAttachmentId));
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

export const AddUpdateInvoicingDefaults = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = Object.assign({}, state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo, data);
    dispatch(actions.AddUpdateInvoicingDefaults(modifiedData));
};