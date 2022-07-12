import { commonActionTypes } from '../constants/actionTypes';
import { commonAPIConfig, RequestPayload } from '../apiConfig/apiConfig';
import IntertekToaster from './baseComponents/intertekToaster';
import { FetchData, PostData, CreateData, DeleteData, FetchDocDownload, MultiDocumentDownload, FetchDocDownloadTS } from '../services/api/baseApiService';
import { getlocalizeData, isEmpty, parseValdiationMessage, isEmptyReturnDefault, isEmptyOrUndefine } from '../utils/commonUtils';
import { moduleViewAllRights } from '../utils/permissionUtil';
import { securitymodule, activitycode } from '../constants/securityConstant';
import { configuration } from '../appConfig';
import sanitize from 'sanitize-html';
const localConstant = getlocalizeData();
const actions = {
    ShowLoader: (payload) => ({
        type: commonActionTypes.SHOW_LOADER,
        data: payload
    }),
    HideLoader: (payload) => ({
        type: commonActionTypes.HIDE_LOADER,
        data: payload
    }),
    UpdateCurrentPage: (payload) => ({
        type: commonActionTypes.UPDATE_CURRENT_PAGE,
        data: payload
    }),
    UpdateCurrentModule: (payload) => ({
        type: commonActionTypes.UPDATE_CURRENT_MODULE,
        data: payload
    }),
    UpdateInteractionMode: (payload) => ({
        type: commonActionTypes.INTERACTION_MODE,
        data: payload
    }),
    SetCurrentPageMode: (payload) => ({
        type: commonActionTypes.GET_CURRENT_MODE,
        data: payload
    }),
    FetchEmailTemplate: (payload) => ({
        type: commonActionTypes.FETCH_EMAIL_TEMPLATE,
        data: payload
    }),
    GrmMasterDataLoaded: (payload) => ({
        type: commonActionTypes.IS_GRM_MASTER_DATA_LOADED,
        data: payload
    }),
    IsARSMasterDataLoaded: (payload) => ({
        type: commonActionTypes.IS_ARS_MASTER_DATA_LOADED,
        data: payload
    })
};

export const ShowLoader = (data) => (dispatch, getstate) => {
    dispatch(actions.ShowLoader(true));
};

