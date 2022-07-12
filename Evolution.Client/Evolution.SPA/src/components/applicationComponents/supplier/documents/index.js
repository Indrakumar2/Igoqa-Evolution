import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty,isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchSupplierDocumentTypes,
    AddSupplierDocumentDetails,
    DeleteSupplierDocumentDetails,
    UpdateSupplierDocumentDetails,   
    AddFilesToBeUpload 
} from '../../../../actions/supplier/supplierDocumentAction';
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
        documentrowData: isEmptyReturnDefault(state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments),
        // supplierDocumentTypeData: state.RootSupplierReducer.SupplierDetailReducers.documentsTypeData,
        supplierDocumentTypeData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Supplier' }),              
        loggedInUser: state.appLayoutReducer.loginUser,
        currentPage: state.CommonReducer.currentPage,
        selectedSupplierNo: state.RootSupplierReducer.SupplierDetailReducers.selectedSupplier.supplierId,
        // interactionMode: state.CommonReducer.interactionMode,   //Commented for Defect 669 -issue 14
        pageMode:state.CommonReducer.currentPageMode,
        editSupplierDocumentDetails: state.RootSupplierReducer.SupplierDetailReducers.editSupplierDocumentDetails,
        fileToBeUploaded:isEmptyReturnDefault(state.RootSupplierReducer.SupplierDetailReducers.supplierData.fileToBeUploaded),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierDocumentTypes,
                AddSupplierDocumentDetails,
                UpdateSupplierDocumentDetails,
                FetchDocumentUniqueName,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DeleteSupplierDocumentDetails,
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