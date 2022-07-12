import React, { Component, Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import {
    getlocalizeData,
    isEmpty,
    numberFormat,
    formInputChangeHandler,
    processMICoordinators,
    decimalCheck,
    isEmptyOrUndefine,
    thousandFormat
} from '../../../../utils/commonUtils';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import ContractAnchor from '../../../viewComponents/contracts/contractAnchor';
import CustomerAnchor from '../../../viewComponents/customer/customerAnchor';
import { AppCustomerRoutes } from '../../../../routes/routesConfig';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
// import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import moment from 'moment';
import CustomModal from '../../../../common/baseComponents/customModal';
import BudgetMonetary from '../../budgetMonetary';
import { GetProjectReportingParams,getCoordinatorData } from '../../../../selectors/assignmentSelector';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

/** General Details section input fields */
const Details = (props) => {
    const { generalDetails = {} } = props;
    
    let { isProjectForNewFacility ='' } =  generalDetails;

    // //Added for D-117
    // if(!isEmptyOrUndefine(generalDetails))
    // {
    //     (props.projectMode ===  localConstant.project.EDITPROJECT &&  (props.isNDT))  ?
    //     generalDetails.logoText=isEmptyOrUndefine(generalDetails.logoText) ? Array.isArray(props.projectLogo) 
    //                                                                                     && props.projectLogo.length>0
    //                                                                                     && props.projectMode === localConstant.project.EDITPROJECT 
    //                                                                                     ? props.projectLogo[0].name
    //                                                                                     : generalDetails.logoText
    //                                                                                     : generalDetails.logoText
    //                                                                                     :null;
    // }
    isProjectForNewFacility = isProjectForNewFacility.toString() === 'true'?'true':(isProjectForNewFacility=== ''|| isProjectForNewFacility === undefined) ?'':'false';
    return (
        <CardPanel className="white lighten-4 black-text mb-2 left shadow-none" title={localConstant.contract.GENERAL_DETAILS} colSize="s12">
            <div className="row">
                <div className="custom-Badge col s3 bold widthChange">{localConstant.project.CONTRACT_NUMBER + ': '}
                    <ContractAnchor
                        data={{ contractNumber: generalDetails.contractNumber, contractHoldingCompanyCode: generalDetails.contractHoldingCompanyCode }} />
                </div>
                <LabelWithValue
                    className="textNoWrapEllipsis custom-Badge col widthChange1 marginAlign"
                    colSize="s6"
                    label={`${ localConstant.customer.CUSTOMER_NAME }: `}
                    value={generalDetails.contractCustomerName}
                />
                <div className="custom-Badge col s3 bold">{localConstant.customer.CUSTOMER_CODE + ': '}
                          <CustomerAnchor
                        data={{ customerCode: generalDetails.contractCustomerCode }} />
                </div>

                <LabelWithValue
                    className="custom-Badge col"
                    colSize="s12"
                    label={` ${ localConstant.project.PROJECT_NUMBER }: `}
                    value={generalDetails.projectNumber}
                />
            </div>
            <div className="customCard ml-4 mr-4 pl-3">
                <div className="row mb-0">
                    <div className="custom-card col s6 br-1 pr-1">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.CUSTOMER_PROJECT_NUMBER}
                            type='text'
                            dataValType='valueText'
                            colSize='s12'
                            inputClass="customInputs"
                            labelClass="customLabel mandate"
                            name="customerProjectNumber"
                            maxLength={fieldLengthConstants.Project.generalDetails.CUSTOMER_PROJECT_NUMBER_MAXLENGTH}
                            readOnly={props.interactionMode}
                            // disabled={props.interactionMode}
                            value={generalDetails.customerProjectNumber ? generalDetails.customerProjectNumber : ""}
                            onValueChange={props.onChangeHandler}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.CUSTOMER_PROJECT_NAME}
                            type='text'
                            dataValType='valueText'
                            colSize='s12'
                            inputClass="customInputs"
                            labelClass="customLabel mandate"
                            name="customerProjectName"
                            maxLength={fieldLengthConstants.Project.generalDetails.CUSTOMER_PROJECT_NAME_MAXLENGTH}
                            readOnly={props.interactionMode}
                            // disabled={props.interactionMode}
                            value={generalDetails.customerProjectName ? generalDetails.customerProjectName : ""}
                            onValueChange={props.onChangeHandler}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.DIVISION}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.projectCompanyDivision || []}
                            labelClass="customLabel mandate"
                            optionName='divisionName'
                            optionValue="divisionName"
                            name="companyDivision"
                            disabled={props.interactionMode}
                            defaultValue={generalDetails.companyDivision}
                            onSelectChange={props.divisionOnChangeHandler}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.OFFICE}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.projectCompanyOffice || []}
                            labelClass="customLabel mandate"
                            optionName='officeName'
                            optionValue="officeName"
                            name="companyOffice"
                            disabled={props.interactionMode}
                            defaultValue={generalDetails.companyOffice}
                            onSelectChange={props.onChangeHandler}//Sorting order                             
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.COST_CENTRE}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.projectCompanyCostCenter || []}
                            labelClass="customLabel mandate"
                            optionName='costCenterName'
                            customDefaultValueCheck={true}
                            defaultValueCheckComparator={localConstant.commonConstants.UNDER_SCORE}
                            optionValue='costCenterCode'
                            name="companyCostCenterCode"
                            disabled={props.interactionMode}
                            defaultValue={generalDetails.companyCostCenterCode + localConstant.commonConstants.UNDER_SCORE +generalDetails.companyCostCenterName}
                            onSelectChange={props.onChangeHandler}
                        />
                    </div>
                    <div className=" custom-card col s6">
                      
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.BUSINESS_UNIT}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.businessUnit || []}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="name"
                            name="projectType"
                            disabled={props.projectMode ===  localConstant.project.EDITPROJECT?true:false}
                            defaultValue={generalDetails.projectType}
                            onSelectChange={props.onChangeHandler}
                        />
                        {
                            (generalDetails.projectType === "NDT (Non Destructive Testing)" || !isEmpty(generalDetails.logoText)) || (props.projectMode === localConstant.project.EDITPROJECT && (props.isNDT)) ?
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    label={localConstant.companyDetails.generalDetails.LOGO}
                                    type='select'
                                    colSize='s12'
                                    className="browser-default"
                                    optionsList={props.projectLogo || []}
                                    labelClass="customLabel mandate"
                                    optionName='name'
                                    optionValue="name"
                                    name="logoText"
                                    disabled={((props.projectMode === localConstant.project.EDITPROJECT && !props.isNDT) || props.interactionMode) ? true : false}
                                    defaultValue={generalDetails.logoText}
                                    onSelectChange={props.onChangeHandler}
                                /> : null
                        }
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.WORKFLOW_TYPE}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.workflowType || []}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="value"
                            name="workFlowType"
                            disabled={props.projectMode === localConstant.project.EDITPROJECT?true:false}
                            defaultValue={generalDetails.workFlowType}
                            onSelectChange={props.onChangeHandler}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.INDUSTRY_SECTOR}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.industrySector || []}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="name"
                            name="industrySector"
                            disabled={props.interactionMode}
                            defaultValue={generalDetails.industrySector}
                            onSelectChange={props.onChangeHandler}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.PROJECT_IS_FOR_NEW_FACILITY}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={localConstant.commonConstants.projectIsForNewFacility}
                            labelClass="customLabel mandate"
                            optionName="name"
                            optionValue="value"
                            name="isProjectForNewFacility"
                            disabled={props.interactionMode}
                            defaultValue={isProjectForNewFacility }
                            onSelectChange={props.onChangeHandler}
                        />
                         <CustomInput
                            type='switch'
                            switchLabel={localConstant.project.CLIENT_EXTRANET_SUMMARY_REPORT}
                            isSwitchLabel={true}
                            switchName="isExtranetSummaryVisibleToClient"
                            id="isExtranetSummaryVisibleToClient"
                            colSize="s12"
                            disabled={props.interactionMode}
                            checkedStatus={generalDetails.isExtranetSummaryVisibleToClient}
                            onChangeToggle={props.switchOnChangeHandler}
                            switchKey={generalDetails.isExtranetSummaryVisibleToClient}
                        />
                    </div>
                </div>
            </div>
        </CardPanel>
    );
};

