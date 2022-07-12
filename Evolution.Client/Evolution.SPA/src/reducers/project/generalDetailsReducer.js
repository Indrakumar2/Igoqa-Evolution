import { projectActionTypes } from '../../constants/actionTypes';

export const ProjectGeneralDetails = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.FETCH_PROJECT_COMPANY_COST_CENTER:
            state = {
                ...state,
                projectCompanyCostCenter:data
            };
            return state;
        case projectActionTypes.FETCH_PROJECT_COMPANY_DIVISION:
            state = {
                ...state,
                projectCompanyDivision:data
            };
            return state;
        case projectActionTypes.FETCH_PROJECT_COMPANY_OFFICES:
            state = {
                ...state,
                projectCompanyOffices:data
            };  
            return state;
            case projectActionTypes.UPDATE_PROJECT_GENERAL_DETAILS:
            state = {
                ...state,
                projectDetail:{
                    ...state.projectDetail,
                    ProjectInfo:data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.FETCH_PROJECT_COORDINATOR:
            state = {
                ...state,
                projectCoordinator:data
            };
            return state;
        case projectActionTypes.FETCH_PROJECT_MICOORDINATOR:
            state = {
                ...state,
                projectMICoordinator:data
            };
            return state;
        default:
            return state;
    }
};