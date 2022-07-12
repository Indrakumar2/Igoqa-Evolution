import { CreateRoleActionTypes } from '../../constants/actionTypes';

export const CreateRoleReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case CreateRoleActionTypes.CREATE_ROLE:     
            state = {
                ...state,
                newRole:data      
            };
            return state;
        case CreateRoleActionTypes.FETCH_MODULE_DATA:
            state = {
                ...state,
                moduleList: data
            };
            return state;
        default:
            return state;
    }
};