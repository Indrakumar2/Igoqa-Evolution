import ProjectSearch from './projectSearch';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { ShowLoader,HideLoader } from '../../../../common/commonAction';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { FetchProjectListSearch, ClearSearchCustomerList,ClearSearchProjectList, ClearGridFormSearchData } from '../../../../actions/project/projectSearchAction';
import { FetchDocumentTypeMasterData } from '../../../../common/masterData/masterDataActions';
import { filterDocTypes } from '../../../../common/selector';
const mapStateToProps = (state) => {
    
    return {
        contractHoldingCompany: state.appLayoutReducer.companyList,
        projectStatus:state.RootProjectReducer.ProjectDetailReducer.projectStatus,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        projectBusinessUnit: state.masterDataReducer.businessUnit,
        projectList: state.RootProjectReducer.ProjectDetailReducer.projectSearchList,
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        currentPage: state.CommonReducer.currentPage,
        // documentTypesData:state.masterDataReducer.documentTypeMasterData,
        documentTypesData:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Project' }),
        pageMode:state.CommonReducer.currentPageMode,
        defaultCustomerId:state.CustomerAndCountrySearch.defaultCustomerId
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                FetchDocumentTypeMasterData,
                FetchProjectListSearch,
                ClearSearchCustomerList,
                ClearSearchProjectList,
                ClearGridFormSearchData,
                ClearData
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ProjectSearch));