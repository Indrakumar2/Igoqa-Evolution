import React from 'react';
import CardPanel from '../../../common/baseComponents/cardPanel';
import { getlocalizeData } from '../../../utils/commonUtils';
import CustomInput from '../../../common/baseComponents/inputControlls';
import LabelWithValue from '../../../common/baseComponents/customLabelwithValue';

const localConstant = getlocalizeData();
const BudgetMonetary = (props) => {
    const { monetaryValues,isCurrencyEditable,monetaryTaxes,monetaryCurrency,unitValues,unitTaxes,monetaryWarning } = props;
    return(
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.commonConstants.BUDGET} colSize="s12">
            <div className="row mb-0">
                <div className="col s6 br-1">
                <span className="col s12 bold">{localConstant.commonConstants.MONETARY}</span>
                            <CustomInput
                                hasLabel={monetaryValues.hasLabel}
                                divClassName={monetaryValues.divClassName}
                                label={monetaryValues.label}
                                type={monetaryValues.type}
                                //dataValType={monetaryValues.dataValType}
                                 dataType={monetaryValues.dataType}
                                 valueType={monetaryValues.valueType}
                                colSize={monetaryValues.colSize}
                                inputClass={monetaryValues.inputClass}
                                labelClass={monetaryValues.labelClass}
                                max={monetaryValues.max}
                                min = {monetaryValues.min}
                                required={monetaryValues.required}
                                name={monetaryValues.name}
                                maxLength={monetaryValues.maxLength}
                                disabled={monetaryValues.disabled}
                                readOnly={monetaryValues.readOnly}
                                // defaultValue={monetaryValues.defaultValue}
                                isLimitType={monetaryValues.isLimitType}
                                value={monetaryValues.value ? monetaryValues.value : ""} // D - 631
                                onValueChange={monetaryValues.onValueChange}
                                onValueBlur={monetaryValues.onValueBlur}
                                prefixLimit={monetaryValues.prefixLimit}
                                suffixLimit={monetaryValues.suffixLimit}
                            />

                    { isCurrencyEditable?
                        <CustomInput
                            hasLabel={monetaryCurrency.hasLabel}
                            divClassName={monetaryCurrency.divClassName}
                            label={monetaryCurrency.label}
                            type={monetaryCurrency.type}
                            colSize={monetaryCurrency.colSize}
                            className={monetaryCurrency.className}
                            labelClass={monetaryCurrency.labelClass}
                            optionsList={monetaryCurrency.optionsList}
                            optionName={monetaryCurrency.optionName}
                            optionValue={monetaryCurrency.optionValue}
                            name={monetaryCurrency.name}
                            readOnly={monetaryValues.readOnly}
                            disabled={monetaryCurrency.disabled}
                            defaultValue={monetaryCurrency.defaultValue}
                            onSelectChange={monetaryCurrency.onSelectChange}
                            value={monetaryCurrency.value}
                            onValueBlur={monetaryCurrency.onValueBlur}//D-675 Changes
                        />:
                        <LabelWithValue
                            className={monetaryCurrency.className}
                            colSize={monetaryCurrency.colSize}
                            label={monetaryCurrency.label}
                            value={monetaryCurrency.value}
                        />
                    }

                    <CustomInput
                                hasLabel={monetaryWarning.hasLabel}
                                divClassName={monetaryWarning.divClassName}
                                label={monetaryWarning.label}
                                type={monetaryWarning.type}
                                dataValType={monetaryWarning.dataValType}
                                dataType={monetaryWarning.dataType}
                                valueType={monetaryWarning.valueType}
                                colSize={monetaryWarning.colSize}
                                inputClass={monetaryWarning.inputClass}
                                labelClass={monetaryWarning.labelClass}
                                max={monetaryWarning.max}
                                min={monetaryWarning.min}
                                required={monetaryWarning.required}
                                name={monetaryWarning.name}
                                maxLength={monetaryWarning.maxLength}
                                readOnly={monetaryWarning.readOnly}
                                disabled={monetaryWarning.disabled}
                                value={monetaryWarning.value}
                                onValueChange={monetaryWarning.onValueChange}
                                onValueBlur={monetaryWarning.onValueBlur}
                                onValueKeypress={monetaryWarning.onValueKeypress}
                            />

                    { monetaryTaxes.map(iteratedValue => {
                        return(
                            <LabelWithValue
                                className={iteratedValue.className}
                                colSize={iteratedValue.colSize}
                                label={iteratedValue.label}
                                value={iteratedValue.value}
                            />
                        );
                    })}
                </div>
                <div className="col s6">
                <span className="col s12 bold">{localConstant.commonConstants.HOURS}</span>
                { unitValues.map(iteratedValue => 
                    {
                        return(
                            <CustomInput
                                hasLabel={iteratedValue.hasLabel}
                                divClassName={iteratedValue.divClassName}
                                label={iteratedValue.label}
                                type={iteratedValue.type}
                                dataValType={iteratedValue.dataValType}
                                dataType={iteratedValue.dataType}
                                valueType={iteratedValue.valueType}
                                // dataType={iteratedValue.dataType}
                                // valueType={iteratedValue.valueType}
                                colSize={iteratedValue.colSize}
                                inputClass={iteratedValue.inputClass}
                                labelClass={iteratedValue.labelClass}
                                max={iteratedValue.max}
                                min = {iteratedValue.min}
                                required={iteratedValue.required}
                                name={iteratedValue.name}
                                maxLength={iteratedValue.maxLength}   
                                readOnly={iteratedValue.readOnly}                           
                                disabled={iteratedValue.disabled}
                                value={iteratedValue.value}
                                isLimitType={iteratedValue.isLimitType}
                                onValueChange={iteratedValue.onValueChange}
                                onValueBlur={iteratedValue.onValueBlur}
                                prefixLimit={iteratedValue.prefixLimit}
                                suffixLimit={iteratedValue.suffixLimit}
                                onValueKeypress={iteratedValue.onValueKeypress}
                            />
                        );
                    }) 
                    }
                    { unitTaxes.map(iteratedValue => {
                        return(
                            <LabelWithValue
                                className={iteratedValue.className}
                                colSize={iteratedValue.colSize}
                                label={iteratedValue.label}
                                value={iteratedValue.value}
                            />
                        );
                    })}
                </div>
            </div>
        </CardPanel>
    );
};

export default BudgetMonetary;