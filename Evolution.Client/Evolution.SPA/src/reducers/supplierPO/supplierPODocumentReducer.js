import { supplierPOActionTypes } from '../../constants/actionTypes';

export const SupplierPODocumentsReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case supplierPOActionTypes.FETCH_SUPPLIERPO_DOCUMENT_TYPES: //Document Type Master Data    
            state = {
                ...state,
                documentsTypeData: data
            };
            return state;
        case supplierPOActionTypes.ADD_SUPPLIERPO_DOCUMENTS:  //Documents Add 
            if (state.supplierPOData.SupplierPODocuments == null) {
                state = {
                    ...state,
                    supplierPOData: {
                        ...state.supplierPOData,
                        SupplierPODocuments: [],
                    },
                    isbtnDisable: false
                };
            }
            
            const newState = data.concat(state.supplierPOData.SupplierPODocuments);
           
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPODocuments: newState,
                },
                isbtnDisable: false
            };
            return state;
        case supplierPOActionTypes.UPDATE_SUPPLIERPO_DOCUMENTS_DETAILS:  //Document Update           
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPODocuments: data
                },
                editSupplierDocumentDetails: {},
                isbtnDisable: false
            };
            return state;
        case supplierPOActionTypes.DELETE_SUPPLIERPO_DOCUMENTS_DETAILS: //Documents Delete 
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPODocuments: data,
                },
                isbtnDisable: false
            };
            return state;   
        case supplierPOActionTypes.FETCH_VISIT_SUPPLIER_PO_DOCUMENTS: //Documents Delete 
            state = {
                ...state,
                visitSupplierPoDocuments:data
            };
            return state;     
            case supplierPOActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    fileToBeUploaded: data,
                },
            };
            return state;

        case supplierPOActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    fileToBeUploaded: [],
                },
            };
            return state;        
        default: return state;
    }
};