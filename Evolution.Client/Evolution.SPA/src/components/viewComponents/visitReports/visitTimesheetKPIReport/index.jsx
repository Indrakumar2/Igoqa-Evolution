import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import  visitTimesheetKPIReport from './visitTimesheetKPIReport';
import { FetchCustomerList,FetchCustomerContracts,FetchContractProjects,
    ClearCustomerContracts, ClearContractProjects,FetchApprovedUnApprovedVistCustomer,FetchKPICustomer,FetchKPIVisitTimesheetContract,FetchKPIVisitTimesheetProject } from '../../../../actions/reports/visitReportsAction';
import { TechSpecClearSearch } from '../../../../grm/actions/techSpec/techSpecSearchActions';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {
    return {
        customerData: state.rootReportReducer.customerList,
        contractList:state.rootReportReducer.customerContracts,
        projectList:state.rootReportReducer.contractProjects,
        selectedCompany:state.appLayoutReducer.selectedCompany,
        companyList:state.appLayoutReducer.userRoleCompanyList,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                FetchCustomerList,
                FetchKPIVisitTimesheetContract,
                FetchKPIVisitTimesheetProject,
                FetchKPICustomer,
                FetchCustomerContracts,
                FetchContractProjects,
                FetchApprovedUnApprovedVistCustomer,
                TechSpecClearSearch,
                ClearCustomerContracts,
                ClearContractProjects
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(visitTimesheetKPIReport);
