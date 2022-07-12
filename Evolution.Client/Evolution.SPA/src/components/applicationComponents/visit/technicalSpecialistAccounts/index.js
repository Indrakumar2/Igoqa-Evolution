import TechnicalSpecialistAccounts from './technicalSpecialistAccounts';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { 
    FetchTechnicalSpecialist,
    FetchVisitTechnicalSpecialists,
    FetchTechSpecRateSchedules,
    FetchTechnicalSpecialistTime,
    FetchTechnicalSpecialistTravel,
    FetchTechnicalSpecialistExpense,
    FetchTechnicalSpecialistConsumable,
    AddTechnicalSpecialistTime,
    UpdateTechnicalSpecialistTime,
    DeleteTechnicalSpecialistTime,
    AddTechnicalSpecialistTravel,
    UpdateTechnicalSpecialistTravel,
    DeleteTechnicalSpecialistTravel,
    AddTechnicalSpecialistExpense,
    UpdateTechnicalSpecialistExpense,
    DeleteTechnicalSpecialistExpense,
    AddTechnicalSpecialistConsumable,
    UpdateTechnicalSpecialistConsumable,
    DeleteTechnicalSpecialistConsumable,
    FetchTechSpecRateSchedulesDefault,
    FetchTechSpecRateDefault,
    UpdateTechSpecRateChanged,
    FetchCompanyExpectedMargin,
    SetLinkedAssignmentExpenses,
    UpdateVisitStatusByLineItems,
    UpdateExpenseOpen
} from '../../../../actions/visit/technicalSpcialistAction';
import { 
    FetchCurrencyExchangeRate
} from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction';
import {
    UpdateShowAllRates,
    UpdateVisitExchangeRates
} from '../../../../actions/visit/visitAction';
import { FetchChargeTypes } from '../../../appLayout/appLayoutActions';
import {
    DisplayModal,
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';
import { 
    isOperator,
    isCoordinator,
    isOperatorCompany,
    isCoordinatorCompany
} from '../../../../selectors/timesheetSelector';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';
import { activitycode } from '../../../../constants/securityConstant';
import {
    isCompanyOperatorCoordinator
} from '../../../../selectors/visitSelector';

const mapStateToProps = (state) => { 
    const visitInfo =isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo);
    const currentPage = state.CommonReducer.currentPage;
    return {
        techSpecAccountData:[ {
            technicalSpecialist:'Miles,Roger',
            grossMargin:64.95
        } ],
        
        visitInfo: visitInfo,
        expenseTypes: isEmptyReturnDefault(state.appLayoutReducer.chargeTypes),
        techSpecRateSchedules: isEmptyReturnDefault(state.rootVisitReducer.techSpecRateSchedules),
        VisitTechnicalSpecialists: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists),
        VisitTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes),
        VisitTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels),
        VisitTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses),
        VisitTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables),
       // currencyMasterData: isEmptyReturnDefault(state.masterDataReducer.currencyMasterData),
        currencyMasterData: state.rootVisitReducer.contractScheduleCurrency,
        currentPage: currentPage, 
        defaultTechSpecRateSchedules: isEmptyReturnDefault(state.rootVisitReducer.defaultTechSpecRateSchedules),
        visitTechnicalSpecialistsGrossMargin: state.rootVisitReducer.visitTechnicalSpecialistsGrossMargin,        
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        companyBusinessUnitExpectedMargin:state.rootVisitReducer.companyBusinessUnitExpectedMargin,
        unLinkedAssignmentExpenses: isEmptyReturnDefault(state.rootVisitReducer.assignmentUnLinkedExpenses),
        isInterCompanyAssignment: isInterCompanyAssignment(visitInfo.visitContractCompanyCode,
            visitInfo.visitOperatingCompanyCode),
        isOperator:isOperator(visitInfo.visitOperatingCompanyCoordinatorCode,state.appLayoutReducer.username),
        isCoordinator:isCoordinator(visitInfo.visitContractCoordinatorCode,state.appLayoutReducer.username),
        isShowAllRates:state.rootVisitReducer.isShowAllRates,
        pageMode:state.CommonReducer.currentPageMode,        
        chargeRateReadonly: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? true 
                                : !state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VISIT_CHARTE_RATE_VIEW).length > 0),
        payRateReadonly: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? true 
                                : !state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VISIT_PAY_RATE_VIEW).length > 0),
        modifyPayUnitPayRate: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VISIT_MODIFY_PAYUNIT_PAYRATE).length > 0),
        modifyAddApprovedLines: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VISIT_MODIFY_ADD_APPROVED_LINES).length > 0),
        selectedCompany: state.appLayoutReducer.selectedCompany,
        isOperatingCompany: isOperatorCompany(visitInfo.visitOperatingCompanyCode, state.appLayoutReducer.selectedCompany),
        isCoordinatorCompany:isCoordinatorCompany(visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany),
        isExpenseOpen: state.rootVisitReducer.isExpenseOpen,
        visitExchangeRates: state.rootVisitReducer.visitExchangeRates,
        notLoggedinCompany: !isCompanyOperatorCoordinator(visitInfo.visitOperatingCompanyCode,visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchTechnicalSpecialist,
                FetchVisitTechnicalSpecialists,
                FetchTechSpecRateSchedules,
                FetchTechnicalSpecialistTime,
                FetchTechnicalSpecialistTravel,
                FetchTechnicalSpecialistExpense,
                FetchTechnicalSpecialistConsumable,
                FetchChargeTypes,
                AddTechnicalSpecialistTime,
                UpdateTechnicalSpecialistTime,
                DeleteTechnicalSpecialistTime,
                AddTechnicalSpecialistTravel,
                UpdateTechnicalSpecialistTravel,
                DeleteTechnicalSpecialistTravel,
                AddTechnicalSpecialistExpense,
                UpdateTechnicalSpecialistExpense,
                DeleteTechnicalSpecialistExpense,
                AddTechnicalSpecialistConsumable,
                UpdateTechnicalSpecialistConsumable,
                DeleteTechnicalSpecialistConsumable,
                FetchTechSpecRateSchedulesDefault,
                FetchTechSpecRateDefault,
                DisplayModal,
                HideModal,
                FetchCompanyExpectedMargin,
                UpdateTechSpecRateChanged,
                FetchCurrencyExchangeRate,
                SetLinkedAssignmentExpenses,
                UpdateShowAllRates,
                UpdateVisitStatusByLineItems,
                UpdateExpenseOpen,
                UpdateVisitExchangeRates
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TechnicalSpecialistAccounts));