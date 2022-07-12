import React, { Component,Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, 
         formInputChangeHandler,
         bindAction,
         bindActionWithChildLevel,
         isEmptyReturnDefault, 
         isEmptyOrUndefine,isEmpty, numberFormat } from '../../../../utils/commonUtils';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './techSpecHeader';
import Modal from '../../../../common/baseComponents/modal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { Tab, Tabs,TabList,TabPanel } from 'react-tabs';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { required,requiredNumeric } from '../../../../utils/validator';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { sortCurrency } from '../../../../utils/currencyUtil';
import { GetScheduleDefaultCurrency } from '../../../../selectors/timesheetSelector';
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
                label={localConstant.visit.DATE}
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
                divClassName='col'
                label={localConstant.visit.TYPE}
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
                label={localConstant.visit.FULL_DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={50}
                name='expenseDescription'
                value={props.editedTimeData && props.editedTimeData.expenseDescription} 
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
            
            <h6 className="col s12 p-0 mt-2">{localConstant.visit.CHARGE}</h6> 
              
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.WORK}
                dataType='number'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeWorkUnit'
                min="0"
                onValueInput={props.decimalWithTwoLimitFormat}
                onValueBlur={props.checkTwoDigitNumber}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeWorkUnit}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />
            
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.TRAVEL}
                dataType='number'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeTravelUnit'
                min="0"
                onValueInput={props.decimalWithTwoLimitFormat}
                onValueBlur={props.checkTwoDigitNumber}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeTravelUnit}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />

            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.WAIT}
                dataType='number'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeWaitUnit'
                min="0"
                onValueInput={props.decimalWithTwoLimitFormat}
                onValueBlur={props.checkTwoDigitNumber}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeWaitUnit}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.REPORT}
                dataType='number'
                colSize='s4'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeReportUnit'
                min="0"
                onValueInput={props.decimalWithTwoLimitFormat}
                onValueBlur={props.checkTwoDigitNumber}
                defaultValue={props.editedTimeData && props.editedTimeData.chargeReportUnit}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />            

            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.UNIT}                
                dataType='decimal'
                valueType='value'
                colSize='s4 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeTotalUnit'
                min="0"
                isLimitType={2}                
                value={props.chargeTotalUnit}
                readOnly = {(!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />
            
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.RATE}
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
               readOnly = {(!props.lineItemPermision.isLineItemEditable
                || !props.lineItemPermision.isLineItemEditableOnChargeSide)} 
            />        

            <CustomInput
                hasLabel={true}             
                label={localConstant.visit.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s4'
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
                label={localConstant.visit.DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={100}
                name='chargeRateDescription'
                value={props.editedTimeData && props.editedTimeData.chargeRateDescription}
                readOnly = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}
            />
              
            <h6 className="col s12 p-0 mt-2">{localConstant.visit.PAY}</h6>

            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.UNIT}
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
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide}
            />
                             
            <CustomInput
                hasLabel={true}
                type="text"
                label={localConstant.visit.RATE}
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
                label={localConstant.visit.CURRENCY}
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
                label={localConstant.visit.DESCRIPTION}                      
                colSize='s12'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='payRateDescription'
                value={props.editedTimeData && props.editedTimeData.payRateDescription}                
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
                label={localConstant.visit.DATE}
                type='date'
                //inputClass="customInputs"
                //maxLength={60}
                //onValueChange={props.onChangeHandler}                                
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />

            <CustomInput
                hasLabel={true}             
                label={localConstant.visit.TYPE}
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
                label={localConstant.visit.CURRENCY}
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
                switchLabel={localConstant.visit.CHEXP}
                isSwitchLabel={true}
                switchName="isContractHolderExpense"
                id="isContractHolderExpense"
                colSize='s3'
                checkedStatus={props.editedExpenseData && props.editedExpenseData.isContractHolderExpense}
                onChangeToggle={props.onChangeHandler}
                switchKey={props.editedExpenseData && props.editedExpenseData.isContractHolderExpense}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />

            <h6 className="col s12 p-0 mt2">{localConstant.visit.CHARGE}</h6>

            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.UNIT}
                dataType='decimal'
                colSize='s3'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeUnit'
                min="0"                
                defaultValue={(props.editedExpenseData && props.editedExpenseData.chargeUnit) ?
                    props.editedExpenseData.chargeUnit:'0'}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
                               
            <CustomInput
                hasLabel={true}
                type="text"
                
                label={localConstant.visit.RATE}
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
                label={localConstant.visit.EXCHANGE}                  
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={200}
                name='chargeRateExchange'
                // defaultValue={props.editedExpenseData && props.editedExpenseData.chargeRateExchange}
                value={(props.chargeRateExchange)?props.chargeRateExchange:''}  
                readOnly = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}                           
            />

            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.visit.CURRENCY}
                type='select'
                name='chargeRateCurrency'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.currencyMasterData}
                optionName='code'
                optionValue='code'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedExpenseData && props.editedExpenseData.chargeRateCurrency}
                readOnly = {props.isNewRecord ? !props.lineItemPermision.isLineItemEditable:
                    (!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide)}            
            />
              
            <h6 className="col s12 p-0 mt-1">{localConstant.visit.PAY}</h6>
            
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s3'
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
                label={localConstant.visit.NETT_UNIT}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"        
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}        
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
                label={localConstant.visit.TAX}
                dataType='decimal'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRateTax'
                min="0"                
                defaultValue={(props.editedExpenseData && props.editedExpenseData.payRateTax)
                    ?props.editedExpenseData.payRateTax : '0.00'}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
            
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.visit.GROSS}
                dataValType='valueText'
                colSize='s3 mb-2'
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
                label={localConstant.visit.EXCHANGE}                  
                colSize='s3'
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
                label={localConstant.visit.CURRENCY}
                type='select'
                name='payRateCurrency'
                colSize='s3 pl-0'
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
                label={localConstant.visit.DATE}
                type='date'
                //inputClass="customInputs"
                //maxLength={60}
                //onValueChange={props.onChangeHandler} 
                //defaultValue={props.editedTravelData && props.editedTravelData.expenseDate}
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            
            <CustomInput
                hasLabel={true}
                type="textarea"
                divClassName='col pl-0'
                label={localConstant.visit.DESCRIPTION}                      
                colSize='s8'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={500}
                name='expenseDescription'    
                value={props.editedTravelData && props.editedTravelData.expenseDescription}
                disabled={!props.lineItemPermision.isLineItemEditable}                   
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

            <h6 className="col s12 p-0 mt-2">{localConstant.visit.CHARGE}</h6>

            <CustomInput
                hasLabel={true}
              
                label={localConstant.visit.CHARGE_TYPE}
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
                label={localConstant.visit.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s2 pl-0 pr-1'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeTotalUnit'
                min="0"
                isLimitType={2}
                // defaultValue={(props.editedTravelData && props.editedTravelData.chargeTotalUnit) 
                //     ? props.editedTravelData.chargeTotalUnit : '0'}
                labelClass="mandate"
                value={props.chargeTotalUnit}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
            <button className="btn btn-small col mt-4x" onClick={props.swapDownHandler}><i class="zmdi zmdi-long-arrow-down"></i></button>
                                
            <CustomInput
                hasLabel={true}
                type="text"
                divClassName='col'
                label={localConstant.visit.RATE}
                dataType='number'
                colSize='s3 pr-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                //value={props.chargeRate}
                defaultValue={(props.editedTravelData && props.editedTravelData.chargeRate) 
                    ? props.editedTravelData.chargeRate : '0.0000'}
                labelClass="mandate"
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />            

            <CustomInput
                hasLabel={true}                
                label={localConstant.visit.CURRENCY}
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
              
            <h6 className="col s12 p-0 mt-1">{localConstant.visit.PAY}</h6>

            <CustomInput
                hasLabel={true}               
                label={localConstant.visit.PAY_TYPE}
                type='select'
                name='payExpenseType'
                colSize='s3 '
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedTravelData && props.editedTravelData.payExpenseType}
                labelClass="mandate"
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />

            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.visit.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s2 pr-1 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payUnit'
                min="0"
                isLimitType={2}                
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
                label={localConstant.visit.RATE}
                dataType='number'
                colSize='s3 pr-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"  
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                 defaultValue={(props.editedTravelData && props.editedTravelData.payRate) 
                    ? props.editedTravelData.payRate : '0.0000'}
                //value={props.payRate}
                readOnly = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />                  

            <CustomInput
                hasLabel={true}                
                label={localConstant.visit.CURRENCY}
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
                label={localConstant.visit.DATE}
                type='date'
                selectedDate={props.expenseDate ? dateUtil.defaultMoment(props.expenseDate):moment()}
                onDateChange={props.expenseDateChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                disabled={!props.lineItemPermision.isLineItemEditable}
            />
            
            <CustomInput
                hasLabel={true}
                divClassName='col'
                type="textarea"
                label={localConstant.visit.FULL_DESCRIPTION}                      
                colSize='s12 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={50}
                name='chargeDescription'
                value={props.editedConsumableData && props.editedConsumableData.chargeDescription} 
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
            <h6 className="col s12 p-0 mt-2 ">{localConstant.visit.CHARGE}</h6>

            <CustomInput
                hasLabel={true}            
                label={localConstant.visit.CHARGE_TYPE}
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
                labelClass="mandate"
            /> 

            <CustomInput
                hasLabel={true}
                type="text"              
                label={localConstant.visit.UNIT}
                dataType='decimal'
                valueType='value'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeTotalUnit'
                min="0"
                isLimitType={2}
                // defaultValue={(props.editedConsumableData && props.editedConsumableData.chargeTotalUnit) 
                //     ? props.editedConsumableData.chargeTotalUnit : '0'}
                value={props.chargeTotalUnit}
                labelClass="mandate"
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />
                                
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.visit.RATE}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='chargeRate'
                min="0"
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}
                defaultValue={(props.editedConsumableData && props.editedConsumableData.chargeRate)
                    ?props.editedConsumableData.chargeRate : '0.0000'}
               labelClass="mandate"
               disabled = {!props.lineItemPermision.isLineItemEditable
                || !props.lineItemPermision.isLineItemEditableOnChargeSide }
            />       

            <CustomInput
                hasLabel={true}
                label={localConstant.visit.CURRENCY}
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
              
            <h6 className="col s12 p-0 mt-2">{localConstant.visit.PAY}</h6>

            <CustomInput
                hasLabel={true}
                divClassName='col'
                type="textarea"
                label={localConstant.visit.DESCRIPTION}                      
                colSize='s12 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={100}
                name='PayRateDescription'
                value={props.editedConsumableData && props.editedConsumableData.PayRateDescription}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }                 
            />
           
            <CustomInput
                hasLabel={true}              
                label={localConstant.visit.PAY_TYPE}
                type='select'
                name='payExpenseType'
                colSize='s3 pl-0'
                className="browser-default"                   
                optionsList={props.chargeType}
                optionName='name'
                optionValue='name'
                onSelectChange={props.onChangeHandler}
                defaultValue={props.editedConsumableData && props.editedConsumableData.payExpenseType}
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
                labelClass="mandate"
            />
                    
            <CustomInput
                hasLabel={true}
                type="text"
                label={localConstant.visit.UNIT}
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
                labelClass="mandate"
                disabled = {!props.lineItemPermision.isLineItemEditable
                    || !props.lineItemPermision.isLineItemEditableOnPaySide }
            />
                              
            <CustomInput
                hasLabel={true}
                type="text"               
                label={localConstant.visit.RATE}
                dataType='number'
                colSize='s3 pl-0'
                inputClass="customInputs"
                onValueChange={props.onChangeHandler}
                maxLength={20}
                name='payRate'
                min="0"    
                onValueInput={props.decimalWithLimtFormat}
                onValueBlur={props.checkNumber}            
                defaultValue={(props.editedConsumableData && props.editedConsumableData.payRate) 
                    ? props.editedConsumableData.payRate : '0.0000'}
                disabled = {!props.lineItemPermision.isLineItemEditable
                        || !props.lineItemPermision.isLineItemEditableOnPaySide }
                labelClass="mandate"
            />      

            <CustomInput
                hasLabel={true}           
                label={localConstant.visit.CURRENCY}
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
        </Fragment>
    );   
};

