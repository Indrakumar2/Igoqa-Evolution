import AssignmentInstructions from './assignmentInstructions';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AssignmentInstructionsChange } from '../../../../actions/assignment/assignmentInstructionsAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { isOperator,isOperatorCompany,isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';

const mapStateToProps =(state)=> {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    return {
        assignmentInstruction: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInstructions, 'object'),
        assignmentId: state.rootAssignmentReducer.assignmentDetail.AssignmentInfo ?
                      state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId : null,

        currentPage: state.CommonReducer.currentPage,
        isOperator: isOperator(assignmentInfo.assignmentOperatingCompanyCoordinator,
            operatingCompanyCoordinators,
            state.appLayoutReducer.username),
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
                state.appLayoutReducer.selectedCompany),
        isInterCompanyAssignment: isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,
                    assignmentInfo.assignmentOperatingCompanyCode),
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(
            {
                AssignmentInstructionsChange
            }, dispatch
        )
    };
};

export default connect(mapStateToProps,mapDispatchToProps)(AssignmentInstructions);