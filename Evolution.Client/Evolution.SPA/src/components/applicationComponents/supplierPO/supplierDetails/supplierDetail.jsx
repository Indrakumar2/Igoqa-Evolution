import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData, isEmpty, isEmptyReturnDefault, formInputChangeHandler, bindAction, numberFormat,ObjectIntoQuerySting, thousandFormat } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import { HeaderData } from './supplierDetailHeader';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import dateUtil from '../../../../utils/dateUtil';
import InputWithPopUpSearch from '../../../applicationComponents/inputWithPopUpSearch';
import ProjectAnchor from '../../../viewComponents/projects/projectAnchor';
import SupplierAnchor from '../../../viewComponents/supplier/supplierAnchor';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import moment from 'moment';
import BudgetMonetary from '../../budgetMonetary';

const localConstant = getlocalizeData();

const GeneralDetails = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.supplierpo.GENERAL_DETAILS} colSize="s12">
            <div className="row mb-0">
                <LabelWithValue
                    className="custom-Badge col textNoWrapEllipsis"
                    colSize="s5"
                    label={`${ localConstant.supplierpo.CUSTOMER }:`}
                    value={props.generalDetailsData !== undefined ? props.generalDetailsData.supplierPOCustomerName : ""}
                />
                <LabelWithValue
                    className="custom-Badge col"
                    colSize="s5"
                    label={` ${
                        localConstant.supplierpo.CUSTOMER_PROJECT_NAME
                        }: `}
                    value={props.generalDetailsData !== undefined ? props.generalDetailsData.supplierPOCustomerProjectName : ""}
                />
                <div className="custom-Badge col s2 bold">{localConstant.supplierpo.PROJECT_NO + ': '}
                    <ProjectAnchor
                        value={props.generalDetailsData.supplierPOProjectNumber} 
                        className="link"  //Added for D-479 Issue1
                        />  
                </div>
            </div>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={localConstant.supplierpo.SUPPLIER_PO_NUMBER}
                    divClassName="m6"
                    type='text'
                    refProps='supplierNameId'
                    name="supplierPONumber"
                    autocomplete="off"
                    dataValType='valueText'
                    value={props.generalDetailsData && props.generalDetailsData.supplierPONumber ? props.generalDetailsData.supplierPONumber : ""}
                    readOnly={props.interactionMode || props.isAssignmentOpenedAsOC}   //For D-456
                    // disabled={props.interactionMode}
                    colSize='s6'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.supplierpo.supplierDetails.SUPPLIER_PO_NUMBER_MAXLENGTH}
                    onValueChange={props.handleInputChange}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    label={localConstant.supplierpo.METERIAL_DESCRIPTION}
                    divClassName="m6"
                    type='text'
                    refProps='supplierNameId'
                    name="supplierPOMaterialDescription"
                    dataValType='valueText'
                    autocomplete="off"
                    value={props.generalDetailsData && props.generalDetailsData.supplierPOMaterialDescription ?
                        props.generalDetailsData.supplierPOMaterialDescription : ""}
                    readOnly={props.interactionMode  || props.isAssignmentOpenedAsOC}   //For D-456
                    // disabled={props.interactionMode}
                    colSize='s6'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.supplierpo.supplierDetails.MATERIAL_DESCRIPTION_MAXLENGTH}
                    onValueChange={props.handleInputChange}
                />
            </div>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    label={localConstant.supplierpo.DELIVERY_DATE}
                    labelClass="customLabel"
                    colSize='s4'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDatePickBlur={props.handleOtherDetailStartDateBlur}
                    type='date'
                    name='supplierPODeliveryDate'
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.generalDetailsData.supplierPODeliveryDate)}
                    onDateChange={props.deliveryDateChange}
                    disabled={props.interactionMode || props.isAssignmentOpenedAsOC}   //For D-456
                />
                <CustomInput
                    hasLabel={true}
                    labelClass={props.supplierPoStatus === "C" ? "mandate" : "customLabel"}
                    label={localConstant.supplierpo.COMPLETED_DATE}
                    colSize='s4'
                    type="date"
                    isNonEditDateField={false}
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.generalDetailsData.supplierPOCompletedDate)}
                    onDateChange={props.completedDateChange}
                    name='supplierPOCompletedDate'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    disabled={props.interactionMode || props.isAssignmentOpenedAsOC} //For D-456
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.contract.STATUS}
                    type='select'
                    colSize='s4'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={localConstant.commonConstants.supplierStatus}
                    optionName='name'
                    optionValue='value'
                    inputClass="customInputs"
                    name="supplierPOStatus"
                    disabled={props.interactionMode || props.isAssignmentOpenedAsOC}   //For D-456
                    defaultValue={props.generalDetailsData && props.generalDetailsData.supplierPOStatus ? props.generalDetailsData.supplierPOStatus : "O"}
                    onSelectChange={props.handleInputChange}
                />
            </div>

        </CardPanel>
    );
};
const SubSuppliers = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2"
            title={localConstant.supplierpo.SUB_SUPPLIER} colSize="s12">
           
                <ReactGrid gridColData={props.headerData.subSupplierDetailHeader}
                    gridRowData={props.rowData}
                    paginationPrefixId={localConstant.paginationPrefixIds.subSupplier}
                    onRef={props.onRef} />
          
             {(props.pageMode !== localConstant.commonConstants.VIEW || props.isAssignmentOpenedAsOC) && <div className="right-align mt-2">
                <a onClick={props.subSupplierCreateHandler}
                    className="btn-small waves-effect waves-teal"
                    disabled={ props.isAssignmentOpenedAsOC ? true : false }    //For D-456
                >{localConstant.commonConstants.ADD}</a>
                <a href="#confirmation_Modal" onClick={props.subSupplierDeleteHandler}
                   className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                    disabled={(props.showButton || props.isAssignmentOpenedAsOC) ? true : false}>{localConstant.commonConstants.DELETE}</a>
             </div> }
        </CardPanel>
    );
};
const MainSupplier = (props) => {
    
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.supplierpo.MAIN_SUPPLIER} colSize="s12">
            <div className="row">
                <div className="col s6">
                    <div className="row">
                        <InputWithPopUpSearch
                            colSize='col s12 pl-0'
                            className="mandate"
                            label={localConstant.supplierpo.MAIN_SUPPLIER}
                            headerData={props.headerData.supplierSearchHeader}
                            name="supplierPOMainSupplierName"
                            searchModalTitle={localConstant.supplierpo.SUPPLIER_LIST}
                            gridRowData={props.modalRowData}
                            defaultInputValue={props.budgetMonitaryDetails && props.budgetMonitaryDetails.supplierPOMainSupplierName ?
                                               props.budgetMonitaryDetails.supplierPOMainSupplierName : ""}
                            onAddonBtnClick={props.supplierPopupOpen}
                            onModalSelectChange={props.getMainSupplier}
                            onInputBlur={props.getMainSupplier}
                            onSubmitModalSearch={props.getSelectedMainSupplier}
                            objectKeySelector="supplierName"
                            columnPrioritySort={props.columnPrioritySort}
                            interactionMode={props.interactionMode || props.isAssignmentOpenedAsOC }   //For D-456
                            disabled={ props.currentPage === "addSupplierPO" ? false : true}
                            handleInputChange={props.handleInputChange}
                            callBackFuncs={props.callBackFunc}
                        />
                   <div className="col s6">
                            {props.budgetMonitaryDetails && isEmpty(props.budgetMonitaryDetails.supplierPOMainSupplierName) ? null :
                                ((props.budgetMonitaryDetails && props.budgetMonitaryDetails.supplierPOMainSupplierId)) ?
                                <SupplierAnchor data={{
                                    "supplierName": "Click Here To View Supplier", 
                                    "supplierId":
                                        props.budgetMonitaryDetails && props.budgetMonitaryDetails.supplierPOMainSupplierId,
                                        "currentPage":localConstant.supplier.EDIT_VIEW_SUPPLIER                                      
                                }} /> : <a  href='javascript:void(0)' className='link isDisabled waves-effect' >{ 'Click Here To View Supplier' }</a>
                            }
                        </div>
                    </div>
                </div>
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.supplier.FULL_ADDRESS}
                    divClassName="m6"
                    type='textarea'
                    name='otherContactDetails'
                    colSize='m6'
                    maxLength={fieldLengthConstants.supplierpo.supplierDetails.SUPPLIER_FULL_ADDRESS_MAXLENGTH}
                    inputClass="customInputs"
                    // disabled={true}
                    readOnly = {true}
                    value={props.budgetMonitaryDetails && props.budgetMonitaryDetails.supplierPOMainSupplierAddress ?
                        props.budgetMonitaryDetails.supplierPOMainSupplierAddress : ""}
                    onValueChange={props.handleInputChange}
                />
            </div>
        </CardPanel>
    );
};
const SubSupplierPopup = (props) => {
    return (
        <Fragment>
            <InputWithPopUpSearch
                colSize='col s4 pl-0'
                label={localConstant.supplier.SUPPLIER_NAME}
                headerData={props.headerData.supplierSearchHeader}
                name="subSupplierName"
                searchModalTitle={localConstant.supplierpo.SUPPLIER_LIST}
                gridRowData={props.modalRowData}
                defaultInputValue={props.editedData && props.editedData.subSupplierName}
                onAddonBtnClick={props.supplierPopupOpen}
                onModalSelectChange={props.getMainSupplier}
                onInputBlur={props.getMainSupplier}
                onSubmitModalSearch={props.getSelectedSupplier}
                objectKeySelector="supplierName"
                columnPrioritySort={ props.columnPrioritySort }
                
                isShowSupplierModal={ props.isShowSupplierModal } // D-623 Added to resolve multiclick issue in subsuppliersearch 
                cancelSubSupplierPopUp= { props.cancelSubSupplierPopUp }
                ClearSearchResult = { props.ClearSearchResult }
                isShowSupplierSearchButton={ true }
            />
        </Fragment>
    );
};

