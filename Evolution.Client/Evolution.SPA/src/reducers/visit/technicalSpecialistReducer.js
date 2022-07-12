import { visitActionTypes } from '../../constants/actionTypes';
import { deepCopy } from '../../utils/commonUtils';

export const TechnicalSpecialistReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST:     
            if (state.visitDetails.VisitTechnicalSpecialists &&
                state.visitDetails.VisitTechnicalSpecialists.length > 0) {
                const techSpecs = deepCopy(state.visitDetails.VisitTechnicalSpecialists);
                const uniqueTechSpecs =  techSpecs.filter(function (o1) {
                    // filter out (!) items in result2
                    return !data.visitTechnicalSpecialists.some(function (o2) {
                        return o1.pin === o2.pin;
                    });
                });
                if(uniqueTechSpecs.length > 0) 
                    state = {
                        ...state,
                        visitDetails: {
                            ...state.visitDetails,
                            VisitTechnicalSpecialists: [
                                ...state.visitDetails.VisitTechnicalSpecialists,
                                ...uniqueTechSpecs ]
                        },
                        visitTechnicalSpecialistsGrossMargin: data.visitAccountGrossMargin
                    };
            } else {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialists: data.visitTechnicalSpecialists
                    },
                    visitTechnicalSpecialistsGrossMargin: data.visitAccountGrossMargin
                };
            }
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE:
            state = {
                ...state,
                techSpecRateSchedules:data
            };
            return state;
        case visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE:
            // if (state.defaultTechSpecRateSchedules == null) {
            //     state = {
            //         ...state,
            //         defaultTechSpecRateSchedules: []
            //     };
            // }
            state = {
                ...state,
                defaultTechSpecRateSchedules: data
                // defaultTechSpecRateSchedules: [
                //     data,
                //     ...state.defaultTechSpecRateSchedules
                // ]
            };            
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_TIME:            
            if (state.visitDetails.VisitTechnicalSpecialistTimes == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistTimes: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTimes: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_TIME:     
            if (state.visitDetails.VisitTechnicalSpecialistTimes == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistTimes: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTimes: [
                        data,
                        ...state.visitDetails.VisitTechnicalSpecialistTimes
                    ]
                },
                isbtnDisable: false
            };            
            return state;
        case visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_TIME:    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTimes: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_TIME:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTimes: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_TRAVEL:     
            if (state.visitDetails.VisitTechnicalSpecialistTravels == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistTravels: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTravels: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_TRAVEL:     
            if (state.visitDetails.VisitTechnicalSpecialistTravels == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistTravels: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTravels: [
                        data,
                        ...state.visitDetails.VisitTechnicalSpecialistTravels
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_TRAVEL:    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTravels: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_TRAVEL:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitTechnicalSpecialistTravels: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_EXPENSE:     
            if (state.visitDetails.VisitTechnicalSpecialistExpenses == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistExpenses: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistExpenses: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_EXPENSE:     
            if (state.visitDetails.VisitTechnicalSpecialistExpenses == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistExpenses: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistExpenses: [
                        data,
                        ...state.visitDetails.VisitTechnicalSpecialistExpenses
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_EXPENSE:    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistExpenses: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_EXPENSE:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitTechnicalSpecialistExpenses: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE:     
            if (state.visitDetails.VisitTechnicalSpecialistConsumables == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistConsumables: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistConsumables: data
                }
            };
            return state;
        case visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE:     
            if (state.visitDetails.VisitTechnicalSpecialistConsumables == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitTechnicalSpecialistConsumables: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistConsumables: [
                        data,
                        ...state.visitDetails.VisitTechnicalSpecialistConsumables
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE:    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialistConsumables: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitTechnicalSpecialistConsumables: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_TECH_SPEC_RATE_CHANGED:
            state = {
                ...state,
                isTechSpecRateChanged: data
            };
            return state;
        case visitActionTypes.VISIT_COMPANY_EXPECTED_MARGIN:
            state = {
                ...state,
                companyBusinessUnitExpectedMargin:data
            };
        return state;
        case visitActionTypes.SAVE_UNLINKED_ASSIGNMENT_EXPENSES:
            state = {
                ...state,
                assignmentUnLinkedExpenses:data
            };
        return state;
    case visitActionTypes.SET_LINKED_ASSIGNMENT_EXPENSES: {
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
    case visitActionTypes.RESET_LINKED_ASSIGNMENT_EXPENSES: {
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
    case visitActionTypes.UPDATE_VISIT_STATUS_BY_LINE_ITEMS:
            state = {
                ...state,
                UpdateVisitStatusByLineItems: data
            };
            return state;         
    case visitActionTypes.EXPENSE_TAB_OPEN:
            state = {
                ...state,
                isExpenseOpen: data
            };
            return state;     
    default:
            return state;
    }
};