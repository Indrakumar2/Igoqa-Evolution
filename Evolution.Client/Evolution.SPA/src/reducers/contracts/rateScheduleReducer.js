import { contractActionTypes } from '../../constants/actionTypes';
const initialState = {
    isRateScheduleEdit:false,
    isChargeTypeEdit:false,
    isRateScheduleOpen:false,
    isChargeTypeOpen:false,
    isAdminContractRatesOpen:false,
    rateSchedule:[],
    chargeTypes:[],
    rateScheduleEditData:{},
    chargeTypeEditData:{},
    AdminChargeScheduleValueChange:{},
    selectedRowIndex:0,
};

export const RateScheduleReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case contractActionTypes.commonActionTypes.SET_CONTRACT_DATA:
            state={
                ...state,
                rateSchedule:data.rateSchedule
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_RATESCHEDULE_EDIT:
            state = {
                ...state,
                isRateScheduleEdit:data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_CHARGETYPE_EDIT:
            state={
                ...state,
                isChargeTypeEdit: data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_RATESCHEDULE_OPEN:
            if(data === false){
                state={
                    ...state,
                    rateScheduleEditData:{}
                };
            }
            state={
                ...state,
                isRateScheduleOpen: data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_CHARGETYPE_OPEN:
            if(data === false){
                state={
                    ...state,
                    chargeTypeEditData:{}
                };
            }
            state={
                ...state,
                isChargeTypeOpen: data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_COPY_CHARGETYPE_OPEN:
            state = {
                ...state,
                isCopyChargeTypeOpen: data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.IS_ADMIN_CONTRACT_RATES_OPEN:
            state = {
                ...state,
                isAdminContractRatesOpen: data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.ADD_RATE_SCHEDULE:
            if(state.contractDetail.ContractSchedules == null){
                state={
                    ...state,
                    contractDetail:{
                        ...state.contractDetail,
                        ContractSchedules:[]
                    },
                    isbtnDisable: false
                };
            }
            state={
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractSchedules:[ ...state.contractDetail.ContractSchedules,data ]
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.EDIT_RATE_SCHEDULE:
            state={
                ...state,
                rateScheduleEditData:data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.UPDATE_RATE_SCHEDULE:
            state={
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractSchedules: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.DELETE_RATE_SCHEDULE:
            state = {
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractSchedules:data
                }, 
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.ADD_CHARGE_TYPE:
            if(!state.contractDetail.ContractScheduleRates){
                state = {
                    ...state,
                    contractDetail:{
                        ...state.contractDetail,
                        ContractScheduleRates: []
                    },
                    isbtnDisable: false
                };    
            }
            state = {
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractScheduleRates:[ data,...state.contractDetail.ContractScheduleRates ]
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.EDIT_CHARGE_TYPE:
            state = {
                ...state,
                chargeTypeEditData:data
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.UPDATE_CHARGE_TYPE:
            state = {
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractScheduleRates:data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.DELETE_CHARGE_TYPE:
            state = {
                ...state,
                contractDetail:{
                    ...state.contractDetail,
                    ContractScheduleRates:data
                }, 
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.ADMIN_CHARGE_RATE_SELECT:
            state = {
                ...state,
                adminRateToCopy:data
            };
            return state;
            case contractActionTypes.rateScheduleActionTypes.ADMIN_CHARGE_RATE_CHANGE:
                state={
                    ...state,
                    AdminChargeScheduleValueChange:data
                };
                return state;
            case contractActionTypes.rateScheduleActionTypes.CLEAR_ADMIN_CHARGE_RATE_CHANGE:
                    state={
                        ...state,
                        AdminChargeScheduleValueChange:[]
                    };
                    return state;
        case contractActionTypes.rateScheduleActionTypes.CHARGE_RATES:
            state = {
                ...state,
                adminChargeRatesValue: [ state.adminChargeRatesValue[0], ...data ]
            };
            return state;
        case contractActionTypes.rateScheduleActionTypes.UPDATE_CHARGE_RATES:  
                state={
                    ...state,
                    adminChargeRatesValue:data
                };
                return state;
        case contractActionTypes.rateScheduleActionTypes.UPDATE_SELECTED_SCHEDULE:
                state={
                    ...state,
                    selectedRowIndex: data
                };
                return state;
        default:
            return state;
    }
};