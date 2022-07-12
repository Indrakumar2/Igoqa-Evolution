import React, { Component,Fragment } from 'react';
import CustomInput from '../../../baseComponents/inputControlls'; 

class Select extends Component {
    selectedRowHandler = (e) => {  
        this.props.editAction(this.props.data,e);    
    };
    render() { 
        const defaultValue = this.props.data && this.props.data[this.props.name];
        const rowIndex = this.props && this.props.rowIndex;
        const optionListData = this.props.optionListProps && this.props.data && this.props.data[this.props.optionListProps];
        const disabled= this.props.data[this.props.isDisabled];
    return (
        <Fragment>
        <CustomInput
            divClassName='col'
            required={true}
            name={this.props.name}
            type='select'
            //To set default value as select or not
            selectstatus = { this.props.isWithoutSelect }
            colSize='s12'
            className="browser-default customInputs"
            optionsList={this.props.isAssignmentReference ? this.props.assignmentReferenceTypes : this.props.optionList || optionListData }
            optionName={this.props.optionName}
            optionValue={this.props.optionValue}
            htmlFor={this.props.name + rowIndex}
            id={this.props.name + rowIndex}
            disabled={this.props.disabled ||  disabled }
            onSelectChange={this.selectedRowHandler}
            defaultValue={defaultValue}
        />
        { 
            this.props.validationProp && this.props.data[this.props.validationProp] &&
            <span class="text-red" title={this.props.data[this.props.validationProp]}><i class="zmdi zmdi-alert-triangle"></i></span>
        }
        </Fragment>
    );
};
};

export default Select;