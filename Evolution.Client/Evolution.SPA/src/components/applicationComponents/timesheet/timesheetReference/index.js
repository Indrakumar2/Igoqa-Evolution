import TimesheetReference from './timesheetReference';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';
import {
    isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { 
    AddNewTimesheetReference,
    UpdatetTimesheetReference,
    DeleteTimesheetReference,
    FetchReferencetypes,
    FetchTimesheetReferences
 } from '../../../../actions/timesheet/timesheetReferenceAction'; 
 import { 
    isTimesheetCreateMode, 
} from '../../../../selectors/timesheetSelector';

const mapStateToProps = (state) => {
    const timesheetInfo= isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object');
    return {
        timesheetReferenceTypes: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetReferenceTypes),
        timesheetReferenceDataGrid: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetReferences),
        loginUser: state.appLayoutReducer.loginUser,
        timesheetId: timesheetInfo.timesheetId?timesheetInfo.timesheetId:null,
        isTimesheetCreateMode:isTimesheetCreateMode(state.CommonReducer.currentPage),
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchReferencetypes,
                DisplayModal,
                HideModal,
                AddNewTimesheetReference,
                UpdatetTimesheetReference,
                DeleteTimesheetReference,
                FetchTimesheetReferences
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(TimesheetReference);