const Coordinator = (props) => {
    const { coordinatorDetails, coordinatorEmail } = props;
    let { isManagedServices = '' } = props.coordinatorDetails;
    isManagedServices = isManagedServices && isManagedServices.toString() === 'true' ? 'true' : (isManagedServices===undefined||isManagedServices ==='') ? '' : 'false';
    return (
        <Fragment>
            <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.contract.COORDINATOR} colSize="s12">
                <div className="row">
                    <div className="custom-card col s6 br-1 pr-1">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.COORDINATOR}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.coordinators || []}
                            labelClass="customLabel mandate"
                            optionName='miDisplayName'
                            optionValue="username"
                            name="projectCoordinatorCode"
                            disabled={props.interactionMode}
                            defaultValue={coordinatorDetails.projectCoordinatorCode} //IGO QC D-900 Issue 1
                            onSelectChange={props.onChangeHandler}
                        />
                        <div className="col s12">
                            <a className={props.interactionMode === true ? "isDisabled" : null} href={coordinatorEmail ? "mailto: " + coordinatorEmail : "mailto: N/A"} disabled={props.interactionMode}>{props.coordinatorEmail ? props.coordinatorEmail : "N/A"}</a>
                        </div>
                    </div>
                    <div className=" custom-card col s6">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.project.MANAGED_SERVICES}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={localConstant.commonConstants.projectIsForNewFacility}
                            labelClass="customLabel mandate"
                            optionName="name"
                            optionValue="value"
                            name="isManagedServices"
                            disabled={props.interactionMode}
                            defaultValue={isManagedServices }
                            onSelectChange={props.onChangeHandler}
                        />
                        {isManagedServices.toString() === 'true' ?
                            <Fragment>
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    label={localConstant.project.TYPES_OF_SERVICE}
                                    type='select'
                                    colSize='s12'
                                    labelClass="customLabel mandate"
                                    className="browser-default"
                                    optionsList={props.managedServiceType || []}
                                    optionName='name'
                                    optionValue="id"
                                    name="managedServiceType"
                                    disabled={props.interactionMode}
                                    defaultValue={coordinatorDetails.managedServiceType}
                                    onSelectChange={props.onChangeHandler}
                                    selectstatus={false} //Default Select is added.
                                />
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    label={`${ localConstant.project.COORDINATOR } / ${ localConstant.project.MANAGER }`}
                                    type='select'
                                    colSize='s12'
                                    className="browser-default"
                                    optionsList={props.coordinators || []}
                                    labelClass="customLabel"
                                    optionName='miDisplayName'
                                    optionValue="username"
                                    disabled={props.interactionMode}
                                    name="managedServiceCoordinatorCode"
                                    defaultValue={coordinatorDetails.managedServiceCoordinatorCode} //IGO QC D-900 Issue 1
                                    onSelectChange={props.onChangeHandler}
                                />
                            </Fragment> : null}
                    </div>
                </div>
            </CardPanel>
        </Fragment>
    );
};
/** Other Details section input fields */
const OtherDetails = (props) => {
    const { otherDetails } = props;
    return (
        <CardPanel className="white lighten-4 black-text mb-2"
            title={localConstant.contract.OTHER_DETAILS} colSize="s12">
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    label={localConstant.contract.START_DATE}
                    labelClass="customLabel mandate"
                    colSize='s3'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDatePickBlur={props.handleOtherDetailStartDateBlur}
                    type='date'
                    name='contractStartDate'
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.startDate)}
                    onDateChange={props.fetchStartDate}
                    shouldCloseOnSelect={true}
                    disabled={props.interactionMode}
                />
                <CustomInput
                    hasLabel={true}
                    // labelClass="customLabel"
                    labelClass={otherDetails.projectStatus === "C" ? "mandate" : "customLabel"}
                    label={localConstant.contract.END_DATE}
                    colSize='s3'
                    type="date"
                    isNonEditDateField={false}
                    selectedDate={isEmpty(props.endDate) ? "" : dateUtil.defaultMoment(props.endDate)}
                    onDateChange={props.fetchEndDate}
                    onDatePickBlur={props.handleOtherDetailEndDateBlur}
                    name='projectEndDate'
                    autocomplete="off"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    shouldCloseOnSelect={true}
                    disabled={props.interactionMode}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.contract.STATUS}
                    type='select'
                    colSize='s3'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.status}
                    optionName='name'
                    optionValue='value'
                    inputClass="customInputs"
                    name="projectStatus"
                    disabled={props.interactionMode}
                    defaultValue={otherDetails.projectStatus } //Sanity Defect Fix
                    onSelectChange={props.onChangeHandler}
                />
            </div>
            <div className="row mb-0">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.project.POPUP_ON_VISIT}
                    isSwitchLabel={true}
                    switchName="isVisitOnPopUp"
                    id="isVisitOnPopUp"
                    colSize="s3"
                    disabled={props.interactionMode}
                    checkedStatus={otherDetails.isVisitOnPopUp}
                    onChangeToggle={props.switchOnChangeHandler}
                    switchKey={otherDetails.isVisitOnPopUp}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.project.EREPT_PROJECT_MAPPED}
                    isSwitchLabel={true}
                    switchName="isEReportProjectMapped"
                    id="eReptProjectMapped"
                    colSize="s3"
                    disabled={props.interactionMode}
                    checkedStatus={otherDetails.isEReportProjectMapped}
                    onChangeToggle={props.switchOnChangeHandler}
                    switchKey={otherDetails.isEReportProjectMapped}
                     />
            </div>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    divClassName='col '
                    label={localConstant.assignments.CUSTOMER_REPORTING_REQUIREMENTS}
                    type='textarea'
                    colSize='s12 mt-4'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.Project.generalDetails.CUSTOMER_REPORTING_REQUIREMENTS_MAXLENGTH}
                    name="projectClientReportingRequirement"
                    // disabled={props.interactionMode}
                    readOnly = {props.interactionMode}
                    value={otherDetails.projectClientReportingRequirement !== undefined ? otherDetails.projectClientReportingRequirement : ""}
                    onValueChange={props.onChangeHandler}
                //onValueBlur={props.onGeneralDetailsDataChange}
                />
            </div>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    divClassName='col '
                    label={localConstant.contract.ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES}
                    type='textarea'
                    colSize='s12'
                    //maxLength={fieldLengthConstants.Project.generalDetails.ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES_MAXLENGTH}
                    inputClass="customInputs"
                    name="projectAssignmentOperationNotes"
                    // disabled={props.interactionMode}
                    readOnly = {props.interactionMode}
                    value={otherDetails.projectAssignmentOperationNotes !== undefined ? otherDetails.projectAssignmentOperationNotes : ""}
                    onValueChange={props.onChangeHandler}
                //onValueBlur={props.onGeneralDetailsDataChange}
                />
            </div>
        </CardPanel>
    );
};

