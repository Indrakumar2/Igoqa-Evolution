import reduceReducers from 'reduce-reducers';
import { SupplierPODocumentsReducer } from '../supplierPO/supplierPODocumentReducer';
import { SupplierPONotesReducer } from '../supplierPO/supplierPONoteReducer';
import { SupplierPOSearchReducer } from '../supplierPO/supplierPOSearchReducer';
import { SupplierPOReducer } from '../supplierPO/supplierPOReducer';
const initialState = {
    supplierPOData: {
    },
    isbtnDisable: true,
    supplierPOSearchData: [],
    supplierList: [],
    selectedSupplierpo: '',
    visitSupplierPoDocuments: []
};
export default reduceReducers(
    SupplierPODocumentsReducer,
    SupplierPONotesReducer,
    SupplierPOReducer,
    SupplierPOSearchReducer,       
    initialState
);