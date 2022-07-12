import AssignmentDocuments from './documents';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchAssignmentDocumentTypes,
    AddAssignmentDocumentDetails,
    DeleteAssignmentDocumentDetails,
    UpdateAssignmentDocumentDetails,
    FetchAssignmentContractDocuments,
    FetchAssignmentProjectDocuments,
    FetchAssignmentSupplierPODocuments,
    FetchAssignmentVisitDocuments,
    FetchAssignmentTimesheetDocuments,
    AddFilesToBeUpload
} from '../../../../actions/assignment/assignmentDocumentAction';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,
} from '../../../../common/commonAction';
import { FetchAssignmentVisits } from '../../../../actions/visit/visitAction';
import { isEmpty,isEmptyReturnDefault,getlocalizeData } from '../../../../utils/commonUtils';
import { ShowHidePanel } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isVisit,isTimesheet } from '../../../../selectors/assignmentSelector';
import { filterDocTypes } from '../../../../common/selector';
const localConstant=getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const businessUnit =  { 
        projectReportParams: state.masterDataReducer.businessUnit,
        businessUnit: assignmentInfo.assignmentProjectBusinessUnit 
    };
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    return {
        assignmentProjectDocumentsData:state.rootAssignmentReducer.assignmentProjectDocumentsData ,
        editassignmentProjectDocumentsData:state.rootAssignmentReducer.editassignmentProjectDocumentsData,
        assignmentVisitDocumentsData: state.rootAssignmentReducer.assignmentVisitDocumentsData,
        assignmentTimesheetDocumentsData:state.rootAssignmentReducer.assignmentTimesheetDocumentsData,
        assignmentContractDocumentsData: state.rootAssignmentReducer.assignmentContractDocumentsData,
        assignmentSupplierPODocumentsData:state.rootAssignmentReducer.assignmentSupplierPODocumentsData,
        documentrowData:isEmpty(state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments)?[]:state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments,
        // assignmentDocumentTypeData: state.rootAssignmentReducer.documentsTypeData,
        assignmentDocumentTypeData: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Assignment' }),
        loggedInUser: state.appLayoutReducer.loginUser,
        interactionMode: state.CommonReducer.interactionMode,
        currentPage:state.CommonReducer.currentPage,
        assignmentSupplierPurchaseOrderNumber:state.rootAssignmentReducer.assignmentDetail.AssignmentInfo &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentSupplierPurchaseOrderNumber,
        assignmentId:state.rootAssignmentReducer.assignmentDetail &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId,
        assignmentType:state.rootAssignmentReducer.assignmentDetail.AssignmentInfo&&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentType,
        pageMode:state.CommonReducer.currentPageMode,
        isVisitAssignment:isVisit(workflowType),
        isTimesheetAssignment:isTimesheet(workflowType),
        fileToBeUploaded:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.fileToBeUploaded),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentDocumentTypes,
                AddAssignmentDocumentDetails,
                UpdateAssignmentDocumentDetails,
                FetchAssignmentContractDocuments,
                FetchAssignmentProjectDocuments,
                FetchAssignmentSupplierPODocuments,
                FetchAssignmentVisitDocuments,
                FetchDocumentUniqueName,
                DisplayModal,
                HideModal,
                UploadDocumentData,
                DeleteAssignmentDocumentDetails,
                PasteDocumentUploadData,
                ShowHidePanel,
                FetchAssignmentVisits,
                FetchAssignmentTimesheetDocuments,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(AssignmentDocuments);