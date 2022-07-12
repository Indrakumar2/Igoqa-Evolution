import { createSelector } from 'reselect';
import { isUndefined, isEmptyOrUndefine, isEmptyReturnDefault } from '../utils/commonUtils';
import sessionStorage from 'redux-persist/es/storage/session';
import { applicationConstants } from '../constants/appConstants';

export const verifiedStatusDisable= createSelector (
    [ (state) => state.obj ],
    (obj) => {
        if(obj.verificationStatus === "Verified" && isUndefined(obj.IsSaved) && !obj.IsSaved){
            return true;
        } else {
            return false;
        }
    }
);

export const isUserTypeCheck=createSelector(
    [ (state) => state.array, (state) => state.param ],
    (array,param)=>{
        return array.includes(param);
    }
);

export const userTypeCheck=createSelector([ isUserTypeCheck ], (flag)=>{
    return flag;   
}   
);
 
export const userTypeGet = ()=>{
     const sessionStorageValue = sessionStorage.getItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE).value; //957 (Edge&IE - 404 issue fixes)
     if(isUndefined(sessionStorageValue)){
        return isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
     } else {
        return !isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE)) ? sessionStorage.getItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE) : isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
     }
}; 
/**
 * Expense Type Filter based on PayReference
 * Created For ITK Defect 1089 (Ref ALM Doc 22-05-2020)
 */
export const isNotNDTExpenseType=createSelector(
    [ (state) => state.array, (state) => state.param ],
    (array,param)=>{
        const filteredArray=[];
        array &&  array.forEach(x => {
            if(!isEmptyOrUndefine(x.payReference) && !x.payReference.startsWith(param)){
                filteredArray.push(x);
            }
        });
        return filteredArray;
    }
);

export const filterExpenseType=createSelector([ isNotNDTExpenseType ], (value)=>{
    return value;   
}   
);