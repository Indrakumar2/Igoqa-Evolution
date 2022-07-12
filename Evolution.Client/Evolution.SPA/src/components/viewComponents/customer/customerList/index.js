import CustomerList from './customerList';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchCustomerList, ClearSearchData,FetchMasterDocumentTypes, ClearGridFormSearchData } from '../../customer/customerAction';
import { FetchCountry } from '../../company/companyAction';
import { filterDocTypes } from '../../../../common/selector';
const mapStateToProps = (state) => {
    return {       
        // countryMasterData: state.CompanyReducer.countryMasterData,
        countryMasterData: state.masterDataReducer.countryMasterData,
        customerData: state.CustomerReducer.customerList,
        //documentTypesData:state.CustomerReducer.masterDocumentTypeData,
        documentTypesData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Customer' }),
        pageMode:state.CommonReducer.currentPageMode  
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {                
                FetchCountry,   
                FetchCustomerList,
                ClearSearchData ,
                FetchMasterDocumentTypes,
                ClearGridFormSearchData       
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CustomerList);
