import VisitSearch from './visitSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { 
    FetchVisits, 
    FetchContractHoldingCoordinator, 
    FetchOperatingCoordinator, 
    UpdateVisitStartDate, 
    UpdateVisitEndDate,
    ClearSupplierSearchList,
    FetchSupplierSearchList,
    ClearVisitSearchResults,
    FetchVisitsForSearch
} from '../../../../actions/visit/visitAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchDocumentTypeMasterData,ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../../../common/masterData/masterDataActions';
import { FetchTaxonomyBusinessUnitForSearch } from '../../../../actions/assignment/assignedSpecialistsAction';

const mapStateToProps = (state) => {
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        contractHoldingCompany: state.appLayoutReducer.companyList,
        visitSearchData: isEmptyReturnDefault(state.rootVisitReducer.visitList),
        contractHoldingCoodinatorList: state.rootVisitReducer.chCoordinators,
        operatingCoordinatorList: state.rootVisitReducer.ocCoordinators,
        defaultCustomerName: state.CustomerAndCountrySearch.defaultCustomerName,
        visitStartDate: state.rootVisitReducer.visitStartDate,
        visitEndDate: state.rootVisitReducer.visitEndDate,
        contractHoldingCompanyCoordinators: state.rootAssignmentReducer.contractHoldingCompanyCoordinators,
        operatingCompanyCoordinators: state.rootAssignmentReducer.operatingCompanyCoordinators,
        supplierList: state.rootVisitReducer.supplierListForSearch,
        documentMasterData:state.masterDataReducer.documentTypeMasterData,
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
                FetchVisits,
                FetchContractHoldingCoordinator,
                FetchOperatingCoordinator,
                UpdateVisitStartDate,
                UpdateVisitEndDate,
                ClearData,
                ClearSupplierSearchList,
                FetchSupplierSearchList,
                ClearVisitSearchResults,
                FetchDocumentTypeMasterData,
                FetchVisitsForSearch,
                ClearSubCategory,
                ClearServices,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                FetchTaxonomyBusinessUnitForSearch
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(VisitSearch));