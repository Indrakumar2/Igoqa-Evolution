/** Inline editing switch component as common. */
import React, { Component , Fragment } from 'react';
import PropTypes from 'prop-types';
import CustomInput from '../../inputControlls';
import { required } from '../../../../utils/validator';

class Switch extends Component{

    onChangeHandler = (e) => {
        this.props.editAction(e,this.props.data);
    };

    render(){
        const { labelName, invertSwitchValue, isSwitchLabel, name, id, switchClass, disabled, data ,isDisabled } = this.props;
        const defaultValue = invertSwitchValue ? !data[name] : data[name];
        const isDisable=data[isDisabled];
        //Defect Id 648 
        let isStatus = true;
        if(!required(this.props.colDef.documentModuleName) && !required(this.props.colDef.moduleType) 
            && this.props.colDef.documentModuleName === this.props.colDef.moduleType){            
                if(this.props.data.isDisableStatus && this.props.data.recordStatus == null ){
                    isStatus = true;
                }
                else if(this.props.data.isDisableStatus && this.props.data.recordStatus !== null ){
                    isStatus = false;
                }else if(!this.props.data.isDisableStatus && !disabled ){
                    isStatus = false;
                }        
        } else{
            isStatus = isDisable ? isDisable : disabled;
        }  
        return(

            <Fragment>
            <CustomInput
                type='switch'
                switchLabel= { labelName }
                isSwitchLabel={ isSwitchLabel }
                switchName={ name }
                id={ id }
                colSize={ switchClass }
                //disabled={ disabled || isDisable }
                disabled={isStatus }
                checkedStatus={ defaultValue ? true : false }
                onChangeToggle={ this.onChangeHandler }
            />
            { 
                this.props.validationProp && this.props.data[this.props.validationProp] &&
                <span class="text-red" title={this.props.data[this.props.validationProp]}><i class="zmdi zmdi-alert-triangle"></i></span>
            }
            </Fragment>
        );
    };
};

export default Switch;

Switch.propTypes = {
    labelName:PropTypes.string,
    isSwitchLabel:PropTypes.bool,
    name:PropTypes.string,
    id:PropTypes.string,
    switchClass:PropTypes.string,
    disabled:PropTypes.bool,
    data:PropTypes.object,
};

Switch.defaultProps = {
    labelName:"",
    isSwitchLabel:false,
    name:"",
    id:"",
    switchClass:"",
    disabled:false,
    data:{}
};