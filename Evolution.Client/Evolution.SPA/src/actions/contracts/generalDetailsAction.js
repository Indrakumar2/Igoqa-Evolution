import { companyActionTypes, contractActionTypes, customerActionTypes } from '../../constants/actionTypes';
import { processApiRequest } from '../../../src/services/api/defaultServiceApi';
import {
    companyAPIConfig,
    RequestPayload,
    homeAPIConfig,
    customerAPIConfig,
    contractAPIConfig
} from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { AddNewInvoiceDefault } from './invoicingDefaultsAction';
import { getlocalizeData, isEmptyOrUndefine, isEmptyReturnDefault } from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import arrayUtil from '../../utils/arrayUtil';

const localConstant = getlocalizeData();
const actions = {
    FetchCompanyOffices: (payload) => (
        {
            type: companyActionTypes.FETCH_COMPANY_OFFICES,
            data: payload
        }
    ),
    FetchParentContractNumber: (payload) => (
        {
            type: companyActionTypes.FETCH_PARENT_CONTRACT_NUMBER,
            data: payload
        }
    ),
    FetchParentContractGeneralDetail: (payload) => (
        {
            type: companyActionTypes.FETCH_PARENT_CONTRACT_GENERAL_DETAILS,
            data: payload
        }
    ),
    ClearParentContractGeneralDetail: (payload) => (
        {
            type: companyActionTypes.CLEAR_PARENT_CONTRACT_GENERAL_DETAILS,
            data: payload
        }
    ),
    ClearCustomerList: () => ({
        type: contractActionTypes.CLEAR_CUSTOMER_LIST
    }),

    //From ContractAction.js File (View Components)
    IfCRMYes: () => ({
        type: contractActionTypes.CRM_YES
    }),
    IfCRMNo: () => ({
        type: contractActionTypes.CRM_NO
    }),
    IfCRMSelect: () => ({
        type: contractActionTypes.CRM_SELECT
    }),
    FetchCustomerList: (payload) => ({
        type: customerActionTypes.FETCH_CUSTOMER_LIST,
        data: payload
    }),
    ClearSearchData: () => ({
        type: contractActionTypes.CLEAR_SEARCH_DATA
    }),
    OnSubmitCustomerName: (payload) => ({
        type: contractActionTypes.SELECTED_CUSTOMER_NAME,
        data: payload
    }),
    CustomerShowModal: () => ({
        type: contractActionTypes.CUSTOMER_SHOW_MODAL
    }),
    CustomerHideModal: () => ({
        type: contractActionTypes.CUSTOMER_HIDE_MODAL
    }),
    AddUpdateGeneralDetails: (payload) => ({
        type: contractActionTypes.ADD_UPDATE_GENERAL_DETAILS,
        data: payload
    }),
    SelectedContractType: (payload) => ({
        type: contractActionTypes.SELECTED_CONTRACT_TYPE,
        data: payload
    })
};

/**
 * Fetch company offices
 */
export const FetchCompanyOffices = () => async (dispatch, getstate) => {
    const state = getstate();
    let selectedCompany = "";
    if (state.RootContractReducer.ContractDetailReducer.currentPage === "Create Contract") {
        selectedCompany = state.appLayoutReducer.selectedCompany;
    }
    else {
        selectedCompany = state.RootContractReducer.ContractDetailReducer.selectedCustomerData.contractHoldingCompanyCode;
    }
    const CompanyOfficeURL = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails + '/' + selectedCompany + '/' + companyAPIConfig.companyOffices;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(CompanyOfficeURL, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPANY_OFFICE, 'dangerToast genDetActCmpnyOfc');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const resultSet = isEmptyReturnDefault(response.result,'array');
        const finalResult = arrayUtil.sort(resultSet, 'officeName', 'asc');
        dispatch(actions.FetchCompanyOffices(finalResult));
    }
};