class supplierPoDetail extends Component {

    constructor(props) {
        super(props);
        this.updatedData = {};
        this.editedRow = {};
        this.state = {
            isSupplierPOContactOpen: false,
            isSupplierPOContactEdit: false,
            deliveryDate: '',
            completedDate: '',
            enableSupplierLink: true,
            selectedSupplier: {},
            supplierName:'' //Added for D-479 Issue1
        };
        this.subSupplierSubmitButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSupplierPoContact,
                type: 'reset',
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type: 'submit',
                btnID: "createRateSchedule",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.callBackFunc ={
            onCancel:()=>{}
          };
        this.updatedData = {};
        this.functionRefs = {};  //D456 issue      
        this.functionRefs["disableEditColumn"]=(e)=>this.disableEditColumn(e);
        this.headerData = HeaderData(this.functionRefs);
        props.callBackFuncs.onCancel =()=>{
            this.cancelSupplierPoChanges();
        };
    }
    cancelSupplierPoChanges =() =>{
        this.callBackFunc.onCancel();
    }
    disableEditColumn = (e) => {  //D456 issue      
        return this.props.pageMode ? true :this.props.supplierPOViewMode ? true:false;
    }
    subSupplierSubmitHandler = (e) => {
        e.preventDefault();
        if (this.state.isSupplierPOContactEdit) {
            if (this.editedRow.recordStatus !== 'N') {
                this.updatedData['recordStatus'] = 'M';
            }
            const editedData = Object.assign({}, this.editedRow, this.updatedData);
            this.props.actions.UpdateSubSupplier(editedData);
        }
        this.setState({
            isSupplierPOContactOpen: false
        });
        this.updatedData = {};
    }

