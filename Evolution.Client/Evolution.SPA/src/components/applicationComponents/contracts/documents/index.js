import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {    
    FetchContractDocumentTypes,
    AddDocumentDetails,
    DeleteContractDocumentDetails,
    CopyDocumentDetails,
    UpdateDocumentDetails,
    FetchCustomerDocumentsofContracts,
    FetchProjectDocumentsofContracts,
    AddFilesToBeUpload,
    FetchContractDocuments } from '../../../../actions/contracts/documentAction';
import { FetchContractProjects } from '../../../../actions/contracts/contractProjectAction';
    import {
        FetchDocumentUniqueName,
        UploadDocumentData,
        PasteDocumentUploadData,
        MultiDocDownload
    } from '../../../../common/commonAction';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { filterDocTypes } from '../../../../common/selector';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        projectDocumentsData: isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractProjectdocumentsData),
        customerDocumentsData:state.RootContractReducer.ContractDetailReducer.contractCustomerdocumentsData,
        parentContractDocumentsData: state.RootContractReducer.ContractDetailReducer.parentContractDocumentsData,
        documentrowData: isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments),
        //contractDocumentTypeData: state.RootContractReducer.ContractDetailReducer.documentTypeData,
        contractDocumentTypeData: filterDocTypes({ 
            docTypes:state.masterDataReducer.documentTypeMasterData, 
            moduleName:'Contract' }),
        copiedDocumentDetails: state.RootContractReducer.ContractDetailReducer.copyDocumentDetails,
        documentDetailsInfo: state.RootContractReducer.ContractDetailReducer.documentDetailsInfo,
        isShowDocumentModal: state.RootContractReducer.ContractDetailReducer.isShowDocumentModal,
        isDocumentModelAddView: state.RootContractReducer.ContractDetailReducer.isDocumentModelAddView,
        editContractDocumentDetails: state.RootContractReducer.ContractDetailReducer.editContractDocumentDetails,
        currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
        contractInfo: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
        loggedInUser: state.appLayoutReducer.loginUser,
        isPanelOpen: state.RootContractReducer.ContractDetailReducer.isPanelOpen,
        isEditContractDocumentDetails: state.RootContractReducer.ContractDetailReducer.isEditContractDocumentDetails,
        selectedContract:state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
        generalDetailsCreateContractCustomerDetails: state.RootContractReducer.ContractDetailReducer.customerName,
        pageMode:state.CommonReducer.currentPageMode,
        fileToBeUploaded:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.fileToBeUploaded),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchProjectDocumentsofContracts,                
                FetchContractDocumentTypes,
                FetchDocumentUniqueName,
                AddDocumentDetails,
                CopyDocumentDetails,
                UpdateDocumentDetails,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DeleteContractDocumentDetails,
                PasteDocumentUploadData,
                ShowHidePanel,
                FetchCustomerDocumentsofContracts,
                FetchContractProjects,
                FetchContractDocuments,
                MultiDocDownload, 
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Documents);