import React, { Component,Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, 
    formInputChangeHandler,
    bindAction,
    bindActionWithChildLevel,
    isEmptyReturnDefault,
    isEmptyOrUndefine,
    isEmpty,
    numberFormat } from '../../../../utils/commonUtils';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './techSpecHeader';
import Modal from '../../../../common/baseComponents/modal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { Tab, Tabs,TabList,TabPanel } from 'react-tabs';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import moment from 'moment';
import { required,requiredNumeric } from '../../../../utils/validator';
import dateUtil from '../../../../utils/dateUtil';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { GetScheduleDefaultCurrency } from '../../../../selectors/timesheetSelector';
import { sortCurrency } from '../../../../utils/currencyUtil';
import { 
    FormatTwoDecimal,
    FormatFourDecimal,
    FormatSixDecimal,
    getTravelDefaultPayType
} from '../../../../common/inlineEditVisitTimesheet/visitTimesheetUtil';

const localConstant = getlocalizeData();
const currentDate = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
const TimePopup = (props) => {
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                name="expenseDate"
                colSize='s4'
                label={localConstant.timesheet.DATE}
                type='date'
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                // selectedDate={(props.editedTimeData && props.editedTimeData.expenseDate)
                //      ?dateUtil.defaultMoment(props.editedTimeData.expenseDate):moment() }
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                labelClass="mandate"
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                optionClassName='danger-txt'
                optionClassArray = {localConstant.commonConstants.NDT_CHARGE_RATE_TYPE}
                optionProperty = "chargeType"
                label={localConstant.timesheet.TYPE}
                type='select'
                name='chargeExpenseType'
                colSize='s8 pl-0'
                className="browser-default"
                labelClass="mandate"
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeExpenseType}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />              
              
            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col'              
                label={localConstant.timesheet.FULL_DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='expenseDescription'
                defaultValue={(props.editedTimeData && props.editedTimeData.expenseDescription)?props.editedTimeData.expenseDescription:''}                    
                readOnly={!props.lineItemPermision.isLineItemEditable}
            />
            <CustomInput
                type='switch'
                switchLabel={localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE}
                isSwitchLabel={true}
                switchName="isInvoicePrintExpenseDescription"
                id="isInvoicePrintExpenseDescription"
                colSize='s12'
                checkedStatus={props.editedTimeData && props.editedTimeData.isInvoicePrintExpenseDescription}
                onChangeToggle={props.onChangeHandler}
                switchKey={props.editedTimeData && props.editedTimeData.isInvoicePrintExpenseDescription}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            
            <h6 className="col s12 p-0 mt-2">{localConstant.timesheet.CHARGE}</h6> 
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.UNIT}                
                dataType='decimal'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeTotalUnit'
                min="0"
                defaultValue={(props.editedTimeData && props.editedTimeData.chargeTotalUnit) 
                    ? props.editedTimeData.chargeTotalUnit : '0'}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}    
            />
            
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.RATE}
                dataType='number'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                defaultValue={(props.editedTimeData && props.editedTimeData.chargeRate)
                     ?props.editedTimeData.chargeRate : '0.0000'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />        

            <CustomInput
                hasLabel={true}             
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s4 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeRateCurrency}
                disabled = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />

            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col'
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='chargeRateDescription'
                defaultValue={props.editedTimeData && props.editedTimeData.chargeRateDescription}
                readOnly = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />
              
            <h6 className="col s12 p-0 mt-2">{localConstant.timesheet.PAY}</h6>
                
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payUnit'
                min="0"
                isLimitType={2}
                value={props.payUnit}
                 //defaultValue={(props.editedTimeData && props.editedTimeData.payUnit) ?
                 //props.editedTimeData.payUnit:'0'}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide}
            />
                             
            <CustomInput
                hasLabel={true}
                type="text"
                label={localConstant.timesheet.RATE}
                dataType='number'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                defaultValue={(props.editedTimeData && props.editedTimeData.payRate) ?
                     props.editedTimeData.payRate:'0.0000'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide}
            />        

            <CustomInput
                hasLabel={true}
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='payRateCurrency'
                colSize='s4 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTimeData && props.editedTimeData.payRateCurrency}
                disabled = {!props.lineItemPermision.isLineItemEditable
                            || !props.lineItemPermision.isLineItemEditableOnPaySide}
            />

            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col'
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='payRateDescription'
                defaultValue={props.editedTimeData && props.editedTimeData.payRateDescription}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                            || !props.lineItemPermision.isLineItemEditableOnPaySide}                
            />
        </Fragment>
    );
};

const ExpensePopup = (props)=>{
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                name="expenseDate"
                colSize='s3'
                label={localConstant.timesheet.DATE}
                type='date'
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                labelClass="mandate"
                disabled={!props.lineItemPermision.isLineItemEditable}
            />

            <CustomInput
                hasLabel={true}             
                label={localConstant.timesheet.TYPE}
                type='select'
                name='chargeExpenseType'
                colSize='s3 pl-0'
                className="browser-default"
                labelClass="mandate"
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedExpenseData && props.editedExpenseData.chargeExpenseType}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
                    
            <CustomInput
                hasLabel={true}               
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='currency'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedExpenseData && props.editedExpenseData.currency}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            <CustomInput
                type='switch'
                switchLabel={localConstant.timesheet.CHEXP}
                isSwitchLabel={true}
                switchName="isContractHolderExpense"
                id="isContractHolderExpense"
                colSize='s3'
                disabled={!props.lineItemPermision.isLineItemEditable}
                checkedStatus={props.editedExpenseData && props.editedExpenseData.isContractHolderExpense}
                onChangeToggle={props.onChangeHandler}
                switchKey={props.editedExpenseData && props.editedExpenseData.isContractHolderExpense}
            />       

            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col'              
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='expenseDescription'
                value={props.editedTimeData && props.editedTimeData.expenseDescription}
                readOnly={!props.lineItemPermision.isLineItemEditable}                    
            />

            <h6 className="col s12 p-0 mt2">{localConstant.timesheet.CHARGE}</h6>

            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                colSize='s3'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeUnit'
                min="0"
                defaultValue={(props.editedExpenseData && props.editedExpenseData.chargeUnit) 
                    ? props.editedExpenseData.chargeUnit : '0'}
                    readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
                               
            <CustomInput
                hasLabel={true}
                type="text"
                label={localConstant.timesheet.RATE}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                defaultValue={(props.editedExpenseData && props.editedExpenseData.chargeRate)
                    ?props.editedExpenseData.chargeRate : '0.0000'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"
                dataType="decimal"
                valueType ="value"
                label={localConstant.timesheet.EXCHANGE}                  
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={200}
                name='chargeRateExchange'
                //value={(props.editedExpenseData && props.editedExpenseData.chargeRateExchange)?props.editedExpenseData.chargeRateExchange:''}
                value={(props.chargeRateExchange)?props.chargeRateExchange:''}
                readOnly = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}                
            />

            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedExpenseData && props.editedExpenseData.chargeRateCurrency}
                disabled ={props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />
              
            <h6 className="col s12 p-0 mt-1">{localConstant.timesheet.PAY}</h6>
            
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payUnit'
                min="0"
                isLimitType={2}
                value={props.payUnit}
                // defaultValue={(props.editedExpenseData && props.editedExpenseData.payUnit)
                //     ?props.editedExpenseData.payUnit : '0'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.NETT_UNIT}
                dataType='number'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                // defaultValue={props.editedExpenseData && props.editedExpenseData.payRate}
                defaultValue={(props.editedExpenseData && props.editedExpenseData.payRate)
                    ?props.editedExpenseData.payRate : '0.0000'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.TAX}
                dataType='decimal'
                colSize='s4 pl-0'
                inputClass="customInputs"
                labelClass="mandate"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRateTax'
                min="0"
                // defaultValue={props.editedExpenseData && props.editedExpenseData.payRateTax}
                defaultValue={(props.editedExpenseData && props.editedExpenseData.payRateTax)
                    ?props.editedExpenseData.payRateTax : '0.00'}
                    readOnly = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.GROSS}
                dataValType='valueText'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='grossValue'
                min="0"
                readOnly={true}
                value={props.grossValue}
            />
             
            <CustomInput
                hasLabel={true}
                type="text"    
                dataType="decimal"
                valueType ="value"           
                label={localConstant.timesheet.EXCHANGE}                  
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={200}
                name='payRateExchange'
                // defaultValue={props.editedExpenseData && props.editedExpenseData.payRateExchange}
                value={(props.payRateExchange)?props.payRateExchange:''}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }                 
            />

            <CustomInput
                hasLabel={true}           
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='payRateCurrency'
                colSize='s4 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedExpenseData && props.editedExpenseData.payRateCurrency}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }    
            />
        </Fragment>
    );   
};

const TravelPopup = (props)=>{
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                name="expenseDate"
                colSize='s4'
                label={localConstant.timesheet.DATE}
                type='date'
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                labelClass="mandate"
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            
            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col pl-0'
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s8'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='expenseDescription'    
                value={props.editedTravelData && props.editedTravelData.expenseDescription}
                readOnly={!props.lineItemPermision.isLineItemEditable}                   
            />
            
            <CustomInput
                type='switch'
                switchLabel={localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE}
                isSwitchLabel={true}
                switchName="isInvoicePrintExpenseDescription"
                id="isInvoicePrintExpenseDescription"
                colSize='s12'
                checkedStatus={props.editedTravelData && props.editedTravelData.isInvoicePrintExpenseDescription}
                onChangeToggle={props.onChangeHandler}
                switchKey={props.editedTravelData && props.editedTravelData.isInvoicePrintExpenseDescription}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />

            <h6 className="col s12 p-0 mt-2">{localConstant.timesheet.CHARGE}</h6>

            <CustomInput
                hasLabel={true}
                label={localConstant.timesheet.CHARGE_TYPE}
                type='select'
                name='chargeExpenseType'
                colSize='s3'
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTravelData && props.editedTravelData.chargeExpenseType}
                labelClass="mandate"
                disabled = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            /> 

            <CustomInput
                hasLabel={true}
                type="text"              
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s2 pl-0 pr-1'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeUnit'
                min="0"
                value={props.chargeUnit}
                isLimitType={2}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
            <button className="btn btn-small col mt-4x" onClick={props.swapDownHandler}><i class="zmdi zmdi-long-arrow-down"></i></button>       
                                
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.timesheet.RATE}
                dataType='number'
                //valueType='value'
                colSize='s3 pr-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                // defaultValue={props.editedTravelData && props.editedTravelData.chargeRate}
                defaultValue={(props.editedTravelData && props.editedTravelData.chargeRate) 
                    ? props.editedTravelData.chargeRate : '0.0000'}
                //value={props.chargeRate}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
                />

            <CustomInput
                hasLabel={true}                
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s3'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTravelData && props.editedTravelData.chargeRateCurrency}
                disabled = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />
              
            <h6 className="col s12 p-0 mt-1">{localConstant.timesheet.PAY}</h6>

            <CustomInput
                hasLabel={true}               
                label={localConstant.timesheet.PAY_TYPE}
                type='select'
                name='payExpenseType'
                colSize='s3 '
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                labelClass="mandate"
                defaultValue={props.editedTravelData && props.editedTravelData.payExpenseType}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />            

            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s2 pr-1 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payUnit'
                min="0"
                isLimitType={2}
                // defaultValue={props.editedTravelData && props.editedTravelData.payUnit}
                // defaultValue={(props.editedTravelData && props.editedTravelData.payUnit) 
                //     ? props.editedTravelData.payUnit : '0'}
                value={props.payUnit}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
            <button className="btn btn-small col mt-4x" onClick={props.swapUpHandler}><i class="zmdi zmdi-long-arrow-up"></i></button>      
                              
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.RATE}
                dataType='number'
                //valueType='value'
                colSize='s3 pr-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                // defaultValue={props.editedTravelData && props.editedTravelData.payRate}
                 defaultValue={(props.editedTravelData && props.editedTravelData.payRate) 
                     ? props.editedTravelData.payRate : '0.0000'}
                //value={props.payRate}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />

            <CustomInput
                hasLabel={true}                
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='payRateCurrency'
                colSize='s3'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTravelData && props.editedTravelData.payRateCurrency}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
        </Fragment>
    );   
};

const ConsumablePopup =(props)=>{
    return(
        <Fragment>            
            <CustomInput
                hasLabel={true}
                name="expenseDate"
                colSize='s4 pl-0'
                label={localConstant.timesheet.DATE}
                type='date'                
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            <CustomInput
                type='switch'
                switchLabel={localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE}
                isSwitchLabel={true}
                switchName="isInvoicePrintChargeDescription"
                id="isInvoicePrintChargeDescription"
                colSize='s12'
                checkedStatus={props.editedConsumableData && props.editedConsumableData.isInvoicePrintChargeDescription}
                onChangeToggle={props.onChangeHandler}
                switchKey={props.editedConsumableData && props.editedConsumableData.isInvoicePrintChargeDescription}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
           
            <h6 className="col s12 p-0 mt-2 ">{localConstant.timesheet.CHARGE}</h6>

            <CustomInput
                hasLabel={true}            
                label={localConstant.timesheet.CHARGE_TYPE}
                optionClassName='danger-txt'
                optionClassArray = {localConstant.commonConstants.NDT_CHARGE_RATE_TYPE}
                optionProperty = "chargeType"
                type='select'
                name='chargeExpenseType'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedConsumableData && props.editedConsumableData.chargeExpenseType}
                disabled ={props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            /> 

            <CustomInput
                hasLabel={true}
                type="text"              
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeUnit'
                min="0"
                // defaultValue={props.editedConsumableData && props.editedConsumableData.chargeUnit}
                defaultValue={(props.editedConsumableData && props.editedConsumableData.chargeUnit) 
                    ? props.editedConsumableData.chargeUnit : '0'}
                disabled = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
                                
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.RATE}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                // defaultValue={props.editedConsumableData && props.editedConsumableData.chargeRate}
                defaultValue={(props.editedConsumableData && props.editedConsumableData.chargeRate) 
                    ? props.editedConsumableData.chargeRate : '0.0000'}
                disabled = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />       

            <CustomInput
                hasLabel={true}
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedConsumableData && props.editedConsumableData.chargeRateCurrency}
                disabled = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />
               <CustomInput
                hasLabel={true}
                divClassName='col'
                type="textarea"
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s12 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='chargeDescription'
                value={props.editedConsumableData && props.editedConsumableData.chargeDescription}   
                disabled ={props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}                 
            />
            <h6 className="col s12 p-0 mt-2">{localConstant.timesheet.PAY}</h6>
           
            <CustomInput
                hasLabel={true}              
                label={localConstant.timesheet.PAY_TYPE}
                optionClassName='danger-txt'
                optionClassArray = {localConstant.commonConstants.NDT_CHARGE_RATE_TYPE}
                optionProperty = "chargeType"
                type='select'
                name='payType'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedConsumableData && props.editedConsumableData.payType}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"
                label={localConstant.timesheet.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payUnit'
                min="0"
                isLimitType={2}
                value={props.payUnit}
                // defaultValue={(props.editedConsumableData && props.editedConsumableData.payUnit) 
                //     ? props.editedConsumableData.payUnit : '0'}
                disabled = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                              
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.timesheet.RATE}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                // defaultValue={props.editedConsumableData && props.editedConsumableData.payRate}
                defaultValue={(props.editedConsumableData && props.editedConsumableData.payRate) 
                    ? props.editedConsumableData.payRate : '0.0000'}
                disabled = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />      

            <CustomInput
                hasLabel={true}           
                label={localConstant.timesheet.CURRENCY}
                type='select'
                name='payRateCurrency'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedConsumableData && props.editedConsumableData.payRateCurrency}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
             <CustomInput
                hasLabel={true}
                divClassName='col'
                type="textarea"
                label={localConstant.timesheet.DESCRIPTION}                      
                colSize='s12 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='payRateDescription'
                value={props.editedConsumableData && props.editedConsumableData.payRateDescription} 
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }                   
            />
        </Fragment>
    );   
};

