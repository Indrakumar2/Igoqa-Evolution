import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import  ApprovedVisitReport from './approvedVisitTimesheetReport';
import { withRouter } from 'react-router-dom';
import { FetchCustomerList,FetchCustomerContracts,FetchContractProjects,
    ClearCustomerContracts, ClearContractProjects,FetchApprovedUnApprovedVistCustomer, FetchApprovedUnApprovedVistContract } from '../../../../actions/reports/visitReportsAction';
import { GetSelectedContractNumber } from '../../customer/customerAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        customerData: state.rootReportReducer.customerList,
        contractList:state.rootReportReducer.customerContracts,
        projectList:state.rootReportReducer.contractProjects,
        companyList:state.appLayoutReducer.userRoleCompanyList,
        selectedCompany:state.appLayoutReducer.selectedCompany
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                FetchCustomerList,
                GetSelectedContractNumber,
                FetchCustomerContracts,
                FetchApprovedUnApprovedVistCustomer,
                FetchApprovedUnApprovedVistContract,
                FetchContractProjects,
                ClearCustomerContracts,
                ClearContractProjects
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ApprovedVisitReport));
