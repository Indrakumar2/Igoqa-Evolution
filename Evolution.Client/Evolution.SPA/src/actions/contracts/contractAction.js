import { contractActionTypes } from '../../constants/actionTypes';
import { SuccessAlert, FailureAlert, DeleteAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import { FetchData,PostData,CreateData } from '../../services/api/baseApiService';
import { contractAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { processApiRequest } from '../../services/api/defaultServiceApi';
import { ShowLoader, HideLoader,SetCurrentPageMode,UpdateCurrentPage, UpdateCurrentModule } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, getlocalizeData,isUndefined, isEmptyReturnDefault,FilterSave,parseValdiationMessage,getNestedObject } from '../../utils/commonUtils';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
import { UpdateSelectedCompany } from '../../components/appLayout/appLayoutActions';
import moment from 'moment';
import { formatToDecimal } from '../../utils/commonUtils';
import { SetDefaultCustomerName } from '../../components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { ClearAdminContractRates } from './rateScheduleAction';
import { FetchInvoicingDefaults } from './invoicingDefaultsAction';
import { 
        FetchCustomerDocumentsofContracts,
        FetchProjectDocumentsofContracts } 
from './documentAction';
import arrayUtil from '../../utils/arrayUtil';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';

const localConstant = getlocalizeData();

const actions = {
    FetchContractData: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.FETCH_CONTRACT_DATA,
            data: payload
        }
    ),
    FetchContractDataForAssignment: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.FETCH_CONTRACT_DATA_ASSIGNMENT,
            data: payload
        }
    ),
    SetContractData: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.SET_CONTRACT_DATA,
            data: payload
        }
    ),
    UpdateInteractionMode: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.UPDATE_INTERACTION_MODE,
            data: payload
        }
    ),
    GetCurrentPage: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.CURRENT_PAGE,
            data: payload
        }
    ),
    GetSelectedContract: (payload) => (
        {
            type: contractActionTypes.CONTRACT_SELECTED_CONTRACT_NUMBER,
            data: payload
        }
    ),
    //SAVE
    SaveContractDetails: (payload) => ({
        type: contractActionTypes.SAVE_CONTRACT_DETAILS,
        data: payload
    }),
    //DELETE
    DeleteContractDetails: (payload) => ({
        type: contractActionTypes.DELETE_CONTRACT_DETAILS,
        data: payload
    }),

    CancelSearchDetails: (payload) => ({
        type: contractActionTypes.CLEAR_CONTRACT_DETAILS
    }),
    /*AddScheduletoRF*/
    AddShedudletoIFContract:(payload)=>(
        {
            type:contractActionTypes.ADD_SCHEDULE_TO_IF_CONTRACT,
            data:payload 
        }
    ),
    SetSelectedCustomerName: (payload) => ({
        type: contractActionTypes.CONTRACT_SELECTED_CUSTOMER_CODE,
        data: payload
    }),
};
export const GetSelectedContract = (data) => (dispatch, getstate) => {  
    dispatch(actions.GetSelectedContract(data));
};
/**
 * Action to fetch the selected contract detail data
 * @param {*Object} data 
 */