export const FetchParentContractNumber = (contractType, contractCustomerCode) => async (dispatch, getstate) => {
    const params = {
        'contractCustomerCode': contractCustomerCode,
        'contractType': contractType
    };
    const parentContractNumber = homeAPIConfig.homeBaseURL + homeAPIConfig.contract;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(parentContractNumber, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActPrntContNum');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchParentContractNumber(response.result));
    }
};
export const FetchParentContractGeneralDetail = (parentContractNumber, contractType, contractCode) => async (dispatch, getstate) => {    
    dispatch(ShowLoader());
    const params = {
        'contractNumber': parentContractNumber
    };
    const contractDetails = getstate().RootContractReducer.ContractDetailReducer.contractDetail;
    const currentPage = getstate().RootContractReducer.ContractDetailReducer.currentPage;
    const parentContractGeneralDetailsUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;// + "?contractNumber=" + contractNumber; //"http://192.168.50.205:5100/contracts/detail?contractNumber="+contractNumber;
    // const parentContractGeneralDetailsUrl = homeAPIConfig.homeBaseURL + homeAPIConfig.contract;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(parentContractGeneralDetailsUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActPrntContGenDet');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        const parentContractData = response;
        parentContractData.ContractInfo.recordStatus = "N";
        parentContractData.ContractInfo.contractStatus = contractDetails.ContractInfo.contractStatus;  // Not to Copy Contract Status from Parent or Framework Contract.
        if (contractType == 'CHD') {
            parentContractData.ContractInfo.isChildContract = true;
            parentContractData.ContractInfo.parentContractNumber = parentContractData.ContractInfo.contractNumber;
            parentContractData.ContractInfo.parentContractHolder = parentContractData.ContractInfo.contractHoldingCompanyName;
            parentContractData.ContractInfo.contractType = contractType;
            parentContractData.ContractInfo.contractInvoicingCompanyCode = parentContractData.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractInfo.contractInvoicingCompanyName = parentContractData.ContractInfo.contractHoldingCompanyName;
            parentContractData.ContractInfo.parentCompanyCode = parentContractData.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractNotes=null;
            parentContractData.ContractDocuments=null;
            if(parentContractData.ContractInfo.contractHoldingCompanyCode !==  contractDetails.ContractInfo.contractHoldingCompanyCode)
            {
            parentContractData.ContractInfo.contractSalesTax=null;
            parentContractData.ContractInfo.contractWithHoldingTax=null;
            parentContractData.ContractInfo.contractInvoiceFooterIdentifier=null;
            parentContractData.ContractInfo.contractInvoiceRemittanceIdentifier=null;
            }
            parentContractData.ContractInfo.contractHoldingCompanyCode= contractDetails.ContractInfo.contractHoldingCompanyCode;
        
        }
        else if (contractType == 'IRF') {
            parentContractData.ContractInfo.isRelatedFrameworkContract = true;
            parentContractData.ContractInfo.frameworkContractNumber = parentContractData.ContractInfo.contractNumber;
            parentContractData.ContractInfo.frameworkContractHolder = parentContractData.ContractInfo.contractHoldingCompanyName;
            parentContractData.ContractInfo.contractType = contractType;
            parentContractData.ContractInfo.contractInvoicingCompanyCode = contractDetails.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractInfo.contractInvoicingCompanyName = contractDetails.ContractInfo.contractHoldingCompanyName;
            parentContractData.ContractInfo.frameworkCompanyCode = parentContractData.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractNotes=null;
            parentContractData.ContractDocuments=null;
            parentContractData.ContractInfo.contractHoldingCompanyCode= contractDetails.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractInfo.contractHoldingCompanyName= contractDetails.ContractInfo.contractHoldingCompanyName;
            
        }
        else {
            parentContractData.ContractInfo.contractHoldingCompanyCode = contractDetails.ContractInfo.contractHoldingCompanyCode;
            parentContractData.ContractInfo.contractHoldingCompanyName = contractDetails.ContractInfo.contractHoldingCompanyName;
            parentContractData.ContractInfo.contractCustomerCode = contractDetails.ContractInfo.contractCustomerCode;
            parentContractData.ContractInfo.contractCustomerName = contractDetails.ContractInfo.contractCustomerName;
        }
        parentContractData.ContractInfo.contractNumber = null;
        parentContractData.ContractInfo.isParentContract = false;
        parentContractData.ContractInfo.isFrameworkContract = false;

        if (currentPage === "Create Contract" && contractType == 'IRF') {
            parentContractData.ContractInfo.contractInvoicePaymentTerms = null;
            parentContractData.ContractInfo.contractCustomerContact = null;
            parentContractData.ContractInfo.contractCustomerContactAddress = null;
            parentContractData.ContractInfo.contractCustomerInvoiceContact = null;
            parentContractData.ContractInfo.contractCustomerInvoiceAddress = null;
            parentContractData.ContractInfo.contractInvoiceRemittanceIdentifier = null;
            parentContractData.ContractInfo.contractSalesTax = null;
            parentContractData.ContractInfo.contractInvoicingCurrency = null; // UnCommented for IGO QC D-872
            parentContractData.ContractInfo.contractInvoiceGrouping = null;
            parentContractData.ContractInfo.contractInvoiceFooterIdentifier = null;
        }

        if (parentContractData.ContractExchangeRates) {
            parentContractData.ContractExchangeRates.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.contractNumber = null;
                    iteratedValue.lastModification =null;    //Added for Sync Issue
                }
            });
        }
        if (parentContractData.ContractInvoiceAttachments) {
            parentContractData.ContractInvoiceAttachments.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.contractNumber = null;
                }
            });
        }
        if (parentContractData.ContractInvoiceReferences) {
            parentContractData.ContractInvoiceReferences.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.contractNumber = null;
                }
            });
        }
        parentContractData.ContractSchedules = arrayUtil.sort(parentContractData.ContractSchedules,'scheduleName','asc'); //D789
        if (parentContractData.ContractSchedules) {
            const sequenceId = 1;
            parentContractData.ContractSchedules.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.id = null;
                    iteratedValue.contractNumber = null;
                    iteratedValue.sequenceId = sequenceId + 1;
                    /*AddScheduletoRF*/
                    if(contractType == 'IRF')
                    {
                        iteratedValue.baseScheduleId=iteratedValue.scheduleId;
                    }
                }
            });
        }
        if (parentContractData.ContractScheduleRates) {
            parentContractData.ContractScheduleRates.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.id = null;
                    iteratedValue.standardChargeSchedule = null;
                    iteratedValue.standardChargeScheduleInspectionGroup = null;
                    iteratedValue.standardChargeScheduleInspectionType = null;
                    iteratedValue.standardInspectionTypeChargeRateId = null;
                    iteratedValue.contractNumber = null;
                    /*AddScheduletoRF*/
                    if(contractType == 'IRF')
                    {
                        iteratedValue.baseRateId=iteratedValue.rateId;
                        if(parentContractData.ContractSchedules)
                    {
                        parentContractData.ContractSchedules.forEach( item=> {
                            if(item.scheduleName==iteratedValue.scheduleName)
                              {
                                  iteratedValue.baseScheduleId=item.scheduleId;
                                }
    
                        } );
                        }
                    }
                }
            });
        }
        if (parentContractData.ContractNotes) {
            parentContractData.ContractNotes.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.contractNumber = null;
                }
            });
        }
        if (parentContractData.ContractDocuments) {
            parentContractData.ContractDocuments.map(iteratedValue => {
                if (iteratedValue.recordStatus == null) {
                    iteratedValue.recordStatus = 'N';
                    iteratedValue.contractNumber = null;
                }
            });
        }        
        dispatch(actions.FetchParentContractGeneralDetail({}));
        dispatch(actions.FetchParentContractGeneralDetail(parentContractData));
        dispatch(HideLoader());
    }
};

