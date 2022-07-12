import { processApiRequest } from '../../../services/api/defaultServiceApi';
import { companyAPIConfig, masterData, RequestPayload } from '../../../apiConfig/apiConfig';
import { companyActionTypes } from '../../../constants/actionTypes';
import { SuccessAlert, FailureAlert, ValidationAlert } from '../customer/alertAction';
import { SetLoader, RemoveLoader } from '../customer/customerAction';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { isEmpty, isEmptyReturnDefault, getlocalizeData, parseValdiationMessage, FilterSave, isUndefined, formatToDecimal, isEmptyOrUndefine } from '../../../utils/commonUtils';
import { ReplaceString } from '../../../utils/stringUtil';
import objectUtil from '../../../utils/objectUtil';
import { FetchData, PostData, CreateData } from '../../../services/api/baseApiService';
import { RemoveDocumentsFromDB, ShowLoader, HideLoader, SetCurrentPageMode, UpdateCurrentModule } from '../../../common/commonAction';
import { UpdateSelectedCompany } from '../../appLayout/appLayoutActions';
import sanitize from 'sanitize-html';
import { FetchCityId, FetchStateId } from '../../../common/masterData/masterDataActions';
const localConstant = getlocalizeData();
const actions = {
    showHidePanel: (payload) => ({
        type: companyActionTypes.SHOW_HIDE_PANEL,
        data: payload
    }),
    FetchCompanyDataList: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_DATALIST,
        data: payload
    }),

    GetSelectedCompanyCode: (payload) => ({
        type: companyActionTypes.SELECTED_COMPANY_CODE,
        data: payload
    }),

    ClearSearchData: () => ({
        type: companyActionTypes.CLEAR_SEARCH_DATA
    }),
    ClearGridFormSearchData: () => ({
        type: companyActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS
    }),

    AddCompanyOffice: (payload) => ({
        type: companyActionTypes.ADD_COMPANY_OFFICE,
        data: payload
    }),
    UpdateCompanyOffice: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_OFFICE,
        data: payload
    }),
    DeleteCompanyOffices: (payload) => ({
        type: companyActionTypes.DELETE_COMPANY_OFFICES,
        data: payload
    }),
    EditCompanyOffice: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_OFFICE,
        data: payload
    }),
    FetchCompanyExpectedMargin: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_EXPECTED_MARGIN,
        data: payload
    }),
    AddExpectedMargin: (payload) => ({
        type: companyActionTypes.ADD_EXPECTED_MARGIN,
        data: payload
    }),
    DeleteExpectedMargin: (payload) => ({
        type: companyActionTypes.DELETE_EXPECTED_MARGIN,
        data: payload
    }),
    EditExpectedMargin: (payload) => ({
        type: companyActionTypes.EDIT_EXPECTED_MARGIN,
        data: payload
    }),
    UpdateExpectedMargin: (payload) => ({
        type: companyActionTypes.UPDATE_EXPECTED_MARGIN,
        data: payload
    }),
    FetchState: (payload) => ({
        type: companyActionTypes.STATE,
        data: payload
    }),
    FetchCountry: (payload) => ({
        type: companyActionTypes.COUNTRY,
        data: payload
    }),
    FetchCity: (payload) => ({
        type: companyActionTypes.CITY,
        data: payload
    }),
    FetchBusinessUnit: (payload) => ({
        type: companyActionTypes.BUSINESS_UNIT,
        data: payload
    }),
    FetchMarginType: (payload) => ({
        type: companyActionTypes.MARGIN_TYPE,
        data: payload
    }),
    FetchTaxData: (payload) => ({
        type: companyActionTypes.TAX,
        data: payload
    }),
    FetchSalutation: (payload) => ({
        type: companyActionTypes.SALUTATION,
        data: payload
    }),
    FetchVatPrefix: (payload) => ({
        type: companyActionTypes.VAT_PREFIX,
        data: payload
    }),
    CompanyFetchVatPrefix: (payload) => ({
        type: companyActionTypes.COMPANY_VAT_PREFIX,
        data: payload
    }),
    ShowButtonHandler: () => ({
        type: companyActionTypes.SHOWBUTTON
    }),
    FetchMasterCompanyDocumentTypes: (payload) => ({
        type: companyActionTypes.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    CopyDocumentDetails: (payload) => ({
        type: companyActionTypes.COPY_DOCUMENTS_DETAILS,
        data: payload
    }),

    AddDocumentDetails: (payload) => ({
        type: companyActionTypes.ADD_COMPANY_DOCUMENTS_DETAILS,
        data: payload
    }),

    AddFilesToBeUpload: (payload) => ({
        type: companyActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: companyActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    DeleteCompanyDocumentDetails: (payload) => ({
        type: companyActionTypes.DELETE_DOCUMENTS_DETAILS,
        data: payload
    }),
    EditCompanyDocumentDetails: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_DOCUMENTS_DETAILS,
        data: payload
    }),

    UpdateDocumentDetails: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_DOCUMENTS_DETAILS,
        data: payload
    }),

    FetchCompanyContract: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_CONTRACT,
        data: payload
    }),

    DownloadDocumentData: (payload) => ({
        data: payload
    }),
    DispalyDocumentDetails: (payload) => ({
        type: companyActionTypes.DISPLAY_COMPANY_DOCUMENTS,
        data: payload
    }),

    PasteDocumentUploadData: (payload) => ({
        data: payload
    }),
    //General Details Actions
    FetchCompanyDetails: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_DETAILS,
        data: payload
    }),
    AddInvoiceRemittance: (payload) => ({
        type: companyActionTypes.ADD_INVOICE_REMITTANCE,
        data: payload
    }),
    EditInvoiceRemittance: (payload) => ({
        type: companyActionTypes.EDIT_INVOICE_REMITTANCE,
        data: payload
    }),
    UpdateInvoiceRemittance: (payload) => ({
        type: companyActionTypes.UPDATE_INVOICE_REMITTANCE,
        data: payload
    }),
    DeleteInvoiceRemittance: (payload) => ({
        type: companyActionTypes.DELETE_INVOICE_REMITTANCE,
        data: payload
    }),
    AddInvoiceFooter: (payload) => ({
        type: companyActionTypes.ADD_INVOICE_FOOTER,
        data: payload
    }),
    EditInvoiceFooter: (payload) => ({
        type: companyActionTypes.EDIT_INVOICE_FOOTER,
        data: payload
    }),
    UpdateInvoiceFooter: (payload) => ({
        type: companyActionTypes.UPDATE_INVOICE_FOOTER,
        data: payload
    }),
    DeleteInvoiceFooter: (payload) => ({
        type: companyActionTypes.DELETE_INVOICE_FOOTER,
        data: payload
    }),

    //Company Divisions
    FetchCompanyDivision: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_DIVISION,
        data: payload
    }),
    FetchDivisionCostCenter: (payload) => ({
        type: companyActionTypes.FETCH_DIVISION_COST_CENTER,
        data: payload
    }),
    AddCompanyDivision: (payload) => ({
        type: companyActionTypes.ADD_NEW_DIVISION,
        data: payload
    }),
    UpdateDivisionNameInCostCenter: (payload) => ({
        type: companyActionTypes.UPDATE_DIVISION_NAME_IN_COST_CENTER,
        data: payload
    }),
    UpdatePayrollPeriodNameInPayrollPeriod: (payload) => ({
        type: companyActionTypes.UPDATE_PAYROLL_PERIOD_NAME_IN_PAYROLL_PERIOD,
        data: payload
    }),
    AddCompanyDivisionCostcentre: (payload) => ({
        type: companyActionTypes.ADD_NEW_DIVISION_COST_CENTRE,
        data: payload
    }),
    EditCompanyDivisionCostcentre: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_DIVISION_COST_CENTER,
        data: payload
    }),
    UpdateCompanyDivisionCostcentre: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_DIVISION_COST_CENTER,
        data: payload
    }),
    UpdateCompanyDivision: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_DIVISION,
        data: payload
    }),
    DeleteCompanyDivision: (payload) => ({
        type: companyActionTypes.DELETE_COMPANY_DIVISION,
        data: payload
    }),
    UpdateCompanyDivisionButton: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_DIVISION_BUTTON,
        data: payload
    }),
    UpdateCostCentreButton: (payload) => ({
        type: companyActionTypes.UPDATE_COST_CENTRE_BUTTON,
        data: payload
    }),
    DeleteCompanyDivisionCostCentre: (payload) => ({
        type: companyActionTypes.DELETE_DIVISION_COST_CENTRE,
        data: payload
    }),

    AddCompanyNotesDetails: (payload) => ({
        type: companyActionTypes.ADD_COMPANY_NOTE,
        data: payload
    }),

    //Payroll
    FetchPayrollData: (payload) => ({
        type: companyActionTypes.FETCH_PAYROLL_DATA,
        data: payload
    }),
    FetchPayrollPeriodName: (payload) => ({
        type: companyActionTypes.FETCH_PAYROLL_PERIOD_NAME,
        data: payload
    }),
    AddNewPayroll: (payload) => ({
        type: companyActionTypes.ADD_NEW_PAYROLL,
        data: payload
    }),
    AddNewPayrollPeriodName: (payload) => ({
        type: companyActionTypes.ADD_NEW_PAYROLL_PERIOD_NAME,
        data: payload
    }),
    FetchCostSaleReference: (payload) => ({
        type: companyActionTypes.FETCH_COST_SALE_REFERENCE,
        data: payload
    }),
    EditPayrollType: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_PAYROLL,
        data: payload
    }),
    DeletePayrollType: (payload) => ({
        type: companyActionTypes.DELETE_PAYROLL_TYPE,
        data: payload
    }),
    PayrollPopupClear: (payload) => ({
        type: companyActionTypes.PAYROLL_POPUP_CLEAR,
        data: payload
    }),
    UpdateCompanyPayrollButton: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_PAYROLL_BUTTON,
        data: payload
    }),
    UpdateCompanyPayroll: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_PAYROLL,
        data: payload
    }),
    AddNewPeriodName: (payload) => ({
        type: companyActionTypes.ADD_NEWPAYROLL_PERIOD_NAME,
        data: payload
    }),
    EditPayrollPeriodName: (payload) => ({
        type: companyActionTypes.EDIT_PAYROLL_PERIOD_NAME,
        data: payload
    }),
    UpdatePayrollPeriodName: (payload) => ({
        type: companyActionTypes.UPADTE_PAYROLL_PERIOD_NAME,
        data: payload
    }),
    DeletePayrollPeriodName: (payload) => ({
        type: companyActionTypes.DELETE_PAYROLL_PERIOD_NAME,
        data: payload
    }),
    UpdateOverrideCostSaleReference: (payload) => ({
        type: companyActionTypes.UPDATE_OVERRIDE_COST_SALE_REFERENCE,
        data: payload
    }),
    updateAvgTSHourlyCost: (payload) => ({
        type: companyActionTypes.UPDATE_AVG_TS_HOURLY_COST,
        data: payload
    }),
    TogglePayrollPeriodNameButton: (payload) => ({
        type: companyActionTypes.TOGGLE_SUBMIT_BUTTON,
        data: payload
    }),
    //Update the email template in editor
    UpdateEmailTemplate: (payload) => ({
        type: companyActionTypes.UPDATE_EMAIL_TEMPLATE,
        data: payload
    }),
    UpdateEmailTemplateType: (payload) => ({
        type: companyActionTypes.UPDATE_EMAIL_TEMPLATE_TYPE,
        data: payload
    }),
    UpdateCompanyEmailTemplate: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_EMAIL_TEMPLATE,
        data: payload
    }),
    FetchPlaceholders: (payload) => ({
        type: companyActionTypes.FETCH_PLACEHOLDERS,
        data: payload
    }),
    //Company Taxes
    AddCompanyTaxes: (payload) => ({
        type: companyActionTypes.ADD_COMPANY_TAXES,
        data: payload
    }),
    EditCompanyTaxes: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_TAXES,
        data: payload
    }),
    UpdateCompanyTaxes: (payload) => ({
        type: companyActionTypes.UPDATE_COMPANY_TAXES,
        data: payload
    }),
    DeleteCompanyTaxes: (payload) => ({
        type: companyActionTypes.DELETE_COMPANY_TAXES,
        data: payload
    }),
    //POST ACTION
    SaveCompanyDetails: (payload) => ({
        type: companyActionTypes.SAVE_COMPANY_DETAILS,
        data: payload
    }),
    CompanyUpdatedAction: (payload) => ({
        type: companyActionTypes.COMPANY_UPDATED,
        data: payload
    }),
    AddUpdateInvoiceDefaults: (payload) => ({
        type: companyActionTypes.ADD_UPDATE_INVOICE_DEFAULTS,
        data: payload
    }),
    AddUpdateCompanyInfo: (payload) => ({
        type: companyActionTypes.ADD_UPDATE_COMPANY_INFO,
        data: payload
    }),
    ClearStateCityData: () => ({
        type: companyActionTypes.CLEAR_STATE_CITY_DATA
    }),
    ClearCityData: () => ({
        type: companyActionTypes.CLEAR_CITY_DATA
    }),
    ClearCompanyDetails: () => ({
        type: companyActionTypes.CLEAR_COMPANY_DETAILS,
    }),
    FetchCompanyLogoMasterData: (payload) => ({
        type: companyActionTypes.FETCH_COMPANY_LOGO_MASTER_DATA,
        data: payload
    }),
    EditCompanyNotesDetails: (payload) => ({
        type: companyActionTypes.EDIT_COMPANY_NOTE,
        data: payload
    }),
    UpdateSelectedPayrollData: (payload) => ({
        type: companyActionTypes.UPDATE_SELECTED_PAYROLL_DATA,
        data: payload
    }),
    UpdateSelectedDivisionData: (payload) => ({
        type: companyActionTypes.UPDATE_SELECTED_DIVISION_DATA,
        data: payload
    }),
};