const ScheduleRate = (props) => {
    //const chargeSchedules = isEmptyReturnDefault(props.defaultTechSpecRateSchedules.chargeSchedules);
    //const paySchedules = isEmptyReturnDefault(props.defaultTechSpecRateSchedules.paySchedules);    
    const chargeSchedules = [];   
    const paySchedules = [];   
    const pin = props.pin;

    const payRateGridData = isEmptyReturnDefault(props.defaultTechSpecRateSchedules);        
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
      
    const chargeRateGridData = isEmptyReturnDefault(props.defaultTechSpecRateSchedules);    
    if(!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {        
        chargeRateGridData.chargeSchedules.forEach(row => {
            if(row.epin === pin) 
            {
                row.chargeScheduleRates = row.chargeScheduleRates.filter(x =>  x.isActive);
                chargeSchedules.push(row);
            }
        });
    }
    // this.editedRowData = {};
    
    return (
        <Fragment>
             <CardPanel title="Charge Schedule Rates" titleClass="pl-0 bold m-0" className="white lighten-4 black-text" colSize="s12 mb-2">
               <ReactGrid 
                    gridRowData={chargeSchedules} 
                    gridColData={props.gridHeader.chargeScheduleRates} 
                    isGrouping={true} 
                    groupName='contractScheduleName' 
                    dataName='chargeScheduleRates'/>
             </CardPanel>
             <CardPanel title="Pay Schedule Rates" titleClass="pl-0 bold m-0" className="white lighten-4 black-text" colSize="s12 mb-2">
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
                chargeTotalUnit = {props.chargeTotalUnit}
                payUnit = {props.payUnit} 
                lineItemPermision = {props.lineItemPermision}   
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}
                checkTwoDigitNumber={props.checkTwoDigitNumber}
                decimalWithTwoLimitFormat={props.decimalWithTwoLimitFormat}           
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
                payUnit={props.payUnit}
                chargeRateExchange = {props.chargeRateExchange}
                payRateExchange =  {props.payRateExchange}
                lineItemPermision = {props.lineItemPermision}
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}     
                checkTwoDigitNumber={props.checkTwoDigitNumber}
                decimalWithTwoLimitFormat={props.decimalWithTwoLimitFormat}
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
                swapUpHandler={props.swapUpHandler}
                swapDownHandler={props.swapDownHandler}                
                chargeTotalUnit={props.chargeTotalUnit}
                payUnit={props.payUnit}
                lineItemPermision = {props.lineItemPermision}
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}   
                checkTwoDigitNumber={props.checkTwoDigitNumber}
                decimalWithTwoLimitFormat={props.decimalWithTwoLimitFormat}  
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
                chargeTotalUnit={props.chargeTotalUnit}
                payUnit={props.payUnit}
                lineItemPermision = {props.lineItemPermision}
                checkNumber={props.checkNumber}
                decimalWithLimtFormat={props.decimalWithLimtFormat}   
                checkTwoDigitNumber={props.checkTwoDigitNumber}
                decimalWithTwoLimitFormat={props.decimalWithTwoLimitFormat}  
            /> 
            </div>
        </Fragment>
    );
};

