import { timesheetActionTypes } from '../../constants/actionTypes';
import { deepCopy } from '../../utils/commonUtils';
export const TimesheetTechSpecsAccountsReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case timesheetActionTypes.FETCH_TECHSPEC_RATE_SCHEDULES:
            state = {
                ...state,
                techSpecRateSchedules: data
            };
            return state;
        case timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_RATE_SCHEDULE:
            if (state.defaultTechSpecRateSchedules == null) {
                state = {
                    ...state,
                    defaultTechSpecRateSchedules: []
                };
            }
            state = {
                ...state,
                defaultTechSpecRateSchedules: data
            };
        return state;
        case timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALISTS_SUCCESS:
            if (state.timesheetDetail.TimesheetTechnicalSpecialists &&
                state.timesheetDetail.TimesheetTechnicalSpecialists.length > 0) {
                const techSpecs = deepCopy(state.timesheetDetail.TimesheetTechnicalSpecialists);
                const uniqueTechSpecs =  techSpecs.filter(function (o1) {
                    // filter out (!) items in result2
                    return !data.timesheetTechnicalSpecialists.some(function (o2) {
                        return o1.pin === o2.pin;
                    });
                });
                   if(uniqueTechSpecs.length > 0) 
                    state = {
                        ...state,
                        timesheetDetail: {
                            ...state.timesheetDetail,
                            TimesheetTechnicalSpecialists: [
                                ...state.timesheetDetail.TimesheetTechnicalSpecialists,
                                ...uniqueTechSpecs ]
                        },
                        timesheetTechnicalSpecialistsGrossMargin: data.timesheetAccountGrossMargin
                    };
            } else {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialists: data.timesheetTechnicalSpecialists
                    },
                    timesheetTechnicalSpecialistsGrossMargin: data.timesheetAccountGrossMargin
                };
            }
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_TIME:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistTimes == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistTimes: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTimes: data
                }
            };
            return state;
        case timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_TIME:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistTimes == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistTimes: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTimes: [
                        data,
                        ...state.timesheetDetail.TimesheetTechnicalSpecialistTimes
                    ]
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TIME:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTimes: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TIME:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTimes: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistTravels == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistTravels: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTravels: data
                }
            };
            return state;
        case timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistTravels == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistTravels: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTravels: [
                        data,
                        ...state.timesheetDetail.TimesheetTechnicalSpecialistTravels
                    ]
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTravels: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistTravels: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistExpenses == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistExpenses: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistExpenses: data
                }
            };
            return state;
        case timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistExpenses == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistExpenses: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistExpenses: [
                        data,
                        ...state.timesheetDetail.TimesheetTechnicalSpecialistExpenses
                    ]
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistExpenses: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistExpenses: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistConsumables == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistConsumables: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistConsumables: data
                }
            };
            return state;
        case timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE:
            if (state.timesheetDetail.TimesheetTechnicalSpecialistConsumables == null) {
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetTechnicalSpecialistConsumables: []
                    }
                };
            }
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistConsumables: [
                        data,
                        ...state.timesheetDetail.TimesheetTechnicalSpecialistConsumables
                    ]
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistConsumables: data
                },
                isbtnDisable:false
            };
            return state;
        case timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialistConsumables: data
                },
                isbtnDisable:false
            };
            return state;
            case timesheetActionTypes.SAVE_UNLINKED_ASSIGNMENT_EXPENSES:
                state = {
                    ...state,
                    assignmentUnLinkedExpenses:data
                };
            return state;
        case timesheetActionTypes.SET_LINKED_ASSIGNMENT_EXPENSES: {
            if (data.length > 0) {
                const  result = deepCopy(state.assignmentUnLinkedExpenses);
                result && data.forEach(row => {
                    result.forEach((iteratedValue) => {
                        if (iteratedValue.assignmentAdditionalExpenseId === row.assignmentAdditionalExpenseId) {
                            iteratedValue.isAlreadyLinked = true;  
                            iteratedValue.recordStatus ='M';
                        }
                    });
                });
                state = {
                    ...state,
                    assignmentUnLinkedExpenses: result
                };
            }
            return state;
        }
        case timesheetActionTypes.RESET_LINKED_ASSIGNMENT_EXPENSES: {
            if (data.length > 0) {
                const  result = deepCopy(state.assignmentUnLinkedExpenses);
                result && data.forEach(row => {
                    result.forEach((iteratedValue) => {
                        if (iteratedValue.assignmentAdditionalExpenseId === row.assignmentAdditionalExpenseId) {
                            iteratedValue.isAlreadyLinked = false;  
                            iteratedValue.recordStatus = null;
                        }
                    });
                });
                state = {
                    ...state,
                    assignmentUnLinkedExpenses: result
                };
            }
            return state;
        }
        case timesheetActionTypes.COMPANY_EXPECTED_MARGIN:
                state = {
                    ...state,
                    companyBusinessUnitExpectedMargin:data
                };
            return state;
        case timesheetActionTypes.EXPENSE_TAB_OPEN:
            state = {
                ...state,
                isExpenseOpen: data
            };
            return state;
        default: return state;
    }
};