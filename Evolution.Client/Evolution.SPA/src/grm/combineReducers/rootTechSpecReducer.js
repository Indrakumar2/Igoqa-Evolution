import { combineReducers } from 'redux';
import reduceReducers from 'reduce-reducers';
import { TechSpecSearchReducer } from '../reducers/techSpec/techSpecSearchReducers';
import { PayRateReducer } from '../reducers/techSpec/payRateReducer';
import { TaxonomyReducer } from '../reducers/techSpec/taxonomyReducer';
import { ResourceStatusReducer } from '../reducers/techSpec/resourceStatusReducer';
import { ProfessionalDetailReducer } from '../reducers/techSpec/professionalDetailReducer';
import { SensitiveDocumentsReducer } from '../reducers/techSpec/sensitiveDocumentsReducer';
import { TechSpecDocumentsReducer } from '../reducers/techSpec/techSpecDocumentReducer';
import { ResourceCapabilityReducer } from '../reducers/techSpec/resourceCapabilityReducer';
import { CommentsReducer } from '../reducers/techSpec/commentsReducer';
import { ContactInformationReducer } from '../reducers/techSpec/contactInformationReducer';
import { TaxonomyApprovalReducer } from '../reducers/techSpec/taxonomyApprovalReducer';
import { TechSpecReducer } from '../reducers/techSpec/techSpecDetailReducer';
import { TechSpecMytaskReducer }from '../reducers/techSpec/techSpecMytaskReducers';
import { TimeOffRequest } from '../reducers/techSpec/timeOffRequestReducer';
import { PreAssignmentReducer } from '../reducers/techSpec/preAssignmentReducer';
import { QuickSearchReducer }from '../reducers/techSpec/quickSearchReducer';
import { TechSpecMySearchReducer } from '../reducers/techSpec/mySearchReducer';
import { GlobalCalendarReducer } from '../reducers/techSpec/globalCalendarReducer';
import { WonLostReducer } from '../reducers/techSpec/wonLostReducer';
import { ReportsReducer }  from '../reducers/techSpec/reportsReducer';
const initialState = {
  techSpecDetailTabs: [
    {
      tabHeader: "Resource Status",
      tabBody: "ResourceStatus",
      tabActive: true,
      tabDisableStatus: []

    },
    {
      tabHeader: "Contact Info",
      tabBody: "ContactInformation",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Pay Rate",
      tabBody: "PayRate",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Taxonomy Approval",
      tabBody: "TaxonomyApproval",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Taxonomy & Other Details",
      tabBody: "Taxonomy",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Professional/Education Details",
      tabBody: "ProfessionalEducational",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Capability & Certification",
      tabBody: "ResourceCapability",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Sensitive Documents",
      tabBody: "SensitiveDocuments",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Documents",
      tabBody: "Documents",
      tabActive: true,
      tabDisableStatus: []
    },
    {
      tabHeader: "Information Comments",
      tabBody: "Comments",
      tabActive: true,
      tabDisableStatus: []
    }
 
  ],
  defaultFieldType:[ 'PrimaryEmail',
                               'PrimaryAddress',
                               'SecondaryEmail',
                               'PrimaryMobile',
                               'PrimaryPhone',
                               'Emergency' ],  
                               passwordSecurityQuestionArray: [
                                { value: 'In what city does your nearest sibling live?' },
                                { value: 'In what city or town was your first job?' },
                                { value: "What is your maternal grandmother's maiden name?" },//def 1422 fix
                                { value: 'What school did you attend for sixth grade?' },
                                { value: 'What was your childhood nickname?' },
                            ],
  
  isResourceShowModal: false,
  currentPage: 'Create Profile',
  selectedProfileDetails:{},
  //Resource Status
  isShowModal: false,
  isPanelOpen: true,
  techSpecSearchData: [],
  techSpecSelectedData: [],

  isbtnDisable: true,
  interactionMode: false,
  rateScheduleEditData: {},

    newUploadDocument: false,
    TechnicalSpecialistPaySchedule:[],
    TechnicalSpecialistPayRate:[],
    payRollNameByCompany:[],
    oldProfileActionType:"",

    /** pre-assignment */
    preAssignmentDetails:{},
    isPreAssignmentWon:false,
    chCoordinators:[],
    ocCoordinators:[],
    assignmentType:[],
    draftDataToCompare:{},
    selectedProfileDraft:{},
    preAssignmentIds:[],
    /** Quick Search */
    quickSearchDetails:{},
    /** Time of Request */
    timeoffRequestDetails:[], 
      resourceNameList:[],
      employmentTypeList:[],
      timeOffRequestCategory:[],
      resourceDetails:[],
    dispositionType:[],
    mySearchData:[],
    techSpecMytaskData:[],
    isMySearchLoaded:false,
    isMyTasksLoaded:false,
    /** GRM Reports Initial State - Starts*/
    wonLostDatas:[],
    /** GRM Reports Initial State - Ends*/
    isTechSpecDataChanged:true, //D576
    companySpecificMatrixReportData:[],
    taxonomyReportData:[],
    companyBasedTSData:[],
    calendarSchedulesData:[],
};

const TechSpecDetailReducer = reduceReducers(

  QuickSearchReducer,
  TimeOffRequest,
  TechSpecSearchReducer,
  TechSpecReducer,
  PayRateReducer,
  ResourceStatusReducer,
  ProfessionalDetailReducer,
  SensitiveDocumentsReducer,
  TechSpecDocumentsReducer,
  ResourceCapabilityReducer,
  TaxonomyReducer,
  CommentsReducer,
  ContactInformationReducer,
  TaxonomyApprovalReducer,
  TechSpecMytaskReducer,
  PreAssignmentReducer,
  TechSpecMySearchReducer,
  GlobalCalendarReducer,
  WonLostReducer,
  ReportsReducer,
  initialState  
);

export default combineReducers({
  TechSpecDetailReducer,
});
