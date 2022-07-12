import React,{ Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import CustomInput from '../../../../common/baseComponents/inputControlls'; 
import { ApproveRejectOverride } from '../../../../actions/assignment/arsSearchAction';
import { isBoolean } from '../../../../utils/commonUtils';

class OverrideApprovalLink extends Component{

    onSelectChange = (e) =>{
        this.props.ApproveRejectOverride(this.props.data,e.target.value);
    }

    render(){
        const optionList = [
            { name : 'Approve', value : 'Approve' },
            { name : 'Reject' , value : 'Reject' },
        ];
        const rowData = this.props.data;
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
                defaultValue = { isBoolean(rowData.isApproved) ? rowData.isApproved ?'Approve' : 'Reject' : ""}
                disabled = { rowData.recordStatus === null && isBoolean(rowData.isApproved) ? true : false} //Changes for D1326
                onSelectChange={this.onSelectChange}
            />
        );
    }
}

export default connect(null, {
    ApproveRejectOverride
})(OverrideApprovalLink);