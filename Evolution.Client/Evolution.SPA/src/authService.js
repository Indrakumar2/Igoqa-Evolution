import { applicationConstants } from './constants/appConstants';
import store, { persistor } from './store/reduxStore';
import { required } from './utils/validator';
import { loginActionTypes } from './constants/actionTypes';
import dateUtil from './utils/dateUtil';
import { isEmptyOrUndefine } from './utils/commonUtils';
// import storage from 'redux-persist/lib/storage'; // defaults to localStorage
class Auth {

    initializeLocalStorage = () => {
        localStorage.setItem(applicationConstants.Authentication.ACCESS_TOKEN, "");
        localStorage.setItem(applicationConstants.Authentication.REFRESH_TOKEN, "");
        localStorage.setItem(applicationConstants.Authentication.ACCESS_TOKEN_EXPIRES_AT, "");
        localStorage.setItem(applicationConstants.Authentication.REFRESH_TOKEN_EXPIRES_AT, "");
        localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
        localStorage.setItem(applicationConstants.Authentication.USER_NAME,"");
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE,"");
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID,"");
        localStorage.setItem(applicationConstants.Authentication.DISPLAY_NAME,"");
        localStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY,"");
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE,"");
        sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY,"");
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID,"");
        localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG,"");
        localStorage.setItem(applicationConstants.Authentication.IDLE_START,"");
        sessionStorage.setItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE,[]);
        sessionStorage.setItem(applicationConstants.Authentication.USER_MENU,[]);
    }

    isAuthenticated() {
        if (this.isTokenAvailable() && !this.isAccessTokenExpired()) {
            return true;
        }
        return false;
    }

    isTokenAvailable = () => {
        if (required(localStorage.getItem(applicationConstants.Authentication.ACCESS_TOKEN))) {
            // console.log("No Access Token available");
            return false;
        }
        return true;
    }
    isAccessTokenExpired() {
        // Check whether the current time is past the
        // access token's  expiry time    
        if (required(localStorage.getItem(applicationConstants.Authentication.ACCESS_TOKEN_EXPIRES_AT))) {
            // console.log("No Access Token Expiration time available");
            return false;
        }
        const accessTokenExpires = JSON.parse(localStorage.getItem(applicationConstants.Authentication.ACCESS_TOKEN_EXPIRES_AT));
        const epochSeconds = dateUtil.getEpochSeconds();
        return epochSeconds > accessTokenExpires;
    }

    async handleAuthentication(authResult) {
        if (authResult && authResult.accessToken && authResult.refreshToken) {
            persistor.purge();
            persistor.persist(); //resumes persistence
            //dispatch an action to save the updated tokens
            await store.dispatch({
                type: loginActionTypes.TOKEN_STORAGE,
                data: authResult
            });
            await this.setSession(authResult);
        }
        // else
        // console.log("Unable to setSession for Localstorage - No Tokens are available");
    }

    setSession(authResult) {
        if (authResult && authResult.accessToken && authResult.refreshToken && authResult.accessTokenExpires && authResult.refreshTokenExpires) {
            const accessTokenExpires = authResult.accessTokenExpires;
            const refreshTokenExpires = authResult.refreshTokenExpires;

            localStorage.setItem(applicationConstants.Authentication.ACCESS_TOKEN, authResult.accessToken);
            localStorage.setItem(applicationConstants.Authentication.REFRESH_TOKEN, authResult.refreshToken);
            localStorage.setItem(applicationConstants.Authentication.ACCESS_TOKEN_EXPIRES_AT, accessTokenExpires);
            localStorage.setItem(applicationConstants.Authentication.REFRESH_TOKEN_EXPIRES_AT, refreshTokenExpires);
            localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
            return true;
        }
        else{
            // console.log("Unable to setSession for Localstorage - No Tokens and Expiration times are available");
            return false;
        }
    }

    logout() {
        // Clear access  token and ID token from local storage
        localStorage.removeItem(applicationConstants.Authentication.ACCESS_TOKEN);
        localStorage.removeItem(applicationConstants.Authentication.ACCESS_TOKEN_EXPIRES_AT);
        localStorage.removeItem(applicationConstants.Authentication.REFRESH_TOKEN);
        localStorage.removeItem(applicationConstants.Authentication.REFRESH_TOKEN_EXPIRES_AT);
        localStorage.removeItem(applicationConstants.Authentication.USER_NAME);
        localStorage.removeItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE);
        localStorage.removeItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID);
        localStorage.removeItem(applicationConstants.Authentication.DISPLAY_NAME);
        localStorage.removeItem(applicationConstants.Authentication.COMPANY_CURRENCY);
        sessionStorage.removeItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE);
        sessionStorage.removeItem(applicationConstants.Authentication.COMPANY_CURRENCY);
        sessionStorage.removeItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID);
        localStorage.removeItem(applicationConstants.Authentication.IDLE_FLAG);
        localStorage.removeItem(applicationConstants.Authentication.IDLE_START);
        sessionStorage.removeItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE);
        sessionStorage.removeItem(applicationConstants.Authentication.USER_MENU);
        persistor.purge();
    }

    isRefreshTokenExpired() {    
        if (required(localStorage.getItem(applicationConstants.Authentication.REFRESH_TOKEN_EXPIRES_AT))) {
            // console.log("No Refresh Token Expiration time available");
            return false;
        }  
        const refreshTokenExpires = JSON.parse(localStorage.getItem(applicationConstants.Authentication.REFRESH_TOKEN_EXPIRES_AT));
        const epochSeconds = dateUtil.getEpochSeconds();
        return epochSeconds > refreshTokenExpires;
    }

    getAccessToken() {
        const access_token = localStorage.getItem(applicationConstants.Authentication.ACCESS_TOKEN);
        if (!access_token) {
            return '';
        }
        return access_token;
    }

    getRefreshToken() {
        const refresh_token = localStorage.getItem(applicationConstants.Authentication.REFRESH_TOKEN);
        if (!refresh_token) {
            return '';
        }
        return refresh_token;
    }

    parseJwt(token) {
        const base64Url = token.split('.')[1];
         const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        // return JSON.parse(window.atob(base64));
       let result = window.atob(base64); 
       result = decodeURIComponent(escape(result)); //Added for D1302 French Letter Decode Fix
       return JSON.parse(result);
    };

    getUserDetails() {
        const ccode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));
        const userDetails = {
            unique_name: localStorage.getItem(applicationConstants.Authentication.USER_NAME),
            ccode: ccode,
            sub: localStorage.getItem(applicationConstants.Authentication.DISPLAY_NAME),
        };
        return userDetails;
    }

    checkUserDetailsInSession() {
        const userName = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
        const customerCode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));

        if (!(userName && customerCode)) {
            return false;
        }
        return true;
    }

    redirectToLogin(errorObj) {
        //this.logout();
        store.dispatch({
            type: loginActionTypes.AUTHENTICATE_LOGIN_FAILED,
            data: errorObj
        });
        store.dispatch({
            type: loginActionTypes.RESET_STORE,
            data: errorObj
        });
    }

    redirectToForbidden(errorObj) {
        //this.logout();
        store.dispatch({
            type: loginActionTypes.AUTHENTICATE_FORBIDDEN,
            data: errorObj
        });
    }
    removeForbidden() {
        //this.logout();
        store.dispatch({
            type: loginActionTypes.REMOVE_FORBIDDEN
        });
    }

    handleUserData(userData) {
        const user = userData;
        const userType = userData.utype.split(',');
        localStorage.setItem(applicationConstants.Authentication.USER_NAME, userData.unique_name);
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE, userData.ccode);
        localStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, userData.cid);
        localStorage.setItem(applicationConstants.Authentication.DISPLAY_NAME, userData.sub);
        localStorage.setItem(applicationConstants.Authentication.USER_TYPE, userType);
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE,userData.ccode);
        sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY,userData.currency);
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID,userData.cid);
    }

    setUserMenu(userMenus) {
        if (userMenus == undefined)
            userMenus = [];
        sessionStorage.setItem(applicationConstants.Authentication.USER_MENU, JSON.stringify(userMenus)); // Def 1382 Fix: to get tab specific menu , user can select different home company in each tab.
    }

    setUserType(userTypes) {
        if (userTypes == undefined){
            userTypes = [];
        }
       const delimitedUserTypes= userTypes.filter(s => s.userType !== ' ').map(e => e.userType).join(",");

        localStorage.setItem(applicationConstants.Authentication.USER_TYPE, delimitedUserTypes);
    }

    getUserMenu() {
        let userMenus = [];
        const state = store.getState();
        userMenus = state.loginReducer.userMenu;
        if (userMenus == undefined || userMenus.length <= 0) {
            if(sessionStorage.getItem(applicationConstants.Authentication.USER_MENU)) //ITK DEf 1511 (404Issue) Fixed 
            userMenus = JSON.parse(sessionStorage.getItem(applicationConstants.Authentication.USER_MENU)); // Def 1382 Fix: to get tab specific menu , user can select different home company in each tab.
        }
        return userMenus;
    }

    getSelectedCompanyId() {
        const state=store.getState();
        const selectedCompany = state.appLayoutReducer.selectedCompanyId;
        if (!selectedCompany) {
            return '';
        }
        return selectedCompany;
    }
}

const auth = new Auth();

export default auth;
