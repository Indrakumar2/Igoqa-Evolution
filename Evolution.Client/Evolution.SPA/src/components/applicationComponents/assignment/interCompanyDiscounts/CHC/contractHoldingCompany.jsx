import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../../utils/commonUtils';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import { fieldLengthConstants } from '../../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const contractHoldingCompany = (props) => {
   
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.assignments.CONTRACT_HOLDING_COMPANY}
                type='text'
                name="assignmentContractHoldingCompanyName"
                colSize='s12 m4'
                inputClass="customInputs"
                
                defaultValue={props.isInterCompany ? props.name : ''}
                readOnly={true}
                // disabled={true}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={''}
                name='assignmentContractHoldingCompanyDiscount'
                type='text'
                dataType='decimal'
                valueType='value'
                value={props.isInterCompany ? props.value:" "}
               // value={props.isInterCompany ? props.value === ''  ? 15 : props.value  : ""}
                colSize='s12 m2'
                inputClass="customInputs"
                //defaultValue={ props.isInterCompany ? 15 : " " }
                maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                max={99999}
                onValueChange={props.interCompanyDiscountsChange}
                //onValueBlur={props.checkNumber}
                required={true}
                readOnly={props.interactionMode || props.disabled}
                // disabled={props.interactionMode || props.disabled}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={' '}
                type='text'
                dataValType="valueText"
                name="assignmentContractHoldingCompanyDescription"
                colSize='s12 m4'
                inputClass="customInputs"
               // defaultValue={ props.isInterCompany ? "InterCo Discount" : " "} 
              //  value={props.isInterCompany ? props.value === ''  ? "InterCo Discount" : props.description  : ""}
                value={props.isInterCompany ? props.description:''}
                onValueChange={props.interCompanyDiscountsChange}
                required={true}
                readOnly={props.interactionMode || props.disabled}
                // disabled={props.interactionMode || props.disabled}
                maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.DESCRIPTION_MAXLENGTH}
            />
        </Fragment>
    );
};

export default contractHoldingCompany;