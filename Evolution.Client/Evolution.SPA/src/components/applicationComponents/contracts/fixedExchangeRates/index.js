import FixedExchangeRates from './fixedExchangeRates';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { UseContractExchangeRate, DeleteFixedExchangeRate, UpdateFixedExchangeRate, EditFixedExchangeRate, AddFixedExchangeRate,ExchangeRateModalState,ExchangeRateEditCheck } from '../../../../actions/contracts/fixedExchangeRatesAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { bindActionCreators } from 'redux';

const mapStateToProps = (state) => {

  return {
    showButton: state.RootContractReducer.ContractDetailReducer.showButton,
    isExchangeRateEdit: state.RootContractReducer.ContractDetailReducer.isExchangeRateEdit,
    ContractFixedRate: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates,
    editFixedExchangeDetails: state.RootContractReducer.ContractDetailReducer.editFixedExchangeDetails,
    selectedContract:state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
    currencyData: state.masterDataReducer.currencyMasterData,
    useContractExchangeRate: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
    isExchangeRateModalOpen:state.RootContractReducer.ContractDetailReducer.isExchangeRateModalOpen,
    isExchangeEdit:state.RootContractReducer.ContractDetailReducer.isExchangeEdit,
    loggedInUser: state.appLayoutReducer.loginUser,
    pageMode:state.CommonReducer.currentPageMode

  };
};
const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {        
        AddFixedExchangeRate,
        DeleteFixedExchangeRate,
        EditFixedExchangeRate,
        UpdateFixedExchangeRate,
        DisplayModal,
        HideModal,
        UseContractExchangeRate,        
        ExchangeRateModalState,
        ExchangeRateEditCheck,      
      },
      dispatch

    )
  };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(FixedExchangeRates));
