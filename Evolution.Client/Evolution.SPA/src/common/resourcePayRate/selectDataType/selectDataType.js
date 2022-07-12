import React,{ Component, Fragment } from 'react';
import Flatpickr from 'react-flatpickr';
import CustomInput from '../../baseComponents/inputControlls'; 
import { 
    getlocalizeData,      
    isEmpty,
    numberFormat,
    thousandFormat
} from '../../../utils/commonUtils';
import moment from 'moment';
import LabelWithValue from '../../baseComponents/customLabelwithValue'; //Added for D634 issue 2
import dateUtil from '../../../utils/dateUtil';
import { DecimalWithLimitFormat } from '../../../utils/stringUtil';

const localConstant = getlocalizeData();

class SelectDataType extends Component{
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state={
            resourceDate: ''
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
                this.setState({ resourceDate: '' });
                this[type]('', this.props.colDef.name);                
            }
        }
    }

    /* Form Input data Change Handler*/
    formInputChangeHandler = (e, name) => {         
        this.updatedData = {};
        let result = {};        
        if(name && (name === 'effectiveFrom' || name === 'effectiveTo')) {
            const date = (e && e !== '' ? this.expenseDateFormat(e) : '');
            this.updatedData[name] = date;
            result = { value: date, name: name };
            this.setState({ resourceDate:date });
        } else {
            result = this.inputChangeHandler(e); 
            if(result.name === 'payCurrency' && !isEmpty(result.value)){ //D774 #34 issue    
                this.updatedData["payCurrencyValidation"]="";
                this.updatedData[result.name] = result.value;//D954 
            }     
            if(result.name && result.name === 'isActive'){
                this.updatedData[result.name] = !(result.value);
            } else {
                this.updatedData[result.name] = result.value;
            }
        }
        if(this.props.data && this.props.data.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
      //  this.updatedData["oldScheduleName"] = this.props.data.payScheduleName;
        if(this.props.colDef.moduleType === "payRate") { 
            this.props.actions.UpdateDetailPayRate(this.updatedData, this.props.data);
        } else {
            const paySchedule = Object.assign({}, this.props.data, this.updatedData);
            if(result.name === 'payScheduleName') {         
                this.props.DetailPayRate && this.props.DetailPayRate.map(iteratedValue => {                                       
                    if (iteratedValue.payScheduleId === paySchedule.id) {     //D384(foreignKey issue)                    
                        iteratedValue.payScheduleName = result.value;                        
                        this.props.actions.UpdateDetailPayRate(iteratedValue, iteratedValue);
                    }
                });             
            }
            //D384
            if(result.name === 'payCurrency'){
                this.props.DetailPayRate && this.props.DetailPayRate.map(iteratedValue => {                                       
                    if (iteratedValue.payScheduleName === this.props.data.payScheduleName) {                        
                        iteratedValue.payScheduleCurrency = result.value;                        
                        this.props.actions.UpdateDetailPayRate(iteratedValue, iteratedValue);
                    }
                });  
            }
            this.props.actions.UpdatePayRateSchedule(paySchedule);
        }        
    }

    onValueInput = (evt,wholeNOLimit,decimalLimit) => {  
        const e = evt || window.event;   
        e.target.value = DecimalWithLimitFormat(e.target.value,wholeNOLimit,decimalLimit); 
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

    componentDidMount() {
        if(this.props.colDef.type === 'date') {
            this.setState({ resourceDate:this.props.value });
        }
    }

    render(){
        const rowIndex = this.props.rowIndex !== undefined ? this.props.rowIndex : 0;
        const validation = this.props.validation ? this.props.validation : "";
        const readonly = this.props.readonly ? this.props.readonly : false;     

        if(this.props.colDef.type === 'select') {
            return(      
                <Fragment>
                    <CustomInput
                        hasLabel={false}
                        label={localConstant.companyDetails.Documents.SELECT_FILE_TYPE}
                        divClassName='s12'
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
                        selectedDate={dateUtil.defaultMoment(this.state.resourceDate)}
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
                    id={this.props.colDef.id + rowIndex}
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
            if(this.props.colDef.moduleType==='payRate')
            {
                return(     
                    <Fragment>
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            label={localConstant.visit.WORK}
                            dataType='number'
                            divClassName='s12'
                            colSize='s12'
                            inputClass="customInputs"
                            onValueChange={(e) => this.formInputChangeHandler(e)}                        
                            maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : 20}
                            name={this.props.colDef.name}
                            htmlFor={this.props.colDef.name + rowIndex}
                            id={this.props.colDef.name + rowIndex}
                            min="0"
                            onValueInput={ (e) => this.onValueInput(e,7,4) } //def 897, 1016 db synch overflow issue
                            onValueBlur={this.checkFourNumber}
                            defaultValue={ thousandFormat(this.FormatFourDecimal(this.props.value))}
                            readOnly={readonly}
                        />
                        { validation !== "" && 
                            <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                        }
                    </Fragment>
                );
            }else{
            return(     
                <Fragment>
                    <CustomInput
                        hasLabel={false}
                        type="text"
                        label={localConstant.visit.WORK}
                        dataType='number'
                        divClassName='s12'
                        colSize='s12'
                        inputClass="customInputs"
                        onValueChange={(e) => this.formInputChangeHandler(e)}                        
                        maxLength={this.props.colDef.maxLength ? this.props.colDef.maxLength : 20}
                        name={this.props.colDef.name}
                        htmlFor={this.props.colDef.name + rowIndex}
                        id={this.props.colDef.name + rowIndex}
                        min="0"
                        onValueInput={ (e) => this.onValueInput(e,10,4)  }
                        onValueBlur={this.checkFourNumber}
                        defaultValue={ thousandFormat(this.FormatFourDecimal(this.props.value))}
                        readOnly={readonly}
                    />
                    { validation !== "" && 
                        <span class="text-red grid-error-icon_font" title={validation}><i class="zmdi zmdi-alert-triangle"></i></span>
                    }
                </Fragment>
            ); 
                }           
        } else if(this.props.colDef.type === 'label') { 
            return(  
                <Fragment>
                 {/* { (this.props.data.scheduleValidation !== undefined && this.props.data.scheduleValidation !== "") && 
                        <span class="text-red grid-error-icon_font" title={this.props.data.scheduleValidation}><i class="zmdi zmdi-alert-triangle grid-mandate-triangle"></i></span>
                } */}
                     <LabelWithValue 
                        isValidation = {true}
                        validationMsg = {this.props.data.scheduleValidation}
                         value={this.props.labelValue}
                     /> 
            </Fragment>
            );
        }
    }
}

export default SelectDataType;