import Timesheet from './timesheet';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchTimesheetPendingAproval } from '../../../viewComponents/dashboard/dahboardActions';
const mapStateToProps = (state) => {
    return {
        timeSheetPendingAproval:state.dashboardReducer.timeSheetPendingAproval,
        selectedCompany:state.appLayoutReducer.selectedCompany        
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            FetchTimesheetPendingAproval
        }, 
        dispatch
      ),
    };
  };
export default connect(mapStateToProps,mapDispatchToProps)(Timesheet);
