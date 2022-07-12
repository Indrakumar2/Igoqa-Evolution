import { visitActionTypes } from '../../constants/actionTypes';

export const VisitReferenceReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_VISIT_REFERENCE:     
            if (state.visitDetails.VisitReferences == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitReferences: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitReferences: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_REFERENCE:
            if (state.visitDetails.VisitReferences == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitReferences: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitReferences: [
                        data,
                        ...state.visitDetails.VisitReferences
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_REFERENCE:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitReferences: data
                },
                isbtnDisable: false
            };
            return state;
            case visitActionTypes.UPDATE_VISIT_REFERENCE:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_REFERENCE_TYPES:     
            state = {
                ...state,
                visitReferenceTypes: data      
            };
            return state;
        default:
            return state;
    }
};