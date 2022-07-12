import { createSelector } from 'reselect';
import { getlocalizeData, isEmptyReturnDefault } from '../utils/commonUtils';
const localConstant = getlocalizeData();

const VisitId = state => state.visitId;
const VisitStatus = state => state.visitStatus;
const VisitMode = state => state.currentPage;
const TimeLineItems = state => state.VisitTechnicalSpecialistTimes;
const ExpenseLineItems = state => state.VisitTechnicalSpecialistExpenses;
const TravelLineItems = state => state.VisitTechnicalSpecialistTravels;
const ConsumableLineItems = state => state.VisitTechnicalSpecialistConsumables;

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

export const isOperatorCompany = createSelector(
    isEqual,
    (isOperatorCompany) => {
        return isOperatorCompany;
    }
);

export const isCoordinator = createSelector(
    isEqual,
    (isCoordinator) => {
        return isCoordinator;
    }
);

export const isCoordinatorCompany = createSelector(
    isEqual,
    (isCoordinatorCompany) => {
        return isCoordinatorCompany;
    }
);

export const getVisitId = createSelector(
    [ VisitId ],
    (id) => {
        return id?id:null;
    }
);

export const getVisitStatus = createSelector(
    [ VisitStatus, VisitMode ],
    (status, visitMode) => {
       // console.log("status = ",status, "visitMode = ", visitMode);
        if(!status){
            status = 'N';
        }
        const visitStatus = localConstant.commonConstants.visitStatus.filter(x=>x.value === status);
        return visitStatus.length ===0?{}:visitStatus[0];
    }
);
 
export const isVisitCreateMode = createSelector(
    [ (state)=>state ], (visitMode) => {
        return visitMode === localConstant.visit.CREATE_TIMESHEET_MODE;
    }
);

export const lineitemsExists = createSelector([ TimeLineItems, ExpenseLineItems, TravelLineItems, ConsumableLineItems ],
    (time, expense, travel, consumable) => {
        if ((Array.isArray(time) && time.filter(x => x.recordStatus !== "D").length > 0) 
            || (Array.isArray(expense) && expense.filter(x => x.recordStatus !== "D").length > 0) 
            || (Array.isArray(travel) && travel.filter(x => x.recordStatus !== "D").length > 0) 
            || (Array.isArray(consumable) && consumable.filter(x => x.recordStatus !== "D").length > 0)) {
            return true;
        }
        return false;
});

export const isOperatorApporved = createSelector([ VisitStatus ],
   
    (status) => {        
        return (isOperatorCompany && status === 'O' || isOperatorCompany && status === 'A' ? true : false);
    }
    
);

export const isCompanyOperatorCoordinator = createSelector(
    isEqualThree,
    (isCompanyOperatorCoordinator) => {
        return isCompanyOperatorCoordinator;
    }
);