export const FetchParentContractDetail = (parentContractNumber) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const params = {
        'contractNumber': parentContractNumber
    };
    const contractDetails = getstate().RootContractReducer.ContractDetailReducer.contractDetail;
    const currentPage = getstate().RootContractReducer.ContractDetailReducer.currentPage;
    const parentContractGeneralDetailsUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;// + "?contractNumber=" + contractNumber; //"http://192.168.50.205:5100/contracts/detail?contractNumber="+contractNumber;
    // const parentContractGeneralDetailsUrl = homeAPIConfig.homeBaseURL + homeAPIConfig.contract;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(parentContractGeneralDetailsUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT, 'dangerToast genDetActPrntContGenDet');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    dispatch(HideLoader());
    return response;
};

export const ClearCustomerList = () => (dispatch, getstate) => {
    dispatch(actions.ClearCustomerList());
};

export const ClearParentContractGeneralDetail = () => (dispatch) => {
    dispatch(actions.ClearParentContractGeneralDetail());
};

//From ContractAction.js File (View Components)
export const IfCRMYes = () => (dispatch) => {
    dispatch(actions.IfCRMYes());
};
export const IfCRMNo = () => (dispatch) => {
    dispatch(actions.IfCRMNo());
};
export const IfCRMSelect = () => (dispatch) => {
    dispatch(actions.IfCRMSelect());
};
export const CustomerShowModal = () => (dispatch) => {
    dispatch(actions.CustomerShowModal());
};
export const CustomerHideModal = () => (dispatch) => {
    dispatch(actions.CustomerHideModal());
};
export const OnSubmitCustomerName = (data) => (dispatch) => {
    dispatch(actions.OnSubmitCustomerName(data));
};
export const FetchCustomerList = (data) => async (dispatch) => {
    dispatch(actions.FetchCustomerList(null));
    let apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails + '?';
    if (data.customerName !== undefined) {
        apiUrl += "customerName=" + data.customerName + '&';
    }
    if (data.operatingCountry !== undefined) {
        apiUrl += "operatingCountry=" + data.operatingCountry + '&';
    }
    apiUrl = apiUrl.slice(0, -1);
    const params = {};
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

export const FetchCustomerContacts = (data) => async (dispatch, getstate) => {
    const customerContacts = contractAPIConfig.contractBaseUrl + contractAPIConfig.customers + data + contractAPIConfig.contacts;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(customerContacts, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_CONTACTS, 'dangerToast genDetActCustContr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        return response.result;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustContrSmtWrng');
    }
};

