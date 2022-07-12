import React, { Component, Fragment } from 'react';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { supplierpoTabDetails } from './supplierpoTabDetails';
import { isEmpty, 
        getlocalizeData,
        parseQueryParam, 
        scrollToTop,
        ResetCurrentModuleTabInfo,
        ObjectIntoQuerySting,
        isUndefined } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { required, requiredNumeric } from '../../../../utils/validator';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import arrayUtil from '../../../../utils/arrayUtil';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList  from '../../../../common/baseComponents/errorList';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { activitycode } from '../../../../constants/securityConstant';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();

const HeaderData = {
    "columnDefs": [
      {
          "headerName": "Supplier PO",
          "field": "supplierPONumber",
          "filter": "agTextColumnFilter",
          "width":150,
          "headerTooltip": "Supplier PO",
          "tooltipField":"supplierPONumber"
      },
      {
        "headerName": "Customer",
        "field": "supplierPOCustomerName",
        "filter": "agTextColumnFilter",
        "width":200,
        "headerTooltip": "Customer",
        "tooltipField":"supplierPOCustomerName"
      },
      {
        "headerName": "Project No",
        "field": "supplierPOProjectNumber",
        "filter": "agTextColumnFilter",
        "width":120,
        "headerTooltip": "Project No",
        "tooltipField":"supplierPOProjectNumber"
      },
      {
        "headerName": "Main Supplier",
        "field": "supplierPOMainSupplierName",
        "filter": "agTextColumnFilter",
        "width":150,
        "headerTooltip": "Main Supplier",
        "tooltipField":"supplierPOMainSupplierName"
      },
      {
        "headerName": "Status",
        "field": "supplierPOStatus",
        "filter": "agTextColumnFilter",
        "valueGetter":(params)=>{
            if(params.data.supplierPOStatus==="O"){
                return 'Open';
            }else{
                return "Closed";
            }
        },
        "width":90,
        "headerTooltip": "Status",
        "tooltipField":"supplierPOStatus"
      },
  ],
  "enableFilter":false, 
  "enableSorting":true,
  "gridHeight":10,
  "pagination": true,
  "searchable":false,
  "gridActions":false,
  "gridTitlePanel":true,
  };

const SupplierPOValidationPopup = (props) => {
    return(
      <Fragment>
        <p className="bold confimationBodyText">At least one other Supplier PO with the same Supplier PO Number already exists</p>
        <ReactGrid gridRowData={props.state.duplicateSupplierPOs} gridColData={props.HeaderData} />
        <p className="bold confimationBodyText">Do you wish to save this record?</p>
      </Fragment>
    );  
  };

