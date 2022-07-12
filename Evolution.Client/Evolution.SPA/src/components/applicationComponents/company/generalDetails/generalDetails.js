import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import PropTypes from 'proptypes';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, captalize,bindAction,isEmpty } from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import Modal from '../../../../common/baseComponents/modal';
import Editor from '../../../../common/baseComponents/quill/customEditor';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();
const GeneralDetail = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.generalDetails.GENERAL_DETAILS} colSize="s12">
        <div className="row m-0 mb-2">
            <LabelwithValue
                colSize="s3"
                label={localConstant.companyDetails.generalDetails.CODE}
                value={props.companyDetail.companyCode} />
            <LabelwithValue
                colSize="s5"
                label={localConstant.companyDetails.generalDetails.NAME}
                value={props.companyDetail.companyName} />
            <LabelwithValue
                colSize="s4"
                label={localConstant.companyDetails.generalDetails.OPERATING_COUNTRY}
                value={props.companyDetail.operatingCountryName} />
            <LabelwithValue
                colSize="s3"
                label={localConstant.companyDetails.generalDetails.COGNOS_NO}
                value={props.companyDetail.cognosNumber} />
            <LabelwithValue
                colSize="s3"
                label={localConstant.companyDetails.generalDetails.NATIVE_CURRENCY}
                value={props.companyDetail.currency} />
            <CustomInput hasLabel={false}
                type='checkbox'
                checkBoxArray={ [ { label: localConstant.companyDetails.generalDetails.FULL_USE, value: props.companyDetail.isFullUse } ] }
                colSize='s2'
                name="isFullUse"
                onCheckboxChange={props.onBlur}
                disabled={props.interactionMode} 
                />
            <CustomInput hasLabel={false}
                type='checkbox'
                checkBoxArray={ [ { label: localConstant.companyDetails.generalDetails.ACTIVE, value: props.companyDetail.isActive } ] }
                colSize='s2'
                name="isActive"
                onCheckboxChange={props.onBlur} 
                disabled={props.interactionMode}
                />
            <CustomInput hasLabel={false}
                type='checkbox'
                checkBoxArray={ [ { label: localConstant.companyDetails.generalDetails.USER_ICTMS, value: props.companyDetail.isUseIctms } ] }
                colSize='s2'
                name="isUseIctms"
                onCheckboxChange={props.onBlur}
                disabled={props.interactionMode}
                 />
        </div>
    </CardPanel>
);
const TaxDetails = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.generalDetails.TAXDETAILS} colSize="s12">
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.companyDetails.generalDetails.VAT_TAX_REGISTRATION_NO}
                type='select'
                colSize='s2'
                className="browser-default"
                name="euVatPrefix"
                optionsList={props.vatTaxNo}
                optionName='value'
                optionValue="value"
                defaultValue={props.taxRegistrationNo.euVatPrefix && (props.taxRegistrationNo.euVatPrefix).trim()}
                onSelectChange={props.onBlur}
                disabled={props.interactionMode}
            />
            <CustomInput
                hasLabel={false}
                label={localConstant.companyDetails.generalDetails.VAT_TAX_REGISTRATION_NO}
                divClassName="m4 mt-4x"
                type='text'
                name="vatTaxRegNo"
                value={props.taxRegistrationNo.vatTaxRegNo ? props.taxRegistrationNo.vatTaxRegNo : " "}
                colSize='s4'
                inputClass="customInputs"
                maxLength={ fieldLengthConstants.company.generalDetails.VATTAX_REGISTRATION_NUMBER_MAXLENGTH }
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.REVERSE_CHARGES_DISCLAIMER}
                divClassName="m6"
                type='text'
                name='reverseChargeDisclaimer'
                colSize='s6'
                inputClass="customInputs"
                maxLength={ fieldLengthConstants.company.generalDetails.REVERSE_CHARGE_DISCLAIMER_MAXLENGTH }
                value={props.invoiceDefaultData.reverseChargeDisclaimer ? props.invoiceDefaultData.reverseChargeDisclaimer : ""}
                onValueBlur={props.onChange}
                onValueChange={props.onChange}
                dataValType='valueText'
                readOnly={props.interactionMode}
                title={props.invoiceDefaultData.reverseChargeDisclaimer ? props.invoiceDefaultData.reverseChargeDisclaimer : ""}
            />
        </div>
        <div className="row mb-0">
        <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.VAT_REGULATION_TEXT_WITHIN_EC}
                divClassName="s6"
                type='textarea'
                name='vatRegulationTextWithinEC'
                maxLength={ fieldLengthConstants.company.generalDetails.VAT_REGUATION_TEXT_WITHIN_EC_MAXLENGTH}
                colSize='s6'
                inputClass="customInputs"
                value={props.taxRegistrationNo.vatRegulationTextWithinEC ? props.taxRegistrationNo.vatRegulationTextWithinEC : ""}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />
         <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.VAT_REGULATION_TEXT_OUTSIDE_EC}
                divClassName="s6"
                type='textarea'
                name='vatRegulationTextOutsideEC'
                maxLength={ fieldLengthConstants.company.generalDetails.VAT_REGUATION_TEXT_OUTSIDE_EC_MAXLENGTH}
                colSize='s6'
                inputClass="customInputs"
                value={props.taxRegistrationNo.vatRegulationTextOutsideEC ? props.taxRegistrationNo.vatRegulationTextOutsideEC : ""}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />
        </div>
    </CardPanel>
);

