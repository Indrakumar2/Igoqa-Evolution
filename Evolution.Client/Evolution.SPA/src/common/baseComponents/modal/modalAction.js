import { commonActionTypes } from '../../../constants/actionTypes';
const actions = {
    ShowModalPopup: () => {
        return {
            type: commonActionTypes.SHOW_MODAL_POPUP            
        };
    },
    HideModalPopup: () => {
        return {
            type: commonActionTypes.HIDE_MODAL_POPUP              
        };
    },
};
export const ShowModalPopup =()=>(dispatch)=> {
    return dispatch(actions.ShowModalPopup());
};
export const HideModalPopup =()=>(dispatch)=> {
    return dispatch(actions.HideModalPopup());
};
