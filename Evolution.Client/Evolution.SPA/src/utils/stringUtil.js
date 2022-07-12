import { numberFormat } from './commonUtils';

export const StringFormat = (...params) => {
    let str = '';
    if (params.length) {
        str = params[0];
        let key;
        params.shift();
        for (key in params) {
            str = str.replace(new RegExp("\\{" + key + "\\}", "gi"), params[key]);
        }
    }
    return str;
};

export const capitalize = (s) => {
    if (typeof s !== 'string')
        return '';

    const splitStr = s.toLowerCase().split(' ');
    for (let i = 0; i < splitStr.length; i++) {
        splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].slice(1).toLowerCase();
    }
    return splitStr.join(' ');
};

export const ReplaceString = (str, replaceFrom, replaceTo) => {
    if (str.length <= 0)
        return '';

    const expression = "\\" + replaceFrom;
    const regExp = new RegExp(expression, "g");
    return str.replace(regExp, replaceTo);
};
function escapeRegExp(str) {
    return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
export function replaceAll(str, find, replace) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
export function FormatTwoDecimal(value) {
    if(value === null || value === undefined || value === "") {
        return "";
    } else {
        if(isNaN(value)) {
            return parseFloat(numberFormat(value)).toFixed(2);
        } else {
            return parseFloat(value).toFixed(2);
        }
    }
}
export function FormatFourDecimal(value) {
    if(value === null || value === undefined || value === "") {
        return "";
    } else {
        if(isNaN(value)) {
            return parseFloat(numberFormat(value)).toFixed(4);
        } else {
            return parseFloat(value).toFixed(4);
        }
    }
}

export function DecimalWithLimitFormat (value,wholeNoLimit=10,decimalLimit=4) {   
    const expression = ("(\\d{0,"+ parseInt(wholeNoLimit)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(decimalLimit) +"})?)"); //def 897, 1016 db synch overflow issue
    const rg = new RegExp(expression,"g"); 
    const match = rg.exec(value.replace(/[^\d.]/g, ''));
    return match[1] + match[2];  
};
