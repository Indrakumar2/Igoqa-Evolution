import React, { Fragment } from 'react';
import PropTypes from 'prop-types';
import CustomInput from '../baseComponents/inputControlls';
import { getlocalizeData,isUndefined,isEmptyReturnDefault } from '../../utils/commonUtils';
import ReactGrid from '../baseComponents/reactAgGrid';
import LabelWithValue from '../baseComponents/customLabelwithValue';
import dateUtil from '../../utils/dateUtil';
import { required } from '../../utils/validator';
import MultiSelectField from './multiSelect';
import moment from 'moment';

const localConstant = getlocalizeData();

export const CustomerContactInfoDiv = (props) => {
    const { inputChangeHandler,isQuickSearch,moreDetailsData,isPreAssignmentPage } = props;
    return (
        <div className={ isQuickSearch || isPreAssignmentPage ? 'col s12 pl-0 pr-0' : 'col s12'}>
            <CustomInput
                hasLabel={true}
                name="customerContactPerson"
                colSize='s3'
                label={localConstant.resourceSearch.CUSTOMER_CONTACT_PERSON}
                labelClass="customLabel"
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.customerContactPerson ? moreDetailsData.customerContactPerson : ""}
                onValueChange={inputChangeHandler}
                readOnly={props.interactionMode} />
            <CustomInput
                hasLabel={true}
                name="customerPhoneNumber1"
                divClassName='pl-0'
                colSize='s3'
                label={localConstant.resourceSearch.CUSTOMER_OFFICE_PHONE}
                labelClass="customLabel"
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.customerPhoneNumber1 ? moreDetailsData.customerPhoneNumber1 : ""}
                onValueChange={inputChangeHandler} 
                readOnly={props.interactionMode}/>
            <CustomInput
                hasLabel={true}
                name="customerMobileNumber"
                divClassName='pl-0'
                colSize='s3'
                label={localConstant.resourceSearch.CUSTOMER_MOBILE_PHONE}
                labelClass="customLabel"
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.customerMobileNumber ? moreDetailsData.customerMobileNumber : ""}
                onValueChange={inputChangeHandler}
                readOnly={props.interactionMode} />
            <CustomInput
                hasLabel={true}
                name="customerContactEmail"
                divClassName='pl-0'
                colSize='s3'
                label={localConstant.resourceSearch.CUSTOMER_CONTACT_EMAIL}
                labelClass="customLabel"
                type='email'
                dataValType='valueText'
                inputClass="customInputs"
                value={moreDetailsData.customerContactEmail ? moreDetailsData.customerContactEmail : ""}
                onValueChange={inputChangeHandler}
                readOnly={props.interactionMode} />
        </div>
    );
};

