import DownloadServerReports from './downloadServerReports';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { FetchServerReportData, ClearServerReports,DeleteReportFiles } from '../../../../actions/reports/visitReportsAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {    
    return {
        loggedInUser: state.appLayoutReducer.username,
        currentPage :state.CommonReducer.currentPage,
        serverReportData:state.rootReportReducer.serverReportData,
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators({
            FetchServerReportData,
            ClearServerReports,
            ShowLoader,
            HideLoader,
            DeleteReportFiles
        },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(DownloadServerReports));