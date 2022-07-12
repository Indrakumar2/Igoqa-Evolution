import  SupplierVisitPerformanceReport from './supplierVisitPerformanceReportModal';
import { withRouter } from 'react-router-dom';
//import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { bindActionCreators } from 'redux';
import {  
    updateSupplierDetails,  
} from '../../../../actions/supplierPO/supplierPOAction';
import { fetchSupplierVisitPerformanceReport } from '../../../../actions/supplier/supplierReportsAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';
import { UpdateReportCustomer, ClearCustomerData, ClearReportsData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import {
    FetchSupplierSearchList,
    ClearSupplierSearchList,

} from '../../../../actions/supplierPO/supplierPOSearchAction';
import { connect } from 'react-redux';
const mapStateToProps = (state) => {

    return {
        companyList: state.appLayoutReducer.companyList,
        defaultCustomerName: state.CustomerAndCountrySearch.defaultCustomerName,
        supplierList: state.rootSupplierPOReducer.supplierList,    
        supplierVisitPerformaceDetails:state.RootSupplierReducer.SupplierDetailReducers.supplierVisitPerformanceData,
        supplierPoDetails: state.rootSupplierPOReducer.supplierPOData ? state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo : {},
        reportsCustomerName:state.CustomerAndCountrySearch.reportsCustomerName, 
        defaultReportCustomerName:state.CustomerAndCountrySearch.defaultReportCustomerName,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                ClearSupplierSearchList,
                updateSupplierDetails,
                FetchSupplierSearchList,
                fetchSupplierVisitPerformanceReport,
                UpdateReportCustomer,
                ClearCustomerData,
                ClearReportsData
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(SupplierVisitPerformanceReport));