class TechnicalSpecialistAccounts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            scheduleRateShowModal: false,
            payRateShowModal: false,
            timeModel: false,
            expensesShowModal: false,
            travelShowModal: false,
            consumableShowModal:false,
            expenseDate: '',
            isNewRecord: false,
            visitTechSpecTimes: [],
            visitTechSpecTravels: [],
            visitTechSpecExpenses: [],
            visitTechSpecConsumables: [],
            selectedPayRateGridData: [],
            isAddUnlinkedAssignmentExpenses:false,
            chargeRate:0,
            payRate:0,
            isShowAdditionalExpenses: false,
            isInvoiceNotesPopup: false,
            selectedTabIndex: 0,
            isPayRate: false,
            isReloadGrid: false,
            moduleType: "R"
        };
        this.updatedData = {};    
        
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);

        this.props.callBackFuncs.onVisitCancel =()=>{
            this.clearLineItemsOnCancel();
        };

        this.props.callBackFuncs.onCancel =()=>{
            this.clearLineItemsInState();
        };

        this.props.callBackFuncs.reloadLineItemsOnSaveValidation =()=>{
            if(this.gridChild) {
                const selectedData = this.gridChild.getSelectedRows();                
                if(selectedData !== undefined) {                  
                    this.bindTechSpecLineItems(selectedData[0].pin);
                }
            }
        };
        
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
            visitTechSpecTimes: [],
            visitTechSpecTravels: [],
            visitTechSpecExpenses: [],
            visitTechSpecConsumables: [],
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
                const {
                    isInterCompanyAssignment,
                    isOperatingCompany
                } = this.props;
                const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);
                if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTimes)) {
                    this.props.VisitTechnicalSpecialistTimes.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            row.modifyPayUnitPayRate = (this.props.modifyPayUnitPayRate ? false : true);
                            row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
                            techSpecTime.push(row);
                        }
                    });                    
                }      
                this.setState((state) => {
                    return {
                        visitTechSpecTimes: techSpecTime,
                    };
                });  
                if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTravels)) {
                    this.props.VisitTechnicalSpecialistTravels.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            if(row.chargeRate && row.chargeRate != undefined){
                                const rowChargeRate = parseFloat(row.chargeRate);
                                row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                            }
                            techSpecTravel.push(row);
                        }
                    });                    
                }
                this.setState((state) => {
                    return {
                        visitTechSpecTravels: techSpecTravel,
                    };
                });
                if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistExpenses)) {
                    this.props.VisitTechnicalSpecialistExpenses.forEach(row => {
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
                        visitTechSpecExpenses: techSpecExpense,
                    };
                });
                if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistConsumables)) {
                    this.props.VisitTechnicalSpecialistConsumables.forEach(row => {
                        if(pin == row.pin && row.recordStatus !== 'N') {
                            row.chargeRate = row.chargeRate;
                            row.payRate = FormatFourDecimal(row.payRate);
                            row.lineItemPermision = lineItemPermision;
                            if(row.chargeRate && row.chargeRate != undefined){
                                const rowChargeRate = parseFloat(row.chargeRate);
                                row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                            }
                            techSpecConsumable.push(row);
                        }
                    });                    
                }
                this.setState((state) => {
                    return {
                        visitTechSpecConsumables: techSpecConsumable,
                    };
                });
            } 
        }               
    }

    enableEditColumn = (e) => {
        return this.props.isTBAVisitStatus ? (this.props.isTBAVisitStatus === true ? true : false) : false;
    }

    componentDidMount() {        
        if(this.isPageRefresh()) {
            this.props.actions.FetchChargeTypes();            
        }
        if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
            this.props.actions.FetchCompanyExpectedMargin();          
            this.fetchExchangeRatesForlineItmes();
        }
        this.currencyData = sortCurrency(this.props.currencyMasterData);
    };

    fetchExchangeRatesForlineItmes = async () => {
        const calculateExchangeRates = [];
        this.props.VisitTechnicalSpecialistExpenses && this.props.VisitTechnicalSpecialistExpenses.forEach(row => {            
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
            const resultRates = await this.props.actions.FetchCurrencyExchangeRate(calculateExchangeRates, this.props.visitInfo.visitContractNumber);  
            if(resultRates && resultRates.length > 0)
            {
                await this.props.actions.UpdateVisitExchangeRates(resultRates);
            }
        }        
    }

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "TechnicalSpecialistAccounts") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }

    editRateScheduleRowHandler = (data) => {       
        this.editedRowData = data;            
        this.setState((state) => {
            return {
                scheduleRateShowModal: !state.scheduleRateShowModal,
            };
        });      
    }

    addChargeScheduleModal=(e)=>{
        e.preventDefault();
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
        const payRateSchedule = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);
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
        const payRateGridData = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);        
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
        const chargeRateSchedule = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);
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
        const payRateGridData = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);        
        const selectedPayData = [];
        if(!isEmptyOrUndefine(payRateGridData.chargeSchedules) && !isEmptyOrUndefine(this.editedRowData)) {   
            const expenseDate = moment(this.editedRowData.expenseDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            payRateGridData.chargeSchedules.forEach(row => {
                row.chargeScheduleRates.forEach(rowData => { 
                    if( (isEmptyOrUndefine(rowData.effectiveTo) 
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
                        this.updatedData["payRate"] = row.chargeRate;
                    }
                    this.updatedData["payRateId"] = row.rateId;
                    this.updatedData["payRateCurrency"] = row.currency;
                    this.updatedData["payRateDescription"] = row.description;   
                    this.updatedData["chargeDescription"] = row.description;                   
                }
                if(isEmptyOrUndefine(this.updatedData["recordStatus"])) {
                    this.updatedData["recordStatus"] = "M";
                }
                selectedType = row.type;
            });            
            const focusedCell = this.getFocusedLineItemGrid();
            if(selectedType === 'R') {       
                if(isPrintDescriptionOnInvoice) this.updatedData["isInvoicePrintPayRateDescrition"] = true;
                this.props.actions.UpdateTechnicalSpecialistTime(this.updatedData, this.editedRowData);
                this.updateTimeLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridTimeChild.gridApi.setRowData(this.state.visitTechSpecTimes); 
                }, 0);
                this.gridTimeChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId);                      
            } else if(selectedType === 'T') { 
                if(isPrintDescriptionOnInvoice) this.updatedData["isInvoicePrintExpenseDescription"] = true;
                if(expenseDescription !== '') this.updatedData["expenseDescription"] = expenseDescription;
                this.props.actions.UpdateTechnicalSpecialistTravel(this.updatedData, this.editedRowData);
                this.updateTravelLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridTravelChild.gridApi.setRowData(this.state.visitTechSpecTravels);
                }, 0);
                this.gridTravelChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId);
            } else if(selectedType === 'E') {
                if(expenseDescription !== '') this.updatedData["expenseDescription"] = expenseDescription;
                const data = { payRate:this.updatedData.payRate,
                    payUnit:this.updatedData.payUnit,
                    payRateTax:this.updatedData.payRateTax };  
                //this.updatedData["payRateExchange"] = this.calculateExpenseGross(data);                      
                this.props.actions.UpdateTechnicalSpecialistExpense(this.updatedData, this.editedRowData);
                this.updateExpenseLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridExpenseChild.gridApi.setRowData(this.state.visitTechSpecExpenses);
                }, 0);
                this.gridExpenseChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId);
            } else if(selectedType === 'C' || selectedType === 'Q') {                    
                this.props.actions.UpdateTechnicalSpecialistConsumable(this.updatedData, this.editedRowData);
                this.updateConsumableLineItems(this.updatedData, this.editedRowData);
                setTimeout(() => {
                    this.gridConsumableChild.gridApi.setRowData(this.state.visitTechSpecConsumables);
                }, 0);
                this.gridConsumableChild.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId);
            }            
            this.props.actions.UpdateVisitStatusByLineItems(true);
            //this.props.actions.UpdateTechSpecRateChanged(true);
        } else {
            IntertekToaster("Please select the record", 'warningToast GridValidation');
            isValid = false;
        }
        this.updatedData = {};
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

    addDefaultExchangeRate = async (exchangeRates, lineItemData) =>{
        const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates, this.props.visitInfo.visitContractNumber);
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

    addDefaultCurrency = (pin, lineItemData) => {
        const rateSchedules =this.props.defaultTechSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });
        if(!('chargeRateCurrency' in lineItemData) 
            || !lineItemData.chargeRateCurrency 
            || lineItemData.chargeRateCurrency === "" 
            && !isEmptyOrUndefine(defaultScheduleCurrency.defaultChargeCurrency)){
            this.updatedData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
            lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        }
        if(!('payRateCurrency' in lineItemData)
            || !lineItemData.payRateCurrency 
            || lineItemData.payRateCurrency === ""
            && !isEmptyOrUndefine(defaultScheduleCurrency.defaultPayCurrency)){
            this.updatedData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
            lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        }
    }

    addTimeNewRow=(e)=>{
        e.preventDefault();
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);

        const rateSchedules =this.props.defaultTechSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

        const lineItemData = {};        
        lineItemData["TechSpecTimeId"] = Math.floor(Math.random() * 9999) - 10000;        
        lineItemData["recordStatus"] = 'N';            
        lineItemData["pin"] = pin;
        lineItemData["chargeTotalUnit"] = "0.00";
        lineItemData["chargeRate"] = "0.0000";
        lineItemData["payUnit"] = "0.00";
        lineItemData["payRate"] = "0.0000";
        lineItemData["expenseDate"] = this.props.visitInfo.visitStartDate? this.props.visitInfo.visitStartDate : '';
        lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
        lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
        lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
        lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;    
        lineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
        lineItemData["payRateReadonly"] = this.props.payRateReadonly;
        lineItemData["modifyPayUnitPayRate"] = (this.props.modifyPayUnitPayRate ? false : true);
        lineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        lineItemData["chargeUnitRequired"] = "";
        lineItemData["recordStatus"] = "N";
        lineItemData["lineItemPermision"] = this.isLineItemEditable();

        const techSpecTime = [];
        techSpecTime.push(lineItemData);
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTimes)) {            
            this.props.VisitTechnicalSpecialistTimes.forEach(row => {
                if(row.recordStatus !== 'D' && pin == row.pin) {                    
                    techSpecTime.push(row);
                }
            });            
        }
        this.setState((state) => {
            return {
                visitTechSpecTimes: techSpecTime,
            };
        });
        this.props.actions.AddTechnicalSpecialistTime(lineItemData);     
        
        setTimeout(() => {
            this.gridTimeChild.gridApi.setRowData(this.state.visitTechSpecTimes);
        }, 0);
    }

    addTimeShowModal=(e)=>{        
        e.preventDefault();
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeTotalUnit:0,            
            expenseDate: this.props.visitInfo.visitStartDate
        };        
        this.setState({ expenseDate: this.props.visitInfo.visitStartDate?this.props.visitInfo.visitStartDate:'' });
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isNewRecord: true,
                payUnit: 0,
                chargeTotalUnit: 0
            };
        });
    }

    TimeItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["chargeTotalUnitRequired"] = "";
        row["chargeRateRequired"] = "";
        row["chargeUnitRequired"] = "";
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
            if(!(isEmptyOrUndefine(row.chargeWorkUnit) && isEmptyOrUndefine(row.chargeTravelUnit) && isEmptyOrUndefine(row.chargeWaitUnit)
                && isEmptyOrUndefine(row.chargeReportUnit))) {
                    const chargeWorkUnit = isNaN(parseFloat(row.chargeWorkUnit))?0:parseFloat(row.chargeWorkUnit);
                    const chargeTravelUnit = isNaN(parseFloat(row.chargeTravelUnit))?0:parseFloat(row.chargeTravelUnit);
                    const chargeWaitUnit = isNaN(parseFloat(row.chargeWaitUnit))?0:parseFloat(row.chargeWaitUnit);
                    const chargeReportUnit = isNaN(parseFloat(row.chargeReportUnit))?0:parseFloat(row.chargeReportUnit);
                    const sumValue = FormatTwoDecimal(chargeWorkUnit + chargeTravelUnit + chargeWaitUnit + chargeReportUnit);
                    const chargeUnit = FormatTwoDecimal(isNaN(parseFloat(row.chargeTotalUnit))?0:parseFloat(row.chargeTotalUnit));
                    if(sumValue !== chargeUnit) {
                        row["chargeTotalUnitRequired"] = localConstant.validationMessage.VISIT_CHARGE_UNIT_VALIDATION;
                    }
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
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0 && !requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payRate) && row.payRate > 0 && !requiredNumeric(row.payUnit) && row.payUnit > 0) ? false : true;
            }
        }
        return row;
    }

    submitTimeModal = (e) => {        
        e.preventDefault();
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeRate)){
            IntertekToaster("Charge Rate is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payRate)){
            IntertekToaster("Pay Rate is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
                && !requiredNumeric(lineItemData.chargeTotalUnit) && lineItemData.chargeTotalUnit > 0
                && required(lineItemData.chargeRateCurrency)){
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
             && !requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
            && required(lineItemData.payRateCurrency)){
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
            this.updatedData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
            this.updatedData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            this.updatedData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            this.updatedData["projectNumber"] = this.props.visitInfo.visitProjectNumber;
            this.props.actions.UpdateTechnicalSpecialistTime(this.updatedData, this.editedRowData);
            this.updateTimeLineItems(this.updatedData, this.editedRowData);
            
        } else {            
            lineItemData["TechSpecTimeId"] = Math.floor(Math.random() * 9999) - 10000;        
            lineItemData["recordStatus"] = 'N';            
            lineItemData["pin"] = pin;
            lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;
            lineItemData["chargeRate"] = FormatFourDecimal(lineItemData.chargeRate);
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);
            this.props.actions.AddTechnicalSpecialistTime(lineItemData);            
            this.state.visitTechSpecTimes.push(lineItemData);
        }        
        this.props.actions.UpdateVisitStatusByLineItems(true);                            
        this.gridTimeChild.gridApi.setRowData(this.state.visitTechSpecTimes);
        this.cancelTimeModal();
        this.updatedData = {};        
    }

    updateTimeLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecTime = this.state.visitTechSpecTimes;
        let checkProperty = "visitTechnicalSpecialistAccountTimeId";
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
                visitTechSpecTimes: newState,
            };
        });
    }

    cancelTimeModal=()=>{
        this.editedRowData = {};
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isTimePayUnitEntered: false
            };
        });
    }

    deleteTechSpecTime = () => {
        const selectedRecords = this.gridTimeChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const {                
                visitInfo,
                isOperatingCompany,
                isCoordinatorCompany,
                isInterCompanyAssignment
            } = this.props;
            let hasErrors = false;
            if([ 'O','A','R' ].includes(visitInfo.visitStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' 
                    && (([ 'O','E' ].includes(x.costofSalesStatus) && (x.unPaidStatus === 'Rejected' || x.unPaidStatus === 'On-hold'))
                        || ([ 'S','A' ].includes(x.costofSalesStatus))
                    )).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true, true);
            }
            if(!hasErrors && isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.chargeRate > 0 && x.chargeTotalUnit > 0 && !required(x.chargeRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true);
            } else if(!hasErrors && isInterCompanyAssignment && isCoordinatorCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.payRate > 0 && x.payUnit > 0 && !required(x.payRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(false);
            }
            if(!hasErrors) this.deleteTechSpecTimeLineItem();
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
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

    deleteTechSpecTimeLineItem = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DELETE_VISIT_TECH_SPEC,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.deleteSelectedTechSpecTime,
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

    deleteSelectedTechSpecTime = () => {
        const selectedData = this.gridTimeChild.getSelectedRows();        
        this.deleteVisitTimeRecord(selectedData);
        this.props.actions.DeleteTechnicalSpecialistTime(selectedData);
        this.props.actions.HideModal();
    }

    deleteVisitTimeRecord(selectedData) {        
        if(!isEmptyOrUndefine(this.state.visitTechSpecTimes)) {
            this.setState((state)=>({                
                visitTechSpecTimes: state.visitTechSpecTimes.filter(item =>{
                    return !selectedData.some(ts=>{
                        let checkProperty = "visitTechnicalSpecialistAccountTimeId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecTimeId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                }),
            }),()=>{
                this.gridTimeChild.gridApi.setRowData(this.state.visitTechSpecTimes);
            });
        }
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
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

    addExpenseNewRow=(e)=>{
        e.preventDefault();
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        //const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);

        const rateSchedules =this.props.defaultTechSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

        const lineItemData = {};         
        lineItemData["TechSpecExpenseId"] = Math.floor(Math.random() * 9999) - 10000;        
        lineItemData["recordStatus"] = 'N';
        lineItemData["pin"] = pin;
        lineItemData["chargeUnit"] = "0.00";
        lineItemData["chargeRate"] = "0.0000";
        lineItemData["payUnit"] = "0.00";
        lineItemData["payRate"] = "0.0000";
        lineItemData["payRateTax"] = "0.00";
        lineItemData["expenseDate"] = this.props.visitInfo.visitStartDate? this.props.visitInfo.visitStartDate : '';
        lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
        lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
        lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
        lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;
        lineItemData["chargeRateReadonly"] = this.props.chargeRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
        lineItemData["payRateReadonly"] = this.props.payRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
        lineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        lineItemData["currencyRequired"] = localConstant.validationMessage.CURRENCY_REQUIRED;
        lineItemData["recordStatus"] = "N";
        lineItemData["lineItemPermision"] = this.isLineItemEditable();

        const techSpecExpense = [];
        techSpecExpense.push(lineItemData);
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistExpenses)) {            
            this.props.VisitTechnicalSpecialistExpenses.forEach(row => {
                if(row.recordStatus !== 'D' && pin == row.pin) {                    
                    techSpecExpense.push(row);
                }
            });            
        }
        this.setState((state) => {
            return {
                visitTechSpecExpenses: techSpecExpense,
            };
        });
        this.props.actions.AddTechnicalSpecialistExpense(lineItemData);
        setTimeout(() => {
            this.gridExpenseChild.gridApi.setRowData(this.state.visitTechSpecExpenses);
        }, 0);
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
            row["expenseDateRequired"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
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
        if(dateUtil.isValidDate(row.expenseDate)){                   
            row["expenseDateRequired"] = localConstant.validationMessage.VALID_EXPENSE_DATE;
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
            if(!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0 ) {      
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
            if(!requiredNumeric(row.payRateTax)){
                const TaxMultiply = row.payUnit * row.payRate;            
                if(row.payRateTax > TaxMultiply) {                
                    row["payRateTaxRequired"] = localConstant.validationMessage.PAY_RATE_TAX_UNITS_VALIDATION;
                }            
            }    
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0){            
                if(required(row.payRateCurrency)) row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;                
            }
            if(required(row.currency)) row["currencyRequired"] = localConstant.validationMessage.CURRENCY_REQUIRED;
        }
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }
        return row;
    }

    addExpensesShowModal=(e)=>{
        e.preventDefault();
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeTotalUnit:0,
            expenseDate: this.props.visitInfo.visitStartDate
        };
        this.setState({ expenseDate: this.props.visitInfo.visitStartDate?this.props.visitInfo.visitStartDate:'' });
        this.setState((state) => {
            return {
                expensesShowModal: !state.expensesShowModal,
                isNewRecord: true,
                payUnit: 0,
            };
        });
    }

    submitExpenseModal = async (e) => {        
        e.preventDefault();            
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }  
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isValidDate(lineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeRate)){
            IntertekToaster("Please select the type of rate", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payUnit)){
            IntertekToaster("Pay Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.payRate)){
            IntertekToaster("Pay Nett/Units is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(lineItemData.payRateTax)){
            const TaxMultiply = lineItemData.payUnit * lineItemData.payRate;            
            if(lineItemData.payRateTax > TaxMultiply) {
                IntertekToaster("Tax must be less than Units x Nett/Unit", 'warningToast Tax');
                return false;
            }            
        }

        this.addDefaultCurrency(pin,lineItemData);

        if(!('currency' in lineItemData) 
        || !lineItemData.currency || lineItemData.currency === ""){
            this.updatedData["currency"] = lineItemData.chargeRateCurrency;
            lineItemData["currency"] = lineItemData.chargeRateCurrency;
        }
        if(!requiredNumeric(lineItemData.chargeUnit) && lineItemData.chargeUnit > 0
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.chargeRateCurrency)){
           IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
           return false;
       }
       if(!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
            && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
           && required(lineItemData.payRateCurrency)){
           IntertekToaster("Pay Currency is required", 'warningToast PercentRange');
           return false;
       }

       if(!requiredNumeric(lineItemData.chargeUnit) && lineItemData.chargeUnit > 0
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.currency)){
           IntertekToaster("Currency is required", 'warningToast PercentRange');
           return false;
       }
       
       if(!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
            && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
           && required(lineItemData.currency)){
           IntertekToaster("Currency is required", 'warningToast PercentRange');
           return false;
       }

        if ((isEmptyOrUndefine(lineItemData.chargeRateExchange)
            || isEmptyOrUndefine(lineItemData.payRateExchange))
            && !isEmptyOrUndefine(this.updatedData["currency"])
            && !isEmptyOrUndefine(this.updatedData["chargeRateCurrency"])
            && !isEmptyOrUndefine(this.updatedData["payRateCurrency"])) {
            const exchangeRates = [ {
                currencyFrom: this.updatedData["currency"],
                currencyTo: this.updatedData["chargeRateCurrency"],
                effectiveDate: currentDate
            },
            {
                currencyFrom: this.updatedData["currency"],
                currencyTo: this.updatedData["payRateCurrency"],
                effectiveDate: currentDate
            } ];
            await this.addDefaultExchangeRate(exchangeRates, lineItemData);
        }
        else {
            this.updatedData["chargeRateExchange"] = this.state.chargeRateExchange;
            lineItemData["chargeRateExchange"] = this.updatedData["chargeRateExchange"];
            this.updatedData["payRateExchange"] = this.state.payRateExchange;
            lineItemData["payRateExchange"] = this.updatedData["payRateExchange"];
        }

        if (!this.state.isNewRecord)
        {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }      
            this.updatedData["pin"] = pin;
            this.updatedData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;    
            this.updatedData["assignmentId"] = this.props.visitInfo.visitAssignmentId; 
            this.updatedData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            this.updatedData["projectNumber"] = this.props.visitInfo.visitProjectNumber; 
            this.props.actions.UpdateTechnicalSpecialistExpense(this.updatedData, this.editedRowData);
            this.updateExpenseLineItems(this.updatedData, this.editedRowData);
        } else {
            lineItemData["TechSpecExpenseId"] = Math.floor(Math.random() * 9999) - 10000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;   
            lineItemData["chargeRate"] = FormatFourDecimal(lineItemData.chargeRate);
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);         
            this.props.actions.AddTechnicalSpecialistExpense(lineItemData);
            this.state.visitTechSpecExpenses.push(lineItemData);
        }

        this.props.actions.UpdateVisitStatusByLineItems(true);
        setTimeout(() => {
            this.gridExpenseChild.gridApi.setRowData(this.state.visitTechSpecExpenses);
        }, 0);
        this.cancelExpensesShowModal();
        this.updatedData = {};
    }

    updateExpenseLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecExpense = this.state.visitTechSpecExpenses;
        let checkProperty = "visitTechnicalSpecialistAccountExpenseId";
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
                visitTechSpecExpenses: newState,
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
                payUnit: 0,
                payRate: 0.00,
                payRateTax: 0.00,
                chargeRateExchange:'',
                payRateExchange:'',
                isExpensePayUnitEntered: false
            };
        });                
    }

    deleteTechSpecExpense = () => {
        const selectedRecords = this.gridExpenseChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const {                
                visitInfo,
                isOperatingCompany,
                isCoordinatorCompany,
                isInterCompanyAssignment
            } = this.props;
            let hasErrors = false;      
            if([ 'O','A','R' ].includes(visitInfo.visitStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' 
                    && (([ 'O','E' ].includes(x.costofSalesStatus) && (x.unPaidStatus === 'Rejected' || x.unPaidStatus === 'On-hold'))
                    || ([ 'S','A' ].includes(x.costofSalesStatus))
                    )).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true, true);
            }
            if(!hasErrors && isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.chargeRate > 0 && x.chargeUnit > 0 && !required(x.chargeRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true, false);
            } else if(!hasErrors && isInterCompanyAssignment && isCoordinatorCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.payRate > 0 && x.payUnit > 0 && !required(x.payRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(false, false);
            }
            if(!hasErrors) this.deleteTechSpecExpenseLineItem();
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
        }
    }

    deleteTechSpecExpenseLineItem = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DELETE_VISIT_TECH_SPEC,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.deleteSelectedTechSpecExpense,
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

    deleteSelectedTechSpecExpense = () => {
        const selectedData = this.gridExpenseChild.getSelectedRows();        
        this.deleteVisitExpenseRecord(selectedData);
        this.props.actions.DeleteTechnicalSpecialistExpense(selectedData);
        this.props.actions.HideModal();
    }

    deleteVisitExpenseRecord(selectedData) {        
        if(!isEmptyOrUndefine(this.state.visitTechSpecExpenses)) {
            this.setState((state)=>({                
                visitTechSpecExpenses: state.visitTechSpecExpenses.filter(item =>{
                    return !selectedData.some(ts=>{
                        let checkProperty = "visitTechnicalSpecialistAccountExpenseId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecExpenseId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                }),
            }),()=>{
                this.gridExpenseChild.gridApi.setRowData(this.state.visitTechSpecExpenses);
            });
        }
    }

    addTravelNewRow=(e)=>{
        e.preventDefault();
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);
        
        const rateSchedules =this.props.defaultTechSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

        const defaultTravelType = getTravelDefaultPayType(pin, rateSchedules, this.props.expenseTypes);

        const lineItemData = {}; 
        lineItemData["TechSpecTravelId"] = Math.floor(Math.random() * 9999) - 10000;        
        lineItemData["recordStatus"] = 'N';
        lineItemData["pin"] = pin;
        lineItemData["chargeTotalUnit"] = "0.00";
        lineItemData["chargeRate"] = "0.0000";
        lineItemData["payUnit"] = "0.00";
        lineItemData["payRate"] = "0.0000";
        lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
        lineItemData["expenseDate"] = this.props.visitInfo.visitStartDate? this.props.visitInfo.visitStartDate : '';
        lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
        lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
        lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;
        lineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
        lineItemData["payRateReadonly"] = this.props.payRateReadonly;
        lineItemData["chargeExpenseType"] = defaultTravelType["chargeType"];
        lineItemData["payExpenseType"] = defaultTravelType["payType"];
        lineItemData["recordStatus"] = "N";
        lineItemData["lineItemPermision"] = this.isLineItemEditable();
        
        const techSpecTravel = [];
        techSpecTravel.push(lineItemData);
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTravels)) {            
            this.props.VisitTechnicalSpecialistTravels.forEach(row => {
                if(row.recordStatus !== 'D' && pin == row.pin) {                    
                    techSpecTravel.push(row);
                }
            });            
        }
        this.setState((state) => {
            return {
                visitTechSpecTravels: techSpecTravel,
            };
        });
        this.props.actions.AddTechnicalSpecialistTravel(lineItemData);
        setTimeout(() => {
            this.gridTravelChild.gridApi.setRowData(this.state.visitTechSpecTravels);
        }, 0);
    }

    TravelItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["payExpenseTypeRequired"] = "";
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
            if(requiredNumeric(row.chargeTotalUnit)){            
                row["chargeTotalUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 
                && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0
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
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0
                && !requiredNumeric(row.payRate) && row.payRate > 0
                && required(row.payRateCurrency)){                     
                row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;
            }
        }        
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }
        return row;
    }

    addTravelShowModal=(e)=>{
        e.preventDefault();
        this.editedRowData = {
            payRate:this.state.payRate,
            payUnit:0,
            chargeRate:this.state.chargeRate,
            chargeTotalUnit:0,
            expenseDate: this.props.visitInfo.visitStartDate
        };
        this.setState({ expenseDate: this.props.visitInfo.visitStartDate?this.props.visitInfo.visitStartDate:'' });
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                isNewRecord: true
            };
        });
    }

    submitTravelModal = (e) => {        
        e.preventDefault();        
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isValidDate(lineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Charge Type is required", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.payExpenseType)){
            IntertekToaster("Pay Type is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeTotalUnit)){
            IntertekToaster("Charge Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeRate)){
            IntertekToaster("Charge Rate is required", 'warningToast PercentRange');
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
        if(!requiredNumeric(lineItemData.chargeTotalUnit) && lineItemData.chargeTotalUnit > 0 
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.chargeRateCurrency)){
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
             && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
            && required(lineItemData.payRateCurrency)){
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
            this.updatedData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId; 
            this.updatedData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            this.updatedData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            this.updatedData["projectNumber"] = this.props.visitInfo.visitProjectNumber; 
            // this.updatedData["chargeRate"] = FormatFourDecimal(this.updatedData["chargeRate"]);
            // this.updatedData["payRate"] = FormatFourDecimal(this.updatedData["payRate"]);
            this.props.actions.UpdateTechnicalSpecialistTravel(this.updatedData, this.editedRowData);
            this.updateTravelLineItems(this.updatedData, this.editedRowData);
        } else {
            lineItemData["TechSpecTravelId"] = Math.floor(Math.random() * 9999) - 10000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber; 
            lineItemData["chargeRate"] = FormatFourDecimal(lineItemData.chargeRate);
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);
            this.props.actions.AddTechnicalSpecialistTravel(lineItemData);
            this.state.visitTechSpecTravels.push(lineItemData);
        }

        this.props.actions.UpdateVisitStatusByLineItems(true);
        this.gridTravelChild.gridApi.setRowData(this.state.visitTechSpecTravels);
        this.cancelTravelShowModal();
        this.updatedData = {};
    }

    updateTravelLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecTravel = this.state.visitTechSpecTravels;
        let checkProperty = "visitTechnicalSpecialistAccountTravelId";
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
                visitTechSpecTravels: newState,
            };
        });
    }
    
    cancelTravelShowModal=()=>{        
        this.editedRowData = {};
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                chargeTotalUnit: 0,
                payUnit: 0
            };
        });
    }

    deleteTechSpecTravel = () => {
        const selectedRecords = this.gridTravelChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const {                
                visitInfo,
                isOperatingCompany,
                isCoordinatorCompany,
                isInterCompanyAssignment
            } = this.props;
            let hasErrors = false;
            if([ 'O','A','R' ].includes(visitInfo.visitStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' 
                    && (([ 'O','E' ].includes(x.costofSalesStatus) && (x.unPaidStatus === 'Rejected' || x.unPaidStatus === 'On-hold'))
                    || ([ 'S','A' ].includes(x.costofSalesStatus))
                    )).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true, true);
            }
            if(!hasErrors && isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.chargeRate > 0 && x.chargeTotalUnit > 0 && !required(x.chargeRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true);
            } else if(!hasErrors && isInterCompanyAssignment && isCoordinatorCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.payRate > 0 && x.payUnit > 0 && !required(x.payRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(false);
            }
            if(!hasErrors) this.deleteTechSpecTravelLineItem();
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
        }
    }

    deleteTechSpecTravelLineItem = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DELETE_VISIT_TECH_SPEC,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.deleteSelectedTechSpecTravel,
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

    deleteSelectedTechSpecTravel = () => {
        const selectedData = this.gridTravelChild.getSelectedRows();        
        this.deleteVisitTravelRecord(selectedData);
        this.props.actions.DeleteTechnicalSpecialistTravel(selectedData);
        this.props.actions.HideModal();
    }

    deleteVisitTravelRecord(selectedData) {        
        if(!isEmptyOrUndefine(this.state.visitTechSpecTravels)) {
            this.setState((state)=>({                
                visitTechSpecTravels: state.visitTechSpecTravels.filter(item =>{
                    return !selectedData.some(ts=>{
                        let checkProperty = "visitTechnicalSpecialistAccountTravelId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecTravelId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                }),
            }),()=>{
                this.gridTravelChild.gridApi.setRowData(this.state.visitTechSpecTravels);
            });
        }
    }

    addConsumableShowModal=(e)=>{        
        e.preventDefault();
        this.editedRowData = {
            payRate:0,
            payUnit:0,
            chargeRate:0,
            chargeTotalUnit:0,
            expenseDate: this.props.visitInfo.visitStartDate
        };
        this.setState({ expenseDate: this.props.visitInfo.visitStartDate?this.props.visitInfo.visitStartDate:'' });
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                isNewRecord: true
            };
        });
    }

    addConsumableNewRow=(e)=>{
        e.preventDefault();
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        }
        const {
            isInterCompanyAssignment,
            isOperatingCompany
        } = this.props;
        const isChargeRateReadOnly = (isInterCompanyAssignment && isOperatingCompany ? true : false);
        
        const rateSchedules =this.props.defaultTechSpecRateSchedules;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ techSpecRateSchedules: rateSchedules, pin });

        const lineItemData = {}; 
        lineItemData["TechSpecConsumableId"] = Math.floor(Math.random() * 9999) - 10000;        
        lineItemData["recordStatus"] = 'N';
        lineItemData["pin"] = pin;
        lineItemData["chargeTotalUnit"] = "0.00";
        lineItemData["chargeRate"] = "0.0000";
        lineItemData["payUnit"] = "0.00";
        lineItemData["payRate"] = "0.0000";
        lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
        lineItemData["chargeRateCurrency"] = defaultScheduleCurrency.defaultChargeCurrency;
        lineItemData["payRateCurrency"] = defaultScheduleCurrency.defaultPayCurrency;
        lineItemData["expenseDate"] = this.props.visitInfo.visitStartDate? this.props.visitInfo.visitStartDate : '';
        lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
        lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
        lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber;
        lineItemData["chargeRateReadonly"] = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
        lineItemData["payRateReadonly"] = this.props.payRateReadonly;
        lineItemData["chargeExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        lineItemData["payExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        lineItemData["recordStatus"] = "N";
        lineItemData["lineItemPermision"] = this.isLineItemEditable();        

        const techSpecConsumable = [];
        techSpecConsumable.push(lineItemData);
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistConsumables)) {            
            this.props.VisitTechnicalSpecialistConsumables.forEach(row => {
                if(row.recordStatus !== 'D' && pin == row.pin) {                    
                    techSpecConsumable.push(row);
                }
            });            
        }
        this.setState((state) => {
            return {
                visitTechSpecConsumables: techSpecConsumable,
            };
        });
        this.props.actions.AddTechnicalSpecialistConsumable(lineItemData);
        setTimeout(() => {
            this.gridConsumableChild.gridApi.setRowData(this.state.visitTechSpecConsumables);
        }, 0);
    }

    ConsumableItemValidation(row, e, rowIndex, lineItemPermision) {
        row["expenseDateRequired"] = "";
        row["chargeExpenseTypeRequired"] = "";
        row["payExpenseTypeRequired"] = "";
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
        if(required(row.payExpenseType)){            
            row["payExpenseTypeRequired"] = localConstant.validationMessage.CHARGE_RATE_VALIDATION;
        }
        if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeTotalUnit)){            
                row["chargeTotalUnitRequired"] = localConstant.validationMessage.CHARGE_UNIT_REQUIRED;
            }
            if(requiredNumeric(row.chargeRate)){            
                row["chargeRateRequired"] = localConstant.validationMessage.CHARGE_RATE_REQUIRED;
            }
            if(!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 
                && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0
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
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0
                && !requiredNumeric(row.payRate) && row.payRate > 0
                && required(row.payRateCurrency)){                     
                row["payRateCurrencyRequired"] = localConstant.validationMessage.PAY_CURRENCY_REQUIRED;
            }
        }
        if(row.recordStatus !== "N") {
            if(!lineItemPermision.isLineItemEditableOnChargeSide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 && !requiredNumeric(row.chargeRate) &&  parseFloat(row.chargeRate) > 0) ? false : true;
            } else if(!lineItemPermision.isLineItemEditableOnPaySide) {
                row["isLineItemEditableExpense"] = (!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) ? false : true;
            }
        }
        return row;
    }

    submitConsumableModal = (e) => {        
        e.preventDefault();   
        const selectedData = this.gridChild.getSelectedRows();
        let visitTechnicalSpecialistId = 0;
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                visitTechnicalSpecialistId = row.visitTechnicalSpecialistId;
                pin = row.pin;
            });
        } 
        const lineItemData = Object.assign({},this.editedRowData, this.updatedData);
        if(required(lineItemData.expenseDate)){
            IntertekToaster("Expense Date is required", 'warningToast PercentRange');
            return false;
        }
        if(dateUtil.isValidDate(lineItemData.expenseDate)){
            IntertekToaster("Please enter valid Expense Date", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.chargeExpenseType)){
            IntertekToaster("Charge Type is required", 'warningToast PercentRange');
            return false;
        }
        if(required(lineItemData.payExpenseType)){
            IntertekToaster("Pay Type is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeTotalUnit)){
            IntertekToaster("Charge Unit is required", 'warningToast PercentRange');
            return false;
        }
        if(requiredNumeric(lineItemData.chargeRate)){
            IntertekToaster("Charge Rate is required", 'warningToast PercentRange');
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
        if(!requiredNumeric(lineItemData.chargeTotalUnit) && lineItemData.chargeTotalUnit > 0 
            && !requiredNumeric(lineItemData.chargeRate) && lineItemData.chargeRate > 0
            && required(lineItemData.chargeRateCurrency)){
            IntertekToaster("Charge Currency is required", 'warningToast PercentRange');
            return false;
        }
        if(!requiredNumeric(lineItemData.payUnit) && lineItemData.payUnit > 0
             && !requiredNumeric(lineItemData.payRate) && lineItemData.payRate > 0
            && required(lineItemData.payRateCurrency)){
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
            this.updatedData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId; 
            this.updatedData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            this.updatedData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            this.updatedData["projectNumber"] = this.props.visitInfo.visitProjectNumber; 
            this.props.actions.UpdateTechnicalSpecialistConsumable(this.updatedData, this.editedRowData);
            this.updateConsumableLineItems(this.updatedData, this.editedRowData);
        } else {
            lineItemData["TechSpecConsumableId"] = Math.floor(Math.random() * 9999) - 10000;        
            lineItemData["recordStatus"] = 'N';
            lineItemData["pin"] = pin;
            lineItemData["visitTechnicalSpecialistId"] = visitTechnicalSpecialistId;
            lineItemData["assignmentId"] = this.props.visitInfo.visitAssignmentId;
            lineItemData["contractNumber"] = this.props.visitInfo.visitContractNumber;
            lineItemData["projectNumber"] = this.props.visitInfo.visitProjectNumber; 
            lineItemData["chargeRate"] = FormatFourDecimal(lineItemData.chargeRate);
            lineItemData["payRate"] = FormatFourDecimal(lineItemData.payRate);
            this.props.actions.AddTechnicalSpecialistConsumable(lineItemData);
            this.state.visitTechSpecConsumables.push(lineItemData);
        }

        this.props.actions.UpdateVisitStatusByLineItems(true);
        this.gridConsumableChild.gridApi.setRowData(this.state.visitTechSpecConsumables);
        this.cancelConsumableShowModal();
        this.updatedData = {};
    }

    updateConsumableLineItems(editedRowData, updatedData){        
        const editedRow = Object.assign({},updatedData, editedRowData );
        const techSpecConsumable = this.state.visitTechSpecConsumables;
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
                visitTechSpecConsumables: newState,
            };
        });
    }
    
    cancelConsumableShowModal=()=>{        
        this.editedRowData = {};
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                chargeTotalUnit: 0,
                payUnit: 0,
                isConsumablePayUnitEntered: false
            };
        });
    }    

    deleteTechSpecConsumable = () => {
        const selectedRecords = this.gridConsumableChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const {                
                visitInfo,
                isOperatingCompany,
                isCoordinatorCompany,
                isInterCompanyAssignment
            } = this.props;
            let hasErrors = false;
            if([ 'O','A','R' ].includes(visitInfo.visitStatus)) {
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' 
                    && (([ 'O','E' ].includes(x.costofSalesStatus) && (x.unPaidStatus === 'Rejected' || x.unPaidStatus === 'On-hold'))
                    || ([ 'S','A' ].includes(x.costofSalesStatus))
                    )).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true, true);
            }
            if(!hasErrors && isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.chargeRate > 0 && x.chargeTotalUnit > 0 && !required(x.chargeRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(true);
            } else if(!hasErrors && isInterCompanyAssignment && isCoordinatorCompany && [ 'O','A' ].includes(visitInfo.visitStatus)) {                              
                hasErrors = selectedRecords.filter(x => x.recordStatus !== 'D' && x.recordStatus !== 'N'
                    && x.payRate > 0 && x.payUnit > 0 && !required(x.payRateCurrency)).length > 0;                
                if(hasErrors) this.deleteTechSpecLineItemError(false);
            }
            if(!hasErrors) this.deleteTechSpecConsumableLineItem();
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
        }
    }

    deleteTechSpecConsumableLineItem = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DELETE_VISIT_TECH_SPEC,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.deleteSelectedTechSpecConsumable,
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

    deleteSelectedTechSpecConsumable = () => {
        const selectedData = this.gridConsumableChild.getSelectedRows();        
        this.deleteVisitConsumableRecord(selectedData);
        this.props.actions.DeleteTechnicalSpecialistConsumable(selectedData);
        this.props.actions.HideModal();
    }

    deleteVisitConsumableRecord(selectedData) {        
        if(!isEmptyOrUndefine(this.state.visitTechSpecConsumables)) {
            this.setState((state)=>({                
                visitTechSpecConsumables: state.visitTechSpecConsumables.filter(item =>{
                    return !selectedData.some(ts=>{
                        let checkProperty = "visitTechnicalSpecialistAccountConsumableId";
                        if (item.recordStatus === 'N') {
                            checkProperty = "TechSpecConsumableId";
                        }
                        return ts[checkProperty] === item[checkProperty];
                    });
                }),
            }),()=>{
                this.gridConsumableChild.gridApi.setRowData(this.state.visitTechSpecConsumables);
            });
        }
    }

    editTimeRowHandler=(data)=>{
        this.editedRowData = data;
        this.setGridDate();        
        this.setState((state) => {
            return {
                timeShowModal: !state.timeShowModal,
                isNewRecord: false,
                chargeTotalUnit: data.chargeTotalUnit,
                payUnit: data.payUnit
            };
        });
    }

    editExpenseRowHandler=(data)=>{
        this.editedRowData = data;
        this.setGridDate();
        const grossValue =  this.calculateExpenseGross(data);
        this.setState((state) => {
            return {
                expensesShowModal: !state.expensesShowModal,
                isNewRecord: false,
                grossValue:grossValue,
                payUnit: data.payUnit,
                chargeRateExchange:data.chargeRateExchange,
                payRateExchange:data.payRateExchange 
            };
        });        
    }

    editTravelRowHandler=(data)=>{
        this.editedRowData = data;
        this.setGridDate();
        this.setState((state) => {
            return {
                travelShowModal: !state.travelShowModal,
                isNewRecord: false,
                payUnit: data.payUnit,
                chargeTotalUnit: data.chargeTotalUnit
            };
        });        
    }

    editConsumableRowHandler=(data)=>{
        this.editedRowData = data;
        this.setGridDate();
        this.setState((state) => {
            return {
                consumableShowModal: !state.consumableShowModal,
                isNewRecord: false,
                payUnit: data.payUnit,
                chargeTotalUnit: data.chargeTotalUnit
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
            this.setState({ chargeTotalUnit:payUnit,payUnit:payUnit });
            this.updatedData["chargeTotalUnit"] = payUnit; 
        } else {
            if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                const payUnit = this.state.payUnit;
                this.setState({ chargeTotalUnit:payUnit,payUnit:payUnit });
                this.updatedData["chargeTotalUnit"] = payUnit; 
            } else {
                const payUnit = this.state.payUnit;
                this.setState({ payUnit:payUnit });
                if(editedRow.payExpenseType === 'Miles') {
                    const changedChargeTotalUnit = this.milesToKiloMeters(payUnit);
                    this.setState({ chargeTotalUnit:changedChargeTotalUnit });
                    this.updatedData["chargeTotalUnit"] = changedChargeTotalUnit; 
                } else {
                    const changedChargeTotalUnit = this.kiloMetersToMiles(payUnit);
                    this.setState({ chargeTotalUnit:changedChargeTotalUnit });
                    this.updatedData["chargeTotalUnit"] = changedChargeTotalUnit; 
                }                
            }
        }
    }

    onswapDownHandler=(e)=>{
        e.preventDefault();
        const editedRow = Object.assign({},this.editedRowData, this.updatedData);        
        if(isEmpty(editedRow.chargeExpenseType) || isEmpty(editedRow.payExpenseType)) {
            const chargeTotalUnit = this.state.chargeTotalUnit;
            this.setState({ chargeTotalUnit:chargeTotalUnit,payUnit:chargeTotalUnit });      
            this.updatedData["payUnit"] = chargeTotalUnit;      
        } else {
            if(editedRow.chargeExpenseType === editedRow.payExpenseType) {
                const chargeTotalUnit = this.state.chargeTotalUnit;
                this.setState({ chargeTotalUnit:chargeTotalUnit,payUnit:chargeTotalUnit }); 
                this.updatedData["payUnit"] = chargeTotalUnit;      
            } else {
                const chargeTotalUnit = this.state.chargeTotalUnit;
                this.setState({ chargeTotalUnit:chargeTotalUnit });
                if(editedRow.chargeExpenseType === 'Miles') {
                    const changedPayUnit = this.milesToKiloMeters(chargeTotalUnit);
                    this.setState({ payUnit:changedPayUnit });
                    this.updatedData["payUnit"] = changedPayUnit;
                } else {
                    const changedPayUnit = this.kiloMetersToMiles(chargeTotalUnit);
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

    onChangeHandler = (e) => {                
        const inputvalue = formInputChangeHandler(e);        
        this.updatedData[inputvalue.name] = inputvalue.value;        
        // if(inputvalue.name === 'chargeRate' || inputvalue.name === 'payRate') {            
        //     this.props.actions.UpdateTechSpecRateChanged(true);
        // }
        if(this.state.timeShowModal ) {
            this.TimeUnitCalculation(inputvalue);            
        }      

        if(this.state.expensesShowModal) {            
            if (inputvalue.name ==='payRate' || inputvalue.name === 'payUnit' 
                || inputvalue.name === 'payRateTax')  {
                const editedRow = Object.assign({},this.editedRowData, this.updatedData);
                const data = { payRate:editedRow.payRate,
                    payUnit:editedRow.payUnit,
                    payRateTax:editedRow.payRateTax };
                const grossValue = this.calculateExpenseGross(data);            
                this.setState((state) => {
                    return {
                        grossValue:grossValue
                    };
                });
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
        
        if(this.state.travelShowModal) {
            if(inputvalue.name === 'chargeTotalUnit') {
                this.setState((state) => {
                    return {
                        chargeTotalUnit:inputvalue.value
                    };
                });
            }
            if(inputvalue.name === 'payUnit') {
                this.setState((state) => {
                    return {
                        payUnit:inputvalue.value
                    };
                });
            }
        }

        if(this.state.consumableShowModal) {
            if(inputvalue.name === 'chargeTotalUnit') {
                this.setState((state) => {
                    return {
                        chargeTotalUnit:inputvalue.value
                    };
                });

                if(this.state.isConsumablePayUnitEntered === undefined || this.state.isConsumablePayUnitEntered === false) {
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
            const res1 = await this.props.actions.FetchCurrencyExchangeRate([ obj ], this.props.visitInfo.visitContractNumber);
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
            const res = await this.props.actions.FetchCurrencyExchangeRate([ obj ], this.props.visitInfo.visitContractNumber);
            if (res && res.length > 0) {
                this.setState(() => {
                    return {
                        payRateExchange: res[0] ? this.formatExchangeRate(res[0].rate) : null
                    };
                });
            }
        }
    }

    TimeUnitCalculation(inputvalue) {
        const editedRow = Object.assign({},this.editedRowData, this.updatedData);
        if(inputvalue.name === 'payUnit') {
            this.setState((state) => {
                return {
                    payUnit: inputvalue.value,
                    isTimePayUnitEntered: true
                };
            });
        }

        if(inputvalue.name === 'chargeTotalUnit') {
            this.setState((state) => {
                return {
                    chargeTotalUnit:inputvalue.value
                };
            });                
            if(this.state.isTimePayUnitEntered === undefined || this.state.isTimePayUnitEntered === false) {
                this.setState((state) => {
                    return {                            
                        payUnit: inputvalue.value
                    };
                });
                this.updatedData["payUnit"] = inputvalue.value;
            }
        }         

        if(inputvalue.name === "chargeWorkUnit" || inputvalue.name === "chargeTravelUnit" 
            || inputvalue.name === "chargeWaitUnit" || inputvalue.name === "chargeReportUnit") {
                const chargeWorkUnit = isNaN(parseFloat(editedRow["chargeWorkUnit"]))?0.00:parseFloat(editedRow["chargeWorkUnit"]);
                const chargeTravelUnit = isNaN(parseFloat(editedRow["chargeTravelUnit"]))?0.00:parseFloat(editedRow["chargeTravelUnit"]);
                const chargeWaitUnit = isNaN(parseFloat(editedRow["chargeWaitUnit"]))?0.00:parseFloat(editedRow["chargeWaitUnit"]);
                const chargeReportUnit = isNaN(parseFloat(editedRow["chargeReportUnit"]))?0.00:parseFloat(editedRow["chargeReportUnit"]);
                const sumValue = chargeWorkUnit + chargeTravelUnit + chargeWaitUnit + chargeReportUnit;                
                
                this.setState((state) => {
                    return {
                        chargeTotalUnit:sumValue
                    };
                });
                this.updatedData["chargeTotalUnit"] = sumValue;                

                if(this.state.isTimePayUnitEntered === undefined || this.state.isTimePayUnitEntered === false) {                    
                    this.setState((state) => {
                        return {                            
                            payUnit: sumValue
                        };
                    });
                    this.updatedData["payUnit"] = sumValue;
                }
        }
    }
    
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
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTimes)) {
            this.props.VisitTechnicalSpecialistTimes.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeWorkUnit = FormatTwoDecimal(row.chargeWorkUnit);
                    row.chargeTravelUnit = FormatTwoDecimal(row.chargeTravelUnit);
                    row.chargeWaitUnit = FormatTwoDecimal(row.chargeWaitUnit);
                    row.chargeReportUnit = FormatTwoDecimal(row.chargeReportUnit);
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
                    techSpecTime.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    visitTechSpecTimes: techSpecTime,
                };
            });
            if(this.state.isReloadGrid && this.gridTimeChild && this.gridTimeChild.gridApi) this.gridTimeChild.gridApi.setRowData(techSpecTime);
        }        
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistTravels)) {
            rowIndex = 0;
            this.props.VisitTechnicalSpecialistTravels.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);                    
                    row.chargeTotalUnit = FormatTwoDecimal(row.chargeTotalUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);            
                    row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
                    row.payRateReadonly = this.props.payRateReadonly;
                    row = this.TravelItemValidation(row, e, rowIndex, lineItemPermision);
                    row.lineItemPermision = lineItemPermision; 
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }                           
                    techSpecTravel.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    visitTechSpecTravels: techSpecTravel,
                };
            });
            if(this.state.isReloadGrid && this.gridTravelChild && this.gridTravelChild.gridApi) this.gridTravelChild.gridApi.setRowData(techSpecTravel);
        }
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistExpenses)) {    
            rowIndex = 0;       
            for (let i = 0; i < this.props.VisitTechnicalSpecialistExpenses.length; i++) {    
                let row = this.props.VisitTechnicalSpecialistExpenses[i];                            
                if(pin == row.pin && row.recordStatus !== 'D') {
                    if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.chargeRateCurrency) && row.currency !== row.chargeRateCurrency) && (row.chargeRateExchange == null || row.chargeRateExchange === '' || row.chargeRateExchange === 0)) {
                        if(this.props.visitExchangeRates && this.props.visitExchangeRates.filter(rates => rates.currencyFrom === row.currency 
                        && rates.currencyTo === row.chargeRateCurrency).length > 0) {
                            this.props.visitExchangeRates.map(rates => {
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
                            const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates, this.props.visitInfo.visitContractNumber);
                            if (res && res.length > 0) {
                                row.chargeRateExchange = res[0] ? FormatSixDecimal(res[0].rate) : null;
                                if(row.recordStatus !== "N") row.recordStatus = "M";
                            }
                        }
                    } else if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.chargeRateCurrency) && row.currency === row.chargeRateCurrency) && (row.chargeRateExchange == null || row.chargeRateExchange === '' || row.chargeRateExchange === 0)) {
                        row.chargeRateExchange = 1.000000;
                        if(row.recordStatus !== "N") row.recordStatus = "M";
                    } else {
                        row.chargeRateExchange = FormatSixDecimal(row.chargeRateExchange);
                    }
                    if((!isEmptyOrUndefine(row.currency) && !isEmptyOrUndefine(row.payRateCurrency) && row.currency !== row.payRateCurrency) && (row.payRateExchange === null || row.payRateExchange === '' || row.payRateExchange === 0)) {                        
                        if(this.props.visitExchangeRates && this.props.visitExchangeRates.filter(rates => rates.currencyFrom === row.currency 
                        && rates.currencyTo === row.payRateCurrency).length > 0) {
                            this.props.visitExchangeRates && this.props.visitExchangeRates.map(rates => {
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
                            const res = await this.props.actions.FetchCurrencyExchangeRate(exchangeRates, this.props.visitInfo.visitContractNumber);
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
                    row.chargeRateReadonly = this.props.chargeRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
                    row.payRateReadonly = this.props.payRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
                    row = this.ExpenseItemValidation(row, e, rowIndex, lineItemPermision);                    
                    row.lineItemPermision = lineItemPermision;      
                    if(row.chargeRate && row.chargeRate != undefined){
                        const rowChargeRate = parseFloat(row.chargeRate);
                        row.hasNoChargeRate = rowChargeRate > 0 ? false : true;
                    }
                    techSpecExpense.push(row);
                    rowIndex++;
                }
            }
            this.setState((state) => {
                return {
                    visitTechSpecExpenses: techSpecExpense,
                };
            });
            if(this.state.isReloadGrid && this.gridExpenseChild && this.gridExpenseChild.gridApi) this.gridExpenseChild.gridApi.setRowData(techSpecExpense);
        }
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialistConsumables)) {
            rowIndex = 0;
            this.props.VisitTechnicalSpecialistConsumables.forEach(row => {
                if(pin == row.pin && row.recordStatus !== 'D') {
                    row.chargeRate = row.chargeRate;
                    row.payRate = FormatFourDecimal(row.payRate);
                    row.chargeTotalUnit = FormatTwoDecimal(row.chargeTotalUnit);
                    row.payUnit = FormatTwoDecimal(row.payUnit);                    
                    row.chargeRateReadonly = !this.props.chargeRateReadonly ? isChargeRateReadOnly : this.props.chargeRateReadonly;
                    row.payRateReadonly = this.props.payRateReadonly;
                    if(isEmptyOrUndefine(row.payExpenseType) && !isEmptyOrUndefine(row.chargeExpenseType)) {
                        row.payExpenseType = row.chargeExpenseType;
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
                    techSpecConsumable.push(row);
                    rowIndex++;
                }
            });
            this.setState((state) => {
                return {
                    visitTechSpecConsumables: techSpecConsumable,
                };
            });
            if(this.state.isReloadGrid && this.gridConsumableChild && this.gridConsumableChild.gridApi) this.gridConsumableChild.gridApi.setRowData(techSpecConsumable);
        }
    }

    techSpecRowSelectHandler = (e) => {
        if(e.node.isSelected()) {            
            this.bindTechSpecLineItems(e.data.pin);
        } else {
            const selectedData = this.gridChild.getSelectedRows();            
            if(isEmptyOrUndefine(selectedData)) {
                e.node.setSelected(true);
            }
        }
    };

    calculateExpenseGross(data) {
        const payUnit = isNaN(parseFloat(data.payUnit))?0:parseFloat(data.payUnit),
         payRate = isNaN(parseFloat(data.payRate))?0:parseFloat(data.payRate),
         payRateTax = isNaN(parseFloat(data.payRateTax))?0:parseFloat(data.payRateTax);          
         return (payUnit * payRate) + payRateTax;
    }
    
    visitGrossMarginClass = (grossMargin)=>{
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

    formatExchangeRate= (rate)=>{
        return FormatSixDecimal(rate);
    }

    tabSelect = (index, lastIndex, event) => {
        const lineItemPermision = this.isLineItemEditable();
        if(this.props.pageMode !== localConstant.commonConstants.VIEW && this.props.isExpenseOpen) {
            if (event.target.innerText === localConstant.visit.EXPENSES && lineItemPermision.isLineItemEditable) {
                const { unLinkedAssignmentExpenses } = this.props;
                if (unLinkedAssignmentExpenses && unLinkedAssignmentExpenses.length > 0 && this.props.isTBAVisitStatus !== true) {
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
                            && event.target.innerText === localConstant.visit.EXPENSES ? true : false),
            moduleType: (event.target.innerText === localConstant.visit.TIME ? "R" : "")
        });
        
        this.setState({
            selectedTabIndex: index            
        });

        const selectedData = this.gridChild.getSelectedRows();       
        let pin = 0;
        if(selectedData !== undefined) { 
            selectedData.map(row => {
                pin = row.pin;
            });
        }
        this.bindTechSpecLineItems(pin);
    }
    submitUnLinkedExpenses = async (e) => {
        e.preventDefault();
        const selectedRecords = isEmptyReturnDefault(this.UnLinkedAssignmentExpensesGrid.getSelectedRows());
        const selectedTechnicalSpecialist = isEmptyReturnDefault(this.gridChild.getSelectedRows());
        const currentSelectedTechSpec = selectedTechnicalSpecialist.length > 0 ? selectedTechnicalSpecialist[0].pin : 0;
        const technicalSpecialists = this.props.VisitTechnicalSpecialists;
        const defaultScheduleCurrency = GetScheduleDefaultCurrency({ 
            techSpecRateSchedules: this.props.defaultTechSpecRateSchedules, pin:currentSelectedTechSpec });
        if (selectedRecords && selectedRecords.length > 0) {
            selectedRecords.forEach( async (rowData) => {
                const rateCurrencyObj= {};                                  
                if (!isEmptyOrUndefine(rowData.currencyCode)
                        && !isEmptyOrUndefine(defaultScheduleCurrency.defaultChargeCurrency)
                        && !isEmptyOrUndefine(defaultScheduleCurrency.defaultPayCurrency)) {
                            rateCurrencyObj.chargeRateCurrency = defaultScheduleCurrency.defaultChargeCurrency;
                            rateCurrencyObj.payRateCurrency = defaultScheduleCurrency.defaultPayCurrency;
                        const exchangeRates = [ {
                            currencyTo: defaultScheduleCurrency.defaultChargeCurrency,
                            currencyFrom: rowData.currencyCode,
                            effectiveDate: currentDate
                        },
                        {
                            currencyTo: defaultScheduleCurrency.defaultPayCurrency,
                            currencyFrom: rowData.currencyCode,
                            effectiveDate: currentDate
                        } ];
                        await this.addDefaultExchangeRate(exchangeRates,rateCurrencyObj);
                    }
                selectedTechnicalSpecialist.forEach((eachTs) => {
                    const tempData = {};
                    tempData.expenseDate = this.props.visitInfo.visitStartDate;
                    tempData.chargeExpenseType = rowData.expenseType;
                    tempData.currency = rowData.currencyCode ? rowData.currencyCode : null;
                    tempData.chargeRateCurrency = defaultScheduleCurrency.defaultChargeCurrency;
                    tempData.payRateCurrency = defaultScheduleCurrency.defaultPayCurrency;
                    tempData.chargeUnit =  rowData.totalUnit;
                    tempData.chargeRate = rowData.perUnitRate;
                    tempData.isContractHolderExpense = false;
                    tempData.expenseDescription = rowData.description;
                    tempData.payRate = "0.0000";
                    tempData.payRateTax = "0.00";
                    tempData.chargeRateExchange = rateCurrencyObj.chargeRateExchange ? this.formatExchangeRate(rateCurrencyObj.chargeRateExchange) : "0.000000";
                    tempData.payRateExchange =  rateCurrencyObj.payRateExchange ? this.formatExchangeRate(rateCurrencyObj.payRateExchange) : "0.000000";
                    tempData.payUnit = "0.00";
                    tempData.pin = eachTs.pin;
                    tempData.visitTechnicalSpecialistId = eachTs.visitTechnicalSpecialistId;
                    tempData.recordStatus = 'N';
                    tempData.TechSpecExpenseId = Math.floor(Math.random() * 99) - 100;
                    tempData.assignmentExpensesAlreadyMapped = false;
                    tempData.assignmentId = this.props.visitInfo.visitAssignmentId;
                    tempData.contractNumber = this.props.visitInfo.visitContractNumber;
                    tempData.projectNumber = this.props.visitInfo.visitProjectNumber;
                    tempData.chargeRateReadonly = this.props.chargeRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
                    tempData.payRateReadonly = this.props.payRateReadonly; // Comment this code to fix Defect 1582 & 1101. Kept on Hold
                    tempData.assignmentAdditionalExpenseId = rowData.assignmentAdditionalExpenseId;
                    tempData.lineItemPermision = this.isLineItemEditable();
                    this.props.actions.AddTechnicalSpecialistExpense(tempData);
                    if (currentSelectedTechSpec === eachTs.pin){
                        this.state.visitTechSpecExpenses.unshift(tempData);
                        setTimeout(() => {
                            this.gridExpenseChild.gridApi.setRowData(this.state.visitTechSpecExpenses);
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

    isLineItemEditable = () => {
        //Once the line item is created by the OC (Intercompany Scenario) OC coordinator can edit the 
        //line item before the stage of Approval (Approved by Operating Company Coordinator)
        // whereas at this stage CHC cannot edit the data which is saved by OC (still not approved by OC).
        const {
            isInterCompanyAssignment,
            isOperator,
            isCoordinator,
            visitInfo,
            isOperatingCompany,
            isCoordinatorCompany,
            interactionMode,
            notLoggedinCompany
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
            isLoggedinCompany: ([ 'O', 'A' ].includes(visitInfo.visitStatus) ? notLoggedinCompany: false),
            isInterOperatingCompany: (isInterCompanyAssignment && isOperatingCompany ? true : false)
        };        
        const {
            isNewRecord,
        } = this.state;

        if(interactionMode || ([ 'A' ].includes(visitInfo.visitStatus) && !this.props.modifyAddApprovedLines)) {
            lineItemPermision.isLineItemEditable = false;
            lineItemPermision.isLineItemEditableExpense = false;
            lineItemPermision.isLineItemEditableOnChargeSide = false;
            lineItemPermision.isLineItemEditableOnPaySide = false;
            lineItemPermision.isLineItemDeletable = false;
        } else if (isInterCompanyAssignment) {
            if (isCoordinatorCompany && [ 'T','J','Q','C','R' ].includes(visitInfo.visitStatus)) {
                lineItemPermision.isLineItemEditable = false;
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = false;
            } else if (isCoordinatorCompany && 'O' === visitInfo.visitStatus) {
                lineItemPermision.isLineItemEditable = true;
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = true;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = true;
            } else if (isCoordinatorCompany && 'A' === visitInfo.visitStatus) {
                lineItemPermision.isLineItemEditable = true;
                lineItemPermision.isLineItemEditableExpense = true;
                lineItemPermision.isLineItemEditableOnChargeSide = true;
                lineItemPermision.isLineItemEditableOnPaySide = false;
                lineItemPermision.isLineItemDeletable = true;
            } else if (isOperatingCompany && 'O' === visitInfo.visitStatus) {               
                lineItemPermision.isLineItemEditable = this.props.modifyAddApprovedLines;                
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = this.props.modifyAddApprovedLines;
                lineItemPermision.isLineItemDeletable = this.props.modifyAddApprovedLines;
            } else if(isOperatingCompany && 'A' === visitInfo.visitStatus){
                lineItemPermision.isLineItemEditable = this.props.modifyAddApprovedLines;                
                lineItemPermision.isLineItemEditableExpense = false;
                lineItemPermision.isLineItemEditableOnChargeSide = false;
                lineItemPermision.isLineItemEditableOnPaySide = this.props.modifyAddApprovedLines;
                lineItemPermision.isLineItemDeletable = true;
            }
        }
        
        return lineItemPermision;
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

    checkTwoDigitNumber = (e) => {
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) ){
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        }
        this.formInputHandler(e);
    }

    decimalWithTwoLimitFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(2) +"})?)");
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
        if (e.rowIndex !== null) {
            const selectedData = this.gridChild.getSelectedRows();       
            let pin = 0;
            if(selectedData !== undefined) { 
                selectedData.map(row => {
                    pin = row.pin;
                });
            }

            this.bindTechSpecLineItems(pin, e);

            if(e.column.getId() === 'expenseDate' || e.column.getId() === 'expenseDescription') {
                this.setState({ isReloadGrid : true });
            } else {
                this.setState({ isReloadGrid : false });
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
        const { 
            disableField,
            interactionMode, 
            techSpecRateSchedules,
            visitTechnicalSpecialistsGrossMargin,
            isTBAVisitStatus,
            visitInfo }= this.props; 
        
        let selectedTechnicalSpecialist = [];
        let disableAddDelete = false;
        if(!isEmptyOrUndefine(this.props.VisitTechnicalSpecialists)) {
            selectedTechnicalSpecialist = this.props.VisitTechnicalSpecialists.filter(x => x.recordStatus !== "D");            
            if (isEmptyOrUndefine(selectedTechnicalSpecialist)) {
                disableAddDelete = true;
            }
        } else {
            disableAddDelete = true;
        }

        const workFlowType = visitInfo.workflowType && visitInfo.workflowType.trim();
        const isShowConsumableTab = 'S' === workFlowType;
        
        const expensesCategories =  this.props.expenseTypes;        
        this.timeTypes = expensesCategories.filter(expense =>expense.chargeType === 'R');
        this.travelTypes = expensesCategories.filter(expense =>expense.chargeType === 'T');
        this.expenseTypes = expensesCategories.filter(expense =>expense.chargeType === 'E');
        this.consumableTypes = expensesCategories.filter(expense => (expense.chargeType === 'C' || expense.chargeType === 'Q')); // ITK D-721

        bindAction(this.headerData.technicalSpecialist, "RateSchedules", this.editRateScheduleRowHandler); 

        bindAction(this.headerData.timeHeaderDetails,"EditTimeColumn",this.editTimeRowHandler);
        bindAction(this.headerData.expensesDetails,"EditExpenseColumn",this.editExpenseRowHandler);
        bindAction(this.headerData.travelDetails,"EditTravelColumn",this.editTravelRowHandler);
        bindAction(this.headerData.consumableDetails,"EditConsumableColumn",this.editConsumableRowHandler);        
        bindActionWithChildLevel(this.headerData.timeHeaderDetails.columnDefs,"EditTimeChargePayRate",this.editChargeRateRowHandler); 
        bindActionWithChildLevel(this.headerData.timeHeaderDetails.columnDefs,"EditTimePayRate",this.editPayRateRowHandler);
        bindActionWithChildLevel(this.headerData.expensesDetails.columnDefs,"EditExpenseChargePayRate",this.editChargeRateRowHandler);
        bindActionWithChildLevel(this.headerData.expensesDetails.columnDefs,"EditExpensePayRate",this.editPayRateRowHandler);
        bindActionWithChildLevel(this.headerData.travelDetails.columnDefs,"EditTravelChargePayRate",this.editChargeRateRowHandler);
        bindActionWithChildLevel(this.headerData.travelDetails.columnDefs,"EditTravelPayRate",this.editPayRateRowHandler);
        bindActionWithChildLevel(this.headerData.consumableDetails.columnDefs,"EditConsumableChargePayRate",this.editChargeRateRowHandler); 
        bindActionWithChildLevel(this.headerData.consumableDetails.columnDefs,"EditConsumablePayRate",this.editPayRateRowHandler);

        const grossMarginClass = this.visitGrossMarginClass(visitTechnicalSpecialistsGrossMargin);

        const lineItemPermision = this.isLineItemEditable();
        return (
            <Fragment>
                {this.state.isAddUnlinkedAssignmentExpenses ?
                    <Modal title="Add Unlinked Expenses"
                        modalClass="UnlinkedAssignmentExpensesModel"
                        modalId="UnlinkedAssignmentExpensesPopup"
                        formId="UnlinkedAssignmentExpensesForm"
                        buttons={
                            [ {
                                name: localConstant.commonConstants.CANCEL,
                                action: this.cancelUnLinkedExpenses,
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                                type: "button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action:(e)=>this.submitUnLinkedExpenses(e),
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                            } ]
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
                    title={ localConstant.visit.SCHEDULE_RATES}
                    buttons={[                           
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelChargeScheduleModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            } ]
                    }
                    isShowModal={this.state.scheduleRateShowModal}>
                    <ScheduleRate 
                        techSpecRateSchedules={techSpecRateSchedules}
                        defaultTechSpecRateSchedules = {this.props.defaultTechSpecRateSchedules}
                        pin = {this.editedRowData.pin}
                        selectedCompany = {this.props.selectedCompany}
                        modifyPayUnitPayRate = {this.props.modifyPayUnitPayRate}
                    gridHeader={this.headerData}/>
                   
                </Modal>
            }
            {this.state.timeShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.visit.TIME}
                    buttons={[                           
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelTimeModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            }, {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitTimeModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                }, ]
                    }
                    isShowModal={this.state.timeShowModal}>
                    <TimeModal
                        editedTimeData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.timeTypes}
                        isTimeModel={true}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}  
                        chargeTotalUnit = {this.state.chargeTotalUnit}
                        payUnit = {this.state.payUnit}   
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                        checkTwoDigitNumber={this.checkTwoDigitNumber}
                        decimalWithTwoLimitFormat={this.decimalWithTwoLimitFormat}       
                    />
                </Modal>
            }
            {this.state.expensesShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.visit.EXPENSES}
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
                            }
                             ]
                    }
                    isShowModal={this.state.expensesShowModal}>
                    <ExpensesModal 
                        editedExpenseData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.expenseTypes}
                        isExpensesModel={true}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        grossValue={this.state.grossValue}
                        payUnit={this.state.payUnit}
                        chargeRateExchange = {this.state.chargeRateExchange}
                        payRateExchange =  {this.state.payRateExchange}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                        checkTwoDigitNumber={this.checkTwoDigitNumber}
                        decimalWithTwoLimitFormat={this.decimalWithTwoLimitFormat}          
                    />
                </Modal>
            }
             {this.state.travelShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.visit.TRAVEL}
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
                            }
                             ]
                    }
                    isShowModal={this.state.travelShowModal}>
                    <TravelModal 
                        editedTravelData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.travelTypes}
                        isTravelModel={true}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        swapUpHandler={this.onswapUpHandler}
                        swapDownHandler={this.onswapDownHandler}                       
                        chargeTotalUnit={this.state.chargeTotalUnit}
                        payUnit={this.state.payUnit}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                        checkTwoDigitNumber={this.checkTwoDigitNumber}
                        decimalWithTwoLimitFormat={this.decimalWithTwoLimitFormat}           
                    />
                </Modal>
            }
            {this.state.consumableShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    modalContentClass="modalMaxHeight"
                    title={ localConstant.visit.CONSUMABLE_EQUIPMENT}
                    buttons={[
                            
                            {
                            name: localConstant.commonConstants.CANCEL,
                            action: this.cancelConsumableShowModal,
                            btnClass: "btn-small mr-2",
                            showbtn: true,
                            type:"button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action: this.submitConsumableModal,
                                btnClass: "btn-small mr-2",
                                showbtn: lineItemPermision.isLineItemEditable? true:false,
                                }, ]
                    }
                    isShowModal={this.state.consumableShowModal}>
                    <ConsumableModal 
                        editedConsumableData = { this.editedRowData }
                        currencyMasterData = {this.currencyData}
                        chargeType = {this.consumableTypes}
                        isConsumableModel={true}
                        expenseDate = { this.state.expenseDate }
                        expenseDateChange = { this.expenseDateChange }
                        onChangeHandler = {this.onChangeHandler}
                        chargeTotalUnit={this.state.chargeTotalUnit}
                        payUnit={this.state.payUnit}
                        lineItemPermision = {lineItemPermision}
                        isNewRecord={this.state.isNewRecord}
                        checkNumber={this.checkNumber}
                        decimalWithLimtFormat={this.decimalWithLimtFormat}
                        checkTwoDigitNumber={this.checkTwoDigitNumber}
                        decimalWithTwoLimitFormat={this.decimalWithTwoLimitFormat}           
                    />
                </Modal>
            }
            {this.state.payRateShowModal &&
                <Modal
                    modalClass="chargescheduleModal"
                    title={ this.state.isPayRate ? localConstant.visit.PAY_RATES :localConstant.visit.RATES}
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
                        showbtn: true,
                        disabled: isTBAVisitStatus
                        },
                    ]}
                    isShowModal={this.state.payRateShowModal}>
                    <ReactGrid 
                        gridRowData={this.state.selectedPayRateGridData} 
                        gridColData={this.state.isPayRate ? (this.state.moduleType === "R" && !this.props.modifyPayUnitPayRate ? this.headerData.payRateModifyHeader : this.headerData.payRateHeader) : this.headerData.chargeRateHeader}    
                        onRef={ref => { this.gridPayRate = ref; }}   
                        paginationPrefixId={this.state.isPayRate ? localConstant.paginationPrefixIds.visitPayRateId : localConstant.paginationPrefixIds.visitChargeRateId}
                    />
                </Modal>
            }
            {this.state.isInvoiceNotesPopup ?
                <Modal title={localConstant.project.invoicingDefaults.INVOICE_NOTES}
                    modalClass="visitModal"
                    modalId="visitInvoiceNotesPopup"
                    formId="visitInvoiceNotesForm"
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
                    defaultValue={isEmptyOrUndefine(visitInfo.projectInvoiceInstructionNotes) ? '' : visitInfo.projectInvoiceInstructionNotes}
                    readOnly={true}
                    />
                </Modal> : null}
            <div className="customCard left fullWidth">
            <h6 className="bold col s12 pl-0 pr-3">{localConstant.visit.TECHNICAL_SPECIALIST_ACCOUNTS}</h6>
                <div className="bold col s6">Visit Gross Margin: {(visitTechnicalSpecialistsGrossMargin)
                    ?<span className={grossMarginClass}>
                        { grossMarginClass ? <i class="zmdi zmdi-alert-triangle"></i>:'' }{visitTechnicalSpecialistsGrossMargin}
                    </span>
                :''}</div>
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
                            disabled={ interactionMode || this.props.pageMode==localConstant.commonConstants.VIEW || (this.props.isTBAVisitStatus  ? this.props.isTBAVisitStatus : false) }
                        />
                      <a className="waves-effect btn-small right invoiceNotesMargin"
                            onClick={this.invoiceNotes} disabled = {this.props.isTBAVisitStatus ? this.props.isTBAVisitStatus : false} >
                            {localConstant.project.invoicingDefaults.INVOICE_NOTES}</a>
                         {this.state.isShowAdditionalExpenses && <a className="waves-effect btn-small right" 
                            onClick={this.showAdditionalExpenses} disabled = {this.props.isTBAVisitStatus||interactionMode ? this.props.isTBAVisitStatus : false} >
                            {localConstant.commonConstants.SHOW_ADDITIONAL_EXPENSES}</a> }
                </div>
            <ReactGrid 
                gridRowData={selectedTechnicalSpecialist} 
                gridColData={this.headerData.technicalSpecialist}
                rowSelected={this.techSpecRowSelectHandler}
                onRef={ref => { this.gridChild = ref; }}
                paginationPrefixId={localConstant.paginationPrefixIds.visitTechSpecGridId}
            />
           
            </div>
            <div className="customCard col s12 horizontalTabs pl-0 pr-0 mt-2 fullWidth">
            <Tabs tabActive={2} onSelect={this.tabSelect}>
            <TabList>
            <Tab className="react-tabs__tab time">{localConstant.visit.TIME}</Tab>
            <Tab className="react-tabs__tab expenses">{localConstant.visit.EXPENSES}</Tab>
            <Tab className="react-tabs__tab travel">{localConstant.visit.TRAVEL}</Tab>                    
            {
                isShowConsumableTab ? <Tab className="react-tabs__tab equipment">{localConstant.visit.CONSUMABLE_EQUIPMENT}</Tab> : null
            }
            </TabList>
                <TabPanel title={localConstant.visit.TIME}>
                    <ReactGrid 
                        gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                        gridRowData={this.state.visitTechSpecTimes} 
                        gridColData={this.headerData.timeHeaderDetails} 
                        onRef={ref => { this.gridTimeChild = ref; }}
                        onCellFocused={this.onCellFocused}           
                        gridTwoPagination={true}             
                    />
                    { lineItemPermision.isLineItemEditable && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                        <div className="right-align mt-2 mb-2">
                            <a className="waves-effect btn-small" onClick={this.addTimeNewRow} disabled={disableField || interactionMode || isTBAVisitStatus || disableAddDelete} >{localConstant.commonConstants.ADD}</a>                            
                            {lineItemPermision.isLineItemDeletable ?
                                <a className="btn-small ml-2 dangerBtn mr-2 " onClick={this.deleteTechSpecTime}
                                    disabled={disableAddDelete || disableField || interactionMode || isTBAVisitStatus} >
                                    {localConstant.commonConstants.DELETE}</a>
                                : null
                            }
                        </div> : null }                                        
                    </TabPanel>
                    <TabPanel title={localConstant.visit.EXPENSES}>
                    <ReactGrid 
                        gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                        gridRowData={this.state.visitTechSpecExpenses} 
                        gridColData={this.headerData.expensesDetails} 
                        onRef={ref => { this.gridExpenseChild = ref; }}
                        onCellFocused={this.onCellFocused}
                        gridTwoPagination={true}
                    />
                    { lineItemPermision.isLineItemEditable && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                        <div className="right-align mt-2 mb-2">
                            <a className="waves-effect btn-small" onClick={this.addExpenseNewRow} disabled = {disableField || interactionMode || isTBAVisitStatus || disableAddDelete} >{localConstant.commonConstants.ADD}</a>                            
                            {lineItemPermision.isLineItemDeletable ?
                                <a className="btn-small ml-2 dangerBtn mr-2" onClick={this.deleteTechSpecExpense}
                                    disabled={disableAddDelete || disableField || interactionMode || isTBAVisitStatus} >
                                    {localConstant.commonConstants.DELETE}</a>
                                : null
                            }
                        </div> : null }
                    </TabPanel>
                    <TabPanel title={localConstant.visit.TRAVEL}>
                    <ReactGrid 
                        gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                        gridRowData={this.state.visitTechSpecTravels}
                        gridColData={this.headerData.travelDetails} 
                        onRef={ref => { this.gridTravelChild = ref; }}
                        onCellFocused={this.onCellFocused}
                        gridTwoPagination={true}
                    />
                    { lineItemPermision.isLineItemEditable && this.props.pageMode!==localConstant.commonConstants.VIEW?
                        <div className="right-align mt-2 mb-2">
                            <a className="waves-effect btn-small" onClick={this.addTravelNewRow} disabled = {disableField || interactionMode || isTBAVisitStatus || disableAddDelete} >{localConstant.commonConstants.ADD}</a>                            
                            {lineItemPermision.isLineItemDeletable ?
                                <a className="btn-small ml-2 dangerBtn mr-2" onClick={this.deleteTechSpecTravel}
                                    disabled={disableAddDelete || disableField || interactionMode || isTBAVisitStatus} >
                                    {localConstant.commonConstants.DELETE}</a>
                                : null
                            }
                        </div> : null } 
                    </TabPanel>
                    {
                        isShowConsumableTab ?
                        <TabPanel title={localConstant.visit.CONSUMABLE_EQUIPMENT}>
                            <ReactGrid 
                                gridCustomClass={'inlineGridStyle inlineFixedGrid'}
                                gridRowData={this.state.visitTechSpecConsumables}
                                gridColData={this.headerData.consumableDetails} 
                                onRef={ref => { this.gridConsumableChild = ref; }}
                                onCellFocused={this.onCellFocused}
                                gridTwoPagination={true}
                            />
                            { lineItemPermision.isLineItemEditable && this.props.pageMode!==localConstant.commonConstants.VIEW ?
                            <div className="right-align mt-2 mb-2">
                                <a className="waves-effect btn-small" onClick={this.addConsumableNewRow} disabled = {disableField || interactionMode || isTBAVisitStatus || disableAddDelete} >{localConstant.commonConstants.ADD}</a>                                
                                {lineItemPermision.isLineItemDeletable ?
                                    <a className="btn-small ml-2 dangerBtn mr-2" onClick={this.deleteTechSpecConsumable}
                                        disabled={disableAddDelete || disableField || interactionMode || isTBAVisitStatus} >
                                        {localConstant.commonConstants.DELETE}</a>
                                    : null
                                }
                            </div> : null}
                    </TabPanel>
                        : null
                    }                   
                    
            </Tabs>
            </div>
            </Fragment>
        );
    }
}

export default TechnicalSpecialistAccounts;
