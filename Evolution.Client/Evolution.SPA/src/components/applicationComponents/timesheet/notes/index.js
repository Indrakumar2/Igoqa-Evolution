import Notes from './notes';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { AddTimesheetNotes,
        FetchTimesheetNotes,EditTimesheetNotes  } from '../../../../actions/timesheet/timesheetNoteAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { 
    getTimesheetId
} from '../../../../selectors/timesheetSelector';

const mapStateToProps = (state) => { 
    const timesheetInfo= isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object');
    return {
        timesheetNotes:isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetNotes),
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        timesheetId: timesheetInfo.timesheetId?timesheetInfo.timesheetId:null,
        pageMode:state.CommonReducer.currentPageMode
        };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddTimesheetNotes,
                FetchTimesheetNotes,
                EditTimesheetNotes
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);