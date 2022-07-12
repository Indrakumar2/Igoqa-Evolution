import AppLogin from './login';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { loginStatus } from '../../appLayout/appLayoutActions';
import { AuthenticateLogin,UserMenu } from '../../../actions/login/loginAction';
import { loginUserName } from '../../../actions/login/forgotPasswordAction';
import { FetchDashboardCount } from '../dashboard/dahboardActions';
import { withRouter } from 'react-router-dom';
import authService from '../../../authService';
import { FetchAllMasterData,getViewAllRightsCompanies } from '../../../common/masterData/masterDataActions';
import { FetchAnnoncements } from '../../appLayout/appLayoutActions';

const mapStateToProps = (state) => {
  return {
      loginUser:state.appLayoutReducer.loginUser,
      loginPassWord:state.appLayoutReducer.loginPassWord,
      loginStatus: authService.isAuthenticated(),
      annoncementData:state.appLayoutReducer.annoncementData
     
  };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            loginStatus,
            AuthenticateLogin,
            UserMenu,            
            FetchDashboardCount,
            FetchAllMasterData,
            loginUserName,
            FetchAnnoncements,
            getViewAllRightsCompanies           
        }, 
        dispatch
      ),
    };
  };

export default withRouter(connect(mapStateToProps,mapDispatchToProps)(AppLogin));