    subSupplierCreateHandler = () => {
        this.setState({
            isSupplierPOContactOpen: true,
            isSupplierPOContactEdit: false,
        });
        this.editedRow = {};
    }

    subSupplierDeleteHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.SUPPLIER_PO_CONTACT_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSupplierPoContact,
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
        else {
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE, 'warningToast supplierPoContactDeleteToaster');
        }
    }

    deleteSupplierPoContact = () => {
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteSubSupplier(selectedRecords);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    cancelSupplierPoContact = () => {
        this.updatedData = {};
        this.setState({
            isSupplierPOContactOpen: false,
            isSupplierPOContactEdit: false,
        });
        this.editedRow = {};
    }
    ClearSearchResult=() => {
        this.props.actions.ClearSupplierSearchList();
    }

    editSubSupplier = (data) => {
        this.setState((state) => {
            return {
                isSupplierPOContactOpen: !state.isSupplierPOContactOpen,
                isSupplierPOContactEdit: true
            };
        });
        this.editedRow = data;
    }
    /**On Change Handler for Sub-Supplier details data */
    supplierPoOnChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }

    getMainSupplier = (data) => { 
            const params = { 
                supplierName: data.serachInput,
                country: data.selectedCountry
            };
            this.setState({
                supplierName: data.serachInput
            }); //Added for D-479 Issue1
        if (data.serachInput || data.selectedCountry ) //Def 752
        {
            this.props.actions.FetchSupplierSearchList(params);
        }
        else { //Def 752
            this.props.actions.ClearSupplierSearchList();
        }
    }

    supplierPopupOpen = (data) => {
        this.props.actions.ClearSupplierSearchList();
    }

    getSelectedMainSupplier = (data) => {
        if (data) {
            this.setState({
                selectedSupplier: data[0],
                enableSupplierLink: false
            });
            const params = {
                supplierPOMainSupplierName: data[0] && data[0].supplierName,
                supplierPOMainSupplierAddress: data[0] && data[0].supplierAddress,
                supplierPOMainSupplierId: data[0] && data[0].supplierId
            };
            this.props.actions.updateSupplierDetails(params);
        }
    }
    handleInputChange = (e) => {

        // restriction of field length to supplier po budget and supplier po budget hours
        if(e.target.name === 'supplierPOBudget' ||  e.target.name === 'supplierPOBudgetHours')
        {
            if(e.target.value)
            {
                //const value = e.target.value.toString().split('.')[0];
                const value = e.target.value.toString();
                if(value.length >16)
                {
                    e.target.value = value.substring(0,16);
                }
                // e.target.value=parseInt(e.target.value);
            }
           
        }
        
        // restriction of field length to supplier po budget warning and supplier po budget hours warning
        if(e.target.name === 'supplierPOBudgetWarning' ||  e.target.name === 'supplierPOBudgetHoursWarning')
        {
            if(e.target.value)
            {
                const value = e.target.value.toString();
                if(value.length >2)
                {
                    e.target.value = value.substring(0,2);
                }
                e.target.value=parseInt(e.target.value);
            }
            
        }

        this.updatedData[e.target.name] = e.target.value;
        this.props.actions.updateSupplierDetails(this.updatedData);
        this.updatedData = {};
    }
    /** Handler to make the entered decimal data as two digit */
    checkNumber = (e) => {

        if(!isEmpty(e.target.value) && e.target.value >0)
        {
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        }
        else{
            e.target.value = '00.00';
        }
        this.updatedData[e.target.name] = e.target.value;
        this.handleInputChange(e);
    }
    
    warningBlurHandler =(e) =>{
        // if(!e.target.value)
        // {
        //     e.target.value =75;
        // }
        this.updatedData[e.target.name] = e.target.value;
    }
    deliveryDateChange = (date) => {
        this.setState({
            deliveryDate: date
        }, () => {
            const data = {
                supplierPODeliveryDate: this.state.deliveryDate ?moment.parseZone(this.state.deliveryDate).utc().format():null
            };
            this.props.actions.updateSupplierDetails(data);
        });
    }
    completedDateChange = (date) => {
        this.setState({
            completedDate: date
        }, () => {
            const data = {
                supplierPOCompletedDate: this.state.completedDate ?moment.parseZone(this.state.completedDate).utc().format():null
            };
            this.props.actions.updateSupplierDetails(data);
        });
    }
        
    //handler function on supplier detail page
    handleSupplierDetailRedirection = () => {
        this.props.actions.GetSelectedSupplier(this.state.selectedSupplier);
    }

    viewSubSupplier = (data) => {

        const queryObj={            
            subSupplierId:data.subSupplierId,
            supplierPOId:data.supplierPOId,           
            supplierId:data.supplierId,
            
            };
        // this.props.actions.GetSelectedSupplier(data);
        // this.props.actions.UpdateCurrentPage('Edit/View Supplier');
         const queryStr = ObjectIntoQuerySting(queryObj);
        window.open('/supplierDetails?'+ queryStr, '_blank');
        //this.props.history.push('/supplierDetails');
    }

    addSubSupplier = (data) => {
        if (data && Array.isArray(data) && data.length > 0) {
            let isExist = false;
            if (this.state.isSupplierPOContactEdit) {
                if (this.props.subSupplierData.length > 0) {
                    this.props.subSupplierData.forEach(subSupplier => {
                        if (subSupplier.supplierId == data[0].supplierId) {
                            return isExist = true;
                        }
                    });
                }
                if (isExist === true) {
                    IntertekToaster(localConstant.validationMessage.DUPLICATE_SUB_SUPPLIER, 'warningToast duPsuBsuPPlierValid');
                }
                else {
                    if (this.editedRow.supplierId !== data[0].supplierId) {                
                        if (this.editedRow.recordStatus !== 'N') {
                            this.updatedData['recordStatus'] = 'M';
                        }
                        this.updatedData["subSupplierAddress"] = data[0].supplierAddress;
                        this.updatedData["supplierId"] = data[0].supplierId;
                        this.updatedData["subSupplierName"] = data[0].supplierName;
                        const editedData = Object.assign({}, this.editedRow, this.updatedData);
                        this.props.actions.UpdateSubSupplier(editedData);
                    }
                }
            }
            else {
                if (this.props.subSupplierData.length > 0) {
                    this.props.subSupplierData.forEach(subSupplier => {
                        if (subSupplier.supplierId == data[0].supplierId) {
                            return isExist = true;
                        }
                    });
                }
                if (isExist === true) {
                    IntertekToaster(localConstant.validationMessage.DUPLICATE_SUB_SUPPLIER, 'warningToast duPsuBsuPPlierValid');
                }
                else {
                    this.updatedData = {};
                    this.updatedData['recordStatus'] = 'N';
                    this.updatedData["subSupplierId"] = null;
                    this.updatedData["supplierPOId"] = this.props.supplierPoDetails ? this.props.supplierPoDetails.supplierPOId : null;
                    this.updatedData["subSupplierUniqueIdentifier"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["subSupplierAddress"] = data[0].supplierAddress;
                    this.updatedData["subSupplierName"] = data[0].supplierName;
                    this.updatedData["supplierId"] = data[0].supplierId;
                    this.updatedData["modifiedBy"] = this.props.loggedInUser;
                    this.props.actions.AddSubSupplier(this.updatedData);
                    this.updatedData = {};
                    this.setState({
                        isSupplierPOContactOpen: false
                    });
                }
            }
        }
    }
    // Unnecessary Call No where used this Action Value.    
    // componentDidMount(){   
    //      this.props.actions.FetchProjectDetailForSupplier();
    // }
    render() {
        const supplierPoDetails = isEmptyReturnDefault(this.props.supplierPoDetails, 'object');
        const defaultSort = [
            { "colId": "supplierName",
                "sort": "asc" },
        ];
     
        const monetaryValues= {
            hasLabel:true,
            divClassName:'col',
            label:localConstant.budget.VALUE,
            type:'text',
            dataType:'decimal',
            valueType:'value',
            colSize:'s6',
            inputClass:'customInputs',
            labelClass:'customLabel mandate',
            max:'99999999999',
            required:true,
            name:'supplierPOBudget',
            maxLength: fieldLengthConstants.common.budget.BUDGET_VALUE_MAXLENGTH,
            min:'0',
            prefixLimit: fieldLengthConstants.common.budget.BUDGET_VALUE_PREFIX_LIMIT,
            suffixLimit: fieldLengthConstants.common.budget.BUDGET_VALUE_SUFFIX_LIMIT,
            isLimitType:true,
            readOnly:this.props.interactionMode || this.props.supplierPOViewMode,  //For D-456
            // disabled:this.props.interactionMode,
            value:supplierPoDetails.supplierPOBudget?thousandFormat(supplierPoDetails.supplierPOBudget):'', //Changes for D623 issue5
            onValueChange:this.handleInputChange
        };
        
        const monetaryWarning ={
            hasLabel:true,
            divClassName:'col',
            label:localConstant.budget.WARNING,
            type:'number',
            dataValType:'valueText',
            colSize:'s3',
            inputClass:'customInputs',
           // labelClass:'customLabel mandate',
            required:true,
            name:'supplierPOBudgetWarning',
            maxLength:fieldLengthConstants.supplierpo.supplierDetails.SUPPLIERPO_BUDGET_WARNING_MAXLENGTH,
            max:'100',
            min:'0',
            readOnly: this.props.interactionMode  || this.props.supplierPOViewMode, //For D-456
            // disabled:this.props.interactionMode,
            value:supplierPoDetails.supplierPOBudgetWarning,
            onValueChange:this.handleInputChange,
            onValueBlur:this.warningBlurHandler
        };

        const monetaryCurrency ={
            label:`${ localConstant.budget.CURRENCY }`,
            value:supplierPoDetails.supplierPOCurrency,
            hasLabel:true,
            divClassName:'col',
            colSize:'s3 mt-4',
            className:'browser-default',
            labelClass:'mandate',
            name:"supplierPOCurrency",
            disabled:true,
            defaultValue:supplierPoDetails.supplierPOCurrency ? supplierPoDetails.supplierPOCurrency: ''
        };

        const monetaryTaxes = [
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.INVOICED_TO_DATE_EXCL }: `,
                value:supplierPoDetails.supplierPOBudgetInvoicedToDate ?thousandFormat(supplierPoDetails.supplierPOBudgetInvoicedToDate):0.00
            },
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.UNINVOICED_TO_DATE_EXCL }: `,
                value:supplierPoDetails.supplierPOBudgetUninvoicedToDate?thousandFormat(supplierPoDetails.supplierPOBudgetUninvoicedToDate):0.00
            },
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.REMAINING }: `,
                value:supplierPoDetails.supplierPORemainingBudgetValue ?thousandFormat(supplierPoDetails.supplierPORemainingBudgetValue):0.00
            },
            {
                className:this.props.currentPage !== localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE  && supplierPoDetails.supplierPOBudgetValueWarningPercentage <= 0 ?"hide":this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE?"hide":"custom-Badge col text-red-parent",
                colSize:"s12",
                label: supplierPoDetails.supplierPOBudgetValueWarningPercentage > 0 ? thousandFormat(supplierPoDetails.supplierPOBudgetValueWarningPercentage) + " " + ` ${ localConstant.commonConstants.SUPPLIERPO_BUDGET_WARNING } ` : null
            }
        ];

        const unitValues = [
            {
                hasLabel: true,
                divClassName: 'col',
                label: localConstant.budget.UNITS,
                type: 'text',
                dataType: 'decimal',
                valueType: 'value',
                colSize: 's6',
                inputClass: 'customInputs',
                labelClass: 'customLabel mandate',
                max: '999999999',
                required: true,
                name: 'supplierPOBudgetHours',
                maxLength:fieldLengthConstants.common.budget.BUDGET_HOURS_MAXLENGTH,
                min: '0',
                prefixLimit: fieldLengthConstants.common.budget.BUDGET_HOURS_PREFIX_LIMIT, 
                suffixLimit : fieldLengthConstants.common.budget.BUDGET_HOURS_SUFFIX_LIMIT,
                isLimitType:true,
                readOnly: this.props.interactionMode,
                // disabled: this.props.interactionMode,
                value: supplierPoDetails.supplierPOBudgetHours?thousandFormat(supplierPoDetails.supplierPOBudgetHours):'',
                onValueChange: this.handleInputChange,
                //onValueBlur: this.checkNumber
            },
            {
                hasLabel:true,
                divClassName:'col',
                label:localConstant.budget.WARNING,
                type:'number',
                dataValType:'valueText',
                colSize:'s3',
                inputClass:'customInputs',
               // labelClass:'customLabel mandate',
                name:'supplierPOBudgetHoursWarning',
                max:'100',
                min:'0',
                maxLength:fieldLengthConstants.supplierpo.supplierDetails.SUPPLIERPO_BUDGET_WARNING_MAXLENGTH,
                readOnly: this.props.interactionMode || this.props.supplierPOViewMode,    //For D-456
                // disabled:this.props.interactionMode,
                value: supplierPoDetails.supplierPOBudgetHoursWarning,
                onValueChange:this.handleInputChange,
                onValueBlur:this.warningBlurHandler
            }
        ];

        const unitTaxes =[
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.INVOICED_TO_DATE }: `,
                value: supplierPoDetails.supplierPOBudgetHoursInvoicedToDate?thousandFormat(supplierPoDetails.supplierPOBudgetHoursInvoicedToDate):0.00
            },
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE  ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.UNINVOICED_TO_DATE }: `,
                value: supplierPoDetails.supplierPOBudgetHoursUnInvoicedToDate?thousandFormat(supplierPoDetails.supplierPOBudgetHoursUnInvoicedToDate):0.00
            },
            {
                className:this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.REMAINING }:  `,
                value: supplierPoDetails.supplierPORemainingBudgetHours ?thousandFormat(supplierPoDetails.supplierPORemainingBudgetHours):0.00
            },
            {
                className:this.props.currentPage !== localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE  && supplierPoDetails.supplierPOBudgetHourWarningPercentage <= 0 ?"hide":this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE?"hide":"custom-Badge col text-red-parent",
                colSize:"s12",
                label: supplierPoDetails.supplierPOBudgetHourWarningPercentage > 0 ? thousandFormat(supplierPoDetails.supplierPOBudgetHourWarningPercentage )+ " " + ` ${ localConstant.commonConstants.SUPPLIERPO_BUDGETHOUR_WARNING } ` : null
            }
        ];
        
        let subSupplierData = !isEmpty(this.props.subSupplierData) && this.props.subSupplierData.filter(row => row.recordStatus !== 'D');
        subSupplierData = isEmptyReturnDefault(subSupplierData, 'array');
        bindAction(this.headerData.subSupplierDetailHeader, "editSubSupplier", this.editSubSupplier); //D456 issue      
        bindAction(this.headerData.subSupplierDetailHeader, "viewSubSupplier", this.viewSubSupplier);

        let filteredSupplierList=this.props.supplierList;
        let filteredSubSupplierList=[];
        if (this.props.supplierPoDetails) { //Changes For D479 issue 1
                 if( this.state.supplierName && this.state.supplierName.lastIndexOf('*')< 0){
                    filteredSupplierList = this.props.supplierList.filter(x => x.supplierId === this.props.supplierPoDetails.supplierPOMainSupplierId);
                    if (this.props.subSupplierData.length > 0 && filteredSupplierList.length === 0 && this.props.supplierList.length > 0 ) {
                        filteredSupplierList = this.props.supplierList.filter(subSupplier => {
                                                          return this.props.subSupplierData.some(item =>{
                                                                 return item.supplierId === subSupplier.supplierId;
                                                            });
                                                 });
                    }
                } else {//Changes for D907 Main-Sub Supplier Duplicates
                    filteredSubSupplierList = this.props.supplierList.filter(x => x.supplierId !== this.props.supplierPoDetails.supplierPOMainSupplierId);
                    if (this.props.subSupplierData.length > 0) {
                        this.props.subSupplierData.map(subSupplier => {
                            filteredSupplierList = filteredSupplierList.filter(x => x.supplierId !== subSupplier.supplierId);
                         });
                        if (!this.state.isSupplierPOContactEdit) {
                            this.props.subSupplierData.map(subSupplier => {
                               filteredSubSupplierList = filteredSubSupplierList.filter(x => x.supplierId !== subSupplier.supplierId);
                            });
                        }
                        else if(this.state.isSupplierPOContactEdit) {
                            const updatedSubSupplierList = this.props.subSupplierData.filter(x => x.supplierId !== this.editedRow.supplierId);
                            updatedSubSupplierList.map(subSupplier => {
                            filteredSubSupplierList = filteredSubSupplierList.filter(x => x.supplierId !== subSupplier.supplierId);
                            });
                        }
                 } 
              }
        }
        
        return (
            <Fragment>
                <CustomModal />
                {this.state.isSupplierPOContactOpen &&
                    // <Modal title={localConstant.supplierpo.SUB_SUPPLIER} modalId="subSupplierPopup" formId="subSupplierForm"
                    //     onSubmit={this.subSupplierSubmitHandler} modalClass="subSupplierPopup popup-position"
                    //     buttons={this.subSupplierSubmitButtons} isShowModal={this.state.isSupplierPOContactOpen}>
                        <SubSupplierPopup
                            supplierChange={this.supplierPoOnChangeHandler}
                            editedData={this.editedRow}
                            headerData={this.headerData} //D456 issue      
                            getMainSupplier={data => this.getMainSupplier(data)}
                            getSelectedSupplier={data => this.addSubSupplier(data)}
                            modalRowData={filteredSubSupplierList}
                            handleInputChange={e => this.handleInputChange(e)}
                            supplierPopupOpen={data => this.supplierPopupOpen(data)}
                            columnPrioritySort={ defaultSort }
                            isShowSupplierModal={ this.state.isSupplierPOContactOpen }
                            cancelSubSupplierPopUp= { e => this.cancelSupplierPoContact(e) }
                            ClearSearchResult= { this.ClearSearchResult }
                            isSupplierPOContactEdit= {this.state.isSupplierPOContactEdit }
                        />
            
                }
                <div className="genralDetailContainer customCard">
                    <GeneralDetails
                        generalDetailsData={supplierPoDetails}
                        handleInputChange={e => this.handleInputChange(e)}
                        deliveryDateChange={this.deliveryDateChange}
                        completedDateChange={this.completedDateChange}
                        deliveryDate={this.state.deliveryDate}
                        completedDate={this.state.completedDate} 
                        interactionMode={this.props.interactionMode}                       
                        isAssignmentOpenedAsOC={this.props.supplierPOViewMode}           //For D-456          
                    />
                    <BudgetMonetary
                       monetaryValues = {monetaryValues}
                       monetaryTaxes = {monetaryTaxes}
                       monetaryCurrency = {monetaryCurrency}
                       monetaryWarning ={monetaryWarning}
                       unitTaxes = {unitTaxes}
                       unitValues = {unitValues}
                       isCurrencyEditable={false}
                    />
                    <MainSupplier
                        headerData={this.headerData} //D456 issue      
                        supplierPopupOpen={data => this.supplierPopupOpen(data)}
                        getMainSupplier={data => this.getMainSupplier(data)}
                        getSelectedMainSupplier={data => this.getSelectedMainSupplier(data)}
                        handleSupplierDetailRedirection={this.handleSupplierDetailRedirection}
                        budgetMonitaryDetails={supplierPoDetails}
                        modalRowData={filteredSupplierList}
                        handleInputChange={e => this.handleInputChange(e)}
                        enableSupplierLink={this.state.enableSupplierLink}
                        columnPrioritySort={ defaultSort }
                        interactionMode={this.props.interactionMode}
                        pageMode={this.props.pageMode}  //D456 issue      
                        callBackFunc={this.callBackFunc}     
                        isAssignmentOpenedAsOC={this.props.supplierPOViewMode}       //For D-456
                    />
                    <SubSuppliers
                        rowData={subSupplierData}
                        headerData={this.headerData} //D456 issue      
                        subSupplierCreateHandler={this.subSupplierCreateHandler}
                        subSupplierDeleteHandler={this.subSupplierDeleteHandler}
                        onRef={ref => { this.child = ref; }}
                        showButton={subSupplierData.length > 0 ? false : true}
                        pageMode={this.props.pageMode}
                        isAssignmentOpenedAsOC={this.props.supplierPOViewMode}   //For D-456
                    />
                </div>
            </Fragment>
        );
    }
}

export default supplierPoDetail;