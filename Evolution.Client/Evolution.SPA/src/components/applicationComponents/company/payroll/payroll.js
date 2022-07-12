import React, { Component, Fragment } from 'react';
import PropTypes from 'proptypes';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, numberFormat, isEmpty, isEmptyOrUndefine, formatToDecimal, isEmptyReturnDefault } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import {
    payrollHeaderData,
    sellReferrenceHeaderData
} from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import ReactGridTwo from '../../../../common/baseComponents/reactAgGridTwo';
import MaterializeComponent from 'materialize-css';
import CustomModal from '../../../../common/baseComponents/customModal';
import store from '../../../../store/reduxStore';
import moment from 'moment';
import Draggable from 'react-draggable'; // The default
import arrayUtil from '../../../../utils/arrayUtil';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

class Payroll extends Component {
    constructor(props) {
        super(props);
        this.state = {
            // SelectedPayrollName: "Select",
            ExportPrefix: '',
            PayrollCurrency: 'Select',
            // selectedPayrollData: {},
            isPayrollEditMode: false,
            isPayrollPeriodEditMode:false,
            payrollStartDate: '',
            payrollEndDate:'',
            inValidStartDate: false,
            inValidEndDate: false

        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        //Added to update the CompanyPayrollId for newly added record
        this.props.callBackFuncs.onAfterSaveChange = () => {
           this.UpdateSelectedPayrollData();
        };

    }
    UpdateSelectedPayrollData =() =>{
        if (this.props.PayrollData.length > 0) {
                this.props.PayrollData.map((iteratedValue) => {
                    if (iteratedValue.payrollType === this.props.selectedPayrollData.payrollType) {
                        this.props.actions.UpdateSelectedPayrollData(iteratedValue);
                    }
                });
            }
            else {
                this.props.actions.UpdateSelectedPayrollData({});
            }
    }
    handlePayrollStartDateChange = (date) => {
        this.setState({
            payrollStartDate: date
        });
    }
    handlePayrollStartDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {
                IntertekToaster(localConstant.companyDetails.payroll.VALID_START_DATE_WARNING,'warningToast invalidStartDate');
               
               // this.setState({ inValidStartDate: true });
            } else {
                this.setState({ inValidStartDate: false });
            }
        }

    }
    handlePayrollEndDateChange = (date) => {
        this.setState({
            payrollEndDate: date
        });
    }
    handlePayrollEndDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {                
                IntertekToaster(localConstant.companyDetails.payroll.VALID_END_DATE_WARNING,'warningToast invalidEndDate');
                //this.setState({ inValidEndDate: true });
            } else {
                this.setState({ inValidEndDate: false });
            }
        }
    }
    onInputChange =(e) =>{
        this.setState({ newPayrollPeriodName: e.target.value });
    }
    componentDidMount() {
        const elems = document.querySelectorAll('.modal');
        MaterializeComponent.Modal.init(elems, { dismissible: false });
        this.props.actions.FetchPayrollData();
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
         * 
         * Commented FetchCurrency actions because the data we are getting from master data
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
            // this.props.actions.FetchCurrency();
            this.props.actions.FetchCostSaleReference(true);   //Changes for Defect 983
            this.props.actions.FetchPayrolls();
        }
        this.setState( { SelectedPayrollName:"" } );
    };
    componentWillReceiveProps(nextProps) {
        if (nextProps.showButton) {
            this.setState({
                payrollStartDate: '',
                payrollEndDate: '',
                newPayrollPeriodName:''
            });
            document.getElementById('payrollPeriodNameStatus').checked = false;
        } else {
            this.setState({
                payrollStartDate: moment(nextProps.editedPairollPeriodName.startDate),
                payrollEndDate: moment(nextProps.editedPairollPeriodName.endDate),
                newPayrollPeriodName:nextProps.editedPairollPeriodName.periodName
            });
            if(document.getElementById('payrollPeriodNameStatus')){
                document.getElementById('payrollPeriodNameStatus').checked = !nextProps.editedPairollPeriodName.isActive;  
            } 
        }
    }

    loadParollPeriodNames = (e) => {
        this.props.actions.FetchPayrollPeriodName();
        this.setState({ SelectedPayrollName: e.target.value });
        if ((e.target.value).toLowerCase() === 'select' || e.target.value === "") {
            this.setState({
                ExportPrefix: '',
                PayrollCurrency: '',
                // selectedPayrollData: {},
            });
            this.props.actions.UpdateSelectedPayrollData({});
        } else {
            const currentPayroll = this.props.PayrollData && this.props.PayrollData.filter((iteratedValue) => {
                if (iteratedValue.companyPayrollId == e.target.value) {
                    return iteratedValue;
                }
            });
            this.setState({
                ExportPrefix: currentPayroll[0].ExportPrefix,
                PayrollCurrency: currentPayroll[0].currency,
                // selectedPayrollData: currentPayroll[0],
                // SelectedPayrollName: currentPayroll[0].payrollType
            });
            this.props.actions.UpdateSelectedPayrollData(currentPayroll[0]);
        }
    };
    addNewPayroll = (e) => {
        e.preventDefault();
        this.props.actions.UpdateCompanyPayrollButton(false);
        const payrollName = document.getElementById('newPayrollName').value.trim();
        const exportPrefix = document.getElementById('payrollNewExportPrefix').value.trim();
        const payrollCurrency = document.getElementById('payrollNewCurrency').value.trim();
        if (payrollName === "") {
            IntertekToaster(localConstant.companyDetails.payroll.SELECT_PAYROLL_NAME,'warningToast selectPayrollName');
            return false;
        }
        if (exportPrefix === "") {            
            IntertekToaster(localConstant.companyDetails.payroll.SELECT_EXPORT_PREFIX,'warningToast selectExportPrefix');            
            return false;
        }
        if (payrollCurrency === "") {
            IntertekToaster(localConstant.companyDetails.payroll.SELECT_PAYROLL_CURRENCY,'warningToast selectPayrollCurrency');
            return false;
        }
        const isDuplicate = this.props.PayrollData && this.props.PayrollData.find((itertedValue) => {
            if ((itertedValue.payrollType).toUpperCase() === payrollName.toUpperCase()) {
                return true;
            }
        });
        if (isDuplicate) {            
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_EXISTS,'warningToast payrollDup');
            return false;
        }
        const data = {
            "companyCode": this.props.selectedCompanyCode,
            "payrollType": payrollName,
            "currency": payrollCurrency,
            "exportPrefix": exportPrefix,
            "recordStatus": "N",
            "companyPayrollId":Math.floor(Math.random() * 99) -100
        };
        // this.setState({
        //     selectedPayrollData:data,
        //     SelectedPayrollName:payrollName
        // });
        this.props.actions.AddNewPayroll(data);
        this.props.actions.UpdateSelectedPayrollData(data);
        document.getElementById('createPayrollModalClose').click();
    };

    /**
     * Payroll Type Edit Handler
     */
    payrollEditHandler = () => {
        this.props.actions.UpdateCompanyPayrollButton(true);
        this.setState({ isPayrollEditMode: true });
        const oldPayroll = this.props.selectedPayrollData.payrollType;
        this.props.PayrollData && this.props.PayrollData.map((iteratedValue) => {
            if (iteratedValue.companyPayrollId === this.props.selectedPayrollData.companyPayrollId) {
                document.getElementById("newPayrollName").value = iteratedValue.payrollType;
                document.getElementById("payrollNewExportPrefix").value = iteratedValue.exportPrefix;
                document.getElementById("payrollNewCurrency").value = iteratedValue.currency;
            }
        });

        if (oldPayroll && oldPayroll.toLowerCase() === "select") {
            IntertekToaster(localConstant.companyDetails.payroll.SELECT_PAYROLL_TO_UPDATE,'warningToast selectPayrollToUpdate');
            return false;
        }
    }

    /**
     * Payroll Type Delete Handler
     */
    payrollDeleteHandler = () => {

        const isChilExists = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((itertedValue) => {
            if (itertedValue.recordStatus === "D" && itertedValue.companyPayrollId === this.props.selectedPayrollData.companyPayrollId) {
                return false;
            } else if (itertedValue.recordStatus !== "D" && itertedValue.companyPayrollId ===this.props.selectedPayrollData.companyPayrollId) {
                return true;
            }
        });
        if (isChilExists) {
            IntertekToaster(localConstant.companyDetails.payroll.SELECTED_PAYROLL_ASSOCIATED_WARNING,'warningToast assoCiatedPayroll');
            
            return false;
        }
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.PAYROLL_MESSAGE,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.deleteSelectedPayroll,
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

    /**
     * Delete Payroll after confirmation
     */
    deleteSelectedPayroll = () => {
        this.setState({ SelectedPayrollName: "Select" });
        this.props.actions.DeletePayrollType(this.props.selectedPayrollData.companyPayrollId);
        this.props.actions.HideModal();
        // this.setState({ selectedPayrollData: {} });
        document.getElementById('loadedPayroll').value = "";
        this.props.actions.UpdateSelectedPayrollData({});
    }

    /**
     * popup clear handler
     */
    clearPayrollPopup = () => {
        this.props.actions.PayrollPopupClear();
    }

    /**
     * Create Payroll Handler
     */
    handleCreatePayroll = () => {
        this.props.actions.UpdateCompanyPayrollButton(false);
        this.setState({ 
            isPayrollEditMode: false
        });
        document.getElementById("newPayrollName").defaultValue = '';
        document.getElementById("payrollNewExportPrefix").defaultValue = '';
    }

    /**
     * Company Payroll Edit Handler
     */
    editCompanyPayroll = (e) => {
        e.preventDefault();
        document.getElementById("newPayrollName").defaultValue = this.props.selectedPayrollData.payrollType;
        document.getElementById("payrollNewExportPrefix").defaultValue = this.props.selectedPayrollData.exportPrefix;
        document.getElementById("payrollNewCurrency").defaultValue = this.props.selectedPayrollData.currency;

        const updatedPayrollName = document.getElementById("newPayrollName").value.trim();
        const updatedExportPrefix = document.getElementById("payrollNewExportPrefix").value.trim();
        const updatedPayrollCurrency = document.getElementById("payrollNewCurrency").value.trim();
        // const isDuplicate = this.props.PayrollData.find((itertedValue) => {
        //     if (itertedValue.ExportPrefix === updatedExportPrefix && (itertedValue.payrollType).toUpperCase() === updatedPayrollName.toUpperCase()) {
        //         return true;
        //     } else if ((itertedValue.payrollType).toUpperCase() === updatedPayrollName.toUpperCase() && itertedValue.ExportPrefix !== updatedExportPrefix) {
        //         return false;
        //     }
        // });
        // if (isDuplicate) {            
        //     IntertekToaster(localConstant.companyDetails.payroll.NO_CHANGES_FOUND,'warningToast noChange');
        //     return false;
        // }
        const isDuplicate = this.props.PayrollData && this.props.PayrollData.find((itertedValue) => {
            if ((itertedValue.payrollType).toUpperCase() === updatedPayrollName.toUpperCase() && itertedValue.companyPayrollId !== this.props.selectedPayrollData.companyPayrollId) {
                return true;
            }
        });
        if (isDuplicate) {            
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_EXISTS,'warningToast payrollDup');
            return false;
        }

        if (updatedPayrollName === '' || updatedExportPrefix === '' || updatedPayrollCurrency === '') {
            IntertekToaster(localConstant.companyDetails.payroll.MANDATE_FIELD,'warningToast mandateFillWarn');
            return false;
        }
        this.setState({
            SelectedPayrollName: updatedPayrollName,
            ExportPrefix: updatedExportPrefix,
            PayrollCurrency: updatedPayrollCurrency
        });
        let UpdatedCompanyPayroll;
        if (this.props.selectedPayrollData.recordStatus !== "N") {
            UpdatedCompanyPayroll = {
                "oldPayrollName": this.props.selectedPayrollData.payrollType,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "payrollType": updatedPayrollName,
                    "exportPrefix": updatedExportPrefix,
                    "periodStatus": "N",
                    "currency": updatedPayrollCurrency,
                    "recordStatus": "M",
                    "modifiedBy": this.props.loginUser,
                    "companyPayrollId":this.props.selectedPayrollData.companyPayrollId
                }
            };
        } else {
            UpdatedCompanyPayroll = {
                "oldPayrollName": this.props.selectedPayrollData.payrollType,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "payrollType": updatedPayrollName,
                    "exportPrefix": updatedExportPrefix,
                    "periodStatus": "N",
                    "currency": updatedPayrollCurrency,
                    "recordStatus": "N",
                    "modifiedBy": this.props.loginUser,
                    "companyPayrollId":this.props.selectedPayrollData.companyPayrollId
                }

            };
        }
        this.props.actions.UpdateCompanyPayroll(UpdatedCompanyPayroll);
        this.props.actions.UpdatePayrollPeriodNameInPayrollPeriod(UpdatedCompanyPayroll.data);
        // this.setState({ selectedPayrollData: UpdatedCompanyPayroll.data });
        this.props.actions.UpdateSelectedPayrollData(UpdatedCompanyPayroll.data);
        document.getElementById('createPayrollModalClose').click();
    }

    /**
     * Form Reset Handler
     */
    formReset = () => {
        document.getElementById("newPayrollName").defaultValue = '';
        document.getElementById("payrollNewExportPrefix").defaultValue = '';
        document.getElementById("payrollNewCurrency").value = '';
    };

    handleNewPayrollPeriodName = () => {
        this.setState({ newPayrollPeriodName:'' });
        //document.getElementById('newPayrollPeriodName').value = '';
       // document.getElementById("payrollPeriodNameStatus").checked = true;
        this.props.actions.TogglePayrollPeriodNameButton(true);
        //this.setState({ payrollEndDate:'', payrollStartDate:'', inValidEndDate:false, inValidStartDate:false });
    }
    //Sanity Defect -183 Fix Start
    cancelPayrollPeriod =(e)=>{
        e.preventDefault();
        this.props.actions.EditPayrollPeriodName({});
    }
    //Sanity Defect -183 Fix End
    //Add new payroll period name
    addNewPayrollPeriodName = (e) => {
        e.preventDefault();

        const invalidEndDate = this.state.invalidEndDate;
        const invalidStartDate = this.state.inValidStartDate;
        let periodName='';

        const NewPayrollPeriodName = this.state.newPayrollPeriodName.trim();
        const NewPayrollPeriodStartDate = this.state.payrollStartDate;
        const NewPayrollPeriodEndDate = this.state.payrollEndDate;
        const NewPayrollPeriodHidden = document.getElementById('payrollPeriodNameStatus').checked;
        const currentPayrollType = this.state.SelectedPayrollName;
      
        if(isEmpty(NewPayrollPeriodName)){
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_PERIOD_NAME_VALIDATION,'warningToast inValidStartDate');
            return false;
        }
        if(isEmpty(NewPayrollPeriodStartDate)){
            IntertekToaster(localConstant.companyDetails.payroll.VALID_START_DATE_WARNING,'warningToast inValidStartDate');
            return false;
        }
        if(isEmpty(NewPayrollPeriodEndDate)){
            IntertekToaster(localConstant.companyDetails.payroll.VALID_END_DATE_WARNING,'warningToast inValidStartDate');
            return false;
        }
        if (this.state.inValidStartDate) {
            
            IntertekToaster(localConstant.companyDetails.payroll.VALID_START_DATE_WARNING,'warningToast inValidStartDate');
            this.setState({ inValidStartDate:false });
            return false;
        }
        if (this.state.inValidEndDate) {
            IntertekToaster(localConstant.companyDetails.payroll.VALID_END_DATE_WARNING,'warningToast invalidEndDate');
            this.setState({ inValidEndDate:false });
            return false;
        }
        if (NewPayrollPeriodStartDate > NewPayrollPeriodEndDate) {            
            IntertekToaster(localConstant.companyDetails.payroll.START_DATE_LESS_WARNING,'warningToast lessStartDate');
            return false;
        }
        const isDuplicate = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            if (iteratedValue.periodName === NewPayrollPeriodName && iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D") {
                periodName=iteratedValue.periodName;
                return true;
            }
        });
        const PayrollStartDateDuplicate = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D") {
                if(moment(NewPayrollPeriodStartDate).isBetween(iteratedValue.startDate, iteratedValue.endDate) || moment(NewPayrollPeriodStartDate).isSame(iteratedValue.endDate,'day') || moment(NewPayrollPeriodStartDate).isSame(iteratedValue.startDate,'day')){
                    periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });

        const PayrollEndDateDuplicate = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D") {
                if(moment(NewPayrollPeriodEndDate).isBetween(iteratedValue.startDate, iteratedValue.endDate) || moment(NewPayrollPeriodEndDate).isSame(iteratedValue.startDate,'day')  || moment(NewPayrollPeriodEndDate).isSame(iteratedValue.endDate,'day')){
                   periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });

        const PayrollDateOverlap = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D") {
                if(moment(NewPayrollPeriodStartDate).isSameOrBefore(iteratedValue.startDate) && moment(NewPayrollPeriodEndDate).isSameOrAfter(iteratedValue.endDate) ){
                    periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });
        if (PayrollStartDateDuplicate) {
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_START_DATE_OVERLAPP + periodName,'warningToast startDateOverlapp');
            return false;
        }

        if (PayrollEndDateDuplicate) {
            
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_ENDDATE_OVERLAP + periodName,'warningToast endDateOverlapp');
            return false;
        }
        if (PayrollDateOverlap) {
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_DATE_OVERLAP + periodName,'warningToast payrollDateOverlapp');
            return false;
        }
        if (isDuplicate) {
            
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_PERIOD_ALREADY_EXIST,'warningToast dupPayrollPeriodNameSec');
            return false;
        }

        const data = {
            "companyCode": this.props.selectedPayrollData.companyCode,
            "payrollType": this.props.selectedPayrollData.payrollType,
            "periodName": NewPayrollPeriodName,
            "startDate": NewPayrollPeriodStartDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
            "endDate": NewPayrollPeriodEndDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
            "isActive": !NewPayrollPeriodHidden,
            "recordStatus": "N",
            "periodStatus": "N",
            "modifiedBy": this.props.loginUser,
            "companyPayrollId":this.props.selectedPayrollData.companyPayrollId,
            "companyPayrollPeriodId":Math.floor(Math.random() * 99) -100
        };
        this.props.actions.AddNewPayrollPeriodName(data);
        this.props.actions.FetchPayrollPeriodName(this.state.SelectedPayrollName);
        document.getElementById('createPayrollPeriodModalClose').click();
        this.props.actions.TogglePayrollPeriodNameButton(false);
        this.props.actions.EditPayrollPeriodName({});//Sanity Defect -183 Fix
        this.setState({ payrollStartDate:'',payrollEndDate:'',newPayrollPeriodName:'' });

    }

    //Edit Payroll Period Name

    UpdatePayrollPeriodName = (e) => {
        e.preventDefault();
        const NewPayrollPeriodName = this.state.newPayrollPeriodName.trim();
        const NewPayrollPeriodStartDate = this.state.payrollStartDate;
        const NewPayrollPeriodEndDate = this.state.payrollEndDate;
        let periodName='';

        const NewPayrollPeriodHidden = document.getElementById('payrollPeriodNameStatus').checked;
        const currentPayrollType = this.props.selectedPayrollData.payrollType;
       
        if(isEmpty(NewPayrollPeriodName)){
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_PERIOD_NAME_VALIDATION,'warningToast inValidStartDate');
            return false;
        }
        if(isEmpty(NewPayrollPeriodStartDate)){
            IntertekToaster(localConstant.companyDetails.payroll.VALID_START_DATE_WARNING,'warningToast inValidStartDate');
            return false;
        }
        if(isEmpty(NewPayrollPeriodEndDate)){
            IntertekToaster(localConstant.companyDetails.payroll.VALID_END_DATE_WARNING,'warningToast inValidStartDate');
            return false;
        }
        if (this.state.inValidStartDate) {            
            IntertekToaster(localConstant.companyDetails.payroll.VALID_START_DATE_WARNING,'warningToast validStartDateSec');
            this.setState({ inValidStartDate:false });
            return false;
        }
        if (this.state.inValidEndDate) {            
            IntertekToaster(localConstant.companyDetails.payroll.VALID_END_DATE_WARNING,'warningToast validEndDateSec');
            this.setState({ inValidEndDate:false });
            return false;
        }
        if (NewPayrollPeriodStartDate > NewPayrollPeriodEndDate) {            
            IntertekToaster(localConstant.companyDetails.payroll.START_DATE_LESS_WARNING,'warningToast lessStartDateSec');
            return false;
        }
        const isDuplicateName = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((itertedValue) => {
            if ((itertedValue.periodName).toUpperCase() === NewPayrollPeriodName.toUpperCase() && itertedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && itertedValue.companyPayrollPeriodId != this.props.editedPairollPeriodName.companyPayrollPeriodId) {
                periodName=itertedValue.periodName;
                return true;
            }
        });
        const PayrollStartDateDuplicate = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
              
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D" && iteratedValue.companyPayrollPeriodId != this.props.editedPairollPeriodName.companyPayrollPeriodId) {
                
                if(moment(NewPayrollPeriodStartDate).isBetween(iteratedValue.startDate, iteratedValue.endDate) || moment(NewPayrollPeriodStartDate).isSame(iteratedValue.startDate,'day') || moment(NewPayrollPeriodStartDate).isSame(iteratedValue.endDate,'day')){
                    periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });

        const PayrollEndDateDuplicate = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D" && iteratedValue.companyPayrollPeriodId != this.props.editedPairollPeriodName.companyPayrollPeriodId) {
                if(moment(NewPayrollPeriodEndDate).isBetween(iteratedValue.startDate, iteratedValue.endDate) || moment(NewPayrollPeriodEndDate).isSame(iteratedValue.endDate,'day') || moment(NewPayrollPeriodEndDate).isSame(iteratedValue.startDate,'day')){
                    periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });

        const PayrollDateOverlap = this.props.PayrollPeriodName && this.props.PayrollPeriodName.find((iteratedValue) => {
            
            if (iteratedValue.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D" && iteratedValue.companyPayrollPeriodId != this.props.editedPairollPeriodName.companyPayrollPeriodId) {
                if(moment(NewPayrollPeriodStartDate).isSameOrBefore(iteratedValue.startDate,'day') && moment(NewPayrollPeriodEndDate).isSameOrAfter(iteratedValue.endDate,'day') ){
                    periodName=iteratedValue.periodName;
                    return true;
                }
            }
        });

        if (PayrollStartDateDuplicate) {
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_START_DATE_OVERLAPP + periodName,'warningToast payrollStartDateOverlapSec');
            return false;
        }

        if (PayrollEndDateDuplicate) {            
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_ENDDATE_OVERLAP + periodName,'warningToast payrollEndDateOverlapSec');
            return false;
        }
        if (PayrollDateOverlap) {
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_DATE_OVERLAP + periodName,'warningToast payrollDateOverlapSec');
            return false;
        }
        if (isDuplicateName) {
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_PERIOD_ALREADY_EXIST,'warningToast payrollDateOverlapSec');
            return false;
        }
        let updatedData;
        if (this.props.editedPairollPeriodName.recordStatus !== "N") {
            updatedData = {
                oldPayrollNameData: this.props.editedPairollPeriodName,
                data: {
                    "companyCode": this.props.selectedPayrollData.companyCode,
                    "payrollType": this.props.selectedPayrollData.payrollType,
                    "periodName": NewPayrollPeriodName,
                    "startDate": NewPayrollPeriodStartDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                    "endDate": NewPayrollPeriodEndDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                    "isActive": !NewPayrollPeriodHidden,
                    "recordStatus": "M",
                    "modifiedBy": this.props.loginUser,
                    "companyPayrollId":this.props.selectedPayrollData.companyPayrollId
                }
            };
        } else {
            updatedData = {
                oldPayrollNameData: this.props.editedPairollPeriodName,
                data: {
                    "companyCode": this.props.selectedPayrollData.companyCode,
                    "payrollType": this.props.selectedPayrollData.payrollType,
                    "periodName": NewPayrollPeriodName,
                    "startDate": NewPayrollPeriodStartDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                    "endDate": NewPayrollPeriodEndDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                    "isActive": !NewPayrollPeriodHidden,
                    "recordStatus": "N",
                    "modifiedBy": this.props.loginUser,
                    "companyPayrollId":this.props.selectedPayrollData.companyPayrollId
                }
            };
        }
        this.props.actions.UpdatePayrollPeriodName(updatedData);
        this.props.actions.EditPayrollPeriodName({});//Sanity Defect -183 Fix
        document.getElementById('createPayrollPeriodModalClose').click();
        this.setState({ payrollStartDate:'',payrollEndDate:'',newPayrollPeriodName:'' });

    }
    //Delete Payroll Period Name
    deletePayrollPeriodName = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.PAYROLL_PERIOD_NAME_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedPayrollPeriodName,
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
            IntertekToaster(localConstant.companyDetails.payroll.SELECT_ONE_RECORD_TO_DELETE,'warningToast minOneRecordSelect');
        }
    }
    deleteSelectedPayrollPeriodName = () => {
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeletePayrollPeriodName(selectedRecords);
        this.props.actions.HideModal();
        this.forceUpdate();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    //handleOverrideCostSaleReference
    handleOverrideCostSaleReference = (e) => {
        this.props.actions.UpdateOverrideCostSaleReference(e.target.checked);
    }

    duplicateDatas=(name)=>{
        const payrollData=this.props.PayrollData && this.props.PayrollData.find(x=>x.payrollType.toUpperCase() == name.toUpperCase());
        return payrollData;
    }
    // Generate the Copy Payroll Name
    generateCopiedPayrollName = (originalName) =>{
        let count=0;
        let name="Copy of " + originalName;
        if(name.length >20)
            name=name.substring(0,20);
        while(true){
            const payrollData=this.duplicateDatas(name);
            if(!payrollData)
                break;
            ++count;
            name="Copy (" + count+ ") of "+ originalName;
            if (name.length > 20) name = name.substring(0, 20);
        }
        return name;
    }
    /**
     * Copy Selected Payroll
     */
    copyPayrollHandler = () =>{
        const copyPayroll = Object.assign({},this.props.selectedPayrollData);
        const copyPayrollName = this.generateCopiedPayrollName(this.props.selectedPayrollData.payrollType);
            copyPayroll.payrollType = copyPayrollName;
            copyPayroll.recordStatus = 'N';
            copyPayroll.companyPayrollId = Math.floor(Math.random() * 99) -100;
            this.props.actions.AddNewPayroll(copyPayroll);
            this.props.PayrollPeriodName && this.props.PayrollPeriodName.map(iteratedValue => {
                if (iteratedValue.companyPayrollId === this.props.selectedPayrollData.companyPayrollId && iteratedValue.recordStatus !== "D") 
                {    
                    iteratedValue.isActive=true;//Company validation as per EVO
                    this.copyPayrollPeriod(copyPayroll, iteratedValue);
                }
            });
            this.props.actions.UpdateSelectedPayrollData(copyPayroll);
            IntertekToaster(localConstant.companyDetails.payroll.PAYROLL_COPY_SUCCESS,'warningToast payrollCopySuccess');
            // this.setState({ selectedPayrollData: copyPayroll, SelectedPayrollName: copyPayroll.payrollType });
    }

    /**
     * Copy payroll period for the selected payroll
     */
    copyPayrollPeriod = (payroll,iteratedValue) =>{
        const payrollPeriod = Object.assign({},iteratedValue);
        payrollPeriod.payrollType = payroll.payrollType;
        payrollPeriod.recordStatus = "N";
        payrollPeriod.periodStatus = "N";
        payrollPeriod.modifiedBy = this.props.loginUser;
        payrollPeriod.companyPayrollId =payroll.companyPayrollId;
        payrollPeriod.companyPayrollPeriodId =Math.floor(Math.random() * 99) -100;
        this.props.actions.AddNewPayrollPeriodName(payrollPeriod);
    }
    inputChangeHandler = (e) => {
        if(e.target.type === "number"){
            if(e.target.value != "0"){
                e.target.value=numberFormat(e.target.value);
            }
        }
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }

    getPayrollExportPrefix(payroll)
    {
        if(payroll!=null && payroll !== '' && payroll != undefined)
        {
        const payrolls = this.props.PayrollData;
        const selectedPayroll= payrolls.filter(function(item){return item.payrollType == payroll;});
          if(!selectedPayroll || selectedPayroll.length <=0)
                return '';
            else
            return selectedPayroll[0].exportPrefix;
        }
        else
            return '';
    }

    getPayrollCurrency(payroll)
    {
        if(payroll!=null && payroll !== '' && payroll != undefined)
        {
            const payrolls = this.props.PayrollData;
            const selectedPayroll= payrolls.filter(function(item){return item.payrollType == payroll;});

            if(!selectedPayroll || selectedPayroll.length <=0)
                return '';
           
        else 
            return selectedPayroll[0].currency;
                }
        else
            return '';
    }
    /**
     * Form Input data Change Handler
     */
    checkInt = (e) => {
        const result = this.inputChangeHandler(e);        
        this.props.actions.updateAvgTSHourlyCost(result.value);
    }

    render() {
        const currencyArray = [];
        this.props.currency && this.props.currency.map((iteratedValue) => {
            currencyArray.push({ value: iteratedValue.code });
        });
        const masterDivisionData = [];
        this.props.masterPayrolls && this.props.masterPayrolls.map((iteratedValue) => {
            masterDivisionData.push({ value: iteratedValue.name });
        });
        const payrollArray = [];
        const payrollNames = this.props.PayrollData && this.props.PayrollData.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus != "D";
        });
        
        let payrollPeriodNames = [];
        payrollPeriodNames = this.props.PayrollPeriodName && this.props.PayrollPeriodName.filter((iteratedValues) => {
            return iteratedValues.companyPayrollId == this.props.selectedPayrollData.companyPayrollId && iteratedValues.recordStatus !== "D";
        });

        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const CostOfSaleReference=arrayUtil.sort(this.props.CostOfSaleReference,"name",'asc');
        //createPayrollPopup modal
        return (
            <Fragment>
                {/* New Payroll Period Pop Up */}
                <CustomModal modalData={modelData} />
                <Draggable handle=".handle">
                <div id="createPayrollPeriod" className="modal popup-position">
                    <form className="col s12">
                        <div className="modal-content pb-0">
                            <div className="row mb-0">
                                <h6 className="mb-2 ml-0 mr-0 mt-0 handle">{this.props.showButton ?localConstant.companyDetails.payroll.ADD_PAYROLL_PERIOD_NAME:localConstant.companyDetails.payroll.EDIT_PAYROLL_PERIOD_NAME}
                                <i class={"zmdi zmdi-close right modal-close"}></i></h6>
                                <span class="boldBorder"></span>
                                <CustomInput
                                    hasLabel={true}
                                    label={localConstant.companyDetails.payroll.PERIOD_NAME}
                                    labelClass="customLabel mandate"
                                    type='text'
                                    name='newPayrollPeriodName'
                                    dataValType='valueText'
                                    colSize='m3 s12 mt-2'
                                    inputClass="customInputs browser"
                                    required={true}
                                    onValueChange={this.onInputChange}
                                    value={this.state.newPayrollPeriodName}                                    
                                    maxLength= {fieldLengthConstants.company.payRoll.PERIOD_NAME_MAXLENGTH}
                                />
                                <CustomInput
                                    hasLabel={true}
                                    isNonEditDateField={false}
                                    label={localConstant.companyDetails.payroll.START_DATE}
                                    labelClass="customLabel mandate"
                                    htmlFor="newPayrollPeriodStartDate"
                                    colSize='m3 s12 mt-2'
                                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                                    onDatePickBlur={this.handlePayrollStartDateBlur}
                                    type='date'
                                    selectedDate={this.state.payrollStartDate}
                                    onDateChange={this.handlePayrollStartDateChange}
                                    shouldCloseOnSelect={true}
                                    defaultValue={this.props.editedPairollPeriodName.startDate}//Sanity Defect -183 Fix
                                />
                                <CustomInput
                                    hasLabel={true}
                                    isNonEditDateField={false}
                                    labelClass="customLabel mandate"
                                    htmlFor="newPayrollPeriodEndDate"
                                    label={localConstant.companyDetails.payroll.END_DATE}
                                    onDatePickBlur={this.handlePayrollEndDateBlur}
                                    colSize='m3 s12 mt-2'
                                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                                    type='date'
                                    selectedDate={this.state.payrollEndDate}
                                    onDateChange={this.handlePayrollEndDateChange}
                                    shouldCloseOnSelect={true}
                                    defaultValue={this.props.editedPairollPeriodName.endDate}//Sanity Defect -183 Fix

                                />
                                <CustomInput
                                    type='switch'
                                    switchLabel={localConstant.companyDetails.payroll.HIDDEN}
                                    isSwitchLabel={true}
                                    checkedStatus={this.props.editedPairollPeriodName && !this.props.editedPairollPeriodName.isActive}
                                    //isSwitchLabel={this.props.editedPairollPeriodName.isActive}
                                    switchName="hidden"
                                    id="payrollPeriodNameStatus"
                                    colSize='m3 s12 mt-2'
                                />
                            </div>
                        </div>
                        <div className="modal-footer col s12">
                            <button type="reset" id="createPayrollPeriodModalClose" className="modal-close waves-effect waves-teal btn-small mr-2" onClick={this.cancelPayrollPeriod}>{localConstant.commonConstants.CANCEL}</button>
                            {
                                this.props.showButton ?
                                    <button onClick={this.addNewPayrollPeriodName} className="waves-effect waves-teal btn-small ml-2">{localConstant.commonConstants.SUBMIT}</button> :
                                    <button onClick={this.UpdatePayrollPeriodName} className="waves-effect waves-teal btn-small ml-2">{localConstant.commonConstants.SUBMIT}</button>
                            }
                        </div>
                    </form>                    
                </div>
                </Draggable>
                {/* New Payroll */}
                <Draggable handle=".handle">
                <div id="createPayrollPopup" className="modal popup-position">
                    <form className="col s12" id="payrollTypeForm">
                        <div className="modal-content pb-0">
                            <div className="row mb-0">
                                <h6 className="mb-2 ml-0 mr-0 mt-0 handle">{this.props.isEditCompanyPayroll?localConstant.companyDetails.payroll.EDIT_PAYROLL:localConstant.companyDetails.payroll.CREATE_PAYROLL}
                                <i class={"zmdi zmdi-close right modal-close"}></i></h6>
                                <span class="boldBorder"></span>
                                <CustomInput
                                    hasLabel={true}
                                    label={localConstant.companyDetails.payroll.PAYROLL_NAME}
                                    labelClass="customLabel mandate"
                                    type='text'
                                    inputName='newPayrollName'
                                    htmlFor="newPayrollName"
                                    colSize='m3 s12 mt-2'
                                    inputClass="customInputs browser"
                                    required={true}
                                    maxLength= {fieldLengthConstants.company.payRoll.PAYROLL_NAME_MAXLENGTH}
                                    // defaultValue={this.state.selectedPayrollData.payrollType}
                                />
                                <CustomInput
                                    hasLabel={true}
                                    label={localConstant.companyDetails.payroll.EXPORT_PREFIX}
                                    labelClass="customLabel mandate"
                                    type='text'
                                    inputName='payrollNewExportPrefix'
                                    htmlFor="payrollNewExportPrefix"
                                    colSize='m3 s12 mt-2'
                                    inputClass="customInputs browser"
                                    required={true}
                                    maxLength= {fieldLengthConstants.company.payRoll.EXPORT_PREFIX_MAXLENGTH}
                                    // defaultValue={this.state.selectedPayrollData.exportPrefix}
                                />
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    labelClass="customLabel mandate"
                                    required={true}
                                    label={localConstant.companyDetails.payroll.CURRENCY}
                                    type='select'
                                    colSize='s12 m4 mt-2'
                                    className="browser-default customInputs"
                                    optionsList={currencyArray}
                                    optionName='value'
                                    optionValue="value"
                                    id="payrollNewCurrency"
                                    // defaultValue={this.state.selectedPayrollData.currency}
                                />
                            </div>
                        </div>
                        <div className="modal-footer col s12">
                            <button type="reset" onClick={this.formReset} id="createPayrollModalClose" className="modal-close waves-effect waves-teal btn-small mr-2">{localConstant.commonConstants.CANCEL}</button>
                            {this.props.isEditCompanyPayroll ?
                                <button onClick={this.editCompanyPayroll} className="waves-effect waves-teal btn-small mr-2">{localConstant.commonConstants.SUBMIT}</button> :
                                <button onClick={this.addNewPayroll} className="waves-effect waves-teal btn-small mr-2">{localConstant.commonConstants.SUBMIT}</button>
                            }
                        </div>
                    </form>
                </div>
                </Draggable>

                <div className="customCard mt-0">
                    <p className="bold">{localConstant.companyDetails.payroll.PAYROLL}</p>
                    <div className="row">
                        {/* <a className="right danger-txt mr-4">*{localConstant.validationMessage.ALL_FIELDS_ARE_MANDATORY} </a> */}

                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.companyDetails.payroll.PAYROLL_NAME}
                            type='select'
                            colSize='s6 m4'
                            className="browser-default customInputs"
                            optionsList={payrollNames}
                            optionName='payrollType'
                            optionValue="companyPayrollId"
                            id="loadedPayroll"
                            onSelectChange={(e) => this.loadParollPeriodNames(e)}
                            // defaultValue = {this.state.selectedPayrollData.companyPayrollId}
                            defaultValue ={ this.props.selectedPayrollData.companyPayrollId}
                    
                        />
                        {this.props.pageMode !== localConstant.commonConstants.VIEW &&
                            <div className="col s12 m6">
                                <a href="#createPayrollPopup" onClick={this.handleCreatePayroll} className="waves-effect modal-trigger btn-small btn-primary mr-1 customCreateDiv">{localConstant.companyDetails.payroll.ASSIGN_PAYROLL}</a>
                                <a href="#createPayrollPopup" onClick={this.payrollEditHandler} disabled={isEmptyOrUndefine(this.props.selectedPayrollData.payrollType)}//D45 - Changes
                                    className="btn-small btn-primary mr-1 editTxtColor modal-trigger waves-effect customCreateDiv">{localConstant.companyDetails.common.EDIT}</a>
                                <a className="btn-small btn-primary mr-1 dangerBtn modal-trigger waves-effect customCreateDiv" href="#confirmation_Modal"
                                    onClick={this.payrollDeleteHandler} disabled={isEmptyOrUndefine(this.props.selectedPayrollData.payrollType)}>{localConstant.companyDetails.common.DELETE}</a>
                                <a className="btn-small btn-primary mr-1 customCreateDiv" onClick={this.copyPayrollHandler}
                                    disabled={isEmptyOrUndefine(this.props.selectedPayrollData.payrollType)}>{localConstant.companyDetails.payroll.COPY_PAYROLL}</a>
                            </div>}
                    </div>
                    <div className="row">
                        <div className="col s5 labelPrimaryColor">
                            <span>
                                <span className="bold">{localConstant.companyDetails.payroll.EXPORT_PREFIX}:</span> {this.getPayrollExportPrefix(this.props.selectedPayrollData.payrollType)} |
                            </span>
                            <span>
                                <span className="bold">{localConstant.companyDetails.payroll.CURRENCY}: </span>{this.getPayrollCurrency(this.props.selectedPayrollData.payrollType)}
                            </span>
                        </div>
                    </div>
                    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.payroll.PAYROLL_PERIOD_NAME} colSize="s12">
                            <ReactGrid gridRowData={payrollPeriodNames} gridColData={payrollHeaderData} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.payrollPeriod}/>
                      
                       {this.props.pageMode!==localConstant.commonConstants.VIEW && <div className="right-align mr-0 mt-2">
                            <a href="#createPayrollPeriod" onClick={this.handleNewPayrollPeriodName} className="waves-effect modal-trigger btn-small" disabled={isEmptyOrUndefine(this.props.selectedPayrollData.payrollType)}>{localConstant.commonConstants.ADD} </a>
                            <a href="#confirmation_Modal" onClick={this.deletePayrollPeriodName} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " disabled={isEmptyOrUndefine(this.props.selectedPayrollData.payrollType)}>{localConstant.commonConstants.DELETE}</a>
                        </div>}
                    </CardPanel>
                    <div className="row">
                    <div className="col s4 left-align mt-3">
                        <label>
                            <input type="checkbox" className="filled-in" onClick={this.handleOverrideCostSaleReference} disabled={this.props.interactionMode} defaultChecked={this.props.isCOSEmailOverrideAllow ? true : false} />
                            <span className="labelPrimaryColor">{localConstant.companyDetails.payroll.OVERRIDE_COST_OF_SALES_EMAIL}</span>
                        </label>
                    </div>

                    <CustomInput
                        hasLabel={true}
                        label={localConstant.companyDetails.payroll.AVERAGE_TS_HOURLY_CHARGED}
                        labelClass="customLabel"
                        type='text'
                        dataType='decimal'
                        isLimitType={true}
                        prefixLimit={18}
                        suffixLimit={2}
                        min="0"
                        inputName='avgTSHourlyCost'
                        htmlFor="avgTSHourlyCost"
                        colSize='m2 s12'
                        inputClass="customInputs browser"
                        defaultValue={this.props.avgTSHourlyCost && formatToDecimal(this.props.avgTSHourlyCost,2)}   
                        onValueChange={this.checkInt}
                        name="avgTSHourlyCost"
                        readOnly={this.props.interactionMode}
                    />

                    </div>
                    <CardPanel className="white lighten-4 black-text" title={localConstant.companyDetails.payroll.COST_OF_SALE_REFERENCES} colSize="s12">
                    <ReactGridTwo gridRowData={CostOfSaleReference} gridColData={sellReferrenceHeaderData} paginationPrefixId={localConstant.paginationPrefixIds.costOfSaleReference}/>
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}
Payroll.propTypes = {
    gridRowData: PropTypes.array.isRequired,
    headerData: PropTypes.array.isRequired
};

Payroll.defaultprops = {
    gridRowData: [],
    headerData: []
};
export default Payroll;