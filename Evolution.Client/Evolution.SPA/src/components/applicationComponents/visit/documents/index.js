import Documents from './documents';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault,isEmpty } from '../../../../utils/commonUtils';
import { 
    FetchVisitDocuments,
    FetchVisitDocumentTypes,
    AddVisitDocumentDetails,
    UpdateVisitDocumentDetails,
    DeleteVisitDocumentDetails,
    FetchAssignmentDocuments,
    AddFilesToBeUpload
} from '../../../../actions/visit/documentsAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,
} from '../../../../common/commonAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { filterDocTypes } from '../../../../common/selector';
import {   
    isOperatorApporved,
    isOperatorCompany,
    isCoordinatorCompany, 
} from '../../../../selectors/visitSelector';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';

const mapStateToProps = (state) => { 
    const visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object');
    return {
        VisitDocuments: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitDocuments),
        editVisitDocuments: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.editVisitDocuments),
        documentrowData:isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitDocuments),
        projectDocumentTypeData: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        loggedInUser: state.appLayoutReducer.loginUser,
        currentPage: state.CommonReducer.currentPage,
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        visitInfo: visitInfo,
        assignmentDocumentsData: state.rootVisitReducer.assignmentDocumentsData,
        pageMode:state.CommonReducer.currentPageMode,
        isOperatorApporved: isOperatorApporved(visitInfo),
        isCoordinatorCompany:isCoordinatorCompany(visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany),
        isOperatorCompany:isOperatorCompany(visitInfo.visitOperatingCompanyCode,state.appLayoutReducer.selectedCompany),
        iInterCompanyAssignment: isInterCompanyAssignment(visitInfo.visitContractCompanyCode, visitInfo.visitOperatingCompanyCode),
        fileToBeUploaded:isEmptyReturnDefault(state.rootVisitReducer.visitDetails.fileToBeUploaded),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchVisitDocuments,
                FetchVisitDocumentTypes,
                FetchDocumentUniqueName,
                UploadDocumentData,
                PasteDocumentUploadData,
                DisplayModal,
                HideModal,
                AddVisitDocumentDetails,
                UpdateVisitDocumentDetails,
                DeleteVisitDocumentDetails,
                FetchAssignmentDocuments,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Documents));