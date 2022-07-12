import React, { Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import {
 getlocalizeData,
 isEmpty,
 numberFormat,
formInputChangeHandler, 
bindAction, 
isEmptyReturnDefault,
decimalCheck } from '../../../../utils/commonUtils';
import arrayUtil  from '../../../../utils/arrayUtil';
import PCH from '../interCompanyDiscounts/PCH';
import CHC from '../interCompanyDiscounts/CHC';
import OC from '../interCompanyDiscounts/OC';
import HC from '../interCompanyDiscounts/HC';
import AICO from '../interCompanyDiscounts/AICO';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import { required } from '../../../../utils/validator';
import { isNullOrUndefined } from 'util';
const localConstant = getlocalizeData();

const sumCalculation = (arrData) => {
    const sum = arrData.map(item => parseFloat(item.value)).reduce((prev, next) => prev + next, 0);
    return isNaN(parseFloat(sum))? 0: parseFloat(sum).toFixed(2);
};

const Revenue = (props) => {
    return (
        <Fragment>
            <span className="col ml-0 pl-0 bold">{localConstant.assignments.REVENUE}</span>         
                <div className="customCard"> <ReactGrid gridColData={props.revenueHeader}
                    gridRowData={props.revenueData}
                    onRef={props.onRevenueItemDelete}
                    paginationPrefixId={localConstant.paginationPrefixIds.icRevenue}
                />
                </div>
                <div className="right-align mt-2">
                    <a onClick={props.revenueCreateHandler} 
                        className="btn-small mt-1 waves-effect" 
                        disabled={props.interactionMode || props.disableButton}>
                        {localConstant.commonConstants.ADD}</a>
                    <a href="#confirmation_Modal"
                        onClick={props.onRevenueDeleteClick}
                        className="btn-small mt-1 ml-2 mr-2 modal-trigger waves-effect btn-primary dangerBtn"
                        disabled={props.interactionMode || props.disableButton} >
                        {localConstant.commonConstants.DELETE}</a>
                    <LabelWithValue
                        className="custom-Badge col br1"
                        label={localConstant.assignments.REVENUE_SUBTOTAL}
                        value={sumCalculation(props.revenueData)}
                    />
                </div>           
        </Fragment>
    );
};

const Costs = (props) => {
    return (
        <Fragment>
            <span className="col ml-0 pl-0 bold">{localConstant.assignments.COSTS}</span>            
                <div className="customCard"><ReactGrid gridColData={props.costHeader}
                    gridRowData={props.costData}
                    onRef={props.onCostItemDelete}
                    paginationPrefixId={localConstant.paginationPrefixIds.icCosts}
                />
                </div>
                <div className="right-align mt-2">
                    <a onClick={props.costCreateHandler} 
                        className="btn-small mt-1 waves-effect"
                        disabled = {props.disableButton}>
                        {localConstant.commonConstants.ADD}</a>
                    <a href="#confirmation_Modal"
                        onClick={props.onCostDeleteClick}
                        className="btn-small ml-2 mt-1 mr-2 modal-trigger waves-effect btn-primary dangerBtn"
                        disabled = {props.disableButton}>
                        {localConstant.commonConstants.DELETE}</a>
                    <LabelWithValue
                        className="custom-Badge col br1"
                        label={localConstant.assignments.COSTS_SUBTOTAL}
                        value={sumCalculation(props.costData)}
                    />
                </div>           
        </Fragment>
    );
};
 
const Contribution = (props) => {
    const {
        contractHolderPercentage = '0.00',
        operatingCompanyPercentage = '0.00',
        countryCompanyPercentage = '0.00',
        contractHolderValue = '0.0',
        operatingCompanyValue = '0.0',
        countryCompanyValue = '0.0',
        totalContributionValue ='0.0',
        markupPercentage='0.00',
        totalContributionModdyPercent='0.00'
    } = props.contributionCalculator;
    const {
        operatingCompany='',
        hostCompany='',
        contractHoldingCompany='',
      
    } = props;
    // moody,
    // totalContributionPercent
    const totalPercentage = contractHolderPercentage + operatingCompanyPercentage + countryCompanyPercentage;
    return (
        <Fragment >
            <div className="row">
                <div className = "col s9"><span className="bold">{localConstant.assignments.CONTRIBUTION}</span></div>
                <div className = "col s2"><span className="bold right">{localConstant.assignments.PERCENTAGE}</span></div>
                <div className = "col s1"><span className="bold right">{localConstant.assignments.USD}</span></div>
                
                <div className = "col s9"><label className="label-lineHeight pl-3">{localConstant.assignments.APPROX_TOTAL_CONTRIBUTION_MOODY}</label></div>
                <div className = "col s2"><label className="labelBoldWithFont-1x mandateEmpty right">{Number(totalContributionModdyPercent).toFixed(2)}</label></div>
                <div className = "col s1"><label className="labelBoldWithFont-1x mandateEmpty right">{Number(totalContributionValue).toFixed(2)}</label></div>

                <div className = "col s9"><label className="label-lineHeight pl-3">{localConstant.assignments.ALLOCATED_CONTRIBUTION_MI}<span className="bold"> {contractHoldingCompany}</span></label></div>
                <div className = "col s2"><label className="labelBoldWithFont-1x mandate right">{Number(contractHolderPercentage).toFixed(2)}</label></div>
                <div className = "col s1"><label className="labelBoldWithFont-1x mandate right">{Number(contractHolderValue).toFixed(2)}</label></div>

                <div className = "col s9"><label className="label-lineHeight pl-3">{localConstant.assignments.ALLOCATED_CONTRIBUTION_MI}<span className="bold"> {operatingCompany}</span></label></div>
                <div className = "col s2"><label className="labelBoldWithFont-1x mandate right">{Number(operatingCompanyPercentage).toFixed(2)}</label></div>
                <div className = "col s1"><label className="labelBoldWithFont-1x mandate right">{Number(operatingCompanyValue).toFixed(2)}</label></div>

                <div className = "col s9"><label className="label-lineHeight pl-3">{localConstant.assignments.ALLOCATED_CONTRIBUTION_MI}<span className="bold"> {hostCompany}</span></label></div>
                <div className = "col s2"><label className="labelBoldWithFont-1x mandate right">{Number(countryCompanyPercentage).toFixed(2)}</label></div>
                <div className = "col s1"><label className="labelBoldWithFont-1x mandate right">{Number(countryCompanyValue).toFixed(2)}</label></div>

                <div className = "col s9"><label className="label-lineHeight pl-3">{localConstant.assignments.TOTAL_PERCENTAGE_CONTRIBUTION}</label></div>
                {/* <div className = "col s2"><label className="labelBoldWithFont-1x mandateEmpty right">{Number(markupPercentage).toFixed(2)}</label></div> */}
                {/* totalPercentage */}
                <div className = "col s2"><label className="labelBoldWithFont-1x mandateEmpty right">{Number(totalPercentage).toFixed(2)}</label></div>
                <div className = "col s1"><label className="labelBoldWithFont-1x mandateEmpty right">{Number(totalContributionValue).toFixed(2)}</label></div>
            </div>
        </Fragment>
    );
};

const RevenueModalPopup = (props) => (
    <div className="row">
        <CustomInput
            hasLabel={true}
            divClassName='col'
            label={localConstant.assignments.DESCRIPTION}
            type='text'
            colSize='s6'
            inputClass="customInputs"
            labelClass="mandate"
            name="description"
            onValueChange={props.onChangeHandler}
            defaultValue={props.editedRevenue.description}
            readOnly={props.editedRevenue.description && (props.editedRevenue.description.toLowerCase() === ("Bill Rate").toLowerCase()) ? true : false}
            // disabled={props.editedRevenue.description && (props.editedRevenue.description.toLowerCase() === ("Bill Rate").toLowerCase()) ? true : false}
        />
        <CustomInput
            hasLabel={true}
            divClassName='col'
            label={localConstant.assignments.VALUE_USD}
            type='number'
            colSize='s6'
            inputClass="customInputs"
            labelClass="customLabel mandate"
            required={true}
            name="value"
            defaultValue={parseFloat(props.editedRevenue.value)}
            onValueChange={props.onChangeHandler}
            onValueBlur={props.checkNumber}
        />
    </div>
);

const CostModalPopup = (props) => (
    <div className="row">
        <CustomInput
            hasLabel={true}
            divClassName='col'
            label={localConstant.assignments.DESCRIPTION}
            type='text'
            colSize='s6'
            inputClass="customInputs"
            labelClass="mandate"
            name="description"
            readOnly={props.interactionMode}
            onValueChange={props.onChangeHandler}
            defaultValue={props.editedCost.description}
        />
        <CustomInput
            hasLabel={true}
            divClassName='col'
            label={localConstant.assignments.VALUE}
            type='number'
            colSize='s6'
            inputClass="customInputs"
            labelClass="customLabel mandate"
            required={true}
            name="value"
            defaultValue={parseFloat(props.editedCost.value)}
            onValueChange={props.onChangeHandler}
            onValueBlur={props.checkNumber}
        />
    </div>
);

class InterCompanyDiscounts extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.editedRowData = {};
        this.state = {
            isOpen: false,
            isInterCompanyDiscountsOpen: false,
            isInterCompanyDiscountsEdit: false,
            isContributionCalculatorModalOpen: false,
            isRevenueModalOpen: false,
            isRevenueEditMode: false,
            isCostModalOpen: false,
            isCostEditMode: false,
           };
        this.ICDScenarios = {
            "PCH": { disabled: true },
            "CHC": { disabled: true },
            "OC": { disabled: true },
            "HC": { disabled: true },
            "AICO": { disabled: true },
            "AICO2": { disabled: true },
        };
        this.projectType = 'TIS (Technical Inspection Services)';
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.isContributionCalculatorDisable;
        this.headerData = HeaderData(functionRefs);
    };
    
  componentDidMount() {
         if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_NEW) {
         this.updateDefaultValue(); 
         }
       }; 

    updateDefaultValue=() =>{
        const contractNumber = this.props.assignmentInfo.assignmentContractNumber;
        let contractType='';
        const data = {
            contractNumber: contractNumber
        };
        this.props.actions.FetchContractDataForAssignment(contractNumber).then(response => {
            if (response) {
                contractType = response.ContractInfo.contractType;
              if (contractType && (contractType == 'STD' || contractType == 'FRW' || contractType == 'IRF')) {
                   if (this.props.assignmentInfo.assignmentContractHoldingCompany !==
                       this.props.assignmentInfo.assignmentOperatingCompany) {
                       this.updatedData["assignmentContractHoldingCompanyDiscount"] = parseFloat(15).toFixed(2);
                       this.updatedData["assignmentContractHoldingCompanyDescription"] = "InterCo Discount";
                       this.updatedData["assignmentOperatingCompanyDiscount"] = parseFloat(85).toFixed(2);
                   }
               }
               this.updatedData["contractType"] = contractType;
             this.props.actions.UpdateICDiscounts(this.updatedData);
            }
        });
       
    };

    isInterCompanyAssignment() {
        if (this.props.assignmentInfo.assignmentContractHoldingCompany !==
            this.props.assignmentInfo.assignmentOperatingCompany ) {
            return true;
        }
        return false;
    };
    isInterHostCompany(){
        if( this.props.assignmentInfo.assignmentOperatingCompany !== this.props.assignmentInfo.assignmentHostCompany){
            return true;
        }
        return false;
    }

    isInterCompanyOperator = ()=>{
        /**
         *   Changes: removed Operating company coordinator check. 
         *   Old Code if condition: this.isInterCompanyAssignment() && this.props.isOperator && this.props.isOperatorCompany
         */
        if (this.isInterCompanyAssignment() && this.props.isOperatorCompany) {
            return true;
        }
        return false;
    }

    isParentContract() {
        if (!isEmpty(this.props.assignmentInfo.assignmentParentContractCompany)) {
            return true;
        } else {
            return false;
        }
    };

    isDisableParentContract() {
        //if it is intercompany assignment then check user is OC Coordinator then he is not allowed to edit ICD
        // if(this.isInterCompanyOperator()){
        if(this.props.isInterCompanyAssignment && !this.props.isContractHolderCompany){ // ITK D - 712 & 715
            return true;
        }
        if (this.isParentContract()) {
            return false;
        }
        return true;
    };

    isContractHoldingCompany() {
         // ITK D - 712 & 715
        if(this.props.isInterCompanyAssignment && this.props.isContractHolderCompany)
            return false;
        else
            return true;
        // if(this.isInterCompanyOperator() || !this.isInterCompanyAssignment()){
        //     return true;
        // } else {
        //     return false;
        // }
    };

    isHostCompany() {
        const { projectReportParams, assignmentInfo } = this.props;
        const { assignmentProjectBusinessUnit } = assignmentInfo;
        //if it is intercompany assignment then check user is OC Coordinator then he is not allowed to edit ICD
        // if(this.isInterCompanyOperator()){
        if(this.props.isInterCompanyAssignment && !this.props.isContractHolderCompany){ // ITK D - 712 & 715
            return true;
        }
        /**
         * Changes: Removed interCompany settling type validtion and added workflow validation.
         */
        // const obj = arrayUtil.FilterGetObject(projectReportParams, "name", assignmentProjectBusinessUnit);
        // if(this.props.isTimesheetAssignment 
        // ITK D - 712 & 715
        if(this.props.isSettlingTypeMargin
            && (!required(assignmentInfo.assignmentHostCompanyCode)) 
            && this.isInterHostCompany()){
                if(this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
                    return true;
                }
            return false;
        }
        return true;
    };

    isAdditionalCompany=()=>{
        const { projectReportParams, assignmentInfo } = this.props;
        const { assignmentProjectBusinessUnit } = assignmentInfo;
        /**
         * Changes: Removed interCompany settling type validtion and added workflow validation.
         */
        // const obj = arrayUtil.FilterGetObject(projectReportParams, "name", assignmentProjectBusinessUnit);
        // if (this.props.isVisitAssignment){     //For ITK D-799 ,Removed WorkFlow Type Validation and added intercompany settling Type.
        if(this.props.isSettlingTypeCost){
            return true;
        }
        return false;
    }

    isDisableAdditionalCompany(currentPage,companyCode) {
        //if it is intercompany assignment then check user is OC Coordinator then he is not allowed to edit ICD
        // if(this.isInterCompanyOperator()){
        if(this.props.isInterCompanyAssignment && !this.props.isContractHolderCompany){ // ITK D - 712 & 715
            return true;
        }
        if (this.isAdditionalCompany()) {
            if(currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
                return true;
            }
            return false;
        }
        return true;
    }

    isDisableAdditionalCompanyOne(){
        const { currentPage,assignmentInfo,interCompanyDiscounts } = this.props;
        return this.isDisableAdditionalCompany(currentPage,interCompanyDiscounts.assignmentAdditionalIntercompany1_Code);
    }

    isDisableAdditionalCompanyTwo(){
        const { currentPage,assignmentInfo,interCompanyDiscounts } = this.props;
        return this.isDisableAdditionalCompany(currentPage,interCompanyDiscounts.assignmentAdditionalIntercompany2_Code);
    }
    
    validateField() {
        this.ICDScenarios.PCH.disabled = this.isDisableParentContract();
        this.ICDScenarios.CHC.disabled = this.isContractHoldingCompany();
        this.ICDScenarios.OC.disabled = true;
        this.ICDScenarios.HC.disabled = this.isHostCompany();
        this.ICDScenarios.HC.showValues = this.isInterHostCompany();
        this.ICDScenarios.AICO.disabled = this.isDisableAdditionalCompanyOne();
        this.ICDScenarios.AICO2.disabled = this.isDisableAdditionalCompanyTwo();
        this.ICDScenarios.AICO.showValues = this.isAdditionalCompany();
    };
    
    interCompanyDiscountsChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        if (e.target.tagName === "SELECT") {
            const selectedOption = e.nativeEvent.target[e.nativeEvent.target.selectedIndex];
            this.updatedData[e.target.name] = selectedOption.text !== localConstant.commonConstants.SELECT ? selectedOption.text : "";
            this.updatedData[e.target.id] = selectedOption.value;
        } else {
            this.updatedData[e.target.name] = decimalCheck(result.value,2);
        }
        if(e.target.name==="assignmentAdditionalIntercompany1_Name")
        {
            if(e.target.value==="")
            {
            this.updatedData["assignmentAdditionalIntercompany1_Discount"]='';
            this.updatedData["assignmentAdditionalIntercompany1_Description"]='';
     
            }         
        }
        else if(e.target.name==="assignmentAdditionalIntercompany2_Name")
        {
            if(e.target.value==="")
            {
          
            this.updatedData["assignmentAdditionalIntercompany2_Description"]='';
            this.updatedData["assignmentAdditionalIntercompany2_Discount"]='';
            }
        }
     // this.updatedData[result.name] = result.value;
        let { pchValue, chcValue, hcValue, aicoValue1, aicoValue2, pCHC, cHC, hC, aICO1, aICO2, finalDiscount, totalDiscount, discountNumbers } = 0;
        pCHC = this.updatedData.parentContractHoldingCompanyDiscount;
        pchValue = isNaN(pCHC) ? this.props.interCompanyDiscounts.parentContractHoldingCompanyDiscount === undefined ?
            null : this.props.interCompanyDiscounts.parentContractHoldingCompanyDiscount : pCHC;

        cHC = this.updatedData.assignmentContractHoldingCompanyDiscount;
        chcValue = isNaN(cHC) ? this.props.interCompanyDiscounts.assignmentContractHoldingCompanyDiscount === undefined ?
            null : this.props.interCompanyDiscounts.assignmentContractHoldingCompanyDiscount : cHC;

            if(this.props.interCompanyDiscounts.assignmentContractHoldingCompanyDiscount === chcValue){
                this.updatedData["changeCHCDiscount"] = false;
            }
            else{
                this.updatedData["changeCHCDiscount"] = true;
            }
            if( e.target.name == "assignmentContractHoldingCompanyDescription"){
                this.updatedData["changeCHCDiscount"] = true;
            }
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
        if (this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE
            && this.props.interCompanyDiscounts && this.props.interCompanyDiscounts.recordStatus !== 'N') {
            this.updatedData['recordStatus'] = 'M';
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
        }
        else {
            this.updatedData['recordStatus'] = 'N';
            this.updatedData["createdBy"] = this.props.loggedInUser;
        }
        if (finalDiscount >= 0 && result.name !== null) {
            this.updatedData["assignmentId"] = this.props.assignmentInfo.assignmentId;
            this.updatedData["parentContractHoldingCompanyDiscount"] = pchValue;
            this.updatedData["assignmentContractHoldingCompanyDiscount"] = chcValue;
            this.updatedData["assignmentAdditionalIntercompany1_Discount"] = aicoValue1;
            this.updatedData["assignmentAdditionalIntercompany2_Discount"] = aicoValue2;
            this.updatedData["assignmentHostcompanyDiscount"] = hcValue;
            this.updatedData["assignmentOperatingCompanyDiscount"] = finalDiscount.toFixed(2);
            this.updatedData["assignmentHostcompanyName"]=this.props.assignmentInfo.assignmentHostCompany;
            this.updatedData["assignmentHostcompanyCode"]=this.props.assignmentInfo.assignmentHostCompanyCode;
            this.updatedData["assignmentOperatingCompanyCode"]=this.props.assignmentInfo.assignmentOperatingCompanyCode;
            this.updatedData["assignmentOperatingCompanyName"]=this.props.assignmentInfo.assignmentOperatingCompany;
            this.updatedData["assignmentContractHoldingCompanyName"]=this.props.assignmentInfo.assignmentContractHoldingCompany;
            this.updatedData["assignmentContractHoldingCompanyCode"]=this.props.assignmentInfo.assignmentContractHoldingCompanyCode;
            this.updatedData["parentContractHoldingCompanyName"]=this.props.assignmentInfo.assignmentParentContractCompany;
            this.updatedData["parentContractHoldingCompanyCode"]=this.props.assignmentInfo.assignmentParentContractCompanyCode;
            this.updatedData["parentContractHoldingCompanyDiscount"]=this.props.assignmentInfo.assignmentParentContractDiscount;
            this.props.actions.UpdateICDiscounts(this.updatedData);
            this.updatedData={};
            
        } else {
            this.updatedData[result.name] = this.props.interCompanyDiscounts[result.name];
            IntertekToaster(localConstant.assignments.VALUE_EXCEEDING, 'warningToast CreatRevDescData');
            e.target.value = "";
        }
        
    }

    /** Handler to make the entered decimal data as two digit */

    checkNumber = (e) => {
        if(!isEmpty(e.target.value) ){
        e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        this.interCompanyDiscountsChangeHandler(e);
        }
    }
    checkNumberForCalculator = (e) => {
        if(!isEmpty(e.target.value) ){
        e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
            this.onChangeHandler(e);
        }
    }
    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    onChangeHandler = (e) => {
        let result = this.inputChangeHandler(e);
        if(e.target.name ==='value'){
            result= parseFloat(numberFormat(e.target.value)).toFixed(2);
        }
        this.updatedData[e.target.name] = result;
    }

    /**
     * Contibution calculator popup open 
     */
    contributionCalculatorPopup = (e, revenueData) => {
        const { assignmentInfo, interCompanyDiscounts, ContributionCalculator } = this.props;
        const contributionCalculatorObj = isEmptyReturnDefault(ContributionCalculator[0], 'object');
        this.setState({
            isContributionCalculatorModalOpen: true,
        });
        const contributionData = {
            assignmentId: assignmentInfo.assignmentId,
            contractHolderPercentage: null,
            countryCompanyPercentage: null
        };

        if (this.isInterCompanyAssignment()) {
            contributionData.contractHolderPercentage = interCompanyDiscounts.assignmentContractHoldingCompanyDiscount;
        }

        if (this.isInterHostCompany()) {
            contributionData.countryCompanyPercentage = interCompanyDiscounts.assignmentHostcompanyDiscount;
        }
        //add default contribution data contribution data
        if (revenueData.length <= 0) {
            contributionData.assignmentContCalculationUniqueId = Math.floor(Math.random() * (Math.pow(10, 5)));
            contributionData.assignmentContributionRevenueCostUniqueId = Math.floor(Math.random() * (Math.pow(10, 5)));
            contributionData.operatingCompanyPercentage = interCompanyDiscounts.assignmentOperatingCompanyDiscount;
            this.props.actions.AddDefaultContributionData(contributionData);
            this.props.actions.savedContributionCalculatorChanges();
        } else if (this.isInterCompanyAssignment() || this.isInterHostCompany()) {

            if (interCompanyDiscounts.assignmentOperatingCompanyDiscount != contributionCalculatorObj.operatingCompanyPercentage
                || interCompanyDiscounts.assignmentContractHoldingCompanyDiscount != contributionCalculatorObj.contractHolderPercentage
                || interCompanyDiscounts.assignmentHostcompanyDiscount != contributionCalculatorObj.countryCompanyPercentage) {

                contributionData.contractHolderPercentage = interCompanyDiscounts.assignmentContractHoldingCompanyDiscount;
                contributionData.countryCompanyPercentage = interCompanyDiscounts.assignmentHostcompanyDiscount;
                contributionData.operatingCompanyPercentage = interCompanyDiscounts.assignmentOperatingCompanyDiscount;

                const updateContibutionPercentObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.ASSIGNMENT_CC_APPLY_UPDATE_PERCENT,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: (e) => this.updateContributionPercent(e, contributionData),
                            className: "modal-close m-1 btn-small"
                        },
                        {
                            buttonName: "No",
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                this.props.actions.DisplayModal(updateContibutionPercentObject);
            }
            else {
                this.props.actions.savedContributionCalculatorChanges();
            }
        }else {
            this.props.actions.savedContributionCalculatorChanges();
        }
    }

    updateContributionPercent(e,contributionData) {
        this.props.actions.UpdateContributionCalculator(contributionData);
        this.props.actions.savedContributionCalculatorChanges();
        this.props.actions.HideModal();
    }

    /**
     * Revenue Popup will be open in add mode
     */
    revenueCreateHandler = () => {
        this.setState({
            isRevenueModalOpen: true,
            isRevenueEditMode: false
        });
        this.editedRowData = {};
    }

    editRevenueRowHandler = (data) => {
        this.setState((state) => {
            return {
                isRevenueModalOpen: !state.isRevenueModalOpen,
                isRevenueEditMode: true
            };
        });
        this.editedRowData = data;
    }

    /**
    * Costs Popup will be open in add mode
    */
    costCreateHandler = () => {
        this.setState({
            isCostModalOpen: true,
            isCostEditMode: false
        });
        this.editedRowData = {};
    }

    editCostRowHandler = (data) => {
        this.setState((state) => {
            return {
                isCostModalOpen: !state.isCostModalOpen,
                isCostEditMode: true
            };
        });
        this.editedRowData = data;
    }

    /**
     * Create Revenue
     */
    createRevenueData = (e) => {
        e.preventDefault();
        if (isEmpty(this.updatedData.description)) {
            IntertekToaster(localConstant.warningMessages.ENTER_DESCRIPTION_DATA, 'warningToast CreatRevDescData');
        }
        else if (isEmpty(this.updatedData.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatRevValcData');
        }
        else if (isNaN(this.updatedData.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatRevValcData');
        }
        else {
            const ContributionCalculator  = isEmptyReturnDefault(this.props.ContributionCalculator[0],'object');
            this.updatedData["recordStatus"] = "N";
            this.updatedData["createdBy"] = this.props.loggedInUser;
            this.updatedData["type"] = "A";
            this.updatedData["assignmentId"] = this.props.assignmentInfo.assignmentId;
            this.updatedData["assignmentContributionRevenueCostId"] = null;
            this.updatedData["assignmentContCalculationId"] = ContributionCalculator.assignmentContCalculationUniqueId;
            this.updatedData["assignmentContributionRevenueCostUniqueId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
            this.props.actions.AddRevenueData(this.updatedData);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isRevenueModalOpen: false
            });
        }
    }

    /**
     * Edit Revenue
     */
    editRevenueData = (e) => {
        e.preventDefault();
        const editedItem = Object.assign({}, this.editedRowData,this.updatedData);
        if (isEmpty(editedItem.description)) {
            IntertekToaster(localConstant.warningMessages.ENTER_DESCRIPTION_DATA, 'warningToast CreatRevDescData');
        }
        else if (isNaN(editedItem.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatRevValData');
        }
        else {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.updatedData["type"] = "A";
            this.updatedData["assignmentId"] = this.props.assignmentInfo.assignmentId;
            this.props.actions.UpdateRevenueData(editedItem);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isRevenueModalOpen: false
            });
        }
    }

    /**
     * Confirmation on click of Delete Button.
     */
    onRevenueDeleteClick = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            for (let i = 0; i < selectedRecords.length; i++) {
                if (selectedRecords[i].description.toLowerCase() === ("Bill Rate").toLowerCase()) {
                    return IntertekToaster(localConstant.warningMessages.DELETE_BILL_RATE, 'warningToast idProjOneRowSelectReq');
                }
            }
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.REVENUE_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelectedRevenue,
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
            IntertekToaster(localConstant.warningMessages.SELECT_ANY_ONE_ROW_TO_DELETE, 'warningToast idRevenueOneRowSelectReq');
        }
    }
    /**
     * Delete the Selected Revenue.
     */
    deleteSelectedRevenue = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteRevenueData(selectedData);
        this.props.actions.HideModal();
    }

    /**
     * Create Cost
     */
    createCostData = (e) => {
        e.preventDefault();
        if (isEmpty(this.updatedData.description)) {
            IntertekToaster(localConstant.warningMessages.ENTER_DESCRIPTION_DATA, 'warningToast CreatCostDescData');
        }
        else if (isEmpty(this.updatedData.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatCostValcData');
        }
        else if (isNaN(this.updatedData.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatCostValData');
        }
        else {
            const ContributionCalculator  = isEmptyReturnDefault(this.props.ContributionCalculator[0],'object');
            this.updatedData["recordStatus"] = "N";
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.updatedData["id"] = null;
            this.updatedData["type"] = "B";
            this.updatedData["assignmentId"] = this.props.assignmentInfo.assignmentId;
            this.updatedData["assignmentContributionRevenueCostId"] = null;
            this.updatedData["assignmentContCalculationId"] = ContributionCalculator.assignmentContCalculationUniqueId;
            this.updatedData["assignmentContributionRevenueCostUniqueId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
            this.props.actions.AddCostData(this.updatedData);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isCostModalOpen: false
            });
        }
    }

    /**
     * Edit Cost
     */
    editCostData = (e) => {
        e.preventDefault();
        const editedItem = Object.assign({}, this.editedRowData,this.updatedData);
        if (isEmpty(editedItem.description)) {
            IntertekToaster(localConstant.warningMessages.ENTER_DESCRIPTION_DATA, 'warningToast CreatCostDescData');
        }
        else if (isNaN(editedItem.value)) {
            IntertekToaster(localConstant.warningMessages.ENTER_VALUE, 'warningToast CreatCostValData');
        }
        else {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            this.updatedData["type"] = "B";
            this.updatedData["assignmentId"] = this.props.assignmentInfo.assignmentId;
            this.props.actions.UpdateCostData(editedItem);
            this.updatedData = {};
            this.editedRowData = {};
            this.setState({
                isCostModalOpen: false
            });
        }
    }

    /**
     * Confirmation Popup Delete Cost
     */
    onCostDeleteClick = () => {
        const selectedRecords = this.secondChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COST_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelectedCost,
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
            IntertekToaster(localConstant.warningMessages.SELECT_ANY_ONE_ROW_TO_DELETE, 'warningToast idCostOneRowSelectReq');
        }
    }

    /**
     * Delete Cost
     */
    deleteSelectedCost = () => {
        const selectedData = this.secondChild.getSelectedRows();
        this.secondChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteCostData(selectedData);
        this.props.actions.HideModal();
    }

    /**
     * To Close the opened popup
     */
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    /**
   * Closing the popup of Revenue
   */
    cancelRevenuePopup = (e) => {
        e.preventDefault();
        this.setState({
            isRevenueModalOpen: false
        });
    }

    /**
  * Closing the popup of Costs
  */
    cancelCostPopup = (e) => {
        e.preventDefault();
        this.setState({
            isCostModalOpen: false
        });
    }

    /**
     * Cancel Contribution Calculator Popup
     */
    cancelContributionCalculator = (e) => {
        e.preventDefault();
        //Do this when calculator has any changes
        if (this.props.isContrinutionCalculatorModified) {
            const cancelContibutionObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ASSIGNMENT_CC_KEEP_CHANGES,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler:(e)=> this.CloseContibutionPopup(e,"yes"),
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler:(e)=> this.CloseContibutionPopup(e,"no"),
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "Cancel",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(cancelContibutionObject);
        }else{
            this.setState({
                isContributionCalculatorModalOpen: false
            });
        }
    }

    CloseContibutionPopup(e, actionType) {
        if (actionType === "no") {
            this.props.actions.resetContributionCalculator();
        }
        this.props.actions.HideModal();
        this.setState({
            isContributionCalculatorModalOpen: false
        });
    }

    /**
     * On Submit Click of Contribution Calculator Popup
     */
    updateContibutionCalculator = (e) => {
        e.preventDefault();
        const { ContributionCalculator } = this.props;
        const ContributionCalculatorObj  = isEmptyReturnDefault(ContributionCalculator[0],'object');
        const assignmentContributionRevenueCosts  = isEmptyReturnDefault(ContributionCalculatorObj.assignmentContributionRevenueCosts);
        const revenueData = assignmentContributionRevenueCosts.filter(eachItem =>eachItem.type ==='A');
        
        //Bill Rate Cannot be Zero or Empty. Please  enter the value for Bill Rate!!!
        if (!isEmpty(revenueData)) {
            for (let i = 0; i < revenueData.length; i++) {
                const revenueDesc = revenueData[i].description && revenueData[i].description.trim();
                const revenueValue = revenueData[i].value ? parseFloat(revenueData[i].value) : 0;
                if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase() && revenueValue !== 0) {
                  break;
                }
                else if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase() && revenueValue === 0) {
                    IntertekToaster(localConstant.validationMessage.BILL_RATE_ZERO_VALIDATION, 'warningToast UpdateContriCalc');
                    return false;
                }
            }
        }
         //Confirm:Do you wish  to copy the percentages to the Inter Company  Discounts on the Assignment?
         const updateContibutionObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.ASSIGNMENT_CC_APPLY_PERCENT,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler:(e)=> this.confirmUpdateContibution(e),
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler:(e)=> this.cancelUpdateContibution(e),
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        this.props.actions.DisplayModal(updateContibutionObject);
        
    }
    confirmUpdateContibution(){
        this.setState({
            isContributionCalculatorModalOpen: false
        });
        this.props.actions.HideModal();
    }

    cancelUpdateContibution(){
        //changes to be done for cancel updations
        this.props.actions.HideModal();
    }

    isContributionCalculatorDisable = () => {
        if (this.props.isTimesheetAssignment && this.props.isSettlingTypeMargin) { // ITK D - 712 & 715
            // Changes: Hot fix in Inter Company Discount
            // if (!this.isInterCompanyAssignment()) {
            //     return false;
            // }
            // else 
            if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
                return false;
            }
        }
        return true;
    }

    totalMoodyPercentValue(revenueData, costData) {
        const moodyUSD = sumCalculation(revenueData) - sumCalculation(costData);
        let lastBillrate = 1;
        if (!isEmpty(revenueData)) {
            for (let i = 0; i < revenueData.length; i++) {
                const revenueDesc = revenueData[i].description && revenueData[i].description.trim();
                if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase()) {
                    lastBillrate = revenueData[i].value;
                }
            }
        }

        let moodyPercent = (moodyUSD / lastBillrate) * 100;
        moodyPercent = isNaN(moodyPercent) ? '0.00' : moodyPercent;
        return {
            moodyUSD: Number(moodyUSD).toFixed(1),
            moodyPercent: Number(moodyPercent).toFixed(2)
        };
    };

    // Not used
    totalContributionPercent(ContributionCalculatorObj) {
        let contractHolderPercentage = parseFloat(ContributionCalculatorObj.contractHolderPercentage),
            operatingCompanyPercentage = parseFloat(ContributionCalculatorObj.operatingCompanyPercentage),
            countryCompanyPercentage = parseFloat(ContributionCalculatorObj.countryCompanyPercentage);

        contractHolderPercentage = isNaN(contractHolderPercentage) ? 0.0 : contractHolderPercentage;
        operatingCompanyPercentage = isNaN(operatingCompanyPercentage) ? 0.0 : operatingCompanyPercentage;
        countryCompanyPercentage = isNaN(countryCompanyPercentage) ? 0.0 : countryCompanyPercentage;

        const markupPercentage = contractHolderPercentage + operatingCompanyPercentage + countryCompanyPercentage;
        return Number(markupPercentage).toFixed(2);
    };

    zeroValidation(company, value) {
        const temp = company?!isNullOrUndefined(value)?(value===0)?parseFloat(value).toFixed(2):value:'':'';
        return temp;
    };

    disableAICODescAndPercent(companyCode) {
        if (this.props.interactionMode) {
            return true;
        } else if (!companyCode || this.ICDScenarios.AICO.disabled) {
            return true;
        } 
      return false;
    }
    // function disableAICODescAndPercent(e){return!!this.props.interactionMode||!(e&&!this.ICDScenarios.AICO.disabled)}
    render() {
        const { interactionMode, companyList, interCompanyDiscounts ,ContributionCalculator,assignmentInfo,isTimesheetAssignment,isSettlingTypeMargin } = this.props;
        const ContributionCalculatorObj  = isEmptyReturnDefault(ContributionCalculator[0],'object');
        const assignmentContributionRevenueCosts  = isEmptyReturnDefault(ContributionCalculatorObj.assignmentContributionRevenueCosts);
        const revenueData = assignmentContributionRevenueCosts.filter(eachItem =>eachItem.type ==='A');
        const costData = assignmentContributionRevenueCosts.filter(eachItem =>eachItem.type ==='B');
        const isInterCompany = this.isInterCompanyAssignment();
        const isDisableContributionCalculator = this.isContributionCalculatorDisable();   
        this.validateField();
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        this.defaultContibutionCalculatorBtns = [
            {
                name: localConstant.commonConstants.CANCEL,
                action:(e)=> this.cancelContributionCalculator(e),
                btnID: "cancelContributionCalculatorfield",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action:(e)=> this.updateContibutionCalculator(e),
                btnID: "updateContributionCalculator",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: !isDisableContributionCalculator
            }
        ];
        this.revenueModalBtns = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelRevenuePopup,
                btnID: "cancelRevenue",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isRevenueEditMode ?(e)=> this.editRevenueData(e) :(e)=> this.createRevenueData(e),
                //action: this.state.isRevenueEditMode ? this.editRevenueData : this.createRevenueData,
                btnID: "CreateRevenue",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.costsModalBtns = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCostPopup,
                btnID: "cancelCost",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isCostEditMode ? this.editCostData : this.createCostData,
                btnID: "CreateCost",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        bindAction(this.headerData.Revenue, "EditRevenue", this.editRevenueRowHandler);
        bindAction(this.headerData.Costs, "EditCosts", this.editCostRowHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className={this.state.isRevenueModalOpen ? 'customCard nestedmodal' : 'customCard'}>
                   {this.state.isContributionCalculatorModalOpen && <Modal title="Contribution Calculator" modalId="ContributionCalculatorModal" formId="ContributionCalculatorDefaultModal"
                        modalClass="contribution nestedModal" overlayClass="nestedOverlay" modalContentClass="modalMaxHeight" buttons={this.defaultContibutionCalculatorBtns} isShowModal={this.state.isContributionCalculatorModalOpen}
                        disabled={interactionMode}>
                        <div className="col s5">
                            <Revenue
                                revenueHeader = {this.headerData.Revenue}
                                revenueCreateHandler={this.revenueCreateHandler}
                                onRevenueDeleteClick={this.onRevenueDeleteClick}
                                onRevenueItemDelete={ref => { this.child = ref; }}
                                revenueData={revenueData}
                                disableButton = {isDisableContributionCalculator}
                            />
                            <Costs
                            costHeader = {this.headerData.Costs}
                            costCreateHandler={this.costCreateHandler}
                            onCostDeleteClick={this.onCostDeleteClick}
                            onCostItemDelete={ref => { this.secondChild = ref; }}
                            costData={costData}
                            disableButton = {isDisableContributionCalculator}
                            />
                        </div>
                        <div className="col s7 pl-0">
                            <Contribution
                                operatingCompany={assignmentInfo.assignmentOperatingCompany}
                                hostCompany={assignmentInfo.assignmentHostCompany}
                                contractHoldingCompany={assignmentInfo.assignmentContractHoldingCompany}
                                contributionCalculator={ContributionCalculatorObj}
                                // moody={this.totalMoodyPercentValue(revenueData,costData)}
                                // totalContributionPercent={this.totalContributionPercent(ContributionCalculatorObj)}
                            />
                        </div>
                    </Modal>
                   }
                    {this.state.isRevenueModalOpen &&
                        <Modal title="Revenue" modalId="RevenueModal" formId="RevenueFormId" modalClass="popup-position nestedModal"
                            overlayClass="nestedOverlay" buttons={this.revenueModalBtns} isShowModal={this.state.isRevenueModalOpen} disabled={interactionMode}>
                            <RevenueModalPopup
                                onChangeHandler={this.onChangeHandler}
                                editedRevenue={this.editedRowData}
                                checkNumber={this.checkNumberForCalculator}
                            />
                        </Modal>}
                    {this.state.isCostModalOpen &&
                        <Modal title="Costs" modalId="CostModal" formId="CostFormId" modalClass="popup-position nestedModal"
                            overlayClass="nestedOverlay" buttons={this.costsModalBtns} isShowModal={this.state.isCostModalOpen} disabled={interactionMode}>
                            <CostModalPopup
                                onChangeHandler={this.onChangeHandler}
                                editedCost={this.editedRowData}
                                checkNumber={this.checkNumberForCalculator}
                            />
                        </Modal>}
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.INTERCOMPANY_DISCOUNTS} colSize="s12">
                        <div className='col s12'>
                            <label className='col s4'></label>
                            <label className='col s2'>Percentage</label>
                            <label className='col s4'>Description</label>
                           {/* {this.props.pageMode!==localConstant.commonConstants.VIEW &&  */}
                           <a onClick={(e)=>this.contributionCalculatorPopup(e,revenueData)} 
                            className={`btn-small waves-effect col s2 ${ (isSettlingTypeMargin) ? '':'isDisabled' } `}> 
                                {localConstant.commonConstants.CONTRIBUTION_CALCULATOR}</a>
                            {/* } */}
                        </div>
                        <div className='row'>
                            <PCH
                                disabled={this.ICDScenarios.PCH.disabled}
                                discounts={this.props.assignmentInfo}
                                name={this.props.assignmentInfo.assignmentParentContractCompany}
                                value={this.zeroValidation(this.props.assignmentInfo.assignmentParentContractCompany, this.props.assignmentInfo.assignmentParentContractDiscount)}
                                checkNumber={this.checkNumber}
                                description={interCompanyDiscounts.parentContractHoldingCompanyDescription}
                                interCompanyDiscountsChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                                interactionMode={interactionMode}
                            />
                            <CHC
                                disabled={this.ICDScenarios.CHC.disabled}
                                description={interCompanyDiscounts.assignmentContractHoldingCompanyDescription}
                                name={this.props.assignmentInfo.assignmentContractHoldingCompany}
                                isInterCompany={isInterCompany}
                                checkNumber={this.checkNumber}
                                value={this.zeroValidation(this.props.assignmentInfo.assignmentContractHoldingCompany, interCompanyDiscounts.assignmentContractHoldingCompanyDiscount)}
                                interCompanyDiscountsChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                                interactionMode={interactionMode}
                            />

                            <OC
                                disabled={this.ICDScenarios.OC.disabled}
                                interactionMode={interactionMode}
                                discounts={this.props.assignmentInfo.assignmentOperatingCompany}
                                value={interCompanyDiscounts.assignmentOperatingCompanyDiscount}
                                checkNumber={this.checkNumber}
                            />
                            <HC
                                disabled={this.ICDScenarios.HC.disabled}
                                showValues={this.ICDScenarios.HC.showValues}
                                description={interCompanyDiscounts.assignmentHostcompanyDescription}
                                discounts={this.props.assignmentInfo.assignmentHostCompany}
                                interCompanyDiscountsChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                                value={this.zeroValidation(this.props.assignmentInfo.assignmentHostCompany, interCompanyDiscounts.assignmentHostcompanyDiscount)}
                                checkNumber={this.checkNumber}
                                interactionMode={interactionMode}
                            />
                            <AICO
                                company={companyList}
                                disabled={this.ICDScenarios.AICO.disabled}
                                companyCode={interCompanyDiscounts.assignmentAdditionalIntercompany1_Code}
                                descriptionValue={interCompanyDiscounts.assignmentAdditionalIntercompany1_Description}
                                description={'assignmentAdditionalIntercompany1_Description'}
                                interCompanyDiscountsChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                                discount={this.zeroValidation(interCompanyDiscounts.assignmentAdditionalIntercompany1_Code, interCompanyDiscounts.assignmentAdditionalIntercompany1_Discount)}
                                percentage={'assignmentAdditionalIntercompany1_Discount'}
                                checkNumber={this.checkNumber}
                                name={'assignmentAdditionalIntercompany1_Name'}
                                id={'assignmentAdditionalIntercompany1_Code'}
                                disableDescPercent={this.disableAICODescAndPercent(interCompanyDiscounts.assignmentAdditionalIntercompany1_Code)}
                                interactionMode={interactionMode}
                                showValues={this.ICDScenarios.AICO.showValues}
                            />
                            <AICO
                                company={companyList}
                                disabled={this.ICDScenarios.AICO2.disabled}
                                companyCode={interCompanyDiscounts.assignmentAdditionalIntercompany2_Code}
                                descriptionValue={interCompanyDiscounts.assignmentAdditionalIntercompany2_Description}
                                description={'assignmentAdditionalIntercompany2_Description'}
                                interCompanyDiscountsChange={(e)=>this.interCompanyDiscountsChangeHandler(e)}
                                discount={this.zeroValidation(interCompanyDiscounts.assignmentAdditionalIntercompany2_Code, interCompanyDiscounts.assignmentAdditionalIntercompany2_Discount)}
                                percentage={'assignmentAdditionalIntercompany2_Discount'}
                                checkNumber={this.checkNumber}
                                name={'assignmentAdditionalIntercompany2_Name'}
                                id={'assignmentAdditionalIntercompany2_Code'}
                                disableDescPercent={this.disableAICODescAndPercent(interCompanyDiscounts.assignmentAdditionalIntercompany2_Code)}
                                interactionMode={interactionMode}
                                showValues={this.ICDScenarios.AICO.showValues}
                            />
                        </div>
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default InterCompanyDiscounts;