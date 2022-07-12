import React, { Component, Fragment } from 'react';
import { getlocalizeData, isEmpty,numberFormat,isEmptyReturnDefault,formInputChangeHandler,bindAction } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { HeaderData } from './additionalExpensesHeader';
import { required } from '../../../../utils/validator';
import { FormatFourDecimal,FormatTwoDecimal } from '../../../../utils/stringUtil';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import PropTypes from 'prop-types';
const localConstant = getlocalizeData();

/** Additional Expenses Popup  */
const AdditionalExpensesPopup = (props) => {
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                required={true}
                labelClass="customLabel"
                label={localConstant.assignments.COMPANY}
                type='text'
                name="companyName"
                colSize='s12 m6'
                inputClass="customInputs"
                defaultValue={props.assignmentOperatingCompanyName}
                onValueChange={props.additionalExpensesChange}
                readOnly={true} />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.assignments.CURRENCY}
                type='select'
                className="browser-default"
                name="currencyCode"
                optionsList={props.currencyCodes}
                optionName='code'
                optionValue="code"
                defaultValue={props.additionalExpensesData.currencyCode}
                colSize='s12 m3'
                onSelectChange={props.additionalExpensesChange} />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.assignments.EXPENSE_TYPE}
                type ='select'
                name="expenseType"
                defaultValue={props.additionalExpensesData.expenseType}
                optionsList={props.expenseTypes}
                optionName='name'
                optionValue="name"
                colSize='s12 m3'
                id="expenseTypeId"
                onSelectChange={props.additionalExpensesChange} />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.assignments.DESCRIPTION}
                divClassName="m6"
                type='textarea'
                name='description'
                colSize='s12'
                maxLength={fieldLengthConstants.assignment.additionalExpenses.DESCRIPTION_MAXLENGTH}
                inputClass="customInputs"
                defaultValue={props.additionalExpensesData.description}
                onValueChange={props.additionalExpensesChange} />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.assignments.RATE}
                type='text'
                dataType='decimal'
                name="perUnitRate"
                defaultValue={props.FormatFourDecimal(props.additionalExpensesData.perUnitRate)} //defect Id 203
                colSize='s12 m3'
                inputClass="customInputs"
                min="0" 
                maxLength={fieldLengthConstants.assignment.additionalExpenses.RATE_MAXLENGTH} //defect Id 203
                max={999999999999999} //defect Id 203
                prefixLimit = {10} //defect Id 203
                suffixLimit = {4} //defect Id 203
                onValueBlur={(e) =>props.checkNumber(e,4)}
                onValueInput={props.additionalExpensesChange}//defect Id 203,
                readOnly={props.interactionMode} />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.assignments.UNITS}
                type='text' //defect Id 203
                dataType='decimal' //defect Id 203
                name='totalUnit'
                defaultValue={props.FormatTwoDecimal(props.additionalExpensesData.totalUnit)}
                colSize='s12 m3'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.assignment.additionalExpenses.UNITS_MAXLENGTH} //defect Id 203
                max={999999999999999} //defect Id 203
                prefixLimit = {10} //defect Id 203
                suffixLimit = {2} //defect Id 203
                onValueBlur={(e) =>props.checkNumber(e,2)}  //defect Id 203
                onValueInput={props.additionalExpensesChange} />
            <CustomInput
                type='switch'
                switchLabel={localConstant.assignments.ALREADY_LINKED}
                isSwitchLabel={true}
                switchName="isAlreadyLinked"
                id= "isAlreadyLinkedId"
                colSize='s12 m6 mt-3'
                disabled={true}
                checkedStatus={props.additionalExpensesData.isAlreadyLinked}
                onToggleChange={props.additionalExpensesChange} />
        </Fragment>
    );
};

class AdditionalExpenses extends Component {
    constructor(props){
        super(props);
        this.updatedData = {};
        this.editedRow = {};
        this.state = {
            isAdditionalExpensesOpen:false,
            isAdditionalExpensesEdit:false,
        };
        this.additionalExpensesSubmitButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelAdditionalExpenses,
                type: 'reset',
                btnID: "cancelCreateAdditionalExpenses",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type : 'submit',
                btnID: "createAdditionalExpenses",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
    }

    componentDidMount(){
        this.props.actions.FetchExpenseType();
    }

