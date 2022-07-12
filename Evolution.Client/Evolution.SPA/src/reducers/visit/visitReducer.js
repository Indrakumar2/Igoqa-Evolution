import { visitActionTypes, sideMenu } from '../../constants/actionTypes';
import { isEmptyReturnDefault } from '../../utils/commonUtils';

export const VisitReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_ASSIGNMENT_VISITS:     
            state = {
                ...state,
                visitList:data      
            };
            return state;
        case visitActionTypes.SELECTED_VISIT_STATUS:
            state={
                ...state,
                selectedVisitStatus:data
            };
        return state;
        case visitActionTypes.FETCH_VISITS:     
            state = {
                ...state,
                chCoordinators: [],
                ocCoordinators: [],
                visitList:data      
            };
            return state;
        case visitActionTypes.FETCH_ASSIGNMENT_TO_ADD_VISIT:
            state = {
                ...state,
                visitDetails:data,
                visitList:[],
                isbtnDisable:true,
                visitTechnicalSpecialistsGrossMargin:null
            };
            return state;
        case visitActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR:
            state = {
                ...state,
                chCoordinators:data
            };
            return state;
        case visitActionTypes.FETCH_OPERATING_COORDINATOR:
            state = {
                ...state,
                ocCoordinators:data
            };
            return state;
        case visitActionTypes.UPDATE_START_DATE:
            state = {
                ...state,
                visitStartDate:data,
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_END_DATE:
            state = {
                ...state,
                visitEndDate:data,
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.GET_SELECTED_VISIT:
            state = {
                ...state,
                selectedVisitData :data
            };
            return state;
        case visitActionTypes.FETCH_VISIT_DETAIL_SUCCESS:
            // state = {
            //     ...state,
            //     visitDetails: data,
            //     isbtnDisable: true
            // };            
            state = {
                ...state,
                visitList:[],
                visitDetails: {
                    ...data,
                    VisitTechnicalSpecialists: data.VisitTechnicalSpecialists ? data.VisitTechnicalSpecialists.visitTechnicalSpecialists : []
                },
                visitTechnicalSpecialistsGrossMargin: data.VisitTechnicalSpecialists ? data.VisitTechnicalSpecialists.visitAccountGrossMargin ? data.VisitTechnicalSpecialists.visitAccountGrossMargin.toFixed(2) : data.VisitTechnicalSpecialists.visitAccountGrossMargin : null,
                selectedVisitStatus: data.VisitInfo.visitStatus,
                isTBAVisitStatus: (data.VisitInfo.visitStatus === 'U'),
                isTechSpecRateChanged: false,                
                isbtnDisable: true,
                isShowAllRates: false,
                UpdateVisitStatusByLineItems: false
            }; 
            return state;
        case visitActionTypes.UPDATE_VISIT_STATUS:
            state={
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitInfo:{
                         ...state.visitDetails.VisitInfo,
                         visitStatus: data
                        }
                },
                isbtnDisable:true
            };
            return state;
        case visitActionTypes.CLEAR_VISIT_SEARCH_RESULTS:
            state = {
                ...state,
                visitList:[],
                visitStartDate:'',
                visitEndDate:'',      
            }; 
            return state;
        case sideMenu.HANDLE_MENU_ACTION:
            state = {
                ...state,
                //visitDetails:{},
                isbtnDisable:true,
                UpdateVisitStatusByLineItems: false
            };
            return state;
        case  visitActionTypes.CREATE_NEW_VISIT:
            state={
                ...state,
                visitDetails: data,
                isbtnDisable: true,
                visitTechnicalSpecialistsGrossMargin:null         
            };
            return state;
        case visitActionTypes.UPDATE_SHOW_ALL_RATES:
            state = {
                ...state,
                isShowAllRates: data
            };
            return state;
        case visitActionTypes.CUSTOMER_REPORTING_NOTIFICATION_CONTANT:
                state = {
                    ...state,
                    customerReportingNotificationContant: data
                };
                return state;
        case visitActionTypes.FETCH_SUPPLIER_LIST_FOR_SEARCH:
            state = {
                ...state,
                supplierListForSearch: data
            };
            return state;
        case visitActionTypes.CLEAR_SUPPLIER_LIST_FOR_SEARCH:
            state = {
                ...state,
                supplierListForSearch: []
            };
            return state;
        case visitActionTypes.CLEAR_CALENDAR_DATA:
            state = {
                ...state,
                visitCalendarData: []
            };
            return state;
            case visitActionTypes.CLEAR_VISIT_DETAILS:
            state = {
                ...state,
                visitDetails:{}
            };
            return state;
        case visitActionTypes.ERROR_FETCHING_VISIT_CONTRACT_SCHEDULES:
             state = {
                    ...state,
                    contractScheduleCurrency :data
                };
                return state;
        case visitActionTypes.VISIT_BUTTON_DISABLE:
            state = {
                ...state,
                isbtnDisable: true,
            };
            return state;
        case visitActionTypes.VISIT_VALID_CALENDAR_DATA_SAVE:
            state = {
                ...state,
                validCalendarData: data,
            };
            return state;
        case visitActionTypes.ADD_ATTACHED_DOCUMENTS:
            state = {
                ...state,
                attachedFiles: data,
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_EXCHANGE_RATES:
            state = {
                ...state,
                visitExchangeRates: data,
            };
            return state;
        default:
            return state;
    }
};