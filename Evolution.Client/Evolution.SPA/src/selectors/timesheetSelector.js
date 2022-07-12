import { createSelector } from 'reselect';
import { getlocalizeData, isEmptyReturnDefault,isEmptyOrUndefine } from '../utils/commonUtils';
const localConstant = getlocalizeData();
const TimesheetId = state => state.timesheetId;
const TimesheetStatus = state => state.timesheetStatus;
const TimesheetMode = state => state.currentPage;
const TimeLineItems = state => state.TimesheetTechnicalSpecialistTimes;
const ExpenseLineItems = state => state.TimesheetTechnicalSpecialistExpenses;
const TravelLineItems = state => state.TimesheetTechnicalSpecialistTravels;
const ConsumableLineItems = state => state.TimesheetTechnicalSpecialistConsumables;

function isEqual(value1,value2){
    return value1 == value2;
}

function isEqualThree(value1,value2, value3){
    return (value1 == value3 || value2 == value3);
}

export const isOperator = createSelector(
    isEqual,
    (isOperator) => {
        return isOperator;
    }
);

export const isCoordinator = createSelector(
    isEqual,
    (isCoordinator) => {
        return isCoordinator;
    }
);

export const getTimesheetId = createSelector(
    [ TimesheetId ],
    (id) => {
        return id?id:null;
    }
);

export const getTimesheetStatus = createSelector(
    [ TimesheetStatus, TimesheetMode ],
    (status, timesheetMode) => {
       // console.log("status = ",status, "timesheetMode = ", timesheetMode);timesheetMode === localConstant.timesheet.CREATE_TIMESHEET_MODE && 
        if(!status){
            status = 'N';
        }
        const timesheetStatus = localConstant.commonConstants.timesheet_Status.filter(x=>x.value === status);
        return timesheetStatus.length ===0?{}:timesheetStatus[0];
    }
);
 
export const isTimesheetCreateMode = createSelector(
    [ (state)=>state ], (timesheetMode) => {
        return timesheetMode === localConstant.timesheet.CREATE_TIMESHEET_MODE;
    }
);

function bubbleSort(array) {
    let done = false;
    while (!done) {
      done = true;
      for (let i = 1; i < array.length; i += 1) {
        if (array[i - 1].assignmentTechnicalSpecilaistId > array[i].assignmentTechnicalSpecilaistId) {
          done = false;
          array[i - 1] = [ array[i], array[i]=array[i - 1] ][0];
        }
      }
    }
    return array;
  }

export const GetScheduleDefaultCurrency = createSelector([ (state) => state.techSpecRateSchedules, (state) => state.pin ], (techSpecRateSchedules, pin) => {

    let chargeSchedules = [];
    let paySchedules = [];
    const defaultCurrency = {
        defaultPayCurrency: null,
        defaultChargeCurrency: null,
    };
    try {
        if (!isEmptyOrUndefine(techSpecRateSchedules.paySchedules)) {
            if (techSpecRateSchedules.paySchedules.length === 1) {
                defaultCurrency.defaultPayCurrency = techSpecRateSchedules.paySchedules[0].payScheduleCurrency;
            } else {
                paySchedules = techSpecRateSchedules.paySchedules.filter(row => {
                    return (row.epin === pin);
                });
                paySchedules = bubbleSort(paySchedules);
                defaultCurrency.defaultPayCurrency = paySchedules[0].payScheduleCurrency;
            }
        }

        if (!isEmptyOrUndefine(techSpecRateSchedules.chargeSchedules)) {
            if (techSpecRateSchedules.chargeSchedules.length === 1) {
                defaultCurrency.defaultChargeCurrency = techSpecRateSchedules.chargeSchedules[0].chargeScheduleCurrency;
            } else {
                chargeSchedules = techSpecRateSchedules.chargeSchedules.filter(row => {
                    return (row.epin === pin);
                });
                chargeSchedules = bubbleSort(chargeSchedules);
                defaultCurrency.defaultChargeCurrency = chargeSchedules[0].chargeScheduleCurrency;
            }
        }
    }
    catch (e) {
        //check error
    }
    return defaultCurrency;
}); 

export const lineitemsExists = createSelector([ TimeLineItems, ExpenseLineItems, TravelLineItems, ConsumableLineItems ],
    (time, expense, travel, consumable) => {
        if ((Array.isArray(time) && time.length > 0) 
            || (Array.isArray(expense) && expense.length > 0) 
            || (Array.isArray(travel) && travel.length >0) 
            || (Array.isArray(consumable) && consumable.length > 0)) {
            return true;
        }
        return false;
});

export const isOperatorCompany = createSelector(
    isEqual,
    (isOperatorCompany) => {
        return isOperatorCompany;
    }
);
 
export const isCoordinatorCompany = createSelector(
    isEqual,
    (isCoordinatorCompany) => {
        return isCoordinatorCompany;
        
    }
);

export const isApprovedByCoordinator = createSelector([ TimesheetStatus ],
   
    (status) => { 
        return (isCoordinatorCompany && status === 'O' || isCoordinatorCompany && status === 'A' ? true : false);
    }    
);

export const isCompanyOperatorCoordinator = createSelector(
    isEqualThree,
    (isCompanyOperatorCoordinator) => {
        return isCompanyOperatorCoordinator;
    }
);