const InvoiceDetails = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.generalDetails.INVOICE_DETAILS} colSize="s12">
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INVOICE_DESCRIPTION}
                type='text'
                name='invoiceDescriptionText'
                labelClass="customLabel mandate"
                required={true}
                colSize='s4'
                inputClass="customInputs"
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_DESCRIPTION_MAXLENGTH }
                value={props.invoiceDefaultData.invoiceDescriptionText? props.invoiceDefaultData.invoiceDescriptionText.toUpperCase() : ""}
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                readOnly={props.interactionMode}
                dataValType='valueText'
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INVOICE_DRAFT_TEXT}
                type='text'
                name='invoiceDraftText'
                labelClass="customLabel mandate"
                required={true}
                colSize='s4'
                inputClass="customInputs"
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_DRAFT_TEXT_MAXLENGTH }
                value={props.invoiceDefaultData.invoiceDraftText ? props.invoiceDefaultData.invoiceDraftText : ""}
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.LOGO}
                type='select'
                name='logoText'
                divClassName="col mb-1"
                className="browser-default"
                labelClass="customLabel mandate"
                required={true}
                colSize='s4'
                optionsList={props.companyLogo}
                optionName='name'
                optionValue="name"
                defaultValue={props.logo.logoText}
                onSelectChange={props.onChange}
                disabled={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INTER_COMPNY_TEXT}
                type='text'
                name="invoiceInterCompText"
                colSize='s4'
                maxLength={ fieldLengthConstants.company.generalDetails.INTER_COMPANY_TEXT_MAXLENGTH}
                inputClass="customInputs"
                value={props.invoiceDefaultData.invoiceInterCompText ? props.invoiceDefaultData.invoiceInterCompText : " "}
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INTERCO_DRAFT_TEXT}
                type='text'
                name='invoiceInterCompDraftText'
                colSize='s4'
                labelClass="customLabel mandate"
                required={true}
                maxLength={ fieldLengthConstants.company.generalDetails.INTERCO_DRAFT_TEXT_MAXLENGTH }
                inputClass="customInputs"
                value={props.invoiceDefaultData.invoiceInterCompDraftText ? props.invoiceDefaultData.invoiceInterCompDraftText : " "}
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INTER_COMPANY_DESCRIPTION}
                type='text'
                name='invoiceInterCompDescription'
                labelClass="customLabel mandate"
                required={true}
                colSize='s4'
                maxLength={ fieldLengthConstants.company.generalDetails.INTER_COMPANY_DESCRIPTION_MAXLENGTH }
                inputClass="customInputs"
                value={props.invoiceDefaultData.invoiceInterCompDescription ? props.invoiceDefaultData.invoiceInterCompDescription.toUpperCase() : ""}
                onValueBlur={props.onBlur}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INVOICE_SUMMARY_PAGE}
                divClassName="s6"
                type='textarea'
                name='invoiceSummarryText'
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_SUMMARY_PAGE_FREE_TEXT_MAXLENGTH}
                colSize='s12'
                inputClass="customInputs"
                value={props.invoiceDefaultData.invoiceSummarryText ? props.invoiceDefaultData.invoiceSummarryText : ""}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.generalDetails.INVOICE_HEADER}
                divClassName="s12"
                type='textarea'
                name='invoiceHeader'
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_HEADER_MAXLENGTH }
                colSize='s12'
                inputClass="customInputs"
                value={props.invoiceDefaultData.invoiceHeader ? props.invoiceDefaultData.invoiceHeader : ""}
                onValueChange={props.onBlur}
                dataValType='valueText'
                readOnly={props.interactionMode}
            />
        </div>
    </CardPanel>);
const Extranet = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.generalDetails.PORTAL} colSize="s12">
        <div className="row mb-0">
            <div class="col mb-1 s12 s12">
                <label class="">{localConstant.companyDetails.generalDetails.TECHNICAL_SPECIALIST}</label>            
                <Editor 
                    editorHtml={props.invoiceDefaultData.techSpecialistExtranetComment ? props.invoiceDefaultData.techSpecialistExtranetComment : ""} 
                    onChange = {props.onChange}
                    readOnly = {props.interactionMode}
                />
            </div>
        </div>
    </CardPanel>
);
const InvoiceRemittanceTextPopup = (props) => (
    <div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.IDENTIFIER}
                labelClass="mandate"
                divClassName="s9"
                type='text'
                maxLength={ fieldLengthConstants.company.generalDetails.IDENTIFIER_REMITTANCE_MAXLENGTH }
                name='msgIdentifier'
                autocomplete='off'
                colSize='s9'
                inputClass="customInputs"
                defaultValue={props.selectedRow.msgIdentifier}
                onValueChange={props.onChange}
                readOnly= {props.editField}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.INVOICE_REMITTANCE_TEXT}
                labelClass="mandate"
                divClassName="s12"                
                type='textarea'                
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_REMITTANCE_TEXT_MAXLENGTH }
                name='msgText'
                autocomplete='off'
                colSize='s12'
                inputClass="customInputs"
                defaultValue={props.selectedRow.msgText ? props.selectedRow.msgText.trim() : ""}
                onValueChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                type='switch'
                switchLabel={localConstant.modalConstant.DEFAULT}
                isSwitchLabel={true}
                switchName="isDefaultMsg"
                id="isDefaultMsg"
                colSize="s3"
               checkedStatus={props.selectedRow.isDefaultMsg}
                onChangeToggle={props.onSwitchChange}
            />
            <CustomInput
                type='switch'
                switchLabel={localConstant.modalConstant.NOT_USED}
                isSwitchLabel={true}
                switchName="isActive"
                id="isActive"
                checkedStatus={props.selectedRow.isActive !==undefined ? !props.selectedRow.isActive :false}  //Changes for D-1233
                colSize="s3"
                onChangeToggle={props.onSwitchChange}
            />
        </div>
    </div>
);

