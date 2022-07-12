import { processApiRequest } from '../../services/api/defaultServiceApi';
import { appLayoutActionTypes } from '../../constants/actionTypes';
import { companyAPIConfig, masterData,roleAPIConfig,homeAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import MaterializeComponent from 'materialize-css';
import { userAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { ToggleAllCoordinator, Dashboardrefresh } from '../viewComponents/dashboard/dahboardActions';
import { UpdateMasterCurrency } from '../../common/masterData/masterDataActions';
import { getlocalizeData, parseValdiationMessage, isEmpty, isUndefined, isEmptyOrUndefine, } from '../../utils/commonUtils';
import { PostData,FetchData } from '../../services/api/baseApiService';
import { applicationConstants } from '../../constants/appConstants';
import { configuration } from '../../appConfig';
import { required } from '../../utils/validator';
import { ShowLoader, HideLoader } from '../../common/commonAction';

const actions = {
    FetchCompanyList: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_COMPANY_LIST,
            data: payload
        };
    },
    UpdateSelectedCompany: (payload) => {
        return {
            type: appLayoutActionTypes.UPADTE_SELECTED_COMPANY,
            data: payload
        };
    },
    AuthenticateUser: (payload) => {
        return {
            type: appLayoutActionTypes.AUTHENTICATE_USER,
            data: true
        };
    },
    logOut: (payload) => {
        return {
            type: appLayoutActionTypes.LOG_OUT_USER,
            data: payload
        };
    },
    FetchCurrency: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_CURRENCY,
            data: payload
        };
    },
    FetchDivisionName: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_DIVISION_NAME,
            data: payload
        };
    },
    FetchPayrolls: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_PAYROLLS,
            data: payload
        };
    },
    FetchExportPrefixes: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_EXPORT_PREFIXES,
            data: payload
        };
    },
    FetchChargeTypes: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_CHARGE_TYPES,
            data: payload
        };
    },
    FetchUserRoleCompany: (payload) => {
        return {
            type: appLayoutActionTypes.FETCH_USER_ROLE_COMPANY,
            data: payload
        };
    }, 
    FetchUserPermissionsData:(payload)=>{
        return{
           type: appLayoutActionTypes.FETCH_USER_PERMISSIONS,
           data: payload
        };
    },
    FetchAnnoncements:(payload)=>{
        return{
            type: appLayoutActionTypes.FETCH_ANNOUNCEMENTS,
            data: payload
         };
    },
    FetchAboutInformation:(payload)=>{
        return{
            type: appLayoutActionTypes.FETCH_ABOUT_INFORMATION,
            data: payload
         };
    },
    ChangeDataAvailableStatus:(payload)=>{
        return{
            type: appLayoutActionTypes.CHANGE_DATA_AVAILABLE_STATUS,
            data: payload
         };
    },
    ClearCompanyData: (payload) => ({
        type: appLayoutActionTypes.CLEAR_COMPANY_DATA
        //data: payload
    }),
    ClearUserCompanyList: (payload) => ({
        type: appLayoutActionTypes.CLEAR_USER_COMPANY_LIST
        //data: payload
    }),
    AddDashboardCompanyId:(payload)=>{
        return{
            type: appLayoutActionTypes.ADD_DASHBOARD_COMPANY_ID,
            data: payload
         };
    },
    ClearLoginUserDetails:()=>{
        return{
            type: appLayoutActionTypes.AUTHENTICATE_LOGOUT,
         };
    },
};

const localConstant = getlocalizeData();

export const ClearCompanyData = () => async (dispatch) => {
    dispatch(actions.ClearCompanyData());
};
export const FetchCompanyList = () => async (dispatch,getstate) => {
    const state = getstate();
    if(isEmptyOrUndefine(state.appLayoutReducer.companyList)){
        const url = companyAPIConfig.companyBaseURL + homeAPIConfig.dashboardCompanyList;
        const param = {
            isActive : true
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast companyApiWrong');
            });
        if (response && response.code === "1") {
            const ccode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                            ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                            : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));                
            const selectedCompanyCode = !isEmptyOrUndefine(ccode) ? ccode : state.appLayoutReducer.selectedCompany;
            const selectedCompany = response.result && response.result.filter(item => item.companyCode === selectedCompanyCode);
            if(!isEmpty(selectedCompany)){
                localStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompany[0].currency);
                sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompany[0].currency);
                localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompany[0].companyCode);
                sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompany[0].companyCode);
                localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, selectedCompany[0].id);
                sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, selectedCompany[0].id);
            }
            dispatch(actions.FetchCompanyList(response.result));
            return response;
        } else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast companyApiWrong');
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast companyApiWrong');
        }
    }

    //To avoid company currency sorting issue in slow network
    if(!isEmpty(state.masterDataReducer.currencyMasterData)){
        dispatch(UpdateMasterCurrency());
        }

};

