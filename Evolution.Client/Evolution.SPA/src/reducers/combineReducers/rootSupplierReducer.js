import { combineReducers } from 'redux';
import reduceReducers from 'reduce-reducers';
import { SupplierReducer } from '../supplier/supplierReducer';
import { SupplierDocumentsReducer } from  '../supplier/supplierDocumentReducer';
import { SupplierDetailReducer } from '../supplier/supplierDetailReducer';
import { SupplierSearchReducer } from '../supplier/supplierSearchReducer';
import { NotesReducer } from '../supplier/notesReducer'; 

const initialState = {
    isbtnDisable: true,
    selectedSupplier:'',
    supplierData:{},
    supplierDetails:{
      stateForAddress:[],
      cityForAddress:[],
    },
    reportsCustomer:[],
    supplierSearch:{
      state:[],
      city:[],
      supplierSearchList:[]
    },
};

const SupplierDetailReducers = reduceReducers(
    SupplierReducer,
    SupplierDetailReducer,
    SupplierSearchReducer,
    SupplierDocumentsReducer,
    NotesReducer,
    initialState
);

export default combineReducers({
    SupplierDetailReducers
});