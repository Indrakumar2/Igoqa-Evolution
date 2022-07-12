import SupplierpoSearch from './supplierpoSearch';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchSupplierPOSearchResults,
    ClearSupplierpoSearchResults,
    FetchSupplierSearchList,
    ClearSupplierSearchList,
} from '../../../../actions/supplierPO/supplierPOSearchAction';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
import { ClearData } from "../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction";
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        supplierPOSearchData: state.rootSupplierPOReducer.supplierPOSearchData,
        supplierList:state.rootSupplierPOReducer.supplierList,
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        documentTypesData:state.masterDataReducer.documentTypeMasterData && 
        state.masterDataReducer.documentTypeMasterData.filter(x=>x.moduleName==="Supplier PO"),
        defaultCustomerId:state.CustomerAndCountrySearch.defaultCustomerId,
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        activities:state.appLayoutReducer.activities,
        selectedCompany:state.appLayoutReducer.selectedCompany
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierPOSearchResults,
                ClearSupplierpoSearchResults,
                FetchSupplierSearchList,
                ClearSupplierSearchList,
                ClearData,
                FetchDocumentTypeMasterData
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps,mapDispatchToProps)(SupplierpoSearch));