import AssignmentLifecycleStatus from './assignmentLifecycleStatusDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { SaveAssignmentLifecycle } from '../../../actions/admin/assignmentLifecycleDetailAction';
const mapStateToProps = (state) => { 
    return {
        interactionMode: state.CommonReducer.interactionMode,
        currentPage: state.CommonReducer.currentPage        
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {         
                SaveAssignmentLifecycle         
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(AssignmentLifecycleStatus));