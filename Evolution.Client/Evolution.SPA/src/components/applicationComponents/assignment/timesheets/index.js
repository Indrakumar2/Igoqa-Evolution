import TimeSheets from './timesheets';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchAssignmentTimesheets } from '../../../../actions/timesheet/timesheetAction';

const mapStateToProps = (state) => {
    return {
        timesheetData: state.rootTimesheetReducer.timesheetList,
        selectedTimesheetStatus:state.rootTimesheetReducer.selectedTimesheetStatus
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentTimesheets
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps,mapDispatchToProps)(TimeSheets);