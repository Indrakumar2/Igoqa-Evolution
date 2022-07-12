import DocumentAproval from './documentAproval';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchDocumentApproval,ApproveDocument,FetchProjectForDocumentApproval,SelectedDocumentToApprove,RejectDocument,
         FetchAssignmentForDocumentApproval,FetchSupplierPOForDocumentApproval,UpdateSelectedRecord,
         FetchTimesheetForDocumentApproval,FetchVisitForDocumentApproval,FetchDocumentType,FetchContractForDocumentApproval } from '../../../viewComponents/dashboard/dahboardActions';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        documentApprovalGridData:isEmptyReturnDefault(state.dashboardReducer.documentApproval),
        selectedDocumentToApprove:state.dashboardReducer.selectedDocumentToApprove,
        selectedDocumentCustomer:state.dashboardReducer.selectedDocumentCustomer,
        documentType:state.dashboardReducer.documentType,
        contractsForDocumentApproval:state.dashboardReducer.contractsForDocumentApproval,
        projectForDocumentApproval:state.dashboardReducer.projectForDocumentApproval,
        assignmentForDocumentApproval:state.dashboardReducer.assignmentForDocumentApproval,
        supplierPOForDocumentApproval:state.dashboardReducer.supplierPOForDocumentApproval,
        timesheetForDocumentApproval:state.dashboardReducer.timesheetForDocumentApproval,
        visitForDocumentApproval:state.dashboardReducer.visitForDocumentApproval,
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            FetchDocumentApproval,
            ApproveDocument,
            FetchProjectForDocumentApproval,
            FetchAssignmentForDocumentApproval,
            FetchSupplierPOForDocumentApproval,
            UpdateSelectedRecord,
            SelectedDocumentToApprove,
            RejectDocument,
            DisplayModal,
            HideModal ,
            FetchTimesheetForDocumentApproval,
            FetchVisitForDocumentApproval,
            FetchDocumentType,
            FetchContractForDocumentApproval
        }, 
        dispatch
      ),
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(DocumentAproval);