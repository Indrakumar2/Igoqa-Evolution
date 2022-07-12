import { applicationConstants } from '../constants/appConstants';
import arrayUtil from '../utils/arrayUtil';
import { isEmptyOrUndefine } from './commonUtils';
export const sortCurrency = (currencyData) => {
    if(currencyData)
    {
        const selectedCompanyCurrency = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.COMPANY_CURRENCY))
                                            ? sessionStorage.getItem(applicationConstants.Authentication.COMPANY_CURRENCY)
                                            : localStorage.getItem(applicationConstants.Authentication.COMPANY_CURRENCY));
        const currencyWithoutSelectedCompCurrency = arrayUtil.sort(currencyData.filter(x=> x.code !== selectedCompanyCurrency),'code','asc');
        const filteredCurrency =  currencyData.filter(x=> x.code === selectedCompanyCurrency);
        const result= [ ...filteredCurrency, ...currencyWithoutSelectedCompCurrency ];
        return result;
    }
    else{
        return [];
    }
};