import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../../utils/commonUtils';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import { fieldLengthConstants } from '../../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const parentContractHolder = (props) => {
    return (
        <Fragment>
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.assignments.PARENT_CONTRACT_HOLDER}
                    type='text'
                    name="parentContractHoldingCompanyName"
                    colSize='s12 m4'
                    inputClass="customInputs"
                    defaultValue={props.name}
                    readOnly={true}
                    // disabled = {true}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={''}
                    type='text'
                    dataType='decimal'
                    valueType='value'
                    name='parentContractHoldingCompanyDiscount'
                    value={props.value ? parseFloat(props.value).toFixed(2) : ''}
                    colSize='s12 m2'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                    max={99999}
                    onValueChange={props.interCompanyDiscountsChange}
                    onValueBlur={props.checkNumber}
                    required={true}
                    readOnly={true}
                    // disabled = {true} 
                    onRef={props.onRef}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={''}
                    type='text'
                    dataValType="valueText"
                    name="parentContractHoldingCompanyDescription"
                    colSize='s12 m4'
                    inputClass="customInputs"
                    value={props.description ? props.description:''}
                    onValueChange={props.interCompanyDiscountsChange}
                    required={true}
                    readOnly={props.interactionMode || props.disabled}
                    // disabled = {props.interactionMode || props.disabled}
                    maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.DESCRIPTION_MAXLENGTH}
                />   
    </Fragment>
    );
};

export default parentContractHolder;