class GeneralDetails extends Component {
    state = {
        isActive: false
    }
    constructor(props) {
        super(props);
        this.state = {
            startDate: '',
            endDate: "",
            isActive: false,
            statusSelected: "",
            isNDTProjectType:false
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.updatedData = {};
        this.interactionMode = false;
    }

    isActiveToggle = (evnet) => {
        this.setState({ isActive: !this.state.isActive });
    }

    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0)
            this.props.actions.FetchModuleLogo("project");

        // D117 - fixes
        if (!isEmpty(this.props.generalDetails) && !isEmpty(this.props.businessUnit)) {
            const businessUnit =  GetProjectReportingParams({ 
                projectReportParams: this.props.businessUnit,
                businessUnit: this.props.generalDetails.projectType 
            });
            //const businessUnit = this.props.businessUnit.filter(iteratedValue => iteratedValue.name === this.props.generalDetails.projectType);
            if (businessUnit && businessUnit.length > 0 && businessUnit[0].invoiceType === "NDT") {
                this.updatedData["logoText"] = "Intertek";
                this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
            }
        }

    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    validatePercentage =(e) => {
        if (e.charCode === 45 || e.charCode === 46) {
            e.preventDefault();
        }
    }
    /**On Change Handler for general details data */
    generalDetailsOnchangeHandler = (e) => {
        // commented for editing the properly in budget monetary section
        //const result = formInputChangeHandler(e);
        //this.updatedData[result.name] = result.value;

        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;

        if(e.target.name === "companyCostCenterCode"){
            this.updatedData["companyCostCenterName"]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent;  //Added for D-996 
        }
        if (e.target.name === 'companyDivision') {
            this.updatedData["companyCostCenterCode"] = null;
        }
        //Commented for IGO QC D-890
        // restricting length of project budget warnings and hours budget warnings
        // if (e.target.name === 'projectBudgetWarning' || e.target.name === 'projectBudgetHoursWarning') {
        //     const value = e.target.value.toString();
        //     if (value.length > 2) {
        //         e.target.value = value.substring(0, 2);
        //         this.updatedData[e.target.name] = e.target.value;
        //     }
        // }

        // restricting length of project budget values and hours budget unit
        if (e.target.name === 'projectBudgetValue' || e.target.name === 'projectBudgetHoursUnit') {
            this.updatedData[e.target.name] = decimalCheck(e.target.value, 2);
        }

        if (e.target.name === "isManagedServices") {
            if (result === "true") {
                this.updatedData["isManagedServices"] = true;
                this.updatedData["managedServiceType"]= null; //Changes for D-1056
            } else {
                this.updatedData["managedServiceType"] = null;
                this.updatedData["managedServiceCoordinatorName"] = null;
                this.updatedData["managedServiceCoordinatorCode"] = null; //IGO QC D-900 Issue 1
                this.updatedData["isManagedServices"] = result === "false"?false:"";
                this.updatedData["managedServiceTypeName"]="";//1035 audit issue
            }
        }
        if(e.target.name === "projectCoordinatorCode"){
            if (!isEmpty(e.target.name)) {
                const coordinatorInfo=this.props.projectMICoordinator.filter(x => x.username === e.target.value);//IGO QC D-900 Issue 1
                this.updatedData["projectCoordinatorEmail"] = coordinatorInfo[0].email;
                this.updatedData["projectCoordinatorName"] = coordinatorInfo[0].displayName; //IGO QC D-900 Issue 1
            }
        }
        //IGO QC D-900 Issue 1 -Start
        if(e.target.name === "managedServiceCoordinatorCode"){
            if (!isEmpty(e.target.name)) {
                const coordinatorInfo=this.props.projectMICoordinator.filter(x => x.username === e.target.value);
                this.updatedData["managedServiceCoordinatorName"] = coordinatorInfo[0].displayName;
            }
        }
        //IGO QC D-900 Issue 1 -End
        //1035 audit issue start
        if(e.target.name === "managedServiceType"){
            this.updatedData["managedServiceTypeName"]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        }
        //1035 audit issue end
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        if (e.target.name === "projectType") {
            const businessUnit = this.props.businessUnit.filter(iteratedValue => iteratedValue.name === e.target.value);
            if (Array.isArray(businessUnit) && businessUnit.length > 0) {
                if (businessUnit[0].invoiceType === "NDT") {
                    this.setState({ isNDTProjectType: true });
                }
                else {
                    this.setState({ isNDTProjectType: false });
                    this.updatedData["logoText"] = null;
                }
            }
                //D767-No needs default selected based on NDT. Select manually.
            // if (e.target.value === "NDT (Non Destructive Testing)")
            //     this.updatedData["logoText"] = "Intertek";
            // else
            //     this.updatedData["logoText"] = "";
        }
        if(e.target.name === 'projectBudgetWarning' || e.target.name === 'projectBudgetHoursWarning')
        {
            const value = e.target.value.toString();
            if (value.length > 2) {
                const budgetwarning = parseInt(value.substring(0, 3));
                if(budgetwarning > 100){
                    e.target.value = 100; 
                }
                else{
                    e.target.value = budgetwarning;
                }
                this.updatedData[e.target.name] = e.target.value;
            }
        }
        this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
        this.updatedData = {};
    }

