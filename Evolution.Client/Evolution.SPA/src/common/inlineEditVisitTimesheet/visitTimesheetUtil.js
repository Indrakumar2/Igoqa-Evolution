import { 
    numberFormat,
    isEmptyOrUndefine,
    isEmptyReturnDefault
} from '../../utils/commonUtils';

export const FormatTwoDecimal = (value) => {
    if(value === "" || value === null || value === undefined) {
        return "";
    } else {
        if(isNaN(value)) {
            return parseFloat(numberFormat(value)).toFixed(2);
        } else {
            return parseFloat(value).toFixed(2);
        }
    }
};

export const FormatSixDecimal = (value) => {
    if(value === "") {
        return "";
    } else if(value === null || value === undefined) {
        return "0.000000";
    } else {
        if(isNaN(value)) {
            return parseFloat(numberFormat(value)).toFixed(6);
        } else {
            return parseFloat(value).toFixed(6);
        }
    }
};

export const FormatFourDecimal = (value) => {
    if(value === "") {
        return "";
    } else if(value === null || value === undefined) {
        return "0.0000";
    } else {
        if(isNaN(value)) {
            return parseFloat(numberFormat(value)).toFixed(4);
        } else {
            return parseFloat(value).toFixed(4);
        }
    }
};

export const getTravelDefaultPayType = (pin, techSpecRateSchedules, expenseTypes)  => {
    const payRateSchedule = isEmptyReturnDefault(techSpecRateSchedules);
    let chargeType, payType = '';
    if(payRateSchedule.chargeSchedules) {
        const chargeSchedule =  payRateSchedule.chargeSchedules.filter(x => x.epin === pin);
        if(chargeSchedule && chargeSchedule.length > 0) {
            const chargeScheduleRates = chargeSchedule[0].chargeScheduleRates.filter(x => x.type === 'T' && x.isActive);
            if(chargeScheduleRates && chargeScheduleRates.length > 0) {
                chargeType = chargeScheduleRates[0].chargeType;
            }
        }
    }
    if(payRateSchedule.paySchedules) {
        const paySchedule =  payRateSchedule.paySchedules.filter(x => x.epin === pin);
        if(paySchedule && paySchedule.length > 0) {
            const payScheduleRates = paySchedule[0].payScheduleRates.filter(x => x.type === 'T' && x.isActive);
            if(payScheduleRates && payScheduleRates.length > 0) {
                payType = payScheduleRates[0].expenseType;
            }
        }
    }
    if(isEmptyOrUndefine(chargeType) && isEmptyOrUndefine(payType)) {
        const travelExpenseType = expenseTypes.filter(expense =>expense.chargeType === 'T');
        if(travelExpenseType && travelExpenseType.length > 0) {
            chargeType = payType = travelExpenseType[0].name;
        }
    } else if(isEmptyOrUndefine(chargeType)) {
        chargeType = payType;
    } else if(isEmptyOrUndefine(payType)) {
        payType = chargeType;
    }

    return { "chargeType": chargeType, "payType": payType };
};