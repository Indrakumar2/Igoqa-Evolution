import { projectActionTypes } from '../../constants/actionTypes';

export const ProjectDocuments = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case projectActionTypes.projectDocuments.FETCH_DOCUMENT_TYPES: //Document Type Master Data    
            state = {
                ...state,
                documentsTypeData: data
            };
            return state;
        case projectActionTypes.projectDocuments.ADD_PROJECT_DOCUMENTS:  //Documents Add  
            if (state.projectDetail.ProjectDocuments == null) {
                state = {
                    ...state,
                    projectDetail: {
                        ...state.projectDetail,
                        ProjectDocuments: [],
                    },
                    isbtnDisable: false
                };
            }
            const newState = data.concat(state.projectDetail.ProjectDocuments);
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectDocuments: newState,
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.projectDocuments.UPDATE_PROJECT_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectDocuments: data
                },
                editProjectDocumentDetails: {},
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.projectDocuments.DELETE_PROJECT_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectDocuments: data,
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.projectDocuments.FETCH_PROJECT_CUSTOMER_DOCUMENTS: // Fetch Project Related Customer Documents
            state = {
                ...state,
                projectCustomerDocumentsData: data
            };
            return state;
        case projectActionTypes.projectDocuments.FETCH_PROJECT_CONTRACT_DOCUMENTS: // fetch Project related customer documents
            state = {
                ...state,
                projectContractDocumentsData: data
            };
            return state;
            case projectActionTypes.projectDocuments.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case projectActionTypes.projectDocuments.CLEAR_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    fileToBeUploaded: [],
                },
            };
            return state;
        default: return state;
    }
};