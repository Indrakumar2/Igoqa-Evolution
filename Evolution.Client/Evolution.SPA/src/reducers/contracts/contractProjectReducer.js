import { contractActionTypes,sideMenu } from '../../constants/actionTypes';
import _ from 'lodash';

const initialState = {
    ContractProjects:[],
    selectedProjectStatus:'O'
};
export const ContractProjectReducer = (state = initialState, action) => {
    const { type, data } = action;
    switch (type) {
        case contractActionTypes.FETCH_CONTRACT_PROJECTS:
            state = {
                ...state,
                ContractProjects: data.responseResult,
                selectedProjectStatus:data.selectedValue
            };
            return state;      
        case sideMenu.CREATE_CONTRACT: 
            state = {
                ...state,
                ContractProjects: []
            };
            return state;    
        default:
        return state;
    }
};