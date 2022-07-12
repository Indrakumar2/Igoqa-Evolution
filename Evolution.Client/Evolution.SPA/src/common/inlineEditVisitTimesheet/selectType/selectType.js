import React,{ Component, Fragment } from 'react';
import Flatpickr from 'react-flatpickr';
import CustomInput from '../../baseComponents/inputControlls'; 
import { 
    getlocalizeData, 
    isEmptyOrUndefine, 
    isEmpty,
    numberFormat,
    thousandFormat
} from '../../../utils/commonUtils';
import moment from 'moment';
import { 
    FormatTwoDecimal,
    FormatFourDecimal,
    FormatSixDecimal
} from '../visitTimesheetUtil';
import dateUtil from '../../../utils/dateUtil';
import { fieldLengthConstants } from '../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();
const currentDate = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);

class SelectType extends Component{
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state={
            expenseDateState: ''
        };
    }
    
     /*Input Change Handler*/
     inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }

    expenseDateFormat = (date) => {       
        if(date) {            
            return dateUtil.defaultMoment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT);       
         } else{
             return "";
         }
    };

    handleDateChangeRaw = (e, type) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);            
            if (!isValid) {                
                this.setState({ expenseDateState: '' });
                this[type]('', 'expenseDate');                
            }
        }
    }
    
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e, name) => { 
        this.updatedData = {};
        let result = {};
        if(name && name === 'expenseDate') {            
            const date = (e && e !== '' ? this.expenseDateFormat(e) : '');
            this.updatedData[name] = date;
            result = { value: date, name: name };
            this.setState({ expenseDateState:date });
        } else {
            result = this.inputChangeHandler(e);        
            this.updatedData[result.name] = result.value;            
        }
        this.updatedData["recordStatus"] = "M"; 
        
        if(this.props.colDef.moduleType === "Time" ) {
            if(this.props.colDef.moduleName === "Visit") {
                const techSpecTime = this.props.VisitTechnicalSpecialistTimes;
                let checkProperty = "visitTechnicalSpecialistAccountTimeId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecTimeId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecTime)) {
                    techSpecTime.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;                
                this.updatedData = this.VisitTimeCalculation(result, editedData);
                this.props.actions.VisitUpdateTechnicalSpecialistTime(this.updatedData, editedData);                
            } else if(this.props.colDef.moduleName === "Timesheet") {
                const techSpecTime = this.props.timesheetTechnicalSpecialistTimes;
                let checkProperty = "timesheetTechnicalSpecialistAccountTimeId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecTimeId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecTime)) {
                    techSpecTime.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;                
                this.updatedData = this.TimesheetTimeCalculation(result, editedData);
                this.props.actions.UpdateTimesheetTechnicalSpecialistTime(this.updatedData, editedData);                       
            }
        } else if(this.props.colDef.moduleType === "Travel" ) {
            if(this.props.colDef.moduleName === "Visit") {
                const techSpecTravel = this.props.VisitTechnicalSpecialistTravels;
                let checkProperty = "visitTechnicalSpecialistAccountTravelId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecTravelId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecTravel)) {
                    techSpecTravel.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;                
                this.props.actions.VisitUpdateTechnicalSpecialistTravel(this.updatedData, editedData);                       
            } else if(this.props.colDef.moduleName === "Timesheet") {
                const techSpecTravel = this.props.timesheetTechnicalSpecialistTravels;
                let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecTravelId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecTravel)) {
                    techSpecTravel.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;
                this.props.actions.UpdateTimesheetTechnicalSpecialistTravel(this.updatedData, editedData);                       
            }
        } else if(this.props.colDef.moduleType === "Expense" ) {
            if(this.props.colDef.moduleName === "Visit") {
                const techSpecExpense = this.props.VisitTechnicalSpecialistExpenses;
                let checkProperty = "visitTechnicalSpecialistAccountExpenseId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecExpenseId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecExpense)) {
                    techSpecExpense.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }

                editedData[result.name] = result.value; 
                if (!isEmptyOrUndefine(result.value) && (result.name === 'currency' || result.name === 'expenseDate' || result.name === 'chargeRateCurrency' || result.name === 'payRateCurrency')){
                    this.ExpenseExchangeRateCalculation(result, editedData, this.updatedData["recordStatus"]);                    
                } else {     
                    this.updatedData = this.VisitExpenseCalculation(result);                
                    this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, editedData);                                      
                }
                
                // editedData = this.ExpenseExchangeRateCalculation(result, editedData, this.updatedData["recordStatus"]);                
                // this.updatedData = this.VisitExpenseCalculation(result);                
                // this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, editedData);                       
            } else if(this.props.colDef.moduleName === "Timesheet") {
                const techSpecExpense = this.props.timesheetTechnicalSpecialistExpenses;
                let checkProperty = "timesheetTechnicalSpecialistAccountExpenseId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecExpenseId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecExpense)) {
                    techSpecExpense.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value; 
                if (!isEmptyOrUndefine(result.value) && (result.name === 'currency' || result.name === 'chargeRateCurrency' || result.name === 'payRateCurrency')){
                    this.ExpenseExchangeRateCalculation(result, editedData, this.updatedData["recordStatus"]);                    
                } else {     
                    this.updatedData = this.TimesheetExpenseCalculation(result);
                    this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, editedData);                   
                }
                // editedData = this.ExpenseExchangeRateCalculation(result, editedData, this.updatedData["recordStatus"]);
                // this.updatedData = this.TimesheetExpenseCalculation(result);
                // this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, editedData);                       
            }
        } else if(this.props.colDef.moduleType === "Consumable" ) {
            if(this.props.colDef.moduleName === "Visit") {
                const techSpecConsumable = this.props.VisitTechnicalSpecialistConsumables;
                let checkProperty = "visitTechnicalSpecialistAccountConsumableId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecConsumableId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecConsumable)) {
                    techSpecConsumable.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;
                this.updatedData = this.VisitConsumableCalculation(result, editedData);
                this.props.actions.VisitUpdateTechnicalSpecialistConsumable(this.updatedData, editedData);                       
            } else if(this.props.colDef.moduleName === "Timesheet") {
                const techSpecConsumable = this.props.timesheetTechnicalSpecialistConsumables;
                let checkProperty = "timesheetTechnicalSpecialistAccountConsumableId";
                if (this.props.data.recordStatus === 'N') {
                    checkProperty = "TechSpecConsumableId";
                    this.updatedData["recordStatus"] = "N";
                }
                let editedData = {};
                if(!isEmptyOrUndefine(techSpecConsumable)) {
                    techSpecConsumable.forEach(row => {
                        if(row[checkProperty] === this.props.data[checkProperty]) {
                            editedData = row;
                        }
                    });
                }
                editedData[result.name] = result.value;
                this.updatedData = this.TimesheetConsumableCalculation(result, editedData);
                this.props.actions.UpdateTimesheetTechnicalSpecialistConsumable(this.updatedData, editedData);                       
            }
        }        
    }

    VisitTimeCalculation(inputvalue, editedRow) { 
        if(inputvalue.name === 'chargeTotalUnit') {                    
            this.updatedData[inputvalue.name] = inputvalue.value;
            if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide && isEmptyOrUndefine(this.props.costofSalesStatus) ) { // ITK LIVE DEF 654 fix
                this.updatedData["payUnit"] = inputvalue.value;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
                    document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
                }
            }
        }              

        if(inputvalue.name === "chargeWorkUnit" || inputvalue.name === "chargeTravelUnit" 
            || inputvalue.name === "chargeWaitUnit" || inputvalue.name === "chargeReportUnit") {
                const chargeWorkUnit = isNaN(parseFloat(editedRow["chargeWorkUnit"]))?0:parseFloat(editedRow["chargeWorkUnit"]);
                const chargeTravelUnit = isNaN(parseFloat(editedRow["chargeTravelUnit"]))?0:parseFloat(editedRow["chargeTravelUnit"]);
                const chargeWaitUnit = isNaN(parseFloat(editedRow["chargeWaitUnit"]))?0:parseFloat(editedRow["chargeWaitUnit"]);
                const chargeReportUnit = isNaN(parseFloat(editedRow["chargeReportUnit"]))?0:parseFloat(editedRow["chargeReportUnit"]);
                const sumValue = chargeWorkUnit + chargeTravelUnit + chargeWaitUnit + chargeReportUnit;                
                
                this.updatedData["chargeTotalUnit"] = FormatTwoDecimal(sumValue);                      
                if(document.getElementById("chargeTotalUnit" + this.props.rowIndex) !== null ) {                        
                    document.getElementById("chargeTotalUnit" + this.props.rowIndex).value = FormatTwoDecimal(sumValue);   
                }
                if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide &&  isEmptyOrUndefine(this.props.costofSalesStatus)) { // ITK LIVE DEF 654 fix 
                    this.updatedData["payUnit"] = FormatTwoDecimal(sumValue); 
                    if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                        
                        document.getElementById("payUnit" + this.props.rowIndex).value = FormatTwoDecimal(sumValue);
                    }
                }
        }        
        return this.updatedData;
    }

    calculateExpenseGross(data) {
        const payUnit = isNaN(parseFloat(data.payUnit))?0:parseFloat(data.payUnit),
         payRate = isNaN(parseFloat(data.payRate))?0:parseFloat(data.payRate),
         payRateTax = isNaN(parseFloat(data.payRateTax))?0:parseFloat(data.payRateTax);          
         return (payUnit * payRate) + payRateTax;
    }

    VisitExpenseCalculation(inputvalue) { 
        if(inputvalue.name ==='chargeUnit') {         
            this.updatedData[inputvalue.name] = inputvalue.value;    
            if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide &&  isEmptyOrUndefine(this.props.costofSalesStatus)) { // ITK LIVE DEF 654 fix 
                this.updatedData["payUnit"] = inputvalue.value;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
                    document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
                }     
            }                       
        }        
        return this.updatedData;
    }

    ExpenseExchangeRateCalculation = async (inputvalue, editedRow, recordStatus) => {        
        await this.fetchDefaultExchangeRate(inputvalue.name, inputvalue.value, editedRow, recordStatus, this.props.rowIndex);
    }

    fetchDefaultExchangeRate = async (inputname, inputvalue, row, recordStatus, rowIndex) => {

        const obj = {
            currencyFrom: '',
            currencyTo: '',
            effectiveDate: row.expenseDate ? moment(row.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT): currentDate
        };
        let chargeCurrency = '', payCurrency = '',contractNumber = '' , expenseDate = '';
        if(this.props.colDef.moduleName === "Visit") {
            contractNumber = this.props.visitInfo.visitContractNumber;
        }else{
            contractNumber = this.props.timesheetInfo.timesheetContractNumber;
        }

        if (recordStatus === 'M') {
            // obj.currencyTo = this.updatedData.currency ? this.updatedData.currency : this.editedRowData.currency;
            // chargeCurrency = this.updatedData.chargeRateCurrency?this.updatedData.chargeRateCurrency:this.editedRowData.chargeRateCurrency;
            // payCurrency = this.updatedData.payRateCurrency?this.updatedData.payRateCurrency:this.editedRowData.payRateCurrency;
            obj.currencyFrom = row.currency;
            chargeCurrency = row.chargeRateCurrency;
            payCurrency = row.payRateCurrency;
            expenseDate= row.expenseDate ? moment(row.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT): currentDate;
        }
        else {
            obj.currencyFrom = row.currency;
            chargeCurrency = row.chargeRateCurrency;
            payCurrency = row.payRateCurrency;
            expenseDate= row.expenseDate ? moment(row.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT): currentDate;
        }

        if (inputname === 'currency') {
            obj.currencyFrom = inputvalue;
            const exchangeRates = [];
            if (isEmptyOrUndefine(inputvalue)) {                
                return false;
            }
            if (!isEmptyOrUndefine(chargeCurrency)) {
                exchangeRates.push({ ...obj, currencyTo: chargeCurrency });
            }
            if (!isEmptyOrUndefine(payCurrency)) {
                exchangeRates.push({ ...obj, currencyTo: payCurrency });
            }
            if (!isEmptyOrUndefine(expenseDate)) {
                exchangeRates.push({ ...obj, currencyTo: chargeCurrency });
            }
            if (exchangeRates.length > 0) {
                const rateCurrencyObj= {
                    chargeRateCurrency: chargeCurrency,
                    payRateCurrency: payCurrency
                };                                  
                await this.addDefaultExchangeRate(exchangeRates,rateCurrencyObj,contractNumber);
                this.updatedData[inputname] = inputvalue;
                this.updatedData["recordStatus"] = recordStatus;
                this.updatedData["chargeRateExchange"] = rateCurrencyObj.chargeRateExchange ? this.formatExchangeRate(rateCurrencyObj.chargeRateExchange) : null;
                this.updatedData["payRateExchange"] = rateCurrencyObj.payRateExchange ? this.formatExchangeRate(rateCurrencyObj.payRateExchange) : null;
                row.chargeRateExchange = this.updatedData["chargeRateExchange"];
                row.payRateExchange = this.updatedData["payRateExchange"];                
                if(document.getElementById("chargeRateExchange" + rowIndex) !== null ) {
                    document.getElementById("chargeRateExchange" + rowIndex).value = this.updatedData["chargeRateExchange"];
                }
                if(document.getElementById("payRateExchange" + rowIndex) !== null ) {
                    document.getElementById("payRateExchange" + rowIndex).value = this.updatedData["payRateExchange"];
                }
                if(this.props.colDef.moduleName === "Visit") {
                    this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, row);
                } else {
                    this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, row);
                }
            }
        }
        else if (inputname === 'expenseDate' && !isEmptyOrUndefine(obj.currencyFrom)) {
            const exchangeRates = [];
            let chargeRate='' ; 
            let payRate='';
            if (!isEmptyOrUndefine(chargeCurrency)) {
                exchangeRates.push({ ...obj, currencyTo: chargeCurrency });
            }
            if (!isEmptyOrUndefine(payCurrency)) {
                exchangeRates.push({ ...obj, currencyTo: payCurrency });
            }
            const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates,contractNumber);
            if (res && res.length > 0) {
                chargeRate= res.filter(x=>x.currencyTo === chargeCurrency) && res.filter(x=>x.currencyTo === chargeCurrency).length > 0 ?
                res.filter(x=>x.currencyTo === chargeCurrency)[0].rate : null;

                payRate= res.filter(x=>x.currencyTo === payCurrency) && res.filter(x=>x.currencyTo === payCurrency).length > 0 ?
                res.filter(x=>x.currencyTo === payCurrency)[0].rate : null;

                this.updatedData[inputname] = inputvalue;
                this.updatedData["recordStatus"] = recordStatus;
                this.updatedData["chargeRateExchange"] = chargeRate ? this.formatExchangeRate(chargeRate) : null;
                this.updatedData["payRateExchange"] = payRate ? this.formatExchangeRate(payRate) : null;
                row.chargeRateExchange = this.updatedData["chargeRateExchange"];      
                row.payRateExchange = this.updatedData["payRateExchange"];                
                        
                if(document.getElementById("chargeRateExchange" + rowIndex) !== null ) {
                    document.getElementById("chargeRateExchange" + rowIndex).value = this.updatedData["chargeRateExchange"];
                }
                if(document.getElementById("payRateExchange" + rowIndex) !== null ) {
                    document.getElementById("payRateExchange" + rowIndex).value = this.updatedData["payRateExchange"];
                }  
                if(this.props.colDef.moduleName === "Visit") {
                    this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, row);
                } else {
                    this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, row);
                }
            }
        }
        else if (inputname === 'chargeRateCurrency' && !isEmptyOrUndefine(obj.currencyFrom)) {
            obj.currencyTo = inputvalue;
            const res1 = await this.props.actions.FetchCurrencyExchangeRate([ obj ],contractNumber);
            if (res1 && res1.length > 0) {
                this.updatedData[inputname] = inputvalue;
                this.updatedData["recordStatus"] = recordStatus;
                this.updatedData["chargeRateExchange"] = res1[0] ? this.formatExchangeRate(res1[0].rate) : null;
                row.chargeRateExchange = this.updatedData["chargeRateExchange"];                
                if(document.getElementById("chargeRateExchange" + rowIndex) !== null ) {
                    document.getElementById("chargeRateExchange" + rowIndex).value = this.updatedData["chargeRateExchange"];
                }
                if(this.props.colDef.moduleName === "Visit") {
                    this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, row);
                } else {
                    this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, row);
                }
            }
        }
        else if (inputname === 'payRateCurrency' && !isEmptyOrUndefine(obj.currencyFrom)) {
            obj.currencyTo = inputvalue;
            const res = await this.props.actions.FetchCurrencyExchangeRate([ obj ],contractNumber);
            if (res && res.length > 0) {
                this.updatedData[inputname] = inputvalue;
                this.updatedData["recordStatus"] = recordStatus;
                this.updatedData["payRateExchange"] = res[0] ? this.formatExchangeRate(res[0].rate) : null;
                row.payRateExchange = this.updatedData["payRateExchange"];                
                if(document.getElementById("payRateExchange" + rowIndex) !== null ) {
                    document.getElementById("payRateExchange" + rowIndex).value = this.updatedData["payRateExchange"];
                }
                if(this.props.colDef.moduleName === "Visit") {
                    this.props.actions.VisitUpdateTechnicalSpecialistExpense(this.updatedData, row);
                } else {
                    this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, row);
                }
            }
        }
        return row;
    }

    addDefaultExchangeRate = async (exchangeRates, lineItemData,contractNumber) =>{
        const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates,contractNumber);
        if (res && res.length > 0) {
            res.forEach(eachRate =>{
                if(eachRate.currencyTo === lineItemData["chargeRateCurrency"]){
                        this.updatedData["chargeRateExchange"] = this.formatExchangeRate(eachRate.rate);
                        lineItemData["chargeRateExchange"] = this.updatedData["chargeRateExchange"];
                    }
                    if(eachRate.currencyTo === lineItemData["payRateCurrency"]){
                        this.updatedData["payRateExchange"] = this.formatExchangeRate(eachRate.rate);
                        lineItemData["payRateExchange"] = this.updatedData["payRateExchange"];
                    } 
            });
        }
        return lineItemData;
    }

    formatExchangeRate= (rate)=>{        
        //return FormatSixDecimal(Math.round(rate * 1000) / 1000);
        return FormatSixDecimal(rate);
    }

    VisitConsumableCalculation(inputvalue) { 
        if(inputvalue.name === 'chargeTotalUnit') {                    
            this.updatedData[inputvalue.name] = inputvalue.value; 
            //Commented the code for CR14
            // if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide && isEmptyOrUndefine(this.props.costofSalesStatus)) {   // ITK LIVE DEF 654 fix             
            //     this.updatedData["payUnit"] = inputvalue.value;
            //     if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
            //         document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
            //     }
            // }
        }
        return this.updatedData;
    }

    TimesheetTimeCalculation(inputvalue) { 
        if(inputvalue.name === 'chargeTotalUnit') {                    
            this.updatedData[inputvalue.name] = inputvalue.value;     
            if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide &&  isEmptyOrUndefine(this.props.costofSalesStatus)) {       // ITK LIVE DEF 654 fix  
                this.updatedData["payUnit"] = inputvalue.value;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
                    document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
                }
            }
        }
        return this.updatedData;
    }

    TimesheetExpenseCalculation(inputvalue) { 
        if(inputvalue.name ==='chargeUnit') {         
            this.updatedData[inputvalue.name] = inputvalue.value;  
            if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide &&  isEmptyOrUndefine(this.props.costofSalesStatus)) {    //ITK LIVE DEF 654 fix              
                this.updatedData["payUnit"] = inputvalue.value;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
                    document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
                }
            }
        }
        return this.updatedData;
    }

    TimesheetConsumableCalculation(inputvalue) {
        if(inputvalue.name === 'chargeUnit') {                    
            this.updatedData[inputvalue.name] = inputvalue.value;     
            //Commented the code for CR14
            // if(this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide &&  isEmptyOrUndefine(this.props.costofSalesStatus)) {   // ITK LIVE DEF 654 fix       
            //     this.updatedData["payUnit"] = inputvalue.value;
            //     if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {                
            //         document.getElementById("payUnit" + this.props.rowIndex).value = inputvalue.value;
            //     }
            // }
        }
        return this.updatedData;
    }

    decimalWithFourLimitFormat = (evt) => { 
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ ( this.props.colDef.name === 'chargeRate' ? parseInt(14) : parseInt(10)) + "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    }; 

    checkTwoDigitNumber = (e) => {
        if(isEmpty(e.target.value)) {
            e.target.value = "";
        } else  if(!isEmpty(e.target.value) && e.target.value > 0){            
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);            
        } else {
            e.target.value = 0.00;
        }        
        this.formInputChangeHandler(e);
    }

    decimalWithTwoLimitFormat = (evt) => {  
        const e = evt || window.event;
        let expression = '';
        const propss = this.props;
        const name = propss.colDef.name;
        const moduleType = propss.colDef.moduleType;
        // console.log(name);
        if (name === 'chargeRate') {
            expression = ("(\\d{0," + (parseInt(14)) + "})[^.]*((?:\\.\\d{0," + parseInt(2) + "})?)");
            const rg = new RegExp(expression, "g");
            const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
            e.target.value = match[1] + match[2];
        }
        else if (name === 'payUnit' || name === 'chargeUnit' || name === 'chargeTotalUnit' || name === 'chargeWorkUnit' 
            || name === 'chargeTravelUnit' || name === 'chargeWaitUnit' || name === 'chargeReportUnit') {
            if (moduleType == 'Time' || moduleType == 'Travel') {
                expression = ("(^[+-]?\\d{0," + (parseInt(10)) + "})[^.]*((?:\\.\\d{0," + parseInt(2) + "})?)");
                // console.log(expression);
                const rg = new RegExp(expression, "g");
                const match = rg.exec(e.target.value);
                e.target.value = match[1] + match[2];
            }
            else {
                expression = ("(^[+-]?\\d{0," + (parseInt(10)) + "})[^.]*((?:\\.\\d{0," + parseInt(2) + "})?)");
                // console.log(expression);
                const rg = new RegExp(expression, "g");
                const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
                e.target.value = match[1] + match[2];
            }
        }
        else{
            expression = ("(\\d{0," + (parseInt(10)) + "})[^.]*((?:\\.\\d{0," + parseInt(2) + "})?)");
            const rg = new RegExp(expression, "g");
            const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
            e.target.value = match[1] + match[2];
        }
        if (e.preventDefault) e.preventDefault();
    };

    checkFourNumber = (e) => {
        if(!isEmpty(e.target.value) && e.target.value > 0){
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);            
        } else {
            e.target.value = 0.0000;
        }        
        this.formInputChangeHandler(e);
    }

    decimalWithSixLimitFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(6) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    }; 

    checkSixNumber = (e) => {
        if(!isEmpty(e.target.value) && e.target.value > 0){
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(6);            
        } else {
            e.target.value = 0.000000;
        }
        this.formInputChangeHandler(e);
    }

    FormatSixDecimal(value) {
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
    }

    editRate= (e) => {
        if(this.props.colDef.name.indexOf('payRate') >= 0) {
            this.props.colDef.EditPayRateRowHandler(this.props.data);            
        } else {
            this.props.colDef.EditChargeRateRowHandler(this.props.data);            
        }        
    }

    swapTravelPayUnit= (e) => {
        e.preventDefault();
        if(this.props.colDef.moduleName === "Visit") {
            this.updatedData = {};
            this.updatedData["recordStatus"] = "M"; 
            const techSpecTravel = this.props.VisitTechnicalSpecialistTravels;
            let checkProperty = "visitTechnicalSpecialistAccountTravelId";
            if (this.props.data.recordStatus === 'N') {
                checkProperty = "TechSpecTravelId";
                this.updatedData["recordStatus"] = "N";
            }
            let editedRow = {};
            if(!isEmptyOrUndefine(techSpecTravel)) {
                techSpecTravel.forEach(row => {
                    if(row[checkProperty] === this.props.data[checkProperty]) {
                        editedRow = row;
                    }
                });
            }

            if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {                     
                this.updatedData["chargeTotalUnit"] = editedRow.payUnit;
                if(document.getElementById("chargeTotalUnit" + this.props.rowIndex) !== null ) {
                    document.getElementById("chargeTotalUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.payUnit);             
                }
            } else {
                if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                    this.updatedData["chargeTotalUnit"] = editedRow.payUnit;
                    if(document.getElementById("chargeTotalUnit" + this.props.rowIndex) !== null ) {
                        document.getElementById("chargeTotalUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.payUnit); 
                    }
                } else {                
                    if(editedRow.payExpenseType === 'Miles') {                              
                        this.updatedData["chargeTotalUnit"] = this.milesToKiloMeters(editedRow.payUnit);
                        if(document.getElementById("chargeTotalUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("chargeTotalUnit" + this.props.rowIndex).value = this.milesToKiloMeters(editedRow.payUnit); 
                        }
                    } else {                    
                        this.updatedData["chargeTotalUnit"] = this.kiloMetersToMiles(editedRow.payUnit); 
                        if(document.getElementById("chargeTotalUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("chargeTotalUnit" + this.props.rowIndex).value = this.kiloMetersToMiles(editedRow.payUnit);
                        }
                    }                
                }
            }
            this.props.actions.VisitUpdateTechnicalSpecialistTravel(this.updatedData, editedRow);
        } else {
            this.updatedData = {};
            this.updatedData["recordStatus"] = "M"; 
            const techSpecTravel = this.props.timesheetTechnicalSpecialistTravels;
            let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
            if (this.props.data.recordStatus === 'N') {
                checkProperty = "TechSpecTravelId";
                this.updatedData["recordStatus"] = "N";
            }
            let editedRow = {};
            if(!isEmptyOrUndefine(techSpecTravel)) {
                techSpecTravel.forEach(row => {
                    if(row[checkProperty] === this.props.data[checkProperty]) {
                        editedRow = row;
                    }
                });
            }

            if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {                     
                this.updatedData["chargeUnit"] = editedRow.payUnit;
                if(document.getElementById("chargeUnit" + this.props.rowIndex) !== null ) {
                    document.getElementById("chargeUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.payUnit);             
                }
            } else {
                if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                    this.updatedData["chargeUnit"] = editedRow.payUnit;
                    if(document.getElementById("chargeUnit" + this.props.rowIndex) !== null ) {
                        document.getElementById("chargeUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.payUnit); 
                    }
                } else {                
                    if(editedRow.payExpenseType === 'Miles') {                              
                        this.updatedData["chargeUnit"] = this.milesToKiloMeters(editedRow.payUnit);
                        if(document.getElementById("chargeUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("chargeUnit" + this.props.rowIndex).value = this.milesToKiloMeters(editedRow.payUnit); 
                        }
                    } else {                    
                        this.updatedData["chargeUnit"] = this.kiloMetersToMiles(editedRow.payUnit); 
                        if(document.getElementById("chargeUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("chargeUnit" + this.props.rowIndex).value = this.kiloMetersToMiles(editedRow.payUnit);
                        }
                    }                
                }
            }
            this.props.actions.UpdateTimesheetTechnicalSpecialistTravel(this.updatedData, editedRow);
        }
    }

    swapTravelChargeUnit= (e) => {
        e.preventDefault();
        if(this.props.colDef.moduleName === "Visit") {
            this.updatedData = {};
            this.updatedData["recordStatus"] = "M"; 
            const techSpecTravel = this.props.VisitTechnicalSpecialistTravels;
            let checkProperty = "visitTechnicalSpecialistAccountTravelId";
            if (this.props.data.recordStatus === 'N') {
                checkProperty = "TechSpecTravelId";
                this.updatedData["recordStatus"] = "N";
            }
            let editedRow = {};
            if(!isEmptyOrUndefine(techSpecTravel)) {
                techSpecTravel.forEach(row => {
                    if(row[checkProperty] === this.props.data[checkProperty]) {
                        editedRow = row;
                    }
                });
            }
            
            if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {            
                this.updatedData["payUnit"] = editedRow.chargeTotalUnit;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                    document.getElementById("payUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.chargeTotalUnit);      
                }
            } else {
                if(editedRow.chargeExpenseType === editedRow.payExpenseType) {                
                    this.updatedData["payUnit"] = editedRow.chargeTotalUnit;    
                    if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {  
                        document.getElementById("payUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.chargeTotalUnit);
                    }
                } else {
                    if(editedRow.chargeExpenseType === 'Miles') {                    
                        this.updatedData["payUnit"] = this.milesToKiloMeters(editedRow.chargeTotalUnit);
                        if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {  
                            document.getElementById("payUnit" + this.props.rowIndex).value = this.milesToKiloMeters(editedRow.chargeTotalUnit);
                        }
                    } else {
                        this.updatedData["payUnit"] = this.kiloMetersToMiles(editedRow.chargeTotalUnit);
                        if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("payUnit" + this.props.rowIndex).value = this.kiloMetersToMiles(editedRow.chargeTotalUnit);
                        }
                    }                
                }
            } 
            this.props.actions.VisitUpdateTechnicalSpecialistTravel(this.updatedData, editedRow);
        } else {
            this.updatedData = {};
            this.updatedData["recordStatus"] = "M"; 
            const techSpecTravel = this.props.timesheetTechnicalSpecialistTravels;
            let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
            if (this.props.data.recordStatus === 'N') {
                checkProperty = "TechSpecTravelId";
                this.updatedData["recordStatus"] = "N";
            }
            let editedRow = {};
            if(!isEmptyOrUndefine(techSpecTravel)) {
                techSpecTravel.forEach(row => {
                    if(row[checkProperty] === this.props.data[checkProperty]) {
                        editedRow = row;
                    }
                });
            }

            if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {            
                this.updatedData["payUnit"] = editedRow.chargeUnit;
                if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                    document.getElementById("payUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.chargeUnit);      
                }
            } else {
                if(editedRow.chargeExpenseType === editedRow.payExpenseType) {                
                    this.updatedData["payUnit"] = editedRow.chargeUnit;      
                    if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                        document.getElementById("payUnit" + this.props.rowIndex).value = FormatTwoDecimal(editedRow.chargeUnit);
                    }
                } else {
                    if(editedRow.chargeExpenseType === 'Miles') {                    
                        this.updatedData["payUnit"] = this.milesToKiloMeters(editedRow.chargeUnit);
                        if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("payUnit" + this.props.rowIndex).value = this.milesToKiloMeters(editedRow.chargeUnit);
                        }
                    } else {
                        this.updatedData["payUnit"] = this.kiloMetersToMiles(editedRow.chargeUnit);
                        if(document.getElementById("payUnit" + this.props.rowIndex) !== null ) {
                            document.getElementById("payUnit" + this.props.rowIndex).value = this.kiloMetersToMiles(editedRow.chargeUnit);
                        }
                    }                
                }
            } 
            this.props.actions.UpdateTimesheetTechnicalSpecialistTravel(this.updatedData, editedRow);
        }
    }

    milesToKiloMeters(value) {        
        return FormatTwoDecimal(Math.ceil(value * 1.6));
    }

    kiloMetersToMiles(value) {
        return FormatTwoDecimal(Math.ceil(value / 1.6));
    }

    expenseDateChange = (date) => {        
        this.setState({ expenseDate: date });
        this.updatedData["expenseDate"] = moment.parseZone(date).utc().format();
    }

    componentDidMount() {
        if(this.props.colDef.type === 'date') {
            this.setState({ expenseDateState:this.props.value });
        }
    }

    render(){
        const rowIndex = this.props.rowIndex !== undefined ? this.props.rowIndex : 0;
        const validation = this.props.validation ? this.props.validation : "";
        let readonly = (this.props.interactionMode ? this.props.interactionMode : false);
        const isCHUser = ((this.props.colDef.name === "payRate" || this.props.colDef.name === "payRateTax") 
                            && this.props.lineItemPermision.isCHUser);
        if(!readonly && this.props.colDef.moduleName === "Visit") {
            readonly = this.props.isTBAVisitStatus ? this.props.isTBAVisitStatus : false;
        }
        if(!readonly) {
            readonly = ((this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditable !== undefined) 
                            ? !this.props.lineItemPermision.isLineItemEditable : false);
        }

        const status = (this.props.colDef.moduleName === "Visit" ? this.props.visitInfo.visitStatus : this.props.timesheetInfo.timesheetStatus);
        if(!readonly && this.props.recordStatus !== 'N' && (status === 'O' || status === 'A') && this.props.colDef.chargePayType === "") {    
            //Defect 1426 - Added fields for not disabling - "expenseDescription"
            if(this.props.lineItemPermision && (this.props.lineItemPermision.isOperatingCompany || this.props.lineItemPermision.isCoordinatorCompany) && !([ "expenseDescription", "isInvoicePrintExpenseDescription", "currency", "isContractHolderExpense", "isInvoicePrintExpenseDescription", "isInvoicePrintChargeDescription" ].includes(this.props.colDef.name))) {
                readonly = (this.props.lineItemPermision.isLineItemEditableExpense === false) ? true : false;
            }
            //Defect 1426 - Added fields for not disabling - expenseDate, chargeExpenseType

            if(this.props.lineItemPermision && (this.props.lineItemPermision.isOperatingCompany || this.props.lineItemPermision.isCoordinatorCompany)
               && this.props.hasNoChargeRate && this.props.colDef.moduleType !== "Travel" && ([ "expenseDate", "chargeExpenseType" ].includes(this.props.colDef.name))) {
                readonly = false;
            }
            if(this.props.lineItemPermision && this.props.lineItemPermision.isCoordinatorCompany 
               && this.props.costofSalesStatus !== null 
               && this.props.hasNoChargeRate === false && this.props.colDef.moduleType !== "Travel" && ([ "expenseDate", "chargeExpenseType" ].includes(this.props.colDef.name))) {
                readonly = true;
            }
            if (this.props.lineItemPermision.isInterOperatingCompany) {
                if (this.props.lineItemPermision && this.props.lineItemPermision.isOperatingCompany
                    && this.props.costofSalesStatus === null
                    && ([ "expenseDate", "chargeExpenseType", "expenseDescription", "isInvoicePrintExpenseDescription", "currency" ].includes(this.props.colDef.name))) {
                    if (this.props.invoicingStatus === 'N')
                        readonly = true;
                    else
                        readonly = false;
                }
                if (this.props.lineItemPermision && this.props.lineItemPermision.isCoordinatorCompany
                    && this.props.costofSalesStatus === null
                    && ([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency" ].includes(this.props.colDef.name))) {
                    if (this.props.invoicingStatus === 'N')
                        readonly = false;
                    else
                        readonly = true;
                }
            }
            else if (this.props.lineItemPermision && this.props.lineItemPermision.isOperatingCompany
                && this.props.costofSalesStatus === null
                && ([ "expenseDate", "chargeExpenseType", "expenseDescription", "isInvoicePrintExpenseDescription", "currency" ].includes(this.props.colDef.name))) {
                if (this.props.invoicingStatus === 'N')
                    readonly = false;
                else
                    readonly = true;
            }

            if(status === 'O' && this.props.lineItemPermision && this.props.lineItemPermision.isCoordinatorCompany 
                && this.props.costofSalesStatus === null && this.props.colDef.moduleType !== "Travel" 
                && this.props.hasNoChargeRate === false && ([ "expenseDate", "chargeExpenseType" ].includes(this.props.colDef.name))) {
                 readonly = false;
             }
            if(status === 'O' && this.props.lineItemPermision && this.props.lineItemPermision.isCoordinatorCompany && this.props.colDef.moduleType !== "Travel" 
            && this.props.costofSalesStatus !== null && this.props.hasNoChargeRate && ([ "expenseDate", "chargeExpenseType" ].includes(this.props.colDef.name))) {
                readonly = true;
            }
            if(status === 'O' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
               && this.props.hasNoChargeRate === false && 
               (([ "expenseDescription", "currency", "chargeRateCurrency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isOperatingCompany)
               || ([ "expenseDescription", "currency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isCoordinatorCompany))) {
                readonly = true;
            }
            if(status === 'O' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
             && this.props.costofSalesStatus !== null &&
               (([ "expenseDescription", "currency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isCoordinatorCompany))) {
                readonly = true;
            }
            if(status === 'A' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
            && this.props.costofSalesStatus !== null &&
               (([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency", "chargeRateCurrency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isOperatingCompany)
               || ([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isCoordinatorCompany))) {
                readonly = true;
            }
            if(status === 'A' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
            && this.props.costofSalesStatus !== null && this.props.hasNoChargeRate &&
               (([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency", "chargeRateCurrency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isOperatingCompany))) {
                readonly = false;
            }
            if(status === 'O' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
               && this.props.hasNoChargeRate === false && this.props.costofSalesStatus === null &&
               (([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isCoordinatorCompany))) {
                readonly = false;
            }
            if(status === 'O' && this.props.lineItemPermision && this.props.colDef.moduleType === "Expense"
            && this.props.hasNoChargeRate === false && this.props.costofSalesStatus === null &&
            ([ "expenseDate", "chargeExpenseType", "expenseDescription", "currency", "chargeRateCurrency" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isOperatingCompany)){
                readonly = true;
            } 
            if(status === 'A' && this.props.lineItemPermision && this.props.colDef.moduleType === "Time"
               && this.props.hasNoChargeRate && this.props.costofSalesStatus !== null &&
               (([ "expenseDate", "chargeExpenseType" ].includes(this.props.colDef.name) && this.props.lineItemPermision.isCoordinatorCompany))) {
                readonly = true;
            }
        }

        if(!readonly) {
            if(this.props.colDef.chargePayType === "charge" ) {
                readonly = (this.props.colDef.moduleType === "Consumable" && this.props.colDef.name === "chargeExpenseType" && this.props.recordStatus === 'N' ? false : 
                                ((this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnChargeSide !== undefined)
                                    ? !this.props.lineItemPermision.isLineItemEditableOnChargeSide : false));
                if(readonly === false && (this.props.invoicingStatus === 'C' || this.props.invoicingStatus === 'D')) {
                    readonly = true;
                }
            } else if(this.props.colDef.chargePayType === "pay") {
                readonly = (this.props.colDef.moduleType === "Consumable" && ((this.props.colDef.name === "payExpenseType" || this.props.colDef.name === "payType") && this.props.recordStatus === 'N') ? false : 
                                ((this.props.lineItemPermision && this.props.lineItemPermision.isLineItemEditableOnPaySide !== undefined)
                                    ? !this.props.lineItemPermision.isLineItemEditableOnPaySide : false));
                if(readonly === false && this.props.costofSalesStatus === 'X') {
                    readonly = true;
                }
            } else {
                if(this.props.invoicingStatus === 'C' || this.props.invoicingStatus === 'D' || this.props.costofSalesStatus === 'X') {
                    readonly = true;
                }
                if(this.props.costofSalesStatus === 'X' && this.props.lineItemPermision.isCoordinatorCompany &&
                (this.props.colDef.moduleType === "Time" && ((status === 'O' || status === 'A'))
                && [ "expenseDescription", "isInvoicePrintExpenseDescription" ].includes(this.props.colDef.name) ))
                {
                    readonly = false;
                }
            }
        }
        
        if(!readonly && this.props.lineItemPermision && this.props.lineItemPermision.isLoggedinCompany &&
            (this.props.colDef.name === "chargeRateCurrency" || this.props.colDef.name === "currency" 
            || this.props.colDef.name === "payRateCurrency" ))
        {
            readonly = true;
        }
        
        if(!readonly && this.props.lineItemPermision && this.props.lineItemPermision.isInterOperatingCompany && 
            this.props.colDef.name === "chargeRateCurrency" && this.props.colDef.moduleType !== "Expense" )
        {
            readonly = true;
        }

        if((status === 'O' || status === 'A') && this.props.lineItemPermision && [ "Travel", "Consumable" ].includes(this.props.colDef.moduleType)
        && (this.props.hasNoChargeRate || this.props.hasNoChargeRate === undefined)
        && ([ "expenseDescription", "isInvoicePrintExpenseDescription", "chargeExpenseType" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isOperatingCompany)) 
        {
            readonly = false;
        }
        
        if((status === 'O') && this.props.lineItemPermision && this.props.colDef.moduleType === "Travel" 
        && this.props.hasNoChargeRate //&& this.props.costofSalesStatus !== null
        && ([ "expenseDate" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isOperatingCompany)) 
        {
            readonly = false;
        }

        if(status==="A" && this.props.lineItemPermision && this.props.colDef.moduleType === "Travel" 
        && (this.props.costofSalesStatus !== null && this.props.costofSalesStatus !== undefined)
        && ([ "expenseDate","expenseDescription", "isInvoicePrintExpenseDescription", "chargeExpenseType" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isCoordinatorCompany)) 
        {
            readonly = true;
        }

        if(status==="A" && this.props.lineItemPermision && this.props.colDef.moduleType === "Travel" 
        && this.props.hasNoChargeRate && this.props.costofSalesStatus !== null
        && ([ "expenseDate" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isOperatingCompany)) 
        {
            readonly = false;
        }

        if(status==="A" && this.props.lineItemPermision && this.props.colDef.moduleType === "Travel" 
        && (this.props.costofSalesStatus === null || this.props.costofSalesStatus === undefined)
        && ([ "expenseDate","expenseDescription","isInvoicePrintExpenseDescription","chargeExpenseType" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isOperatingCompany)) 
        {
            if(this.props.invoicingStatus === 'C')
            readonly = true;
            else
            readonly = false;
        }
        
        if((status === 'O' || status === 'A') && this.props.lineItemPermision && this.props.colDef.moduleType === "Consumable" 
        && ([ "expenseDate","payExpenseType" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isCoordinatorCompany)) 
        {
            readonly = false;
        }

        if((status === "O" || status === "A") && this.props.lineItemPermision && this.props.colDef.moduleType === "Consumable" 
        && this.props.costofSalesStatus !== null 
        && ([ "expenseDate","isInvoicePrintChargeDescription","payExpenseType","chargeExpenseType" ].includes(this.props.colDef.name) 
        && (this.props.lineItemPermision.isOperatingCompany || this.props.lineItemPermision.isCoordinatorCompany))) 
        {
            readonly = false;
        }

        if((status === "O" || status === "A") && this.props.lineItemPermision && [ "Travel", "Consumable" ].includes(this.props.colDef.moduleType)
        && this.props.invoicingStatus === 'C'
        && ([ "expenseDate","isInvoicePrintChargeDescription","chargeExpenseType","expenseDescription","isInvoicePrintExpenseDescription" ].includes(this.props.colDef.name) 
        && (this.props.lineItemPermision.isOperatingCompany || this.props.lineItemPermision.isCoordinatorCompany))) 
        {
            readonly = true;
        }

        if((status === "A") && this.props.lineItemPermision && this.props.colDef.moduleType === "Consumable" 
        && this.props.costofSalesStatus !== null && this.props.hasNoChargeRate === false
        && ([ "expenseDate" ].includes(this.props.colDef.name) 
        && this.props.lineItemPermision.isOperatingCompany)) 
        {
            readonly = true;
        }

        //Commented on 18-Jan-2021(Basis email recieved from Francina)
        // if(!readonly && this.props.recordStatus !== 'N' && this.props.lineItemPermision && this.props.lineItemPermision.isInterOperatingCompany && 
        //     (this.props.colDef.name === 'chargeTotalUnit' || this.props.colDef.name === 'chargeUnit'))
        // {
        //     readonly = true;
        // }

        const switchValue = this.props.value;
        //Commented code because of D717 issue
        // if(this.props.colDef.name === "isInvoicePrintExpenseDescription" && (this.props.invoicingStatus === 'C' || this.props.invoicingStatus === 'D')) { 
        //     switchValue = true;
        // }

        // if(this.props.colDef.name === "isInvoicePrintPayRateDescrition" && this.props.costofSalesStatus === 'X') { 
        //     switchValue = true;
        // }

        if(this.props.colDef.type === 'InvoiceStatus') {
            if(this.props.value === 'C' || this.props.value === 'D') {
                return(      
                    <Fragment>
                        <span class="text-gold" title={localConstant.visit.CHARGE_SIDE_LOCK_MESSAGE}>
                            <i className="zmdi zmdi-lock zmdi-hc-2x"></i>    
                        </span>               
                    </Fragment> 
                );
            } else {
                return(      
                    <Fragment>                                         
                    </Fragment> 
                );
            }
        } else if (this.props.colDef.type === 'CostofSalesStatus') { 
            if(this.props.value === 'X') {
                return(                
                    <Fragment>      
                        <span class="text-gold" title={localConstant.visit.PAY_SIDE_LOCK_MESSAGE}>
                            <i className="zmdi zmdi-lock zmdi-hc-2x"></i>    
                        </span>                     
                    </Fragment> 
                );        
            } else if ((this.props.value === 'O' || this.props.value === 'E') && this.props.unPaidStatus && this.props.unPaidStatus === 'Rejected') {                
                return(                
                    <Fragment>      
                        <span class="text-red grid-invoice-status-font" title={localConstant.visit.PAY_SIDE_REJECTED_MESSAGE + this.props.unPaidStatusReason}>R</span>                     
                    </Fragment> 
                );
            } else if (this.props.value === 'S') {
                return(                
                    <Fragment>      
                        <span class="text-blue grid-invoice-status-font" title={localConstant.visit.PAY_SIDE_SUBMIT_MESSAGE}>S</span>                     
                    </Fragment> 
                );
            } else if ((this.props.value === 'O' || this.props.value === 'E') && this.props.unPaidStatus && this.props.unPaidStatus === 'On-hold') {                
                return(                
                    <Fragment>      
                        <span class="text-brown grid-invoice-status-font" title={localConstant.visit.PAY_SIDE_HOLD_MESSAGE + this.props.unPaidStatusReason}>H</span>                     
                    </Fragment> 
                );
            } else if (this.props.value === 'A' && this.props.unPaidStatus === null && this.props.unPaidStatusReason === null) {
                return(                
                    <Fragment>      
                        <span class="text-green grid-invoice-status-font" title={localConstant.visit.PAY_SIDE_APPROVE_MESSAGE}>A</span>                     
                    </Fragment> 
                );                
            } else {
                return(      
                    <Fragment>                                         
                    </Fragment> 
                );
            }
        } else if(this.props.colDef.type === 'select') {
            return(      
                <Fragment>
                    <CustomInput
                        hasLabel={false}
                        label={localConstant.companyDetails.Documents.SELECT_FILE_TYPE}
                        divClassName='pl-0 pr-0 width_95'
                        type='select'
                        labelClass="mandate"                        
                        required={true}
                        selected={true}
                        className="customInputs browser-default"
                        optionsList={this.props[this.props.colDef.selectListName]}
                        optionName={this.props.colDef.optionName}
                        optionValue={this.props.colDef.optionValue}
                        onSelectChange={(e) => this.formInputChangeHandler(e)}
                        name={this.props.colDef.name}
                        id={this.props.colDef.name + rowIndex}
                        defaultValue={this.props.value}                        
                        disabled={readonly}
                    />
                    { validation !== "" && 
                        <span class="text-red-inline grid-error-icon_font blink-image" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment> 
            );
        } else if(this.props.colDef.type === 'date') {
            return(
                <Fragment>
                     <CustomInput                      
                        isNonEditDateField={false}  
                        dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}                       
                        type='inlineDatepicker'
                        name={this.props.colDef.name}
                        htmlFor={this.props.colDef.name + rowIndex}
                        id={this.props.colDef.name + rowIndex}                        
                        divClassName={readonly ? "browser-default read-mode width_95" : " browser-default edit-mode width_95"}
                        selectedDate={dateUtil.defaultMoment(this.state.expenseDateState)}
                        onDateChange={(e) => this.formInputChangeHandler(e, this.props.colDef.name)}                           
                        onDatePickBlur={(e) => { this.handleDateChangeRaw(e, 'formInputChangeHandler'); }}                  
                        disabled={readonly}
                    />                   
                    { validation !== "" && 
                        <span class="text-red-inline grid-error-icon_font blink-image" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment>
            );
        } else if(this.props.colDef.type === 'textarea') {
            return(
                <CustomInput
                    hasLabel={false}
                    type="text"
                    divClassName='pl-0 pr-0'
                    colSize='s12'
                    inputClass="customInputs"
                    onValueChange={(e) => this.formInputChangeHandler(e)}
                    maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : fieldLengthConstants.common.selectType.DEFAULT_DESCRIPTION_MAXLENGTH}
                    name={this.props.colDef.name}
                    htmlFor={this.props.colDef.name + rowIndex}
                    id={this.props.colDef.name + rowIndex}
                    defaultValue={this.props.value}                     
                    readOnly={readonly}                 
                />
            );
        } else if(this.props.colDef.type === 'switch') {
            return(
                <CustomInput
                    type='switch'
                    //switchLabel={localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE}
                    isSwitchLabel={false}
                    switchName={this.props.colDef.name}                    
                    id={this.props.colDef.name + rowIndex}
                    colSize='s12 pl-0 pr-0'
                    divClassName='s12 pl-0 pr-0'
                    checkedStatus={switchValue}
                    onChangeToggle={(e) => this.formInputChangeHandler(e)}
                    switchKey={switchValue}                    
                    disabled={readonly}
                />
            );
        } else if(this.props.colDef.type === 'text') {            
            if(this.props.colDef.name === "chargeRate" || this.props.colDef.name === "payRate") {                
                return(     
                    <Fragment>
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='pl-0 pr-0'
                            label={localConstant.visit.WORK}
                            dataType='number'
                            colSize='s8'
                            inputClass="customInputs"
                            onValueChange={(e) => this.formInputChangeHandler(e)}
                            maxLength={fieldLengthConstants.common.selectType.VISIT_TIMESHEET_TEXT_MAXLENGTH}
                            name={this.props.colDef.name}
                            htmlFor={this.props.colDef.name + rowIndex}
                            id={this.props.colDef.name + rowIndex}                            
                            min="0"
                            onValueInput={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.decimalWithFourLimitFormat 
                                            : this.decimalWithTwoLimitFormat) : this.decimalWithTwoLimitFormat}
                            //onValueBlur={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.checkFourNumber 
                            //: this.checkTwoDigitNumber) : this.checkTwoDigitNumber}
                            defaultValue={ (isCHUser ?  "" :  this.props.modifyPayUnitPayRate ? FormatFourDecimal(0) : thousandFormat(this.props.value)) }
                            readOnly={readonly || this.props.ratesReadonly || this.props.modifyPayUnitPayRate || isCHUser || (this.props.lineItemPermision && this.props.lineItemPermision.isLoggedinCompany)}
                        />                        
                        { !readonly && !isCHUser &&
                            <button className="btn btn-small col s3 p-0" onClick={this.editRate}><i class="zmdi zmdi-long-arrow-down"></i></button>                        
                        }
                        { validation !== "" && 
                            <span class="text-red-inline grid-error-icon_font blink-image" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                        }
                    </Fragment>
                );
            } else if(this.props.colDef.moduleType === 'Travel') {
                    return(
                        <Fragment>          
                            <CustomInput
                                hasLabel={false}
                                type="text"
                                divClassName='pl-0 pr-0'                                
                                dataType='number'
                                colSize='s7'
                                inputClass="customInputs"
                                onValueChange={(e) => this.formInputChangeHandler(e)}
                                maxLength={fieldLengthConstants.common.selectType.VISIT_TIMESHEET_TEXT_MAXLENGTH}
                                name={this.props.colDef.name}
                                htmlFor={this.props.colDef.name + rowIndex}
                                id={this.props.colDef.name + rowIndex}
                                min="0"                                
                                onValueInput={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.decimalWithFourLimitFormat 
                                                : this.decimalWithTwoLimitFormat) : this.decimalWithTwoLimitFormat}
                                //onValueBlur={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.checkFourNumber 
                                //: this.checkTwoDigitNumber) : this.checkTwoDigitNumber}
                                defaultValue={ thousandFormat(FormatTwoDecimal(this.props.value))}
                                readOnly={readonly}
                            />
                            
                            {(this.props.colDef.name === 'chargeTotalUnit' || this.props.colDef.name === 'chargeUnit') && !readonly &&
                                <button className="btn btn-small col s3 p-0" onClick={this.swapTravelChargeUnit}><i class="zmdi zmdi-long-arrow-right"></i></button>
                            }
                            {this.props.colDef.name === 'payUnit' && !readonly &&
                                <button className="btn btn-small col s3 p-0" onClick={this.swapTravelPayUnit}><i class="zmdi zmdi-long-arrow-left"></i></button>
                            }
                            { validation !== "" && 
                                <span class="text-red-inline grid-error-icon_font blink-image" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                            }
                        </Fragment>
                    );
            } else {
                return(
                    <Fragment>          
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='pl-0 pr-0 width_95'
                            label={localConstant.visit.WORK}
                            dataType='number'
                            colSize='s12'
                            inputClass="customInputs"
                            onValueChange={(e) => this.formInputChangeHandler(e)}
                            maxLength={fieldLengthConstants.common.selectType.VISIT_TIMESHEET_TEXT_MAXLENGTH}
                            name={this.props.colDef.name}
                            htmlFor={this.props.colDef.name + rowIndex}
                            id={this.props.colDef.name + rowIndex}   
                            min="0"                         
                            onValueInput={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.decimalWithFourLimitFormat 
                                            : (this.props.colDef.decimalType === "six" ? this.decimalWithSixLimitFormat : this.decimalWithTwoLimitFormat)) : this.decimalWithTwoLimitFormat}
                            //onValueBlur={ this.props.colDef.decimalType ? (this.props.colDef.decimalType === "four" ? this.checkFourNumber 
                            //: (this.props.colDef.decimalType === "six" ? this.checkSixNumber :this.checkTwoDigitNumber)) : this.checkTwoDigitNumber}
                            defaultValue={ (isCHUser ? "" : this.props.colDef.decimalType === "six" ? thousandFormat(FormatSixDecimal(this.props.value)) : thousandFormat(FormatTwoDecimal(this.props.value))) }                            
                            readOnly={readonly || isCHUser}
                        />
                        { validation !== "" && 
                            <span class="text-red-inline grid-error-icon_font blink-image" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                        }
                    </Fragment>
                );
            }
        }
    }
}

export default SelectType;