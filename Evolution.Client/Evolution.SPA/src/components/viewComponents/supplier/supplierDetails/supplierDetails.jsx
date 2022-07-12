import React, { Component, Fragment } from 'react';
import { SupplierTabDetail } from './supplierTabDetails';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { isEmpty, getlocalizeData, parseQueryParam, scrollToTop,isUndefined,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList  from '../../../../common/baseComponents/errorList';
import { activitycode } from '../../../../constants/securityConstant';
import { StringFormat } from '../../../../utils/stringUtil';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';

const localConstant = getlocalizeData();

class supplierDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            errorList: []
        };
        this.errorListButton = [
            {
                name: localConstant.commonConstants.OK,
                action: this.closeErrorList,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
    }

    componentDidMount() { 
        if(this.props.currentPage ===''){
            this.props.actions.UpdateCurrentPage(localConstant.supplier.CREATE_SUPPLIER);
        }
        const result = this.props.location.search && parseQueryParam(this.props.location.search);
        if (result.supplierId) {
            this.props.actions.GetSelectedSupplier(result);
        }
        if(result.supplierViewMode){
            this.props.actions.UpdateInteractionMode(true);
            this.props.actions.SetCurrentPageMode(localConstant.commonConstants.VIEW);
        }
    }

      componentWillUnmount(){       
        this.props.actions.ClearSupplierDetails();
       }

    supplierSaveClickHandler = () => {
        const validDoc = this.uploadedDocumentCheck();
        const valid = this.supplierDetailValidation();
        if (valid === true && validDoc) {
            if (this.props.supplierInfo.supplierId) {
                if (this.props.supplierInfo.oldSupplierName !== this.props.supplierInfo.supplierName.trim()) {
                    this.props.actions.SupplierDuplicateName({ "supplierName": this.props.supplierInfo && this.props.supplierInfo.supplierName.trim() }).then((res) => {
                        if (res && res.length > 0) {
                            let duplicateSuppliers = "";
                            res.forEach((iteratedValue,index) => {
                                if(index >=1)
                                    duplicateSuppliers = `${ duplicateSuppliers }\n`;
                                duplicateSuppliers = `${ duplicateSuppliers } ${ iteratedValue.supplierName }, ${ iteratedValue.country }, ${ iteratedValue.city }${ iteratedValue.postalCode ? ", " + iteratedValue.postalCode : "" }`; // D-170
                            });
                            this.supplierDupplicateClickHandler(duplicateSuppliers);
                        } else {
                            this.onSaveSupplier();
                        }
                    });
                }
                else {
                    this.onSaveSupplier();
                }
            }
            else {
                this.props.actions.SupplierDuplicateName({ "supplierName": this.props.supplierInfo && this.props.supplierInfo.supplierName }).then((res) => {
                    if (res && res.length > 0) {
                        let duplicateSuppliers = "";
                        res.forEach((iteratedValue,index) => {
                            if(index >=1)
                                duplicateSuppliers = `${ duplicateSuppliers }\n`;
                            duplicateSuppliers = `${ duplicateSuppliers } ${ iteratedValue.supplierName }, ${ iteratedValue.country }, ${ iteratedValue.city }${ iteratedValue.postalCode ? ", " + iteratedValue.postalCode : "" }`; // D-170
                        });
                        this.supplierDupplicateClickHandler(duplicateSuppliers);
                    } else {
                        this.onSaveSupplier();
                    }
                });
            }
        }
    }
    onSaveSupplier = () => {
        // const valid = this.supplierDetailValidation();
        // if (valid === true) {
            if (this.props.supplierInfo.recordStatus === 'N') {
                const supplierId = this.props.supplierInfo.supplierId;
                this.props.actions.SaveSupplierData(supplierId)
                    .then(response => {
                        if (response) {
                            if (response.code == 1) {
                                this.props.actions.EditSupplier({
                                    "currentModule": "supplier",
                                    "currentPage": localConstant.supplier.EDIT_VIEW_SUPPLIER
                                });
                                this.props.actions.FetchSupplierData({ supplierId: response.result },false);
                                // this.props.history.push('/supplierDetails');
                            }
                        }
                    });
            }
            else {
                this.props.actions.UpdateSupplierData(this.props.supplierInfo.supplierId);
            }
        // }
        this.props.actions.HideModal();
    }
    supplierDupplicateClickHandler = (duplicateSuppliers) => {

        // const supplierDetail = `${ this.props.supplierInfo.supplierName }, ${ this.props.supplierInfo.country }, ${ this.props.supplierInfo.state }, ${ this.props.supplierInfo.city }`;
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: StringFormat(modalMessageConstant.SUPPLIER_DUPPLICATE_MESSAGE,duplicateSuppliers),
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.onSaveSupplier,
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

    supplierCancelClickHandler = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.CANCEL_CHANGES,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.cancelSupplierChanges,
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

    cancelSupplierChanges = () => {
        const data = !isEmpty(this.props.supplierInfo) ? this.props.supplierInfo : null;
        this.props.actions.CancelEditSupplierDetailsDocument();
        this.props.actions.FetchSupplierData(data,true);
        this.props.actions.HideModal();
    }

    supplierDeleteClickHandler = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.SUPPLIER_DELETE_MESSAGE,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.deleteSupplier,
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

    deleteSupplier = () => {
        this.props.actions.HideModal();
        this.props.actions.DeleteSupplierData(this.props.supplierInfo)
            .then(response => {
                if (response) {
                    if (response.code == 1) {
                        setTimeout(x => {
                            this.props.history.push('/editSupplier');
                        }, 1000);
                        //  this.props.actions.EditSupplier();
                    }
                }
            });
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
        let count = 0;
        if (Array.isArray(this.props.documentrowData) && this.props.documentrowData.length > 0) {
            this.props.documentrowData.map(document =>{                             
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

    supplierDetailValidation = () => {
        const errors = [];
        if (!isEmpty(this.props.supplierInfo)) {
            if (isEmptyOrUndefine(this.props.supplierInfo.supplierName)) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.SUPPLIER_NAME }`);
            }
            else if(required(this.props.supplierInfo.supplierName.trim())){
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.SUPPLIER_NAME }`);
            }
            if (required(this.props.supplierInfo.country)) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.COUNTRY } `);
            }
            if (required(this.props.supplierInfo.state)) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.STATE }`);
            }
            //Added for D-1076 -Starts
            if (!required(this.props.supplierInfo.state) && this.props.supplierInfo.state.toLowerCase() === localConstant.supplier.OFFLINE) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.STATE }- ${ localConstant.supplier.STATE_OR_CITY_OFFLINE }`);
            }
            //Added for D-1076 -End
            if (required(this.props.supplierInfo.city) && required(this.props.supplierInfo.postalCode)) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.CITY } or ${ localConstant.supplier.POSTAL_CODE }`);
            }
            else if(required(this.props.supplierInfo.city) && required(this.props.supplierInfo.postalCode.trim())){
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.CITY } or ${ localConstant.supplier.POSTAL_CODE }`);
            }
            //Added for D-1076 -Starts
            if (!required(this.props.supplierInfo.city) && this.props.supplierInfo.city.toLowerCase() === localConstant.supplier.OFFLINE) {
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.CITY }- ${ localConstant.supplier.STATE_OR_CITY_OFFLINE }`);
            }
            //Added for D-1076 -End
            if (isEmptyOrUndefine(this.props.supplierInfo.supplierAddress)){
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.FULL_ADDRESS }`);
            }
            else if(required(this.props.supplierInfo.supplierAddress.trim())){
                errors.push(`${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.FULL_ADDRESS }`);
            }
            //   if(this.props.duplicateMessage.length>0){
            //       return false;
            //   }
            if(!isEmpty(this.props.documentrowData)){
                const issueDoc = [];
                this.props.documentrowData.map(document =>{
                    if(document.recordStatus!=='D')
                    {
                    if (isEmpty(document.documentType)) {
                        errors.push(`${ localConstant.supplier.documents.DOCUMENT } - ${ document.documentName } - ${ localConstant.supplier.documents.SELECT_FILE_TYPE } `);
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

            if (errors.length > 0) {
                this.setState({
                    errorList: errors
                });
                return false;
            }
            return true;
        }
        else {
            IntertekToaster(localConstant.validationMessage.SUPPLIER_NAME_REQUIRED, 'warningToast supSupplierNameReq');
        }
    }

    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    render() {
        const isInEditMode=(this.props.interactionMode ===false);
        const isInViewMode=(this.props.pageMode===localConstant.commonConstants.VIEW)?true:false;
        const isInCreateMode=( this.props.currentPage === localConstant.supplier.CREATE_SUPPLIER);
        const response = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
        let interactionMode= this.props.interactionMode;
        if(this.props.pageMode===localConstant.commonConstants.VIEW){
         interactionMode=true;
        }
        const supplierSave = [
            {
                name: localConstant.commonConstants.SAVE,
                clickHandler: () => this.supplierSaveClickHandler(),
                className: "btn-small mr-0 ml-2",
                permissions:[ activitycode.NEW,activitycode.MODIFY ],
                isbtnDisable: this.props.isbtnDisable,
                showBtn:response[0]
            }, {
                name: localConstant.commonConstants.REFRESHCANCEL,
                clickHandler: () => this.supplierCancelClickHandler(),
                className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
                permissions:[ activitycode.NEW,activitycode.MODIFY ],
                isbtnDisable: this.props.isbtnDisable,
                showBtn:response[0]
            },
            {
                name: localConstant.commonConstants.DELETE,
                clickHandler: () => this.supplierDeleteClickHandler(),
                className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",  
                permissions:[  activitycode.DELETE ],
                showBtn: response[1]  
            }
        ];

        supplierSave.map((btn, i) => {
            if (this.props.currentPage === localConstant.supplier.CREATE_SUPPLIER &&
                btn.name === localConstant.commonConstants.DELETE) {
                btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger disabled";
            }

            if (this.props.currentPage === localConstant.supplier.EDIT_VIEW_SUPPLIER
                && btn.name === localConstant.commonConstants.DELETE
                && this.props.interactionMode === false) {
                btn.showBtn = true;
            }
        });
        if (Array.isArray(SupplierTabDetail) && SupplierTabDetail.length > 0) {
            SupplierTabDetail.map((tabs, i) => {
                return tabs.tabDisableStatus.map((tabDisable, index) => {
                    if (tabs.tabHeader === "Documents") {
                        if (this.props.currentPage === localConstant.supplier.EDIT_VIEW_SUPPLIER) {
                            SupplierTabDetail[i].tabActive = true;
                        }
                        else {
                            SupplierTabDetail[i].tabActive = false;
                        }
                    }
                });
            });
        }
        return (
            <Fragment>
                {this.state.errorList.length > 0 ?
                    <Modal title={localConstant.commonConstants.CHECK_MANDATORY_FIELD}
                        titleClassName="chargeTypeOption"
                        modalContentClass="extranetModalContent"
                        modalClass="ApprovelModal"
                        modalId="errorListPopup"
                        formId="errorListForm"
                        buttons={this.errorListButton}
                        isShowModal={true}>
                        <ErrorList errors={this.state.errorList} />
                    </Modal> : null
                }
                <SaveBarWithCustomButtons
                    codeLabel={localConstant.sideMenu.SUPPLIER}
                    codeValue={isEmpty(this.props.supplierInfo) ? "" : this.props.supplierInfo.supplierName}
                    currentMenu={localConstant.supplier.SUPPLIER}
                    currentSubMenu={this.props.currentPage === "Create Supplier" ? this.props.currentPage : this.props.interactionMode === true ? "View Supplier" : "Edit Supplier"}
                    isbtnDisable={this.props.isbtnDisable}
                    buttons={supplierSave}
                    activities={this.props.activities}  />
                <div className="row ml-2 mb-0">
                    <div className="col s12 pl-0 pr-2 verticalTabs">
                        <CustomTabs
                            tabsList={SupplierTabDetail || []}
                            moduleName="supplier"
                            interactionMode={interactionMode}
                            onSelect={scrollToTop}
                        />
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default supplierDetails;