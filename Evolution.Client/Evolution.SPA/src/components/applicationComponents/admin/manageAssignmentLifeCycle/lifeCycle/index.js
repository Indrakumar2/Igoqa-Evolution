import LifeCycle from './assignmentLifeCycle';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../../common/baseComponents/customModal/customModalAction';
// import { isEmptyReturnDefault } from '../../../utils/commonUtils';
import { FetchAssignmentLifeCycle ,AddAssignmentLifeCycle, UpdateAssignmenttLifeCycle,
    DeleteAssignmentLifeCycle } from '../../../../../actions/admin/assignmnetLifecycleAction';
import { isEmptyReturnDefault } from '../../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';

const mapStateToProps = (state) => {
    return {
        assignmnetlifecycle:isEmptyReturnDefault(state.rootAdminReducer.assignmnetlifecycle)
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                FetchAssignmentLifeCycle,
                AddAssignmentLifeCycle,
                UpdateAssignmenttLifeCycle,
                DeleteAssignmentLifeCycle,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(LifeCycle));