export const ClearCityData = () => (dispatch) => {
    dispatch(actions.ClearCityData());
};

export const ApiRequest = (dispatch, response) => {

    if (response.status === 200) {
        dispatch(actions.FetchCompanyOffice(response.data.result));

    } else {
        IntertekToaster(localConstant.companyDetails.common.FETCH_COMPANY_API_WENT_WRONG, 'dangerToast FetchCompanyOfficeAPI');

    }
};
export const showHidePanel = () => (dispatch) => {
    dispatch(actions.showHidePanel());
};

export const FetchCompanyDataList = (data) => (dispatch) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCompanyDataList([]));
    let apiUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails + '?';
    if (data.companyCode !== undefined) {
        apiUrl += "companyCode=" + data.companyCode + '&';
    }
    if (data.companyName !== undefined) {
        apiUrl += "companyName=" + data.companyName + '&';
    }
    if (data.operatingCountry !== undefined) {
        apiUrl += "operatingCountry=" + data.operatingCountry + '&';
    }
    if (data.searchDocumentType !== undefined) {
        apiUrl += "searchDocumentType=" + data.searchDocumentType + '&';
    }
    if (data.documentSearchText !== undefined && data.documentSearchText !== '') {
        const searchText = ReplaceString(data.documentSearchText, '+', '%2B');
        apiUrl += "documentSearchText=" + searchText + '&';
    }

    apiUrl = apiUrl.slice(0, -1);
    processApiRequest(apiUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCompanyDataList(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchCompanyListAPI');
        }
        dispatch(HideLoader());
    }).catch(error => {
        // console.error(error); // To show the error details in console
        IntertekToaster(error, 'successToast errorUnique3');
        dispatch(HideLoader());
    });

};
export const GetSelectedCompanyCode = (data) => (dispatch, getstate) => {
    dispatch(actions.GetSelectedCompanyCode(data));
    dispatch(SetCurrentPageMode());
};

