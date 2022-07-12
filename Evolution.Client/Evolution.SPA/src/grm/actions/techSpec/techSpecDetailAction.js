import { SuccessAlert, FailureAlert, WarningAlert, ValidationAlert, DeleteAlert, CreateAlert } from '../../../components/viewComponents/customer/alertAction';
import { ShowLoader, HideLoader, UpdateCurrentPage, RemoveDocumentsFromDB, DownloadDocument, DownloadMultiDocument } from '../../../common/commonAction';
import { FetchData, PostData, CreateData, DeleteData } from '../../../services/api/baseApiService';
import { GrmAPIConfig, RequestPayload, loginApiConfig } from '../../../apiConfig/apiConfig';
import { isEmpty, FilterSave, getlocalizeData, mergeobjects, isEmptyReturnDefault, isEmptyOrUndefine } from '../../../utils/commonUtils';
import { StringFormat } from '../../../utils/stringUtil';
import { techSpecActionTypes } from '../../constants/actionTypes';
import { commonActionTypes } from '../../../constants/actionTypes';
import moment from 'moment';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../constants/appConstants';
import { FetchState, FetchCity, FetchResourceCoordinator, FetchStateId, FetchCityId } from '../../../common/masterData/masterDataActions';
import { stat } from 'fs';
import { GetSelectedDraftProfile, FetchSavedDraftProfile } from './techSpecSearchActions';
import { ClearMyTasksData } from './techSpecMytaskActions';
import { userTypeCheck } from '../../../selectors/techSpechSelector';
import { isViewable } from '../../../common/selector';
import { levelSpecificActivities } from '../../../constants/securityConstant';
import sanitize from 'sanitize-html';

const localConstant = getlocalizeData();

const actions = {
    FetchTechSpecDetailSuccess: (payload) => ({
        type: techSpecActionTypes.saveActionTypes.FETCH_SPEC_DETAIL_SUCCESS,
        data: payload
    }),
    FetchSelectedProfileDatails: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_SELECTED_PROFILE_DETAILS,
            data: payload
        }
    ),
    UpdateInteractionMode: (payload) => ({
        type: commonActionTypes.INTERACTION_MODE,
        data: payload
    }),
    UpdateBtnEnableStatus: (payload) => ({
        type: techSpecActionTypes.techSpecSearch.UPDATE_SAVE_BTN,
        data: payload
    }),
    BtnEnable: (payload) => ({
        type: techSpecActionTypes.techSpecSearch.TS_BTN,
        data: payload
    }),
    SaveSelectedTechSpecEpin: (payload) => ({
        type: techSpecActionTypes.techSpecSearch.SAVE_SELECTED_EPIN,
        data: payload
    }),
    ClearTechSpecDetails: () => ({
        type: techSpecActionTypes.commentsActionTypes.CLEAR_TECHSPEC_DETAILS
    }),
    SetTechSpecCurrentPage: (payload) => ({
        type: techSpecActionTypes.commentsActionTypes.SET_TECHSPEC_CURRENT_PAGE,
        data: payload
    }),
    UpdateProfileAction: (payload) => ({
        type: techSpecActionTypes.commentsActionTypes.UPDATE_PROFILE_ACTION,
        data: payload
    }),
    SetProfileActionType: (payload) => ({
        type: techSpecActionTypes.professionalDetailsActionTypes.ADD_PROFILE_ACTION_TYPE,
        data: payload
    }),
    SetSelectedMyTaskDraftRefCode: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_REFCODE,
            data: payload
        }),

    GetDraftData: (payload) => ({
        type: techSpecActionTypes.GET_TECH_SPEC_DRAFT,
        data: payload
    }),
    SetSelectedMyTaskDraftType: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_TYPE,
            data: payload
        }),
    SetSelectedMyTaskDraftRefCode: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_REFCODE,
            data: payload
        }
    ),
};
/**
 * 
 * @param { Set "isSaveDraftBtnClick=true" when profile is draft profile else set as "isSaveDraftBtnClick=false" } isSaveDraftBtnClick 
 */
