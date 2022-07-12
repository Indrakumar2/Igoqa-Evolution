import React,{ Component,Fragment } from 'react';
import { getlocalizeData,bindAction, isEmpty } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import { StringFormat } from '../../../../utils/stringUtil';
const localConstant = getlocalizeData();

const RateScheduleModal = (props) =>(
    <div className="row">
        <CustomInput
            hasLabel={true}
            type='select'
            divClassName='col'
            optionClassName='chargeTypeOption'
            optionClassArray = {[ true ]}
            optionProperty = "expired"
            label={localConstant.gridHeader.CONTRACT_SCHEDULE_NAME}
            colSize='s6'
            optionsList={props.rateScheduleNames}
            optionName='scheduleName'
            optionValue="scheduleId"
            labelClass="mandate"
            name="contractScheduleName"
            id="contractScheduleId"
            className="browser-default customInputs"
            onSelectChange={props.handlerChange}
            defaultValue={props.eidtedRateSchedule && parseInt(props.eidtedRateSchedule.contractScheduleId)}
        />
    </div>
);

class ContractRateSchedule extends Component{
    constructor(props){
        super(props);
        this.updatedData = {};         
        this.state = {
            isOpen: false,
            isRateScheduleModalOpen:false,
            isRateScheduleEditMode:false,            
            selectedRowData:{}
        };
        const functionRefs = {};
        this.selectValue = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);        
    }    
    addRateSchedule = () => {
        this.setState({
            isRateScheduleModalOpen:true,
            isRateScheduleEditMode:false
        });
        this.editedRowData={};
    }
    cancelRateSchedule = (e) => {
        e.preventDefault();
        this.setState({
            isRateScheduleModalOpen:false
        });
    }
    
    handlerChange = (e) => {
        const result = this.inputChangeHandler(e); 
        if (e.target.tagName === "SELECT") {
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            this.updatedData[e.target.id] = e.nativeEvent.target.value;
            if(e.target.name === 'contractScheduleName'){
              this.selectValue["contractScheduleName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;                
            }             
        }
        else{
        this.updatedData[e.target.name] = result;
        }
    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    editRowHandler=(data)=>{
        this.setState((state)=>{
            return {
                isRateScheduleModalOpen: true,
                isRateScheduleEditMode:true
            };
        });
        this.editedRowData=data;
    }

    // Handler to check duplicate schedules - return schedule name;
    duplicateScheduleCheck = (data, isEditMode) => {
        const scheduleList = this.props.rateScheduleDataGrid;
        if(data){
            if(isEditMode){
                const uniqueKey = data.recordStatus !== 'N' ? "assignmentContractRateScheduleId" : "assignmentContractRateAddUniqueId";
                const duplicateScheules = scheduleList.filter(x => x.recordStatus !== 'D' && x.contractScheduleId == data.contractScheduleId && x[uniqueKey] != data[uniqueKey]);
                return duplicateScheules.length > 0 ? duplicateScheules[0].contractScheduleName : "";
            } else {
                const duplicateScheules = scheduleList.filter(x => x.recordStatus !== "D" && x.contractScheduleId == data.contractScheduleId);
                return duplicateScheules.length > 0 ? duplicateScheules[0].contractScheduleName : "";
            }
        }
        return "";
    }

    submitNewRateSchedule = (e) =>{       
        e.preventDefault();
        this.updatedData["assignmentContractRateScheduleId"] = null;
        this.updatedData["assignmentContractRateAddUniqueId"] = Math.floor(Math.random() * 99) -100;
        this.updatedData["assignmentId"] = this.props.assignmentId;
        this.updatedData["recordStatus"] = 'N';
        this.updatedData["modifiedBy"] = this.props.loginUser;
         
        if(this.updatedData.contractScheduleId === '' || this.updatedData.contractScheduleId === undefined){
            IntertekToaster(localConstant.assignments.PLEASE_CONTRACT_SCHEDULE_NAME,'warningToast selectCustomerContract');
            return false;
        }
        //While submitting we check duplicate values are existing or not.
        const duplicateScheduleName = this.duplicateScheduleCheck(this.updatedData, false);
        if(duplicateScheduleName){
            const errorMessage =StringFormat(localConstant.assignments.CONTRACT_SCHEDULE_NAME_ALREADY_EXISTS, duplicateScheduleName); 
            IntertekToaster(errorMessage, 'warningToast duplicateScheduleName');
            return false;
        }
        this.props.actions.AddNewRateSchedule(this.updatedData);
        this.setState({
            isRateScheduleModalOpen:false
        });
        this.updatedData = {};
    }
    editContractRateSchedule = (e) => {
        e.preventDefault();
        if(this.updatedData.contractScheduleId === ''){
            IntertekToaster(localConstant.assignments.PLEASE_CONTRACT_SCHEDULE_NAME,'warningToast selectCustomerContract');
            return false;
        }

        const updatedSchedule = Object.assign({}, this.editedRowData, this.updatedData);
        const duplicateScheduleName = this.duplicateScheduleCheck(updatedSchedule, true);
        if(this.updatedData.contractScheduleId && (this.updatedData.contractScheduleId != this.editedRowData.contractScheduleId) && !this.validateContractSchedule([ this.editedRowData ])){
            IntertekToaster(localConstant.validationMessage.CONTRACT_SCHEDULE_LINKED_EDIT, 'warningToast udpRateScheduleLinked');
            return false;
        }        
        else if (duplicateScheduleName) {
            const errorMessage = StringFormat(localConstant.assignments.CONTRACT_SCHEDULE_NAME_ALREADY_EXISTS, duplicateScheduleName); 
            IntertekToaster(errorMessage,'warningToast udpRateSchedule');
            return false;
        }
        else{
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loginUser;
            this.props.actions.UpdatetRateSchedule(this.updatedData,this.editedRowData);
            this.updatedData = {};
            this.setState({
                isRateScheduleModalOpen:false
            });
        }
    }

    deleteRateSchedules = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const value= this.validateContractSchedule(selectedRecords);
            if (value) {
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.RATE_SCHEDULE_MAESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: localConstant.commonConstants.YES,
                            onClickHandler: this.deleteSelectedRateSchedule,
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
            else 
            IntertekToaster(localConstant.validationMessage.CONTRACT_SCHEDULE_LINKED, "warningToast AssignmentContractSchedules");             
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
        }  
    }
    validateContractSchedule = (selectedRecords) => {
        const assignedSpecialists = this.props.assignmentTechSpecialists.filter(x =>x.recordStatus !== 'D');
        let contractSchedulesLinked = [];
        if (assignedSpecialists.length > 0) {
            for(let i=0 ;i< assignedSpecialists.length ; i++)
            {
                const techSpec = assignedSpecialists[i];
                const schedule = techSpec.assignmentTechnicalSpecialistSchedules && techSpec.assignmentTechnicalSpecialistSchedules.filter(x => x.recordStatus !== "D");
                if (schedule && schedule.length > 0) {
                    contractSchedulesLinked= selectedRecords.filter(x => (schedule.some(x1 => x1.contractScheduleName === x.contractScheduleName)));
                    if(contractSchedulesLinked.length > 0)
                    break;
                }
            }
            if(contractSchedulesLinked.length > 0)
            return false;
        }
        return true;
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    deleteSelectedRateSchedule = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteRateSchedule(selectedData);
        this.props.actions.HideModal();
    }
    componentDidMount = () => {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchContractScheduleName();
        }
        else{
            this.props.actions.RateScheduleExpiryCheck(this.props.rateScheduleNames,this.props.contractRates);
        }
    }
    enableEditColumn = (e) => {
        return this.props.isInterCompanyAssignment && this.props.isOperatingCompany ? true :false;
    }
    render(){
        const { interactionMode } = this.props;
        bindAction(this.headerData,"EditColumn",this.editRowHandler);

        // TODO: Move this button constant to constructor.
        const rateScheduleButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelRateSchedule,
                btnID: "cancelRateSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                // TODO: Combine both editContractRateSchedule and submitNewRateSchedule handlers in to 
                //     one with separating it with a check this.state.isRateScheduleEditMode
                action: this.state.isRateScheduleEditMode?this.editContractRateSchedule:this.submitNewRateSchedule,
                btnID: "editClientNotification",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const rateScheduleDataGrid = this.props.rateScheduleDataGrid && this.props.rateScheduleDataGrid.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });

        // TODO : Object destructuring for all props which is used inside the render()
        const isInterCompanyAssignment=this.props.isInterCompanyAssignment;
        
        return(
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    {this.state.isRateScheduleModalOpen &&
                        <Modal 
                            title={localConstant.assignments.CONTRACT_RATE_SCHEDULES}
                            modalId="classificaionModal"
                            formId="classificaionModal" 
                            modalClass="popup-position" 
                            buttons={rateScheduleButtons} 
                            isShowModal={this.state.isRateScheduleModalOpen} 
                            disabled={interactionMode}>
                            <RateScheduleModal 
                                handlerChange = { this.handlerChange }
                                eidtedRateSchedule = { this.editedRowData }
                                isEditMode = { this.state.isRateScheduleEditMode }
                                rateScheduleNames = { this.props.rateScheduleNames }
                                
                            />
                        </Modal>}
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.CONTRACT_RATE_SCHEDULES} colSize="s12">
                            <ReactGrid
                                gridRowData={rateScheduleDataGrid}
                                gridColData={this.headerData}
                                onRef={ref => { this.child = ref; }}
                                paginationPrefixId={localConstant.paginationPrefixIds.assignmentCSchdl} />
                     
                        {this.props.pageMode !== localConstant.commonConstants.VIEW  && <div className="right-align mt-2 mr-2">
                            <a onClick={this.addRateSchedule}
                                disabled={(isInterCompanyAssignment && this.props.isOperatingCompany) ? true : false}
                                className="btn-small waves-effect modal-trigger">
                                {localConstant.commonConstants.ADD}
                            </a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                                disabled={(isInterCompanyAssignment && this.props.isOperatingCompany) ? true : false}
                                onClick={this.deleteRateSchedules}>
                                {localConstant.commonConstants.DELETE}
                            </a>
                        </div>}
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default ContractRateSchedule;