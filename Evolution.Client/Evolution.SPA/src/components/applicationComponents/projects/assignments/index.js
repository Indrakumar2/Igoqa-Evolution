import Assignments from './assignments';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchAssignments } from '../../../../actions/project/projectAssignmentsAction';
import { isEmpty,isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        assignmentData:isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.assignmentData),
        selectedAssignmentStatus:state.RootProjectReducer.ProjectDetailReducer.selectedAssignmentStatus
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignments
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps,mapDispatchToProps)(Assignments);