import React, { Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData, isEmpty,formInputChangeHandler,bindAction, isValidEmailAddress } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { HeaderData } from './supplierDetailHeader';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';

const localConstant = getlocalizeData();

const GeneralDetails = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.supplier.GENERAL_DETAILS} colSize="s12">
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={localConstant.supplier.SUPPLIER_NAME}
                    divClassName="col"
                    type='text'
                    refProps='supplierNameId'
                    name="supplierName"
                    dataValType='valueText'
                    value={props.generalDetailsData.supplierName !== undefined ?props.generalDetailsData.supplierName:""}
                    readOnly = {props.interactionMode}
                    // disabled = {props.interactionMode}
                    colSize='s12 m6'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.supplier.supplierDetails.SUPPLIER_NAME_MAXLENGTH}
                    onValueChange={props.supplierChange}
                />
            </div>
        </CardPanel>
    );
};

const Address = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.supplier.ADDRESS} colSize="s12">
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    labelClass="mandate"
                    required={true}
                    name="country"
                    label={localConstant.supplier.COUNTRY}
                    type='select'
                    colSize='s12 m3'
                    defaultValue={props.addressData.countryId}   //Changes for D-1076
                    disabled = {props.interactionMode}
                    className="browser-default customInputs"
                    optionsList={props.country}
                    optionName='name'
                    optionValue="id"   //Changes for D-1076
                    id="countryId"   //Changes for D-1076
                    onSelectChange={props.placeChange}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    labelClass="mandate"
                    required={true}
                    name="state"
                    label={localConstant.supplier.STATE}
                    type='select'
                    colSize='s12 m3'
                    defaultValue={props.addressData.stateId}   //Changes for D-1076
                    disabled = {props.interactionMode}
                    className="browser-default customInputs"
                    optionsList={props.state}
                    optionName='name'
                    optionValue="id"   //Changes for D-1076
                    id="stateId"   //Changes for D-1076
                    onSelectChange={props.placeChange}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    required={true}
                    name="city"
                    label={localConstant.supplier.CITY}
                    type='select'
                    colSize='s12 m3'
                    defaultValue={props.addressData.cityId}  //Changes for D-1076
                    disabled = {props.interactionMode}
                    className="browser-default customInputs"
                    optionsList={props.city}
                    optionName='name'
                    optionValue="id"    //Changes for D-1076
                    id="cityId"     //Changes for D-1076
                    onSelectChange={props.supplierChange}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.supplier.POSTAL_CODE}
                    divClassName="col"
                    type='text'
                    dataValType='valueText'
                    refProps='postalCodeForAddressId'
                    name="postalCode"
                    value={props.addressData.postalCode?props.addressData.postalCode:""}
                    readOnly = {props.interactionMode}
                    // disabled = {props.interactionMode}
                    colSize='s12 m3'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.supplier.supplierDetails.POSTAL_CODE_MAXLENGTH}
                    onValueChange={props.supplierChange}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={localConstant.supplier.FULL_ADDRESS}
                    divClassName="col"
                    type='textarea'
                    required={true}
                    name='supplierAddress'
                    colSize='s12 m8'
                    maxLength={fieldLengthConstants.supplier.supplierDetails.FULL_ADDRESS_MAXLENGTH}
                    inputClass="customInputs"
                    value={required(props.addressData.supplierAddress)?"":props.addressData.supplierAddress}
                    // disabled = {props.interactionMode}
                    readOnly = {props.interactionMode}
                    onValueChange={props.supplierChange}
                />
            </div>
        </CardPanel>
    );
};

const SupplierContacts = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.supplier.SUPPLIER_CONTACTS} colSize="s12">
            <ReactGrid gridColData={props.headerData} gridRowData={props.rowData} onRef={props.onRef} paginationPrefixId={localConstant.paginationPrefixIds.supplierContact}/>
          
           {props.pageMode!==localConstant.commonConstants.VIEW && <div className="right-align mt-2">
                <a onClick={props.supplierContactCreateHandler} className="btn-small waves-effect waves-teal" disabled={props.interactionMode}>{localConstant.commonConstants.ADD}</a>
                <a href="#confirmation_Modal" onClick={props.supplierContactDeleteHandler} 
                 className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={(props.interactionMode || props.showButton)?true:false}>{localConstant.commonConstants.DELETE}</a>
           </div> }
        </CardPanel>
    );
};