const InvoiceFooterTextPopup = (props) => (
    <div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.IDENTIFIER}
                labelClass="mandate"
                divClassName="s9"
                type='text'
                maxLength={ fieldLengthConstants.company.generalDetails.IDENTIFIER_FOOTER_MAXLENGTH }
                name='msgIdentifier'
                colSize='s9'
                inputClass="customInputs"
                defaultValue={props.selectedRow.msgIdentifier}
                onValueChange={props.onChange}
                readOnly={props.editField}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.modalConstant.INVOICE_FOOTER_TEXT}
                labelClass="mandate"
                divClassName="s12"
                type='textarea'                
                name='msgText'
                maxLength={ fieldLengthConstants.company.generalDetails.INVOICE_FOOTER_TEXT_MAAXLENGTH }
                colSize='s12'
                inputClass="customInputs"
                defaultValue={props.selectedRow.msgText ? props.selectedRow.msgText.trim() : ""}
                onValueChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                type='switch'
                switchLabel={localConstant.modalConstant.DEFAULT}
                isSwitchLabel={true}
                switchName="isDefaultMsg"
                id="isDefaultMsg"
                colSize="s3"
                checkedStatus={props.selectedRow.isDefaultMsg}
                onChangeToggle={props.onSwitchChange}
            />
        </div>
    </div>
);

const ResourceOutsideDistance = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.generalDetails.OUTSIDE_DISTANCE} colSize="s12">
        <div className="row mb-0">          
            <CustomInput
                hasLabel={true} 
                labelClass="customLabel mandate"
                label={localConstant.companyDetails.generalDetails.RESOURCE_OUTSIDE_DISTANCE}
                type='text'
                colSize='s4'
                divClassName="s6 numerInput"
                name="resourceOutsideDistance"
                dataType='numeric'  
                // step="any"
                inputClass="customInputs"
                max="9999"  
                maxLength={fieldLengthConstants.company.generalDetails.RESOURCE_OUTSIDE_DISTANCE_MAXLENGTH} //Length in th DB is 4
                value={props.companyDetail ? props.companyDetail.resourceOutsideDistance : 0 }
                onValueChange={props.onChange}
                onValueBlur={props.onBlur} //Save btn enable
                readOnly={props.interactionMode}    //Added For Defect 691 -Issue 1
            />
        </div>
    </CardPanel>);

