import SupplierAnchor from './supplierAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { GetSelectedSupplier } from '../../../../actions/supplier/supplierSearchAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { UpdateCurrentPage,UpdateCurrentModule,SetCurrentPageMode } from '../../../../common/commonAction';

const mapStateToProps = (state) => {    
    return{
        selectedCompany: state.appLayoutReducer.selectedCompany,
        selectedCompanyData: isEmptyReturnDefault(state.appLayoutReducer.selectedCompanyData,'object'),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                GetSelectedSupplier,
                SetCurrentPageMode,
                UpdateCurrentPage,
                UpdateCurrentModule,
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(SupplierAnchor);