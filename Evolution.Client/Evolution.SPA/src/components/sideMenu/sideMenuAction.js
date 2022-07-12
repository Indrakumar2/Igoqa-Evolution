import { sideMenu,commonActionTypes,techSpecActionTypes } from '../../constants/actionTypes';
import { UpdateCurrentPage, 
        UpdateCurrentModule,
        ShowLoader, 
        HideLoader,
        SetCurrentPageMode,
        IsGRMMasterDataFetched } from '../../common/commonAction';
import { FetchMyProfileDatails, CleareMytaskRefCode, FetchSelectedProfileTaxnomyHistoryCount } from '../../grm/actions/techSpec/techSpecDetailAction';
import { FetchTechSpecData, FetchSavedDraftProfile } from '../../grm/actions/techSpec/techSpecSearchActions';
import { applicationConstants } from '../../constants/appConstants';
import { getlocalizeData,isEmptyReturnDefault,isEmpty , isFunction } from '../../utils/commonUtils';

const localConstant = getlocalizeData();

const actions = {
    EditContract: () => ({
        type: sideMenu.EDIT_CONTRACT

    }),
    ViewContract: () => ({
        type: sideMenu.VIEW_CONTRACT
    }),
    CreateContract: (payload) => ({
        type: sideMenu.CREATE_CONTRACT,
        data: payload
    }),    
    EditProject: () => ({
        type: sideMenu.EDIT_PROJECT

    }),
    ViewProject: () => ({
        type: sideMenu.VIEW_PROJECT
    }),
    CreateProject: (payload) => ({
        type: sideMenu.CREATE_PROJECT,
        data: payload
    }),
    EditSupplier: () => ({
        type: sideMenu.EDIT_SUPPLIER
    }),
    CreateSupplier:(payload) =>({
        type: sideMenu.CREATE_SUPPLIER,
        data: payload
    }),    
    CreateProfile: () => ({
        type: sideMenu.CREATE_PROFILE,
       
    }),
    EditViewTechnicalSpecialist: () => ({
        type: sideMenu.EDIT_VIEW_TECHNICAL_SPECIALIST,
       
    }),
    HandleMenuAction: (payload) => ({
        type: sideMenu.HANDLE_MENU_ACTION,
        data: payload
    }),
    CheckPreassignmentWon: (payload) => ({
        type: techSpecActionTypes.CHECK_PREASSIGNMENT_WON,
        data: payload
    }),

    UpdateInteractionMode: (payload) => ({
        type: commonActionTypes.INTERACTION_MODE,
        data: payload
    }),
    viewRole: (payload) => ({
        type: sideMenu.EDIT_VIEW_TECHNICAL_SPECIALIST,
        data: payload
    }),
};
export const EditContract = (data) => (dispatch) => {
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule));
    dispatch(actions.EditContract());
    dispatch(IsGRMMasterDataFetched(false));
};
export const ViewContract = () => (dispatch) => {
    dispatch(actions.ViewContract());
};
export const CreateContract = (data) => (dispatch) => {
    const uuidv4 = require('uuid/v4');
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule));
    dispatch(actions.CreateContract({
        "contractNumber": uuidv4(),
        "contractType": "",
        "contractBudgetHours": '00.00',
        // "contractBudgetMonetaryValue": '00.00',
        "contractBudgetMonetaryWarning":75,
        "contractBudgetHoursWarning":75,
        'contractStatus': 'O'
    }));
    dispatch(SetCurrentPageMode(null));
    dispatch(IsGRMMasterDataFetched(false));
};

export const EditProject = (data) => (dispatch) => {
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule));   
    dispatch(actions.EditProject());
    dispatch(IsGRMMasterDataFetched(false));
    dispatch(SetCurrentPageMode(null));
};
 
export const CreateProject = (data) => (dispatch) => {
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule));     
     dispatch(actions.CreateProject());
     dispatch(SetCurrentPageMode(null));
     dispatch(IsGRMMasterDataFetched(false));
};

/** Edit Supplier Menu Action */
export const EditSupplier = (data) => (dispatch) => {
    dispatch(actions.EditSupplier());
    dispatch(actions.UpdateInteractionMode(false));
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule)); 
    dispatch(IsGRMMasterDataFetched(false));
};

/** Create Supplier Menu Action */
export const CreateSupplier = (data) => (dispatch) => {
    dispatch(actions.CreateSupplier());
    dispatch(actions.UpdateInteractionMode(false));
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule)); 
    dispatch(SetCurrentPageMode(null));
    dispatch(IsGRMMasterDataFetched(false));
};

export const CreateProfile = (data) => (dispatch) => {
    dispatch(actions.CreateProfile());
    dispatch(actions.UpdateInteractionMode(false));
    dispatch(UpdateCurrentPage(localConstant.techSpec.common.Create_Profile));
    dispatch(UpdateCurrentModule("grm"));
    dispatch(SetCurrentPageMode(null));
    dispatch(CleareMytaskRefCode());
};
export const EditViewTechnicalSpecialist = () => (dispatch) => {
    // TO-DO: Need to do with param and to use handler menu action as common.
    dispatch(actions.EditViewTechnicalSpecialist());
    dispatch(UpdateCurrentPage(localConstant.techSpec.common.Edit_View_Technical_Specialist));
    dispatch(UpdateCurrentModule("grm"));
};
export const HandleMenuAction = (data) => async (dispatch,getstate) =>{  
    dispatch(UpdateCurrentPage(data.currentPage));
    dispatch(UpdateCurrentModule(data.currentModule));
    dispatch(actions.UpdateInteractionMode(false));
    dispatch(SetCurrentPageMode(null)); 
    dispatch(actions.HandleMenuAction(data));
    if(data.currentModule !== localConstant.moduleName.TECHSPECIALIST){
        dispatch(IsGRMMasterDataFetched(false));
    }
    return true;
};
export const viewRole = () => (dispatch) => {
    dispatch(actions.viewRole());
};
export const EditMyProfileDetails = (data,functionRef) => async (dispatch) => {
    dispatch(ShowLoader());
    ///dispatch(UpdateCurrentPage(localConstant.techSpec.common.My_Profile_View_Technical_Specialist));
    dispatch(UpdateCurrentPage(localConstant.techSpec.common.Edit_View_Technical_Specialist));
    const userType = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
    if(userType===localConstant.techSpec.userTypes.TS)
    {
        const userName = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
        const param={
            "logonName":userName
        };
        let response = await dispatch(FetchTechSpecData(param));
        if(response&& (response.result.length>0)) {
            const param1={
                "epin":response.result[0].epin
            }; 
            response = await dispatch(FetchSavedDraftProfile(param1.epin,localConstant.techSpec.common.Edit_View_Technical_Specialist,"TS_EditProfile"));
            if( response && response.code === "1" &&  response.result && Array.isArray(response.result) && response.result.length <= 0) {
            const result = await dispatch(FetchMyProfileDatails(param1)); 
                if(result.TechnicalSpecialistInfo.taxonomyIsAcceptRequired && isFunction(functionRef)){
                    dispatch(FetchSelectedProfileTaxnomyHistoryCount(result.TechnicalSpecialistInfo.epin)).then(res => {
                        if(res){
                            functionRef();
                        }
                    });
                }
            }
        }
    }
    dispatch(HideLoader());
};