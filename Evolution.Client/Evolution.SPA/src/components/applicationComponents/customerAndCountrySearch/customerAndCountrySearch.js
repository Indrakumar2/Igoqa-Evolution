import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../common/baseComponents/reactAgGridTwo';
import { HeaderData } from './customerHeaderData.js';
import CustomInput from '../../../common/baseComponents/inputControlls';
import Modal from '../../../common/baseComponents/modal';
import { getlocalizeData, isEmpty, mergeobjects } from '../../../utils/commonUtils';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();
class SearchCustomer extends Component {
    constructor(props) {
        super(props);
        this.textInputCreateRef= React.createRef();
    }
    customerNameFocus() {
        this.textInputCreateRef.current.focus();
        // document.getElementById("customerNameSearchId").focus();
     }
    
    render() {
        return (<form>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                name='customerName'
                htmlFor="customerNameSearchId"
                divClassName='col' label={localConstant.companyDetails.generalDetails.NAME}
                type='text'
                colSize='s6'
                inputClass="customInputs"
                dataValType='valueText'
                onValueChange={this.props.searchCustomer}
                onValueKeyDown={this.props.handlerKeyDown}
                value={this.props.defaultCustomerName } autocomplete="off"
                onValueBlur={this.props.onBlur}
                refProps={this.textInputCreateRef} 
                />

            <CustomInput hasLabel={true}
                name='operatingCountry'
                divClassName='col'
                label={localConstant.modalConstant.COUNTRY}
                type='select'
                colSize='s6'
                inputClass="customInputs"
                optionsList={this.props.countryMasterData}
                optionName='name'
                optionValue="name"
                className="browser-default"
                onSelectChange={this.props.selectedCountrySearch}
                defaultValue={this.props.defaultCountryName} />
        </div>
        </form>
        );
    };
}
   
class CustomerAndCountrySearch extends Component {
    constructor(props) {
        super(props);
        this.state = {  
            CustomerSearchModalOpen:false
        };
        this.updatedInputData = {};
        this.selectedcustomerName = []; 
        this.btnSearchCustomerPopup= React.createRef();
        this.parentChildRef= React.createRef();
    }
     
    componentDidMount() {
        // this.props.actions.ClearData();//D0648-GENERAL-Customer name retained in Create Contract
    }

    componentWillUnmount() {
        this.updatedInputData = {};
        //  this.props.actions.ClearData();
    }

    handlerChange = (e) => {
        this.updatedInputData.customerName = this.props.isReport ? this.props.defaultReportCustomerName : this.props.defaultCustomerName;
        this.updatedInputData[e.target.name] = e.target.value;
        if (e.target.name === 'customerName' && (e.target.value === '' || e.target.value === null || e.target.value === undefined)) {
            this.updatedInputData.operatingCountry = '';
            this.props.actions.SetSelectedCountryName('');
            this.props.actions.ClearData();
        }
        if (this.props.contractHoldingCompany === '')
            this.updatedInputData.operatingCountry = this.props.contractHoldingCompany;
        if (this.updatedInputData.customerName !== undefined) {
            if (this.props.isReport) {
                this.props.actions.SetDefaultReportCustomerName(this.updatedInputData);
            }
            else {
                this.props.actions.SetDefaultCustomerName(this.updatedInputData);
            }
        }
    }

    handlerKeyDown = (e) => {
        this.updatedInputData[e.target.name] = e.target.value;
        this.updatedInputData.customerName = this.updatedInputData.customerName.trim();
        const strValue = e.target.value;
        if (this.updatedInputData.customerName !== '' && (e.keyCode === 9 || e.keyCode === 13)) {
            if (e.keyCode === 9 || e.keyCode === 13) {
                if (strValue[strValue.length - 1] != '*') {
                    this.updatedInputData.customerName = strValue + '*';
                }
                if (this.props.isReport) {
                    this.props.actions.SetDefaultReportCustomerName(this.updatedInputData);
                }
                else {
                    this.props.actions.SetDefaultCustomerName(this.updatedInputData);
                }
            }

           // this.selectCustomerSearch(e);
        }
        if (this.updatedInputData.customerName !== '' && e.keyCode === 13) {
            if (strValue[strValue.length - 1] != '*') {
                e.target.value = strValue + '*';
            }
            this.updatedInputData.customerName = e.target.value;
            this.props.actions.FetchCustomerList(this.updatedInputData);
            this.updatedInputData.customerName = this.updatedInputData.customerName.replace(/%26/g, "&");
            if (this.props.isReport) {
                this.props.actions.SetDefaultReportCustomerName(this.updatedInputData);
            }
            else {
                this.props.actions.SetDefaultCustomerName(this.updatedInputData);
            }
        }
        if (e.keyCode === 13) {
            e.preventDefault();
        }
    }