export const MaterialInfoDiv = (props) => {
    const { inputChangeHandler ,interactionMode,moreDetailsData,isQuickSearchPage,isPreAssignmentPage } = props;
    return (
      <div className={ isQuickSearchPage || isPreAssignmentPage ? "col s4 pl-0 pr-0" : "col s12 pl-0"}>
            <CustomInput
                hasLabel={true}
                name="materialDescription"
                colSize={isQuickSearchPage || isPreAssignmentPage ? 's12' :'s4'}
                label={localConstant.resourceSearch.MATERIAL_DESCRIPTION}
                labelClass={!props.isQuickSearchPage ? "customLabel mandate" : "customLabel"}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                disabled={interactionMode}
                value={moreDetailsData.materialDescription ? moreDetailsData.materialDescription : ""}
                onValueChange={inputChangeHandler} />
       </div>
    );
};
export const CategoryInfoDiv = (props) => {
    const { inputChangeHandler,interactionMode,taxonomyCategory,taxonomySubCategory,taxonomyServices,moreDetailsData, isDashboardARSView } = props;
    return (
        <Fragment>
            {(!isDashboardARSView) && <div className="col s12 pl-0 pr-0">
            <CustomInput
                hasLabel={true}
                name="categoryName"
                id="categoryId"
                colSize='s4'
                label={localConstant.resourceSearch.CATEGORY}
                type='select'
                optionsList={taxonomyCategory}
                labelClass={!props.isQuickSearch ? "customLabel mandate" : "customLabel"}
                optionName='name'
                optionValue="name"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {moreDetailsData.categoryName}
                onSelectChange={inputChangeHandler}/>
            <CustomInput
                hasLabel={true}
                name="subCategoryName"
                id="subCategoryId"
                divClassName='pl-0'
                colSize='s4'
                label={localConstant.resourceSearch.SUB_CATEGORY}
                type='select'
                optionsList={taxonomySubCategory}
                labelClass={!props.isQuickSearch ? "customLabel mandate" : "customLabel"}
                optionName='taxonomySubCategoryName'
                optionValue="taxonomySubCategoryName"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {moreDetailsData.subCategoryName}
                onSelectChange={inputChangeHandler} />
            <CustomInput
                hasLabel={true}
                name="serviceName"
                id="serviceId"
                colSize='s4 pl-0'
                label={localConstant.resourceSearch.SERVICES}
                type='select'
                optionsList={taxonomyServices}
                labelClass={!props.isQuickSearch ? "customLabel mandate" : "customLabel"}
                optionName='taxonomyServiceName'
                optionValue="taxonomyServiceName"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {moreDetailsData.serviceName}
                onSelectChange={inputChangeHandler} />
        </div>
            }
            {isDashboardARSView && 
             <Fragment>
                      <div className="col s12">
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.CATEGORY } : ` }
                        value={moreDetailsData.categoryName}
                    />
                    <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.SUB_CATEGORY } : ` }
                        value={moreDetailsData.subCategoryName}
                    />
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.SERVICES } : ` }
                        value={moreDetailsData.serviceName}
                    />
                    </div>
            </Fragment>  }
        </Fragment>
        
    );
};
export const SupplierInfoDiv = (props) => {
    const { currentPage,isQuickSearchPage,inputChangeHandler,isARSSearch,interactionMode,moreDetailsData,isPreAssignmentPage,isDashboardARSView } = props;
    return (
      <Fragment>
           { (!isDashboardARSView) &&  <div className='col s12 pl-0 pr-0' >
            <CustomInput
                hasLabel={true}
                name="supplier"
                colSize={isQuickSearchPage || isPreAssignmentPage ?'s6': ((isARSSearch && !isDashboardARSView) ? 's4' :'s4 pl-0') }
                label={localConstant.resourceSearch.SUPPLIER}
                labelClass={!props.isQuickSearch ? "customLabel mandate" : "customLabel"}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                readOnly={interactionMode }
                value={moreDetailsData.supplier ? moreDetailsData.supplier : ""}
                onValueChange={inputChangeHandler} /> 

                {(isARSSearch && (!isDashboardARSView)) && <CustomInput
                hasLabel={true}
                name="supplierPo"
                colSize='s4'
                label={localConstant.resourceSearch.SUPPLIER_PO}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.supplierPurchaseOrder ? moreDetailsData.supplierPurchaseOrder : ""}
                onValueChange={inputChangeHandler} 
                readOnly={interactionMode}/>
                }
                <CustomInput
                hasLabel={true}
                name="supplierLocation"
                divClassName='pl-0'
                colSize={isQuickSearchPage || isPreAssignmentPage ?'s6 pl-0':'s4 pl-0'}
                label={localConstant.resourceSearch.SUPPLIER_LOCATION}
                labelClass={!props.isQuickSearch ? "customLabel mandate" : "customLabel"}
                type='textarea'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.supplierLocation ? moreDetailsData.supplierLocation : ""}
                onValueChange={inputChangeHandler} 
                // disabled={interactionMode} 
                readOnly = {interactionMode} />
            {/* { isQuickSearchPage &&
                <CustomInput
                    hasLabel={true}
                    name="firstVisitDate"
                    colSize='s4 clearMb'
                    label={localConstant.resourceSearch.FIRST_VISIT_DATE}
                    type='date'
                    inputClass="customInputs"
                    maxLength={10}
                    onValueChange={inputChangeHandler}
                    disabled={interactionMode} />
            } */}
        </div>
           }
           {
               (isDashboardARSView) &&  <div className='col s12  pr-0' >
                <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.SUPPLIER } : ` }
                        value={moreDetailsData.supplier ? moreDetailsData.supplier : ""}
                    /> 
                    <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.SUPPLIER_PO } : ` }
                        value={moreDetailsData.supplierPurchaseOrder ? moreDetailsData.supplierPurchaseOrder : ""}
                    /> 
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.SUPPLIER_LOCATION } : ` }
                        value={moreDetailsData.supplierLocation ? moreDetailsData.supplierLocation : ""}
                    /> 
               </div>
           }
       </Fragment> 
    
    );
};

