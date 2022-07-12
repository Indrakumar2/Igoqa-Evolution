import reduceReducers from 'reduce-reducers';

import { TechnicalDesciplineReducer } from '../assignment/technicalDesciplineReducer';
import { AssignmentRateScheduleReducer } from '../assignment/assignmentRateScheduleReducer';
import { AssignmentReferenceReducer } from '../assignment/assignmentReferenceReducer';
import { GeneralDetailsReducer } from '../assignment/generalDetailsReducer';
import { AssignmentReducer } from '../assignment/assignmentReducer';
import { AdditionalExpensesReducer  } from '../assignment/assignmentAdditionalExpensesReducer';
import { AssignmentICDiscountsReducer  } from '../assignment/assignmentICDiscountsReducer';
import { AssignmentDocumentReducer } from '../assignment/assignmentDocumentReducer';
import { AssignmmentNotesReducer } from '../assignment/assignmentNoteReducer';
import { AssignedSpecialistsReducer } from '../assignment/assignedSpecialistsReducer';
import { AssignmentInstructionsReducer } from '../assignment/assignmentInstructionsReducer';
import { SupplierInformationReducer } from '../assignment/supplierInformationReducer';
import { ContributionCalculatorReducer } from '../assignment/contributionCalculatorReducer';
import { ArsReducer } from '../assignment/arsSearchReducer';

const initialState = {
    assignmentDetail:{
    },   
    assignmentStatus:[],
    assignmentList:[],    
    dataToValidateAssignment: {},
    selectedAssignmentNumber: '',
    paySchedules:[],
    chargeRates:[],
    payRates:[],
    isbtnDisable:true,
    isContrinutionCalculatorModified:false,
    assignedSubSupplierTS:[],
    isSupplierPOChanged:false,
    isARSSearch:false,
    preAssignmentIds:[],
    selectedPreAssignment:{},
    isResourceMatched:true,
    unMatchedResources:[],
    isAssignResourceClicked:false,
    operationalManagers:[],
    technicalSpecialists:[],
    selectedARSMyTask:{},
    defaultPreAssignmentID:'',
    ploTaxonomyService:[],
    ploTaxonomySubCategory:[],
    isAssignmentHasFinalVisit:false,
    isSubSupplierContactUpdated: true,
    isInterCompanyDiscountChanged: false,
    selectedTechSpec: [],
};

export default reduceReducers(
    TechnicalDesciplineReducer,
    AssignmentRateScheduleReducer,
    AssignmentReferenceReducer,
    GeneralDetailsReducer,
    AdditionalExpensesReducer,
    AssignmentICDiscountsReducer,
    AssignmentDocumentReducer,
    AssignmmentNotesReducer,
    AssignmentInstructionsReducer,  //AssignmentInstructionsReducer Added
    SupplierInformationReducer, //supplierInformationReducer Added
    AssignmentReducer,
    AssignedSpecialistsReducer,
    ContributionCalculatorReducer,
    ArsReducer,
    initialState
);