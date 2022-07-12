import { commonActionTypes } from '../../../constants/actionTypes';
const initialState = {    
    showModal: false
};
export const ModalReducer = (state = initialState, action) => {
    const { type } = action;
   
    switch (type) {
        case commonActionTypes.SHOW_MODAL_POPUP:
                state = {
                    showModal:!state.showModal
                };
                return state;
        case commonActionTypes.HIDE_MODAL_POPUP:
                state = {
                    showModal:!state.showModal
                };
                return state;
            
        default:
            return state;
    }
};