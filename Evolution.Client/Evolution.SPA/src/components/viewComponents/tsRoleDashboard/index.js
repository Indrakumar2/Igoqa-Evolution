import TSRoleDashboard from './tsRoleDashboard';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { GetTechSpecDashboardCompanyMessage } from '../dashboard/dahboardActions';
import { isEmptyReturnDefault, } from '../../../utils/commonUtils';
import { EditMyProfileDetails } from '../../sideMenu/sideMenuAction';

const mapStateToProps = (state) => {
  return {
      loginUserType:state.loginReducer.userDetails.utype,
      dashboardmsg:state.dashboardReducer.techSpecDashboardmessage,
      selectedTechSpecInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object')
  };
};
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
          GetTechSpecDashboardCompanyMessage,
          EditMyProfileDetails
        }, 
        dispatch
      ),
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(TSRoleDashboard);