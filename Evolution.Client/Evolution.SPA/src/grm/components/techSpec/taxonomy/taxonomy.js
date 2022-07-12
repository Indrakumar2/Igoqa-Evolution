import {
    getlocalizeData, mergeobjects, isEmpty, mobileNumberFormat, bindAction, isArray, mapArrayObject, findObject,
    addElementToArray, formInputChangeHandler, isEmptyReturnDefault, isUndefined, compareObjects,bindProperty,isEmptyOrUndefine,deepCopy
} from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import moment from 'moment';
import CustomModal from '../../../../common/baseComponents/customModal';
import React, { Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import UploadDocument from '../../../../common/baseComponents/uploadDocument';
import { applicationConstants } from '../../../../constants/appConstants';
import arrayUtil from '../../../../utils/arrayUtil';
import { required } from '../../../../utils/validator';
import {  levelSpecificActivities } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
import { userTypeGet } from '../../../../selectors/techSpechSelector';

const localConstant = getlocalizeData();

const ApprovedTaxonomy = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.Taxonomy.APPROVED_TAXONOMY} colSize="s12" >
            <ReactGrid gridColData={props.headerData} gridRowData={props.gridRowData} onRef={props.onRefer} paginationPrefixId={localConstant.paginationPrefixIds.techSpecApprovedTaxonomyGridId} />
        </CardPanel>
    );
};
const InternalTrainingDetails = (props) => { 
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.Taxonomy.INTERNAL_TRAINING_DETAILS} colSize="s12" >

            {/* <ReactGrid gridColData={props.headerData} gridRowData={props.gridRowData} onRef={props.onRefer} rowClassRules={props.rowClass} draftData={props.isDraftData ? props.draftData : null} /> */}
            <ReactGrid gridColData={props.headerData} gridRowData={props.gridRowData} onRef={props.onRefer} paginationPrefixId={localConstant.paginationPrefixIds.techSpecInternalTrainingGridId} />

            {(!isEmpty(props.pageMode) && props.pageMode !== localConstant.commonConstants.VIEW) || !props.disableEdit() ?  /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
              <div className="right-align mt-2">
                <button  disabled={ props.interactionMode } onClick={props.AddInternalTrainDetails} className="waves-effect btn btn-small">{localConstant.commonConstants.ADD} </button>
                <button disabled={(props.gridRowData <= 0) || props.interactionMode } onClick={props.DeleteHAndler}
                    className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.commonConstants.DELETE}</button>
            </div> : null }
        </CardPanel>
    );
};
const CompetencyDetails = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.Taxonomy.COMPETENCY_DETAILS} colSize="s12" >

            <ReactGrid gridColData={props.headerData} gridRowData={props.gridRowData} onRef={props.onCompRefer} paginationPrefixId={localConstant.paginationPrefixIds.techSpecCompetencyGridId}/>

            {(!isEmpty(props.pageMode) && props.pageMode !== localConstant.commonConstants.VIEW) || !props.disableEdit()? /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
            <div className="right-align mt-2">
                <button disabled={props.interactionMode} onClick={props.AddCompetencyDetails} className="waves-effect btn btn-small">{localConstant.commonConstants.ADD}</button>
                <button disabled={(props.gridRowData <= 0) || props.interactionMode } onClick={props.DeleteHAndler}
                    className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.commonConstants.DELETE}</button>
            </div> : null }
        </CardPanel>
    );
};
const CustomerApprovedDetails = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.Taxonomy.CUSTOMER_APPROVED_DETAILS} colSize="s12" >

            <ReactGrid gridColData={props.headerData} gridRowData={props.gridRowData} onRef={props.onCustRefer}  rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(props.draftDataToCompare)?props.draftData : null} paginationPrefixId={localConstant.paginationPrefixIds.techSpecCustomerApprovalGridId} />

            {props.pageMode !== localConstant.commonConstants.VIEW && props.enableEdit() ?
             <div className="right-align mt-2">
                <button disabled={props.interactionMode} onClick={props.AddCustomerApprovalDetails} className="waves-effect btn btn-small">{localConstant.commonConstants.ADD}</button> {/**SMN QA Failed issue Fixes for D687 */}
                <button disabled={(props.gridRowData <= 0 || props.interactionMode)} onClick={props.DeleteHAndler}
                    className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.commonConstants.DELETE}</button>
            </div> : null }
        </CardPanel>
    );
};
const InternalTrainingModalPopup = (props) => {
    
    const internalType = (props.editedRowData &&
        props.editedRowData.technicalSpecialistInternalTrainingTypes &&
        props.editedRowData.technicalSpecialistInternalTrainingTypes[0].typeId);
    let documents=[];
    if(props.editedRowData && props.editedRowData.documents)
    {
        documents=props.editedRowData.documents.filter(x=>x.recordStatus!=='D'); 
    }

    return (
        <Fragment>
            <div className="row ">
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision'
                    label={localConstant.techSpec.Taxonomy.INTERNAL_TRAINING}
                    type='select'
                    colSize="s4 "
                    labelClass="mandate"
                    className="browser-default customInputs"
                    optionsList={props.internalTraining}
                    optionName="name"
                    optionValue="id"
                    name="typeName"
                    onSelectChange={props.inputChangeHandler}
                    defaultValue={internalType}
                    disabled={ internalType !== undefined && props.editedRowData.isILearn} />
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision pl-0'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.DATE_OF_TRAINING}
                    colSize='s4'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    name="trainingDate"
                    selectedDate={props.dateofTraining}
                    onDateChange={props.fetchDateofTraining}
                    onDatePickBlur={props.handleDateBlur} 
                    disabled={ props.editedRowData !== undefined && props.editedRowData.isILearn}/>
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision pl-0'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.EXPIRY}
                    colSize='s4'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    name="expiry"
                    onDatePickBlur={props.handleDateBlur}
                    selectedDate={props.expiryDate}
                    onDateChange={props.fetchExpiryDate} 
                    disabled={ props.editedRowData !== undefined && props.editedRowData.isILearn}/>
            </div>
            <div className="row">
                <div className="col s8">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pl-0 '
                        label={localConstant.techSpec.Taxonomy.SCORE_FIELD}
                        type='text'
                        colSize='s6'
                        inputClass="customInputs"
                        name="score"
                        //maxLength={60}
                        onValueInput={props.inputChangeHandler}
                        defaultValue={props.editedRowData && props.editedRowData.score}
                        disabled={ props.editedRowData !== undefined && props.editedRowData.isILearn}/>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pl-0'
                        label={localConstant.techSpec.Taxonomy.COMPETENCY}
                        type='text'
                        colSize='s6'
                        inputClass="customInputs"
                        name="competency"
                        maxLength={255}
                        onValueChange={props.inputChangeHandler}
                        defaultValue={props.editedRowData && props.editedRowData.competency} 
                        disabled={ props.editedRowData !== undefined && props.editedRowData.isILearn}/>
                    <UploadDocument
                        //Mode={props.isInternalTrainingDetailsEdit}
                        defaultValue={ documents && documents.length>0 ? documents : '' }
                        className="col s12 pl-0 "
                        label={localConstant.techSpec.Taxonomy.TRAING_UPLOAD}
                        cancelUploadedDocument={props.cancelUploadedDocumentData}
                        editDocDetails={props.editedRowData}
                        gridName={localConstant.techSpec.common.TECHNICALSPECIALISTINTERNALTRAINING}
                        disabled={ false }
                        disabled={!isEmpty(props.editedRowData) && props.editedRowData.isILearn === true} //Scenario Fix (Ref 08-04-200 Doc issue9)
                        documentType="TS_InternalTraining"
                        uploadFileRefType="internalTrainingUpload"
                    />
                </div>
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.Taxonomy.NOTES}
                    type='textarea'
                    colSize='s4 '
                    inputClass="customInputs"
                    name="notes"
                    maxLength={4000}
                    onValueChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.notes}
                    readOnly={props.editedRowData !== undefined && props.editedRowData.isILearn}/>
            </div>

            {/* <div className=" pr-4 pl-0"> */}

            {/* </div> */}

        </Fragment>
    );
};
const CompetencyModalPopUp = (props) => {
    let documents=[];
    if(props.competencyeditedRowData && props.competencyeditedRowData.documents)
    {
        documents=props.competencyeditedRowData.documents.filter(x=>x.recordStatus!=='D'); 
    }
    return (
        <Fragment>
            <div className="row ">
            <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision '
                    label={localConstant.techSpec.Taxonomy.DVA_CHARTERS}
                    type='select'
                    valueType="defaultValue"
                    colSize="s12"
                    labelClass="mandate"
                    optionName="name"
                    optionValue="id"
                    optionsList={props.competencyDetails}
                    name="trainingOrCompetencyTypeNames"
                    className="browser-default customInputs"
                    onSelectChange={props.inputChangeHandler}
                  defaultValue={props.competencyeditedRowData && props.competencyeditedRowData.typeId}
                  disabled={ props.competencyeditedRowData !==undefined && props.competencyeditedRowData.isILearn} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.Taxonomy.DURATION}
                    type='text'
                    colSize='s4 '
                    inputClass="customInputs"
                    name="duration"
                    maxLength={50} //Changes for GRM Data Migration issue
                    onValueChange={props.inputChangeHandler}
                    defaultValue={props.competencyeditedRowData && props.competencyeditedRowData.duration} 
                    disabled={ props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn}/>
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.DVA_EFFECTIVE_DATE}
                    colSize='s4 pl-0'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    name="effectiveDate"
                    onDatePickBlur={props.handleDateBlur}
                    selectedDate={props.dvaEffectiveDate}
                    onDateChange={props.fetchDVAEffectiveDate} 
                    disabled={props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn}/>
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.EXPIRY}
                    colSize='s4 pl-0'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    name="expiry"
                    onDatePickBlur={props.handleDateBlur}
                    selectedDate={props.competencyExpiryDate}
                    onDateChange={props.fetchCompetencyExpiryDate} 
                    disabled={ props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn}/>
            </div>
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.techSpec.Taxonomy.SCORE_FIELD}
                type='text'
                colSize='s4 pl-0'
                inputClass="customInputs"
                name="score"
                maxLength={200}
                onValueInput={props.inputChangeHandler}
                defaultValue={props.competencyeditedRowData && props.competencyeditedRowData.score} 
                disabled={ props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn}/>
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.techSpec.Taxonomy.COMPETENCY}
                type='text'
                colSize='s4 pl-0'
                inputClass="customInputs"
                name="competency"
                maxLength={255}
                onValueChange={props.inputChangeHandler}
                defaultValue={props.competencyeditedRowData && props.competencyeditedRowData.competency}
                disabled={ props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn} />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.techSpec.Taxonomy.NOTES}
                type='text'
                colSize='s4 pl-0'
                inputClass="customInputs"
                name="notes"
                maxLength={4000}
                onValueChange={props.inputChangeHandler}
                defaultValue={props.competencyeditedRowData && props.competencyeditedRowData.notes}
                readOnly={ props.competencyeditedRowData !== undefined && props.competencyeditedRowData.isILearn}
            />
            <div className="col s8 pl-0">
                <UploadDocument
                    //Mode={props.isInternalTrainingDetailsEdit}
                    defaultValue={ documents && documents.length>0 ? documents : '' }
                    className="col s12 pl-0 pr-0"
                    label={localConstant.techSpec.Taxonomy.DVA_UPLOAD}
                    cancelUploadedDocument={props.cancelUploadedDocumentData}
                    editDocDetails={props.competencyeditedRowData}
                    gridName={localConstant.techSpec.common.TECHNICALSPECIALISTCOMPETANCY}
                    disabled={ false }
                    disabled={ !isEmpty(props.competencyeditedRowData) && props.competencyeditedRowData.isILearn === true} //Scenario Fix (Ref 08-04-200 Doc issue9)
                    documentType="TS_Competency"
                    uploadFileRefType="competencyUpload"
                />
            </div>

        </Fragment>
    );
};
const CustomerApprovalPopUp = (props) => {
    let work = [];
    let selectedData = [];
    let newDocument = []; 
    let documents=[];
    if(!isEmpty(props.editedRowData)){
        work = props.draftCustomerApproved.filter(x=>x.id === props.editedRowData.id);
    } 
    if(!isEmpty(props.selectedDraftCustomerApproved)){
        selectedData = props.selectedDraftCustomerApproved.filter(x => x.id === props.editedRowData.id);
    }
    if(props.editedRowData && props.editedRowData.documents)
    {
        documents=props.editedRowData.documents.filter(x=>x.recordStatus!=='D'); 
        newDocument  = documents; 
    }
    if(props.uploadedDocument && props.uploadedDocument.length>0) //def 848
    {
        newDocument= props.uploadedDocument.filter(x => (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_CustomerApproval' ) && x.recordStatus!=='D');  
    }
    return (
        <Fragment>
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision'
                    label={localConstant.techSpec.Taxonomy.CUSTOMER_APPROVAL}
                    type='select'
                    colSize="s3"
                    inputClass={"browser-default customInputs " + (props.compareDraftData(props.editedRowData.customerCode,(selectedData.length>0?selectedData[0].customerCode:undefined), (work.length>0?work[0].customerCode:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")} //D556 & 848
                    optionsList={props.taxonomyCustomerApproved}
                    optionName="name"
                    optionValue="code"
                    name="customerName"
                    id="customerCode"
                    onSelectChange={props.inputChangeHandlerCustomer}
                    defaultValue={props.editedRowData && props.editedRowData.customerCode}
                    labelClass="mandate" />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.Taxonomy.SAP_ID}
                    type='text'
                    colSize='s3 '
                    inputClass={"customInputs " + (props.compareDraftData(props.editedRowData.customerSap,(selectedData.length>0?selectedData[0].customerSap:undefined), (work.length>0?work[0].customerSap:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")} 
                    name="customerSap"
                    maxLength={255}
                    onValueChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.customerSap} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision pl-0'
                    label={localConstant.techSpec.Taxonomy.COMMIDITY_CODES}
                    type='select'
                    colSize="s6"
                    inputClass={"browser-default customInputs " + (props.compareDraftData(props.editedRowData.custCodes,(selectedData.length>0?selectedData[0].custCodes:undefined), (work.length>0?work[0].custCodes:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")}//D556
                    optionsList={props.customerApprovedCommodityData}
                    optionSelecetLabel={props.commodityOptionSelecetLabel? props.commodityOptionSelecetLabel:'select'}
                    optionName="commodityName"
                    optionValue="commodityName"
                    name="custCodes"                    
                    onSelectChange={props.inputChangeHandler}
                    defaultValue={props.editedRowData && props.editedRowData.custCodes} />

            </div>
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.EFFECTIVE_FROM}
                    colSize='s3'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    onDatePickBlur={props.handleDateBlur}
                    selectedDate={props.customerApprovedEffectiveFromDate}
                    onDateChange={props.fetchCustomerApprovedEffectiveFromDate}
                    inputClass={"customInputs " + (props.compareDraftData(props.editedRowData.fromDate,(selectedData.length>0?selectedData[0].fromDate:undefined), (work.length>0?work[0].fromDate:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")} //D556
                    name="fromDate" />
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col loadedDivision'
                    type='date'
                    label={localConstant.techSpec.Taxonomy.EXPIRY}
                    colSize='s3'
                    labelClass=" customLabel"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    onDatePickBlur={props.handleDateBlur}
                    selectedDate={props.customerApprovedExpiryDate}
                    onDateChange={props.fetchCustomerApprovedExpiryDate}
                    name="toDate"
                    inputClass={"customInputs " + (props.compareDraftData(props.editedRowData.toDate,(selectedData.length>0?selectedData[0].toDate:undefined), (work.length>0?work[0].toDate:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")} 

                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.Taxonomy.COMMENTS}
                    type='textarea'
                    colSize='s6'
                    name="comments"
                    maxLength={4000}
                    onValueChange={props.inputChangeHandler}
                    inputClass={"customInputs " + (props.compareDraftData(props.editedRowData.comments,(selectedData.length>0?selectedData[0].comments:undefined), (work.length>0?work[0].comments:undefined),props.editedRowData.recordStatus)?"inputHighlight":"")} 
                    defaultValue={props.editedRowData && props.editedRowData.comments} />
                
                <UploadDocument
                    //Mode={props.isInternalTrainingDetailsEdit}
                    defaultValue={ documents && documents.length>0 ? documents : '' }
                    className="col s8 "
                    label={localConstant.techSpec.Taxonomy.ATTACH_DOCUMENT}
                    cancelUploadedDocument={props.cancelUploadedDocumentData}
                    editDocDetails={props.editedRowData}
                    gridName={localConstant.techSpec.common.TECHNICALSPECIALISTCUSTOMERAPPROVAL}
                    disabled={ false }
                    documentType="TS_CustomerApproval"
                    uploadFileRefType="customerUpload"
                    inputClass={(props.compareDraftData((newDocument.length>0 ? newDocument[0].documentUniqueName : ''), (selectedData.length > 0 && isArray( selectedData[0].documents) && selectedData[0].documents.length>0 ? selectedData[0].documents[0].documentUniqueName  : " "),(work.length > 0 && isArray(work[0].documents) && work[0].documents.length>0 ? work[0].documents[0].documentUniqueName : " "), (newDocument.length>0 ?(newDocument[0].recordStatus==null? undefined : newDocument[0].recordStatus) : undefined)) ? " customInputs inputHighlight" : "")}//def848
                />
            </div>
        </Fragment>
    );
};
class Taxonomy extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            notes: "",
            competency: "",
            scoreField: "",
            expiryDate: '',
            dateofTraining: '',
            competencyExpiryDate: '',
            dvaEffectiveDate: '',
            customerApprovedEffectiveFromDate: '',
            customerApprovedExpiryDate: '',
            inValidDateFormat: false,
            internalTrainingShowModal: false,
            isInternalTrainingDetailsEdit: false,
            competencyShowModal: false,
            isCompetencyDetailsEdit: false,
            customerApprovedShowModal: false,
            isCustomerApprovedDetailsEdit: false,
            oldProfileActionType: this.props.oldProfileActionType,
            SelectedCustomerName: "",
            isRCRMUpdate:false,
        };
        this.updatedData = {};
        this.competencyDocument={};
        this.customerApporvalDocument={};
        this.internalDocument={};
        this.loggedUserType =userTypeGet();
        const functionRefs = {}; 
        functionRefs["enableEditColumn"] = this.enableEdit;
        functionRefs["disableEditColumn"] = this.disableEdit;
        this.headerData = HeaderData(functionRefs);

        const isEditEnbled=this.enableEdit();
        const isEditDisable=this.disableEdit();

        bindProperty(this.headerData.InternalTrainingHeader, "EditColumn", "hide",isEditDisable);
        bindProperty(this.headerData.CompetencyDetailHeader, "EditColumn", "hide",isEditDisable);
        bindProperty(this.headerData.CustomerApprovedDetailHeader, "EditColumn", "hide",!isEditEnbled);

        if(!isEditDisable)
        {
            bindAction(this.headerData.InternalTrainingHeader, "EditColumn", this.internalTrainingEditRowHandler);
            bindAction(this.headerData.CompetencyDetailHeader, "EditColumn", this.competencyEditRowHandler);
        }

        if(isEditEnbled)      
        {
            bindAction(this.headerData.CustomerApprovedDetailHeader, "EditColumn", this.customerApprovedEditRowHandler);
        }
         
        this.internalTrainingAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideInternalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.CreateInternalTrainingDetails,
                btnClass: "btn-small",
                showbtn: !isEditDisable
            }
        ];
        this.internalTrainingEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideInternalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateInternalTrainingDetails,
                btnClass: "btn-small",
                showbtn: !isEditDisable
            }
        ];
        this.competencyDetailsAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideCompetencyModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.CreateCompetencyDetails,
                btnClass: "btn-small",
                showbtn: !isEditDisable
            }
        ];
        this.competencyDetailsEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideCompetencyModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: (e) => this.updateCompetencyDetails(e),
                btnClass: "btn-small",
                showbtn: !isEditDisable
            }
        ];
        this.customerApprovedDetailsAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideCustomerApporvalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.CreateCustomerApprovedDetails,
                btnClass: "btn-small",
                showbtn: isEditEnbled,

            }
        ];
        this.customerApprovedDetailsEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideCustomerApporvalModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateCustomerApprovedDetails,
                btnClass: "btn-small",
                showbtn: isEditEnbled,
            }
        ];
        this.updateFromRC_RMCustomerApproved=[];
    }

    enableEdit = () => { /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        // if((this.props.techManager || this.loggedUserType.includes(localConstant.techSpec.userTypes.TM) || this.loggedUserType.includes(localConstant.techSpec.userTypes.TS)))
        // {
        //     return (isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesTM }) && !this.props.interactionMode );
        // } 
        // return false; 
        if(this.props.isTMUserTypeCheck){
            if(this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
                return false;
            }
            else if (!isViewable({ activities: this.props.activities,levelType: 'LEVEL_3', viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
                return false;
            }//Added for D1374
        }
        return  isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });//D1374 Failed on 29-10-2020
    }
    disableEdit = () => { /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
       //return((this.loggedUserType.includes(localConstant.techSpec.userTypes.RM) || this.loggedUserType.includes(localConstant.techSpec.userTypes.RC) || this.loggedUserType.includes(localConstant.techSpec.userTypes.TS)) && !(this.props.techManager || this.loggedUserType.includes(localConstant.techSpec.userTypes.TM)) );
       if(this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
            return true;
      }
      else if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
        return true;
        }
           return !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });//D1374 Failed on 29-10-2020
    }

    fetchExpiryDate = (date) => {
        this.setState({ expiryDate: date }, () => {
            this.updatedData.expiry = this.state.expiryDate;
        });
    }
    fetchDateofTraining = (date) => {
        this.setState({ dateofTraining: date }, () => {
            this.updatedData.effectiveDate = this.state.dateofTraining;
        });
    }
    fetchCompetencyExpiryDate = (date) => {
        this.setState({ competencyExpiryDate: date }, () => {
            this.updatedData.expiry = this.state.competencyExpiryDate;
        });
    }
    fetchDVAEffectiveDate = (date) => {
        this.setState({ dvaEffectiveDate: date }, () => {
            this.updatedData.effectiveDate = this.state.dvaEffectiveDate;
        });
    }
    fetchCustomerApprovedExpiryDate = (date) => {
        this.setState({ customerApprovedExpiryDate: date }, () => {
            this.updatedData.toDate = this.state.customerApprovedExpiryDate;
        });
    }
    fetchCustomerApprovedEffectiveFromDate = (date) => {
        this.setState({ customerApprovedEffectiveFromDate: date }, () => {
            this.updatedData.fromDate = this.state.customerApprovedEffectiveFromDate;
        });
    }

    handleDateBlur = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isUIValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    } else
                        this.setState({ inValidDateFormat: false });
                }
            }
        }
    }
    //Internal Training Date Range Validator
    InternalTrainingDateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.TRAINING_INVALID_DATE_RANGE, 'warningToast');
            } else if(from == to){//D687 (As per mail confirmation on 16-03-2020)
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.TRAINING_INVALID_DATE_EQUAL_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    //Competency Details Date Range Validator
    CompetencyDateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.COMPETENCY_INVALID_DATE_RANGE, 'warningToast');
            } else if(from == to){//D687 (As per mail confirmation on 16-03-2020)
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.COMPETENCY_INVALID_DATE_EQUAL_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    //Validation for Internal Training Details
    internalTrainingDetailsValidation = (data) => {
        if (isUndefined(data.technicalSpecialistInternalTrainingTypes)) {
            if (data.typeName === null || data.typeName === undefined || data.typeName === "") {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Internal Training', 'warningToast');
                return false;
            }
        } else if (isEmpty(data.technicalSpecialistInternalTrainingTypes)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Internal Training', 'warningToast');
            return false;
        }
        return true;
    }
    //Validation for Competency Details
    competencyDetailsValidation = (data) => {
        if (isEmpty(data.trainingOrCompetencyTypeNames) || data.trainingOrCompetencyTypeNames === undefined || data.trainingOrCompetencyTypeNames === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' DVA Charters', 'warningToast');
            return false;
        }
        return true;
    }
    //validation for Customer Approved details
    customerApprovedDetailsValidation = (data) => {
        if (data.customerName === undefined || data.customerName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Customer Approval', 'warningToast');
            return false;
        }
        return true;
    }
    //Number Field Validation
    numberValidation = (e) => {
        if (e.target.type === "number") {
            if (e.target.value !== "0") {
                e.target.value = mobileNumberFormat(e.target.value);
            }
        }
        const fieldValue = e.target[e.target.value];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    getCommodity = (selectedName) => {
        if (selectedName === 'customerName') {
            this.props.actions.ClearServices();
            const customerName = this.updatedData[selectedName];
            this.props.actions.FetchTaxonomyCustomerApprovedCommodity(customerName);
        }
    }
    getCommodityOnEdit = (selectedCode) => {
        this.props.actions.ClearServices();
        this.props.actions.FetchTaxonomyCustomerApprovedCommodity(selectedCode);

    }
    inputChangeHandler = (e) => {
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.setState({ isRCRMUpdate: true });
        }
        const result = this.numberValidation(e);
        this.updatedData[result.name] = result.value;
        this.updatedData[e.target.name] = e.target.value;
        if(e.target.name == 'trainingOrCompetencyTypeNames'){
            this.updatedData["typeId"] =  e.target.value;
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        } else if(e.target.name == 'typeName'){
            this.updatedData["typeId"] =  e.target.value;
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        }
    }
    inputChangeHandlerCustomer = (e) => {
        const inputvalue = formInputChangeHandler(e);
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.setState({ isRCRMUpdate: true });
        } 
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (!isEmpty(inputvalue.value)) {
            //To-Do-suresh: Need to change the approach getting Selected dropdown option.
            if (inputvalue.name === "customerName") {
                this.getCommodity(inputvalue.name);
                this.setState({
                    SelectedCustomerName: e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text
                });
                // const selectedOption = e.target.selectedOptions[0];
                // this.updatedData[e.target.name] = selectedOption.text;
                // this.updatedData[e.target.id] = selectedOption.value;
                this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                this.updatedData[e.target.id] = e.nativeEvent.target.value;
            }

        }
        else {
            if (inputvalue.name === "customerName") {
                this.setState({
                    SelectedCustomerName: ""
                });
            }
        }
    }

    //Adding Stamp details to the grid
    CreateInternalTrainingDetails = (e) => {
        e.preventDefault();
        if (this.state.lessReturnDate) {
            IntertekToaster(localConstant.techSpec.common.INVALID_DATE_RANGE, 'warningToast invalidStartDate12');
            return false;
        }
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.internalTrainingDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.dateofTraining !== "" && this.state.dateofTraining !== null)
                    fromDate = this.state.dateofTraining.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.expiryDate !== "" && this.state.expiryDate !== null)
                    toDate = this.state.expiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }

                if (!this.InternalTrainingDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    // this.updatedData={
                    //     technicalSpecialistInternalTrainingTypes:[]
                    // };
                    const technicalSpecialistInternalTrainingTypes = [];
                    const newInternalTraning = [];
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["recordType"] = "IT";
                    this.updatedData["trainingDate"] = fromDate;
                    this.updatedData["expiry"] = toDate;
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus=== "N" && ( x.uploadFileRefType ==='internalTrainingUpload' || x.documentType === 'TS_InternalTraining'));//sanity def 98 fix
                    technicalSpecialistInternalTrainingTypes.push(
                        {
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            epin: this.props.epin !== undefined ? this.props.epin : 0,
                            technicalSpecialistTrainingId: this.updatedData.id,
                            typeName: this.updatedData.typeName,
                            recordStatus: this.updatedData.recordStatus,
                            typeId:this.updatedData.typeId,
                        });
                    this.updatedData["technicalSpecialistInternalTrainingTypes"] = technicalSpecialistInternalTrainingTypes;
                    newInternalTraning.push(this.updatedData);
                    this.props.actions.AddInternalTrainingDetails(newInternalTraning);
                    this.props.actions.UploadDocumentDetails();
                    this.setState({
                        internalTrainingShowModal: false,
                        isInternalTrainingDetailsEdit: true
                    });
                    this.updatedData = {};
                }
            }
        }
        else this.internalTrainingDetailsValidation(this.updatedData);
    }

    CreateCompetencyDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.competencyDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.dvaEffectiveDate !== "" && this.state.dvaEffectiveDate !== null)
                    fromDate = this.state.dvaEffectiveDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.competencyExpiryDate !== "" && this.state.competencyExpiryDate !== null)
                    toDate = this.state.competencyExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);

                if (!this.CompetencyDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["effectiveDate"] = fromDate;
                    this.updatedData["expiry"] = toDate;
                    this.updatedData["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                    this.updatedData["modifiedBy"] = this.props.loggedInUser;
                    this.updatedData["recordType"] = "Co";
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'competencyUpload' || x.documentType === 'TS_Competency'));//sanity def 98 fix
                    this.props.actions.AddCompetencyDetails(this.updatedData);
                    this.props.actions.UploadDocumentDetails();
                    this.setState({
                        competencyShowModal: false,
                        isCompetencyDetailsEdit: true
                    });
                    this.updatedData = {};
                }
            }
        }
        else {
            this.competencyDetailsValidation(this.updatedData);
        }
    }
    //Date Range Validator
    CustomerApprovedDateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.CUST_APPROVED_DATE_RANGE_VALIDATOR, 'warningToast');
            } else if (from == to){//D687 (As per mail confirmation on 16-03-2020)
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.CUST_APPROVED_DATE_RANGE_EQUAL_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    CreateCustomerApprovedDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.customerApprovedDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.customerApprovedEffectiveFromDate !== "" && this.state.customerApprovedEffectiveFromDate !== null)
                    fromDate = this.state.customerApprovedEffectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.customerApprovedExpiryDate !== "" && this.state.customerApprovedExpiryDate !== null)
                    toDate = this.state.customerApprovedExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }
                if (!this.CustomerApprovedDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                    this.updatedData["fromDate"] = fromDate;
                    this.updatedData["toDate"] = toDate;
                    //this.updatedData["customerCode"]="AB00009";
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x =>x.recordStatus=== "N" && ( x.uploadFileRefType ==='customerUpload' || x.documentType ==='TS_CustomerApproval'));//sanity def 98 fix
                    this.updateFromRC_RMCustomerApproved.push(this.updatedData);
                    this.props.actions.AddCustomerApprovalDetails(this.updatedData);
                    this.props.actions.UploadDocumentDetails();
                    this.setState({
                        customerApprovedShowModal: false,
                        isCustomerApprovedDetailsEdit: true,
                        SelectedCustomerName: ""  //Added for D553 issue 1
                    });
                    this.updatedData = {};
                }
            }
        }
        else {
            this.customerApprovedDetailsValidation(this.updatedData);
        }

    }

    AddInternalTrainDetails = (e) => {
        e.preventDefault();
        this.setState({
            internalTrainingShowModal: true,
            isInternalTrainingDetailsEdit: false,
            expiryDate: '',
            dateofTraining: '',
        });
        this.updatedData = {};
    };
    AddCompetencyDetails = (e) => {
        e.preventDefault();
        this.setState({
            competencyShowModal: true,
            isCompetencyDetailsEdit: false,
            competencyExpiryDate: '',
            dvaEffectiveDate: '',
        });

    }
    AddCustomerApprovalDetails = (e) => {
        e.preventDefault();
        this.setState({
            customerApprovedShowModal: true,
            isCustomerApprovedDetailsEdit: false,
            customerApprovedEffectiveFromDate: '',
            customerApprovedExpiryDate: '',
        });
        this.updatedData = {};
        this.editedRowData = {};

    }
    hideInternalModal = (e) => {
        e.preventDefault();
        this.setState({
            internalTrainingShowModal: false,
            isInternalTrainingDetailsEdit: true,
        });
        if(!isEmptyOrUndefine(this.internalDocument)){
            this.updatedData=deepCopy(this.editedRowData);
            this.internalDocument.recordStatus='M';  // Reverting the deleted doc to previous state
            this.updatedData["documents"]=this.internalDocument;
            this.props.actions.UpdateInternalTrainingdetails(this.editedRowData,this.updatedData);
        }
        this.updatedData = {};
        this.editedRowData = {};
        this.internalDocument={};
        this.props.actions.RemoveDocUniqueCode();
    }
    hideCompetencyModal = (e) => {
        e.preventDefault();
        this.setState({
            competencyShowModal: false,
            isCompetencyDetailsEdit: true,
        });
        if(!isEmptyOrUndefine(this.competencyDocument)){
            this.updatedData=deepCopy(this.editedRowData);
            this.competencyDocument.recordStatus='M';  // Reverting the deleted doc to previous state
            this.updatedData["documents"]=this.competencyDocument;
            this.props.actions.UpdateCompetencydetails(this.editedRowData,this.updatedData);
        }
        this.updatedData = {};
        this.editedRowData = {};
        this.competencyDocument={};
        this.props.actions.RemoveDocUniqueCode();
    }
    hideCustomerApporvalModal = (e) => {
        e.preventDefault();
        this.setState({
            customerApprovedShowModal: false,
            isCustomerApprovedDetailsEdit: true,
            SelectedCustomerName: ""
        });
        if(!isEmptyOrUndefine(this.customerApporvalDocument)){
            this.updatedData=deepCopy(this.editedRowData);
            this.customerApporvalDocument.recordStatus= null; //def 848 doc highlight  // Reverting the deleted doc to previous state
            this.updatedData["documents"]=this.customerApporvalDocument;
            this.props.actions.UpdateCustomerApprovedDetails(this.updatedData,this.editedRowData);
        }
        this.updatedData = {};
        this.editedRowData = {};
        this.customerApporvalDocument={};
        this.props.actions.RemoveDocUniqueCode();
    }
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();
        this.gridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteInternalTrainingDetails(selectedData);
        this.props.actions.HideModal();
    }
    deleteCompetencySelected = () => {
        const selectedData = this.gridChildren.getSelectedRows();
        this.gridChildren.removeSelectedRows(selectedData);
        this.props.actions.DeleteCompetencyDetails(selectedData);
        this.props.actions.HideModal();
    }
    deleteCustomerApprovedSelected = () => {
        const selectedData = this.gridCh.getSelectedRows();
        this.gridCh.removeSelectedRows(selectedData);
        this.props.actions.DeleteCustomerApprovedDetails(selectedData);
        this.props.actions.HideModal();
    }
    DeleteInternalTrainingDetailsHandler = () => {
        const selectedRecords = this.gridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.INTERNAL_TRAINING_DETAIL,
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
        else
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    DeleteCompetencyDetailsHandler = () => {
        const selectedRecords = this.gridChildren.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COMPETENCY_DETAIL,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteCompetencySelected,
                        className: " m-1 btn-small"
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
        else {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
            return false;
        }
    }
    DeleteCustomerApprovedDetailsHandler = () => {
        const selectedRecords = this.gridCh.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CUSTOMER_APPROVED_DETAIL,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteCustomerApprovedSelected,
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
        else {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
            return false;
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    updateInternalTrainingDetails = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.internalTrainingDetailsValidation(combinedData)) {
            let fromDate = "";
            let toDate = "";
            if (this.state.dateofTraining !== "" && this.state.dateofTraining !== null)
                fromDate = this.state.dateofTraining.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.expiryDate !== "" && this.state.expiryDate !== null)
                toDate = this.state.expiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }

            if (!this.InternalTrainingDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
                if (!isEmpty(this.editedRowData.technicalSpecialistInternalTrainingTypes)) {
                    if (this.updatedData.typeName !== undefined) {
                        this.editedRowData.technicalSpecialistInternalTrainingTypes[0].typeName = this.updatedData.typeName;
                        this.editedRowData.technicalSpecialistInternalTrainingTypes[0].typeId = this.updatedData.typeId;
                    }
                    this.editedRowData.technicalSpecialistInternalTrainingTypes[0].recordStatus = 'M';
                } else if (isEmpty(this.editedRowData.technicalSpecialistInternalTrainingTypes)) {
                    const internalTrainingTypes = [];
                    internalTrainingTypes.push(
                        {
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            epin: this.props.epin !== undefined ? this.props.epin : 0,
                            technicalSpecialistTrainingId: this.editedRowData.id,
                            typeName: this.updatedData.typeName,
                            recordStatus: 'N',
                            typeId: this.updatedData.typeId,
                        });
                    this.updatedData["technicalSpecialistInternalTrainingTypes"] = internalTrainingTypes;
                }
                this.updatedData["trainingDate"] = fromDate;
                this.updatedData["expiryDate"] = toDate;
                this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'internalTrainingUpload' || x.documentType === 'TS_InternalTraining'));//sanity def 98 fix
                if (this.updatedData.typeName !== undefined) {
                    if (!isEmpty(this.updatedData.technicalSpecialistInternalTrainingTypes)){
                        this.updatedData.technicalSpecialistInternalTrainingTypes[0].typeName = this.updatedData.typeName;
                        this.updatedData.technicalSpecialistInternalTrainingTypes[0].typeId = this.updatedData.typeId;
                    }
                }
                this.props.actions.UpdateInternalTrainingdetails(this.editedRowData, this.updatedData);
                this.props.actions.UploadDocumentDetails();
                this.setState({ internalTrainingShowModal: false });
                this.editedRowData = {};
                this.updatedData = {};
                this.internalDocument={};
            }
        }
    }

    updateCompetencyDetails = (e) => {
        e.preventDefault();
        let fromDate = "";
        let toDate = "";
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.competencyDetailsValidation(combinedData)) {
            if (this.state.dvaEffectiveDate !== "" && this.state.dvaEffectiveDate !== null)
                fromDate = this.state.dvaEffectiveDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.competencyExpiryDate !== "" && this.state.competencyExpiryDate !== null)
                toDate = this.state.competencyExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }

            //let selectedItem = '';
            if (!this.CompetencyDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                // if (!isEmpty(this.updatedData.trainingOrCompetencyTypeNames)) {
                //     selectedItem = combinedData.trainingOrCompetencyTypeNames;
                // }
                // else {
                //     selectedItem = mapArrayObject(combinedData.trainingOrCompetencyTypeNames, 'value').join(',');
                // }
                //Competency TyepeID Update Start
                if (!isEmpty(this.editedRowData.technicalSpecialistCompetencyTypes)) {
                    if (this.updatedData.trainingOrCompetencyTypeNames !== undefined) {
                        this.editedRowData.technicalSpecialistCompetencyTypes[0].typeName = this.updatedData.trainingOrCompetencyTypeNames;
                        this.editedRowData.technicalSpecialistCompetencyTypes[0].typeId = this.updatedData.typeId;
                    }
                    if(this.editedRowData.technicalSpecialistCompetencyTypes[0].recordStatus !== "N")
                       this.editedRowData.technicalSpecialistCompetencyTypes[0].recordStatus = 'M';
                } 
                //Competency TypeId update End
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["effectiveDate"] = fromDate;
                this.updatedData["expiry"] = toDate;//Commented for Scenario Fixes #127 (REf mail 30-03-2020)
                this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'competencyUpload' || x.documentType === 'TS_Competency'));//sanity def 98 fix
                this.updatedData["trainingOrCompetencyTypeNames"] = combinedData.trainingOrCompetencyTypeNames;
                this.updatedData["typeId"] = combinedData.typeId;
                this.props.actions.UpdateCompetencydetails(this.editedRowData, this.updatedData);
                this.props.actions.UploadDocumentDetails();
                this.editedRowData = {};
                this.updatedData = {};
                this.competencyDocument={};
                this.setState({ competencyShowModal: false });

            }
        }
        else {
            this.setState({ competencyShowModal: true });
        }
    }
    updateCustomerApprovedDetails = (e) => { 
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.customerApprovedDetailsValidation(combinedData)) {
            let fromDate = "";
            let toDate = "";
            if (this.state.customerApprovedEffectiveFromDate !== "" && this.state.customerApprovedEffectiveFromDate !== null)
                fromDate = this.state.customerApprovedEffectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.customerApprovedExpiryDate !== "" && this.state.customerApprovedExpiryDate !== null)
                toDate = this.state.customerApprovedExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }

            if (!this.CustomerApprovedDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["fromDate"] = fromDate;
                this.updatedData["toDate"] = toDate;
                this.updatedData["documents"] = this.props.uploadDocument;//filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'customerUpload' || x.documentType === 'TS_CustomerApproval'));//sanity def 98 fix //Changes for Live D656
                const editedRow = mergeobjects(this.updatedData);
                if (this.updatedData && !isEmpty(this.updatedData)) {
                    this.props.actions.UpdateCustomerApprovedDetails(editedRow, this.editedRowData);
                    this.props.actions.UploadDocumentDetails();
                    this.setState({
                        customerApprovedShowModal: false,
                        SelectedCustomerName: ""
                    });
                    this.editedRowData = {};
                    this.updatedData = {};
                    this.customerApporvalDocument={};
                }
            }
        }

    }

    internalTrainingEditRowHandler = (data) => {
        this.setState((state) => {
            return {
                internalTrainingShowModal: !state.internalTrainingShowModal,
                isInternalTrainingDetailsEdit: true,
            };
        });
        this.editedRowData = data;
        this.internalTrainingDateHandler();

    }
    competencyEditRowHandler = (data) => {
        this.setState((state) => {
            return {
                competencyShowModal: !state.competencyShowModal,
                isCompetencyDetailsEdit: true,

            };
        });
        this.editedRowData = data;

        // const { technicalSpecialistCompetencyTypes } = data;
        // if (Array.isArray(technicalSpecialistCompetencyTypes) &&
        //     technicalSpecialistCompetencyTypes.length > 0) {
        //     this.editedRowData.trainingOrCompetencyTypeNames = technicalSpecialistCompetencyTypes.filter(ts => ts.typeName !== '').map(e => e.typeName).join(",");
        // } else {
        //     this.editedRowData.trainingOrCompetencyTypeNames = "";
        // }

        this.competencyDateHandler();

        //this.editMultiSelectedValue(data);
    }
    customerApprovedEditRowHandler = (data) => { 
        this.getCommodityOnEdit(data.customerCode);
        this.setState((state) => {
            return {
                customerApprovedShowModal: !state.customerApprovedShowModal,
                isCustomerApprovedDetailsEdit: true,
                SelectedCustomerName: data.customerName

            };
        });

        this.editedRowData = data;
        this.customerApprovedDateHandler();

    }
    getMultiSelectdValue = (data) => {
        const selectedItem = mapArrayObject(data, 'value').join(',');
        this.updatedData.trainingOrCompetencyTypeNames = selectedItem;
    }
    editMultiSelectedValue = (data) => {
        let dvaChartersData = "";
        dvaChartersData = addElementToArray(this.props.taxonomyCompetency);
        const dvaCharterInt = [];
        if (isArray(data.trainingOrCompetencyTypeNames)) {
            const selectedItem = mapArrayObject(data.trainingOrCompetencyTypeNames, 'value').join(',');
            const answerString = selectedItem;
            answerString.split(',').forEach(function (item) {
                dvaCharterInt.push({ value: item });
            });
        } else {
            const dvaCharterString = data.trainingOrCompetencyTypeNames;
            if (!isEmpty(dvaCharterString)) {
                dvaCharterString.split(',').forEach(function (item) {
                    dvaCharterInt.push({ value: item });
                });
            }
        }
        this.editedRowData.trainingOrCompetencyTypeNames = [];
        dvaCharterInt.map(item => {
            return this.editedRowData.trainingOrCompetencyTypeNames.push(findObject(dvaChartersData, item));
        });
        this.setState({ competencyShowModal: true });
    }
    internalTrainingDateHandler = () => {
        if (this.editedRowData) {
            if ((this.editedRowData.expiry !== null && this.editedRowData.expiry !== "")) {
                this.setState({ expiryDate: moment(this.editedRowData.expiry) });
            }
            else {
                this.setState({ expiryDate: "" });
            }
            if ((this.editedRowData.trainingDate !== null && this.editedRowData.trainingDate !== "")) {
                this.setState({ dateofTraining: moment(this.editedRowData.trainingDate) });
            }
            else {
                this.setState({ dateofTraining: "" });
            }
        }
        else {
            this.setState({
                dateofTraining: "",
                expiryDate: "",
            });
        }
    }
    competencyDateHandler = () => {
        if (this.editedRowData) {
            if ((this.editedRowData.expiry !== null && this.editedRowData.expiry !== "")) {
                this.setState({ competencyExpiryDate: moment(this.editedRowData.expiry) });
            }
            else {
                this.setState({ competencyExpiryDate: "" });
            }
            if ((this.editedRowData.effectiveDate !== null && this.editedRowData.effectiveDate !== "")) {
                this.setState({ dvaEffectiveDate: moment(this.editedRowData.effectiveDate) });
            }
            else {
                this.setState({ dvaEffectiveDate: "" });
            }
        }
        else {
            this.setState({
                dvaEffectiveDate: "",
                competencyExpiryDate: "",
            });
        }
    }
    customerApprovedDateHandler = () => {
        if (this.editedRowData) {
            if ((this.editedRowData.toDate !== null && this.editedRowData.toDate !== "")) {
                this.setState({ customerApprovedExpiryDate: moment(this.editedRowData.toDate) });
            }
            else {
                this.setState({ customerApprovedExpiryDate: "" });
            }
            if ((this.editedRowData.fromDate !== null && this.editedRowData.fromDate !== "")) {
                this.setState({ customerApprovedEffectiveFromDate: moment(this.editedRowData.fromDate) });
            }
            else {
                this.setState({ customerApprovedEffectiveFromDate: "" });
            }
        }
        else {
            this.setState({
                customerApprovedEffectiveFromDate: "",
                customerApprovedExpiryDate: "",
            });
        }
    }
    /**
     * Handling document cancel
     */
    cancelcompetencyUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.competencyData);
                this.competencyDocument=filterdData[0];
            }
        }    
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
            if (newDoc.length > 0) {
                this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // e.preventDefault();
        // const req = this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.competencyData);
    }
    cancelTrainingUploadedDocument = (e,documentUniqueName) => {
         // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.internalTrainingData);
                this.internalDocument=filterdData[0];
            }
        }    
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
                if (newDoc.length > 0) {
                    this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // e.preventDefault();
        // const req = this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.internalTrainingData);
    }
    cancelcustomerUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.customerApprovedData);
                this.customerApporvalDocument=filterdData[0];
            }
        }    
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
            if (newDoc.length > 0) {
                this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // e.preventDefault();
        // const req = this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.customerApprovedData);
    }
    //internal training, competancy, customer approved
    /** internal Traning history draft field comparision */
    internalTrainingDraftData = (originalData) => {
        if (originalData && originalData.data) {
            const training = this.props.draftInternalTraining.filter(x => x.id == originalData.data.id);
            if (training.length > 0) {
                const result = compareObjects(this.excludeProperty(training[0]), this.excludeProperty(originalData.data));
                return !result;
            }
            return true;
        }
        return false;
    }

    /** competancy draft field comparision */
    competancyDraftData = (originalData) => {
        if (originalData && originalData.data) {
            const competancy = this.props.draftCompetency.filter(x => x.id == originalData.data.id);
            if (competancy.length > 0) {
                const result = compareObjects(this.excludeProperty(competancy[0]), this.excludeProperty(originalData.data));
                return !result;
            }
            return true;
        }
        return false;
    }

    /** customer Approved draft field comparision */
    // customerApprovedDraftData = (originalData) => {
    //     if (originalData && originalData.data) {
    //         const approved = this.props.draftCustomerApproved.filter(x => x.id == originalData.data.id);
    //         if (approved.length > 0) {
    //             const result = compareObjects(this.excludeProperty(approved[0]), this.excludeProperty(originalData.data));
    //             return !result;
    //         }
    //         return true;
    //     }
    //     return false;
    // }

    customerApprovedDraftData = (originalData) => {
        let work = 0;
        if (originalData && originalData.data && originalData.data.customerCode && isEmptyOrUndefine(originalData.data.recordStatus)) {  
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                work = this.props.draftCustomerApproved.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result ) { //def 848 #1
                        return true;   //D556 
                    }
                    return false;
                } else if (!isEmpty(this.updateFromRC_RMCustomerApproved)) {
                    work = this.updateFromRC_RMCustomerApproved.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const currentResult = compareObjects(work[0], originalData.data);
                        return !currentResult;
                    }
                    else if (this.state.isRCRMUpdate) {
                        work = this.props.selectedDraftCustomerApproved.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                            return compareDataResult;
                        }
                    }
                } else if (!isEmpty(this.props.selectedDraftCustomerApproved)) {
                    work = this.props.selectedDraftCustomerApproved.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        return compareDataResult;
                    }
                } else {
                    return false;
                }
            } else {
                this.updateFromRC_RMCustomerApproved = [];
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
compareDraftData = (fieldValue,selectedDraftValue,draftValue,recordStatus) => {
    const _this = this;
    if ((recordStatus !== null && recordStatus !== undefined ) || fieldValue==undefined) { //def 848 #1
        return false;
    }
    else if(!isEmpty(_this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )){ //def 848 #1
        if(!_this.state.isRCRMUpdate){
            if(draftValue === undefined && !required(fieldValue)){
                return true;
            }
            else if(draftValue !== undefined && draftValue !== fieldValue && draftValue !== null){//D556
                return true;
            }
            else if ( (draftValue === undefined || draftValue === null) && !required(fieldValue) && draftValue !== fieldValue) { //def 848 #1
                return true;
            }
            return false;   
        }else if(_this.state.isRCRMUpdate){
            //RC RM While modifing Value Not showing Highlight
            if(draftValue === undefined && !required(fieldValue)){
                return false;
            }
            else if(draftValue !== undefined && draftValue !== fieldValue){
                   if(selectedDraftValue === fieldValue){
                     return true;
                   }
                return false;
            }
            return false; 
        }                    
    }
    return false;
    };
    render() {
        const {
            approvedTaxonomy,
            internalTrainingData,
            competencyData,
            customerApprovedData,
            taxonomyInternalTraining,
            taxonomyCompetency,
            taxonomyCustomerApproved,
            taxonomyCustomerCommodity,
            interactionMode,
            draftDataToCompare,
            selectedDraftCustomerApproved,
            uploadDocument//def848
        } = this.props;
        const competencyGridData = [];
        const updatedTaxonomyCompetency = [];
        if(!isEmpty(taxonomyCompetency)) {
            taxonomyCompetency.forEach( item => {
                if(item.name !== "eReporting")
                  {
                    updatedTaxonomyCompetency.push(item);
                  }
            });
        }
        this.props.competencyData.map(item => {
            let selectedItem = '';
            let selectedId = '';
            if (!isEmpty(item.technicalSpecialistCompetencyTypes)) {
                selectedItem = item.technicalSpecialistCompetencyTypes.filter(ts => ts.recordStatus !== 'D').map(e => e.typeName);
                selectedId = item.technicalSpecialistCompetencyTypes.filter(ts => ts.recordStatus !== 'D').map(e => e.typeId);
                item['trainingOrCompetencyTypeNames'] = selectedItem[0];
                item['name'] = selectedItem[0];
                item['typeId']=selectedId[0];
            }
            competencyGridData.push(item);
        });

        // const { technicalSpecialistCompetencyTypes } = data;
        // if (Array.isArray(technicalSpecialistCompetencyTypes) &&
        //     technicalSpecialistCompetencyTypes.length > 0) {
        //     this.editedRowData.trainingOrCompetencyTypeNames = technicalSpecialistCompetencyTypes.filter(ts => ts.recordStatus !== 'D').map(e => e.typeName).join(",");
        // } else {
        //     this.editedRowData.trainingOrCompetencyTypeNames = "";
        // }

        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.state.internalTrainingShowModal &&
                    <Modal title={this.state.isInternalTrainingDetailsEdit ? localConstant.techSpec.Taxonomy.EDIT_INTERNAL_TRAINING :
                        localConstant.techSpec.Taxonomy.ADD_INTERNAL_TRAINING} modalClass="techSpecModal"
                        buttons={this.state.isInternalTrainingDetailsEdit ? this.internalTrainingEditButtons : this.internalTrainingAddButtons}
                        isShowModal={this.state.internalTrainingShowModal}
                    >
                        < InternalTrainingModalPopup
                            supportFileType={configuration}
                            internalTraining={taxonomyInternalTraining && arrayUtil.sort(taxonomyInternalTraining, 'name', 'asc')}
                            inputChangeHandler={this.inputChangeHandler}
                            uploadDocHandler={this.uploadDocumentHandler}
                            fetchDateofTraining={this.fetchDateofTraining}
                            fetchExpiryDate={this.fetchExpiryDate}
                            expiryDate={this.state.expiryDate}
                            dateofTraining={this.state.dateofTraining}
                            handleDateBlur={this.handleDateBlur}
                            isUploadDocumentEdit={this.props.isUploadDocumentEdit}
                            editedRowData={this.editedRowData}
                            isInternalTrainingDetailsEdit={this.state.isInternalTrainingDetailsEdit}
                            cancelUploadedDocumentData={this.cancelTrainingUploadedDocument} />
                    </Modal>}
                {this.state.competencyShowModal &&
                    <Modal title={this.state.isCompetencyDetailsEdit ? localConstant.techSpec.Taxonomy.EDIT_COMPETENCY_DETAILS :
                        localConstant.techSpec.Taxonomy.ADD_COMPETENCY_DETAILS} modalClass="techSpecModal"
                        buttons={this.state.isCompetencyDetailsEdit ? this.competencyDetailsEditButtons : this.competencyDetailsAddButtons}
                        isShowModal={this.state.competencyShowModal}>
                        <CompetencyModalPopUp
                            supportFileType={configuration}
                            inputChangeHandler={this.inputChangeHandler}
                            uploadDocHandler={this.uploadDocumentHandler}

                            competencyDetails={arrayUtil.sort(addElementToArray(updatedTaxonomyCompetency && updatedTaxonomyCompetency.filter(x => x.recordStatus !== "D")), 'name', 'asc')}
                            fetchDVAEffectiveDate={this.fetchDVAEffectiveDate}
                            fetchCompetencyExpiryDate={this.fetchCompetencyExpiryDate}
                            competencyExpiryDate={this.state.competencyExpiryDate}
                            dvaEffectiveDate={this.state.dvaEffectiveDate}
                            handleDateBlur={this.handleDateBlur}
                            isUploadDocumentEdit={this.props.isUploadDocumentEdit}

                            newUploadDocument={this.props.newUploadDocument}
                            competencyeditedRowData={this.editedRowData}
                            isCompetencyDetailsEdit={this.state.isCompetencyDetailsEdit}
                            getMultiSelectdValue={this.getMultiSelectdValue}
                            cancelUploadedDocumentData={this.cancelcompetencyUploadedDocument} />
                    </Modal>}
                {this.state.customerApprovedShowModal &&
                    <Modal title={this.state.isCustomerApprovedDetailsEdit ? localConstant.techSpec.Taxonomy.EDIT_CUSTOMER_APPROVED_DETAILS :
                        localConstant.techSpec.Taxonomy.ADD_CUSTOMER_APPROVED_DETAILS} modalClass="techSpecModal"
                        buttons={this.state.isCustomerApprovedDetailsEdit ? this.customerApprovedDetailsEditButtons : this.customerApprovedDetailsAddButtons}
                        isShowModal={this.state.customerApprovedShowModal}>
                        < CustomerApprovalPopUp
                            supportFileType={configuration}
                            inputChangeHandler={this.inputChangeHandler}
                            uploadDocHandler={this.uploadDocumentHandler}
                            inputChangeHandlerCustomer={this.inputChangeHandlerCustomer}
                            taxonomyCustomerApproved={taxonomyCustomerApproved}
                            customerApprovedCommodityData={this.state.SelectedCustomerName ? taxonomyCustomerCommodity : null}
                            commodityOptionSelecetLabel={this.state.SelectedCustomerName ? isEmpty(taxonomyCustomerCommodity) ? 'N/A' : null :null }
                            fetchCustomerApprovedEffectiveFromDate={this.fetchCustomerApprovedEffectiveFromDate}
                            fetchCustomerApprovedExpiryDate={this.fetchCustomerApprovedExpiryDate}
                            customerApprovedExpiryDate={this.state.customerApprovedExpiryDate}
                            customerApprovedEffectiveFromDate={this.state.customerApprovedEffectiveFromDate}
                            handleDateBlur={this.handleDateBlur}
                            isUploadDocumentEdit={this.props.isUploadDocumentEdit}
                            uploadedDocument={uploadDocument}
                            newUploadDocument={this.props.newUploadDocument}
                            editedRowData={this.editedRowData}
                            isCustomerApprovedDetailsEdit={this.state.isCustomerApprovedDetailsEdit}
                            cancelUploadedDocumentData={this.cancelcustomerUploadedDocument}
                            draftCustomerApproved={this.props.draftCustomerApproved}
                            compareDraftData={this.compareDraftData}
                            selectedDraftCustomerApproved={selectedDraftCustomerApproved} />
                    </Modal>}
                <div className="genralDetailContainer customCard">
                    <ApprovedTaxonomy
                        gridRowData={approvedTaxonomy && approvedTaxonomy
                            // approvedTaxonomy.filter(approve =>
                            //     approve.approvalStatus === localConstant.commonConstants.APPROVE)  commented for Defect 406
                        }
                        headerData={this.headerData.ApprovedTaxonomyHeader}
                    />
                    <InternalTrainingDetails
                        AddInternalTrainDetails={this.AddInternalTrainDetails}
                        DeleteHAndler={this.DeleteInternalTrainingDetailsHandler}
                        gridRowData={internalTrainingData && internalTrainingData.filter(x => x.recordStatus !== "D")}
                        onRefer={ref => { this.gridChild = ref; }}
                        rowClass={{ rowHighlight: true }}
                        draftData={this.internalTrainingDraftData}
                        isDraftData={!isEmpty(draftDataToCompare) ? true : false}
                        loggedUserType={this.loggedUserType}
                        actionType={this.props.oldProfileActionType}
                        interactionMode={this.props.interactionMode}
                        pageMode={this.props.pageMode}
                        headerData={this.headerData.InternalTrainingHeader}
                        disableEdit={this.disableEdit} />
                    <CompetencyDetails AddCompetencyDetails={this.AddCompetencyDetails}
                        DeleteHAndler={this.DeleteCompetencyDetailsHandler}
                        gridRowData={competencyGridData && competencyGridData.filter(x => x.recordStatus !== "D")}
                        onCompRefer={ref => { this.gridChildren = ref; }}
                        loggedUserType={this.loggedUserType}
                        rowClass={{ rowHighlight: true }}
                        draftData={this.competancyDraftData}
                        isDraftData={!isEmpty(draftDataToCompare) ? true : false}
                        actionType={this.state.oldProfileActionType}
                        interactionMode={this.props.interactionMode}
                        pageMode={this.props.pageMode}
                        headerData={this.headerData.CompetencyDetailHeader}
                        disableEdit={this.disableEdit}
                    />
                    <CustomerApprovedDetails AddCustomerApprovalDetails={this.AddCustomerApprovalDetails}
                        DeleteHAndler={this.DeleteCustomerApprovedDetailsHandler}
                        gridRowData={customerApprovedData && customerApprovedData.filter(x => x.recordStatus !== "D")}
                        onCustRefer={ref => { this.gridCh = ref; }}
                        loggedUserType={this.loggedUserType}
                        rowClass={{ rowHighlight: true }}
                        draftData={this.customerApprovedDraftData}
                        //isDraftData={!isEmpty(draftDataToCompare) ? true : false}
                        actionType={this.state.oldProfileActionType}
                        interactionMode={this.props.interactionMode}
                        pageMode={this.props.pageMode}
                        headerData={this.headerData.CustomerApprovedDetailHeader}
                        draftDataToCompare={draftDataToCompare}
                        selectedDraftDataToCompare={this.props.selectedDraftDataToCompare}
                        selectedDraftCustomerApproved={this.props.selectedDraftCustomerApproved}
                        enableEdit={this.enableEdit} />
                </div>
            </Fragment>
        );
    }
}
export default Taxonomy;