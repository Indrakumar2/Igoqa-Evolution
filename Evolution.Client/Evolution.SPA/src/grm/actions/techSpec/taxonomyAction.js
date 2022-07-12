import { techSpecActionTypes } from '../../constants/actionTypes';

import { mergeobjects, convertObjectToArray, isEmpty, isEmptyReturnDefault, mapArrayObject } from '../../../utils/commonUtils';
import { object } from 'prop-types';

const actions = {

    AddInternalTrainingDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.ADD_INTERNAL_TRAINING,
            data: payload
        }
    ),
    DeleteInternalTrainingDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.DELETE_INTERNAL_TRAINING,
            data: payload
        }
    ),
    UpdateInternalTrainingdetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.UPDATE_INTERNAL_TRAINING,
            data: payload
        }
    ),
    AddCompetencyDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.ADD_COMPETENCY_DETAILS,
            data: payload
        }
    ),
    DeleteCompetencyDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.DELETE_COMPETENCY,
            data: payload
        }
    ),
    UpdateCompetencydetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.UPDATE_COMPETENCY,
            data: payload
        }
    ),
    AddCustomerApprovalDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.ADD_CUSTOMER_APPROVAL_DETAILS,
            data: payload
        }
    ),
    DeleteCustomerApprovedDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.DELETE_CUSTOMER_APPROVED,
            data: payload
        }
    ),
    UpdateCustomerApprovedDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyActionTypes.UPDATE_CUSTOMER_APPROVED,
            data: payload
        }
    ),
    NewUploadDocumentHandler: (payload) => ({
        type: techSpecActionTypes.taxonomyActionTypes.IS_NEW_UPLOAD_DOCUMENT,
        data: payload
    })
};

export const NewUploadDocumentHandler = (data) => (dispatch) => {
    dispatch(actions.NewUploadDocumentHandler(data));
};

export const AddInternalTrainingDetails = (data) => (dispatch) => {
    dispatch(actions.AddInternalTrainingDetails(data));

};
export const DeleteInternalTrainingDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    // const competencyData = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancyType;
   
    // if (!isEmpty(competencyData)) {
    //     data.map(row => {
    //         competencyData.map(compVal => {
    //             if (compVal.technicalSpecialistTrainingAndCompetencyId === row.id) {
    //                 const compIndex = competencyData.findIndex(value => value.technicalSpecialistTrainingAndCompetencyId === row.id);
    //                 if (row.recordStatus !== "N") {
    //                     competencyData[compIndex].recordStatus = "D";
    //                 }
    //                 else {
    //                     competencyData.splice(compIndex, 1);
    //                 }
    //             }
    //         });
    //     });
    // }

    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInternalTraining);
    
    data.map(row => {
        newState.map(iteratedValue => {
           
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                // iteratedValue.technicalSpecialistInternalTrainingTypes[0].recordStatus="D";
                iteratedValue.technicalSpecialistInternalTrainingTypes.forEach(x=>x.recordStatus="D");
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteInternalTrainingDetails({ newState }));
};

export const UpdateInternalTrainingdetails = (editedRowData,updatedData) => (dispatch, getstate) => {

    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
  
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInternalTraining).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInternalTraining);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateInternalTrainingdetails(newState));
    }
};

