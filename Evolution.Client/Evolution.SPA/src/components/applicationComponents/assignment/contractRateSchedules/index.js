import ContractRateSchedule from './contractRateSchedules';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { 
    AddNewRateSchedule,
    DeleteRateSchedule,
    UpdatetRateSchedule,
    FetchContractScheduleName,
    RateScheduleExpiryCheck 
} from '../../../../actions/assignment/contractRateScheduleActions';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { isOperator,isCoordinator,isInterCompanyAssignment,isOperatorCompany } from '../../../../selectors/assignmentSelector';

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    const contractHoldingCompanyCoordinators=isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators);

    // TODO: Remove unused props
    return {
        rateScheduleDataGrid:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentContractSchedules),
        loginUser:state.appLayoutReducer.loginUser,
        rateScheduleNames:state.rootAssignmentReducer.RateScheduleNames,
        assignmentId: state.rootAssignmentReducer.assignmentDetail.AssignmentInfo?
                      state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId:null,
        isOperatingCompany: isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
                    state.appLayoutReducer.selectedCompany),
        isOperator: isOperator(assignmentInfo.assignmentOperatingCompanyCoordinator,
                    operatingCompanyCoordinators,
                    state.appLayoutReducer.username),
        isCoordinator:isCoordinator(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
                    contractHoldingCompanyCoordinators,
                    state.appLayoutReducer.username),
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        pageMode:state.CommonReducer.currentPageMode,
        contractRates:state.rootAssignmentReducer.ContractRates,  //Added for Contract Rates Expired Validation
        assignmentTechSpecialists: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists)
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                AddNewRateSchedule,
                DeleteRateSchedule,
                UpdatetRateSchedule,
                FetchContractScheduleName,
                RateScheduleExpiryCheck  //Added for Contract Rates Expired Validation
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps,mapDispatchToProps)(ContractRateSchedule);