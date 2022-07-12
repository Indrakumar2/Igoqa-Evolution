import TechnicalSpecialistAccounts from './technicalSpecialistAccounts';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import {
    FetchTechSpecRateSchedules,
    FetchTimesheetTechnicalSpecialists,
    FetchTimesheetTechnicalSpecialistTime,
    FetchTimesheetTechnicalSpecialistTravel,
    FetchTimesheetTechnicalSpecialistExpense,
    FetchTimesheetTechnicalSpecialistConsumable,
    AddTimesheetTechnicalSpecialistTime,
    UpdateTimesheetTechnicalSpecialistTime,
    DeleteTimesheetTechnicalSpecialistTime,
    AddTimesheetTechnicalSpecialistTravel,
    UpdateTimesheetTechnicalSpecialistTravel,
    DeleteTimesheetTechnicalSpecialistTravel,
    AddTimesheetTechnicalSpecialistExpense,
    UpdateTimesheetTechnicalSpecialistExpense,
    DeleteTimesheetTechnicalSpecialistExpense,
    AddTimesheetTechnicalSpecialistConsumable,
    UpdateTimesheetTechnicalSpecialistConsumable,
    DeleteTimesheetTechnicalSpecialistConsumable,
    FetchTechSpecRateDefault,
    SetLinkedAssignmentExpenses,
    FetchCompanyExpectedMargin,
    FetchCurrencyExchangeRate,
    UpdateExpenseOpen
} from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction';
import { FetchExpenseType } from '../../../../common/masterData/masterDataActions';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';
import { 
    isOperator,
    isCoordinator,
    isTimesheetCreateMode,
    isOperatorCompany,
    isCoordinatorCompany
} from '../../../../selectors/timesheetSelector';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';
import {
    UpdateShowAllRates,
    UpdateTimesheetExchangeRates
} from '../../../../actions/timesheet/timesheetAction';
import { activitycode } from '../../../../constants/securityConstant';
 
const mapStateToProps = (state) => { 
    const timesheetInfo= isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object');
    const currentPage = state.CommonReducer.currentPage;
    return {
        timesheetInfo: timesheetInfo,
        techSpecRateSchedules:isEmptyReturnDefault(state.rootTimesheetReducer.techSpecRateSchedules,'object'),
        expenseTypes:isEmptyReturnDefault(state.masterDataReducer.expenseType),
        timesheetTechnicalSpecialistsGrossMargin: state.rootTimesheetReducer.timesheetTechnicalSpecialistsGrossMargin,
        timesheetTechnicalSpecilists: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialists).filter(ts=>ts.recordStatus !== 'D'),
        timesheetId: timesheetInfo.timesheetId ? timesheetInfo.timesheetId : null,
        timesheetTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes),
        timesheetTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels),
        timesheetTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses),
        timesheetTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables),
        currentPage: currentPage,
        //currencyMasterData: isEmptyReturnDefault(state.masterDataReducer.currencyMasterData),
        currencyMasterData: isEmptyReturnDefault(state.rootTimesheetReducer.contractScheduleCurrency),
        isInterCompanyAssignment: isInterCompanyAssignment(timesheetInfo.timesheetContractCompanyCode,
            timesheetInfo.timesheetOperatingCompanyCode),
        isOperator:isOperator(timesheetInfo.timesheetOperatingCoordinatorCode,state.appLayoutReducer.username),
        isCoordinator:isCoordinator(timesheetInfo.timesheetContractCoordinatorCode,state.appLayoutReducer.username),
        isTimesheetCreateMode:isTimesheetCreateMode(currentPage),
        assignmentExpenses:  isEmptyReturnDefault(state.rootAssignmentReducer.assignmentAdditionalExpenses),
        unLinkedAssignmentExpenses: isEmptyReturnDefault(state.rootTimesheetReducer.assignmentUnLinkedExpenses),
        companyBusinessUnitExpectedMargin:state.rootTimesheetReducer.companyBusinessUnitExpectedMargin,
        isShowAllRates:state.rootTimesheetReducer.isShowAllRates,
        pageMode:state.CommonReducer.currentPageMode,        
        chargeRateReadonly: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? true 
                                : !state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.TIMESHEET_CHARTE_RATE_VIEW).length > 0),
        payRateReadonly: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? true 
                                : !state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.TIMESHEET_PAY_RATE_VIEW).length > 0),
        modifyPayUnitPayRate: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                    : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.TIMESHEET_MODIFY_PAYUNIT_PAYRATE).length > 0),
        modifyAddApprovedLines: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                    : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.TIMESHEET_MODIFY_ADD_APPROVED_LINES).length > 0),
        selectedCompany: state.appLayoutReducer.selectedCompany,
        isOperatingCompany: isOperatorCompany(timesheetInfo.timesheetOperatingCompanyCode, state.appLayoutReducer.selectedCompany),
        isCoordinatorCompany:isCoordinatorCompany(timesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany),
        isExpenseOpen: state.rootTimesheetReducer.isExpenseOpen,
        timesheetExchangeRates: state.rootTimesheetReducer.timesheetExchangeRates
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchTechSpecRateSchedules,
                FetchTimesheetTechnicalSpecialists,
                FetchExpenseType,
                FetchTimesheetTechnicalSpecialistTime,
                FetchTimesheetTechnicalSpecialistTravel,
                FetchTimesheetTechnicalSpecialistExpense,
                FetchTimesheetTechnicalSpecialistConsumable,
                AddTimesheetTechnicalSpecialistTime,
                UpdateTimesheetTechnicalSpecialistTime,
                DeleteTimesheetTechnicalSpecialistTime,
                AddTimesheetTechnicalSpecialistTravel,
                UpdateTimesheetTechnicalSpecialistTravel,
                DeleteTimesheetTechnicalSpecialistTravel,
                AddTimesheetTechnicalSpecialistExpense,
                UpdateTimesheetTechnicalSpecialistExpense,
                DeleteTimesheetTechnicalSpecialistExpense,
                AddTimesheetTechnicalSpecialistConsumable,
                UpdateTimesheetTechnicalSpecialistConsumable,
                DeleteTimesheetTechnicalSpecialistConsumable,
                FetchTechSpecRateDefault,
                DisplayModal, 
                HideModal,
                SetLinkedAssignmentExpenses,
                FetchCompanyExpectedMargin,
                FetchCurrencyExchangeRate,
                UpdateShowAllRates,
                UpdateExpenseOpen,
                UpdateTimesheetExchangeRates
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(TechnicalSpecialistAccounts));
