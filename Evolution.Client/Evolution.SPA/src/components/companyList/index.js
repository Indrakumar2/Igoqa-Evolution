import CompanyList from './companyList';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { UserMenu ,UserType } from '../../actions/login/loginAction';
import { FetchCompanyList, FetchUserRoleCompanyList, UpdateSelectedCompany,FetchUserPermissionsData } from '../appLayout/appLayoutActions';
import { UpdateMasterCurrency } from '../../common/masterData/masterDataActions';
import { withRouter } from 'react-router-dom';
import {
    Dashboardrefresh,
    FetchDashboardCount,
    ToggleAllCoordinator,
    ClearDashboardReducer, ////added For home dashboard my task and my search count not refreshing
    MySearchPropertyChange,
    MyTaskPropertyChange,
} from '../viewComponents/dashboard/dahboardActions';
import { ClearMyTaskAssignUser } from '../../grm/actions/techSpec/techSpecMytaskActions';//D908 (#2Item 16-07-2020)

const mapStateToProps = (state) => {
    return {
        companyList: state.appLayoutReducer.companyList,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        userName: state.appLayoutReducer.username,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UserMenu,
                UserType,
                FetchCompanyList,
                FetchUserRoleCompanyList,
                UpdateSelectedCompany,
                Dashboardrefresh,
                FetchDashboardCount,
                FetchUserPermissionsData,
                UpdateMasterCurrency,
                ToggleAllCoordinator,  
                ClearDashboardReducer,//added For home dashboard my task and my search count not refreshing
                MySearchPropertyChange,
                MyTaskPropertyChange,
                ClearMyTaskAssignUser,
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(CompanyList));