export const SaveTechSpecDetails = (isSaveDraftBtnClick) => async (dispatch, getstate) => {
    const state = getstate();
    let techSpecData = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails;
    //To-Do-Suresh: Need to maintain currentPage in common Reducer across all GRM module
    const currentPage = state.CommonReducer.currentPage;
    const loggedInUser = state.appLayoutReducer.loginUser;
    const taskRefCode = state.RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode;
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    //const taskRefCode = state.RootTechSpecReducer.TechSpecDetailReducer.techSpecMytaskData.forEach(x=>x.taskRefCode);
    if (!isSaveDraftBtnClick) {
        if (techSpecData.TechnicalSpecialistStamp) {
            techSpecData.TechnicalSpecialistStamp.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistContact) {
            techSpecData.TechnicalSpecialistContact.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistPaySchedule) {
            techSpecData.TechnicalSpecialistPaySchedule.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = -1;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistPayRate) {
            techSpecData.TechnicalSpecialistPayRate.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                    // row.payScheduleId=0; //D660
                }
            });
        }
        if (techSpecData.TechnicalSpecialistTaxonomy) {
            techSpecData.TechnicalSpecialistTaxonomy.forEach(row => {
                if (techSpecData.TechnicalSpecialistInfo.taxonomyStatus !== "IsAcceptRequired" && techSpecData.TechnicalSpecialistInfo.taxonomyStatus !== undefined) {
                    if (row.taxonomyStatus !== "Accept" && row.taxonomyStatus !== "Reject") {
                        row.taxonomyStatus = techSpecData.TechnicalSpecialistInfo.taxonomyStatus;
                        row.recordStatus = "M";
                    }
                } else {
                    if (techSpecData.TechnicalSpecialistInfo.profileStatus === 'Active' && (techSpecData.TechnicalSpecialistInfo.isPreviouslyProfileMadeActive === false)) {
                        row.recordStatus = "M";
                    }
                }
            });
        }
        // if (techSpecData.TechnicalSpecialistInternalTraining) {
        //     techSpecData.TechnicalSpecialistInternalTraining.forEach(row => {
        //         if (row.recordStatus === "N") {
        //             row.id = 0;
        //         }
        //     });
        // }
        // if (techSpecData.TechnicalSpecialistCompetancy) {
        //     techSpecData.TechnicalSpecialistCompetancy.forEach(row => {
        //         if (row.recordStatus === "N") {
        //             row.id = -1;
        //         }
        //     });
        // }
        // if (techSpecData.TechnicalSpecialistCompetancyType) {
        //     techSpecData.TechnicalSpecialistCompetancyType.forEach(row => {
        //         if (row.recordStatus === "N") {
        //             row.technicalSpecialistTrainingAndCompetencyId = -1;
        //         }
        //     });
        // }
        if (techSpecData.TechnicalSpecialistCustomerApproval) {
            techSpecData.TechnicalSpecialistCustomerApproval.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistWorkHistory) {
            techSpecData.TechnicalSpecialistWorkHistory.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistCodeAndStandard) {
            techSpecData.TechnicalSpecialistCodeAndStandard.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistTraining) {
            techSpecData.TechnicalSpecialistTraining.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistCertification) {
            techSpecData.TechnicalSpecialistCertification.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistCommodityAndEquipment) {
            techSpecData.TechnicalSpecialistCommodityAndEquipment.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistComputerElectronicKnowledge) {
            techSpecData.TechnicalSpecialistComputerElectronicKnowledge.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistLanguageCapabilities) {
            techSpecData.TechnicalSpecialistLanguageCapabilities.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
            });
        }
        if (techSpecData.TechnicalSpecialistDocuments) {
            techSpecData.TechnicalSpecialistDocuments.forEach(row => {
                if (row.recordStatus === "N") {
                    row.id = 0;
                }
                // row.status = row.status.trim();
            });
        }
    } else {  //Scenario fixes - 81(23-03-2020)
        if (techSpecData.TechnicalSpecialistTaxonomy) {
            const taxonomyStatus = techSpecData.TechnicalSpecialistInfo.taxonomyStatus;
            const taxonomy = techSpecData.TechnicalSpecialistTaxonomy.filter(x => x.taxonomyStatus !== "Accept" && x.taxonomyStatus !== "Reject");
            if (taxonomy.length > 0) {
                if (taxonomyStatus !== "IsAcceptRequired" && taxonomyStatus !== undefined) {
                    taxonomy.forEach(row => {
                        row.taxonomyStatus = taxonomyStatus;
                        row.recordStatus = "M";
                    });
                }
            }
        }
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistInfo.startDate)) {
        techSpecData.TechnicalSpecialistInfo.startDate = moment(techSpecData.TechnicalSpecialistInfo.startDate).format(localConstant.techSpec.common.DATE_FORMAT);
    }

    if (!isEmpty(techSpecData.TechnicalSpecialistInfo.endDate)) {
        techSpecData.TechnicalSpecialistInfo.endDate = moment(techSpecData.TechnicalSpecialistInfo.endDate).format(localConstant.techSpec.common.DATE_FORMAT);
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistInfo.dateOfBirth)) {
        techSpecData.TechnicalSpecialistInfo.dateOfBirth = moment(techSpecData.TechnicalSpecialistInfo.dateOfBirth).format(localConstant.techSpec.common.DATE_FORMAT);
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistInfo.passportExpiryDate)) {
        techSpecData.TechnicalSpecialistInfo.passportExpiryDate = moment(techSpecData.TechnicalSpecialistInfo.passportExpiryDate).format(localConstant.techSpec.common.DATE_FORMAT);
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistInfo.drivingLicenseExpiryDate)) {
        techSpecData.TechnicalSpecialistInfo.drivingLicenseExpiryDate = moment(techSpecData.TechnicalSpecialistInfo.drivingLicenseExpiryDate).format(localConstant.techSpec.common.DATE_FORMAT);
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistWorkHistory)) {
        techSpecData.TechnicalSpecialistWorkHistory.forEach((row) => {
            if (row.fromDate) {
                row.fromDate = moment(row.fromDate).format(localConstant.techSpec.common.DATE_FORMAT);
            }
            if (row.toDate) {
                row.toDate = moment(row.toDate).format(localConstant.techSpec.common.DATE_FORMAT);
            }
        });
    }
    if (!isEmpty(techSpecData.TechnicalSpecialistEducation)) {
        techSpecData.TechnicalSpecialistEducation.forEach((row) => {
            if (row.fromDate) {
                row.fromDate = moment(row.fromDate).format(localConstant.techSpec.common.DATE_FORMAT);
            }
            if (row.toDate) {
                row.toDate = moment(row.toDate).format(localConstant.techSpec.common.DATE_FORMAT);
            }
        });
    }

    techSpecData.TechnicalSpecialistInfo.modifiedBy = loggedInUser;
    techSpecData.TechnicalSpecialistInfo.isDraft = !!isSaveDraftBtnClick || techSpecData.TechnicalSpecialistInfo.isDraft;
    techSpecData.TechnicalSpecialistInfo.DraftId = taskRefCode;
    techSpecData.TechnicalSpecialistInfo.DraftType = state.RootTechSpecReducer.TechSpecDetailReducer.myTaskType;
    techSpecData.TechnicalSpecialistInfo.UserCompanyCode = selectedCompany;

    //To-Do-Suresh: Later ProfileAction need to be passed dynamically
    // techSpecData.TechnicalSpecialistInfo.profileAction = "Create/Update Profile";

    if (currentPage === localConstant.techSpec.common.Create_Profile) {
        techSpecData.TechnicalSpecialistInfo.recordStatus = 'N';
        techSpecData.TechnicalSpecialistInfo.assignedByUser = '';
        techSpecData.TechnicalSpecialistInfo.assignedToUser = '';
        techSpecData.TechnicalSpecialistInfo.pendingWithUser = '';

        const response = await dispatch(CreateTechspecDetails(techSpecData, isSaveDraftBtnClick));
        return response;
    }
    if (currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist) {
        techSpecData.TechnicalSpecialistInfo.recordStatus = "M";
        if (!isSaveDraftBtnClick) //def 790 #84 Fix
        {
            techSpecData = FilterSave(techSpecData);
        }
        const response = await dispatch(UpdateTechSpecDetails(techSpecData, isSaveDraftBtnClick));
        return response;
    }

};

