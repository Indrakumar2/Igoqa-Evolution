import React, { Component, Fragment } from 'react';
import {
    getlocalizeData, isEmpty, mergeobjects, bindAction, mapArrayObject, addElementToArray, compareObjects, isUndefined,isEmptyReturnDefault,isEmptyOrUndefine,isBoolean
} from '../../../../utils/commonUtils';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import Modal from '../../../../common/baseComponents/modal';
import { configuration } from '../../../../appConfig';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import dateUtil from '../../../../utils/dateUtil';
import moment, { lang } from 'moment';
import { StarRating } from 'react-star-rating-input';
import UploadDocument from '../../../../common/baseComponents/uploadDocument';
import { applicationConstants } from '../../../../constants/appConstants';
import { required } from '../../../../utils/validator';
import { verifiedStatusDisable, userTypeGet } from '../../../../selectors/techSpechSelector';
import { StringFormat } from '../../../../utils/stringUtil';
import { isEditable,isViewable } from '../../../../common/selector';
import { activitycode,levelSpecificActivities } from '../../../../constants/securityConstant'; 

const localConstant = getlocalizeData();

const ResourceCapability = (props) => {
    return (
        <CardPanel className='white lighten-4 black-text' title={localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY} colSize='s12'>
            <div className='row mb-0' >
                <CustomMultiSelect
                    hasLabel={true}
                    divClassName={"col pr-0 " + (!isEmpty(props.draftDataToCompare) && props.compareDraftMultiselectCodeStandard(props.draftcodeandstand, props.defaultCodeStanded) ? "highlightMultiSelecet" : "test")}
                    label={localConstant.techSpec.resourceCapability.CODES_STANDARDS}
                    type='multiSelect'
                    colSize='s6'
                    className='browser-default'
                    optionsList={props.codeStandard}
                    optionLabelName={'codeStandardName'}
                    onSelectChange={props.onChangeHandler}
                    multiSelectdValue={props.getMultiSelectedValue}
                    defaultValue={props.defaultCodeStanded}
                    disabled={props.isResourceDisabled || props.interactionMode} />
                <CustomMultiSelect
                    hasLabel={true}
                    divClassName={"col " + (!isEmpty(props.draftDataToCompare) && props.compareDraftMultiselectCompKnowledge(props.draftcomputerKnowledge, props.defaultcomputerKnowledge) ? "highlightMultiSelecet" : "test2")}
                    label={localConstant.techSpec.resourceCapability.COMPUTER_ELECTRONIC_KNOWLEDGE}
                    labelClass={props.isTSUserTypeCheck || props.technicalSpecialistInfo.profileStatus === 'Active' ? 'mandate' :''}
                    type='multiSelect'
                    colSize='s6'
                    className='browser-default'
                    optionsList={props.computerKnowledge}
                    optionLabelName={'computerKnowledge'}
                    onSelectChange={props.onChangeHandler}
                    multiSelectdValue={props.getComKnwldgeMltiSelectdVal}
                    defaultValue={props.defaultcomputerKnowledge}
                    disabled={props.isResourceDisabled || props.interactionMode} />
            </div>
        </CardPanel>
    );
};
const LanguageCapabilitiesModalPopUp = (props) => {
    const { editLanguageCapability } = props;
    let language = [];
    let selectedLanguage = [];
    const comprehensionCapabilityLevel = [];
    if (editLanguageCapability) {
        language = props.draftLanguageDetails.filter(x => x.id === editLanguageCapability.id);
        selectedLanguage = props.selectedDraftLanguageDetails.filter(x => x.id === editLanguageCapability.id);
    }
    return (
        <Fragment>
            <div className='col s6 p-0' >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.techSpec.resourceCapability.LANGUAGE}
                    name='language'
                    type='select'
                    inputClass={'customInputs ' + (props.compareDraftData((language.length > 0 ? language[0].language : " "), (selectedLanguage.length > 0 ? selectedLanguage[0].language : " "), props.editLanguageCapability.language,props.editLanguageCapability.recordStatus) ? "inputHighlight" : "")}
                    className='browser-default'
                    labelClass='mandate'
                    optionsList={props.languages}
                    optionName='name'
                    optionValue='name'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editLanguageCapability.language} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.techSpec.resourceCapability.WRITING_CAPABILITY}
                    name='writingCapabilityLevel'
                    type='select'
                    className='browser-default'
                    inputClass={'customInputs ' + (props.compareDraftData((language.length > 0 ? language[0].writingCapabilityLevel : " "), (selectedLanguage.length > 0 ? selectedLanguage[0].writingCapabilityLevel : " "), props.editLanguageCapability.writingCapabilityLevel,props.editLanguageCapability.recordStatus) ? "inputHighlight" : "")}
                    labelClass='mandate'
                    optionsList={props.writingCapability}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editLanguageCapability.writingCapabilityLevel} />
            </div>
            <div className='col s6 p-0' >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.resourceCapability.SPEAKING_CAPABILITY}
                    name='speakingCapabilityLevel'
                    type='select'
                    className='browser-default'
                    labelClass='mandate'
                    inputClass={'customInputs ' + (props.compareDraftData((language.length > 0 ? language[0].speakingCapabilityLevel : " "), (selectedLanguage.length > 0 ? selectedLanguage[0].speakingCapabilityLevel : " "), props.editLanguageCapability.speakingCapabilityLevel,props.editLanguageCapability.recordStatus) ? "inputHighlight" : "")}
                    optionsList={props.speakingCapability}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editLanguageCapability.speakingCapabilityLevel} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.resourceCapability.COMPREHENSION_CAPABILITY}
                    name='comprehensionCapabilityLevel'
                    type='select'
                    className='browser-default'
                    inputClass={'customInputs ' + (props.compareDraftData((language.length > 0 ? language[0].comprehensionCapabilityLevel : " "), (selectedLanguage.length > 0 ? selectedLanguage[0].comprehensionCapabilityLevel : " "), props.editLanguageCapability.comprehensionCapabilityLevel,props.editLanguageCapability.recordStatus) ? "inputHighlight" : "")}
                    labelClass='mandate'
                    optionsList={props.writingCapability}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editLanguageCapability.comprehensionCapabilityLevel} />
            </div>
        </Fragment>
    );
};
const CertificateDetailsModalPopUp = (props) => {
    const { editCertificateDetails } = props;
    let documents=[];
    let verificationDocuments=[];
    let certificate = [];
    let selectedCertificate = [];
    let isStatusVerified=false;
    let newDocument=[];
    if(props.editCertificateDetails && props.editCertificateDetails.documents)
    {
        documents=props.editCertificateDetails.documents.filter(x=>x.recordStatus!=='D');
        newDocument  = documents; 
    }
    if(props.uploadedDocument && props.uploadedDocument.length>0)
    {
        newDocument= props.uploadedDocument.filter(x => (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_Certificate' ) && x.recordStatus!=='D');  
    }
    if(props.editCertificateDetails && props.editCertificateDetails.verificationDocuments)
    {
        verificationDocuments=props.editCertificateDetails.verificationDocuments.filter(x=>x.recordStatus!=='D'); 
    }
    if (editCertificateDetails) {
        certificate = props.draftCertificationDetails.filter(x => x.id === editCertificateDetails.id);
        selectedCertificate = props.selectedDraftCertificationDetails.filter(x => x.id === editCertificateDetails.id);
        isStatusVerified= verifiedStatusDisable( { obj:editCertificateDetails } );//D573
    }
    return (
        <Fragment>
            <div className='row' >
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.CERTIFICATE_NAME}
                    name='certificationName'
                    type='select'
                    className='browser-default'
                    labelClass='mandate'
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].certificationName : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].certificationName : " "), props.editCertificateDetails.certificationName, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    optionsList={props.certificates}
                    optionName='name'
                    optionValue='id' //D1172
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editCertificateDetails.iLearnID}
                    disabled={isStatusVerified} 
                     />
                {props.fieldsToHide ? null : <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.TYPE}
                    name='type'
                    type='select'
                    className='browser-default'
                    optionsList={props.type}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].isExternal : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].isExternal : " "), props.editCertificateDetails.isExternal , props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    defaultValue={props.editCertificateDetails.type ? props.editCertificateDetails.type : ''}
                    disabled={isStatusVerified} 
                    />}
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.CERTIFICATE_ID}
                    name='certificateRefId'
                    type='text'
                    onValueChange={props.onChangeHandler}
                    maxLength={255}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].certificateRefId : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].certificateRefId : " "),props.editCertificateDetails.certificateRefId , props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    defaultValue={props.editCertificateDetails.certificateRefId}
                    readOnly={isStatusVerified} />
            </div>
            <div className='row' >
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.CERTIFICATE_DETAILS}
                    name='description'
                    type='text'
                    labelClass='mandate'
                    onValueChange={props.onChangeHandler}
                    maxLength={255}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].description : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].description : " "), props.editCertificateDetails.description, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    defaultValue={props.editCertificateDetails.description}
                    readOnly={isStatusVerified} />
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.EFFECTIVE_DATE}
                    name='effeciveDate'
                    type='date'
                    selectedDate={props.effectiveDate}
                    onDateChange={props.fetchEffectiveDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDatePickBlur={props.handleDate}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].effeciveDate : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].effeciveDate : " "), props.editCertificateDetails.effeciveDate, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    disabled={isStatusVerified} />
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.EXPIRY_DATE}
                    name='expiryDate'
                    type='date'
                    selectedDate={props.certificateExpiryDate}
                    onDateChange={props.fetchCertificateExpiryDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDatePickBlur={props.handleDate}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].expiryDate : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].effeciveDate : " "), props.editCertificateDetails.expiryDate, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")} //D556
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT} 
                    disabled={isStatusVerified} />
            </div>
            {props.fieldsToHide ? null : <div><div className='row' >
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.VERIFIED_BY}
                    name='verifiedBy'
                    type='text'
                    dataValType='valueText'
                    disabled={true}
                    maxLength={100}
                    onValueChange={props.onChangeHandler}
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].verifiedBy : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].verifiedBy : " "), props.editCertificateDetails.verifiedBy, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    value={props.verifiedBy !== null ? props.verifiedBy : null } //D218 (Changes for 12-02-2020 Failed ALM Doc)
                    readOnly={true} />

                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    labelClass={props.isRCUserTypeCheck ? 'mandate' : ''}
                    label={localConstant.techSpec.resourceCapability.VERIFICATION_STATUS}
                    name='verificationStatus'
                    type='select'
                    className='browser-default'
                    optionsList={props.verificationStatus}
                    optionName='value'
                    optionValue='value'
                    inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].verificationStatus : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].verificationStatus : " "), props.editCertificateDetails.verificationStatus, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                    onSelectChange={props.onChangeHandler}
                    defaultValue={props.editCertificateDetails.verificationStatus}
                    disabled={isStatusVerified} />
                <div className="col s4 pl-0 mt-1">
                    <label className="col s12">{localConstant.techSpec.resourceCapability.VERIFICATION_DATE}</label>
                    <span className="col s12"> {moment(props.certificateVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT)}</span>
                </div>
            </div>
                <div className='row' >
                    <CustomInput
                        hasLabel={true}
                        colSize='s4'
                        label={localConstant.techSpec.resourceCapability.VERIFICATION_TYPE}
                        name='verificationType'
                        type='select'
                        className='browser-default'
                        optionsList={props.editCertificateDetails.isILearn === true ? props.verificationTypeILearn : props.verificationType} //D1172
                        optionName='value'
                        optionValue='value'
                        inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].verificationType : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].verificationType : " "), props.editCertificateDetails.verificationType, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                        onSelectChange={props.onChangeHandler}
                        defaultValue={props.editCertificateDetails.verificationType}
                        disabled={isStatusVerified} />

                    <CustomInput
                        hasLabel={true}
                        divClassName='col mt-1'
                        label={localConstant.techSpec.resourceCapability.NOTES}
                        type='textarea'
                        defaultValue={props.editCertificateDetails.notes}
                        name='notes'
                        colSize='s8'
                        inputClass={'customInputs ' + (props.compareDraftData((certificate.length > 0 ? certificate[0].notes : " "), (selectedCertificate.length > 0 ? selectedCertificate[0].notes : " "), props.editCertificateDetails.notes, props.editCertificateDetails.recordStatus) ? "inputHighlight" : "")}
                        maxLength={4000}
                        onValueChange={props.onChangeHandler}
                        readOnly={isStatusVerified} />
                </div></div>}
            <div className="row" >
                <UploadDocument
                    defaultValue={ documents && documents.length>0 ? documents : '' }
                    className={"col s6 "}
                    label={localConstant.techSpec.resourceCapability.CERTIFICATE_UPLOAD}
                    cancelUploadedDocument={props.cancelUploadedDocumentData}
                    cancelDisplaynone={isStatusVerified}
                    editDocDetails={props.editCertificateDetails}
                    gridName={localConstant.techSpec.common.TECHNICALSPECIALISTCERTIFICATION}
                    id="uploadFiles"
                    uploadFileRefType="uploadDocument"
                    disabled={isStatusVerified}
                    inputClass={(props.compareDraftData((certificate.length > 0 && !isEmptyOrUndefine(certificate[0].documents) &&  certificate[0].documents.length>0 ? certificate[0].documents[0].documentUniqueName : " "), ( selectedCertificate.length > 0  && !isEmptyOrUndefine(selectedCertificate[0].documents) && selectedCertificate[0].documents.length>0 ? selectedCertificate[0].documents[0].documentUniqueName  : " "), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ? newDocument[0].documentUniqueName : ''), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ?(newDocument[0].recordStatus==null? undefined : newDocument[0].recordStatus) : undefined)) ? " customInputs inputHighlight" : "")}
                    documentType="TS_Certificate" />

                {(!props.fieldsToHide && props.isShowVerifiedUpload) && <UploadDocument
                    defaultValue= { verificationDocuments && verificationDocuments.length>0 ? verificationDocuments : '' }
                    className={"col s6 "}
                    label={localConstant.techSpec.resourceCapability.VERIFICATION_DOCUMENT}
                    cancelUploadedDocument={props.verificationCancelUploadedDocumentData}
                    cancelDisplaynone={isStatusVerified}
                    editDocDetails={props.editCertificateDetails}
                    gridName={localConstant.techSpec.common.TECHNICALSPECIALISTCERTIFICATION}
                    id="verficationUploadFile"
                    uploadFileRefType="verficationUpload"
                    disabled={isStatusVerified} 
                    documentType="TS_CertVerification" />
                }

            </div>
        </Fragment>

    );
};
const CommodityModalPopUp = (props) => { 
    const { editCommodityDetails } = props;
    let commodity = [];
    let selectedCommodity = []; 
    if (editCommodityDetails) {
        commodity = props.draftEquipmentDetails.filter(x => x.commodity == editCommodityDetails.commodity);
        selectedCommodity = props.draftEquipmentDetails.filter(x => x.commodity == editCommodityDetails.commodity); 
    }

    return (
        <div className='row mb-0' >
            <CustomInput
                hasLabel={true}
                divClassName='col pr-0'
                label={localConstant.techSpec.resourceCapability.COMMODITY}
                type='select'
                colSize='s3'
                className='browser-default'
                optionName='name'
                optionValue='name'
                optionsList={props.commodity}
                onSelectChange={props.onChangeHandler}
                name="commodity"
                labelClass='mandate'
                inputClass={'customInputs ' + (props.compareDraftData((commodity.length > 0 ? commodity[0].id : " "), (selectedCommodity.length > 0 ? selectedCommodity[0].id : " "), props.editCommodityDetails.id,props.editCommodityDetails.recordStatus) ? "inputHighlight" : "")}
                defaultValue={props.editCommodityDetails.commodity}
                disabled={props.disabledCommodity}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.techSpec.resourceCapability.EQUIP_KNOWLEDGE}
                type='multiSelect'
                valueType="defaultValue"
                colSize='s9'
                className='browser-default'
                labelClass='mandate'
                optionsList={props.equipment}
                divClassName={"col " + ((props.compareDraftData(( !isEmptyOrUndefine(commodity) && commodity.length > 0 ? commodity.map(x=> { return  x.equipmentKnowledge; }).sort().join(","): " "), ( !isEmptyOrUndefine(selectedCommodity) && selectedCommodity.length > 0 ? selectedCommodity.map(x=> { return  x.equipmentKnowledge; }).sort().join(",") : " "), (!isEmptyOrUndefine(props.editCommodityDetails.equipmentKnowledge) && props.editCommodityDetails.equipmentKnowledge.length>0 ? props.editCommodityDetails.equipmentKnowledge.map(x=> { return  x.value; }).sort().join(","): null ),props.editCommodityDetails.recordStatus)) ? "highlightMultiSelecet" : "test2")} // def 848
                defaultValue={props.editCommodityDetails && props.editCommodityDetails.equipmentKnowledge}
                multiSelectdValue={props.getCommodityMultiSelectedValue}
                disabled={props.enableEquipment}
                refProps={props.onRef} //D219
            />
            <div className='row mb-0 pl-2 modalRowHeight'>
                {
                props.equipmentRating.map((data) => {
                const curEquipKn= props.editCommodityDetails.equipmentKnowledge && props.editCommodityDetails.equipmentKnowledge.find(x=>x.id===data.id);  //def 848 highlight issue fix

                    return (<Fragment>
                        <div className='col s3 pl-3 customCommodity_Div' >{data.value}</div>
                        <CustomInput
                            divClassName='col pl-0'
                            type='select'
                            selectstatus={true}
                            className='browser-default'
                            colSize='s1'
                            optionName='value'
                            optionValue='value'
                            optionsList={props.rating}
                            name={data.value}
                            defaultValue={(data.rating || data.rating === 0) ? data.rating.toString() : '0'}
                            dataType='ratingSelect'
                            id={'ratingSelect'}
                            inputClass={'customInputs ' + (props.compareDraftData((commodity.length > 0 ? commodity[0].equipmentKnowledgeLevel : " "), (selectedCommodity.length > 0 ? selectedCommodity[0].equipmentKnowledgeLevel : " "), ( !isEmptyOrUndefine(curEquipKn) ? curEquipKn.rating :' ' ) , props.editCommodityDetails.recordStatus) ? "inputHighlight" : "")}
                            onSelectChange={props.onChangeHandler} />

                    </Fragment>);
                }
                )}
            </div>
            <div className="col s12">
                <label>{localConstant.techSpec.NOTE}:</label>
                <ul className="knowledgeNote">
                    <li>{localConstant.techSpec.NOTE_TYPE.NO_EXPERIENCE}</li>
                    <li>{localConstant.techSpec.NOTE_TYPE.BRIEF_EXPERIENCE}</li>
                    <li>{localConstant.techSpec.NOTE_TYPE.LIMITED_EXPERIENCE}</li>
                    <li>{localConstant.techSpec.NOTE_TYPE.AVERAGE_EXPERIENCE}</li>
                    <li>{localConstant.techSpec.NOTE_TYPE.EXTENSIVE_EXPERIENCE}</li>
                    <li>{localConstant.techSpec.NOTE_TYPE.EXTENSIVE_REPETITIVE_EXPERIENCE}</li>
                </ul>
            </div>
        </div>
    );
};
const TrainingDetailsModalPopUp = (props) => {
    const { editTrainingDetails } = props;
    let documents=[];
    let verificationDocuments=[];
    let training = [];
    let selectedTraining = [];
    let isStatusVerified=false;
    let newDocument=[];
    if (editTrainingDetails) {
        training = props.drafttrainingDetails.filter(x => x.id == editTrainingDetails.id);
        selectedTraining = props.drafttrainingDetails.filter(x => x.id == editTrainingDetails.id);
        isStatusVerified= verifiedStatusDisable( { obj:editTrainingDetails } );//D573
    }
    if(props.editTrainingDetails && props.editTrainingDetails.documents)
    {
        documents=props.editTrainingDetails.documents.filter(x=>x.recordStatus!=='D'); 
        newDocument  = documents; 
    }
    if(props.uploadedDocument && props.uploadedDocument.length>0) //def 848
    {
        newDocument= props.uploadedDocument.filter(x => (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_Training' ) && x.recordStatus!=='D');  
    }
    if(props.editTrainingDetails && props.editTrainingDetails.verificationDocuments)
    {
        verificationDocuments=props.editTrainingDetails.verificationDocuments.filter(x=>x.recordStatus!=='D'); 
    }

    return (
        <Fragment>
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.TRAINING_NAME}
                    type='select'
                    className='browser-default'
                    optionName='name'
                    optionValue='id' //D1172
                    optionsList={props.training}
                    name='trainingName'
                    defaultValue={props.editTrainingDetails.iLearnID}
                    labelClass='mandate'
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].trainingName : " "), (selectedTraining.length > 0 ? selectedTraining[0].trainingName : " "), props.editTrainingDetails.trainingName,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    onSelectChange={props.onChangeHandler}
                    disabled={isStatusVerified} />
                {props.fieldsToHide ? null : <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.TYPE}
                    type='select'
                    className='browser-default'
                    optionName='value'
                    optionValue='value'
                    optionsList={props.type}
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].isExternal : " "), (selectedTraining.length > 0 ? selectedTraining[0].isExternal : " "), props.editTrainingDetails.isExternal,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    defaultValue={props.editTrainingDetails.type ? props.editTrainingDetails.type : ''}
                    name='type'
                    onSelectChange={props.onChangeHandler}
                    disabled={isStatusVerified} />}
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.TRAINING_ID}
                    type='text'
                    defaultValue={props.editTrainingDetails.trainingRefId}
                    name='trainingRefId'
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].trainingRefId : " "),(selectedTraining.length > 0 ? selectedTraining[0].trainingRefId : " "),props.editTrainingDetails.trainingRefId,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    maxLength={255}
                    onValueChange={props.onChangeHandler}
                    readOnly={isStatusVerified} />
            </div>
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.DURATION}
                    type='text'
                    defaultValue={props.editTrainingDetails.duration}
                    onValueChange={props.onChangeHandler}
                    name='duration'
                    maxLength={300} //changes for GRM Data Migration issue
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].duration : " "), (selectedTraining.length > 0 ? selectedTraining[0].duration : " "), props.editTrainingDetails.duration,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    labelClass='mandate'
                    readOnly={isStatusVerified} />
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.DATE_OF_TRAINING}
                    type='date'
                    selectedDate={props.dateOfTraining}
                    onDateChange={props.fetchDateOfTraining}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    defaultValue={props.editTrainingDetails.effeciveDate}
                    name='effeciveDate'
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].effeciveDate : " "), (selectedTraining.length > 0 ? selectedTraining[0].effeciveDate : " "), props.editTrainingDetails.effeciveDate,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    onDatePickBlur={props.handleDate}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    disabled={isStatusVerified} />

                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.EXPIRY_DATE}
                    type='date'
                    selectedDate={props.trainingExpiryDate}
                    onDateChange={props.fetchTrainingExpiryDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    defaultValue={props.editTrainingDetails.expiryDate}
                    name='expiryDate'
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].expiryDate : " "), (selectedTraining.length > 0 ? selectedTraining[0].expiryDate : " "), props.editTrainingDetails.expiryDate,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    onDatePickBlur={props.handleDate}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    disabled={isStatusVerified} />
            </div>
            {props.fieldsToHide ? null : <div><div className="row">
                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.VERIFIED_BY}
                    type='text'
                    dataValType='valueText'
                    value={props.verifiedBy !== null ? props.verifiedBy : null } //D222 (Changes for 12-02-2020 Failed ALM Doc)
                    name='verifiedBy'
                    maxLength={100}
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].verifiedBy : " "), (selectedTraining.length > 0 ? selectedTraining[0].verifiedBy : " "), props.editTrainingDetails.verifiedBy,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    onValueChange={props.onChangeHandler}
                    readOnly={true} />

                <CustomInput
                    hasLabel={true}
                    colSize='s4'
                    label={localConstant.techSpec.resourceCapability.VERIFICATION_STATUS}
                    type='select'
                    className='browser-default'
                    optionName='value'
                    optionValue='value'
                    optionsList={props.verificationStatus}
                    defaultValue={props.editTrainingDetails.verificationStatus}
                    name='verificationStatus'
                    inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].verificationStatus : " "), (selectedTraining.length > 0 ? selectedTraining[0].verificationStatus : " "), props.editTrainingDetails.verificationStatus,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                    onSelectChange={props.onChangeHandler}
                    disabled={isStatusVerified} />

                <div className="col s4 pl-0 mt-1">
                    <label className="col s12">{localConstant.techSpec.resourceCapability.VERIFICATION_DATE}</label>
                    <span className="col s12">{moment(props.trainingVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT)}</span>
                </div>
            </div>
                <div className="row">
                    <CustomInput
                        hasLabel={true}
                        colSize='s4'
                        label={localConstant.techSpec.resourceCapability.VERIFICATION_TYPE}
                        type='select'
                        className='browser-default'
                        optionName='value'
                        optionValue='value'
                        optionsList={props.editTrainingDetails.isILearn === true ? props.verificationTypeILearn : props.verificationType} //D1172
                        defaultValue={props.editTrainingDetails.verificationType}
                        name='verificationType'
                        inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].verificationType : " "), (selectedTraining.length > 0 ? selectedTraining[0].verificationType : " "), props.editTrainingDetails.verificationType,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                        onSelectChange={props.onChangeHandler}
                        disabled={isStatusVerified} />

                    <CustomInput
                        hasLabel={true}
                        divClassName='col mt-1'
                        label={localConstant.techSpec.resourceCapability.NOTES}
                        type='textarea'
                        defaultValue={props.editTrainingDetails.notes}
                        name='notes'
                        colSize='s8'
                        inputClass={'customInputs ' + (props.compareDraftData((training.length > 0 ? training[0].notes : " "), (selectedTraining.length > 0 ? selectedTraining[0].notes : " "), props.editTrainingDetails.notes,props.editTrainingDetails.recordStatus) ? "inputHighlight" : "")}
                        maxLength={4000}
                        onValueChange={props.onChangeHandler} 
                        readOnly={isStatusVerified} />

                </div></div>}
            <div className="row">
                <div className="col s6 pr-0 pt-2" >
                    <UploadDocument
                        defaultValue={ documents && documents.length>0 ? documents : '' }
                        className={"col s12 pl-0 pr-0 "}
                        label={localConstant.techSpec.resourceCapability.TRAINING_UPLOAD}
                        cancelUploadedDocument={props.cancelUploadedDocumentData}
                        cancelDisplaynone={isStatusVerified}
                        editDocDetails={props.editTrainingDetails}
                        gridName={localConstant.techSpec.common.TECHNICALSPECIALISTTRAINING}
                        id="uploadFiles"
                        uploadFileRefType="uploadDocument" 
                        disabled={isStatusVerified}
                        inputClass={(props.compareDraftData((training.length > 0 && !isEmptyOrUndefine(training[0].documents) && training[0].documents.length>0  ? training[0].documents[0].documentUniqueName : " "), (selectedTraining.length > 0 && !isEmptyOrUndefine(selectedTraining[0].documents) && selectedTraining[0].documents.length>0  ? selectedTraining[0].documents[0].documentUniqueName  : " "), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ? newDocument[0].documentUniqueName : ''), (!isEmptyOrUndefine(newDocument) && newDocument.length>0 ?(newDocument[0].recordStatus==null? undefined : newDocument[0].recordStatus) : undefined)) ? " customInputs inputHighlight" : "")}//def848
                        documentType="TS_Training" />
                </div>
                {(!props.fieldsToHide && props.isShowVerifiedUpload ) && <UploadDocument
                    defaultValue={ verificationDocuments && verificationDocuments.length>0 ? verificationDocuments : '' }
                    className={"col s6 pt-2"}
                    label={localConstant.techSpec.resourceCapability.VERIFICATION_DOCUMENT}
                    cancelUploadedDocument={props.traningVerificationCancelUploadedDocumentData}
                    cancelDisplaynone={isStatusVerified}
                    editDocDetails={props.editTrainingDetails}
                    gridName={localConstant.techSpec.common.TECHNICALSPECIALISTTRAINING}
                    id="verficationUploadFile"
                    uploadFileRefType="verficationUpload"
                    disabled={isStatusVerified}
                    documentType="TS_TrainingVerification" />
                }
            </div>

        </Fragment>

    );
};
class ResourceCapabilityCertification extends Component {
    constructor(props) {
        super(props);
        this.state = {
            effectiveDate: '',
            certificateExpiryDate: '',
            certificateVerificationDate: moment(),
            dateOfTraining: '',
            trainingExpiryDate: '',
            trainingVerificationDate: moment(),
            inValidDateFormat: false,
            equipmentRating: [], 
            enableEquipment: true,
            disabledCommodity: false,
            //Show Modal
            isLanguageCapabilityShowModal: false,
            isCertificateDetailsShowModal: false,
            isCommodityShowModal: false,
            isTrainingDetailsShowModal: false,
            //Edit Modal
            isLanguageCapabilityEditModal: false,
            isCertificateDetailsEditModal: false,
            isCommodityEditModal: false,
            isTrainingDetailsEditModal: false,

            uploadCertificateDocument: false,
            isShowVerifiedUpload:false,
            verifiedBy:"",
            recordChng: false

        }; 
        this.commodityArray = [];
        this.updatedRating = [];
        const functionRefs = {};
        const fieldsToHide = this.fieldsToHide();
        functionRefs["enableEditColumn"] = ()=>this.enableEditColumn(props); /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        this.headerData = HeaderData(functionRefs, fieldsToHide);
        this.selectedEquipmentName = [];
        this.selectedRating = [];
        this.ratingValue = [];
        this.updatedData = {};
        this.editedRowData = {};
        this.updateFromRC_RM = [];
        this.updateFromRc_RMLanguage = [];
        this.updateFromRc_RMCertificate = [];
        this.updateFromRc_RMCommodity = [];
        this.updateFromRc_RMTraining = [];
        this.multiSelectUpdatedArray = [];
        this.languageAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.languageCapabilityHideModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addLanguageDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Language Capability: Edit Buttons
        this.languageEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.languageCapabilityHideModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateLanguageDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Certificate Details: Add Buttons
        this.certificateAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCertificateDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addCertificateDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Certificate Details: Edit Buttons
        this.certificateEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCertificateDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateCertificateDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Commodity: Add Buttons
        this.commodityAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commodityHideModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addCommodityDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Commodity: Edit Buttons
        this.commodityEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commodityHideModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateCommodityDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];

