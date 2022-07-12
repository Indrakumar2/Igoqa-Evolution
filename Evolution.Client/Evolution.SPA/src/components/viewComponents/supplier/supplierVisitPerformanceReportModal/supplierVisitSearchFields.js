import React, { Fragment } from 'react';
import InputWithPopUpSearch from '../../../applicationComponents/inputWithPopUpSearch';
import CustomerAndCountrySearch from '../../../applicationComponents/customerAndCountrySearch';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomInput from '../../../../common/baseComponents/inputControlls';
const localConstant = getlocalizeData();

export const SupplierVisitSearchFields = (props) => {
    return (
<Fragment>
<div className="row">

<CustomerAndCountrySearch 
    isSupplier={true} 
    ClearReportsData={ props.ClearReportsData } 
    defaultReportCustomerName={ props.defaultReportCustomerName } 
    reportsCustomerName={props.reportsCustomerName} 
    isReport={true} 
    selectedReportCountryName={props.selectedReportCountryName}
    OnChangeSearchCustomer={props.OnChangeSearchCustomer} 
    divClassName="s6"
    colSize="col s6 pl-0 pr-0"  />

<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='ContractNumber'
    divClassName='col'
    label={ localConstant.contract.CONTRACT_NUMBER }
    type='text'
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    onValueChange={ props.handlerChange }
    autocomplete="off"
/>
</div>
<div class='row'>
<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='CHCompanyName'
    divClassName='col'
    label={localConstant.contract.CONTRACT_HOLDING_COMPANY}
    type='select'
    onSelectChange={props.handlerChange}
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    optionsList={props.sortedCompanyList}
    optionName='companyName'
    optionValue="companyName"
    autocomplete="off"
/>

<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='OCCompanyName'
    divClassName='col'
    label={ localConstant.assignments.OPERATING_COMPANY }
    type='select'
    onSelectChange={props.handlerChange}
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    optionName='companyName'
    optionValue="companyName"
    optionsList={ props.sortedCompanyList }
    autocomplete="off"
/>
</div>
<div class='row'>
<InputWithPopUpSearch
    divClassName="s6"
    colSize="col s6 pl-0 pr-0"
    label={localConstant.supplier.SUPPLIER}
    dataKey="supplierList"
    name="supplierPOMainSupplierName"
    searchModalTitle={ localConstant.supplierpo.SUPPLIER_LIST }
    gridRowData={props.supplierList}
    headerData={ props.headerData.supplierSearchHeader }
    defaultValue=""
    cancelSupplierVisit={props.cancelSupplierVisit}
    onAddonBtnClick={ props.supplierPopupOpen }
    onModalSelectChange={ props.getMainSupplier }
    onInputBlur={ props.getMainSupplier }
    onSubmitModalSearch={ props.handleSupplier }
    objectKeySelector="supplierName"
    columnPrioritySort={props.defaultSort}
/>

<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='ProjectNumber'
    divClassName='col'
    label={localConstant.project.PROJECT_NUMBER}
    type='text'
    dataType= 'numeric' //Changes for IGO - D901
    onValueChange={props.handlerChange}
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    autocomplete="off"
/>
</div>
<div class='row'>
<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='AssignmentNumber'
    divClassName='col'
    label={localConstant.assignments.ASSIGNMENT_NUMBER}
    type='text'
    dataType= 'numeric'
    onValueChange={props.handlerChange}
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    autocomplete="off"
/>
<CustomInput
    hasLabel={true}
    labelClass="customLabel"
    name='SupplierPONumber'
    divClassName='col'
    label={localConstant.supplier.SUPPLIER_PO}
    type='text'
    onValueChange={props.handlerChange}
    colSize='s12 m6'
    inputClass="customInputs"
    required={true}
    autocomplete="off"
/>
</div>
<div class='row'>
<label class='bold'>{localConstant.supplier.NCR_STATUS}</label>
<label className='pr-3'>
    <input className="with-gap" value='open' onChange={props.handlerChange} name="NCR" type="radio" defaultChecked={true} ref={ props.NcrRef }/>
    <span>{localConstant.supplier.OPEN}</span>
</label>
<label className='pr-3'>
    <input className="with-gap" value='closed' onChange={props.handlerChange} name="NCR" type="radio" />
    <span>{localConstant.supplier.CLOSED}</span>
</label>
<label className='pr-3'>
    <input className="with-gap" value='both' onChange={props.handlerChange} name="NCR" type="radio" />
    <span>{localConstant.supplier.BOTH}</span>
</label>
</div>
<div class='row'>
<label class='bold'>{localConstant.supplier.FORMAT}</label>
<label>
    <input className="with-gap" onChange={props.handlerChange} value='excel' name="Format" type="radio"  defaultChecked={true}/>
    <span>{localConstant.supplier.EXCEL}</span>
</label>
</div>
    </Fragment>
    );
};