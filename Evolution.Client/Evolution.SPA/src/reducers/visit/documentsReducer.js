import { visitActionTypes } from '../../constants/actionTypes';

export const DocumentsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_DOCUMENT_TYPES: //Document Type Master Data    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    documentsTypeData: data
                } 
            };
            return state;
        case visitActionTypes.FETCH_VISIT_DOCUMENTS:     
            if (state.visitDetails.VisitDocuments == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitDocuments: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,                    
                    VisitDocuments: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_DOCUMENTS:
            if(state.visitDetails.VisitDocuments == null)
            {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitDocuments: []
                    },
                    isbtnDisable: false
                };
            }
            const newState1 = data.concat(state.visitDetails.VisitDocuments);
            
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitDocuments: newState1,
                },
                isbtnDisable: false    
            };
            return state;   
        case visitActionTypes.UPDATE_VISIT_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitDocuments: data,
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_ASSIGNMENT_DOCUMENTS:
            state = {
                ...state,
                assignmentDocumentsData: data
            };
            return state;   
            case visitActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case visitActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    fileToBeUploaded: [],
                },
            };
            return state;   
        default:
            return state;
    }
};