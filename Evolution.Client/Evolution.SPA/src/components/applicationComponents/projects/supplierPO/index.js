import SupplierPO from './supplierPO';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchSupplierPo } from '../../../../actions/project/projectSupplierPoAction';
import { isEmpty } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        supplierPoData:isEmpty(state.RootProjectReducer.ProjectDetailReducer.supplierPoData)?[]:state.RootProjectReducer.ProjectDetailReducer.supplierPoData,
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierPo
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps,mapDispatchToProps)(SupplierPO);
