import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../../utils/commonUtils';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import { fieldLengthConstants } from '../../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const hostCompany = (props) => {
    return (
        <Fragment>
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.assignments.HOST_COMPANY}
                    type='text'
                    name="assignmentHostcompanyName"
                    colSize='s12 m4'
                    inputClass="customInputs"
                    defaultValue={props.showValues?props.discounts:''}
                    //defaultValue={props.discounts}
                    onValueChange={props.interCompanyDiscountsChange}
                    readOnly={true}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={''}
                    type='text'
                    dataType='decimal'
                    valueType='value'
                    name="assignmentHostcompanyDiscount"
                    value={props.showValues?props.value?props.value:'':''}
                    //value={props.value?props.value:''}
                    colSize='s12 m2'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                    max={99999}
                    onValueChange={props.interCompanyDiscountsChange}
                    onValueBlur={props.checkNumber}
                    required={true}
                    readOnly = {props.interactionMode || props.disabled}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={' '}
                    type='text'
                    dataValType="valueText"
                    name="assignmentHostcompanyDescription"
                    colSize='s12 m4'
                    inputClass="customInputs"
                    value={props.showValues?props.description:''}
                    //value={props.description?props.description:''}
                    onValueChange={props.interCompanyDiscountsChange}
                    required={true}
                    readOnly = {props.interactionMode || props.disabled}
                    maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.DESCRIPTION_MAXLENGTH}
                />   
    </Fragment>
    );
};

export default hostCompany;