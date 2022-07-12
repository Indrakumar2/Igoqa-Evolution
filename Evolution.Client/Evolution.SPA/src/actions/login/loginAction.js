import { loginActionTypes } from '../../constants/actionTypes';
import { RequestPayload, loginApiConfig } from '../../apiConfig/apiConfig';
import { ValidateLogin, PostData, FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import authService from '../../authService';
import { getlocalizeData, isEmptyOrUndefine, parseValdiationMessage  } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import {
    ValidationAlert,
} from '../../components/viewComponents/customer/alertAction';
import { ClearLoginUserDetails } from '../../components/appLayout/appLayoutActions';

const localConstant = getlocalizeData();

const actions = {
    authenticateLoginSuccess: (payload) => {
        return {
            type: loginActionTypes.AUTHENTICATE_LOGIN_SUCCESS,
            data: payload
        };
    },
    authenticateLoginFailed: (payload) => (
        {
            type: loginActionTypes.AUTHENTICATE_LOGIN_FAILED,
            data: payload
        }
    ),
    tokenStorage: (payload) => {
        return {
            type: loginActionTypes.TOKEN_STORAGE,
            data: payload
        };
    },
    handleLogOut: (payload) => {
        return {
            type: loginActionTypes.LOG_OUT,
            data: payload
        };
    },
    resetApplication: (payload) => {
        return {
            type: loginActionTypes.RESET_STORE,
            data: payload
        };
    },
    setUserMenu: (payload) => {
        return {
            type: loginActionTypes.SET_USER_MENU,
            data: payload
        };
    },
    setUserType: (payload) => {
        return {
            type: loginActionTypes.SET_USER_TYPE,
            data: payload
        };
    },
};

/**
 * 
 * @param {*} data 
 */
export const AuthenticateLogin = (data) => async (dispatch) => {
    const authenticateLoginUrl = loginApiConfig.authLogin;
    const requestPayload = new RequestPayload(data);

    const response = await ValidateLogin(authenticateLoginUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(error + ' Authentication Failed', 'dangerToast ValidateLoginError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (response) {
        if (response.code == 1) {
            authService.initializeLocalStorage(); //To initialize localstorage values
            if (response.result.isAuthTokenAlreadyExistsForUser){
                // console.log("Multiple login detected for this user");
                setTimeout(function() { return IntertekToaster("Multiple login detected for this user", 'warningToast');}, 2000); // To show multiple user login with same credentials
            }
            const userData = authService.parseJwt(response.result.accessToken);
            if (!userData.ccode) {
                dispatch(ValidationAlert("No default company associated to the user.", "login"));
                dispatch(actions.authenticateLoginFailed());
                return false;
            }
            dispatch(actions.authenticateLoginSuccess(userData));
            authService.handleAuthentication(response.result);
            authService.handleUserData(userData);
            return true;
        }
        else if (response.code == 11 || response.code == 41) {
            const validationMessagesLen = response.validationMessages.length;
            if (validationMessagesLen > 0) {
                for (let i = 0; i < validationMessagesLen; i++) {
                    const result = response.validationMessages[i];
                    if (result.messages.length > 0) {
                        result.messages.forEach((valMessage) => {
                            dispatch(ValidationAlert(valMessage.message, "login"));
                        });
                    }
                    else {
                        IntertekToaster(localConstant.commonConstants.SERVER_ERROR, 'dangerToast ValidateLoginError');
                    }
                }
                dispatch(actions.authenticateLoginFailed());
                return false;
            }
            else if (response.messages.length > 0) {
                response.messages.forEach(result => {
                    if(result.code == "9115"){
                        IntertekToaster(result.message, 'dangerToast ValidateLoginAttemptError');  
                    } 
                    else{
                        dispatch(ValidationAlert(result.message, "login"));
                   }  
                });
            }
            else {
                dispatch(actions.authenticateLoginFailed());
                IntertekToaster(localConstant.commonConstants.SERVER_ERROR, 'dangerToast ValidateLoginError');
                return false;
            }
        }
        else {
            dispatch(actions.authenticateLoginFailed());
            IntertekToaster(localConstant.commonConstants.SERVER_ERROR, 'dangerToast ValidateLoginError');
            return false;
        }
    }
    else {
        dispatch(actions.authenticateLoginFailed());
        IntertekToaster(localConstant.commonConstants.SERVER_ERROR, 'dangerToast ValidateLoginError');
        return false;
    }
};

export const handleLogOut = (data) => async (dispatch, getstate) => {
    const loginUser = authService.getUserDetails().unique_name;
    const token = authService.getAccessToken();
    const logOutData = {
        UserName: loginUser,
        Token: token
    };
    const requestPayload = new RequestPayload(logOutData);
    const logOutUrl = loginApiConfig.logOutUrl;
    await PostData(logOutUrl, requestPayload)
        .then(res => {
            localStorage.clear();
            dispatch(ClearLoginUserDetails());
            authService.logout();
        }).catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            localStorage.clear();
            authService.logout();
        });
    dispatch(actions.authenticateLoginFailed());
    dispatch(actions.resetApplication());
};

export const RefreshToken = (data) => async (dispatch, getstate) => {
    const refreshToken = authService.getRefreshToken();
    const userData = authService.getUserDetails();
    const data = {
        "Token": refreshToken,
        "Username": userData.unique_name
    };
    const requestPayload = new RequestPayload(data);

   return await PostData(loginApiConfig.refreshToken, requestPayload)
        .then( async res => {
            if (res.code == 1) {
               await authService.handleAuthentication(res.result);
            }
        }).catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

};

export const UserMenu = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const userData = authService.getUserDetails();
    const url = loginApiConfig.userMenu.replace('{userSamaName}', encodeURIComponent(userData.unique_name)) + '?compCode=' + getstate().appLayoutReducer.selectedCompany; //D1302 Ref ALM(24-09-2020 Doc)
    const requestPayload = new RequestPayload();
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast');
            dispatch(HideLoader());
            return false;
        });
      
    if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.code) && response.code == "1") {
        let result = [];
        if (!isEmptyOrUndefine(response.result)) {
            result = response.result;
        }
        dispatch(actions.setUserMenu(result));
        authService.setUserMenu(result);
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        dispatch(HideLoader());
        return false;
    }
};

export const UserType = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const userData = authService.getUserDetails();
    const url = loginApiConfig.userType.replace('{userSamaName}', userData.unique_name) + '?compCode=' + getstate().appLayoutReducer.selectedCompany;
    const requestPayload = new RequestPayload();
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast');
            dispatch(HideLoader());
            return false;
        });
      
    if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.code) && response.code == "1") {
        let result = [];
        if (!isEmptyOrUndefine(response.result)) {
            result = response.result;
        }
        dispatch(actions.setUserType(result));
        authService.setUserType(result);
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        dispatch(HideLoader());
        return false;
    }
};
export const  authenticateLoginSuccess=(userData)=>async (dispatch, getstate) => {
    dispatch(actions.authenticateLoginSuccess(userData));
};
