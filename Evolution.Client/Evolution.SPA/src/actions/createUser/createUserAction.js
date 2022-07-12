import { CreateUserActionTypes } from '../../constants/actionTypes';
import { createUserAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';

const actions = {
    CreateUserData: (payload) => ({
        type: CreateUserActionTypes.CREATE_USER,
        data: payload
    }),   
  
};
export const CreateUserData = (data) => async (dispatch) => {
    dispatch(actions.CreateUserData(data)); 
};
