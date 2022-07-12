import TimesheetSearch from './timesheetSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { 
    FetchCoordinatorForContractHoldingCompanyForSearch,
    FetchCoordinatorForOperatingCompanyForSearch } from '../../../../actions/assignment/generalDetailsActions';
import {
    FetchTimesheetSearchResults,
    ClearTimesheetSearchResults
}    from '../../../../actions/timesheet/timesheetAction';

import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { filterDocTypes } from '../../../../common/selector';

import { FetchTaxonomyBusinessUnitForSearch } from '../../../../actions/assignment/assignedSpecialistsAction';
import { ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        contractHoldingCompanyCoordinators: state.rootAssignmentReducer.contractHoldingCompanyCoordinators,
        operatingCompanyCoordinators: state.rootAssignmentReducer.operatingCompanyCoordinators,
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        defaultCustomerId:state.CustomerAndCountrySearch.defaultCustomerId,
        timesheetList: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetList),
        documentTypes:filterDocTypes({ 
            docTypes:state.masterDataReducer.documentTypeMasterData, 
            moduleName:'Visit' }),
        selectedCompany:state.appLayoutReducer.selectedCompany,
        viewAllRightsCompanies: isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies),    
        taxonomyCategory: isEmptyReturnDefault(state.rootAssignmentReducer.taxonomyBusinessUnit),
        techSpecSubCategory: state.masterDataReducer.techSpecSubCategory,
        techSpecServices: state.masterDataReducer.techSpecServices,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                FetchCoordinatorForContractHoldingCompanyForSearch,
                FetchCoordinatorForOperatingCompanyForSearch,
                FetchTimesheetSearchResults,
                ClearTimesheetSearchResults ,
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

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(TimesheetSearch));