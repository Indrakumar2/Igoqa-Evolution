import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';

import { HeaderData } from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import SecondReactGrid from '../../../../common/baseComponents/reactAgGridTwo';
import AssignmentRefModal from './assignmentModal/assignmentRefModal';
import AccountRefModal from './assignmentModal/accountRefModal';
import AccountReference from './accountReference';
import AssignmentReference from './assignmentReference';

import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';

const localConstant = getlocalizeData();

class Assignment extends Component {

    constructor(props) {
        super(props);
        this.updatedData = {};
        this.selectedAssignmentReferenceType = '';
        this.state = {
            isOpen: false,
            showRefOverlay:false,
            showAccntOverlay:false,
        };
        this.refModal= React.createRef();
        this.accntModal=React.createRef();
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
    }

    componentDidMount() {
        /**
         * FetchAssignmentRefTypes api call is commented, Now we are getting reference types from master data
         */
        //this.props.actions.FetchAssignmentRefTypes();

        const tab = document.querySelectorAll('.tabs');
        const tabInstances = MaterializeComponent.Tabs.init(tab);
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select);

        const datePicker = document.querySelectorAll('.datepicker');
        const datePickerInstances = MaterializeComponent.Datepicker.init(datePicker);
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });
    }

    inputChangeHandler = (e) => {
        this.updatedData[e.target.name] = e.target.value;
    }

    selectChangeHandler = (e) => {
        this.updatedData[e.target.name] = e.target.value;
    }

    clearAssignmentReference = () => {
        this.refModal.current.style.cssText="display:none";
        this.setState( { showRefOverlay:false } );
        document.getElementById("addAssignmentReferenceType").reset();
        this.updatedData = {};
        this.props.actions.ShowButtonHandler();
    }
    showAssignmentReference=()=>
    {
        this.refModal.current.style.cssText="z-index: 1100;display:block";
        this.setState( { showRefOverlay:true } );
    }
    showAccountReference =() =>
    {
        this.accntModal.current.style.cssText="z-index: 1100;display:block";
        this.setState( { showAccntOverlay:true } );
    }
    clearAccountReference = () => {
        this.accntModal.current.style.cssText="display:none";
        this.setState( { showAccntOverlay:false } );
        document.getElementById("addAccountReference").reset();
        this.updatedData = {};
        this.props.actions.ShowButtonHandler();
    }

    confirmationRejectHandler = () => {
        // this.setState({ isOpen: false })
        this.props.actions.HideModal();
    }

    deleteAssignmentReference = () => {
        const selectedData = this.child.getSelectedRows();

        if (selectedData.length > 0) {
            this.child.removeSelectedRows(selectedData);
            this.props.actions.DeleteAssignmentReference(selectedData);
            //this.setState({isOpen:false})
            this.props.actions.HideModal();
        }
    }

    assignmentReferenceDeleteHandler = () => {
        const selectedData = this.child.getSelectedRows();
        if (selectedData.length === 0) {
            IntertekToaster(localConstant.customer.SELECT_ASSIGNMENT_REFERENCES_TO_DELETE, 'warningToast CustAssignRefDel');
        }
        else {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ASSIGNMENT_REFERENCE_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteAssignmentReference,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            // this.confirmationModalData = confirmationObject;
            // this.setState({ isOpen: true })
            this.props.actions.DisplayModal(confirmationObject);
        }
    }

    accountReferenceDeleteHandler = () => {
        const selectedData = this.secondChild.getSelectedRows();
        if (selectedData.length === 0) {
            IntertekToaster(localConstant.customer.SELECT_ACCOUNT_REFERENCES_TO_DELETE, 'warningToast CustAcctRefDel');
        }
        else {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ACCOUNT_REFERENCE_DELETE_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteAccountReference,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            // this.confirmationModalData = confirmationObject;
            // this.setState({ isOpen: true })
            this.props.actions.DisplayModal(confirmationObject);
        }
    }

    deleteAccountReference = () => {
        const selectedData = this.secondChild.getSelectedRows();

        if (selectedData.length > 0) {
            this.secondChild.removeSelectedRows(selectedData);
            this.props.actions.DeleteAccountReference(selectedData);
            // this.setState({isOpen:false})
            this.props.actions.HideModal();
        }

    }

    assignmentRefSubmitHandler = (e) => {
        e.preventDefault();
        let alreadySelected = false;
        if (this.props.showButton === true) {
            const selectedAssignmentRefType = this.props.editedAssignmentReference.assignmentRefType;
            if (this.updatedData.assignmentRefType === "") {
                IntertekToaster(localConstant.customer.SELECT_ANY_ONE_ASSIGNMENT_REFERENCE_TYPE, 'warningToast CustAsignRefTypSel');
            }
            else {
                if (this.props.customerAssignmentData) {
                    this.props.customerAssignmentData.map(result => {
                        if (result.assignmentRefType === this.updatedData.assignmentRefType && result.assignmentRefType !== selectedAssignmentRefType) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                            else {
                                this.updatedData = result;
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.customer.ASSIGNMENT_REFERENCE_TYPE_ALREADY_SELECTED, 'warningToast CustAsignRefTypSelDupChk');
                }
                else {
                    if (this.props.editedAssignmentReference.recordStatus !== "N" && this.updatedData.recordStatus !== 'D') {
                        this.updatedData["recordStatus"] = "M";
                        this.updatedData["modifiedBy"] = this.props.loginUser;
                        this.props.actions.UpdateAssignmentReference(this.updatedData);
                    }
                    else if (this.props.editedAssignmentReference.recordStatus === "N") {
                        this.props.actions.UpdateAssignmentReference(this.updatedData);
                    }
                    else {
                        const editAssignmentArray = [ this.props.editedAssignmentReference ];
                        this.props.actions.DeleteAssignmentReference(editAssignmentArray);
                        this.updatedData["recordStatus"] = null;
                        this.props.actions.UpdateAssignmentReference(this.updatedData);
                    }

                    this.updatedData = {};
                    document.getElementById("addAssignmentReferenceType").reset();
                    document.getElementById("cancelAssignmentReference").click();
                    this.refModal.current.style.cssText="display:none";
                    
                }
            }
        }
        if (this.props.showButton === false) {
            if (this.updatedData.assignmentRefType === undefined || this.updatedData.assignmentRefType === "") {
                IntertekToaster(localConstant.customer.SELECT_ANY_ONE_ASSIGNMENT_REFERENCE_TYPE, 'warningToast CustAsignRefTypSelValidChk');
            }
            else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loginUser;
                if (this.props.customerAssignmentData) {
                    this.props.customerAssignmentData.map(result => {
                        if (result.assignmentRefType === this.updatedData.assignmentRefType) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                            else {
                                this.updatedData = result;
                                this.updatedData["recordStatus"] = null;
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.customer.ASSIGNMENT_REFERENCE_TYPE_ALREADY_SELECTED, 'warningToast CustAsignRefTypSelValidDupChk');
                }
                else {
                    if (this.updatedData.recordStatus === "N") {
                        this.updatedData["customerAssignmentReferenceId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                        this.props.actions.AddAssignmentReference(this.updatedData);
                    }
                    else {
                        this.props.actions.UpdateAssignmentReference(this.updatedData);
                    }
                    this.updatedData = {};
                    document.getElementById("addAssignmentReferenceType").reset();
                    document.getElementById("cancelAssignmentReference").click();
                    this.refModal.current.style.cssText="display:none";
                }
            }
        }
    }

        /** Account Reference Validation Handler */
    accountReferenceValidation = (data) => {
        //check AccountReference value is empty
        const accountRefValue = document.getElementById('AccountReference').value.trim();
        if(isEmpty(data) || required(data.companyCode)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_A_COMPANY,"warningToast companyCodeVal");
            return false;
        }
        if(accountRefValue===""){
            IntertekToaster(localConstant.validationMessage.ACCOUNT_REFERENCE_VALIDATION,"warningToast accountRefValueVal");
            return false;
        } 
        if(required(data.accountReferenceValue)){
            IntertekToaster(localConstant.validationMessage.ACCOUNT_REFERENCE_VALIDATION,"warningToast accountRefValueVal");
            return false;
        }        
        return true;
    };

    accountRefSubmitHandler = (e) => {
        e.preventDefault();
        let alreadySelected = false;
        let alreadyExistRef = false;
        if (this.props.showButton === true) {
            const updatedAccountReferece = Object.assign({},this.props.editedAccountReference,this.updatedData);
            if(this.accountReferenceValidation(updatedAccountReferece)){
                const selectedCompanyCode = this.props.editedAccountReference.companyCode;
                if (this.props.customerAccountData) {
                    this.props.customerAccountData.map(result => {
                        if (result.companyCode === this.updatedData.companyCode && result.companyCode !== selectedCompanyCode) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.customer.COMPANY_ALREADY_SELECTED, 'warningToast CustAcctRefCompCodeDupChk');
                    return false;
                }
                if (this.props.customerAccountData) {
                    this.props.customerAccountData.map(result => {
                        if (result.accountReferenceValue === this.updatedData.accountReferenceValue && result.companyCode !== selectedCompanyCode) {
                            if (result.recordStatus !== 'D') {
                                alreadyExistRef = true;
                            }
                        }
                    });
                }
                if (alreadyExistRef === true) {
                    IntertekToaster(localConstant.customer.ACCOUNT_REFERENCE_ALREADY_EXIST, 'warningToast CustAcctRefCompCodeDupChkExist');
                }
                else {
                    if (this.props.editedAccountReference.recordStatus !== "N") {
                        this.updatedData["recordStatus"] = "M";
                    }
                    this.updatedData["modifiedBy"] = this.props.loginUser;
                    const selectedOption = document.getElementById("companyName").options;
                    this.updatedData["companyName"] = selectedOption[selectedOption.selectedIndex].text;
                    this.props.actions.UpdateAccountReference(this.updatedData);
                    this.updatedData = {};
                    document.getElementById("addAccountReference").reset();
                    document.getElementById("cancelAccountReference").click();
                }
            }
        }
        if (this.props.showButton === false) {
            if(this.accountReferenceValidation(this.updatedData)){
                if (this.props.customerAccountData) {
                    this.props.customerAccountData.map(result => {
                        if (result.companyCode === this.updatedData.companyCode) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.customer.COMPANY_ALREADY_SELECTED, 'warningToast CustAcctRefAddCompCodeDupChk');
                    return false;
                }
                if (this.props.customerAccountData) {
                    this.props.customerAccountData.map(result => {
                        if (result.accountReferenceValue === this.updatedData.accountReferenceValue) {
                            if (result.recordStatus !== 'D') {
                                alreadyExistRef = true;
                            }
                        }
                    });
                }
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loginUser;
                const selectedOption = document.getElementById("companyName").options;
                this.updatedData["companyName"] = selectedOption[selectedOption.selectedIndex].text;
                this.updatedData["customerCompanyAccountReferenceId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.props.actions.AddAccountReference(this.updatedData);
                this.updatedData = {};
                document.getElementById("addAccountReference").reset();
                document.getElementById("cancelAccountReference").click();  
                // if (alreadyExistRef === true) {
                //    // IntertekToaster(localConstant.customer.ACCOUNT_REFERENCE_ALREADY_EXIST, 'warningToast CustAcctRefAddCompCodeDupChk');
                // }
                // else {

                //     this.updatedData["recordStatus"] = "N";
                //     this.updatedData["modifiedBy"] = this.props.loginUser;
                //     const selectedOption = document.getElementById("companyName").options;
                //     this.updatedData["companyName"] = selectedOption[selectedOption.selectedIndex].text;
                //     this.updatedData["customerCompanyAccountReferenceId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                //     this.props.actions.AddAccountReference(this.updatedData);
                //     this.updatedData = {};
                //     document.getElementById("addAccountReference").reset();
                //     document.getElementById("cancelAccountReference").click();                    
                // }
            }
        }
    }

    render() {
        const { showButton } = this.props;
        const { editedAccountReference, customerAssignmentData, customerAccountData, assignmentReferenceTypes, companyData, editedAssignmentReference } = this.props;
        let customerAssignmentReference = [];
        let customerAccountReference = [];
        if (customerAssignmentData) {
            customerAssignmentReference = customerAssignmentData.filter(note => note.recordStatus != "D");
        }
        if (customerAccountData) {
            customerAccountReference = customerAccountData.filter(note => note.recordStatus != "D");
        }
        // const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                {/* <CustomModal modalData={modelData} /> */}
               
                    <AssignmentRefModal formSubmit={this.assignmentRefSubmitHandler}
                        assignmentRefTypes={assignmentReferenceTypes}
                        editedAssignmentRef={editedAssignmentReference}
                        clearAssignmentRef={this.clearAssignmentReference}
                        selectChange={this.selectChangeHandler}
                        showButton={showButton}
                        refModal={this.refModal}
                        showOverlay={ this.state.showRefOverlay }
                         />
                    <AccountRefModal formSubmit={this.accountRefSubmitHandler}
                        selectChange={this.selectChangeHandler}
                        companyData={companyData}
                        editedAccountRef={editedAccountReference}
                        inputChange={this.inputChangeHandler}
                        clearAccountRef={this.clearAccountReference}
                        showButton={showButton} 
                        accntModal={this.accntModal}
                        showAccntOverlay={ this.state.showAccntOverlay }/>

                    <div className="genralDetailContainer customCard mt-0">
                    <h6 className="col s12 label-bold mt-2 mb-0 pl-0">{ localConstant.customer.ASSIGNMENT_REFERENCE }</h6>
                        <AssignmentReference assignmentReference={customerAssignmentReference}
                            headerData={HeaderData}
                            clearAssignmentRef={this.clearAssignmentReference}
                            deleteAssignmentRef={this.assignmentReferenceDeleteHandler}
                            pageMode={this.props.pageMode}
                            onRef={ref => { this.child = ref; }}
                            showAssignmentReference={this.showAssignmentReference}
                             />

                    <h6 className="col s12 label-bold mt-2 mb-0 pl-0">Accounts References</h6>
                        <AccountReference accountReference={customerAccountReference}
                            headerData={HeaderData}
                            clearAccountRef={this.clearAccountReference}
                            deleteAccountRef={this.accountReferenceDeleteHandler}
                            pageMode={this.props.pageMode}
                            onRef={ref => { this.secondChild = ref; }}
                            showAccountReference={this.showAccountReference} 
                            />
                    </div>                
            </Fragment>
        );
    }
}

export default Assignment;