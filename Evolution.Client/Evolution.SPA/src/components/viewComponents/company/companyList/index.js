import CompanyList from './companyList';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchCompanyDataList,ClearGridFormSearchData, ClearSearchData,FetchCountry,showHidePanel,ClearCompanyDetails } from '../companyAction';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
const mapStateToProps = (state) => {
    return {       
        countryMasterData: state.masterDataReducer.countryMasterData,
        companyDataList: state.CompanyReducer.companyDataList,
        isopen: state.CompanyReducer.isopen,   
        isSearch: state.CompanyReducer.isSearch,
        documentTypesData:state.masterDataReducer.documentTypeMasterData,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {                
                FetchCountry,   
                FetchCompanyDataList,
                ClearSearchData,
                showHidePanel,
                ClearCompanyDetails ,
                FetchDocumentTypeMasterData,
                ClearGridFormSearchData    
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CompanyList);
