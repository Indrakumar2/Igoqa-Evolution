import CustomerRoleDashboard from './customerRoleDashboard';
import  { connect } from 'react-redux';

const mapStateToProps = (state) => {
  return {
      loginUserType:state.loginReducer.userDetails.utype,
      dashboardmsg:""
  };
};
const mapDispatchToProps = dispatch => {
    return {
     
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(CustomerRoleDashboard);