import React, { Component } from 'react';
import { getlocalizeData,ObjectIntoQuerySting } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();

class EditLink extends Component {
    selectedRowHandler = () => {
        this.props.editAction(this.props.data);          
    }
    EnableButtonLink = (data) => {
        //D363 CR change
        if(data.displayLink === localConstant.gridHeader.REASSIGN){
            if(data.data.isDisableMyTaskReassign)
                return "isDisabled";
        } else {
            if(data.data.isDisableMyTaskLink)
                return "isDisabled";
        }
        //D363 CR change
        if (data.displayLink && !data.checkDisable) {
            if(data.disableField && data.disableField() === true){  return "isDisabled"; } //D456 issue
            return null;
        }
        else if (data.pageMode=== localConstant.commonConstants.VIEW || data.interactionMode === true || data.projectInteractionMode === true || data.contractInteractionMode === true || data.disableField && data.disableField() === true) {
            return "isDisabled";
        }
        /*AddScheduletoRF*/
        else if(data.data)
        {
            if(data.data.baseScheduleId)
            return "isDisabled";
            else
            return null;
        }
        else
            return null;
    }

    DisplayLinkName=(data) =>{       
        if(data.displayLink){
            return true;
        } else{
            if(data.data.rating !==undefined && data.data.rating >= 0)
            return "";
        }
    }
   
    render() {      
        const { property,displayLink }= this.props;
        const links={ chargeRate:"chargeRate",payRate:"payRate" };
        const linkToBeDisabled = this.EnableButtonLink(this.props);  
        const linkDisplay=this.DisplayLinkName(this.props);
        return (
            <a className={"waves-effect waves-light " + linkToBeDisabled} onClick={this.selectedRowHandler}
                href="javascript:void(0)" disabled={linkToBeDisabled === 'isDisabled' ? true : false}>
                {/* {(displayLink===links[displayLink] && displayLink !==undefined ) ? this.props.data[displayLink] : (displayLink ? displayLink : (this.props.colDef.field ==='reassign' ? "Reassign": this.props.colDef.field === 'approve' ? "Approve" : this.props.colDef.field === 'reject' ? "Reject" : this.props.colDef.field === 'EditCommodityDetails' && this.props.data.rating ? "" : localConstant.commonConstants.EDIT))} */}
                {(displayLink===links[displayLink] && displayLink !==undefined ) ? this.props.data[displayLink] : (linkDisplay !==undefined ? displayLink : localConstant.commonConstants.EDIT)}
                </a>
        );
    }
}
export default EditLink;
