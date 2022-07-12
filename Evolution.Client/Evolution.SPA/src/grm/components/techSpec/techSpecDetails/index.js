import TechSpecDetails from './techSpecDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import {
    SaveTechSpecDetails,
    FetchSelectedProfileDatails,
    CancelCreateTechSpecDetails,
    CancelEditTechSpecDetails,
    CancelTechSpecDraftChanges,
    UpdateProfileAction,
    UpdateTechSpecInfo,
    ExportToCV,
    DeleteTechSpecDraft,
    FetchUserTypeForInterCompanyResource
} from '../../../actions/techSpec/techSpecDetailAction';
import { GetSelectedProfile,FetchSavedDraftProfile } from '../../../actions/techSpec/techSpecSearchActions';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { grmDetailsMasterData,FetchProfileAction,FetchTechnicalManager } from '../../../../common/masterData/masterDataActions';
import { isEmptyReturnDefault,isUndefined,getlocalizeData,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { SetCurrentPageMode,UpdateCurrentPage }  from '../../../../common/commonAction';
import { ClearMyTasksData } from '../../../actions/techSpec/techSpecMytaskActions';
import { DeleteAlert } from '../../../../components/viewComponents/customer/alertAction';
import { EditMyProfileDetails } from '../../../../components/sideMenu/sideMenuAction';  //Added for Extranet-TS SSO
import { GetMyTaskARSSearch } from '../../../../actions/assignment/arsSearchAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
import { FetchUserPermission, FetchUserPermissionsData } from '../../../../../src/components/appLayout/appLayoutActions';
import { AddComments } from '../../../actions/techSpec/commentsAction';
const localConstant = getlocalizeData();

const mapStateToProps = (state) => { 
    return {
        interactionMode: state.CommonReducer.interactionMode,
        currentPage: state.CommonReducer.currentPage,
        techSpecDetailTabs: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techSpecDetailTabs),
        selectedProfileInfo: state.RootTechSpecReducer.TechSpecDetailReducer.techSpecSelectedData,
        selectedEpinNo: state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? '' : isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin)?'':state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        isDraft:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? false : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.isDraft,
        isbtnDisable: state.RootTechSpecReducer.TechSpecDetailReducer.isbtnDisable,
        loader: state.CommonReducer.loader,
        selectedProfileDetails:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails,'object'),
        defaultFieldType:state.RootTechSpecReducer.TechSpecDetailReducer.defaultFieldType,
        sendToInfo: state.masterDataReducer.profileActionMasterData,
        listOfTM: state.masterDataReducer.technicalManager,
        myTaskRefCode:  state.RootTechSpecReducer.TechSpecDetailReducer.myTaskRefCode,
        isbtnDisableDraft:state.RootTechSpecReducer.TechSpecDetailReducer.isbtnDisableDraft, 
        oldProfileAction: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        TechnicalSpecialistDocument:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistDocuments),
        isGrmMasterDataFeteched:state.CommonReducer.isGrmMasterDataFeteched,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        loginUserName: state.appLayoutReducer.userName,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList,
        userName:state.appLayoutReducer.username,// Added for D496 CR 
        listOfRC:state.masterDataReducer.resourceCoordinator,// Added for D496 CR 
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RM }),
        isTSUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TS }),
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }),// Added for D496 CR 
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),//Sanity defect 93 
        loginUser:state.appLayoutReducer.loginUser, // def 1306 
        prevBusinessInformationComment: state.RootTechSpecReducer.TechSpecDetailReducer.prevBussinessInformationComment,// def 1306 
        selectedProfileDraftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        payRateSchedule: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                grmDetailsMasterData,
                SaveTechSpecDetails,
                FetchSelectedProfileDatails,
                CancelCreateTechSpecDetails,
                CancelEditTechSpecDetails,
                CancelTechSpecDraftChanges,
                UpdateProfileAction,
                UpdateTechSpecInfo,
                DisplayModal,
                HideModal,      
                SetCurrentPageMode,
                ClearMyTasksData,
                UpdateCurrentPage,
                ExportToCV,
                DeleteTechSpecDraft,
                DeleteAlert,
                GetSelectedProfile,
                EditMyProfileDetails, //Added for Extranet-TS SSO
                GetMyTaskARSSearch,
                FetchSavedDraftProfile,
                FetchUserPermission,
                FetchUserPermissionsData,
                FetchUserTypeForInterCompanyResource,
                FetchProfileAction,
                AddComments, // def 1306
                FetchTechnicalManager, 
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TechSpecDetails));