export const CreateTechspecDetails = (payload, isSaveDraftBtnClick) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    let postUrl = "";
    const draftRefCode = getstate().RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode;
    if (!!isSaveDraftBtnClick) {
        if (!isEmpty(draftRefCode)) {
            postUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecilists + draftRefCode + GrmAPIConfig.draft + '?draftType=CreateProfile';
        }
        else {
            postUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecilists + '0' + GrmAPIConfig.draft + '?draftType=CreateProfile';
        }
    }
    else {
        postUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + '0/' + GrmAPIConfig.detail;
    }

    const requestPayload = new RequestPayload(payload);
    let response = "";
    if (!!isSaveDraftBtnClick) //Create
    {
        if (isEmpty(draftRefCode)) {
            response = await PostData(postUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast ProjActDataNotFound');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                });
        }
        else if (!isEmpty(draftRefCode)) //Modify
        {
            response = await CreateData(postUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast ProjActDataNotFound');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                });
        }

    } else {
        response = await PostData(postUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast ProjActDataNotFound');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
    }

    if (response) {
        if (response.code === "1" && !isEmpty(response.result)) {
            if (!!isSaveDraftBtnClick) {
                dispatch(actions.SetSelectedMyTaskDraftRefCode(response.result));
                dispatch(actions.UpdateBtnEnableStatus());
                IntertekToaster(localConstant.errorMessages.TS_DATA_SAVED_AS_DRAFT_SUCCESSFULLY, 'successToast TechnicalSpecialistsavedasdraft');
            }
            else {
                dispatch(CreateAlert(response.result, "Resource"));
                // dispatch(FetchSelectedProfileDatails(response.result.epin));
                dispatch(actions.SetTechSpecCurrentPage(localConstant.techSpec.common.Edit_View_Technical_Specialist));
                dispatch(actions.UpdateBtnEnableStatus());
                dispatch(UpdateCurrentPage(localConstant.techSpec.common.Edit_View_Technical_Specialist));
                response = await dispatch(FetchSelectedProfileDatails(response.result.epin));
            }
        }
        else if (response.code === "11" || response.code === "41") {
            if (response.validationMessages.length > 0) {
                response.validationMessages.forEach(result => {
                    if (result.messages.length > 0) {
                        result.messages.forEach(valMessage => {
                            if (valMessage.code === "20101") { //ITK QC D1281 #Test1
                                const message = StringFormat(localConstant.techSpecValidationMessage.TS_DUPLICATE_VALIDATION, result.entityValue.value);
                                IntertekToaster(message, 'dangerToast Resource');
                            } else {
                                dispatch(ValidationAlert(valMessage.message, "Resource"));
                            }
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.forEach(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "Resource"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
            }
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.UNABLE_TO_CREATE_TECHSPEC, "TechSpec"));
        }
    }
    dispatch(HideLoader());
    return response;
};

function isUserTypeCheck() {
    const userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),
        userTypeList = localConstant.userTypeList;
    return {
        RC: userTypeCheck({ array: userTypes, param: userTypeList.ResourceCoordinator }),
        RM: userTypeCheck({ array: userTypes, param: userTypeList.ResourceManager }),
        TM: userTypeCheck({ array: userTypes, param: userTypeList.TechnicalManager }),
        TS: userTypeCheck({ array: userTypes, param: userTypeList.TechnicalSpecialist }),
        MI: userTypeCheck({ array: userTypes, param: userTypeList.MICoordinator }),
        OM: userTypeCheck({ array: userTypes, param: userTypeList.OperationManager })
    };
}

/**
 * Update the exisiting TechSpec details 
 */
export const UpdateTechSpecDetails = (payload, isSaveDraftBtnClick) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    let putUrl = "";
    let draftType = '';
    const draftRefCode = getstate().RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode;
    const selectedEpin = getstate().RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin;
    const userTypeData = isUserTypeCheck();
    if (!!isSaveDraftBtnClick) {
        if (userTypeData.TS) {
            draftType = 'TS_EditProfile';
        }
        else if (userTypeData.RC || userTypeData.RM) { //D667 (Changes for 15-01-2020 Issue Doc on ALM)
            draftType = 'RCRM_EditProfile';
        }
        else if (userTypeData.TM) {
            draftType = 'TM_EditProfile';
        }

        if (draftRefCode) {
            putUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecilists + draftRefCode + GrmAPIConfig.draft + '?draftType=' + draftType;
        }
        else {
            putUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecilists + selectedEpin + GrmAPIConfig.draft + '?draftType=' + draftType;
        }
    }
    else {
        putUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + selectedEpin + '/' + GrmAPIConfig.detail;
    }

    const requestPayload = new RequestPayload(payload);
    let response = "";
    let flag = false;
    if (!!isSaveDraftBtnClick) //Create
    {
        if (draftRefCode) { //Modify
            response = await CreateData(putUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.errorMessages.TS_DATA_SAVED_AS_DRAFT_FAILED, 'dangerToast TSDrfActDataNotFound');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                });
        }
        else {
            response = await PostData(putUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.errorMessages.TS_DATA_SAVED_AS_DRAFT_FAILED, 'dangerToast TSDrfActDataNotFound');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                });
        }

    } else {
        response = await CreateData(putUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.PROJECT_UPDATE_ERROR, 'dangerToast TSDrfActDataNotFound');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
    }

    if (response) {
        if (response.code == 1) {
            if (!!isSaveDraftBtnClick) {
                // dispatch(actions.SetSelectedMyTaskDraftRefCode(response.result));
                dispatch(actions.SetSelectedMyTaskDraftType(draftType));
                if (draftRefCode) {
                    dispatch(actions.SetSelectedMyTaskDraftRefCode(draftRefCode));
                    // if (userTypeData.TS) {
                    dispatch(FetchSavedDraftProfile(draftRefCode, localConstant.techSpec.common.EDIT_VIEW_TECHSPEC, draftType));
                    // }else if (userTypeData.TM) {
                    //     dispatch(FetchSavedDraftProfile(draftRefCode,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,"TM_EditProfile"));
                    // } else {
                    // dispatch(FetchSavedDraftProfile(draftRefCode,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,"RCRM_EditProfile"));
                    // }
                }
                else {
                    dispatch(actions.SetSelectedMyTaskDraftRefCode(selectedEpin));
                    // if (userTypeData.TS) {
                    dispatch(FetchSavedDraftProfile(selectedEpin, localConstant.techSpec.common.EDIT_VIEW_TECHSPEC, draftType));
                    // }else if (userTypeData.TM) {
                    //     dispatch(FetchSavedDraftProfile(draftRefCode,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,"TM_EditProfile"));
                    // } else {
                    // dispatch(FetchSavedDraftProfile(selectedEpin,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,"RCRM_EditProfile"));
                    // }
                }
                IntertekToaster(localConstant.errorMessages.TS_DATA_SAVED_AS_DRAFT_SUCCESSFULLY, 'successToast TechnicalSpecialistsavedasdraft');
            }
            else {
                dispatch(SuccessAlert(response.result, "Resource"));
                //  dispatch(FetchSelectedProfileDatails(selectedEpin));  
                response = await dispatch(FetchSelectedProfileDatails(selectedEpin));
            }
            dispatch(actions.UpdateBtnEnableStatus());
        }
        else if (response.code == 11 || response.code == 41) {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map(result => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            if (valMessage.code === "30001") {
                                flag = true;
                                IntertekToaster(valMessage.message, 'dangerToast Resource');
                            } //D879 IGOQC
                            else {
                                dispatch(ValidationAlert(valMessage.message, "Resource"));
                            } //ITK QC D1281 #Test1 issue on ALM(15-09-2020)
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.map(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "Resource"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
            }
            if (!flag) //D879 IGOQC
                response = await dispatch(FetchSelectedProfileDatails(selectedEpin));
            else
                dispatch(actions.BtnEnable()); //D879 IGOQC
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
        }
    }
    dispatch(HideLoader());
    return response;
};
//D684
export const FetchSelectedProfileTaxnomyHistoryCount = (pinId) => async (dispatch, getstate) => {
    const fetchUrl = StringFormat(GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalspecialist + GrmAPIConfig.taxonomyHistory, pinId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(fetchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console       
        // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast DataNotFound');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        return false;
    });
    if (!isEmptyOrUndefine(response)) {
        if (response.code === "1") {
            dispatch(UpdateProfileAction({ isPreviouslyProfileMadeActive: response.result }));
            return response.result;
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SomethingWrong');
            return null;
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Resource"));
            return null;
        }
    }
};

export const FetchSelectedProfileDatails = (pinId, taskId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const userTypeData = isUserTypeCheck();
    const taskNum = taskId;
    const epin = pinId;
    const state = getstate();
    const fetchUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalspecialist + GrmAPIConfig.detail;
    const params = {
        'epin': epin
    };
    dispatch(actions.SaveSelectedTechSpecEpin(epin));
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console       
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast TechSpecDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        const userName = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
        if (response.TechnicalSpecialistInfo && !isEmpty(response.TechnicalSpecialistInfo.profileAction) && response.TechnicalSpecialistInfo.profileAction !== localConstant.techSpec.common.SEND_TO_RC_RM) {
            dispatch(actions.GetDraftData({}));
        }
        // if(!isEmpty(response.TechnicalSpecialistInfo.professionalSummary)){
        //     response.TechnicalSpecialistInfo.professionalSummary = sanitize(response.TechnicalSpecialistInfo.professionalSummary); // Security Issue "Stored Cross Site Scripting" fix (https://github.com/apostrophecms/sanitize-html)
        // }
        dispatch(actions.FetchSelectedProfileDatails(response));
        //D684
        let checkTaxonomy = response.TechnicalSpecialistTaxonomy && response.TechnicalSpecialistTaxonomy.filter(x => (x.recordStatus !== 'D')); //D1156 issue 2 
        if (checkTaxonomy && checkTaxonomy.length > 0) {
            dispatch(FetchSelectedProfileTaxnomyHistoryCount(epin)).then(res => {
                if (res == false) { //D954
                    checkTaxonomy = checkTaxonomy.filter(x => {
                        //if(x.approvalStatus === 'Approve' ){
                        x.taxonomyStatus = 'Accept';
                        //} //Commented For Sanity Def 144
                    });
                }
            });
        }
        if (response.TechnicalSpecialistInfo && response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM &&
            userTypeData.RC || userTypeData.RM) { //def 556 fix
            dispatch(GetDraftData(response.TechnicalSpecialistInfo));

        }
        else {
            dispatch(actions.GetDraftData({}));
        }
        //  if (userType.includes(localConstant.techSpec.userTypes.TS)) {
        //         const address = response && response.TechnicalSpecialistContact.filter((obj) => {
        //             if (obj.contactType === 'PrimaryAddress') {
        //                 return obj;
        //             }
        //         });
        //         //const { country, county } = this.props.contactInfo;
        //         if (!isEmpty(address)) {
        //             dispatch(FetchState(address[0].country));
        //             dispatch(FetchCity(address[0].county));
        //         }
        //  }
        if (response.TechnicalSpecialistInfo && response.TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.InProgress && userTypeData.RC) { //Changes For IGOQC D935 
            response.TechnicalSpecialistInfo.isTSApprovalRequired = true;
        }
        let interactionMode = techspecInteractionModeChange(response);
        interactionMode = techSpecInteractionModeBasedOnPendingWith(response, taskNum);
        dispatch(actions.UpdateInteractionMode(interactionMode));
        dispatch(actions.SetProfileActionType(response.TechnicalSpecialistInfo && response.TechnicalSpecialistInfo.profileAction));
        if (response.TechnicalSpecialistInfo) {
            if (userTypeData.TM && !isViewable({ activities: state.appLayoutReducer.activities, levelType: 'LEVEL_3', viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !userTypeData.RC) {
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.SEND_TO_RC_RM }));
                if (response.TechnicalSpecialistInfo.assignedToUser == null && response.TechnicalSpecialistInfo.assignedByUser == null) {
                    dispatch(FetchResourceCoordinator());
                }
            } //Added the IF Condition for D1374 
            else if ((response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE || (response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TS && response.TechnicalSpecialistInfo.profileStatus == 'Active')) &&
                (userTypeData.RC || userTypeData.RM || userTypeData.MI || userTypeData.OM)) { //Scenario Fixes
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.CREATE_UPDATE_PROFILE }));
            }
            else if (userTypeData.TM && response.TechnicalSpecialistInfo && (response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE || (response.TechnicalSpecialistInfo.profileStatus === 'Active' && response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TS && ((!isEmpty(response.TechnicalSpecialistInfo.approvalStatus) && response.TechnicalSpecialistInfo.approvalStatus !== 'R') || !userTypeData.RC))) && (response.TechnicalSpecialistInfo.createdBy !== userName || (!isEmpty(response.TechnicalSpecialistInfo.assignedToUser)) && response.TechnicalSpecialistInfo.assignedToUser === userName)) {  //SystemRole based UserType relevant Quick Fixes --//Sceario Fixes(Profile Status is Sent to TS, TM can access to update for ACTIVE Profiles) 
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.SEND_TO_RC_RM }));
                if (response.TechnicalSpecialistInfo.assignedToUser == null && response.TechnicalSpecialistInfo.assignedByUser == null) {
                    dispatch(FetchResourceCoordinator());
                }
            }
            else if (userTypeData.TM && response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && response.TechnicalSpecialistInfo && (response.TechnicalSpecialistInfo.createdBy === userName)) { //SystemRole based UserType relevant Quick Fixes 
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.CREATE_UPDATE_PROFILE }));
            }
            else if (userTypeData.TS && (response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TS || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE)) { //SystemRole based UserType relevant Quick Fixes 
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.SEND_TO_RC_RM }));
                if (response.TechnicalSpecialistInfo.assignedToUser == null && response.TechnicalSpecialistInfo.assignedByUser == null) {
                    dispatch(FetchResourceCoordinator());
                }
            }
        }
        dispatch(CleareMytaskRefCode()); //Scenario Fix (Ref 08-04-200 Doc issue11)
        dispatch(HideLoader());
        return response;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast TecSpecDataSomethingWrong');
    }
};
/**
 * Cancel EditTechSpecDetails
 */