const SupplierContactsPopup = (props) => {
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.supplier.CONTACT_NAME}
                divClassName="col"
                type='text'
                refProps='supplierContactNameId'
                name="supplierContactName"
                defaultValue={props.supplierContactData.supplierContactName}
                colSize='s12 m8'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.supplier.supplierDetails.CONTACT_NAME_MAXLENGTH}
                onValueChange={props.supplierChange}
                //required={true}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.supplier.TELEPHONE_NO}
                divClassName="col"
                type='text'
                refProps='supplierTelephoneNumberId'
                name="supplierTelephoneNumber"
                defaultValue={props.supplierContactData.supplierTelephoneNumber}
                colSize='s12 m4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.supplier.supplierDetails.TELEPHONE_NUMBER_MAXLENGTH}
                onValueChange={props.supplierChange}
                //required={true}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.supplier.FAX_NO}
                divClassName="col"
                type='text'
                refProps='supplierFaxNumberId'
                name="supplierFaxNumber"
                defaultValue={props.supplierContactData.supplierFaxNumber}
                colSize='s12 m4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.supplier.supplierDetails.FAX_NUMBER_MAXLENGTH}
                onValueChange={props.supplierChange}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.supplier.MOBILE_NO}
                divClassName="col"
                type='text'
                refProps='supplierMobileNumberId'
                name="supplierMobileNumber"
                defaultValue={props.supplierContactData.supplierMobileNumber}
                colSize='s12 m4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.supplier.supplierDetails.MOBILE_NUMBER_MAXLENGTH}
                onValueChange={props.supplierChange}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.supplier.EMAIL}
                divClassName="col"
                type='text'
                refProps='supplierEmailId'
                name="supplierEmail"
                defaultValue={props.supplierContactData.supplierEmail}
                colSize='s12 m4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.supplier.supplierDetails.SUPPLIER_EMAIL_MAXLENGTH}
                onValueChange={props.supplierChange}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.supplier.OTHER_CONTACT_DETAILS}
                divClassName="col"
                type='textarea'
                name='otherContactDetails'
                colSize='s12 m8'
                maxLength={fieldLengthConstants.supplier.supplierDetails.OTHER_CONTACT_DETAILS_MAXLENGTH}
                inputClass="customInputs"
                defaultValue={required(props.supplierContactData.otherContactDetails)?"":props.supplierContactData.otherContactDetails}
                onValueChange={props.supplierChange}
            />
        </Fragment>
    );
};

class supplierDetail extends Component {

