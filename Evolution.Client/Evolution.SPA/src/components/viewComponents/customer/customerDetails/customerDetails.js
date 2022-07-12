import React, { Component } from 'react';
import MaterializeComponent from 'materialize-css';
import { customerTabDetails } from '../customerTabDetails.js';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { modalTitleConstant,modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import { getlocalizeData,isEmpty,parseQueryParam, scrollToTop,ResetCurrentModuleTabInfo,isUndefined } from '../../../../utils/commonUtils';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { activitycode } from '../../../../constants/securityConstant';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList  from '../../../../common/baseComponents/errorList';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();
class CustomerDetails extends Component {
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
          ResetCurrentModuleTabInfo(customerTabDetails);
    }
    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
      }
      
    componentWillMount() {
        // this.props.actions.SetLoader();
    }
    
    componentDidMount() {  
        
        const tab = document.querySelectorAll('.tabs');
        const tabInstances = MaterializeComponent.Tabs.init(tab);
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select);

        const datePicker = document.querySelectorAll('.datepicker');
        const datePickerInstances = MaterializeComponent.Datepicker.init(datePicker);
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });

         /**
         * If customer code is selected, it will proceed to customer detail else, it will redirect to the search page.
         */
        if(this.props.selectedCustomer || this.props.location.search){ //D-55
            let customerCode;            
            customerCode = { customerCode:this.props.selectedCustomer };
            if(this.props.location.search && isEmpty(this.props.selectedCustomer)){
                const result = parseQueryParam(this.props.location.search);
                customerCode = {
                    customerCode:decodeURIComponent(result.customerCode),
                    selectedCompany:result.selectedCompany
                    };
            }
            this.props.actions.GetSelectedCustomerCode(customerCode.customerCode);
            this.props.actions.FetchCustomerDetail(customerCode);
            // this.props.actions.FetchCountry();
            // this.props.actions.FetchSalutation();
            this.props.actions.FetchVatPrefix();
        }
        // else{
        //     this.props.history.push(AppMainRoutes.customer);
        // } //D-55
    }

    uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
        let count = 0;
        if (Array.isArray(this.props.DocumentsData) && this.props.DocumentsData.length > 0) {
            this.props.DocumentsData.map(document =>{                             
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
        if(this.props.DocumentsData){
            if(!isEmpty(this.props.DocumentsData)){
                const issueDoc = [];
                this.props.DocumentsData.map(document =>{
                    if(document.recordStatus!=='D')
                    {
                    if (isEmpty(document.documentType)) {
                        errors.push(`${ localConstant.customer.documents.DOCUMENTS }: ${ document.documentName } - ${ localConstant.customer.documents.SELECT_FILE_TYPE } `);
                }
                        // if (document.documentType === "Evolution Email" && (document.recordStatus === "N" || document.recordStatus === "M")) {
                        //     issueDoc.push(document.documentName);
                        // }
                        // else
                            if (document.documentSize == 0 && document.documentType !== "Evolution Email") {
                                const today = new Date();
                                const currentYear = today.getFullYear();
                                const currentQuarter = Math.floor((today.getMonth() + 3) / 3);
                                const createdDate = new Date(document.createdOn);
                                const createdYear = createdDate.getFullYear();
                                const docCreatedQuarter = Math.floor((createdDate.getMonth() + 3) / 3);
                                if (currentYear === createdYear && currentQuarter === docCreatedQuarter) {
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
        return true;
    }

    customerSaveHandler = () => { 
        const validDoc = this.uploadedDocumentCheck();
        const valid = this.mandatoryFieldsValidationCheck();
        // this.props.actions.SetLoader();
        if(this.props.customerUpdatedStatus && valid && validDoc){
            this.props.actions.SaveCustomerDetails();
        }
    }

    confirmationRejectHandler = () => {
        // this.setState({ isOpen: false })
        this.props.actions.HideModal();
    }

    cancelCustomerChanges = () =>{
        this.props.actions.CancelCustomerDetail();
        this.props.actions.HideModal();
    }

    customerCancelHandler = () => {
        if(this.props.customerUpdatedStatus){
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CANCEL_CHANGES,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [ {
                    buttonName: "Yes",
                    onClickHandler: this.cancelCustomerChanges,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                } ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
    }

    render() {
        const { customerDetailData } = this.props;
        let customerCodeNumber = [];
        if (customerDetailData) {
            customerCodeNumber = customerDetailData;
        }
        let interactionMode= false;
        if(this.props.pageMode===localConstant.commonConstants.VIEW){
         interactionMode=true;
        }
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const customerSave = [
            {
              name: 'Save',
              clickHandler: () => this.customerSaveHandler(),
              className: "btn-small mr-0 ml-2",
              permissions:[ activitycode.NEW,activitycode.MODIFY ],
              isbtnDisable: this.props.isbtnDisable
            }, 
            {
              name: localConstant.commonConstants.REFRESHCANCEL,
              clickHandler: () => this.customerCancelHandler(),
              className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
              permissions:[  activitycode.NEW,activitycode.MODIFY ] ,
              isbtnDisable: this.props.isbtnDisable
            }
         ];

         const saveBarText = [
             {
                 codeLabel:"Customer Code",
                 codeValue: customerDetailData.customerCode,
                 codeLabelDivClass:"col s12 m2 pt-1"
             },
             {
                codeLabel:"Customer Name",
                codeValue: customerDetailData.customerName,
                codeLabelDivClass:"col s12 m4 pt-1"
            },
            {
                codeLabel:"Parent Company Name",
                codeValue: customerDetailData.parentCompanyName,
                codeLabelDivClass:"col s12 m4 pt-1"
            },
         ];
        return (
            <div>
                {<CustomModal modalData={modelData} isOpen={this.props.isOpen} />}
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
             <SaveBarWithCustomButtons
                    saveBarText = {saveBarText}
                    currentMenuClass = "col s12 m2"
                   currentMenu={localConstant.customer.CUSTOMER_TITLE}
                   currentSubMenu={localConstant.customer.EDIT_VIEW_CUSTOMER}
                    buttons={customerSave}
                    activities={this.props.activities} />
                    <div className="row">
                    <div className="col s12 pl-0 pr-0 verticalTabs customSavebarPosition">
                            <CustomTabs
                                tabsList={customerTabDetails}
                                moduleName="customer" 
                                interactionMode={interactionMode}
                                onSelect = { scrollToTop }>
                            </CustomTabs>
                    </div>
                    </div>

            </div>
        );
    }
}

export default CustomerDetails;