import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty,isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchInterCompanyDiscounts: (payload) => ({
        type: visitActionTypes.FETCH_INTER_COMPANY_DISCOUNTS,
        data: payload
    }),    
    UpdateICDiscounts:(payload)=>({
        type: visitActionTypes.UPDATE_VISIT_INTERCOMPANY_DISCOUNTS,
        data:payload
    })
};

export const UpdateICDiscounts = (data) => (dispatch) => {
    dispatch(actions.UpdateICDiscounts(data));    
};

export const FetchInterCompanyDiscounts = (isNewVisit) => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitInterCompanyDiscounts)) {
        return;
    }
    dispatch(ShowLoader());       
    
    let url = '';
    if(isNewVisit) {
        let assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId;
        if(!assignmentId) assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        url = visitAPIConfig.visitBaseUrl + visitAPIConfig.assignment + assignmentId + visitAPIConfig.InterCompanyDiscounts;
    } else {
        let visitId = getstate().rootVisitReducer.selectedVisitData.visitId;
        if (!visitId) visitId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitId;
        url = StringFormat(visitAPIConfig.VisitInterCompanyDiscounts, visitId);
    }
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.FETCH_INTER_COMPANY_DISCOUNTS, 'warningToast');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });            
        if (!isEmpty(response)) {
            if (response.code === "1") {
                dispatch(actions.FetchInterCompanyDiscounts(response.result));
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {                    
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {                    
                }
            }
            else {

            }
        }
        else {
            //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
        }

    dispatch(HideLoader());
};