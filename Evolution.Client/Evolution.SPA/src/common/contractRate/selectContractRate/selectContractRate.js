import React,{ Component, Fragment } from 'react';
import Flatpickr from 'react-flatpickr';
import CustomInput from '../../baseComponents/inputControlls'; 
import { 
    getlocalizeData,      
    isEmpty,
    numberFormat,
    thousandFormat,
    isEmptyOrUndefine
} from '../../../utils/commonUtils';
import moment from 'moment';
import LabelWithValue from '../../baseComponents/customLabelwithValue';
import dateUtil from '../../../utils/dateUtil';
import { required } from '../../../utils/validator';
const localConstant = getlocalizeData();

class SelectContractRate extends Component{
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state={
            contractDate: ''
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
                this.setState({ contractDate: '' });
                this[type]('', this.props.colDef.name);                
            }
        }
    }
    chargeRateDetailValidation(row) {
        row["effectiveFromValidation"] = "";
       
        if(required(row.effectiveFrom)){            
            row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_VAL;
        }
        if(!required(row.effectiveFrom) && !this.isValidDateFormat(row.effectiveFrom)) {
            row["effectiveFromValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveFrom"] = "";
        }
        if(!required(row.effectiveTo) && !this.isValidDateFormat(row.effectiveTo)) {
            row["effectiveToValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveTo"] = "";
        }
        if (!required(row.effectiveFrom) && !required(row.effectiveTo) && this.isValidDateFormat(row.effectiveFrom) && this.isValidDateFormat(row.effectiveTo)
                && moment(row.effectiveFrom).isAfter(row.effectiveTo, 'day')) {
            row["effectiveFromValidation"] = localConstant.techSpecValidationMessage.PAY_RATE_DATE_RANGE_VALIDATION;
        }
        if(!isEmpty(this.props.contractInfo.contractStartDate)){//D589
            if(moment(row.effectiveFrom).isBefore(this.props.contractInfo.contractStartDate)){
                row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_BEFORE_VALIDATION;
            }
        }        
        return row;
    }
    isValidDateFormat(date) {        
        if(date.indexOf(":") === -1 && dateUtil.isUIValidDate(date)) {
            return false;
        }
        return true;
    } 
    /* Form Input data Change Handler*/
    formInputChangeHandler = async(e, name) => {      
        this.updatedData = {};
        let result = {};             
        if(name && (name === 'effectiveFrom' || name === 'effectiveTo')) {
            const date = (e && e !== '' ? this.expenseDateFormat(e) : '');
            this.updatedData[name] = date;
            result = { value: date, name: name };
            this.setState({ contractDate:date });
        } else {
            result = this.inputChangeHandler(e);        
            if(result.name && result.name == 'isActive')
               this.updatedData[result.name] = !(result.value);
            else
               this.updatedData[result.name] = result.value;
        }
        if(this.props.data && this.props.data.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }

        let chargeType = Object.assign({}, this.props.data, this.updatedData);
        chargeType=this.chargeRateDetailValidation(chargeType);  //Added for Defect 883
        if (this.props.colDef.moduleType === "chargeRate") {
            // if (result.name === 'chargeValue' || result.name === 'standardValue') {
            //     if (chargeType["standardValue"] && chargeType["chargeValue"]) {
            //         chargeType["discountApplied"] = parseFloat(chargeType["chargeValue"]) - parseFloat(chargeType["standardValue"]);
            //         if (chargeType["standardValue"] === "0.00")
            //             chargeType["percentage"] = "0.00";
            //         else
            //             chargeType["percentage"] = (chargeType["chargeValue"] - chargeType["standardValue"]) / chargeType["standardValue"] * 100;
            //     }
            // }
            const res = await this.props.actions.UpdateChargeType(chargeType);
            // if (res) { //Prod def 651 issue fix
            //     if (name && (name === 'effectiveFrom' || name === 'effectiveTo')) {
            //        this.props.editAction(this.state.contractDate);
            //     }
            // }
        } else {
            if (result.name === 'scheduleName') {
                this.props.chargeTypes && this.props.chargeTypes.map(iteratedValue => {
                    if (iteratedValue.scheduleId === this.props.data.scheduleId) {
                        iteratedValue.scheduleName = result.value;
                        this.props.actions.UpdateChargeType(iteratedValue);
                    }
                });
            }
            this.props.actions.UpdateRateSchedule(chargeType);
        }
    }

    decimalWithFourLimitFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(14)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)"); // As request from francina - 17-12-20
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    };

    decimalWithTwoLimitFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(2) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();        
    };

    checkFourNumber = (e) => {        
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) ){
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);            
        }
        this.formInputChangeHandler(e);
    }

    checkTwoDigitNumber = (e) => {
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) ){            
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        }
        this.formInputChangeHandler(e);
    }

    FormatFourDecimal(value) {
        if(value === null || value === undefined) {
            return "0.0000";
        } else if(value === "" || value === "NaN" ) {
            return "";
        } else {
            if(isNaN(value)) {
                return parseFloat(numberFormat(value)).toFixed(4);
            } else {
                return parseFloat(value).toFixed(4);
            }
        }
    }

    FormatTwoDecimal(value) {
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

    componentDidMount() {
        if(this.props.colDef.type === 'date') {
            this.setState({ contractDate:this.props.value });
        }
    }

    render(){
        const rowIndex = this.props.rowIndex !== undefined ? this.props.rowIndex : 0;
        const validation = this.props.validation ? this.props.validation : "";
        const readonly = (this.props.readonly || this.props.isScheduleBatchProcess) ? true : false;        
        if(this.props.colDef.type === 'select') {            
            return(      
                <Fragment>
                    <CustomInput
                        hasLabel={false}                        
                        divClassName='s12'
                        optionClassName='chargeTypeOption'
                        optionClassArray = {localConstant.commonConstants.NDT_CHARGE_RATE_TYPE}
                        optionProperty = "chargeType"
                        type='select'
                        labelClass="mandate"
                        colSize="s12"
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
                        <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
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
                        divClassName={readonly ? "browser-default read-mode" : " browser-default edit-mode"}
                        selectedDate={dateUtil.defaultMoment(this.state.contractDate)}
                        onDateChange={(e) => this.formInputChangeHandler(e, this.props.colDef.name)}                           
                        onDatePickBlur={(e) => { this.handleDateChangeRaw(e, 'formInputChangeHandler'); }}                  
                        disabled={readonly}
                    />  
                    { validation !== "" && 
                        <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment>
            );
        } else if(this.props.colDef.type === 'textarea') {
            return(
                <CustomInput
                    hasLabel={false}
                    type="text"
                    divClassName='s12'
                    colSize='s12'
                    inputClass="customInputs"
                    onValueChange={(e) => this.formInputChangeHandler(e)}                    
                    maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : 500}
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
                    isSwitchLabel={false}
                    switchName={this.props.colDef.name}
                    id={this.props.colDef.name + rowIndex}
                    colSize='s12'
                    divClassName='s12'
                    checkedStatus={this.props.value === 'Yes' ? true : false}
                    onChangeToggle={(e) => this.formInputChangeHandler(e)}
                    switchKey={this.props.value === 'Yes' ? true : false}                    
                    disabled={readonly}
                />
            );
        } else if (this.props.colDef.type === 'textbox') {
            return(
                <Fragment>
                    <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='s12'
                            colSize='s12'
                            inputClass="customInputs"
                            onValueChange={(e) => this.formInputChangeHandler(e)}
                            maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : 100}
                            name={this.props.colDef.name}
                            htmlFor={this.props.colDef.name + rowIndex}
                            id={this.props.colDef.name + rowIndex}
                            defaultValue={this.props.value}                     
                            readOnly={readonly}                 
                    />
                    { validation !== "" && 
                            <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment>
            );
        } else if(this.props.colDef.type === 'text') {
            return(     
                <Fragment>
                    <CustomInput
                        hasLabel={false}
                        type="text"
                        label={localConstant.visit.WORK}
                        dataType='number'
                        divClassName='s12'
                        colSize='s12'
                        inputClass={this.props.value ? this.props.value < 0 ? "customInputs text-red" : "customInputs" : "customInputs"}
                        onValueChange={(e) => this.formInputChangeHandler(e)}
                        maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : 18} // As request from francina - 17-12-20
                        name={this.props.colDef.name}
                        htmlFor={this.props.colDef.name + rowIndex}
                        id={this.props.colDef.name + rowIndex}
                        min="0"
                        onValueInput={ this.props.colDef.decimalType ? this.decimalWithTwoLimitFormat : this.decimalWithFourLimitFormat}
                        //onValueBlur={ this.props.colDef.decimalType ? this.checkTwoDigitNumber : this.checkFourNumber }
                        defaultValue={ this.props.colDef.decimalType ? thousandFormat(this.props.value) : thousandFormat(this.props.value)}
                        readOnly={readonly}
                    />
                    { validation !== "" && 
                        <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment>
            );            
        } else if(this.props.colDef.type === 'label') { 
            return(  
                <Fragment>
                { (this.props.data.scheduleValidation !== undefined && this.props.data.scheduleValidation !== "") && 
                        <span class="text-red grid-error-icon_font" title={this.props.data.scheduleValidation}><i class="zmdi zmdi-alert-triangle grid-mandate-triangle"></i></span>
                }
                     <LabelWithValue 
                         value={this.props.labelValue}
                     /> 
            </Fragment>
            );
        }
    }
}

export default SelectContractRate;