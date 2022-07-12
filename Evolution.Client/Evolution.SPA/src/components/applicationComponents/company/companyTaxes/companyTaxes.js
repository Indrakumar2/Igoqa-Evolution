import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();
const CompanyTaxesPopup = (props) => (
    <div className="row mb-0">        
        <CustomInput
            type='selectOptionGroup'
            hasLabel={true}
            label={localConstant.modalConstant.TAX}
            divClassName='col'
            colSize='s12'
            labelClass='mandate'
            optionGroups={props.taxList}
            optionGroupLabelKey='label'
            optionKeyValue="type"
            optionKeyName="name"
            subOption="options"
            htmlFor="ddlTaxMaster"
            name="tax"            
            defaultValue={props.selectedRow.tax}
            onSelectGroupChange={props.onChange}
        />
    </div>
);

class CompanyTaxes extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.groupedTaxMasterData = [];
        this.selectedText = "";
        this.selectedValue = "";
        this.state = {
            isOpen: false
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
    }

    componentDidMount() {
        const tab = document.querySelectorAll('.tabs');
        const tabInstances = MaterializeComponent.Tabs.init(tab);
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select);

        const datePicker = document.querySelectorAll('.datepicker');
        const datePickerInstances = MaterializeComponent.Datepicker.init(datePicker);
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });
    }    

    selectChangeHandler = (e) => {        
        this.updatedData[e.target.name] = e.target.value;
        const select = document.getElementById('ddlTaxMaster'), // the <select> element
        selectedIndex = select.selectedIndex, // 0-based index of which element in the <select> is selected
        selectedElement = select.options[selectedIndex]; // DOM element selected
        this.selectedText = selectedElement.text;
        this.selectedValue = selectedElement.value;
        //optGroup = selectedElement.parentNode; // <optgroup> element        
        //optGroupOptions = optGroup.children, // <option> elements in the <optgroup>
        //positionInOptGroup = Array.prototype.indexOf.call(optGroupOptions, selectedElement); // the selected <option>'s position in the <optgroup>        
        //alert(positionInOptGroup);
    }

    companyTaxesSubmitHandler = (e) => {        
        e.preventDefault();
        let alreadySelected = false;
        if (this.props.showButton === true) {
            const selectedCompanyTax = this.props.editCompanyTaxes.tax;            
            if (this.updatedData.tax === "") {
                IntertekToaster(localConstant.companyDetails.common.PLEASE_SELECT_A_TAX,'warningToast SelectoneTaxWarning');               
            }
            else {               
                if(Object.keys(this.selectedText).length > 0){                    
                this.props.taxMasterData.map(taxMaster => {
                    if((taxMaster.name.trim() === this.selectedText.trim()) && (taxMaster.type.trim() === this.selectedValue.trim())){
                        this.updatedData["taxType"] = taxMaster.type;
                        this.updatedData["taxRate"] = taxMaster.rate;
                        this.updatedData["tax"] = taxMaster.name;
                    }
                });
            }
                if (this.props.CompanyTaxes) {                    
                    this.props.CompanyTaxes.map(result => {
                        if (result.tax === this.updatedData.tax && result.tax !== selectedCompanyTax) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                            else {                                
                                this.updatedData = result;                                
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.companyDetails.common.TAX_ALREADY_SELECTED,'warningToast alreadyTaxWarning');                   
                }
                else {
                    if (this.props.editCompanyTaxes.recordStatus !== "N" && this.updatedData.recordStatus !== 'D') {
                        this.updatedData["recordStatus"] = "M";
                        this.updatedData["modifiedBy"] = this.props.loggedInUser;
                        this.props.actions.UpdateCompanyTaxes(this.updatedData);
                    }
                    else if (this.props.editCompanyTaxes.recordStatus === "N") {
                        this.props.actions.UpdateCompanyTaxes(this.updatedData);
                    }
                    else {
                        const editCompanyTaxArray = [ this.props.editCompanyTaxes ];
                        this.props.actions.DeleteCompanyTaxes(editCompanyTaxArray);
                        this.updatedData["recordStatus"] = null;
                        this.props.actions.UpdateCompanyTaxes(this.updatedData);
                    }

                    this.updatedData = {}; this.selectedValue = {}; this.selectedText = {};
                    document.getElementById("taxPopup").reset();
                    document.getElementById("cancelCompanyTaxesSubmit").click();
                }
            }
        }
        if (this.props.showButton === false) {                
            if (Object.keys(this.selectedText).length === 0) {
                IntertekToaster(localConstant.companyDetails.common.PLEASE_SELECT_A_TAX,'warningToast oneTaxWarning');               
            }
            else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["modifiedBy"] = this.props.loggedInUser;                
                this.props.taxMasterData.map(taxMaster => {
                    if((taxMaster.name.trim() === this.selectedText.trim()) && (taxMaster.type.trim() === this.selectedValue.trim())){
                        this.updatedData["taxType"] = taxMaster.type;
                        this.updatedData["taxRate"] = taxMaster.rate;
                        this.updatedData["tax"] = taxMaster.name;
                    }
                });
                if (this.props.CompanyTaxes) {                   
                    this.props.CompanyTaxes.map(result => {                                                
                        if (result.tax === this.updatedData.tax) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                            else {
                                this.updatedData = result;
                                this.updatedData["recordStatus"] = null;
                            }
                        }
                    });
                }

                if (alreadySelected === true) {
                    IntertekToaster(localConstant.companyDetails.common.TAX_ALREADY_SELECTED,'warningToast alreadySelectedTaxWarn');                    
                }
                else {
                    if (this.updatedData.recordStatus === "N") {
                        this.updatedData["companyTaxId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                        this.updatedData["companyCode"] = this.props.selectedCompanyCode;
                        this.props.actions.AddCompanyTaxes(this.updatedData);
                    }
                    else {
                        this.props.actions.UpdateCompanyTaxes(this.updatedData);
                    }
                    this.updatedData = {}; this.selectedValue = {}; this.selectedText = {};
                    document.getElementById("taxPopup").reset();
                    document.getElementById("cancelCompanyTaxesSubmit").click();
                }
            }
        }
    }

    companyTaxesDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COMPANY_TAXES_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [ {
                    buttonName: "Yes",
                    onClickHandler: this.deleteSelected,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                } ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast oneRowSelectWarn');
        }
    }

    deleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteCompanyTaxes(selectedData);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    clearData = () => {
        document.getElementById("taxPopup").reset();
        this.props.actions.ShowButtonHandler();
        this.updatedData = {}; this.selectedValue = {}; this.selectedText = {};
    }

    render() {        
        const { CompanyTaxes, editCompanyTaxes, taxMasterData, showButton } = this.props;
        const headData = HeaderData;        
        let salesTaxData = [];
        let withHoldTaxData = [];
        const rowData = CompanyTaxes && CompanyTaxes.filter(x => x.recordStatus != "D");
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        if (taxMasterData.length > 0) {
            salesTaxData = taxMasterData.filter(taxType => taxType.type === "S");
            withHoldTaxData = taxMasterData.filter(taxType => taxType.type === "W");
            this.groupedTaxMasterData = [ { "label": "Sales Tax", "options": salesTaxData }, { "label": "Withholding Tax", "options": withHoldTaxData } ];
        }
        return (
            <Fragment>
                <CustomModal modalData={modelData} />              
                    <div id="addCompanyTaxes" className="modal popup-position">
                        <form id="taxPopup" onSubmit={this.companyTaxesSubmitHandler}>
                            <div className="modal-content">
                                <h6>Company Taxes</h6>
                                <CompanyTaxesPopup taxList={this.groupedTaxMasterData} onChange={this.selectChangeHandler} selectedRow={editCompanyTaxes} />
                            </div>
                            <div className="modal-footer">
                                <button type="button" id="cancelCompanyTaxesSubmit" onClick={this.clearData} className="modal-close waves-effect waves-teal btn-small mr-2">CANCEL</button>
                                {!showButton ?
                                    <button type="submit" className="btn-small">SUBMIT</button> :
                                    <button type="submit" className="btn-small">SUBMIT</button>}
                            </div>
                        </form>
                    </div>

                    <div className="customCard">
                        <p><span className='bold'>Company Taxes</span></p>
                        <div className="customCard">
                            <ReactGrid gridRowData={rowData} gridColData={headData} onRef={ref => { this.child = ref; }}  paginationPrefixId={localConstant.paginationPrefixIds.companyTax} />
                        </div>
                        {this.props.pageMode!==localConstant.commonConstants.VIEW  && <div className="right-align">
                            <a onClick={this.clearData} href="#addCompanyTaxes" className="btn-small waves-effect modal-trigger">{localConstant.commonConstants.ADD}</a>
                            <a href="#confirmation_Modal" className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " onClick={this.companyTaxesDeleteClickHandler}>{localConstant.commonConstants.DELETE}</a>
                        </div>}
                    </div>               
            </Fragment>
        );
    }
}

export default CompanyTaxes;