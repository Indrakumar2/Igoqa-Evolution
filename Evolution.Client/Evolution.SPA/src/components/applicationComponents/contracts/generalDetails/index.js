import GeneralDetails from './generalDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { UpdateInteractionMode } from '../../../../actions/contracts/contractAction';
import {
  FetchCompanyOffices,
  FetchParentContractNumber,
  FetchParentContractGeneralDetail,
  IfCRMYes, IfCRMNo, IfCRMSelect,
  OnSubmitCustomerName,
  CustomerShowModal,
  CustomerHideModal,
  FetchCustomerContacts,
  FetchCustomerList,
  ClearCustomerList,
  AddUpdateGeneralDetails,
  SelectedContractType,
  FetchInvoicingDefaultForContract,
  FetchParentContractDetail,
  ClearParentContractGeneralDetail
} from '../../../../actions/contracts/generalDetailsAction';
import { GetSelectedCustomerName } from '../../../../actions/contracts/contractSearchAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ShowModalPopup, HideModalPopup } from '../../../../common/baseComponents/modal/modalAction';
import { FetchInvoicingDefaults } from '../../../../actions/contracts/invoicingDefaultsAction';
import { GetSelectedCustomerCode } from '../../../viewComponents/customer/customerAction';
import { FetchContractData } from '../../../../actions/contracts/contractAction';

const mapStateToProps = (state) => {
  return {
    CustomerCodeInCRM: state.RootContractReducer.ContractDetailReducer.CustomerCodeInCRM,
    isShowModal: state.RootContractReducer.ContractDetailReducer.isShowModal,
    countryMasterData: state.masterDataReducer.countryMasterData,
    currencyData: state.masterDataReducer.currencyMasterData,
    customerList: state.RootContractReducer.ContractDetailReducer.customerList,
    generalDetailsData: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
    companyOffices: state.RootContractReducer.ContractDetailReducer.companyOffices,
    parentContractNumber: state.RootContractReducer.ContractDetailReducer.parentContractNumber,
    currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
    generalDetailsCreateContractCustomerDetails: state.RootContractReducer.ContractDetailReducer.customerName,
    selectedData: state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
    selectedCompany: state.appLayoutReducer.selectedCompany,
    companyList: state.appLayoutReducer.companyList,
    defaultCustomerName:state.CustomerAndCountrySearch.selectedCustomerData,
  };
};
const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {
        IfCRMYes,
        IfCRMNo,
        IfCRMSelect,
        CustomerShowModal,
        CustomerHideModal,
        FetchCustomerList,
        UpdateInteractionMode,
        FetchCompanyOffices,
        FetchParentContractNumber,
        OnSubmitCustomerName,
        FetchParentContractGeneralDetail,
        DisplayModal,
        ShowModalPopup,
        HideModalPopup,
        ClearCustomerList,
        AddUpdateGeneralDetails,
        HideModal,
        FetchInvoicingDefaults,
        FetchCustomerContacts,
        SelectedContractType,
        GetSelectedCustomerCode,
        FetchContractData,
        GetSelectedCustomerName,
        FetchInvoicingDefaultForContract,
        FetchParentContractDetail,
        ClearParentContractGeneralDetail
      },
      dispatch
    ),
  };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(GeneralDetails));