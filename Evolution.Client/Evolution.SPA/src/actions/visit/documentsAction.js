import { visitActionTypes, projectActionTypes } from '../../constants/actionTypes';
import { assignmentAPIConfig, visitAPIConfig, RequestPayload, masterData } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty, isEmptyOrUndefine } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchVisitDocuments: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_DOCUMENTS,
        data: payload
    }),
    FetchVisitDocumentTypes: (payload) => ({
        type: visitActionTypes.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    AddVisitDocumentDetails: (payload) => ({
        type: visitActionTypes.ADD_VISIT_DOCUMENTS,
        data: payload
    }),
    UpdateVisitDocumentDetails: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteVisitDocumentDetails: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_DOCUMENTS_DETAILS,
        data: payload
    }),
    FetchAssignmentDocuments: (payload) => ({
        type: visitActionTypes.FETCH_ASSIGNMENT_DOCUMENTS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: visitActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: visitActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchVisitDocuments = () => async (dispatch, getstate) => {
    const assignmentVisitDocUrl = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.Documents;// visitAPIConfig.visitDocuments;
    
        const visitID = getstate().rootVisitReducer.selectedVisitData.visitId;
        const param = {
            ModuleCode: "VST",
            ModuleRefCode: visitID
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(assignmentVisitDocUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code == 1) {
                dispatch(actions.FetchVisitDocuments(response.result));
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage, 'warningToast');
                            });
                        }
                        else {
                            IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'dangerToast assignmentvisitrDocumentsSWWVal');
                        }
                    });
                }
            }
            else {
                if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
            }
        }
};

export const FetchVisitDocumentTypes = (data) => async (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=project";
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(documentTypesMasterDataUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast projectDocumentTypeVal');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchVisitDocumentTypes(response.result));
        }
        else if (response.code == 41) {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map((result, index) => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            IntertekToaster(valMessage, 'warningToast');
                        });
                    }
                    else {
                        IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsTypesSWWVal');
                    }
                });
            }
        }
        else if (response.code === 11) {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map((result, index) => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            IntertekToaster(valMessage, 'danger Toast');
                        });
                    }
                    else {
                        IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
                    }
                });
            }
        }
        else {
            if (response.messages.length > 0) {
                response.messages.map(result => {
                    if (result.message.length > 0) {
                        IntertekToaster(result.message, 'warningToast');
                    }
                });
            }
        }
    }
};

export const AddVisitDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddVisitDocumentDetails(data));
};

export const UpdateVisitDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if(editedData){
    const editedRow = Object.assign({}, editedData, data);    
    const index = state.rootVisitReducer.visitDetails.VisitDocuments.findIndex(document => document.id === editedRow.id);
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitDocuments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateVisitDocumentDetails(newState));
    }
}
else{
    dispatch(actions.UpdateVisitDocumentDetails(data));
}
};

export const DeleteVisitDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteVisitDocumentDetails(newState));
};

export const FetchAssignmentDocuments = (isNewVisit) => async (dispatch, getstate) => {      
    let assignmentId = 0;
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) && 
                !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }
    if(!assignmentId) assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
    const assignmentDocUrl = StringFormat(visitAPIConfig.AssignmentDocuemnts, assignmentId);
    const param = {
        AssignmentId: assignmentId
    };
    const requestPayload = new RequestPayload(param);
    if(assignmentId){
        const response = await FetchData(assignmentDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'wariningToast fetchAssignVal');
        });    
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {            
            dispatch(actions.FetchAssignmentDocuments(response.result));
        }
        else if (response.code == 41 || response.code == 11) {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map((result, index) => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            IntertekToaster(valMessage, 'warningToast');
                        });
                    }
                    else {
                        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast assignmentProjectrDocumentsSWWVal');
                    }
                });
            }
        }
        else {
            if (response.messages.length > 0) {
                response.messages.map(result => {
                    if (result.message.length > 0) {
                        IntertekToaster(result.message, 'warningToast');
                    }
                });
            }
        }
    }
    }

};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};