export const AssignMentType = (props) => {
    const { inputChangeHandler, assignmentType,interactionMode,isARSSearch,assignMentStatusList,optionAttributs,isQuickSearch, isPreAssignmentPage,
        moreDetailsData, isDashboardARSView,assignmentTypeData } = props;
    return (
        <Fragment>
         {(!isDashboardARSView) &&  <div className={isQuickSearch || isPreAssignmentPage ? "col  pl-0 " :"col s12 pl-0 pr-0"}>
            {/* <CustomInput
                hasLabel={true}
                name="assignmentType"
                id="assignmentTypeId"
                colSize={isQuickSearch || isPreAssignmentPage ? 's12' : 's4' }
                label={localConstant.resourceSearch.ASSIGNMENT_TYPE}
                type='select'
                optionsList={assignmentType}
                labelClass="customLabel mandate"
                optionName='name'
                optionValue="description"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {moreDetailsData.assignmentType}
                onSelectChange={inputChangeHandler} /> */}
                {isARSSearch && <CustomInput
                    hasLabel={true}
                    name="assignmentStatus"
                    id="assignmentStatus"
                    colSize='s4 pl-0'
                    label={localConstant.resourceSearch.ASSIGNMENT_STATUS}
                    optionsList={assignMentStatusList}
                    labelClass="customLabel"
                    optionName="name"
                    optionValue="description"
                    disabled={props.interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={moreDetailsData.assignmentStatus}
                    onSelectChange={inputChangeHandler} />
             }
             {isARSSearch && <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    label={localConstant.resourceSearch.ASSIGNMENT_CREATE_DATE}
                    labelClass="customLabel"
                    colSize='s4 clearMb'
                    dateFormat="DD-MM-YYYY"
                    type='date'
                    name="assignmentCreatedDate"
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(moreDetailsData.assignmentCreatedDate)}
                    // onDateChange={}
                    shouldCloseOnSelect={true}
                    disabled={interactionMode}/>
             }
        </div>
         }
         {isDashboardARSView && <Fragment>
                        <div className="col s12">
                       <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.ASSIGNMENT_TYPE } : ` }
                        value={assignmentTypeData}
                        /> 
                        {/* <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.ASSIGNMENT_STATUS }:` }
                        value={moreDetailsData.assignmentStatus}
                        />  */}
                        <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.ASSIGNMENT_CREATE_DATE } : ` }
                        value={moreDetailsData.assignmentCreatedDate && moment(moreDetailsData.assignmentCreatedDate).format(localConstant.commonConstants.UI_DATE_FORMAT)}
                        // value={dateUtil.formatDate(moreDetailsData.assignmentCreatedDate,'-')}
                        /> 
                        </div>
                    </Fragment>
                 }
        </Fragment>
        
    );
};
export const FirstVisit = (props) => {
    const { inputChangeHandler, interactionMode ,isARSSearch,isQuickSearch,isPreAssignment,firstVisitFrom,firstVisitTo,moreDetailsData,isDashboardARSView } = props;
    return (
       <Fragment>
           
           { (isPreAssignment || (isARSSearch &&  (!isDashboardARSView)) ) &&  <CustomInput
                hasLabel={true}
                name="firstVisitFromDate"
                isNonEditDateField={false}
                colSize={ isPreAssignment ? 's4 clearMb pl-0 ' : 's3 clearMb ' }
                label={localConstant.resourceSearch.FIRST_VISIT_FROM_DATE}
                type='date'
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                inputClass="customInputs"
                onDateChange={firstVisitFrom}
                selectedDate={dateUtil.defaultMoment(moreDetailsData['firstVisitFromDate'])} 
                disabled={interactionMode}/>
                }
           {(isPreAssignment || (isARSSearch &&  (!isDashboardARSView))) &&  <CustomInput
                hasLabel={true}
                name="firstVisitToDate"
                colSize={ isPreAssignment ? 's4 clearMb pl-0 ' : 's3 clearMb pl-0 ' }
                label={localConstant.resourceSearch.FIRST_VISIT_TO_DATE}
                type='date'
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                inputClass="customInputs"
                onDateChange={firstVisitTo}
                selectedDate={dateUtil.defaultMoment(moreDetailsData['firstVisitToDate'])}
                disabled={interactionMode} />
           }
           {
                (isARSSearch &&  (!isDashboardARSView)) && <CustomInput
                hasLabel={true}
                name="firstVisitLocation"
                colSize='s3 clearMb pl-0'
                label={localConstant.resourceSearch.FIRST_VISIT_TIMESHEETLOCATION}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={255}
                value={moreDetailsData.firstVisitLocation ? moreDetailsData.firstVisitLocation : ""}
                onValueChange={inputChangeHandler}
                disabled={interactionMode} />
               }
               {
                (isARSSearch &&  (!isDashboardARSView)) && <CustomInput
                hasLabel={true}
                name="firstVisitStatus"
                colSize='s3 clearMb pl-0'
                label={localConstant.resourceSearch.FIRST_VISIT_TIMESHEET_STATUS}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={10}
                onValueChange={inputChangeHandler}
                disabled={interactionMode} />
               
               }
               {
                isQuickSearch && <CustomInput
                hasLabel={true}
                name="firstVisitToDate"
                colSize='s3 clearMb pl-0 '
                label={localConstant.resourceSearch.FIRST_VISIT_DATE}
                type='date'
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                inputClass="customInputs"
                onDateChange={props.firstVisitChange}
                selectedDate={dateUtil.defaultMoment(moreDetailsData.firstVisitFromDate)}
                disabled={interactionMode}
                />
               }
               {isDashboardARSView &&  
                   <Fragment>
                       <LabelWithValue
                        className="custom-Badge labelLineHeight"
                        colSize="s3"
                        label={ `${ localConstant.resourceSearch.FIRST_VISIT_FROM_DATE } : ` }
                        // value={dateUtil.formatDate(moreDetailsData['firstVisitFromDate'],'-')}
                        value={moreDetailsData['firstVisitFromDate']&&moment(moreDetailsData['firstVisitFromDate']).format(localConstant.commonConstants.UI_DATE_FORMAT)}
                    /> 
                    <LabelWithValue
                        className="custom-Badge labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.FIRST_VISIT_TO_DATE } : ` }
                        value={moreDetailsData['firstVisitToDate']&&moment(moreDetailsData['firstVisitToDate']).format(localConstant.commonConstants.UI_DATE_FORMAT)}
                        // value={dateUtil.formatDate(moreDetailsData['firstVisitToDate'],'-')}
                    /> 
                    <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.FIRST_VISIT_TIMESHEETLOCATION } : ` }
                        value={moreDetailsData.firstVisitLocation ? moreDetailsData.firstVisitLocation : ""}
                    /> 
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.FIRST_VISIT_TIMESHEET_STATUS } : ` }
                        value={moreDetailsData.firstVisitStatus}
                    /> 
                   </Fragment>
               }
      </Fragment>

    );
};

