import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty,isEmptyReturnDefault,isEmptyOrUndefine } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchHistoricalVisit: (payload) => ({
        type: visitActionTypes.FETCH_HISTORICAL_VISIT,
        data: payload
    }),    
};

export const FetchHistoricalVisit = (isNewVisit) => async (dispatch, getstate) => {   
    const state = getstate();    
    dispatch(ShowLoader());  
    let assignmentId = 0;    
    if(isNewVisit) {        
        if(!isEmptyOrUndefine(state.rootVisitReducer.visitDetails) && 
                !isEmptyOrUndefine(state.rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId ? state.rootVisitReducer.selectedVisitData.visitAssignmentId 
                        : state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;        
    }    
    if(assignmentId) {        
        const assignmentVisits = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.GetHistoricalVisits, assignmentId);
        const  params = {
                    VisitAssignmentId: assignmentId
                };
        
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(assignmentVisits, requestPayload)
            .catch(error => {       
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.ASSIGNMENT_VISITS_FETCH_FAILED, 'dangerToast assignmentvisit');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {    
                      
            dispatch(actions.FetchHistoricalVisit(response.result));
        }
        else {        
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast assignemntSomthingWrong');
        }
    }
    dispatch(HideLoader());
};