// export const FetchCompanyList = (requestData) => (dispatch, getstate) => {
//     const state = getstate();
//     //companyList
//     if(isEmptyOrUndefine(state.appLayoutReducer.companyList)){
//     const url = companyAPIConfig.companyBaseURL + companyAPIConfig.companyDetails;
//     processApiRequest(url, { method: 'get' })
//         .then(response => {
//             if (response.status === 200) {
//                 const ccode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
//                                 ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
//                                 : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));                
//                 const selectedCompanyCode = !isEmptyOrUndefine(ccode) ? ccode : state.appLayoutReducer.selectedCompany;
//                 const selectedCompany = response.data.result.filter(item => item.companyCode === selectedCompanyCode);
//                 localStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompany[0].currency);
//                 sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompany[0].currency);
//                 localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompany[0].companyCode);
//                 sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompany[0].companyCode);
//                 if(!isEmpty(state.masterDataReducer.currencyMasterData)){
//                     dispatch(UpdateMasterCurrency());
//                 }
//                 dispatch(actions.FetchCompanyList(response.data.result));
//             }
//             else {
//                 IntertekToaster('Company API went wrong', 'dangerToast companyApiWrong');
//             }
//         }).catch(error => {
//             // alert("error",error);
//             IntertekToaster(error + 'Company API went wrong', 'dangerToast companyApiWrongError');
//         });
//     }
// };

export const FetchUserRoleCompanyList = (userLogonName) => async (dispatch, getstate) => {
    if (isEmptyOrUndefine(getstate().appLayoutReducer.userRoleCompanyList)) {
        const params = [];
        params.push(userLogonName);
        const fetchUrl = userAPIConfig.userRoleCompany;
        const requestPayload = new RequestPayload(params);
        let response = PostData(fetchUrl, requestPayload);
        response = await response
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
                return false;
            });

        if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
            dispatch(actions.FetchUserRoleCompany(response.result));
            return true;
        }
        else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
            return false;
        }
    }
};
export const ClearUserCompanyList = () => async (dispatch) => {
    dispatch(actions.ClearUserCompanyList());
};

export const FetchUserPermission = (userLogonName,selectedCompCode,module) => async (dispatch, getstate) => {
    const state = getstate();
    const companyId=state.appLayoutReducer.selectedCompanyId;
    const params = {
        'companyId':companyId,
        'module':module
    };
    dispatch(ShowLoader()); //Added for ITK Def957(issue1.2) ref by 14-05-2020 ALM Doc)
    if(userLogonName!==null){
        const fetchUrl = userAPIConfig.userPermission.replace('{userSamaName}',encodeURIComponent(userLogonName)); //D1302
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast'); 
            dispatch(HideLoader());
            return false;
        });  
        if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.code) && response.code == "1") { 
            dispatch(HideLoader());
            return response;
        }
    }
    dispatch(HideLoader());//Added for ITK Def957(issue1.2) ref by 14-05-2020 ALM Doc)
};

export const FetchUserPermissionsData=(data)=>(dispatch)=>{
    dispatch(actions.FetchUserPermissionsData(data));
};

export const UpdateSelectedCompany = (selectedCompany) => (dispatch, getstate) => {
    localStorage.removeItem(applicationConstants.Authentication.COMPANY_CURRENCY);
    sessionStorage.removeItem(applicationConstants.Authentication.COMPANY_CURRENCY);
    const state = getstate();
    const data = {
        allCoOrdinator: state.dashboardReducer.allCoOrdinator,
        futureDays: state.dashboardReducer.futureDays
    };
    const companyList = getstate().appLayoutReducer.companyList;
    const selectedCompanyData = companyList.filter(iteratedValue => iteratedValue.companyCode === selectedCompany.companyCode);
    if (selectedCompanyData && selectedCompanyData.length > 0) {
        selectedCompany.selectedCompanyData = selectedCompanyData[0];
        localStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompanyData[0].currency);
        sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompanyData[0].currency);
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompanyData[0].companyCode);
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, selectedCompanyData[0].companyCode);
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, selectedCompanyData[0].id);
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, selectedCompanyData[0].id);
    }
    dispatch(actions.UpdateSelectedCompany(selectedCompany));
    if (selectedCompany.selectedCompanyData){
        const selectedCompanyId=selectedCompany.selectedCompanyData.id;
        dispatch(actions.AddDashboardCompanyId(selectedCompanyId));
    }
    dispatch(ToggleAllCoordinator(data));
};