export const TimesheetLocation = (props) => {
    const { interactionMode,moreDetailsData,inputChangeHandler,isTimesheet } = props;
    return (
        <div className={isTimesheet ? 'row mb-0' :''}>
            <CustomInput
            hasLabel={true}
            name="firstVisitLocation"
            colSize='s4 clearMb pl-4'
            label={localConstant.resourceSearch.TIMESHEET_LOCATION}
            type='text'
            dataValType='valueText'
            inputClass="customInputs"
            maxLength={255}
            value={moreDetailsData.firstVisitLocation}
            onValueChange={inputChangeHandler}
            disabled={interactionMode} />
        </div>
        
    );
};

export const Btn = (props) => {
    return (
        <div className="col s12 right-align" >
         {props.buttons && props.buttons.map((button,i)=>{
                       return( 
                        button.showbtn && <button type={button.type || null} disabled={button.disabled || props.interactionMode || null } id={button.btnID || null} key={i} className={button.btnClass} onClick={button.action} >{button.name}</button>
                       );                    
            })}
          
        </div>
    );
};
export const GridView = (props) => {
    return (
        <ReactGrid gridCustomClass={props.gridCustomClass}
            gridRowData={props.gridData} 
            gridColData={props.headerData} 
            onRef={props.gridRef}
            rowSelected={props.rowSelected}
            isGrouping={props.isGrouping} 
            rowClassRules={props.rowClassRules}
            groupName={props.gridGroupProps && props.gridGroupProps.groupName} 
            dataName={props.gridGroupProps && props.gridGroupProps.dataName} />
    );
};

