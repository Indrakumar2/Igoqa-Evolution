const initialState = {    
        showModal: false
    };
    export const headerReducer = (state = initialState, action) => {
        const { type } = action;
       
        switch (type) {
            case 'ABOUT_SHOW_MODAL':
                    state = {
                        showModal:!state.showModal
                    };
                    return state;
            case 'ABOUT_HIDE_MODAL':
                    state = {
                        showModal:!state.showModal
                    };
                    return state;
                
            default:
                return state;
        }
    };