import { techSpecActionTypes } from '../../constants/actionTypes';

export const TaxonomyApprovalReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case techSpecActionTypes.taxonomyApprovalActionTypes.ADD_TAXONOMY_APPROVAL:
            if (state.selectedProfileDetails.TechnicalSpecialistTaxonomy == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistTaxonomy: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTaxonomy: [
                        ...state.selectedProfileDetails.TechnicalSpecialistTaxonomy, data
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyApprovalActionTypes.DELETE_TAXONOMY_APPROVAL:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTaxonomy: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyApprovalActionTypes.UPDATE_TAXONOMY_APPROVAL:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTaxonomy: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyApprovalActionTypes.UPDATE_TAXONOMY_TQM_COMMENT:
            state = {
            ...state,
            selectedProfileDetails: {
                ...state.selectedProfileDetails,
                TechnicalSpecialistInfo: {
                    ...state.selectedProfileDetails.TechnicalSpecialistInfo,
                    tqmComment:data
                }
            },
            isbtnDisable: false
        };
        return state;
        default:
            return state; 
    }
};