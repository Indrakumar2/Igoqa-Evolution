import { techSpecActionTypes } from '../../constants/actionTypes';

export const CommentsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
    
            case techSpecActionTypes.commentsActionTypes.ADD_COMMENTS:
         
            if (state.selectedProfileDetails.TechnicalSpecialistNotes == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistNotes:[ ...state.selectedProfileDetails.TechnicalSpecialistNotes,data ]  
                },
                isbtnDisable: false
            };
          
            return state;
            case techSpecActionTypes.commentsActionTypes.EDIT_COMMENTS: //D661 issue8
                    state = {
                        ...state,
                        selectedProfileDetails: {
                            ...state.selectedProfileDetails,
                            TechnicalSpecialistNotes: data
                        },
                        isbtnDisable: false
                    };
            return state;
        default:
            return state;
    }
};