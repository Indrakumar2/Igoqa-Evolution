import { commonActionTypes } from '../../../constants/actionTypes';
const initialState = {
    uploadDocument: []
};
export const UploadDocumentReducer = (state = initialState, action) => {
    const { type, data } = action;

    switch (type) {
        case commonActionTypes.UPLOAD_DOCUMENT:
            state = {
                uploadDocument:data,
            };
            return state;
        case commonActionTypes.DELETE_DOCUMENTS: //Documents Delete on Cancel Button
            state = {
                ...state,
                uploadDocument: data
            };
            return state;
            case commonActionTypes.CLEAR_DOCUMENTS: //Documents Clear on Cancel Button
            state = {
                ...state,
                uploadDocument:[]
            };
            return state;
            case commonActionTypes.REVERT_DELETE_DOCUMENTS: //Documents Clear on Cancel Button
            state = {
                ...state,
                uploadDocument:data
            };
            return state;

        default:
            return state;
    }
};