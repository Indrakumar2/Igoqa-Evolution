import React,{ Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import CustomInput from '../../../../common/baseComponents/inputControlls'; 
import { AdminRateScheduleSelect } from '../../../../actions/contracts/rateScheduleAction';

class SelectChargeValue extends Component{

    onSelectChange = (e) =>{
        this.props.AdminRateScheduleSelect(this.props.data,e.target.value);
    }

    render(){
        const optionList = [
            { name : 'Rate Onshore Oil', value : 'rateOnShoreOil' },
            { name : 'Rate Onshore' , value : 'rateOnShoreNonOil' },
            { name : 'Rate Offshore Oil' , value : 'rateOffShoreOil' }
        ];
        return(
            <CustomInput
                divClassName='col'
                required={true}
                name="chargeType"
                type='select'
                colSize='s12'
                className="browser-default customInputs"
                optionsList={optionList}
                optionName='name'
                optionValue="value"
                id="chargeTypeId"
                onSelectChange={this.onSelectChange}
            />
        );
    }
}

export default connect(null, {
    AdminRateScheduleSelect
})(SelectChargeValue);