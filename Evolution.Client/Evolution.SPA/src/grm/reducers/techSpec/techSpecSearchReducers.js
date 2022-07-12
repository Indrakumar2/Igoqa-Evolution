import { sideMenu } from '../../../constants/actionTypes';
import { techSpecActionTypes } from '../../constants/actionTypes';
export const TechSpecSearchReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {       
        case techSpecActionTypes.techSpecSearch.TECH_SPEC_SERACH_DATA:
            state = {
                ...state,
                techSpecSearchData:data
            };
            return state;
            case techSpecActionTypes.techSpecSearch.GET_SELECTED_DATA:
            state = {
                ...state,
                techSpecSelectedData:data
            };
            return state;
            case techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_DRAFT_SELECTED_PROFILE:
            state={
                ...state,
                selectedProfileDetails:{
                    TechnicalSpecialistInfo:data.technicalSpecialistInfo,
                    TechnicalSpecialistPayRate:data.technicalSpecialistPayRate,
                    TechnicalSpecialistStamp:data.technicalSpecialistStamp,
                    TechnicalSpecialistPaySchedule:data.technicalSpecialistPaySchedule,
                    TechnicalSpecialistContact:data.technicalSpecialistContact,
                    TechnicalSpecialistTaxonomy:data.technicalSpecialistTaxonomy,
                    TechnicalSpecialistInternalTraining:data.technicalSpecialistInternalTraining,
                    TechnicalSpecialistCompetancy:data.technicalSpecialistCompetancy,
                    TechnicalSpecialistCustomerApproval:data.technicalSpecialistCustomerApproval,
                    TechnicalSpecialistWorkHistory:data.technicalSpecialistWorkHistory,
                    TechnicalSpecialistEducation:data.technicalSpecialistEducation,
                    TechnicalSpecialistCodeAndStandard:data.technicalSpecialistCodeAndStandard,
                    TechnicalSpecialistTraining:data.technicalSpecialistTraining,
                    TechnicalSpecialistCertification:data.technicalSpecialistCertification,
                    TechnicalSpecialistCommodityAndEquipment:data.technicalSpecialistCommodityAndEquipment,
                    TechnicalSpecialistComputerElectronicKnowledge:data.technicalSpecialistComputerElectronicKnowledge,
                    TechnicalSpecialistLanguageCapabilities:data.technicalSpecialistLanguageCapabilities,
                    TechnicalSpecialistDocuments:data.technicalSpecialistDocuments,
                    TechnicalSpecialistSensitiveDocuments:data.technicalSpecialistSensitiveDocuments, //Def 793 issue 97 fix
                    TechnicalSpecialistNotes:data.technicalSpecialistNotes

                },
                prevBussinessInformationComment: data.technicalSpecialistInfo.businessInformationComment,//def 1306
                isbtnDisableDraft:false               
            };
           
            return state;
        case sideMenu.CREATE_PROFILE:       
            state = {
                ...state,
                currentPage: 'Create Profile',
                techSpecSearchData: [],
                techSpecSelectedData:{},
                selectedProfileDetails:{},
                TechnicalSpecialistPaySchedule:[],
                TechnicalSpecialistPayRate:[],
                expenseType:[],
                interactionMode: false,
                rateScheduleEditData: {},
                draftDataToCompare:{},
                selectedEpinNo:'',
                newUploadDocument: false,
                isbtnDisableDraft:false,
                TechnicalSpecialistPayScheduleOnCancel: [],
                TechnicalSpecialistPayRateOnCancel: [],   
            };
            return state;
        case sideMenu.EDIT_VIEW_TECHNICAL_SPECIALIST:
            state = {
                ...state,
                currentPage: 'Edit/View Resource',
                techSpecSearchData: [],
                techSpecSelectedData:{},
                selectedProfileDetails:{},
                draftDataToCompare:{},
                //ResourceStatus
                stampDetails:[],
                editStampDetails: {},
                uploadResourceStatusDocument: false,
                isbtnDiasble: false,
                //contact information
                contactInformationDetails: [],
                contactInformationSearchPage: [],
               uploadCertificateDocument: false,
                uploadTrainingDocument: false,
                expenseType:[],
                interactionMode: false,
                
                rateScheduleEditData: {},
                ispayRateScheduleEdit: false,
             
                  newUploadDocument: false,

            };
            return state;
            case techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_SELECTED_PROFILE:
            state = {
                ...state,
                 techSpecSelectedData:data,
                 currentPage:'Edit/View Resource'
            };
           
            return state;
         
            case techSpecActionTypes.techSpecSearch.CLEAR_TECHSPEC_DATA:
            state = {
                ...state,
                techSpecSearchData: [],
                techSpecSelectedData:{},
                selectedProfileDetails:{},
                selectedEpinNo:'',
                TechnicalSpecialistPayScheduleOnCancel: [],
                TechnicalSpecialistPayRateOnCancel: [],
               
            };
            return state;
            case techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_REFCODE:
            state = {
                ...state,
                myTaskRefCode:data              
            };
            return state;
            case techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_TYPE:
            state = {
                ...state,
                myTaskType:data              
            };
            return state;
        default:
            return state;
    }
};