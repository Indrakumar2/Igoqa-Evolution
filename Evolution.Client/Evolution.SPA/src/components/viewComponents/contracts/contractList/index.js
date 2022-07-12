import ContractList from './contractList';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
// import { FetchCompanyList } from '../../../../components/appLayout/appLayoutActions';
import { UpdateInteractionMode,GetCurrentPage } from '../../../../actions/contracts/contractAction';
import { ShowLoader,HideLoader } from '../../../../common/commonAction';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
import { FetchCountry,ShowHidePanel,FetchCustomerContract,ClearSearchData,ClearGridFormSearchData } from '../../../../actions/contracts/contractSearchAction';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
const mapStateToProps = (state) => {
    return {
           contractHoldingCompany:state.appLayoutReducer.companyList,
           selectedCompany:state.appLayoutReducer.selectedCompany,
           currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
           isPanelOpen: state.RootContractReducer.ContractDetailReducer.isPanelOpen,           
           defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
           defaultCustomerId:state.CustomerAndCountrySearch.defaultCustomerId,
           countryMasterData: state.masterDataReducer.countryMasterData,
           customerContract: state.RootContractReducer.ContractDetailReducer.customerContract,
           contractStatus: state.RootContractReducer.ContractDetailReducer.contractStatus === ''? 'O' : state.RootContractReducer.ContractDetailReducer.contractStatus ,
           projectMode: state.RootProjectReducer.ProjectDetailReducer.projectMode,
           documentTypesData:state.masterDataReducer.documentTypeMasterData,
           pageMode:state.CommonReducer.currentPageMode,
           activities:state.appLayoutReducer.activities
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowHidePanel,
                FetchCountry,
                // FetchCompanyList,               
                FetchCustomerContract,
                ClearSearchData,
                ClearGridFormSearchData,          
                UpdateInteractionMode,
                FetchDocumentTypeMasterData,
                GetCurrentPage,          
                ShowLoader,
                HideLoader,
                ClearData
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ContractList));