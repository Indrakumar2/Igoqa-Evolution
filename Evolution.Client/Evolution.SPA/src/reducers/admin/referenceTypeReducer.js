import { adminActionTypes } from '../../constants/actionTypes';

export const ReferenceTypeReducer = (state , actions) => {
    const { data, type } = actions;
    switch (type) {
        case adminActionTypes.referenceType.FETCH_LANGUAGES:
            state = {
                ...state,
                languageData: data
            };
            return state;
        case adminActionTypes.referenceType.FETCH_REFERENCE_TYPE:
            state = {
                ...state,
                referenceTypeData: data

            };
            return state;
        case adminActionTypes.referenceType.ADD_REFERENCE_TYPE:
            const newState = Object.assign([], state.referenceTypeData);

            newState.push(data);
            state = {
                ...state,
                referenceTypeData: newState
            };

            return state;
        case adminActionTypes.referenceType.UPDATE_REFERENCE_TYPE:
            state = {
                ...state,
                referenceTypeData: data
            };
            return state;
        case adminActionTypes.referenceType.DELETE_REFERENCE_TYPE:
            state = {
                ...state,
                referenceTypeData: data
            };
            return state;
        default:
            return state;
    }

};