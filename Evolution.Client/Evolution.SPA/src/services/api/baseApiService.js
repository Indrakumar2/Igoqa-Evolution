import axios from 'axios';
import { configuration } from '../../appConfig';
import authService from '../../authService';
import { applicationConstants } from '../../constants/appConstants';
import { loginApiConfig } from '../../apiConfig/apiConfig';
import moment from 'moment';
import dateUtil from '../../utils/dateUtil';

// `baseURL` will be prepended to `url` unless `url` is absolute.
axios.defaults.baseURL = configuration.apiBaseUrl;

axios.interceptors.request.use(
    config => {
        const accessToken = authService.getAccessToken();
        const selectedCompany = authService.getSelectedCompanyId();
        if (accessToken) {
            config.headers.Authorization = `Bearer ${ accessToken }`;
        }
        if (selectedCompany)
            config.headers.company_id  = selectedCompany;

        config.headers.Pragma = 'no-cache';
        config.headers.client_code = applicationConstants.client_code;
        config.headers.client_aud_code = applicationConstants.client_aud_code;
        config.headers['cache-control'] = 'no-cache, no-store';
        config.headers['Strict-Transport-Security'] = 'max-age=31536000; includeSubDomains';
        config.headers['Content-Security-Policy'] = "default-src https: 'unsafe-eval' 'unsafe-inline'; object-src 'none'";
        config.headers['X-Frame-Options'] = 'SAMEORIGIN';
        config.headers['X-Content-Type-Options'] = 'nosniff';
        config.headers['Access-Control-Expose-Headers'] = 'Content-Disposition,X-Suggested-Filename';
        config.headers['Referrer-Policy'] = 'no-referrer-when-downgrade';
        config.headers['Feature-Policy'] = "microphone 'none'; geolocation 'none'";
        return config;
    },
    error => Promise.reject(error)
);

/**
 * Async - declares an asynchronous function (async function someName(){...}).

   - Automatically transforms a regular function into a Promise.
   - When called async functions resolve with whatever is returned in their body.
   - Async functions enable the use of await.

Await - pauses the execution of async functions. (var result = await someAsyncCall();).

   - When placed in front of a Promise call, await forces the rest of the code to wait until 
    that Promise finishes and returns a result.
   - Await works only with Promises, it does not work with callbacks.
   - Await can only be used inside async functions.
*/

/**
 * @param {*} url 
 * @param {*} config 
 */
export const ValidateLogin = async (url, config) => {
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    delete axios.defaults.headers.common.Authorization;
    const response = await axios.post(url, config.data);
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }
    return response;
};

/**
 * @param {*} url 
 * @param {*} config 
 */
export const PostData = async (url, config) => {
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    axios.defaults.headers.post['Accept'] = 'application/json';
    axios.defaults.headers.post['Acces-Control-Allow-Origin'] = '*';
    const response = await axios.post(url, config.data);
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }
    return response;
};

/**
 * @param {*} url 
 * @param {*} config 
 */
export const FetchData = async (url, config) => {
    const response = await axios.get(url, {
        params: config.data,
    }).catch(error => {
        // console.error(error); // To show the error details in console
    });
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }

    const responseFetch = await fetch(url, { referrerPolicy: 'no-referrer-when-downgrade' }).catch(error => {
        // console.error(error); // To show the error details in console
    });
    if (responseFetch && responseFetch.status === 200 && responseFetch.statusText === 'OK') {
        return responseFetch.data;
    }
    if(response != null){
        return response;
    }
    else if(responseFetch != null){
        return responseFetch;
    }
    
};

export const FetchDocDownload = async (url) => {
    const response = await axios({ url:url, method: 'GET',responseType: 'blob' })
    .catch(error => {
        // console.error(error); // To show the error details in console
    });
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response;
    }
    return response;
};
/**
 * @param {*} url 
 * @param {*} config 
 */
export const DeleteData = async (url, config) => {
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    axios.defaults.headers.post['Accept'] = 'application/json';
    axios.defaults.headers.post['Acces-Control-Allow-Origin'] = '*';

    const response = await axios.delete(url, config.data);
    if (response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }
    return response;
};

/**
 * @param {*} url 
 * @param {*} config 
 */
export const CreateData = async (url, config) => {
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    axios.defaults.headers.post['Accept'] = 'application/json';
    axios.defaults.headers.post['Acces-Control-Allow-Origin'] = '*';
    axios.defaults.headers.put['Content-Type'] = 'application/json';
    axios.defaults.headers.put['Accept'] = 'application/json';
    axios.defaults.headers.put['Acces-Control-Allow-Origin'] = '*';
    const response = await axios.put(url, config.data);
    if (response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }
    return response;
};

const restFulApi = {
    CreateData,
    FetchData,
    PostData,
    DeleteData
};

//if required to perform all the curd operation import this, otherwise import individual 
export default restFulApi;

//generic method to make any type of Http(GET,POST,PUT,DELETE) call
export const GenericAsyncClient = async (url, config) => {
    const response = await axios(url, config);
    if (response.status === 200 && response.statusText === 'OK') {
        return response.data;
    }
    return response;
};

