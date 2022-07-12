import InterCompanyDiscounts from './interCompanyDiscounts';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { 
    UpdateICDiscounts
 } from '../../../../actions/assignment/assignmentICDiscountsAction';
import { FetchChildContractsOfPrarent } from '../../../../actions/contracts/childContractAction';
import { FetchContractData, FetchContractDataForAssignment } from '../../../../actions/contracts/contractAction';
import { isEmptyReturnDefault,getlocalizeData } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { 
    AddRevenueData, 
    UpdateRevenueData, 
    DeleteRevenueData,
    AddCostData,
    UpdateCostData,
    DeleteCostData,
    AddDefaultContributionData,
    UpdateContributionCalculator,
    resetContributionCalculator,
    savedContributionCalculatorChanges,
 } from '../../../../actions/assignment/contributionCalculatorActions';
import { 
    isOperator,
    isCoordinator,
    isOperatorCompany,
    isContractHolderCompany,
    isInterCompanyAssignment,
    isTimesheet,
    isVisit,
    isSettlingTypeMargin,
    isSettlingTypeCost } from '../../../../selectors/assignmentSelector';

const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const contractHoldingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators),
        operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    const businessUnits = {
        projectReportParams: isEmptyReturnDefault(state.masterDataReducer.businessUnit),
        businessUnit: assignmentInfo.assignmentProjectBusinessUnit
    };
    return {
        interCompanyDiscounts: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInterCompanyDiscounts, 'object'),
        isInterCompanyDiscountChanged : state.rootAssignmentReducer.isInterCompanyDiscountChanged,
        companyList: state.appLayoutReducer.companyList,
        currentPage: state.CommonReducer.currentPage,
        userDefaultCompany: state.loginReducer.userDetails.ccode,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        loggedInUser: state.appLayoutReducer.loginUser,
        assignmentInfo: assignmentInfo,
        ContributionCalculator: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentContributionCalculators),
        projectReportParams: state.masterDataReducer.businessUnit,
        isContrinutionCalculatorModified: state.rootAssignmentReducer.isContrinutionCalculatorModified,
        //contractHoldingCompanyCoordinators: state.rootAssignmentReducer.contractHoldingCompanyCoordinators,
        //operatingCompanyCoordinators: state.rootAssignmentReducer.operatingCompanyCoordinators,
        username: state.appLayoutReducer.username,
        isOperator: isOperator(assignmentInfo.assignmentOperatingCompanyCoordinator,
            operatingCompanyCoordinators,
            state.appLayoutReducer.username),
        isCoordinator: isCoordinator(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
            contractHoldingCompanyCoordinators,
            state.appLayoutReducer.username),
        pageMode:state.CommonReducer.currentPageMode,
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
                state.appLayoutReducer.selectedCompany),
        isContractHolderCompany: isContractHolderCompany(assignmentInfo.assignmentContractHoldingCompanyCode,
                state.appLayoutReducer.selectedCompany),
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        isTimesheetAssignment: isTimesheet(workflowType),
        isVisitAssignment: isVisit(workflowType),
        isSettlingTypeMargin: isSettlingTypeMargin(businessUnits),
        isSettlingTypeCost:isSettlingTypeCost(businessUnits),
        isContractType :  state.RootContractReducer.ContractDetailReducer.childContractsOfParent,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                UpdateICDiscounts,
                AddRevenueData,
                UpdateRevenueData,
                DeleteRevenueData,
                DisplayModal,
                HideModal,
                AddCostData,
                UpdateCostData,
                FetchContractData,
                FetchContractDataForAssignment,
                DeleteCostData,
                AddDefaultContributionData,
                UpdateContributionCalculator,
                resetContributionCalculator,
                savedContributionCalculatorChanges,
                FetchChildContractsOfPrarent
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(InterCompanyDiscounts);