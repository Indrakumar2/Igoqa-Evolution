import AppLayout from './appLayout'; 
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { FetchChargeTypes, FetchAnnoncements,AddDashboardCompanyId,FetchCompanyList } from './appLayoutActions';
const mapStateToProps = (state) => {
    return {
        loader:state.CommonReducer.loader
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {                
                FetchChargeTypes,
                FetchAnnoncements,
                AddDashboardCompanyId,
                FetchCompanyList
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(AppLayout));
