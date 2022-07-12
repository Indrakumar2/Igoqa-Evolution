import { supplierActionTypes } from '../../constants/actionTypes';

export const SupplierDocumentsReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case supplierActionTypes.suppler_Documents.FETCH_SUPPLIER_DOCUMENT_TYPES: //Document Type Master Data    
            state = {
                ...state,
                documentsTypeData: data
            };
            return state;
        case supplierActionTypes.suppler_Documents.ADD_SUPPLIER_DOCUMENTS:  //Documents Add  
            if (state.supplierData.SupplierDocuments == null) {
                state = {
                    ...state,
                    supplierData: {
                        ...state.supplierData,
                        SupplierDocuments: [],
                    },
                    isbtnDisable: false
                };
            }
            const newState = data.concat(state.supplierData.SupplierDocuments);
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierDocuments: newState,
                },
                isbtnDisable: false
            };
            return state;
        case supplierActionTypes.suppler_Documents.UPDATE_SUPPLIER_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierDocuments: data
                },
                editSupplierDocumentDetails: {},
                isbtnDisable: false
            };
            return state;
        case supplierActionTypes.suppler_Documents.DELETE_SUPPLIER_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierDocuments: data,
                },
                isbtnDisable: false
            };
            return state;     
            case supplierActionTypes.suppler_Documents.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case supplierActionTypes.suppler_Documents.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    fileToBeUploaded: [],
                },
            };
            return state;   
        default: return state;
    }
};