import { visitActionTypes } from '../../constants/actionTypes';

export const GeneralDetailReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_VISIT_ID:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitInfo: data
                },
                isbtnDisable: true
            };

            return state;
        case visitActionTypes.UPDATE_VISIT_DETAILS:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitInfo: data
                },
                isbtnDisable: false
            };

            return state;
        case visitActionTypes.FETCH_VISIT_STATUS:
            state = {
                ...state,
                visitStatus: data
            };
            return state;
        case visitActionTypes.FETCH_UNUSED_REASON:
            state = {
                ...state,
                unusedReason: data
            };
            return state;
        case visitActionTypes.FETCH_SUPPLIER_LIST:
            state = {
                ...state,
                supplierList: data
            };
            return state;
        case visitActionTypes.FETCH_SUB_SUPPLIER_LIST:
            state = {
                ...state,
                subSupplierList: data
            };
            return state;
        case visitActionTypes.FETCH_SUBSUPPLIERS_FOR_VISIT:
            state = {
                ...state,
                visitSupplierList: data
            };
            return state;
        case visitActionTypes.FETCH_TECHNICAL_SPECIALIST_LIST:
            state = {
                ...state,
                technicalSpecialistList: data
            };
            return state;
        case visitActionTypes.SELECTED_VISIT_TECHNICAL_SPECIALISTS:
            state = {
                ...state,
                visitSelectedTechSpecs: data
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialists: data
                },
            };
            return state;
        case visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST:
            // if (state.visitDetails.VisitTechnicalSpecialists == null) {
            //     state = {
            //         ...state,
            //         visitDetails: {
            //             ...state.visitDetails,
            //             VisitTechnicalSpecialists: []
            //         }
            //     };
            // }
            // state = {
            //     ...state,
            //     visitDetails: {
            //         ...state.visitDetails,
            //         VisitTechnicalSpecialists: data
            //     }
            // };
            // if (state.visitDetails.VisitTechnicalSpecialists == null) {
            //     state = {
            //         ...state,
            //         visitDetails: {
            //             ...state.visitDetails,
            //             VisitTechnicalSpecialists: []
            //         }
            //     };
            // }
            // if (state.visitDetails.VisitTechnicalSpecialists &&
            //     state.visitDetails.VisitTechnicalSpecialists.length > 0) {
            //     state = {
            //         ...state,
            //         visitDetails: {
            //             ...state.visitDetails,
            //             VisitTechnicalSpecialists: [
            //                 ...state.visitDetails.VisitTechnicalSpecialists,
            //                 ...data.visitTechnicalSpecialists ]
            //         },
            //         visitTechnicalSpecialistsGrossMargin: data.visitAccountGrossMargin
            //     };
            // } else {
            //     state = {
            //         ...state,
            //         visitDetails: {
            //             ...state.visitDetails,
            //             VisitTechnicalSpecialists: data.visitTechnicalSpecialists
            //         },
            //         visitTechnicalSpecialistsGrossMargin: data.visitAccountGrossMargin
            //     };
            // }            
            return state;
        case visitActionTypes.VISIT_TECHNICAL_SPECIALIST_ADD:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialists: state.visitDetails.VisitTechnicalSpecialists == null
                        ? [ ...[], data ]
                        : [ ...state.visitDetails.VisitTechnicalSpecialists, data ]
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.VISIT_TECHNICAL_SPECIALIST_REMOVE:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitTechnicalSpecialists: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_VISIT_VALIDATION_DATA:
            state = {
                ...state,
                visitValidationData: data
            };
            return state;
        case visitActionTypes.IS_TBA_VISIT_STATUS:
            state = {
                ...state,
                isTBAVisitStatus: data
            };
            return state;
        case visitActionTypes.ADD_CALENDAR_DATA:
            state = {
                ...state,
                visitCalendarData: state.visitCalendarData ? [ ...state.visitCalendarData, data ] : [ ...[], data ],
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.UPDATE_CALENDAR_DATA:
            state = {
                ...state,
                visitCalendarData: data,
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.REMOVE_TS_CALENDAR_DATA:
            state = {
                ...state,
                visitCalendarData: state.visitCalendarData ? state.visitCalendarData.filter(a => a.technicalSpecialistId !== data) : [],
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};