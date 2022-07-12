import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddAssignmentNotesDetails,EditAssignmentNotesDetails } from '../../../../actions/assignment/assignmentNoteAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {    
    return {
        assignmentsNotesData:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentNotes),
        loggedInUser: state.appLayoutReducer.username,     
        loggedInUserName: state.appLayoutReducer.loginUser,  
        interactionMode: state.CommonReducer.interactionMode,
        assignmentId: state.rootAssignmentReducer.assignmentDetail.AssignmentInfo ?
        state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId : null,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                AddAssignmentNotesDetails,
                EditAssignmentNotesDetails
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);
