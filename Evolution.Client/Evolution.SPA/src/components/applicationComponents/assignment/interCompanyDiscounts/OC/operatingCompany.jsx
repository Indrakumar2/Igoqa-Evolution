import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../../utils/commonUtils';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import { required,requiredNumeric } from '../../../../../utils/validator';
import { fieldLengthConstants } from '../../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const OperatingCompany = (props) => {
    return (
        <Fragment>
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.assignments.OPERATING_COMPANY }
                    type='text'
                    name="assignmentOperatingCompanyName"
                    colSize='s12 m4'
                    inputClass="customInputs mandate"
                    defaultValue={props.discounts}
                    readOnly = {props.interactionMode || props.disabled}
                    // disabled = {props.interactionMode || props.disabled}
                />
                <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={''}
                type='text'
                dataType='decimal'
                valueType='value'
                name="assignmentOperatingCompanyDiscount"
                colSize='s12 m2'
                inputClass="customInputs mandate"
                maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                max={99999}
                value={!requiredNumeric(props.value)?isNaN(parseFloat(props.value))?parseFloat(100).toFixed(2):parseFloat(props.value).toFixed(2):parseFloat(100).toFixed(2)}
                onValueChange={props.interCompanyDiscountsChange}
                required={true}
                readOnly = {props.interactionMode || props.disabled}
                // disabled = {props.interactionMode || props.disabled}
                />
                <div className='col s12'> <label className='col s4'></label></div>
    </Fragment>
    );
};

export default OperatingCompany;