import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../../utils/commonUtils';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import { fieldLengthConstants } from '../../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const additionalICOffice = (props) => {
    const isDisableValueAndDesc = props.companyCode?false:true;
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                required={true}
                labelClass="customLabel"
                label={localConstant.assignments.ADDITIONAL_INTERCOMPANY_OFFICE}
                type ='select'
                name={props.name}
                defaultValue={props.showValues?props.companyCode?props.companyCode:'':''}
                //defaultValue={props.companyCode?props.companyCode:''}
                optionsList={props.company}
                optionName='companyName'
                optionValue="companyCode"
                colSize='s12 m4'
                id={props.id}
                onSelectChange={props.interCompanyDiscountsChange}
                disabled ={props.interactionMode || props.disabled}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={''}
                type='text'
                dataType='decimal'
                valueType='value'
                name={props.percentage}
                value={props.showValues?props.discount?props.discount:'':''}
                //value={props.discount?props.discount:''}
                colSize='s12 m2'
                onValueBlur={props.checkNumber}
                inputClass="customInputs"
                maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                max={99999}
                onValueChange={props.interCompanyDiscountsChange}
                required={true}
                readOnly ={isDisableValueAndDesc || props.interactionMode || props.disabled}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={''}
                type='text'
                dataValType="valueText"
                name={props.description}
                colSize='s12 m4'
                inputClass="customInputs"
                value={props.showValues?props.descriptionValue:''}
                //value={props.descriptionValue?props.descriptionValue:''}
                onValueChange={props.interCompanyDiscountsChange}
                required={true}
                readOnly ={isDisableValueAndDesc || props.interactionMode || props.disabled}
                maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.DESCRIPTION_MAXLENGTH}
            />   
    </Fragment>
    );
};

export default additionalICOffice;