class GeneralDetails extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.editedRow = {};
        this.state = {
            isOpen: false,
            isInvoicingDefaultOpen:false,
            isInvoicingDefaultEdit:false,
            isInvoicingFooterOpen:false,
            isInvoicingFooterEdit:false            
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.isDuplicateRecord = false;
        this.defaultMsg = {};
        this.invoiceDefaultSubmitButtons = [
            {
                name: "Cancel",
                action: this.clearData,
                type: 'reset',
                btnID: "cancelInvoiceDefault",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                id:"cancelInvoiceRemittanceSubmit"
            },
            {
                name: "Submit",
                type : 'submit',
                btnID: "createInvoiceDefault",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
    }

    componentDidMount() {
        const tab = document.querySelectorAll('.tabs');
        MaterializeComponent.Tabs.init(tab);
        const select = document.querySelectorAll('select');
        MaterializeComponent.FormSelect.init(select);
        const datePicker = document.querySelectorAll('.datepicker');
        MaterializeComponent.Datepicker.init(datePicker);
        const custModal = document.querySelectorAll('.modal');
        MaterializeComponent.Modal.init(custModal, { "dismissible": false, "preventScrolling": true });

        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchCompanyLogoMasterData();
        }
    };

    handlerChange = (e) => { 
        this.updatedData[e.target.name] = e.target.value;
    }

    switchHandlerChange = (e) => {
        this.updatedData[e.target.name] = document.getElementById(e.target.name).checked ? true : false;
        //Added for D-1233 -Start
        if(e.target.name === "isActive")
            this.updatedData[e.target.name]=document.getElementById(e.target.name).checked ? false : true;
        //Added for D-1233 -End
    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    invoiceDetailsHandlerChange = (e) => {   
        if(e.target.name === "invoiceInterCompDescription"){
            e.target.value = e.target.value.toUpperCase();
        }
        this.updatedData[e.target.name] = this.inputChangeHandler(e); 
        this.updatedData["recordStatus"] = "M";
        this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
        this.updatedData = {};
    }

    invoiceDetailsExtranetCommentChange = (content, delta, source, editor) => {     
        if (source === "user") {
            this.updatedData["techSpecialistExtranetComment"] = content;
            this.updatedData["recordStatus"] = "M";
            this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
            this.updatedData = {};
        }
    }

    companyInfoHandlerChange = (e) => {
        this.updatedData[e.target.name] = this.inputChangeHandler(e);         
        this.updatedData["recordStatus"] = "M";              
        this.props.actions.AddUpdateCompanyInfo(this.updatedData);
        this.updatedData = {};
    }

    /** Msg Identifier is already exist or not */
    isMsgIdentiferAlreadyExists = (editedRow,updatedData,invoicingArray) => {
        let isAlreadyExist = false;
        if(editedRow && editedRow.msgIdentifier !== updatedData.msgIdentifier){
            isAlreadyExist = !isEmpty(invoicingArray) && invoicingArray.map(value => {
                if (value.msgIdentifier !== null && value.msgIdentifier.toUpperCase() === updatedData.msgIdentifier.toUpperCase() && value.recordStatus !== "D") {
                    return !isAlreadyExist;
                }
                else {
                    return isAlreadyExist;
                }
            });
        }
        return isAlreadyExist;
    };

    invoiceRemittanceSubmitHandler = (e) => {
        e.preventDefault();
        let isAlreadyExist = false;
        let enteredWord = "";
        let invoiceRemittanceText = "";
       
        if (this.state.isInvoicingDefaultEdit === true) {
            const validateInvoiceRemittance = Object.assign({},this.editedRow,this.updatedData);
            const  validateIdentifier = validateInvoiceRemittance.msgIdentifier && validateInvoiceRemittance.msgIdentifier.trim();
            invoiceRemittanceText = validateInvoiceRemittance.msgText && validateInvoiceRemittance.msgText.trim(); 
            if(isEmpty(validateIdentifier)){
                IntertekToaster(localConstant.companyDetails.generalDetails.VALID_INDENTIFIER,'warningToast Identifier');
                return false;
            }
            if(isEmpty(invoiceRemittanceText)){
                IntertekToaster(localConstant.companyDetails.generalDetails.INVOICE_REMITTANCE,'warningToast Identifier');
                return false;
            }
            if (this.updatedData.msgIdentifier) {
                enteredWord = captalize(this.updatedData.msgIdentifier);
                if(!isEmpty(enteredWord)){
                    if (!isEmpty(this.props.InvoicingDetails)) {
                        isAlreadyExist = this.isMsgIdentiferAlreadyExists(this.editedRow,this.updatedData,this.props.InvoicingDetails.invoiceRemittances);
                        // if(this.editedRow && this.editedRow.msgIdentifier !== this.updatedData.msgIdentifier){
                        //     isAlreadyExist = !isEmpty(this.props.InvoicingDetails.invoiceRemittances) && this.props.InvoicingDetails.invoiceRemittances.map(remittance => {
                        //         if (remittance.msgIdentifier !== null && remittance.msgIdentifier.toUpperCase() === enteredWord.toUpperCase() && remittance.recordStatus !== "D") {
                        //             return !isAlreadyExist;
                        //         }
                        //         else {
                        //             return isAlreadyExist;
                        //         }
                        //     });
                        // }
                    }
                }
            }
            else {
                if (!isEmpty(this.props.InvoicingDetails)) {
                    isAlreadyExist = this.props.InvoicingDetails.invoiceRemittances && this.props.InvoicingDetails.invoiceRemittances.map(remittance => {
                        if (remittance.msgIdentifier !== null && remittance.msgIdentifier !== this.editedRow.msgIdentifier && remittance.msgIdentifier === this.updatedData.msgIdentifier && remittance.recordStatus !== "D") {
                            return isAlreadyExist;
                        }
                    });
                }
            }

            if (Array.isArray(isAlreadyExist) && isAlreadyExist.includes(true)) {                
                IntertekToaster(localConstant.companyDetails.generalDetails.INDENTIFIER_EXIST,'warningToast DupidentifierExist');
            }
            else {
                if (!isEmpty(this.props.InvoicingDetails)) {
                    this.props.InvoicingDetails.invoiceRemittances && this.props.InvoicingDetails.invoiceRemittances.map(remittance => {
                        if (remittance.isDefaultMsg === true && this.updatedData.isDefaultMsg === true && remittance.recordStatus !== "D") {
                            this.defaultMsg = remittance;
                            this.isDuplicateRecord = true;
                        }
                    });
                };
                if (this.editedRow.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["oldIdentifier"] = this.editedRow.msgIdentifier;
                this.updatedData = Object.assign({},this.editedRow,this.updatedData);

                if (this.isDuplicateRecord === true) {
                    const confirmationObject = {
                        title: modalTitleConstant.CONFIRMATION,
                        message: modalMessageConstant.INVOICE_REMITTANCE_CHECK_MESSAGE,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: "Yes",
                                onClickHandler: this.changeRemittanceDefaultMsg,
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
                    this.props.actions.UpdateInvoiceRemittance(this.updatedData);
                    this.updatedData = {};
                    this.updatedData["recordStatus"] = "M";
                    this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
                   this.clearData();
                }
            }
        }
       else{            
            enteredWord = captalize(this.updatedData.msgIdentifier);
            enteredWord = this.updatedData.msgIdentifier && this.updatedData.msgIdentifier.trim();
            invoiceRemittanceText = this.updatedData.msgText && this.updatedData.msgText.trim(); 
            if(isEmpty(enteredWord)){
                IntertekToaster(localConstant.companyDetails.generalDetails.VALID_INDENTIFIER,'warningToast Identifier');
                return false;
            }
            if(isEmpty(invoiceRemittanceText)){
                IntertekToaster(localConstant.companyDetails.generalDetails.INVOICE_REMITTANCE,'warningToast Identifier');
                return false;
            }
            if(!isEmpty(enteredWord)){
                if (!isEmpty(this.props.InvoicingDetails)) {
                    isAlreadyExist = !isEmpty(this.props.InvoicingDetails.invoiceRemittances) && this.props.InvoicingDetails.invoiceRemittances.map(remittance => {
                        if (remittance.msgIdentifier !== null && remittance.msgIdentifier.toUpperCase() === enteredWord.toUpperCase() && remittance.recordStatus !== "D") {
                            return !isAlreadyExist;
                        }
                        else {
                            return isAlreadyExist;
                        }
                    });
                }
            }            
            if (Array.isArray(isAlreadyExist) && isAlreadyExist.includes(true)) {                
                IntertekToaster(localConstant.companyDetails.generalDetails.INDENTIFIER_EXIST,'warningToast DupidentifierExist');
            }
            else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["isDefaultMsg"] = document.getElementById("isDefaultMsg").checked;
                this.updatedData["isActive"] = !document.getElementById("isActive").checked; //Changes for D-1233
                this.updatedData["msgType"] = "InvoiceRemittanceText";
                if (!isEmpty(this.props.InvoicingDetails)) {
                    !isEmpty(this.props.InvoicingDetails.invoiceRemittances) && this.props.InvoicingDetails.invoiceRemittances.map(remittance => {
                        if (remittance.isDefaultMsg === true && this.updatedData.isDefaultMsg === true && remittance.recordStatus !== "D") {
                            this.defaultMsg = remittance;
                            this.isDuplicateRecord = true;
                        }
                    });
                };

                if (this.isDuplicateRecord === true) {
                    const confirmationObject = {
                        title: modalTitleConstant.CONFIRMATION,
                        message: modalMessageConstant.INVOICE_REMITTANCE_CHECK_MESSAGE,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: "Yes",
                                onClickHandler: this.changeRemittanceDefaultMsg,
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
                    this.props.actions.AddInvoiceRemittance(this.updatedData);
                    this.updatedData = {};
                    this.updatedData["recordStatus"] = "M";
                    this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
                   this.clearData();
                }
            }
        }
    }

    invoiceRemittanceDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        let confirmationObject = {};
        if (selectedRecords.length > 0) {
            const defaultRemittance = selectedRecords.filter(x => x.isDefaultMsg === true);
            if(defaultRemittance && defaultRemittance.length > 0){
                confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.INVOICE_REMITTANCE_DEFAULT_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Ok",
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
            }
            else{
                confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.INOVICE_REMITTANCE_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: this.deleteInvoiceRemittance,
                            className: "modal-close m-1 btn-small"
                        },
                        {
                            buttonName: "No",
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
            }
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast zeroRow');
        }
    }

    deleteInvoiceRemittance = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteInvoiceRemittance(selectedData);
        this.props.actions.HideModal();
    }

    changeRemittanceDefaultMsg = () => {
        this.defaultMsg.isDefaultMsg = false;
        this.defaultMsg["oldIdentifier"] = this.defaultMsg.msgIdentifier;
        this.defaultMsg.modifiedBy = this.props.loggedInUser;
        if (this.defaultMsg.recordStatus !== "N") {
            this.defaultMsg.recordStatus = "M";
        }
        if (this.state.isInvoicingDefaultEdit === false) {
            this.props.actions.UpdateInvoiceRemittance(this.defaultMsg);
            this.props.actions.AddInvoiceRemittance(this.updatedData);
        }
        else {
            this.props.actions.UpdateInvoiceRemittance(this.defaultMsg);
            this.props.actions.UpdateInvoiceRemittance(this.updatedData);
        }
        //D800 Start  added because record status should update(companyinvoiceInfo) even the popup cliks ok (default popup)
        if(this.updatedData["recordStatus"] !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);
        //D800 Start
        this.defaultMsg = {};
        this.updatedData = {};
        this.isDuplicateRecord = false;
        this.clearData();
        this.props.actions.HideModal();
    }

    invoiceFooterSubmitHandler = (e) => {
        e.preventDefault();
        let isAlreadyExist =  false ;
        let enteredWord = "";
        let invoiceFooterText ="";
        if (this.state.isInvoicingFooterEdit) {
            const validateInvoiceRemittance = Object.assign({},this.editedRow,this.updatedData);
            const  validateIdentifier = validateInvoiceRemittance.msgIdentifier && validateInvoiceRemittance.msgIdentifier.trim();
            invoiceFooterText = validateInvoiceRemittance.msgText && validateInvoiceRemittance.msgText.trim(); 
            if(isEmpty(validateIdentifier)){
                IntertekToaster(localConstant.companyDetails.generalDetails.VALID_INDENTIFIER,'warningToast Identifier');
                return false;
            }
            if(isEmpty(invoiceFooterText)){
                IntertekToaster(localConstant.companyDetails.generalDetails.INVOICE_FOOTER,'warningToast Identifier');
                return false;
            }
            if (this.updatedData.msgIdentifier) {
                enteredWord = captalize(this.updatedData.msgIdentifier);
                if (enteredWord != undefined) {
                    if (!isEmpty(this.props.InvoicingDetails) && this.props.InvoicingDetails.invoiceFooters) {
                        isAlreadyExist = this.isMsgIdentiferAlreadyExists(this.editedRow,this.updatedData,this.props.InvoicingDetails.invoiceFooters);
                        // if(this.editedRow && this.editedRow.msgIdentifier !== this.updatedData.msgIdentifier){
                        //     isAlreadyExist = this.props.InvoicingDetails.invoiceFooters.map(footer => {
                        //         if (footer.msgIdentifier !== null && footer.msgIdentifier.toUpperCase() === enteredWord.toUpperCase() && footer.recordStatus !== "D") {
                        //             return !isAlreadyExist;
                        //         }
                        //         else {
                        //             return isAlreadyExist;
                        //         }
                        //     });
                        // }
                    }
                }
            }
            else {
                if (!isEmpty(this.props.InvoicingDetails) && this.props.InvoicingDetails.invoiceFooters) {
                    isAlreadyExist = this.props.InvoicingDetails.invoiceFooters.map(footer => {
                        if (footer.msgIdentifier !== null && footer.msgIdentifier !== this.editedRow.msgIdentifier && footer.msgIdentifier === this.updatedData.msgIdentifier && footer.recordStatus !== "D") {
                            return isAlreadyExist;
                        }
                    });
                }
            }

            if (Array.isArray(isAlreadyExist) && isAlreadyExist.includes(true)) {
                IntertekToaster(localConstant.companyDetails.generalDetails.INDENTIFIER_EXIST, 'warningToast dupIdentifier');
            }
            else {
                if (!isEmpty(this.props.InvoicingDetails) && this.props.InvoicingDetails.invoiceFooters) {
                    this.props.InvoicingDetails.invoiceFooters.map(footer => {
                        if (footer.isDefaultMsg === true && this.updatedData.isDefaultMsg === true && footer.recordStatus !== "D") {
                            this.defaultMsg = footer;
                            this.isDuplicateRecord = true;
                        }
                    });
                };
                if (this.editedRow.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["oldIdentifier"] = this.editedRow.msgIdentifier;
                this.updatedData["isDefaultMsg"] = document.getElementById("isDefaultMsg").checked;
                this.updatedData=Object.assign({}, this.editedRow, this.updatedData);

                if (this.isDuplicateRecord === true) {
                    const confirmationObject = {
                        title: modalTitleConstant.CONFIRMATION,
                        message: modalMessageConstant.INVOICE_DEFAULT_CHECK_MESSAGE,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: "Yes",
                                onClickHandler: this.changeDefaultMsg,
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
                    const editedData = Object.assign({}, this.editedRow, this.updatedData);
                    this.props.actions.UpdateInvoiceFooter(editedData);
                    this.updatedData = {};
                    this.updatedData["recordStatus"] = "M";
                    this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
                   this.clearData();
                }
            }
        }
        else{
            enteredWord = captalize(this.updatedData.msgIdentifier);
            enteredWord = this.updatedData.msgIdentifier && this.updatedData.msgIdentifier.trim();
            invoiceFooterText = this.updatedData.msgText && this.updatedData.msgText.trim(); 
            if(isEmpty(enteredWord)){
                IntertekToaster(localConstant.companyDetails.generalDetails.VALID_INDENTIFIER,'warningToast Identifier');
                return false;
            }
            if(isEmpty(invoiceFooterText)){
                IntertekToaster(localConstant.companyDetails.generalDetails.INVOICE_FOOTER,'warningToast Identifier');
                return false;
            }
            if(enteredWord !=undefined){
                if (!isEmpty(this.props.InvoicingDetails) && this.props.InvoicingDetails.invoiceFooters) {
                    isAlreadyExist = this.props.InvoicingDetails.invoiceFooters.map(footer => {
                        if (footer.msgIdentifier !== null && footer.msgIdentifier.toUpperCase() === enteredWord.toUpperCase() && footer.recordStatus !== "D") {
                            return !isAlreadyExist;
                        }
                        else {
                            return isAlreadyExist;
                        }
                    });
                }
            }
            if (Array.isArray(isAlreadyExist) && isAlreadyExist.includes(true)) {
                IntertekToaster(localConstant.companyDetails.generalDetails.INDENTIFIER_EXIST,'warningToast dupIdentifierWarn');
            }
            else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["isDefaultMsg"] = document.getElementById("isDefaultMsg").checked;
                this.updatedData["msgType"] = "InvoiceFooterText";
                if (!isEmpty(this.props.InvoicingDetails) && this.props.InvoicingDetails.invoiceFooters) {
                    this.props.InvoicingDetails.invoiceFooters.map(footer => {
                        if (footer.isDefaultMsg === true && this.updatedData.isDefaultMsg === true && footer.recordStatus !== "D") {
                            this.defaultMsg = footer;
                            this.isDuplicateRecord = true;
                        }
                    });
                };

                if (this.isDuplicateRecord === true) {
                    const confirmationObject = {
                        title: modalTitleConstant.CONFIRMATION,
                        message: modalMessageConstant.INVOICE_DEFAULT_CHECK_MESSAGE,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: "Yes",
                                onClickHandler: this.changeDefaultMsg,
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
                    this.props.actions.AddInvoiceFooter(this.updatedData);
                    this.updatedData = {};
                    this.updatedData["recordStatus"] = "M";
                    this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);             
                   this.clearData();
                }
            }
        }
    }

    invoiceFooterDeleteClickHandler = () => {
        const selectedRecords = this.secondChild.getSelectedRows();
        let confirmationObject = {};
        if (selectedRecords.length > 0) {
            const defaultFooter = selectedRecords.filter(x => x.isDefaultMsg === true);
            if(defaultFooter && defaultFooter.length > 0){
                confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.INVOICE_FOOTER_DEFAULT_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Ok",
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
            }
            else{
                confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.INOVICE_FOOTER_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: this.deleteInvoiceFooter,
                            className: "modal-close m-1 btn-small"
                        },
                        {
                            buttonName: "No",
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
            }
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast noRowSelectedWarning');
        }
    }

    deleteInvoiceFooter = () => {
        const selectedData = this.secondChild.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteInvoiceFooter(selectedData);
        this.props.actions.HideModal();
    }

    changeDefaultMsg = () => {
        this.defaultMsg.isDefaultMsg = false;
        this.defaultMsg["oldIdentifier"] = this.defaultMsg.msgIdentifier;
        this.defaultMsg.modifiedBy = this.props.loggedInUser;
        if (this.defaultMsg.recordStatus !== "N") {
            this.defaultMsg.recordStatus = "M";
        }
        if (this.state.isInvoicingFooterEdit === false) {
            this.props.actions.UpdateInvoiceFooter(this.defaultMsg);
            this.props.actions.AddInvoiceFooter(this.updatedData);
        }
        else {
            this.props.actions.UpdateInvoiceFooter(this.defaultMsg);
            this.props.actions.UpdateInvoiceFooter(this.updatedData);
        }
        //D800 Start  added because record status should update(companyinvoiceInfo) even the popup cliks ok (default popup)
        if(this.updatedData["recordStatus"] !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        this.props.actions.AddUpdateInvoiceDefaults(this.updatedData);
        //D800 Start 
        this.defaultMsg = {};
        this.updatedData = {};
        this.isDuplicateRecord = false;
        this.clearData();
        this.props.actions.HideModal();
        //document.getElementById("cancelInvoiceFooterSubmit").click();
    }

    // clearData = () => {
    //     document.getElementById("remittanceTextPopup").reset();
    //     document.getElementById("invoiceFooterTextPopup").reset();
    //     this.props.actions.ShowButtonHandler();
    // }
    clearData = () => {
        this.updatedData = {};
        this.setState({ 
            isInvoicingDefaultOpen:false,
            isInvoicingDefaultEdit:false,
            isInvoicingFooterOpen:false,
            isInvoicingFooterEdit:false
        });
        this.editedRow = {};
        this.updatedData={};
    }

    confirmationRejectHandler = () => {
        this.isDuplicateRecord = false;
        this.props.actions.HideModal();
    }
    editInvoicingDefaultRowHandler = (data) => {
        this.setState((state) => {
            return {
                isInvoicingDefaultOpen:!state.isInvoicingDefaultOpen,
                isInvoicingDefaultEdit:true
            };
        });
        this.editedRow = data;
    }
    editInvoicingFooterRowHandler = (data) => {
        this.setState((state) => {
            return {
                isInvoicingFooterOpen:!state.isInvoicingFooterOpen,
                isInvoicingFooterEdit:true
            };
        });
        this.editedRow = data;
    }
    invoiceDefaultCreateHandler = () => {
        this.setState({ 
            isInvoicingDefaultOpen:true,
            isInvoicingDefaultEdit:false,
        });
        this.editedRow = {};
    } 
    invoiceFooterCreateHandler = () => {
        this.setState({ 
            isInvoicingFooterOpen:true,
            isInvoicingFooterEdit:false,
        });
        this.editedRow = {};
    }    
    render() {
        const { InvoicingDetails, companyDetail,interactionMode, editInvoiceRemittance, companyVatPrefixMasterData, editInvoiceFooter, showButton, companyLogo } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const { invoiceRemittances = [], invoiceFooters=[] } = !isEmpty(InvoicingDetails) ?InvoicingDetails:{};
        const InvoiceRemittancerowData = invoiceRemittances && invoiceRemittances.filter(x => x.recordStatus !== 'D');
        const InvoiceFooterrowData = invoiceFooters && invoiceFooters.filter(x => x.recordStatus !== 'D');

        let customerGeneralData = [];
        let invoiceDefault = [];
        if (companyDetail) {
            customerGeneralData = companyDetail;
        }
        if (InvoicingDetails) {
            invoiceDefault = InvoicingDetails;
        }
        bindAction(HeaderData.RemittanceTextHeader, "InvoiceDefault", this.editInvoicingDefaultRowHandler);
        bindAction(HeaderData.InvoiceFooterTextHeader, "InvoiceFooter", this.editInvoicingFooterRowHandler);        
        return (
            <Fragment>
                <CustomModal modalData={modelData} />                
                    {
                        this.state.isInvoicingDefaultOpen &&
                        <Modal 
                            title={this.state.isInvoicingDefaultEdit?localConstant.companyDetails.generalDetails.EDIT_INVOICE_REMITTANCE:localConstant.companyDetails.generalDetails.ADD_INVOICE_REMITTANCE}
                            modalId="invoicingDefaultPopup" 
                            formId="invoicingDefaultForm" 
                            onSubmit={this.invoiceRemittanceSubmitHandler} 
                            modalClass="popup-position invoiceDetailsPopUp" 
                            buttons={ this.invoiceDefaultSubmitButtons } 
                            isShowModal={this.state.isInvoicingDefaultOpen}
                            >
                                <InvoiceRemittanceTextPopup
                                    onSwitchChange={this.switchHandlerChange}
                                    onChange={this.handlerChange}
                                    selectedRow = {this.editedRow}
                                    editField = {this.state.isInvoicingDefaultEdit ? true:false}
                                />
                        </Modal>
                    }
                     {
                        this.state.isInvoicingFooterOpen &&
                        <Modal title={ this.state.isInvoicingFooterEdit?localConstant.companyDetails.generalDetails.EDIT_FOOTER_TEXT:localConstant.companyDetails.generalDetails.ADD_FOOTER_TEXT}
                        modalId="invoicingFooterPopup" formId="invoicingFooterForm" onSubmit={this.invoiceFooterSubmitHandler} modalClass="popup-position invoiceDetailsPopUp" buttons={ this.invoiceDefaultSubmitButtons } isShowModal={this.state.isInvoicingFooterOpen}>
                            <InvoiceFooterTextPopup
                                onSwitchChange={this.switchHandlerChange}
                                onChange={this.handlerChange}
                                selectedRow = {this.editedRow} 
                                editField = {this.state.isInvoicingFooterEdit ? true : false}
                        />
                        </Modal>
                    }
                    <div className="genralDetailContainer customCard mt-0">
                        <GeneralDetail
                        companyDetail={customerGeneralData}
                         onBlur={this.companyInfoHandlerChange}
                         interactionMode={interactionMode}
                          />
                        <TaxDetails 
                        vatTaxNo={companyVatPrefixMasterData} 
                        onBlur={this.companyInfoHandlerChange}
                         onChange={this.invoiceDetailsHandlerChange} 
                         taxRegistrationNo={customerGeneralData} 
                         invoiceDefaultData={invoiceDefault}
                         interactionMode={interactionMode}
                          />
                        <CardPanel className="white lighten-4 black-text pt-3" title={localConstant.companyDetails.generalDetails.INVOICIING_DEFAULTS} colSize="s12">
                           
                                <h6 className="label-bold mt-0">Invoice Remittance Text</h6>
                        <ReactGrid gridRowData={InvoiceRemittancerowData} gridColData={HeaderData.RemittanceTextHeader} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.invoiceRemittance}/>
                               {this.props.pageMode!==localConstant.commonConstants.VIEW &&  
                               <div className="right-align mt-2 mr-2">
                                    <a onClick={this.invoiceDefaultCreateHandler} className="btn-small modal-trigger waves-effect">{localConstant.commonConstants.ADD}</a>
                                    <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " onClick={this.invoiceRemittanceDeleteClickHandler}>{localConstant.commonConstants.DELETE}</a>
                                </div>}
                           
                        </CardPanel>
                        <CardPanel className="white lighten-4 black-text pt-3" colSize="s12">                              
                                <h6 className="label-bold mt-0">Invoice Footer Text</h6>
                                    <ReactGrid gridRowData={InvoiceFooterrowData} gridColData={HeaderData.InvoiceFooterTextHeader} onRef={ref => { this.secondChild = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.invoiceFooter}/>

                                    {this.props.pageMode!==localConstant.commonConstants.VIEW && 
                                     <div className="right-align mt-2 mr-2">
                                    <a onClick={this.invoiceFooterCreateHandler} className="btn-small modal-trigger waves-effect">{localConstant.commonConstants.ADD}</a>
                                    <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " onClick={this.invoiceFooterDeleteClickHandler}>{localConstant.commonConstants.DELETE}</a>
                                </div>}                           
                        </CardPanel>
                        <InvoiceDetails 
                        invoiceDefaultData={invoiceDefault} 
                        onBlur={this.invoiceDetailsHandlerChange} 
                        onChange={this.companyInfoHandlerChange}
                         companyLogo={companyLogo}
                          logo={customerGeneralData} 
                          interactionMode={interactionMode}
                          />
                        <Extranet
                            invoiceDefaultData={invoiceDefault}                         
                            onChange={this.invoiceDetailsExtranetCommentChange}//Changes for D1012                     
                            interactionMode={interactionMode} 
                            />

                            <ResourceOutsideDistance 
                            companyDetail ={companyDetail}
                            onChange={this.companyInfoHandlerChange}
                            onBlur={this.companyInfoHandlerChange}
                            interactionMode={interactionMode} />
                        {/* <AllComponent/>  //FOR Example  */}
                    </div>               
            </Fragment>
        );
    }
}

GeneralDetails.propTypes = {
    InvoiceRemittanceDetails: PropTypes.array,
    InvoiceFooterText: PropTypes.array
};

GeneralDetails.defaultprops = {
    InvoiceRemittanceDetails: [],
    InvoiceFooterText: []
};

export default GeneralDetails;
