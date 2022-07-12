import { ViewUserActionTypes } from '../../constants/actionTypes';
import { viewUserAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,mergeobjects, convertObjectToArray,isEmptyReturnDefault  } from '../../utils/commonUtils';
const localConstant = getlocalizeData();

const actions = {
    FetchViewUser: (payload) => ({
        type: ViewUserActionTypes.FETCH_VIEWUSER,
        data: payload
    }),
    DeleteUserDetails: (payload) => (
        {
            type: ViewUserActionTypes.DELETE_USER_DETAILS,
            data: payload
        }
    ),
  
};
export const FetchViewUser = () => async (dispatch) => {   
    const Url = "";
    const  params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => { 
            // console.error(error); // To show the error details in console           
            // IntertekToaster(localConstant.errorMessages.VIEWUSER_FAILED, 'dangerToast viewUser');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {        
        dispatch(actions.FetchViewUser(response.result));      
        return response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewUserSomthingWrong');
    }
};
// Dispatch action to delete the stamp details from store
export const DeleteUserDetails = (data) => (dispatch, getstate) => {
    // const state = getstate();
    // const newState = convertObjectToArray(state.rootAdminReducer.roleData);
    // data.map(row => {
    //     newState.map((iteratedValue, index) => {
    //         if (iteratedValue.id === row.id) {
    //             if (iteratedValue.recordStatus !== "N") {
    //                 newState[index].recordStatus = "D";
    //             }
    //             else {
    //                 newState.splice(index, 1);
    //             }
    //         }
    //     });
    // });
    dispatch(actions.DeleteUserDetails());
};