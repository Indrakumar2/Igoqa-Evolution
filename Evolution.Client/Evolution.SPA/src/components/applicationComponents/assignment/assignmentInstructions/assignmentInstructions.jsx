import React, { Component, Fragment } from 'react'; 
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler } from '../../../../utils/commonUtils';
import CustomModal from '../../../../common/baseComponents/customModal';
import { isOperator } from '../../../../selectors/assignmentSelector';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

class AssignmentInstructions extends Component {
    constructor(props){
        super(props);
        this.updatedData={};
        this.state ={
        };
    }

    onChangeHandler=(e)=>{
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE){
            this.updatedData['recordStatus'] = "N";
            this.updatedData.assignmentId =this.props.assignmentId;
            this.props.actions.AssignmentInstructionsChange(this.updatedData);
            this.updatedData = {};
        }

        else if(this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
            this.updatedData['recordStatus'] = "M";
            this.updatedData.assignmentId =this.props.assignmentId;
            this.props.actions.AssignmentInstructionsChange(this.updatedData);
            this.updatedData = {};
        }
    }
  
    render() {
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const { assignmentInstruction,interactionMode } = this.props;
        const isOCLogged = this.props.isInterCompanyAssignment  && this.props.isOperatorCompany;
        return (
           <Fragment>
                <CustomModal modalData={modelData} />
                 <div className="genralDetailContainer customCard">
                     <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.ASSIGNMENT_INSTRUCTIONS} colSize="s12">
                        <div className="row">
                            <div className="col s12 m6 l12 instruction">
                                <CustomInput 
                                    hasLabel={true}
                                    label={localConstant.assignments.INTER_COMPANY_INSTRUCTIONS}
                                    divClassName='col'
                                    type='textarea'
                                    colSize='s12'
                                    inputClass="customInputs"
                                    labelClass="customLabel"
                                    name="interCompanyInstructions"
                                    id="companyInstructionId"
                                    onValueChange={(e)=>this.onChangeHandler(e)}
                                    //maxLength={fieldLengthConstants.assignment.assignmentInstructions.INTER_COMPANY_INSTRUCTIONS_MAXLENGTH}
                                    value={assignmentInstruction.interCompanyInstructions}
                                    // disabled={interactionMode || isOCLogged}
                                    readOnly = {interactionMode || isOCLogged}
                                     >
                                </CustomInput>
                                <CustomInput 
                                    hasLabel={true}
                                    label={localConstant.assignments.TECHNICAL_SPEC_INSTRUCTIONS}
                                    divClassName='col'
                                    type='textarea'
                                    colSize='s12'
                                    inputClass="customInputs"
                                    labelClass="customLabel"
                                    name="technicalSpecialistInstructions"
                                    id="techSpecInstructionId"
                                    onValueChange={(e)=>this.onChangeHandler(e)}
                                    //maxLength={fieldLengthConstants.assignment.assignmentInstructions.RESOURCE_INSTRUCTIONS_MAXLENGTH}
                                    value={assignmentInstruction.technicalSpecialistInstructions}
                                    // disabled={interactionMode}
                                    readOnly = {interactionMode}
                                     >
                                </CustomInput>
                            </div>
                        </div>
                     </CardPanel>
                </div>
           </Fragment>
           
        );
    }
}

export default AssignmentInstructions;