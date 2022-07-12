import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import { HeaderData } from './headerData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { numberFormat, isEmpty } from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData } from '../../../../utils/commonUtils';
import { required,requiredNumeric } from '../../../../utils/validator';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import Draggable from 'react-draggable'; // The default

const localConstant = getlocalizeData();

class ExpectedMarginByBusinessUnit extends Component {
    constructor(props) {
        super(props);
        this.updatedExpectedMarginData = {};
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
        const custModal = document.querySelectorAll('.modal');
        MaterializeComponent.Modal.init(custModal, { "dismissible": false });
        // this.props.actions.FetchCompanyExpectedMargin();
        
        //commented, Now we are getting business unit from master data reducer
        //this.props.actions.FetchBusinessUnit();
    }
    clearData = () => {
        document.getElementById("addExpectedMargin").reset();
        this.updatedExpectedMarginData = {};
        this.props.actions.ShowButtonHandler();
    }
    companyMarginDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COMPANY_EXPECTED_MARGIN_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelected,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast noRowSelectedWarning');
        }
    }

    deleteSelected = () => {
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteExpectedMargin(selectedRecords);
        this.props.actions.HideModal();
    }
    handleExpectedMarginChange = (e) => {
        if (e.target.type === "number") {
            e.target.value = numberFormat(e.target.value);           
        }
        //Added For Defect 880 -Starts
        if(e.target.name ==="minimumMargin"){
            const expression = ("(\\d{0,"+ parseInt(9)+ "})[^.]*");
            const rg = new RegExp(expression,"g"); 
            const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
            e.target.value = match[1]; 
        }
        //Added For Defect 880 -Ends
        this.updatedExpectedMarginData[e.target.name] = e.target.value;
    }
    expectedMarginSubmitHandler = (e) => {
        e.preventDefault();
        let alreadySelected = false;
        if (this.props.showButton === true) {
            const selectedBusinessUnitType = this.props.editExpectedMarginDetails.marginType;
            const updatedMarginData = Object.assign({},this.props.editExpectedMarginDetails,this.updatedExpectedMarginData);
            if (isEmpty(updatedMarginData) || required(updatedMarginData.marginType)) {
                IntertekToaster(localConstant.companyDetails.expectedMargin.SELECT_ANY_ONE_BUSINESS_UNIT, 'warningToast noBusinessSelectedWarning');
            }
            else if (requiredNumeric(updatedMarginData.minimumMargin)) {
                IntertekToaster(localConstant.companyDetails.expectedMargin.EXCPECTED_MARGIN_MINIMUMMARGIN, 'warningToast selectOneBusinessUnit');
            }
            else {

                if (this.props.expextedMarginDetail) {
                    this.props.expextedMarginDetail.map(result => {
                        if (result.marginType === this.updatedExpectedMarginData.marginType && result.marginType !== selectedBusinessUnitType) {
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
                    IntertekToaster(localConstant.companyDetails.expectedMargin.BUSINESS_UNIT_ALREADY_SELECTED, 'warningToast alreadybusinessUnitExist');
                }
                else {

                    if (this.props.editExpectedMarginDetails.recordStatus !== "N") {
                        this.updatedExpectedMarginData["recordStatus"] = "M";
                    }
                    this.updatedExpectedMarginData["modifiedBy"] = this.props.loggedInUser;
                    this.props.actions.UpdateExpectedMargin(this.updatedExpectedMarginData);
                    this.updatedExpectedMarginData = {};

                    document.getElementById("addExpectedMargin").value = "";
                    document.getElementById("addExpectedMargin").reset();
                    this.clearData();
                    document.getElementById("cancelExpectedMarginDetail").click();
                }

            }
        }
        if (this.props.showButton === false) {
            if (isEmpty(this.updatedExpectedMarginData) || required(this.updatedExpectedMarginData.marginType)) {
                IntertekToaster(localConstant.companyDetails.expectedMargin.SELECT_ANY_ONE_BUSINESS_UNIT, 'warningToast selectOneBusinessUnit');
            }
          else if (requiredNumeric(this.updatedExpectedMarginData.minimumMargin)) {
                IntertekToaster(localConstant.companyDetails.expectedMargin.EXCPECTED_MARGIN_MINIMUMMARGIN, 'warningToast selectOneBusinessUnit');
            }
            else {
                if (this.props.expextedMarginDetail) {
                    this.props.expextedMarginDetail.map(result => {
                        if (result.marginType === this.updatedExpectedMarginData.marginType) {
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
                    IntertekToaster(localConstant.companyDetails.expectedMargin.BUSINESS_UNIT_ALREADY_SELECTED, 'warningToast alreadyBusinessSelected');
                }

                else {
                    this.updatedExpectedMarginData["recordStatus"] = "N";
                    this.updatedExpectedMarginData["modifiedBy"] = this.props.loggedInUser;
                    this.updatedExpectedMarginData["companyExpectedMarginId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.props.actions.AddExpectedMargin(this.updatedExpectedMarginData);
                    this.updatedExpectedMarginData = {};

                    document.getElementById("addExpectedMargin").reset();
                    this.clearData();
                    document.getElementById("cancelExpectedMarginDetail").click();
                }
            }
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    render() {
        const headData = HeaderData;
        const { expextedMarginDetail, editExpectedMarginDetails, buisnessUnitMasterData } = this.props;
        const { showButton } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };

        let companyExpectedMarginData = [];
        if (expextedMarginDetail) {
            companyExpectedMarginData = expextedMarginDetail.filter(margin => margin.recordStatus !== "D");
        }

        return (
                <Fragment>
                <CustomModal modalData={modelData} />
                <Draggable handle=".handle">
                 <div id="add-expected-margin" className="modal popup-position">
                    <form onSubmit={this.expectedMarginSubmitHandler} id="addExpectedMargin" className="col s12 pl-0 pr-0">
                        <div className="modal-content">
                            <h6 className="handle mt-0 mb-0 ml-0 mr-0 ">{!showButton?"Add Expected Margin":"Edit Expected Margin"}
                            <i class={"zmdi zmdi-close right modal-close"}></i></h6>
                            <span class="boldBorder"></span>
                            <div className="row mt-2">
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    label='Business Unit'
                                    type='select'
                                    colSize='s6 mt-2'
                                    name="marginType"
                                    labelClass='mandate'
                                    className="browser-default"
                                    optionsList={buisnessUnitMasterData}
                                    optionName="name"
                                    optionValue="name"
                                    onSelectChange={this.handleExpectedMarginChange}
                                    defaultValue={editExpectedMarginDetails.marginType}
                                    id="bussiness"
                                />
                                {/* <CustomInput
                                    hasLabel={true}
                                    label='Minimum Expected Margin %'
                                    labelClass="customLabel mandate"
                                    divClassName="s6 numerInput"
                                    type='number'
                                    required={true}
                                    name='minimumMargin'
                                    colSize='s6'
                                    step="any"
                                    inputClass="customInputs"
                                    max="999999999"
                                    maxLength={12}
                                    defaultValue={editExpectedMarginDetails.minimumMargin}
                                    onValueChange={this.handleExpectedMarginChange}                                    
                                /> */}
                                <CustomInput
                                    hasLabel={true}
                                    label='Minimum Expected Margin %'
                                    labelClass="customLabel mandate"
                                    divClassName="s6 numerInput"
                                    type='text'
                                    name='minimumMargin'
                                    colSize='s6 mt-2'
                                    // step="any"
                                    inputClass="customInputs"
                                    max="999999999"
                                    maxLength={fieldLengthConstants.company.expectedMargin.MINIMUM_EXPECTED_MARGIN_MAXLENGTH }
                                    defaultValue={editExpectedMarginDetails.minimumMargin}
                                    onValueInput={this.handleExpectedMarginChange}  //Added For Defect 880 //changed for single digit handler not hitting
                                    // onValueBlur={this.handleExpectedMarginChange}                                    
                                />

                            </div>

                        </div>
                        <div className="modal-footer">
                            <button id="cancelExpectedMarginDetail" type="button" onClick={this.clearData} className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
                            {!showButton ? <button type="submit" className="btn-small">Submit</button>
                                :
                                <button type="submit" className="btn-small">Submit</button>}
                        </div>
                    </form>
                </div>
                </Draggable>

                <div className="customCard mt-0">
                    <p><span className='bold'>{localConstant.companyDetails.expectedMargin.EXPECTED_MARGIN_BY_BUSINESS_UNIT}</span></p>
                    
                    <ReactGrid gridRowData={companyExpectedMarginData} gridColData={headData} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.expectedMargin}/>
                  
                       {this.props.pageMode!==localConstant.commonConstants.VIEW &&<div className="right-align mt-2">
                            <a onClick={this.clearData} href="#add-expected-margin" className="btn-small waves-effect modal-trigger">{localConstant.commonConstants.ADD}</a>
                            <a href="#confirmation_Modal" className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn " onClick={this.companyMarginDeleteClickHandler}>{localConstant.commonConstants.DELETE}</a>
                        </div>}
                    </div>
                    </Fragment>
        );
    }
}

// ExpectedMarginByBusinessUnit.propTypes = {
//     rowData: PropTypes.array.isRequired,
//     headData: PropTypes.array.isRequired
// }

// ExpectedMarginByBusinessUnit.defaultprops = {
//     rowData: [],
//     headData: []
// }
export default ExpectedMarginByBusinessUnit;