export const SearchParameterLabelView = (props) => {
    const { optionalParamLabelData } = props;
    return (
        <Fragment>
            <h6 className="pl-0 bold bb-0 pb-0"><span> Search Parameter</span></h6>
       
        <div className="col s12 pl-0 pr-0 mb-2">
            <LabelWithValue
                className="custom-Badge col labelLineHeight"
                colSize="s1"
                label={ `${ localConstant.resourceSearch.TAXONOMY_INFORMATION } ` }
                //value={optionalParamLabelData.opCompanyName}
            />
            <LabelWithValue
                className="custom-Badge col labelLineHeight"
                colSize="s3"
                label={`${ localConstant.resourceSearch.CATEGORY } : `}
                value={optionalParamLabelData.categoryName}
            />
            <LabelWithValue
                className="custom-Badge col labelLineHeight"
                colSize="s3"
                label={`${ localConstant.resourceSearch.SUB_CATEGORY } : `}
                value={optionalParamLabelData.subCategoryName}
            />
            <LabelWithValue
                className="custom-Badge col labelLineHeight"
                colSize="s3"
                label={`${ localConstant.resourceSearch.SERVICES } : `}
                value={optionalParamLabelData.serviceName}
            />
        </div>
        </Fragment>

    );
};
export const OptionalParameter = (props) => {
    const { inputChangeHandler,isARSSearch,multiSelectOptionsList,defaultMultiSelection,optionAttributs,certificatesMultiSelectOptionsList,expiryFrom,expiryTo,searchGRMData,defaultCertificateMultiSelection } = props;
    return (
        <Fragment>
        <h6 className="pl-0 bold bb-0 pb-3"><span> Optional Parameter</span></h6>
        <div className="col s12 pl-0 pr-0 ">
                <MultiSelectField 
                    hasLabel={true}
                    name="equipmentDescription"
                    colSize='s4'
                    label={localConstant.resourceSearch.MATERIAL_DESCRIPTION}
                    type='multiSelect'
                    className='browser-default customInputs'
                    multiSelectOnChange={props.equipmentDescriptionMultiselectValueChange}
                    optionsList={createMultiSelectOptions(props.equipment)}
                    defaultValue={props.defaultEquipmentMultiSelection}
                    isDisabled={props.interactionMode}    
                />
                <MultiSelectField 
                    hasLabel={true}
                    name="customerName"
                    colSize='s4 '
                    label={localConstant.techSpec.Taxonomy.CUSTOMER_APPROVAL}
                    type='multiSelect'
                    className='browser-default customInputs'
                    optionsList={createMultiSelectOptions(props.taxonomyCustomerApproved)}
                    multiSelectOnChange={props.customerApprovalMultiselectValueChange}
                    defaultValue={props.defaultCustomerApprovalMultiSelection}
                    isDisabled={props.interactionMode}
                />
                <MultiSelectField
                    hasLabel={true}                    
                    name="certification"
                    colSize='s4 clearMb'
                    label={localConstant.resourceSearch.CERTIFICATION}
                    type='multiSelect'
                    className='browser-default customInputs'
                    multiSelectOnChange={props.certificationMultiSelectValueChange}
                    optionsList={createMultiSelectOptions(certificatesMultiSelectOptionsList)}
                    defaultValue={defaultCertificateMultiSelection}
                    isDisabled={props.interactionMode} 
                />
        </div>
        <div className="col s12 pl-0 pr-0 ">
            <CustomInput
                hasLabel={true}
                name="certificationExpiryFrom"
                colSize={isARSSearch ? 's12' : 's6' }
                label={localConstant.resourceSearch.EXPIRY_FROM}
                type='date'
                inputClass="customInputs"
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                onDateChange={expiryFrom}
                selectedDate={dateUtil.defaultMoment(searchGRMData.certificationExpiryFrom)} 
                disabled={props.interactionMode}/>
            <CustomInput
                hasLabel={true}
                name="certificationExpiryTo"
                colSize={isARSSearch ? 's12' : 's6' }
                label={localConstant.resourceSearch.EXPIRY_TO}
                type='date'
                inputClass="customInputs"
                placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                onDateChange={expiryTo}
                selectedDate={dateUtil.defaultMoment(searchGRMData.certificationExpiryTo)} 
                disabled={props.interactionMode}/>
               <h6 className="pl-0 bold"><span>{localConstant.resourceSearch.LANGUAGE_CAPABILITY}</span></h6>
                <LanguageParameter
                    inputChangeHandler={inputChangeHandler}
                    multiSelectOptionsList={multiSelectOptionsList}
                    defaultMultiSelection={defaultMultiSelection}
                    optionAttributs={optionAttributs}
                    languageSpeakingMultiSelectValueChange={props.languageSpeakingMultiSelectValueChange}
                    languageWritingMultiSelectValueChange={props.languageWritingMultiSelectValueChange}
                    languageComprehensionMultiSelectValueChange={props.languageComprehensionMultiSelectValueChange}
                    defaultLanguageSpeakingMultiSelection={props.defaultLanguageSpeakingMultiSelection}
                    defaultLanguageWritingMultiSelection={props.defaultLanguageWritingMultiSelection}
                    defaultLanguageComprehensionMultiSelection={props.defaultLanguageComprehensionMultiSelection}
                    interactionMode={props.interactionMode}
                    searchGRMData={searchGRMData}
                />
        </div>
        </Fragment>
    );
};
export const LanguageParameter = (props) => {
    const { inputChangeHandler,multiSelectOptionsList,defaultMultiSelection,optionAttributs,onValueChange,searchGRMData } = props;
    return (
        <Fragment>
        <div className="col s12 pl-0 pr-0">
            <MultiSelectField 
                hasLabel={true}
                name="languageSpeaking"
                colSize='s4 clearMb'
                label={localConstant.resourceSearch.SPEAKING}
                type='multiSelect'
                className='browser-default customInputs'
                multiSelectOnChange={props.languageSpeakingMultiSelectValueChange}
                optionsList={createMultiSelectOptions(multiSelectOptionsList)}
                defaultValue={props.defaultLanguageSpeakingMultiSelection}
                isDisabled={props.interactionMode}
            />
            <MultiSelectField 
                hasLabel={true}
                name="languageWriting"
                colSize='s4 clearMb'
                label={localConstant.resourceSearch.WRITING}
                type='multiSelect'
                multiSelectOnChange={props.languageWritingMultiSelectValueChange}
                optionsList={createMultiSelectOptions(multiSelectOptionsList)}
                defaultValue={props.defaultLanguageWritingMultiSelection}
                isDisabled={props.interactionMode}
            />
            <MultiSelectField 
                hasLabel={true}
                name="languageComprehension"
                colSize='s4 clearMb'
                label={localConstant.resourceSearch.COMPREHENSION}
                type='multiSelect'
                multiSelectOnChange={props.languageComprehensionMultiSelectValueChange}
                optionsList={createMultiSelectOptions(multiSelectOptionsList)}
                defaultValue={props.defaultLanguageComprehensionMultiSelection}
                isDisabled={props.interactionMode}
            />
        </div>
        <div className="col s12 pl-0 pr-0">
            <CustomInput
                hasLabel={true}
                name="radius"
                colSize='s4'
                label={localConstant.resourceSearch.RADIUS}
                type='text'
                inputClass="customInputs"
                maxLength={10}
                disabled = {false}
                onValueChange={inputChangeHandler}
                value={searchGRMData.radius ? searchGRMData.radius : ""}
                dataValType='valueText'
                readOnly={props.interactionMode} />
            <CustomInput
                hasLabel={true}
                name="searchInProfile"
                colSize='s4'
                label={localConstant.resourceSearch.SEARCH_IN_PROFILE}
                type='text'
                inputClass="customInputs"
                maxLength={200} 
                onValueChange={inputChangeHandler}
                value={searchGRMData.searchInProfile ? searchGRMData.searchInProfile : ""}
                dataValType='valueText'
                readOnly={props.interactionMode} />
        </div>
        </Fragment>
    );
};
export const Action =(props)=>{
    const { actionList,currentPage,isPreAssignmentPage,isARSSearch,inputChangeHandler,optionAttributs,
        actionData,dispositionTypeList,isQuickSearch,interactionMode,operationalManagers,technicalSpecialists,
        techSpecMultiSelectChangeHandler,defaultTechSpecMultiSelection,selectedMyTask,isDashboardARSView,
        hasRejectedResources,rejectedResourceValue,overrideTaskStatus } = props;
    let isCommentsMandate = false;

    if(hasRejectedResources && selectedMyTask.taskType === 'OM Verify and Validate')
        isCommentsMandate = true;
    if(actionData.dispositionType && actionData.dispositionType === 'OTH')
        isCommentsMandate = true;

    return(
        <div className="col s12">
            { (isDashboardARSView && selectedMyTask.taskType === 'OM Verify and Validate') ? 
                <div className={isDashboardARSView ? "col s8" : "col s12"}>
                 <ReactGrid 
                    gridRowData={props.overrideGridRowData} 
                    gridColData={props.overrideGridHeaderData} 
                    onRef={props.gridRef} />
                </div>                
            : null }
            {(isDashboardARSView && selectedMyTask.taskType === 'OM Verify and Validate')?
                null:
                <CustomInput
                    hasLabel={true}
                    name="searchAction"
                    id="actionId"
                    colSize={isPreAssignmentPage ? 's4 pl-0': 's3 pl-0'}
                    label={localConstant.resourceSearch.ACTION}
                    type='select'
                    optionsList={actionList}
                    labelClass="customLabel mandate"
                    optionName='name'
                    optionValue="value"
                    disabled={props.interactionMode}
                    inputClass="customInputs"
                    onSelectChange={inputChangeHandler}
                    defaultValue={props.defaultActionValue}
                    /> }
                {/* TO-DO:  Add isARSSearch in this condition */}
            {((isPreAssignmentPage||isQuickSearch||isARSSearch) && (actionData.searchAction === "SD" || actionData.searchAction === "PLO" || actionData.searchAction === "L" || (selectedMyTask && selectedMyTask.taskType==="PLO to RC"))) ?
                <CustomInput
                    hasLabel={true}
                    name="dispositionType"
                    id="dispositionId"
                    colSize={isPreAssignmentPage ? 's4 pl-0': 's3 pl-0'}
                    label={localConstant.resourceSearch.DISPOSITION_DETAILS}
                    optionsList={dispositionTypeList}
                    labelClass="customLabel mandate"
                    optionName='name'
                    optionValue="code"
                    disabled={(selectedMyTask && selectedMyTask.taskType==="PLO to RC")|| props.interactionMode ?true:false}
                    type='select'
                    inputClass="customInputs"
                    onSelectChange={inputChangeHandler} 
                    defaultValue={(selectedMyTask && selectedMyTask.taskType==="PLO to RC") || isARSSearch?actionData.dispositionType:""}
                    />:null
            }
            {(isARSSearch && actionData.searchAction === 'OPR')?
                <div>
                    <CustomInput
                           hasLabel={true}
                           labelClass="mandate"
                           name="listOfTS"
                           colSize='s3 pl-0 clearMb'
                           label={localConstant.resourceSearch.LIST_OF_TS}
                           type='multiSelect'                          
                           multiSelectdValue={techSpecMultiSelectChangeHandler}
                           optionsList={createTSMultiSelectOptions(technicalSpecialists)}
                           defaultValue={defaultTechSpecMultiSelection} 
                           />
                    <CustomInput
                        hasLabel={true}
                        name="assignedToOmLognName"
                        id="assignedToOmLognNameId"
                        colSize='s3 pl-0'
                        label={localConstant.resourceSearch.LIST_OF_OM}
                        optionsList={operationalManagers}
                        labelClass="customLabel mandate"
                        optionName='displayName'
                        optionValue="username"
                        disabled={false}
                        type='select'
                        inputClass="customInputs"
                        defaultValue = {actionData.assignedToOmLognName}
                        onSelectChange={inputChangeHandler}/>
                </div>
                : null
            }
            {((isPreAssignmentPage||isQuickSearch||isARSSearch) && ((!required(actionData.searchAction) && (actionData.searchAction !== "CUP" && actionData.searchAction !== "SS")|| (selectedMyTask && selectedMyTask.taskType === 'OM Verify and Validate')))) ? 
            <CustomInput
                hasLabel={true}
                name="description"
                divClassName='pl-0'
                colSize={isPreAssignmentPage ? 's4': 's3'}
                label={localConstant.resourceSearch.COMMOENTS}
                labelClass={isCommentsMandate ? "customLabel mandate" : "customLabel"}
                type='textarea'
                inputClass="customInputs customtextHeight"
                maxLength={255}
                value={actionData.description}
                onValueChange={inputChangeHandler}
                // disabled={interactionMode}
                readOnly = {interactionMode} /> : null}
            {(isARSSearch && hasRejectedResources && selectedMyTask && selectedMyTask.taskType !== 'OM Verify and Validate') ?
            <CustomInput
                hasLabel={true}
                name="rejectedResources"
                divClassName='pl-0'
                colSize='s3'
                label={localConstant.resourceSearch.REJECTED_RESOURCES}
                labelClass={"customLabel mandate"}
                type='textarea'
                inputClass="customInputs customtextHeight"
                maxLength={255}
                value={rejectedResourceValue}
                onValueChange={inputChangeHandler}
                // disabled={hasRejectedResources}
                readOnly = {hasRejectedResources} /> : null }
            <div>{ isARSSearch && <a className="right link" onClick={props.commentsHistoryClickHandler}>{ localConstant.resourceSearch.COMMENTS_HISTORY_REPORT }</a> }</div>
        </div>
    );
};

