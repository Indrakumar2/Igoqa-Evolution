import AssignmentReferences from './assignmentReferences';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { 
    AddNewAssignmentReference,
    DeleteAssignmentReference,
    UpdatetAssignmentReference,
    // FetchReferencetypes
} from '../../../../actions/assignment/assignmentReferenceActions';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from  '../../../../utils/commonUtils';
import { isOperator,isCoordinator,isInterCompanyAssignment,isOperatorCompany } from '../../../../selectors/assignmentSelector';
//To-Do:need to introduce selectors to get common properties
const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    const contractHoldingCompanyCoordinators=isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators);
     return {
        assignmentReferenceDataGrid:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentReferences),
        loginUser:state.appLayoutReducer.loginUser,
        assignmentReferenceTypes:isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentReferenceTypes),
        assignmentId: state.rootAssignmentReducer.assignmentDetail.AssignmentInfo?
                state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId:null,
        isOperator: isOperator(assignmentInfo.assignmentOperatingCompanyCoordinator,
                    operatingCompanyCoordinators,
                    state.appLayoutReducer.username),
        isCoordinator:isCoordinator(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
                        contractHoldingCompanyCoordinators,
                        state.appLayoutReducer.username),
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        pageMode:state.CommonReducer.currentPageMode,
        isOperatingCompany: isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
            state.appLayoutReducer.selectedCompany),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                AddNewAssignmentReference,
                DeleteAssignmentReference,
                UpdatetAssignmentReference,
                // FetchReferencetypes
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps,mapDispatchToProps)(AssignmentReferences);