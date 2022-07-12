import { techSpecActionTypes } from '../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { ShowLoader, HideLoader,UpdateCurrentModule } from '../../../common/commonAction';
import { FetchData, PostData, CreateData } from '../../../services/api/baseApiService';
import { getlocalizeData, mergeobjects, isEmptyReturnDefault, isEmpty } from '../../../utils/commonUtils';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../../common/masterData/masterDataActions';
import { required } from '../../../utils/validator';
import { ClearData,SetDefaultCustomerName } from '../../../components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { ValidationAlert } from '../../../components/viewComponents/customer/alertAction';
import {
    ChangeDataAvailableStatus
} from '../../../components/appLayout/appLayoutActions';

const localConstant = getlocalizeData();
const actions = {
    FetchQuickSearchData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_QUICK_SEARCH_DATA,
            data: payload
        }
    ),
    UpdateQuickSearchData: (payload) => (
        {
            type: techSpecActionTypes.quickSearchActionTypes.UPDATE_QUICK_SEARCH_INFORMATION,
            data: payload
        }
    ),
    AddSearchResultData: (payload) => (
        {
            type: techSpecActionTypes.quickSearchActionTypes.ADD_SEARCH_RESULT_DATA,
            data: payload
        }
    ),
    AddOptionalSearch:(payload)=>({
        type: techSpecActionTypes.ADD_OPTIONAL_SEARCH,
        data:payload
    }),
    ClearQuickSearchDetails:(payload)=>({
        type: techSpecActionTypes.quickSearchActionTypes.CLEAR_QUICK_SEARCH_INFORMATION,
        data:payload
    }),
    ClearQuickSearchResults:(payload)=>({
        type: techSpecActionTypes.quickSearchActionTypes.CLEAR_QUICK_SEARCH_RESULTS,
        data:payload
    })
};

export const FetchQuickSearchData = (data) => async (dispatch,getstate) => {  
    dispatch(ShowLoader());
    if(data){
        const apiUrl = GrmAPIConfig.quickSearch;
        const param = {
            "id" : data.id
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_QUICK_SEARCH_FAILED, 'dangerToast quickSearchErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if(response.result && response.result.length > 0){

                    //646 Fixes - If search action is LOST or WON, We shouldn't show the page to user
                    if (response.result[0].searchAction === 'L' || response.result[0].searchAction === 'W') {
                        dispatch(HideLoader());
                        dispatch(ChangeDataAvailableStatus(true));
                        return response;
                    }

                    response.result[0].searchParameter = JSON.parse(response.result[0].searchParameter);
                    const quickSearchData = response.result[0].searchParameter;
                    dispatch(LoadQuickSearchMaster(quickSearchData));
                    dispatch(searchDetails(response.result[0]));
                    response.result[0].searchAction = ""; //Scenario Changes (ref:Scenario - Issues mail on 28-02-2020)
                    dispatch(actions.FetchQuickSearchData(response.result[0]));
                    dispatch(SetDefaultCustomerName(quickSearchData));
                    dispatch(UpdateCurrentModule(localConstant.moduleName.TECHSPECIALIST));//D576
                }
                else{
                    dispatch(actions.FetchQuickSearchData({}));
                    IntertekToaster(localConstant.errorMessages.FETCH_QUICK_SEARCH_FAILED, 'dangerToast quickSearchErr');
                }
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages             
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages             
                }
            }
            else {
               
            }
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast fetchQuickSearchSWRErr');
            dispatch(HideLoader());
        }
    }
    else{
        dispatch(HideLoader());
    }
};

/** Load Quick search master data based on result */
export const LoadQuickSearchMaster = (data) => (dispatch,getstate) => {
    if(!required(data.categoryName)){
        dispatch(ClearSubCategory());
        dispatch(ClearServices());
        dispatch(FetchTechSpecSubCategory(data.categoryName));
    }
    if(!required(data.categoryName) && !required(data.subCategoryName)){
        dispatch(ClearServices());
        dispatch(FetchTechSpecServices(data.categoryName , data.subCategoryName));
    }
};