export const CancelEditTechSpecDetails = () => async (dispatch, getstate) => {
    const state = getstate();
    const documentData = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments;
    documentData && documentData.forEach(itratedValue => {
        if (itratedValue.id === 0 && itratedValue.recordStatus === 'D') {
            itratedValue.recordStatus = null;
        }
        else if (itratedValue.id > 0 && itratedValue.recordStatus === 'M' && itratedValue.newlyUploadedDoc) {  //D223 changes 
            itratedValue.recordStatus = 'D';
        }
    });
    const epin = state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo;
    const deleteUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.techSpecDocuments + epin + GrmAPIConfig.subModuleRefCode + 0;
    if (!isEmpty(documentData)) {
        await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    const response = await dispatch(FetchSelectedProfileDatails(epin));
    const address = response && response.TechnicalSpecialistContact.filter((obj) => {
        if (obj.contactType === 'PrimaryAddress') {
            return obj;
        }
    });
    if (!isEmpty(address)) {
        if (address[0].countryId) { dispatch(FetchStateId(address[0].countryId)); }
        if (address[0].countyId) { dispatch(FetchCityId(address[0].countyId)); }

    }
    return response;
};
export const CancelTechSpecDraftChanges = () => async (dispatch, getstate) => {
    const taskRef = getstate().RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode;
    dispatch(actions.ClearTechSpecDetails());
    const userTypeData = isUserTypeCheck();
    let response = ""; //Sanity Def 96
    if (userTypeData.TS) {
        response = await dispatch(FetchSavedDraftProfile(taskRef, localConstant.techSpec.common.EDIT_VIEW_TECHSPEC, "TS_EditProfile"));
    } else { response = await dispatch(FetchSavedDraftProfile(taskRef, localConstant.techSpec.common.EDIT_VIEW_TECHSPEC, "RCRM_EditProfile")); }
    //dispatch(GetSelectedDraftProfile(taskRef));
    return response;
};
export const CancelCreateTechSpecDetails = () => (dispatch, getstate) => {
    const state = getstate();
    const obj = {};
    dispatch(actions.ClearTechSpecDetails());
    //Scenario Failed(#91 ref mail ALM Defects - Yet to fix on 18-03-2020)
    const selectedCompany = state.appLayoutReducer.userRoleCompanyList && state.appLayoutReducer.userRoleCompanyList.filter(item => item.companyCode === state.appLayoutReducer.selectedCompany);
    if (!isEmpty(selectedCompany)) {
        obj['companyCode'] = selectedCompany[0].companyCode;
        obj['companyName'] = selectedCompany[0].companyName;
    }
    obj['isEnableLogin'] = true;
    dispatch(actions.UpdateProfileAction(obj));
};

/** Profile Action update */
export const UpdateProfileAction = (data) => (dispatch, getstate) => {
    const state = getstate();
    const techSpecInfo = Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo);
    const updatedTechSpecInfo = mergeobjects(techSpecInfo, data);
    dispatch(actions.UpdateProfileAction(updatedTechSpecInfo));
};

