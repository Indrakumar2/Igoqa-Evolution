import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty,isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchProjectDocumentTypes,
    AddProjectDocumentDetails,
    DeleteProjectDocumentDetails,
    UpdateProjectDocumentDetails,
    FetchCustomerDocumentsofProject,
    FetchContractDocumentsofProject,
    AddFilesToBeUpload
} from '../../../../actions/project/documentAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload
} from '../../../../common/commonAction';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { filterDocTypes } from '../../../../common/selector';

const mapStateToProps = (state) => {
    return {
        projectCustomerDocumentsData: isEmpty(state.RootProjectReducer.ProjectDetailReducer.projectCustomerDocumentsData) ? []
            : state.RootProjectReducer.ProjectDetailReducer.projectCustomerDocumentsData,
        projectContractDocumentsData: isEmpty(state.RootProjectReducer.ProjectDetailReducer.projectContractDocumentsData) ? []
            : state.RootProjectReducer.ProjectDetailReducer.projectContractDocumentsData,
        documentrowData: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments),
        // projectDocumentTypeData: state.RootProjectReducer.ProjectDetailReducer.documentsTypeData,
        projectDocumentTypeData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Project' }),
        editProjectDocumentDetails: state.RootProjectReducer.ProjectDetailReducer.editProjectDocumentDetails,
        projectInfo: state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo,
        loggedInUser: state.appLayoutReducer.loginUser,
        isPanelOpened: state.RootProjectReducer.ProjectDetailReducer.isPanelOpen,
        projectMode: state.RootProjectReducer.ProjectDetailReducer.projectMode,
        selectedProjectNo: state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo,
        // interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode,
        fileToBeUploaded:isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.fileToBeUploaded),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchProjectDocumentTypes,
                AddProjectDocumentDetails,
                UpdateProjectDocumentDetails,
                FetchDocumentUniqueName,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DeleteProjectDocumentDetails,
                PasteDocumentUploadData,
                ShowHidePanel,
                FetchCustomerDocumentsofProject,
                FetchContractDocumentsofProject,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Documents);