    additionalExpensesChangeHandler = (e) => {
        let value='';
        const result = formInputChangeHandler(e);
        if(e.type==="change")
        {
            value=result.value;
        } else {
            value=result.value;
            if(result.name === "totalUnit"){
                value=this.decimalWithTwoLimitFormat(e); 
            } 
        }
        this.updatedData[result.name] = value;
    }

    /** Additional Expenses Validation Handler */
    additionalExpensesValidation = (data) => {
        if(isEmpty(data.currencyCode)){
            IntertekToaster(localConstant.validationMessage.ADDITIONAL_EXPENSES_CURRENCY,"warningToast CurrencyVal");
            return false;
        }
        if(isEmpty(data.expenseType)){
            IntertekToaster(localConstant.validationMessage.ADDITIONAL_EXPENSES_EXPENSESTYPE,"warningToast ExpensesTypeVal");
            return false;
        }
        if(isEmpty(data.description)){
            IntertekToaster(localConstant.validationMessage.ADDITIONAL_EXPENSES_DESCRIPTION,"warningToast DescriptionVal");
            return false;
        }
        if(required(data.perUnitRate)){
            IntertekToaster(localConstant.validationMessage.ADDITIONAL_EXPENSES_RATE,"warningToast RateVal");
            return false;
        }
        if(required(data.totalUnit)){
            IntertekToaster(localConstant.validationMessage.ADDITIONAL_EXPENSES_UNITS,"warningToast UnitsVal");
            return false;
        }
        return true;
    };

    additionalExpensesSubmitHandler = (e) => {
        e.preventDefault();
        if(this.state.isAdditionalExpensesEdit){
            if(this.editedRow.recordStatus !== 'N'){
                this.updatedData['recordStatus'] = 'M';
            }
            this.updatedData['companyName'] = this.props.assignmentInfo.assignmentOperatingCompany;
            this.updatedData['companyCode'] = this.props.assignmentInfo.assignmentOperatingCompanyCode;
            const editedData = Object.assign({},this.editedRow,this.updatedData);            
            if(this.additionalExpensesValidation(editedData)){
                this.props.actions.UpdateAdditionalExpenses(editedData);
                this.cancelAdditionalExpenses(e);
            }      
        }
        else{
            this.updatedData['recordStatus'] = 'N';
            this.updatedData['assignmentAdditionalExpenseId'] = null;
            this.updatedData['assignmentAdditionalExpenseUniqueId'] = Math.floor(Math.random() * (Math.pow(10, 5)));
            this.updatedData['companyName'] = this.props.assignmentInfo.assignmentOperatingCompany;
            this.updatedData['companyCode'] = this.props.assignmentInfo.assignmentOperatingCompanyCode;
            const updatedAdditionalExpenses = Object.assign([],this.props.additionalExpenses);                 
            if(this.additionalExpensesValidation(this.updatedData)){
                updatedAdditionalExpenses.push(this.updatedData);      
                this.props.actions.AddAdditionalExpenses(updatedAdditionalExpenses);
                this.cancelAdditionalExpenses(e);
            }
        }
    }

    additionalExpensesCreateHandler = () => {
        if(this.props.assignmentInfo.assignmentOperatingCompany)
        {
            this.setState({ 
                isAdditionalExpensesOpen:true,
                isAdditionalExpensesEdit:false,
            });
            this.editedRow = {};
            this.updatedData={};
        } else {
            IntertekToaster(localConstant.validationMessage.ASSIGNMENT_ADDITIONAL_EXPENSE_OC_VALIDATION,'warningToast additionalExpensesOCToaster');
        }
    }; 

    additionalExpensesDeleteHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.assignment.additionalExpenses.ADDITIONAL_EXPENSES_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteAdditionalExpenses,
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
        } else {
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast additionalExpensesDeleteToaster');
        }
    };

    deleteAdditionalExpenses = () =>{
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteAdditionalExpenses(selectedRecords);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () =>{
        this.props.actions.HideModal();
    }

    cancelAdditionalExpenses = (e) => {
        e.preventDefault();
        this.updatedData = {};
        this.setState({ 
            isAdditionalExpensesOpen:false,
            isAdditionalExpensesEdit:false,
        });
        this.editedRow = {};
    }

    editAdditionalExpensesRowHandler = (data) => {
        this.setState((state) => {
            return {
                isAdditionalExpensesOpen:!state.isAdditionalExpensesOpen,
                isAdditionalExpensesEdit:true
                };
            });
        this.editedRow = data;
    };

    blurValue = (e) => {
        if (e.target.value > 9999999999999) {
        } else {
            e.target.value = numberFormat(e.target.value);
        }
    }
    
    checkNumber = (e,decimalVal) => {
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if (!isEmpty(e.target.value) && e.target.value > 0) {
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);//def 203
            this.additionalExpensesChangeHandler(e);
        }
        else {
            e.target.value = parseFloat("0").toFixed(decimalVal); 
            this.additionalExpensesChangeHandler(e);
        }
    }

    /**
     * Form Input data Change Handler
     */
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }
    
    //def 203
    decimalWithTwoLimitFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(2) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 
        if (e.preventDefault) e.preventDefault(); 
        return e.target.value;       
    };

    render() {
        const { assignmentInfo,interactionMode,currencyCodes } = this.props;
        let expenseTypes = this.props.expenseTypes && this.props.expenseTypes.filter(row=>row.chargeType == 'E');
        expenseTypes = isEmptyReturnDefault(expenseTypes,'array');
        let additionalExpenses = !isEmpty(this.props.additionalExpenses) && this.props.additionalExpenses.filter(row=>row.recordStatus !== 'D');
        additionalExpenses = isEmptyReturnDefault(additionalExpenses,'array');
        bindAction(HeaderData,"AdditionalExpensesRenderColumn",this.editAdditionalExpensesRowHandler);
        
        return (
           <Fragment>
               <CustomModal />
                { this.state.isAdditionalExpensesOpen &&
                    <Modal title="Prepaid Expenses" modalId="AdditionalExpensesPopup" formId="additionalExpensesForm" onSubmit={this.additionalExpensesSubmitHandler} modalClass="popup-position" buttons={ this.additionalExpensesSubmitButtons } isShowModal={this.state.isAdditionalExpensesOpen}>
                        <AdditionalExpensesPopup
                            additionalExpensesChange ={this.additionalExpensesChangeHandler}
                            additionalExpensesData = {this.editedRow}
                            assignmentOperatingCompanyName = {assignmentInfo.assignmentOperatingCompany}
                            additionalExpensesCreateHandler = {this.additionalExpensesCreateHandler}
                            additionalExpensesDeleteHandler = {this.additionalExpensesDeleteHandler}
                            currencyCodes = {currencyCodes}
                            expenseTypes = {expenseTypes}
                            interactionMode = {interactionMode}
                            blurValue = {this.blurValue}
                            checkNumber={(e,val) => this.checkNumber(e,val)}
                            FormatTwoDecimal= {FormatTwoDecimal} 
                            FormatFourDecimal= {FormatFourDecimal} //Defect Id 203
                        />
                    </Modal>}
               <div className="customCard">
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.ADDITIONAL_EXPENSES} colSize="s12">
                        <ReactGrid 
                            gridRowData={additionalExpenses} 
                            gridColData={HeaderData} 
                            onRef={ref => { this.child = ref; }}
                            paginationPrefixId={localConstant.paginationPrefixIds.assignmentAddExp} />
                        { this.props.pageMode !== localConstant.commonConstants.VIEW && 
                            <div className="right-align mt-2 mr-2">
                                <a onClick={this.additionalExpensesCreateHandler} disabled={interactionMode} className="btn-small waves-effect modal-trigger">
                                    {localConstant.commonConstants.ADD}
                                </a>
                                <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={interactionMode} onClick={this.additionalExpensesDeleteHandler}>
                                    {localConstant.commonConstants.DELETE}
                                </a>
                            </div> }         
                    </CardPanel>
                </div>
           </Fragment>
        );
    }
}
AdditionalExpenses.propTypes = { 
    assignmentInfo: PropTypes.object,
    currencyCodes: PropTypes.array,
    expenseTypes: PropTypes.array,
    additionalExpenses: PropTypes.array,
};
AdditionalExpenses.defaultProps = {
    assignmentInfo:{},
    currencyCodes:[],
    expenseTypes:[],
    additionalExpenses:[],
};
export default AdditionalExpenses;