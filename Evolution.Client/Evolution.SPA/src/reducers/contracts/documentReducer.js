import { contractActionTypes } from '../../constants/actionTypes';

export const DocumentReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case contractActionTypes.FETCH_DOCUMENT_TYPES: //Document Type Master Data            
            state = {
                ...state,
                documentTypeData: data,
            };
            return state;
        case contractActionTypes.FETCH_CONTRACT_CUSTOMER_DOCUMENTS:
            state = {
                ...state,
                contractCustomerdocumentsData: data,
            };
            return state;
        case contractActionTypes.FETCH_CONTRACT_PROJECT_DOCUMENTS://projectdocuments in contracts
            state={
                ...state,
                contractProjectdocumentsData:data
            };
            return state;
        case contractActionTypes.DISPLAY_CONTRACT_DOCUMENTS:
            state = {
                ...state,
                displayDocuments: data
            };
            return state;
        case contractActionTypes.ADD_CONTRACT_DOCUMENTS_DETAILS:  //Documents Add          
            if (state.contractDetail.ContractDocuments == null) {
                state = {
                    ...state,
                    contractDetail: {
                        ...state.contractDetail,
                        ContractDocuments: [],
                    },
                    isbtnDisable: false
                };
            }
            const newState = data.concat(state.contractDetail.ContractDocuments);

            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractDocuments: newState,
                },
                isbtnDisable: false,                
            };

            return state;        
        case contractActionTypes.COPY_DOCUMENTS_DETAILS: //Documents Copy 
            state = {
                ...state,
                copyDocumentDetails: data,
                isbtnDisable: false
            };
            return state;

        case contractActionTypes.DELETE_CONTRACT_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractDocuments: data,
                },
                copyDocumentDetails: [],
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.UPDATE_CONTRACT_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractDocuments: data
                },
                editContractDocumentDetails: {},
                isDocumentModelAddView: true,
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.FETCH_CONTRACT_DOCUMENTS:
            state = {
                ...state,
                parentContractDocumentsData:data
            };
            return state;
            case contractActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case contractActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    fileToBeUploaded: [],
                },
            };
            return state;
        default: return state;
    }
};