    //Clear Search Data
    clearSearchData = () => {
        this.updatedInputData = {};
        this.props.actions.ClearSearchData();
    }
  
    //selected customer Country Change
    selectedCustomerCountrySearch = (e) => {
        e.preventDefault();
        if (e.target.name === 'operatingCountry') {
            if (this.props.isReport) {
                this.props.actions.SetReportSelectedCountryName(e.target.value);
            }
            else {
                this.props.actions.SetSelectedCountryName(e.target.value);
            }
        }
        this.updatedInputData[e.target.name] = e.target.value;
        this.props.actions.FetchCustomerList(this.updatedInputData);
    }

    //selected Customer search on Text Box Onchange 
    OnChangeSearchCustomer = (e) => {
        e.preventDefault();
        this.updatedInputData[e.target.name] = e.target.value;
        this.updatedInputData.customerName = this.updatedInputData.customerName;
        if (e.keyCode === 9 || e.keyCode === 13) {
            this.updatedInputData.customerName = e.target.value + '*';
        }
        if (this.props.isReport) {
            this.props.actions.SetDefaultReportCustomerName(this.updatedInputData);
        }
        else {
            this.props.actions.SetDefaultCustomerName(this.updatedInputData);
        }
    }

    //onButton Click Open Modalpoup
    selectCustomerSearch = (e) => {
        e.preventDefault();

        this.updatedInputData = {};
        let customerName = this.props.isReport ? this.props.defaultReportCustomerName ? this.props.defaultReportCustomerName : this.props.reportsCustomerName : this.props.defaultCustomerName ? this.props.defaultCustomerName : this.props.customerName ? this.props.customerName : "";
        customerName = customerName.trim();
        if (e.keyCode === 9 || e.keyCode === 13) {
            const strValue = e.target.value;
            if (strValue[strValue.length - 1] != '*') {
                customerName = strValue + '*';
            }
        }

        if (customerName !== undefined && customerName !== "" && customerName[customerName.length - 1] != '*') {
            customerName = customerName + '*';
        }

        this.updatedInputData.customerName = customerName.trim();
        //this.props.actions.ContractShowModal(this.updatedInputData);
        // this.props.actions.ShowModalPopup();
        this.setState({
            CustomerSearchModalOpen: true
        }, () => {
            this.parentChildRef.current.customerNameFocus();
        });
        if (this.updatedInputData.customerName !== "" && this.updatedInputData.customerName !== undefined) {
            this.props.actions.FetchCustomerList(this.updatedInputData);
            this.updatedInputData.customerName = this.updatedInputData.customerName.replace(/%26/g, "&");
            if (this.props.isReport) {
                this.props.actions.SetDefaultReportCustomerName(this.updatedInputData);
            }
            else {
                this.props.actions.SetDefaultCustomerName(this.updatedInputData);
            }
        }
        else {
            if (this.props.isReport) {
                if(!this.updatedInputData.operatingCountry){
                     this.props.actions.SetReportSelectedCountryName('');
                }
            }
        }
    }