const ScheduleRate = (props) => {
    // const chargeSchedules = isEmptyReturnDefault(props.techSpecRateSchedules.chargeSchedules);
    // const paySchedules = isEmptyReturnDefault(props.techSpecRateSchedules.paySchedules);

    const chargeSchedules = [];   
    const paySchedules = [];   
    const pin = props.pin;

    const payRateGridData = isEmptyReturnDefault(props.techSpecRateSchedules);        
    if(!isEmptyOrUndefine(payRateGridData.paySchedules)) {        
        payRateGridData.paySchedules.forEach(row => {
                if(row.epin === pin && row.technicalSpecialistCompanyCode === props.selectedCompany) 
                {
                    row.payScheduleRates = row.payScheduleRates.filter(x =>  x.isActive);
                    const payRates = [];                    
                    row.payScheduleRates.forEach(payRate => {
                        const rate = payRate;
                        rate.updatedPayRate = rate.payRate;
                        if(!props.modifyPayUnitPayRate && payRate.type === 'R') rate.updatedPayRate = null;
                        payRates.push(payRate);
                    });
                    row.payScheduleRates = payRates;
                    paySchedules.push(row);
                }
        });
    }    
      
    const chargeRateGridData = isEmptyReturnDefault(props.techSpecRateSchedules);    
    if(!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {        
        chargeRateGridData.chargeSchedules.forEach(row => {
            if(row.epin === pin){
                row.chargeScheduleRates = row.chargeScheduleRates.filter(x =>  x.isActive);
                chargeSchedules.push(row);
            }
        });
    }
    // this.editedRowData = {};
    return (
        <Fragment>
             <CardPanel title="Charge Schedule Rates" className="white lighten-4 black-text" colSize="s12 mb-2">
               <ReactGrid 
                    gridRowData={chargeSchedules} 
                    gridColData={props.gridHeader.chargeScheduleRates} 
                    isGrouping={true} 
                    groupName='contractScheduleName' 
                    dataName='chargeScheduleRates'/>
             </CardPanel>
             <CardPanel title="Pay Schedule Rates"  className="white lighten-4 black-text" colSize="s12 mb-2">
                <ReactGrid 
                    gridRowData={paySchedules} 
                    gridColData={props.gridHeader.payScheduleRate} 
                    isGrouping={true} 
                    groupName='technicalSpecialistPayScheduleName' 
                    dataName='payScheduleRates'/>
            </CardPanel>
            </Fragment>
            );
};

const TimeModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
            <TimePopup 
                editedTimeData = {props.editedTimeData}
                currencyMasterData = {props.currencyMasterData}
                chargeType = {props.chargeType}
                onChangeHandler = {props.onChangeHandler}
                expenseDate = {props.expenseDate}
                expenseDateChange = { props.expenseDateChange }
                lineItemPermision = {props.lineItemPermision}
                isNewRecord={props.isNewRecord}
                payUnit = {props.payUnit} 
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}           
            /> 
            </div>
        </Fragment>
    );
};

const ExpensesModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
            <ExpensePopup 
                editedExpenseData = {props.editedExpenseData}
                currencyMasterData = {props.currencyMasterData}
                chargeType = {props.chargeType}
                onChangeHandler = {props.onChangeHandler}
                expenseDate = {props.expenseDate}
                expenseDateChange = { props.expenseDateChange }
                grossValue={props.grossValue}
                lineItemPermision = {props.lineItemPermision}
                isNewRecord={props.isNewRecord}
                chargeRateExchange = {props.chargeRateExchange}
                payRateExchange =  {props.payRateExchange}
                payUnit = {props.payUnit} 
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}           
            /> 
            </div>
        </Fragment>
    );
};

const TravelModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
            <TravelPopup 
                editedTravelData = {props.editedTravelData}
                currencyMasterData = {props.currencyMasterData}
                chargeType = {props.chargeType}
                onChangeHandler = {props.onChangeHandler}
                expenseDate = {props.expenseDate}
                expenseDateChange = { props.expenseDateChange }
                lineItemPermision = {props.lineItemPermision}
                isNewRecord={props.isNewRecord}
                swapUpHandler={props.swapUpHandler}
                swapDownHandler={props.swapDownHandler}
                chargeUnit={props.chargeUnit}
                payUnit={props.payUnit}
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}
            /> 
            </div>
        </Fragment>
    );
};

const ConsumableModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
            <ConsumablePopup 
                editedConsumableData = {props.editedConsumableData}
                currencyMasterData = {props.currencyMasterData}
                chargeType = {props.chargeType}
                onChangeHandler = {props.onChangeHandler}
                expenseDate = {props.expenseDate}
                expenseDateChange = { props.expenseDateChange }
                lineItemPermision = {props.lineItemPermision}
                isNewRecord={props.isNewRecord}
                payUnit={props.payUnit}
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}
            /> 
            </div>
        </Fragment>
    );
};
 