        //Training Details: Add Buttons
        this.trainingAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.CancelTrainingDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addTrainingDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        //Training Details: Edit Buttons
        this.trainingEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.CancelTrainingDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateTrainingDetails,
                btnClass: 'btn-small',
                showbtn: true
            }
        ];
        this.groupingParam = {
            groupName: "commodity",
            dataName: "equipmentKnowledge"
        };
        this.userTypes = userTypeGet();

        bindAction(this.headerData.LanguageCapabilitiesHeader, "EditLanguageCapability", this.editLanguageCapabilityHandler);
        bindAction(this.headerData.CertificateDetailsHeader, "EditCertificateDetails", this.editCertificateDetailsHandler);
        bindAction(this.headerData.CommodityKnowledgeHeader, "EditCommodityDetails", this.editCommodityDetailsHandler);
        bindAction(this.headerData.TrainingDetailsHeader, "EditTrainingDetails", this.editTrainingDetailsHandler);
    }
    fieldsToHide = () => {
        if (this.props.isTSUserTypeCheck) {
            return true;
        }
        else {
            return false;
        }
    };
    enableEditColumn = (e) => {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        return  !this.fieldDisableHandler();
    }

    fieldDisableHandler = () => { /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
    if(this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
        if(!this.fieldsToHide()){
            return false;
           }
      }
      else if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
        return false;
     }//Added for D1374
      return  isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });//D1374 Failed on 29-10-2020
    }
 
    //ResourceCapability-codeStandard: Multi Selection Handler
    getMultiSelectedValue = (data) => {
        const codeStandtedData = [];
        let matchedIdRecords = [];
        let finalCodeStandard = [];
        if (!isEmpty(data)) {
            const selectedItem = mapArrayObject(data, 'value').join(',');
            const array = selectedItem.split(',');
            this.codeStandted = {};
            if (this.props.selectedCodeStanded === null) {
                for (let i = 0; i < array.length; i++) {
                    this.codeStandted["id"] = 0; 
                    this.codeStandted["codeStandardName"] = array[i];
                    this.codeStandted["epin"] = this.props.epin;
                    this.codeStandted["updateCount"] = null;
                    this.codeStandted["lastModification"] = null;
                    this.codeStandted["recordStatus"] = "N";
                    this.codeStandted["modifiedBy"] = null;
                    codeStandtedData.push(this.codeStandted);
                    this.codeStandted = {};
                }
            } else {
                matchedIdRecords = this.props.selectedCodeStanded.filter(function (cd, i) {
                    return array.some(d => d === cd.codeStandardName);
                });
                const itemsTobeRemoved = this.props.selectedCodeStanded.filter(val => {
                    return array.indexOf(val.codeStandardName) === -1;
                });
                let recordsToAdd = [];
                recordsToAdd = matchedIdRecords.filter(val => {
                    return array.indexOf(val.codeStandardName) > -1;
                });
                recordsToAdd = array.filter(val => {
                    return !recordsToAdd.some(d => d.codeStandardName === val);
                });
                if (!isEmpty(recordsToAdd)) {
                    for (let i = 0; i < recordsToAdd.length; i++) {
                        this.codeStandted["id"] = 0;
                        this.codeStandted["codeStandardName"] = recordsToAdd[i];
                        this.codeStandted["epin"] = this.props.epin;
                        this.codeStandted["updateCount"] = null;
                        this.codeStandted["lastModification"] = null;
                        this.codeStandted["recordStatus"] = "N";
                        this.codeStandted["modifiedBy"] = null;
                        codeStandtedData.push(this.codeStandted);
                        this.codeStandted = {};
                    }
                }
                if (!isEmpty(itemsTobeRemoved)) {
                    for (let i = 0; i < itemsTobeRemoved.length; i++) {
                        if (itemsTobeRemoved[i].recordStatus !== "N") {
                            itemsTobeRemoved[i].recordStatus = 'D';
                            codeStandtedData.push(itemsTobeRemoved[i]);
                        }
                    }
                }
            }
        } else {
            for (let i = 0; i < this.props.selectedCodeStanded.length; i++) {
                if (this.props.selectedCodeStanded[i].recordStatus !== "N") {
                    this.props.selectedCodeStanded[i].recordStatus = 'D';
                    codeStandtedData.push(this.props.selectedCodeStanded[i]);
                }
            }
        }
        finalCodeStandard = [
            ...matchedIdRecords, ...codeStandtedData
        ];
        if ((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM)) || (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))) {
            this.props.actions.IsRCRMUpdatedResourceCapabilityCodeStandard(true);
        }
        this.props.actions.UpdateResourceCapabilityCodeStandard(finalCodeStandard);
    }
    //ResourceCapability-computerKnowledge: Multi Selection Handler
    getComKnwldgeMltiSelectdVal = (data) => {
        const computerKnowledgeData = [];
        let matchedIdRecords = [];
        let finalComputerKnowledge = [];
        if (!isEmpty(data)) {
            const selectedItem = mapArrayObject(data, 'value').join(',');
            const array = selectedItem.split(',');
            this.computerKnowledge = {};
            if (this.props.selectedComputerKnowledge === null) {
                for (let i = 0; i < array.length; i++) {
                    this.computerKnowledge["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.computerKnowledge["computerKnowledge"] = array[i];
                    this.computerKnowledge["epin"] = this.props.epin;
                    //this.computerKnowledge["updateCount"] = null;
                    //this.computerKnowledge["lastModification"] = null;
                    this.computerKnowledge["recordStatus"] = "N";
                    //this.computerKnowledge["modifiedBy"] = null;
                    computerKnowledgeData.push(this.computerKnowledge);
                    this.computerKnowledge = {};
                }
            } else {
                matchedIdRecords = this.props.selectedComputerKnowledge.filter(function (cd) {
                    return array.some(d => d === cd.computerKnowledge);
                });
                const itemsTobeRemoved = this.props.selectedComputerKnowledge.filter(val => {
                    return array.indexOf(val.computerKnowledge) === -1;
                });
                let recordsToAdd = [];
                recordsToAdd = matchedIdRecords.filter(val => {
                    return array.indexOf(val.computerKnowledge) > -1;
                });
                recordsToAdd = array.filter(val => {
                    return !recordsToAdd.some(d => d.computerKnowledge === val);
                });
                if (!isEmpty(recordsToAdd)) {
                    for (let i = 0; i < recordsToAdd.length; i++) {
                        this.computerKnowledge["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                        this.computerKnowledge["computerKnowledge"] = recordsToAdd[i];
                        this.computerKnowledge["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                        //this.computerKnowledge["updateCount"] = null;
                        //this.computerKnowledge["lastModification"] = null;
                        this.computerKnowledge["recordStatus"] = "N";
                        //this.computerKnowledge["modifiedBy"] = null;
                        computerKnowledgeData.push(this.computerKnowledge);
                        this.computerKnowledge = {};
                    }
                }
                if (!isEmpty(itemsTobeRemoved)) {
                    for (let i = 0; i < itemsTobeRemoved.length; i++) {
                        if (itemsTobeRemoved[i].recordStatus !== "N") {
                            itemsTobeRemoved[i].recordStatus = 'D';
                            computerKnowledgeData.push(itemsTobeRemoved[i]);
                        }
                    }
                }
            }
        } else {
            for (let i = 0; i < this.props.selectedComputerKnowledge.length; i++) {
                if (this.props.selectedComputerKnowledge[i].recordStatus !== "N") {
                    this.props.selectedComputerKnowledge[i].recordStatus = 'D';
                    computerKnowledgeData.push(this.props.selectedComputerKnowledge[i]);
                }
            }
        }
        finalComputerKnowledge = [
            ...matchedIdRecords, ...computerKnowledgeData
        ];
        if ((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM)) || (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))) {
            this.props.actions.IsRCRMUpdatedResourceCapabilityComKnowledge(true);
        }
        this.props.actions.UpdateResourceCapabilityComputerKnowledge(finalComputerKnowledge);
    }
    //Commodity: Multi Selection Handler
    getCommodityMultiSelectedValue = (data) => {
        this.updatedRating = [];
        data.map((data, i) => {
            if (!isEmpty(this.state.equipmentRating)) {
                this.state.equipmentRating.map((item) => {
                    if (data.name === undefined) {
                        if (item.value === data.value) {
                            this.updatedRating.push({ id: item.id, value: item.value, rating: Number(item.rating) });
                        }
                    }
                    else if (item.value === data.name) {
                        this.updatedRating.push({ id: item.id, value: item.value, rating: Number(item.rating) });
                    }
                });
                if (i + 1 > this.state.equipmentRating.length) {
                    this.updatedRating.push({ id: data.id, value: data.name, rating: 0 });
                }
            } else {
                this.updatedRating.push({ id: data.id, value: data.name, rating: 0 });
            }
        });
        this.setState({ equipmentRating: this.updatedRating });
        this.updatedData.equipmentKnowledge = this.updatedRating;
         if(!isEmpty(this.editedRowData)){
            this.updatedData.commodity = this.editedRowData.commodity;
         }
    }
    editMultiSelectedValue = (data) => {
        const equipmentName = [];
        if (!isEmpty(data.equipmentKnowledge)) {
            data.equipmentKnowledge && data.equipmentKnowledge.map(item => {
                equipmentName.push({ id: item.id, value: item.value, label: item.value, rating: Number(item.rating) });
            });
            this.setState({ equipmentRating: equipmentName });
        }
        this.editedRowData.equipmentKnowledge = [];
        this.editedRowData.equipmentKnowledge = equipmentName;
        this.setState({
            isCommodityShowModal: true,
            enableEquipment: false,
            disabledCommodity: true
        });
    }
    //Edit Handlers
    editLanguageCapabilityHandler = (data) => {
        this.setState((state) => {
            return {
                isLanguageCapabilityShowModal: !state.isLanguageCapabilityShowModal,
                isLanguageCapabilityEditModal: true
            };
        });
        this.editedRowData = data;
    }
    editCertificateDetailsHandler = (data) => {
        this.setState((state) => {
            return {
                isCertificateDetailsShowModal: !state.isCertificateDetailsShowModal,
                isCertificateDetailsEditModal: true
            };
        });
        this.editedRowData = data;
        if (isBoolean(data.isExternal)) { //D218 (Changes for 12-02-2020 Failed ALM Doc)
            if (data.isExternal === true) {
                this.editedRowData['type'] = 'External';
            } else {

                this.editedRowData['type'] = 'Internal';
            }
        }
        else{
            this.editedRowData['type']='';
        }
        if(isEmpty(data.verifiedBy))
            this.setState({ verifiedBy:this.props.loggedInUser });//Sanity defect 91
        else
            this.setState({ verifiedBy:data.verifiedBy }); //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
        if(data.verificationStatus === 'Verified'){           
            this.setState({ isShowVerifiedUpload: true,
                            }); //D1130 Issue (7.2)
        }else{
            this.setState({ isShowVerifiedUpload: false,
                                }); //D218 (Changes for 12-02-2020 Failed ALM Doc)
        }
        this.setCertificateGridDate();
        // this.checkingEmptyCertificateDocument();
    }
    setCertificateGridDate = () => {
        if (this.editedRowData) {
            if (this.editedRowData.effeciveDate !== null && this.editedRowData.effeciveDate !== "") {
                this.setState({ effectiveDate: moment(this.editedRowData.effeciveDate) });
            }
            else {
                this.setState({ effectiveDate: '' });
            }
            if (this.editedRowData.expiryDate !== null && this.editedRowData.expiryDate !== "") {
                this.setState({ certificateExpiryDate: moment(this.editedRowData.expiryDate) });
            }
            else {
                this.setState({ certificateExpiryDate: "" });
            }
            if (this.editedRowData.verificationDate !== null && this.editedRowData.verificationDate !== "") {
                this.setState({ certificateVerificationDate: moment(this.editedRowData.verificationDate) });
            }
            else {
                this.setState({ certificateVerificationDate: moment() });
            }
        } else {
            this.setState({
                effectiveDate: '',
                certificateExpiryDate: '',
                certificateVerificationDate: moment()
            });
        }
    }

    editCommodityDetailsHandler = (data) => {
        this.setState((state) => {
            return {
                isCommodityShowModal: !state.isCommodityShowModal,
                isCommodityEditModal: true
            };
        });
        this.editedRowData = data;
        this.updatedData.commodity=data.commodity;
        this.updatedData.equipmentKnowledge =data.equipmentKnowledge;
        this.props.actions.FetchEquipment(data.commodity);
        this.editMultiSelectedValue(data);
    }
    editTrainingDetailsHandler = (data) => {
        this.setState((state) => {
            return {
                isTrainingDetailsShowModal: !state.isTrainingDetailsShowModal,
                isTrainingDetailsEditModal: true
            };
        });
        this.editedRowData = data;
        if(isBoolean(data.isExternal)) //D222 (Changes for 12-02-2020 Failed ALM Doc)
        {
            if(data.isExternal===true)
            {
            this.editedRowData['type']='External';
            }
            else{
                this.editedRowData['type']='Internal';
            }
        }
        else{
            this.editedRowData['type']='';
        }
        if(isEmpty(data.verifiedBy))
            this.setState({ verifiedBy:this.props.loggedInUser }); //Sanity defect 91
        else
            this.setState({ verifiedBy:data.verifiedBy }); //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
        if(data.verificationStatus === 'Verified'){           
            this.setState({ isShowVerifiedUpload: true,
                             }); //D1130 Issue (7.2)
        }else{
            this.setState({ isShowVerifiedUpload: false,
                             }); //D222 (Changes for 12-02-2020 Failed ALM Doc)
        }
        this.setTrainingGridDate();
    }
    setTrainingGridDate = () => {
        if (this.editedRowData) {
            if (this.editedRowData.effeciveDate !== null && this.editedRowData.effeciveDate !== "") {
                this.setState({ dateOfTraining: moment(this.editedRowData.effeciveDate) });
            }
            else {
                this.setState({ dateOfTraining: '' });
            }
            if (this.editedRowData.expiryDate !== null && this.editedRowData.expiryDate !== "") {
                this.setState({ trainingExpiryDate: moment(this.editedRowData.expiryDate) });
            }
            else {
                this.setState({ trainingExpiryDate: "" });
            }
            if (this.editedRowData.verificationDate !== null && this.editedRowData.verificationDate !== "") {
                this.setState({ trainingVerificationDate: moment(this.editedRowData.verificationDate) });
            }
            else {
                this.setState({ trainingVerificationDate: moment() });
            }
        } else {
            this.setState({
                dateOfTraining: '',
                trainingExpiryDate: "",
                trainingVerificationDate: moment()
            });
        }
    }

    //Certificate Details: Dates
    fetchEffectiveDate = (date) => {
        this.setState({
            effectiveDate: date,
            recordChng: true
        });
        this.updatedData.effeciveDate = this.state.effectiveDate;
    }
    fetchCertificateExpiryDate = (date) => {
        this.setState({
            certificateExpiryDate: date,
            recordChng: true
        });
        this.updatedData.expiryDate = this.state.certificateExpiryDate;
    }
    fetchCertificateVerificationDate = (date) => {
        this.setState({ certificateVerificationDate: date });
        this.updatedData.verificationDate = this.state.certificateVerificationDate;
    }
    //Training Details: Dates
    fetchDateOfTraining = (date) => {
        this.setState({
            dateOfTraining: date,
            recordChng: true
        });
        this.updatedData.effeciveDate = this.state.dateOfTraining;
    }
    fetchTrainingExpiryDate = (date) => {
        this.setState({
            trainingExpiryDate: date,
            recordChng: true
        });
        this.updatedData.expiryDate = this.state.trainingExpiryDate;
    }
    fetchTrainingVerificationDate = (date) => {
        this.setState({ trainingVerificationDate: date });
        this.updatedData.verificationDate = this.state.trainingVerificationDate;
    }
    //Show Modals
    languageCapabilityShowModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isLanguageCapabilityShowModal: true,
                isLanguageCapabilityEditModal: false
            };
        });
        this.editedRowData = {};
    }
    certificateDetailsShowModal = (e) => {
        e.preventDefault();
        this.setState({ effectiveDate: '', certificateExpiryDate: '',verifiedBy:this.props.loggedInUser }); //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
        this.setState((state) => {
            return {
                isCertificateDetailsShowModal: true,
                isCertificateDetailsEditModal: false
            };
        });
        this.editedRowData = {};
    }

    commodityShowModal = () => {
        this.setState((state) => {
            return {
                isCommodityShowModal: true,
                isCommodityEditModal: false,
                equipmentRating: [],
                enableEquipment: true,
                disabledCommodity: false
            };
        });
    }
    trainingDetailsShowModal = () => {
        this.setState({ dateOfTraining: '', trainingExpiryDate: '', verifiedBy:this.props.loggedInUser }); //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
        this.setState((state) => {
            return {
                isTrainingDetailsShowModal: true,
                isTrainingDetailsEditModal: false
            };
        });
        this.editedRowData = {};
    }
    //Hide Modals
    languageCapabilityHideModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isLanguageCapabilityShowModal: false,
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    certificateDetailsHideModal = () => {
        this.setState((state) => {
            return {
                isCertificateDetailsShowModal: false,
                certificateVerificationDate: moment(),
                verifiedBy:"" //D218 (Changes for 12-02-2020 Failed ALM Doc)
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    cancelCertificateDetailsModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isCertificateDetailsShowModal: false,
                certificateVerificationDate: moment(),
                verifiedBy:"" //D218 (Changes for 12-02-2020 Failed ALM Doc)
            };
        });
        this.onclickModalCancel();
        this.updatedData = {};
        this.editedRowData = {};
        //this.props.actions.ClearDocumentUploadedDocument();
        //this.props.actions.RemoveDocUniqueCode();
        this.setState({ isShowVerifiedUpload: false });
    }

    commodityHideModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isCommodityShowModal: false
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
        this.updatedRating={};
    }
    trainingDetailsHideModal = () => {
        this.setState((state) => {
            return {
                isTrainingDetailsShowModal: false,
                trainingVerificationDate: moment(),
                verifiedBy:"" //D222 (Changes for 12-02-2020 Failed ALM Doc)
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    CancelTrainingDetailsModal = (e) => {       
        e.preventDefault();
        this.setState((state) => {
            return {
                isTrainingDetailsShowModal: false,
                trainingVerificationDate: moment(),
                verifiedBy:"" //D222 (Changes for 12-02-2020 Failed ALM Doc)
            };
        });
        this.onclickModalCancel();
        this.updatedData = {};
        this.editedRowData = {};      
        this.setState({ isShowVerifiedUpload: false }); 
        //this.props.actions.RemoveDocUniqueCode();
    }
    onclickModalCancel=()=>{ 
        if(!isEmpty(this.editedRowData)){
            const RevertDeletedDocument=[];
            !isEmpty(this.props.uploadDocument) && this.props.uploadDocument.map(uploadDoc=>{
                if(uploadDoc.recordStatus === 'D'){
                    uploadDoc.recordStatus = null; //def 848 doc highlight
                    RevertDeletedDocument.push(uploadDoc);
                }
            });
            this.props.actions.RevertDeletedDocument(RevertDeletedDocument).then(res=>{
                if(res){
                    this.trainingGridChild.refreshGrid();
                }
            });
        }else{
            this.props.actions.ClearDocumentUploadedDocument();
        }
    }
    uploadDocumentHandler = (e) => {
        const url = e.target.value;
        //const verfication_documentFile = Array.from(document.getElementById("verficationUploadFiles").files);
        //this.fileupload(verfication_documentFile);
        const filename = url.replace(/^.*[\\/]/, '');
        this.updatedData[e.target.name] = filename;
    }
    //Handling date
    handleDate = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isUIValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    } else {
                        this.setState({ inValidDateFormat: false });
                    }
                }
            }
        }
    }
    //Training Details Date Range Validator
    trainingDetailsDateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.TRAINING_INVALID_DATE_RANGE, 'warningToast');
            } else if(from == to){ //D687 (As per mail confirmation on 16-03-2020)
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.TRAINING_INVALID_DATE_EQUAL_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    //On data change handler
    onChangeHandler = (e) => {       
        this.updatedRating = [];
        if (e.target.id === 'ratingSelect') {  
            this.state.equipmentRating.map(item => {
                if (item.value === e.target.name) {
                    this.updatedRating.push({ id: item.id, value: item.value, rating: e.target.value });
                } else {
                    this.updatedRating.push({ id: item.id, value: item.value, rating: item.rating });
                }
            });
            this.setState({ equipmentRating: this.updatedRating });
            this.updatedData.equipmentKnowledge = this.updatedRating;
        }
        else {

            if (e.target.name === 'commodity'){
                this.props.actions.FetchEquipment(e.target.value);
                this.EquipmentClear.onChange([]); //D219
                if(!isEmpty(e.target.value)){
                    this.setState({ enableEquipment: false });
                } else{
                    this.setState({ enableEquipment: true });
                }
            }
        }
        if ((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM)) || (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))) {
            this.props.actions.IsRCRMUpdatedResourceCapability(true); 
        }
        if(e.target.name=='verificationStatus' && e.target.value === 'Verified'){           
            this.setState({ isShowVerifiedUpload: true,
                             //D218,D222 (Changes for 12-02-2020 Failed ALM Doc)
                        });
        }else if(e.target.name=='verificationStatus'){
            this.setState({ isShowVerifiedUpload: false,
                                         //D218,D222 (Changes for 12-02-2020 Failed ALM Doc)
             });
            //D573 -- start
            const isVerifiedDocupload = this.props.uploadDocument.filter(x => x.uploadFileRefType === 'verficationUpload' && x.recordStatus !== 'D');
            if(isVerifiedDocupload.length > 0){
                this.editedRowData["verificationDocuments"]=isVerifiedDocupload;
            }
            //D573 -- end
        }
        
        this.updatedData[e.target.name] = e.target.value;
        //D1172
        if(e.target.name == 'trainingName'){
            this.updatedData["iLearnID"] =  e.target.value;
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        } else if(e.target.name == 'certificationName'){
                this.updatedData["iLearnID"] =  e.target.value;
                this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        }
        //D1172 End
        if(!isEmpty(this.editedRowData)){
            this.updatedData.commodity = this.editedRowData.commodity;
         }
                         //D218,D222 (Changes for 12-02-2020 Failed ALM Doc)
         if(e.target.name === 'certificationName' || e.target.name === 'certificateRefId' ||
         e.target.name === 'trainingName' || e.target.name === "trainingRefId" || 
         e.target.name === "duration" || e.target.name ==='verificationStatus' ||
         e.target.name ==='description' || e.target.name ==='effeciveDate' ||
         e.target.name ==='expiryDate' || e.target.id ==='uploadFiles'){
            this.setState({ recordChng: true });
         }
    }
    //Adding Language details to the grid
    addLanguageDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.languageDetailsValidation(this.updatedData)) {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                const langArray = Object.assign([], this.props.languageDetails);
                langArray.push(this.updatedData);
                this.updateFromRc_RMLanguage.push(this.updatedData);
                this.props.actions.AddLanguageCapabilityDetails(langArray);
                this.languageCapabilityHideModal(e);
                this.updatedData = {};
            }
        }
        else this.languageDetailsValidation(this.updatedData);
    }
    //Validation for Language Details
    languageDetailsValidation = (data) => {
        if (data.language === undefined || data.language === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Language', 'warningToast');
            return false;
        }
        if (data.speakingCapabilityLevel === undefined || data.speakingCapabilityLevel === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Speaking Capability', 'warningToast');
            return false;
        }
        if (data.writingCapabilityLevel === undefined || data.writingCapabilityLevel === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Writing Capability', 'warningToast');
            return false;
        }
        if (data.comprehensionCapabilityLevel === undefined || data.comprehensionCapabilityLevel === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Comprehension Capability', 'warningToast');
            return false;
        }
        if (this.props.languageDetails.filter(x=>x.language=== data.language &&x.recordStatus!=="D" && x.id != data.id ).length > 0 ) {
            IntertekToaster(localConstant.techSpec.resourceCapability.DUPLICATE_LANGUAGE_CAPABILITIES , 'warningToast');
            return false;
        }

        return true;
    }
    //Certificate Details Date Range Validator
    certificateDetailsDateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.CERTIFICATE_DATE_RANGE_VALIDATOR, 'warningToast');
            } else if(from == to){//D687 (As per mail confirmation on 16-03-2020)
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.CERTIFICATE_DATE_RANGE_EQUAL_VALIDATOR, 'warningToast');
            }
        }
        return isInValidDate;
    }
    fileupload = (files) => {
        //e.preventDefault();
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        const docType = this.updatedData.documentType;
        const documentFile = files;
        if (documentFile.length > 0) {
            const newlyCreatedRecords = [];
            const filesToBeUpload = [];
            const failureFiles = [];
            const documentUniqueName = [];
            const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
            let visibleToTSStatus = false;
            const docTypeData = this.props.masterDocumentTypesData;
            for (let i = 0; i < docTypeData.length; i++) {
                if (docTypeData[i].name === docType) {
                    visibleToTSStatus = docTypeData[i].isTSVisible;
                }
            }
            documentFile.map(document => {
                if (parseInt(document.size / 1024) > configuration.fileLimit) {
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
                        moduleCode: "Ce",
                        requestedBy: this.props.loggedInUser,
                        moduleCodeReference: this.props.companyInfo.companyCode,
                    };
                    documentUniqueName.push(this.updatedData);
                    this.updatedData = {};
                    this.setState({
                        isDocumentModalOpen: false,
                        isDocumentModelAddView: false
                    });
                }
                this.props.actions.FetchDocumentUniqueName(documentUniqueName, filesToBeUpload)
                    .then(response => {
                        if (!isEmpty(response)) {
                            for (let i = 0; i < response.length; i++) {
                                if (!response[i].status) {
                                    this.updatedData = {
                                        documentName: response[i].fileName,
                                        documentType: docType,
                                        documentSize: parseInt(filesToBeUpload[i].size / 1024),
                                        createdOn: date,
                                        isVisibleToCustomer: customerVisibleStatus,
                                        isVisibleToTS: visibleToTSStatus,
                                        recordStatus: "N",
                                        id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                        documentUniqueName: response[i].uploadedFileName,
                                        modifiedBy: this.props.loggedInUser,
                                        moduleCode: response[i].moduleCode,
                                        moduleRefCode: response[i].moduleReferenceCode,
                                        status: "C",
                                        newlyAddedRecord: true
                                    };
                                    newlyCreatedRecords.push(this.updatedData);
                                    this.updatedData = {};
                                }
                            }
                            if (newlyCreatedRecords.length > 0) {
                                const addModal = {};
                                addModal.showModalPop = false;
                                this.props.actions.AddDocumentDetails(newlyCreatedRecords);
                            }
                        };
                    });
            }
            if (failureFiles.length > 0) {
                IntertekToaster(failureFiles.toString() + localConstant.companyDetails.Documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
            }
        }
        else {
            IntertekToaster(localConstant.companyDetails.Documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
        }
    }
    //Adding Certificate details to the grid
    addCertificateDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.certificateDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.effectiveDate !== "" && this.state.effectiveDate !== null)
                    fromDate = this.state.effectiveDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.certificateExpiryDate !== "" && this.state.certificateExpiryDate !== null)
                    toDate = this.state.certificateExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }
                if (!this.certificateDetailsDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["epin"] = this.props.epin !== undefined ? this.props.epin : 0;
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["recordType"] = "Ce";
                    this.updatedData["effeciveDate"] = fromDate;
                    this.updatedData["expiryDate"] = toDate;
                    this.updatedData["verificationDate"] = this.updatedData.verificationStatus==='Verified'?moment(this.state.certificateVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT):null;
                    if(this.props.isTSUserTypeCheck) //Sanity defect 91
                    this.updatedData["verifiedBy"] = "";
                    else
                    this.updatedData["verifiedBy"] = this.props.loggedInUser; //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
                    if(this.updatedData.verificationStatus==='Verified')
                    {
                    this.setState({ isShowVerifiedUpload: false });
                    this.updatedData["IsSaved"] = false;//D573
                    }
                    if(this.updatedData.verificationStatus==='Unverified' || this.updatedData.verificationStatus==='Pending Verification'){
                        this.updatedData["verifiedBy"] = "";
                    }
                    //this.updatedData["isExternal"] = this.updatedData.type === 'External' ? true : false;
                    this.updatedData["isExternal"] = this.updatedData.type? this.updatedData.type === 'External' ? true : false:'';
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && x.uploadFileRefType === 'uploadDocument'); //sanity def 98 fix
                    this.updatedData["verificationDocuments"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && x.uploadFileRefType === 'verficationUpload');//sanity def 98 fix
                    const certificate = Object.assign([], this.props.certificateDetails);
                    certificate.push(this.updatedData);
                    this.updateFromRc_RMCertificate.push(this.updatedData);
                    this.props.actions.AddCertificateDetails(certificate);
                    this.props.actions.UploadDocumentDetails();
                    this.certificateDetailsHideModal();
                    this.setState({
                        recordChng : false
                    });
                }
            }
        }
        else
            this.certificateDetailsValidation(this.updatedData);
    }
    //Validation for Certificate details
    certificateDetailsValidation = (data) => {
        const hiddenField = this.fieldsToHide();
        if (data.certificationName === undefined || data.certificationName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Certificate Name', 'warningToast');
            return false;
        }
        if (data.description === undefined || data.description === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Certificate Details', 'warningToast');
            return false;
        }
        if (hiddenField === false && data.verificationStatus === undefined || data.verificationStatus === "" || data.verificationStatus === null) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Verification Status', 'warningToast');
            return false;
        } //uncommented for D1183
        return true;
    }

    //Commodity Item Add and delete from Grid Common function
    addOrRemoveGridItems = (currentData, exgistingCommodityDetails, recordStatus) => {
        const finalCommodityArray = [];
        let commodityObjMap = {};
        let result = [];
        //Json Format Ungrouping format
        const commodityData = Object.assign([], exgistingCommodityDetails);
        currentData.equipmentKnowledge && currentData.equipmentKnowledge.map(obj => {
            commodityObjMap['epin'] = this.props.epin !== undefined ? this.props.epin : 0;
            commodityObjMap["recordStatus"] = recordStatus;
            commodityObjMap["id"] = recordStatus === 'N' ? Math.floor(Math.random() * (Math.pow(10, 5))) : obj.id;
            commodityObjMap['equipmentKnowledge'] = obj.value;
            commodityObjMap['equipmentKnowledgeLevel'] = obj.rating;
            commodityObjMap['commodity'] = currentData.commodity;
            finalCommodityArray.push(commodityObjMap);
            commodityObjMap = {};
        });
        if (recordStatus !== 'D') {
            result = {};
            result = commodityData.concat(finalCommodityArray);
        } else {
            const exgistCommodity = [];
            this.props.commodityDetails.map(obj => {
                if (currentData.commodity !== obj.commodity)
                    exgistCommodity.push(obj);
            });
            result = exgistCommodity.concat(finalCommodityArray);
        }

        return result;
    }
    //Adding commodity details to the grid
    addCommodityDetails = (e) => {
        e.preventDefault();
        if (!isEmpty(this.updatedData)) {
            if (this.commodityDetailsValidation(this.updatedData)) {
                const commodity = this.addOrRemoveGridItems(this.updatedData, this.props.commodityDetails, 'N');
                if(this.commodityDetailsDuplicateValidation(commodity)){ //D1053 changes (Ref 15-04-2020 ALM Doc)
                    this.props.actions.AddCommodityDetails(commodity);
                    this.commodityHideModal(e);
                }
            }
        } else {
            this.commodityDetailsValidation(this.updatedData);
        }
    }
    commodityDetailsDuplicateValidation = (data) =>{
        let duplicateData= this.props.commodityDetails.filter(value => {
            return data.some(item => item.equipmentKnowledge === value.equipmentKnowledge && value.commodity === item.commodity && item.recordStatus === 'N' && item.id != value.id);
        });
        duplicateData = duplicateData.filter(row => row.recordStatus !== 'D');
        if(duplicateData.length > 0){
            let duplicateEquipments= duplicateData.map(item =>{
                return item.equipmentKnowledge;
            }); 
            duplicateEquipments = duplicateEquipments.join(',');
            const message = StringFormat(localConstant.techSpec.resourceCapability.DUPLICATE_COMMODITY_EQUIP_KNOWLEDGE,duplicateEquipments,duplicateData[0].commodity);
            IntertekToaster(message, 'warningToast');
            return false;
        }
        return true; 
    }
    //Validation for Commodity Details
    commodityDetailsValidation = (data) => {
        if (data.commodity === undefined || data.commodity === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Commodity', 'warningToast');
            return false;
        }
        if (isEmpty(data.equipmentKnowledge)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Equipment Knowledge', 'warningToast');
            return false;
        }
        return true;
    }
    //Add Training Details
    addTrainingDetails = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.trainingDetailsValidation(this.updatedData)) {
                let fromDate = "";
                let toDate = "";
                if (this.state.dateOfTraining !== "" && this.state.dateOfTraining !== null)
                    fromDate = this.state.dateOfTraining.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.trainingExpiryDate !== "" && this.state.trainingExpiryDate !== null)
                    toDate = this.state.trainingExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                    return false;
                }
                if (!this.trainingDetailsDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["recordType"] = "Tr";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["epin"] = this.props.epin !== null ? this.props.epin : 0;
                    this.updatedData["effeciveDate"] = fromDate;
                    this.updatedData["expiryDate"] = toDate;
                    this.updatedData["verificationDate"] = this.updatedData.verificationStatus==='Verified'?moment(this.state.trainingVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT):null;
                    if(this.props.isTSUserTypeCheck) //Sanity defect 91
                        this.updatedData["verifiedBy"] = "";
                    else
                        this.updatedData["verifiedBy"] = this.props.loggedInUser; //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
                    if(this.updatedData.verificationStatus==='Verified')
                    {
                        this.setState({ isShowVerifiedUpload: false });
                        this.updatedData["IsSaved"] = false;//D573
                    }
                    if(this.updatedData.verificationStatus==='Unverified' || this.updatedData.verificationStatus==='Pending Verification'){
                        this.updatedData["verifiedBy"] = "";
                    }
                    this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && x.uploadFileRefType === 'uploadDocument' );//sanity def 98 fix
                    this.updatedData["verificationDocuments"] = this.props.uploadDocument.filter(x => x.recordStatus === "N" && x.uploadFileRefType === 'verficationUpload' );//sanity def 98 fix
                    //this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus !== 'D');
                    // this.updatedData["isExternal"] = this.updatedData.type === 'External' ? true : false;
                    this.updatedData["isExternal"] = this.updatedData.type? this.updatedData.type=== 'External' ? true : false:'';//
                    const training = Object.assign([], this.props.trainingDetails);
                    training.push(this.updatedData);
                    this.updateFromRc_RMTraining.push(this.updatedData);
                    this.props.actions.AddTrainingDetails(training);
                    this.props.actions.UploadDocumentDetails();
                    this.trainingDetailsHideModal();
                    this.setState({
                        recordChng : false
                    });
                }
            }
        }
        else this.trainingDetailsValidation(this.updatedData);
    }
    //Validation for Certificate details
    trainingDetailsValidation = (data) => {
        if (data.trainingName === undefined || data.trainingName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Training Name', 'warningToast');
            return false;
        }
        if (data.duration === undefined || data.duration === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Duration', 'warningToast');
            return false;
        }
        return true;
    }
    //Updating the edited Language data to the grid
    updateLanguageDetails = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.languageDetailsValidation(combinedData)) {
            if (this.editedRowData.recordStatus !== "N")
                this.updatedData["recordStatus"] = "M";

            this.updateFromRc_RMLanguage.push(this.updatedData);
            this.props.actions.UpdateLanguageDetails(this.editedRowData, this.updatedData);
            this.languageCapabilityHideModal(e);
        }
    }

    //Updating the edited Certificate details to the grid
    updateCertificateDetails = (e) => {
        e.preventDefault();
        const docUpl = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_Certificate'));
        const docVer = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'verficationUpload' || x.documentType === 'TS_CertVerification'));
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        let docDelete = '';
        if(this.editedRowData.documents !=null){
        docDelete = this.editedRowData.documents.filter(x => x.recordStatus === "D");
        }
        let docChk = false;
        if (combinedData.documents !== null) {
            const docChk1 = ((docUpl.length == 1 && combinedData.documents.length == 1) && docUpl[0].documentName === combinedData.documents[0].documentName) ||
                (docUpl.length == 0 && combinedData.documents.length == 0) || (docUpl.length == 0 && combinedData.documents.length == 1);
            const docChk3 = (docUpl.length == 1 && combinedData.documents.length == 0);
            const docChk5 = (docUpl.length == 1 && combinedData.documents.length == 1) && docUpl[0].documentName !== combinedData.documents[0].documentName;
            if (docChk1 && !docChk3 && !docChk5) {
                docChk = true;
            }
        }
         if( (!this.state.recordChng && this.props.isTSUserTypeCheck && docUpl.length == 0 && !docChk)
         ||(docUpl.length >0 && docChk)) {
            this.exitPopup();
        }
        if (!this.state.recordChng && combinedData.verificationStatus === 'Verified' && (this.editedRowData.recordStatus !== "N" && 
        (combinedData.verificationDocuments.length > 0 && combinedData.verificationDocuments[0].recordStatus !== 'D'))){
            this.exitPopup(); 
        }
        if(!this.state.recordChng && docUpl && docUpl.length == 0 && docDelete && docDelete.length == 0){
            this.exitPopup();
        }
        else if (this.certificateDetailsValidation(combinedData)) {
            let fromDate = "";
            let toDate = "";
            if (this.state.effectiveDate !== "" && this.state.effectiveDate !== null)
                fromDate = this.state.effectiveDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.certificateExpiryDate !== "" && this.state.certificateExpiryDate !== null)
                toDate = this.state.certificateExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }
            if (!this.certificateDetailsDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
                this.updatedData["recordType"] = "Ce";
                this.updatedData["effeciveDate"] = fromDate;
                this.updatedData["expiryDate"] = toDate;
                if(combinedData.verificationStatus === 'Verified' && !this.props.isTSUserTypeCheck){
                    this.updatedData["verificationDate"] = moment(this.state.certificateVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                }
                // else if(combinedData.verificationStatus !== 'Verified'){
                //     this.updatedData["verificationDate"] = '';
                // }
                if (this.props.isTSUserTypeCheck) {
                    this.updatedData["verifiedBy"] = "";
                }
                else {
                    this.updatedData["verifiedBy"] = this.props.loggedInUser;
                }
                //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
                if (combinedData.verificationStatus === 'Verified') //D218 (Changes for 12-02-2020 Failed ALM Doc)
                {
                    this.setState({ isShowVerifiedUpload: false });
                    this.updatedData["IsSaved"] = false;//D573 
                }
                if (combinedData.verificationStatus === 'Unverified' || combinedData.verificationStatus === 'Pending Verification') {
                    this.updatedData["verifiedBy"] = "";
                    if (!this.props.isTSUserTypeCheck) {
                        this.updatedData["verificationDate"] =""; //moment(this.state.certificateVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                    }
                }
                if (docUpl && docUpl.length > 0 && combinedData.documents
                    && combinedData.documents.length > 0 && (combinedData.documents[0].recordStatus !== 'D'
                    && combinedData.documents[0].recordStatus !== 'M')) {
                    this.updatedData["documents"] = combinedData.documents[0];
                }
                else if (docUpl && docUpl.length > 0) {
                    this.updatedData["documents"] = docUpl;
                    if(this.editedRowData.documents && this.editedRowData.documents.length > 0 && this.editedRowData.documents[0].recordStatus === 'D')
                    this.updatedData.documents.push(this.editedRowData.documents[0]);
                }
                else {
                    this.updatedData["documents"] = docUpl;
                }
                this.updatedData["verificationDocuments"] = docVer && docVer.length>0 ? docVer : '';//this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'verficationUpload' || x.documentType === 'TS_CertVerification'));//sanity def 98 fix
                //    if(docData.length !== 0 && docVerify.length !== 0){
                //     this.updatedData["documents"] = docData;//sanity def 98 fix
                //     this.updatedData["verificationDocuments"] = docVerify;
                //    }
                //    else{
                //     this.updatedData["documents"] = null;//sanity def 98 fix
                //     this.updatedData["verificationDocuments"] = null;
                //    }
                // this.updatedData["isExternal"] = this.updatedData.type === 'External' ? true : this.updatedData.type === 'Internal' ? false : this.editedRowData.isExternal;
                this.updatedData["isExternal"] = combinedData.type ? combinedData.type === 'External' ? true : combinedData.type === 'Internal' ? false : this.editedRowData.isExternal : ''; //D218 (Changes for 12-02-2020 Failed ALM Doc)
                this.updateFromRc_RMCertificate.push(this.updatedData);
                this.props.actions.UpdateCertificateDetails(this.updatedData, this.editedRowData);
                this.props.actions.UploadDocumentDetails();
                this.certificateDetailsHideModal();
                this.editedRowData = {};
                this.updatedData = {};
                this.setState({
                    recordChng: false
                });
            }
        }
    }
    exitPopup=()=>{
        this.setState((state) => {
            return {
                isCertificateDetailsShowModal: false,
                certificateVerificationDate: moment(),
                verifiedBy: ""
            };
        });
        this.onclickModalCancel();
        this.updatedData = {};
        this.editedRowData = {};
        this.setState({ isShowVerifiedUpload: false });
    }
    //Get filtering Exgisting Commodity removed item and Add items
    filterExgistingCommodity = (a, b) => {
        const valuesA = a.reduce((a, { value }) => Object.assign(a, { [value]: value }), {});
        const valuesB = b.reduce((a, { value }) => Object.assign(a, { [value]: value }), {});
        const result = [ ...a.filter(({ value }) => !valuesB[value]), ...b.filter(({ value }) => !valuesA[value]) ];
        return result;
    }
    //Updating the edited commodity data to the grid
    updateCommodityDetails = (e) => {
        e.preventDefault();
        const commodityfinalUpdate = [];
        const finalCommodityArray = [];
        const itemTobeExgist = [];
        const itemTobeAdded = [];
        let combinedRating = [];
        let itemTobeRmoved=[];
        let notMatchItem =[];
        //Get Exgisting Rating In Edited Row
        if(this.commodityDetailsValidation(this.updatedData)){
        if (!isEmpty(this.updatedRating)) {
            for (let i = 0; i < this.editedRowData.equipmentKnowledge.length; i++) {
                this.updatedRating.map(obj => {
                    if (obj.value === this.editedRowData.equipmentKnowledge[i].value && obj.rating === this.editedRowData.equipmentKnowledge[i].rating) {
                        itemTobeExgist.push({
                            id: this.editedRowData.equipmentKnowledge[i].id,
                            value: this.editedRowData.equipmentKnowledge[i].value,
                            rating: Number(this.editedRowData.equipmentKnowledge[i].rating),
                            recordStatus: this.editedRowData.recordStatus
                        });
                    } else if (obj.value === this.editedRowData.equipmentKnowledge[i].value && obj.rating !== this.editedRowData.equipmentKnowledge[i].rating) {
                        itemTobeExgist.push({
                            id: this.editedRowData.equipmentKnowledge[i].id,
                            value: this.editedRowData.equipmentKnowledge[i].value,
                            rating: Number(obj.rating),
                            recordStatus: this.editedRowData.recordStatus
                        });
                    }
                });
            };
        //Get Removed Item
         itemTobeRmoved = this.filterExgistingCommodity(this.editedRowData.equipmentKnowledge, itemTobeExgist);
        //Get Add New Item
         notMatchItem = this.filterExgistingCommodity(this.editedRowData.equipmentKnowledge, this.updatedRating);
        };
        //Get New Item              
        if (!isEmpty(notMatchItem)) {
            for (let j = 0; j < this.updatedRating.length; j++) {
                notMatchItem.map(obj => {
                    if (obj.value === this.updatedRating[j].value) {
                        itemTobeAdded.push({
                            id: this.updatedRating[j].id,
                            value: this.updatedRating[j].value,
                            rating: Number(this.updatedRating[j].rating)
                        });
                    }
                });
            }
        };
        const combined = itemTobeExgist.concat(itemTobeRmoved);
        combinedRating = combined.concat(itemTobeAdded);
        combinedRating && combinedRating.map(item => {
            this.updatedData['equipmentKnowledge'] = item.value;
            this.updatedData['equipmentKnowledgeLevel'] = Number(item.rating);
            this.updatedData["id"] = item.id;
            this.updatedData['epin'] = this.editedRowData.epin;
            this.updatedData['commodity'] = this.editedRowData.commodity;

            for (let e = 0; e < this.editedRowData.equipmentKnowledge.length; e++) {
                if (item.value === this.editedRowData.equipmentKnowledge[e].value) {
                    if (this.editedRowData.equipmentKnowledge[e].rating === Number(item.rating)) {
                        this.updatedData['recordStatus'] = null;
                    } else {
                        this.updatedData['recordStatus'] = 'M';
                    }
                }
            }
            for (let r = 0; r < itemTobeRmoved.length; r++) {
                if (item.value === itemTobeRmoved[r].value) {
                    this.updatedData['recordStatus'] = 'D';
                }
                if (itemTobeRmoved[r].id === this.props.commodityDetails[r].id) {
                    this.updatedData['updateCount'] = this.props.commodityDetails[r].updateCount;
                }
            }
            for (let a = 0; a < itemTobeAdded.length; a++) {
                if (item.value === itemTobeAdded[a].value) {
                    this.updatedData['recordStatus'] = 'N';
                }
            }

            finalCommodityArray.push(this.updatedData);
            this.updatedData = {};
        });
        this.props.commodityDetails.map(obj => {
            if (this.editedRowData.commodity !== obj.commodity) {
                finalCommodityArray.push(obj);
            }
            for (let f = 0; f < finalCommodityArray.length; f++) {
                if (finalCommodityArray[f].id === obj.id && finalCommodityArray[f].recordStatus !== 'N') {
                    finalCommodityArray[f]['updateCount'] = obj.updateCount;
                }
                if(finalCommodityArray[f].id === obj.id && finalCommodityArray[f].recordStatus === null){
                    finalCommodityArray[f]['recordStatus'] = obj.recordStatus;
                }
            }
        });

        // if(isEmpty(finalCommodityArray.equipmentKnowledge.filter(x=>x.recordStatus !== "D"))){
        //     IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Equipment Knowledge', 'warningToast');
        //     return false;
        // }
        // else{
            /* changes to resolve invalid CommodityEqupmentKnowledge id is provided*/
                // finalCommodityArray.forEach((row,index) => {
                //     if (row.recordStatus === "D" && isUndefined(row.updateCount)) {
                //         finalCommodityArray.splice(index, 1);
                //     }
                // });
        this.updateFromRc_RMCommodity.push(finalCommodityArray);
        this.props.actions.UpdateCommodityDetails(finalCommodityArray);
        this.commodityHideModal(e);
        // }
        }
    };

    //Updating the edited Training details to the grid
    updateTrainingDetails = (e) => {
        e.preventDefault();
        const docUpl = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'uploadDocument' || x.documentType === 'TS_Training'));
        const docVer = this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'verficationUpload' || x.documentType === 'TS_TrainingVerification'));
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        let docDelete = '';
        if(this.editedRowData.documents !=null){
        docDelete = this.editedRowData.documents.filter(x => x.recordStatus === "D");
        }
        let docChk = false;
        if (combinedData.documents !== null) {
            const docChk1 = ((docUpl.length == 1 && combinedData.documents.length == 1) && docUpl[0].documentName === combinedData.documents[0].documentName) ||
                (docUpl.length == 0 && combinedData.documents.length == 0) || (docUpl.length == 0 && combinedData.documents.length == 1);
            const docChk3 = (docUpl.length == 1 && combinedData.documents.length == 0);
            const docChk5 = (docUpl.length == 1 && combinedData.documents.length == 1) && docUpl[0].documentName !== combinedData.documents[0].documentName;
            if (docChk1 && !docChk3 && !docChk5) {
                docChk = true;
            }
        }
        if ((!this.state.recordChng && this.props.isTSUserTypeCheck && docUpl.length == 0 && !docChk)
            || (docUpl.length > 0 && docChk)) {
            this.exitTrainingPopup();
        }
        if (!this.state.recordChng && combinedData.verificationStatus === 'Verified' && (this.editedRowData.recordStatus !== "N" && 
        (combinedData.verificationDocuments.length > 0 && combinedData.verificationDocuments[0].recordStatus !== 'D'))){
            this.exitTrainingPopup();
        }
        if(!this.state.recordChng && docUpl && docUpl.length == 0 && docDelete && docDelete.length == 0){
            this.exitTrainingPopup();
        }
        else if (this.trainingDetailsValidation(combinedData)) {
            let fromDate = "";
            let toDate = "";
            if (this.state.dateOfTraining !== "" && this.state.dateOfTraining !== null)
                fromDate = this.state.dateOfTraining.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.trainingExpiryDate !== "" && this.state.trainingExpiryDate !== null)
                toDate = this.state.trainingExpiryDate.format(localConstant.techSpec.common.DATE_FORMAT);
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                return false;
            }

            if (!this.trainingDetailsDateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["recordType"] = "Tr";
                this.updatedData["effeciveDate"] = fromDate;
                this.updatedData["expiryDate"] = toDate;
                if(combinedData.verificationStatus === 'Verified' && !this.props.isTSUserTypeCheck){
                    this.updatedData["verificationDate"] = moment(this.state.trainingVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                }
                if (combinedData.verificationStatus === 'Unverified' || combinedData.verificationStatus === 'Pending Verification') {
                    this.updatedData["verifiedBy"] = "";
                    if (!this.props.isTSUserTypeCheck) {
                        this.updatedData["verificationDate"] =""; //moment(this.state.certificateVerificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                    }
                }
                //this.updatedData["verifiedBy"] = this.props.loggedInUser; //D1130 Ref mail BUG_1130_D1130-RESOURCE-Data Migration Issues-29-5-20 
                if (combinedData.verificationStatus === 'Verified') //D222 (Changes for 12-02-2020 Failed ALM Doc)
                {
                    this.setState({ isShowVerifiedUpload: false });
                    this.updatedData["IsSaved"] = false;//D573 
                }
                if (this.props.isTSUserTypeCheck) {
                    this.updatedData["verifiedBy"] = "";
                }
                else {
                    this.updatedData["verifiedBy"] = this.props.loggedInUser;
                }
                if (docUpl && docUpl.length > 0 && combinedData.documents
                    && combinedData.documents.length > 0 && (combinedData.documents[0].recordStatus !== 'D'
                    && combinedData.documents[0].recordStatus !== 'M')) {
                    this.updatedData["documents"] = combinedData.documents[0];
                }
                else if (docUpl && docUpl.length > 0) {
                    this.updatedData["documents"] = docUpl;
                    if(this.editedRowData.documents && this.editedRowData.documents.length > 0 && this.editedRowData.documents[0].recordStatus === 'D')
                    this.updatedData.documents.push(this.editedRowData.documents[0]);
                }
                else {
                    this.updatedData["documents"] = docUpl;
                }
                this.updatedData["verificationDocuments"] = docVer && docVer.length>0 ? docVer : '';//this.props.uploadDocument.filter(x => x.recordStatus === "N" && (x.uploadFileRefType === 'verficationUpload' || x.documentType === 'TS_TrainingVerification'));  //sanity def 98 fix       
                //this.updatedData["documents"] = this.props.uploadDocument.filter(x => x.recordStatus !== 'D');
                //this.updatedData["isExternal"] = this.updatedData.type === 'External' ? true : this.updatedData.type === 'Internal' ? false : this.editedRowData.isExternal;
                this.updatedData["isExternal"] = combinedData.type ? combinedData.type === 'External' ? true : combinedData.type === 'Internal' ? false : this.editedRowData.isExternal : '';//D222 (Changes for 12-02-2020 Failed ALM Doc)
                this.updateFromRc_RMTraining.push(this.updatedData);
                this.props.actions.UpdateTrainingDetails(this.editedRowData, this.updatedData);
                this.props.actions.UploadDocumentDetails();
                this.trainingDetailsHideModal();
                this.setState({
                    recordChng: false
                });
            }
        }

    }
    exitTrainingPopup=()=>{
        this.setState((state) => {
            return {
                isTrainingDetailsShowModal: false,
                trainingVerificationDate: moment(),
                verifiedBy:"" 
            };
        });
        this.onclickModalCancel();
        this.updatedData = {};
        this.editedRowData = {};      
        this.setState({ isShowVerifiedUpload: false }); 
    }
    //Language Capability: Showing modal popup for delete
    deleteLanguageCapability = () => {
        const selectedRecords = this.languageGridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.RESOURCE_CAPABILITY_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                isOpen: 'true',
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedLanguageCapability,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.languageCapabilityRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Language Capability: Deleting the grid detail
    deleteSelectedLanguageCapability = () => {
        const selectedData = this.languageGridChild.getSelectedRows();
        this.languageGridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteLanguageDetails(selectedData);
        this.props.actions.HideModal();
    }
    languageCapabilityRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //Certificate details: Showing modal popup for delete
    deleteCertificateDetails = () => {
        const selectedRecords = this.certificateGridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.RESOURCE_CAPABILITY_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                isOpen: 'true',
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedCertificateDetails,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.certificateDetailsRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Certificate Details: Deleting the grid detail
    deleteSelectedCertificateDetails = () => {
        const selectedData = this.certificateGridChild.getSelectedRows();
        this.certificateGridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteCertificateDetails(selectedData);
        this.props.actions.HideModal();
    }
    certificateDetailsRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //Commodity: Showing modal popup for delete
    deleteCommodity = () => {
        const selectedRecords = this.commodityGridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.RESOURCE_CAPABILITY_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                isOpen: 'true',
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedCommodityDetails,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.commodityDetailsRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Commodity: Deleting the grid detail
    deleteSelectedCommodityDetails = () => {
        const selectedData = this.commodityGridChild.getSelectedRows();
        this.commodityGridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteCommodityDetails(selectedData);
        this.props.actions.HideModal();
    }
    commodityDetailsRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //Training details: Showing modal popup for delete
    deleteTrainingDetails = () => {
        const selectedRecords = this.trainingGridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.RESOURCE_CAPABILITY_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                isOpen: 'true',
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedTrainingDetails,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.trainingDetailsRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Training details: Deleting the grid detail
    deleteSelectedTrainingDetails = () => {
        const selectedData = this.trainingGridChild.getSelectedRows();
        this.trainingGridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteTrainingDetails(selectedData);
        this.props.actions.HideModal();
    }
    trainingDetailsRejectHandler = () => {
        this.props.actions.HideModal();
    }

    commodityDetailsArrayMap = () => {
        this.commodityArray = [];
        let equipmentKnowledge = [];
        let commodityObjectmap = {};
        if (!isEmpty(this.props.commodityDetails)) {
            this.props.commodity.map(obj => {
                this.props.commodityDetails.map((com, i) => {
                    if (obj.name === com.commodity && com.recordStatus !== 'D') {
                        commodityObjectmap["id"] = com.id;
                        commodityObjectmap['epin'] = this.props.epin !== undefined ? this.props.epin : 0;
                        commodityObjectmap["recordStatus"] = com.recordStatus;
                        commodityObjectmap['commodity'] = com.commodity;
                        equipmentKnowledge.push({
                            id: com.id,
                            value: com.equipmentKnowledge, rating: com.equipmentKnowledgeLevel
                        });
                    }

                });
                if (!isEmpty(equipmentKnowledge)) {
                    commodityObjectmap['equipmentKnowledge'] = equipmentKnowledge;
                    equipmentKnowledge = [];
                }
                if (!isEmpty(commodityObjectmap)) {
                    this.commodityArray.push(commodityObjectmap);
                }

                commodityObjectmap = {};
            });
        }

    }
    certificateCancelUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.certificateDetails);
               // this.educationalSummaryDocument=filterdData[0];
            }
        }
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
                if (newDoc.length > 0) {
                    this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // const req = this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.certificateDetails).then(res=>{
        //     if(res){
        //         this.certificateGridChild.refreshGrid();
        //     }
        // });
    }
    verificationCertificateCancelUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.verificationDocuments)) {
            const filterdData = this.editedRowData.verificationDocuments.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.certificateDetails, 'verficationDocument');
            }
        }
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
                if (newDoc.length > 0) {
                    this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // const req = this.editedRowData.verificationDocuments[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.certificateDetails, 'verficationDocument').then(res=>{
        //     if(res){
        //         this.certificateGridChild.refreshGrid();
        //     }
        // });
    }
    traningVerificationCancelUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.verificationDocuments)) {
            const filterdData = this.editedRowData.verificationDocuments.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.trainingDetails, 'verficationDocument');
            }
        }
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
                if (newDoc.length > 0) {
                    this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // const req = this.editedRowData.verificationDocuments[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.trainingDetails, 'verficationDocument').then(res=>{
        //     if(res){
        //         this.trainingGridChild.refreshGrid();
        //     }
        // });
    }

    traningCancelUploadedDocument = (e,documentUniqueName) => {
        // def 653 #14 
        if ( !isEmptyOrUndefine(this.editedRowData) && Array.isArray(this.editedRowData.documents)) {
            const filterdData = this.editedRowData.documents.filter(x => x.documentUniqueName === documentUniqueName);
            if (filterdData.length > 0) {
                this.props.actions.RemoveDocumentDetails(filterdData[0], this.props.trainingDetails);
            }
        }
        if ( Array.isArray(this.props.uploadDocument) && this.props.uploadDocument.length>0) {
            const newDoc = this.props.uploadDocument.filter(x => x.documentUniqueName === documentUniqueName && (x.recordStatus === 'M' || x.recordStatus === 'N'));//sanity def 98 fix
                if (newDoc.length > 0) {
                    this.props.actions.RemoveDocumentDetails(newDoc[0]); 
            }
        }
        // const req = this.editedRowData.documents[0];
        // this.props.actions.RemoveDocumentDetails(req, this.props.trainingDetails).then(res=>{
        //     if(res){
        //         this.trainingGridChild.refreshGrid();
        //     }
        // });
    }

    /** Language draft field comparision */ 
    languageDraftData = (originalData) => { 
        let work = 0;
        if (originalData && originalData.data && originalData.data.language && isEmptyOrUndefine(originalData.data.recordStatus)) {
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                work = this.props.draftLanguageDetails.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result ){ //D556
                        return true;   //D556
                    }
                    return false;
                } else if (!isEmpty(this.updateFromRc_RMLanguage)) {
                    work = this.updateFromRc_RMLanguage.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const currentResult = compareObjects(work[0], originalData.data);
                        this.updateFromRc_RMLanguage = [];
                        return !currentResult;
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedDraftLanguageDetails.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                            return compareDataResult;
                        }
                    }
                }
                else if (!isEmpty(this.props.selectedDraftLanguageDetails)) {
                    work = this.props.selectedDraftLanguageDetails.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        return compareDataResult;
                    }
                } else {
                    return false;
                }
            } else {
                this.updateFromRc_RMLanguage = [];
                return false;
            }
        }
        return false;
    }

    /** certification draft field comparision */ 
    certificationDraftData = (originalData) => { 
        let work = 0;
        if (originalData && originalData.data && originalData.data.certificationName && isEmptyOrUndefine(originalData.data.recordStatus)) {
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                if(this.props.draftDataToCompare.technicalSpecialistCertification.length==0)
                {
                    return true;   //D556  
                }
                work = this.props.draftCertificationDetails.filter(x => x.id === originalData.data.id);
                if (work.length > 0 ){ //D556
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result) {
                        return true;   //D556
                    }
                    return false;
                } else if (this.updateFromRc_RMCertificate && !isEmpty(this.updateFromRc_RMCertificate)) {
                    work = this.updateFromRc_RMCertificate.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        return !compareObjects(work[0], originalData.data); 
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedDraftCertificationDetails.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            return compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        }
                    }
                }
                else if (!isEmpty(this.props.selectedDraftCertificationDetails)) {
                    work = this.props.selectedDraftCertificationDetails.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        return compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));// def 774 #23
                    }
                } else {
                    return false;
                }
            } else {
                this.updateFromRc_RMCertificate = [];
                return false;
            }
        }
        return false;
    }

    /** equipment draft field comparision */ 
    equipmentDraftData = (originalData) => { 
        let work = 0;
        if (originalData && originalData.data && originalData.data.id && isEmptyOrUndefine(originalData.data.recordStatus)) {
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) { 
                const commodity = this.props.commodityDetails.filter(x => x.id === originalData.data.id);
                const equipment = this.props.draftEquipmentDetails.filter(x => x.id === originalData.data.id);
                if (equipment.length > 0) {
                   if (commodity[0].recordStatus != 'M') {
                        const result = compareObjects(this.excludeProperty(equipment[0]), this.excludeProperty(commodity[0]));
                        if (!result) {  //D556 
                            return true;   //D556 
                        }
                    }
                    return false;
                } else if (this.updateFromRc_RMCommodity && !isEmpty(this.updateFromRc_RMCommodity)) {
                    work = this.updateFromRc_RMCommodity.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const currentResult = compareObjects(work[0], commodity[0]);
                        //this.updateFromRC_RM=[];
                        return !currentResult;
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedDraftEquipmentDetails.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            return compareObjects(this.excludeProperty(work[0]), this.excludeProperty(commodity[0]));
                        }
                    }
                } else if (!isEmpty(this.props.selectedDraftEquipmentDetails)) {
                    work = this.props.selectedDraftEquipmentDetails.filter(x => x.id === originalData.data.id);
                  return (work.length > 0) ;
                    // if (work.length > 0) {
                    //     return !compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data)); 
                    // }
                } else {
                    return false;
                }
            } else {
                this.updateFromRc_RMCommodity = [];
                return false;
            }
        }
        return false;
    }

    /** training draft field comparision */ 
    trainingDraftData = (originalData) => { 
        let work = 0;
        if (originalData && originalData.data && originalData.data.trainingName && isEmptyOrUndefine(originalData.data.recordStatus)) {
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                work = this.props.drafttrainingDetails.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result) {
                        return true;   //D556
                    }
                    return false;
                } else if (!isEmpty(this.updateFromRc_RMTraining)) {
                    work = this.updateFromRc_RMTraining.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        return !compareObjects(work[0], originalData.data);  
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedDrafttrainingDetails.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            return compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        }
                    }
                } else if (!isEmpty(this.props.selectedDrafttrainingDetails)) {
                    work = this.props.selectedDrafttrainingDetails.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        return compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    }
                } else {
                    return false;
                }
            } else {
                this.updateFromRc_RMTraining = [];
                return false;
            }
        }
        return false;
    }
    excludeProperty = (object) => {
        delete object.recordStatus;
        delete object.lastModification;
       // delete object.updateCount;
        delete object.modifiedBy;
        delete object.type;
        if(object.documents && object.documents.length === 0){
            object.documents = null;
        }
        if(object.verificationDocuments && object.verificationDocuments.length === 0){
            object.verificationDocuments = null;
        }
        return object;
    }
    /** Compare field value with draft */
    // compareDraftData = (draftValue, fieldValue) => {
    //     if (!isEmpty(this.props.draftDataToCompare)) {
    //         if (draftValue === undefined && !required(fieldValue)) {
    //             return true;
    //         }
    //         else if (draftValue !== undefined && draftValue != fieldValue) {
    //             return true;
    //         }
    //     }
    //     return false;
    // };
    compareDraftMultiselectCodeStandard = (draftValue, fieldValue) => { 
        const _this = this;
        let isMatch = false;
        if (!_this.props.isRCRMUpdateCodeStandard && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
            if (!isEmpty(_this.props.draftDataToCompare) ) {
                if (!isEmpty(draftValue) || !isEmpty(fieldValue)) {//Changes for D556
                    if(isEmpty(draftValue) || isEmpty(fieldValue)){
                        isMatch = true;
                     } 
                     else{
                        const result=fieldValue.filter(fieldValueObj =>{
                            return !draftValue.some(draftValueObj =>{
                                return fieldValueObj.id === draftValueObj.id;
                            });
                        });
                        if(!isEmpty(result)){
                            isMatch = true;
                        }
                     }
                }
                else {
                    return isMatch = false;
                }
            }
        }
        return isMatch;
    };
    compareDraftMultiselectCompKnowledge = (draftValue, fieldValue) => {
        const _this = this;
        let isMatch = false;
        if (!_this.props.isRCRMUpdateCompKnowledge && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
            if (!isEmpty(_this.props.draftDataToCompare)) {
                 if (!isEmpty(draftValue) || !isEmpty(fieldValue)) { //Changes for D556
                    if(isEmpty(draftValue) || isEmpty(fieldValue)){
                        isMatch = true;
                    } else{
                        const result=fieldValue.filter(fieldValueObj =>{
                            return !draftValue.some(draftValueObj =>{
                                return fieldValueObj.id === draftValueObj.id;
                            });
                        });
                        if(!isEmpty(result)){
                            isMatch = true;
                        }
                    }
                }
                else {
                    return isMatch = false;
                }
            }
        }
        return isMatch;
    };

    compareDraftData = (draftValue, selectedDraftValue, fieldValue,recordStatus) => { 
        const _this = this;
        if ((recordStatus !== null && recordStatus !== undefined ) || fieldValue==undefined) {
            return false;
        }
        else if (!isEmpty(_this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
            if (!_this.props.isRCRMUpdate) {
                if (draftValue === undefined && !required(fieldValue)) {
                    return true;
                }
                else if (draftValue !== undefined && draftValue !== fieldValue && draftValue !== null) { //D556
                    return true;
                }
                else if ( (draftValue === undefined || draftValue === null) && !required(fieldValue) && draftValue !== fieldValue) {
                    return true;
                }
                return false;
            } else if (_this.props.isRCRMUpdate) {
                //RC RM While modifing Value Not showing Highlight 
                if (draftValue === undefined && !required(fieldValue)) {
                    return false;
                }
                else if (draftValue !== undefined && selectedDraftValue === draftValue && draftValue !== fieldValue) {
                    return (selectedDraftValue !== fieldValue);
                }
                else if (draftValue !== undefined && draftValue !== fieldValue) {
                    return (selectedDraftValue === fieldValue);
                }
                return false;
            }
        }
        return false;
    };

    fetchIntertekWorkHistoryReport=()=>{ 
         const epin =this.props.technicalSpecialistInfo && this.props.technicalSpecialistInfo.epin;
         this.props.actions.FetchIntertekWorkHistoryReport(epin);
    };
 
    render() {
        const { languageDetails, certificateDetails, commodityDetails, trainingDetails,intertekWorkHistoryReport,
            codeStandard, computerKnowledge, languages, certificates, commodity, equipment, training, selectedComputerKnowledge, selectedCodeStanded, interactionMode, draftDataToCompare,uploadDocument } = this.props;
        this.commodityDetailsArrayMap(); 
        let isVerified = false;
        if(this.editedRowData){
            isVerified = verifiedStatusDisable({ obj:this.editedRowData });
        }
        this.certificateEdit=[
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCertificateDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button",
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateCertificateDetails,
                btnClass: 'btn-small',
                showbtn: true,
                disabled: isVerified
            }
        ];
        this.trainingEdit=[
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.CancelTrainingDetailsModal,
                btnClass: 'btn-small mr-2',
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateTrainingDetails,
                btnClass: 'btn-small',
                showbtn: true,
                disabled: isVerified
            }
        ];

        return (
            <Fragment>
                <CustomModal />
                {this.state.isLanguageCapabilityShowModal &&
                    <Modal
                        modalClass="techSpecModal"
                        title={this.state.isLanguageCapabilityEditModal ? localConstant.techSpec.resourceCapability.EDIT_LANGUAGE_CAPABILITIES :
                            localConstant.techSpec.resourceCapability.ADD_LANGUAGE_CAPABILITIES}
                        buttons={this.state.isLanguageCapabilityEditModal ? this.languageEditButtons : this.languageAddButtons}
                        isShowModal={this.state.isLanguageCapabilityShowModal}>
                        <LanguageCapabilitiesModalPopUp
                            languages={languages}
                            editLanguageCapability={this.editedRowData}
                            onChangeHandler={this.onChangeHandler}
                            speakingCapability={localConstant.commonConstants.speakingCapability}
                            writingCapability={localConstant.commonConstants.writingCapability}
                            draftLanguageDetails={this.props.draftLanguageDetails}
                            compareDraftData={this.compareDraftData}
                            selectedDraftLanguageDetails={this.props.selectedDraftLanguageDetails}
                        />
                    </Modal>}
                {this.state.isCertificateDetailsShowModal &&
                    <Modal
                        modalClass="techSpecModal"
                        title={this.state.isCertificateDetailsEditModal ? localConstant.techSpec.resourceCapability.EDIT_CERTIFICATE_DETAILS :
                            localConstant.techSpec.resourceCapability.ADD_CERTIFICATE_DETAILS}
                        buttons={this.state.isCertificateDetailsEditModal ? this.certificateEdit : this.certificateAddButtons}
                        isShowModal={this.state.isCertificateDetailsShowModal}>
                        <CertificateDetailsModalPopUp
                            effectiveDate={this.state.effectiveDate}
                            certificateExpiryDate={this.state.certificateExpiryDate}
                            certificateVerificationDate={this.state.certificateVerificationDate}
                            fetchEffectiveDate={this.fetchEffectiveDate}
                            fetchCertificateExpiryDate={this.fetchCertificateExpiryDate}
                            fetchCertificateVerificationDate={this.fetchCertificateVerificationDate}
                            certificateDetails={certificateDetails}
                            uploadedDocument={uploadDocument} //def 848
                            certificates={certificates}
                            onChangeHandler={this.onChangeHandler}
                            uploadDocHandler={this.uploadDocumentHandler}

                            handleDate={this.handleDate}
                            editCertificateDetails={this.editedRowData}
                            // uploadCertificateDocument={this.state.uploadCertificateDocument}
                            editUploadDocument={this.editUploadDocument}
                            isCertificateDetailsEditModal={this.state.isCertificateDetailsEditModal}
                            type={localConstant.commonConstants.type}
                            verificationStatus={localConstant.commonConstants.verificationStatus}
                            verificationType={localConstant.commonConstants.verificationType}
                            verificationTypeILearn={localConstant.commonConstants.verificationTypeILearn}
                            loggedInUser={this.props.loggedInUser}
                            cancelUploadedDocumentData={this.certificateCancelUploadedDocument}
                            verificationCancelUploadedDocumentData={this.verificationCertificateCancelUploadedDocument}
                            draftCertificationDetails={this.props.draftCertificationDetails}
                            selectedDraftCertificationDetails={this.props.selectedDraftCertificationDetails}
                            compareDraftData={this.compareDraftData}
                            fieldsToHide={this.fieldsToHide()}
                            isShowVerifiedUpload={this.state.isShowVerifiedUpload}
                            verifiedBy={this.state.verifiedBy}
                            isRCUserTypeCheck={this.props.isRCUserTypeCheck}
                             />
                    </Modal>
                }
                {this.state.isCommodityShowModal &&
                    <Modal
                        modalClass="techSpecModal commodityModalPopUp"
                        title={this.state.isCommodityEditModal ? localConstant.techSpec.resourceCapability.EDIT_COMMODITY :
                            localConstant.techSpec.resourceCapability.ADD_COMMODITY}
                        buttons={this.state.isCommodityEditModal ? this.commodityEditButtons : this.commodityAddButtons}
                        isShowModal={this.state.isCommodityShowModal}>
                        <CommodityModalPopUp
                            // commodityDetails={commodityDetails}
                            commodityDetails={this.commodityArray}
                            editCommodityDetails={this.editedRowData}
                            onChangeHandler={this.onChangeHandler}
                            commodity={commodity}
                            multiSelectdValueHandler={this.multiSelectdValueHandler}
                            equipmentRating={this.state.equipmentRating}
                            equipment={!isEmpty(equipment) ? addElementToArray(equipment) : []}
                            getCommodityMultiSelectedValue={this.getCommodityMultiSelectedValue}
                            isCommodityEditModal={this.isCommodityEditModal}
                            enableEquipment={this.state.enableEquipment}
                            disabledCommodity={this.state.disabledCommodity}
                            rating={localConstant.commonConstants.rating}
                            draftEquipmentDetails={this.props.draftEquipmentDetails}
                            selectedDraftEquipmentDetails={this.props.selectedDraftEquipmentDetails}
                            compareDraftData={this.compareDraftData}
                            onRef={ref => { this.EquipmentClear = ref; }} //D219
                        />
                    </Modal>
                }
                {this.state.isTrainingDetailsShowModal &&
                    <Modal
                        modalClass="techSpecModal"
                        title={this.state.isTrainingDetailsEditModal ? localConstant.techSpec.resourceCapability.EDIT_TRAINING_DETAILS :
                            localConstant.techSpec.resourceCapability.ADD_TRAINING_DETAILS}
                        buttons={this.state.isTrainingDetailsEditModal ? this.trainingEdit : this.trainingAddButtons}
                        isShowModal={this.state.isTrainingDetailsShowModal}>
                        <TrainingDetailsModalPopUp
                            dateOfTraining={this.state.dateOfTraining}
                            trainingExpiryDate={this.state.trainingExpiryDate}
                            trainingVerificationDate={this.state.trainingVerificationDate}
                            fetchDateOfTraining={this.fetchDateOfTraining}
                            fetchTrainingExpiryDate={this.fetchTrainingExpiryDate}
                            fetchTrainingVerificationDate={this.fetchTrainingVerificationDate}
                            onChangeHandler={this.onChangeHandler}
                            uploadDocHandler={this.uploadDocumentHandler}
                            trainingDetails={trainingDetails}
                            training={training}
                            handleDate={this.handleDate}
                            editUploadDocument={this.editUploadDocument}
                            isTrainingDetailsEditModal={this.state.isTrainingDetailsEditModal}
                            editTrainingDetails={this.editedRowData}
                            // uploadTrainingDocument={this.state.uploadTrainingDocument}
                            type={localConstant.commonConstants.type}
                            verificationStatus={localConstant.commonConstants.verificationStatus}
                            verificationType={localConstant.commonConstants.verificationType}
                            verificationTypeILearn={localConstant.commonConstants.verificationTypeILearn}
                            loggedInUser={this.props.loggedInUser}
                            cancelUploadedDocumentData={this.traningCancelUploadedDocument}
                            traningVerificationCancelUploadedDocumentData={this.traningVerificationCancelUploadedDocument}
                            drafttrainingDetails={this.props.drafttrainingDetails}
                            selectedDrafttrainingDetails={this.props.selectedDrafttrainingDetails}
                            fieldsToHide={this.fieldsToHide()}
                            compareDraftData={this.compareDraftData}
                            isShowVerifiedUpload={this.state.isShowVerifiedUpload}
                            verifiedBy={this.state.verifiedBy}
                            uploadedDocument={uploadDocument}//def 848
                        />
                    </Modal>
                }
                <div className="customCard" >
                    <ResourceCapability
                        codeStandard={!isEmpty(codeStandard) ? addElementToArray(codeStandard) : []}
                        defaultCodeStanded={selectedCodeStanded && selectedCodeStanded.filter(x => x.recordStatus !== "D")}
                        computerKnowledge={!isEmpty(computerKnowledge) ? addElementToArray(computerKnowledge) : []}
                        defaultcomputerKnowledge={selectedComputerKnowledge && selectedComputerKnowledge.filter(x => x.recordStatus !== "D")}
                        getMultiSelectedValue={this.getMultiSelectedValue}
                        getComKnwldgeMltiSelectdVal={this.getComKnwldgeMltiSelectdVal} 
                        interactionMode={this.props.interactionMode}
                        draftcodeandstand={this.props.draftcodeandstand}
                        draftcomputerKnowledge={this.props.draftcomputerKnowledge}
                        draftDataToCompare={draftDataToCompare}
                        selectedDraftcodeandstand={this.props.selectedDraftcodeandstand}
                        selectedDraftcomputerKnowledge={this.props.selectedDraftcomputerKnowledge}
                        compareDraftMultiselectCodeStandard={this.compareDraftMultiselectCodeStandard}
                        compareDraftMultiselectCompKnowledge={this.compareDraftMultiselectCompKnowledge}
                        isResourceDisabled={!this.fieldDisableHandler()}
                        isTSUserTypeCheck={this.props.isTSUserTypeCheck} 
                        technicalSpecialistInfo={this.props.technicalSpecialistInfo}
                        /> {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}

                    {/* Language Details */}
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.resourceCapability.LANGUAGE_CAPABILITIES} colSize="s12"
                    titleClass={ this.props.technicalSpecialistInfo.profileStatus === 'Active' || this.props.isTSUserTypeCheck ? "pl-0 bold mandate" : "pl-0 bold"}>

                        <ReactGrid gridRowData={languageDetails && languageDetails.filter(x => x.recordStatus !== "D")}
                            gridColData={this.headerData.LanguageCapabilitiesHeader} onRef={ref => { this.languageGridChild = ref; }} rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(draftDataToCompare) ? this.languageDraftData : null} paginationPrefixId={localConstant.paginationPrefixIds.techSpecLanguageGridId}/>
                        {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                        {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || this.fieldDisableHandler()) && <div className="right-align mt-2">
                            <button id="addLanguage" disabled={this.props.interactionMode} className=" btn-small" onClick={this.languageCapabilityShowModal} >
                                {localConstant.commonConstants.ADD}</button>
                            <button className="btn-small ml-2 modal-trigger btn-primary dangerBtn" disabled={languageDetails && languageDetails.filter(x => x.recordStatus !== "D").length <= 0 || this.props.interactionMode ?
                                true : false}
                                onClick={this.deleteLanguageCapability} >{localConstant.commonConstants.DELETE}</button>
                        </div>}
                    </CardPanel>
                    {/* Certificate Details */}
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.resourceCapability.CERTIFICATE_DETAILS} colSize="s12">

                        <ReactGrid gridRowData={certificateDetails && certificateDetails.filter(x => x.recordStatus !== "D")}
                            gridColData={this.headerData.CertificateDetailsHeader} onRef={ref => { this.certificateGridChild = ref; }} rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(draftDataToCompare) ? this.certificationDraftData : null} paginationPrefixId={localConstant.paginationPrefixIds.techSpecCertificateGridId}/>
                        {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                        {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || this.fieldDisableHandler()) && <div className="right-align mt-2"> 
                            <button id="addCertificate" disabled={  this.props.interactionMode} className="waves-effect btn-small" onClick={this.certificateDetailsShowModal}>
                                {localConstant.commonConstants.ADD}</button>
                            <button className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={certificateDetails && certificateDetails.filter(x => x.recordStatus !== "D").length <= 0 || this.props.interactionMode ?
                                true : false}
                                onClick={this.deleteCertificateDetails} >{localConstant.commonConstants.DELETE}</button>
                        </div>}
                    </CardPanel>
                    {/* Commodity Details */}
                    <CardPanel className="white lighten-4 black-text"
                        title={localConstant.techSpec.resourceCapability.COMMODITY_EQUIPMENT_KNOWLEDGE} colSize="s12"
                        titleClass={(this.props.technicalSpecialistInfo.profileStatus === 'Active' || this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM || this.props.isTSUserTypeCheck) ? "pl-0 bold mandate" : "pl-0 bold "}>

                        <ReactGrid gridRowData={this.commodityArray && this.commodityArray.filter(x => x.recordStatus !== "D")}
                            gridColData={this.headerData.CommodityKnowledgeHeader} onRef={ref => { this.commodityGridChild = ref; }} rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(draftDataToCompare) ? this.equipmentDraftData : null}
                            isGrouping={true}
                            groupName={this.groupingParam && this.groupingParam.groupName}
                            dataName={this.groupingParam && this.groupingParam.dataName}
                            paginationPrefixId={localConstant.paginationPrefixIds.techSpecCommodityGridId} />
                       <div className="row pl-2"> 
                        <label>{localConstant.techSpec.NOTE}:</label>
                        <ul className="knowledgeNote">
                            <li>{localConstant.techSpec.NOTE_TYPE.NO_EXPERIENCE}</li>
                            <li>{localConstant.techSpec.NOTE_TYPE.BRIEF_EXPERIENCE}</li>
                            <li>{localConstant.techSpec.NOTE_TYPE.LIMITED_EXPERIENCE}</li>
                            <li>{localConstant.techSpec.NOTE_TYPE.AVERAGE_EXPERIENCE}</li>
                            <li>{localConstant.techSpec.NOTE_TYPE.EXTENSIVE_EXPERIENCE}</li>
                            <li>{localConstant.techSpec.NOTE_TYPE.EXTENSIVE_REPETITIVE_EXPERIENCE}</li>
                        </ul>
                        </div>

                        {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                        {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || this.fieldDisableHandler()) && <div className="right-align mt-2">
                            <button id="addCommodity" disabled={ this.props.interactionMode} className="waves-effect btn-small" onClick={this.commodityShowModal}>{localConstant.commonConstants.ADD}</button>
                            <button className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={commodityDetails && commodityDetails.filter(x => x.recordStatus !== "D").length <= 0   || this.props.interactionMode ? true : false}
                                onClick={this.deleteCommodity} >{localConstant.commonConstants.DELETE}</button>
                        </div>}
                    </CardPanel>
                    {/* Training Details */}
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.resourceCapability.TRAINING_DETAILS} colSize="s12">

                        <ReactGrid gridRowData={trainingDetails && trainingDetails.filter(x => x.recordStatus !== "D")}
                            gridColData={this.headerData.TrainingDetailsHeader} onRef={ref => { this.trainingGridChild = ref; }}
                            rowClassRules={{ rowHighlight: true }} draftData={!isEmpty(draftDataToCompare) ? this.trainingDraftData : null} paginationPrefixId={localConstant.paginationPrefixIds.techSpecTrainingGridId} />
                        {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                        {((!isEmpty(this.props.pageMode) && this.props.pageMode !== localConstant.commonConstants.VIEW) || this.fieldDisableHandler()) && <div className="right-align mt-2">
                            <button id="addTraining" disabled={ this.props.interactionMode} className="waves-effect btn-small" onClick={this.trainingDetailsShowModal}>
                                {localConstant.commonConstants.ADD}</button>
                            <button className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                                disabled={trainingDetails && trainingDetails.filter(x => x.recordStatus !== "D").length <= 0 || this.props.interactionMode ? true : false}
                                onClick={this.deleteTrainingDetails} >{localConstant.commonConstants.DELETE}</button>
                        </div>}
                    </CardPanel>

                    {/* Intertek Work history report */}
                    { this.props.currentPage === localConstant.techSpec.EDIT_VIEW_TECHSPEC &&
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.resourceCapability.INTERTEK_WORK_HISTORY_REPORT} colSize="s12">
                        <div className="right-align mt-2">
                            <button id="getIntertekWorkReport" disabled={this.props.interactionMode} className="waves-effect btn-small" onClick={this.fetchIntertekWorkHistoryReport} >
                                {localConstant.commonConstants.GET_INTERTEK_WORK_HISTORY_REPORT}</button>
                        </div> 
                      {intertekWorkHistoryReport && intertekWorkHistoryReport.length>0 && <ReactGrid gridRowData={intertekWorkHistoryReport}
                            gridColData={this.headerData.IntertekWorkHistoryReportHeader}
                            rowClassRules={{ rowHighlight: true }} 
                            paginationPrefixId={localConstant.paginationPrefixIds.techSpecInternalWorkHistoryGridId} />  } 
                    </CardPanel> 
                    }
                </div>
            </Fragment>
        );
    }
}
export default ResourceCapabilityCertification;