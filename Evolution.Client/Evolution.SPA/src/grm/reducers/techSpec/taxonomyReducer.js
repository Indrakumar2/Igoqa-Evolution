import { techSpecActionTypes } from '../../constants/actionTypes';

export const TaxonomyReducer = (state, action) => {  
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.taxonomyActionTypes.IS_NEW_UPLOAD_DOCUMENT:
            state = {
                ...state,
                newUploadDocument: data,
            };
            return state;

        case techSpecActionTypes.taxonomyActionTypes.ADD_INTERNAL_TRAINING:

            if (state.selectedProfileDetails.TechnicalSpecialistInternalTraining == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistInternalTraining: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInternalTraining: [
                        ...state.selectedProfileDetails.TechnicalSpecialistInternalTraining, ...data
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyActionTypes.DELETE_INTERNAL_TRAINING:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInternalTraining: data.newState,
                    //TechnicalSpecialistCompetancyType: data.competencyData
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyActionTypes.UPDATE_INTERNAL_TRAINING:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInternalTraining: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyActionTypes.ADD_COMPETENCY_DETAILS:
            const array = data.trainingOrCompetencyTypeNames.split(',');
            const competancyTypeData = [];
            let competancyInfo = {};
            let dvaCharterName = {};
            competancyInfo["id"] = data.id;
            competancyInfo["name"] = data.name;
            competancyInfo["epin"] = data.epin !== undefined ? data.epin : 0;
            competancyInfo["duration"] = data.duration;
            competancyInfo["effectiveDate"] = data.effectiveDate;
            competancyInfo["expiry"] = data.expiry;
            competancyInfo["competency"] = data.competency;
            competancyInfo["notes"] = data.notes;
            competancyInfo["score"] = data.score;
            competancyInfo["documents"] = data.documents;
            competancyInfo["technicalSpecialistTrainingAndCompetencyId"] = data.id;
            competancyInfo["recordType"] = "Co";
            competancyInfo["trainingOrCompetencyTypeNames"] = data.trainingOrCompetencyTypeNames;
            competancyInfo["typeId"]=data.typeId;
            competancyInfo["recordStatus"] = "N";
            competancyInfo["modifiedBy"] = data.modifiedBy;

            for (let i = 0; i < array.length; i++) {
                dvaCharterName["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                dvaCharterName["name"] = null;
                dvaCharterName["epin"] = data.epin;
                dvaCharterName["technicalSpecialistCompetencyId"] = data.id;
                //this.dvaCharterName["recordType"] = "Co";
                dvaCharterName["typeName"] = array[i];
                dvaCharterName["recordStatus"] = competancyInfo.recordStatus;
                dvaCharterName["modifiedBy"] = data.modifiedBy;
                dvaCharterName["typeId"] = data.typeId;
                competancyTypeData.push(dvaCharterName);
                dvaCharterName = {};
            }
           competancyInfo['technicalSpecialistCompetencyTypes'] = competancyTypeData;
            if (state.selectedProfileDetails.TechnicalSpecialistCompetancy == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistCompetancy: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCompetancy: [
                        //...state.selectedProfileDetails.TechnicalSpecialistCompetancy, data
                        ...state.selectedProfileDetails.TechnicalSpecialistCompetancy, competancyInfo
                    ]

                },
                isbtnDisable: false
            };
            competancyInfo = {};
            return state;
        case techSpecActionTypes.taxonomyActionTypes.DELETE_COMPETENCY:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCompetancy: data.newState,
                    //TechnicalSpecialistCompetancyType: data.competencyData
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyActionTypes.UPDATE_COMPETENCY:
            const updateDataCompetancy = [];
            let competancyTypesubArray = [];
            let updateCompetancyInfo = {};
            let updateDvaCharterName = {};
            for (let i = 0; i < data.length; i++) {
                updateCompetancyInfo["id"] = data[i].id;
                updateCompetancyInfo["name"] = data[i].name;
                updateCompetancyInfo["epin"] = data[i].epin !== undefined ? data[i].epin : 0;
                updateCompetancyInfo["duration"] = data[i].duration;
                updateCompetancyInfo["effectiveDate"] = data[i].effectiveDate;
                updateCompetancyInfo["expiry"] = data[i].expiry;
                updateCompetancyInfo["competency"] = data[i].competency;
                updateCompetancyInfo["notes"] = data[i].notes;
                updateCompetancyInfo["score"] = data[i].score;
                updateCompetancyInfo["documents"] = data[i].documents;
                updateCompetancyInfo["technicalSpecialistTrainingAndCompetencyId"] = data[i].id;
                updateCompetancyInfo["recordType"] = "Co";
                updateCompetancyInfo["trainingOrCompetencyTypeNames"] = data[i].trainingOrCompetencyTypeNames;
                updateCompetancyInfo["typeId"] = data[i].typeId;
                updateCompetancyInfo["recordStatus"] = data[i].recordStatus;
                updateCompetancyInfo["updateCount"] = data[i].updateCount;
                updateCompetancyInfo["modifiedBy"] = data[i].modifiedBy;

                for (let j = 0; j < data[i].technicalSpecialistCompetencyTypes.length; j++) {
                    updateDvaCharterName["id"] = data[i].technicalSpecialistCompetencyTypes[j].id;
                    updateDvaCharterName["name"] = null;
                    updateDvaCharterName["epin"] = data[i].epin !== undefined ? data[i].epin : 0;
                    updateDvaCharterName["technicalSpecialistCompetencyId"] = data[i].id;
                    updateDvaCharterName["typeName"] = data[i].technicalSpecialistCompetencyTypes[j].typeName;
                    updateDvaCharterName["typeId"] = data[i].technicalSpecialistCompetencyTypes[j].typeId;
                    updateDvaCharterName["recordStatus"] = data[i].technicalSpecialistCompetencyTypes[j].recordStatus;
                    updateDvaCharterName["modifiedBy"] = data[i].technicalSpecialistCompetencyTypes[j].modifiedBy;
                    competancyTypesubArray.push(updateDvaCharterName);
                    updateDvaCharterName = {};
                }
                updateCompetancyInfo['technicalSpecialistCompetencyTypes'] = competancyTypesubArray;
                updateDataCompetancy.push(updateCompetancyInfo);
                competancyTypesubArray = [];
                updateCompetancyInfo = {};

            }
                
                state = {
                    ...state,
                    selectedProfileDetails:{
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistCompetancy:updateDataCompetancy
                    },
                    isbtnDisable: false
                };           
            return state;
        case techSpecActionTypes.taxonomyActionTypes.ADD_CUSTOMER_APPROVAL_DETAILS:
            if (state.selectedProfileDetails.TechnicalSpecialistCustomerApproval == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistCustomerApproval: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCustomerApproval: [
                        ...state.selectedProfileDetails.TechnicalSpecialistCustomerApproval, data
                    ]
                },
                isbtnDisable: false
            };

            return state;
        case techSpecActionTypes.taxonomyActionTypes.DELETE_CUSTOMER_APPROVED:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCustomerApproval: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.taxonomyActionTypes.UPDATE_CUSTOMER_APPROVED:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCustomerApproval: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.commentsActionTypes.SET_TECHSPEC_CURRENT_PAGE:
            state = {
                ...state,
                currentPage: data,
            };
            return state;
        default:
            return state;
    }
};