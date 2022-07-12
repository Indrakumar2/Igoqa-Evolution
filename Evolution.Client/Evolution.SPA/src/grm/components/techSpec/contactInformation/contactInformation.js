import React, { Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,
    isEmpty, 
    isEmptyReturnDefault, 
    mapArrayObject, 
    isArray, 
    formInputChangeHandler, 
    customRegExValidator, 
    mergeobjects, 
    isUndefined, 
    isEmptyOrUndefine } from '../../../../utils/commonUtils';

import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import { applicationConstants } from '../../../../constants/appConstants';
import { required } from '../../../../utils/validator';
import { activitycode,levelSpecificActivities } from '../../../../constants/securityConstant';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { isEditable,isViewable } from '../../../../common/selector';
import { userTypeGet } from '../../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();

const PersonalDetailsComponent = (props) => {   
    const { draftContactInfo } = props,
    isLevel0Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 }),
    isLevel0Viewable = isViewable({ activities:props.activities,levelType:'LEVEL0',viewActivityLevels:levelSpecificActivities.viewActivitiesLevel0 }),
    isLevel1Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }),
    isLevel1Viewable = isViewable({ activities:props.activities,levelType:'LEVEL_0',viewActivityLevels:levelSpecificActivities.viewActivitiesLevel1 }),
    isTS=props.userTypes.includes(localConstant.techSpec.userTypes.TS),
    isTM=props.userTypes.includes(localConstant.techSpec.userTypes.TM);
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.contactInformation.PERSONAL_DETAILS} colSize="s12">
            <div className="row mb-0">
                <div className="col s12 p-0" >
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.contactInformation.SALUTATION}
                        type='select'
                        colSize='s1'
                        className="browser-default"
                        labelClass={(props.contactInformationDetails.profileAction === undefined || props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_TS || props.contactInformationDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) ? "browser-default" : "mandate browser-default"}
                        optionName='name'
                        optionValue="name"
                        inputClass={props.compareDraftData(draftContactInfo.salutation,props.selectedProfileDraftContactInfoData.salutation, props.contactInformationDetails.salutation,'salutation') ? "inputHighlight" : ""}
                        disabled={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        optionsList={props.salutationMasterData}
                        name='salutation'
                        defaultValue={props.contactInformationDetails.salutation}
                        onSelectChange={props.onChangeHandler} />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.contactInformation.FIRST_NAME}
                        type='text'
                        colSize='s4'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.firstName, props.selectedProfileDraftContactInfoData.firstName, props.contactInformationDetails.firstName,'firstName') ? "inputHighlight" : "")}
                        labelClass="mandate"
                        maxLength={fieldLengthConstants.Resource.contactInformation.FIRST_NAME_MAXLENGTH} // changes for as per DB Requirement (06-12-19)
                        name='firstName'
                        readOnly={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        value={props.contactInformationDetails.firstName ? props.contactInformationDetails.firstName : ""}
                        dataValType='valueText'
                        onValueChange={props.onChangeHandler}
                        autocomplete="off" />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.MIDDLE_NAME}
                        type='text'
                        colSize='s3'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.middleName,props.selectedProfileDraftContactInfoData.middleName, props.contactInformationDetails.middleName,'middleName') ? "inputHighlight" : "")}
                        maxLength={100}
                        name='middleName'
                        readOnly={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        value={props.contactInformationDetails.middleName ? props.contactInformationDetails.middleName : ""}
                        dataValType='valueText'
                        onValueChange={props.onChangeHandler}
                        autocomplete="off" />

                        <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.LAST_NAME}
                        type='text'
                        colSize='s4'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.lastName,props.selectedProfileDraftContactInfoData.lastName, props.contactInformationDetails.lastName,'lastName') ? "inputHighlight" : "")}
                        labelClass="mandate"
                        maxLength={fieldLengthConstants.Resource.contactInformation.LAST_NAME_MAXLENGTH} // changes for as per DB Requirement (06-12-19)
                        name='lastName'
                        readOnly={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        value={props.contactInformationDetails.lastName ? props.contactInformationDetails.lastName : ""}
                        dataValType='valueText'
                        onValueChange={props.onChangeHandler}
                        autocomplete="off" />
                </div>
                <div className="col s12 p-0" >
                    { (!isLevel0Editable && !isLevel0Viewable) || (isLevel1Editable && isLevel1Viewable) ?  //D667
                        <CustomInput
                            hasLabel={true}
                            isNonEditDateField={false}
                            labelClass="customLabel"
                            divClassName={(props.compareDraftData(draftContactInfo.dateOfBirth,props.selectedProfileDraftContactInfoData.dateOfBirth, props.contactInformationDetails.dateOfBirth,'dateOfBirth') ? " inputHighlightDateSelect" : "")}
                            label={localConstant.techSpec.contactInformation.DATE_OF_BIRTH}
                            onDatePickBlur={props.handleDOBDateBlur}
                            colSize='s6'
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            type='date'
                            disabled={ !isLevel1Editable ||  props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                            selectedDate={props.contactInformationDetails.dateOfBirth ? moment(props.contactInformationDetails.dateOfBirth) : ''}
                            onDateChange={props.fetchDOB}
                            shouldCloseOnSelect={true}
                            name='dateOfBirth' /> : null}
                </div>
            </div>
        </CardPanel>
    );
};

