import { techSpecActionTypes } from '../../constants/actionTypes';
import { dashBoardActionTypes } from '../../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { isEmpty, getlocalizeData,isEmptyReturnDefault,parseValdiationMessage } from '../../../utils/commonUtils';
import { FetchData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { ShowLoader, HideLoader } from '../../../common/commonAction';
import { homeAPIConfig } from '../../../apiConfig/apiConfig';
import { applicationConstants } from '../../../constants/appConstants';
import { isUndefined } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();

const actions = {
    FetchMySearchData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.MY_SEARCH_DATA,
            data:payload
        }
    ),
    GetSelectedMySearch: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.GET_SELECTED_MY_SEARCH_DATA,
            data:payload
        }
    ),
    ClearMySearchData: (payload) => ({
        type: techSpecActionTypes.CLEAR_MY_SEARCH_DATA,
        data: payload
    }),
    FetchDashboardCountForMySearch:(payload)=>({
            type:dashBoardActionTypes.FETCH_DASHBOARD_COUNT,
            data:payload
    }),
};

export const GetSelectedMySearch = (payload) => (dispatch) => {
    dispatch(actions.GetSelectedMySearch(payload));
};

export const FetchMySearchData = (data) => async (dispatch,getstate) => { //changes happened for home dashboard my task and my search count not refreshing
    dispatch(ShowLoader());
    const state = getstate();
    if(isUndefined(data) && state.RootTechSpecReducer.TechSpecDetailReducer.isMySearchLoaded){
        dispatch(HideLoader()); 
        return false;
    }
    let mySearchStatus = false;
    if(isUndefined(data)) {
        mySearchStatus = (!isUndefined(state.dashboardReducer.mySearchStatus) ? state.dashboardReducer.mySearchStatus : false);      
    } else {
        mySearchStatus = data;
    }   
    const selectedCompany = state.appLayoutReducer.selectedCompany;
   // const allCoOrdinator = state.dashboardReducer.allCoOrdinator;
    const mySearchUrl = GrmAPIConfig.mySearch;
    const logOnName = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
    const param = {
        "companyCode":selectedCompany,
        "assignedTo": logOnName,
        "isAllCoordinator": mySearchStatus
    };

    const requestPayload = new RequestPayload(param);
    const response = await FetchData(mySearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_MY_SEARCH_FAILED, 'dangerToast mysearchdatafetch');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === '1') {
            if(response.result && response.result.length > 0){
                response.result.forEach(iteratedValue => {
                    iteratedValue.searchParameter = JSON.parse(iteratedValue.searchParameter);
                });
            }            
            dispatch(actions.FetchMySearchData(response.result));   
            const count={ 'MySearchCount':response.recordCount };
            const taskcount=Object.assign({},state.dashboardReducer.count,count);
            dispatch(actions.FetchDashboardCountForMySearch(taskcount)); 
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchtechspecdashboardcompmessage');       
        }
        dispatch(HideLoader());  
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_MY_SEARCH_FAILED, 'dangerToast mySearchErrMsg1');
        dispatch(HideLoader());
    }
};

export const ClearMySearchData = (data) => (dispatch,getstate) => {
    dispatch(actions.ClearMySearchData());
};