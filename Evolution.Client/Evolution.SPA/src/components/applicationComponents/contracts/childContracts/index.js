import ChildContracts from './childContract';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchChildContractsOfPrarent } from '../../../../actions/contracts/childContractAction';
import { isEmpty } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        contractprojectDetail: isEmpty(state.RootContractReducer.ContractDetailReducer.childContractsOfParent) ? [] :
            state.RootContractReducer.ContractDetailReducer.childContractsOfParent,
        selectedContractStatus: state.RootContractReducer.ContractDetailReducer.selectedContractStatus
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchChildContractsOfPrarent
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(ChildContracts);