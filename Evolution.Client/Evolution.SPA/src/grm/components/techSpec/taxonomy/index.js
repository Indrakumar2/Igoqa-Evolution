import Taxonomy from './taxonomy';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault ,isUndefined,getlocalizeData,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { bindActionCreators } from 'redux';
import {
    AddInternalTrainingDetails,
    DeleteInternalTrainingDetails,
    UpdateInternalTrainingdetails,
    AddCompetencyDetails,
    DeleteCompetencyDetails,
    UpdateCompetencydetails,
    AddCustomerApprovalDetails,
    DeleteCustomerApprovedDetails,
    UpdateCustomerApprovedDetails,
    NewUploadDocumentHandler

} from '../../../actions/techSpec/taxonomyAction';
import { UploadDocumentDetails,RemoveDocUniqueCode  } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
    FetchTaxonomyInternalTraining,
    FetchTaxonomyCompetency,
    FetchTaxonomyCustomerApproved,
    FetchTaxonomyCustomerApprovedCommodity,
    ClearServices
} from '../../../../common/masterData/masterDataActions';
import { RemoveDocumentDetails } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {
     return {
        approvedTaxonomy: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy),
        internalTrainingData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInternalTraining),
        competencyData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCompetancy),
        customerApprovedData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCustomerApproval),
        newUploadDocument: state.RootTechSpecReducer.TechSpecDetailReducer.newUploadDocument,
        taxonomyInternalTraining: state.masterDataReducer.taxonomyInternalTraining,
        taxonomyCompetency: isEmptyReturnDefault(state.masterDataReducer.taxonomyCompetency),
        taxonomyCustomerApproved: state.masterDataReducer.taxonomyCustomerApproved,
        taxonomyCustomerCommodity: state.masterDataReducer.taxonomyCustomerCommodity,
        selectedEpinNo: state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? 0 : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        loggedInUser: state.appLayoutReducer.loginUser,
        uploadDocument: isEmptyReturnDefault(state.UploadDocumentReducer.uploadDocument),
        oldProfileActionType: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,

        selectedDraftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        selectedDraftInternalTraining: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInternalTraining),
        selectedDraftCompetency: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistCompetancy),
        selectedDraftCustomerApproved: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistCustomerApproval),

        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,
        draftInternalTraining: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistInternalTraining),
        draftCompetency: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistCompetancy),
        draftCustomerApproved: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistCustomerApproval),
        pageMode:state.CommonReducer.currentPageMode,
        activities:state.appLayoutReducer.activities,

        technicalSpecialistInfo:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'), /** Added for TM Edit/View Access changes, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RM }),
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                FetchTaxonomyInternalTraining,
                FetchTaxonomyCompetency,
                FetchTaxonomyCustomerApproved,
                FetchTaxonomyCustomerApprovedCommodity,
                AddInternalTrainingDetails,
                DeleteInternalTrainingDetails,
                UpdateInternalTrainingdetails,
                AddCompetencyDetails,
                DeleteCompetencyDetails,
                UpdateCompetencydetails,
                AddCustomerApprovalDetails,
                DeleteCustomerApprovedDetails,
                UpdateCustomerApprovedDetails,
                NewUploadDocumentHandler,
                UploadDocumentDetails,
                ClearServices,
                RemoveDocumentDetails,
                RemoveDocUniqueCode 
            },
            dispatch

        )
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Taxonomy)); 