/** Update Tech spec info */
export const UpdateTechSpecInfo = (data) => (dispatch, getstate) => {
    const state = getstate();
    const techSpecInfo = Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo);
    const updatedTechSpecInfo = mergeobjects(techSpecInfo, data);
    dispatch(actions.UpdateProfileAction(updatedTechSpecInfo));
};

const techSpecInteractionModeBasedOnPendingWith = (data, taskId) => {
    const username = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
    if (data && data.TechnicalSpecialistInfo) {
        const { TechnicalSpecialistInfo } = data;
        if (TechnicalSpecialistInfo.pendingWithUserLogOnName !== null && isEmptyOrUndefine(taskId)) {
            return true;
        }
    }
};

const techspecInteractionModeChange = (data) => {
    const username = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
    if (data && data.TechnicalSpecialistInfo) {
        const { TechnicalSpecialistInfo } = data;
        const sendToTypes = localConstant.techSpec.common;
        const userTypeData = isUserTypeCheck();
        if ((userTypeData.RC || userTypeData.RM || userTypeData.MI || userTypeData.OM) && (!userTypeData.TM)) { //Scenario Fixes
            if ((isEmpty(TechnicalSpecialistInfo.profileAction) || TechnicalSpecialistInfo.profileAction === sendToTypes.CREATE_UPDATE_PROFILE || TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_RC_RM) && !TechnicalSpecialistInfo.isDraft) {
                return false;
            }
            else if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS && TechnicalSpecialistInfo.profileStatus === 'Active') {
                return false;
            }
            else if ((TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS || (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TM && (userTypeData.TM && TechnicalSpecialistInfo.assignedToUser === username)))
                && !TechnicalSpecialistInfo.isDraft) {
                return true;
            }
            else {
                return true;
            }
        }
        if (userTypeData.TS) {
            if ((isEmpty(TechnicalSpecialistInfo.approvalStatus) || TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Approved || TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected) && TechnicalSpecialistInfo.profileStatus === 'Active' && (TechnicalSpecialistInfo.profileAction !== sendToTypes.SEND_TO_TM) && !TechnicalSpecialistInfo.isDraft) {
                return false;
            }
            else if ((!isEmpty(TechnicalSpecialistInfo.approvalStatus) && (TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.InProgress)) && TechnicalSpecialistInfo.profileStatus === 'Active' && TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_RC_RM && !TechnicalSpecialistInfo.isDraft) {
                return true;
            }
            else if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS && !TechnicalSpecialistInfo.isDraft) {
                return false;
            }
            else if ((TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TM || TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_RC_RM || TechnicalSpecialistInfo.profileStatus !== 'Active') && !TechnicalSpecialistInfo.isDraft) { // Changes for D946 CR (Ref mail D946 #4 point)
                return true;
            }
        }
        if (userTypeData.TM) {
            if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TM) {
                if (TechnicalSpecialistInfo.profileStatus === 'Active' && TechnicalSpecialistInfo.assignedToUser === username) {
                    return false;
                } else if (!isEmpty(TechnicalSpecialistInfo.createdBy) && TechnicalSpecialistInfo.createdBy === username) {// Changes for D496 CR (Ref mail D946 #4 point)
                    return true;
                }
                ///  return true;  D406
            }
            else if (TechnicalSpecialistInfo.profileStatus === 'Active' && (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS || TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_RC_RM || TechnicalSpecialistInfo.profileAction === sendToTypes.CREATE_UPDATE_PROFILE)) {
                return false;
            }
            else if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS && !TechnicalSpecialistInfo.isDraft) { //SystemRole based UserType relevant Quick Fixes 
                return true;
            }
        }

        ///  return true; D406
    }
};

/** Get method for Draft */
export const GetDraftData = (data) => async (dispatch, getstate) => {
    if (data) {
        const draftUrl = StringFormat(GrmAPIConfig.drafts, "TS", "ProfileChangeHistory");
        const params = {
            'draftId': data.epin
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(draftUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //To-Do-Suresh: Remove hardcoded string 
                // IntertekToaster(localConstant.errorMessages.DRAFT_DATA_NOT_FETCHED, 'dangerToast TechSpecDraftDataNotFound');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

        if (response) {
            if (response.code == 1) {
                if (response.result && response.result.length > 0) {
                    response.result[response.result.length - 1].serilizableObject = JSON.parse(response.result[response.result.length - 1].serilizableObject);
                    // response.result[0].serilizableObject.technicalSpecialistInfo.salutation = "Mr";
                    // response.result[0].serilizableObject.technicalSpecialistInfo.firstName = "TestName";
                    dispatch(actions.GetDraftData(response.result[response.result.length - 1].serilizableObject));
                }
                else {
                    dispatch(actions.GetDraftData({}));
                }
            }
            // TO-DO: Do validation message show in toaster based on response.code.
            else if (response.code == 11 || response.code == 41) {

            }
            else {

            }
        }
    }
};
/** Get My Profile Details */
export const FetchMyProfileDatails = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const userTypeData = isUserTypeCheck();
    const fetchUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalspecialist + GrmAPIConfig.detail;
    const params = {
        'epin': data.epin
    };

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast TechSpecDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response) {
        const userType = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        const userName = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
        const taxonomyIsAcceptRequired = response.TechnicalSpecialistTaxonomy && response.TechnicalSpecialistTaxonomy.filter(x => (x.recordStatus !== 'D' && x.taxonomyStatus === 'IsAcceptRequired')).length > 0;
        response.TechnicalSpecialistInfo.taxonomyIsAcceptRequired = taxonomyIsAcceptRequired;
        // if(!isEmpty(response.TechnicalSpecialistInfo.professionalSummary)){
        //     response.TechnicalSpecialistInfo.professionalSummary = sanitize(response.TechnicalSpecialistInfo.professionalSummary); // Security Issue "Stored Cross Site Scripting" fix (https://github.com/apostrophecms/sanitize-html)
        // }
        dispatch(actions.FetchSelectedProfileDatails(response));
        dispatch(actions.SaveSelectedTechSpecEpin(`${ response.TechnicalSpecialistInfo.epin }`)); //Changes for D918 (ref by 26-02-2020 ALM Doc) 
        if (userTypeData.TS) {
            const address = response && response.TechnicalSpecialistContact.filter((obj) => {
                if (obj.contactType === 'PrimaryAddress') {
                    return obj;
                }
            });
            //const { country, county } = this.props.contactInfo;
            if (!isEmpty(address)) {
                dispatch(FetchStateId(address[0].countryId));
                dispatch(FetchCityId(address[0].countyId));
            }
        }
        const interactionMode = techspecInteractionModeChange(response);
        dispatch(actions.UpdateInteractionMode(interactionMode));
        dispatch(actions.SetProfileActionType(response.TechnicalSpecialistInfo && response.TechnicalSpecialistInfo.profileAction));
        if (response.TechnicalSpecialistInfo) {
            if ((response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) &&
                (userTypeData.RC || userTypeData.RM)) {
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.CREATE_UPDATE_PROFILE }));
            }
            else if (userTypeData.TM && response.TechnicalSpecialistInfo && (response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) && response.TechnicalSpecialistInfo.assignedToUser === userName) {
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.SEND_TO_RC_RM }));
            }
            else if (userTypeData.TS && response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TS || response.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) {
                dispatch(UpdateProfileAction({ profileAction: localConstant.techSpec.common.SEND_TO_RC_RM }));
                if (response.TechnicalSpecialistInfo.assignedToUser == null && response.TechnicalSpecialistInfo.assignedByUser == null) { //Scenario Fix for D946
                    dispatch(FetchResourceCoordinator());
                }
            }
        }
        dispatch(UpdateCurrentPage(localConstant.techSpec.common.Edit_View_Technical_Specialist));
        dispatch(HideLoader());
        return response;
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast TecSpecDataSomethingWrong');
    }
};

