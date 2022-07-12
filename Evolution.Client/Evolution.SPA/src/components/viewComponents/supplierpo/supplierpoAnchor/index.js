import SupplierpoAnchor from './supplierpoAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchSupplierPoData
} from '../../../../actions/supplierPO/supplierPOAction';
import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        selectedCompany: state.appLayoutReducer.selectedCompany
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators({
            FetchSupplierPoData,
            SetCurrentPageMode
        }
            , dispatch)
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(SupplierpoAnchor);
