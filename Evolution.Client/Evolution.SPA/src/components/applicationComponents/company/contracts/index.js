import Contracts from './contracts';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchCompanyContract } from '../../../viewComponents/company/companyAction';
import { isEmpty } from '../../../../utils/commonUtils';
import { sortSelector } from '../../../../common/selector';
const mapStateToProps = (state) => {
    return {
        companyContractDetail: sortSelector(state.CompanyReducer.companyContracts,"contractCustomerName",'asc'),
		selectedContractStatus:isEmpty(state.CompanyReducer.selectedContractStatus)?[]:state.CompanyReducer.selectedContractStatus
    };
};
    const mapDispatchToProps = dispatch => {
        return {
            actions: bindActionCreators(
                {
                    FetchCompanyContract
                },
                dispatch
            ),
        };
    };
export default connect(mapStateToProps, mapDispatchToProps)(Contracts);