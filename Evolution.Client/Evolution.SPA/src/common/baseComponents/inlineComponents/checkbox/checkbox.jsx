import React, { Component,Fragment } from 'react';
import CustomInput from '../../../baseComponents/inputControlls'; 

class Checkbox extends Component {
    selectedRowHandler = (e) => {  
        this.props.editAction(this.props.data,e);    
    };
    render() {  
        const defaultValue = this.props.data && (this.props.data[this.props.colDef.name] === null ?false :this.props.data[this.props.colDef.name]);
        const rowIndex = this.props && this.props.rowIndex;
    return (
        <Fragment>
              <CustomInput hasLabel={false}
                        type='checkbox'
                        checkBoxArray={[ { label:this.props.data[this.props.colDef.labelName],value: defaultValue } ]}
                        colSize='s6'
                        name={this.props.colDef.name}
                        id={this.props.colDef.name +rowIndex}
                        onCheckboxChange={this.selectedRowHandler}
                        checked={defaultValue}
                        // refProps ={props.checkboxRefProps}
                />
        { 
            this.props.isValidation && this.props.validationProp && this.props.data[this.props.validationProp] &&
            <span class="text-red" title={this.props.data[this.props.validationProp]}><i class="zmdi zmdi-alert-triangle"></i></span>
        }
        </Fragment>
    );
};
};

export default Checkbox;