export const FetchContractDataForAssignment = (contractNumber) => async (dispatch) => {
    dispatch(ShowLoader());
    const fetchUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;
    const params = {
        'contractNumber': decodeURIComponent(contractNumber),
        'IsInvoiceDetailRequired': true
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        response.ContractInfo = Object.assign({}, response.ContractInfo);
        response.ContractInfo.contractBudgetMonetaryValue = formatToDecimal(response.ContractInfo.contractBudgetMonetaryValue, 2);
        response.ContractInfo.contractBudgetHours = formatToDecimal(response.ContractInfo.contractBudgetHours, 2);
        response.ContractInfo.contractRemainingBudgetValue = formatToDecimal(response.ContractInfo.contractRemainingBudgetValue, 2);
        response.ContractInfo.contractInvoicedToDate = formatToDecimal(response.ContractInfo.contractInvoicedToDate, 2);
        response.ContractInfo.contractUninvoicedToDate = formatToDecimal(response.ContractInfo.contractUninvoicedToDate, 2);
        response.ContractInfo.contractHoursInvoicedToDate = formatToDecimal(response.ContractInfo.contractHoursInvoicedToDate, 2);

        response.ContractInfo.contractHoursUninvoicedToDate = formatToDecimal(response.ContractInfo.contractHoursUninvoicedToDate, 2);
        response.ContractInfo.contractRemainingBudgetHours = formatToDecimal(response.ContractInfo.contractRemainingBudgetHours, 2);
        if (response.ContractInfo.parentContractDiscount !== null) { //Sanity Defect 23
            response.ContractInfo.parentContractDiscount = formatToDecimal(response.ContractInfo.parentContractDiscount, 2);
        }

        const resultData = {
            customerName: response.ContractInfo.contractCustomerName,
            cutomerCode: response.ContractInfo.contractCustomerCode
        };

        dispatch(actions.FetchContractDataForAssignment(response));
        dispatch(HideLoader());
        return response;
    }

};
export const FetchContractData = (data, isScheduleSort) => async (dispatch, getstate) => {    
    // TODO: dispatch action to update the store with contract data for every individual reducer
    dispatch(ShowLoader());
    let contractNumber = getstate().RootContractReducer.ContractDetailReducer.selectedContract.contractNumber;
    let company = getstate().appLayoutReducer.selectedCompany;
    let contractSchedules=getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules;
    if(data && data.contractNumber && isEmpty(contractNumber)){
        contractNumber=data.contractNumber;
    }
    if(data && data.selectedCompany){
        dispatch(UpdateSelectedCompany({ companyCode:data.selectedCompany }));
        company = data.selectedCompany;
    }
    // else{
    //     contractNumber = getstate().RootContractReducer.ContractDetailReducer.selectedCustomerData.contractNumber;
    // }
    const fetchUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;
    const params = {
        'contractNumber': decodeURIComponent(contractNumber),
        'IsInvoiceDetailRequired': true
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.contract.CONTRACT_DATA_fECTH_FAILED, 'dangerToast conActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        
         //646 Fixes
         if (!isEmpty(response) && isEmpty(response.ContractInfo)){
            dispatch(HideLoader());
            dispatch(ChangeDataAvailableStatus(true));
            return response;
        }

        response.ContractInfo=Object.assign({},response.ContractInfo);
        response.ContractInfo.contractBudgetMonetaryValue = formatToDecimal(response.ContractInfo.contractBudgetMonetaryValue,2);
        response.ContractInfo.contractBudgetHours = formatToDecimal(response.ContractInfo.contractBudgetHours,2);
        response.ContractInfo.contractRemainingBudgetValue = formatToDecimal(response.ContractInfo.contractRemainingBudgetValue,2);
        response.ContractInfo.contractInvoicedToDate = formatToDecimal(response.ContractInfo.contractInvoicedToDate,2);
        response.ContractInfo.contractUninvoicedToDate = formatToDecimal(response.ContractInfo.contractUninvoicedToDate,2);
        response.ContractInfo.contractHoursInvoicedToDate = formatToDecimal(response.ContractInfo.contractHoursInvoicedToDate,2);

        response.ContractInfo.contractHoursUninvoicedToDate = formatToDecimal(response.ContractInfo.contractHoursUninvoicedToDate,2);
        response.ContractInfo.contractRemainingBudgetHours = formatToDecimal(response.ContractInfo.contractRemainingBudgetHours,2);
        if(response.ContractInfo.parentContractDiscount !== null){ //Sanity Defect 23
            response.ContractInfo.parentContractDiscount = formatToDecimal(response.ContractInfo.parentContractDiscount,2);
        }
        if(isScheduleSort){
            response.ContractSchedules = arrayUtil.sort(response.ContractSchedules,'scheduleName','asc');
        }
        else{
            contractSchedules = arrayUtil.gridTopSort(contractSchedules);
            contractSchedules.forEach((schedule, index) => {
                if(schedule.recordStatus !== null && schedule.recordStatus !== 'D'){
                    const scheduleIndex = response.ContractSchedules.findIndex(x => x.scheduleName === schedule.scheduleName);
                    if(scheduleIndex>=0){
                        contractSchedules[index] = response.ContractSchedules[scheduleIndex];  
                    }
                }
                else if(schedule.recordStatus === 'D'){
                    contractSchedules.splice(index,1);
                }
            });
            response.ContractSchedules = contractSchedules;
        }
        dispatch(actions.FetchContractData(response));

        if (response.ContractInfo && response.ContractInfo.contractHoldingCompanyCode === company && response.ContractInfo.isContractHoldingCompanyActive) {
            dispatch(GetCurrentPage('Edit Contract'));
            dispatch(UpdateInteractionMode(false));
            dispatch(SetCurrentPageMode());
        }
        else {
            dispatch(GetCurrentPage('View Contract'));
            dispatch(UpdateInteractionMode(true));
            dispatch(SetCurrentPageMode("View"));
        }
        const resultData = { 
                            customerName:response.ContractInfo.contractCustomerName , 
                            cutomerCode : response.ContractInfo.contractCustomerCode 
                        };
                        dispatch(UpdateCurrentPage(localConstant.contract.EDIT_VIEW_CONTRACT));
                        dispatch(UpdateCurrentModule(localConstant.moduleName.CONTRACT));
        dispatch(SetDefaultCustomerName(resultData));
        dispatch(HideLoader());
        return response;
    }
    else {
        IntertekToaster(localConstant.contract.SOME_THING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
    }
    dispatch(HideLoader());
};

export const CancelContractDataOnEdit = () => async (dispatch, getstate) => {
    const documentData = getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments;
    const contractNumber = getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo.contractNumber;
    const deleteUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contractDocuments + contractNumber;
    if (!isEmpty(documentData)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    const contractSuccess = await dispatch(FetchContractData({ contractNumber:contractNumber },true)); //Added fixes for D651
    // dispatch(FetchContractData({ contractNumber:contractNumber },true));  //Added true for Sanity Defect 73
    dispatch(ClearAdminContractRates());
    dispatch(FetchInvoicingDefaults());
    dispatch(FetchCustomerDocumentsofContracts());
    dispatch(FetchProjectDocumentsofContracts());
    return contractSuccess;
};

export const ClearContractDocument = () => async (dispatch, getstate) => {
    const state = getstate();
    const contractNumber = getNestedObject(state.RootContractReducer.ContractDetailReducer.contractDetail,[ "ContractInfo","contractNumber" ]);
    const documentData = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments;
    // const contractNumber = contractInfo.contractNumber;
    const deleteUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contractDocuments + contractNumber;
    let response = null;
    if (!isEmpty(documentData) && !isUndefined(contractNumber)) {
       response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response; 
};

/**
 * check mode of view
 * @param {string} data
 */
export const UpdateInteractionMode = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateInteractionMode(data));
};
export const CancelSearchDetails = () => (dispatch) => {
    dispatch(actions.CancelSearchDetails());
    dispatch(FetchInvoicingDefaults());
    dispatch(FetchCustomerDocumentsofContracts());
    dispatch(FetchProjectDocumentsofContracts());
};

export const GetCurrentPage = (data) => (dispatch) => {
    dispatch(actions.GetCurrentPage(data));
};

//POST CONTRACT DATA
export const SaveContractDetails = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    let contractData = state.RootContractReducer.ContractDetailReducer.contractDetail;
    const currentPage = state.RootContractReducer.ContractDetailReducer.currentPage;

    // if (contractData.ContractInfo.isCRM === "true") {
    //     contractData.ContractInfo.contractCRMReason = null;
    // }
    // else if (contractData.ContractInfo.isCRM === "false") {
    //     contractData.ContractInfo.contractConflictOfInterest = null;
    //     contractData.ContractInfo.contractCRMReference = null;
    // }

    if (contractData.ContractInfo.contractType == 'FRW') {
        if (state.RootContractReducer.ContractDetailReducer.currentPage === "Create Contract") {
            const contractInvoicePaymentTerms = isEmptyReturnDefault(state.masterDataReducer.invoicePaymentTerms);
            const customerContractAddress = isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.customerContractAddress);
            const invoiceCurrency = isEmptyReturnDefault(state.masterDataReducer.currencyMasterData);
            const invoiceRemittance = isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.invoiceRemittanceandFooterText.invoiceRemittances);
            const invoiceFooter = isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.invoiceRemittanceandFooterText.invoiceFooters);
            const customerContractContact = isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.customerContractContact);
            const taxes = isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.taxes);
            contractData.ContractInfo.contractInvoicePaymentTerms = contractInvoicePaymentTerms[0] && contractInvoicePaymentTerms[0].name;
            contractData.ContractInfo.contractCustomerContactAddress = customerContractAddress[0] && customerContractAddress[0].address;
            contractData.ContractInfo.contractCustomerInvoiceAddress = customerContractAddress[0] && customerContractAddress[0].address;
            contractData.ContractInfo.invoiceCurrency = invoiceCurrency[0] && invoiceCurrency[0].code;
            contractData.ContractInfo.contractInvoicingCurrency = invoiceCurrency[0] && invoiceCurrency[0].code;
            contractData.ContractInfo.contractInvoiceFooterIdentifier = invoiceFooter[0] && invoiceFooter[0].msgIdentifier;
            contractData.ContractInfo.contractInvoiceRemittanceIdentifier = invoiceRemittance[0] && invoiceRemittance[0].msgIdentifier;
            contractData.ContractInfo.contractInvoiceGrouping = 'Project';
            contractData.ContractInfo.contractCustomerInvoiceContact = customerContractContact[0] && customerContractContact[0].contactPersonName;
            contractData.ContractInfo.contractCustomerContact = customerContractContact[0] && customerContractContact[0].contactPersonName;
            contractData.ContractInfo.contractSalesTax = taxes[0] && taxes[0].taxName;
        }
    }
    if (contractData.ContractInfo.contractType && contractData.ContractInfo.contractType !=='CHD') {  //Added for Sanity Defect on Parent/Child Scenario
        contractData.ContractInfo.contractInvoicingCompanyCode = contractData.ContractInfo.contractHoldingCompanyCode;
        contractData.ContractInfo.contractInvoicingCompanyName = contractData.ContractInfo.contractHoldingCompanyName;
    }
    else{
        contractData.ContractInfo.contractInvoicingCompanyCode = contractData.ContractInfo.parentCompanyCode; //Added for Sanity Defect 193
        contractData.ContractInfo.contractInvoicingCompanyName = contractData.ContractInfo.parentContractHolder;//Added for Sanity Defect 193
    }

    if (!isEmpty(contractData.ContractInfo.contractStartDate)) {
        contractData.ContractInfo.contractStartDate = moment(contractData.ContractInfo.contractStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    if (!isEmpty(contractData.ContractInfo.contractEndDate)) {
        contractData.ContractInfo.contractEndDate = moment(contractData.ContractInfo.contractEndDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    }

    if (contractData.ContractInvoiceReferences) {
        contractData.ContractInvoiceReferences.map((row, index) => {
            if (row.recordStatus === "N") {
                row.contractInvoiceReferenceTypeId = 0;
            }
            //Need to check below logic  //Changes for D1015
            // if (!isEmpty(row.recordStatus) && row.displayOrder - 1 !== index) {
            //  //   row.displayOrder = index + 1;
            //     if (row.recordStatus !== "N" && row.recordStatus !== "D") {
            //         row.recordStatus = "M";
            //     }
            // }
        });
    }
    if (contractData.ContractDocuments) {
        contractData.ContractDocuments.map(row => {
            if (!isEmpty(row.status)) {
                row.status = row.status.trim();
            }
            if (row.recordStatus === "N") {
                row.id = 0;
            }
        });
    }
    if (contractData.ContractNotes) {
        contractData.ContractNotes.forEach(row => {
            if (row.recordStatus === "N") {
                row.contractNoteId = 0;
            }
        });
    }

    if (contractData.ContractInvoiceAttachments) {
        contractData.ContractInvoiceAttachments.map(row => {
            if (row.recordStatus === "N") {
                row.contractInvoiceAttachmentId = 0;
            }
        });
    }

    if (contractData.ContractSchedules) {
        contractData.ContractSchedules.map(row => {
            if (row.recordStatus === "N") {
                row.scheduleId = 0;
            }
        });
    }

    if (contractData.ContractScheduleRates) {
        contractData.ContractScheduleRates.map(row => {
            if (!isEmpty(row.effectiveFrom)) {
                row.effectiveFrom = moment(row.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            }
            if (!isEmpty(row.effectiveTo)) {
                row.effectiveTo = moment(row.effectiveTo).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            }
            if (row.recordStatus === "N") {
                row.rateId = 0;
            }
        });
    }

    if (contractData.ContractExchangeRates) {
        contractData.ContractExchangeRates.map(row => {
            if (!isEmpty(row.effectiveFrom)) {
                row.effectiveFrom = moment(row.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            }
            if (row.recordStatus === "N") {
                row.exchangeRateId = 0;
            }
        });
    }
let res=null;
    if (currentPage === "Create Contract") {
        contractData.ContractInfo.recordStatus = 'N';
        contractData.ContractInfo.createdBy = state.appLayoutReducer.loginUser;
        //590 - CONTRACT-Inv Defaults-Use Parent Contract Details- UAT Testing
        // contractData.ContractInfo.IsParentContractInvoiceUsed = (contractData.ContractInfo.parentCompanyOffice && contractData.ContractInfo.parentCompanyOffice != null) ? true : false;
        res = await dispatch(CreateContractDetails(contractData));
        dispatch(ClearAdminContractRates());
    }
    if (currentPage === "Edit Contract") {
        contractData.ContractInfo.recordStatus = 'M';
        contractData.ContractInfo.modifiedBy = state.appLayoutReducer.loginUser;
        //590 - CONTRACT-Inv Defaults-Use Parent Contract Details- UAT Testing
        //contractData.ContractInfo.IsParentContractInvoiceUsed = contractData.ContractInfo.isParentContract;
        contractData = FilterSave(contractData);
        res = await dispatch(UpdateContractDetails(contractData));
        dispatch(ClearAdminContractRates());
    }
    return res;
};

export const CreateContractDetails = (payload) => async (dispatch, getstate) => {
    const postContractUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;

    const requestPayload = new RequestPayload(payload);
    const response = await PostData(postContractUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.contract.CREATE_CONTRACT_ERROR, 'dangerToast careatContractErrorWhileCreating');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response&&response.code == 1) {
        const contractSuccess = await dispatch(FetchContractData(response.result));
        dispatch(actions.SetSelectedCustomerName(response.result));
        dispatch(CreateAlert(response.data, "Contract"));
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssigntSearchResultsErr');
    }
    else {
        dispatch(FailureAlert(response.data, "Contract"));
    }
    dispatch(HideLoader());
    return response;
};

export const UpdateContractDetails = (payload) => async (dispatch, getstate) => {
    const postContractUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;

    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(postContractUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.contract.UPDATE_CONTRACT_ERROR, 'dangerToast updateContractErrorWhileUpdating');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response&&response.code == 1) {
        dispatch(SuccessAlert(response, "Contract"));
        const contractSuccess = await dispatch(FetchContractData(response.result));
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchAssigntSearchResultsErr');
    }
    else {
        dispatch(FailureAlert(response.data, "Contract"));
    }
    dispatch(HideLoader());
    return response;
};

//Delete Contract
export const DeleteContractDetails = () => (dispatch, getstate) => {
    dispatch(ShowLoader());
    const contractData = getstate().RootContractReducer.ContractDetailReducer.contractDetail;
    contractData.ContractInfo.recordStatus = "D";
    const deleteContractUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;
    return processApiRequest(deleteContractUrl, {
        method: "DELETE",
        data: contractData,
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Acces-Control-Allow-Origin": "*"
        }
    }).then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(DeleteAlert(response.data, "Contract"));
                dispatch(HideLoader());
                //dispatch(FetchContractData());
                //this.props.history.push('/editContracts');
                return response.data;
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast DeleteContractWentWrong');  //D669 issue -9
            }
            dispatch(HideLoader());
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.contract.DELETE_CONTRACT_ERROR, 'dangerToast DeleteContractErrorWhileDelete');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};
//Copy Schedule to RF contract
/*AddScheduletoRF*/
export const AddShedudletoIFContract = (payload) => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const postContractUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.rateSchedule;
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(postContractUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.contract.UPDATE_CONTRACT_ERROR, 'dangerToast updateContractErrorWhileUpdating');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) 
    {
        if (response.code == 1) 
        {
             IntertekToaster(localConstant.contract.rateSchedule.ADD_TO_RF, 'successToast alertActSuccAlt');     
        }  
     }
     dispatch(HideLoader());
};

/** Clear Contract related Details */
export const ClearContractData = (payload) => (dispatch,getstate) => {
    dispatch(UpdateInteractionMode(false));
};