import ProfessionalEducationalDetails from './professionalEducationalDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { isEmptyReturnDefault,isUndefined, getNestedObject,getlocalizeData,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { UploadDocumentDetails,RemoveDocUniqueCode } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { withRouter } from 'react-router-dom';
import {

    AddWorkHistoryDetails,
    UpdateWorkHistoryDetails,
    DeleteWorkHistoryDetails,
    AddEducationalDetails,
    UpdateEducationalDetails,
    DeleteEducationalDetails,
    UpdateProfessionalSummary,
    AddProfessionalEducationDocuments,
    RemoveProfessionalEducationDocument,
    IsRCRMUpdatedProfessionalEducationInformation
} from '../../../actions/techSpec/professionalDetailAction';
import { RemoveDocumentDetails } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import {
    FetchDocumentUniqueName,
    DownloadDocumentData
} from '../../../../common/commonAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {  
    const IsRCRMUpdatedProfessionalEduDetails = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedProfessionalEduDetails" ]);  
    return { 
        workHistory: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistWorkHistory),
        educationaldetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistEducation),
        technicalSpecialistInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, 'object'),
        selectedEpinNo: state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? 0 : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        loggedInUser: state.appLayoutReducer.loginUser,
        uploadDocument: isEmptyReturnDefault(state.UploadDocumentReducer.uploadDocument),
        currentPage: state.CommonReducer.currentPage,
        oldProfileActionType:state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,
        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,

        selectedProfileDraft:state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        selectedProfileDraftWorkHistory: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistWorkHistory,'object'),
        selectedProfileDraftEducationaldetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistEducation,'object'),
        selectedProfileDraftTechSpecialistInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo, 'object'),

        draftWorkHistory: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistWorkHistory),
        draftEducationaldetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistEducation),
        draftTechnicalSpecialistInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistInfo, 'object'),
        pageMode:state.CommonReducer.currentPageMode,
        activities:state.appLayoutReducer.activities,
        isRCRMUpdate: IsRCRMUpdatedProfessionalEduDetails?IsRCRMUpdatedProfessionalEduDetails:false,
        isRCUserTypeCheck:userTypeCheck( { array:userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array:userTypeGet() ,param:localConstant.techSpec.userTypes.RM }),
        isTSUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TS }), 
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }), 
        isbtnDisable:state.RootTechSpecReducer.TechSpecDetailReducer.isbtnDisable,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddWorkHistoryDetails,
                UpdateWorkHistoryDetails,
                DeleteWorkHistoryDetails,
                AddEducationalDetails,
                UpdateEducationalDetails,
                DeleteEducationalDetails,
                DisplayModal,
                FetchDocumentUniqueName,
                HideModal,
                UpdateProfessionalSummary,
                UploadDocumentDetails,
                RemoveDocumentDetails,
                AddProfessionalEducationDocuments,
                RemoveProfessionalEducationDocument,
                DownloadDocumentData,
                RemoveDocUniqueCode,
                IsRCRMUpdatedProfessionalEducationInformation
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ProfessionalEducationalDetails));
