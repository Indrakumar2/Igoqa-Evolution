import AssignmentAnchor from './assignmentAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import {
    SaveSelectedAssignmentId
} from '../../../../actions/assignment/assignmentAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        currentPage: state.CommonReducer.currentPage,
        selectedCompany:state.appLayoutReducer.selectedCompany,
        pageMode:state.CommonReducer.currentPageMode, //Added for D479 issue1
        assignmentData:isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.assignmentData),
        supplierPOAssignmentData: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentList),
    };
};

const mapDispatchToProps = (dispatch)=>{
return {
    actions:bindActionCreators({
        SaveSelectedAssignmentId,
    }
    ,dispatch)
};
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(AssignmentAnchor));