    constructor(props){
        super(props);
        this.updatedData = {};
        this.editedRow = {};
        this.state = {
            isSupplierContactOpen:false,
            isSupplierContactEdit:false,
            cancelDupliacteMessage:true,
        };
        this.supplierContactSubmitButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSupplierContact,
                type: 'reset',
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type : 'submit',
                btnID: "createRateSchedule",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.errormessageButton = [
            {
                name: "No",
                action: this.cancelSuppliername,
                btnClass: "btn-small mr-2",
                showbtn: true
            },
            {
                name:"Yes",
                action: this.saveDuplicateSupplierName,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
    }

 cancelSuppliername=(e)=>{
        e.preventDefault();
    
        this.updatedData["supplierName"]="";
        const updatedSupplierDetail = Object.assign({},this.props.supplierInfo,this.updatedData);
        this.props.actions.AddSupplierDetails(updatedSupplierDetail);
        this.updatedData["isSupplierNameDuplicate"]=false;
         this.props.actions.SupplierDuplicateName(this.updatedData);
       
    }
    saveDuplicateSupplierName=(e)=>{
        e.preventDefault();
            this.updatedData1 ={};
        this.updatedData["isSupplierNameDuplicate"]=true;
        const updatedSupplierDetail = Object.assign({},this.props.supplierInfo,this.updatedData);
        this.props.actions.AddSupplierDetails(updatedSupplierDetail);
        this.updatedData1["isSupplierNameDuplicate"]=false;
        this.props.actions.SupplierDuplicateName(this.updatedData1);  
    }

    supplierChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        //Added for D-1076 -Starts
        if(result.name === "country"){
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData['state'] = null;   //D-1270
            this.updatedData['city'] = null;    //D-1270
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; //ITK-D-1270
        }
        if(result.name === "state"){
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData["city"]=null;      //D-1270
            this.updatedData["cityId"]=null;    //D-1270
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; //ITK-D-1270
        }
        if(result.name === "city"){
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; //ITK-D-1270
        }
        //Added for D-1076 -End
        if(this.props.currentPage === localConstant.supplier.EDIT_VIEW_SUPPLIER){
            if(this.updatedData.supplierName){
                if(this.props.supplierContact){
                    this.props.supplierContact.forEach(iteratedValue => {
                        iteratedValue.supplierName = this.updatedData.supplierName;
                        if(iteratedValue.recordStatus !== "N"){
                            iteratedValue.recordStatus = "M";
                        }
                        this.props.actions.UpdateSupplierContact(iteratedValue);
                    });
                }
            }
            this.updatedData['recordStatus'] = 'M';
        }
        else{
            if(isEmpty(this.props.supplierInfo.supplierId)){
                this.updatedData.supplierId = null;
            }
            this.updatedData['recordStatus'] = 'N';
        }
        const updatedSupplierDetail = Object.assign({},this.props.supplierInfo,this.updatedData);
        this.props.actions.AddSupplierDetails(updatedSupplierDetail);
        this.updatedData={};
    }

    supplierContactChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }

    placeChangeHandlerForAddress = (e) => {
        if(e.target.name === 'country' ){
            this.props.actions.FetchStateForAddress(e.target.value);
            this.updatedData['state'] = null;
            this.updatedData['city'] = null;
        }
        if(e.target.name === 'state' ){
            this.props.actions.FetchCityForAddress(e.target.value);
            this.updatedData['city'] = null;
        }
        this.supplierChangeHandler(e);
    }

    placeChangeHandlerContact = (e) => {
        this.supplierChangeHandler(e);
        if(e.target.name === 'country' ){
            this.props.actions.FetchStateForContact(e.target.value);
            this.updatedData['state'] = null;
            this.updatedData['city'] = null;
        }
        if(e.target.name === 'state' ){
            this.props.actions.FetchCityForContact(e.target.value);
            this.updatedData['city'] = null;
        }
    }

    /** SupplierDetails Validation Handler */
supplierContactValidation = (data) => {
    if(isEmpty(data.supplierContactName)){
        IntertekToaster(localConstant.validationMessage.SUPPLIER_CONTACTS_CONTACT_NAME,"warningToast ContactNameVal");
        return false;
    }
    else if(isEmpty(data.supplierTelephoneNumber)){
        IntertekToaster(localConstant.validationMessage.SUPPLIER_CONTACTS_TELEPHONE_NO,"warningToast TelephoneNoVal");
        return false;
    }
    else if(!isEmpty(data.supplierEmail) && !isValidEmailAddress(data.supplierEmail)){
        IntertekToaster(localConstant.validationMessage.EMAIL_VALIDATION,"warningToast emailVal");
        return false;
    }
    else{
        return true;
    }
}

    supplierContactSubmitHandler = (e) => {
        e.preventDefault();
        if(this.state.isSupplierContactEdit){
            if(this.editedRow.recordStatus !== 'N'){
                this.updatedData['recordStatus'] = 'M';
            }
            const editedData = Object.assign({},this.editedRow,this.updatedData);     
            if(this.supplierContactValidation(editedData))
            {                
                this.props.actions.UpdateSupplierContact(editedData);
                this.cancelSupplierContact();
            }
            
        }
        else{
            this.updatedData['recordStatus'] = 'N';
            this.updatedData['supplierContactId'] = Math.floor(Math.random() * 99) -100;
            if(this.props.supplierInfo){
                this.updatedData['supplierName'] = this.props.supplierInfo.supplierName;
                this.updatedData.supplierId = this.props.supplierInfo.supplierId;
            }
            const updatedContact = Object.assign([],this.props.supplierContact);
                updatedContact.push(this.updatedData);
            if(this.supplierContactValidation(this.updatedData)){                       
                this.props.actions.AddSupplierContact(updatedContact);
                this.cancelSupplierContact();
            }
        }
    }