    onModalBlur =(e)=>{
        this.updatedInputData[e.target.name] = e.target.value;
        this.updatedInputData.customerName = this.updatedInputData.customerName.trim();
        if (this.updatedInputData.customerName !== '') {
            const strValue = e.target.value;
            if (strValue[strValue.length - 1] != '*') {
                e.target.value = strValue + '*';
            }
            this.updatedInputData.customerName = e.target.value;
        }
    }
    //get customer Name From Popup AG Grid 
    getCustomerName = (e) => {
        e.preventDefault();
        const selectedCustomer = this.child.getSelectedRows();
        if (this.props.isReport) {
            selectedCustomer.map((customer) => {
                this.selectedcustomerName = [];
                this.selectedcustomerName.push(customer);
                this.props.OnChangeSearchCustomer(this.selectedcustomerName);
            });
            this.setState({
                CustomerSearchModalOpen: false
            });
            // console.log(this.updatedInputData);
            this.btnSearchCustomerPopup.current.focus();
        }
        else {
            if (this.props.customerList && this.props.customerList.length > 0) {
                if (isEmpty(selectedCustomer)) {
                    IntertekToaster('Select Customer Name', 'warningToast customerSearchCustomerNameReq');
                }
                else {
                    selectedCustomer.map((customer) => {
                        this.selectedcustomerName = [];
                        this.selectedcustomerName.push(customer);
                        this.props.actions.OnSubmitCustomerName(this.selectedcustomerName);
                        const obj = mergeobjects(this.updatedInputData, customer);
                        if (this.props.isReport) {
                            this.props.actions.SetDefaultReportCustomerName(obj);
                        }
                        else {
                            this.props.actions.SetDefaultCustomerName(obj);
                        }
                    });
                    // this.props.actions.HideModalPopup();
                    this.setState({
                        CustomerSearchModalOpen: false
                    });
                }
                this.btnSearchCustomerPopup.current.focus();
                this.props.contractCustomerChange && this.props.contractCustomerChange(selectedCustomer[0].customerCode);
            }
        }
    }
   
    hideModal = (e) => {
        e.preventDefault();
        this.updatedInputData = {};
        this.btnSearchCustomerPopup.current.focus();
        this.setState({
            CustomerSearchModalOpen: false
        });
        if (this.props.isReport) {
            this.props.ClearReportsData();
            this.props.actions.ClearData();
            this.props.actions.SetReportSelectedCountryName('');
        }
        else {
            this.props.actions.ClearData();
        }
    }
    
    render() {
        const { customerName,disabled ,interactionMode } = this.props;
        const selectedCustomerName=this.props.isReport?this.props.reportsCustomerName?this.props.reportsCustomerName:this.props.defaultReportCustomerName: customerName ? customerName : this.props.defaultCustomerName;
      
        return (
            <Fragment>
                <Modal id="contractModalPopup"
                    title={'Customer list '}
                    modalClass="contractModal"
                    buttons={[
                        {
                            name: 'Cancel', action: this.hideModal,
                            type: "reset",
                            btnClass: 'btn-small mr-2',
                            showbtn: true
                        },
                        {
                            name: 'Submit',
                            type: "submit",
                            action: this.getCustomerName,
                            btnClass: 'btn-small',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.state.CustomerSearchModalOpen}>
                    <SearchCustomer countryMasterData={this.props.countryMasterData}
                        searchCustomer={this.OnChangeSearchCustomer}
                        defaultCustomerName={this.props.isReport? this.props.defaultReportCustomerName: this.props.defaultCustomerName}
                        handlerChange={this.handlerChange}
                        handlerKeyDown={this.handlerKeyDown}
                        selectedCountrySearch={this.selectedCustomerCountrySearch}
                        onBlur={this.onModalBlur}
                        defaultCountryName={this.props.isReport?this.props.selectedReportCountryName:this.props.selectedCountryName}
                        ref={this.parentChildRef}
                        />
                    <ReactGrid gridRowData={this.props.customerList}
                        gridColData={HeaderData} onRef={ref => { this.child = ref; }} />
                </Modal>
                    <div className={this.props.colSize ? this.props.colSize :"col pl-0 "+this.props.divClassName} >
                    
                    <CustomInput
                    dataValType='valueText'
                    hasLabel={true}
                    divClassName='col customerSearchBox'
                    label={localConstant.customer.CUSTOMER_NAME}
                    labelClass={this.props.isSupplier ? " " : (!this.props.isMandate ? '' : "mandate")}
                    type='text'
                    colSize='s11 pr-0'
                    name='customerName'
                    inputClass="customInputs"
                    maxLength={200}
                    value={ selectedCustomerName }
                    onValueFocus={this.handlerFocus}
                    onValueKeyDown={this.handlerKeyDown}
                    onValueChange={this.handlerChange} 
                    onValueBlur={this.onValueBlur}
                    autocomplete="off"
                    readOnly={interactionMode}
                    disabled={disabled}
                    /> 
                        <div className="customerSearchButton">
                        <button type="button" ref={this.btnSearchCustomerPopup}  disabled={disabled} className="waves-effect waves-green btn p-2 btn-lineHeight mt-4x" onClick={(e) => this.selectCustomerSearch(e)} >...</button>
                        </div>
                </div>
            </Fragment>
        );
    }
}

export default CustomerAndCountrySearch;
