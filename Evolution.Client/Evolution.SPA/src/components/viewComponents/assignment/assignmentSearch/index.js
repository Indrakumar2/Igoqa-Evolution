import AssignmentSearch from './assignmentSearch';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchAssignmentSearchResultswithLoadMore,
     FetchAssignmentStatus,
     ClearAssignmentSearchResults
} from '../../../../actions/assignment/assignmentAction';
import { FetchTaxonomyBusinessUnitForSearch } from '../../../../actions/assignment/assignedSpecialistsAction';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {
    return {
        assignmentStatus: state.rootAssignmentReducer.assignmentStatus,
        companyList:state.appLayoutReducer.companyList,
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        assignmentList: state.rootAssignmentReducer.assignmentList,
        documentTypesData:state.masterDataReducer.documentTypeMasterData && 
        state.masterDataReducer.documentTypeMasterData.filter(x=>x.moduleName==="Assignment"),
        currentPage: state.CommonReducer.currentPage,
        selectedCompany:state.appLayoutReducer.selectedCompany,
        viewAllRightsCompanies: isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies),
        defaultCustomerId:state.CustomerAndCountrySearch.defaultCustomerId,
        taxonomyCategory: isEmptyReturnDefault(state.rootAssignmentReducer.taxonomyBusinessUnit),
        techSpecSubCategory: state.masterDataReducer.techSpecSubCategory,
        techSpecServices: state.masterDataReducer.techSpecServices,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentSearchResultswithLoadMore,
                FetchAssignmentStatus,
                ClearAssignmentSearchResults,
                ClearData,
                FetchTaxonomyBusinessUnitForSearch,
                ClearSubCategory,
                ClearServices,
                FetchTechSpecSubCategory,
                FetchTechSpecServices
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AssignmentSearch);