export const ExportBtn = (props) => {
    return (
        <div className="col s12 right-align pr-4" >
         {props.exportButtons && props.exportButtons.map((button,i)=>{
                       return( 
                        button.showbtn && <button type={button.type || null} disabled={props.isShowMapBtn === true ? false : button.disabled || null } id={button.btnID || null} key={i} className={button.btnClass} onClick={button.action} >{button.name}</button>
                       );                    
            })}
          
        </div>
    );
};

export const ResourceAssigned = (props) => {
    const { inputChangeHandler,interactionMode,assignedResourceData,isResourceNotMatched,isDashboardARSView } = props;
    return(
        <Fragment>
            {(!isDashboardARSView) && 
                    <div className="col s12">
                    {isResourceNotMatched ? <span className="dashboardInfo col s6">Resources on Pre-Assignment not meeting the assignment search criteria</span> : null}
                    <CustomInput
                        hasLabel={true}
                        name="supplierLocation"
                        divClassName='pl-0'
                        colSize='s6'
                        label={localConstant.resourceSearch.ASSIGNED_RESOURCE}
                        labelClass="customLabel mandate"
                        type='textarea'
                        inputClass="customInputs"
                        maxLength={255}
                        value={assignedResourceData}
                        onValueChange={inputChangeHandler}
                        // disabled={interactionMode}
                        readOnly = {interactionMode} />
                    </div>
        }
       {(isDashboardARSView) && <Fragment>
                         
                        <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s3 pl-0"
                        label={ `${ localConstant.resourceSearch.CH_COMPANY } : ` }
                        value={assignedResourceData}
                    />
                    </Fragment>
       }
        </Fragment>
        
    );
};

