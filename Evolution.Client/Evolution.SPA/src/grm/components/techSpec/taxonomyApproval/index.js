import TaxonomyApproval from './taxonomyApproval';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import {
    AddTaxonomyApprovalDetails,
    DeleteTaxonomyApprovalDetails,
    UpdateTaxonomyApprovaldetails,
    UpdateTQMComments,
} from '../../../actions/techSpec/taxonomyApprovalAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault,isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import { ShowModalPopup } from '../../../../common/baseComponents/modal/modalAction';
import {
    FetchTechSpecCategory,
    FetchTechSpecSubCategory,
    FetchTechSpecServices,
    ClearSubCategory,
    ClearServices
} from '../../../../common/masterData/masterDataActions';
import { userTypeCheck } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();

const mapStateToProps = (state) => {

    return {
        taxonomyApproval: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy),
        techSpecCategory: state.masterDataReducer.techSpecCategory,
        techSpecSubCategory: state.masterDataReducer.techSpecSubCategory,
        techSpecServices: state.masterDataReducer.techSpecServices,
        // isTaxonomyApprovalDetailsEdit: state.RootTechSpecReducer.TechSpecDetailReducer.isTaxonomyApprovalDetailsEdit,
        // taxonomyApprovalModal: state.RootTechSpecReducer.TechSpecDetailReducer.taxonomyApprovalModal,
        // taxonomyApprovalEditData: state.RootTechSpecReducer.TechSpecDetailReducer.taxonomyApprovalEditData,
        loggedInUser: state.appLayoutReducer.loginUser,
        showModal: state.ModalReducer.showModal,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? null : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        pageMode:state.CommonReducer.currentPageMode,
        activities:state.appLayoutReducer.activities,//SystemRole based UserType relevant Quick Fixes 
        selectedProfileDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'),//SystemRole based UserType relevant Quick Fixes 
        isTSUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TS }),
        isTMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TM }),
        isRCUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RC }),
        assignedToUser:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo)?'':state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo.assignedToUser,
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                FetchTechSpecCategory,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                AddTaxonomyApprovalDetails,
                DeleteTaxonomyApprovalDetails,
                UpdateTaxonomyApprovaldetails,
                ShowModalPopup,
                ClearSubCategory,
                ClearServices,
                UpdateTQMComments,
            },
            dispatch

        )
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TaxonomyApproval)); 