    supplierContactCreateHandler = () => {
        if(this.props.supplierInfo.supplierName){
            this.setState({ 
                isSupplierContactOpen:true,
                isSupplierContactEdit:false,
            });
            this.editedRow = {};
        }
        else{
            IntertekToaster(localConstant.validationMessage.ADD_SUPPLIER_DETAILS_VAL,"warningToast supplierDetailVal");
            return false;
        }
    } 

    supplierContactDeleteHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.SUPPLIER_CONTACT_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSupplierContact,
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
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast supplierContactDeleteToaster');
        }
    }

    deleteSupplierContact = () =>{
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteSupplierContact(selectedRecords);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () =>{
        this.props.actions.HideModal();
    }

    cancelSupplierContact = () => {
        this.updatedData = {};
        this.setState({ 
            isSupplierContactOpen:false,
            isSupplierContactEdit:false,
        });
        this.editedRow = {};
    }

    editSupplierContractRowHandler = (data) => {
        this.setState((state) => {
            return {
                isSupplierContactOpen:!state.isSupplierContactOpen,
                isSupplierContactEdit:true
            };
        });
        this.editedRow = data;
    }

    render() {
        const { supplierInfo,supplierContact,interactionMode,country,stateForAddress,cityForAddress } = this.props;
        let supplierContactList = [];
        if(supplierContact.length > 0){
            supplierContactList = supplierContact.filter(row=>row.recordStatus !== 'D');
        }
        bindAction(HeaderData, "SupplierRenderColumn", this.editSupplierContractRowHandler);
        return (
            <Fragment>
                <CustomModal />
                {this.state.isSupplierContactOpen &&
                    <Modal title={localConstant.supplier.SUPPLIER_CONTACTS} modalId="supplierContactPopup" formId="supplierContactForm" onSubmit={this.supplierContactSubmitHandler} modalClass="popup-position" buttons={ this.supplierContactSubmitButtons } isShowModal={this.state.isSupplierContactOpen}>
                        <SupplierContactsPopup
                            supplierChange={this.supplierContactChangeHandler}
                            supplierContactData = {this.editedRow}
                        />
                    </Modal>}
                    { this.props.duplicateMessage   &&
                    <Modal modalClass="techSpecModal"
                        title={ "Supplier Name already exists.Do u want to continue?"}
                        buttons={this.errormessageButton}
                        isShowModal={ this.props.duplicateMessage.isSupplierNameDuplicate }
                    ></Modal>
                        } 
                <div className="genralDetailContainer customCard">
                    <GeneralDetails
                        generalDetailsData={supplierInfo}
                        supplierChange={this.supplierChangeHandler}
                        interactionMode = {interactionMode}
                    />
                    <Address
                        addressData={supplierInfo}
                        supplierChange={this.supplierChangeHandler}
                        placeChange={this.placeChangeHandlerForAddress}
                        country = {country}
                        state = {stateForAddress}
                        city = {cityForAddress}
                        interactionMode = {interactionMode}
                    />
                    <SupplierContacts
                        rowData={supplierContactList}
                        headerData={HeaderData}
                        supplierContactCreateHandler = {this.supplierContactCreateHandler}
                        supplierContactDeleteHandler = {this.supplierContactDeleteHandler}
                        onRef = {ref => { this.child = ref; }}
                        showButton = {supplierContactList.length > 0 ? false : true}
                        interactionMode = {interactionMode}
                        pageMode={this.props.pageMode}
                    />
                </div>
            </Fragment>
        );
    }
}

export default supplierDetail;