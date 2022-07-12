import React,{ Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData,bindAction, isEmpty } from '../../../../utils/commonUtils';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
const localConstant = getlocalizeData();

const ClientNotificationModal = (props) => (
    <div>        
        <div className="row">
                <CustomInput
                    hasLabel={true}
                    type='select'
                    divClassName='col'
                    label={localConstant.modalConstant.CUSTOMER_CONTRACT_NAME}
                    colSize='s6'
                    optionsList={props.contractCustomer}
                    optionName='contactPersonName'
                    optionValue="contactPersonName"
                    labelClass="mandate"
                    name="customerContact"
                    className="browser-default customInputs"
                    onSelectChange={props.handlerChange}
                    interactionMode={props.interactionMode}
                    defaultValue={props.eidtedClientNotification && props.eidtedClientNotification.customerContact}
                />
            </div>
            <div className="row">
                <CustomInput
                    type='switch'
                    colSize="s6"
                    switchLabel={localConstant.modalConstant.FLASH_REPORT_NOTIFICATION}
                    isSwitchLabel={true}
                    labelClass = 'col s12'
                    switchSpanClass="switchAlign"
                    switchName="isSendFlashReportingNotification"
                    id="isSendFlashReportingNotification"
                    className="lever"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={false}
                    interactionMode={props.interactionMode}
                    checkedStatus={props.eidtedClientNotification && props.eidtedClientNotification.isSendFlashReportingNotification}
                />    
                 <CustomInput
                    type='switch'
                    colSize="s6"
                    switchLabel={localConstant.modalConstant.CUSTOMER_REPORTING_NOTIFICATION}
                    isSwitchLabel={true}
                    labelClass = 'col s12'
                    switchSpanClass="switchAlign"
                    switchName="isSendCustomerReportingNotification"
                    id="isSendCustomerReportingNotification"
                    className="lever"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={false}
                    interactionMode={props.interactionMode}
                    checkedStatus={props.eidtedClientNotification && props.eidtedClientNotification.isSendCustomerReportingNotification}
                />
            </div>
            <div className="row"> 
                <CustomInput
                    type='switch'
                    colSize="s6"
                    switchLabel={localConstant.modalConstant.INSPECTION_RELEASE_NOTES}
                    isSwitchLabel={true}
                    labelClass = 'col s12'
                    switchSpanClass="switchAlign"
                    switchName="isSendInspectionReleaseNotesNotification"
                    id="isSendInspectionReleaseNotesNotification"
                    className="lever"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={false}
                    interactionMode={props.interactionMode}
                    checkedStatus={props.eidtedClientNotification && props.eidtedClientNotification.isSendInspectionReleaseNotesNotification}
                />  
                <CustomInput
                    type='switch'
                    colSize="s6"
                    switchLabel={localConstant.modalConstant.NCR_REPORTING}
                    isSwitchLabel={true}
                    labelClass = 'col s12'
                    switchSpanClass="switchAlign"
                    switchName="isSendNCRReportingNotification"
                    id="isSendNCRReportingNotification"
                    className="lever"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={false}
                    interactionMode={props.interactionMode}
                    checkedStatus={props.eidtedClientNotification && props.eidtedClientNotification.isSendNCRReportingNotification}
                /> 
            </div>
            <div className="row">  
                <CustomInput
                    type='switch'
                    colSize="s6"
                    switchLabel={localConstant.modalConstant.CUSTOMER_DIRECT_REPORTING}
                    isSwitchLabel={true}
                    labelClass = 'col s12'
                    switchSpanClass="switchAlign"
                    switchName="isSendCustomerDirectReportingNotification"
                    id="isSendCustomerDirectReportingNotification"
                    className="lever"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={false}
                    interactionMode={props.interactionMode}
                    checkedStatus={props.eidtedClientNotification && props.eidtedClientNotification.isSendCustomerDirectReportingNotification}
                />  
            </div>
    </div>
);

