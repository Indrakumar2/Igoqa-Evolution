import GlobalCalendar from './globalCalendar';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import {
  FetchCalendarData,
  FetchVisitByID,
  FetchTimesheetGeneralDetail,
  FetchPreAssignment,
  SaveCalendarTask,
  ClearCalendarData,
  FetchTimeOffRequestData
} from '../../../actions/techSpec/globalCalendarAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
  return {
    calendarData: state.RootTechSpecReducer.TechSpecDetailReducer.calendarData,
    selectedCompanyCode: state.appLayoutReducer.selectedCompany,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {
        FetchCalendarData,
        FetchVisitByID,
        FetchTimesheetGeneralDetail,
        FetchPreAssignment,
        SaveCalendarTask,
        ClearCalendarData,
        FetchTimeOffRequestData
      },
      dispatch

    )
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(GlobalCalendar));