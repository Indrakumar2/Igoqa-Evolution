import CustomerAndCountrySearch from './customerAndCountrySearch';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';

import {     
         FetchCustomerList,
         OnSubmitCustomerName,        
         SetDefaultCustomerName,
         ClearData ,
         SetSelectedCountryName,
         SetReportSelectedCountryName,
         SetDefaultReportCustomerName  } from './cutomerAndCountrySearchAction';         
import { ShowModalPopup, HideModalPopup } from '../../../common/baseComponents/modal/modalAction';        
const mapStateToProps = (state) => {
    return {
           contractHoldingCompany:state.appLayoutReducer.companyList,        
           isShowModal:state.ModalReducer.showModal,         
           defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
           countryMasterData: state.masterDataReducer.countryMasterData,
           customerList: state.CustomerAndCountrySearch.customerList,
           selectedCountryName:state.CustomerAndCountrySearch.selectedCountryName,   
           selectedReportCountryName: state.CustomerAndCountrySearch.selectedReportCountryName
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowModalPopup,
                HideModalPopup,
                FetchCustomerList,             
                OnSubmitCustomerName,                       
                SetDefaultCustomerName,
                ClearData,
                SetSelectedCountryName,
                SetReportSelectedCountryName,
                SetDefaultReportCustomerName
            
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(CustomerAndCountrySearch));