import {
    countryCode
} from '../constants/appConstants';
import {
    localeConstants_en
} from '../constants/locale_en';
import IntertekToaster from '../common/baseComponents/intertekToaster';
import _ from 'lodash';
import { required, requiredNumeric } from './validator';

export const getlocalizeData = () => {
    const selectedLanguage = 'EN';
    if (selectedLanguage === countryCode.ENGLISH) {
        return localeConstants_en;
    }
    return localeConstants_en;
};
export const fromPairs = function (obj) {
    return _.fromPairs(obj);
};
export const toPairs = function (obj) {
    return _.toPairs(obj);
};
export const uniq = function (data,key) {
    return _.uniqBy(data, key); 
};
export const arrayMap = function (arrayName, element) {
    return _.map(arrayName, element);
};
const getClass = function (val) {
    return Object.prototype.toString.call(val)
        .match(/^\[object\s(.*)\]$/)[1];
};

//Defines the type of the value, extended typeof
const whatis = function (val) {

    if (val === undefined) {
        return 'undefined';
    }
    if (val === null) {
        return 'null';
    }

    let type = typeof val;

    if (type === 'object') {
        type = getClass(val)
            .toLowerCase();
    }

    if (type === 'number') {
        if (val.toString()
            .indexOf('.') > 0) {
            return 'float';
        } else {
            return 'integer';
        }
    }

    return type;
};

export const isKey = function (obj, key) {
    return _.has(obj, key);
};

export const isEmpty = function (obj) {
    if (obj === null) return true;
    if (_.isArray(obj) || _.isString(obj)) return obj.length === 0;
    for (const key in obj)
        if (_.has(obj, key)) return false;
    return true;
};

export const isEmptyOrUndefine = function (obj) {
    if (obj === 'undefined' || obj === null || obj === "" || obj === " " || isEmpty(obj))
        return true;

    if (_.isArray(obj) || _.isString(obj))
        return obj.length === 0;

    for (const key in obj)
        if (_.has(obj, key)) return false;

    return true;
};

//method returns default value of type specified
//in case type is not specified, array is considered as default
export const isEmptyReturnDefault = function (data, type) {
    type = type ? type : 'array';
    const result = isEmpty(data) || isUndefined(data);
    if (result && type === 'number') {
        return 0;
    }
    if (result && type === 'boolean') {
        return false;
    }
    if (result && type === 'array') {
        return [];
    }
    if (result && type === 'object') {
        return {};
    }

    return data;
};

// Returns if a value is a function
export const isFunction = function (value) {
    return typeof value === 'function';
};

export const isString = function (value) {
    return typeof value === 'string';
};

export const isUndefined = function (value) {
    return typeof value === 'undefined';
};

export const mergeobjects = (dest, src) => {
    return _.merge({}, dest, src);
};

export const truncate = (text, length) => {
    return _.truncate(text, {
        length: length ? length : 50,
        separator: ' '
    });
};
export const compareObjects = function (a, b) {
    if (a === b) {
        return true;
    }
    for (const i in a) {
        if (b.hasOwnProperty(i)) {
            if (!equal(a[i], b[i])) return false;
        } else {
            return false;
        }
    }

    for (const i in b) {
        if (!a.hasOwnProperty(i)) {
            return false;
        }
    }
    return true;
};

const compareArrays = function (a, b) {
    if (a === b) {
        return true;
    }
    if (a.length !== b.length) {
        return false;
    }
    for (let i = 0; i < a.length; i++) {
        if (!equal(a[i], b[i])) return false;
    }

    return true;
};

const _equal = {};
_equal.array = compareArrays;
_equal.object = compareObjects;
_equal.date = function (a, b) {
    return a.getTime() === b.getTime();
};
_equal.regexp = function (a, b) {
    return a.toString() === b.toString();
};

export const equal = function (a, b) {
    if (a !== b) {
        const atype = whatis(a),
            btype = whatis(b);

        if (atype === btype) {
            return _equal.hasOwnProperty(atype) ? _equal[atype](a, b) : a === b;
        }

        return false;
    }

    return true;
};
export const bindAction = (data, columnName, action) => {
    data.columnDefs.filter((iteratedValue) => {       
        if (iteratedValue.field && iteratedValue.field === columnName) {          
            return iteratedValue.cellRendererParams.editAction = action;
        }
    });
};
export const bindActionWithChildLevel = (data, columnName, action) => {   
          data.map(d=>{
          if(d.children!==undefined){
            d.children.filter((iteratedValue) => {       
                if (iteratedValue.field && iteratedValue.field === columnName) {        
                    return iteratedValue.cellRendererParams.editAction = action;
                }
            });
          }
      });
    
};

