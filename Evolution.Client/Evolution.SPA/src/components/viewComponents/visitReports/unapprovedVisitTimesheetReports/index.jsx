import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import  unapprovedVisitTimesheetReport from './unapprovedVisitTimesheetReport';
import  { FetchProjectCoordinator,FetchReportCoordinator }from '../../../../actions/project/generalDetailsAction';
import { FetchCustomerBasedOnCoordinators,
     FetchCustomerContracts, FetchContractProjects, ClearCustomerList,ClearCustomerContracts,ClearContractProjects,
     FetchApprovedUnApprovedVistCustomer,FetchApprovedUnApprovedVistContract, FetchUnApprovedVistContract,FetchUnApprovedVistCustomer
     } from '../../../../actions/reports/visitReportsAction';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {
    return {
        coordinatorList: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectCoordinator),
        selectedCompany:state.appLayoutReducer.selectedCompany,
        companyList:state.appLayoutReducer.userRoleCompanyList,
        customerList: state.rootReportReducer.coordinatorCustomerList,
        contractList:state.rootReportReducer.customerContracts,
        projectList:state.rootReportReducer.contractProjects,
        
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                DisplayModal,
                HideModal,
                FetchProjectCoordinator,
                FetchReportCoordinator,
                FetchCustomerBasedOnCoordinators,
                FetchApprovedUnApprovedVistCustomer,
                FetchApprovedUnApprovedVistContract,
                FetchUnApprovedVistContract,
                FetchUnApprovedVistCustomer,
                FetchCustomerContracts,
                FetchContractProjects,
                ClearCustomerList,
                ClearCustomerContracts,
                ClearContractProjects
             },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(unapprovedVisitTimesheetReport));
