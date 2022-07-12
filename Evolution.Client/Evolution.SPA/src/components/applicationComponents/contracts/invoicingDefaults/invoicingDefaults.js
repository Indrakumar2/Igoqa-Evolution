import React, { Component, Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, isEmpty, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import arrayUtil from '../../../../utils/arrayUtil';
import { required } from '../../../../utils/validator';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants.js';

const localConstant = getlocalizeData();

const InvoicingDefault = (props) => {
    const invoiceInstructionNotes = !required(props.fetchContractInfo.contractInvoiceInstructionNotes) ? props.fetchContractInfo.contractInvoiceInstructionNotes : "";
    const invoiceFreeText = !required(props.fetchContractInfo.contractInvoiceFreeText) ? props.fetchContractInfo.contractInvoiceFreeText : "";
    return (
    <CardPanel className="white lighten-4 black-text" title={localConstant.contract.INVOICING_DEFAULTS} colSize="s12">
            {/**590 - CONTRACT-Inv Defaults-Use Parent Contract Details- UAT Testing */}
            { props.fetchContractInfo.contractType ?
      props.fetchContractInfo.contractType=="PAR"?
     <div className="row mb-0">
     <label>
        <input name="isParentContractInvoiceUsed" className='filled-in' type="checkbox" onClick={props.onChange} defaultChecked={props.fetchContractInfo.isParentContractInvoiceUsed} />
        <span>Use Invoice Details From Parent Contract</span>
     </label>
     </div> : null : null }
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col '
                className="browser-default "
                colSize='s9'
                label={localConstant.contract.INVOICE_PAYMENT_TERMS}
                labelClass="mandate"
                inputClass="customInputs"
                optionValue='name'
                optionName="name"
                name="contractInvoicePaymentTerms"
                optionsList={props.invoicePaymentTerms}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractInvoicePaymentTerms}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.contract.SALES_TAX}
                labelClass="mandate"
                optionValue='taxName'
                optionName="taxName"
                name="contractSalesTax"
                optionsList={props.salesTax}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractSalesTax}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                name='contractCustomerContact'
                label={localConstant.contract.CUSTOMER_CONTRACT_CONTACT}
                labelClass="mandate"
                optionValue='contactPersonName'
                optionName="contactPersonName"
                optionsList={props.customerContractContact}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractCustomerContact}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.contract.WITH_HOLDING_TAX}
                optionValue='taxName'
                optionName="taxName"
                name="contractWithHoldingTax"
                optionsList={props.withHoldingTax}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractWithHoldingTax}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.contract.CUSTOMER_CONTRACT_ADDRESS}
                labelClass="mandate"
                optionValue='address'
                optionName="address"
                name="contractCustomerContactAddress"
                optionsList={props.customerContractAddress}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractCustomerContactAddress}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.contract.INVOICE_CURRENCY}
                labelClass="mandate"
                optionValue='code'
                optionName="code"
                name="contractInvoicingCurrency"
                optionsList={props.invoiceCurrency}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractInvoicingCurrency}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.contract.CUSTOMER_INVOICE_CONTACT}
                labelClass="mandate"
                optionValue='contactPersonName'
                optionName="contactPersonName"
                name="contractCustomerInvoiceContact"
                optionsList={props.customerContractContact}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractCustomerInvoiceContact}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.contract.INVOICE_GROUPING}
                labelClass="mandate"
                optionValue='value'
                optionName="value"
                name="contractInvoiceGrouping"
                optionsList={props.invoiceGrouping}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractInvoiceGrouping}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.contract.INVOICE_ADDRESS}
                labelClass="mandate"
                optionValue='address'
                optionName="address"
                name="contractCustomerInvoiceAddress"
                optionsList={props.customerContractAddress}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractCustomerInvoiceAddress}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.contract.INVOICE_FOOTER}
                labelClass="mandate"
                optionValue='msgIdentifier'
                optionName="msgIdentifier"
                name="contractInvoiceFooterIdentifier"
                optionsList={props.invoiceFooter}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractInvoiceFooterIdentifier}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.contract.INVOICE_REMITTANCE_TEXT}
                labelClass="mandate"
                name='contractInvoiceRemittanceIdentifier'
                optionValue='msgIdentifier'
                optionName="msgIdentifier"
                optionsList={props.invoiceRemittanceText}
                disabled={props.interactionMode}
                defaultValue={props.fetchContractInfo.contractInvoiceRemittanceIdentifier}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mt-2 mb-0">
            <CustomInput hasLabel={true}
                label={localConstant.contract.INVOICE_INSTRUCTION_NOTES}
                divClassName="col"
                type='textarea'
                // maxLength={fieldLengthConstants.Contract.invoiceDefaults.INVOICE_NOTES_MAXLENGTH}
                inputName='InvoiceHeader'
                colSize='s12'
                inputClass="customInputs textAreaInView"
                name="contractInvoiceInstructionNotes"
                readOnly = {props.interactionMode}
                value={invoiceInstructionNotes}
                onValueChange={props.onBlur}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                label={localConstant.contract.INVOICE_FREE_TEXT}
                divClassName="col"
                type='textarea'
                inputName='InvoiceHeader'
                colSize='s12'
                inputClass="customInputs textAreaInView"
                name="contractInvoiceFreeText"
                maxLength={fieldLengthConstants.Contract.invoiceDefaults.INVOICE_FREE_TEXT_MAXLENGTH}
                readOnly = {props.interactionMode}
                value={invoiceFreeText}
                onValueChange={props.onBlur}
            />
        </div>
    </CardPanel>
    );
};

