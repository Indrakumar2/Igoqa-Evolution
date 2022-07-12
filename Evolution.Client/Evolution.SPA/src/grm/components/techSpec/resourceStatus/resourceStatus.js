import React, { Component, Fragment } from 'react';
import { getlocalizeData, 
    isEmpty, 
    mergeobjects, 
    mobileNumberFormat, 
    bindAction, 
    isEmptyReturnDefault,
    isEmptyOrUndefine,
    deepCopy } from '../../../../utils/commonUtils';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import Modal from '../../../../common/baseComponents/modal';
import UploadDocument from '../../../../common/baseComponents/uploadDocument';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../../constants/appConstants';
import arrayUtil from '../../../../utils/arrayUtil';
import { required } from '../../../../utils/validator';
import {  levelSpecificActivities } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
import { userTypeGet } from '../../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();

const ResourceStatusComponent = (props) => {
    const { lastModification } =  props.selectedProfileDetails;
    const employmentStatusOptionList=props.employmentStatusOptionList;  /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
    
    return (
        <Fragment>
            <CardPanel className="white lighten-4 black-text" colSize="s12"
                title={localConstant.techSpec.resourceStatus.RESOURCE_STATUS} >
                <div className="row mb-2" >
                    <LabelwithValue
                        colSize="s3"
                        label={localConstant.techSpec.resourceStatus.NAME}
                        value={ `${ props.selectedProfileDetails.firstName ? props.selectedProfileDetails.firstName : "" } 
                                 ${ props.selectedProfileDetails.lastName ? props.selectedProfileDetails.lastName :"" }` } />
                    <LabelwithValue
                        colSize="s3"
                        label={localConstant.techSpec.resourceStatus.LAST_MODIFIED_ON}
                        value={lastModification ? moment(lastModification).format(localConstant.commonConstants.UI_DATE_FORMAT) : ''} />
                    <LabelwithValue
                        colSize="s3"
                        label={localConstant.techSpec.resourceStatus.MODIFIED_BY}
                        value={props.selectedProfileDetails.modifiedBy} />
                </div>
            </CardPanel>
            <CardPanel className='white lighten-4 black-text' colSize='s12'>
                <div className="row mb-0">
                    <div className='col s12 p-0'>
                        {/* <CustomInput
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.resourceStatus.COMPANY}
                            type='select'
                            className="browser-default"
                            colSize='s6'
                            // inputClass="inputHighlight"
                            labelClass="mandate"
                            name='companyName'
                            id='companyCode'
                            disabled={true}
                            //Commented for D363 CR Change
                           // disabled={(props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist) || props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.companyList}
                            optionName='companyName'
                            optionValue='companyCode'
                            defaultValue={props.selectedCompany}
                            //Commented for D363 CR Change
                           // defaultValue={props.selectedProfileDetails.companyCode}
                            onSelectChange={props.handlerChange} />  */}
                            {/* //Changes for D363 CR Change */}
                        <LabelwithValue
                            colSize="s6 mt-4"
                            label={localConstant.techSpec.resourceStatus.COMPANY +':'}
                            value={props.selectedProfileDetails.companyName} />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.techSpec.resourceStatus.SUB_DIVISION}
                            type='select'
                            colSize='s6'
                            className="browser-default"
                            labelClass="mandate"
                            name='subDivisionName'
                            disabled={props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.subDivision}
                            optionName='name'
                            optionValue='name'
                            defaultValue={props.selectedProfileDetails.subDivisionName}
                            onSelectChange={props.handlerChange} />
                    </div>
                    <div className='col s12 p-0'>
                        <CustomInput
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.resourceStatus.PROFILE_STATUS}
                            type='select'
                            colSize='s6'
                            className="browser-default"
                            labelClass="mandate"
                            name='profileStatus'
                            disabled={props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.profileStatus}
                            optionName='name'
                            optionValue='name'
                            defaultValue={props.selectedProfileDetails.profileStatus}
                            onSelectChange={props.handlerChange} />   
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.techSpec.resourceStatus.EMPLOYMENT_TYPE}
                            type='select'
                            colSize='s6'
                            className="browser-default"
                            name='employmentType'
                            disabled={(props.employmentStatusOptionList.length === 0)|| props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.employmentStatusOptionList}
                            optionName='name'
                            optionValue='name'
                           // defaultValue={(props.employmentStatusOptionList.length===1)?employmentStatusOptionList[0].name: props.selectedProfileDetails.employmentType }
                            defaultValue={props.selectedProfileDetails.employmentType ? (props.employmentStatusOptionList.length===1)?employmentStatusOptionList[0].name: props.selectedProfileDetails.employmentType : "" } //Sanity Def 203
                            onSelectChange={props.handlerChange}
                            labelClass={props.employmentStatusOptionList.length === 0 ? "" : "mandate"} />
                    </div> 
                    <div className='col s12 p-0'>
                    {(props.isTsChangeApproveRequired || props.rejectionShow) ?  <CustomInput
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.resourceStatus.APPROVAL_STATUS}
                            type='select'
                            colSize='s6'
                            className="browser-default"
                            labelClass="mandate"
                            name='approvalStatus'
                            disabled={ props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.tsApprovalStatus}
                            optionName='label'
                            optionValue='value'
                            defaultValue={props.selectedProfileDetails.approvalStatus === 'U' ? 'R' :props.selectedProfileDetails.approvalStatus}
                            onSelectChange={props.handlerChange}
                             /> : null }
                             {/** changes for D730 issue 4&5 (03-01-2020) */}
                             {/** changes for 684 (24-01-2020) */}
                     {(props.isAcceptRequired && props.selectedProfileDetails.isPreviouslyProfileMadeActive && props.selectedProfileDetails.prevProfileAction === localConstant.techSpec.common.SEND_TO_RC_RM) ?  <CustomInput //D954 //Profle Action Check added for IGO QC D886 #6issue (Ref ALM Confirmation Doc)
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.resourceStatus.APPROVAL_STATUS}
                            type='select'
                            colSize='s6'
                            className="browser-default"
                            labelClass="mandate"
                            name='taxonomyStatus'
                            disabled={ !props.isAcceptRequired || props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                            optionsList={props.taxonomyApprovalStatus}
                            optionName='label'
                            optionValue='value'
                            defaultValue={props.selectedProfileDetails.taxonomyStatus}
                            onSelectChange={props.handlerChange}
                             /> : null }
                     </div>
                     <div className='col s12 p-0'>
                    { (props.isTsChangeApproveCommentEnabled && props.isTsChangeApproveRequired ) || props.rejectionShow ?  <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.BUSINESS_INFORMATION_COMMENTS}
                        type='textarea'
                        colSize='s12' 
                        maxLength={4000}
                        name='businessInformationComment'
                        onValueChange={props.handlerChange}
                        readOnly={props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)}//SystemRole based UserType relevant Quick Fixes 
                        value={props.selectedProfileDetails.businessInformationComment} /> : null } 
                     </div>
                    <div className='col s12 p-0'>
                        <CustomInput
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.resourceStatus.START_DATE}
                            type='date'
                            colSize='s6'
                            labelClass = {props.selectedProfileDetails.profileStatus === 'Active'?'mandate':""}
                            name='startDate'
                            disabled={props.disableField || props.interactionMode|| (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)}//SystemRole based UserType relevant Quick Fixes 
                            selectedDate={props.selectedProfileDetails.startDate ? moment(props.selectedProfileDetails.startDate) : ''}
                            onDateChange={props.fetchStartDate}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            onDatePickBlur={props.resourceStartDateBlur}
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT} />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            colSize='s6'
                            label={localConstant.techSpec.resourceStatus.END_DATE}
                            type='date'
                            name='endDate'
                            disabled={props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)}//SystemRole based UserType relevant Quick Fixes 
                            selectedDate={props.selectedProfileDetails.endDate ? moment(props.selectedProfileDetails.endDate) : ''}
                            onDateChange={props.fetchEndDate}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            onDatePickBlur={props.resourceEndDateBlur}
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT} />
                    </div>
                    <div className='col s12 p-0'>
                        <CustomInput
                            divClassName='col pr-0'
                            colSize='s6'
                            switchLabel={localConstant.techSpec.resourceStatus.E_REPORTING_QUALIFIED}
                            type='switch'
                            className="lever"
                            disabled={ ( props.disableField || props.interactionMode || (props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM))} /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
                            isSwitchLabel={true}
                            switchName='isEReportingQualified'
                            onChangeToggle={props.handlerChange}
                            checkedStatus={props.selectedProfileDetails.isEReportingQualified}
                            switchKey={props.selectedProfileDetails.isEReportingQualified}
                        />
                        <CustomInput
                            divClassName='col'
                            switchLabel={localConstant.techSpec.resourceStatus.REVIEW_MODERATION_PROCESS}
                            type='switch'
                            colSize='s6'
                            className="lever"
                            disabled={ ( props.interactionMode || props.enableEdit() ) } //D1374 ITKQA Issue Failed on 23-10-2020 /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
                            isSwitchLabel={true}
                            switchName='reviewAndModerationProcessName'
                            onChangeToggle={props.handlerChange}
                            checkedStatus={props.selectedProfileDetails.reviewAndModerationProcessName}
                            switchKey={props.selectedProfileDetails.reviewAndModerationProcessName}
                        />
                    </div>
                </div>
            </CardPanel>
        </Fragment>
    );
};
const AddNewStamp = (props) => {
    let stampNumberMinLength = 0;
    if(props.editStampDetails.isSoftStamp && props.editStampDetails.isSoftStamp === 'Soft Stamp'){
        stampNumberMinLength = 4;
    }
    else if(props.editStampDetails.isSoftStamp && props.editStampDetails.isSoftStamp === 'Hard Stamp'){
        stampNumberMinLength = 3;
    }
    let documents=[];
    if(props.editStampDetails && props.editStampDetails.documents)
    {
        documents=props.editStampDetails.documents.filter(x=>x.recordStatus!=='D'); 
    }
    return (
        <Fragment>
            <div className="col s6 p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.techSpec.resourceStatus.STAMP_TYPE}
                    type='select'
                    name='isSoftStamp'
                    colSize='s6'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.stampType}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editStampDetails.isSoftStamp} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.resourceStatus.COUNTRY_CODE}
                    type='select'
                    name='countryCode'
                    id='countryName'
                    colSize='s6'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.country}
                    optionName='displayName'
                    optionValue='name'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editStampDetails.countryName} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0 mt-0'
                    label={localConstant.techSpec.resourceStatus.ISSUED_ON}
                    type='date'
                    name='issuedDate'
                    labelClass="mandate"
                    selectedDate={props.issuedDate}
                    onDateChange={props.fetchIssuedDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT} />
                <div className="col s12 pl-0 pr-0" >
                <UploadDocument
                        //Mode={props.editStampDetails.documents[0].recordStatus === 'D' ? !props.isResourceStatusEdit  : props.isResourceStatusEdit }
                        defaultValue={ documents && documents.length>0 ? documents : '' }
                        className="col s12 pl-0 pr-0"
                        label={localConstant.techSpec.resourceStatus.ACKNOWLEDGEMENT_DOCUMENT}
                        cancelUploadedDocument={props.cancelTimeStampUploadedDocument} 
                        editDocDetails = {props.editStampDetails}
                        gridName={localConstant.techSpec.common.TECHNICALSPECIALISTSTAMP}
                        documentType="TS_Stamp"
                        uploadFileRefType="stampUpload"
                        disabled={ false }
                        />
                </div>
            </div>
            <div className="col s6 p-0" >
                <CustomInput
                    hasLabel={true}
                    type="text"
                    divClassName='col pr-0'
                    label={localConstant.techSpec.resourceStatus.STAMP_NUMBER}
                    labelClass="mandate"
                    dataType='numeric'
                    colSize='s12'
                    inputClass="customInputs"
                    onValueChange={props.onChangeHandler}
                    maxLength={20}
                    name='stampNumber'
                    min="0"
                    minLength={stampNumberMinLength}
                    defaultValue={props.editStampDetails.stampNumber} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.resourceStatus.RETURNED_ON}
                    type='date'
                    name='returnedDate'
                    selectedDate={props.returnedDate}
                    onDateChange={props.fetchReturnedDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT} />
            </div>
        </Fragment>
    );
};
class ResourceStatus extends Component {
    constructor(props) {
        super(props);
        this.state = {
            issuedDate: '',
            returnedDate: '',
            startDate: '',
            endDate: '',
            inValidDateFormat: false,
            resourceStatusShowModal: false,
            isResourceStatusEdit: false,
            lessReturnDate:false,
        };
        this.updatedData = {};
        this.editedRowData = {};
        this.stampDocument={};
        this.userTypes = userTypeGet();
        this.isShowTsChangeApprovalStatus=false;
         // Hot Fixes Changes done as per ITK requirement -- start
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);
        bindAction(this.headerData.StampHeader, "EditColumn", this.editRowHandler);
         // Hot Fixes Changes done as per ITK requirement --end
           //Add Buttons
           this.newStampAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelStampModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addStampDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        //Edit Buttons
        this.newStampEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelStampModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateStampDetails,
                btnClass: "btn-small",
                showbtn: true
            }
        ];
        this.username = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
    }

    componentDidMount=()=>{ 
        const tabInfo = this.props.tabInfo;
        if (tabInfo && tabInfo.componentRenderCount === 0) {
             this.props.actions.FeatchTechSpechStampCountryCode(); 
        }
    }
    enableEditColumn = () => {  // Hot Fixes Changes done as per ITK requirement
        /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        const isTS=this.props.isTSUserTypeCheck;
        if((this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM ||
            this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM) 
            && this.props.profileInfo.assignedToUser !== this.username){
            return true;
        }
        else if(this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS){
            return !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesTM });
        } 
        else if(this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && isTS){
            return isTS;
        }
        return !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 }); //D1374 Failed on 29-10-2020
    }

    setGridDate = () => {
        if (this.editedRowData) {
            if (this.editedRowData.returnedDate !== null && this.editedRowData.returnedDate !== "") {
                this.setState({
                    issuedDate: moment(this.editedRowData.issuedDate),
                    returnedDate: moment(this.editedRowData.returnedDate)
                });
            }
            else {
                this.setState({
                    issuedDate: moment(this.editedRowData.issuedDate),
                    returnedDate: ""
                });
            }
        } else {
            this.setState({
                issuedDate: "",
                returnedDate: ""
            });
        }
    }
    editRowHandler = (data) => {
        this.setState((state) => {
            return {
                resourceStatusShowModal: !state.resourceStatusShowModal,
                isResourceStatusEdit: true
            };
        });
        if((this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM ||
            this.props.profileInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM) && this.props.profileInfo.assignedToUser !== this.username){
            this.setState((state) => {
                return {
                    resourceStatusShowModal: false,
                };
            });
        }
        if (data.isSoftStamp === true || data.isSoftStamp === "Soft Stamp")
            data["isSoftStamp"] = "Soft Stamp";
        else
            data["isSoftStamp"] = "Hard Stamp";
        this.editedRowData = data;
        this.setGridDate();
       
    }

    fetchIssuedDate = (date) => {
        this.setState({ issuedDate: date });
        this.updatedData.issuedDate = this.state.issuedDate;
    }
    fetchReturnedDate = (date) => {
        this.setState({ returnedDate: date });
        this.updatedData.returnedDate = this.state.returnedDate;
    }
    fetchStartDate = (date) => {
        this.setState({
            startDate: date
        }, () => {
            this.updatedData.startDate = this.state.startDate !== null ? this.state.startDate : '';
            this.props.actions.UpdateResourceStatus(this.updatedData);
            this.updatedData = {};
        });
    }
    fetchEndDate = (date) => {
       
        this.setState({
            endDate: date
        }, () => {
            this.updatedData.endDate = this.state.endDate !== null ? this.state.endDate : '';
            this.props.actions.UpdateResourceStatus(this.updatedData);
            this.updatedData = {};
        });
    }
    showStampModal = (e) => {
        e.preventDefault();
        this.setState({ issuedDate: '', returnedDate: '' });
        this.setState((state) => {
            return {
                resourceStatusShowModal: true,
                isResourceStatusEdit: false
            };
        });
        this.props.actions.UploadDocumentDetails(null);
        this.editedRowData = {};
    }
    //Hiding the modal
    hideStampModal = () => {
        this.setState((state) => {
            return {
                resourceStatusShowModal: false,
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    //Cancel the updated data in model popup
    cancelStampModal=(e)=>{
        e.preventDefault();
        this.updatedData = {};
        if (this.editedRowData.isSoftStamp === true || this.editedRowData.isSoftStamp === "Soft Stamp")
            this.editedRowData["isSoftStamp"] = true;
        else
            this.editedRowData["isSoftStamp"] = false;
            if(this.editedRowData&&this.editedRowData.documents===[])
            {
                this.props.actions.RemoveDocUniqueCode();
            }
            this.setState((state) => {
                return {
                    resourceStatusShowModal: false,
                };
            });
        if(!isEmptyOrUndefine(this.stampDocument)){
            this.updatedData=deepCopy(this.editedRowData);
            this.stampDocument.recordStatus='M';  // Reverting the deleted doc to previous state
            this.updatedData["documents"]=this.stampDocument;
            this.props.actions.UpdateStampDetails(this.editedRowData,this.updatedData);
        }
        this.updatedData = {};
        this.editedRowData = {};
        this.stampDocument={};
    }
    //Input Change Handler
    inputChangeHandler = (e) => {
        if (e.target.type === "number") {
            e.target.value = mobileNumberFormat(e.target.value);
        }
        const fieldName = e.target.name;
        const fieldValue = e.target.value;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    //Resource Status Component: On data change handler
    handlerChange = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        this.updatedData[e.target.name] = fieldValue;
        //To-Do-suresh: Need to change the approach getting Selected dropdown option.
        if (e.target.name === "companyName") {
            // const selectedOption = e.target.selectedOptions[0];
            // this.updatedData[e.target.name] = selectedOption.text;
            // this.updatedData[e.target.id] = selectedOption.value;
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            this.updatedData[e.target.id] = e.nativeEvent.target.value;
        }

        // if (e.target.name === "profileStatus") {
        //     if (e.target.value === 'Active') {
        //         IntertekToaster(localConstant.techSpec.resourceStatus.PROFILE_STATUS_ACTIVE_WARNING, 'warningToast');
        //     }
        // }

        if (e.target.name === "profileStatus") {
            if(e.target.value === 'Inactive' || e.target.value === 'Suspended'){
                this.updatedData['employmentType'] = "Former";
            } else {
                this.updatedData['employmentType'] = null;
            }
        }

        if(e.target.name === "taxonomyStatus"){
            if(e.target.value === "Reject"){
                this.updatedData['profileAction'] = "Send to TM";
                this.updatedData['assignedToUser'] = this.props.profileInfo.assignedByUser;//D926(Ref by ALM Doc on 11-04-2020)
            } else {
                this.updatedData['profileAction'] = "Create/Update Profile";
            }
        }

        if(e.target.name === "approvalStatus"){
            this.isShowTsChangeApprovalStatus=true;
            if(e.target.value === "R"){
                this.updatedData['profileAction'] = "Send to TS";
                this.updatedData['businessInformationComment'] ="";
            } else {
                this.updatedData['profileAction'] = "Create/Update Profile";
                if(isEmpty(e.target.value)){
                    this.updatedData['approvalStatus'] = localConstant.techSpec.tsChangeApprovalStatus.InProgress;
                } 
            }
        }

        this.props.actions.UpdateResourceStatus(this.updatedData);
        this.updatedData = {};
    }
    //Stamp Details: On data change handler
    onChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        if (e.target.name === "countryCode") {        
            if(required(e.target.value)){
                this.updatedData[e.target.name] = "";
                this.updatedData[e.target.id] = "";
            }
            else{
                const countryCode= e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text.split('-',1);
                this.updatedData[e.target.name] = countryCode[0];
                this.updatedData[e.target.id] = e.nativeEvent.target.value;
            }
        }else{
            this.updatedData[result.name] = result.value;
        }
             
    }
    docChangeHandler = (e) => {
        const url = e.target.value;
        const filename = url.replace(/^.*[\\/]/, '');
        this.updatedData[e.target.name] = filename;
      
    }
    //Validation for Stamp Details
    stampDetailsValidation = (data) => {
        if (data.isSoftStamp === undefined || data.isSoftStamp === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Stamp Type', 'warningToast');
            return false;
        }
        if (data.countryCode === undefined || data.countryCode === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Country Code', 'warningToast');
            return false;
        }
        if (data.stampNumber === "" || data.stampNumber === undefined) {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Stamp Number', 'warningToast');
            return false;
        }
        else {
            const stampNum = data.stampNumber.toString();
            const decimalData = stampNum.split(".");
            if (decimalData[1] !== undefined) {
                IntertekToaster(localConstant.techSpec.resourceStatus.INVALID_STAMP_NUMBER, 'warningToast');
                return false;
            }
            if(decimalData[0] && decimalData[0].length < 3 && data.isSoftStamp === 'Hard Stamp'){
                IntertekToaster(localConstant.techSpecValidationMessage.HARD_STAMP_NUMBER_VALIDATION, 'warningToast');
                return false;
            }
            if(decimalData[0] && decimalData[0].length < 4 && data.isSoftStamp === 'Soft Stamp'){
                IntertekToaster(localConstant.techSpecValidationMessage.SOFT_STAMP_NUMBER_VALIDATION, 'warningToast');
                return false;
            }
        }
        if (this.state.issuedDate === null || this.state.issuedDate === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Issued Date', 'warningToast');
            return false;
        }
        return true;
    }

    stampDuplicateValidation =(data) =>{
        if(this.props.stampDetails){
            const isSoftStamp = this.props.stampDetails.filter(x => x.isSoftStamp == true && isEmpty(x.returnedDate) && x.recordStatus != 'D');
            if(isSoftStamp.length > 0 && data.isSoftStamp === 'Soft Stamp' && (this.state.returnedDate == null || this.state.returnedDate == undefined || this.state.returnedDate == "")){
                IntertekToaster(localConstant.techSpecValidationMessage.DUPLICATE_VALIDATION_FOR_STAMP, 'warningToast');
                return false;
            }
            const isHardStamp = this.props.stampDetails.filter(x => x.isSoftStamp == false && isEmpty(x.returnedDate) && x.recordStatus != 'D');
            if(isHardStamp.length > 0 && data.isSoftStamp === 'Hard Stamp' && (this.state.returnedDate == null || this.state.returnedDate == undefined|| this.state.returnedDate == "")){
                IntertekToaster(localConstant.techSpecValidationMessage.DUPLICATE_VALIDATION_FOR_STAMP, 'warningToast');
                return false;
            }
        }
        return true;
    }
    //Adding Stamp details to the grid
    addStampDetails = (e) => {
         e.preventDefault();
        if (this.state.lessReturnDate) {
            IntertekToaster(localConstant.techSpec.common.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
            return false;
        }
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.stampDetailsValidation(this.updatedData) && this.stampDuplicateValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.issuedDate !== "" && this.state.issuedDate !== null)
                    fromDate = this.state.issuedDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.returnedDate !== "" && this.state.returnedDate !== null)
                    toDate = this.state.returnedDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }
                if (this.updatedData.isSoftStamp === "Soft Stamp" || this.updatedData.isSoftStamp === true)
                    this.updatedData["isSoftStamp"] = true;
                else
                    this.updatedData["isSoftStamp"] = false;
                   
                if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData['epin']= this.props.profileInfo.epin!== undefined ? this.props.profileInfo.epin : 0;
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["issuedDate"] = fromDate;
                    this.updatedData["returnedDate"] = toDate;
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.uploadFileRefType === "stampUpload" || x.documentType === 'TS_Stamp');
                    const stamDetails = Object.assign([], this.props.stampDetails);
                    stamDetails.push(this.updatedData);
                    this.props.actions.AddStampDetails(stamDetails);
                    //this.props.actions.UploadDocumentDetails();
                    this.hideStampModal();
                    this.updatedData = {};
                }
            }
        }
        else this.stampDetailsValidation(this.updatedData);
    }
    //Showing modal popup for delete
    deleteStampDetailsHandler = () => {
        const selectedRecords = this.gridChild.getSelectedRows();

        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.RESOURCE_STATUS_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelected,
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
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Deleting the grid detail
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();
        this.gridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteStampDetails(selectedData);
        this.props.actions.HideModal();
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //Updating the edited data to the grid
    updateStampDetails = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.stampDetailsValidation(combinedData) && this.stampDuplicateValidation(combinedData)) {
            let fromDate = "";
            let toDate = "";
            if (this.state.issuedDate !== "" && this.state.issuedDate !== null)
                fromDate = this.state.issuedDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.returnedDate !== "" && this.state.returnedDate !== null)
                toDate = this.state.returnedDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }
         
            if (combinedData.isSoftStamp === "Soft Stamp" || combinedData.isSoftStamp === true)
                this.updatedData["isSoftStamp"] = true;
            else
                this.updatedData["isSoftStamp"] = false;
            if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["issuedDate"] = fromDate;
                this.updatedData["returnedDate"] = toDate;
                this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.uploadFileRefType === "stampUpload" || x.documentType === 'TS_Stamp');
            
                this.props.actions.UpdateStampDetails(this.editedRowData, this.updatedData);
                 this.props.actions.UploadDocumentDetails();
                this.hideStampModal();
                this.stampDocument={};
            }
        }
    }
    /**
     * Handling document cancel
     */
    cancelTimeStampUploadedDocument=(e,documentUniqueName)=>{  
         // def 653 #14 
         if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.stampDetails);
                this.stampDocument=filterdData[0];
            }
        }    
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && x.recordStatus==='M');
            if (newDoc.length > 0) {
                this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }     
        // const req=this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req,this.props.stampDetails);
       }
    //Handling date
    handleDate = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    } else
                        this.setState({ inValidDateFormat: false });
                }
            }
        }
        this.props.actions.UpdateResourceStatus(this.updatedData);
    }
    resourceStartDateBlur = (e) => {
        const isValid = moment(this.state.endDate).isBefore(this.state.startDate);
        if (isValid) {
            IntertekToaster(localConstant.techSpec.common.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
            this.setState({ inToDateLess: true });
        } else
            this.setState({ inToDateLess: false });
    }
    resourceEndDateBlur = (e) => {
        const isValid = moment(this.state.endDate).isBefore(this.state.startDate);
        if (isValid) {
            IntertekToaster(localConstant.techSpecValidationMessage.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
            this.setState({ inToDateLess: true });
        } else this.setState({ inToDateLess: false });
    }
  
    //Date Range Validator
    dateRangeValidator = (from, to) => {
      
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.techSpecValidationMessage.STAMP_DATE_RANGE_VALIDATION, 'warningToast');
            }
        }
        return isInValidDate;
    }

    /** User type based enabling and disabling */
    fieldDisableHandler = () => {  /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        // if (this.props.currentPage === "Edit/View Resource" && this.props.techManager ) {
        //     return isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesTM });
        // }
        // if(this.enableEditColumn()){
        //     return true;
        // }
        // return false;
         if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
            return true;
            } //D1374
        return this.enableEditColumn();
    };
   
    render() {       
        const { stampDetails, companyList, subDivision, profileStatus, employmentStatus,profileInfo,currentPage,interactionMode,countryCodes,userRoleCompanyList,taxonomyDetails,activities } = this.props;
        const profileDetails = profileInfo === undefined ? {} : profileInfo;
     //   const companyDetails = arrayUtil.sort(userRoleCompanyList, 'companyName', 'asc'); //Commented for D363 CR Change
        const subDivisionDetails = arrayUtil.sort(subDivision, 'name', 'asc');
        const profileStatusDetails = arrayUtil.sort(profileStatus, 'name', 'asc');
        
        // if (Array.isArray(companyDetails) && (this.props.userRoleCompanyList).length > 0) {
        //     companyDetails = (this.props.userRoleCompanyList).sort((a, b) => {
        //         return (a.companyName < b.companyName) ? -1 : (a.companyName > b.companyName) ? 1 : 0;
        //     });
        // }
        const countryList=[];
        countryCodes.forEach((iteratedValue) =>{
            const displaycountryList={
                'displayName' : `${ iteratedValue.code } -  ${ iteratedValue.name }`,
            };
            const value=Object.assign({},iteratedValue,displaycountryList);
            countryList.push(value);
        });

        const arrayList=[];
        if (!isEmpty(profileDetails.profileStatus)) {
                const options = isEmptyReturnDefault(employmentStatus);
                options.map(eachItem => {
                    if(profileDetails.profileStatus === "Active"){
                        if(eachItem.name !== "Former" && eachItem.name !== "Offline Technical Specialist" && eachItem.name !== "Prospect"){
                            arrayList.push(eachItem);
                        }
                    } else if(profileDetails.profileStatus === "Inactive" || profileDetails.profileStatus === "Suspended"){
                        if(eachItem.name === "Former"){  
                            arrayList.push(eachItem);
                        }
                    } else{
                    }
                });
        }
        const disableField = this.fieldDisableHandler();
        const isAcceptRequired = (taxonomyDetails && taxonomyDetails.filter(record => record.taxonomyStatus === "IsAcceptRequired").length > 0 && (profileInfo && !profileInfo.isDraft)) ? true : false ;
        const isTsChangeApproveRequired = ( profileDetails &&  profileDetails.isTSApprovalRequired  );
        const isTS=this.enableEditColumn();  // Hot Fixes Changes done as per ITK requirement 

        return (
            <Fragment>
                <CustomModal />
                {this.state.resourceStatusShowModal &&
                    <Modal
                        modalClass="techSpecModal"
                        title={this.state.isResourceStatusEdit ? localConstant.techSpec.resourceStatus.EDIT_NEW_STAMP :
                            localConstant.techSpec.resourceStatus.ADD_NEW_STAMP}
                        buttons={this.state.isResourceStatusEdit ? this.newStampEditButtons : this.newStampAddButtons}
                        isShowModal={this.state.resourceStatusShowModal}>
                        <AddNewStamp
                            issuedDate={this.state.issuedDate}
                            returnedDate={this.state.returnedDate}
                            fetchIssuedDate={this.fetchIssuedDate}
                            fetchReturnedDate={this.fetchReturnedDate}
                            stampDetails={stampDetails}
                            editStampDetails={this.editedRowData}
                            onChangeHandler={this.onChangeHandler}
                            docChangeHandler={this.docChangeHandler}
                            cancelTimeStampUploadedDocument={this.cancelTimeStampUploadedDocument}
                            // editUploadDocument={this.props.uploadDocument.map(x=>x.recordStatus!=='D')}
                            isResourceStatusEdit={this.state.isResourceStatusEdit}
                            stampType={localConstant.commonConstants.stampType}
                            country={countryList}
                        />
                    </Modal>}
                <div className="customCard" >
                    <ResourceStatusComponent
                        startDate={this.state.startDate}
                        endDate={this.state.endDate}
                        fetchStartDate={this.fetchStartDate}
                        fetchEndDate={this.fetchEndDate}
                        resourceStartDateBlur={this.resourceStartDateBlur}
                        resourceEndDateBlur={this.resourceEndDateBlur}
                        subDivision={subDivisionDetails}
                        profileStatus={profileStatusDetails}
                        taxonomyApprovalStatus={localConstant.commonConstants.taxonomyApprovalStatus}
                        employmentStatusOptionList={ arrayList }
                       // companyList={companyDetails}  //Commented for D363 CR Change
                        handlerChange={this.handlerChange}
                        selectedProfileDetails={profileDetails} 
                        currentPage = {currentPage}
                        disableField = {disableField}
                        interactionMode = {interactionMode}
                        isAcceptRequired = {isAcceptRequired}
                        activities={activities}
                        isTsChangeApproveRequired={isTsChangeApproveRequired}
                        tsApprovalStatus={localConstant.commonConstants.techSpecChangeApprovalStatus }
                        isTsChangeApproveCommentEnabled={ this.props.profileInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected }
                        selectedCompany={this.props.selectedCompany} // D363 CR Change
                        rejectionShow ={this.props.rejectionShow && this.props.isTSUserTypeCheck}
                        enableEdit={this.enableEditColumn}
                        isRCUserTypeCheck={this.props.isRCUserTypeCheck}
                        oldProfileAction={this.props.oldProfileAction}
                        />
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.resourceStatus.HARD_SOFT_STAMP} colSize="s12">
                       
                         <ReactGrid gridRowData={stampDetails && stampDetails.filter(x => x.recordStatus !== "D")}
                            gridColData={this.headerData.StampHeader} onRef={ref => { this.gridChild = ref; }} rowClassRules={{ rowHighlight: true }} paginationPrefixId={localConstant.paginationPrefixIds.techSpecStampGridId}/>
                      {/**  Hot Fixes Changes done as per ITK requirement */}
                       {(!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || !isTS ?<div className="right-align mt-2"> 
                            <button  className="waves-effect btn-small"onClick={this.showStampModal} disabled = { interactionMode } >{localConstant.commonConstants.ADD}</button>
                            <button className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.deleteStampDetailsHandler}
                                disabled={(stampDetails && stampDetails.filter(x => x.recordStatus !== "D").length <= 0) ||  interactionMode ? true : false } >
                                {localConstant.commonConstants.DELETE}</button>
                       </div> : null}
                   
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}
export default ResourceStatus;