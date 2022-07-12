import React, { Component,Fragment } from 'react';
import CustomInput from '../../../baseComponents/inputControlls'; 

class Textbox extends Component {
    selectedRowHandler = (e) => {  
        this.props.editAction(this.props.data,e);    
    };
    render() {  
        const defaultValue = this.props.data && this.props.data[this.props.name];
        const rowIndex = this.props && this.props.rowIndex;
    return (
        <Fragment>
            <CustomInput
                hasLabel={false}
                type="text"
                divClassName='s12'
                colSize='s12'
                inputClass="customInputs"
                // onValueInput={(e) => this.selectedRowHandler(e)}
                name={this.props.name}
                maxLength={this.props.maxLength?this.props.maxLength:''}
                onValueBlur={(e) => this.selectedRowHandler(e)}
                htmlFor={this.props.name + rowIndex}
                id={this.props.name +rowIndex}
                defaultValue={defaultValue}
                readOnly={this.props.disabled}
            />
        { 
            this.props.validationProp && this.props.data[this.props.validationProp] &&
            <span class="text-red" title={this.props.data[this.props.validationProp]}><i class="zmdi zmdi-alert-triangle"></i></span>
        }
        </Fragment>
    );
};
};

export default Textbox;