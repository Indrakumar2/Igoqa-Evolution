import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, captalize,bindAction, isEmpty } from '../../../../utils/commonUtils';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';

import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { required } from '../../../../utils/validator';

const localConstant = getlocalizeData();
const CompanyOffice = (props) => (
    <div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.OFFICE_NAME}
                labelClass="customLabel mandate"
                divClassName="s3"
                type='text'
                maxLength={fieldLengthConstants.company.companyOffices.OFFICE_NAME_MAXLENGTH}
                name='officeName'
                autocomplete="off"
                colSize='s3'
                inputClass="customInputs"
                defaultValue={props.selectedRow.officeName}
                onValueChange={props.onChange}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.ACCOUNT_REFERENCE}
                labelClass="customLabel mandate"
                divClassName="s3"
                maxLength={fieldLengthConstants.company.companyOffices.ACCOUNT_REFERENCE_MAXLENGTH}
                type='text'
                autocomplete="off"
                name='accountRef'
                colSize='s3'
                inputClass="customInputs"
                defaultValue={props.selectedRow.accountRef}
                onValueChange={props.onChange}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.FULL_ADDRESS}
                divClassName="s4"     
                labelClass="customLabel mandate"           
                type='textarea'                
                maxLength= { fieldLengthConstants.company.companyOffices.COMPANY_FULL_ADDRESS_MAXLENGTH }
                autocomplete='off'
                name='fullAddress'
                colSize='s6'
                inputClass="customInputs"
                defaultValue={props.selectedRow.fullAddress && props.selectedRow.fullAddress.trim()}
                onValueChange={props.onChange}
            />
            {/* <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.FULL_ADDRESS}
                divClassName="s4"
                type='textarea'
                name='fullAddress'
                maxLength={200}
                colSize='s4'
                inputClass="customInputs"
                value={props.showButton?props.selectedRow.fullAddress:''}
                onValueChange={props.onChange}
            /> */}

        </div>
        <div className="row mb-2">
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.modalConstant.COUNTRY}
                type='select'
                colSize='s3'
                name="country"
                labelClass='mandate'
                className="browser-default"
                optionsList={props.countryMasterData}
                optionName="name"
                optionValue="id"
                onSelectChange={props.onChange}
                defaultValue={props.selectedRow.countryId}
                id="countryId" //Added for ITK D1536
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.modalConstant.STATE_COUNTRY_PRO}
                type='select'
                colSize='s3'
                name='state'
                className="browser-default"
                optionsList={props.stateMasterData}
                optionName='name'
                optionValue='id'
                onSelectChange={props.onChange}
                defaultValue={props.selectedRow.stateId}
                id="stateId" //Added for ITK D1536
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.modalConstant.CITY}
                type='select'
                colSize='s3'
                name="city"
                className="browser-default"
                optionsList={props.cityMasterData}
                optionName='name'
                optionValue="id"
                onSelectChange={props.onChange}
                defaultValue={props.selectedRow.cityId}
                id="cityId" //Added for ITK D1536
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.PINCODE}
                labelClass="customLabel"
                divClassName="s3"
                type='text'
                autocomplete="off"
                name='postalCode'
                maxLength={fieldLengthConstants.company.companyOffices.PINCODE_MAXLENGTH}
                colSize='s3'
                inputClass="customInputs"
                defaultValue={props.selectedRow.postalCode}
                onValueChange={props.onChange}
            />
        </div>
    </div>
);
class CompanyOffices extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.editedRow = {};
        this.state = {
            isOpen: false,
            isCompanyOfficeModalOpen: false,
            isCompanyOfficeModalEdit: false,
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.companyOfficeButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                type: 'reset',
                action: this.cancelCompanyOffice,
                btnID: "cancelCompanyOffice",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type: 'submit',
                btnID: "addCompanyOffice",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
    }
    /** 
         * FetchCountry api call is commented, Now we are getting doc type from master data
        */
    // componentWillMount() {
    //     this.props.actions.FetchCountry();
    // }

    componentDidMount() {
        const tab = document.querySelectorAll('.tabs');
        const tabInstances = MaterializeComponent.Tabs.init(tab);
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select);

        const datePicker = document.querySelectorAll('.datepicker');
        const datePickerInstances = MaterializeComponent.Datepicker.init(datePicker);
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });
    }

    handlerChange = (e) => {
       //
       const row = this.editedRow;
       const updateRow = this.updatedData;
       this.updatedData[e.target.name] = e.target.value;
        if (e.target.name === "country") {
          
            this.props.actions.ClearStateCityData();
            const selectedCountry =e.target.value;
            document.getElementById("stateId").value = ""; 
            document.getElementById("cityId").value = ""; 
            this.updatedData["city"] ="";
            this.updatedData["state"]="";
            this.updatedData[e.target.id]=parseInt(e.target.value);
            this.updatedData[e.target.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
            //Commeneted for #800 - because editeRow is becomming null - cancel grid is resulting empty city and state
            // this.editedRow["city"]="";
            // this.editedRow["state"]="";
            this.props.actions.FetchStateId(selectedCountry);
        }
        if (e.target.name === "state") {
            this.props.actions.ClearCityData();
            document.getElementById("cityId").value = ""; 
            this.updatedData["city"] ="";   
            this.updatedData[e.target.id]=parseInt(e.target.value);
            this.updatedData[e.target.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
            // this.editedRow["city"]="";
            // this.editedRow["state"]=e.target.value;   
            const selectedState = e.target.value;
            this.props.actions.FetchCityId(selectedState); 
                           
        }
        if (e.target.name === "city") {
            this.updatedData[e.target.id]=parseInt(e.target.value);
            this.updatedData[e.target.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        }
        const updateRow2 = this.updatedData;
        
    }

    /** Company Office Validation Handler */
    companyOfficeValidation = (data) => {
        if(isEmpty(data) || required(data.officeName && data.officeName.trim())){
            IntertekToaster(localConstant.validationMessage.OFFICE_NAME_VALIDATION,"warningToast officeNameVal");
            return false;
        }
        if(required(data.accountRef && data.accountRef.trim())){
            IntertekToaster(localConstant.validationMessage.ACCOUNT_REFERENCE_VALIDATION,"warningToast accountRefVal");
            return false;
        }
        if(required(data.fullAddress && data.fullAddress.trim())){
            IntertekToaster(localConstant.validationMessage.FULL_ADDRESS_VALIDATION,"warningToast fullAddressVal");
            return false;
        }
        if(required(data.country && data.country.trim())){
            IntertekToaster(localConstant.validationMessage.COUNTRY_VALIDATION,"warningToast countryVal");
            return false;
        }
        return true;
    };

    companyOfficesSubmitHandler = (e) => {
        e.preventDefault();
        let isAlreadyExist = false;
        let enteredWord = "";
        let accountRefText = "";
        let fieldName = "";
        if (this.state.isCompanyOfficeModalEdit) {
            const updatedCompanyOffice = Object.assign({},this.editedRow,this.updatedData);
            if(this.companyOfficeValidation(updatedCompanyOffice)){
                if (this.updatedData.officeName || this.updatedData.accountRef) {
                    enteredWord = captalize(this.updatedData.officeName);
                    accountRefText = captalize(this.updatedData.accountRef);
                    if (this.props.companyOfficeDetail) {
                        isAlreadyExist = this.props.companyOfficeDetail.map(detail => {
                            if (this.updatedData.officeName && detail.officeName.toUpperCase() === enteredWord.toUpperCase() && detail.recordStatus !== "D") {
                                fieldName = "officeName";
                                return !isAlreadyExist;
                            }
                            if (this.updatedData.accountRef && detail.accountRef.toUpperCase() === accountRefText.toUpperCase() && detail.recordStatus !== "D") {
                                fieldName = "accountRef";
                                return !isAlreadyExist;
                            }
                            else {
                                return isAlreadyExist;
                            }
                        });
                    }
                }
                else {
                    if (this.props.companyOfficeDetail) {
                        isAlreadyExist = this.props.companyOfficeDetail.map(detail => {
                            if ((detail.officeName && detail.officeName === this.editedRow.officeName || detail.accountRef === this.editedRow.accountRef) && detail.recordStatus !== "D") {
                                return isAlreadyExist;
                            }
                        });
                    }
                }

                if (isAlreadyExist.includes(true) && fieldName === "officeName") {
                    IntertekToaster(localConstant.companyDetails.common.OFFICE_NAME_ALREADY_EXISTS, 'warningToast officeNameWarning');
                }
                else if (isAlreadyExist.includes(true) && fieldName === "accountRef") {
                    IntertekToaster(localConstant.companyDetails.common.ACCOUNT_REFERENCE_ALREADY_EXISTS, 'warningToast accRefExistWarning');

                }
                else {
                    if (this.editedRow.recordStatus !== "N") {
                        this.updatedData["recordStatus"] = "M";
                    }
                    if (document.getElementById("stateId").value === "") {
                        this.updatedData.state = null; //For audit changes
                    };
                    if (document.getElementById("cityId").value === "") {
                        this.updatedData.city = null;//For audit changes
                    };
                    this.updatedData["modifiedBy"] = this.props.loggedInUser;
                    const updatedOffice = Object.assign({},this.editedRow,this.updatedData);
                    this.props.actions.UpdateCompanyOffice(updatedOffice);
                    this.cancelCompanyOffice();
                }
            }
        }
        else {
            if(this.companyOfficeValidation(this.updatedData)){
                enteredWord = captalize(this.updatedData.officeName);
                accountRefText = captalize(this.updatedData.accountRef);
                if (this.props.companyOfficeDetail) {
                    isAlreadyExist = this.props.companyOfficeDetail.map(detail => {
                        if (detail.officeName && detail.officeName.toUpperCase() === enteredWord.toUpperCase() && detail.recordStatus !== "D") {
                            fieldName = "officeName";
                            return !isAlreadyExist;
                        }
                        if (detail.accountRef && detail.accountRef.toUpperCase() === accountRefText.toUpperCase() && detail.recordStatus !== "D") {
                            fieldName = "accountRef";
                            return !isAlreadyExist;
                        }
                        else {
                            return isAlreadyExist;
                        }
                    });
                }
                if (isAlreadyExist.includes(true) && fieldName === "officeName") {
                    IntertekToaster(localConstant.companyDetails.common.OFFICE_NAME_ALREADY_EXISTS, 'warningToast officeDupWarning');

                }
                else if (isAlreadyExist.includes(true) && fieldName === "accountRef") {
                    IntertekToaster(localConstant.companyDetails.common.ACCOUNT_REFERENCE_ALREADY_EXISTS, 'warningToast accRefeDupWarn');
                }
                else {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["modifiedBy"] = this.props.loggedInUser;
                    this.updatedData["addressId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.props.actions.AddCompanyOffice(this.updatedData);
                    this.cancelCompanyOffice();
                }
            }
        }
    }

    companyOfficeDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.OFFICE_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelected,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast emptyRowWarning');
        }
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    clearData = () => {
        document.getElementById("addCompany").reset();
        this.props.actions.ShowButtonHandler();
        this.props.actions.ClearStateCityData();
        this.updatedData = {};
    }

    deleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteCompanyOffices(selectedData);
        this.props.actions.HideModal();
    }

    companyOfficeAddHandler = () => {
        this.setState({
            isCompanyOfficeModalEdit:false,
            isCompanyOfficeModalOpen:true,
        });
    }

    companyOfficeEditHandler = (data) => {
        this.setState({
            isCompanyOfficeModalEdit:true,
            isCompanyOfficeModalOpen:true,
        });
        this.editedRow = data;
        this.props.actions.EditCompanyOffice(data);
    }

    cancelCompanyOffice = () => {
        this.setState({
            isCompanyOfficeModalEdit:false,
            isCompanyOfficeModalOpen:false,
        });
        this.updatedData = {};
        this.editedRow = {};
    }

    render() {
        const selectedRow = this.props.editRecord;
        const headData = HeaderData;
        const rowData = this.props.companyOfficeDetail && this.props.companyOfficeDetail.filter(x => x.recordStatus != 'D');
        const { showButton, stateMasterData, countryMasterData, cityMasterData } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        bindAction(headData, "EditColumn", this.companyOfficeEditHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />              

                {this.state.isCompanyOfficeModalOpen ?
                    <Modal title={ this.state.isCompanyOfficeModalEdit?localConstant.companyDetails.offices.EDIT_COMPANY_OFFICE:localConstant.companyDetails.offices.ADD_COMPANY_OFFICE} modalId="companyOfficePopup" formId="companyOfficeForm" modalClass="popup-position" onSubmit={this.companyOfficesSubmitHandler} buttons={this.companyOfficeButtons} isShowModal={this.state.isCompanyOfficeModalOpen}>
                        <CompanyOffice 
                            stateMasterData={stateMasterData} 
                            showButton={showButton} 
                            countryMasterData={countryMasterData} 
                            cityMasterData={cityMasterData} 
                            selectedRow={this.editedRow} 
                            onChange={this.handlerChange} />
                    </Modal>:null
                }

                    {/* <div id="add-location" className="modal popup-position">
                        <form id="addCompany" onSubmit={this.companyOfficesSubmitHandler}>
                            <div className="modal-content">
                                <h6>Company Office</h6>
                                <CompanyOffice stateMasterData={stateMasterData} showButton={showButton} countryMasterData={countryMasterData} cityMasterData={cityMasterData} selectedRow={selectedRow} onChange={this.handlerChange} />
                            </div>
                            <div className="modal-footer">
                                <button type="button" id="cancelCompanyOfficesSubmit" onClick={this.clearData} className="modal-close waves-effect waves-teal btn-flat mr-2">CANCEL</button>
                                {!showButton ?
                                    <button type="submit" className="btn-small">SUBMIT</button> :
                                    <button type="submit" className="btn-small">SUBMIT</button>}
                            </div>
                        </form>
                    </div> */}

                    <div className="customCard">
                        <h6 className='label-bold'>Company Offices</h6>
                       
                    <ReactGrid
                        gridRowData={rowData}
                        gridColData={headData}
                        onRef={ref => { this.child = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.companyOffice} />
                            {this.props.pageMode!== localConstant.commonConstants.VIEW && <div className="right-align mt-2">
                                <a onClick={this.companyOfficeAddHandler} className="btn-small waves-effect" >{localConstant.commonConstants.ADD}</a>
                                {/* <a onClick={this.clearData} href="#add-location" className="btn-small waves-effect modal-trigger">Add</a> */}
                                <a href="#confirmation_Modal" className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " onClick={this.companyOfficeDeleteClickHandler}>{localConstant.commonConstants.DELETE}</a>
                            </div>}
                        </div>
                   
            </Fragment>
        );
    }
}

export default CompanyOffices;