class ClientNotification extends Component{
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isOpen: false,
            isClientNotificationOpen:false,
            selectedRowData:{},
            isClientNotificationEditMode:false
        };
        
    }    

    handlerChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    cancelClientNotification = (e) => {
        e.preventDefault();
        this.setState({
            isClientNotificationOpen:false
        });
        this.updatedData = {};
    }
    addClientNotificationHandler = () => {
        this.setState({
            isClientNotificationOpen:true,
            isClientNotificationEditMode:false
        });
        this.editedRowData={};
    }
    addNewClientNotification = (e) => {
        e.preventDefault();
        this.updatedData["isSendFlashReportingNotification"] = this.updatedData.isSendFlashReportingNotification ? true : false;
        this.updatedData["isSendNCRReportingNotification"] = this.updatedData.isSendNCRReportingNotification? true : false;
        this.updatedData["isSendCustomerReportingNotification"] = this.updatedData.isSendCustomerReportingNotification ? true : false;
        this.updatedData["isSendCustomerDirectReportingNotification"] = this.updatedData.isSendCustomerDirectReportingNotification ? true : false;
        this.updatedData["isSendInspectionReleaseNotesNotification"] = this.updatedData.isSendInspectionReleaseNotesNotification ? true : false;
        this.updatedData["recordStatus"] = "N";
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        this.updatedData["projectClientNotificationId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
        if(!this.updatedData.customerContact){
            IntertekToaster(localConstant.project.PLEASE_SELECT_CUSTOMER,'warningToast selectCustomerContract');
            return false;
        }
        let isExist = false;
        isExist = this.props.clientNotificationGrid.map(iteratedValue => {
            if (iteratedValue.customerContact === this.updatedData.customerContact && iteratedValue.recordStatus !== "D") {
                return !isExist;
            }else{
                return isExist;
            }
        });
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.project.CLIENT_NOTIFICATION_EXIST,'warningToast udpClientNotification');
            return false;
        }

        this.props.actions.AddClientNotification(this.updatedData);
        this.setState({
            isClientNotificationOpen:false
        });
        this.updatedData = {};
        this.editedRowData={};
    }

    editClientNotification = (e) => {
        e.preventDefault();
        let isExist = [];
        if(this.updatedData.customerContact==''){
            IntertekToaster(localConstant.project.PLEASE_SELECT_CUSTOMER,'warningToast selectCustomerContract');
            return false;
        }
        isExist = this.props.clientNotificationGrid.map(iteratedValue => {
            if(iteratedValue.customerContact === this.updatedData.customerContact && iteratedValue.recordStatus !== "D" && iteratedValue.projectClientNotificationId !== this.editedRowData.projectClientNotificationId){
                return true;
            }
        });
        // if(!this.updatedData.customerContact){
        //     isExist = this.props.clientNotificationGrid.map(iteratedValue => {
        //         if(iteratedValue.customerContact === this.editedRowData.customerContact && iteratedValue.recordStatus !== "D"){
        //             return false;
        //         }
        //     });
        // }else{
        //     isExist = this.props.clientNotificationGrid.map(iteratedValue => {
        //        if (iteratedValue.customerContact === this.updatedData.customerContact && iteratedValue.recordStatus !== "D" ) {
        //            return !isExist;
        //        }else{
        //            return isExist;
        //        }
        //    });
        // }
        
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.project.CLIENT_NOTIFICATION_EXIST,'warningToast udpClientNotification');
            return false;
        }else{
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.props.actions.UpdatetNotificationData(this.updatedData,this.editedRowData);
            this.updatedData = {};
            this.setState({
                isClientNotificationOpen:false
            });
        }
    }

    deleteClientNotification = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CLIENT_NOTIFICATION_DELETE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedAssignmentType,
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
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast idOneRecordSelectReq');
        }
    }

    /**
     * Delete the selected records of Attachment Types
     */
    deleteSelectedAssignmentType = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteClientNotification(selectedData);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    editRowHandler=(data)=>{
            this.setState((state)=>{
                return {
                    isClientNotificationOpen: !state.isClientNotificationOpen,
                    isClientNotificationEditMode:true
                };
            });
            this.editedRowData=data;
    }
    componentDidMount = () =>{
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0)
            this.props.actions.FetchCustomerContact();
    }
    render(){ 
        bindAction(HeaderData,"EditColumn",this.editRowHandler);
        const clientNotificationButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelClientNotification,
                btnID: "cancelClientNotification",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isClientNotificationEditMode?this.editClientNotification:this.addNewClientNotification,
                btnID: "editClientNotification",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const clientNotificationGridData = this.props.clientNotificationGrid && this.props.clientNotificationGrid.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        
        return(
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard mt-2">
                    {this.state.isClientNotificationOpen &&
                        <Modal 
                            title={localConstant.project.CLIENT_NOTIFICATION}
                            modalId="clientNotificationModal"
                            formId="clientNotificationModal" 
                            modalClass="popup-position" 
                            buttons={clientNotificationButtons} 
                            isShowModal={this.state.isClientNotificationOpen} 
                            disabled={this.props.interactionMode}>
                            <ClientNotificationModal 
                            handlerChange = { this.handlerChange }
                            eidtedClientNotification = { this.editedRowData }
                            contractCustomer = {this.props.customerContacts.filter(x=> !isEmpty(x.email))} //Only Contacts with email id should be listed
                            />
                        </Modal>}

                <CardPanel className="white lighten-4 black-text col s12" title={localConstant.project.CLIENT_NOTIFICATION} colSize="s12">
              
                 <ReactGrid 
                            gridRowData={clientNotificationGridData}
                            gridColData={HeaderData} 
                            paginationPrefixId={localConstant.paginationPrefixIds.clientNotification}
                            onRef = {ref => { this.child = ref; }}/>
                        {this.props.pageMode !== localConstant.commonConstants.VIEW && 
                        <div className="right-align mt-2 col s12 pr-0">
                            <a href="javascript:void(0);" onClick={this.addClientNotificationHandler} disabled={this.props.interactionMode} className="btn-small waves-effect modal-trigger">
                                {localConstant.commonConstants.ADD}
                            </a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={isEmpty(this.props.clientNotificationGrid) || this.props.interactionMode} onClick={this.deleteClientNotification}>
                                {localConstant.commonConstants.DELETE}
                            </a>
                        </div>}           
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default ClientNotification;