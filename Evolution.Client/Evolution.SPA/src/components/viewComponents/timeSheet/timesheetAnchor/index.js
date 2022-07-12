import TimesheetAnchor from './timesheetAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { GetSelectedTimesheet,FetchTimesheetDetail } from '../../../../actions/timesheet/timesheetAction';
import { FetchTimesheetGeneralDetail } from '../../../../actions/timesheet/timesheetGeneralDetails';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { HandleMenuAction } from '../../../sideMenu/sideMenuAction';
import { SetCurrentPageMode } from '.././../../../common/commonAction';
const mapStateToProps = (state) => {    
    return{
        selectedCompany: state.appLayoutReducer.selectedCompany,
        selectedCompanyData: isEmptyReturnDefault(state.appLayoutReducer.selectedCompanyData,'object'),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                GetSelectedTimesheet,
                FetchTimesheetGeneralDetail,
                FetchTimesheetDetail,
                HandleMenuAction,
                SetCurrentPageMode
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(TimesheetAnchor);