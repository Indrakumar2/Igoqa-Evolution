import React,{ Component } from 'react';
import CustomInput from '../baseComponents/inputControlls'; 
import { getlocalizeData } from '../../utils/commonUtils';
import { applicationConstants } from '../../constants/appConstants';
const localConstant = getlocalizeData();
class SelectDocumentValue extends Component{
    constructor(props) {
        super(props);
        this.updatedData = {};
    }
    
     /*Input Change Handler*/
     inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }   
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e) => {   
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if (this.props.data.recordStatus !== 'N')
            this.updatedData["recordStatus"] = "M"; 
        this.props[this.props.colDef.documentModuleName].map(doc => {
            if (doc.name === result.value) {
                this.updatedData["isVisibleToTS"] = doc.isTSVisible;
            }           
        });

        //Defect ID 702 #10 - customer visible switch can Enable while document Upload 
        // As per discussion with Sumit and Bhavithira, Documnt Type like 'Non Conformance Report','Report - Flash','Release Note' Based customer Visible Switch Enabled only Visit and Timesheet module.
        if(this.props.colDef.documentModuleName === this.props.colDef.moduleType){
        this.updatedData["isVisibleToCustomer"] = (applicationConstants.customerVisibleDocType.filter( x=> { return x===result.value; }).length>0);
        }else{
            this.updatedData["isVisibleToCustomer"]=false;
        }
        this.props.actions[this.props.colDef.documentModuleName](this.updatedData,this.props.data);
    }

    render(){        
        let isStatus = true;        
        // Defect Id 948 isDisableStatus  - when the visit is Approved and existing  Upload Document Document Type option should be disabled
        if(this.props.colDef.documentModuleName === this.props.colDef.moduleType){            
            if(this.props.data.roleBase && this.props.data.isDisableStatus && this.props.data.recordStatus == null ){
                 isStatus = true;
            }else if(!this.props.data.roleBase && this.props.data.isDisableStatus && this.props.data.recordStatus == null){
                 isStatus = true;
            }else if(!this.props.data.roleBase && this.props.data.isDisableStatus && this.props.data.recordStatus !== null ){
                 isStatus = false;
            }else if(!this.props.data.roleBase && !this.props.data.isDisableStatus){
                 isStatus = false;
            }        
        } else{
            isStatus = this.props.data.roleBase;  //Changes for D-669 
        }  
        return(
            <CustomInput
            hasLabel={false}
            label={localConstant.companyDetails.Documents.SELECT_FILE_TYPE}
            divClassName='s12'
            type='select'
            labelClass="mandate"
            colSize="s12"
            required={true}
            selected={true}
            className="customInputs browser-default"
            optionsList={this.props[this.props.colDef.documentModuleName]}
            optionName='name'
            optionValue="name"
            onSelectChange={(e) => this.formInputChangeHandler(e)}
            name="documentType"
            id="documentType"
            defaultValue={this.props.value}
            //disabled={this.props.data.roleBase}
            disabled={isStatus}  // Defect Id 948 isDisableStatus
            />
        );
    }
}

export default SelectDocumentValue;