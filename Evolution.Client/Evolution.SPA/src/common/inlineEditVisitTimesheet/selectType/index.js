import SelectType from './selectType';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import { isEmptyReturnDefault, isEmpty } from '../../../utils/commonUtils';
import { 
    UpdateTechnicalSpecialistTime as VisitUpdateTechnicalSpecialistTime,
    UpdateTechnicalSpecialistExpense as VisitUpdateTechnicalSpecialistExpense,
    UpdateTechnicalSpecialistTravel as VisitUpdateTechnicalSpecialistTravel,
    UpdateTechnicalSpecialistConsumable as VisitUpdateTechnicalSpecialistConsumable
} from '../../../actions/visit/technicalSpcialistAction';
import { 
    UpdateTimesheetTechnicalSpecialistTime,
    UpdateTimesheetTechnicalSpecialistExpense,
    UpdateTimesheetTechnicalSpecialistTravel,
    UpdateTimesheetTechnicalSpecialistConsumable,
    FetchCurrencyExchangeRate
} from '../../../actions/timesheet/timesheetTechSpecsAccountsAction';

const mapStateToProps = (state) => {
    return {
        VisitTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes),
        VisitTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels),
        VisitTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses),
        VisitTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables),
        timesheetTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes),
        timesheetTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels),
        timesheetTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses),
        timesheetTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables),
        TimeChargeType: isEmpty(state.appLayoutReducer.chargeTypes) ? isEmptyReturnDefault(state.appLayoutReducer.chargeTypes) 
                            : state.appLayoutReducer.chargeTypes.filter(expense =>expense.chargeType === 'R'),
        TravelChargeType: isEmpty(state.appLayoutReducer.chargeTypes) ? isEmptyReturnDefault(state.appLayoutReducer.chargeTypes) 
                            : state.appLayoutReducer.chargeTypes.filter(expense =>expense.chargeType === 'T'),
        ExpenseChargeType: isEmpty(state.appLayoutReducer.chargeTypes) ? isEmptyReturnDefault(state.appLayoutReducer.chargeTypes) 
                            : state.appLayoutReducer.chargeTypes.filter(expense =>expense.chargeType === 'E'),
        ConsumableChargeType: isEmpty(state.appLayoutReducer.chargeTypes) ? isEmptyReturnDefault(state.appLayoutReducer.chargeTypes) 
                            : state.appLayoutReducer.chargeTypes.filter(expense =>(expense.chargeType === 'C' || expense.chargeType === 'Q')), // ITK D-721 
        CurrencyMasterData: isEmptyReturnDefault(state.masterDataReducer.currencyMasterData), 
        //CurrencyMasterData: isEmpty(state.rootTimesheetReducer.timesheetDetail) ? isEmptyReturnDefault(state.rootVisitReducer.contractScheduleCurrency) :isEmptyReturnDefault(state.rootTimesheetReducer.contractScheduleCurrency), 
        TimesheetTimeChargeType: isEmpty(state.masterDataReducer.expenseType) ? isEmptyReturnDefault(state.masterDataReducer.expenseType) 
                            : state.masterDataReducer.expenseType.filter(expense =>expense.chargeType === 'R'),
        TimesheetTravelChargeType: isEmpty(state.masterDataReducer.expenseType) ? isEmptyReturnDefault(state.masterDataReducer.expenseType) 
                            : state.masterDataReducer.expenseType.filter(expense =>expense.chargeType === 'T'),
        TimesheetExpenseChargeType: isEmpty(state.masterDataReducer.expenseType) ? isEmptyReturnDefault(state.masterDataReducer.expenseType) 
                            : state.masterDataReducer.expenseType.filter(expense =>expense.chargeType === 'E'),
        TimesheetConsumableChargeType: isEmpty(state.masterDataReducer.expenseType) ? isEmptyReturnDefault(state.masterDataReducer.expenseType) 
                            : state.masterDataReducer.expenseType.filter(expense => (expense.chargeType === 'C' || expense.chargeType === 'Q')),  
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        timesheetInfo:isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object'),
        visitInfo:isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo,'object'),
        interactionMode: state.CommonReducer.interactionMode,
    };    
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                VisitUpdateTechnicalSpecialistTime,
                VisitUpdateTechnicalSpecialistExpense,
                VisitUpdateTechnicalSpecialistTravel,
                VisitUpdateTechnicalSpecialistConsumable,
                UpdateTimesheetTechnicalSpecialistTime,
                UpdateTimesheetTechnicalSpecialistExpense,
                UpdateTimesheetTechnicalSpecialistTravel,
                UpdateTimesheetTechnicalSpecialistConsumable,
                FetchCurrencyExchangeRate
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(SelectType);