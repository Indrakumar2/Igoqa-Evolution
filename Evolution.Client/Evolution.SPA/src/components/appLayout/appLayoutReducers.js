import { appLayoutActionTypes } from '../../constants/actionTypes';
import { configuration } from '../../appConfig';
import { applicationConstants } from '../../constants/appConstants';
import { isEmptyOrUndefine } from '../../utils/commonUtils';

const initialState = {
    companyList: [],
    selectedCompany: (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE))
        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)
        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)),
    selectedCompanyId: (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID))
        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID)
        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID)),
    selectedCompanyName: "",
    selectedCompanyData: {},
    loginStatus: false,
    loading: true,
    loginUser: localStorage.getItem(applicationConstants.Authentication.DISPLAY_NAME),
    userName: localStorage.getItem(applicationConstants.Authentication.USER_NAME),
    loginPassWord: configuration.password,
    currency: [],
    divisionName: [],
    payrolls: [],
    exportPrefixes: [],
    chargeTypes: [],
    username: localStorage.getItem(applicationConstants.Authentication.USER_NAME),
};

export const appLayoutReducer = (state = initialState, actions) => {
    const { type, data } = actions;
    switch (type) {
        case appLayoutActionTypes.FETCH_COMPANY_LIST:
            state = {
                ...state,
                companyList: data
            };
            return state;

        case appLayoutActionTypes.CLEAR_COMPANY_DATA:
            state = {
                ...state,
                companyList: []
            };
            return state;
            
        case appLayoutActionTypes.UPADTE_SELECTED_COMPANY:
            state = {
                ...state,
                selectedCompany: data.companyCode,
                selectedCompanyName: data.companyName,
                selectedCompanyData: data.selectedCompanyData
            };
            return state;

        case appLayoutActionTypes.AUTHENTICATE_USER:
            state = {
                ...state,
                loginStatus: data
            };
            return state;

        case appLayoutActionTypes.LOG_OUT_USER:
            state = {
                ...state,
                loginStatus: data,
            };
            return state;
        case appLayoutActionTypes.AUTHENTICATE_LOGIN_SUCCESS:
            state = {
                ...state,
                selectedCompany: data.ccode,
                selectedCompanyId: data.cid,
                loginUser: data.sub,
                username: data.unique_name,
                loginStatus: true
            };
            return state;
        case appLayoutActionTypes.FETCH_CURRENCY:
            state = {
                ...state,
                currency: data
            };
            return state;
        case appLayoutActionTypes.FETCH_DIVISION_NAME:
            state = {
                ...state,
                divisionName: data
            };
            return state;
        case appLayoutActionTypes.FETCH_PAYROLLS:
            state = {
                ...state,
                payrolls: data
            };
            return state;
        case appLayoutActionTypes.FETCH_EXPORT_PREFIXES:
            state = {
                ...state,
                exportPrefixes: data
            };
            return state;
        case appLayoutActionTypes.FETCH_CHARGE_TYPES:
            state = {
                ...state,
                chargeTypes: data
            };
            return state;
        case appLayoutActionTypes.FETCH_USER_ROLE_COMPANY:
            state = {
                ...state,
                userRoleCompanyList: data
            };
            return state;
        case appLayoutActionTypes.CLEAR_USER_COMPANY_LIST:
            state = {
                ...state,
                userRoleCompanyList: []
            };
            return state;
        case appLayoutActionTypes.FETCH_USER_PERMISSIONS:
            state = {
                ...state,
                activities: data
            };
            return state;
        case appLayoutActionTypes.FETCH_ANNOUNCEMENTS:
            state = {
                ...state,
                annoncementData: data
            };
            return state;
        case appLayoutActionTypes.CHANGE_DATA_AVAILABLE_STATUS:
            state = {
                ...state,
                isDataAvailable: data
            };
            return state;
        case appLayoutActionTypes.FETCH_ABOUT_INFORMATION:
            state = {
                ...state,
                systemSettingData: data
            };
            return state;
            case appLayoutActionTypes.ADD_DASHBOARD_COMPANY_ID:
            state = {
                ...state,
                selectedCompanyId: data,
            };
            return state;
        case appLayoutActionTypes.AUTHENTICATE_LOGOUT:
            state = {
                ...state,
                selectedCompany: "",
                selectedCompanyId: "",
                loginUser: "",
                username: "",
            };
            return state;
        default:
            return state;
    }
};