import React, { Component, Fragment } from 'react';
import Title from '../../../../common/baseComponents/pageTitle';
import { companyTabDetails } from '../companyTabDetails';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { modalTitleConstant,modalMessageConstant } from '../../../../constants/modalConstants';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { isEmpty,
    parseQueryParam,
    getlocalizeData,
    isEmptyReturnDefault,
    scrollToTop,
    ResetCurrentModuleTabInfo,
    isUndefined } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList  from '../../../../common/baseComponents/errorList';
import { activitycode } from '../../../../constants/securityConstant';
import { requiredNumeric,stringWithOnlySpaces } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();

class CompanyDetails extends Component {   
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false,
            errorList:[],
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action: this.closeErrorList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
          ResetCurrentModuleTabInfo(companyTabDetails);

          this.callBackFuncs ={      
           onAfterSaveChange:()=>{}
          }; 
    }

    async componentDidMount(){ 
        //const CustModal = document.querySelectorAll('.modal');
        //const custModalInstances = MaterializeComponent.Modal.init(CustModal, { "dismissible": false });
        if(this.props.location.search){
            const result = this.props.location.search && parseQueryParam(this.props.location.search);
            await this.props.actions.GetSelectedCompanyCode(result.companyCode);
            this.props.actions.FetchCompanyDetails(result);
        }
        if(this.props.selectedCompany){ // D-55
            this.props.actions.CompanyFetchVatPrefix();
            this.props.actions.FetchDivisionName();
            this.props.actions.FetchExportPrefixes();
        }
        // else if(isEmpty(this.props.selectedCompany) && isEmpty(this.props.location.search)){
        //     this.props.history.push(AppMainRoutes.company);
        // }   // D-55
    }
    confirmationRejectHandler = () => {       
        this.props.actions.HideModal();
    }
    cancelCompanyChanges = () =>{  
        this.props.actions.CancelCompanyDetails();      
        this.props.actions.HideModal();
    }
    // onUnmount = async () => {  
    //     debugger;  
    //     if(window.event.type === "beforeunload"){
    //         await this.props.actions.ClearCompanyDocument();
    //     }  
    //   }
      
    async componentWillUnmount(){         
        // if(this.props.ContractDocuments.filter( x => { return ( x.recordStatus === 'M' || x.recordStatus ==='N'); }).length > 0) {     
        // await this.props.actions.ClearCompanyDocument();   
        // }
        // this.props.actions.CancelCompanyDetails();  Commented for Defect 576 issue 1
    }
    uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
        let count = 0;
        if (Array.isArray(this.props.companyDocuments) && this.props.companyDocuments.length > 0) {
            this.props.companyDocuments.map(document =>{                             
                    if(isUndefined(document.documentUniqueName)){ 
                    IntertekToaster( localConstant.commonConstants.DOCUMENT_STATUS , 'warningToast documentRecordToPasteReq');
                    count++;
                    }                   
            });
             if(count > 0){
                 return false;
             }else{
                 return true;
             }
        }
        return true;
    }

    mandatoryFieldsValidationCheck = () =>{
        const errors=[];
        if(this.props.companyDetail && this.props.companyInvoiceInfo){
            const invoiceRemittances = isEmptyReturnDefault(this.props.companyInvoiceInfo.invoiceRemittances);
                if (invoiceRemittances.length === invoiceRemittances.filter(x => x.recordStatus === "D").length) {
                    errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVICE_REMITTANCE_TEXT } `);
                }else if(invoiceRemittances.filter(x=>x.isDefaultMsg == true &&  x.recordStatus !== "D").length === 0) {
                    errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVICE_REMITTANCE_TEXT_DEFAULT } `);
                }
            const invoiceFooters = isEmptyReturnDefault(this.props.companyInvoiceInfo.invoiceFooters);
            if (invoiceFooters.length === invoiceFooters.filter(x => x.recordStatus === "D").length) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_FOOTER_TEXT } `);
            } else if(invoiceFooters.filter(x=>x.isDefaultMsg == true && x.recordStatus !== "D").length === 0) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVICE_FOOTER_TEXT_DEFAULT } `);
            }
            if (isEmpty(this.props.companyInvoiceInfo.invoiceDescriptionText ? this.props.companyInvoiceInfo.invoiceDescriptionText.trim() : '')) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_DESCRIPTION } `);
            }
            if (isEmpty(this.props.companyInvoiceInfo.invoiceDraftText ? this.props.companyInvoiceInfo.invoiceDraftText.trim() : '')) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_DRAFT_TEXT } `);
            }
            if (isEmpty(this.props.companyDetail.logoText)) {
                 errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.LOGO } `);
            }
            if (isEmpty(this.props.companyInvoiceInfo.invoiceInterCompDraftText ? this.props.companyInvoiceInfo.invoiceInterCompDraftText.trim() : '')) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INTERCO_DRAFT_TEXT } `);
            }
            if (isEmpty(this.props.companyInvoiceInfo.invoiceInterCompDescription ? this.props.companyInvoiceInfo.invoiceInterCompDescription.trim() : '')) {
                errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INTER_COMPANY_DESCRIPTION } `);
            }

            if(!isEmpty(this.props.companyInvoiceInfo.techSpecialistExtranetComment)){
                const tsComment =this.props.companyInvoiceInfo.techSpecialistExtranetComment;
                if(tsComment.length > 4000){
                  errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS } - ${ localConstant.companyDetails.generalDetails.TS_EXTRANET_COMMENT_EXCEEDS }`);
                } 
            }

            if (requiredNumeric(this.props.companyDetail.resourceOutsideDistance)) {
                 errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.RESOURCE_OUTSIDE_DISTANCE } `);
            }
            if(!isEmpty(this.props.companyDocuments)){
                const issueDoc = [];
                this.props.companyDocuments.map(document =>{
                    if(document.recordStatus!="D"){
                    if (isEmpty(document.documentType)) {
                        errors.push(`${ localConstant.companyDetails.Documents.DOCUMENTS }: ${ document.documentName } - ${ localConstant.companyDetails.Documents.SELECT_FILE_TYPE } `);
                   }
                //    if(document.documentType === "Evolution Email" && (document.recordStatus === "N" || document.recordStatus === "M")){
                //     issueDoc.push(document.documentName);
                //   }
                //   else
                   if (document.documentSize == 0 && document.documentType !== "Evolution Email") {
                    const today = new Date();
                    const currentYear = today.getFullYear();
                    const currentQuarter = Math.floor((today.getMonth() + 3) / 3);
                    const createdDate = new Date(document.createdOn);
                    const createdYear = createdDate.getFullYear();
                    const docCreatedQuarter = Math.floor((createdDate.getMonth() + 3) / 3);
                    if (currentYear === createdYear && currentQuarter === docCreatedQuarter){
                      issueDoc.push(document.documentName);
                    }
                  }
                }
              });
              if(issueDoc && issueDoc.length > 0){
                let techSpecData = '';
                for (let i = 0; i < issueDoc.length; i++) {
                  techSpecData = techSpecData +'\"' +issueDoc[i]+'\"'+ '; \r\n';
                }
                errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
              }
            }
                       if(errors.length > 0){
                this.setState({
                  errorList:errors
                });
                return false;
              }

            return true;
        }
        else if(!this.props.companyInvoiceInfo)
        {
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVICE_REMITTANCE_TEXT } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_FOOTER_TEXT } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_DESCRIPTION } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INVOICE_DRAFT_TEXT } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.LOGO } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INTERCO_DRAFT_TEXT } `);
            errors.push(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.companyDetails.generalDetails.INTER_COMPANY_DESCRIPTION } `);
            if(errors.length > 0){
                this.setState({
                  errorList:errors
                });
                return false;
              }
        }
    }

    /** Non mandatory field validation handler
     * Hold till we get ITK Confirmation - 23-12-2019
     */
    nonMandatoryFieldValidationCheck = () => {
        const companyInfo = isEmptyReturnDefault(this.props.companyDetail,'object');
        const companyInvoiceInfo = isEmptyReturnDefault(this.props.companyInvoiceInfo,'object');
        if(!isEmpty(companyInfo.vatTaxRegNo) && stringWithOnlySpaces(companyInfo.vatTaxRegNo)){
            IntertekToaster(localConstant.validationMessage.VAT_TAX_REGISTRATION_NO_VAL);
            return false;
        }
        if(!isEmpty(companyInvoiceInfo.reverseChargeDisclaimer) && stringWithOnlySpaces(companyInvoiceInfo.reverseChargeDisclaimer)){
            IntertekToaster(localConstant.validationMessage.REVERSE_CHARGES_DISCLAIMER_VAL);
            return false;
        }
        if(!isEmpty(companyInvoiceInfo.invoiceInterCompText) && stringWithOnlySpaces(companyInvoiceInfo.invoiceInterCompText)){
            IntertekToaster(localConstant.validationMessage.INTER_COMPANY_TEXT_VAL);
            return false;
        }
        if(!isEmpty(companyInvoiceInfo.invoiceSummarryText) && stringWithOnlySpaces(companyInvoiceInfo.invoiceSummarryText)){
            IntertekToaster(localConstant.validationMessage.INVOICE_SUMMARY_PAGE_VAL);
            return false;
        }
        if(!isEmpty(companyInvoiceInfo.invoiceHeader) && stringWithOnlySpaces(companyInvoiceInfo.invoiceHeader)){
            IntertekToaster(`${ localConstant.companyDetails.generalDetails.GENERAL_DETAILS }-${ localConstant.validationMessage.INVOICE_HEADER_VAL }`);
            return false;
        }
        return true;
    }
    
    companySaveHandler = async () => {
        const validDoc = this.uploadedDocumentCheck();
        const valid = this.mandatoryFieldsValidationCheck();
        // if(this.props.companyUpdatedStatus){
        //     this.props.actions.SaveCompanyDetails();
        // }
        if (valid && validDoc ) {
            const res = await this.props.actions.SaveCompanyDetails();
            if (res && res.code === "1") {
                this.callBackFuncs.onAfterSaveChange();
            }
        }
    };

    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
      }

    companyCancelHandler = () => {        
        //if(this.props.companyUpdatedStatus){
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CANCEL_CHANGES,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [ {
                    buttonName: "Yes",
                    onClickHandler: this.cancelCompanyChanges,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                } ]
            };           
            this.props.actions.DisplayModal(confirmationObject);
       // }
    };    

    render() {
        const { companyDetail } = this.props;
        const modelData = { ...this.confirmationModalData,isOpen:this.state.isOpen };
        let companyDetailData = [];
        if(companyDetail){
            companyDetailData = companyDetail;
        }
        let interactionMode= false;
        if(this.props.pageMode===localConstant.commonConstants.VIEW){
         interactionMode=true;
        }
        const companySave = [
            {
              name: 'Save',
              clickHandler: () => this.companySaveHandler(),
              className: "btn-small mr-0 ml-2",
              permissions:[ activitycode.NEW,activitycode.MODIFY ],
              isbtnDisable: this.props.isbtnDisable
            }, 
            {
              name: localConstant.commonConstants.REFRESHCANCEL,
              clickHandler: () => this.companyCancelHandler(),
              className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
              permissions:[  activitycode.NEW,activitycode.MODIFY ] ,
              isbtnDisable: this.props.isbtnDisable
            }
         ];
        return (
            <Fragment>
            {this.state.errorList.length > 0 ?
                    <Modal title={ localConstant.commonConstants.CHECK_MANDATORY_FIELD }
                        titleClassName="chargeTypeOption"
                        modalContentClass="extranetModalContent"
                        modalClass="ApprovelModal"
                        modalId="errorListPopup"
                        formId="errorListForm"
                        buttons={this.errorListButton}
                        isShowModal={true}>
                            <ErrorList errors={this.state.errorList}/>
                    </Modal> : null
            }
                <Title title="Evolution - Company:Details" />
                <SaveBarWithCustomButtons
                    codeLabel={localConstant.companyDetails.COMPANY_CODE}
                    codeValue={companyDetailData.companyCode}
                    currentMenu={localConstant.companyDetails.COMPANY}
                    currentSubMenu={localConstant.companyDetails.EDIT_VIEW_COMPANY}
                    buttons={companySave}
                    activities={this.props.activities} />
                <div className="row">
                    <div className="col s12 pl-0 pr-0 verticalTabs">
                        <CustomTabs
                            tabsList={companyTabDetails}
                            moduleName="company"
                            interactionMode={interactionMode}
                            onSelect = { scrollToTop } 
                            callBackFuncs = {this.callBackFuncs}>
                        </CustomTabs>
                    </div>
                </div>                
            </Fragment>
        );
    }
}

export default CompanyDetails;