export const ExportToCV = (techspecialistId, isChevron, section) => async (dispatch, getstate) => {
    let url = "";
    if (section && section.length > 0) {
        const sectionList = section.map(x => x.entity);
        url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.DocumentApi + GrmAPIConfig.exportCV + "?techspecialistEpin=" + techspecialistId + '&isChevron=' + isChevron
            + '&WorkhistoryCurrentCompany=' + sectionList.includes("WorkhistoryCurrentCompany")
            + '&WorkhistoryOtherCompany=' + sectionList.includes("WorkhistoryOtherCompany")
            + '&CertificateNotExpired=' + sectionList.includes("CertificateNotExpired")
            + '&CertificateExpired=' + sectionList.includes("CertificateExpired")
            + '&RemoveDuplicates=' + sectionList.includes("RemoveDuplicates")
            + '&CertificatePending=' + sectionList.includes("CertificatePending")
            + '&Certificateverified=' + sectionList.includes("Certificateverified")
            + '&CertificateUnverified=' + sectionList.includes("CertificateUnverified")
            + '&TrainingDetailsPending=' + sectionList.includes("TrainingDetailsPending")
            + '&TrainingDetailsUnverified=' + sectionList.includes("TrainingDetailsUnverified")
            + '&TrainingDetailsVerified=' + sectionList.includes("TrainingDetailsVerified")
            + '&LanguageCapabilities=' + sectionList.includes("LanguageCapabilities")
            + '&EducationSummary=' + sectionList.includes("EducationSummary")
            + '&CommodityEquipments=' + sectionList.includes("CommodityEquipments")
            + '&IsSection=' + true;
    }
    else {
        url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.DocumentApi + GrmAPIConfig.exportCV + "?techspecialistEpin=" + techspecialistId + '&isChevron=' + isChevron + '&IsSection=' + false;
    }
    dispatch(DownloadDocument(url));
    // window.location.href = url;
};

