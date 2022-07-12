import CustomModal from '../../../../common/baseComponents/customModal';
import React, { Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { getlocalizeData, mergeobjects, isEmpty, bindAction, formInputChangeHandler, mapArrayObject, isArray, findObject, isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { configuration } from '../../../../appConfig';
import moment from 'moment';
import { applicationConstants } from '../../../../constants/appConstants';
import { activitycode } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
import arrayUtil from '../../../../utils/arrayUtil';
import { StringFormat } from '../../../../utils/stringUtil';
import { userTypeGet } from '../../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();

const TaxonomyApprovalGrid = (props) => {  
    const isTaxonomyEditable= isEditable({ activities: props.activities, editActivityLevels: [ activitycode.EDIT_TM ] });
    const isTMProfileAction = props.selectedProfileDetails.profileAction !== localConstant.techSpec.common.SEND_TO_RC_RM;//SystemRole based UserType relevant Quick Fixes 
    const isRC_RM_TMProfileAction = (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) && (props.userTypes.includes(localConstant.techSpec.userTypes.TM) && !props.userTypes.includes(localConstant.techSpec.userTypes.RC) && !props.userTypes.includes(localConstant.techSpec.userTypes.RM));//SystemRole based UserType relevant Quick Fixes 
    const isRCProfile = (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) && (props.assignedToUser !== props.userName);
        return ( 
        <Fragment> 
           
            <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.taxonomyApproval.TAXONOMY_APPROVAL_DETAILS} colSize="s12" 
            titleClass={ props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_TM || props.selectedProfileDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE  ? "pl-0 bold" : "pl-0 bold mandate"}>
            <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.taxonomyApproval.COMMENTS}
                        type='textarea'
                        colSize='s12'
                        inputClass={"customInputs "} 
                        maxLength={4000} 
                        name='tqmComment'
                        value={props.selectedProfileDetails && props.selectedProfileDetails.tqmComment}
                        dataValType='valueText'
                        onValueChange={props.tqmCommentsChangeHandler}
                        readOnly={ !isTaxonomyEditable || (isTMProfileAction ? !isRC_RM_TMProfileAction : isRC_RM_TMProfileAction) || props.interactionMode} 
                        autocomplete="off" />

                <ReactGrid gridColData={props.headerData.TaxonomyApprovalHeader} gridRowData={props.gridRowData} onRef={props.onRefer} />
                {props.pageMode !== localConstant.commonConstants.VIEW && <div className="right-align mt-2">
                    <a onClick={props.addTaxonomyApprovalDetails} className="waves-effect btn btn-small" disabled={!isTaxonomyEditable || (isTMProfileAction ? !isRC_RM_TMProfileAction : isRC_RM_TMProfileAction) || props.interactionMode || isRCProfile}>{localConstant.commonConstants.ADD} </a>
                    <a disabled={(props.gridRowData <= 0 || !isTaxonomyEditable || (isTMProfileAction ? !isRC_RM_TMProfileAction : isRC_RM_TMProfileAction)|| props.interactionMode || isRCProfile)} onClick={props.DeleteHAndler}
                        className="btn-small ml-2 modal-trigger dangerBtn" >{localConstant.commonConstants.DELETE} </a>
                </div>}
            </CardPanel>
        </Fragment> 
);
};
const TaxonomyApprovalModalPopup = (props) => { 
    const isTaxonomyEditable= isEditable({ activities: props.activities, editActivityLevels: [ activitycode.EDIT_TM ] });
    return (
        <Fragment>
            <div className="row ">
                <LabelwithValue
                    label={'Approved By:'}
                    className="pr-2 mt-2" />
                <CustomInput
                    hasLabel={false}
                    divClassName='col'
                    type='text'
                    colSize="pl-0 s6"
                    inputClass="browser-default customInputs"
                    disabled={ true } //def 737
                    defaultValue={!isEmpty(props.editedRowData) ? props.editedRowData.approvedBy : props.loggedInUser} //D737 CR Changes
                     />
            </div>
            <div className="row ">
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision'
                    label={localConstant.techSpec.taxonomyApproval.CATEGORY}
                    type='select'
                    colSize="s4"
                    // labelClass={!props.disableBtn?'mandate':""}
                    labelClass={props.disableRejectMandate ? '' : "mandate"}
                    className="browser-default customInputs"
                    optionsList={props.taxonomyCategory}
                    optionName="name"
                    optionValue="name"
                    name="taxonomyCategoryName"
                    disabled={(props.editMode ||  !isTaxonomyEditable) && props.hideTaxonomyCategory ? props.enableSelectSubCategory : ''}
                    onSelectChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.taxonomyCategoryName} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision pl-0'
                    label={localConstant.techSpec.taxonomyApproval.SUB_CATEGORY}
                    type='select'
                    colSize="s4"
                    labelClass={props.enableSelectSubCategory ? '' : 'mandate'}
                    // labelClass={props.disableRejectMandate?'':"mandate"}
                    className="browser-default customInputs"
                    optionsList={props.taxonomySubCategory}
                    optionName="taxonomySubCategoryName"
                    optionValue="taxonomySubCategoryName"
                    name="taxonomySubCategoryName"
                    defaultValue={props.editedRowData && props.editedRowData.taxonomySubCategoryName}
                    disabled={props.enableSelectSubCategory}
                    onSelectChange={props.inputChangeHandlerSubCategory} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision pl-0'
                    label={localConstant.techSpec.taxonomyApproval.SERVICES}
                    type='select'
                    colSize="s4"
                    labelClass={props.enableSelectServices ? "" : 'mandate'}
                    className="browser-default customInputs"
                    optionsList={props.taxonomyServices}
                    optionName="taxonomyServiceName"
                    optionValue="taxonomyServiceName"
                    name="taxonomyServices"
                    disabled={props.enableSelectServices}
                    onSelectChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.taxonomyServices} />
            </div>
            <div className="row ">
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision'
                    label={localConstant.techSpec.taxonomyApproval.APPROVAL_STATUS}
                    type='select'
                    colSize="s4"
                    labelClass="mandate"
                    className="browser-default customInputs"
                    optionsList={props.approvalStatus}
                    optionName="label"
                    optionValue="value"
                    name="approvalStatus"
                    onSelectChange={props.approvalInputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.approvalStatus}
                    disabled={ !isTaxonomyEditable } />
                {props.status ?
                    <Fragment>
                        <CustomInput
                            hasLabel={true}
                            isNonEditDateField={false}
                            divClassName='col loadedDivision pl-0'
                            type='date'
                            label={localConstant.techSpec.taxonomyApproval.EFFECTIVE_FROM}
                            colSize='s4'
                            labelClass={props.disableBtn ? '' : 'mandate'}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            required={true}
                            name="fromDate"
                            disabled={ (!isTaxonomyEditable ||  props.disableBtn) }
                            onDatePickBlur={props.taxonomyStartDateBlur}
                            selectedDate={props.effectiveFromDate}
                            onDateChange={props.fetchEffectiveFromDate} />
                        <CustomInput
                            hasLabel={true}
                            isNonEditDateField={false}
                            divClassName='col loadedDivision pl-0'
                            type='date'
                            label={localConstant.techSpec.taxonomyApproval.EFFECTIVE_TO}
                            colSize='s4'
                            labelClass=" customLabel"
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            required={true}
                            name="toDate"
                            disabled={ (!isTaxonomyEditable ||  props.disableBtn) }
                            onDatePickBlur={props.taxonomyStartDateBlur}
                            selectedDate={props.effectiveToDate}
                            onDateChange={props.fetchEffectiveToDate} />
                    </Fragment> : null}
            </div>
            <div className="row ">
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision'
                    label={localConstant.techSpec.taxonomyApproval.INTERVIEW}
                    type='multiSelect'
                    valueType="defaultValue"
                    colSize="s8"
                    className="browser-default customInputs"
                    optionsList={props.interview}
                    onSelectChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.interview}
                    multiSelectdValue={props.getMultiSelectdValue}
                    name="interview"
                    disabled={ !isTaxonomyEditable } />
                   
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.taxonomyApproval.COMMENTS}
                    type='textarea'
                    colSize='s4'
                    name="comments"
                    labelClass={props.status ? '' : 'mandate'}
                    maxLength={4000}
                    onValueChange={props.inputChangeHandler}
                    inputClass="customInputs"
                    defaultValue={props.editedRowData && props.editedRowData.comments}
                    readOnly={ !isTaxonomyEditable } />
            </div>
        </Fragment>
    );
};
class TaxonomyApproval extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            effectiveFromDate: '',
            effectiveToDate: '',
            inValidDateFormat: false,
            enableSelectSubCategory: true,
            enableSelectServices: true,
            status: true,
            disableBtn: true,
            disableRejectMandate: false,
            taxonomyApprovalShowModal: false,
            isTaxonomyApprovalDetailsEdit: false,
        };
        this.updatedData = {}; 
        this.userTypes = userTypeGet();
         //SystemRole based UserType relevant Quick Fixes 
         this.username = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
         const functionRefs = {};
         functionRefs["enableEditColumn"] = this.enableEditColumn;
         this.headerData = HeaderData(functionRefs);
         bindAction(this.headerData.TaxonomyApprovalHeader, "EditColumn", this.editRowHandler);
        this.taxonomyApprovalAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideTaxonomyApprovedModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name:  localConstant.commonConstants.SUBMIT,
                action: this.createTaxonomyApprovalDetails,
                btnClass: "btn-small",
                showbtn: true
            }
        ];
        this.taxonomyApprovalEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideTaxonomyApprovedModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateTaxonomyApprovaldetails,
                btnClass: "btn-small",
                showbtn: isEditable({ activities: this.props.activities, editActivityLevels: [ activitycode.EDIT_TM ] })
            }
        ];
        this.props.callBackFuncs.reloadTaxonomyLineItemsOnSave = () => {
            this.reloadTaxonomyLineItems(props);
        };  
    }

    reloadTaxonomyLineItems = (value) =>{
         const functionRefs = {};
         functionRefs["enableEditColumn"] = this.enableEditColumn;
         this.headerData = HeaderData(functionRefs);
         bindAction(this.headerData.TaxonomyApprovalHeader, "EditColumn", this.editRowHandler);
         this.gridChild.gridApi.setRowData(this.props.taxonomyApproval);
    }

    enableEditColumn = (e) => { //SystemRole based UserType relevant Quick Fixes 
        // const isTMProfileAction = this.props.selectedProfileDetails.profileAction !== localConstant.techSpec.common.SEND_TO_RC_RM;
        // const isRC_RM_TMProfileAction = (this.props.selectedProfileDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) && this.userTypes && (this.userTypes.includes(localConstant.techSpec.userTypes.TM) && !this.userTypes.includes(localConstant.techSpec.userTypes.RC) && !this.userTypes.includes(localConstant.techSpec.userTypes.RM));//SystemRole based UserType relevant Quick Fixes //D562
        // return   ( isTMProfileAction ? !isRC_RM_TMProfileAction : false);
        const isTMProfileAction = this.props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM;
        return  ((isTMProfileAction ? this.props.isTSUserTypeCheck : (this.props.isTMUserTypeCheck && !this.props.isRCUserTypeCheck) ? false : true)); //Added for (this.props.isTMUserTypeCheck && !this.props.isRCUserTypeCheck) D1374
    }

    componentWillUnmount(){
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearServices();
    }
    editRowHandler = (data) => {
      
        if (data.recordStatus === null) {
            this.setState({
                hideTaxonomyCategory: true
            });
        } else {
            this.setState({
                hideTaxonomyCategory: false
            });
        }
        this.setState((state) => {
            return {
                taxonomyApprovalShowModal: !state.taxonomyApprovalShowModal,
                isTaxonomyApprovalDetailsEdit: true
            };
        });
        this.editedRowData = data;
        const { taxonomyCategoryName, taxonomySubCategoryName } = data;
        this.props.actions.ClearSubCategory();
        taxonomyCategoryName && this.props.actions.FetchTechSpecSubCategory(taxonomyCategoryName);
        this.props.actions.ClearServices();
        taxonomyCategoryName && taxonomySubCategoryName && this.props.actions.FetchTechSpecServices(taxonomyCategoryName,taxonomySubCategoryName);//def 916 fix
        this.editMultiSelectedValue(data);
        this.handleApprovalStatus();
        this.defaultDateHandler();
    }
    getCategoryAndService = (selectedName) => {
        if (selectedName === 'taxonomyCategoryName') {
            this.props.actions.ClearSubCategory();
            const category = this.updatedData[selectedName];
            this.props.actions.FetchTechSpecSubCategory(category);
        }
        if (selectedName === 'taxonomySubCategoryName') {
            this.props.actions.ClearServices();
            const subCategory = this.updatedData[selectedName];
            const category = this.updatedData['taxonomyCategoryName'];
            this.props.actions.FetchTechSpecServices(category,subCategory);
        }
    }
    inputChangeHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (!isEmpty(inputvalue.value)) {
            if (inputvalue.name === "taxonomyCategoryName" || inputvalue.name === 'taxonomySubCategoryName' || inputvalue.name === 'taxonomyServiceName') {
                this.getCategoryAndService(inputvalue.name);
            }
            if (this.updatedData.taxonomyCategoryName != null) {
                this.handleSubcategory();
            }
        }
    }
 
    tqmCommentsChangeHandler = (e) => {
        const inputvalue = formInputChangeHandler(e); 
        this.props.actions.UpdateTQMComments(inputvalue.value); 
    }

    approvalInputChangeHandler = (e) => {
        this.updatedData[e.target.name] = e.target.value;
        this.handleApprovalStatus();
    }
    handleSubcategory = () => {
        this.setState({
            enableSelectSubCategory: false,
            //  disableRejectMandate:true,
        });
    }
    handlingApprovalStatus = (data) => {
        if (!isEmpty(data)) {
            if (data.approvalStatus === "Approve") {
                this.setState({
                    status: true,
                    disableBtn: false,
                });
            }
            else if ((data.approvalStatus === "Provisional")) {
                this.setState({
                    status: false,
                    disableRejectMandate: false,
                    effectiveFromDate: '',
                    effectiveToDate: '',
                });
            }
            else if ((data.approvalStatus === "Reject")) {
                this.setState({
                    status: false,
                    effectiveFromDate: '',
                    effectiveToDate: ''
                });
            }
            else {
                this.setState({
                    status: true,
                    disableBtn: true
                });
            }
        }
    }
    handleApprovalStatus = (e) => {
        if (!isEmpty(this.updatedData))
            this.handlingApprovalStatus(this.updatedData);
        else if (this.editedRowData) {
            this.handlingApprovalStatus(this.editedRowData);
        }
    }
    inputChangeHandlerSubCategory = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (!isEmpty(inputvalue.value)) {
            if (inputvalue.name === "taxonomyCategoryName" || inputvalue.name === 'taxonomySubCategoryName' || inputvalue.name === 'taxonomyServiceName') {
                this.getCategoryAndService(inputvalue.name);
            }
            this.setState({ enableSelectServices: false });
        }
    }
    fetchEffectiveFromDate = (date) => {
        this.setState({ effectiveFromDate: date }, () => {
            this.updatedData.fromDate = this.state.effectiveFromDate;
        });
    }
    fetchEffectiveToDate = (date) => {
        this.setState({ effectiveToDate: date }, () => {
            if (!isEmpty(this.state.effectiveFromDate))
                this.updatedData.toDate = this.state.effectiveToDate;
            else {
                this.setState({ effectiveToDate: '' });
                IntertekToaster(localConstant.techSpec.common.END_DATE_WARNING, 'warningToast');
            }
        });
    }
    addTaxonomyApprovalDetails = (e) => {
        e.preventDefault();
        this.handleApprovalStatus();
        this.setState({
            taxonomyApprovalShowModal: true,
            isTaxonomyApprovalDetailsEdit: false,
            disableBtn: true,
            effectiveFromDate: "",
            effectiveToDate: ""
        });
        this.updatedData = {};
    };
    hideTaxonomyApprovedModal = (e) => {
        this.setState({
            taxonomyApprovalShowModal: false,
            isTaxonomyApprovalDetailsEdit: true
        });
        this.changeDisable(true);
        if (this.props.taxonomyApproval.map(data => {
            if (data.approvalStatusName === "Provisional")
                this.setState({ status: false });
            else
                this.setState({ status: true });
            return data;
        }))
            this.updatedData = {};
        this.editedRowData = {};
    }
    //Validation for Taxonomy Approval Details
    taxonomyApprovalDetailsValidation = (data) => {
        if (data.taxonomyCategoryName === undefined || data.taxonomyCategoryName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Category', 'warningToast');
            return false;
        }
        if (data.taxonomySubCategoryName === undefined || data.taxonomySubCategoryName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Sub Category', 'warningToast');
            return false;
        }
        if (data.taxonomyServices === undefined || data.taxonomyServices === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Services', 'warningToast');
            return false;
        }
        if (data.approvalStatus === undefined || data.approvalStatus === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Approval Status', 'warningToast');
            return false;
        }
        if (data.approvalStatus === "Provisional") {
            if (isEmpty(data.comments)) {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_FIELD + ' Comments', 'warningToast');
                return false;
            }
        }
        if (data.approvalStatus === "Approve") {
            if (isEmpty(data.fromDate)) {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Effective From', 'warningToast');
                return false;
            }
        }
        if (data.approvalStatus === "Reject") {
            if (isEmpty(data.comments)) {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_FIELD + ' Comments', 'warningToast');
                return false;
            }
        }
        //D943
        if (this.props.taxonomyApproval.filter(x=>x.taxonomyCategoryName === data.taxonomyCategoryName && x.taxonomySubCategoryName === data.taxonomySubCategoryName && x.taxonomyServices === data.taxonomyServices && x.recordStatus!=="D" && x.id != data.id ).length > 0 ) {
            const message = StringFormat(localConstant.techSpec.taxonomyApproval.TAX_DUPLICATE_VALIDATION,data.taxonomyServices);
            IntertekToaster(message , 'warningToast');
            return false;
        }

        return true;
    }
    //Date Range Validator
    dateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.TAXONOMY_APPROVAL_DATE_RANGE_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    taxonomyStartDateBlur = () => {
        const isValid = moment(this.state.effectiveToDate).isBefore(this.state.effectiveFromDate);
        if (isValid)
            IntertekToaster(localConstant.techSpec.common.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
    }
    taxonomyEndDateBlur = () => {
        const isValid = moment(this.state.effectiveToDate).isBefore(this.state.effectiveFromDate);
        if (isValid)
            IntertekToaster(localConstant.techSpec.common.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
    }
    //setting disabled as true or false
    changeDisable = (data) => {
        this.setState({
            enableSelectSubCategory: data,
            enableSelectServices: data
        });
    }
    createTaxonomyApprovalDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.taxonomyApprovalDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.effectiveFromDate !== "" && this.state.effectiveFromDate !== null)
                    fromDate = this.state.effectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null)
                    toDate = this.state.effectiveToDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }
                if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = - Math.floor(Math.random() * (Math.pow(10, 5)));//D943(ref by ALM Doc 13-03-2020)
                    this.updatedData["epin"] = this.props.epin!==undefined ? this.props.epin : 0;
                    this.updatedData["fromDate"] = fromDate;
                    this.updatedData["toDate"] = toDate;
                    this.updatedData["taxonomyStatus"] = "IsAcceptRequired";
                    this.updatedData["approvedBy"]= this.props.loggedInUser; //Added for D737 CR 
                    this.props.actions.AddTaxonomyApprovalDetails(this.updatedData);
                    this.updatedData = {};
                    this.changeDisable(true);
                    this.setState({
                        status: true,
                        taxonomyApprovalShowModal: false,
                        isTaxonomyApprovalDetailsEdit: true
                    });
                }
            }
        }
        else this.taxonomyApprovalDetailsValidation(this.updatedData);
    }
    deleteTaxonomyApprovalDetailsHandler = () => {
        const selectedRecords = this.gridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.TAXONOMY_APPROVAL_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelected,
                        className: " m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: " m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    updateTaxonomyApprovaldetails = (e) => {
        e.preventDefault();
        let fromDate = "";
        let toDate = "";
        if(!isEmpty(this.updatedData)){ //Added for D737 CR Changes
            this.updatedData["approvedBy"]= this.props.loggedInUser;
        }
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.taxonomyApprovalDetailsValidation(combinedData)) {
            if (this.state.effectiveFromDate !== "" && this.state.effectiveFromDate !== null)
                fromDate = this.state.effectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null)
                toDate = this.state.effectiveToDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }
            let selectedData = '';
            if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (!isEmpty(this.updatedData.interview)) {
                    selectedData = combinedData.interview;
                }
                else {
                    selectedData = mapArrayObject(combinedData.interview, 'value').join(',');
                }
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["fromDate"] = fromDate;
                this.updatedData["toDate"] = toDate;
                this.updatedData["interview"] = selectedData;
                this.updatedData["taxonomyStatus"]="IsAcceptRequired";
                const editedRow = mergeobjects(this.updatedData);
                if (this.updatedData && !isEmpty(this.updatedData)) {
                    this.props.actions.UpdateTaxonomyApprovaldetails(editedRow, this.editedRowData);
                    this.handleApprovalStatus();
                    this.editedRowData = {};
                    this.updatedData = {};
                    this.setState({ taxonomyApprovalShowModal: false });
                }
            }
        }
        else {
            this.setState({ taxonomyApprovalShowModal: true });
        }
    }
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();
        const approvedTaxonomy =selectedData.filter(x=>(x.taxonomyStatus === 'Accept'));
        if(approvedTaxonomy && approvedTaxonomy.length > 0){
            IntertekToaster('Approved Taxonomy Cannot be Deleted', 'warningToast');
            this.props.actions.HideModal();
        } else {
            this.gridChild.removeSelectedRows(selectedData);
            this.props.actions.DeleteTaxonomyApprovalDetails(selectedData);
            this.props.actions.HideModal();
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    defaultDateHandler = () => {
        if (this.editedRowData) {
            if ((this.editedRowData.fromDate !== null && this.editedRowData.fromDate !== "")) {
                this.setState({ effectiveFromDate: moment(this.editedRowData.fromDate) });
            }
            else {
                this.setState({ effectiveFromDate: "" });
            }
            if ((this.editedRowData.toDate !== null && this.editedRowData.toDate !== "")) {
                this.setState({ effectiveToDate: moment(this.editedRowData.toDate) });
            }
            else {
                this.setState({ effectiveToDate: "" });
            }
        }
        else {
            this.setState({
                effectiveFromDate: "",
                effectiveToDate: "",
            });
        }
    }
    getMultiSelectdValue = (data) => {
        const selectedItem = mapArrayObject(data, 'value').join(',');
        this.updatedData.interview = selectedItem;
    }
    editMultiSelectedValue = (data) => {
        const dvaChartersData = localConstant.commonConstants.interview;
        const dvaCharterInt = [];
        if (isArray(data.interview)) {
            const selectedItem = mapArrayObject(data.interview, 'value').join(',');
            const answerString = selectedItem;
            answerString.split(',').forEach(function (item) {
                dvaCharterInt.push({ value: item });
            });
        } else {
            const dvaCharterString = data.interview;
            dvaCharterString && dvaCharterString.split(',').forEach(function (item) {
                dvaCharterInt.push({ value: item });
            });
        }
        this.editedRowData.interview = [];
        dvaCharterInt.map(item => {
            return this.editedRowData.interview.push(findObject(dvaChartersData, item));
        });
        this.setState({ taxonomyApprovalShowModal: true });
    }
 
    render() {
        const { taxonomyApproval, techSpecCategory, techSpecSubCategory, techSpecServices, taxonomyApprovalEditData, interactionMode,activities,selectedProfileDetails,assignedToUser,loggedInUser } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.state.taxonomyApprovalShowModal &&
                    <Modal title={this.state.isTaxonomyApprovalDetailsEdit ? localConstant.techSpec.taxonomyApproval.EDIT_TAXONOMY_APPROVAL :
                        localConstant.techSpec.taxonomyApproval.ADD_TAXONOMY_APPROVAL} modalClass="techSpecModal"
                        buttons={this.state.isTaxonomyApprovalDetailsEdit ? this.taxonomyApprovalEditButtons : this.taxonomyApprovalAddButtons}
                        isShowModal={this.state.taxonomyApprovalShowModal}
                    >
                        <TaxonomyApprovalModalPopup
                            supportFileType={configuration}
                            taxonomyApproval={taxonomyApproval}
                            taxonomyCategory={arrayUtil.sort(techSpecCategory,'name','asc')}
                            taxonomySubCategory={techSpecSubCategory}
                            taxonomyServices={techSpecServices}
                            inputChangeHandler={this.inputChangeHandler}
                            inputChangeHandlerSubCategory={this.inputChangeHandlerSubCategory}
                            effectiveFromDate={this.state.effectiveFromDate}
                            effectiveToDate={this.state.effectiveToDate}
                            fetchEffectiveFromDate={this.fetchEffectiveFromDate}
                            fetchEffectiveToDate={this.fetchEffectiveToDate}
                            taxonomyApprovalEditData={taxonomyApprovalEditData}
                            taxonomyStartDateBlur={this.taxonomyStartDateBlur}
                            taxonomyEndDateBlur={this.taxonomyEndDateBlur}
                            editMode={this.state.isTaxonomyApprovalDetailsEdit}
                            loggedInUser={this.props.loggedInUser}
                            enableSelectSubCategory={this.state.enableSelectSubCategory}
                            enableSelectServices={this.state.enableSelectServices}
                            status={this.state.status}
                            disableRejectMandate={this.state.disableRejectMandate}
                            disableBtn={this.state.disableBtn}
                            editedRowData={this.editedRowData}
                            approvalInputChangeHandler={this.approvalInputChangeHandler}
                            approvalStatus={localConstant.commonConstants.approvalStatus}
                            interview={localConstant.commonConstants.interview}
                            getMultiSelectdValue={this.getMultiSelectdValue}
                            hideTaxonomyCategory={this.state.hideTaxonomyCategory}
                            activities={ activities }
                        />
                    </Modal>}
                <div className="genralDetailContainer customCard">  
                    <TaxonomyApprovalGrid
                        addTaxonomyApprovalDetails={this.addTaxonomyApprovalDetails}
                        DeleteHAndler={this.deleteTaxonomyApprovalDetailsHandler}
                        gridRowData={taxonomyApproval && taxonomyApproval.filter(x => x.recordStatus !== "D")}
                        onRefer={ref => { this.gridChild = ref; }}
                        pageMode={this.props.pageMode}
                        interactionMode = { interactionMode }
                        activities={ activities }
                        selectedProfileDetails ={ selectedProfileDetails }
                        headerData={this.headerData}
                        userName={this.username}
                        userTypes={this.userTypes} 
                        tqmCommentsChangeHandler={this.tqmCommentsChangeHandler}
                        loginUser={ loggedInUser }
                        assignedToUser={ assignedToUser }
                    />
                </div>
            </Fragment>
        );
    }
}
export default TaxonomyApproval;