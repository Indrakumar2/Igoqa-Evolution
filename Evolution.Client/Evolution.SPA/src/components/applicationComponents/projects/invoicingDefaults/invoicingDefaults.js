import React, { Component, Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, isEmpty, bindAction } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { sortCurrency } from '../../../../utils/currencyUtil.js';
import arrayUtil from '../../../../utils/arrayUtil';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

const InvoicingDefault = (props) => (
    <CardPanel className="white lighten-4 black-text" title={localConstant.contract.INVOICING_DEFAULTS} colSize="s12">
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
                name="projectInvoicePaymentTerms"
                optionsList={props.invoicePaymentTerms}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectInvoicePaymentTerms}
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
                name="projectSalesTax"
                optionsList={props.salesTax}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectSalesTax}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                name='projectCustomerContact'
                label={localConstant.project.invoicingDefaults.CUSTOMER_PROJECT_CONTACT}
                labelClass="mandate"
                optionValue='contactPersonName'
                optionName="contactPersonName"
                optionsList={props.customerContractContact}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectCustomerContact}
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
                name="projectWithHoldingTax"
                optionsList={props.withHoldingTax}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectWithHoldingTax}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.project.invoicingDefaults.PROJECT_COMPANY_ADDRESS}
                labelClass="mandate"
                optionValue='address'
                optionName="address"
                name="projectCustomerContactAddress"
                optionsList={props.customerContractAddress}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectCustomerContactAddress}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.project.invoicingDefaults.DEFAULT_INVOICE_CURRENCY}
                labelClass="mandate"
                optionValue='code'
                optionName="code"
                name="projectInvoicingCurrency"
                optionsList={props.invoiceCurrency}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectInvoicingCurrency}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s9'
                label={localConstant.project.invoicingDefaults.CUSTOMER_INVOICE_CONTACT}
                labelClass="mandate"
                optionValue='contactPersonName'
                optionName="contactPersonName"
                name="projectCustomerInvoiceContact"
                optionsList={props.customerContractContact}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectCustomerInvoiceContact}
                onSelectChange={props.onChange}
            />
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s3'
                label={localConstant.project.invoicingDefaults.DEFAULT_INVOICE_GROUPING}
                labelClass="mandate"
                optionValue='value'
                optionName="value"
                name="projectInvoiceGrouping"
                optionsList={props.invoiceGrouping}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectInvoiceGrouping}
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
                name="projectCustomerInvoiceAddress"
                optionsList={props.customerContractAddress}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectCustomerInvoiceAddress}
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
                name="projectInvoiceFooterIdentifier"
                optionsList={props.invoiceFooter}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectInvoiceFooterIdentifier}
                onSelectChange={props.onChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                type='select'
                divClassName='col'
                className="browser-default"
                colSize='s6'
                label={localConstant.contract.INVOICE_REMITTANCE_TEXT}
                labelClass="mandate"
                name='projectInvoiceRemittanceIdentifier'
                optionValue='msgIdentifier'
                optionName="msgIdentifier"
                optionsList={props.invoiceRemittanceText}
                disabled={props.interactionMode}
                defaultValue={props.fetchProjectInfo.projectInvoiceRemittanceIdentifier}
                onSelectChange={props.onChange}
            />
            <CustomInput 
                hasLabel={true}
                divClassName='col'
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                labelClass="customLabel"
                colSize='s3'
                label={localConstant.project.invoicingDefaults.CUSTOMER_VAT_TAX_REG_NO}
                name="euvatPrefix"
                readOnly={true}
                value={props.fetchProjectInfo.euvatPrefix}
            />
            <CustomInput 
                hasLabel={false}
                type='text'
                divClassName='mt-4x'
                dataValType='valueText'
                inputClass="customInputs"
                colSize='s3'
                name="vatRegistrationNumber"
                readOnly={true}
                value={props.fetchProjectInfo.vatRegistrationNumber}
            />
        </div>
        <div className="row mt-2 mb-0">
            <CustomInput hasLabel={true}
                label={localConstant.project.invoicingDefaults.INVOICE_NOTES}
                divClassName="col"
                type='textarea'
                colSize='s12'
                inputClass="customInputs"
                name="projectInvoiceInstructionNotes"
                // disabled={props.interactionMode}
                // maxLength={ fieldLengthConstants.Project.invoiceDefaults.INVOICE_NOTES_MAXLENGTH}
                readOnly = {props.interactionMode}
                value={!isEmpty(props.fetchProjectInfo.projectInvoiceInstructionNotes) ? props.fetchProjectInfo.projectInvoiceInstructionNotes : ""}
                onValueChange={props.onBlur}
                onValueBlur={props.onBlur}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                label={localConstant.contract.INVOICE_FREE_TEXT}
                divClassName="col"
                type='textarea'
                colSize='s12'
                maxLength={fieldLengthConstants.Project.invoiceDefaults.INVOICE_FREE_TEXT_MAXLENGTH}
                inputClass="customInputs"
                name="projectInvoiceFreeText"
                // disabled={props.interactionMode}
                readOnly = {props.interactionMode}
                value={!isEmpty(props.fetchProjectInfo.projectInvoiceFreeText) ? props.fetchProjectInfo.projectInvoiceFreeText : ""}
                onValueChange={props.onBlur}
                onValueBlur={props.onBlur}
            />
        </div>
    </CardPanel>
);