const ContactDetailsComponent = (props) => {
    const { draftContactInfo, selectedProfileDraftContactInfoData,contactInformationDetails } = props,
        isLevel1Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }),
        isLevel2Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }),
        isLevel1Viewable = isViewable({ activities: props.activities, levelType: 'LEVEL_1', viewActivityLevels: levelSpecificActivities.viewActivitiesLevel1 }),
        isLevel2Viewable = isViewable({ activities: props.activities, levelType: 'LEVEL_2', viewActivityLevels: levelSpecificActivities.viewActivitiesLevel2 }),
        isViewTM = isViewable({ activities:props.activities, levelType:'VIEW_TM',viewActivityLevels:levelSpecificActivities.viewActivitiesTM }), //D400
        isTS=props.userTypes.includes(localConstant.techSpec.userTypes.TS);
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.contactInformation.CONTACT_DETAILS} colSize="s12">
            <div className="row mb-0" >
                <div className="col s12 p-0" >
                    <CustomInput
                        hasLabel={true}
                        divClassName={"col loadedDivision " + (props.compareDraftData(draftContactInfo.modeOfCommunication,props.selectedProfileDraftContactInfoData.modeOfCommunication, props.contactInformationDetails.modeOfCommunication,'modeOfCommunication') ? "highlightMultiSelecet" : "")}
                        label={localConstant.techSpec.contactInformation.MODE_OF_COMMUNICATION}
                        type='multiSelect'
                        colSize='s6 pr-0'
                        className="browser-default customInputs"
                        optionsList={props.modeOfCommunicationArray}
                        name='modeOfCommunication'
                        disabled={props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS) } //SystemRole based UserType relevant Quick Fixes 
                        multiSelectdValue={props.getMultiSelectedValue}
                        defaultValue={props.defaultcontactModeOfCommunication} />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.CONTACT_COMMENTS}
                        type='textarea'
                        colSize='s6'
                        inputClass={props.compareDraftData(draftContactInfo.contactComment,props.selectedProfileDraftContactInfoData.contactComment, props.contactInformationDetails.contactComment,'contactComment') ? "inputHighlight" : ""}
                        maxLength={ fieldLengthConstants.Resource.contactInformation.CONTACT_COMMENTS_MAXLENGTH }
                        name='contactComment'
                        onValueChange={props.onChangeHandler}
                        readOnly={props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS) } //SystemRole based UserType relevant Quick Fixes 
                        value={props.contactInformationDetails.contactComment ? props.contactInformationDetails.contactComment : ''} //D919
                        dataValType='valueText' />
                    { isLevel2Editable || isLevel2Viewable  || isViewTM ?  //D400 
                            <div className="col s12 p-0" >
                                {props.defaultFieldType.map(obj => {
                                    if (obj === 'PrimaryMobile') {
                                        const mobile = props.contactInfo && props.contactInfo.filter((obj) => {
                                            if (obj.contactType === 'PrimaryMobile') {
                                                return obj;
                                            }
                                        });
                                        const draftmobile = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'PrimaryMobile') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftmobile = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'PrimaryMobile') {
                                                return obj;
                                            }
                                        });
                                        return (
                                            <CustomInput
                                                hasLabel={true}
                                                type="text"
                                                divClassName='col pr-0'
                                                label={localConstant.techSpec.contactInformation.MOBILE_NUMBER}
                                                labelClass="mandate"
                                                //dataType='numeric'
                                                //valueType='value'
                                                colSize='s6'
                                                inputClass={"customInputs " + (obj === 'PrimaryMobile' ? props.compareDraftData((!isEmpty(draftmobile) ? draftmobile[0].mobileNumber : null),(!isEmpty(selectedDraftmobile) ? selectedDraftmobile[0].mobileNumber : null), (!isEmpty(mobile) ? mobile[0].mobileNumber : null),'mobileNumber') ? "inputHighlight" : "" : "")}
                                                onValueChange={props.onChangeHandler}
                                                maxLength={fieldLengthConstants.Resource.contactInformation.MOBILE_NUMBER_MAXLENGTH}
                                                name='mobileNumber'
                                                readOnly={ !isLevel2Editable ||  props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                                dataValType='valueText'
                                                value={(obj === 'PrimaryMobile' ? (!isEmpty(mobile) ? mobile[0].mobileNumber : '') : '')}
                                                autocomplete="off" />
                                        );

                                    }
                                })
                                }
                                {props.defaultFieldType.map(obj => {

                                    if (obj === 'PrimaryPhone') {
                                        const primaryPhone = props.contactInfo && props.contactInfo.filter((obj) => {
                                            if (obj.contactType === 'PrimaryPhone') {
                                                return obj;
                                            }
                                        });
                                        const draftPrimaryPhone = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'PrimaryPhone') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftPrimaryPhone = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'PrimaryPhone') {
                                                return obj;
                                            }
                                        });
                                   
                                        return (
                                            <Fragment>
                                                <CustomInput
                                                    hasLabel={true}
                                                    type="text"
                                                    divClassName='col'
                                                    label={localConstant.techSpec.contactInformation.OTHER_PHONE_NUMBER}
                                                    //dataType='numeric'
                                                    //valueType='value'
                                                    dataValType='valueText'
                                                    colSize='s6'
                                                    inputClass={"customInputs " + (obj === 'PrimaryPhone' ? ( props.compareDraftData((!isEmpty(draftPrimaryPhone) ? draftPrimaryPhone[0].telephoneNumber : null),(!isEmpty(selectedDraftPrimaryPhone) ? selectedDraftPrimaryPhone[0].telephoneNumber : null),(!isEmpty(primaryPhone) ? primaryPhone[0].telephoneNumber : null),'otherTelephoneNumber') ? "inputHighlight" : "") : "")}
                                                    onValueChange={props.onChangeHandler}
                                                    maxLength={fieldLengthConstants.Resource.contactInformation.OTHER_PHONE_NUMBER_MAXLENGTH}
                                                    name='otherTelephoneNumber'
                                                    readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS) } //SystemRole based UserType relevant Quick Fixes 
                                                    value={(obj === 'PrimaryPhone' ? (!isEmpty(primaryPhone) ? primaryPhone[0].telephoneNumber : "") : '')}
                                                    autocomplete="off" />
                                            </Fragment>
                                        );
                                    }

                                })
                                }
                    </div> : null}
                    { isLevel2Editable || isLevel2Viewable    || isViewTM ?  //D400
                            <div className="col s12 p-0" >
                                {props.defaultFieldType.map(obj => {
                                    if (obj === 'PrimaryEmail') {
                                        const primaryEmail = props.contactInfo && props.contactInfo.filter(function (obj) {
                                            if (obj.contactType === 'PrimaryEmail') {
                                                return obj;
                                            }
                                        });
                                        const draftPrimaryEmail = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'PrimaryEmail') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftPrimaryEmail = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'PrimaryEmail') {
                                                return obj;
                                            }
                                        });
                                        return (
                                            <Fragment>
                                                <CustomInput
                                                    hasLabel={true}
                                                    divClassName='col pr-0'
                                                    label={localConstant.techSpec.contactInformation.EMAIL}
                                                    type='text'
                                                    colSize='s6'
                                                    inputClass={"customInputs " + (obj === 'PrimaryEmail' ? props.compareDraftData((!isEmpty(draftPrimaryEmail) ? draftPrimaryEmail[0].emailAddress : null), (!isEmpty(selectedDraftPrimaryEmail) ? selectedDraftPrimaryEmail[0].emailAddress : null), (!isEmpty(primaryEmail) ? primaryEmail[0].emailAddress : null),'emailAddress') ? "inputHighlight" : "" : "")}
                                                    labelClass="mandate"
                                                    maxLength={fieldLengthConstants.Resource.contactInformation.EMAIL_MAXLENGTH}
                                                    name='emailAddress'
                                                    readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                                    onValueChange={props.onChangeHandler}
                                                    //onValueBlur={props.onblurHand}
                                                    value={(obj === 'PrimaryEmail' ? (!isEmpty(primaryEmail) ? primaryEmail[0].emailAddress : "") : '')}
                                                    dataValType='valueText'
                                                    autocomplete="off"
                                                />
                                            </Fragment>
                                        );
                                    }
                                })
                                }

                                {props.defaultFieldType.map(obj => {
                                    if (obj === 'SecondaryEmail') {
                                        const secondaryEmail = props.contactInfo && props.contactInfo.filter((obj) => {
                                            if (obj.contactType === 'SecondaryEmail') {
                                                return obj;
                                            }
                                        });
                                        const draftSecondaryEmail = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'SecondaryEmail') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftSecondaryEmail = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'SecondaryEmail') {
                                                return obj;
                                            }
                                        });
                                        return (
                                            <Fragment>

                                                <CustomInput
                                                    hasLabel={true}
                                                    divClassName='col'
                                                    label={localConstant.techSpec.contactInformation.SECONDARY_EMAIL}
                                                    type='text'
                                                    colSize='s6'
                                                    inputClass={"customInputs " + (obj === 'SecondaryEmail' ? props.compareDraftData((!isEmpty(draftSecondaryEmail) ? draftSecondaryEmail[0].emailAddress : null),(!isEmpty(selectedDraftSecondaryEmail) ? selectedDraftSecondaryEmail[0].emailAddress : null), (!isEmpty(secondaryEmail) ? secondaryEmail[0].emailAddress : null),'secondaryEmailAddress') ? "inputHighlight" : "" : "")}
                                                    maxLength={60}
                                                    name='secondaryEmailAddress'
                                                    readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                                    onValueChange={props.onChangeHandler}
                                                    //onValueBlur={props.onblurHand}
                                                    value={(obj === 'SecondaryEmail' ? (!isEmpty(secondaryEmail) ? secondaryEmail[0].emailAddress : "") : '')}
                                                    dataValType='valueText'
                                                    autocomplete="off"
                                                />
                                            </Fragment>
                                        );
                                    }

                                })
                                }
                    </div> : null}
                    { isLevel2Editable || isLevel2Viewable  || isViewTM  ?   //D400
                            <div className="col s12 p-0" >
                                {props.defaultFieldType.map(obj => {
                                    if (obj === 'Emergency') {
                                        const emergency = props.contactInfo && props.contactInfo.filter((obj) => {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        const draftEmergency = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftEmergency = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        return (
                                            <Fragment>
                                                <CustomInput
                                                    hasLabel={true}
                                                    divClassName='col pr-0'
                                                    label={localConstant.techSpec.contactInformation.EMERGENCY_CONTACT_NAME}
                                                    type='text'
                                                    colSize='s6'
                                                    inputClass={"customInputs " + (obj === 'Emergency' ? props.compareDraftData((!isEmpty(draftEmergency) ? draftEmergency[0].emergencyContactName : null),((!isEmpty(selectedDraftEmergency) ? selectedDraftEmergency[0].emergencyContactName : null)), (!isEmpty(emergency) ? emergency[0].emergencyContactName : null),'emergencyContactName') ? "inputHighlight" : "" : "")}
                                                    maxLength={fieldLengthConstants.Resource.contactInformation.EMERGENCY_CONTACT_NAME_MAXLENGTH}
                                                    labelClass = {(props.contactInformationDetails.profileStatus === 'Active' ? "mandate" :"")}
                                                    // labelClass={(props.contactInformationDetails.profileAction === undefined || props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_TS || props.contactInformationDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) ? "" : "mandate"}
                                                    name='emergencyContactName'
                                                    readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                                    onValueChange={props.onChangeHandler}
                                                    value={obj === 'Emergency' ? (!isEmpty(emergency) ? emergency[0].emergencyContactName : "") : ''}
                                                    dataValType='valueText'
                                                    autocomplete="off" />
                                            </Fragment>
                                        );
                                    }
                                })
                                }
                                {props.defaultFieldType.map(obj => {
                                    if (obj === 'Emergency') {
                                        const emergency = props.contactInfo && props.contactInfo.filter(function (obj) {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        const draftEmergency = props.draftContact && props.draftContact.filter((obj) => {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        const selectedDraftEmergency = props.selectedProfileDraftContactData && props.selectedProfileDraftContactData.filter((obj) => {
                                            if (obj.contactType === 'Emergency') {
                                                return obj;
                                            }
                                        });
                                        return (
                                            <Fragment>
                                                <CustomInput
                                                    hasLabel={true}
                                                    type="text"
                                                    divClassName='col'
                                                    label={localConstant.techSpec.contactInformation.EMERGENCY_CONTACT_NUMBER}
                                                    labelClass = {(props.contactInformationDetails.profileStatus === 'Active' ? "mandate" :"")}
                                                    // labelClass={(props.contactInformationDetails.profileAction === undefined || props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_TS || props.contactInformationDetails.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) ? "" : "mandate"}
                                                    //dataType='numeric'
                                                    //valueType='value'
                                                    dataValType='valueText'
                                                    colSize='s6'
                                                    inputClass={"customInputs " + (obj === 'Emergency' ? props.compareDraftData((!isEmpty(draftEmergency) ? draftEmergency[0].telephoneNumber : null), (!isEmpty(selectedDraftEmergency) ? selectedDraftEmergency[0].telephoneNumber : null),(!isEmpty(emergency) ? emergency[0].telephoneNumber : null),'telephoneNumber') ? "inputHighlight" : "" : "")}
                                                    onValueChange={props.onChangeHandler}
                                                    maxLength={fieldLengthConstants.Resource.contactInformation.EMERGENCY_CONTACT_NUMBER_MAXLENGTH}
                                                    name='telephoneNumber'
                                                    readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                                    value={(obj === 'Emergency' ? (!isEmpty(emergency) ? emergency[0].telephoneNumber : "") : '')}
                                                    autocomplete="off" />
                                            </Fragment>
                                        );
                                    }
                                })
                            }
                        </div> : null } {/* //D400 */}
                    { isLevel1Editable || isLevel1Viewable || isViewTM  ? <div className="col s12 p-0" >
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.contactInformation.DRIVING_LICENSE_NUMBER}
                        type='text'
                        colSize='s6'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.drivingLicenseNo,props.selectedProfileDraftContactInfoData.drivingLicenseNo, props.contactInformationDetails.drivingLicenseNo,'drivingLicenseNo') ? "inputHighlight" : "")}
                        name='drivingLicenseNo'
                        readOnly={ !isLevel1Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        onValueChange={props.onChangeHandler}
                    //  onValueBlur={props.licenseInputhandler}
                        maxLength={100}
                        value={props.contactInformationDetails.drivingLicenseNo ? props.contactInformationDetails.drivingLicenseNo : ""}
                        dataValType='valueText'
                        autocomplete="off" />

                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.PASSPORT_NUMBER}
                        type='text'
                        colSize='s6'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.passportNo,props.selectedProfileDraftContactInfoData.passportNo, props.contactInformationDetails.passportNo,'passportNo') ? "inputHighlight" : "")}
                        name='passportNo'
                        readOnly={!isLevel1Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        maxLength={100}
                     //   onValueBlur={props.passportInputhandler}
                        onValueChange={props.onChangeHandler}
                        value={props.contactInformationDetails.passportNo ? props.contactInformationDetails.passportNo : ""}
                        dataValType='valueText'
                        autocomplete="off" />
                </div> : null } {/* //D400 */}
                    {isLevel1Editable || isLevel1Viewable  || isViewTM   ? <div className="col s12 p-0" >
                    <div className="col s6 p-0" >
                    {props.licenseFieldEnable === true ?
                        <CustomInput
                            hasLabel={true}
                            divClassName='col pr-0'
                            label={localConstant.techSpec.contactInformation.EXPIRY_DATE}
                            type='date'
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            colSize='s12'
                            inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.drivingLicenseExpiryDate, props.selectedProfileDraftContactInfoData.drivingLicenseExpiryDate, props.contactInformationDetails.drivingLicenseExpiryDate,'drivingLicenseExpiryDate') ? "inputHighlight" : "")}
                            selectedDate={props.contactInformationDetails.drivingLicenseExpiryDate ?
                                moment(props.contactInformationDetails.drivingLicenseExpiryDate) : ''}
                            onDateChange={props.fetchLicenceExpiryDate}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            onDatePickBlur={props.handleExpiryDateBlur}
                            labelClass="mandate"
                            name='drivingLicenseExpiryDate'
                            disabled={ !isLevel1Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} /> //SystemRole based UserType relevant Quick Fixes 
                        : null}
                        </div>
                        <div className="col s6 p-0" >
                    {props.passportFieldEnable === true ?
                        <Fragment>
                            <CustomInput
                                hasLabel={true}
                                divClassName='col'
                                label={localConstant.techSpec.contactInformation.EXPIRY_DATE}
                                type='date'
                                colSize='s6'
                                inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.passportExpiryDate,props.selectedProfileDraftContactInfoData.passportExpiryDate, props.contactInformationDetails.passportExpiryDate,'passportExpiryDate') ? "inputHighlight" : "")}
                                selectedDate={props.contactInformationDetails.passportExpiryDate ?
                                    moment(props.contactInformationDetails.passportExpiryDate) : ''}
                                onDateChange={props.fetchPassportExpiryDate}
                                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                                onDatePickBlur={props.handleExpiryDateBlur}
                                labelClass="mandate"
                                name='passportExpiryDate'
                                disabled={ !isLevel1Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)}  //SystemRole based UserType relevant Quick Fixes 
                                /> 
                            <CustomInput
                                hasLabel={true}
                                divClassName='col pl-0'
                                label={localConstant.techSpec.contactInformation.COUNTRY_OF_ORIGIN}
                                type='select'
                                colSize='s6'
                                inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.passportCountryName,props.selectedProfileDraftContactInfoData.passportCountryName, props.contactInformationDetails.passportCountryName,'passportCountryName') ? "inputHighlight" : "")}
                                optionName="name"
                                optionValue="name"
                                labelClass="mandate"
                                optionsList={props.countryMasterData}
                                name='passportCountryName'
                                disabled={ !isLevel1Editable || props.disableField || props.interactionMode || (contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                id='passportCountryName'
                                onSelectChange={props.onChangeHandler}
                                defaultValue={props.contactInformationDetails.passportCountryName} />
                        </Fragment>
                        : null}
                        </div>
                </div> : null }
            </div>
            </div>
        </CardPanel>
    );
};

