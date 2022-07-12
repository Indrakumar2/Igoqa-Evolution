import { commonActionTypes } from '../../../constants/actionTypes';
import { isEmpty,isUndefined } from '../../../utils/commonUtils';
import { GrmAPIConfig } from '../../../apiConfig/apiConfig';
import { RemoveDocumentsFromDB } from '../../../common/commonAction';
const actions = {

    UploadDocumentDetails: (payload) => ({
        type: commonActionTypes.UPLOAD_DOCUMENT,
        data: payload
    }),
    RemoveDocumentDetails: (payload) => ({
        type: commonActionTypes.DELETE_DOCUMENTS,
        data: payload

    }),
    ClearDocumentUploadedDocument: (payload) => ({
        type: commonActionTypes.CLEAR_DOCUMENTS,
        data: payload

    }),
    RevertDeletedDocument: (payload) => ({
        type: commonActionTypes.REVERT_DELETE_DOCUMENTS,
        data: payload

    }),
};
export const RevertDeletedDocument =(revetedData,GridData,gridName)=> async(dispatch, getstate) => {
     await dispatch(actions.RevertDeletedDocument(revetedData));   
     return true;
};
export const ClearDocumentUploadedDocument =()=> (dispatch, getstate) => {    
    dispatch(actions.ClearDocumentUploadedDocument());
};
export const UploadDocumentDetails = (data, editedRow, gridName) => (dispatch, getstate) => {
    let finalArray =[];    
    const uploadedDocuments = getstate().RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails;
    if (gridName === "TechnicalSpecialistDocuments") {
        if (!isEmpty(uploadedDocuments[gridName]) && uploadedDocuments[gridName].length > 0 && !isEmpty(editedRow)) {
            uploadedDocuments[gridName].forEach(record => {
                if (record.id === editedRow.id){

                }
            });
        }
    }
    else { 
        if (!isEmpty(uploadedDocuments[gridName]) && uploadedDocuments[gridName].length > 0 && !isEmpty(editedRow)) {
            if(!isEmpty(data)){
            const existingFile =  getstate().UploadDocumentReducer.uploadDocument; 
            uploadedDocuments[gridName].forEach(record => {
                if (record.id === editedRow.id) {
                    if(!isEmpty(record.verificationDocuments)){
                        if(record.verificationDocuments[0].recordStatus ==='D'){
                            finalArray = [ ...existingFile,...data ]; 
                        }else{
                            finalArray = [ ...data ];
                        }
                    }else{
                        finalArray = [ ...existingFile,...data ];
                    }
                    if(!isEmpty(record.documents) && record.documents.length > 0){
                        if(record.documents[0].recordStatus === 'D'){                                            
                            finalArray = [ ...existingFile,...data ];   
                        }else{
                            finalArray = [ ...data ];
                        }
                       
                    }else{
                        finalArray = [ ...existingFile,...data ];
                    }
                    
                }
            });
            }
        } else {          
            if(!isEmpty(data)){                
                const existingFile =  getstate().UploadDocumentReducer.uploadDocument;
                finalArray = [ ...existingFile,...data ]; // ID 222 - uploaded duplicate Document issues.
                //finalArray = [ ...data ]; 
            }
           
        }
    }
    
    dispatch(actions.UploadDocumentDetails(finalArray));
};
export const RemoveDocumentDetails = (data, parentState,removeDocType) => async (dispatch, getstate) => {   
    const state = getstate(); 
    if (data) {
        //const newState = Object.assign([], state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistStamp);        
        let docStatus = [];
        const existingFile = getstate().UploadDocumentReducer.uploadDocument;
        if (parentState) {
            parentState.forEach(res => {
                if (removeDocType === 'verficationDocument') {
                    if (res.verificationDocuments && res.verificationDocuments.length > 0) {
                        res.verificationDocuments.forEach(doc => {
                            if (doc.id === data.id) {
                                data.recordStatus = 'D';
                            if (Array.isArray(existingFile) && existingFile.length === 0) {
                                docStatus = [
                                    ...existingFile, data
                                ];
                            }
                            else {
                                existingFile.forEach((file, index) => {
                                    if (file.id === data.id) {
                                        existingFile.splice(index, 1);
                                        existingFile.push(data);
                                    }
                                });
                                docStatus = existingFile;
                            }
                            }
                        });
                    }
                } else if (res.documents && res.documents.length > 0) {
                    res.documents.forEach(doc => {
                        if (doc.id === data.id) {
                            data.recordStatus = 'D';
                            if (Array.isArray(existingFile) && existingFile.length === 0) {
                                docStatus = [
                                    ...existingFile, data
                                ];
                            }
                            else {
                                existingFile.forEach((file, index) => {
                                    if (file.id === data.id) {
                                        existingFile.splice(index, 1);
                                        existingFile.push(data);
                                    }
                                });
                                docStatus = existingFile;
                            }
                        }
                    });
                }
            });
        }
        else { //def 653 #14
            data.recordStatus = 'D';
            docStatus = [
                ...existingFile, data 
            ];
        }

        await dispatch(actions.RemoveDocumentDetails(docStatus));
        return true;
    }
};
// Dispatch an action to remove document unique code from uploadDocument
export const RemoveDocUniqueCode = () => async (dispatch, getstate) => {
    const state = getstate();
    let deleteUrl='';
    const documentData = state.UploadDocumentReducer.uploadDocument && state.UploadDocumentReducer.uploadDocument[0] ? state.UploadDocumentReducer.uploadDocument[0] : state.RootTechSpecReducer.TechSpecDetailReducer.documentUniqueCode;
    let epin = isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo)
    ? null : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin;
    if(!isUndefined(documentData) && !isEmpty(documentData)){
        const  docUniqueCode=documentData && documentData.documentUniqueName;  
        epin=  ( !isEmpty(epin) && !isUndefined(epin))?epin:0;
         deleteUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.techSpecDocuments + epin + GrmAPIConfig.subModuleRefCode + 0;
        if(deleteUrl!==''){
            const res = await RemoveDocumentsFromDB([ documentData ], deleteUrl);
        }
    }
};
