import Dashboard from './dashboard';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
  ToggleAllCoordinator,
  Dashboardrefresh,
  FetchDashboardCount,
  FetchBudget,
  BudgetPropertyChange,
  MyTaskPropertyChange,
  MySearchPropertyChange,
} from './dahboardActions';
import { UpdateCurrentPage } from '../../../common/commonAction';
import { applicationConstants } from '../../../constants/appConstants';
import {
  FetchTechSpecMytaskData
} from '../../../grm/actions/techSpec/techSpecMytaskActions';
import { FetchMySearchData } from '../../../grm/actions/techSpec/mySearchActions';

const mapStateToProps = (state) => {
  return {
      dashboardCount:state.dashboardReducer.count,
      loginStatus:state.appLayoutReducer.loading,
      loader:state.CommonReducer.loader,
      currentPage: state.CommonReducer.currentPage,
      isAllCoordinator:state.dashboardReducer.allCoOrdinator,
      loginUserType:state.loginReducer.userDetails.utype,
      userTypes:localStorage.getItem(applicationConstants.Authentication.USER_TYPE),
      myTaskStatus:state.dashboardReducer.myTaskStatus,//added For home dashboard my task and my search count not refreshing
      mySearchStatus:state.dashboardReducer.mySearchStatus, 
  };
};
const mapDispatchToProps = dispatch => {
    return {
      
      actions: bindActionCreators(
        {
            ToggleAllCoordinator,
            Dashboardrefresh,
            FetchDashboardCount,            
            FetchBudget,
            BudgetPropertyChange,
            UpdateCurrentPage,
            FetchTechSpecMytaskData,
            MyTaskPropertyChange,
            FetchMySearchData,
            MySearchPropertyChange
        }, 
        dispatch
      ),
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(Dashboard);