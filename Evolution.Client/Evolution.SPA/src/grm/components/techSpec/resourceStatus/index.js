import ResourceStatus from './resourceStatus';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault,getlocalizeData } from '../../../../utils/commonUtils';
import { UploadDocumentDetails,RemoveDocUniqueCode } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
    AddStampDetails,
    DeleteStampDetails,
    UpdateStampDetails,
    UpdateResourceStatus,
    FeatchTechSpechStampCountryCode
} from '../../../actions/techSpec/resourceStatusAction';
import { RemoveDocumentDetails } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();
const mapStateToProps = (state, props) => { 
    const profileInfo = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object');    
    return {
        stampDetails:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistStamp),
        companyList: state.masterDataReducer.companies,
        profileStatus: state.masterDataReducer.profileStatus,
        employmentStatus: state.masterDataReducer.employmentStatus,
        subDivision: state.masterDataReducer.subDivision,
        countryCodes: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.masterTechSpecStampCountryCodes),
        profileInfo: profileInfo,
        uploadDocument: isEmptyReturnDefault(state.UploadDocumentReducer.uploadDocument),
        currentPage : state.CommonReducer.currentPage,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList,
        pageMode:state.CommonReducer.currentPageMode,
        taxonomyDetails:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy),
        activities:state.appLayoutReducer.activities,
        tsChangeApprovalStatus:(profileInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.InProgress || profileInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected),
        selectedCompany: state.appLayoutReducer.selectedCompany, //D363 CR change
        rejectionShow:(profileInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.UpdateAfterReject || profileInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected),
        isTSUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TS }),
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }),
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        oldProfileAction: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,
        loginUser:state.appLayoutReducer.loginUser,
        //assignedToUser:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo.assignedToUser)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddStampDetails,
                DeleteStampDetails,
                UpdateStampDetails,
                DisplayModal,
                HideModal,
                UpdateResourceStatus,
                UploadDocumentDetails,
                RemoveDocumentDetails,
                RemoveDocUniqueCode,
                FeatchTechSpechStampCountryCode
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ResourceStatus));