class SupplierpoDetails extends Component {
    constructor(props) {    
        super(props);
        this.state = {
            errorList:[],
            duplicateSupplierPOs:[],
            isPODuplicateValidationModalOpen:false,
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
        this.PODuplicateValidationButton= [
            {
                name: localConstant.commonConstants.YES,
                action: this.SaveSupplierPoData,
                btnID: "submitduplicatePOs",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.NO,
                action: this.ClosePODuplicateValidationPopup,
                btnID: "closeduplicatePOs",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        ResetCurrentModuleTabInfo(supplierpoTabDetails);
        this.callBackFuncs ={
            onCancel:()=>{}
        };
    }
    componentDidMount(){       
         const result = this.props.location.search && parseQueryParam(this.props.location.search);
        // if(this.props.location.pathname === '/SupplierPODetails'){
        //     this.props.actions.HandleMenuAction({
        //      currentPage: localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE,
        //      currentModule: localConstant.moduleName.SUPPLIER_PO
        //      }).then(res=>{
        //         if(res===true){
        //           if (!result.supplierPOId){                  
        //               const projNo = result.projectNumber?result.projectNumber:this.props.projectNumber;
        //               this.props.actions.SaveSelectedProjectNumber(projNo);             
        //               this.props.actions.ClearSupplierPOData();
        //               this.props.actions.FetchProjectForSupplierPOCreation();
        //               this.props.actions.SetCurrentPageMode(null);                      
        //             }
        //         }
        //      });
        //    }

        // if (this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE) {
        //     this.props.actions.ClearSupplierPOData();
        //     this.props.actions.FetchProjectForSupplierPOCreation(result);
        //     this.props.actions.SetCurrentPageMode(null);
        // }
        //Added for ITK D-456 -Starts
        let isAssignmentOpenedAsOC= (result.isAssignmentOpenedAsOC=== "true" && !this.props.interactionMode) ? true : false;
        isAssignmentOpenedAsOC =(result.chCompany === result.selectedCompany) ? false :true; //IGO QC D-900 Issue 2
        this.props.actions.SetSupplierPOViewMode(isAssignmentOpenedAsOC); 
        //Added for ITK D-456 -End
        if(result.supplierPOId){             
            this.props.actions.FetchSupplierPoData(result);
        } 
        else {
            const projNo = result.projectNumber?result.projectNumber:this.props.projectNumber;
            this.props.actions.SaveSelectedProjectNumber(projNo);             
          //  this.props.actions.ClearSupplierPOData(); //D623issue1
          if(isEmpty(this.props.supplierPoInfo)){            
            this.props.actions.FetchProjectForSupplierPOCreation(result.projectNumber);
            this.props.actions.SetCurrentPageMode(null);
          }
            this.props.actions.FetchSupplierPoData();
        }
    }

    supplierPoSaveClickHandler = () => {
      //  this.props.actions.FetchProjectDetailForSupplier(this.props.supplierPoInfo && this.props.supplierPoInfo.supplierPOProjectNumber);
      const validDoc = this.uploadedDocumentCheck();
        const valid = this.supplierPoDetailValidation();
        if (valid && validDoc) {
            if(this.props.supplierPoInfo.supplierPOId){
                if(this.props.supplierPoInfo.oldSupplierPONumber !== this.props.supplierPoInfo.supplierPONumber){
                    this.props.actions.SupplierPODuplicateName({ "supplierPONumber": this.props.supplierPoInfo && this.props.supplierPoInfo.supplierPONumber }).then((res) => {
                        if (res && res.length > 0) {
                            const duplicateValues=this.state.duplicateSupplierPOs;
                            res.forEach(iteratedValue => {
                                duplicateValues.push(iteratedValue);
                                this.setState({
                                    duplicateSupplierPOs:duplicateValues
                                });
                            });
                         if(duplicateValues.length > 0){
                             this.setState({
                                isPODuplicateValidationModalOpen:true
                             });
                         }
                        } else {
                            this.props.actions.SaveSupplierPoData();
                        }
                    });
                } else {
                    this.props.actions.SaveSupplierPoData();
                }
            } else {
            this.props.actions.SupplierPODuplicateName({ "supplierPONumber": this.props.supplierPoInfo && this.props.supplierPoInfo.supplierPONumber }).then((res) => {
                if (res && res.length > 0) {
                    const duplicateValues=this.state.duplicateSupplierPOs;
                    res.forEach(iteratedValue => {
                        duplicateValues.push(iteratedValue);
                        this.setState({
                            duplicateSupplierPOs:duplicateValues
                        });
                    });
                 if(duplicateValues.length > 0){
                     this.setState({
                        isPODuplicateValidationModalOpen:true
                     });
                 }
                } else {
                    this.props.actions.SaveSupplierPoData();
                }
            });
        }
        }
    }

    supplierPoCancelClickHandler = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message:modalMessageConstant.CANCEL_CHANGES,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.cancelSupplierPoChanges,
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

    cancelSupplierPoChanges = async () => {
        // this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE ?
        //     this.props.actions.CancelCreateSupplierPODetails() : this.props.actions.CancelEditSupplierPODetails();
        let res = null;
        if(this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE){
            res = await this.props.actions.CancelCreateSupplierPODetails(); //Changes for D932
        }  
        else{
            res = await this.props.actions.CancelEditSupplierPODetails();
        } 
        if(res)
          this.callBackFuncs.onCancel();
        this.props.actions.HideModal();
    }
    supplierPoDeleteClickHandler = () => {
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.SUPPLIER_PO_DELETE_MESSAGE,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.deleteSupplierPoHandler,
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
    deleteSupplierPoHandler = () => {
        this.props.actions.HideModal();
        this.props.actions.DeleteSupplierPoData()
            .then(response => {
                if (response) {
                    if (response.code === "1") {
                        this.props.history.push('/EditSupplierPO');
                        this.props.actions.DeleteAlert(response.result, "Supplier PO");
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

    supplierPoDetailValidation = () => {
        const errors=[];
       let iteratedValue={};
       this.props.projectDetails && this.props.projectDetails.map((x)=>{
           return iteratedValue=x;
       });
       
        if (!isEmpty(this.props.supplierPoInfo)) {
            if (required(this.props.supplierPoInfo.supplierPONumber)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.supplierpo.SUPPLIER_PO_NUMBER }`);
            }
            if (required(this.props.supplierPoInfo.supplierPOMaterialDescription)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.supplierpo.METERIAL_DESCRIPTION }`);
            }
            if (required(this.props.supplierPoInfo.supplierPOStatus)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.contract.STATUS }`);
            }
            if (required(this.props.supplierPoInfo.supplierPOMainSupplierName)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.supplierpo.MAIN_SUPPLIER }`);
            }
            if (requiredNumeric(this.props.supplierPoInfo.supplierPOBudget)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.budget.BUDGET_MONETARY }`);
            }
            if (requiredNumeric(this.props.supplierPoInfo.supplierPOBudgetHours)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.budget.BUDGET_HOURS }`);
            }
            // if (requiredNumeric(this.props.supplierPoInfo.supplierPOBudgetWarning)) {
            //     errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.budget.BUDGET_MONETARY_WARNING }`);
            // }  //  ITK D-779

            // if (requiredNumeric(this.props.supplierPoInfo.supplierPOBudgetHoursWarning)) {
            //     errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.budget.BUDGET_HOURS_WARNING }`);
            // }
            if (this.props.supplierPoInfo.supplierPOStatus==='C' && required(this.props.supplierPoInfo.supplierPOCompletedDate)) {
                errors.push(`${ localConstant.supplierpo.SUPPLIER_DETAILS } - ${ localConstant.supplierpo.COMPLETED_DATE }`);
            }
            // if(this.props.supplierPoInfo.supplierPOBudget>iteratedValue.projectBudgetValue){
                
            //     IntertekToaster(" Total Supplier PO budget value has been used",'warningToast');
            //     return false;
            // }
            // if(this.props.supplierPoInfo.supplierPOBudgetHours>iteratedValue.projectBudgetHoursUnit){
            //     IntertekToaster(" Total Supplier PO budget hours has been used",'warningToast');
            //     return false;
            // }
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
            if(errors.length > 0){
                this.setState({
                  errorList:errors
                });
                return false;
              }else {
                  if(this.props.supplierPoInfo.supplierPOBudgetWarning > 100)
                  {
                      IntertekToaster(localConstant.budget.BUDGET_MONETARY_MESSAGE,'warningToast');
                      return false;
                  }
                  if(this.props.supplierPoInfo.supplierPOBudgetHoursWarning > 100)
                  {
                    IntertekToaster(localConstant.budget.BUDGET_HOURS_MESSAGE,'warningToast');
                    return false;
                  }
                }
            return true;
        }
        else {
            IntertekToaster(localConstant.validationMessage.SUPPLIER_PO_NUMBER_REQUIRED, 'warningToast supSupplierNameReq');
        }
    }

    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
      }

      ClosePODuplicateValidationPopup =(e) =>{
          e.preventDefault();
          this.setState({
            duplicateSupplierPOs:[],
            isPODuplicateValidationModalOpen:false
          });
      }

    newAssignmentClickHandler = () => {
        this.props.actions.SaveSelectedProjectNumber(this.props.supplierPoInfo.supplierPOProjectNumber);
        // this.props.actions.HandleMenuAction({
        //   currentPage:localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE,
        //   currentModule:"assignment"
        // });
        const queryObj={            
            projectNumber : this.props.supplierPoInfo.supplierPOProjectNumber,
            supplierPOId : this.props.supplierPoInfo.supplierPOId,
            supplierPONumber : this.props.supplierPoInfo.supplierPONumber,
            supplierPOMainSupplierId : this.props.supplierPoInfo.supplierPOMainSupplierId,
            supplierPOMainSupplierName:this.props.supplierPoInfo.supplierPOMainSupplierName // Scenario 159 fixes
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        window.open(AppMainRoutes.createAssignment+
            '?' + queryStr ,'_blank');
        // IntertekToaster(localConstant.commonConstants.COMPONENT_UNDER_CONSTRUCTION, 'warningToast compUnderConstruction');
    }

    filterTabDetails = () => {
        const tabsToHide = [];
        if (this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE) {
            tabsToHide.push(...[
                "Assignments", "Documents"
            ]);
        }
        const supplierPOTabs = arrayUtil.negateFilter(supplierpoTabDetails, 'tabBody', tabsToHide);
        return supplierPOTabs;
    }

    SaveSupplierPoData =()=>{
        this.props.actions.SaveSupplierPoData();
        this.setState({
          isPODuplicateValidationModalOpen:false
        });
    }
    render() {      
        this.supplierpoTab = this.filterTabDetails();
        const isInEditMode=( this.props.currentPage===localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE);
        const isInViewMode=(this.props.pageMode===localConstant.commonConstants.VIEW)?true:false;
        const isInCreateMode=( this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE);
        const response = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
        let interactionMode= this.props.interactionMode;
        if(this.props.pageMode===localConstant.commonConstants.VIEW){
         interactionMode=true;
        }
        const  supplierPOSave = [
            {
                name: localConstant.commonConstants.SAVE,
                clickHandler: () => this.supplierPoSaveClickHandler(),
                className: "btn-small mr-0 ml-2",
                permissions:[ activitycode.NEW,activitycode.MODIFY ],
                isbtnDisable: this.props.isbtnDisable,
                showBtn:response[0]
            }, {
                name: localConstant.commonConstants.REFRESHCANCEL,
                clickHandler: () => this.supplierPoCancelClickHandler(),
                className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
                permissions:[ activitycode.NEW,activitycode.MODIFY ],
                isbtnDisable: this.props.isbtnDisable,
                showBtn:response[0]
            },
            {
                name: localConstant.commonConstants.DELETE,
                clickHandler: () => this.supplierPoDeleteClickHandler(),
                className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",
                permissions:[  activitycode.DELETE ],
                showBtn: response[1]      
            },
            {
                name: localConstant.commonConstants.NEW_ASSIGNMENT,
                clickHandler: () => this.newAssignmentClickHandler(),
                className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
                permissions:[  activitycode.NEW ],
                showBtn: this.props.supplierPOViewMode ? false : response[1]
            }
        ];
        supplierPOSave.forEach((btn, i) => {
            if (this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE &&
                btn.name === localConstant.commonConstants.DELETE) {
                btn.className = "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect disabled";
            }
            //Changes For D-479 issue 1
            if (this.props.currentPage === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE &&
                this.props.pageMode===localConstant.commonConstants.VIEW  
                && btn.name === localConstant.commonConstants.DELETE
                && this.props.interactionMode === false) {
                btn.showBtn = response[1];
                btn.className = "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect";
            }
            if (this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE && 
                btn.name === localConstant.commonConstants.NEW_ASSIGNMENT) {
                btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger hide";
                btn.showBtn = false;
            }
            if (this.props.currentPage === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE && 
                this.props.pageMode===localConstant.commonConstants.VIEW
                && btn.name === localConstant.commonConstants.NEW_ASSIGNMENT
                && this.props.interactionMode === false) {
                btn.showBtn = response[1];
                btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger";
            }
        });

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

            {this.state.isPODuplicateValidationModalOpen ?
            <Modal title="The following problem occured while saving changes."
                modalId="PODuplicateValidationPopup"
                formId="PODuplicateValidationForm"
                buttons={this.PODuplicateValidationButton}
                isShowModal={this.state.isPODuplicateValidationModalOpen}>
            <SupplierPOValidationPopup
              state={this.state}
              HeaderData={HeaderData}/>
          </Modal> : null
        }

                <SaveBarWithCustomButtons
                    codeLabel={localConstant.modalConstant.SUPPLIER_PO}
                    codeValue={isEmpty(this.props.supplierPoInfo) ? "" : this.props.supplierPoInfo.supplierPONumber}
                    currentMenu={localConstant.sideMenu.SUPPLIER_PO}
                    currentSubMenu={this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE ?
                        localConstant.supplierpo.ADD_SUPPLIER_PO : localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO}
                    buttons={supplierPOSave}
                    activities={this.props.activities}
                />
         
                <div className="row ml-2 mb-0">
                    <div className="col s12 pl-0 pr-2 verticalTabs">
                        <CustomTabs
                            callBackFuncs = {this.callBackFuncs}
                            tabsList={this.supplierpoTab}
                            moduleName="supplierPo"
                            interactionMode={interactionMode}
                            onSelect={scrollToTop}                         
                        />
                    </div>
                </div>
                <div className="row ml-2 mb-0">
        </div>
            </Fragment>
        );
    }
}

export default SupplierpoDetails;
