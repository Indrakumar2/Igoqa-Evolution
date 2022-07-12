import Documents from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchMasterCompanyDocumentTypes,
    AddDocumentDetails,
    CopyDocumentDetails,
    DeleteCompanyDocumentDetails,
    UpdateDocumentDetails,
    ShowButtonHandler,
    DispalyDocumentDetails,
    AddFilesToBeUpload,
} from '../../../viewComponents/company/companyAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload
} from '../../../../common/commonAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { filterDocTypes } from '../../../../common/selector';
const mapStateToProps = (state) => {
    return {
        DocumentsData: isEmptyReturnDefault(state.CompanyReducer.companyDetail.CompanyDocuments),
        //masterDocumentTypesData: isEmptyReturnDefault(state.CompanyReducer.masterDocumentTypeData),
        masterDocumentTypesData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Company' }),
        gridProps: state.CompanyReducer.gridProps,
        copiedDocumentDetails: isEmptyReturnDefault(state.CompanyReducer.copyDocumentDetails,'object'),
        editCompanyDocumentDetails: isEmptyReturnDefault(state.CompanyReducer.editCompanyDocumentDetails,'object'),
        showButton: state.CompanyReducer.showButton,
        companyInfo: state.CompanyReducer.companyDetail.CompanyInfo,
        loggedInUser: state.appLayoutReducer.loginUser,
        displayDocuments: isEmptyReturnDefault(state.CompanyReducer.displayDocuments),
        pageMode:state.CommonReducer.currentPageMode,
        fileToBeUploaded:isEmptyReturnDefault(state.CompanyReducer.fileToBeUploaded)
    };    
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {         
                FetchDocumentUniqueName,      
                FetchMasterCompanyDocumentTypes,
                AddDocumentDetails,
                CopyDocumentDetails,
                DeleteCompanyDocumentDetails,
                UpdateDocumentDetails,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DispalyDocumentDetails,
                PasteDocumentUploadData,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Documents);