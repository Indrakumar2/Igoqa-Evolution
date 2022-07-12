import Status from './assignmentStatus';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../../common/baseComponents/customModal/customModalAction';
import { FetchAssignmentStatus ,AddAssignmentStatus, UpdateAssignmentStatus,
    DeleteAssignmentStatus } from '../../../../../actions/admin/assignmentStatusAction';
import { isEmptyReturnDefault } from '../../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';

const mapStateToProps = (state) => {
    return {
        assignmnetStatus:isEmptyReturnDefault(state.rootAdminReducer.assignmnetStatus)
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                FetchAssignmentStatus,
                AddAssignmentStatus,
                UpdateAssignmentStatus,
                DeleteAssignmentStatus,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Status));
