import Select from './select';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        assignmentReferenceTypes: isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentReferenceTypes),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Select));