export const MultiDocumentDownload = async (url,config) => {
    const response = await axios({ url:url, data:config.data, method: 'POST',responseType: 'blob' })
    .catch(error => {
        // console.error(error); // To show the error details in console
    });
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response;
    }
    return response;
};

export const MultiFetch = async (data) => {
    const response = await axios.all(data)
        .then(axios.spread(function (response1, response2) {
            const resultData = [ ...response1, response2 ];
            return resultData;
        }));
    if (response) {
        return response;
    }
    return response;
};

let isRefreshing = false;
let subscribers = [];

axios.interceptors.response.use(function (response) {
    return response;
}, function (err) {
    const { response, message } = err;
    if ((response && response.status === 404) || (response && response.status === 500) || (response === undefined && message === "Network Error")) {
        const errorObj = {
            status: response && response.status ? response.status : 404,
            error: err
        };
        // console.log("Base API interceptors error:");
        // console.log(errorObj);
        // authService.redirectToLogin(errorObj);
    }
    else if(response.status === 403){
        const errorObj = {
            status: response && response.status ? response.status : 404,
            error: err
        };
        // console.log("Base API interceptors error:");
        // console.log(response);
        authService.redirectToForbidden(errorObj);
    }
    else if (response.status === 401) {
        const originalRequest = err.config;
        if (!isRefreshing) {
            isRefreshing = true;

            const refreshToken = authService.getRefreshToken();
            const userData = authService.getUserDetails();
            const renwTokenData = {
                "Token": refreshToken,
                "Username": userData.unique_name
            };
            RenewJwtTokenAsync(loginApiConfig.refreshToken,{ data:renwTokenData }).then(response => {
                if (response && response.data && response.data.code == 1) {
                    isRefreshing = false;
                    const { data } = response;
                    authService.handleAuthentication(data.result);
                    onRrefreshed(data.result.accessTokenExpires);                    
                }
                else {
                    // TO-DO: Redirect to the page session has been expired
                    // console.log("Renew token api error:");
                    // console.log(response);
                    authService.redirectToLogin(response);
                }
                subscribers = [];
                isRefreshing = false;
            });
        }
        const requestSubscribers = new Promise(resolve => {
            subscribeTokenRefresh(token => {
                originalRequest.headers.Authorization = `Bearer ${ token }`;
                resolve(axios(originalRequest));
            });
        });
        return requestSubscribers;
    }
    return Promise.reject(err);
});

function subscribeTokenRefresh(cb) {
    subscribers.push(cb);
}

function onRrefreshed(token) {
    subscribers.map(cb => cb(token));
}

export const RenewJwtTokenAsync = async (url, config) => {
    axios.defaults.headers.post['Content-Type'] = 'application/json';
    delete axios.defaults.headers.common.Authorization;
    const response = await axios.post(url, config.data);
    if (response && response.status === 200 && response.statusText === 'OK') {
        return response;
    }
    return response;
};
  
// axios.interceptors.response.use(function (response) {
//     return response;
// }, function (error) {
//     const originalRequest = [];
//     const { response, message } = error;
//     if ((response && response.status === 404) || (response && response.status === 500)
//         || (response === undefined && message === "Network Error")) {
//         // Redirect to Error page 
//         const errorObj = {
//             status: response && response.status ? response.status : 404,
//             error: error
//         };
//         authService.redirectToLogin(errorObj);
//     }
//     if (response && response.status === 401 && !error.config._retry) {
//         originalRequest.push(error.config);
//         // originalRequest._retry = true;
//             if (!isRefreshing) {
//                 isRefreshing = true;

//         //get refresh token
//         const refreshToken = authService.getRefreshToken();
//         const userData = authService.getUserDetails();
//         const data = {
//             "Token": refreshToken,
//             "Username": userData.unique_name
//         };
//         //make refresh token request
//         return ValidateLogin(loginApiConfig.refreshToken, { data: data })
//             .then((responseData) => {
//                 if (responseData && responseData.data && responseData.data.code == 1) {
//                     authService.handleAuthentication(responseData.data.result);
//                     axios.defaults.headers.common['Authorization'] = `Bearer ${ responseData.data.result.accessToken }`;
//                     originalRequest.headers['Authorization'] = `Bearer ${ responseData.data.result.accessToken }`;
//                     //retry failed request
//                     originalRequest.map(iteratedValue=>{
//                         iteratedValue._retry = true;
//                         return axios(iteratedValue);
//                     });
//                     return axios(originalRequest);
//                 }
//                 else {
//                     authService.redirectToLogin(false);
//                 }
//                 isRefreshing = false;
//             }).catch(function (error) {
//                 // auth service redirect to login page by showing appropriate message
//                 const errorObj = {
//                     status: error && error.status ? error.status : 404,
//                     error: error
//                 };
//                 authService.redirectToLogin(errorObj);
//                     isRefreshing = false;
//             });
//         }
//     }
//     return Promise.reject(error);
// });