    // /** Start date updation handler */
    // fetchStartDate = (date) => {
    //     this.setState({
    //         startDate: date
    //     }, () => {
    //         this.updatedData.projectStartDate = this.state.startDate.format();
    //         this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
    //     });
    // }

    /** Start date updation handler */
    fetchStartDate = (date) => {
        this.setState({
            startDate: date
        }, () => {
            this.updatedData.projectStartDate = this.state.startDate !== null ? this.state.startDate : '';
            this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
        });
    }

    /** End date updation handler */
    fetchEndDate = (date) => {
        this.setState({
            endDate: date
        }, () => {
            this.updatedData.projectEndDate = this.state.endDate !== null ? this.state.endDate : "";
            this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
        });
    }

    /** Date blur validation for start date */
    handleOtherDetailStartDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);//DateValidation
            if (!isValid) {
                IntertekToaster(localConstant.techSpec.common.INVALID_START_DATE, 'warningToast gdStartDateVal');
            }
        }
    }

    /** Date blur validation for End date */
    handleOtherDetailEndDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isValidDate(e.target.value);
            if (!isValid) {
                // IntertekToaster("Invalid End Date", 'warningToast gdStartDateVal');
            }
        }
    }

    /** division on change handler to load the cost center for the selected division */
    divisionOnChangeHandler = (e) => {
        this.generalDetailsOnchangeHandler(e);
        this.props.actions.FetchProjectCompanyCostCenter(e.target.value);
    }

    /** Handler to make the entered decimal data as two digit */
    checkNumber = (e) => {
        // if(e.target.value ==="."){
        //     e.target.value="0";
        // }
        // if(!isEmpty(e.target.value)){
        // e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
        // this.generalDetailsOnchangeHandler(e);
        // }

            if(!isEmpty(e.target.value) && e.target.value >0){            
                e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
                this.generalDetailsOnchangeHandler(e);
            }
            else
            {
                e.target.value="00.00";
                this.generalDetailsOnchangeHandler(e);
            }
           
    }

    budgetWarningHandler =(e) => {
        if(!e.target.value)
        {
            e.target.value = 75;
        }
    }
    /** On Change handler for switches */
    switchOnChangeHandler = (e) => {
       
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        this.props.actions.UpdateProjectGeneralDetails(this.updatedData);
        this.updatedData = {};
    }
    render() {
        const { generalDetails = {}, interactionMode } = this.props;
        const monetaryValues = {
            hasLabel :true,
            divClassName :'col',
            label:localConstant.budget.VALUE,
            type:'text',
            dataType:'decimal',
            valueType:'value',
            colSize:'s6',
            inputClass:'customInputs',
            labelClass:"customLabel mandate",
            max:'99999999999',
            min:'0',
            name:'projectBudgetValue',
            maxLength: fieldLengthConstants.common.budget.BUDGET_VALUE_MAXLENGTH,
            prefixLimit:fieldLengthConstants.common.budget.BUDGET_VALUE_PREFIX_LIMIT,
            suffixLimit:fieldLengthConstants.common.budget.BUDGET_VALUE_SUFFIX_LIMIT,
            isLimitType:true,
            readOnly:this.props.interactionMode,
            // disabled:this.props.interactionMode,
            value:generalDetails.projectBudgetValue?thousandFormat(generalDetails.projectBudgetValue):'',
            onValueChange:this.generalDetailsOnchangeHandler,
            //onValueBlur:this.checkNumber
        };
       
        const monetaryWarning = {
            hasLabel :true,
            divClassName :'col',
            label:localConstant.budget.WARNING,
            type:'number',
            dataValType:'valueText',
            // dataType:'numeric',
            // valueType:'value',
            colSize:'s3',
            inputClass:'customInputs',
            labelClass:"customLabel mandate",
            max:'100',
            min:'0',
            name:'projectBudgetWarning',
            maxLength:'2',
            readOnly:this.props.interactionMode,
            // disabled:this.props.interactionMode,
            value:generalDetails.projectBudgetWarning,
            onValueChange:this.generalDetailsOnchangeHandler,
            onValueKeypress:this.validatePercentage
            // onValueBlur:this.budgetWarningHandler  //Commented for IGO QC D-890
        };
        const monetaryCurrency ={
            label:`${ localConstant.budget.CURRENCY }`,
            value:generalDetails.projectBudgetCurrency,
            hasLabel:true,
            divClassName:'col',          
            colSize:'s3 mt-4',
            className:"browser-default",
            labelClass:"mandate",
            name:"projectBudgetCurrency",
            disabled:true,
            defaultValue:generalDetails.projectBudgetCurrency !== undefined ? generalDetails.projectBudgetCurrency : "",
        };

        const monetaryTaxes=[
            {
                className:this.props.projectMode === "createProject"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.INVOICED_TO_DATE_EXCL }:`,
                value:generalDetails.projectInvoicedToDate?thousandFormat(generalDetails.projectInvoicedToDate) :"0.00"
            },
            {
                className:this.props.projectMode === "createProject"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.UNINVOICED_TO_DATE_EXCL }:`,
                value:generalDetails.projectUninvoicedToDate ?thousandFormat(generalDetails.projectUninvoicedToDate) :"0.00"
            },
            {
                className:this.props.projectMode === "createProject"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.REMAINING }:`,
                value:generalDetails.projectRemainingBudgetValue ? thousandFormat(generalDetails.projectRemainingBudgetValue) : "0.00"
            },
            {
                className:this.props.projectMode !== "createProject" && generalDetails.projectBudgetValueWarningPercentage <= 0 ?"hide":this.props.projectMode === "createProject"?"hide":"custom-Badge col text-red-parent",
                colSize:"s12",
                label: generalDetails.projectBudgetValueWarningPercentage > 0 ? thousandFormat(generalDetails.projectBudgetValueWarningPercentage) + " " + ` ${ localConstant.commonConstants.PROJECT_BUDGET_WARNING } ` : null
            }
        ];

        const unitValues = [
            {
                hasLabel:true,
                divClassName:"col",
                label:localConstant.budget.UNITS,
                type: 'text',
                dataType: 'decimal',
                valueType: 'value',
                colSize:'s6',
                inputClass:'customInputs',
                labelClass:'customLabel mandate',
                max:'999999999',
                name:'projectBudgetHoursUnit',
                maxLength:fieldLengthConstants.common.budget.BUDGET_HOURS_MAXLENGTH,
                min:'0',
                prefixLimit:fieldLengthConstants.common.budget.BUDGET_HOURS_PREFIX_LIMIT,
                suffixLimit :fieldLengthConstants.common.budget.BUDGET_HOURS_SUFFIX_LIMIT, 
                isLimitType:true,
                readOnly:this.props.interactionMode,
                // disabled:this.props.interactionMode,
                value:generalDetails.projectBudgetHoursUnit?thousandFormat(generalDetails.projectBudgetHoursUnit):'',
                onValueChange:this.generalDetailsOnchangeHandler,
                //onValueBlur:this.checkNumber
            },
            {
                hasLabel:true,
                divClassName:'col',
                label:localConstant.budget.WARNING,
                type:'number',
                dataValType:'valueText',
                colSize:'s3',
                inputClass:"customInputs",
                labelClass:"customLabel mandate",
                name:'projectBudgetHoursWarning',
                max:'100',
                min:'0',
                readOnly:this.props.interactionMode,
                // disabled:this.props.interactionMode,
                value:generalDetails.projectBudgetHoursWarning,
                onValueChange: this.generalDetailsOnchangeHandler,
                onValueKeypress:this.validatePercentage
                // onValueBlur:this.budgetWarningHandler   //Commented for IGO QC D-890
            }
        ];

        const unitTaxes= [
            {
                className:this.props.projectMode === "createProject" ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.INVOICED_TO_DATE }: `,
                value:generalDetails.projectHoursInvoicedToDate?thousandFormat(generalDetails.projectHoursInvoicedToDate):"0.00"
            },
            {
                className:this.props.projectMode === "createProject" ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.UNINVOICED_TO_DATE }: `,
                value: generalDetails.projectHoursUninvoicedToDate?thousandFormat(generalDetails.projectHoursUninvoicedToDate):"0.00"
            },
            {
                className:this.props.projectMode === "createProject" ? "hide" : "custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.REMAINING }:  `,
                value: generalDetails.projectRemainingBudgetHours ?thousandFormat(generalDetails.projectRemainingBudgetHours):"0.00"
            },
            {
                className:this.props.projectMode !== "createProject" && generalDetails.projectBudgetHourWarningPercentage <= 0 ?"hide":this.props.projectMode === "createProject"?"hide":"custom-Badge col text-red-parent",
                colSize:"s12",
                label: generalDetails.projectBudgetHourWarningPercentage > 0 ? thousandFormat(generalDetails.projectBudgetHourWarningPercentage) + " " + ` ${ localConstant.commonConstants.PROJECT_BUDGETHOUR_WARNING } ` : null
            }
        ];

        /** Check for Inactive MI-Coordinator */
        let projectMICoordinatorList = [];
        if(!isEmpty(this.props.projectMICoordinator)){
            projectMICoordinatorList = processMICoordinators(this.props.projectMICoordinator);
        }

        /** Display e-mail ID for selected Coordinator */
        let coordinatorEmail = null;
        if (!isEmpty(generalDetails) && !isEmpty(generalDetails.projectCoordinatorName)) {
            // coordinatorEmail = this.props.projectMICoordinator.map(iteratedValue => {
            //     if (iteratedValue.username === generalDetails.projectCoordinatorName) {
            //         return iteratedValue.email;
            //     }
            // });
            coordinatorEmail =getCoordinatorData(generalDetails.projectCoordinatorCode,
                projectMICoordinatorList, "username", "email");
        }

        /** checking project type */
        let isNDT = false;
        if(!isEmpty(generalDetails) && !isEmpty(this.props.businessUnit)){
            const businessUnit = this.props.businessUnit.filter(iteratedValue=>iteratedValue.name === generalDetails.projectType);
            if(businessUnit && businessUnit.length > 0 && businessUnit[0].invoiceType === "NDT"){
                isNDT = true;     
            }
        }

        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard">
                    <Details
                        generalDetails={generalDetails}
                        projectCompanyDivision={this.props.projectCompanyDivision}
                        projectCompanyOffice={this.props.projectCompanyOffices}
                        projectCompanyCostCenter={this.props.projectCompanyCostCenter}
                        businessUnit={this.props.businessUnit}
                        projectLogo ={this.props.projectLogo}
                        isNDTProjectType = {this.state.isNDTProjectType}
                        isNDT = {isNDT}
                        workflowType={localConstant.commonConstants.workFlow}
                        industrySector={this.props.industrySector && arrayUtil.sort(this.props.industrySector, 'name', 'asc')}
                        divisionOnChangeHandler={this.divisionOnChangeHandler}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        switchOnChangeHandler={this.switchOnChangeHandler}
                        projectMode = {this.props.projectMode}
                        interactionMode={interactionMode}
                        >
                    </Details>
                    <Coordinator
                        coordinatorDetails={generalDetails}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        // coordinators={this.props.projectCoordinator}
                        coordinators = {projectMICoordinatorList && arrayUtil.sort(projectMICoordinatorList, 'miDisplayName', 'asc')}
                        managedServiceType={this.props.managedServiceType}
                        coordinatorEmail={coordinatorEmail}
                        switchOnChangeHandler={this.switchOnChangeHandler}
                        interactionMode={interactionMode}
                        >
                    </Coordinator>

                    <BudgetMonetary 
                        monetaryValues={monetaryValues}
                        monetaryCurrency={monetaryCurrency}
                        monetaryTaxes={monetaryTaxes}
                        unitValues={unitValues}
                        unitTaxes={unitTaxes}
                        isCurrencyEditable={false}                
                        monetaryWarning={monetaryWarning}
                    />
                   
                    <OtherDetails
                        status={localConstant.commonConstants.status}
                        startDate={generalDetails.projectStartDate ? generalDetails.projectStartDate : ''}
                        endDate={generalDetails.projectEndDate ? generalDetails.projectEndDate : ""}
                        otherDetails={generalDetails} 
                        handleOtherDetailStartDateBlur={this.handleOtherDetailStartDateBlur}
                        handleOtherDetailEndDateBlur={this.handleOtherDetailEndDateBlur}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        fetchStartDate={this.fetchStartDate}
                        fetchEndDate={this.fetchEndDate}
                        switchOnChangeHandler={this.switchOnChangeHandler}
                        interactionMode={interactionMode}
                    />
                </div>
            </Fragment >
        );
    }
}

export default GeneralDetails;