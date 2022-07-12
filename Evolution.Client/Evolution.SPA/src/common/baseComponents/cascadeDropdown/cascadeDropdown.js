import React, { Fragment } from 'react';
import CustomInput from '../inputControlls';
import IntertekToaster from '../intertekToaster';

const cascadeDropdown = (props) => {
    if(props.dropdownList && props.dropdownList.length > 1){
        return (
            <Fragment>
                <div className={props.alignmentClass}>
                {
                    props.dropdownList.map(iteratedValue=>{
                        return(
                        <CustomInput 
                            hasLabel={iteratedValue.hasLabel}
                            divClassName={iteratedValue.divClassName}
                            labelClass={iteratedValue.labelClass}
                            required={iteratedValue.required}
                            name={iteratedValue.name}
                            label={iteratedValue.label}
                            type={iteratedValue.type}
                            colSize={iteratedValue.colSize}
                            defaultValue={iteratedValue.defaultValue}
                            disabled = {iteratedValue.disabled}
                            className={iteratedValue.className}
                            optionsList={iteratedValue.optionsList}
                            optionName={iteratedValue.optionName}
                            optionValue={iteratedValue.optionValue}
                            id={iteratedValue.id}
                            onSelectChange={iteratedValue.onSelectChange}
                        />
                        );
                    })
                }
                </div>
            </Fragment>      
        );
    }
    else{
        IntertekToaster('More than one dropdown required','warningToast cascadeDropdown');
        return null;
    }
    
};

export default cascadeDropdown;