export const HideLoader = (data) => (dispatch, getstate) => {
    dispatch(actions.HideLoader(false));
};
export const FetchDocumentUniqueName = (data, fileList, isSyncProcess) => async (dispatch, getstate) => {
    if (isSyncProcess) { //Sync upload need for Resource Module
        dispatch(actions.ShowLoader(true));
    }
    const failureFiles = [];
    const failureFormat = [];
    //Validation added to handle unsupported file type and size limit in upload
    fileList.map(document => {
        const filterType = configuration.allowedFileFormats.indexOf(document.name.substring(document.name.lastIndexOf(".")).toLowerCase());
        if (filterType >= 0 && document.name.lastIndexOf(".") !== -1) { // sanity 204 fix
            if (parseInt(document.size / 1024) > configuration.fileLimit) {
                failureFiles.push(document.name);
            }
        } else {
            failureFormat.push(document.name);
        }
        return document;
    });
    if (failureFormat.length === 0 && failureFiles.length == 0) {
        const documents = [];
        const documentUniqueNameURL = commonAPIConfig.baseUrl + commonAPIConfig.documents + commonAPIConfig.uniqueName;
        const requestPayload = new RequestPayload(data);
        const response = await PostData(documentUniqueNameURL, requestPayload)
            .catch(error => {
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                if (response.result) {
                    for (let i = 0; i < response.result.length; i++) {
                        const formData = new FormData();
                        const fileContent = fileList.filter(x => x.name == response.result[i].documentName);
                        formData.append("file", fileContent[0]);
                        const docUniqueName = await UploadDocumentData(response.result[i], formData);
                        if (docUniqueName && docUniqueName.code == 1) {
                            documents.push(docUniqueName.result);
                        }
                        else {
                            //alert("Error in the document upload");
                            const failedDocData = {
                                moduleCode: response.result[i].moduleCode,
                                documentUniqueName: response.result[i].uniqueName,
                                moduleRefCode: response.result[i].moduleCodeReference,
                                status: "F"
                            };
                            const failedUploadDocument = await FailedUploadDocumentData(failedDocData);
                            if (failedUploadDocument && failedUploadDocument.code === "1") {
                                documents.push(failedUploadDocument.result);
                            }
                        }
                    };
                    if (isSyncProcess) { //Sync upload need for Resource Module
                        dispatch(actions.HideLoader(false));
                    }
                    return documents;
                }
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage.message, 'warningToast');
                            });
                        }
                    });
                }
                else if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
                dispatch(actions.HideLoader(false));
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.UNABLE_TO_FETCH_DOCUMENT_UNIQUE_DATA, 'dangerToast DocActFetDocUniName');
            dispatch(actions.HideLoader(false));
        }
    }
    else if (failureFormat.length > 0) {
        IntertekToaster(failureFiles.toString() + localConstant.companyDetails.Documents.FILE_FORMAT_WRONG, 'warningToast contractDocSizeReq');
        dispatch(actions.HideLoader(false));
    }
    else if (failureFiles.length > 0) {
        IntertekToaster(failureFiles.toString() + localConstant.assignments.FILE_LIMIT_EXCEDED, 'warningToast assignmentDocSizeReq');
        dispatch(actions.HideLoader(false));
    }

};
export const UploadDocumentData = async (data, file) => {
    let documentUploadIdUrl;

    if (!isEmptyOrUndefine(data.moduleCodeReference))
        documentUploadIdUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data.moduleCode + commonAPIConfig.uploadFileAsStream + data.uniqueName + "&referenceCode=" + data.moduleCodeReference;
    else
        documentUploadIdUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data.moduleCode + commonAPIConfig.uploadFileAsStream + data.uniqueName;
    //const documentUploadIdUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data.moduleCode + commonAPIConfig.uploadFileAsStream + data.uniqueName + "&referenceCode=" + data.moduleCodeReference;
    const requestPayload = new RequestPayload(file, { 'Content-Type': 'multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW' });
    const response = await PostData(documentUploadIdUrl, requestPayload)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        return response;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast DocActUploadDoc');
    }
};
export const FailedUploadDocumentData = async (data) => {
    const failedDocumentUploadUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + commonAPIConfig.changeStatus;
    const requestPayload = new RequestPayload(data);
    const response = await CreateData(failedDocumentUploadUrl, requestPayload)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        return response;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast DocActFailUploadDoc');
    }
};
export const DownloadDocumentData = (data) => async (dispatch, getstate) => {
    const downloadDocumentDataUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data.moduleCode + commonAPIConfig.download + data.documentUniqueName;
    //window.location.href=downloadDocumentDataUrl;
    dispatch(DownloadDocument(downloadDocumentDataUrl));
};

export const MultiDocDownload = (data) => async (dispatch, getstate) => {
    data.map(selectedRecord => {
        const downloadDocumentDataUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + selectedRecord.moduleCode + commonAPIConfig.download + selectedRecord.documentUniqueName;
        dispatch(DownloadDocument(downloadDocumentDataUrl));
    });
};

export const DownloadDocument = (data) => async (dispatch, getstate) => {
    const response = await FetchDocDownload(data)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        const type = response.headers['content-type'];
        const headerval = response.headers['content-disposition'];
        let filename = headerval.split(';')[1].split('=')[1].replace('"', '').replace('"', '');
        filename = decodeURIComponent(filename);
        const ua = window.navigator.userAgent;
        const msie = ua.indexOf(".NET ");
        if (msie > 0) {
            window.navigator.msSaveBlob(new Blob([ response.data ]), filename);
        }
        else {
            const blob = new Blob([ response.data ], { type: type, encoding: 'UTF-8' });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.remove();
        }
    }
    else{
        console.log("Download Error");
    }
};

