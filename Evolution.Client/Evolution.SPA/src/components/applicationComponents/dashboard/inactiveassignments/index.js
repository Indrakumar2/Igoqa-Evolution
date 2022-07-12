import Inactiveassignments from './inactiveassignments';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchInActiveAssignments } from '../../../viewComponents/dashboard/dahboardActions';
const mapStateToProps = (state) => {
    return {
        inactiveAssignmentData:state.dashboardReducer.inactiveAssignmentData,
        selectedCompany:state.appLayoutReducer.selectedCompany
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            FetchInActiveAssignments
        }, 
        dispatch
      ),
    };
  }; 
export default connect(mapStateToProps,mapDispatchToProps)(Inactiveassignments);