const DefaultInvoiceReferences = (props) => (
    <div className="row">
        <div className="col s7">
            <p className="col pl-0 bold">{localConstant.contract.DEFAULT_INVOICE_REFERENCE}</p>
            
            <div className="customCard"> <ReactGrid gridColData={HeaderData.ContractInvoicingDefaultHeader} gridRowData={props.defaultInvoiceRefernces} onRef={props.onInvoiceDelete} onRowDragEnd={props.onRowDragEnd} paginationPrefixId={localConstant.paginationPrefixIds.invoiceReference}/></div>
            {props.pageMode!== localConstant.commonConstants.VIEW &&  <div className="col s12 right-align mt-2">
                    <button  onClick={props.invoiceReferenceCreateHandler} className="btn-small waves-effect" disabled={props.interactionMode}>{localConstant.commonConstants.ADD}</button>
                    <button onClick={props.onInvoiceDefultDeleteClick} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={props.interactionMode}>{localConstant.commonConstants.DELETE}</button>
                </div>}          
        </div>
        <div className="mt-2 col s5">
            <p className="col mt-2 pl-0 bold">{localConstant.contract.DEFAULT_INVOICE_ATTACHMNET_TYPES}</p>
          
            <div className="customCard"> <ReactGrid gridColData={HeaderData.ContractAttachmentTypesHeader} gridRowData={props.defaultInvoiceAttachmentTypes} onRef={props.onAttachmentDelete} paginationPrefixId={localConstant.paginationPrefixIds.invoiceAttachmentTypes}/></div>
            {props.pageMode!== localConstant.commonConstants.VIEW && <div className="col s12 right-align mt-2">
                    <button onClick={props.invoiceAccountTypeCreateHandler} className="btn-small waves-effect" disabled={props.interactionMode}>{localConstant.commonConstants.ADD}</button>
                    <button onClick={props.onAttachmentTypeDeleteClick} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={props.interactionMode}>{localConstant.commonConstants.DELETE}</button>
                </div>}
           
        </div>
    </div>
);
const InvoicingDefaultModal = (props) => (
    <div>
        <LabelwithValue colSize="s12" divClassName='col loadedDivision' />
        <div className="row">
            <CustomInput
                hasLabel={true}
                type='select'
                divClassName='col loadedDivision'
                label={localConstant.modalConstant.REFERENCE_TYPE}
                colSize='s12'
                optionsList={props.referenceType}
                optionName='name'
                optionValue="name"
                labelClass="mandate"
                name="referenceType"
                className="browser-default customInputs"
                onSelectChange={props.handlerChange}
                interactionMode={props.interactionMode}
                defaultValue={props.editInvoiceReferences && props.editInvoiceReferences.referenceType}
            />
        </div>
        <div className="row">
            <CustomInput
                type='switch'
                colSize="s4"
                switchLabel={localConstant.modalConstant.ASSIGNMENT}
                isSwitchLabel={true}
                switchName="isVisibleToAssignment"
                id="isVisibleToAssignment"
                className="lever"
                onChangeToggle={props.handlerChange}
                checkedStatus={false}
                interactionMode={props.interactionMode}
                checkedStatus={props.editInvoiceReferences && props.editInvoiceReferences.isVisibleToAssignment}
            />
            <CustomInput
                type='switch'
                colSize="s4"
                switchLabel={localConstant.modalConstant.VISIT}
                isSwitchLabel={true}
                switchName="isVisibleToVisit"
                id="isVisibleToVisit"
                className="lever"
                onChangeToggle={props.handlerChange}
                checkedStatus={false}
                interactionMode={props.interactionMode}
                checkedStatus={props.editInvoiceReferences && props.editInvoiceReferences.isVisibleToVisit}
            />
            <CustomInput
                type='switch'
                colSize="s4"
                switchLabel={localConstant.modalConstant.TIMESHEET}
                isSwitchLabel={true}
                switchName="isVisibleToTimesheet"
                id="isVisibleToTimesheet"
                className="lever"
                onChangeToggle={props.handlerChange}
                checkedStatus={false}
                interactionMode={props.interactionMode}
                checkedStatus={props.editInvoiceReferences && props.editInvoiceReferences.isVisibleToTimesheet}
            />
        </div>
    </div>
);