export const PasteDocumentUploadData = (data) => async (dispatch, getstate) => {
    dispatch(actions.ShowLoader(true));
    const recordsToBePasted = [];
    if (Array.isArray(data) && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            let pasteDocumentUploadIdUrl = "";
            if (data[i].moduleRefCode)
                pasteDocumentUploadIdUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data[i].moduleCode + commonAPIConfig.paste + "?referenceCode=" + data[i].moduleRefCode + "&copyDocumentUniqueName=" + data[i].documentUniqueName;
            else
                pasteDocumentUploadIdUrl = commonAPIConfig.baseUrl + commonAPIConfig.documents + '/' + data[i].moduleCode + commonAPIConfig.paste + '?referenceCode= &copyDocumentUniqueName=' + data[i].documentUniqueName; // dont remove whitespace in moduleRefCode
            const requestPayload = new RequestPayload();
            const response = await PostData(pasteDocumentUploadIdUrl, requestPayload)
                .catch(error => {
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                });
            if (!isEmpty(response)) {
                if (response.code === "1") {
                    recordsToBePasted.push(response.result);
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
                                IntertekToaster(localConstant.errorMessages.PASTE_DOCUMENT_WENT_WRONG, 'dangerToast projectMastedrDocumentsSWWVal');
                            }
                        });
                    }
                    else if (response.messages.length > 0) {
                        response.messages.map(result => {
                            if (result.message.length > 0) {
                                IntertekToaster(result.message, 'warningToast');
                            }
                        });
                    }
                }
                else {
                    IntertekToaster(localConstant.errorMessages.PASTE_DOCUMENT_WENT_WRONG, 'dangerToast projectMastedrDocumentsSWWVal');
                }
            }
        }
    }
    dispatch(actions.HideLoader(false));
    return recordsToBePasted;
};
export const RemoveDocumentsFromDB = async (documentData, deleteUrl) => {
    const docToBeDeleted = [];
    documentData.forEach(row => {
        //Def 512 -- pls dont change order  
        //    if (row.recordStatus === "D") {  Commented due to D1186
        //     docToBeDeleted.push(row);
        //     }

        //Changes for D-247 -- documents getting deleted after modifying.
        if (row.recordStatus === "N" || row.newlyAddedRecord || row.newlyUploadedDoc) { //D223,D512
            row.recordStatus = "D";
            docToBeDeleted.push(row);
        }

    });
    if (!isEmpty(docToBeDeleted)) {
        const params = {
            data: docToBeDeleted,
        };
        const requestPayload = new RequestPayload(params);
        const response = await DeleteData(deleteUrl, requestPayload)
            .catch(error => {
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code == 1) {
            return response;
        }
        return response;
    }
};

export const UpdateCurrentPage = (data) => (dispatch) => {
    dispatch(actions.UpdateCurrentPage(data));
};

export const UpdateCurrentModule = (data) => (dispatch) => {
    dispatch(actions.UpdateCurrentModule(data));
};

export const SetCurrentPageMode = (data, isViewAllRights) => (dispatch, getstate) => {
    // export const  SetCurrentPageMode=(data, module, isViewAllRights)=>(dispatch,getstate)=>{
    const state = getstate();
    const activities = state.appLayoutReducer.activities;
    if (activities !== undefined && activities.length > 0) {
        const viewRes = activities.filter(x => x.activity === activitycode.VIEW || x.activity === activitycode.LEVEL_0_VIEW || x.activity === activitycode.LEVEL_1_VIEW || x.activity === activitycode.LEVEL_2_VIEW || x.activity === activitycode.VIEW_SENSITIVE_DOC || x.activity === activitycode.VIEW_PAYRATE || x.activity === activitycode.VIEW_TM || x.activity === activitycode.INTER_COMP_LEVEL_0_VIEW);//def 957
        const editRes = activities.filter(x => x.activity === activitycode.MODIFY || x.activity === activitycode.LEVEL_0_MODIFY || x.activity === activitycode.LEVEL_1_MODIFY || x.activity === activitycode.LEVEL_2_MODIFY || x.activity === activitycode.EDIT_SENSITIVE_DOC || x.activity === activitycode.EDIT_PAYRATE || x.activity === activitycode.EDIT_TM);//def 957
        if (data !== null && viewRes.length > 0 && editRes.length === 0) {
            dispatch(actions.SetCurrentPageMode(localConstant.commonConstants.VIEW));
        }
        else if (data === localConstant.commonConstants.VIEW) {
            dispatch(actions.SetCurrentPageMode(data));
        }
        else {
            if (isEmpty(editRes) && isViewAllRights)
                // if(isEmpty(editRes) && (moduleViewAllRights(module, isViewAllRights)))
                dispatch(actions.SetCurrentPageMode(localConstant.commonConstants.VIEW));
            else
                dispatch(actions.SetCurrentPageMode(null));
        }
    }
};

export const UpdateInteractionMode = (data, isViewAllRights, isMandatoryView) => async (dispatch, getstate) => {
    // export const UpdateInteractionMode = (data,module,isViewAllRights,isMandatoryView) => async (dispatch, getstate) => {
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const isArray = Array.isArray(data);
    const condition = isArray ? !data.includes(selectedCompany) : data !== selectedCompany;
    if (data) {
        if (isMandatoryView) {
            dispatch(actions.UpdateInteractionMode(true));
            dispatch(actions.SetCurrentPageMode(localConstant.commonConstants.VIEW));
        }
        else if (condition && !isViewAllRights) {
            dispatch(actions.UpdateInteractionMode(true));

            dispatch(SetCurrentPageMode("View", isViewAllRights));
            // dispatch(SetCurrentPageMode("View",module,isViewAllRights));
        }
        else {
            dispatch(actions.UpdateInteractionMode(false));

            dispatch(SetCurrentPageMode("", isViewAllRights));
            // dispatch(SetCurrentPageMode("",module,isViewAllRights));
        }
    }
};

export const DownloadMultiDocument = (url, data) => async (dispatch, getstate) => {
    dispatch(actions.ShowLoader(true));
    const requestPayload = new RequestPayload(data);
    const response = await MultiDocumentDownload(url, requestPayload)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    dispatch(actions.ShowLoader(false));
    //For  CR6 Changes   -----

    // if (response) {
    //         const type= response.headers['content-type'];
    //         const headerval = response.headers['content-disposition'];
    // 		let filename = headerval.split(';')[1].split('=')[1].replace('"', '').replace('"', '');
    //         filename = decodeURIComponent(filename);
    //         const ua = window.navigator.userAgent;
    //         const msie = ua.indexOf(".NET ");
    //         if(msie>0) {
    //             window.navigator.msSaveBlob(new Blob([ response.data ]), filename);
    //         }
    //         else{
    //         const blob = new Blob([ response.data ],{ type: type, encoding: 'UTF-8' });
    //         const link = document.createElement('a');
    //         link.href = window.URL.createObjectURL(blob);
    //         link.setAttribute('download', filename);
    //         document.body.appendChild(link);
    //         link.click();
    //         link.remove();
    //         }
    //         dispatch(actions.ShowLoader(false));
    // }
};

export const FetchEmailTemplate = (data) => async (dispatch, getstate) => {
    dispatch(actions.FetchEmailTemplate(null));
    const apiUrl = commonAPIConfig.emailTemplate;
    const requestPayload = new RequestPayload(data);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            return false;
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchEmailTemplate(sanitize(response.result)));
        return response.result;
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchEmailTemplate');
        return false;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
        return false;
    }
};

export const IsGRMMasterDataFetched = (data) => (dispatch) => {
    dispatch(actions.GrmMasterDataLoaded(data));
};

export const IsARSMasterDataLoaded = (data) => (dispatch) => {
    dispatch(actions.IsARSMasterDataLoaded(data));
};