import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './techSpecMytaskHeader';
import { bindAction, getlocalizeData, isEmptyReturnDefault,formInputChangeHandler,isEmpty } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import ArsSearch from '../../../../components/applicationComponents/assignment/arsSearch';
import Modal from '../../../../common/baseComponents/modal';
import { applicationConstants } from '../../../../constants/appConstants';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getUserTypesJsonArray } from '../../../../utils/jsonUtil';

const localConstant = getlocalizeData();

class TechSpecMyTask extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            isDashboardARSView:true,
            isAssignTask:false,
            selectedData:'',            
        };
        this.updatedData = {};
        this.headerData = HeaderData();
        bindAction(this.headerData.myTask, "reassign", this.editRowHandler);        
    }
        
    componentDidMount(){  
       this.props.actions.TechSpecClearSearch(); 
      // this.props.actions.grmSearchMatsterData();   
       this.props.actions.FetchTechSpecMytaskData();
      // this.props.actions.FetchMyTaskAssignUsers();
    }
    
    editRowHandler = (data) => {
        this.props.actions.FetchMyTaskAssignUsers();
        if(data.taskType === localConstant.commonConstants.RESOURCE_TO_UPDATE_PROFILE){//Added for D761 CR
            this.reassignMyTask(data, data.assignedTo);//Skipping Coordinator select
        }
        else {
            this.updatedData = data;
            this.setState({
                isAssignTask: true            
            }); 
        }
    }
    
    cancelAssignTask = ()=>{
        this.setState({
            isAssignTask:false,
            selectedData:''
        });
        this.updatedData = {}; 
    }

    reassignMyTask = (editedData, selectedRecords) => {                
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.REASSIGN_MY_TASK,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,                    
                    onClickHandler: (e) => this.reassignSelectedMyTask(editedData, selectedRecords),
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: localConstant.commonConstants.NO,
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };        
        this.props.actions.DisplayModal(confirmationObject);
    }

    reassignSelectedMyTask = (editedData, selectedRecords) => {  
        const editedRow = [];              
        if(editedData.taskType !== localConstant.commonConstants.RESOURCE_TO_UPDATE_PROFILE){//Added for D761 CR
            editedData["assignedBy"] = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
            editedData["assignedTo"] = (Array.isArray(selectedRecords) && selectedRecords.length > 0) ? selectedRecords[0].userLogonName : "";
        } else{ //Added for D946 CR Reassign
            editedData["assignedBy"] = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
            editedData["assignedTo"] = null; //UAT 07-08Dec20 Doc Ref: Resource #2 Issue
        }
        editedData["recordStatus"] = "M";            
        editedRow.push(editedData);        
        this.props.actions.UpdateMyTaskReassign(editedRow);
        this.confirmationRejectHandler();
        this.props.actions.Dashboardrefresh(this.props.location.pathname);
        this.props.actions.FetchDashboardCount();
    }

    confirmationRejectHandler = () => {        
        this.updatedData = {};
        this.props.actions.HideModal();
    }

    submitAssignTask = (e) => {
        e.preventDefault();
        if(isEmpty(this.state.selectedData)){
            IntertekToaster(localConstant.validationMessage.MYTASKWORKFLOWSELECT , 'warningToast submitAssignTask');
            return false;            
        }
        const selectedRecords = isEmptyReturnDefault(this.AssignTaskGrid.getSelectedRows());        
        if (selectedRecords && selectedRecords.length > 0) {
            const editedData = this.updatedData;
            this.cancelAssignTask();            
            this.reassignMyTask(editedData, selectedRecords);            
        } else {
            IntertekToaster(localConstant.validationMessage.MYTASKUSERSELECT, 'warningToast submitAssignTask');            
        }        
    }

    OnChangeHandler=(e)=>{
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.setState({
            selectedData:inputvalue.value
        });
    }

    render() {     
        let userTypes=getUserTypesJsonArray();
        let techSpecAssignUser=[];
        if(!isEmpty(this.state.selectedData)){
            techSpecAssignUser=this.props.techSpecAssignUser.filter(x => x.userType === this.state.selectedData); //Changes for ITK Defect 908(Ref ALM Document 14-05-2020)
        }  
        userTypes=userTypes.filter(x => x.value !== localConstant.userTypeList.TechnicalSpecialist && x.name !== localConstant.userTypeList.Customer && x.name !== localConstant.userTypeList.Coordinator && x.value !== localConstant.userTypeList.SeniorCoordinator); //D908(Ref ALM 11-06-2020)
        return (
            <Fragment>
                <CustomModal/>{}
                {this.state.isAssignTask ?
                    <Modal title="Assign Task"
                        modalClass="myTaskAssignUsersModal"
                        modalId="AssignTask"
                        formId="AssignTask"
                        buttons={
                            [ {
                                name: localConstant.commonConstants.CANCEL,
                                action: this.cancelAssignTask,
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                                type: "button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action:(e)=>this.submitAssignTask(e),
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                            } ]
                        }
                        isShowModal={true}>
                        <div className="customCard left">
                        <div className='row'>
                        <CustomInput 
                            hasLabel={true}
                            name='userType'
                            divClassName='col'
                            colSize='s6'
                            label={localConstant.security.WORK_FLOW_TYPE}
                            labelClass="mandate"
                            type='select'
                            inputClass="customInputs"
                            optionsList={userTypes}
                            optionName="name"
                            optionValue="value"
                            onSelectChange={this.OnChangeHandler}
                        />
                        </div>
                        <div>
                            <ReactGrid
                                gridRowData={techSpecAssignUser}
                                gridColData={this.headerData.assignTask}
                                onRef={ref => { this.AssignTaskGrid = ref; }}
                                paginationPrefixId={localConstant.paginationPrefixIds.dashboardMyTaskOneGridId}
                            />
                        </div>
                        </div>
                    </Modal> : null}
			    <ReactGrid gridRowData={this.props.techSpecMytaskData} gridColData={this.headerData.myTask } paginationPrefixId={localConstant.paginationPrefixIds.dashboardMyTaskTwoGridId}/>
                {this.props.isARSSearch && <ArsSearch isShowARSModal={this.props.isARSSearch} isDashboardARSView={this.state.isDashboardARSView}/> }                
            </Fragment>      
        );
    }
}

export default TechSpecMyTask;
