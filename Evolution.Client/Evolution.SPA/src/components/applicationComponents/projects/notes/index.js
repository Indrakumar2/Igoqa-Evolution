import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty } from '../../../../utils/commonUtils';
import { AddProjectNotesDetails,EditProjectNotesDetails } from '../../../../actions/project/noteAction';
const mapStateToProps = (state) => {    
    return {
        projectsNotesData:(isEmpty(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotes)?[]
        :state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotes),
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        // interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode
       
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                AddProjectNotesDetails,
                EditProjectNotesDetails, //D661 issue8
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);