export const bindProperty = (data, columnName, property, value) => {
    data.columnDefs.filter((iteratedValue) => {       
        if (iteratedValue.field && iteratedValue.field === columnName) {          
            return iteratedValue[property] = value;
        }
    });
};

export const numberFormat = (number) => {
    if (parseInt(number) === 0) {
        return number;
    }
    else {
        const trimmed = number.replace(/^0+/, '');
        const formatedNumber = trimmed.replace(/[^\d.-]/g, '');
        return formatedNumber;
    }
};

//Thousand Seperator for Values but not for decimal part
export function thousandFormat(number) {
    let formatedNumber = number.toString();
    if(parseInt(number) === 0)
        return number; 
    else {
        while(true){
            const commaFormat = formatedNumber.replace(/\B(?=(\d{3})+(?!\d))/g, ",");//To add thousand seperator
            if (formatedNumber == commaFormat) break;
            formatedNumber = commaFormat;
        }
        if(formatedNumber.split('.').length>1) {
            formatedNumber = `${ formatedNumber.split('.')[0] }.${ formatedNumber.split('.')[1].replace(/,/g,"") }`;// To remove comma from decimal part
            return formatedNumber;
        }
        else 
            return formatedNumber;
    }
};

//Number format for Mobile Number
//Decimal values are not considered
export const mobileNumberFormat = (number) => {
    if (number === "0") {
        return number;
    }
    else {
        const trimmed = number.replace(/^0+/, '');
        const formatedNumber = trimmed.replace(/[^\d-]/g, '');
        return formatedNumber;
    }
};

export const isValidEmailAddress = (email) => {
    if (isEmptyOrUndefine(email)) {
        return false;
    }
    return email.match(/^([\w.%+-]+)@([\w-]+\.)+([\w]{2,})$/i);
};
//Remove Array Duplicates
export const RemoveDuplicateArray = (params,key) => {
    const isObj = _.unionBy(params,key);
    return isObj;
};
//Is Array 
export const isArray = (params) => {
    const isArr = _.isArray(params);
    return isArr;
};

//Find Object in Array
export const findObject = (array, key) => {
    const obj = _.find(array, key);
    return obj;
};

//Map Array Object 
export const mapArrayObject = (array, key) => {
    const mapValue = _.map(array, key);
    return mapValue;
};

//To convert Objects to Array of Objects
export const convertObjectToArray = (obj) => {
    const arr = _.values(obj);
    return arr;
};

//To add a new key-value pair to an array of objects
export const addElementToArray = (data) => {
    const arr = data.map(res => {
        return _.extend({}, res, { 'value': res.name, 'label': res.name });
    });
    return arr;
};
//To add a new key-value pair to an array of objects
export const addElementToArrayParam = (data,paramValue,paramName) => {
    const arr = data.map(res => {
        return _.extend({}, res, { 'value': res[paramValue], 'label': res[paramName] });
    });
    return arr;
};

//Convert Array to Object 
export const convertArrayToObject = (params, name) => {
    const arrayObj = _.keyBy(params, name);
    return arrayObj;
};

export const toaster = (htmlData, toastClass) => {
    for (const el of document.getElementsByClassName(toastClass)) {
        el.remove();
    }
    const randomVal = Math.random();
    IntertekToaster(htmlData, toastClass + ' invalidStartDate' + randomVal);
};

const inputChangeHandler = (e) => {
    if (e.target.type === "number") {
        if (e.target.value !== "0" && e.target.value !== "0.00") {
            e.target.value = numberFormat(e.target.value);
        }
    }
    const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
    const fieldName = e.target.name;
    const result = {
        value: fieldValue,
        name: fieldName
    };
    return result;
};

export const formInputChangeHandler = (event) => {
    const result = inputChangeHandler(event);
    return result;
};

export const captalize = (word) => {
    if (word !== undefined) {
        word = word.toLowerCase()
            .split(' ')
            .map((s) => s.charAt(0).toUpperCase() + s.substring(1))
            .join(' ');
        return word;
    }
};

//RegExp validator
export const customRegExValidator = (pattern, modifiers, value) => {
    const regex = new RegExp(pattern, modifiers);
    return value && !(regex.test(value));
};

export const fetchCoordinatorDetails = (name, coordinatorData, displayName, fieldName) => {
    const coordinatorDetail = [];
    if(coordinatorData && Array.isArray(coordinatorData)){
        for (let i = 0; i < coordinatorData.length; i++) {
            if (coordinatorData[i][displayName] === name) {
                coordinatorDetail.push(coordinatorData[i][fieldName]);
            }
        }
    }
    return coordinatorDetail;
};
/** Parse query param */
export const parseQueryParam = (param) => {
    let parsedParam = param.substring(1);   
    //parsedParam = JSON.parse('{"' + decodeURI(parsedParam.replace(/&/g, "\",\"").replace(/=/g, "\":\"")) + '"}');
    parsedParam = JSON.parse('{"' + parsedParam.replace(/&/g, "\",\"").replace(/=/g, "\":\"") + '"}');
    return parsedParam;
};
export const ObjectIntoQuerySting =(params)=>{
    const filteredKeys = Object.keys(params).filter(key => params[key] !== undefined);
    const queryString = filteredKeys && filteredKeys.map(key => key + '=' + encodeURIComponent(params[key])).join('&');
    return queryString;
};