export const ExportToMultiCV = (data, isChevron, exportCVFrom, customerCode, projectNumber, assignmentNumber, section) => async (dispatch, getstate) => {
    const epins = [];
    let url = "";
    data.map(selectedRecord => {
        epins.push(selectedRecord.epin);
    });
    if (section && section.length > 0) {
        const sectionList = section.map(x => x.entity);
        url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.DocumentApi + GrmAPIConfig.exportCV + "?isChevron=" + isChevron + "&exportCVFrom=" + exportCVFrom + "&customerCode=" + customerCode + "&projectNumber=" + projectNumber + "&assignmentNumber=" + assignmentNumber
            + '&WorkhistoryCurrentCompany=' + sectionList.includes("WorkhistoryCurrentCompany")
            + '&WorkhistoryOtherCompany=' + sectionList.includes("WorkhistoryOtherCompany")
            + '&CertificateNotExpired=' + sectionList.includes("CertificateNotExpired")
            + '&CertificateExpired=' + sectionList.includes("CertificateExpired")
            + '&RemoveDuplicates=' + sectionList.includes("RemoveDuplicates")
            + '&CertificatePending=' + sectionList.includes("CertificatePending")
            + '&Certificateverified=' + sectionList.includes("Certificateverified")
            + '&CertificateUnverified=' + sectionList.includes("CertificateUnverified")
            + '&TrainingDetailsPending=' + sectionList.includes("TrainingDetailsPending")
            + '&TrainingDetailsUnverified=' + sectionList.includes("TrainingDetailsUnverified")
            + '&TrainingDetailsVerified=' + sectionList.includes("TrainingDetailsVerified")
            + '&LanguageCapabilities=' + sectionList.includes("LanguageCapabilities")
            + '&EducationSummary=' + sectionList.includes("EducationSummary")
            + '&CommodityEquipments=' + sectionList.includes("CommodityEquipments")
            + '&IsSection=' + true;
    }
    else {
        url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.DocumentApi + GrmAPIConfig.exportCV + "?isChevron=" + isChevron + "&exportCVFrom=" + exportCVFrom + "&customerCode=" + customerCode + "&projectNumber=" + projectNumber + "&assignmentNumber=" + assignmentNumber + '&IsSection=' + false;
    }
    dispatch(DownloadMultiDocument(url, epins));
};

export const DeleteTechSpecDraft = () => async (dispatch, getstate) => {
    const draftRefCode = getstate().RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode;
    const draftType = getstate().RootTechSpecReducer.TechSpecDetailReducer.myTaskType;
    const deleteUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecilists + draftRefCode + GrmAPIConfig.draft + '?draftType=' + draftType;
    const requestPayload = new RequestPayload({});
    if (draftRefCode) {
        dispatch(ShowLoader());
        const response = await DeleteData(deleteUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.PROJECT_POST_ERROR, 'dangerToast techSpecDraftDelete');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (response) {
            if (response.code == 1) {
                dispatch(HideLoader());
                dispatch(ClearMyTasksData());
                return response;
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast techSpecDraftDelete');
            }
            else {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast techSpecDraftDelete');
            }
        }
        else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Assignment');
        }
        dispatch(HideLoader());
    }
};

export const CleareMytaskRefCode = () => (dispatch) => {
    dispatch(actions.SetSelectedMyTaskDraftRefCode(''));
};

export const FetchUserTypeForInterCompanyResource = (userName, compCode) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const url = loginApiConfig.userType.replace('{userSamaName}', userName) + '?compCode=' + compCode;
    const requestPayload = new RequestPayload();
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast');
            dispatch(HideLoader());
            return false;
        });
    if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.code) && response.code == "1") {
        if (!isEmptyOrUndefine(response.result)) {
            const delimitedUserTypes = response.result.filter(s => s.userType !== ' ').map(e => e.userType).join(",");
            sessionStorage.setItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE, delimitedUserTypes);
            dispatch(HideLoader());
            return true;
        }
        dispatch(HideLoader());
        return false;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast');
        dispatch(HideLoader());
        return false;
    }
};