export const updateQuickSearchData = (payload) => (dispatch,getstate) => {
    const state = getstate();
    dispatch(actions.UpdateQuickSearchData(payload));
};
// export const addSearchResultData = (payload) => (dispatch) => {
//     dispatch(actions.AddSearchResultData(payload));
// };

export const searchDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.quickSearchsSearch;
    const quickSearchDetails = Object.assign({},payload);
    const searchParameter=Object.assign({},quickSearchDetails.searchParameter);
    searchParameter.supplierFullAddress = searchParameter.supplierLocation;
    searchParameter.supplierLocation=`${ searchParameter.supplier ? searchParameter.supplier.replace(/,/g, '') + ',' :"" } ${ searchParameter.supplierLocation ? searchParameter.supplierLocation : "" }`;
    quickSearchDetails.searchParameter=searchParameter;
    const requestPayload = new RequestPayload(quickSearchDetails);
    let response = "";
    response = await PostData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_QUICK_SEARCH_SEARCH_FAILED, 'dangerToast ProjActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        //To-do Search Result Grid Bind
        if (response.code === "1") {
            if(!isEmpty(response.result)){
                dispatch(actions.AddSearchResultData(response.result));
            }
            else{
                dispatch(actions.AddSearchResultData([]));
            }
        }
    }
    dispatch(HideLoader());
};

export const saveQuickSearchDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.quickSearch;
    const requestPayload = new RequestPayload(payload);
    let response = "";
    response = await PostData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.QUICK_SEARCH_POST_ERROR, 'dangerToast quickSearch');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(response){
            if(response.code === '1'){
                dispatch(HideLoader());
                return true;
            }
            else if (response.code === "11" || response.code === "41") {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map(result => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                dispatch(ValidationAlert(valMessage.message, "quickSearch"));
                            });
                        }
                        else {
                            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                        }
                    });
                }
                else if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            dispatch(ValidationAlert(result.message, "quickSearch"));
                        }
                    });
                }
                else {
                    dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                }
                dispatch(HideLoader());
                return false;
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                dispatch(HideLoader());
                return false;
            }
        }
        else{
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
            dispatch(HideLoader());
            return false;
        }
};

export const updateQuickSearchDetails = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.quickSearch;
    const state = getstate();
    const quickDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.quickSearchDetails);
    quickDetails.recordStatus="M";
    const requestPayload = new RequestPayload(quickDetails);
    let response = "";
    response = await CreateData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.QUICK_SEARCH_UPDATE_ERROR, 'dangerToast quickSearch');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(response){
            if(response.code === '1'){
                dispatch(HideLoader());
                return true;
            }
            else if (response.code === "11" || response.code === "41") {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map(result => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                dispatch(ValidationAlert(valMessage.message, "quickSearch"));
                            });
                        }
                        else {
                            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                        }
                    });
                }
                else if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            dispatch(ValidationAlert(result.message, "quickSearch"));
                        }
                    });
                }
                else {
                    dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                }
                dispatch(HideLoader());
                return false;
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                dispatch(HideLoader());
                return false;
            }
        }
        else{
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
            dispatch(HideLoader());
            return false;
        }
};

export const AddOptionalSearch= (data)=>(dispatch,getstate)=>{
    if(data){
        const state = getstate();
        const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.quickSearchDetails.searchParameter);
        const optionalSearch = Object.assign({},searchParameter.optionalSearch,data);
        dispatch(actions.AddOptionalSearch(optionalSearch));
    }
};

/** Cancel Quick Search - Create Mode */
export const CancelCreateQuickSearchDetails = () => (dispatch,getstate) => {
    dispatch(clearAllQuickSearchDetails());
};
/** Cancel Quick Search - Edit Mode */
export const CancelEditQuickSearchDetails = () => (dispatch,getstate) => {
    dispatch(ClearData());
    const state = getstate();
    const quickSearchDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.quickSearchDetails);
    dispatch(FetchQuickSearchData({ id:quickSearchDetails.id }));
};

/** Clear Quick-Search Details */
export const clearAllQuickSearchDetails = () => (dispatch,getstate) => {
    dispatch(actions.ClearQuickSearchDetails());
    dispatch(actions.ClearQuickSearchResults());
    dispatch(ClearData());
    dispatch(ClearSubCategory());
    dispatch(ClearServices());
};