export  function checkNested(obj, ...rest){
    for (let i = 0; i < rest.length; i++) {
        if (!obj || !obj.hasOwnProperty(rest[i])) {
            return false;
        }
        obj = obj[rest[i]];
    }
    return true;
};

//
//EG:// pass in your object structure as array elements
//const name = getNestedObject(user, ['personalInfo', 'name']);
// to access nested array, just pass in array index as an element the path array.
// const city = getNestedObject(user, ['personalInfo', 'addresses', 0, 'city']);

export const getNestedObject = (nestedObj, pathArr) => {
    return pathArr.reduce((obj, key) =>
        (obj && obj[key] !== 'undefined') ? obj[key] : undefined, nestedObj);
};

const operateObject = function (object) {
    if (isEmpty(object) )//|| object["recordStatus"] === undefined
        return object;

    if ([ 'N', 'M', 'D',undefined ].includes(object.recordStatus)) {
        const FilteredObject = FilterSave(object, {});

        if (FilteredObject !== undefined && !isEmpty(FilteredObject) && FilteredObject.constructor === Object) {
            for (const key in FilteredObject) {
                object[key] = FilteredObject[key];
            }
        }
        return object;
    }
    return null;
    // return [ 'N', 'M', 'D' ].includes(object.recordStatus);
};

const operateArray = function (object) {
    const result = [] = object.filter(eachItem => {
        return [ 'N', 'M', 'D' ].includes(eachItem.recordStatus) || eachItem["recordStatus"] === undefined;
    });
    const finalResult = result.map(mapItem => {
        return operateObject(mapItem);
    });

    return finalResult;
};

//Method to filter json based on recordStatus M,N,N
export const FilterSave = (saveObj, resultSaveObj = {}) => {
    const baseObjectType = whatis(saveObj);
    if (![ 'array', 'object' ].includes(baseObjectType))
        return saveObj;
    for (const key in saveObj) {
        const keyData = saveObj[key];
        const objectType = whatis(keyData);
        const operateType = {};
        operateType.array = operateArray;
        operateType.object = operateObject;
        if ([ 'array', 'object' ].includes(objectType)) {
            const data = operateType[objectType](keyData);
            resultSaveObj[key] = data === undefined ? null : data;
        } else {
            resultSaveObj[key] = keyData;
        }
    }
    return resultSaveObj;
};

export function deepCopy(obj) {
    if(typeof obj !== 'object' || obj === null) {
        return obj;
    }
     
    if(obj instanceof Array) {
        return obj.reduce((arr, item, i) => {
            arr[i] = deepCopy(item);
            return arr;
        }, []);
    }

    if(obj instanceof Object) {
        return Object.keys(obj).reduce((newObj, key) => {
            newObj[key] = deepCopy(obj[key]);
            return newObj;
        }, {});
    };
}
export const parseValdiationMessage = (response) => {
    let messageToBeDisplay ='';
    try {
        if (!isEmptyOrUndefine(response)) {
            const valdMsg = response.validationMessages,
                messages = response.messages;
            if (!isEmptyOrUndefine(valdMsg)) {
                valdMsg.forEach(msg => {
                    if (!isEmptyOrUndefine(msg.messages)) {
                        msg.messages.forEach(msg1 => {
                            messageToBeDisplay = messageToBeDisplay + '\r' + msg1.message;
                        });
                    }
                });
            }
            if (!isEmptyOrUndefine(messages)) {
                messages.forEach(resMsg => {
                    messageToBeDisplay = messageToBeDisplay + '\r' + resMsg.message;
                });
            }
        }
    }
    catch (error) {
        // messageToBeDisplay = localConstant.errorMessages.SOMETHING_WENT_WRONG;
    }
    return messageToBeDisplay === ''? getlocalizeData().errorMessages.SOMETHING_WENT_WRONG:messageToBeDisplay;
};

/** Process Inactive MI-Coordinator */
export const processMICoordinators = (miCoordinatorList) => {
    if(miCoordinatorList && miCoordinatorList.length > 0){
        miCoordinatorList.forEach(iteratedValue => {
            if(iteratedValue.status !== null){
                iteratedValue.miDisplayName = `${ iteratedValue.displayName } ( ${ iteratedValue.status } )`;
            }
            else{
                iteratedValue.miDisplayName = iteratedValue.displayName;
            }
        });
    }
    return isEmptyReturnDefault(miCoordinatorList);
};