export const AddCompetencyDetails = (data) => (dispatch) => {
    dispatch(actions.AddCompetencyDetails(data));
};
export const DeleteCompetencyDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    let competencyData = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancyType);
    const resultSet2 = [];
    if (competencyData) {
        data.map(dt => {
            const resultSet1 = competencyData.filter(val => {
                return (dt.id === val.technicalSpecialistTrainingAndCompetencyId);
            });
            if (resultSet1) {
                resultSet1.map(x => {
                    if (dt.id === x.technicalSpecialistTrainingAndCompetencyId && dt.recordStatus !== "N") {
                        x.recordStatus = "D";
                        return resultSet2.push(x);
                    }
                    if (dt.id === x.technicalSpecialistTrainingAndCompetencyId && dt.recordStatus === "N") {
                        return resultSet2.push(x);
                    }
                });
            }
        });
    }

    const recordsToRemove = resultSet2.filter(x => { return x.recordStatus === "N"; });
    if (competencyData) {
        competencyData = competencyData.filter(ele => {
            return !recordsToRemove.includes(ele);
        });
    }

    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancy);
    data.map(row => {
        newState.map(iteratedValue => {
          
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                iteratedValue.technicalSpecialistCompetencyTypes.forEach(x=>x.recordStatus="D");
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteCompetencyDetails({ newState, competencyData }));
};

export const UpdateCompetencydetails = (editedRowData,updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const newItemData = [];
    const editedRow = mergeobjects(editedRowData, updatedData);
    const array = [];
    const finalData = [];
    // if (editedRow && editedRow.trainingOrCompetencyTypeNames) {
    //     array.push(editedRow.trainingOrCompetencyTypeNames);
    // } //Commented for Scenario Fixes #127 (REf mail 30-03-2020)
    let competencyData = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancy);
    const matchedIdRecords = competencyData.filter(function (cd) {
        return editedRow.id === cd.id;
    });

    // const itemsTobeRemoved =  matchedIdRecords[0] && matchedIdRecords[0].technicalSpecialistCompetencyTypes.filter(val => {
    //     return array.indexOf(val.typeName) === -1;
    // });

    let recordsToAdd = [];
    recordsToAdd = matchedIdRecords[0] && matchedIdRecords[0].technicalSpecialistCompetencyTypes.filter(val => {
        return array.indexOf(val.typeName) > -1;
    });
    // recordsToAdd = array.filter(val => {
    //     return !recordsToAdd.some(d => d.typeName === val);
    // });

    if (array.length === 1) {
        recordsToAdd.push(array[0]);
    }

    // itemsTobeRemoved.forEach(x => {
    //     if (x.recordStatus === "N") {
    //         return finalData.push(x);
    //     }
    //     else {
    //         x.recordStatus = "D";
    //         return finalData.push(x);
    //     }
    // });

    const recordsToRemove = finalData.filter(x => { return x.recordStatus === "N"; });
    if (recordsToRemove) {
        competencyData = competencyData.filter(ele => {
            ele.technicalSpecialistCompetencyTypes.filter(x => {
                return !recordsToRemove.includes(x);
            });
        });
    }

    if (!isEmpty(recordsToAdd)) {
        let competancyInfo = {};
        for (let i = 0; i < recordsToAdd.length; i++) {
            competancyInfo["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
            //this.competancyInfo["name"] = null;
            competancyInfo["epin"] = editedRow.epin;
            competancyInfo["technicalSpecialistTrainingAndCompetencyId"] = editedRow.id;
            competancyInfo["recordType"] = "Co";
            competancyInfo["typeName"] = recordsToAdd[i];
            competancyInfo["recordStatus"] = "N";
            competancyInfo["modifiedBy"] = editedRow.modifiedBy;
            newItemData.push(competancyInfo);
            competancyInfo = {};
        }
    }

    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancy)
        .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancy);
        newState[index]=editedRow;
    if (newState[index].recordStatus !== "N") {
        newState[index].recordStatus = 'M';
    }
    const removedAndNewCompetency = [ ...finalData, ...newItemData ];

    const finalArray = matchedIdRecords[0].technicalSpecialistCompetencyTypes.filter(rec => {
        return !removedAndNewCompetency.includes(rec);
    });

    //  newState[index].technicalSpecialistCompetencyTypes = [ ...finalArray, ...removedAndNewCompetency ];//Commented for Scenario Fixes #127 (REf mail 30-03-2020)

    if (index >= 0) {
        //newState[index].technicalSpecialistCompetencyTypes.splice(0,1);//Commented for Scenario Fixes #127 (REf mail 30-03-2020)
        dispatch(actions.UpdateCompetencydetails(newState));
    }

};

export const AddCustomerApprovalDetails = (data) => (dispatch) => {
    dispatch(actions.AddCustomerApprovalDetails(data));

};

export const DeleteCustomerApprovedDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCustomerApproval);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteCustomerApprovedDetails(newState));
};

export const UpdateCustomerApprovedDetails = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCustomerApproval)
        .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCustomerApproval);
    //editedRow.documents=editedRow.documents.filter(x => x.recordStatus !=='D');  //def848 doc highlight

    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateCustomerApprovedDetails(newState));
    }
};