const AddressComponent = (props) => {
    const { draftContact, selectedProfileDraftContactData } = props,
        isLevel2Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }),
        isTS=props.userTypes.includes(localConstant.techSpec.userTypes.TS),
        isInterCompLevel_0=isViewable({ activities: props.activities, levelType: 'InterCompLevel_0', viewActivityLevels: [ activitycode.INTER_COMP_LEVEL_0_VIEW ] });
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.contactInformation.ADDRESS} colSize="s12">
            <div className="row mb-0" >
                {props.defaultFieldType.map(obj => {
                    if (obj === 'PrimaryAddress') {
                        const primaryAddress = props.contactInfo && props.contactInfo.filter((obj) => {
                            if (obj.contactType === 'PrimaryAddress') {
                                return obj;
                            }
                        });
                        const draftPrimaryAddress = draftContact && draftContact.filter(obj => {
                            if (obj.contactType === 'PrimaryAddress') {
                                return obj;
                            }
                        });
                        const selectedDraftPrimaryAddress = selectedProfileDraftContactData && selectedProfileDraftContactData.filter(obj => {
                            if (obj.contactType === 'PrimaryAddress') {
                                return obj;
                            }
                        });
                        return (
                            <Fragment>
                                <div className="col s12 p-0" >
                                    <CustomInput
                                        hasLabel={true}
                                        divClassName='col pr-0'
                                        label={localConstant.techSpec.contactInformation.COUNTRY_OF_RESIDENCE}
                                        type='select'
                                        colSize='s6'
                                        inputClass={"customInputs " + (obj === 'PrimaryAddress' ? props.compareDraftData((!isEmpty(draftPrimaryAddress) ? draftPrimaryAddress[0].countryId : null),(!isEmpty(selectedDraftPrimaryAddress) ? selectedDraftPrimaryAddress[0].countryId : null), (!isEmpty(primaryAddress) ? primaryAddress[0].countryId : null),'country') ? "inputHighlight" : "" : "")}
                                        labelClass="mandate"
                                        optionsList={props.countryMasterData}
                                        optionName="name"
                                        optionValue="id" // Added For ITK DEf 1536
                                        onSelectChange={props.onChangeHandler}
                                        name="country"
                                        disabled={ !isLevel2Editable || props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                        id="countryId"
                                        defaultValue={(obj === 'PrimaryAddress' ? (!isEmpty(primaryAddress) ? primaryAddress[0].countryId : '') : '')} />
                                    <CustomInput
                                        hasLabel={true}
                                        divClassName='col'
                                        label={localConstant.techSpec.contactInformation.STATE_PREFECTURE_PROVINCE}
                                        type='select'
                                        colSize='s6'
                                        inputClass={"customInputs " + (obj === 'PrimaryAddress' ? props.compareDraftData((!isEmpty(draftPrimaryAddress) ? draftPrimaryAddress[0].countyId : null),(!isEmpty(selectedDraftPrimaryAddress) ? selectedDraftPrimaryAddress[0].countyId : null), (!isEmpty(primaryAddress) ? primaryAddress[0].countyId : null),'county') ? "inputHighlight" : "" : "")}
                                        labelClass="mandate"
                                        optionsList={props.stateMasterData}
                                        optionName="name"
                                        optionValue="id" // Added For ITK DEf 1536
                                        onSelectChange={props.onChangeHandler}
                                        name="county"
                                        disabled={ !isLevel2Editable || props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS) } //SystemRole based UserType relevant Quick Fixes 
                                        id="countyId"
                                        defaultValue={(obj === 'PrimaryAddress' ? (!isEmpty(primaryAddress) ? primaryAddress[0].countyId : '') : '')} />
                                </div>
                                <div className="col s12 p-0" >
                                    <CustomInput
                                        hasLabel={true}
                                        divClassName='col pr-0'
                                        label={localConstant.techSpec.contactInformation.CITY}
                                        type='select'
                                        colSize='s6'
                                        inputClass={"customInputs " + (obj === 'PrimaryAddress' ? props.compareDraftData((!isEmpty(draftPrimaryAddress) ? draftPrimaryAddress[0].cityId : null), (!isEmpty(selectedDraftPrimaryAddress) ? selectedDraftPrimaryAddress[0].cityId : null),(!isEmpty(primaryAddress) ? primaryAddress[0].cityId : null),'city') ? "inputHighlight" : "" : "")}
                                        labelClass="mandate"
                                        optionsList={props.cityMasterData}
                                        optionName="name"
                                        optionValue="id" // Added For ITK DEf 1536
                                        onSelectChange={props.onChangeHandler}
                                        name="city"
                                        disabled={ !isLevel2Editable || props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                        id="cityId"
                                        defaultValue={(obj === 'PrimaryAddress' ? (!isEmpty(primaryAddress) ? primaryAddress[0].cityId : '') : '')} />
                                   <CustomInput
                                        hasLabel={true}
                                        divClassName='col pr-0'
                                        label={localConstant.techSpec.contactInformation.ZIP_POSTAL_code}
                                        type='text'
                                        colSize='s6'
                                        inputClass={"customInputs " + (obj === 'PrimaryAddress' ? props.compareDraftData((!isEmpty(draftPrimaryAddress) ? draftPrimaryAddress[0].postalCode : null),(!isEmpty(selectedDraftPrimaryAddress) ? selectedDraftPrimaryAddress[0].postalCode : null), (!isEmpty(primaryAddress) ? primaryAddress[0].postalCode : null),'postalCode') ? "inputHighlight" : "" : "")}
                                        maxLength={fieldLengthConstants.Resource.contactInformation.ZIP_POSTAL_CODE_MAXLENGTH}
                                        name='postalCode'
                                        readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                        value={(obj === 'PrimaryAddress' ? (!isEmpty(primaryAddress)) ? primaryAddress[0].postalCode ? primaryAddress[0].postalCode:'': '' : '')}
                                        dataValType='valueText'
                                        onValueChange={props.onChangeHandler}
                                        autocomplete="off" />
                                </div>
                                { !isInterCompLevel_0 ? <div className="col s12 p-0" >
                                  <CustomInput
                                        hasLabel={true}
                                        divClassName='col'
                                        label={localConstant.techSpec.contactInformation.STREET_ADDRESS}
                                        type='textarea'
                                        colSize='s6'
                                        inputClass={"customInputs " + (obj === 'PrimaryAddress' ? props.compareDraftData((!isEmpty(draftPrimaryAddress) ? draftPrimaryAddress[0].address : null),(!isEmpty(selectedDraftPrimaryAddress) ? selectedDraftPrimaryAddress[0].address : null), (!isEmpty(primaryAddress) ? primaryAddress[0].address : null),'address') ? "inputHighlight" : "" : "")}
                                        labelClass="mandate"
                                        maxLength={fieldLengthConstants.Resource.contactInformation.STREET_ADDRESS_MAXLENGTH} // changes for as per DB Requirement (06-12-19)
                                        name='address'
                                        readOnly={ !isLevel2Editable || props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                                        value={(obj === 'PrimaryAddress' ? (!isEmpty(primaryAddress) ? primaryAddress[0].address : '') : '')}
                                        dataValType='valueText'
                                        onValueChange={props.onChangeHandler}
                                autocomplete="off" /> 
                                    
                        </div> : null }  
                            </Fragment>
                        );
                    }
                })}
            </div>
        </CardPanel>
    );
};
const LoginCredentialsComponent = (props) => {   
    const { draftContactInfo,selectedProfileDraftContactInfoData } = props,
    //let isRCRM = false;
    isTS= props.userTypes.includes(localConstant.techSpec.userTypes.TS),
    isLevel2Editable = isEditable({ activities: props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }); //def 289 fix
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.contactInformation.LOGIN_CREDENTIALS} colSize="s12">
            <div className="row mb-0" >
                <div className="col s12 p-0">
                    <CustomInput
                        autocomplete="new-username"
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.contactInformation.USER_NAME}
                        type='text'
                        colSize='s6'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.logonName,selectedProfileDraftContactInfoData.logonName, props.contactInformationDetails.logonName,'logonName') ? "inputHighlight" : "")}
                        labelClass="mandate"
                        maxLength={fieldLengthConstants.Resource.contactInformation.USER_NAME_MAXLENGTH}
                        value={props.contactInformationDetails.logonName}
                        dataValType='valueText'
                        name='logonName'
                        onValueChange={props.onChangeHandler}
                        onValueBlur={props.userNameBlurHandler}
                        readOnly={ !isLevel2Editable || props.interactionMode || props.disableField || props.oldProfileActionTM } //def 289
                         />
                    <CustomInput
                        autocomplete="new-password"
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.PASSWORD}
                        type={props.passwordToggle ? 'text' : 'password'}
                        dataTypePassword='text'
                        colSize='s6'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.password, selectedProfileDraftContactInfoData.password,props.contactInformationDetails.password,'password') ? "inputHighlight" : "")}
                        labelClass="mandate"
                        maxLength={fieldLengthConstants.Resource.contactInformation.PASSWORD_MAXLENGTH}
                        value={props.contactInformationDetails.password ? (!isLevel2Editable || props.interactionMode || props.disableField)?"********":props.contactInformationDetails.password :""} //To avoid showing password in f12
                        dataValType='valuePassword'
                        showPasswordToggle={props.passwordToggleHandler}
                        onValueFocus={props.passwordOnFocusHandler}
                        id="password"
                        name='password'
                        isVisable={props.passwordToggle}
                        onValueChange={props.onChangeHandler}
                        onValueBlur={props.passwordBlurHandler}
                        disabled={ !isLevel2Editable || props.interactionMode || props.disableField || props.oldProfileActionTM } //def 289
                        />
                </div>
                <div className="col s12 p-0">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.contactInformation.PASSWORD_SECURITY_QUESTION}
                        labelClass={props.isTSUserType || props.contactInformationDetails.profileStatus === 'Active' ? "mandate" : ""} //D655(REf ALM Doc 02-04-2020) //IGO QC D886 (Ref ALM Confirmation Doc)
                        type='select'
                        inputClass={props.compareDraftData(draftContactInfo.securityQuestion,selectedProfileDraftContactInfoData.securityQuestion, props.contactInformationDetails.securityQuestion) ? "inputHighlight" : ""} //Changes for D556
                        colSize='s6'
                        className="browser-default"
                        optionName="value"
                        optionValue="value"
                        onSelectChange={props.onChangeHandler}
                        optionsList={props.passwordSecurityQuestionArray}
                        defaultValue={props.contactInformationDetails.securityQuestion}
                        name='securityQuestion'
                        disabled={props.interactionMode || props.disableField || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS) } //SystemRole based UserType relevant Quick Fixes 
                        />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.PASSWORD_ANSWER}
                        labelClass={props.isTSUserType || props.contactInformationDetails.profileStatus === 'Active' ? "mandate" : ""} //D655(REf ALM Doc 02-04-2020) //IGO QC D886 (Ref ALM Confirmation Doc)
                        type={props.passwordAnswerToggle ? 'text' : 'password'}
                        showPasswordToggle={props.passwordAnswerToggleHandler}
                        dataTypePassword='text'
                        colSize='s6'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.securityAnswer,selectedProfileDraftContactInfoData.securityAnswer, props.contactInformationDetails.securityAnswer) ? "inputHighlight" : "")} //Changes for D556
                        maxLength={fieldLengthConstants.Resource.contactInformation.PASSWORD_ANSWER_MAXLENGTH}
                        value={props.contactInformationDetails.securityAnswer ? (props.interactionMode || props.disableField || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS))?"********":props.contactInformationDetails.securityAnswer :""} //To avoid showing securityAnswer in f12
                        dataValType='valuePassword'
                        isVisable={props.passwordAnswerToggle}
                        name='securityAnswer'
                        onValueChange={props.onChangeHandler}
                        onValueBlur={props.passwordBlurHandler}
                        disabled={props.interactionMode || props.disableField || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !isTS)} //SystemRole based UserType relevant Quick Fixes 
                        />
                </div>
                <div className="col s12 p-0">                    
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.techSpec.contactInformation.TS_HOME_PAGE_COMMENTS}
                        type='textarea'
                        colSize='s12'
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.homePageComment,selectedProfileDraftContactInfoData.homePageComment, props.contactInformationDetails.homePageComment) ? "inputHighlight" : "")}
                        value={props.contactInformationDetails.homePageComment ? props.contactInformationDetails.homePageComment :''}//D919
                        dataValType='valueText'
                        name='homePageComment'
                        onValueChange={props.onChangeHandler}
                        readOnly={ props.interactionMode || props.disableField || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) } //SystemRole based UserType relevant Quick Fixes                       
                        maxLength={fieldLengthConstants.Resource.contactInformation.TS_HOME_PAGE_COMMENTS_MAXLENGTH}
                         />
                </div>
                <div className="col s12 p-0">
                    <CustomInput
                        divClassName='col'
                        switchLabel={localConstant.techSpec.contactInformation.LOCKED_OUT}
                        type='switch'
                        colSize='s3'
                        className="lever"
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.isLockOut, selectedProfileDraftContactInfoData.isLockOut, props.contactInformationDetails.isLockOut) ? "inputHighlight" : "")}
                        isSwitchLabel={true}
                        disabled={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)} //SystemRole based UserType relevant Quick Fixes 
                        checkedStatus={props.contactInformationDetails.isLockOut}
                        switchName='isLockOut'
                        onChangeToggle={props.onChangeHandler}
                        switchKey={props.contactInformationDetails.isLockOut} />
                    <CustomInput
                        divClassName='col'
                        switchLabel={localConstant.techSpec.contactInformation.ENABLE_EXTRANET_ACCOUNT}
                        type='switch'
                        colSize='s3'
                        className="lever"
                        inputClass={"customInputs " + (props.compareDraftData(draftContactInfo.isEnableLogin, selectedProfileDraftContactInfoData.isEnableLogin,props.contactInformationDetails.isEnableLogin) ? "inputHighlight" : "")}
                        isSwitchLabel={true}
                        disabled={props.disableField || props.interactionMode || (props.contactInformationDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM )} //SystemRole based UserType relevant Quick Fixes 
                        checkedStatus={props.contactInformationDetails.isEnableLogin}
                        switchName='isEnableLogin'
                        onChangeToggle={props.onChangeHandler}
                        switchKey={props.contactInformationDetails.isEnableLogin} />
                </div>
            </div>
        </CardPanel>
    );
};
class ContactInformation extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.updatedobj = {};
        this.state = {
            DOB: '',
            currentDate: moment(),
            licenceExpiryDate: '',
            passportNumberExpiryDate: '',
            inValidDateFormat: false,
            passwordToggle: false,
            passwordAnswerToggle: false,
            checkSelect: false,
            selectedValue: "",
            enablePassportExpiryAndCountry: false,
            enableLicenseExpiry: false,
            modeOfCommunication: [],           
        };
        this.currentUpdatedField='';
        this.updateContactData = [];
        this.contactarrayData = [];
        this.modeOfCommunication = [];
        this.userTypes = userTypeGet();
    }
    // passportInputhandler = (e) => { 
    //     if (e.target.name === 'passportNo' && e.target.value !== "")
    //         this.setState({ enablePassportExpiryAndCountry: true });
    //     else this.setState({ enablePassportExpiryAndCountry: false });
    // }
    // licenseInputhandler = (e) => {
    //     if (e.target.name === 'drivingLicenseNo' && e.target.value !== "")
    //         this.setState({ enableLicenseExpiry: true });
    //     else this.setState({ enableLicenseExpiry: false });
    // }
    fetchDOB = (date) => {
        this.setState({ DOB: date }, () => {
            this.updatedData.dateOfBirth = this.state.DOB !== null ? this.state.DOB : '';
            this.props.actions.UpdateContactInformation(this.updatedData);
        });
    }
    fetchLicenceExpiryDate = (date) => {
        this.setState({ licenceExpiryDate: date }, () => {
            this.updatedData.drivingLicenseExpiryDate = this.state.licenceExpiryDate !== null ? this.state.licenceExpiryDate : '';
            this.props.actions.UpdateContactInformation(this.updatedData);
        });
    }
    fetchPassportExpiryDate = (date) => {
        this.setState({ passportNumberExpiryDate: date }, () => {
            this.updatedData.passportExpiryDate = this.state.passportNumberExpiryDate !== null ? this.state.passportNumberExpiryDate : '';
            this.props.actions.UpdateContactInformation(this.updatedData);
        });
    }
    passwordToggleHandler = (event) => {
        this.setState({ passwordToggle: !this.state.passwordToggle });
    }
    passwordOnFocusHandler =(e) =>{
           this.setState({
              passwordToggle:true
          });
    }
    passwordAnswerToggleHandler = (event) => {
        this.setState({ passwordAnswerToggle: !this.state.passwordAnswerToggle });
    }
    componentDidMount() {
        //this.props.actions.ClearStateCityData();
        const tabInfo = this.props.tabInfo; 
            if (tabInfo && tabInfo.componentRenderCount === 0) { //changes for TS city county binding null
                if (this.props.location.pathname !== '/CreateProfile') {
                const address = this.props.contactInfo && this.props.contactInfo.filter((obj) => {
                    if (obj.contactType === 'PrimaryAddress') {
                        return obj;
                    }
                });
                //const { country, county } = this.props.contactInfo;
                if (!isEmpty(address)) {
                    address[0].countryId && this.props.actions.FetchStateId(address[0].countryId);
                    address[0].countyId && this.props.actions.FetchCityId(address[0].countyId);
                }
            } else {
                this.props.actions.ClearStateCityData();
            }
            // if(this.props.contactInformationDetails.modeOfCommunication) {
            //     const contactInfoDetail= this.props.contactInformationDetails && this.props.contactInformationDetails.modeOfCommunication;
            //     const contactModeOfCommunication=contactInfoDetail && contactInfoDetail.split(',');    
            //     this.setState({ modeOfCommunication: this.convertToMultiSelectObject(contactModeOfCommunication) });
            // }       
        }
        // else {
        //     if (this.props.contactInformationDetails.logonName === '' || this.props.contactInformationDetails.logonName === undefined) {
        //         this.props.actions.AutoGenerate();
        //     }
        // }
        
        if((this.props.contactInformationDetails.dateOfBirth === null || this.props.contactInformationDetails.dateOfBirth === "" || this.props.contactInformationDetails.dateOfBirth === undefined)
        && this.props.selectedProfileDraftContactInfoData.dateOfBirth !== null){
            this.props.actions.UpdateContactInformation( { dateOfBirth : this.props.selectedProfileDraftContactInfoData.dateOfBirth });
        }
    }
    // To handle Country, State and City
    getStateAndCity = (selectedValueName) => {
        if (selectedValueName === "country") {
            this.props.actions.ClearStateCityData();
            const selectedCountry = this.updatedData[selectedValueName];
            this.props.actions.FetchStateId(selectedCountry);
        }
        if (selectedValueName === "county") {
            this.props.actions.ClearCityData();
            const selectedState = this.updatedData[selectedValueName];
            this.props.actions.FetchCityId(selectedState);
        }
    }
    handleSecurityQuestion = (selectedQuestion) => {
        this.setState({
            checkSelect: true,
            selectedValue: selectedQuestion
        });
    }
    //password answer-login credentials validation
    mandatoryPasswordQuestion (value)  {
        if (!isEmpty(value)) {
            if (isEmpty(this.state.selectedValue) && isEmpty(this.props.contactInformationDetails.securityQuestion)) //D917 (ref mail on 03-03-2020)
                IntertekToaster(localConstant.techSpec.contactInformation.SELECT_SECURITY_QUESTION, 'warningToast');
        }
    }
    //login credentials- password validation
    passwordValidation(value, e) {
        const validation = /^(?=.{7,})(?=.*[A-Za-z0-9])(?=.*[\!\@\$\^\&\*\(\)\_\?\.\+\?\^\/\\%#<>-]).*$/; 
        if (validation.test(value) && value.length<=128) //IGO QC D946
            return true; 
        else { 
            IntertekToaster(localConstant.techSpec.contactInformation.PASSWORD_VALIDATION, 'warningToast');
            e.target.value = '';
            return false;
        }
    }
    //personal details- firstName middleName and lastName validation
    alphabetsValidation(value) {
        if ((value) &&
            customRegExValidator(/^[a-zA-Z !@#$%^&_*()+\-=[\]{};':"`\\|,.<>/?]*$/, 'i', value)) { //`symbol include Issue Raised by E-mail (Sub:New issue in ITK End to End testing, on:01-09-2020, from:Francina)
            IntertekToaster(localConstant.techSpec.contactInformation.ALPHABETS_VALIDATION, 'warningToast');
            return false;
        }
    }
    //contact details- email validation
    emailValidation(value, valMessage) {
        if ((value) &&
            customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i', value)) {
            IntertekToaster(valMessage, 'warningToast');
            return false;
        }
    }

    userNameBlurHandler= (e) => {
        if (e.target.name === 'logonName' && e.target.value !== "")
        {
           if(isEmptyOrUndefine(this.props.contactInformationDetails.password))
           { 
            this.updatedData["password"]=RandomStringGenerator(15);
           }
           if(this.updatedData && !isEmptyOrUndefine(this.updatedData.password)) //D917(Ref 09-03-2020 ALM Doc )
                this.props.actions.UpdateContactInformation(this.updatedData);
        //   this.setState({
        //       passwordToggle:true
        //   }); //Changed as onfocus function
          this.updatedData={}; //D917(Ref 09-03-2020 ALM Doc )
        }
   }

   passwordBlurHandler=(e)=>{
    if (e.target.name === 'password')
    {
        if(!this.passwordValidation(e.target.value, e)){
            this.updatedData['password']="";
            this.props.actions.UpdateContactInformation(this.updatedData); //D917(Ref 09-03-2020 ALM Doc )
        }
    } 
    if(e.target.name === 'securityAnswer'){
        this.mandatoryPasswordQuestion(e.target.value);
    }
   }

    onChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if (result.name === 'country' || result.name === 'county' || result.name === 'city')
            this.getStateAndCity(result.name);
        if (result.name === 'securityQuestion')
            this.handleSecurityQuestion(result.value);
        
        if(result.name === 'country'){
            this.updatedobj['county'] = "";
            this.updatedobj['city'] = "";
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        } // Added For ITK DEf 1536
        if(result.name === "county"){
            this.updatedobj["city"]=null;      
            this.updatedobj["cityId"]=null;    
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        } // Added For ITK DEf 1536
        if(result.name === "city"){
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        } // Added For ITK DEf 1536
        // if (result.name === 'securityAnswer')
        //     this.mandatoryPasswordQuestion();

        /** Reg ITKD1302 -- Need to Remove the Name Validation  
         * ref Email-- Reg: Special letters in Resource name
         */
        // if (result.name === 'firstName' || result.name === 'middleName' || result.name === 'lastName')
        //     this.alphabetsValidation(result.value);
         /** Reg ITKD1302 -- Need to Remove the Name Validation  
         * ref Email-- Reg: Special letters in Resource name
         */
        // if (result.name === 'emailAddress' || result.name === 'secondaryEmailAddress') {                       
        //     if(result.value.indexOf("@") === -1 && result.value.indexOf(".") === -1) {
        //         this.emailValidation(result.value);
        //     }
        // }
        if (result.name === 'country' || result.name === 'county' || result.name === 'city' || result.name === 'postalCode' || result.name === 'address') {
            this.updatedobj['contactType'] = 'PrimaryAddress';
            this.updatedobj[result.name] = result.value;
            if(result.name === 'country' || result.name === 'county' || result.name === 'city'){
                this.updatedobj[e.target.id]=parseInt(result.value);
                this.updatedobj[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
            } 
            if (this.updatedobj['country'] === "") {
                this.updatedobj['county'] = "";
            }           // D-367
            if (this.updatedobj['county'] === "") {
                this.updatedobj['city'] = "";
            }   // D-367
        } 
        else if (result.name === 'mobileNumber') {
            this.updatedobj['contactType'] = 'PrimaryMobile'; 
            this.updatedobj[result.name] = result.value;
        }
        else if (result.name === 'otherTelephoneNumber') {
            this.updatedobj['contactType'] = 'PrimaryPhone'; 
            this.updatedobj['otherTelephoneNumber'] = result.value;
        }
        else if (result.name === 'emailAddress') {
            result.value = result.value.replace(/\s/g,'');//D377 Added to restrict space button
            this.updatedobj['contactType'] = 'PrimaryEmail'; 
            this.updatedobj[result.name] = result.value;
        }
        else if (result.name === 'secondaryEmailAddress') {
            result.value = result.value.replace(/\s/g,'');//D377 Added to restrict space button
            this.updatedobj['contactType'] = 'SecondaryEmail'; 
            this.updatedobj['secondaryEmailAddress'] = result.value;
        }
        else if (result.name === 'emergencyContactName' || result.name === 'telephoneNumber') {
            this.updatedobj['contactType'] = 'Emergency'; 
            this.updatedobj[result.name] = result.value;
        }
        // if (result.name === 'password')
        // {
        //     if(!this.passwordValidation(result.value, e)){
        //         this.updatedData['password']="";
        //     }
        // }
        else {
            this.updatedData[result.name] = result.value;
        }
        
        this.props.actions.UpdateContactInformation(this.updatedData);
        this.props.actions.UpdateContact(this.updatedobj);
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){ 
                this.props.actions.IsRCRMUpdatedContactInformation(true);            
                this.currentUpdatedField=result.name; 
        }
        this.updatedData = {};
        this.updatedobj = {};
    }
    onblurHand = (e) => {
        const result = formInputChangeHandler(e);
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedContactInformation(true);   
        } 
        if (result.name === 'emailAddress' || result.name === 'secondaryEmailAddress') {
            const validationMessage = result.name === 'emailAddress' ? localConstant.techSpec.contactInformation.EMAIL_VALIDATION : localConstant.techSpecValidationMessage.SECONDARY_EMAIL_VALIDATION;
            this.emailValidation(result.value, validationMessage);
        }
    }

    // to handle DOB
    dateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from >= to) {
                isInValidDate = true;
                IntertekToaster(localConstant.techSpec.contactInformation.INVALID_DOB_WARNING, 'warningToast');
            }
        }
        return isInValidDate;
    }
    handleDOBDateBlur = (e) => {
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedContactInformation(true);  
        } 
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isUIValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    //Sanity Def 181
                    this.updatedData.dateOfBirth = "";
                    this.props.actions.UpdateContactInformation(this.updatedData);
                    //Sanity Def 181
                    } else
                        this.setState({ inValidDateFormat: false });
                    let dateOfBirth = '';
                    if (this.state.DOB !== '' && this.state.DOB !== null)
                        dateOfBirth = this.state.DOB.format(localConstant.techSpec.common.DATE_FORMAT);
                    if (!this.dateRangeValidator(dateOfBirth, this.state.currentDate.format(localConstant.techSpec.common.DATE_FORMAT))
                        && !this.state.inValidDateFormat) {
                        return false;
                    }
                }
            }
        }
    }
    //To handle Expiry Date
    handleExpiryDateBlur = (e) => {
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedContactInformation(true);  
        } 
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                this.setState({ inValidDateFormat: true });
            }
            else this.setState({ inValidDateFormat: false });
        }
    }
    //Contact Information-modeOfCommunication: Multi Selection Handler
    getMultiSelectedValue = (data) => {
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedContactInformation(true);  
        } 
        const selectedItem = mapArrayObject(data, 'value').join(',');
        this.updatedData.modeOfCommunication = selectedItem;
        this.props.actions.UpdateContactInformation(this.updatedData);
        // const selectedItem = [];
        // data.map(value => {
        //     selectedItem.push(value.label);
        // });
        // this.updatedData.modeOfCommunication = selectedItem;
        // this.props.actions.UpdateContactInformation(this.updatedData);
    }

    /** Disable field handler */
    fieldDisableHandler = () => { 
        if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
            return true;
        }
        return  !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });
           
    };

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
    
    compareDraftData = (draftValue, selectedDraftValue, fieldValue, fieldName) => {
        const _this = this;
        if (!isEmpty(_this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
            // if (!_this.state.isRCRMUpdate) {
            if (!_this.props.isRCRMUpdate) {
                if (draftValue === undefined && !required(fieldValue)) {
                    return true;
                }
                else if (draftValue !== undefined && draftValue !== fieldValue) {
                    return true;
                }
                return false;
            } else if (_this.props.isRCRMUpdate)// if (_this.state.isRCRMUpdate) 
            {
                //'drivingLicenseNo',  'passportNo','drivingLicenseExpiryDate','passportExpiryDate','passportCountryName'
                const contactFields = [
                    'mobileNumber', 'otherTelephoneNumber', 'emailAddress', 'secondaryEmailAddress', 'emergencyContactName', 'telephoneNumber', 'country', 'county', 'city', 'postalCode', 'address'
                ];
                //RC RM While modifing Value Not showing Highlight 
                if (draftValue && draftValue === fieldValue) {
                    return false;
                }
                else if ((draftValue || required(draftValue)) && !required(fieldValue) && fieldValue === selectedDraftValue && contactFields.findIndex(x => x === fieldName) >= 0) {
                    return draftValue !== fieldValue;
                }
                else if (draftValue && !required(fieldValue) && contactFields.findIndex(x => x === fieldName) >= 0) {
                    return false;
                }
                // else if (draftValue && !selectedDraftValue && draftValue !== fieldValue) { 
                //     return true;
                // } 
                else if (draftValue && draftValue !== fieldValue) {
                    return (selectedDraftValue === fieldValue);
                }
                else if (fieldValue === '' && contactFields.findIndex(x => x === fieldName) >= 0) {
                    return false;
                }
                else if (isEmptyOrUndefine(draftValue) && !isEmptyOrUndefine(selectedDraftValue) && !isEmptyOrUndefine(fieldValue)) { 
                    return selectedDraftValue===fieldValue;
                } 
            }
            return false;
        }
        return false;
    };     

    convertToMultiSelectObject = (datas) => {
        const multiselectArray = [];
        if (datas) {
            datas.map(data => {
                multiselectArray.push({ value: data, label: data });
            });
        }
        return multiselectArray;
    } 
    
    // componentWillReceiveProps(nextProps){  
    //     if(nextProps.contactInformationDetails){
    //         if(nextProps.contactInformationDetails.modeOfCommunication){
    //             const contactInfoDetail= this.props.contactInformationDetails && this.props.contactInformationDetails.modeOfCommunication;
    //             const contactModeOfCommunication=contactInfoDetail && contactInfoDetail.split(',');    
    //             this.setState({ modeOfCommunication: this.convertToMultiSelectObject(contactModeOfCommunication) });
    //         }      
    //     }
    // }

    render() { 
        const { countryMasterData, stateMasterData, cityMasterData, salutationMasterData,
            contactInformationDetails, contactInfo, defaultFieldType, interactionMode,
            draftContactInfoData, draftContactData, securityQuestionsMasterData 
            ,activities,selectedProfileDraftContactInfoData,selectedProfileDraftContactData } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const disableField = this.fieldDisableHandler();
        const contactInfoDetail = contactInformationDetails && contactInformationDetails.modeOfCommunication;
        const contactModeOfCommunication = contactInfoDetail && contactInfoDetail.split(',');
        const defaultcontactModeOfCommunication = this.convertToMultiSelectObject(contactModeOfCommunication);
        return (
            <div className="customCard" >
                <CustomModal modalData={modelData} />
                <PersonalDetailsComponent
                    DOB={this.state.DOB} fetchDOB={this.fetchDOB}
                    handleDOBDateBlur={this.handleDOBDateBlur}
                    contactInformationDetails={contactInformationDetails}
                    salutationMasterData={salutationMasterData}
                    onChangeHandler={(e) => this.onChangeHandler(e)}
                    disableField={disableField}
                    interactionMode={interactionMode}
                    draftContactInfo={draftContactInfoData}
                    selectedProfileDraftContactInfoData={selectedProfileDraftContactInfoData}
                    compareDraftData={this.compareDraftData}
                    profileAction={this.state.profileAction} 
                    activities = {activities}
                    userTypes={this.userTypes}
                />
                <ContactDetailsComponent
                    licenceExpiryDate={this.state.licenceExpiryDate}
                    fetchLicenceExpiryDate={this.fetchLicenceExpiryDate}
                    passportNumberExpiryDate={this.state.passportNumberExpiryDate}
                    fetchPassportExpiryDate={this.fetchPassportExpiryDate}
                    countryMasterData={countryMasterData}
                    handleExpiryDateBlur={this.handleExpiryDateBlur}
                    licenseFieldEnable={this.props.contactInformationDetails && contactInformationDetails.drivingLicenseNo ? true :false}
                 //   licenseInputhandler={this.licenseInputhandler}
                    passportFieldEnable={this.props.contactInformationDetails && contactInformationDetails.passportNo ? true :false}
                  //  passportInputhandler={this.passportInputhandler}
                    onChangeHandler={(e) => this.onChangeHandler(e)}
                    onblurHand={(e) => this.onblurHand(e)}
                    contactInformationDetails={contactInformationDetails}
                    contactInfo={contactInfo}
                    secondaryEmailAddress={this.state.secondaryEmailAddress}
                    modeOfCommunicationArray={localConstant.commonConstants.modeOfCommunicationArray}
                    getMultiSelectedValue={this.getMultiSelectedValue}
                    selectedProfileDetails={this.props.selectedProfileDetails}
                    defaultFieldType={defaultFieldType}
                    disableField={disableField}
                    interactionMode={interactionMode}
                    draftContactInfo={draftContactInfoData}
                    draftContact={draftContactData}
                    selectedProfileDraftContactInfoData={selectedProfileDraftContactInfoData}
                    selectedProfileDraftContactData={selectedProfileDraftContactData}
                    compareDraftData={this.compareDraftData}
                    defaultcontactModeOfCommunication={defaultcontactModeOfCommunication}
                    //  defaultcontactModeOfCommunication={this.state.modeOfCommunication}
                    currentUrl={this.props.location.pathname} 
                    activities = {activities}
                    userTypes={this.userTypes}
                />
                { isEditable({ activities: activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel2 }) || isViewable({ activities: activities, levelType: 'LEVEL_2', viewActivityLevels: levelSpecificActivities.viewActivitiesLevel2 }) || isViewable({ activities: activities, levelType: 'InterCompLevel_0', viewActivityLevels: levelSpecificActivities.viewActivitiesInterCompLevel0 }) ? <AddressComponent  //def 957 fix
                        countryMasterData={countryMasterData}
                        stateMasterData={stateMasterData}
                        cityMasterData={cityMasterData}
                        contactInfo={contactInfo}
                        contactInformationDetails={contactInformationDetails}
                        onChangeHandler={(e) => this.onChangeHandler(e)}
                        defaultFieldType={defaultFieldType}
                        disableField={disableField}
                        interactionMode={interactionMode}
                        draftContactInfo={draftContactInfoData}
                        draftContact={draftContactData}
                        selectedProfileDraftContactInfoData={selectedProfileDraftContactInfoData}
                        selectedProfileDraftContactData={selectedProfileDraftContactData}
                        compareDraftData={this.compareDraftData}
                        activities = {activities}
                        userTypes={this.userTypes}
                    />: null}
                <LoginCredentialsComponent
                    contactInformationDetails={contactInformationDetails}
                    passwordToggleHandler={this.passwordToggleHandler}
                    passwordOnFocusHandler={this.passwordOnFocusHandler}
                    passwordToggle={this.state.passwordToggle}
                    passwordAnswerToggle={this.state.passwordAnswerToggle}
                    passwordAnswerToggleHandler={this.passwordAnswerToggleHandler}
                    passwordSecurityQuestionArray={securityQuestionsMasterData}
                    onChangeHandler={this.onChangeHandler}
                    userNameBlurHandler={this.userNameBlurHandler}
                    passwordBlurHandler={this.passwordBlurHandler}
                    userTypes={this.userTypes}
                    interactionMode={interactionMode}
                    disableField={disableField}
                    draftContactInfo={draftContactInfoData}
                    compareDraftData={this.compareDraftData}
                    selectedProfileDraftContactInfoData={selectedProfileDraftContactInfoData}
                    selectedProfileDraftContactData={selectedProfileDraftContactData}
                    activities = {activities}
                    isTSUserType ={this.props.isTSUserTypeCheck}
                    currentPage={this.props.currentPage === localConstant.techSpec.common.CREATE_PROFILE ? false : true}
                    oldProfileActionTM={this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_TM}
                />
            </div>
        );
    }
}
export default ContactInformation;

function RandomStringGenerator(length) { 
    const chars = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+<>_ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    let pass = "";
    for (let x = 0; x < length; x++) {
        const i = Math.floor(Math.random() * chars.length);
        pass += chars.charAt(i);
    }  
    return pass;
}