const DefaultInvoiceReferences = (props) => (
    <div className="row">
        <div className="col s7">
            <p className="col pl-0 bold">{localConstant.contract.DEFAULT_INVOICE_REFERENCE}</p>
            <CardPanel className="col white lighten-4 black-text s12" colSize="s12">
          
             <ReactGrid gridColData={HeaderData.ProjectInvoicingDefaultHeader}
                    gridRowData={props.defaultInvoiceRefernces}
                    onRef={props.onInvoiceDelete}
                    onRowDragEnd={props.onRowDragEnd} 
                    paginationPrefixId={localConstant.paginationPrefixIds.projectInvoiceReference}/>
          
             {props.pageMode!== localConstant.commonConstants.VIEW &&
                 <div className="right-align mt-2 col s12">
                    <button onClick={props.invoiceReferenceCreateHandler} className="btn-small waves-effect" disabled={props.interactionMode}>{localConstant.commonConstants.ADD}</button>
                    <button 
                        onClick={props.onInvoiceDefultDeleteClick}
                        className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn "
                        disabled={isEmpty(props.defaultInvoiceRefernces) || props.interactionMode} >
                        {localConstant.commonConstants.DELETE}</button>
                </div>}
            </CardPanel>
        </div>
        <div className="mt-2 col s5">
            <p className="col mt-2 pl-0 bold">{localConstant.contract.DEFAULT_INVOICE_ATTACHMNET_TYPES}</p>
            <CardPanel className="white lighten-4 black-text left col s12" colSize="s12 pl-0">
          
            <ReactGrid gridColData={HeaderData.ProjectAttachmentTypesHeader} 
                gridRowData={props.defaultInvoiceAttachmentTypes}
                onRef={props.onAttachmentDelete} 
                paginationPrefixId={localConstant.paginationPrefixIds.projectAttachmentTypes}/>
           
               {props.pageMode!== localConstant.commonConstants.VIEW &&  <div className="right-align mt-2 col s12">
                    <button onClick={props.invoiceAccountTypeCreateHandler} className="btn-small waves-effect" disabled={props.interactionMode} >{localConstant.commonConstants.ADD}</button>
                    <button
                        onClick={props.onAttachmentTypeDeleteClick}
                        className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn "
                        disabled={isEmpty(props.defaultInvoiceAttachmentTypes) || props.interactionMode} >
                        {localConstant.commonConstants.DELETE}</button>
             </div> }
            </CardPanel>
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
            isInvoiceReferenceModalOpen: false,
            isInvoiceReferenceEditMode: false,
            isInvoiceAttachmentModalOpen: false,
            isInvoiceAttachmentEditMode: false,
        };
    }

    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0)
            this.props.actions.FetchInvoicingDefaults(this.props.invoicingProjectInfo.assignmentParentContractCompanyCode); //Changes for D-1174 
    }

    editReferenceRowHandler = (data) => {
        this.setState((state) => {
            return {
                isInvoiceReferenceModalOpen: !state.isInvoiceReferenceModalOpen,
                isInvoiceReferenceEditMode: true
            };
        });
        this.editedRowData = data;
    }

    editAttachmentRowHandler = (data) => {
        this.setState((state) => {
            return {
                isInvoiceAttachmentModalOpen: !state.isInvoiceAttachmentModalOpen,
                isInvoiceAttachmentEditMode: true
            };
        });
        this.editedRowData = data;
    }

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
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        this.props.actions.AddUpdateInvoicingDefaults(this.updatedData);
        this.updatedData = {};
    }
    /**
     * Invoice Reference OnClick Handler
     */
    InvoiceReferenceCreateHandler = () => {
        this.setState({
            isInvoiceReferenceModalOpen: true,
            isInvoiceReferenceEditMode: false
        });
        this.editedRowData = {};
    }

    /**
     * Create or Add the new Invoice Reference 
     */
    createInvoiceReference = (e) => {
        e.preventDefault();
        let isExist = false;
        let displayOrder = 1;
        if (isEmpty(this.updatedData.referenceType)) {
            IntertekToaster(localConstant.warningMessages.PLEASE_SELECT_REFERENCE_TYPE, 'warningToast idProjReferenceTypeReq');
        }
        else {
            if (!isEmpty(this.updatedData.referenceType)) {
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
                IntertekToaster(localConstant.warningMessages.REFERENCE_TYPE_ALREADY_EXIST, 'warningToast idProjReferenceExistVal');
            }
            else {
                // if (!isEmpty(this.props.selectedContract.contractNumber)) {
                //     this.updatedData["projectNumber"] = this.props.selectedContract.contractNumber;
                // }

                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["projectInvoiceReferenceTypeId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["displayOrder"] = this.props.defaultInvoiceRefernces.length+1;
                this.updatedData["isVisibleToTimesheet"] = this.updatedData.isVisibleToTimesheet == true ? true : false;
                this.updatedData["isVisibleToAssignment"] = this.updatedData.isVisibleToAssignment == true ? true : false;
                this.updatedData["isVisibleToVisit"] = this.updatedData.isVisibleToVisit == true ? true : false;
                const invoiceReference = Object.assign([], this.props.defaultInvoiceRefernces);
                invoiceReference.push(this.updatedData);
                this.props.actions.AddInvoiceDefault(invoiceReference);
                this.updatedData = {};
               // this.editedRowData = {};
                this.setState({
                    isInvoiceReferenceModalOpen: false
                });
            }
        }
    }

    /**
     * Edit the existing Invoice Reference
     */
    editInvoiceReferenceType = (e) => {        
        e.preventDefault();
        let isExist = false;
        // if (this.updatedData.referenceType) {
        //     if (this.props.defaultInvoiceRefernces) {
        //         isExist = this.props.defaultInvoiceRefernces.map(invoice => {
        //             if (invoice.referenceType !== "" && invoice.referenceType === this.updatedData.referenceType && invoice.referenceType !== "D") {
        //                 return !isExist;
        //             }
        //             else {
        //                 return isExist;
        //             }
        //         });
        //     }
        // }
        // else {
        //     if (this.props.defaultInvoiceRefernces) {
        //         isExist = this.props.defaultInvoiceRefernces.map(invoice => {
        //             if (!isEmpty(invoice.referenceType)  && invoice.referenceType === this.editedRowData.referenceType && invoice.referenceType !== "D") {
        //                 return isExist;
        //             }
        //         });
        //     }
        // }
        if (this.updatedData.referenceType == '') {
            IntertekToaster(localConstant.warningMessages.PLEASE_SELECT_REFERENCE_TYPE, 'warningToast EditidProjReferenceTypeReq');
            return false;
        }
        isExist = this.props.defaultInvoiceRefernces.map(iteratedValue => {
            if (iteratedValue.referenceType === this.updatedData.referenceType && iteratedValue.recordStatus !== "D" && iteratedValue.referenceType !== this.editedRowData.referenceType) {
                return true;
            }
        });
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.warningMessages.REFERENCE_TYPE_ALREADY_EXIST, 'warningToast idProjReferenceExists');
        }
        else {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.props.actions.UpdateInvoiceDefault(this.updatedData, this.editedRowData);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isInvoiceReferenceModalOpen: false
            });
        }
    }

    /**
     * Closing the popup of Invoice Reference
     */
    cancelInvoiceReference = (e) => {
		e.preventDefault();
        this.setState({
            isInvoiceReferenceModalOpen: false
        });
        this.updatedData = {};
        this.editedRowData = {};
    }

    /**
     * Account Type OnClick Handler
     */
    InvoiceAccountTypeCreateHandler = () => {
        this.setState({
            isInvoiceAttachmentModalOpen: true,
            isInvoiceAttachmentEditMode: false
        });
        this.editedRowData = {};
    }

    /**
     * Create or Add the new Invoice Attachment Types
     */
    createInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        let isExist = false;
        if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.warningMessages.PLEASE_SELECT_DOCUMENT_TYPE, 'warningToast idProjDocumentTypeReq');
        }
        else {
            if (!isEmpty(this.updatedData.documentType)) {
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
                IntertekToaster(localConstant.warningMessages.ATTACHMENT_TYPE_ALREADY_EXIST, 'warningToast idProjAttachmentExistVal');
            }
            else {
                // if (!isEmpty(this.props.selectedContract.contractNumber)) {
                //     this.updatedData["contractNumber"] = this.props.selectedContract.contractNumber;
                // }
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.updatedData["projectInvoiceAttachmentId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["documentType"] = this.updatedData.documentType;
                this.props.actions.AddAttachmentTypes(this.updatedData);
                this.updatedData = {};
                this.editedRowData = {};
                this.setState({
                    isInvoiceAttachmentModalOpen: false
                });
            }
        }
    }

    /**
     * Edit the existing Invoice Attachment Types
     */
    editInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        let isExist = false;
        // if (this.updatedData.documentType) {
        //     if (this.props.defaultInvoiceAttachmentTypes) {
        //         isExist = this.props.defaultInvoiceAttachmentTypes.map(attachment => {
        //             if (!isEmpty(attachment.documentType) && attachment.documentType === this.updatedData.documentType && attachment.documentType !== "D") {
        //                 return !isExist;
        //             }
        //             else {
        //                 return isExist;
        //             }
        //         });
        //     }
        // }
        // else {
        //     if (this.props.defaultInvoiceAttachmentTypes) {
        //         isExist = this.props.defaultInvoiceAttachmentTypes.map(attachment => {
        //             if (!isEmpty(attachment.documentType) && attachment.documentType === this.editedRowData.documentType && attachment.documentType !== "D") {
        //                 return isExist;
        //             }
        //         });
        //     }
        // }
        if (this.updatedData.documentType == '') {
            IntertekToaster(localConstant.warningMessages.PLEASE_SELECT_DOCUMENT_TYPE, 'warningToast EditidProjDocumentTypeReq');
            return false;
        }
        isExist = this.props.defaultInvoiceAttachmentTypes.map(iteratedValue => {
            if (iteratedValue.documentType === this.updatedData.documentType && iteratedValue.recordStatus !== "D" && iteratedValue.documentType !== this.editedRowData.documentType) {
                return true;
            }
        });
        if (isExist.includes(true)) {
            IntertekToaster(localConstant.warningMessages.ATTACHMENT_TYPE_ALREADY_EXIST, 'warningToast idProjAttachmentExist');
        }
        else {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.props.actions.UpdateAttachmentTypes(this.updatedData, this.editedRowData);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isInvoiceAttachmentModalOpen: false
            });
        }
    }

    /**
     * Closing the popup of Invoice Attachment Types
     */
    cancelInvoiceAttachmentTypes = (e) => {
        e.preventDefault();
        this.setState({
            isInvoiceAttachmentModalOpen: false
        });
        this.updatedData = {};
        this.editedRowData = {};
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
            IntertekToaster(localConstant.warningMessages.SELECT_ANY_ONE_ROW_TO_DELETE, 'warningToast idProjOneRowSelectReq');
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
        if(this.props.pageMode !== localConstant.commonConstants.VIEW) {
        const id=e.node.id;
        const gridData = [];
        const eIndex=e.overNode && e.overNode.id;
        e.api.rowModel.rowsToDisplay.forEach((iteratedValue) => {
            if(iteratedValue.id=== id){
                const ordervalue= e.overIndex;
                iteratedValue.data.displayOrder= ordervalue === -1 ? 1:ordervalue+1; //Changes for P-795
                //iteratedValue.data.displayOrder= ordervalue+1;
                if(iteratedValue.data.recordStatus!=='N')
                   iteratedValue.data.recordStatus='M';  //Changes for D1015
            }
            else if(iteratedValue.id < id && iteratedValue.id >= eIndex){ //Changes for D96
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
        this.props.actions.AddInvoiceDefault(gridData);
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
            IntertekToaster(localConstant.warningMessages.SELECT_ANY_ONE_ROW_TO_DELETE, 'warningToast idProjOneRecordSelectReq');
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
        const { defaultInvoiceRefernces, defaultInvoiceAttachmentTypes, taxes, customerContractContact, projectDocumentTypeMasterData, invoiceRemittanceandFooterText, referenceType } = this.props;
        const invoiceReferenceDefault = [];
        if (defaultInvoiceRefernces) {
            const invoiceReference = [
                ...defaultInvoiceRefernces
            ];
            invoiceReference.sort((a, b) => a.displayOrder - b.displayOrder);
            invoiceReference.map((item, i) => { invoiceReferenceDefault.push(item); });
        }
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        let projectInfo = [];
        if (this.props.invoicingProjectInfo) {
            projectInfo = this.props.invoicingProjectInfo;
        }
        this.defaultInvoiceReferenceButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceReference,
                btnID: "cancelProjectInvoiceReference",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isInvoiceReferenceEditMode ? this.editInvoiceReferenceType : this.createInvoiceReference,
                btnID: "createProjectInvoiceReference",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];

        this.defaultInvoiceAttachmentTypesButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInvoiceAttachmentTypes,
                btnID: "cancelProjectInvoiceAttachmentTypes",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isInvoiceAttachmentEditMode ? this.editInvoiceAttachmentTypes : this.createInvoiceAttachmentTypes,
                btnID: "createProjectInvoiceAttachmentTypes",
                btnClass: "waves-effect waves-teal btn-small",
                showbtn: true
            }
        ];
        bindAction(HeaderData.ProjectInvoicingDefaultHeader, "EditInvoiceRenderColumn", this.editReferenceRowHandler);
        bindAction(HeaderData.ProjectAttachmentTypesHeader, "EditInvoiceAttachmentTypeColumn", this.editAttachmentRowHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard">
                    {this.state.isInvoiceReferenceModalOpen &&
                        <Modal title={localConstant.contract.invoicingDefaults.INOVICE_REFERENCE} modalId="InvoicingDefaultModal" formId="invoicingDefaultModal" modalClass="popup-position" buttons={this.defaultInvoiceReferenceButtons} isShowModal={this.state.isInvoiceReferenceModalOpen} disabled={this.props.interactionMode}>
                            <InvoicingDefaultModal referenceType={referenceType} editInvoiceReferences={this.editedRowData} switchHandlerChange={this.switchHandlerChange} handlerChange={this.handlerChange} disabled={this.props.interactionMode} />
                        </Modal>}
                    {this.state.isInvoiceAttachmentModalOpen &&
                        <Modal title={localConstant.contract.invoicingDefaults.INVOICE_ATTACHMENT_TYPES} modalId="InvoiceAttachmentModal" formId="InvoiceAttachmentModal" modalClass="popup-position" buttons={this.defaultInvoiceAttachmentTypesButtons} isShowModal={this.state.isInvoiceAttachmentModalOpen} disabled={this.props.interactionMode}>
                            <InvoiceAttachmentTypeModal documentType={projectDocumentTypeMasterData} handlerChange={this.handlerChange} editAttachmentTypes={this.editedRowData} />
                        </Modal>}
                    <InvoicingDefault
                        customerContractContact={customerContractContact && arrayUtil.sort(customerContractContact,'contactPersonName','asc')}
                        salesTax={taxes && (this.props.currentPage === localConstant.sideMenu.CREATE_PROJECT ? taxes.filter(x => x.taxType === "S" && x.isActive ===true):taxes.filter(x => x.taxType === "S"))}
                        withHoldingTax={taxes && taxes.filter(x => x.taxType === "W" && x.isActive ===true)}
                        invoicePaymentTerms={this.props.invoicePaymentTerms}
                        customerContractAddress={this.props.customerContractAddress}
                        invoiceCurrency={this.props.invoiceCurrency}
                        invoiceGrouping={localConstant.commonConstants.invoicingGrouping}
                        invoiceRemittanceText={invoiceRemittanceandFooterText && invoiceRemittanceandFooterText.invoiceRemittances}
                        invoiceFooter={invoiceRemittanceandFooterText && invoiceRemittanceandFooterText.invoiceFooters}
                        fetchProjectInfo={projectInfo}
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