export const AddUpdateGeneralDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = Object.assign({}, state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo, data);
    dispatch(actions.AddUpdateGeneralDetails(modifiedData));
};

export const SelectedContractType = (data) => (dispatch) => {
    dispatch(actions.SelectedContractType(data));
};

export const FetchInvoicingDefaultForContract = (data) =>async (dispatch,getstate) => {
    // const apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customer + data + "/" + customerAPIConfig.customerDetail;
    dispatch(AddNewInvoiceDefault([]));       // ITK D-782 Fix
    const apiUrl = StringFormat(customerAPIConfig.customerReference, data);
    const loginUser = getstate().appLayoutReducer.loginUser;
    const contractDetails = getstate().RootContractReducer.ContractDetailReducer.contractDetail;
    const invoiceDefaultReferences = [];
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_INVOICEDEFAULT_FOR_CONTRACT, 'dangerToast genDetActCustContr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if(response){
        /**
         * dispatch(AddNewInvoiceDefault(null)); this line is added refresh the 
         * contract references on company change
         */
        if(isEmptyOrUndefine(contractDetails.ContractInvoiceReferences))
        {        
        if(response.result && response.result.length > 0){
            response.result.forEach((iteratedValue,index) => {
                const invoiceDefault = {};
                invoiceDefault.referenceType = iteratedValue.assignmentRefType;
                invoiceDefault.recordStatus = "N";
                invoiceDefault.modifiedBy = loginUser;
                invoiceDefault.contractInvoiceReferenceTypeId = Math.floor(Math.random() * (Math.pow(10, 5)));
                invoiceDefault.displayOrder = index + 1;
                invoiceDefault.isVisibleToTimesheet = false;
                invoiceDefault.isVisibleToAssignment = false;
                invoiceDefault.isVisibleToVisit = false;

                invoiceDefaultReferences.push(invoiceDefault);
            });
          }
            dispatch(AddNewInvoiceDefault(invoiceDefaultReferences));
        }
    }
    else{
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustContrSmtWrng');
    }
};