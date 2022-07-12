import { projectActionTypes, sideMenu } from '../../constants/actionTypes';

export const ProjectReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.FETCH_PROJECT_CONTRACT_INFO:
            state = {
                ...state,
                projectDetail: data
            };
            return state;
        case projectActionTypes.SAVE_SELECTED_PROJECT_NUMBER:
            state = {
                ...state,
                selectedProjectNo: data
            };
            return state;
        case projectActionTypes.UPDATE_BTN_ENABLE_STATUS:
        state={
            ...state,
            isbtnDisable: true
        };
        return state;
        case projectActionTypes.FETCH_PROJECT_DETAIL_SUCCESS:
            state = {
                ...state,
                projectDetail: data,
                isbtnDisable: true
            };
            return state;
        case projectActionTypes.UPDATE_INTERACTION_MODE_PROJECT:        
            state={
                ...state,
                interactionMode: data
            };
            return state;
        case sideMenu.CREATE_PROJECT:
            state = {
                ...state,
                projectMode: "createProject",
                selectedProjectNo: '',
                isbtnDisable:true,
                projectDetail:{},
                projectSearchList: []
            };
            return state;
        case sideMenu.EDIT_PROJECT:
            state = {
                ...state,
                projectMode: "editProject",
                projectSearchList: [],
                isbtnDisable:true,
            };
            return state;
        case projectActionTypes.UPDATE_PROJECT_MODE:
            state = {
                ...state,
                projectMode:data
            };
            return state;
            case sideMenu.HANDLE_MENU_ACTION:
                    state = {
                        ...state,
                       projectSearchList: []
                    };
                    return state;
        default:
            return state;
    }
};