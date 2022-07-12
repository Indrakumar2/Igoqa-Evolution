import React, { PureComponent } from 'react';
import { getlocalizeData } from '../../utils/commonUtils';
import CustomInput from '../baseComponents/inputControlls';

const localConstant = getlocalizeData();

class multiSelectField extends PureComponent {
    constructor(props){
        super(props);
    }

    shouldComponentUpdate(nextProps, nextState){
        if(JSON.stringify(this.props.defaultValue) === JSON.stringify(nextProps.defaultValue)
            && JSON.stringify(this.props.optionsList) === JSON.stringify(nextProps.optionsList) && this.props.isDisabled == nextProps.isDisabled) //isDisabled Condition Check added for Sanity Def 165
            return false;
        return true;
    }
    render(){
        const { hasLabel, name, colSize, className, label, multiSelectOnChange, optionsList, defaultValue, isDisabled } = this.props;
        return(
            <CustomInput
                hasLabel={hasLabel}
                name={name}
                colSize={colSize}
                label={label}
                type='multiSelect'
                className={className}
                multiSelectdValue={multiSelectOnChange}
                optionsList={optionsList}
                defaultValue={defaultValue}
                disabled={isDisabled}
            />
        );
    }
}

export default multiSelectField;