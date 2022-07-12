import MyCalendar from './myCalendar';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchCalendarData, FetchVisitByID, FetchTimesheetGeneralDetail, FetchPreAssignment,FetchTimeOffRequestData } from '../../../actions/techSpec/globalCalendarAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
  return {
    selectedCompanyCode: state.appLayoutReducer.selectedCompany,
    userLogonName : state.appLayoutReducer.username
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
        FetchTimeOffRequestData
      },
      dispatch

    )
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(MyCalendar));