import SupplierSearch from './supplierSearch';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { SupplierFetchState,SupplierFetchCity,FetchSupplierSearchList,ClearSupplierSearchList,ClearGridFormSearchData } from '../../../../actions/supplier/supplierSearchAction';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {    
    return{
        country: state.masterDataReducer.countryMasterData,
        state: state.RootSupplierReducer.SupplierDetailReducers.supplierSearch.state,
        city: state.RootSupplierReducer.SupplierDetailReducers.supplierSearch.city,
        supplierSearchList: isEmptyReturnDefault(state.RootSupplierReducer.SupplierDetailReducers.supplierSearch.supplierSearchList),
        documentTypesData:state.masterDataReducer.documentTypeMasterData
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                SupplierFetchState,
                SupplierFetchCity,
                FetchSupplierSearchList,
                ClearSupplierSearchList,
                FetchDocumentTypeMasterData,
                ClearGridFormSearchData,
                ClearData
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(SupplierSearch);