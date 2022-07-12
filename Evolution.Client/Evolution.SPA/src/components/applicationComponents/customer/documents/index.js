import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchMasterDocumentTypes,
    FetchCustomerContractDocuments,
    FetchCustomerProjectDocuments,
    AddDocumentDetails,
    CopyDocumentDetails,
    DeleteDocumentDetails,
    EditDocumentDetails,
    UpdateDocumentDetails,
    ShowButtonHandler,
    DisplayDocuments,
    FetchCustomerContracts,
    FetchCustomerProjects,
    AddFilesToBeUpload 
} from '../../../viewComponents/customer/customerAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,

} from '../../../../common/commonAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { filterDocTypes } from '../../../../common/selector';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        projectDocumentsData: isEmptyReturnDefault(state.CustomerReducer.customerProjectDocumentData),
        contractDocumentsData: isEmptyReturnDefault(state.CustomerReducer.customerContractDocumentData),
        DocumentsData: isEmptyReturnDefault(state.CustomerReducer.customerDetail.Documents), //Changes for Live D780
        // masterDocumentTypesData: state.CustomerReducer.masterDocumentTypeData,
        masterDocumentTypesData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Customer' }),
        copiedDocumentDetails: state.CustomerReducer.copyDocumentDetails,
        editDocumentDetails: state.CustomerReducer.editDocumentDetails,
        showButton: state.CustomerReducer.showButton,
        selectedCustomerCode: state.CustomerReducer.selectedCustomerCode,
        isPanelOpen: state.RootContractReducer.ContractDetailReducer.isPanelOpen,
        customerInfo:state.CustomerReducer.customerDetail.Detail,
        loggedInUser: state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode,
        fileToBeUploaded:isEmptyReturnDefault(state.CustomerReducer.fileToBeUploaded)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchDocumentUniqueName,
                FetchCustomerContractDocuments,
                FetchCustomerProjectDocuments,
                FetchMasterDocumentTypes,
                AddDocumentDetails,
                CopyDocumentDetails,
                DeleteDocumentDetails,
                EditDocumentDetails,
                UpdateDocumentDetails,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DisplayDocuments,
                PasteDocumentUploadData,
                ShowHidePanel,
                FetchCustomerContracts,
                FetchCustomerProjects,
                MultiDocDownload,    
                AddFilesToBeUpload   
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Documents);