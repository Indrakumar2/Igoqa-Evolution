import SupplierDetail from './supplierDetail';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchCityForAddress,FetchStateForAddress,AddSupplierDetails,AddSupplierContact,UpdateSupplierContact,DeleteSupplierContact } from '../../../../actions/supplier/supplierDetailAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchSupplierSearchList } from '../../../../actions/supplier/supplierSearchAction';
const mapStateToProps = (state) => {    
    return{
        supplierInfo: isEmptyReturnDefault(state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierInfo,'object'),
        supplierContact: isEmptyReturnDefault(state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierContacts),
        country: state.masterDataReducer.countryMasterData,
        stateForAddress: state.RootSupplierReducer.SupplierDetailReducers.supplierDetails.stateForAddress,
        cityForAddress: state.RootSupplierReducer.SupplierDetailReducers.supplierDetails.cityForAddress,
        currentPage: state.CommonReducer.currentPage,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                FetchCityForAddress,
                FetchStateForAddress,
                AddSupplierDetails,
                AddSupplierContact,
                UpdateSupplierContact,
                DeleteSupplierContact,
                DisplayModal,
                HideModal,
                FetchSupplierSearchList,
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(SupplierDetail);