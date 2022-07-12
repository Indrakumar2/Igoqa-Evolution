import Contracts from './contracts';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchCustomerContracts } from '../../../viewComponents/customer/customerAction';
import { isEmpty } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        selectedcontractStatusValue:state.CustomerReducer.selectedcontractStatusOfCustomer,
        customerContractDetail: isEmpty(state.CustomerReducer.customerContracts) ? [] : state.CustomerReducer.customerContracts
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {            
                FetchCustomerContracts
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Contracts);