export const loginStatus = () => (dispatch, getstate) => {
    dispatch(actions.AuthenticateUser());
};
export const logOut = (data) => (dispatch, getstate) => {
    dispatch(actions.logOut(data));
};

export const FetchCurrency = (data) => (dispatch, getstate) => {
    // TODO: Change the api config from company to master
    const url = companyAPIConfig.companyBaseURL + companyAPIConfig.currencies;
    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchCurrency(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast CurrencyApiWrong');
            }
        }).catch(error => {
            // console.error(error); // To show the error details in console
            // alert("error",error);
            // IntertekToaster(error + 'Currency API went wrong', 'dangerToast CurrencyApiWrongError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};

/**
 * Division master data action
 */

export const FetchDivisionName = (data) => (dispatch, getstate) => {
    const url = masterData.baseUrl + masterData.divisionName;

    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchDivisionName(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast DivisionApiWrong');
            }
        }).catch(error => {
            // console.error(error); // To show the error details in console
            // alert("error",error);
            // IntertekToaster(error + 'Division Name API went wrong', 'dangerToast DivisionApiWrongError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};
export const FetchPayrolls = (data) => (dispatch, getstate) => {
    const url = companyAPIConfig.companyBaseURL + masterData.payrolls;
    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchPayrolls(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast PayrollApiWrong');
            }
        }).catch(error => {
            // alert("error",error);   
            // console.error(error); // To show the error details in console         
            // IntertekToaster(error + 'Payroll API went wrong', 'dangerToast PayrollApiWrongError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};

/**
 * Export Prefix Master Data Action
 */
export const FetchExportPrefixes = (data) => (dispatch, getstate) => {
    const url = masterData.baseUrl + masterData.exportPrefixes;

    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchExportPrefixes(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast ExportApiWrong');
            }
        }).catch(error => {
            // alert("error",error);
            // console.error(error); // To show the error details in console
            // IntertekToaster(error + 'Export Prefix API went wrong', 'dangerToast ExportApiWrongError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};

/**
 * Charge Type master data fetch action
 */
export const FetchChargeTypes = () => (dispatch, getstate) => {
    const url = masterData.baseUrl + masterData.chargeTypes;
    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchChargeTypes(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast ChargeTypeAPIWrong');
            }
        }).catch(error => {
            // alert("error",error);
            // console.error(error); // To show the error details in console
            // IntertekToaster(error + 'Charge Type API went wrong', 'dangerToast ChargeTypeAPIError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};
export const FetchAnnoncements=()=>(dispatch)=>{
    //const url='http://localhost:5101/api/announcements';
    const url = roleAPIConfig.roleBaseUrl + roleAPIConfig.AnnonceMents;
    processApiRequest(url, { method: 'get' })
        .then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
                dispatch(actions.FetchAnnoncements(response.data.result));
                return response.data.result;
                }
                else {
                    IntertekToaster(parseValdiationMessage(response.data), 'dangerToast AnnoncementsAPIWrong');
                }
        }).catch(error => {
            // console.error(error); // To show the error details in console
            // alert("error",error);
            // IntertekToaster(error + 'Annoncements API went wrong', 'dangerToast AnnoncementsAPIError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
};

export const FetchAboutInformation = (data) => async (dispatch, getstate) => {
    const state = getstate();
    if (isEmptyOrUndefine(state.appLayoutReducer.systemSettingData)) {
        const apiUrl = masterData.baseUrl + masterData.systemSettings;

        const requestPayload = new RequestPayload(applicationConstants.systemContants);
        const response = await PostData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast SystemSettingData');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchAboutInformation(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast SystemSettingData');
        }
    }
};

// export const FetchAboutInformation=()=>(dispatch)=> async (dispatch, getstate) => {
//     const url = masterData.baseUrl + masterData.systemSettings;
//     const params=applicationConstants.systemContants;
//     const requestPayload = new RequestPayload(params);
//     let response = await PostData(url, requestPayload);
//     response =  response
//         .catch(error => {
//             IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast SystemSettingSomethingWrong');
//             return false;
//         });

//     if (!isUndefined(response) && !isUndefined(response.code) && response.code === "1") {
//         dispatch(actions.FetchAboutInformation(response.result));
//         return true;
//     }
// };

export const ChangeDataAvailableStatus=(status)=>(dispatch)=>{
    dispatch(actions.ChangeDataAvailableStatus(status));
};

export const AddDashboardCompanyId = () => (dispatch,getstate) => {
    const selectedCompany = sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID);
    dispatch(actions.AddDashboardCompanyId(selectedCompany));
};

export const ClearLoginUserDetails = () => (dispatch,getstate) => {
    dispatch(actions.ClearLoginUserDetails());
};