import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmentDocumentReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case assignmentsActionTypes.FETCH_DOCUMENT_TYPES: //Document Type Master Data    
            state = {
                ...state,
                documentsTypeData: data
            };
            return state;
        case assignmentsActionTypes.ADD_ASSIGNMENT_DOCUMENTS:  //Documents Add  
            if (state.assignmentDetail.AssignmentDocuments == null) {
                state = {
                    ...state,
                    assignmentDetail: {
                        ...state.assignmentDetail,
                        AssignmentDocuments: [],
                    },
                    isbtnDisable: false
                };
            }
            const newState = data.concat(state.assignmentDetail.AssignmentDocuments);
            
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentDocuments: newState,
                },
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.UPDATE_ASSIGNMENT_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentDocuments: data
                },
                editAssignmentDocumentDetails: {},
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.DELETE_ASSIGNMENT_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentDocuments: data,
                },
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_CONTRACT_DOCUMENTS: // Fetch Contracts Related Assignment Documents
            state = {
                ...state,
                assignmentContractDocumentsData: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_DOCUMENTS: // fetch Projects related Assignment documents
            state = {
                ...state,
                assignmentProjectDocumentsData: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_SUPPLIER_PO_DOCUMENTS: // fetch Visits related Assignment documents
            state = {
                ...state,
                assignmentSupplierPODocumentsData: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_VISIT_DOCUMENTS: // fetch Visits related Assignment documents
            state = {
                ...state,
                assignmentVisitDocumentsData: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_DOCUMENTS: // fetch Timesheets related Assignment documents
            state = {
                ...state,
                assignmentTimesheetDocumentsData: data
            };
            return state;
        case assignmentsActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case assignmentsActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    fileToBeUploaded: [],
                },
            };
            return state;   
            default: return state;
    }
};