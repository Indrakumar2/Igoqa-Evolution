import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty,isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchVisitNotes: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_NOTES,
        data: payload
    }),
    AddUpdateVisitNotes: (payload) => ({
        type: visitActionTypes.ADD_UPDATE_VISIT_NOTES,
        data: payload
    }),
    EditVisitNotes: (payload) => ({
        type: visitActionTypes.EDIT_VISIT_NOTES,
        data: payload
    }),
};

export const AddUpdateVisitNotes = (data) => (dispatch, getstate) => {    
    dispatch(actions.AddUpdateVisitNotes(data));
};

export const EditVisitNotes = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index =state.rootVisitReducer.visitDetails.VisitNotes.findIndex(iteratedValue => iteratedValue.visitNoteId === editedData.visitNoteId);
    const newState =Object.assign([],state.rootVisitReducer.visitDetails.VisitNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditVisitNotes(newState));
    }
};

export const FetchVisitNotes = () => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitNotes)) {
        return;
    }
    //dispatch(ShowLoader());
    
    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.Notes, visitID);
    const param = {
        VisitId: visitID //723216
    };    
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_NOTES, 'wariningToast OperatingCoordinatorSWWValPreAssign');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

        if (!isEmpty(response)) {
            if (response.code === "1") {                
                dispatch(actions.FetchVisitNotes(response.result));
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
            //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast OperatingCoordinatorSWWValPreAssign');
        }

   // dispatch(HideLoader());
};