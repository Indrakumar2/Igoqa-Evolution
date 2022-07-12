import { modalActionTypes } from '../../../constants/actionTypes';

const actions = {
    DisplayModal: (payload) => ({
        type: modalActionTypes.DISPLAY_MODAL,
        data: payload
    }),
    HideModal:(payload)=>({
        type:modalActionTypes.HIDE_MODAL
    }),
    CustomModalToggle:(payload)=>({
        type:modalActionTypes.TOGGLE_MODAL_STATE,
        data:payload
    })
};

export const DisplayModal = (data) => (dispatch, getstate) => {    
    dispatch(actions.DisplayModal(data));
};

export const HideModal = (data) => (dispatch, getstate) => {
    dispatch(actions.HideModal());
};

export const CustomModalToggle = (data) => (dispatch,getstate)=>{
    dispatch(actions.CustomModalToggle(data));
};