export const CompanyCoordinater = (props) => {
    const { inputChangeHandler,interactionMode,moreDetailsData,companyList,contractHoldingCoodinatorList,operatingCoordinatorList,
        isDashboardARSView,opCoordinator,chCoordinator } = props;
    return (
        <div className="col s12">
           {!isDashboardARSView && <Fragment>
            <CustomInput
                    hasLabel={true}
                    name="chCompanyCode"
                    id="chCompanyCodeId"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.CH_COMPANY}
                    optionsList={companyList}
                    labelClass="customLabel mandate"
                    optionName='companyName'
                    optionValue="companyCode"
                    disabled={interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={moreDetailsData.chCompanyCode}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="opCompanyCode"
                    id="opCompanyCodeId"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.OPERATING_COMPANY}
                    optionsList={companyList}
                    labelClass="customLabel mandate"
                    optionName='companyName'
                    optionValue="companyCode"
                    disabled={interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={moreDetailsData.opCompanyCode}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="chCoordinatorLogOnName"
                    id="chCoordinatorLogOnNameId"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.CH_COORDINATOR}
                    optionsList={contractHoldingCoodinatorList}
                    labelClass="customLabel mandate"
                    optionName='displayName'
                    optionValue="displayName"
                    disabled={interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={moreDetailsData.chCoordinatorLogOnName}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="opCoordinatorLogOnName"
                    id="opCoordinatorLogOnNameId"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.OC_COORDINATOR}
                    optionsList={operatingCoordinatorList}
                    labelClass="customLabel mandate"
                    optionName='displayName'
                    optionValue="displayName"
                    disabled={interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={moreDetailsData.opCoordinatorLogOnName}
                    onSelectChange={inputChangeHandler} />
            </Fragment>
           }
                   { isDashboardARSView &&  
                      <Fragment>
                        <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s6 pl-0"
                        label={ `${ localConstant.resourceSearch.CH_COMPANY } : ` }
                        value={moreDetailsData.chCompanyName}
                    />
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s6 pl-0"
                        label={ `${ localConstant.resourceSearch.OPERATING_COMPANY } : ` }
                        value={moreDetailsData.opCompanyName}
                    />
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s6 pl-0"
                        label={ `${ localConstant.resourceSearch.CH_COORDINATOR } : ` }
                        value={chCoordinator}
                    />
                     <LabelWithValue
                        className="custom-Badge col labelLineHeight"
                        colSize="s6 pl-0"
                        label={ `${ localConstant.resourceSearch.OC_COORDINATOR }:` }
                        value={opCoordinator}
                    />
                      </Fragment>
                    
                   }
        </div>
    );
};

