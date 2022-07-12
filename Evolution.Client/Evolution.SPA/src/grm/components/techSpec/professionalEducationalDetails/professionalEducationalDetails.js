import React, { Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import { getlocalizeData, isEmpty, formInputChangeHandler, bindAction, mergeobjects,compareObjects,isEmptyReturnDefault,bindProperty, isEmptyOrUndefine, deepCopy } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import UploadDocument from '../../../../common/baseComponents/uploadDocument';
import { configuration } from '../../../../appConfig';
import CustomModal from '../../../../common/baseComponents/customModal';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { applicationConstants } from '../../../../constants/appConstants';
import { required } from '../../../../utils/validator';
import { isEditable,isViewable } from '../../../../common/selector';
import { activitycode,levelSpecificActivities } from '../../../../constants/securityConstant'; 
import { userTypeGet } from '../../../../selectors/techSpechSelector';
import Editor from '../../../../common/baseComponents/quill/customEditor';
import sanitize from 'sanitize-html';
const localConstant = getlocalizeData(); 
const ProfessionalSummary = (props) => {  
    const draftTechnicalSpecialistInfoDoc= isEmptyReturnDefault(props.draftTechnicalSpecialistInfo.professionalAfiliationDocuments).map(x=>x.documentName).join(',');
    const selectedProfileDraftTechSpecialistInfoDoc= isEmptyReturnDefault(props.selectedProfileDraftTechSpecialistInfo.professionalAfiliationDocuments).map(x=>x.documentName).join(',');
    const technicalSpecialistInfoDoc= isEmptyReturnDefault(props.technicalSpecialistInfo.professionalAfiliationDocuments).map(x=>x.documentName).join(',');
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_SUMMARY} colSize="s12">
            <div className="row mb-0" >
                <CustomInput 
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_AFFILIATION}
                    labelClass={(props.technicalSpecialistInfo.profileAction === undefined || props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TS || props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) ? "" : ""}
                    type='text'
                    onValueChange={props.professionalInputHandler}
                    colSize='s6'
                   // inputClass={"customInputs "+ (props.RCRMUpdate ? (props.compareDraftData(props.selectedProfileDraftTechnicalSpecialistInfo.professionalAfiliation,props.technicalSpecialistInfo.professionalAfiliation) ?"uninputHighlight":"") : (props.compareDraftData(props.draftTechnicalSpecialistInfo.professionalAfiliation,props.technicalSpecialistInfo.professionalAfiliation) ?"inputHighlight":""))}
                   inputClass={"customInputs "+ (props.compareDraftData(props.draftTechnicalSpecialistInfo.professionalAfiliation,props.selectedProfileDraftTechSpecialistInfo.professionalAfiliation,props.technicalSpecialistInfo.professionalAfiliation) ?"inputHighlight":"")}
                   maxLength={100}
                    value={props.technicalSpecialistInfo.professionalAfiliation ? props.technicalSpecialistInfo.professionalAfiliation : ""}
                    dataValType='valueText'
                    name="professionalAfiliation"
                    autocomplete="off"
                    readOnly={ props.interactionMode||props.disableField} />
                <div className=" col s6 pl-3">
                    <label className={props.isTSUserTypeCheck ||  props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM || props.technicalSpecialistInfo.profileStatus === 'Active' ? 'mandate' :'' }>{localConstant.techSpec.professionalEducationalDetails.UPLOAD_CV}</label>
                    <div className="file-field">
                        <div className="btn">
                            <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                            <input disabled={  props.interactionMode || props.disableField} id="uploadFiles" name="documentName"
                                type="file" onChange={props.uploadDocumentHandler} accept={configuration.allowedFileFormats}  multiple required />
                        </div>
                        <div className=" pl-0 file-path-wrapper ">
                            <input disabled={  props.interactionMode || props.disableField} className={"browser-default file-path validate Input"} value=''/>
                        </div>
                    </div>
                    {
                        !isEmpty(props.technicalSpecialistInfo.professionalAfiliationDocuments) ? (
                            props.technicalSpecialistInfo.professionalAfiliationDocuments.map(doc => {
                                if (doc.recordStatus !== "D") {
                                    return (
                                        <div id={doc.id} className="col s4 pr-1 pl-0 uploadedText">
                                            {/* D556 */}
                                            <a onClick={props.downloadProfessionalSummaryDocuments} className={"pr-2 link " + (props.compareDraftData(draftTechnicalSpecialistInfoDoc,selectedProfileDraftTechSpecialistInfoDoc,technicalSpecialistInfoDoc) ?"inputHighlight":"")}>{doc.documentName}</a>
                                            <i onClick={props.cancelProfessionalSummaryUploadedDocument} className={ (props.interactionMode || props.disableField) ? "" : "zmdi zmdi-close zmdi-hc-lg s1"} ></i> {/** D288 Changes For 13-01-2020 Doc on ALM */}
                                        </div>
                                    );
                                }
                            })
                        ) : null
                    }
                </div>
            </div>
            {/* <div className="row mb-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.professionalEducationalDetails.OVERALL_PROFESSIONAL_SUMMARY}
                    type='textarea'
                    onValueChange={props.professionalInputHandler}
                    colSize='s12'
                    //inputClass={"customInputs "+ (props.RCRMUpdate ? (props.compareDraftData(props.selectedProfileDraftTechnicalSpecialistInfo.professionalSummary,props.technicalSpecialistInfo.professionalSummary) ?"uninputHighlight":"") :(props.compareDraftData(props.draftTechnicalSpecialistInfo.professionalSummary,props.technicalSpecialistInfo.professionalSummary) ?"inputHighlight":""))}
                    inputClass={"customInputs "+ (props.compareDraftData(props.draftTechnicalSpecialistInfo.professionalSummary,props.selectedProfileDraftTechSpecialistInfo.professionalSummary,props.technicalSpecialistInfo.professionalSummary) ?"inputHighlight":"")}
                    maxLength={4000}
                    value={props.technicalSpecialistInfo.professionalSummary ? props.technicalSpecialistInfo.professionalSummary : ""}
                    name="professionalSummary"
                    autocomplete="off" 
                    readOnly = {  props.interactionMode ||props.disableField}
                    labelClass={props.isTSUserTypeCheck || props.technicalSpecialistInfo.profileStatus === 'Active'? 'mandate' :''} //IGO QC D886 (Ref ALM Confirmation Doc)
                />
            </div> */}
            <div className="row mb-0" >
                <div class="col mb-1 s12 s12">
                    <label class={props.isTSUserTypeCheck || props.technicalSpecialistInfo.profileStatus === 'Active'? 'mandate' :''}>{localConstant.techSpec.professionalEducationalDetails.OVERALL_PROFESSIONAL_SUMMARY}</label>            
                    <Editor 
                        editorHtml={ props.technicalSpecialistInfo.professionalSummary ? props.technicalSpecialistInfo.professionalSummary : ""} 
                        onChange = {props.overAllProfessionalSummaryChange}
                        readOnly = {props.interactionMode ||props.disableField}
                        className = {(props.compareDraftData(props.draftTechnicalSpecialistInfo.professionalSummary,props.selectedProfileDraftTechSpecialistInfo.professionalSummary,props.technicalSpecialistInfo.professionalSummary) ?"inputHighlight":"")} //sanity def 182 fix
                    />
                </div>
            </div>
        </CardPanel>
    );
};
const AddWorkDetailsModalPopUp = (props) => { 
    const { workHistoryEditData } = props;
    let work = [];
    let selectedData = [];
    if(workHistoryEditData){
        work = props.draftWorkHistory.filter(x=>x.id == workHistoryEditData.id);
    } 
    if(!isEmpty(props.selectedProfileDraftWorkHistory)){
        selectedData = props.selectedProfileDraftWorkHistory.filter(x => x.id === workHistoryEditData.id);
    } 
    return (
        <Fragment>
            <div className="row p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.professionalEducationalDetails.CUSTOMER}
                    type='text'
                    colSize='s4'
                    name="clientName"
                    maxLength={100}
                    onValueBlur={props.enablingCheckBox}
                    inputClass = {'customInputs '+ (props.compareDraftData( work.length>0?work[0].clientName:" ",(selectedData.length>0?selectedData[0].clientName:" "), props.workHistoryEditData.clientName, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}//selectedData check condition added for D556 
                    onValueChange={props.formInputHandler}
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.clientName}
                    labelClass="mandate"
                    disabled= {props.disableField} />
                   
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.professionalEducationalDetails.PROJECT_NAME}
                    type='text'
                    colSize='s4'
                    name="projectName"
                    //inputClass = {'customInputs '+ (props.compareDraftData(props.workHistoryEditData.projectName, work.length>0?work[0].projectName:undefined)?"inputHighlight":"")}
                    inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].projectName:" "),(selectedData.length>0?selectedData[0].projectName:" "),props.workHistoryEditData.projectName, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    maxLength={100}
                    onValueChange={props.formInputHandler}
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.projectName}
                    disabled= {props.disableField}
                    />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.professionalEducationalDetails.JOB_TITLE}
                    type='text'
                    colSize='s4'
                    name="jobTitle"
                    inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].jobTitle:" "), (selectedData.length>0?selectedData[0].jobTitle:" "),props.workHistoryEditData.jobTitle, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    maxLength={100}
                    onValueChange={props.formInputHandler}
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.jobTitle}
                    disabled= {props.disableField}
                     />
            </div>
            <div className="row s6 p-0" >

                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0 '
                    label={localConstant.techSpec.professionalEducationalDetails.FROM}
                    type='date'
                    colSize='s3'
                    name="fromDate"
                    inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].fromDate:" "), (selectedData.length>0?selectedData[0].fromDate:" "),props.workHistoryEditData.fromDate, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    selectedDate={props.fromDate}
                    onDateChange={props.fetchFromDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    labelClass="mandate" 
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.fromDate}
                    disabled= {props.disableField} />
                {!props.checkedValue ?
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.professionalEducationalDetails.TO}
                        type='date'
                        colSize='s3'
                        name="toDate"
                        inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].toDate:" "), (selectedData.length>0?selectedData[0].toDate:" "),props.workHistoryEditData.toDate, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                        labelClass={!props.checkedValue && props.fromDate ? "mandate" : ""}
                        placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                        selectedDate={props.toDate}
                        onDateChange={props.fetchToDate}
                        dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                        defaultValue={props.workHistoryEditData && props.workHistoryEditData.toDate}
                        disabled= {props.disableField} />
                    : null}
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.professionalEducationalDetails.NUMBER_OF_YEARS_EXPERIENCE}
                    type='text'
                    dataValType='valueText'
                    colSize='s3'
                    name="noofYearsExp"
                    inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].noofYearsExp:" "), (selectedData.length>0?selectedData[0].noofYearsExp:" "),props.workHistoryEditData.noofYearsExp, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    readOnly={props.disableField || props.enableCheckBox}
                    value={props.yearsCalculated} />
                    {/** def 1187 fix */}
               { props.showCurrentCompany || props.checkedValue ? <label className={'col s3 mt-4 '+ (props.compareDraftData(props.workHistoryEditData.checkedValue, props.checkedValue)?"inputHighlightCheckBox":"")} >
                    <input type="checkbox" onChange={props.currentCompancyOnClick}
                        name="isCurrentCompany" checked={props.checkedValue} disabled={ props.disableField } className="filled-in" />
                    <span onClick={props.checkBoxValidation}>{localConstant.techSpec.professionalEducationalDetails.CURRENT_COMPANY}</span>
                </label>
                : null} 
            </div>
            <div className="row s6 p-0" >
            {/* <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.professionalEducationalDetails.DESCRIPTION}
                    type='textarea'
                    colSize='s6'
                    name="description"
                    inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].description:" "), (selectedData.length>0?selectedData[0].description:" "),props.workHistoryEditData.description, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    maxLength={4000}
                    labelClass="mandate" 
                    onValueChange={props.formInputHandler}
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.description}
                    readOnly= {props.disableField}
                     /> */}
                 <div class="col mb-1 s6 s6">     
                <label class="mandate">{localConstant.techSpec.professionalEducationalDetails.DESCRIPTION}</label>            
                <Editor 
                    editorHtml={!isEmptyOrUndefine(props.workHistoryEditData) ? props.workHistoryEditData.description :''}  //IGO QC D928 #issue 2
                    onChange = {props.descriptionChange}
                    readOnly = {props.disableField}
                    className = {props.compareDraftData((work.length>0?work[0].description:" "), (selectedData.length>0?selectedData[0].description:" "),props.workHistoryEditData.description, props.workHistoryEditData.recordStatus)?"inputHighlight":""}
                    />
                </div>     
                <CustomInput
                    hasLabel={true}  
                    divClassName='col'                 
                    label={localConstant.techSpec.professionalEducationalDetails.RESPONSIBILITY}
                    type='textarea'
                    colSize='s6'
                    name="responsibility"
                   inputClass = {'customInputs '+ (props.compareDraftData((work.length>0?work[0].responsibility:" "), (selectedData.length>0?selectedData[0].responsibility:" "),props.workHistoryEditData.responsibility, props.workHistoryEditData.recordStatus)?"inputHighlight":"")}
                    onValueChange={props.formInputHandler}
                    maxLength={200}
                    defaultValue={props.workHistoryEditData && props.workHistoryEditData.responsibility} 
                    readOnly= {props.disableField}/>
      
            </div>
        </Fragment>
    );
};
const AddEducationalSummaryModalPopup = (props) => { 
    const { educationalEditData } = props,
        isLevel1Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }),
        isLevel1Viewable = isViewable({ activities: props.activities, levelType: 'LEVEL_1', viewActivityLevels: levelSpecificActivities.viewActivitiesLevel1 });
    const isEducationSummaryLableMandate= props.educationalEditData && (!isEmptyOrUndefine(props.educationalEditData.qualification) || !isEmptyOrUndefine(props.educationalEditData.institution) || !isEmptyOrUndefine(props.educationalEditData.toDate) || !isEmptyOrUndefine(props.educationalEditData.documents));
    let documents=[];
    let education = [];
    let selectedEduData=[];
    let newDocument=[];
    if(props.educationalEditData && props.educationalEditData.documents)
    {
        documents=props.educationalEditData.documents.filter(x=>x.recordStatus!=='D');
        newDocument  = documents;  //def 848
    }
    if(props.uploadedDocument && props.uploadedDocument.length>0) //def 848
    {
        newDocument= props.uploadedDocument.filter(x => (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_EducationQualification' ) && x.recordStatus!=='D');  
    }
    if(educationalEditData){ //D556
        education = props.draftEducationaldetails && props.draftEducationaldetails.filter(x=>x.id === educationalEditData.id);
    }
    if(!isEmpty(props.selectedProfileDraftEducationaldetails)){
        selectedEduData = props.selectedProfileDraftEducationaldetails.filter(x => x.id === educationalEditData.id);
    } 
    return (
        <Fragment>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.professionalEducationalDetails.EDUCATION_QUALIFICATION}
                    type='text'
                    colSize='s6'
                    name="qualification"
                    inputClass = {'customInputs '+ (props.compareDraftData((education.length>0?education[0].qualification:" "), (selectedEduData.length>0?selectedEduData[0].qualification:" "),props.educationalEditData.qualification,props.educationalEditData.recordStatus)?"inputHighlight":"")}
                    onValueChange={props.formInputHandler}
                    defaultValue={props.educationalEditData.qualification}
                    labelClass={ (props.isEducationSummaryLableMandate || isEducationSummaryLableMandate) ? "mandate" :'' } />
                <CustomInput
                    hasLabel={true}
                    divClassName='col '
                    label={localConstant.techSpec.professionalEducationalDetails.UNIVERSITY_INSTITUTION_COLLEGE}
                    type='text'
                    colSize='s6'
                    name="institution"
                    inputClass = {'customInputs '+ (props.compareDraftData((education.length>0?education[0].institution:" "), (selectedEduData.length>0?selectedEduData[0].institution:" "),props.educationalEditData.institution,props.educationalEditData.recordStatus)?"inputHighlight":"")}
                    onValueChange={props.formInputHandler}
                    defaultValue={props.educationalEditData.institution}
                    labelClass={ (props.isEducationSummaryLableMandate || isEducationSummaryLableMandate) ? "mandate" :'' } />
            </div>
            <div className="row mb-0">
                { (isLevel1Editable || isLevel1Viewable) ? <CustomInput
                    hasLabel={true}
                    divClassName='col  pr-0 '
                    label={localConstant.techSpec.professionalEducationalDetails.DATE_GAINED}
                    type='date'
                    colSize='s6'
                    name="toDate"
                    inputClass = {'customInputs '+ (props.compareDraftData((education.length>0?education[0].toDate:" "), (selectedEduData.length>0?selectedEduData[0].toDate:" "),props.educationalEditData.toDate,props.educationalEditData.recordStatus)?"inputHighlight":"")}
                    selectedDate={props.yearGained}
                    onDateChange={props.fetchYearGained}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    defaultValue={props.educationalEditData.toDate}
                    disabled={  !isLevel1Editable  }
                    labelClass={ (props.isEducationSummaryLableMandate || isEducationSummaryLableMandate) ? "mandate" :'' }
                /> : null}
                <div className="col s6 " >
                    <UploadDocument
                        //Mode={props.editStampDetails.documents[0].recordStatus === 'D' ? !props.isResourceStatusEdit  : props.isResourceStatusEdit }
                        defaultValue={ documents && documents.length>0 ? documents : ''}
                        className={"col s12 pl-0 pr-0 "}
                        label={localConstant.techSpec.resourceStatus.ACKNOWLEDGEMENT_DOCUMENT}
                        cancelUploadedDocument={props.cancelUploadedDocumentData}
                        editDocDetails={props.educationalEditData}
                        gridName={localConstant.techSpec.common.TECHNICALSPECIALISTEDUCATION}
                        labelClass={ (props.isEducationSummaryLableMandate || isEducationSummaryLableMandate) ? "mandate" :''}
                        disabled={ false }
                        uploadFileRefType="educationQualificationUpload"
                        documentType="TS_EducationQualification"
                        inputClass={(props.compareDraftData((education.length > 0 && !isEmptyOrUndefine(education[0].documents) && education[0].documents.length>0  ? education[0].documents[0].documentUniqueName : " "), (selectedEduData.length > 0 && !isEmptyOrUndefine(selectedEduData[0].documents) && selectedEduData[0].documents.length>0  ? selectedEduData[0].documents[0].documentUniqueName  : " "), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ? newDocument[0].documentUniqueName : ''), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ?(newDocument[0].recordStatus==null? undefined : newDocument[0].recordStatus) : undefined)) ? " customInputs inputHighlight" : "")}//def848
                        />
                </div>
            </div>
        </Fragment>
    );
};
class ProfessionalEducationalDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fromDate: '',
            toDate: '',
            yearGained: '',
            noOfYears: '',
            todayDate: moment(),
            inValidDateFormat: false,
            workHistoryShowModal: false,
            workHistoryEditMode: false,
            isEducationalShowModal: false,
            educationalEditMode: false,
            // uploadDocumentCheck: false,
            checkedValue: false, 
            isRowUpdate:false,
            isEducationSummaryLableMandate:false,
        };
        this.educationalSummaryDocument= {};
        this.updatedData = {};
        const functionRefs = {};  
        this.loggedUserType = userTypeGet();
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);
        bindAction(this.headerData.HeaderDataEducationalSummary, "EditColumn", this.editRowHandler);
        bindProperty(this.headerData.HeaderDataEducationalSummary, "toDate", "hide",this.isHiddenColumn()); 

        this.workHistoryAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelWorkHistoryModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addWorkHistoryDetails,
                btnClass: "btn-small ",
                showbtn: !this.fieldDisableHandler(this.loggedUserType)
            }
        ];
        this.workHistoryEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelWorkHistoryModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateWorkHistoryDetails,
                btnClass: "btn-small ",
                showbtn: !this.fieldDisableHandler(this.loggedUserType)
            }
        ];
        this.EducationalAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelEducationalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addEducationalDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.EducationalEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelEducationalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateEducationalDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.updateFromRC_RM=[];
        this.updateFromRC_RM_EduSummary=[];
    }
    enableEditColumn = (e) => {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        return this.fieldDisableHandler(this.userTypes);
    }

    isHiddenColumn = () => { 
        //D667
        const isLevel1EditableandViewable = isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }) || isViewable({ activities: this.props.activities, viewActivityLevels: levelSpecificActivities.viewActivitiesLevel1 }); //D821(ref ALM Doc 18-03-2020)
        return !isLevel1EditableandViewable;
    }

    /** User type based enabling and disabling */
    fieldDisableHandler = (userTypes) => {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        if (this.props.currentPage === "Edit/View Resource") {
            // // if (((this.props.techManager || userTypes.includes(localConstant.techSpec.userTypes.TM)) && !userTypes.includes(localConstant.techSpec.userTypes.TS) )) {
            // //     return !(isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }) || isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }));
            // // }
            // else if (!userTypes.includes(localConstant.techSpec.userTypes.TS)) {
            //     return !(isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }) || isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }));
            // }
            if(this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
                if(!this.props.isTSUserTypeCheck){
                    return true;
                   }
              }
            else if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
                return true;
             } //Added for D1374
             return  !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }) && !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });//D1374 Failed on 29-10-2020
        }
        return false;
    };
   
    fetchFromDate = (date) => {
        this.setState({ fromDate: date },
            () => { this.calculateYears(); });
        this.updatedData.fromDate = this.state.fromDate;
    }
    fetchToDate = (date) => {
        this.setState({ toDate: date },
            () => { this.calculateYears(); });
        this.updatedData.toDate = this.state.toDate !== null ? this.state.toDate : '';
    }
    fetchYearGained = (date) => {
        this.setState({ yearGained: date });
        this.updatedData.toDate = this.state.yearGained ? this.state.yearGained : '';
    }
    dateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                if(to === this.state.todayDate){
                    isInValidDate = true;
                    IntertekToaster('From date should not be higher than today date', 'warningToast');
                } else{
                    isInValidDate = true;
                    IntertekToaster(localConstant.techSpec.common.WORK_HISTORY_INVALID_DATE_RANGE, 'warningToast');
                }
            }
        }
        return isInValidDate;
    }
    professionalInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e); 
        const url = inputvalue.value; 
        this.updatedData[inputvalue.name] = url; 
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedProfessionalEducationInformation(true); 
        }
        this.updatedData["btnDisable"]=false;
        this.props.actions.UpdateProfessionalSummary(this.updatedData);
        this.updatedData = {};
    }
    overAllProfessionalSummaryChange = (content, delta, source, editor) => { 
        if(source === "user"){
            this.updatedData["professionalSummary"] = content; 
            this.props.actions.UpdateProfessionalSummary(this.updatedData);             
            this.updatedData = {};
        }
    }
    descriptionChange = (e) =>{
        this.updatedData["description"] = e; 
    }
    receivingWorkHistoryDateProps = () => {
        if (this.editedRowData) {
            if ((this.editedRowData.toDate !== null && this.editedRowData.toDate !== "")) {
                this.setState({ toDate: moment(this.editedRowData.toDate) });
            }
            else {
                this.setState({ toDate: "" });
            }
            if ((this.editedRowData.fromDate !== null && this.editedRowData.fromDate !== "")) {
                this.setState({ fromDate: moment(this.editedRowData.fromDate) });
            }
            else {
                this.setState({ fromDate: "" });
            }
        }
        else {
            this.setState({
                fromDate: "",
                toDate: "",
            });
        }
    }
    editWorkHistoryRowHandler = (data) => {
        this.editedRowData = data;
        let toDate =data.toDate;
        this.setState((state) => {
            return {
                workHistoryEditMode: true,
                workHistoryShowModal: !state.workHistoryShowModal,
                noOfYears: data.noofYearsExp
            };
        });
        if (this.editedRowData.isCurrentCompany === true) {
            this.setState({ checkedValue: true });
        }
        else {
            this.setState({ checkedValue: false });
        }
        this.receivingWorkHistoryDateProps();
        if(isEmpty(toDate)){
            toDate = this.state.todayDate;
        }
        this.calculatingYears(data.fromDate, toDate);
    }
    currentCompancyOnClick = (e) => {
        this.setState({ toDate: "", });
        if (e.target.checked === true) {
            this.setState({ checkedValue: true });
            if (!this.dateRangeValidator(this.state.fromDate, this.state.todayDate) && !this.state.inValidDateFormat) {
                this.calculatingYears(this.state.fromDate, this.state.todayDate);
            }
        }
        else {
            this.setState({
                noOfYears: "",
                checkedValue: false
            });
        }
    }
    workHistoryShowModalOpen = (e) => {
        e.preventDefault();
        this.setState({
            enableCheckBox: true,
            checkedValue: false,
            workHistoryShowModal: true,
            workHistoryEditMode: false,
            fromDate: '',
            toDate: '',
            noOfYears: '',
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    cancelWorkHistoryModal = (e) => {
        e.preventDefault();
        this.setState({
            workHistoryShowModal: false,
            noOfYears: '',
            enableCheckBox: false,
        });
        this.updatedData = {};
    }
    enablingCheckBox = () => {
        if ((!isEmpty(this.state.fromDate)) && (!isEmpty(this.updatedData.clientName))) {
            this.setState({ enableCheckBox: false });
        }
        else if ((!isEmpty(this.state.fromDate)) && (!isEmpty(this.editedRowData.clientName))) {
            this.setState({ enableCheckBox: false });
        }
        else this.setState({ enableCheckBox: true });
    }
    calculatingYears = (fromDate, toDate) => {
        if(!isEmpty(fromDate)&&!isEmpty(toDate)){
            const from = moment([
                moment(fromDate).year(), moment(fromDate).month(), moment(fromDate).date()
            ]);
            const to = moment([
                moment(toDate).year(), moment(toDate).month(), moment(toDate).date()
            ]);
            const total = (to.diff(from, 'years') + "yrs").concat(" " + to.diff(from, 'months') % 12 + "months");
            this.setState({ noOfYears: total });
        }
    }
    calculateYears = (e) => {
        if ((!isEmpty(this.state.fromDate)) && (!isEmpty(this.updatedData.clientName)))
            this.enablingCheckBox();
        else if ((!isEmpty(this.state.fromDate)) && (!isEmpty(this.editedRowData.clientName)))
            this.enablingCheckBox();
        else if (isEmpty(this.state.fromDate) || (isEmpty(this.editedRowData.clientName))) {
            this.setState({
                enableCheckBox: true,
                checkedValue: false
            });
        }
        if (this.state.checkedValue === true) {
            this.updatedData["toDate"] = "";
            this.setState({ toDate: "" });
            if (!this.dateRangeValidator(this.state.fromDate, this.state.todayDate) && !this.state.inValidDateFormat) {
                this.calculatingYears(this.state.fromDate, this.state.todayDate);
            }
        }
        else {
            let fromDate = "";
            let toDate = "";
            if (this.state.fromDate !== "" && this.state.fromDate !== null) {
                fromDate = this.state.fromDate;
            }
            if (this.state.toDate !== "" && this.state.toDate !== null) {
                toDate = this.state.toDate;
            }
            if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (fromDate !== "" && toDate !== "") {
                    this.calculatingYears(fromDate, toDate);
                }
            }
        }
    }
    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedProfessionalEducationInformation(true); 
        } 
        this.updatedData[inputvalue.name] = inputvalue.value;
        if(([ 'qualification','institution','toDate' ]).findIndex(x=>x==inputvalue.name) >=0)
        {
            this.setState({ isEducationSummaryLableMandate: !isEmptyOrUndefine(inputvalue.value) });
        } 
    }
    docInputHandler = (e) => {
        const url = e.target.value;
        const filename = url.replace(/^.*[\\/]/, '');
        this.updatedData[e.target.name] = filename;
    }
    // Validation for Work History Details
    workHistoryDetailsValidation = (data, checkedValue, fromDate, toDate) => {
        if (data.clientName === undefined || data.clientName === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Client/Company', 'warningToast');
            return false;
        }
        // if (data.projectName === undefined || data.projectName === "") {
        //     IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Project Name', 'warningToast');
        //     return false;
        // }
        // if (data.jobTitle === undefined || data.jobTitle === "") {
        //     IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Job Title', 'warningToast');
        //     return false; 
        // }
        if (isEmpty(fromDate)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' From Date', 'warningToast');
            return false;
        }
        if ((!checkedValue && !isEmpty(fromDate)) && isEmpty(toDate)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' To Date', 'warningToast');
            return false;
        }
        if ( isEmptyOrUndefine(data.description) || isEmptyOrUndefine(data.description.replace(/(<([^>]+)>)/ig, ''))) {  //D1395
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Description ', 'warningToast');
            return false; 
        }
        if(data.description.length > 4000){
            IntertekToaster(localConstant.techSpecValidationMessage.WORK_HISTORY_DESCRIPTION_EXISTS, 'warningToast');
            return false; 
        }
        return true;
    }
    addWorkHistoryDetails = (e) => { 
        e.preventDefault();
        let fromDate = "";
        let toDate = "";
        if (this.state.fromDate !== "" && this.state.fromDate !== null) {
            fromDate = this.state.fromDate.format();
        }
        if (this.state.toDate !== "" && this.state.toDate !== null) {
            toDate = this.state.toDate.format();
        }
        if (this.state.inValidDateFormat) {
            IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast rsValidDateWarn');
            return false;
        }

        if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {

            if (this.workHistoryDetailsValidation(this.updatedData, this.state.checkedValue, this.state.fromDate, this.state.toDate)) {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["id"] = - Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["epin"] = this.props.epin !== null ? this.props.epin : 0;
                this.updatedData["fromDate"] = fromDate;
                this.updatedData["toDate"] = toDate;
                this.updatedData["isCurrentCompany"] = this.state.checkedValue;
                this.updatedData["noofYearsExp"] = this.state.noOfYears;
                this.updateFromRC_RM.push(this.updatedData);
                this.props.actions.AddWorkHistoryDetails(this.updatedData);
                this.setState({ workHistoryShowModal: false });
                this.updatedData={};
            }
        }
    }
    updateWorkHistoryDetails = (e) => { 
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        let fromDate = "";
        let toDate = "";
        if (this.state.fromDate !== "" && this.state.fromDate !== null)
            fromDate = this.state.fromDate.format();
        if (this.state.toDate !== "" && this.state.toDate !== null)
            toDate = this.state.toDate.format();
        if (this.state.inValidDateFormat) {
            IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast rsValidDateWarn');
            return false;
        }
        if (this.workHistoryDetailsValidation(combinedData, this.state.checkedValue, this.state.fromDate, this.state.toDate)) {
            if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["fromDate"] = fromDate;
                this.updatedData["toDate"] = toDate;
                this.updatedData["isCurrentCompany"] = this.state.checkedValue;
                this.updatedData["noofYearsExp"] = this.state.noOfYears;
                this.updateFromRC_RM.push(this.updatedData);
                this.props.actions.UpdateWorkHistoryDetails(this.updatedData, this.editedRowData);
                this.setState({ workHistoryShowModal: false });
                this.updatedData = {};
            }
        }
    }
    workHistorydeleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteWorkHistoryDetails(selectedData);
        this.props.actions.HideModal();
    }
    workHistoryDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.PROFESSIONAL_SUMMARY,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.workHistorydeleteSelected,
                        className: "m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    recievingNextProps = (e) => {
        if (this.editedRowData) {
            if (this.editedRowData.toDate !== null && this.editedRowData.toDate !== "") {
                this.setState({ yearGained: moment(this.editedRowData.toDate) });
            }
            else {
                this.setState({ yearGained: "" });
            }
        }
        else this.setState({ yearGained: "" });
    }
    editRowHandler = (data) => {
        this.setState((state) => {
            return {
                isEducationalShowModal: !state.isEducationalShowModal,
                educationalEditMode: true,
            };
        });
        this.editedRowData = data;

        this.recievingNextProps();
    }

    educationalShowModal = (e) => {
        e.preventDefault();
        this.setState({
            isEducationalShowModal: true,
            educationalEditMode: false,
            yearGained: ''
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    cancelEducationalModal = (e) => { 
        e.preventDefault();
        this.setState({ 
            isEducationalShowModal: false,
            isEducationSummaryLableMandate: false });
        if( !isEmptyOrUndefine(this.educationalSummaryDocument))
        {
            this.updatedData=deepCopy(this.editedRowData);
            this.educationalSummaryDocument.recordStatus=null; //def 848 doc highlight  // Reverting the deleted doc to previous state
            this.updatedData["documents"]=this.educationalSummaryDocument;
            this.updateFromRC_RM_EduSummary.push(this.updatedData);
            this.props.actions.UpdateEducationalDetails(this.updatedData,this.editedRowData);
        } 
        this.updatedData = {};
        this.editedRowData = {};
        this.educationalSummaryDocument={};
        this.props.actions.RemoveDocUniqueCode();
    }
    //validation for Educational Summary - Educational Qualification
    educationalSummaryValidation = (data) => {  //def 653 #8 
        // if(!isEmptyOrUndefine(data.qualification) || !isEmptyOrUndefine(data.institution) || !isEmptyOrUndefine(data.toDate) || !isEmptyOrUndefine(data.documents) ) // Commented for Sanity Def 192
        // {
            if (isEmptyOrUndefine(data.qualification)) {
                IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + localConstant.techSpec.common.EDUCATION_QALIFICATION_REQUIRED, 'warningToast');
                return false;
            }
            if (isEmptyOrUndefine(data.institution)) {
                IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + localConstant.techSpec.common.EDUCATION_QALIFICATION_INSTITUTE, 'warningToast');
                return false;
            }
            if (isEmptyOrUndefine(data.toDate)) {
                IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + localConstant.techSpec.common.EDUCATION_QALIFICATION_DATE_GAINED, 'warningToast');
                return false;
            }
            if (isEmptyOrUndefine(data.documents) || (Array.isArray(data.documents) && data.documents.filter(x => x.recordStatus == 'M' || x.recordStatus == 'N' || !x.recordStatus).length == 0)) { //def 855 doc audit changes
                IntertekToaster(localConstant.techSpec.common.NO_FILE_SELECTED, 'warningToast');
                return false;
            }
       // }
        
        return true;
    }
    addEducationalDetails = (e) => {  
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) { 
                let gainedYear = "";
                if (this.state.yearGained !== "" && this.state.yearGained !== null) { //def 653 #8 
                    gainedYear = this.state.yearGained.format();
                    this.updatedData["toDate"] = gainedYear;
                } 
                if(!isEmptyOrUndefine(this.props.uploadDocument)) //def 653 #8  
                {
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus=== "N" && (x.uploadFileRefType === "educationQualificationUpload" || x.documentType === 'TS_EducationQualification')); //sanity def 98 fix
                }
                if (this.educationalSummaryValidation(this.updatedData)) {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["epin"] = this.props.epin !== null ? this.props.epin : 0;  
                    this.updateFromRC_RM_EduSummary.push(this.updatedData);
                    this.props.actions.AddEducationalDetails(this.updatedData);
                    this.props.actions.UploadDocumentDetails();
                    this.setState({ isEducationalShowModal: false });
                    this.updatedData = {}; 
            }
        }
        else this.educationalSummaryValidation(this.updatedData);
    }
    updateEducationalDetails = (e) => { 
        e.preventDefault();
        let gainedYear = "";
        if (this.state.yearGained !== "" && this.state.yearGained !== null) {
            gainedYear = this.state.yearGained.format();
            this.updatedData["toDate"] = gainedYear;
        }
        if (!isEmptyOrUndefine(this.props.uploadDocument)) //def 653 #8  
        { 
            this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === "educationQualificationUpload" || x.documentType === 'TS_EducationQualification')); //sanity def 98 fix
        }
        else if(this.editedRowData.documents && !isEmptyOrUndefine(this.editedRowData.documents[0])) 
        {
            this.updatedData["documents"] = this.editedRowData.documents.filter(x => x.uploadFileRefType === "educationQualificationUpload" || x.documentType === 'TS_EducationQualification'); 
        }
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.educationalSummaryValidation(combinedData)) {
            if (this.editedRowData.recordStatus !== "N")
                this.updatedData["recordStatus"] = "M";

            this.updateFromRC_RM_EduSummary.push(this.updatedData);
            this.props.actions.UpdateEducationalDetails(this.updatedData, this.editedRowData);
            this.props.actions.UploadDocumentDetails();
            this.setState({ isEducationalShowModal: false });
            this.editedRowData = {};
            this.updatedData = {};
            this.educationalSummaryDocument={};
        }
    }
    educationalDeleteSelected = () => {
        const selectedData = this.secondchild.getSelectedRows();
        this.secondchild.removeSelectedRows(selectedData);
        this.props.actions.DeleteEducationalDetails(selectedData);
        this.props.actions.HideModal();
    }
    educationalDetailDeleteClickHandler = () => {
        const selectedRecords = this.secondchild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.EDUCATIONAL_DETAILS,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.educationalDeleteSelected,
                        className: "m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    /**
     * Handling document cancel
     */
    cancelUploadedDocument = (e) => {
        const req = this.editedRowData.documents[0];
        this.props.actions.RemoveDocumentDetails(req, this.props.workHistory);
    }

    eduSummaryCancelUploadedDocument = (e,documentUniqueName) => {  
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.educationaldetails);
                this.educationalSummaryDocument=filterdData[0];
            }
        }  
        // def 653 #14 
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
            if (newDoc.length > 0) {
                this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
    }
    /**
     * Upload the multiple professional summary documents
     */
    uploadDocumentHandler = (e) => { 
        e.preventDefault();
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        const newlyCreatedRecords = [];
        const filesToBeUpload = [];
        const failureFiles = [];
        const documentUniqueName = [];
        const documentFile = Array.from(document.getElementById("uploadFiles").files);
        if (documentFile.length > 0) {
            documentFile.map(document => {
                if (parseInt(document.size / 1024) > configuration.fileLimit) { //As discussed with francina(60MB) - 21-01-2021
                    failureFiles.push(document.name);
                }
                else {
                    filesToBeUpload.push(document);
                }
                return document;
            });
            if (filesToBeUpload.length > 0) {
                for (let i = 0; i < filesToBeUpload.length; i++) {
                    this.updatedData = {
                        documentName: filesToBeUpload[i].name,
                        moduleCode: "TS",
                        requestedBy: this.props.loggedInUser,
                        documentType:"TS_ProfessionalAfiliation",
                    };
                    if(this.props.currentPage === localConstant.techSpec.common.EDIT_VIEW_TECHSPEC){
                        this.updatedData.moduleCodeReference = `${ this.props.selectedEpinNo }`;
                    } // Added For ITK Def 1538 
                    documentUniqueName.push(this.updatedData);
                    this.updatedData = {};
                }
                this.props.actions.FetchDocumentUniqueName(documentUniqueName, filesToBeUpload,true) //Sanity Def 99
                    .then(response => {
                        if (!isEmpty(response)) {
                            for (let i = 0; i < response.length; i++) {
                                if (!response[i].status) {
                                    this.updatedData = {
                                        documentName: filesToBeUpload[i].name,
                                        recordStatus: "M",
                                        createdOn: date,
                                        id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                        epin: this.props.epin ? this.props.epin : 0,
                                        documentUniqueName: response[i].uploadedFileName,
                                        modifiedBy: this.props.loggedInUser,
                                        moduleCode: response[i].moduleCode,
                                        moduleRefCode: response[i].moduleReferenceCode,
                                        status: "C"
                                    };
                                    newlyCreatedRecords.push(this.updatedData);
                                    this.updatedData = {};
                                }
                            }
                            if (newlyCreatedRecords.length > 0) {
                                this.props.actions.AddProfessionalEducationDocuments(newlyCreatedRecords);
                                document.getElementById("uploadFiles").value = "";
                            }
                        };
                    });
            }
            if (failureFiles.length > 0) {
                IntertekToaster(failureFiles.toString() + localConstant.project.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
            }
        }
    }
    /**
     * Remove selected professional summary document
     */
    cancelProfessionalSummaryUploadedDocument = (e) => { 
        const removeItem = parseInt(e.target.parentElement.id);
        this.props.actions.RemoveProfessionalEducationDocument(removeItem);
    }
    /**
     * Download selected professional summary document
     */
    downloadProfessionalSummaryDocuments = (e) => { 
        this.props.technicalSpecialistInfo.professionalAfiliationDocuments.forEach(doc => {
            if (doc.id === parseInt(e.target.parentElement.id)) {
                const allowDonloadFileFormat = configuration.allowedFileFormats.indexOf(doc.documentName.substring(doc.documentName.lastIndexOf(".")).toLowerCase()); //Def 205  #7 
                if(allowDonloadFileFormat > -1){
                this.props.actions.DownloadDocumentData(doc);
                } else{
                    IntertekToaster(localConstant.commonConstants.NO_LONGER_SUPPORT, 'warningToast CustDocDelDocChk'); 
                }
                
            }
        });
    }

    /** work history draft field comparision */
    workHistoryDraftData = (originalData) => { 
        let work=0;
        if (originalData && originalData.data && originalData.data.clientName && originalData.data.recordStatus!='M' && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
            if (!isEmpty(this.props.draftDataToCompare)) {
                work = this.props.draftWorkHistory.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result) {
                        return true;   //D556
                    }
                    return false;
                } else if (!isEmpty(this.updateFromRC_RM)) {
                    work = this.updateFromRC_RM.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const currentResult = compareObjects(work[0], originalData.data);
                        return !currentResult;
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedProfileDraftWorkHistory.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                            return compareDataResult;
                        }
                    }
                }else if (!isEmpty(this.props.selectedProfileDraftWorkHistory)) {
                    work = this.props.selectedProfileDraftWorkHistory.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        return compareDataResult;
                    }
                } else {
                    return false;
                }  
            } else {
                this.updateFromRC_RM = [];
                return false;
            }
        }
        return false;
    }

    /** education draft field comparision */
    // educationDraftData = ( originalData ) => {
    //     if(originalData && originalData.data && originalData.data.qualification){
    //         const education = this.props.draftEducationaldetails.filter(x=>x.id == originalData.data.id);
    //         if(education.length > 0){
    //             const result = compareObjects(this.excludeProperty(education[0]),this.excludeProperty(originalData.data));
    //             return !result;
    //         }
    //         return true;
    //     }
    //     return false;
    // }

    educationDraftData = ( originalData ) => {
        let work=0;
        if(originalData && originalData.data && originalData.data.qualification && originalData.data.recordStatus!='M' && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )){
            if(!isEmpty(this.props.draftDataToCompare)){               
                    work = this.props.draftEducationaldetails.filter(x=>x.id === originalData.data.id);                          
                if(work.length > 0){
                    const result = compareObjects(this.excludeProperty(work[0]),this.excludeProperty(originalData.data));
                    if(!result ){//&& !this.props.isRCRMUpdate
                        return true;   //D556
                    }
                    return false;
                } else if(!isEmpty(this.updateFromRC_RM_EduSummary)){
                    work = this.updateFromRC_RM_EduSummary.filter(x=>x.id === originalData.data.id);
                    if(work.length > 0){
                        const currentResult = compareObjects(work[0],originalData.data);
                        return !currentResult;
                    }
                    else if(this.props.isRCRMUpdate){
                         work = this.props.selectedProfileDraftEducationaldetails.filter(x=>x.id === originalData.data.id);
                           if(work.length > 0 ){
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]),this.excludeProperty(originalData.data));
                            return compareDataResult;
                           }
                    }
                }else if (!isEmpty(this.props.selectedProfileDraftEducationaldetails)) {
                    work = this.props.selectedProfileDraftEducationaldetails.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        return compareDataResult;
                    }
                } else {
                    return false;
                }              
            }else{
                 this.updateFromRC_RM_EduSummary=[];
                 return false;
            }        
        }
        return false;
    }

    excludeProperty = (object) => {
        delete object.recordStatus;
        delete object.lastModification;
        delete object.updateCount;
        delete object.modifiedBy;
        return object;
    }
     /** Compare field value with draft */
     compareDraftData = (draftValue,selectedDraftValue,fieldValue,recordStatus) => { 
         const _this = this;
         if (recordStatus !== undefined || fieldValue==undefined) {
             return false;
         }
         else if (!isEmpty(_this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck ) ) {
             if (this.props.isRCRMUpdate == false) {
                 if (draftValue === undefined && !required(fieldValue)) {
                     return true;
                 }
                 else if (draftValue !== undefined && draftValue !== fieldValue && selectedDraftValue !== null) {//D556
                     return true;
                 }
                 return false;
             } else if (this.props.isRCRMUpdate) {
                 //RC RM While modifing Value Not showing Highlight
                 if (draftValue === undefined && !required(fieldValue)) {
                     return false;
                 }
                 else if (draftValue !== undefined && draftValue !== fieldValue) {
                    return (selectedDraftValue === fieldValue);
                }
                 else if (draftValue !== undefined && draftValue !== null && selectedDraftValue === draftValue && draftValue !== fieldValue) {
                     return (selectedDraftValue !== fieldValue);
                 }
                 return false;
             }
         }
         return false;
        };
    render() {        
        const { workHistory, educationaldetails, technicalSpecialistInfo, interactionMode,draftDataToCompare,selectedProfileDraft,selectedProfileDraftTechSpecialistInfo,selectedProfileDraftWorkHistory,activities,selectedProfileDraftEducationaldetails,uploadDocument } = this.props; //selectedProfileDraftEducationaldetails added for D556
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        this.userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        const disableField = this.fieldDisableHandler(this.userTypes); 
        bindAction(this.headerData.HeaderDataWorkHistoryDetails, "EditColumn", this.editWorkHistoryRowHandler);  
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.state.workHistoryShowModal &&
                    <Modal modalClass="techSpecModal"
                        title={this.state.workHistoryEditMode ? localConstant.techSpec.professionalEducationalDetails.EDIT_WORK_HISTORY_DETAILS
                            : localConstant.techSpec.professionalEducationalDetails.ADD_WORK_HISTORY_DETAILS}
                        buttons={this.state.workHistoryEditMode ? this.workHistoryEditButtons : this.workHistoryAddButtons}
                        isShowModal={this.state.workHistoryShowModal}
                    >
                        <AddWorkDetailsModalPopUp
                            fromDate={this.state.fromDate} fetchFromDate={this.fetchFromDate}
                            toDate={this.state.toDate} fetchToDate={this.fetchToDate}
                            formInputHandler={this.formInputHandler}
                            descriptionChange={this.descriptionChange}
                            workHistoryEditData={this.editedRowData}
                            yearsCalculated={this.state.noOfYears}
                            enableCheckBox={this.state.enableCheckBox}
                            enablingCheckBox={this.enablingCheckBox}
                            isWorkHistoryEditCheck={this.state.workHistoryEditMode}
                            currentCompancyOnClick={this.currentCompancyOnClick}
                            checkedValue={this.state.checkedValue}
                            checkBoxValidation={this.checkBoxValidation} 
                            draftWorkHistory={this.props.draftWorkHistory}
                            compareDraftData={this.compareDraftData}
                            selectedProfileDraftWorkHistory={selectedProfileDraftWorkHistory}
                            showCurrentCompany= { isEmptyReturnDefault(workHistory).filter(x=> x.isCurrentCompany === true && x.recordStatus !== 'D').length==0 } // def 1187 fix
                            disableField={disableField}/>
                    </Modal>}

                {this.state.isEducationalShowModal &&
                    <Modal modalClass="techSpecModal"
                        title={this.state.educationalEditMode ? localConstant.techSpec.professionalEducationalDetails.EDIT_EDUCATIONAL_SUMMARY
                            : localConstant.techSpec.professionalEducationalDetails.ADD_EDUCATIONAL_SUMMARY}
                        buttons={this.state.educationalEditMode ? this.EducationalEditButtons : this.EducationalAddButtons}
                        isShowModal={this.state.isEducationalShowModal}
                    >
                        <AddEducationalSummaryModalPopup
                            yearGained={this.state.yearGained}
                            fetchYearGained={this.fetchYearGained}
                            formInputHandler={this.formInputHandler}
                            docInputHandler={this.docInputHandler}
                            educationalEditData={this.editedRowData}
                            isEducationalCheck={this.state.educationalEditMode}
                            cancelUploadedDocumentData={this.eduSummaryCancelUploadedDocument}
                            draftEducationaldetails={this.props.draftEducationaldetails}
                            compareDraftData={this.compareDraftData}
                            selectedProfileDraftEducationaldetails={selectedProfileDraftEducationaldetails} //D556
                            activities = {activities} 
                            uploadedDocument = {uploadDocument}
                            isEducationSummaryLableMandate={ this.state.isEducationSummaryLableMandate } /> 
                    </Modal>}
                <div className="customCard" >
                    <ProfessionalSummary professionalInputHandler={this.professionalInputHandler}
                        overAllProfessionalSummaryChange={this.overAllProfessionalSummaryChange}
                        editDocument={this.editDocument}
                        technicalSpecialistInfo={technicalSpecialistInfo} 
                        interactionMode={this.props.interactionMode}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        cancelProfessionalSummaryUploadedDocument={this.cancelProfessionalSummaryUploadedDocument}
                        downloadProfessionalSummaryDocuments={this.downloadProfessionalSummaryDocuments}
                        draftTechnicalSpecialistInfo={this.props.draftTechnicalSpecialistInfo}
                        compareDraftData={this.compareDraftData}
                        selectedProfileDraft={selectedProfileDraft}
                        selectedProfileDraftTechSpecialistInfo={ selectedProfileDraftTechSpecialistInfo }
                        RCRMUpdate={this.props.isRCRMUpdate} 
                        isTSUserTypeCheck={this.props.isTSUserTypeCheck}
                        disableField={disableField} />
                    <CardPanel className="white lighten-4 black-text"
                        title={localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS}
                        titleClass={ this.props.technicalSpecialistInfo.profileStatus === 'Active' || this.props.isTSUserTypeCheck ? "pl-0 bold mandate" : "pl-0 bold"} colSize="s12">
                            <ReactGrid gridColData={this.headerData.HeaderDataWorkHistoryDetails}
                            gridRowData={workHistory && workHistory.filter(x => x.recordStatus !== "D")} onRef={ref => { this.child = ref; }} rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(draftDataToCompare)?this.workHistoryDraftData:null} paginationPrefixId={localConstant.paginationPrefixIds.techSpecWorkHistoryGridId}/>
                       {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                       {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || !disableField)  && <div className="right-align add-text mt-2">
                            <a disabled={ this.props.interactionMode|| disableField} onClick={this.workHistoryShowModalOpen} className="btn-small  waves-effect">{localConstant.commonConstants.ADD}</a>
                            <a onClick={this.workHistoryDeleteClickHandler}
                                className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                                disabled={workHistory && workHistory.filter(x => x.recordStatus !== "D").length <= 0 || disableField || this.props.interactionMode ? true : false}>
                                {localConstant.commonConstants.DELETE}</a>
                        </div>}
                    </CardPanel>
                    <CardPanel className="white lighten-4 black-text"
                        title={localConstant.techSpec.professionalEducationalDetails.EDUCATIONAL_SUMMARY} colSize="s12">
                           
                           <ReactGrid gridColData={this.headerData.HeaderDataEducationalSummary}
                            gridRowData={educationaldetails && educationaldetails.filter(x => x.recordStatus !== "D")}
                            onRef={ref => { this.secondchild = ref; }} rowClassRules={{ rowHighlight: true }} draftData={(!isEmpty(draftDataToCompare)?this.educationDraftData:null)} paginationPrefixId={localConstant.paginationPrefixIds.techSpecEducationalGridId}/>
                       {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                       {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || !disableField) && <div className="right-align add-text mt-2">
                            <a disabled={  this.props.interactionMode} onClick={this.educationalShowModal} className="btn-small  waves-effect">{localConstant.commonConstants.ADD}</a>
                            <a onClick={this.educationalDetailDeleteClickHandler}
                                disabled={educationaldetails && educationaldetails.filter(x => x.recordStatus !== "D").length <= 0   || this.props.interactionMode ? true : false}
                                className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.commonConstants.DELETE}</a>
                       </div> }
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}
export default ProfessionalEducationalDetails;