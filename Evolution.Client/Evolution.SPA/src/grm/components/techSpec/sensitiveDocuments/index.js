import SensitiveDocuments from './sensitiveDocuments';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { isEmptyReturnDefault ,isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';
import {
    AddSensitiveDetails,
    DeleteSensitiveDetails,
    UpdateSensitiveDetails,
    StoreDocUniqueCode,
    IsRemoveDocument,
    ClearDocumentUploadedSensitiveDocument,
    RevertDeletedSensitiveDocument
} from '../../../actions/techSpec/SensitiveDocumentsAction';
import {
    FetchDocumentUniqueName,
 
} from '../../../../common/commonAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { RemoveDocumentDetails,RemoveDocUniqueCode  } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {
    return {
        fetchDocumentTypeMasterData: isEmptyReturnDefault(state.masterDataReducer.documentTypeMasterData),
        selectedEpinNo:state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo,
        loggedInUser: state.appLayoutReducer.loginUser,
        currentPage:state.RootTechSpecReducer.TechSpecDetailReducer.currentPage,
        docUniqueCode:state.RootTechSpecReducer.TechSpecDetailReducer.documentUniqueCode,
        IsRemoveDocument: state.RootTechSpecReducer.TechSpecDetailReducer.IsRemoveDocument,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? 0 : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        sensitiveDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments),
        pageMode:state.CommonReducer.currentPageMode,
        activities:state.appLayoutReducer.activities,//SystemRole based UserType relevant Quick Fixes 
        selectedProfileDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'),//SystemRole based UserType relevant Quick Fixes 
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }),
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        assignedToUser:state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo ? isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo.assignedToUser)
        :""
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                AddSensitiveDetails,
                DeleteSensitiveDetails,
                UpdateSensitiveDetails,
                StoreDocUniqueCode,
                FetchDocumentUniqueName,
                RemoveDocumentDetails,
                RemoveDocUniqueCode,
                IsRemoveDocument,
                ClearDocumentUploadedSensitiveDocument,
                RevertDeletedSensitiveDocument
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(SensitiveDocuments));
