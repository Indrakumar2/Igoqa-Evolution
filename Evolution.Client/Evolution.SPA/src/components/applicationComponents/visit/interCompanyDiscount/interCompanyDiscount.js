import React, { Component, Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler, isEmpty, decimalCheck, numberFormat, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

const InterCompanyDiscountModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.visit.REFERENCE_TYPE}
                    type='select'
                    name='referenceType'
                    colSize='s6'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.stampType}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    type="text"
                    divClassName='col pr-0'
                    label={localConstant.visit.REFERENCE_VALUE}
                    labelClass="mandate"
                    dataType='numeric'
                    colSize='s6'
                    inputClass="customInputs"
                    onValueChange={props.onChangeHandler}
                    maxLength={20}
                    name='referenceValue'
                    min="0"
                />

            </div>

        </Fragment>
    );
};
class InterCompanyDiscount extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isInterCompanyDiscountShowModal: false,
            isInterCompanyDiscountEdit: false,
            };
        this.updatedData = {};
        //Add Buttons
        this.InterCompanyDiscountAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInterCompanyDiscountModal,
                btnClass: "btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addVisitDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        //Edit Buttons
        this.InterCompanyDiscountEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelInterCompanyDiscountModal,
                btnClass: "btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateInterCompanyDiscountDetails,
                btnClass: "btn-small",
                showbtn: true
            }
        ];
    }
    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }
    //Adding Stamp details to the grid
    addVisitDetails = (e) => {
        e.preventDefault();

        if (this.updatedData && !isEmpty(this.updatedData)) {
            this.hideInterCompanyDiscountModal();
            this.updatedData = {};
        }
    }

    showInterCompanyDiscountModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isInterCompanyDiscountShowModal: true,
                isInterCompanyDiscountEdit: false
            };
        });
        this.editedRowData = {};
    }
    //Hiding the modal
    hideInterCompanyDiscountModal = () => {
        this.setState((state) => {
            return {
                isInterCompanyDiscountShowModal: false,
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    //Cancel the updated data in model popup
    cancelInterCompanyDiscountRefModal = (e) => {
        e.preventDefault();
        this.updatedData = {};
        this.setState((state) => {
            return {
                isInterCompanyDiscountRefShowModal: false,
            };
        });
    }

    interCompanyDiscountsChangeHandler = (e) => {
        this.updatedData = this.props.InterCompanyDiscounts;
        const result = formInputChangeHandler(e);
        this.updatedData[e.target.name] = decimalCheck(result.value,2);        
             
        let { pchValue, chcValue, hcValue, aicoValue1, aicoValue2, pCHC, cHC, hC, aICO1, aICO2, finalDiscount, totalDiscount, discountNumbers } = 0;
        pCHC = this.updatedData.parentContractHoldingCompanyDiscount;
        pchValue = isNaN(pCHC) ? this.props.interCompanyDiscounts.parentContractHoldingCompanyDiscount === undefined ?
            null : this.props.interCompanyDiscounts.parentContractHoldingCompanyDiscount : pCHC;

        cHC = this.updatedData.assignmentContractHoldingCompanyDiscount;
        chcValue = isNaN(cHC) ? this.props.interCompanyDiscounts.assignmentContractHoldingCompanyDiscount === undefined ?
            null : this.props.interCompanyDiscounts.assignmentContractHoldingCompanyDiscount : cHC;

        hC = this.updatedData.assignmentHostcompanyDiscount;
        hcValue = isNaN(hC) ? this.props.interCompanyDiscounts.assignmentHostcompanyDiscount === undefined ?
            null : this.props.interCompanyDiscounts.assignmentHostcompanyDiscount : hC;

        aICO1 = this.updatedData.assignmentAdditionalIntercompany1_Discount;
        aicoValue1 = isNaN(aICO1) ? this.props.interCompanyDiscounts.assignmentAdditionalIntercompany1_Discount === undefined ?
            null : this.props.interCompanyDiscounts.assignmentAdditionalIntercompany1_Discount : aICO1;

        aICO2 = this.updatedData.assignmentAdditionalIntercompany2_Discount;
        aicoValue2 = isNaN(aICO2) ? this.props.interCompanyDiscounts.assignmentAdditionalIntercompany2_Discount === undefined ?
            null : this.props.interCompanyDiscounts.assignmentAdditionalIntercompany2_Discount : aICO2;
        totalDiscount = Number(pchValue) + Number(chcValue) + Number(aicoValue1) + Number(aicoValue2) + Number(hcValue);

        if (totalDiscount <= 100) {
            discountNumbers = [ 100, totalDiscount ];
            finalDiscount = discountNumbers.reduce(getDiscount);
        }
        function getDiscount(total, num) {
            return total - num;
        }
        
        this.updatedData['recordStatus'] = 'M';
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        
        if (finalDiscount >= 0 && result.name !== null) {
            this.updatedData["parentContractHoldingCompanyDiscount"] = pchValue;
            this.updatedData["assignmentContractHoldingCompanyDiscount"] = chcValue;
            this.updatedData["assignmentAdditionalIntercompany1_Discount"] = aicoValue1;
            this.updatedData["assignmentAdditionalIntercompany2_Discount"] = aicoValue2;
            this.updatedData["assignmentHostcompanyDiscount"] = hcValue;
            this.updatedData["assignmentOperatingCompanyDiscount"] = finalDiscount.toFixed(2);
            this.updatedData["AmendmentReason"]="";
            this.updatedData['ICDChange']=true;
            this.props.actions.UpdateICDiscounts(this.updatedData);
            this.updatedData={};
            
        } else {
            IntertekToaster(localConstant.assignments.VALUE_EXCEEDING, 'warningToast CreatRevDescData');
            e.target.value = "";
        }
        
    }

    checkNumber = (e) => {
        if(!isEmpty(e.target.value) ){
        e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        this.interCompanyDiscountsChangeHandler(e);
        }
    }

    stringValidation(value) {
        return (!this.props.isInterCompanyAssignment || value === null || value === undefined || value === '' ? "" : (typeof value === 'string' ? value : value.toFixed(2)));
    };

    componentDidMount() {
        this.props.actions.FetchInterCompanyDiscounts(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE);
        this.fetchContractDataForVisit();
    };

    fetchContractDataForVisit=() =>{
        const contractNumber = this.props.visitInfo.visitContractNumber;
        let contractType='';
        const data = {
            contractNumber: contractNumber
        };
        this.props.actions.FetchContractDataForAssignment(contractNumber).then(response => {
            if (response) {
                contractType = response.ContractInfo.contractType;
                this.updatedData["contractType"] = contractType;
                this.props.actions.UpdateICDiscounts(this.updatedData);
            }
        });
       
    };
    render() {
        const ocDiscount = this.props.InterCompanyDiscounts ? (this.props.InterCompanyDiscounts.assignmentOperatingCompanyDiscount
            ? this.props.InterCompanyDiscounts.assignmentOperatingCompanyDiscount : 100) : 100;
        return (
            <Fragment>
                {this.state.isInterCompanyDiscountShowModal &&
                    <Modal
                        modalClass="visitModal"
                        title={this.state.isInterCompanyDiscountEdit ? localConstant.visit.ADD_Visit_REFERENCE :
                            localConstant.visit.EDIT_Visit_REFERENCE}
                        buttons={this.state.isInterCompanyDiscountEdit ? this.InterCompanyDiscountEditButtons : this.InterCompanyDiscountAddButtons}
                        isShowModal={this.state.isInterCompanyDiscountShowModal}>
                        <InterCompanyDiscountModal />
                    </Modal>}

                <div className="customCard row">
                    <h6 className="label-bold">{localConstant.visit.COMPANY_RECEIVING_INTER_COMPANY_DISCOUNTS}</h6>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.PARENT_CONTRACT_HOLDER}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.parentContractHoldingCompanyName}
                            name='parentContractHolderName'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={true}
                            label={localConstant.visit.PERCENTAGE}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.parentContractHoldingCompanyDiscount > 0 ?
                                this.props.InterCompanyDiscounts.parentContractHoldingCompanyDiscount.toFixed(2) : ''}
                            name='parentContractHolderPercentage'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={true}
                            label={localConstant.visit.DESCRIPTION}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.parentContractHoldingCompanyDescription}
                            name='parentContractHoldelDesc'
                            readOnly={true}
                        />
                    </div>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.CONTRACT_HOLDER_COMPANY}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'                            
                            value={this.props.InterCompanyDiscounts.assignmentContractHoldingCompanyName}
                            name='parentContractHolderName'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.PERCENTAGE_MAXLENGTH}
                            max={99999}
                            onValueChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}                            
                            value={ this.stringValidation(this.props.InterCompanyDiscounts.assignmentContractHoldingCompanyDiscount) }                                                        
                            onValueBlur={this.checkNumber}
                            required={true}
                            name='assignmentContractHoldingCompanyDiscount'
                            readOnly={this.props.interactionMode || !this.props.isCoordinatorCompany || !this.props.isInterCompanyAssignment}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            dataValType="valueText"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            onValueChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                            value={this.props.InterCompanyDiscounts.assignmentContractHoldingCompanyDescription}
                            name='assignmentContractHoldingCompanyDescription'
                            readOnly={this.props.interactionMode || !this.props.isCoordinatorCompany || !this.props.isInterCompanyAssignment}
                            required={true}
                            maxLength={fieldLengthConstants.assignment.interCompanyDiscounts.DESCRIPTION_MAXLENGTH}
                        />
                    </div>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.OPERATING_COUNTRY}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts ?
                                (this.props.InterCompanyDiscounts.assignmentOperatingCompanyName ?
                                    this.props.InterCompanyDiscounts.assignmentOperatingCompanyName : this.props.visitInfo.visitOperatingCompany) :
                                this.props.visitInfo.visitOperatingCompany}
                            name='operator'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={parseFloat(ocDiscount).toFixed(2)}
                            name='operator_Per'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            name='operator_Desc'
                            readOnly={true}
                        />
                    </div>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.HOST_COMPANY}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentHostcompanyName}
                            name='operatingCountry'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentHostcompanyDiscount > 0 ?
                                this.props.InterCompanyDiscounts.assignmentHostcompanyDiscount.toFixed(2) : ''}
                            name='operatingCountry_Per'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentHostcompanyDescription}
                            name='operatingCountry_Desc'
                            readOnly={true}
                        />
                    </div>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.ADDITIONALINTER_CO_OFFICE}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany1_Name}
                            name='additionalInterCoOffice_0'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany1_Discount > 0 ?
                                this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany1_Discount.toFixed(2) : ''}
                            name='additionalInterCoOffice_0_Per'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany1_Description}
                            name='additionalInterCoOffice_0_Desc'
                            readOnly={true}
                        />
                    </div>
                    <div className="col s12 p-0" >
                        <CustomInput
                            hasLabel={true}
                            type="text"
                            divClassName='col pr-0'
                            label={localConstant.visit.ADDITIONALINTER_CO_OFFICE}
                            colSize='s6'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany2_Name}
                            name='additionalInterCoOffice_1'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany2_Discount > 0 ?
                                this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany2_Discount.toFixed(2) : ''}
                            name='additionalInterCoOffice_1_per'
                            readOnly={true}
                        />
                        <CustomInput
                            hasLabel={false}
                            type="text"
                            divClassName='col pr-0'
                            colSize='s3 mt-4x'
                            inputClass="customInputs"
                            dataType='decimal'
                            valueType='value'
                            value={this.props.InterCompanyDiscounts.assignmentAdditionalIntercompany2_Description}
                            name='additionalInterCoOffice_1_Desc'
                            readOnly={true}
                        />
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default InterCompanyDiscount;
