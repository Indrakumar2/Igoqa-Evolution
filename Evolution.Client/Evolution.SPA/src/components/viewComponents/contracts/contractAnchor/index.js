import ContractAnchor from './contractAnchor';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { 
  GetSelectedCustomerName,
  //GetSelectedContractData,
  FetchContractForDashboard
  } from '../../../../actions/contracts/contractSearchAction';
import {  } from '../../../../actions/contracts/contractAction';
import { FetchContractForProjectCreation } from '../../../../actions/project/projectAction';
import { FetchUserPermission } from '../../../appLayout/appLayoutActions';
import { UpdateCurrentPage,UpdateCurrentModule } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
  return {
    dashboardData:state.RootContractReducer.ContractDetailReducer.customerContract,
    selectedCompany:state.appLayoutReducer.selectedCompany,
    currentPage: state.CommonReducer.currentPage,
    logonUser:state.appLayoutReducer.username
  };
};
const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {
        GetSelectedCustomerName,
        //GetSelectedContractData,
        FetchUserPermission,
        FetchContractForDashboard,
        FetchContractForProjectCreation,
        UpdateCurrentPage,
        UpdateCurrentModule
      },
      dispatch
    ),
  };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ContractAnchor));