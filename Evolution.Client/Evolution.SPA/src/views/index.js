import App from './App';
import  { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { handleLogOut,RefreshToken } from '../actions/login/loginAction';
import { bindActionCreators } from 'redux';
import { DisplayModal,HideModal } from '../common/baseComponents/customModal/customModalAction';

const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            handleLogOut,
            RefreshToken,
            DisplayModal,
            HideModal
        }, 
        dispatch
      ),
    };
  };

export default withRouter(connect(null,mapDispatchToProps)(App));