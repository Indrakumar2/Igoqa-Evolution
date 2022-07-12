import ForgotPassword from './forgotPassword';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { securityQuestion ,validateSecurityQuestionAnswer ,loginUserName, resetPassword } from '../../../actions/login/forgotPasswordAction';
import { withRouter } from 'react-router-dom';
import { AuthenticateLogin,UserMenu } from '../../../actions/login/loginAction';
import authService from '../../../authService';
import { loginStatus } from '../../appLayout/appLayoutActions';
const mapStateToProps = (state) => {
  return {
    userSecurity: state.forgotPasswordReducer.userSecurity,
    loginUserName: state.forgotPasswordReducer.loginUserName,
    loginStatus: authService.isAuthenticated(),
  };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            securityQuestion,
            validateSecurityQuestionAnswer,
            AuthenticateLogin,
            UserMenu,
            loginStatus,
            loginUserName,
            resetPassword
        }, 
        dispatch
      ),
    };
  };

export default withRouter(connect(mapStateToProps,mapDispatchToProps)(ForgotPassword));