/** Component Scroll to page top */
export const scrollToTop = () => {
    window.scrollTo(0,0);
};

export const decimalCheck =(e,val) =>
{
    if(e){
        if(e.indexOf('.') !== -1 && e.split('.')[1].length >val){
            return e.slice(0, (e.indexOf("."))+val+1);
        }
    }
    return e;
};

export const caseInsensitiveSort=(values)=>{
       values.sort(function (a, b) {
        if (typeof (a.supplierName) === 'undefined') return 1;
        if (typeof (b.supplierName) === 'undefined') return 0;
       
        a = (a.supplierName || '').toLowerCase();
        b = (b.supplierName || '').toLowerCase(); 
        
        return (a > b) ? 1 : ((a < b) ? -1 : 0);
      });
      
      return values;

};

export const SetCurrentModuleTabInfo = (tabData, tabId)=>{
    if(Array.isArray(tabData) && tabData.length > 0 && tabId){
    tabData.forEach(row => {
            if(row["tabBody"] === tabId) {
                row["isCurrentTab"] = true;
                const val = row["componentRenderCount"];
                row["componentRenderCount"] = val+1;
            } else {
                row["isCurrentTab"] = false;
            }
        });
    }
};

/**
 * method will reset the tab's componentRenderCount that is used to fetch the respective api requests in the module
 * @param {*} tabData - required, current module tab data
 * @param {*} resetTabs - Optional, To reset particular tabs. Leave Empty or dont pass if need to reset all the tabs
 */
export const ResetCurrentModuleTabInfo = (tabData, resetTabs) => {
    if (Array.isArray(tabData) && tabData.length > 0) {
        if (Array.isArray(resetTabs) && resetTabs.length > 0) {
            tabData.forEach((row) => {
                if (resetTabs.includes(row.tabBody)) {
                    row.componentRenderCount = -1;
                }
            });
        } else {
            tabData.forEach((row, index) => {
                if (index === 0) {
                    row.componentRenderCount = 0;
                    row.isCurrentTab = true;
                }
                else {
                    row.componentRenderCount = -1;
                    row.isCurrentTab = false;
                }
            });
        }
    }
};

/**
 * Common Function Added For Extranet-TS SSO
 * @param {*} param ===> should be a Encrypted Obeject
 */

export const ssoParseQueryParam=(param)=>{
    let value="";
    if(!required(param)){
        const parsedParam=param.substring(1);
        //    const result =Buffer.from(parsedParam, 'base64').toString('ascii'); // Encrypted Object convert to Decrypted JSONStringify
        //       value=JSON.parse(value);
        value = window.atob(parsedParam); //Added for D1302 French Letter Decode Fix
        value = decodeURIComponent(escape(value)); //Added for D1302 French Letter Decode Fix
        value=JSON.parse(value);
    } 
    return value;
};

export const isTrue=(value)=>{
    if (typeof(value) === 'string'){
        value = value.trim().toLowerCase();
    }
    switch(value){
        case true:
             return true;
        case "true":
             return true;       
        case "no":
             return false;    
        case "yes":
            return true;
        default: 
            return false;
    }
};

export function sortModuleCustomerwiseList(custmoduleList,filteredModuleActivityData,moduleName){
    const objMap=[];      
    for(let  i=0;i<=custmoduleList.length;i++){      
        for(let j=0;j<=filteredModuleActivityData.length;j++){
            if(!isEmpty(filteredModuleActivityData[j]))
            {              
                if(filteredModuleActivityData[j][moduleName] === custmoduleList[i])
                {
                    objMap.push(filteredModuleActivityData[j]);
                } 
            }
        }       
    }       
   return objMap;
}

export const formatToDecimal = (value,decimal) => {
    if(decimal){
        if(requiredNumeric(value))
            return parseFloat('0').toFixed(decimal);
        else
            return parseFloat(value).toFixed(decimal);
    } else {
        return value;
    }
};

export const isBoolean = function (obj) {
    return !!obj === obj;
};
//samlple util changes for commit fdg

//Check document has uniquekey value
export const uploadedDocumentCheck = (documentsList) => {
    let count = 0;
    if (!isEmpty(documentsList)) {
        documentsList.map(document => {
            if (isUndefined(document.documentUniqueName)) {
                IntertekToaster(getlocalizeData().commonConstants.DOCUMENT_STATUS, 'warningToast documentRecordToPasteReq');
                count++;
            }
        });
        if (count > 0) {
            return false;
        } else {
            return true;
        }
    }
    return true;
};