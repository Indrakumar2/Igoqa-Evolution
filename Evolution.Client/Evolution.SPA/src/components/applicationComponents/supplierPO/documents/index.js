import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchSupplierPODocumentTypes,
    AddSupplierPODocumentDetails,
    DeleteSupplierPODocumentDetails,
    UpdateSupplierPODocumentDetails,
    FetchVisitDocOfSupplierPo,
    AddFilesToBeUpload
} from '../../../../actions/supplierPO/supplierPODocumentAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,
} from '../../../../common/commonAction';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
const mapStateToProps = (state) => {
    return {
        documentrowData: isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPODocuments),
        visitsDocumentsData: isEmptyReturnDefault(state.rootSupplierPOReducer.visitSupplierPoDocuments),
        // supplierPODocumentTypeData: state.rootSupplierPOReducer.documentsTypeData,
        editSupplierPODocuments:state.rootSupplierPOReducer.editSupplierDocumentDetails,
        supplierPODocumentTypeData: state.masterDataReducer.documentTypeMasterData && 
        state.masterDataReducer.documentTypeMasterData.filter(x=>x.moduleName==="Supplier PO"),
        loggedInUser: state.appLayoutReducer.loginUser,
        supplierPONumber: 0,
        pageMode:state.CommonReducer.currentPageMode,
        currentPage: state.CommonReducer.currentPage,
        supplierPOViewMode:state.rootSupplierPOReducer.supplierPOViewMode,    //For D-456
        fileToBeUploaded:isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.fileToBeUploaded),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierPODocumentTypes,
                AddSupplierPODocumentDetails,
                DeleteSupplierPODocumentDetails,
                FetchDocumentUniqueName,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                UpdateSupplierPODocumentDetails,
                FetchVisitDocOfSupplierPo,
                PasteDocumentUploadData,
                ShowHidePanel,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Documents);