export const ClearSearchData = () => (dispatch) => {
    dispatch(actions.ClearSearchData());
};

export const AddExpectedMargin = (data) => (dispatch) => {
    dispatch(actions.AddExpectedMargin(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteExpectedMargin = (data) => (dispatch) => {
    dispatch(actions.DeleteExpectedMargin(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const ClearGridFormSearchData = () => (dispatch) => {
    dispatch(actions.ClearGridFormSearchData());
};
// before editing storing the actual data 
export const EditExpectedMargin = (data) => (dispatch) => {
    dispatch(actions.EditExpectedMargin(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const AddCompanyOffice = (data) => (dispatch) => {
    dispatch(actions.AddCompanyOffice(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const UpdateCompanyOffice = (data) => (dispatch) => {
    dispatch(actions.UpdateCompanyOffice(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const DeleteCompanyOffices = (data) => (dispatch) => {
    dispatch(actions.DeleteCompanyOffices(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
// before editing storing the actual data 
export const EditCompanyOffice = (data) => (dispatch) => {
    //dispatch(FetchState(data.country));
    //dispatch(FetchCity(data.state));
    dispatch(FetchStateId(data.countryId)); //Added for ITK D1536
    dispatch(FetchCityId(data.stateId)); //Added for ITK D1536
    dispatch(actions.EditCompanyOffice(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const FetchState = (data) => (dispatch) => {

    // if (data === "") {
    dispatch(actions.FetchState([]));
    //     return false;
    // }

    const stateMasterData = masterData.baseUrl + masterData.state + "?country=" + data;
    processApiRequest(stateMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchState(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchStateAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error + localConstant.companyDetails.common.FAILED, 'dangerToast FetchStateAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
       
    });
};

export const FetchCity = (data) => (dispatch) => {

    // if (data === "") {
    dispatch(actions.FetchCity([]));
    //     return false;
    // }
    const cityMasterData = masterData.baseUrl + masterData.city + "?state=" + data;
    processApiRequest(cityMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCity(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchCityAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchCityAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
      
    });
};

export const FetchBusinessUnit = (data) => (dispatch) => {

    const businessUnitData = masterData.baseUrl + masterData.businessUnit;
    processApiRequest(businessUnitData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchBusinessUnit(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchBusinessUnitAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchBusinessUnitAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');

    });
};

export const FetchMarginType = (data) => (dispatch) => {

    const marginTypeData = masterData.baseUrl + masterData.marginType;
    processApiRequest(marginTypeData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code === "1") {
            dispatch(actions.FetchMarginType(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchMarginTypeAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchMarginTypeAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');

    });
};

export const FetchSalutation = (data) => (dispatch) => {

    const salutationMasterData = masterData.baseUrl + masterData.salutation;
    processApiRequest(salutationMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchSalutation(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchSalutationAPI');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchSalutationAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
     
    });
};
export const FetchTaxData = () => (dispatch) => {

    const taxData = masterData.baseUrl + masterData.tax;
    processApiRequest(taxData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchTaxData(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchTaxDataAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchTaxDataAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
   
    });
};
export const FetchVatPrefix = (data) => (dispatch) => {

    const vatPrefixMasterData = masterData.baseUrl + masterData.prefix;
    processApiRequest(vatPrefixMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchVatPrefix(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchVatPrefixAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchVatPrefixAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
   
    });
};

export const CompanyFetchVatPrefix = (data) => (dispatch) => {

    const vatPrefixMasterData = masterData.baseUrl + masterData.prefix;
    processApiRequest(vatPrefixMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            const euvatPrefixes = response.data.result.map(x => {
                return {
                    name: x,
                    value: x
                };
            });
            dispatch(actions.CompanyFetchVatPrefix(euvatPrefixes));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchCompanyVatPrefixAPI');

        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchCompanyVatPrefixAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
  
    });
};

export const FetchCountry = () => (dispatch) => {

    const countryMasterData = masterData.baseUrl + masterData.country;
    processApiRequest(countryMasterData, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCountry(response.data.result));

        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchCountryAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchCountryAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');

    });
};

export const ShowButtonHandler = () => (dispatch) => {
    dispatch(actions.ShowButtonHandler());
};
//documentTypes Master data
export const FetchMasterCompanyDocumentTypes = () => (dispatch, getstate) => {

    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=company";
    processApiRequest(documentTypesMasterDataUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchMasterCompanyDocumentTypes(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchDocTypeAPI');

        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchDocTypeAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
       
    });
};
export const AddDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddDocumentDetails(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if (editedData) {
        const editedRow = { ...editedData, ...data };
        const index = state.CompanyReducer.companyDetail.CompanyDocuments.findIndex(document => document.id === editedRow.id);
        const newState = Object.assign([], state.CompanyReducer.companyDetail.CompanyDocuments);
        newState[index] = editedRow;
        if (index >= 0) {
            dispatch(actions.UpdateDocumentDetails(newState));
            dispatch(actions.CompanyUpdatedAction(true));
        }
    }
    else {
        dispatch(actions.UpdateDocumentDetails(data));
        dispatch(actions.CompanyUpdatedAction(true));
    }
};
export const DispalyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.DispalyDocumentDetails(data));
};
export const UpdateExpectedMargin = (data) => (dispatch) => {
    dispatch(actions.UpdateExpectedMargin(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteCompanyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.DeleteCompanyDocumentDetails(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const CopyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.CopyDocumentDetails(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const EditCompanyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.EditCompanyDocumentDetails(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
//contracts
export const FetchCompanyContract = (status) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const data = {
        responseResult: null,
        selectedValue: status
    };
    dispatch(actions.FetchCompanyContract(data));
    const companyCode = getstate().CompanyReducer.selectedCompanyCode;
    //const companyContractData = companyAPIConfig.companyBaseURL + companyAPIConfig.companyContractDetail + '=' + companyCode;
    const companyContractData = companyAPIConfig.companyBaseURL + companyAPIConfig.companyContractDetail + '=' + companyCode;
    let params;
    if (status === "all") {
        params = {
            companyCode: companyCode
        };
    } else {
        params = {
            companyCode: companyCode,
            contractStatus: status
        };
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(companyContractData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.companyDetails.common.FECTH_CONTRACT_FAILED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const data = {
            responseResult: response.result ? response.result : [],
            selectedValue: status
        };
        dispatch(actions.FetchCompanyContract(data));
        dispatch(HideLoader());
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'warningToast fetchcompcontract');
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(localConstant.companyDetails.common.FECTH_CONTRACT_FAILED, 'dangerToast assignemntSomthingWrong');
        dispatch(HideLoader());
    }

};
function SetDefaultCompanyDetailProperty(data) {
    if (data !== undefined && data !== null) {
        if (data.CompanyOffices !== undefined && data.CompanyOffices === null)
            data.CompanyOffices = [];

        if (data.CompanyDivisions !== undefined && data.CompanyDivisions === null)
            data.CompanyDivisions = [];

        if (data.CompanyDivisionCostCenters !== undefined && data.CompanyDivisionCostCenters === null)
            data.CompanyDivisionCostCenters = [];

        if (data.CompanyDocuments !== undefined && data.CompanyDocuments === null)
            data.CompanyDocuments = [];

        //if (data.CompanyEmailTemplates !== undefined && data.CompanyEmailTemplates === null)
        //    data.CompanyEmailTemplates = {};

        if (data.CompanyExpectedMargins !== undefined && data.CompanyExpectedMargins === null)
            data.CompanyExpectedMargins = [];

        //if (data.CompanyInvoiceInfo !== undefined && data.CompanyInvoiceInfo === null)
        //    data.CompanyInvoiceInfo = {};

        if (data.CompanyNotes !== undefined && data.CompanyNotes === null)
            data.CompanyNotes = [];

        if (data.CompanyPayrolls !== undefined && data.CompanyPayrolls === null)
            data.CompanyPayrolls = [];

        if (data.CompanyPayrollPeriods !== undefined && data.CompanyPayrollPeriods === null)
            data.CompanyPayrollPeriods = [];

        if (data.CompanyQualifications !== undefined && data.CompanyQualifications === null)
            data.CompanyQualifications = [];

        if (data.CompanyTaxes !== undefined && data.CompanyTaxes === null)
            data.CompanyTaxes = [];

        if (data.CompanyInvoiceInfo && !isEmpty(data.CompanyInvoiceInfo.techSpecialistExtranetComment))
            data.CompanyInvoiceInfo.techSpecialistExtranetComment = sanitize(data.CompanyInvoiceInfo.techSpecialistExtranetComment);
    }

    return data;
}

//General Details
export const FetchCompanyDetails = (data) => async (dispatch, getstate) => {

    const selectedCompanyCode = getstate().CompanyReducer.selectedCompanyCode;
    dispatch(ShowLoader());
    let companyDetailUrl;
    if (data && data.companyCode) {
        if (data.selectedCompany) {
            dispatch(UpdateSelectedCompany({ companyCode: data.selectedCompany }));
        }
        companyDetailUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails + "/" + data.companyCode + companyAPIConfig.companyDetail;
    }
    else {
        companyDetailUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails + "/" + selectedCompanyCode + companyAPIConfig.companyDetail;
    }

    await processApiRequest(companyDetailUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isEmptyOrUndefine(response.data.CompanyInfo)) {
            dispatch(UpdateEmailTemplate());  //Added for Defect 881
            const state = getstate();
            const emailTemplateType = state.CompanyReducer.emailTemplateType;
            let selectedEmail = '';
            if (emailTemplateType === "CRN") {
                selectedEmail = response.data.CompanyEmailTemplates.customerReportingNotificationEmailText;
            }
            if (emailTemplateType === "CDR") {
                selectedEmail = response.data.CompanyEmailTemplates.customerDirectReportingEmailText;
            }
            if (emailTemplateType === "RJVT") {
                selectedEmail = response.data.CompanyEmailTemplates.rejectVisitTimesheetEmailText;
            }
            if (emailTemplateType === "VCECO") {
                selectedEmail = response.data.CompanyEmailTemplates.visitCompletedCoordinatorEmailText;
            }
            if (emailTemplateType === "ICAOCO") {
                selectedEmail = response.data.CompanyEmailTemplates.interCompanyOperatingCoordinatorEmail;
            }
            if (emailTemplateType === "") {
                selectedEmail = "";
            }
            dispatch(UpdateEmailTemplate(selectedEmail));
            dispatch(UpdateEmailTemplateType(emailTemplateType));
            dispatch(actions.FetchCompanyDetails(SetDefaultCompanyDetailProperty(response.data)));
            /**
            * toggle company updated value to false after company fetch done
            */
            dispatch(actions.CompanyUpdatedAction(false));

            dispatch(UpdateCurrentModule(localConstant.moduleName.COMPANY));
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchCompanyDetailAPI');
            dispatch(HideLoader());
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast fetchCompanyDetailAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
};
export const AddInvoiceRemittance = (data) => (dispatch) => {
    dispatch(actions.AddInvoiceRemittance(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const EditInvoiceRemittance = (data) => (dispatch) => {
    dispatch(actions.EditInvoiceRemittance(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateInvoiceRemittance = (data) => (dispatch) => {
    dispatch(actions.UpdateInvoiceRemittance(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteInvoiceRemittance = (data) => (dispatch) => {
    dispatch(actions.DeleteInvoiceRemittance(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const AddInvoiceFooter = (data) => (dispatch) => {
    dispatch(actions.AddInvoiceFooter(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const EditInvoiceFooter = (data) => (dispatch) => {
    dispatch(actions.EditInvoiceFooter(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateInvoiceFooter = (data) => (dispatch) => {
    dispatch(actions.UpdateInvoiceFooter(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteInvoiceFooter = (data) => (dispatch) => {
    dispatch(actions.DeleteInvoiceFooter(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const FetchCompanyDivision = () => (dispatch, getstate) => {
    const state = getstate();
    const companyDivision = state.CompanyReducer.companyDetail.CompanyDivisions;
    dispatch(actions.FetchCompanyDivision(companyDivision));
};
export const FetchDivisionCostCenter = () => (dispatch, getstate) => {
    const state = getstate();
    const divisionCostCentreData = state.CompanyReducer.companyDetail.CompanyDivisionCostCenters;
    // const CompanyDivisionURL = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails +'/'+state.appLayoutReducer.selectedCompany+ companyAPIConfig.detail;
    dispatch(actions.FetchDivisionCostCenter(divisionCostCentreData));
};
export const AddNewDivision = (payload) => (dispatch, getstate) => {
    dispatch(actions.AddCompanyDivision(payload));
    // dispatch(actions.CompanyUpdatedAction(true));
    // dispatch(actions.UpdateAllDivision(payload.allDivisionDataToUpdate));

};

export const UpdateDivisionNameInCostCenter = (payload) => (dispatch, getstate) => {
    const state = getstate();
    const divisionCostCentreData = state.CompanyReducer.companyDetail.CompanyDivisionCostCenters;
    const updatedData = divisionCostCentreData && divisionCostCentreData.map((iteratedValue) => {
        if (iteratedValue.companyDivisionId == payload.companyDivisionId) {
            return iteratedValue = { ...iteratedValue, division: payload.divisionName };
        } else {
            return iteratedValue;
        }
    });
    dispatch(actions.UpdateDivisionNameInCostCenter(updatedData));
};

export const AddNewDivisionCostCentre = (payload) => (dispatch, getstate) => {
    dispatch(actions.AddCompanyDivisionCostcentre(payload));
    // dispatch(actions.CompanyUpdatedAction(true));

};

export const EditCompanyDivisionCostcentre = (data) => (dispatch) => {
    dispatch(actions.UpdateCostCentreButton(true));
    dispatch(actions.EditCompanyDivisionCostcentre(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const UpdateCompanyDivisionCostcentre = (data) => (dispatch, getstate) => {
    const state = getstate();
    const updatedDivisionCostCentre = state.CompanyReducer.companyDetail.CompanyDivisionCostCenters.map((iteratedValue) => {
        if (iteratedValue.division === data.oldDataCostcentre.division && iteratedValue.costCenterCode === data.oldDataCostcentre.costCenterCode && iteratedValue.costCenterName === data.oldDataCostcentre.costCenterName) {
            return iteratedValue = Object.assign({}, iteratedValue, data.data);
        } else {
            return iteratedValue;
        }
    });
    dispatch(actions.UpdateCompanyDivisionCostcentre(updatedDivisionCostCentre));
    // dispatch(actions.CompanyUpdatedAction(true));
};

export const UpdateCompanyDivision = (data) => (dispatch, getstate) => {
    const state = getstate();
    const updatedDivision = state.CompanyReducer.companyDetail.CompanyDivisions.map((iteratedValue) => {
        if (iteratedValue.divisionName === data.oldDivisionName) {
            return iteratedValue = Object.assign({}, iteratedValue, data.data);
        } else {
            return iteratedValue;
        }
    });
    dispatch(actions.UpdateCompanyDivision(updatedDivision));
    // dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteCompanyDivision = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = state.CompanyReducer.companyDetail.CompanyDivisions;
    newState.map((iteratedValue, index) => {
        if (iteratedValue.divisionName == data) {
            if (iteratedValue.recordStatus !== "N") {
                newState[index].recordStatus = "D";
            }
            else {
                newState.splice(index, 1);
            }
        }
    });

    dispatch(actions.DeleteCompanyDivision(newState));
    dispatch(actions.CompanyUpdatedAction(true));

};
export const UpdateCompanyDivisionButton = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateCompanyDivisionButton(data));
};
export const UpdateCostCentreButton = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateCostCentreButton(data));
};
export const DeleteCompanyDivisionCostCentre = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = state.CompanyReducer.companyDetail.CompanyDivisionCostCenters;
    data.map(row => {
        newState.map((iteratedValue, index) => {
            if (iteratedValue.companyDivisionCostCenterId === row.companyDivisionCostCenterId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteCompanyDivisionCostCentre(newState));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const AddCompanyNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddCompanyNotesDetails(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
//Payroll Actions
export const FetchPayrollData = () => (dispatch, getstate) => {
    const state = getstate();
    const payrollData = state.CompanyReducer.companyDetail.CompanyPayrolls;
    dispatch(actions.FetchPayrollData(payrollData));
};

export const FetchPayrollPeriodName = () => (dispatch, getstate) => {
    const state = getstate();
    const payrollPeriodNames = state.CompanyReducer.companyDetail.CompanyPayrollPeriods;
    dispatch(actions.FetchPayrollPeriodName(payrollPeriodNames));
};

export const AddNewPayroll = (payload) => (dispatch, getstate) => {
    dispatch(actions.AddNewPayroll(payload));
    // dispatch(actions.CompanyUpdatedAction(true));
};

export const AddNewPayrollPeriodName = (payload) => (dispatch, getstate) => {
    dispatch(actions.AddNewPayrollPeriodName(payload));
    // dispatch(actions.CompanyUpdatedAction(true));
};

export const FetchCostSaleReference = (isActive) => async (dispatch, getstate) => {

    const CompanySaleReferenceURL = companyAPIConfig.companyBaseURL + companyAPIConfig.costSaleReference;
    const params = {
        isActive: isActive  //Changes for Defect 983
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(CompanySaleReferenceURL, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast fetchCostSaleReferenceAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response.code === "1") {
        dispatch(actions.FetchCostSaleReference(response.result));;
    } else {
        IntertekToaster(localConstant.companyDetails.common.FETCH_COSTOFSALE_REF_API_FAILED, 'dangerToast fetchCostSaleReferenceAPI');
    }
};

/**
 * Delete Payroll Type Action
 */
export const DeletePayrollType = (payload) => (dispatch, getstate) => {
    const state = getstate();
    const newState = state.CompanyReducer.companyDetail.CompanyPayrolls;
    newState.map((iteratedValue, index) => {
        if (iteratedValue.companyPayrollId == payload) {
            if (iteratedValue.recordStatus !== "N") {
                newState[index].recordStatus = "D";
            }
            else {
                newState.splice(index, 1);
            }
        }
    });
    dispatch(actions.DeletePayrollType(newState));
    dispatch(actions.CompanyUpdatedAction(true));
};

/**
 * Payroll popup clear Action
 */
export const PayrollPopupClear = (payload) => (dispatch, getstate) => {
    dispatch(actions.PayrollPopupClear());
};

/**
 * Company Payroll Button Update Action
 */
export const UpdateCompanyPayrollButton = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateCompanyPayrollButton(data));
};

/**
 * Update Payroll Action
 */
export const UpdateCompanyPayroll = (data) => (dispatch, getstate) => {
    const state = getstate();
    let isAlreadychanged = false;//company validation
    const updatedPayroll = state.CompanyReducer.companyDetail.CompanyPayrolls.map((iteratedValue) => {
        if (iteratedValue.payrollType === data.oldPayrollName && isAlreadychanged === false) {
            isAlreadychanged = true;//company validation
            return iteratedValue = Object.assign({}, iteratedValue, data.data);
        } else {
            return iteratedValue;
        }
    });
    dispatch(actions.UpdateCompanyPayroll(updatedPayroll));
    // dispatch(actions.CompanyUpdatedAction(true));
};
//Add New Payroll Period Names
export const AddNewPeriodName = (payload) => (dispatch, getstate) => {
    dispatch(actions.AddNewPeriodName(payload));
};

export const EditPayrollPeriodName = (payload) => (dispatch, getstate) => {
    dispatch(actions.TogglePayrollPeriodNameButton(false));
    dispatch(actions.EditPayrollPeriodName(payload));
};
export const UpdatePayrollPeriodNameInPayrollPeriod = (payload) => (dispatch, getstate) => {
    const state = getstate();
    const payrollPeriodData = state.CompanyReducer.companyDetail.CompanyPayrollPeriods;
    const updatedData = payrollPeriodData && payrollPeriodData.map((iteratedValue) => {
        if (iteratedValue.companyPayrollId == payload.companyPayrollId) {
            return iteratedValue = { ...iteratedValue, payrollType: payload.payrollType };
        } else {
            return iteratedValue;
        }
    });
    dispatch(actions.UpdatePayrollPeriodNameInPayrollPeriod(updatedData));
};

export const UpdatePayrollPeriodName = (payload) => (dispatch, getstate) => {
    const state = getstate();
    const updatedPayrollPeriodName = state.CompanyReducer.companyDetail.CompanyPayrollPeriods.map((iteratedValue) => {
        if (iteratedValue.periodName == payload.oldPayrollNameData.periodName && iteratedValue.payrollType == payload.oldPayrollNameData.payrollType) {
            return iteratedValue = Object.assign({}, iteratedValue, payload.data);
        } else {
            return iteratedValue;
        }
    });

    dispatch(actions.UpdatePayrollPeriodName(updatedPayrollPeriodName));
    // dispatch(actions.CompanyUpdatedAction(true));
};

export const DeletePayrollPeriodName = (payload) => (dispatch, getstate) => {
    const state = getstate();
    const newState = state.CompanyReducer.companyDetail.CompanyPayrollPeriods;
    payload.map(row => {
        newState.map((iteratedValue, index) => {
            if (iteratedValue.companyPayrollPeriodId == row.companyPayrollPeriodId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeletePayrollPeriodName(newState));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const UpdateOverrideCostSaleReference = (payload) => (dispatch, getstate) => {
    dispatch(actions.UpdateOverrideCostSaleReference(payload));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const updateAvgTSHourlyCost = (payload) => (dispatch, getstate) => {
    dispatch(actions.updateAvgTSHourlyCost(payload));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const TogglePayrollPeriodNameButton = (payload) => (dispatch, getstate) => {
    dispatch(actions.TogglePayrollPeriodNameButton(payload));
};
// Email Template Actions
export const UpdateEmailTemplate = (data) => (dispatch) => {
    dispatch(actions.UpdateEmailTemplate(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateEmailTemplateType = (data) => (dispatch) => {
    dispatch(actions.UpdateEmailTemplateType(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateCompanyEmailTemplate = (data) => (dispatch) => {
    dispatch(actions.UpdateCompanyEmailTemplate(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const FetchCompanyLogoMasterData = () => (dispatch) => {

    const companyLogoUrl = masterData.baseUrl + masterData.logo + "?code=company";
    processApiRequest(companyLogoUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCompanyLogoMasterData(response.data.result));;
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast fetchCompanyAPI');
        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast fetchCompanyAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        
    });
};
export const FetchPlaceholders = (data) => (dispatch) => {

    const FetchPlaceholdersApi = companyAPIConfig.companyBaseURL + companyAPIConfig.companyPlaceholder + "?ModuleName=" + data;
    processApiRequest(FetchPlaceholdersApi, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            const data = response.data.result.sort((a, b) => {
                return a.displayName < b.displayName ? -1 : 1;
            });
            dispatch(actions.FetchPlaceholders(data));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast FetchPlaceholdersAPI');

        }

    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast FetchPlaceholdersAPIError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
   
    });
};
export const AddCompanyTaxes = (data) => (dispatch) => {
    dispatch(actions.AddCompanyTaxes(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const EditCompanyTaxes = (data) => (dispatch) => {
    dispatch(actions.EditCompanyTaxes(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const UpdateCompanyTaxes = (data) => (dispatch) => {
    dispatch(actions.UpdateCompanyTaxes(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const DeleteCompanyTaxes = (data) => (dispatch) => {
    dispatch(actions.DeleteCompanyTaxes(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const AddUpdateInvoiceDefaults = (data) => (dispatch) => {
    dispatch(actions.AddUpdateInvoiceDefaults(data));
    dispatch(actions.CompanyUpdatedAction(true));
};
export const AddUpdateCompanyInfo = (data) => (dispatch) => {
    dispatch(actions.AddUpdateCompanyInfo(data));
    dispatch(actions.CompanyUpdatedAction(true));
};

export const ClearStateCityData = () => (dispatch) => {
    dispatch(actions.ClearStateCityData());
};

/**
 * Action to clear company details
 */
export const ClearCompanyDetails = () => (dispatch) => {
    dispatch(actions.ClearCompanyDetails());
};
export const ClearCompanyDocument = () => async (dispatch, getstate) => {
    const state = getstate();
    const companyInfo = Object.assign({}, state.CompanyReducer.companyDetail.CompanyInfo);
    const documentData = state.CompanyReducer.companyDetail.CompanyDocuments;
    const companyCode = companyInfo.companyCode;
    const deleteUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDocuments + companyCode;
    let response = null;
    if (!isEmpty(documentData) && !isUndefined(companyCode)) {
        response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response;
};
export const CancelCompanyDetails = () => async (dispatch, getstate) => {
    const documentData = getstate().CompanyReducer.companyDetail.CompanyDocuments;
    const companyCode = getstate().CompanyReducer.companyDetail.CompanyInfo.companyCode;
    const deleteUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDocuments + companyCode;
    if (!isEmpty(documentData)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    //dispatch(actions.ClearCompanyDetails());
    dispatch(FetchCompanyDetails());
    // const emailTemplateType=getstate().CompanyReducer.emailTemplateType;
    // let selectedEmail='';
    // const allCompanyEmailTemplates=getstate().CompanyReducer.companyDetail.CompanyEmailTemplates;
    // if (allCompanyEmailTemplates) {

    //     if (emailTemplateType === "CRN") {
    //         selectedEmail = allCompanyEmailTemplates.customerReportingNotificationEmailText;
    //     }
    //     if (emailTemplateType === "CDR") {
    //         selectedEmail = allCompanyEmailTemplates.customerDirectReportingEmailText;
    //     }
    //     if (emailTemplateType === "RJVT") {
    //         selectedEmail = allCompanyEmailTemplates.rejectVisitTimesheetEmailText;
    //     }
    //     if (emailTemplateType === "VCECO") {
    //         selectedEmail = allCompanyEmailTemplates.visitCompletedCoordinatorEmailText;
    //     }
    //     if (emailTemplateType === "ICAOCO") {
    //         selectedEmail = allCompanyEmailTemplates.interCompanyOperatingCoordinatorEmail;
    //     }
    //     if(emailTemplateType === ""){
    //         selectedEmail="";
    //     }
    // }
    // dispatch(UpdateEmailTemplate(selectedEmail));
    // dispatch(UpdateEmailTemplateType(emailTemplateType));
};

//POST COMPANY DATA
export const SaveCompanyDetails = () => async (dispatch, getstate) => {
    const state = getstate();
    const companyDetailObject = objectUtil.cloneDeep(state.CompanyReducer.companyDetail);
    dispatch(ShowLoader());
    const companyData = SetDefaultCompanyDetailProperty(companyDetailObject);

    companyData.CompanyInfo["recordStatus"] = "M";

    if (companyData.CompanyInfo.avgTSHourlyCost) {
        companyData.CompanyInfo.avgTSHourlyCost = formatToDecimal(companyData.CompanyInfo.avgTSHourlyCost, 2);
    }

    if (companyData.CompanyOffices) {
        companyData.CompanyOffices.map(row => {
            if (row.recordStatus === "N") {
                row.id = 0;
            }
        });
    }
    if (companyData.CompanyDivisions) {
        companyData.CompanyDivisions.map(row => {
            if (row.recordStatus === "N") {
                row.companyDivisionId = 0;
            }
        });
    }

    if (companyData.CompanyPayrolls) {
        companyData.CompanyPayrolls.map(row => {
            if (row.recordStatus === "N") {
                row.companyPayrollId = 0;
            }
        });
    }

    if (companyData.CompanyPayrollPeriods) {
        companyData.CompanyPayrollPeriods.map(row => {
            if (row.recordStatus === "N") {
                row.companyPayrollPeriodId = 0;
            }
        });
    }

    if (companyData.CompanyExpectedMargins) {
        companyData.CompanyExpectedMargins.map(row => {
            if (row.recordStatus === "N") {
                row.companyExpectedMarginId = 0;
            }
        });
    }
    if (companyData.CompanyNotes) {
        companyData.CompanyNotes.map(row => {
            if (row.recordStatus === "N") {
                row.companyNoteId = 0;
            }
        });
    }
    if (companyData.CompanyDocuments) {
        companyData.CompanyDocuments.map(row => {
            if (!isEmpty(row.status)) {
                row.status = row.status.trim();
            }
            if (row.recordStatus === "N") {
                row.id = 0;
            }
        });
    }
    if (companyData.CompanyTaxes) {
        companyData.CompanyTaxes.map(row => {
            if (row.recordStatus === "N") {
                row.companyTaxId = 0;
            }
        });
    }
    const postCompanyUrl = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails + companyAPIConfig.companyDetail;
    const filterdCompanyData = FilterSave(companyData);

    const requestPayload = new RequestPayload(filterdCompanyData);
    const response = await PostData(postCompanyUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(error, 'dangerToast errorAlert');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });

    if (response && response.code === "1") {
        dispatch(SuccessAlert(response, "Company"));
        await dispatch(FetchCompanyDetails());
    }
    else if (response && (response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast Company');
        dispatch(HideLoader());
    }
    else if (response && (response.code === "11")) {
        IntertekToaster("This Payroll has been used and cannot be deleted.", 'warningToast alertActWarnAlt');
        dispatch(HideLoader());
    }
    else {
        dispatch(FailureAlert(response, "Company"));
        dispatch(HideLoader());
    }
    return response;
    //  }
};

export const EditCompanyNotesDetails = (editedData) => (dispatch, getstate) => { //D661 issue8
    const state = getstate();
    const index = state.CompanyReducer.companyDetail.CompanyNotes.findIndex(iteratedValue => iteratedValue.companyNoteId === editedData.companyNoteId);
    const newState = Object.assign([], state.CompanyReducer.companyDetail.CompanyNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditCompanyNotesDetails(newState));
        dispatch(actions.CompanyUpdatedAction(true));
    }
};

export const UpdateSelectedPayrollData = (data) => (dispatch) => {
    dispatch(actions.UpdateSelectedPayrollData(data));
};

export const UpdateSelectedDivisionData = (data) => (dispatch) => {
    dispatch(actions.UpdateSelectedDivisionData(data));
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};