class TechnicalSpecialistAccounts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            scheduleRateShowModal:false,
            payRateShowModal: false,
            timeModel:false,
            expensesShowModal:false,
            travelShowModal:false,
            consumableShowModal:false,
            expenseDate: '',
            isNewRecord: false,
            timesheetTechSpecTimes: [],
            timesheetTechSpecTravels: [],
            timesheetTechSpecExpenses: [],
            timesheetTechSpecConsumables: [],
            selectedPayRateGridData: [],
            isAddUnlinkedAssignmentExpenses:false,
            chargeUnit:0,
            payUnit:0,
            isShowAdditionalExpenses: false,
            selectedTabIndex: 0,
            isPayRate: false,
            isDatePicker: false,
            moduleType: "R"
        };
        this.updatedData = {};
        this.editedRowData={};
        this.props.callBackFuncs.onCancel =()=>{
            this.clearLineItemsInState();
        };
        this.props.callBackFuncs.onTimesheetCancel =()=>{
            this.clearLineItemsOnCancel();
        };

        this.props.callBackFuncs.reloadLineItemsOnSaveValidation =()=>{
            if(this.gridChild) {
                const selectedData = this.gridChild.getSelectedRows();                
                if(selectedData !== undefined) {                  
                    this.bindTechSpecLineItems(selectedData[0].pin);
                }
            }
        };

        const functionRefs = {};        
        this.headerData = HeaderData(functionRefs);

        bindAction(this.headerData.technicalSpecialist, "RateSchedules", this.editRateScheduleRowHandler);
        bindAction(this.headerData.timeHeaderDetails,"EditTimeColumn",this.editTimeRowHandler);
        bindAction(this.headerData.expensesDetails,"EditExpenseColumn",this.editExpenseRowHandler);
        bindAction(this.headerData.travelDetails,"EditTravelColumn",this.editTravelRowHandler);
        this.editChargeRateRowHandler = this.editChargeRateRowHandler.bind(this);
        this.editPayRateRowHandler = this.editPayRateRowHandler.bind(this);
        bindActionWithChildLevel(this.headerData.timeHeaderDetails.columnDefs,"EditTimeChargePayRate",this.editChargeRateRowHandler); 
        bindActionWithChildLevel(this.headerData.timeHeaderDetails.columnDefs,"EditTimePayRate",this.editPayRateRowHandler); 
        bindActionWithChildLevel(this.headerData.expensesDetails.columnDefs,"EditExpenseChargePayRate",this.editChargeRateRowHandler);
        bindActionWithChildLevel(this.headerData.expensesDetails.columnDefs,"EditExpensePayRate",this.editPayRateRowHandler);
        bindActionWithChildLevel(this.headerData.travelDetails.columnDefs,"EditTravelChargePayRate",this.editChargeRateRowHandler);
        bindActionWithChildLevel(this.headerData.travelDetails.columnDefs,"EditTravelPayRate",this.editPayRateRowHandler);
        bindAction(this.headerData.consumableDetails,"EditConsumableChargePayRate",this.editChargeRateRowHandler); 
        bindAction(this.headerData.consumableDetails,"EditConsumablePayRate",this.editPayRateRowHandler);
        //bindAction(HeaderData.consumableDetails,"EditConsumableColumn",this.editConsumableRowHandler);

        functionRefs["EditChargeRateRowHandler"] = this.EditChargeRateRowHandler;
        functionRefs["EditPayRateRowHandler"] = this.EditPayRateRowHandler;
    }

    EditChargeRateRowHandler = (updatedData) => {
        this.editChargeRateRowHandler(updatedData);
    }

    EditPayRateRowHandler = (updatedData) => {
        this.editPayRateRowHandler(updatedData);
    }

    clearLineItemsInState = ()=>{
        this.setState({
            timesheetTechSpecTimes: [],
            timesheetTechSpecTravels: [],
            timesheetTechSpecExpenses: [],
            timesheetTechSpecConsumables: [],
            selectedPayRateGridData: [],
            isAddUnlinkedAssignmentExpenses:false
        });
    }

    clearLineItemsOnCancel(){
        if(this.gridChild !== undefined && this.gridChild.getSelectedRows() !== undefined) {
            const selectedData = this.gridChild.getSelectedRows();        
            let pin = 0;
            if(selectedData !== undefined) { 
                selectedData.map(row => {                
                    pin = row.pin;
                });

                const techSpecTime = [];
                const techSpecTravel = [];
                const techSpecExpense = [];
                const techSpecConsumable = [];   
                const lineItemPermision = this.isLineItemEditable();     
                if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTimes)) {
                    this.props.timesheetTechnicalSpecialistTimes.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            row.modifyPayUnitPayRate = (this.props.modifyPayUnitPayRate ? false : true);
                            techSpecTime.push(row);
                        }
                    });                    
                } 
                this.setState((state) => {
                    return {
                        timesheetTechSpecTimes: techSpecTime,
                    };
                });
                if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTravels)) {
                    this.props.timesheetTechnicalSpecialistTravels.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            techSpecTravel.push(row);
                        }
                    });                    
                }
                this.setState((state) => {
                    return {
                        timesheetTechSpecTravels: techSpecTravel,
                    };
                });
                if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistExpenses)) {
                    this.props.timesheetTechnicalSpecialistExpenses.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            techSpecExpense.push(row);
                        }
                    });                    
                }
                this.setState((state) => {
                    return {
                        timesheetTechSpecExpenses: techSpecExpense,
                    };
                });
                if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistConsumables)) {
                    this.props.timesheetTechnicalSpecialistConsumables.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            techSpecConsumable.push(row);
                        }
                    });                    
                }
                this.setState((state) => {
                    return {
                        timesheetTechSpecConsumables: techSpecConsumable,
                    };
                });
            }   
        }     
    }

    componentDidMount() {
        this.props.actions.FetchExpenseType();
        this.currencyData = sortCurrency(this.props.currencyMasterData);
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0 &&
            this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE){
                this.props.actions.FetchCompanyExpectedMargin();
                this.fetchExchangeRatesForlineItmes();
        }
    }

    fetchExchangeRatesForlineItmes = async () => {
        const calculateExchangeRates = [];
        this.props.timesheetTechnicalSpecialistExpenses && this.props.timesheetTechnicalSpecialistExpenses.forEach(row => {            
            if(row.currency !== row.chargeRateCurrency) {
                calculateExchangeRates.push({
                    currencyFrom: row.currency,
                    currencyTo: row.chargeRateCurrency,
                    effectiveDate: row.expenseDate
                });
            }
            if(row.currency !== row.payRateCurrency) {
                calculateExchangeRates.push({
                    currencyFrom: row.currency,
                    currencyTo: row.payRateCurrency,
                    effectiveDate: row.expenseDate
                });
            }
        });
        if(calculateExchangeRates && calculateExchangeRates.length > 0) {
            const resultRates = await this.props.actions.FetchCurrencyExchangeRate(calculateExchangeRates, this.props.timesheetInfo.timesheetContractNumber);  
            if(resultRates && resultRates.length > 0)
            {
                await this.props.actions.UpdateTimesheetExchangeRates(resultRates);
            }
        }        
    }
    
    editRateScheduleRowHandler = (data) => {       
        this.editedRowData = data;   
       // this.props.actions.FetchTechSpecRateSchedules(data.pin);
                this.setState((state) => {
                    return {
                        scheduleRateShowModal: !state.scheduleRateShowModal,
                    };
                });      
    }
    cancelChargeScheduleModal=(e)=>{
        e.preventDefault();
        this.editedRowData = {};
        this.setState((state) => {
            return {
                scheduleRateShowModal: !state.scheduleRateShowModal,
            };
        });
    }

    getPayDescription(pin, date, payExpenseType) {
        const payRateSchedule = isEmptyReturnDefault(this.props.techSpecRateSchedules);
        let payDescription = '';
        if(!isEmptyOrUndefine(payRateSchedule.paySchedules)) {
            payRateSchedule.paySchedules.forEach(row => {
                if(row.epin === parseInt(pin)) {
                    row.payScheduleRates.forEach(rowData => { 
                        if(rowData.expenseType === payExpenseType && rowData.isActive && (isEmptyOrUndefine(rowData.effectiveTo) 
                        || (moment(rowData.effectiveTo).format(localConstant.commonConstants.SAVE_DATE_FORMAT) >= date))
                        && moment(rowData.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT) <= date)
                        {
                            payDescription = rowData.description;
                        }
                    });
                }
            });
        }
        return payDescription;
    }

    editPayRateRowHandler = (data) => {        
        const selectedData = this.gridChild.getSelectedRows();        
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {                
                pin = row.pin;
            });
        }
        this.editedRowData = data;        
        const payRateGridData = isEmptyReturnDefault(this.props.techSpecRateSchedules);        
        const selectedPayData = [];
        if(!isEmptyOrUndefine(payRateGridData.paySchedules) && !isEmptyOrUndefine(this.editedRowData)) {   
            const expenseDate = moment(this.editedRowData.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);                 
            payRateGridData.paySchedules.forEach(row => {
                row.payScheduleRates.forEach(rowData => { 
                    if((isEmptyOrUndefine(rowData.effectiveTo) 
                        || (moment(rowData.effectiveTo).format(localConstant.commonConstants.SAVE_DATE_FORMAT) >= expenseDate
                        || this.props.isShowAllRates === true))
                        && (moment(rowData.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT) <= expenseDate
                            || this.props.isShowAllRates === true)
                        && ([ 'T', 'C', 'Q' ].includes(rowData.type) ? rowData.expenseType == this.editedRowData.payExpenseType : rowData.expenseType == this.editedRowData.chargeExpenseType)
                        && row.epin == pin && rowData.isActive && row.technicalSpecialistCompanyCode === this.props.selectedCompany)                     
                    {
                        const addPayData = {};
                        addPayData["description"] = rowData.description;
                        addPayData["effectiveFrom"] = rowData.effectiveFrom;
                        addPayData["chargeRate"] = rowData.payRate;
                        addPayData["sPayRate"] = rowData.sPayRate;
                        addPayData["currency"] = rowData.currency;
                        addPayData["type"] = rowData.type;
                        addPayData["scheduleName"] = row.technicalSpecialistPayScheduleName;
                        addPayData["isChargeRate"] = false;
                        addPayData["isPayRate"] = true;
                        addPayData["rateId"] = rowData.rateId;
                        selectedPayData.push(addPayData);
                    }
                });            
            });
        }
        
        this.setState((state) => {
            return {
                isPayRate: true,
                payRateShowModal: true,
                selectedPayRateGridData: selectedPayData
            };
        });      
    }

    getChargeDescription(pin, date, chargeExpenseType) {
        const chargeRateSchedule = isEmptyReturnDefault(this.props.techSpecRateSchedules);
        let chargeDescription = '';
        if(!isEmptyOrUndefine(chargeRateSchedule.chargeSchedules)) {
            chargeRateSchedule.chargeSchedules.forEach(row => {
                if(row.epin === parseInt(pin)) {
                    row.chargeScheduleRates.forEach(rowData => { 
                        if(rowData.chargeType === chargeExpenseType && rowData.isActive && (isEmptyOrUndefine(rowData.effectiveTo) 
                        || (moment(rowData.effectiveTo).format(localConstant.commonConstants.SAVE_DATE_FORMAT) >= date))
                        && moment(rowData.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT) <= date)
                        {
                            chargeDescription = rowData.description;
                        }
                    });
                }
            });
        }
        return chargeDescription;
    }

    editChargeRateRowHandler = (data) => {
        const selectedData = this.gridChild.getSelectedRows();        
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {                
                pin = row.pin;
            });
        }
        this.editedRowData = data;       
        const payRateGridData = isEmptyReturnDefault(this.props.techSpecRateSchedules);        
        const selectedPayData = [];
        if(!isEmptyOrUndefine(payRateGridData.chargeSchedules) && !isEmptyOrUndefine(this.editedRowData)) {  
            const expenseDate = moment(this.editedRowData.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);                  
            payRateGridData.chargeSchedules.forEach(row => {
                row.chargeScheduleRates.forEach(rowData => { 
                    if((isEmptyOrUndefine(rowData.effectiveTo) 
                        || (moment(rowData.effectiveTo).format(localConstant.commonConstants.SAVE_DATE_FORMAT) >= expenseDate
                        || this.props.isShowAllRates === true))
                        && (moment(rowData.effectiveFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT) <= expenseDate
                            || this.props.isShowAllRates === true)
                        && rowData.chargeType == this.editedRowData.chargeExpenseType
                        && row.epin == pin && rowData.isActive)               
                    {
                        const addPayData = {};
                        addPayData["description"] = rowData.description;
                        addPayData["effectiveFrom"] = rowData.effectiveFrom;
                        addPayData["chargeRate"] = rowData.chargeRate;
                        addPayData["sChargeRate"] = rowData.sChargeRate;
                        addPayData["currency"] = rowData.currency;
                        addPayData["type"] = rowData.type;
                        addPayData["scheduleName"] = row.contractScheduleName;
                        addPayData["isChargeRate"] = true;
                        addPayData["isPayRate"] = false;
                        addPayData["rateId"] = rowData.rateId;
                        addPayData["isPrintDescriptionOnInvoice"] = rowData.isPrintDescriptionOnInvoice;
                        selectedPayData.push(addPayData);
                    }
                });
            });
        }
        
        this.setState((state) => {
            return {
                isPayRate: false,
                payRateShowModal: true,
                selectedPayRateGridData: selectedPayData
            };
        });      
    }

    submitPayRateModal=(e)=>{
        e.preventDefault();        
        let isValid = true;
        this.updatedData = this.editedRowData;
        let expenseDescription = '';
        let isPrintDescriptionOnInvoice = false;
        if(!isEmptyOrUndefine(this.gridPayRate.getSelectedRows())) {
            const selectedData = this.gridPayRate.getSelectedRows();
            let selectedType = '';
            selectedData.map(row => {
                if(row.isChargeRate === true) {
                    if(row.chargeRate === undefined) {
                        this.updatedData["chargeRate"] = row.chargeRate;
                    } else {
                        this.updatedData["chargeRate"] = row.chargeRate;
                    }
                    this.updatedData["chargeRateId"] = row.rateId;
                    this.updatedData["chargeRateCurrency"] = row.currency;
                    this.updatedData["chargeRateDescription"] = row.description;
                    this.updatedData["chargeDescription"] = row.description;
                    if(row.isPrintDescriptionOnInvoice) isPrintDescriptionOnInvoice = true;                    
                    expenseDescription = row.description;
                } else {
                    if(row.chargeRate === undefined || row.chargeRate === null) {
                        this.updatedData["payRate"] = "0.0000";
                    } else {
                        this.updatedData["payRate"] = row.chargeRate.toFixed(4);
                    }
                    this.updatedData["payRateId"] = row.rateId;
                    this.updatedData["payRateCurrency"] = row.currency;
                    this.updatedData["payRateDescription"] = row.description;
                    this.updatedData["chargeDescription"] = row.description;
                }
                selectedType = row.type;
                if(this.updatedData.recordStatus===null){
                    this.updatedData.recordStatus = 'M';
                }
            });            
            const focusedCell = this.getFocusedLineItemGrid();
            if(selectedType === 'R') {   
                if(isPrintDescriptionOnInvoice) this.updatedData["isInvoicePrintPayRateDescrition"] = true;
                this.props.actions.UpdateTimesheetTechnicalSpecialistTime(this.updatedData, this.editedRowData);
                this.updateTimeLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridTimeChild.gridApi.setRowData(this.state.timesheetTechSpecTimes);
                }, 0);
                this.gridTimeChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId); 
            } else if(selectedType === 'T') {      
                if(isPrintDescriptionOnInvoice) this.updatedData["isInvoicePrintExpenseDescription"] = true;
                if(expenseDescription !== '') this.updatedData["expenseDescription"] = expenseDescription;      
                this.props.actions.UpdateTimesheetTechnicalSpecialistTravel(this.updatedData, this.editedRowData);
                this.updateTravelLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridTravelChild.gridApi.setRowData(this.state.timesheetTechSpecTravels);
                }, 0);
                this.gridTravelChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId); 
            } else if(selectedType === 'E') {    
                if(expenseDescription !== '') this.updatedData["expenseDescription"] = expenseDescription;                
                this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, this.editedRowData);
                this.updateExpenseLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridExpenseChild.gridApi.setRowData(this.state.timesheetTechSpecExpenses);
                }, 0);
                this.gridExpenseChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId); 
            } else if(selectedType === 'C' || selectedType === 'Q') {                    
                this.props.actions.UpdateTimesheetTechnicalSpecialistConsumable(this.updatedData, this.editedRowData);
                this.updateConsumableLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridConsumableChild.gridApi.setRowData(this.state.timesheetTechSpecConsumables);
                }, 0);
                this.gridConsumableChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId); 
            }            
        } else {
            IntertekToaster("Please select the record", 'warningToast GridValidation');
            isValid = false;
        }

        if(isValid) {
            this.setState((state) => {
                return {
                    payRateShowModal: false,
                };
            });
        }
    }

    getFocusedLineItemGrid() {
        if(this.state.selectedTabIndex === 0) {
            return this.gridTimeChild.gridApi.getFocusedCell();
        } else if(this.state.selectedTabIndex === 1) {
            return this.gridExpenseChild.gridApi.getFocusedCell();
        } else if(this.state.selectedTabIndex === 2) {
            return this.gridTravelChild.gridApi.getFocusedCell();
        } else if(this.state.selectedTabIndex === 3) {
            return this.gridConsumableChild.gridApi.getFocusedCell();
        }
    }

    focusOnChargePayRate(colId, rowIndex) {
        if(document.getElementById(colId + rowIndex) !== null) {
            document.getElementById(colId + rowIndex).focus();
        }
    }

    cancelPayRateModal=(e)=>{
        e.preventDefault();
        const focusedCell = this.getFocusedLineItemGrid();
        this.setState((state) => {
            return {
                payRateShowModal: false,
            };
        });
        this.focusOnChargePayRate(focusedCell.column.colId, focusedCell.rowIndex);  
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    addDefaultExchangeRate = async (exchangeRates, lineItemData) =>{
        const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates);
        if (res && res.length > 0) {
            res.forEach(eachRate =>{
                if(eachRate.currencyTo === lineItemData.chargeRateCurrency){
                        this.updatedData["chargeRateExchange"] = this.formatExchangeRate(eachRate.rate);
                        lineItemData["chargeRateExchange"] = this.updatedData["chargeRateExchange"];
                    }
                    if(eachRate.currencyTo === lineItemData.payRateCurrency){
                        this.updatedData["payRateExchange"] = this.formatExchangeRate(eachRate.rate);
                        lineItemData["payRateExchange"] = this.updatedData["payRateExchange"];
                    } 
            });
        }
        return lineItemData;
    }
    addDefaultCurrency = (pin, lineItemData) => {
        const rateSchedules =this.props.techSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });
        if(!('chargeRateCurrency' in lineItemData) 
            || !lineItemData.chargeRateCurrency 
            || lineItemData.chargeRateCurrency === ""){
            this.updatedData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        }
        if(!('payRateCurrency' in lineItemData)
            || !lineItemData.payRateCurrency 
            || lineItemData.payRateCurrency === ""){
            this.updatedData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        }
    }

    getNextNumber = (items, fieldName) => {
        const alreadyGenerateNumbers = [];
        if(!isEmptyOrUndefine(items)) {
            items.forEach(row => {
                if(row[fieldName]) {                    
                    alreadyGenerateNumbers.push(row[fieldName]);
                }
            });
        }
        let newNum = Math.floor(Math.random() * 9999) - 10000;
        while (alreadyGenerateNumbers.includes(newNum)) {
            newNum = Math.floor(Math.random() * 9999) - 10000;
        }
        return newNum;
    }

    addTimeNewRow=(e)=>{
        e.preventDefault();
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);
        
        let selectedTechnicalSpecialist = [];
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecilists)) {
            selectedTechnicalSpecialist = this.props.timesheetTechnicalSpecilists.filter(x => x.recordStatus !== "D");
        }

        if (selectedTechnicalSpecialist !== undefined && selectedTechnicalSpecialist.length > 0) {
            const rateSchedules =this.props.techSpecRateSchedules;
            const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

            const timeLineItemData = {};
            timeLineItemData["TechSpecTimeId"] = this.getNextNumber(this.props.timesheetTechnicalSpecialistTimes, "TechSpecTimeId"); //Math.floor(Math.random() * 999) - 1000;        
            timeLineItemData["recordStatus"] = 'N';            
            timeLineItemData["pin"] = pin;
            timeLineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            timeLineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            timeLineItemData["timesheetId"]  = this.props.timesheetId;
            timeLineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            timeLineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            timeLineItemData["expenseDate"] = this.props.timesheetInfo.timesheetStartDate ? 
                                    moment(this.props.timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT) : '';
            timeLineItemData["chargeTotalUnit"] = "0.00";
            timeLineItemData["chargeRate"] = "0.0000";
            timeLineItemData["sChargeRate"] = "0.0000";
            timeLineItemData["sPayRate"] = "0.0000";
            timeLineItemData["payUnit"] = "0.00";
            timeLineItemData["payRate"] = "0.0000";
            timeLineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            timeLineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            timeLineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly; 
            timeLineItemData["payRateReadonly"] = this.props.payRateReadonly;
            timeLineItemData["modifyPayUnitPayRate"] = (this.props.modifyPayUnitPayRate ? false : true);
            timeLineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
            timeLineItemData["lineItemPermision"] = this.isLineItemEditable();

            const techSpecTime = [];
            techSpecTime.push(timeLineItemData);
            if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTimes)) {            
                this.props.timesheetTechnicalSpecialistTimes.forEach(row => {
                    if(row.recordStatus !== 'D' && pin == row.pin) {                    
                        techSpecTime.push(row);
                    }
                });            
            }
            this.setState((state) => {
                return {
                    timesheetTechSpecTimes: techSpecTime,
                };
            });
            this.props.actions.AddTimesheetTechnicalSpecialistTime(timeLineItemData);
            setTimeout(() => {
                this.gridTimeChild.gridApi.setRowData(this.state.timesheetTechSpecTimes);
            }, 0);
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST, 'warningToast idOneRecordSelectReq');
        }        
    }

    TimeItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["chargeTotalUnitRequired"] = "";
        row["chargeRateRequired"] = "";
        row["payUnitRequired"] = "";
        row["payRateRequired"] = "";
        row["chargeRateCurrencyRequired"] = "";
        row["payRateCurrencyRequired"] = "";

        const isValidExpenseDate = required(row.expenseDate);
        if(isValidExpenseDate){            
            row["expenseDateRequired"] = localConstant.validationMessage.EXPENSE_DATE_REQUIRED;
        }
        if(!isValidExpenseDate && row.expenseDate._isAMomentObject){
            row.expenseDate = row.expenseDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }

        if(!isValidExpenseDate && row.expenseDate.indexOf(":") === -1 && dateUtil.isUIValidDate(row.expenseDate)) {
            row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["expenseDate"] = "";
        }
        if(e && e.column && e.column.colDef && e.column.colDef.moduleType && e.column.colDef.moduleType === 'Time'
            && document.getElementById(e.column.getId() + e.rowIndex) !== null && e.rowIndex === rowIndex
            && document.getElementById('expenseDate' + e.rowIndex) && e.column.getId() !== 'expenseDate') {            
                const dateValue = document.getElementById('expenseDate' + e.rowIndex).value;
                if(dateValue && dateValue.indexOf(":") === -1 && !dateUtil.isUIValidDate(dateValue)) {
                    row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
                    row["expenseDate"] = "";
                }
        }
        if(required(row.chargeExpenseType)){                    
            row["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        
        if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeTotalUnit)){            
                row["chargeTotalUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0
                && !requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0
                && required(row.chargeRateCurrency)){            
                row["chargeRateCurrencyRequired"] = localConstant.validationMessage.CHARGE_CURRENCY_REQUIRED;
            }
        }

        if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
                row["payUnitRequired"] = localConstant.validationMessage.PAY_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.payRate)){            
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!lineItemPermision.isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_VALIDATION_ON_UNIT;
            }
            
            if(!requiredNumeric(row.payRate) && row.payRate > 0
                 && !requiredNumeric(row.payUnit) && row.payUnit > 0
                && required(row.payRateCurrency)){            
                row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;
            }
        }
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0 && !requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payRate) && row.payRate > 0 && !requiredNumeric(row.payUnit) && row.payUnit > 0) ? false : true;
            }
        }
        return row;
    }

    addTimeShowModal=(e)=>{
        e.preventDefault();
        this.updatedData = {};
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeTotalUnit:0,
            expenseDate:this.props.timesheetInfo.timesheetStartDate
        };
        this.setState({ expenseDate: this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'' });
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isNewRecord: true,
                payUnit: 0,
            };
        });
    }
    submitTimeModal = (e) => {        
        e.preventDefault();
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const timeLineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(timeLineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(required(timeLineItemData.expenseDate) && dateUtil.isUIValidDate(this.updatedData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(timeLineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(timeLineItemData.chargeRate)){
            IntertekToaster("Charge Rate is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(timeLineItemData.payRate)){
            IntertekToaster("Pay Rate is required", 'warningToast PercentRange');
            return false;
        }

        if(!requiredNumeric(timeLineItemData.chargeRate) && timeLineItemData.chargeRate > 0
                && !requiredNumeric(timeLineItemData.chargeTotalUnit) && timeLineItemData.chargeTotalUnit > 0
                && required(timeLineItemData.chargeRateCurrency)){
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(timeLineItemData.payRate) && timeLineItemData.payRate > 0
             && !requiredNumeric(timeLineItemData.payUnit) && timeLineItemData.payUnit > 0
            && required(timeLineItemData.payRateCurrency)){
            IntertekToaster("Pay Currency is required", 'warningToast PercentRange');
            return false;
        }

        this.addDefaultCurrency(pin,timeLineItemData);

        if (!this.state.isNewRecord)
        {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["pin"] = pin;
            this.updatedData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            this.updatedData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            this.updatedData["timesheetId"]  = this.props.timesheetId; 
            this.updatedData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            this.updatedData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            this.props.actions.UpdateTimesheetTechnicalSpecialistTime(this.updatedData, this.editedRowData);
            this.updateTimeLineItems(this.updatedData, this.editedRowData);
        } else {            
            timeLineItemData["TechSpecTimeId"] = Math.floor(Math.random() * 999) - 1000;        
            timeLineItemData["recordStatus"] = 'N';            
            timeLineItemData["pin"] = pin;
            timeLineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            timeLineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            timeLineItemData["timesheetId"]  = this.props.timesheetId;
            timeLineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            timeLineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            timeLineItemData["chargeRate"] = timeLineItemData.chargeRate;
            timeLineItemData["payRate"] = FormatFourDecimal(timeLineItemData.payRate);
            this.props.actions.AddTimesheetTechnicalSpecialistTime(timeLineItemData);
            this.state.timesheetTechSpecTimes.push(timeLineItemData);
        }

        // this.bindTechSpecLineItems(pin);
        this.gridTimeChild.gridApi.setRowData(this.state.timesheetTechSpecTimes);
        this.cancelTimeModal();
        this.updatedData = {};
    }

    updateTimeLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecTime = this.state.timesheetTechSpecTimes;
        let checkProperty = "timesheetTechnicalSpecialistAccountTimeId";
        if (editedRow.recordStatus === 'N') {
            checkProperty = "TechSpecTimeId";
        }
        const index = techSpecTime.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
        
        const newState = Object.assign([], techSpecTime);
        if (index >= 0) {
            newState[index] = editedRow;
        }        
        this.setState((state) => {
            return {
                timesheetTechSpecTimes: newState,
            };
        });
    }

    cancelTimeModal=()=>{
        this.editedRowData = {};
        this.updatedData = {};
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isTimePayUnitEntered: false,
                payUnit: 0
            };
        });
    }
    //DeleteTimesheetTechnicalSpecialistTime
    //DeleteTimesheetTechnicalSpecialistTravel
   // DeleteTimesheetTechnicalSpecialistExpense
   // DeleteTimesheetTechnicalSpecialistConsumable
   deleteLineItemHandler = (e,gridRef,deleteAction) => {
    const selectedRecords = this[gridRef].getSelectedRows();
        if (selectedRecords.length > 0) {
            const {                
                timesheetInfo,
                isOperatingCompany,
                isCoordinatorCompany,
                isInterCompanyAssignment
            } = this.props;
            let hasErrors = false;
            
            if([ 'O','A','R' ].includes(timesheetInfo.timesheetStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' 
                    && (([ 'O','E' ].includes(x.costofSalesStatus) && (x.unPaidStatus === 'Rejected' || x.unPaidStatus === 'On-hold'))
                    || ([ 'S','A' ].includes(x.costofSalesStatus))
                    )).length > 0;
                if(hasErrors) this.deleteTechSpecLineItemError(true, true);
            }
            if(!hasErrors && isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(timesheetInfo.timesheetStatus)) {   
                if(deleteAction === 'DeleteTimesheetTechnicalSpecialistTime')                           
                    hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                        && x.chargeRate > 0 && x.chargeTotalUnit > 0 && !required(x.chargeRateCurrency)).length > 0;
                else if(deleteAction === 'DeleteTimesheetTechnicalSpecialistExpense' || deleteAction === 'DeleteTimesheetTechnicalSpecialistTravel'
                            || deleteAction === 'DeleteTimesheetTechnicalSpecialistConsumable')
                    hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                        && x.chargeRate > 0 && x.chargeUnit > 0 && !required(x.chargeRateCurrency)).length > 0;                               
                if(hasErrors) this.deleteTechSpecLineItemError(true, false);
            } else if(!hasErrors && isInterCompanyAssignment && isCoordinatorCompany && [ 'O','A' ].includes(timesheetInfo.timesheetStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                && x.payRate > 0 && x.payUnit > 0 && !required(x.payRateCurrency)).length > 0;  
                if(hasErrors) this.deleteTechSpecLineItemError(false, false);
            }
            if(!hasErrors) this.deleteTechSpecLineItem(gridRef,deleteAction);   
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast idOneRecordSelectReq');
        }
    }

    deleteTechSpecLineItemError = (isChargeType, isInvoiced) => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: (isInvoiced ? modalMessageConstant.DELETE_INVOICED_TECH_SPEC_ITEM_ERROR : 
                (isChargeType ? modalMessageConstant.DELETE_TECH_SPEC_ITEM_ERROR : modalMessageConstant.DELETE_PAY_TECH_SPEC_ITEM_ERROR)),
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.OK,
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        this.props.actions.DisplayModal(confirmationObject);
    }

    deleteTechSpecLineItem = (gridRef,deleteAction) => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DELETE_VISIT_TECH_SPEC,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: (e)=>{ 
                        this.deleteSelectedTimesheetLineItem(e,gridRef,deleteAction); 
                    },
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: localConstant.commonConstants.NO,
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        this.props.actions.DisplayModal(confirmationObject);
    }
    
    deleteSelectedTimesheetLineItem = (e,GridRef,action) => {
        const selectedRecords = this[GridRef].getSelectedRows();
        this[GridRef].removeSelectedRows(selectedRecords);
        this.props.actions[action](selectedRecords);
        if(GridRef === 'gridTimeChild'){
            
            this.setState((state)=>({
                timesheetTechSpecTimes:  state.timesheetTechSpecTimes.filter(item =>{
                    return !selectedRecords.some(ts=>{
                        let checkProperty = "timesheetTechnicalSpecialistAccountTimeId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecTimeId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                })
            }),()=>{
                this[GridRef].gridApi.setRowData(this.state.timesheetTechSpecTimes);
            });
        }
        if(GridRef === 'gridExpenseChild'){
            this.setState((state)=>({
                timesheetTechSpecExpenses:  state.timesheetTechSpecExpenses.filter(item =>{
                    return !selectedRecords.some(ts=>{
                        let checkProperty = "timesheetTechnicalSpecialistAccountExpenseId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecExpenseId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                })
            }),()=>{
                this[GridRef].gridApi.setRowData(this.state.timesheetTechSpecExpenses);
            });
        }
        if(GridRef === 'gridTravelChild'){
            this.setState((state)=>({
                timesheetTechSpecTravels:  state.timesheetTechSpecTravels.filter(item =>{
                    return !selectedRecords.some(ts=>{
                        let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecTravelId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                })
            }),()=>{
                this[GridRef].gridApi.setRowData(this.state.timesheetTechSpecTravels);
            });
        }
        if(GridRef === 'gridConsumableChild'){
            this.setState((state)=>({
                timesheetTechSpecConsumables:  state.timesheetTechSpecConsumables.filter(item =>{
                    return !selectedRecords.some(ts=>{
                        let checkProperty = "timesheetTechnicalSpecialistAccountConsumableId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecConsumableId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                })
            }),()=>{
                this[GridRef].gridApi.setRowData(this.state.timesheetTechSpecConsumables);
            });
        }
        
        this.props.actions.HideModal();
    }

    addExpenseNewRow=(e)=>{
        e.preventDefault();
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        //const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);

        let selectedTechnicalSpecialist = [];
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecilists)) {
            selectedTechnicalSpecialist = this.props.timesheetTechnicalSpecilists.filter(x => x.recordStatus !== "D");
        }

        if (selectedTechnicalSpecialist !== undefined && selectedTechnicalSpecialist.length > 0) {
            const rateSchedules =this.props.techSpecRateSchedules;
            const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

            const expenseLineItemData = {};  
            expenseLineItemData["TechSpecExpenseId"] = this.getNextNumber(this.props.timesheetTechnicalSpecialistExpenses, "TechSpecExpenseId"); //Math.floor(Math.random() * 999) - 1000;        
            expenseLineItemData["recordStatus"] = 'N';
            expenseLineItemData["pin"] = pin;
            expenseLineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            expenseLineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            expenseLineItemData["timesheetId"]  = this.props.timesheetId;
            expenseLineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            expenseLineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            expenseLineItemData["expenseDate"] = this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'';
            expenseLineItemData["chargeUnit"] = "0.00";
            expenseLineItemData["chargeRate"] = "0.0000";
            expenseLineItemData["sChargeRate"] = "0.0000";
            expenseLineItemData["sPayRate"] = "0.0000";
            expenseLineItemData["payUnit"] = "0.00";
            expenseLineItemData["payRate"] = "0.0000";
            expenseLineItemData["payRateTax"] = "0.00";
            expenseLineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            expenseLineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            expenseLineItemData["chargeRateReadonly"] = this.props.chargeRateReadonly; 
            expenseLineItemData["payRateReadonly"] = this.props.payRateReadonly;
            expenseLineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
            expenseLineItemData["currencyRequired"] = localConstant.validationMessage.CURRENCY_REQUIRED;
            expenseLineItemData["lineItemPermision"] = this.isLineItemEditable();

            const techSpecExpense = [];
            techSpecExpense.push(expenseLineItemData);
            if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistExpenses)) {            
                this.props.timesheetTechnicalSpecialistExpenses.forEach(row => {
                    if(row.recordStatus !== 'D' && pin == row.pin) {                    
                        techSpecExpense.push(row);
                    }
                });            
            }
            this.setState((state) => {
                return {
                    timesheetTechSpecExpenses: techSpecExpense,
                };
            });
            this.props.actions.AddTimesheetTechnicalSpecialistExpense(expenseLineItemData);
            setTimeout(() => {
                this.gridExpenseChild.gridApi.setRowData(this.state.timesheetTechSpecExpenses);
            }, 0);  
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST, 'warningToast idOneRecordSelectReq');
        }              
    }

    addExpensesShowModal=(e)=>{
        e.preventDefault();
        const { timesheetStartDate }=this.props.timesheetInfo;
        this.updatedData ={};
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeUnit:0,
            // chargeRateExchange:1,
            // payRateExchange : 1,
            expenseDate:timesheetStartDate,
            payRateTax:0
        };
        this.setState({ expenseDate: this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'' });
        this.setState((state) => {
            return {
                expensesShowModal: !state.expensesShowModal,
                isNewRecord: true
            };
        });
    }

    ExpenseItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["chargeUnitRequired"] = "";
        row["chargeRateRequired"] = "";
        row["payUnitRequired"] = "";
        row["payRateRequired"] = "";
        row["payRateTaxRequired"] = "";
        row["chargeRateCurrencyRequired"] = "";
        row["payRateCurrencyRequired"] = "";
        row["currencyRequired"] = "";
        const isValidExpenseDate = required(row.expenseDate);
        if(isValidExpenseDate){            
            row["expenseDateRequired"] = localConstant.validationMessage.EXPENSE_DATE_REQUIRED;
        }
        if(!isValidExpenseDate && row.expenseDate._isAMomentObject){
            row.expenseDate = row.expenseDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }
        if(!isValidExpenseDate && row.expenseDate.indexOf(":") === -1 && dateUtil.isUIValidDate(row.expenseDate)) {
            row["expenseDateRequired"] = localConstant.validationMessage.VALID_EXPENSE_DATE;
            row["expenseDate"] = "";
        }
        if(e && e.column && e.column.colDef && e.column.colDef.moduleType && e.column.colDef.moduleType === 'Expense'
            && document.getElementById(e.column.getId() + e.rowIndex) !== null && e.rowIndex === rowIndex
            && document.getElementById('expenseDate' + e.rowIndex) && e.column.getId() !== 'expenseDate') {            
                const dateValue = document.getElementById('expenseDate' + e.rowIndex).value;
                if(dateValue && dateValue.indexOf(":") === -1 && !dateUtil.isUIValidDate(dateValue)) {
                    row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
                    row["expenseDate"] = "";
                }
        }
        if(required(row.chargeExpenseType)){            
            row["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeUnit)){            
                row["chargeUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0){            
                if(required(row.chargeRateCurrency)) row["chargeRateCurrencyRequired"] = localConstant.validationMessage.CHARGE_CURRENCY_REQUIRED;                
            }
            if(required(row.currency)) row["currencyRequired"] = localConstant.validationMessage.CURRENCY_REQUIRED;
        }
        
        if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
                row["payUnitRequired"] = localConstant.validationMessage.PAY_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.payRate)){            
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!lineItemPermision.isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_VALIDATION_ON_UNIT;
            }
            if(requiredNumeric(row.payRateTax)){            
                row["payRateTaxRequired"] = localConstant.validationMessage.PAY_RATE_TAX_REQUIRED;
            }
            const payUnit = isNaN(parseFloat(row.payUnit))?0:parseFloat(row.payUnit),
            payRate = isNaN(parseFloat(row.payRate))?0:parseFloat(row.payRate),
            payRateTax = isNaN(parseFloat(row.payRateTax))?0:parseFloat(row.payRateTax); 
            if(payRateTax > (payUnit * payRate )){            
                row["payRateTaxRequired"] = localConstant.validationMessage.PAY_RATE_TAX_UNITS_VALIDATION;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0){            
                if(required(row.payRateCurrency)) row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;                
            }
            if(required(row.currency)) row["currencyRequired"] = localConstant.validationMessage.CURRENCY_REQUIRED;
        }
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }
        return row;
    }

    submitExpenseModal = async (e) => {        
        e.preventDefault();   
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const expenseLineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(expenseLineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isUIValidDate(expenseLineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(expenseLineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(expenseLineItemData.chargeRate)){
            IntertekToaster("Charge Rate is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(expenseLineItemData.payUnit)){
            IntertekToaster("Pay Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(expenseLineItemData.payRate)){
            IntertekToaster("Pay Rate is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(expenseLineItemData.payRateTax)){
            IntertekToaster("Pay Rate Tax is required", 'warningToast PercentRange');
            return false;
        }
        const payUnit = isNaN(parseFloat(expenseLineItemData.payUnit))?0:parseFloat(expenseLineItemData.payUnit),
         payRate = isNaN(parseFloat(expenseLineItemData.payRate))?0:parseFloat(expenseLineItemData.payRate),
         payRateTax = isNaN(parseFloat(expenseLineItemData.payRateTax))?0:parseFloat(expenseLineItemData.payRateTax); 
        if(payRateTax > (payUnit * payRate )){
            IntertekToaster("Pay Rate Tax must be less than Units x Nett/Units", 'warningToast PercentRange');
            return false;
        }
        
        this.addDefaultCurrency(pin,expenseLineItemData);

        if(!('currency' in expenseLineItemData) 
        || !expenseLineItemData.currency ||expenseLineItemData.currency === ""){
            this.updatedData["currency"] = expenseLineItemData.chargeRateCurrency;
            expenseLineItemData["currency"] = expenseLineItemData.chargeRateCurrency;
        }

        if(!requiredNumeric(expenseLineItemData.chargeUnit) && expenseLineItemData.chargeUnit > 0
            && !requiredNumeric(expenseLineItemData.chargeRate) && expenseLineItemData.chargeRate > 0
            && required(expenseLineItemData.chargeRateCurrency)){
           IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
           return false;
       }
       if(!requiredNumeric(expenseLineItemData.payUnit) && expenseLineItemData.payUnit > 0
            && !requiredNumeric(expenseLineItemData.payRate) && expenseLineItemData.payRate > 0
           && required(expenseLineItemData.payRateCurrency)){
           IntertekToaster("Pay Currency is required", 'warningToast PercentRange');
           return false;
       } 

        if ((isEmptyOrUndefine(expenseLineItemData.chargeRateExchange) 
            || isEmptyOrUndefine(expenseLineItemData.payRateExchange))
            && !isEmptyOrUndefine(this.updatedData["currency"]) 
            && !isEmptyOrUndefine(this.updatedData["chargeRateCurrency"]) 
            && !isEmptyOrUndefine(this.updatedData["payRateCurrency"])) {
            const exchangeRates = [ {
                currencyFrom: this.updatedData["currency"],
                currencyTo: this.updatedData["chargeRateCurrency"],
                effectiveDate: expenseLineItemData.expenseDate ? moment(expenseLineItemData.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT): currentDate
            },
            {
                currencyFrom: this.updatedData["currency"],
                currencyTo: this.updatedData["payRateCurrency"],
                effectiveDate: expenseLineItemData.expenseDate ? moment(expenseLineItemData.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT): currentDate
            } ];
           await this.addDefaultExchangeRate(exchangeRates,expenseLineItemData);
        } else{
            this.updatedData["chargeRateExchange"] = this.state.chargeRateExchange;
            expenseLineItemData["chargeRateExchange"] = this.updatedData["chargeRateExchange"];
            this.updatedData["payRateExchange"] =  this.state.payRateExchange;
            expenseLineItemData["payRateExchange"] = this.updatedData["payRateExchange"];
        }

        if (!this.state.isNewRecord)
        {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["pin"] = pin;
            this.updatedData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;    
            this.updatedData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            this.updatedData["timesheetId"]  = this.props.timesheetId;
            this.updatedData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            this.updatedData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            this.props.actions.UpdateTimesheetTechnicalSpecialistExpense(this.updatedData, this.editedRowData);
            this.updateExpenseLineItems(this.updatedData, this.editedRowData);
        } else {
            expenseLineItemData["TechSpecExpenseId"] = Math.floor(Math.random() * 999) - 1000;        
            expenseLineItemData["recordStatus"] = 'N';
            expenseLineItemData["pin"] = pin;
            expenseLineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            expenseLineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            expenseLineItemData["timesheetId"]  = this.props.timesheetId;
            expenseLineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            expenseLineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            expenseLineItemData["chargeRate"] = expenseLineItemData.chargeRate;
            expenseLineItemData["payRate"] = FormatFourDecimal(expenseLineItemData.payRate);  
            this.props.actions.AddTimesheetTechnicalSpecialistExpense(expenseLineItemData);
            this.state.timesheetTechSpecExpenses.push(expenseLineItemData);
        }
        this.gridExpenseChild.gridApi.setRowData(this.state.timesheetTechSpecExpenses);
        this.cancelExpensesShowModal();
        this.updatedData = {};
    }

    updateExpenseLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecExpense = this.state.timesheetTechSpecExpenses;
        let checkProperty = "timesheetTechnicalSpecialistAccountExpenseId";
        if (editedRow.recordStatus === 'N') {
            checkProperty = "TechSpecExpenseId";
        }
        const index = techSpecExpense.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
        
        const newState = Object.assign([], techSpecExpense);
        if (index >= 0) {
            newState[index] = editedRow;
        }        
        this.setState((state) => {
            return {
                timesheetTechSpecExpenses: newState,
            };
        });
    }

    cancelExpensesShowModal=()=>{
        this.editedRowData = {};
        this.updatedData = {};
        this.setState((state) => {
            return {
                expensesShowModal: !state.expensesShowModal,
                grossValue:'',
                chargeRateExchange:'',
                payRateExchange:'',
                payUnit: 0,
                isExpensePayUnitEntered: false
            };
        });
    }
    addTravelShowModal=(e)=>{
        e.preventDefault();
        this.editedRowData = {
            payRate:0,
            payUnit:this.state.payUnit,
            chargeRate:0,
            chargeUnit:this.state.chargeUnit,
            expenseDate:this.props.timesheetInfo.timesheetStartDate
        };
        this.updatedData = {};
        this.setState({ expenseDate: this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'' });
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                isNewRecord: true
            };
        });
    }
    
    addTravelNewRow=(e)=>{
        e.preventDefault();
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);

        let selectedTechnicalSpecialist = [];
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecilists)) {
            selectedTechnicalSpecialist = this.props.timesheetTechnicalSpecilists.filter(x => x.recordStatus !== "D");
        }

        if (selectedTechnicalSpecialist !== undefined && selectedTechnicalSpecialist.length > 0) {
            const rateSchedules =this.props.techSpecRateSchedules;
            const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });        
            const defaultTravelType = getTravelDefaultPayType(pin, rateSchedules, this.props.expenseTypes);
            const lineItemData = {};  
            lineItemData["TechSpecTravelId"] = this.getNextNumber(this.props.timesheetTechnicalSpecialistTravels, "TechSpecTravelId"); //Math.floor(Math.random() * 999) - 1000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            lineItemData["timesheetId"]  = this.props.timesheetId;
            lineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            lineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            lineItemData["expenseDate"] = this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'';
            lineItemData["chargeUnit"] = "0.00";
            lineItemData["chargeRate"] = "0.0000";
            lineItemData["sChargeRate"] = "0.0000";
            lineItemData["payUnit"] = "0.00";
            lineItemData["payRate"] = "0.0000";
            lineItemData["sPayRate"] = "0.0000";
            lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            lineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly; 
            lineItemData["payRateReadonly"] = this.props.payRateReadonly;          
            lineItemData["chargeExpenseType"] = defaultTravelType["chargeType"];          
            lineItemData["payExpenseType"] = defaultTravelType["payType"];
            lineItemData["lineItemPermision"] = this.isLineItemEditable();

            const techSpecTravel = [];
            techSpecTravel.push(lineItemData);
            if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTravels)) {            
                this.props.timesheetTechnicalSpecialistTravels.forEach(row => {
                    if(row.recordStatus !== 'D' && pin == row.pin) {                    
                        techSpecTravel.push(row);
                    }
                });            
            }
            this.setState((state) => {
                return {
                    timesheetTechSpecTravels: techSpecTravel,
                };
            });
            this.props.actions.AddTimesheetTechnicalSpecialistTravel(lineItemData);
            setTimeout(() => {
                this.gridTravelChild.gridApi.setRowData(this.state.timesheetTechSpecTravels);
            }, 0);
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST, 'warningToast idOneRecordSelectReq');
        }                
    }

    TravelItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["payExpenseTypeRequired"] = "";
        row["chargeUnitRequired"] = "";
        row["chargeRateRequired"] = "";
        row["payUnitRequired"] = "";
        row["payRateRequired"] = "";
        row["chargeRateCurrencyRequired"] = "";
        row["payRateCurrencyRequired"] = "";
        const isValidExpenseDate = required(row.expenseDate);
        if(isValidExpenseDate){            
            row["expenseDateRequired"] = localConstant.validationMessage.EXPENSE_DATE_REQUIRED;
        }
        if(!isValidExpenseDate && row.expenseDate._isAMomentObject){
            row.expenseDate = row.expenseDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }
        
        if(!isValidExpenseDate && row.expenseDate.indexOf(":") === -1 && dateUtil.isUIValidDate(row.expenseDate)) {      
            row["expenseDateRequired"] = localConstant.validationMessage.VALID_EXPENSE_DATE;
            row["expenseDate"] = "";
        }
        if(e && e.column && e.column.colDef && e.column.colDef.moduleType && e.column.colDef.moduleType === 'Travel'
            && document.getElementById(e.column.getId() + e.rowIndex) !== null && e.rowIndex === rowIndex
            && document.getElementById('expenseDate' + e.rowIndex) && e.column.getId() !== 'expenseDate') {            
                const dateValue = document.getElementById('expenseDate' + e.rowIndex).value;
                if(dateValue && dateValue.indexOf(":") === -1 && !dateUtil.isUIValidDate(dateValue)) {
                    row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
                    row["expenseDate"] = "";
                }
        }
        if(required(row.chargeExpenseType)){            
            row["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        if(required(row.payExpenseType)){            
            row["payExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeUnit)){            
                row["chargeUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0
                && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0
                && required(row.chargeRateCurrency)) {            
                row["chargeRateCurrencyRequired"] = localConstant.validationMessage.CHARGE_CURRENCY_REQUIRED;
            }
        }
        
        if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
                row["payUnitRequired"] = localConstant.validationMessage.PAY_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.payRate)){            
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!lineItemPermision.isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_VALIDATION_ON_UNIT;
            }            
            if (!requiredNumeric(row.payUnit) && row.payUnit > 0
                && !requiredNumeric(row.payRate) && row.payRate > 0
                && required(row.payRateCurrency)) {            
                row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;
            }
        }      
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }  
        return row;
    }

    submitTravelModal = (e) => {                
        e.preventDefault();        
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isUIValidDate(lineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.payExpenseType)){
            IntertekToaster("Pay Expense Type is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payUnit)){
            IntertekToaster("Pay Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payRate)){
            IntertekToaster("Pay Rate is required", 'warningToast PercentRange');
            return false;
        }
        if (!requiredNumeric(lineItemData.chargeUnit) && lineItemData.chargeUnit > 0
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.chargeRateCurrency)) {
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if (!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
            && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
            && required(lineItemData.payRateCurrency)) {
            IntertekToaster("Pay Currency is required", 'warningToast PercentRange');
            return false;
        }
        this.addDefaultCurrency(pin,lineItemData);    
        
        if (!this.state.isNewRecord)
        {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["pin"] = pin;
            this.updatedData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId; 
            this.updatedData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            this.updatedData["timesheetId"]  = this.props.timesheetId;
            this.updatedData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            this.updatedData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            this.updatedData["chargeUnit"]=this.state.chargeUnit;
            this.updatedData["payUnit"]=this.state.payUnit;
            this.props.actions.UpdateTimesheetTechnicalSpecialistTravel(this.updatedData, this.editedRowData);
            this.updateTravelLineItems(this.updatedData, this.editedRowData);
        } else {
            lineItemData["TechSpecTravelId"] = Math.floor(Math.random() * 999) - 1000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            lineItemData["timesheetId"]  = this.props.timesheetId;
            lineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            lineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            lineItemData["chargeUnit"]=this.state.chargeUnit;
            lineItemData["payUnit"]=this.state.payUnit;
            lineItemData["chargeRate"] = lineItemData.chargeRate;
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);
            this.props.actions.AddTimesheetTechnicalSpecialistTravel(lineItemData);
            this.state.timesheetTechSpecTravels.push(lineItemData);
        }
        this.gridTravelChild.gridApi.setRowData(this.state.timesheetTechSpecTravels);
        // this.setState({ chargeUnit:0,payUnit:0 });
        this.cancelTravelShowModal();
        this.updatedData = {};
    }

    updateTravelLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecTravel = this.state.timesheetTechSpecTravels;
        let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
        if (editedRow.recordStatus === 'N') {
            checkProperty = "TechSpecTravelId";
        }
        const index = techSpecTravel.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
        
        const newState = Object.assign([], techSpecTravel);
        if (index >= 0) {
            newState[index] = editedRow;
        }        
        this.setState((state) => {
            return {
                timesheetTechSpecTravels: newState,
            };
        });
    }

    cancelTravelShowModal=()=>{
        this.editedRowData = {};
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                chargeUnit:0,
                payUnit:0
            };
        });
    }
    addConsumableShowModal=(e)=>{
        e.preventDefault();
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeUnit:0,
            expenseDate:this.props.timesheetInfo.timesheetStartDate
        };
        this.updatedData = {};
        this.setState({ expenseDate: this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'' });
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                isNewRecord: true
            };
        });
    }

    addConsumableNewRow=(e)=>{
        e.preventDefault();
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);

        let selectedTechnicalSpecialist = [];
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecilists)) {
            selectedTechnicalSpecialist = this.props.timesheetTechnicalSpecilists.filter(x => x.recordStatus !== "D");
        }

        if (selectedTechnicalSpecialist !== undefined && selectedTechnicalSpecialist.length > 0) {
            const rateSchedules =this.props.techSpecRateSchedules;
            const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });        

            const lineItemData = {};
            lineItemData["TechSpecConsumableId"] = this.getNextNumber(this.props.timesheetTechnicalSpecialistConsumables, "TechSpecConsumableId"); //Math.floor(Math.random() * 999) - 1000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            lineItemData["timesheetId"]  = this.props.timesheetId;
            lineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            lineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            lineItemData["expenseDate"] = this.props.timesheetInfo.timesheetStartDate?this.props.timesheetInfo.timesheetStartDate:'';
            lineItemData["chargeUnit"] = "0.00";
            lineItemData["chargeRate"] = "0.0000";
            lineItemData["sChargeRate"] = "0.0000";
            lineItemData["sPayRate"] = "0.0000";
            lineItemData["payUnit"] = "0.00";
            lineItemData["payRate"] = "0.0000";
            lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            lineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly; 
            lineItemData["payRateReadonly"] = this.props.payRateReadonly;
            lineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
            lineItemData["lineItemPermision"] = this.isLineItemEditable();

            const techSpecConsumable = [];
            techSpecConsumable.push(lineItemData);
            if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistConsumables)) {            
                this.props.timesheetTechnicalSpecialistConsumables.forEach(row => {
                    if(row.recordStatus !== 'D' && pin == row.pin) {                    
                        techSpecConsumable.push(row);
                    }
                });            
            }
            this.setState((state) => {
                return {
                    timesheetTechSpecConsumables: techSpecConsumable,
                };
            });
            this.props.actions.AddTimesheetTechnicalSpecialistConsumable(lineItemData);
            setTimeout(() => {
                this.gridConsumableChild.gridApi.setRowData(this.state.timesheetTechSpecConsumables);
            }, 0); 
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST, 'warningToast idOneRecordSelectReq');
        }
    }
    
    ConsumableItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["chargeUnitRequired"] = "";
        row["chargeRateRequired"] = "";
        row["payUnitRequired"] = "";
        row["payRateRequired"] = "";
        row["chargeRateCurrencyRequired"] = "";
        row["payRateCurrencyRequired"] = "";
        const isValidExpenseDate = required(row.expenseDate);
        if(isValidExpenseDate){            
            row["expenseDateRequired"] = localConstant.validationMessage.EXPENSE_DATE_REQUIRED;
        }
        if(!isValidExpenseDate && row.expenseDate._isAMomentObject){
            row.expenseDate = row.expenseDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        }
        if(!isValidExpenseDate && row.expenseDate.indexOf(":") === -1 && dateUtil.isUIValidDate(row.expenseDate)) {     
            row["expenseDateRequired"] = localConstant.validationMessage.VALID_EXPENSE_DATE;
            row["expenseDate"] = "";
        }
        if(e && e.column && e.column.colDef && e.column.colDef.moduleType && e.column.colDef.moduleType === 'Consumable'
            && document.getElementById(e.column.getId() + e.rowIndex) !== null && e.rowIndex === rowIndex
            && document.getElementById('expenseDate' + e.rowIndex) && e.column.getId() !== 'expenseDate') {            
                const dateValue = document.getElementById('expenseDate' + e.rowIndex).value;
                if(dateValue && dateValue.indexOf(":") === -1 && !dateUtil.isUIValidDate(dateValue)) {
                    row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
                    row["expenseDate"] = "";
                }
        }
        if(required(row.chargeExpenseType)){            
            row["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeUnit)){            
                row["chargeUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0
                && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0
                && required(row.chargeRateCurrency)) {            
                row["chargeRateCurrencyRequired"] = localConstant.validationMessage.CHARGE_CURRENCY_REQUIRED;
            }
        }

        if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
                row["payUnitRequired"] = localConstant.validationMessage.PAY_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.payRate)){            
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!lineItemPermision.isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
                row["payRateRequired"] = localConstant.validationMessage.PAY_RATE_VALIDATION_ON_UNIT;
            }            
            if (!requiredNumeric(row.payUnit) && row.payUnit > 0
                && !requiredNumeric(row.payRate) && row.payRate > 0
                && required(row.payRateCurrency)) {            
                row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;
            }
        }
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) && parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }
        return row;
    }

    submitConsumableModal = (e) => {        
        e.preventDefault();      
        const selectedData = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        let timesheetTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                timesheetTechnicalSpecialistId = row.timesheetTechnicalSpecialistId;
                pin = row.pin;
            });
        }  
        
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isUIValidDate(lineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payUnit)){
            IntertekToaster("Pay Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payRate)){
            IntertekToaster("Pay Rate is required", 'warningToast PercentRange');
            return false;
        }  
        if (!requiredNumeric(lineItemData.chargeUnit) && lineItemData.chargeUnit > 0
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.chargeRateCurrency)) {
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if (!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
            && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
            && required(lineItemData.payRateCurrency)) {
            IntertekToaster("Pay Currency is required", 'warningToast PercentRange');
            return false;
        }
        this.addDefaultCurrency(pin,lineItemData);
        
        if (!this.state.isNewRecord)
        {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["pin"] = pin;
            this.updatedData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId; 
            this.updatedData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            this.updatedData["timesheetId"]  = this.props.timesheetId;
            this.updatedData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            this.updatedData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            this.props.actions.UpdateTimesheetTechnicalSpecialistConsumable(this.updatedData, this.editedRowData);
            this.updateConsumableLineItems(this.updatedData, this.editedRowData);
        } else {
            lineItemData["TechSpecConsumableId"] = Math.floor(Math.random() * 999) - 1000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["timesheetTechnicalSpecialistId"] = timesheetTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.timesheetInfo.timesheetAssignmentId;
            lineItemData["timesheetId"]  = this.props.timesheetId;
            lineItemData["contractNumber"] = this.props.timesheetInfo.timesheetContractNumber;
            lineItemData["projectNumber"] = this.props.timesheetInfo.timesheetProjectNumber;
            lineItemData["chargeRate"] = lineItemData.chargeRate;
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);
            this.props.actions.AddTimesheetTechnicalSpecialistConsumable(lineItemData);
            this.state.timesheetTechSpecConsumables.push(lineItemData);
        }
        this.gridConsumableChild.gridApi.setRowData(this.state.timesheetTechSpecConsumables);
        this.cancelConsumableShowModal();
        this.updatedData = {};
    }    

    updateConsumableLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecConsumable = this.state.timesheetTechSpecConsumables;
        let checkProperty = "visitTechnicalSpecialistAccountConsumableId";
        if (editedRow.recordStatus === 'N') {
            checkProperty = "TechSpecConsumableId";
        }
        const index = techSpecConsumable.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
        
        const newState = Object.assign([], techSpecConsumable);
        if (index >= 0) {
            newState[index] = editedRow;
        }        
        this.setState((state) => {
            return {
                timesheetTechSpecConsumables: newState,
            };
        });
    }

    cancelConsumableShowModal=()=>{        
        this.editedRowData = {};
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                payUnit: 0,
                isConsumablePayUnitEntered: false
            };
        });
    }    

    editTimeRowHandler=(data)=>{
        this.updatedData ={};
        this.editedRowData = data;
        this.setGridDate();
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isNewRecord: false,
                payUnit: data.payUnit
            };
        });
    }

    editExpenseRowHandler=(data)=>{
        this.editedRowData = data;
        this.updatedData ={};
        this.setGridDate();
        const grossValue =  this.calculateExpenseGross(data);
        this.setState((state) => {
            return {
                expensesShowModal: !state.expensesShowModal,
                isNewRecord: false,
                grossValue:grossValue,
                chargeRateExchange:data.chargeRateExchange,
                payRateExchange:data.payRateExchange,
                payUnit: data.payUnit
            };
        });        
    }

    editTravelRowHandler=(data)=>{
        this.updatedData ={};
        this.editedRowData = data;
        this.setGridDate();
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                isNewRecord: false,
                chargeUnit:data.chargeUnit,
                payUnit:data.payUnit
            };
        });        
    }

    editConsumableRowHandler=(data)=>{
        this.updatedData ={};
        this.editedRowData = data;
        this.setGridDate();
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                isNewRecord: false,
                payUnit: data.payUnit
            };
        });        
    }

    expenseDateChange = (date) => {        
        this.setState({ expenseDate: date });
        this.updatedData["expenseDate"] = moment.parseZone(date).utc().format();
    }

    setGridDate = () => {
        if (this.editedRowData && !isEmptyOrUndefine(this.editedRowData.expenseDate)) {            
            this.setState({
                expenseDate: moment(this.editedRowData.expenseDate),
            });
        } else {
            this.setState({
                expenseDate: ""
            });
        }
    }
    onswapUpHandler=(e)=>{
        e.preventDefault();
        const editedRow = Object.assign({},this.editedRowData, this.updatedData);
        if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {
            const payUnit = this.state.payUnit;
            this.setState({ chargeUnit:payUnit,payUnit:payUnit });
            this.updatedData["chargeUnit"] = payUnit; 
        } else {
            if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                const payUnit = this.state.payUnit;
                this.setState({ chargeUnit:payUnit,payUnit:payUnit });
                this.updatedData["chargeUnit"] = payUnit; 
            } else {
                const payUnit = this.state.payUnit;
                this.setState({ payUnit:payUnit });
                if(editedRow.payExpenseType === 'Miles') {
                    const changedChargeUnit = this.milesToKiloMeters(payUnit);
                    this.setState({ chargeUnit:changedChargeUnit });
                    this.updatedData["chargeUnit"] = changedChargeUnit; 
                } else {
                    const changedChargeUnit = this.kiloMetersToMiles(payUnit);
                    this.setState({ chargeUnit:changedChargeUnit });
                    this.updatedData["chargeUnit"] = changedChargeUnit; 
                }                
            }
        }
       
    }
    onswapDownHandler=(e)=>{
        e.preventDefault();
        const editedRow = Object.assign({},this.editedRowData, this.updatedData);        
        if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {
            const chargeUnit = this.state.chargeUnit;
            this.setState({ chargeUnit:chargeUnit,payUnit:chargeUnit });      
            this.updatedData["payUnit"] = chargeUnit;      
        } else {
            if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                const chargeUnit = this.state.chargeUnit;
                this.setState({ chargeUnit:chargeUnit,payUnit:chargeUnit }); 
                this.updatedData["payUnit"] = chargeUnit;      
            } else {
                const chargeUnit = this.state.chargeUnit;
                this.setState({ chargeTotalUnit:chargeUnit });
                if(editedRow.chargeExpenseType === 'Miles') {
                    const changedPayUnit = this.milesToKiloMeters(chargeUnit);
                    this.setState({ payUnit:changedPayUnit });
                    this.updatedData["payUnit"] = changedPayUnit;
                } else {
                    const changedPayUnit = this.kiloMetersToMiles(chargeUnit);
                    this.setState({ payUnit:changedPayUnit });
                    this.updatedData["payUnit"] = changedPayUnit;
                }                
            }
        }  
    }

    milesToKiloMeters(value) {
        return Math.ceil(value/0.62137);
    }

    kiloMetersToMiles(value) {
        return Math.ceil(value * 0.62137);
    }

    showAdditionalExpenses=()=>{
        this.setState({
            isAddUnlinkedAssignmentExpenses: true            
        });
    }

    inputHandleShowAllRates=(e)=>{
        const inputvalue = formInputChangeHandler(e);        
        this.props.actions.UpdateShowAllRates(inputvalue.value);
    }

    onChangeHandler = (e) => {        
        // e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;

        if(this.state.timeShowModal) {
            if(inputvalue.name === 'payUnit') {
                this.setState((state) => {
                    return {
                        payUnit: inputvalue.value,
                        isTimePayUnitEntered: true
                    };
                });
            }

            if(inputvalue.name === 'chargeTotalUnit') {                
                if(this.state.isTimePayUnitEntered === undefined 
                        || this.state.isTimePayUnitEntered === false) {
                    this.setState((state) => {
                        return {                            
                            payUnit: inputvalue.value
                        };
                    });
                    this.updatedData["payUnit"] = inputvalue.value;
                }
            } 
        }

        if (this.state.expensesShowModal &&
            (inputvalue.name === 'payRate' || inputvalue.name === 'payUnit' || inputvalue.name === 'payRateTax')) {
            const data = {
                payRate: this.updatedData.payRate,
                payUnit: this.updatedData.payUnit,
                payRateTax: this.updatedData.payRateTax
            };
            const grossValue = this.calculateExpenseGross(data);
            this.setState(() => {
                return {
                    grossValue: grossValue
                };
            });            
        }

        if(this.state.travelShowModal) {
            if(inputvalue.name === 'chargeUnit') {
                this.setState(() => {
                    return {
                        chargeUnit:inputvalue.value
                    };
                });
            }
            if(inputvalue.name === 'payUnit') {
                this.setState(() => {
                    return {
                        payUnit:inputvalue.value
                    };
                });
            }
        } 

        if (this.state.expensesShowModal) {
            if (inputvalue.name === "chargeRateExchange") {
                this.setState(() => {
                    return {
                        chargeRateExchange: inputvalue.value
                    };
                });
            }

            if (inputvalue.name === "payRateExchange") {
                this.setState(() => {
                    return {
                        payRateExchange: inputvalue.value
                    };
                });
            }

            if (inputvalue.name === 'currency'
                || inputvalue.name === 'chargeRateCurrency'
                || inputvalue.name === 'payRateCurrency'){
                this.fetchDefaultExchangeRate(inputvalue.name, inputvalue.value);
                }

            if(inputvalue.name ==='chargeUnit') {                
                if(this.state.isExpensePayUnitEntered === undefined 
                        || this.state.isExpensePayUnitEntered === false) {
                    this.updatedData["payUnit"] = inputvalue.value;      
                    const editedRow = Object.assign({},this.editedRowData, this.updatedData);          
                    this.setState((state) => {
                        return {
                            payUnit: inputvalue.value
                        };
                    });
                    const dataExpense = { payRate:editedRow.payRate,
                        payUnit:editedRow.payUnit,
                        payRateTax:editedRow.payRateTax };
                    const grossValue = this.calculateExpenseGross(dataExpense);            
                    this.setState((state) => {
                        return {
                            grossValue:grossValue
                        };
                    });
                }                                
            }

            if(inputvalue.name ==='payUnit') {                
                this.setState((state) => {
                    return {
                        payUnit: inputvalue.value,
                        isExpensePayUnitEntered: true
                    };
                });               
            } 
        }

        if(this.state.consumableShowModal) {
            if(inputvalue.name === 'chargeUnit') {
                if(this.state.isConsumablePayUnitEntered === undefined 
                    || this.state.isConsumablePayUnitEntered === false) {
                    this.setState((state) => {
                        return {                            
                            payUnit: inputvalue.value
                        };
                    });
                    this.updatedData["payUnit"] = inputvalue.value;
                }
            }
            if(inputvalue.name === 'payUnit') {
                this.setState((state) => {
                    return {
                        payUnit:inputvalue.value,
                        isConsumablePayUnitEntered:true
                    };
                });
            }
        }
    }

    fetchDefaultExchangeRate = async (inputname, inputvalue) => {

        const obj = {
            currencyFrom: '',
            currencyTo: '',
            effectiveDate: currentDate
        };
        let chargeCurrency = '', payCurrency = '';

        if (!this.state.isNewRecord) {
            obj.currencyFrom = this.updatedData.currency ? this.updatedData.currency : this.editedRowData.currency;
            chargeCurrency = this.updatedData.chargeRateCurrency?this.updatedData.chargeRateCurrency:this.editedRowData.chargeRateCurrency;
            payCurrency = this.updatedData.payRateCurrency?this.updatedData.payRateCurrency:this.editedRowData.payRateCurrency;
        }
        else {
            obj.currencyFrom = this.updatedData.currency;
            chargeCurrency = this.updatedData.chargeRateCurrency;
            payCurrency = this.updatedData.payRateCurrency;
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
            if (exchangeRates.length > 0) {
                const rateCurrencyObj= {
                    chargeRateCurrency: chargeCurrency,
                    payRateCurrency: payCurrency
                };
                 await this.addDefaultExchangeRate(exchangeRates,rateCurrencyObj);
                    this.setState(() => {
                        return {
                            chargeRateExchange: rateCurrencyObj.chargeRateExchange ? this.formatExchangeRate(rateCurrencyObj.chargeRateExchange) : null,
                            payRateExchange: rateCurrencyObj.payRateExchange ? this.formatExchangeRate(rateCurrencyObj.payRateExchange) : null
                        };
                    });
            }
        }
        else if (inputname === 'chargeRateCurrency' && !isEmptyOrUndefine(obj.currencyFrom)) {
            obj.currencyTo = inputvalue;
            const res1 = await this.props.actions.FetchCurrencyExchangeRate([ obj ]);
            if (res1 && res1.length > 0) {
                this.setState(() => {
                    return {
                        chargeRateExchange: res1[0] ? this.formatExchangeRate(res1[0].rate) : null
                    };
                });
            }
        }
        else if (inputname === 'payRateCurrency' && !isEmptyOrUndefine(obj.currencyFrom)) {
            obj.currencyTo = inputvalue;
            const res = await this.props.actions.FetchCurrencyExchangeRate([ obj ]);
            if (res && res.length > 0) {
                this.setState(() => {
                    return {
                        payRateExchange: res[0] ? this.formatExchangeRate(res[0].rate) : null
                    };
                });
            }
        }
    }

    formatExchangeRate= (rate)=>{
        return FormatSixDecimal(rate);
    }
    
    calculateExpenseGross = (data) => {
        const payUnit = isNaN(parseFloat(data.payUnit))?0:parseFloat(data.payUnit),
         payRate = isNaN(parseFloat(data.payRate))?0:parseFloat(data.payRate),
         payRateTax = isNaN(parseFloat(data.payRateTax))?0:parseFloat(data.payRateTax); 
         return (payUnit * payRate) + payRateTax;
     };

    bindTechSpecLineItems = async (pin, e) => {
        const lineItemPermision = this.isLineItemEditable();      
        const techSpecTime = [];
        const techSpecTravel = [];
        const techSpecExpense = [];
        const techSpecConsumable = []; 
        let rowIndex = 0;
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTimes)) {
            this.props.timesheetTechnicalSpecialistTimes.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeTotalUnit = FormatTwoDecimal(row.chargeTotalUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);
                    row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;                    
                    row.payRateReadonly = this.props.payRateReadonly;
                    row.modifyPayUnitPayRate = (this.props.modifyPayUnitPayRate ? false : true);
                    row = this.TimeItemValidation(row, e, rowIndex, lineItemPermision);
                    row.lineItemPermision = lineItemPermision;
                    if(isEmptyOrUndefine(row.chargeRateDescription)) {
                        row.chargeRateDescription = this.getChargeDescription(row.pin, row.expenseDate, row.chargeExpenseType);
                    }
                    if(isEmptyOrUndefine(row.payRateDescription)) {
                        row.payRateDescription = this.getPayDescription(row.pin, row.expenseDate, row.chargeExpenseType);
                    }
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }
                    //row.hasNoChargeRate = row.chargeRate > 0 ? false : true;
                    techSpecTime.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    timesheetTechSpecTimes: techSpecTime,
                };
            });
            if(this.state.isDatePicker && this.gridTimeChild && this.gridTimeChild.gridApi) this.gridTimeChild.gridApi.setRowData(techSpecTime); 
        }        
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistTravels)) {
            rowIndex = 0;
            this.props.timesheetTechnicalSpecialistTravels.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeUnit = FormatTwoDecimal(row.chargeUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);
                    row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
                    row.payRateReadonly = this.props.payRateReadonly;
                    row = this.TravelItemValidation(row, e, rowIndex, lineItemPermision);
                    row.lineItemPermision = lineItemPermision;
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }
                    //row.hasNoChargeRate = row.chargeRate > 0 ? false : true;
                    techSpecTravel.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    timesheetTechSpecTravels: techSpecTravel,
                };
            });
            if(this.state.isDatePicker && this.gridTravelChild && this.gridTravelChild.gridApi) this.gridTravelChild.gridApi.setRowData(techSpecTravel);
        }
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistExpenses)) {
            rowIndex = 0;
            for (let i = 0; i < this.props.timesheetTechnicalSpecialistExpenses.length; i++) {    
                let row = this.props.timesheetTechnicalSpecialistExpenses[i];  
                if(pin == row.pin && row.recordStatus !== 'D') {
                    if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.chargeRateCurrency) && row.currency !== row.chargeRateCurrency) && (row.chargeRateExchange === null || row.chargeRateExchange === '' || row.chargeRateExchange === 0)) {
                        if(this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.filter(rates => rates.currencyFrom === row.currency 
                        && rates.currencyTo === row.chargeRateCurrency).length > 0) {
                            this.props.timesheetExchangeRates.map(rates => {
                                if (rates.currencyFrom === row.currency && rates.currencyTo === row.chargeRateCurrency) {
                                    row.chargeRateExchange = FormatSixDecimal(rates.rate);
                                    if(row.recordStatus !== "N") row.recordStatus = "M";
                                };
                            });
                        } else {
                            const exchangeRates = [ {
                                currencyFrom: row.currency,
                                currencyTo: row.chargeRateCurrency,
                                effectiveDate: row.expenseDate
                            } ];
                            const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates, this.props.timesheetInfo.timesheetContractNumber);
                            if (res && res.length > 0) {
                                row.chargeRateExchange = res[0] ? FormatSixDecimal(res[0].rate) : null;
                                if(row.recordStatus !== "N") row.recordStatus = "M";
                            }
                        } 
                    } else if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.chargeRateCurrency) && row.currency === row.chargeRateCurrency) && (row.chargeRateExchange === null || row.chargeRateExchange === '' || row.chargeRateExchange === 0)) {                       
                        row.chargeRateExchange = 1.000000;
                        if(row.recordStatus !== "N") row.recordStatus = "M";
                    } else {
                        row.chargeRateExchange = FormatSixDecimal(row.chargeRateExchange);
                    }
                    if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.payRateCurrency) && row.currency !== row.payRateCurrency) && (row.payRateExchange === null || row.payRateExchange === '' || row.payRateExchange === 0)) {
                        if(this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.filter(rates => rates.currencyFrom === row.currency 
                        && rates.currencyTo === row.payRateCurrency).length > 0) {
                            this.props.timesheetExchangeRates.map(rates => {
                                if (rates.currencyFrom === row.currency && rates.currencyTo === row.payRateCurrency) {
                                    row.payRateExchange = FormatSixDecimal(rates.rate);
                                    if(row.recordStatus !== "N") row.recordStatus = "M";
                                };
                            });    
                        } else {
                            const exchangeRates = [ {
                                currencyFrom: row.currency,
                                currencyTo: row.payRateCurrency,
                                effectiveDate: row.expenseDate
                            } ];
                            const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates, this.props.timesheetInfo.timesheetContractNumber);
                            if (res && res.length > 0) {
                                row.payRateExchange = res[0] ? FormatSixDecimal(res[0].rate) : null;
                                if(row.recordStatus !== "N") row.recordStatus = "M";
                            }
                        }
                    } else if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.payRateCurrency) && row.currency === row.payRateCurrency) && (row.payRateExchange === null || row.payRateExchange === '' || row.payRateExchange === 0)) {
                        row.payRateExchange = 1.000000;
                        if(row.recordStatus !== "N") row.recordStatus = "M";
                    } else {
                        row.payRateExchange = FormatSixDecimal(row.payRateExchange);
                    }
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeUnit = FormatTwoDecimal(row.chargeUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);
                    row.payRateTax = FormatTwoDecimal(row.payRateTax);
                    row.chargeRateReadonly = this.props.chargeRateReadonly;
                    row.payRateReadonly = this.props.payRateReadonly;
                    row = this.ExpenseItemValidation(row, e, rowIndex, lineItemPermision);
                    row.lineItemPermision = lineItemPermision;
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }
                    //row.hasNoChargeRate = row.chargeRate > 0 ? false : true;
                    techSpecExpense.push(row);
                    rowIndex++;
                }
            }
            this.setState((state) => {
                return {
                    timesheetTechSpecExpenses: techSpecExpense,
                };
            });
            if(this.state.isDatePicker && this.gridExpenseChild && this.gridExpenseChild.gridApi) this.gridExpenseChild.gridApi.setRowData(techSpecExpense);
        }
        if(!isEmptyOrUndefine(this.props.timesheetTechnicalSpecialistConsumables)) {
            rowIndex = 0;
            this.props.timesheetTechnicalSpecialistConsumables.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeUnit = FormatTwoDecimal(row.chargeUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);
                    row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
                    row.payRateReadonly = this.props.payRateReadonly;
                    if(isEmptyOrUndefine(row.payType) && !isEmptyOrUndefine(row.chargeExpenseType)) {
                        row.payType = row.chargeExpenseType;
                    }
                    row = this.ConsumableItemValidation(row, e, rowIndex, lineItemPermision);
                    row.lineItemPermision = lineItemPermision;                    
                    if(isEmptyOrUndefine(row.chargeDescription)) {                        
                        row.chargeDescription = this.getChargeDescription(row.pin, row.expenseDate, row.chargeExpenseType);
                    }
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }
                    //row.hasNoChargeRate = row.chargeRate > 0 ? false : true;
                    techSpecConsumable.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    timesheetTechSpecConsumables: techSpecConsumable,
                };
            });
            if(this.state.isDatePicker && this.gridConsumableChild && this.gridConsumableChild.gridApi) this.gridConsumableChild.gridApi.setRowData(techSpecConsumable);
        }
    }

      /** On row select of schedule populate the charge rate and pay rate */
      techSpecRowSelectHandler = (e) => {       
        if(e.node.isSelected()){
            this.bindTechSpecLineItems(e.data.pin);
        } else {
            const selectedData = this.gridChild.getSelectedRows();            
            if(isEmptyOrUndefine(selectedData)) {
                e.node.setSelected(true);
            }
        }
    };

    isLineItemEditable = () => {
        //Once the line item is created by the OC (Intercompany Scenario) OC coordinator can edit the 
        //line item before the stage of Approval (Approved by Operating Company Coordinator)
        // whereas at this stage CHC cannot edit the data which is saved by OC (still not approved by OC).
        const {
            isInterCompanyAssignment,
            isOperator,
            isCoordinator,
            timesheetInfo,
            isOperatingCompany,
            isCoordinatorCompany,
            interactionMode
        } = this.props;
        const lineItemPermision = {
            isLineItemEditable : true,
            isLineItemEditableOnChargeSide:true,
            isLineItemEditableOnPaySide:true,
            isLineItemDeletable:true,
            isLineItemEditableExpense: true,
            isCoordinatorCompany: isCoordinatorCompany,
            isOperatingCompany: isOperatingCompany,
            isCHUser: (isInterCompanyAssignment && isCoordinatorCompany ? true : false),
            isLoggedinCompany: false,
            isInterOperatingCompany: (isInterCompanyAssignment && isOperatingCompany ? true : false)
        };       
        const {
            isNewRecord,
        } = this.state;

        if(interactionMode || ([ 'A' ].includes(timesheetInfo.timesheetStatus) && !this.props.modifyAddApprovedLines)) {
            lineItemPermision.isLineItemEditable = false;
            lineItemPermision.isLineItemEditableExpense = false;
            lineItemPermision.isLineItemEditableOnChargeSide = false;
            lineItemPermision.isLineItemEditableOnPaySide = false;
            lineItemPermision.isLineItemDeletable = false;
        } else if (isInterCompanyAssignment) {
            if (isCoordinatorCompany && [ 'N', 'C', 'J', 'T', 'Q', 'R' ].includes(timesheetInfo.timesheetStatus)) {
                lineItemPermision.isLineItemEditable = false;
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = false;
            } else if (isCoordinatorCompany && 'O' === timesheetInfo.timesheetStatus) {
                lineItemPermision.isLineItemEditable = true;
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = true;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = true;
            } else if (isCoordinatorCompany && 'A' === timesheetInfo.timesheetStatus) {
                lineItemPermision.isLineItemEditable = true;
                lineItemPermision.isLineItemEditableExpense = true;
                lineItemPermision.isLineItemEditableOnChargeSide = true;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = true;
            } else if (isOperatingCompany && 'O' === timesheetInfo.timesheetStatus) {                
                lineItemPermision.isLineItemEditable = this.props.modifyAddApprovedLines;                
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = this.props.modifyAddApprovedLines;
                lineItemPermision.isLineItemDeletable = this.props.modifyAddApprovedLines;
            } else if(isOperatingCompany && 'A' === timesheetInfo.timesheetStatus){
                lineItemPermision.isLineItemEditable = this.props.modifyAddApprovedLines;                
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = this.props.modifyAddApprovedLines;
                lineItemPermision.isLineItemDeletable = true;
            }
        }
        
        return lineItemPermision;
    }
    tabSelect = (index, lastIndex, event) => {
        const lineItemPermision = this.isLineItemEditable();
        if(this.props.pageMode!==localConstant.commonConstants.VIEW && this.props.isExpenseOpen) {
            if (event.target.innerText === localConstant.timesheet.EXPENSES && lineItemPermision.isLineItemEditable) {
                const { unLinkedAssignmentExpenses } = this.props;
                if (unLinkedAssignmentExpenses && unLinkedAssignmentExpenses.length > 0) {
                    //const unLinkedExpenses = this.props.unLinkedAssignmentExpenses.filter(x => !x.isAlreadyLinked); -- Need to revert once got confirmation
                    if (unLinkedAssignmentExpenses.length > 0) {
                        this.setState({
                            isAddUnlinkedAssignmentExpenses: true
                        });
                    }
                }                
                if(this.props.isExpenseOpen) this.props.actions.UpdateExpenseOpen(false);
            }
        }
        
        this.setState({
            isShowAdditionalExpenses: (this.props.pageMode !== localConstant.commonConstants.VIEW && lineItemPermision.isLineItemEditable
                            && event.target.innerText === localConstant.timesheet.EXPENSES ? true : false),
            moduleType: (event.target.innerText === localConstant.visit.TIME ? "R" : "")
        });

        this.setState({
            selectedTabIndex: index            
        });
    }
    submitUnLinkedExpenses = async (e) => {

        e.preventDefault();
        const selectedRecords = isEmptyReturnDefault(this.UnLinkedAssignmentExpensesGrid.getSelectedRows());
        const selectedTechnicalSpecialist = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        const currentSelectedTechSpec = selectedTechnicalSpecialist.length > 0 ? selectedTechnicalSpecialist[0].pin : 0;
        const { timesheetStartDate } = this.props.timesheetInfo;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ 
            techSpecRateSchedules: this.props.techSpecRateSchedules, pin:currentSelectedTechSpec });
        if (selectedRecords && selectedRecords.length > 0) {
            selectedRecords.forEach(async (rowData) => {
                const rateCurrencyObj= {};
                if (!isEmptyOrUndefine(rowData.currencyCode)
                        && !isEmptyOrUndefine(defaultScheduleCurrency.defaultChargeCurrency)
                        && !isEmptyOrUndefine(defaultScheduleCurrency.defaultPayCurrency)) {
                            rateCurrencyObj.chargeRateCurrency = defaultScheduleCurrency.defaultChargeCurrency;
                        rateCurrencyObj.payRateCurrency = defaultScheduleCurrency.defaultPayCurrency;
                        const exchangeRates = [ {
                            currencyFrom: rowData.currencyCode,
                            currencyTo: defaultScheduleCurrency.defaultChargeCurrency,
                            effectiveDate: timesheetStartDate?
                            moment(timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT)
                            :currentDate
                        },
                        {
                            currencyFrom: rowData.currencyCode,
                            currencyTo: defaultScheduleCurrency.defaultPayCurrency,
                            effectiveDate: timesheetStartDate?
                            moment(timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT)
                            :currentDate
                        } ];
                       await this.addDefaultExchangeRate(exchangeRates,rateCurrencyObj);
                    }
                selectedTechnicalSpecialist.forEach((eachTs) => {
                    const tempData = {};
                    tempData.expenseDate = this.props.timesheetInfo.timesheetStartDate;
                    tempData.chargeExpenseType = rowData.expenseType;
                    tempData.currency = rowData.currencyCode ? rowData.currencyCode : null;
                    tempData.chargeRateCurrency = defaultScheduleCurrency.defaultChargeCurrency;
                    tempData.payRateCurrency = defaultScheduleCurrency.defaultPayCurrency;
                    tempData.chargeUnit = rowData.totalUnit;
                    tempData.chargeRate = rowData.perUnitRate;
                    tempData.isContractHolderExpense = false;
                    tempData.expenseDescription = rowData.description;
                    tempData.payRate = "0.0000";
                    tempData.payRateTax = 0;
                    tempData.chargeRateExchange = rateCurrencyObj.chargeRateExchange ? this.formatExchangeRate(rateCurrencyObj.chargeRateExchange) : null;
                    tempData.payRateExchange =  rateCurrencyObj.payRateExchange ? this.formatExchangeRate(rateCurrencyObj.payRateExchange) : null;
                    tempData.payUnit = 0;
                    tempData.pin = eachTs.pin;
                    tempData.timesheetTechnicalSpecialistId = eachTs.timesheetTechnicalSpecialistId;
                    tempData.recordStatus = 'N';
                    tempData.TechSpecExpenseId = Math.floor(Math.random() * 99) - 100;
                    tempData.assignmentExpensesAlreadyMapped = false;
                    tempData.assignmentId = this.props.timesheetInfo.timesheetAssignmentId;
                    tempData.timesheetId = this.props.timesheetInfo.timesheetId;
                    tempData.contractNumber = this.props.timesheetInfo.timesheetContractNumber;
                    tempData.projectNumber = this.props.timesheetInfo.timesheetProjectNumber;
                    tempData.assignmentAdditionalExpenseId = rowData.assignmentAdditionalExpenseId;
                    tempData.lineItemPermision = this.isLineItemEditable();
                    this.props.actions.AddTimesheetTechnicalSpecialistExpense(tempData);
                    if (currentSelectedTechSpec === eachTs.pin){
                        this.state.timesheetTechSpecExpenses.unshift(tempData);
                        setTimeout(() => {
                            this.gridExpenseChild.gridApi.setRowData(this.state.timesheetTechSpecExpenses);
                        }, 0);
                    }
                });
            });
            
            this.props.actions.SetLinkedAssignmentExpenses(selectedRecords);
        } else {
            IntertekToaster("Please select the record", 'warningToast GridValidation');
            return false;
        }
        this.setState({
            isAddUnlinkedAssignmentExpenses: false
        });
    }

    cancelUnLinkedExpenses = ()=>{
        this.setState({
            isAddUnlinkedAssignmentExpenses:false
        });
    }

    timesheetGrossMarginClass = (grossMargin)=>{
        const { companyBusinessUnitExpectedMargin } = this.props;
       
        if(grossMargin === null){
            return '';
        }
        grossMargin = isNaN(parseFloat(grossMargin))?0:parseFloat(grossMargin);
        if(grossMargin < 0){
            return 'bold text-red';
        }
        if(companyBusinessUnitExpectedMargin !== null 
            && grossMargin < companyBusinessUnitExpectedMargin){
            return 'bold text-red';
        }
        return '';
    }

    checkNumber = (e) => {
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) ){
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);
        }
        this.formInputHandler(e);
    }

    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    };

    decimalWithLimtFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    };

    cancelInvoiceNotesPopup = ()=>{
        this.setState({
            isInvoiceNotesPopup:false
        });
    }

    invoiceNotes = () => {
        this.setState({
          isInvoiceNotesPopup: true
        });
    }

    onCellFocused = (e) => {    
        if(e.rowIndex !== null) {
            const selectedData = this.gridChild.getSelectedRows();       
            let pin = 0;
            if(selectedData !== undefined) { 
                selectedData.map(row => {
                    pin = row.pin;
                });
            }
            this.bindTechSpecLineItems(pin, e);

            if(e.column.getId() === 'expenseDate') {
                this.setState({ isDatePicker : true });
            } else {
                this.setState({ isDatePicker : false });
            }
            
            if(document.getElementById(e.column.getId() + e.rowIndex) !== null) {
                if(document.getElementById(e.column.getId() + e.rowIndex).type === 'text') {
                    document.getElementById(e.column.getId() + e.rowIndex).select();
                } else if(document.getElementById(e.column.getId() + e.rowIndex).type === 'select-one') {
                    if(e.forceBrowserFocus)
                        document.getElementById(e.column.getId() + e.rowIndex).focus();
                    else
                        document.getElementById(e.column.getId() + e.rowIndex).click();
                } else {
                    document.getElementById(e.column.getId() + e.rowIndex).focus();
                }
            }
        }
    }
     
    render() {
        const { interactionMode,
            techSpecRateSchedules,
            timesheetTechnicalSpecilists,
            timesheetTechnicalSpecialistsGrossMargin,
            timesheetInfo }= this.props;

        const expensesCategories =  this.props.expenseTypes;        
        this.timeTypes = expensesCategories.filter(expense =>expense.chargeType === 'R');
        this.travelTypes = expensesCategories.filter(expense =>expense.chargeType === 'T');
        this.expenseTypes = expensesCategories.filter(expense =>expense.chargeType === 'E');
        this.consumableTypes = expensesCategories.filter(expense => (expense.chargeType === 'C' || expense.chargeType === 'Q'));
        const workFlowType= timesheetInfo.assignmentProjectWorkFlow && timesheetInfo.assignmentProjectWorkFlow.trim();    
        const isShowConsumableTab = 'N' === workFlowType;        
        const lineItemPermision = this.isLineItemEditable();
        const grossMarginClass = this.timesheetGrossMarginClass(timesheetTechnicalSpecialistsGrossMargin);

        let disableAddDelete = false;
        if(!isEmptyOrUndefine(timesheetTechnicalSpecilists)) {
            const selectedTechnicalSpecialist = timesheetTechnicalSpecilists.filter(x => x.recordStatus !== "D");
            if (isEmptyOrUndefine(selectedTechnicalSpecialist)) {
                disableAddDelete = true;
            }  
        } else {
            disableAddDelete = true;
        }
            
        return (
            <Fragment>
                {this.state.isAddUnlinkedAssignmentExpenses ?
                    <Modal title="Add Unlinked Expenses"
                        modalClass="UnlinkedAssignmentExpensesModel"
                        modalId="UnlinkedAssignmentExpensesPopup"
                        formId="UnlinkedAssignmentExpensesForm"
                        buttons={
                            [
                            {
                                name: localConstant.commonConstants.CANCEL,
                                action: this.cancelUnLinkedExpenses,
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                                type: "button"
                            }, {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitUnLinkedExpenses,
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                            }
                             ]
                        }
                        isShowModal={this.state.isAddUnlinkedAssignmentExpenses}>

                        <div className="customCard left fullWidth">
                            <ReactGrid
                                gridRowData={this.props.unLinkedAssignmentExpenses}
                                gridColData={this.headerData.UnLinkedAssignmentExpenses}
                                onRef={ref => { this.UnLinkedAssignmentExpensesGrid = ref; }}
                            />
                        </div>
                    </Modal> : null}
            {this.state.scheduleRateShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalRateScheduleMaxHeight"
                    title={ localConstant.timesheet.SCHEDULE_RATES}
                    buttons={[                           
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelChargeScheduleModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true
                            } ]
                    }
                    isShowModal={this.state.scheduleRateShowModal}>
                    <ScheduleRate 
                     techSpecRateSchedules={techSpecRateSchedules}
                     pin={this.editedRowData.pin}
                     selectedCompany = {this.props.selectedCompany}
                     modifyPayUnitPayRate = {this.props.modifyPayUnitPayRate}
                    gridHeader={this.headerData}/>
                   
                </Modal>
            }
            {this.state.timeShowModal &&
                <Modal
                    modalClass="chargescheduleModal "
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.timesheet.TIME}
                    buttons={[                           
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelTimeModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitTimeModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                } ]
                    }
                    isShowModal={this.state.timeShowModal}>
                    <TimeModal
                        editedTimeData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.timeTypes}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        payUnit = {this.state.payUnit}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                    />
                </Modal>
            }
            {this.state.expensesShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.timesheet.EXPENSES}
                    buttons={[
                            
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelExpensesShowModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitExpenseModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                } ]
                    }
                    isShowModal={this.state.expensesShowModal}>
                    <ExpensesModal 
                        editedExpenseData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.expenseTypes}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        grossValue={this.state.grossValue}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        chargeRateExchange = {this.state.chargeRateExchange}
                        payRateExchange =  {this.state.payRateExchange}
                        payUnit = {this.state.payUnit}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                    />
                </Modal>
            }
             {this.state.travelShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.timesheet.TRAVEL}
                    buttons={[
                           
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelTravelShowModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitTravelModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                } ]
                    }
                    isShowModal={this.state.travelShowModal}>
                    <TravelModal 
                        editedTravelData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.travelTypes}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        swapUpHandler={this.onswapUpHandler}
                        swapDownHandler={this.onswapDownHandler}                       
                        chargeUnit={this.state.chargeUnit}
                        payUnit={this.state.payUnit}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                    />
                </Modal>
            }
            {this.state.consumableShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.timesheet.CONSUMABLE_EQUIPMENT}
                    buttons={[
                            
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelConsumableShowModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            },{
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitConsumableModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                } ]
                    }
                    isShowModal={this.state.consumableShowModal}>
                    <ConsumableModal 
                        editedConsumableData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.consumableTypes}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        payUnit = {this.state.payUnit}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                    />
                </Modal>
            }
            {this.state.payRateShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    title= { this.state.isPayRate ? localConstant.visit.PAY_RATES :localConstant.visit.RATES}
                    buttons={[
                        {
                        name: localConstant.commonConstants.CANCEL,
                        action: this.cancelPayRateModal,
                        btnClass: "btn-small mr-2",
                        showbtn: true,
                        type:"button"
                        },
                        {
                            name: localConstant.commonConstants.SUBMIT,
                            action: this.submitPayRateModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true
                            }, ]
                    }
                    isShowModal={this.state.payRateShowModal}>
                    <ReactGrid 
                        gridRowData={this.state.selectedPayRateGridData} 
                        gridColData={this.state.isPayRate ? (this.state.moduleType === "R" && !this.props.modifyPayUnitPayRate ? this.headerData.payRateModifyHeader : this.headerData.payRateHeader) : this.headerData.chargeRateHeader}    
                        onRef={ref => { this.gridPayRate = ref; }} 
                        paginationPrefixId={this.state.isPayRate ? localConstant.paginationPrefixIds.timesheetPayRateId : localConstant.paginationPrefixIds.timesheetChargeRateId}           
                    />
                </Modal>
            }
            {this.state.isInvoiceNotesPopup ?
                <Modal title={localConstant.project.invoicingDefaults.INVOICE_NOTES}
                    modalClass="visitModal"
                    modalId="timesheetInvoiceNotesPopup"
                    formId="timesheetInvoiceNotesForm"
                    buttons={
                                [
                                    {
                                        name: localConstant.commonConstants.CANCEL,
                                        action: this.cancelInvoiceNotesPopup,
                                        btnID: "invoiceNotesBtn",
                                        btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                                        showbtn: true,
                                        type:"button"
                                    }
                                ]
                            }
                    isShowModal={this.state.isInvoiceNotesPopup}>
                    <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 mb-4'
                    colSize="col s12"
                    type='textarea'
                    name="projectInvoiceInstructionNotes"
                    rows="10"
                    inputClass=" customInputs textAreaInView"
                    labelClass="customLabel"
                    maxLength={4000}
                    defaultValue={isEmptyOrUndefine(timesheetInfo.projectInvoiceInstructionNotes) ? '' : timesheetInfo.projectInvoiceInstructionNotes}
                    readOnly={true}
                    />
                </Modal> : null}
            
                <div className="customCard col s12 horizontalTabs">
                    <div className="customCard left fullWidth">
                        <h6 className="bold col s12 pl-0 pr-3">{localConstant.timesheet.TECHNICAL_SPECIALIST_ACCOUNTS}</h6>
                        <div className='row timesheetGrossMarginClass mb-0'>
                            <div className="bold col s6 mb-0">Timesheet Gross Margin: {(timesheetTechnicalSpecialistsGrossMargin)
                                ? <span className={grossMarginClass}>
                                { grossMarginClass ? <i class="zmdi zmdi-alert-triangle"></i>:'' }{timesheetTechnicalSpecialistsGrossMargin}
                                </span>
                                : ''}</div>
                                <div className="col s6 right mt--1">                   
                                    <CustomInput
                                        name="isShowAllRates"
                                        type='switch'
                                        switchLabel={localConstant.commonConstants.SHOW_ALL_RATES}
                                        colSize='pr-0 mt-1 right'
                                        isSwitchLabel={true}
                                        className="lever"
                                        switchName="isShowAllRates"
                                        onChangeToggle={this.inputHandleShowAllRates}
                                        checkedStatus={this.props.isShowAllRates}
                                        switchKey={this.props.isShowAllRates}
                                        disabled={this.props.interactionMode}
                                    />
                                    <a className="waves-effect btn-small right invoiceNotesMargin" onClick={this.invoiceNotes} >
                                        {localConstant.project.invoicingDefaults.INVOICE_NOTES}</a>
                                    {this.state.isShowAdditionalExpenses && this.props.pageMode!==localConstant.commonConstants.VIEW && <a className="waves-effect btn-small right" 
                                        onClick={this.showAdditionalExpenses} disabled = {false} >
                                        {localConstant.commonConstants.SHOW_ADDITIONAL_EXPENSES}</a> }
                                </div>                           
                        </div>
                        <ReactGrid
                            gridRowData={isEmptyReturnDefault(timesheetTechnicalSpecilists)}
                            gridColData={this.headerData.technicalSpecialist}
                            rowSelected={this.techSpecRowSelectHandler}
                            onRef={ref => { this.gridChild = ref; }}
                            paginationPrefixId={localConstant.paginationPrefixIds.timesheetTechSpecGridId}
                        />
                    </div>
                    <div className="customCard col s12 horizontalTabs pl-0 pr-0 mt-2 fullWidth">
                        <Tabs tabActive={1} onSelect={this.tabSelect}>
                        {/* <Tabs tabActive={1}>  */}
                        <TabList>
                            <Tab className="react-tabs__tab time">{localConstant.timesheet.TIME}</Tab>
                            <Tab className="react-tabs__tab expenses">{localConstant.timesheet.EXPENSES}</Tab>
                            <Tab className="react-tabs__tab travel">{localConstant.timesheet.TRAVEL}</Tab>
                            {
                                isShowConsumableTab ? <Tab className="react-tabs__tab equipment">{localConstant.visit.CONSUMABLE_EQUIPMENT}</Tab> : null
                            }
                        </TabList>
                        <TabPanel title={localConstant.timesheet.TIME}>
                            <ReactGrid 
                                gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                                gridRowData={ this.state.timesheetTechSpecTimes}
                                gridColData={this.headerData.timeHeaderDetails}
                                onRef={ref => { this.gridTimeChild = ref; }}
                                onCellFocused={this.onCellFocused}
                                gridTwoPagination={true}
                            />
                            {lineItemPermision.isLineItemEditable && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                                    <div className="right-align mt-2 mb-2 mr-2">
                                        <a className="waves-effect btn-small" onClick={this.addTimeNewRow} disabled={interactionMode || disableAddDelete} >{localConstant.commonConstants.ADD}</a>
                                        {/* <a className="waves-effect btn-small" onClick={this.addTimeShowModal} disabled={interactionMode} >{localConstant.commonConstants.ADD}</a> */}
                                        {lineItemPermision.isLineItemDeletable ?
                                            <a className="btn-small ml-2 dangerBtn mr-2"
                                                onClick={(e) => this.deleteLineItemHandler(e, 'gridTimeChild', 'DeleteTimesheetTechnicalSpecialistTime')}
                                                disabled={disableAddDelete || interactionMode} >
                                                {localConstant.commonConstants.DELETE}</a>
                                            : null
                                        }
                                    </div>
                                    : null
                            }
                        </TabPanel>
                        <TabPanel title={localConstant.timesheet.EXPENSES}>
                            <ReactGrid
                                gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                                gridRowData={this.state.timesheetTechSpecExpenses}
                                gridColData={this.headerData.expensesDetails}
                                onRef={ref => { this.gridExpenseChild = ref; }}
                                onCellFocused={this.onCellFocused}
                                gridTwoPagination={true}
                            />
                            { lineItemPermision.isLineItemEditable  && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                                    <div className="right-align mt-2 mb-2 mr-2">
                                        <a className="waves-effect btn-small" onClick={this.addExpenseNewRow} disabled={interactionMode || disableAddDelete} >{localConstant.commonConstants.ADD}</a>
                                        {/* <a className="waves-effect btn-small" onClick={this.addExpensesShowModal} disabled={interactionMode} >{localConstant.commonConstants.ADD}</a> */}
                                        {lineItemPermision.isLineItemDeletable ?
                                            <a className="btn-small ml-2 dangerBtn mr-2"
                                                onClick={(e) => this.deleteLineItemHandler(e, 'gridExpenseChild', 'DeleteTimesheetTechnicalSpecialistExpense')}
                                                disabled={disableAddDelete || interactionMode} >
                                                {localConstant.commonConstants.DELETE}</a>
                                            : null
                                        }
                                    </div> : null
                            }
                        </TabPanel>
                        <TabPanel title={localConstant.timesheet.TRAVEL}>
                            <ReactGrid
                                gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                                gridRowData={this.state.timesheetTechSpecTravels}
                                gridColData={this.headerData.travelDetails}
                                onRef={ref => { this.gridTravelChild = ref; }}
                                onCellFocused={this.onCellFocused}
                                gridTwoPagination={true}
                            />
                            { lineItemPermision.isLineItemEditable  && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                                    <div className="right-align mt-2 mb-2 mr-2">
                                        <a className="waves-effect btn-small" onClick={this.addTravelNewRow} disabled={interactionMode || disableAddDelete} >{localConstant.commonConstants.ADD}</a>
                                        {/* <a className="waves-effect btn-small" onClick={this.addTravelShowModal} disabled={interactionMode} >{localConstant.commonConstants.ADD}</a> */}
                                        { lineItemPermision.isLineItemDeletable ?
                                            <a className="btn-small ml-2 dangerBtn mr-2"
                                                onClick={(e) => this.deleteLineItemHandler(e, 'gridTravelChild', 'DeleteTimesheetTechnicalSpecialistTravel')}
                                                disabled={disableAddDelete || interactionMode} >
                                                {localConstant.commonConstants.DELETE}</a>
                                            : null
                                        }
                                    </div> : null
                            }
                        </TabPanel>
                        {  isShowConsumableTab ?
                                <TabPanel title={localConstant.timesheet.CONSUMABLE_EQUIPMENT}>
                                    <ReactGrid
                                        gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                                        gridRowData={this.state.timesheetTechSpecConsumables}
                                        gridColData={this.headerData.consumableDetails}
                                        onRef={ref => { this.gridConsumableChild = ref; }}
                                        onCellFocused={this.onCellFocused}
                                        gridTwoPagination={true}
                                    />
                                    {
                                        lineItemPermision.isLineItemEditable  && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                                            <div className="right-align mt-2 mb-2 mr-2">
                                                <a className="waves-effect btn-small" onClick={this.addConsumableNewRow} disabled={interactionMode || disableAddDelete} >{localConstant.commonConstants.ADD}</a>
                                                {/* <a className="waves-effect btn-small" onClick={this.addConsumableShowModal} disabled={interactionMode} >{localConstant.commonConstants.ADD}</a> */}
                                                {lineItemPermision.isLineItemDeletable ?
                                                    <a className="btn-small ml-2 dangerBtn mr-2"
                                                        onClick={(e) => this.deleteLineItemHandler(e, 'gridConsumableChild', 'DeleteTimesheetTechnicalSpecialistConsumable')}
                                                        disabled={disableAddDelete || interactionMode} >
                                                        {localConstant.commonConstants.DELETE}</a>
                                                    : null
                                                }
                                            </div> : null
                                    }
                                </TabPanel>
                                : null
                        }
                    </Tabs>
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default TechnicalSpecialistAccounts;
