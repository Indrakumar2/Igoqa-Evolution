import TechnicalSpecialistDocuments from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,
} from '../../../../common/commonAction';
import {
    AddTechSpecDocDetails,
    DeleteTechSpecDocDetails,
    UpdateTechSpecDocDetails,
    IsRCRMUpdatedDocumentInformation,
    AddFilesToBeUpload
} from '../../../actions/techSpec/techSpecDocumentAction';
import { isEmptyReturnDefault, isUndefined, getNestedObject,getlocalizeData } from '../../../../utils/commonUtils';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { userTypeCheck } from '../../../../selectors/techSpechSelector'; 
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {
    const IsRCRMUpdatedDocumentInfo = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedDocumentInfo" ]);  
    return {
        documentrowData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistDocuments),
        fetchDocumentTypeMasterData: isEmptyReturnDefault(state.masterDataReducer.documentTypeMasterData),
        loggedInUser: state.appLayoutReducer.loginUser,
        interactionMode: state.CommonReducer.interactionMode,
        currentPage:state.CommonReducer.currentPage,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? null : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,

        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,
        selectedDraftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        selectedProfileDraftDocuments: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistDocuments),
        draftDocuments: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistDocuments),  
        activities:state.appLayoutReducer.activities,//SystemRole based UserType relevant Quick Fixes 
        selectedProfileDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'), 
        pageMode:state.CommonReducer.currentPageMode,
        isRCRMUpdate: IsRCRMUpdatedDocumentInfo?IsRCRMUpdatedDocumentInfo:false,
        isRCUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RM }),
        isTSUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TS }),
        isTMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TM }),
        fileToBeUploaded:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.fileToBeUploaded),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchDocumentTypeMasterData,
                AddTechSpecDocDetails,
                DeleteTechSpecDocDetails,
                UpdateTechSpecDocDetails,
                FetchDocumentUniqueName,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                PasteDocumentUploadData,
                ShowHidePanel,
                IsRCRMUpdatedDocumentInformation,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(TechnicalSpecialistDocuments);