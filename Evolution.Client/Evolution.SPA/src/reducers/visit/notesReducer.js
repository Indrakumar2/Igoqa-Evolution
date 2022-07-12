import { visitActionTypes } from '../../constants/actionTypes';

export const NotesReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_VISIT_NOTES:     
            if (state.visitDetails.VisitNotes == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitNotes: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,                    
                    VisitNotes: data
                }
            };
            return state;
        case visitActionTypes.ADD_UPDATE_VISIT_NOTES:     
            if (state.visitDetails.VisitNotes == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,                    
                    VisitNotes: [
                        data,
                        ...state.visitDetails.VisitNotes
                     ]
                },
                isbtnDisable: false
            };
            return state;
            case  visitActionTypes.EDIT_VISIT_NOTES : //D661 issue8
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitNotes: data
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};