const InvoiceAttachmentTypeModal = (props) => (
    <CustomInput
        hasLabel={true}
        type='select'
        divClassName='col'
        label={localConstant.modalConstant.DOCUMENT_TYPE}
        colSize='s12'
        optionsList={props.documentType}
        optionName='name'
        optionValue="name"
        labelClass="mandate"
        name="documentType"
        className="browser-default customInputs"
        onSelectChange={props.handlerChange}
        interactionMode={props.interactionMode}
        defaultValue={props.editAttachmentTypes && props.editAttachmentTypes.documentType}
    />
);

class InvoicingDefaults extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isOpen: false,
        };

        this.defaultInvoiceReferenceAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceReference,
                btnID: "cancelInvoiceReference",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createInvoiceReference,
                btnID: "createInvoiceReference",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];

        this.defaultInvoiceReferenceEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceReference,
                btnID: "cancelInvoiceReference",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.editInvoiceReferenceType,
                btnID: "editInvoiceReference",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];

        this.defaultInvoiceAttachmentTypesAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceAttachmentTypes,
                btnID: "cancelInvoiceAttachmentTypes",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createInvoiceAttachmentTypes,
                btnID: "createInvoiceAttachmentTypes",
                btnClass: "waves-effect waves-teal btn-small",
                showbtn: true
            }
        ];

        this.defaultInvoiceAttachmentTypesEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceAttachmentTypes,
                btnID: "cancelInvoiceAttachmentTypes",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.editInvoiceAttachmentTypes,
                btnID: "editInvoiceAttachmentTypes",
                btnClass: "waves-effect waves-teal btn-small",
                showbtn: true
            }
        ];
    }
   
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        const isFetchInvoicingDefaults = (tabInfo && tabInfo.componentRenderCount === 0)?true:false;
        if (isFetchInvoicingDefaults) {
            if (this.props.currentPage === "Create Contract") {
                if (this.props.generalDetailsCreateContractCustomerDetails) {
                    this.props.actions.FetchInvoicingDefaults();
                }
            }
            else {
                this.props.actions.FetchInvoicingDefaults();
            }
        }
    };

    handlerChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    invoicingDefaultsHandlerChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
        if (this.props.currentPage === "Create Contract") {
            this.updatedData["recordStatus"] = "N";
        }
        else {
            this.updatedData["recordStatus"] = "M";
        }
       let count=0;
        this.props.taxes.map(tax=>{
                    if(tax.taxType ==="W")
                    count=1;
                });
                if(count!==1)
                  {
                    this.updatedData["contractWithHoldingTax"] = '';
                 }
        this.props.actions.AddUpdateInvoicingDefaults(this.updatedData);
        this.updatedData = {};
    }

    /**
     * Invoice Reference OnClick Handler
     */
    InvoiceReferenceCreateHandler = () => {
        this.props.actions.InvoiceReferenceEditCheck(false);
        this.props.actions.InvoiceReferenceModalState(true);
    }

    /**
     * Create or Add the new Invoice Reference 
     */
    createInvoiceReference = (e) => {
        e.preventDefault();
        let isExist = false;
        let displayOrder = 1;
        if (this.updatedData.referenceType === undefined || this.updatedData.referenceType === "") {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_FROM_REFERENCE_TYPE,'warningToast idReferenceTypeReq');
        }
        else {
            if (this.updatedData.referenceType !== null || this.updatedData.referenceType !== undefined) {
                isExist = this.props.defaultInvoiceRefernces.map(invoice => {
                    if (invoice.referenceType === this.updatedData.referenceType && invoice.recordStatus !== "D") {
                        displayOrder = Math.max(invoice.displayOrder) + 1;
                        return !isExist;
                    }
                    else {
                        displayOrder = Math.max(invoice.displayOrder) + 1;
                        return isExist;
                    }
                });
            }
            if (isExist.includes(true)) {
                IntertekToaster(localConstant.contract.REFERENCE_TYPE_ALREADY_EXISTS,'warningToast idReferenceExistVal');
            }
            else {
                if (!isEmpty(this.props.selectedContract.contractNumber)) {
                    this.updatedData["contractNumber"] = this.props.selectedContract.contractNumber;
                }
                this.updatedData["recordStatus"] = "N";
                this.updatedData["createdBy"] = this.props.loggedInUser;
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["contractInvoiceReferenceTypeId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                //this.updatedData["displayOrder"] = displayOrder;
                this.updatedData["displayOrder"] = this.props.defaultInvoiceRefernces.length+1;
                this.updatedData["isVisibleToTimesheet"] = this.updatedData.isVisibleToTimesheet === true ? true : false;
                this.updatedData["isVisibleToAssignment"] = this.updatedData.isVisibleToAssignment === true ? true : false;
                this.updatedData["isVisibleToVisit"] = this.updatedData.isVisibleToVisit === true ? true : false;
                const invoiceReference = Object.assign([], this.props.defaultInvoiceRefernces);
                invoiceReference.push(this.updatedData);
                this.props.actions.AddNewInvoiceDefault(invoiceReference);
                this.updatedData = {};
                this.props.actions.InvoiceReferenceModalState(false);
            }
        }
    }

    /**
     * Edit the existing Invoice Reference
     */
    editInvoiceReferenceType = (e) => {
        e.preventDefault();
        let isExist = false;
        const updatedInvoiceReference = Object.assign({},this.props.editInvoiceReferences,this.updatedData);
        if (updatedInvoiceReference.referenceType === undefined || updatedInvoiceReference.referenceType === "") {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_FROM_REFERENCE_TYPE,'warningToast idReferenceTypeReq');
            return false;
        }
        if (this.updatedData.referenceType) {
            if (this.props.defaultInvoiceRefernces) {
                isExist = this.props.defaultInvoiceRefernces.map(invoice => {
                    //edittedrow is also added for validation
                    if (invoice.referenceType !== "" && invoice.referenceType === this.updatedData.referenceType && invoice.referenceType !== "D" && invoice.referenceType !==this.props.editInvoiceReferences.referenceType ) {
                        return !isExist;
                    }
                    else {
                        return isExist;
                    }
                });
            }
        }
        else {
            if (this.props.defaultInvoiceRefernces) {
                isExist = this.props.defaultInvoiceRefernces.map(invoice => {
                    if (invoice.referenceType !== null && invoice.referenceType === this.props.editInvoiceReferences.referenceType && invoice.referenceType !== "D") {
                        return isExist;
                    }
                });
            }
        }
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.contract.REFERENCE_TYPE_ALREADY_EXISTS,'warningToast idReferenceExists');
        }
        else {
            if (this.props.editInvoiceReferences.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.props.actions.UpdateInvoiceDefault(this.updatedData);
            this.updatedData = {};
            this.props.actions.InvoiceReferenceModalState(false);
        }
    }

    /**
     * Closing the popup of Invoice Reference
     */
    cancelInvoiceReference = (e) => {
        e.preventDefault();
        this.props.actions.InvoiceReferenceModalState(false);
        this.updatedData = {};
    }

    /**
     * Account Type OnClick Handler
     */
    InvoiceAccountTypeCreateHandler = () => {
        this.props.actions.InvoiceAttachmentTypesEditCheck(false);
        this.props.actions.InvoiceAttachmentTypesModalState(true);
    }

    /**
     * Create or Add the new Invoice Attachment Types
     */
    createInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        let isExist = false;
        if (this.updatedData.documentType === undefined || this.updatedData.documentType === "") {
            IntertekToaster(localConstant.contract.SELECT_A_DOCUMENT_TYPE,'warningToast idDocumentTypeReq');
        }
        else {
            if (this.updatedData.documentType !== null || this.updatedData.documentType !== undefined) {
                isExist = this.props.defaultInvoiceAttachmentTypes.map(attachmentType => {
                    if (attachmentType.documentType === this.updatedData.documentType && attachmentType.recordStatus !== "D") {
                        return !isExist;
                    }
                    else {
                        return isExist;
                    }
                });
            }
            if (isExist.includes(true)) {
                IntertekToaster(localConstant.contract.ATTACHMENT_TYPE_ALREADY_EXIST,'warningToast idAttachmentExistVal');
            }
            else {
                if (!isEmpty(this.props.selectedContract.contractNumber)) {
                    this.updatedData["contractNumber"] = this.props.selectedContract.contractNumber;
                }
                this.updatedData["recordStatus"] = "N";
                this.updatedData["createdBy"] = this.props.loggedInUser;
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["contractInvoiceAttachmentId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["documentType"] = this.updatedData.documentType;
                this.props.actions.AddAttachmentTypes(this.updatedData);
                this.updatedData = {};
                this.props.actions.InvoiceAttachmentTypesModalState(false);
            }
        }
    }

    /**
     * Edit the existing Invoice Attachment Types
     */
    editInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        let isExist = false;
        const updatedInvoiceAttachment = Object.assign({},this.props.defaultInvoiceAttachmentTypes,this.updatedData);
        if (updatedInvoiceAttachment.documentType === undefined || updatedInvoiceAttachment.documentType === "") {
            IntertekToaster(localConstant.contract.SELECT_A_DOCUMENT_TYPE,'warningToast idDocumentTypeReq');
            return false;
        }
        if (this.updatedData.documentType) {
            if (this.props.defaultInvoiceAttachmentTypes) {
                isExist = this.props.defaultInvoiceAttachmentTypes.map(attachment => {
                   //edittedrow is also added for validation
                    if (attachment.documentType !== "" && attachment.documentType === this.updatedData.documentType && attachment.documentType !== "D" && attachment.documentType!==this.props.editAttachmentTypes.documentType) {
                        return !isExist;
                    }
                    else {
                        return isExist;
                    }
                });
            }
        }
        else {
            if (this.props.defaultInvoiceAttachmentTypes) {
                isExist = this.props.defaultInvoiceAttachmentTypes.map(attachment => {
                    if (attachment.documentType !== null && attachment.documentType === this.props.editAttachmentTypes.documentType && attachment.documentType !== "D") {
                        return isExist;
                    }
                });
            }
        }
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.contract.ATTACHMENT_TYPE_ALREADY_EXIST,'warningToast idAttachmentExist');
        }
        else {
            if (this.props.editAttachmentTypes.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.props.actions.UpdateAttachmentTypes(this.updatedData);
            this.updatedData = {};
            this.props.actions.InvoiceAttachmentTypesModalState(false);
        }
    }

    /**
     * Closing the popup of Invoice Attachment Types
     */
    cancelInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        this.props.actions.InvoiceAttachmentTypesModalState(false);
        this.updatedData = {};
    }

    /**
     * OnClick handler for deleting the selected records of Invoice References
     */
    invoiceDefaultDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.INVOICE_DEFAULT_MESSAGE,
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
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast idOneRowSelectReq');
        }
    }

    /**
     * Delete the selected records of Invoice References
     */
    deleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteInvoiceDefault(selectedData);
        this.props.actions.HideModal();
    }
    
    /**
     * Row Drag handler for Default Invoice Reference(s) 
     */
    onRowDragEnd(e) {
        //D691 issue 5
        if(this.props.pageMode !== localConstant.commonConstants.VIEW) {
            const id=e.node.id;
            const gridData = [];
            const eIndex=e.overNode && e.overNode.id;
            e.api.rowModel.rowsToDisplay.forEach((iteratedValue) => {
                if(iteratedValue.id=== id){
                    const ordervalue= e.overIndex;
                    iteratedValue.data.displayOrder= ordervalue === -1 ? 1:ordervalue+1;
                    if(iteratedValue.data.recordStatus!=='N')
                      iteratedValue.data.recordStatus='M';  //Changes for D1015
                }
                else if(iteratedValue.id < id && iteratedValue.id >= eIndex){
                    iteratedValue.data.displayOrder=parseInt(iteratedValue.data.displayOrder)+1;
                    if(iteratedValue.data.recordStatus!=='N')
                       iteratedValue.data.recordStatus='M';
                }
                else if(iteratedValue.id >id && iteratedValue.id <= eIndex){
                    iteratedValue.data.displayOrder=parseInt(iteratedValue.data.displayOrder)-1;
                    if(iteratedValue.data.recordStatus!=='N')
                        iteratedValue.data.recordStatus='M';
                }
                gridData.push(iteratedValue.data);
            });
            this.props.actions.AddNewInvoiceDefault(gridData);
        }
    }

    /**
     * OnClick handler for deleting the selected records of Attachment Types
     */
    attachmentTypeDeleteClickHandler = () => {
        const selectedRecords = this.secondChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ATTACHMENT_TYPE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelectedAssignmentType,
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
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast idOneRecordSelectReq');
        }
    }

    /**
     * Delete the selected records of Attachment Types
     */
    deleteSelectedAssignmentType = () => {
        const selectedData = this.secondChild.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteAttachmentTypes(selectedData);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    render() {
        const { defaultInvoiceRefernces, defaultInvoiceAttachmentTypes,customerContractContact, editInvoiceReferences, taxes, editAttachmentTypes, contractDocumentTypeMasterData, invoiceRemittanceandFooterText, referenceType,
            invoiceCurrency } = this.props;
        const tax = taxes;
        const invoicingGrouping = [
            { value: 'Project' },
            { value: 'Assignment' },
            { value: 'Reference' }
        ];
        const invoiceReferenceDefault = [];
        if(defaultInvoiceRefernces){
            const invoiceReference = [ ...defaultInvoiceRefernces ];
            invoiceReference.sort((a,b)=>a.displayOrder - b.displayOrder);
            invoiceReference.map((item,i)=>{ invoiceReferenceDefault.push(item); });
        }
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        let contractInfo = [];
        if (this.props.invoicingContractInfo) {
            contractInfo = this.props.invoicingContractInfo;
        }
        //Changes for D-1233 -Start
        const invoiceRemittances=invoiceRemittanceandFooterText && 
                this.props.currentPage === "Create Contract" ?
                invoiceRemittanceandFooterText.invoiceRemittances && invoiceRemittanceandFooterText.invoiceRemittances.filter(x=>x.isActive===true):
                (invoiceRemittanceandFooterText.invoiceRemittances && invoiceRemittanceandFooterText.invoiceRemittances.filter(x=>x.isActive===true || x.msgIdentifier === contractInfo.contractInvoiceRemittanceIdentifier)); //Sanity D-160 Fix
        //Changes for D-1233 -End
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard customMargin">
                    {this.props.isInvoiceReferenceModalOpen &&
                        <Modal title={this.props.isInvoiceReferenceEdit ? localConstant.contract.invoicingDefaults.EDIT_INOVICE_REFERENCE:localConstant.contract.invoicingDefaults.ADD_INVOICE_REFERENCE} modalId="InvoicingDefaultModal" formId="invoicingDefaultModal" modalClass="popup-position" buttons={this.props.isInvoiceReferenceEdit ? this.defaultInvoiceReferenceEditButtons : this.defaultInvoiceReferenceAddButtons} isShowModal={this.props.isInvoiceReferenceModalOpen} disabled={this.props.interactionMode}>
                            <InvoicingDefaultModal referenceType={referenceType} editInvoiceReferences={editInvoiceReferences} switchHandlerChange={this.switchHandlerChange} handlerChange={this.handlerChange} disabled={this.props.interactionMode} />
                        </Modal>}
                    {this.props.isInvoiceAttachmentTypesModalOpen &&
                        <Modal title={this.props.isInvoiceAttachmentTypesEdit ? localConstant.contract.invoicingDefaults.EDIT_INVOICE_ATTACHMENT:localConstant.contract.invoicingDefaults.ADD_INVOICE_ATTACHMENT} modalId="InvoiceAttachmentModal" formId="InvoiceAttachmentModal" modalClass="popup-position" buttons={this.props.isInvoiceAttachmentTypesEdit ? this.defaultInvoiceAttachmentTypesEditButtons : this.defaultInvoiceAttachmentTypesAddButtons} isShowModal={this.props.isInvoiceAttachmentTypesModalOpen} disabled={this.props.interactionMode}>
                            <InvoiceAttachmentTypeModal documentType={contractDocumentTypeMasterData} handlerChange={this.handlerChange} editAttachmentTypes={editAttachmentTypes} />
                        </Modal>}
                    <InvoicingDefault
                        customerContractContact={arrayUtil.sort(customerContractContact,'contactPersonName','asc')}
                        salesTax={taxes && (this.props.currentPage === "Create Contract" ? taxes.filter(x => x.taxType === "S" && x.isActive ===true):taxes.filter(x => x.taxType === "S"))}     //D112 Changes
                        withHoldingTax={taxes && taxes.filter(x => x.taxType === "W" && x.isActive ===true)}
                        invoicePaymentTerms={this.props.invoicePaymentTerms}
                        customerContractAddress={this.props.customerContractAddress}
                        invoiceCurrency={invoiceCurrency}
                        invoiceGrouping={invoicingGrouping}
                        invoiceRemittanceText={invoiceRemittances} //Changes for D-1233
                        invoiceFooter={invoiceRemittanceandFooterText && invoiceRemittanceandFooterText.invoiceFooters}
                        fetchContractInfo={contractInfo}
                        interactionMode={this.props.interactionMode}
                        onChange={this.invoicingDefaultsHandlerChange}
                        onBlur={this.invoicingDefaultsHandlerChange}
                    />
                    <DefaultInvoiceReferences
                        defaultInvoiceRefernces={invoiceReferenceDefault && invoiceReferenceDefault.filter(x => x.recordStatus !== "D")}
                        defaultInvoiceAttachmentTypes={defaultInvoiceAttachmentTypes && defaultInvoiceAttachmentTypes.filter(x => x.recordStatus !== "D")}
                        invoiceReferenceCreateHandler={this.InvoiceReferenceCreateHandler}
                        invoiceAccountTypeCreateHandler={this.InvoiceAccountTypeCreateHandler}
                        onInvoiceDefultDeleteClick={this.invoiceDefaultDeleteClickHandler}
                        onRowDragEnd={this.onRowDragEnd.bind(this)}
                        onAttachmentTypeDeleteClick={this.attachmentTypeDeleteClickHandler}
                        onInvoiceDelete={ref => { this.child = ref; }}
                        onAttachmentDelete={ref => { this.secondChild = ref; }}
                        interactionMode={this.props.interactionMode}
                        pageMode={this.props.pageMode}
                    />
                </div>
            </Fragment>
        );
    }
}

InvoicingDefaults.propTypes = {
    // InvoicingDefaultsDetails: PropTypes.array.isRequired, // Not used
};

InvoicingDefaults.defaultprops = {
    // InvoicingDefaultsDetails: [], // Not used
};

export default InvoicingDefaults;