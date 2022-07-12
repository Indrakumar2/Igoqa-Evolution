import { combineReducers } from 'redux';
import reduceReducers from 'reduce-reducers';

import { ProjectReducer } from '../project/projectReducer';
import { ProjectSearchReducer } from '../project/projectSearchReducer';
import { ProjectInvoicingDefaults } from '../project/invoicingDefaultReducer';
import { ProjectNotes } from '../project/noteReducer';
import { ProjectDocuments } from '../project/documentReducer';
import { ClientNotificationReducer } from '../project/clientNotificationReducer';
import { AssignmentReducer } from '../project/projectAssignmentReducer';
import { ProjectGeneralDetails } from '../project/generalDetailsReducer';
import { SupplierPoReducer } from '../project/projectSupplierPoReducer';

const initialState = {
    projectDetail:{},
    documentsTypeData:[],
    projectCustomerDocumentsData:[],
    projectContractDocumentsData:[],
    projectStatus: 'O',
    customerList: [],
    projectSearchList: [],
    isClientNotificationModalOpen: false,
    isClientNotificationEdit: false,
    projectCompanyOffices: [],
    projectCompanyDivision: [],
    projectCompanyCostCenter: [],
    assignmentData: null,
    customerContact: [],
    supplierPoData: null,
    selectedProjectNo: '',
    projectMode:'',
    projectCoordinator:[],
    projectMICoordinator:[],
    interactionMode: false,
    isbtnDisable:true,
    selectedAssignmentStatus:'No',
};

const ProjectDetailReducer = reduceReducers(
    ProjectReducer,
    ProjectSearchReducer,
    ProjectGeneralDetails,
    ProjectInvoicingDefaults,
    ProjectNotes,
    ProjectDocuments,
    ClientNotificationReducer,
    AssignmentReducer,
    SupplierPoReducer,
    initialState
);

export default combineReducers({
    ProjectDetailReducer
});
