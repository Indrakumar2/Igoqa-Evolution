import axios from 'axios';
import { configuration } from '../../appConfig';
import authService from '../../authService';
import { applicationConstants } from '../../constants/appConstants';
import { loginApiConfig } from '../../apiConfig/apiConfig';

// `baseURL` will be prepended to `url` unless `url` is absolute.
axios.defaults.baseURL = configuration.apiBaseUrl;

axios.interceptors.request.use(
  config => {
    const accessToken = authService.getAccessToken();
    if(accessToken){
      config.headers.Authorization  = `Bearer ${ accessToken }`;
    }
    config.headers.Pragma ='no-cache';
    config.headers.client_code  = applicationConstants.client_code;
    config.headers.client_aud_code  = applicationConstants.client_aud_code;
    return config;
  },
  error => Promise.reject(error)
);

export const processApiRequest = (URL,requestOptions) =>{    
    return axios(URL,requestOptions)
            .then(request=>request)
            .catch(error=>error);
};

/**
 * @param {*} url 
 * @param {*} config 
 */
export const TokenRenew = async (url, config) => {
  axios.defaults.headers.post['Content-Type'] = 'application/json';
  delete axios.defaults.headers.common.Authorization;
  const response = await axios.post(url, config.data);
  return response;
};

// let isRefreshing = false;
// let subscribers = [];

// axios.interceptors.response.use(function (response) {
//     return response;
// }, function (err) {
//     const { config, response } = err;
//     const { status } = response;
//     const originalRequest = config;
//     if ((response && response.status === 404) || (response && response.status === 500) || (response === undefined && message === "Network Error")) {
//         const errorObj = {
//             status: response && response.status ? response.status : 404,
//             error: error
//         };
//         authService.redirectToLogin(errorObj);
//     }
//     else if (status === 401) {
//         if (!isRefreshing) {
//             isRefreshing = true;

//             const refreshToken = authService.getRefreshToken();
//             const userData = authService.getUserDetails();
//             const renwTokenData = {
//                 "Token": refreshToken,
//                 "Username": userData.unique_name
//             };
//             RenewJwtTokenAsync().then(response => {
//                 if (response && response.data && response.data.code == 1) {
//                     isRefreshing = false;
//                     const { data } = response;
//                     authService.handleAuthentication(responseData.data.result);
//                     onRrefreshed(data.result.accessTokenExpires);                    
//                 }
//                 else {
//                     authService.redirectToLogin(false);
//                 }
//                 subscribers = [];
//             });
//         }
//         const requestSubscribers = new Promise(resolve => {
//             subscribeTokenRefresh(token => {
//                 originalRequest.headers.Authorization = `Bearer ${ token }`;
//                 resolve(axios(originalRequest));
//             });
//         });
//         return requestSubscribers;
//     }
//     return Promise.reject(err);
// });

// function subscribeTokenRefresh(cb) {
//     subscribers.push(cb);
// }

// function onRrefreshed(token) {
//     subscribers.map(cb => cb(token));
// }

// export const RenewJwtTokenAsync = async (url, config) => {
//     axios.defaults.headers.post['Content-Type'] = 'application/json';
//     delete axios.defaults.headers.common.Authorization;
//     const response = await axios.post(url, config.data);
//     if (response && response.status === 200 && response.statusText === 'OK') {
//         return response.data;
//     }
//     return response;
// };

// axios.interceptors.response.use(function (response) {
//     return response;
//   }, function (error) {
//     const originalRequest = error.config;
//     const { response,message } = error;
//     if ((response && response.status === 404) || (response && response.status === 500)
//       || (response === undefined && message === 'Network Error')) {
//       // Redirect to Error page 
//       const errorObj={
//         status: response && response.status? response.status : 404,
//         error:error
//       };
//       authService.redirectToLogin(errorObj);
//     }

//     if (error.response && error.response.status === 401 && !originalRequest._retry) {
//       originalRequest._retry = true;

//       //get refresh token
//       const refreshToken = authService.getRefreshToken();
//       const userData = authService.getUserDetails();
//         const data = {
//           "Token": refreshToken,
//           "Username":userData.unique_name
//       };
//       //make refresh token request
//       return TokenRenew(loginApiConfig.refreshToken, { data:data })
//         .then((responseData) => {
//           if (responseData && responseData.data && responseData.data.code == 1) {
//           authService.handleAuthentication(responseData.data.result);
//           axios.defaults.headers.common['Authorization'] =`Bearer ${ responseData.data.result.accessToken }`;
//           originalRequest.headers['Authorization'] = `Bearer ${ responseData.data.result.accessToken }`;
//           //retry failed request
//           return axios(originalRequest);
//           }
//           else{
//             authService.redirectToLogin(null);
//           }
//         }).catch(function (error) {
//           // auth service redirect to login page by showing appropriate message
//           const errorObj={
//             status: error && error.status? error.status : 404,
//             error:error
//           };
//           authService.redirectToLogin(errorObj);
//         });
//     }

//     return Promise.reject(error);
//   });