export const PLOSearchGrm = (props) => {
    const { moreDetailsData,inputChangeHandler,taxonomyCategory,taxonomySubCategory,taxonomyServices,interactionMode } = props;
    const ploTaxonomy = Object.assign({},moreDetailsData.ploTaxonomyInfo);
    return(
        <div className="col s12 pl-0 pr-0">
            <CustomInput
                hasLabel={true}
                name="categoryName"
                id="categoryId"
                colSize='s4'
                label={localConstant.resourceSearch.CATEGORY}
                type='select'
                optionsList={taxonomyCategory}
                labelClass="customLabel"
                optionName='name'
                optionValue="name"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {ploTaxonomy.categoryName}
                onSelectChange={inputChangeHandler}/>
            <CustomInput
                hasLabel={true}
                name="subCategoryName"
                id="subCategoryId"
                divClassName='pl-0'
                colSize='s4'
                label={localConstant.resourceSearch.SUB_CATEGORY}
                type='select'
                optionsList={taxonomySubCategory}
                labelClass="customLabel"
                optionName='taxonomySubCategoryName'
                optionValue="taxonomySubCategoryName"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {ploTaxonomy.subCategoryName}
                onSelectChange={inputChangeHandler} />
            <CustomInput
                hasLabel={true}
                name="serviceName"
                id="serviceId"
                colSize='s4 pl-0'
                label={localConstant.resourceSearch.SERVICES}
                type='select'
                optionsList={taxonomyServices}
                labelClass="customLabel"
                optionName='taxonomyServiceName'
                optionValue="taxonomyServiceName"
                disabled={interactionMode}
                inputClass="customInputs"
                defaultValue = {ploTaxonomy.serviceName}
                onSelectChange={inputChangeHandler} />
        </div>
    );
};

// export const CustomerApproval =(props)=>{
//     return(
//         <div className='col s4 pl-0 pr-0'>
//               <CustomInput
//                     hasLabel={true}
//                     name="customerName"
//                     colSize='s12 '
//                     label={localConstant.techSpec.Taxonomy.CUSTOMER_APPROVAL}
//                     type='multiSelect'
//                     className='browser-default customInputs'
//                     optionsList={createMultiSelectOptions(props.taxonomyCustomerApproved)}
//                     multiSelectdValue={props.customerApprovalMultiselectValueChange}
//                     defaultValue={props.defaultCustomerApprovalMultiSelection}
//                     /> 
//         </div>
//     );
// };

function createMultiSelectOptions(array) {
    const options = isEmptyReturnDefault(array);
    return  options.map(eachItem => {
              return { value: eachItem.name, label: eachItem.name };
          });
}

function createTSMultiSelectOptions(array) {
    const options = isEmptyReturnDefault(array);
    return  options.map(eachItem => {
              return { value: eachItem.epin, label:  `${ eachItem.fullName }( ${ eachItem.epin } )` };
          });
}