import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import Panel from '../../../../common/baseComponents/panel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import moment from 'moment';
import { isEmpty, getlocalizeData, formInputChangeHandler } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../../constants/appConstants';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';

const localConstant = getlocalizeData();

const TimeOffRequestDiv = (props) => {
    const resorceName = props.userType === "TechnicalSpecialist" ? props.resourceNameList.length === 1 ? props.resourceNameList[0].epin : "" : props.selectedResource;
    const empType= props.userType === "TechnicalSpecialist" ? props.resourceNameList.length === 1 ? props.resourceNameList[0].employmentType : "" : props.selectedempType?props.selectedempType:"";
    const empInteractionMode=(props.selectedempType || props.resourceNameList.length === 1) ? true : false ;
    const comments = props.comments ? props.comments : "" ;
    const resourceInteractionMode= props.resourceNameList.length === 1 ? true : false;
return(
    <Fragment>
         <LabelwithValue
             label={localConstant.techSpec.timeOffRequest.RESOURCE +' : '}
             value={localConstant.techSpec.timeOffRequest.TIME_OFF_REQUEST_HEADER}
             colSize="s3" />
    <CardPanel>
    <form onSubmit={props.saveRequest} onReset={props.clearSearchData} autoComplete="off">
        <div className="row mb-0">
            <div className="col s12 pr-0 pl-0">
                <CustomInput
                    hasLabel={true}
                    name="resourceName"
                    id="epin"
                    labelClass="mandate"
                    divClassName='col'
                    colSize='s4'
                    label={localConstant.techSpec.timeOffRequest.RESOURCE_NAME}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.resourceNameList}
                    optionName="fullName"
                    optionValue="epin"
                    onSelectChange={props.inputHandleChange} 
                    defaultValue={resorceName}
                    disabled={resourceInteractionMode}
                    />
                {/* <CustomInput
                    hasLabel={true}
                    name="employmentType"
                    labelClass="mandate"
                    divClassName='col pl-0'
                    colSize='s4'
                    label={localConstant.techSpec.timeOffRequest.EMPLOYMENT_TYPE}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.employmentTypeList}
                    optionName="name"
                    optionValue="name"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={empType}
                    disabled={empInteractionMode} */}
                     <CustomInput
                    hasLabel={true}
                    name="employmentType"
                    labelClass=""
                    divClassName='col pl-0'
                    colSize='s4'
                    label={localConstant.techSpec.timeOffRequest.EMPLOYMENT_TYPE}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    value={empType}
                    readOnly={true}
                     />
                <CustomInput
                    hasLabel={true}
                    labelClass="mandate"
                    name="leaveCategoryType"
                    divClassName='col pl-0'
                    colSize='s4'
                    label={localConstant.techSpec.timeOffRequest.CATEGORY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.category}
                    optionName="categoryName"
                    optionValue="leaveTypeCategory"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.leaveCategoryType ? props.leaveCategoryType : ""}
                     />
                <CustomInput
                    hasLabel={true}
                    labelClass="mandate"
                    name="timeOffFrom"
                    colSize='s4'
                    label={localConstant.techSpec.timeOffRequest.TIME_OFF_FROM}
                    type='date'
                    inputClass="customInputs"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDateChange={props.fetchTimeOffFrom}
                    onDatePickBlur={props.handleOtherDetailTimeOffFromDateBlur}
                    selectedDate={dateUtil.defaultMoment(props.timeOffFrom)}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="mandate"
                    name="timeOffThrough"
                    colSize='s4 pl-0'
                    label={localConstant.techSpec.timeOffRequest.TIME_OFF_THROUGH}
                    type='date'
                    inputClass="customInputs"
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDateChange={props.fetchTimeOffThrough}
                    onDatePickBlur={props.handleOtherDetailTimeOffThroughDateBlur}
                    selectedDate={dateUtil.defaultMoment(props.timeOffThrough)}
                />
                <CustomInput
                    hasLabel={true}
                    name="comments"
                    divClassName='col'
                    labelClass="customLabel"
                    id="comments"
                    colSize='s8'
                    label={localConstant.techSpec.timeOffRequest.COMMENTS}
                    type='textarea'
                    inputClass="customInputs"
                    maxLength={4000}
                    onValueChange={props.inputHandleChange}
                    value={ comments } />

                <div className="col s4 mt-5 pl-0 pt-3" >
                    <button type="submit" className="waves-effect btn ml-0">Submit</button>
                    <button type="reset" className="ml-2 waves-effect btn" >Reset</button>
                </div>
            </div>

        </div>
    </form>
    </CardPanel>
    </Fragment>
);
};

class TimeOffRequest extends Component {
    constructor(props) {
        super(props);
        this.state = {
            timeOffFrom: '',
            timeOffThrough: '',
            comments:'',
            selectedResource:'',
            leaveCategoryType:'',
            selectedempType:'',
            errorList:[],
            userType: localStorage.getItem(applicationConstants.Authentication.USER_TYPE), 
            categoryList:[]
        };
        this.updatedData = {};
        this.resourceList =[];
        this.errors=[];
        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action: this.closeErrorList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
    }

    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (inputvalue.name === 'resourceName') {
            const selectedOption = e.nativeEvent.target[e.nativeEvent.target.selectedIndex];//e.target.selectedOptions[0];
            this.updatedData[e.target.name] = selectedOption.text;
            this.updatedData[e.target.id] = selectedOption.value;
            this.props.resourceNameList && this.props.resourceNameList.map((selectedValue, Index) => {
                if (selectedValue.epin == selectedOption.value) {
                    this.updatedData['employmentType'] = this.props.resourceNameList[Index].employmentType;
                };
            });
            this.setState({
                selectedResource: this.updatedData.epin,
                selectedempType: this.updatedData.employmentType
            });
            this.props.actions.FetchCategory(this.updatedData.employmentType).then(res => {
                if (res) {
                    if (Array.isArray(this.props.category) && this.props.category.length > 0) {
                        const tempCategoryList = [];
                        this.props.category.forEach(categoryData => {
                            const tempCategory = Object.assign({}, categoryData);
                            if (tempCategory.leaveTypeCategory.toLowerCase() === "sickleave")
                                tempCategory.categoryName = "Sick Leave";
                            else
                                tempCategory.categoryName = tempCategory.leaveTypeCategory;
                            tempCategoryList.push(tempCategory);
                        });
                        this.setState({
                            categoryList: tempCategoryList
                        });
                    }
                }
            });
        }
        if(inputvalue.name === 'comments'){
            this.setState({
                comments:this.updatedData.comments
              });
        }
        if(inputvalue.name === 'leaveCategoryType'){
            this.setState({
                leaveCategoryType:this.updatedData.leaveCategoryType
              });
        }
        this.props.actions.TechSpechUnSavedData(false);
    }

    //Form Search 
    saveRequest = (e) => {
        e.preventDefault();
        const valid = this.mandatoryFieldsValidationCheck();
        if (valid === true) {
            const technicalSpecialistId = this.state.userType === "TechnicalSpecialist" ? this.props.resourceNameList.length === 1 ? this.props.resourceNameList[0].epin : "" : this.updatedData.epin;

            this.props.actions.FetchCalendarData({
                companyCode: this.props.selectedCompany,
                isActive: true,
                startDateTime: moment(this.state.timeOffFrom).format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                endDateTime: moment(this.state.timeOffThrough).format(localConstant.commonConstants.SAVE_DATE_FORMAT),
                technicalSpecialistId: technicalSpecialistId,
                isView: true
            }, false, false).then(res=>{
                if (Array.isArray(res) && res.length > 0){
                    this.errors.push(localConstant.techSpec.timeOffRequest.CONFLICT_DAYS_VALIDATION);
                    this.setState({
                        errorList:this.errors
                      });
                }
                else { 
                    this.updatedData['companyCode']= this.props.selectedCompany;//def 978 
                    this.props.actions.TimeOffRequestSave(this.updatedData);
                    this.clearSearchData(e);
                }
            });
        }

    }

    clearSearchData=(e)=>{
        e.preventDefault();
        this.updatedData={};
        this.setState({
         timeOffFrom: '',
         timeOffThrough: '',
         comments:'',
         selectedResource:'',
         leaveCategoryType:'',
         selectedempType:'',
       });
        const userType = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
        if (userType !== "TechnicalSpecialist") {
            this.props.actions.FetchCategory(); //Fixes for scenario defect 157
            this.setState({
                categoryList: []
            });
        }
     //  this.props.actions.FetchTimeOffRequestData();
     //  this.props.actions.getEmploymentType(this.updatedData);
    }

    mandatoryFieldsValidationCheck =()=>{
        const userType    = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
        if(userType !== "TechnicalSpecialist"){
            if(isEmpty(this.updatedData.epin)){
                this.errors.push(`${ localConstant.techSpec.timeOffRequest.TIME_OFF_REQUEST } - ${ localConstant.techSpec.timeOffRequest.RESOURCE_NAME }`);
             }
        }  
        this.Validation();
        if(this.errors.length > 0){
            this.setState({
              errorList:this.errors
            });
            return false;
        } else{
            return true;
        }
    }

    Validation=()=>{
        if(isEmpty(this.updatedData.leaveCategoryType)){
           this.errors.push(`${ localConstant.techSpec.timeOffRequest.TIME_OFF_REQUEST } - ${ localConstant.techSpec.timeOffRequest.CATEGORY }`);
        }
        if(this.updatedData.timeOffFrom === undefined || this.updatedData.timeOffFrom === ""){
           this.errors.push(`${ localConstant.techSpec.timeOffRequest.TIME_OFF_REQUEST } - ${ localConstant.techSpec.timeOffRequest.TIME_OFF_FROM }`);
        }
        if(this.updatedData.timeOffThrough === undefined || this.updatedData.timeOffThrough === ""){
           this.errors.push(`${ localConstant.techSpec.timeOffRequest.TIME_OFF_REQUEST } - ${ localConstant.techSpec.timeOffRequest.TIME_OFF_THROUGH }`);
        }
    }

    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
        this.errors = [];
      }

    //Time Off From Handler
    fetchTimeOffFrom= (date) => {
        if(!moment(date).isAfter(moment(),'day')){
            if(!moment(moment()).isSame(moment(date), 'day')){
                IntertekToaster(localConstant.validationMessage.TIMEOFF_FROM_CANNOT_LESSTHAN_CURRENT_DATE, 'warningToast gdEndDateValCheck');
                return false;
            }
        }
        this.setState({
            timeOffFrom: date.add(8,'h'),
            timeOffThrough: ""
        }, () => {
            this.updatedData.timeOffFrom = this.state.timeOffFrom !== null ? this.state.timeOffFrom.format(localConstant.techSpec.common.DATE_TIME_FORMAT) : "";
            this.updatedData.timeOffThrough="";
        });
        this.props.actions.TechSpechUnSavedData(false);
    }

    handleOtherDetailTimeOffFromDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isValidDate(e.target.value);
            if (!isValid) {
                // IntertekToaster("Invalid Start Date", 'warningToast gdStartDateVal');
            }
        }
    }

    //Time Off Through Handler
    fetchTimeOffThrough= (date) => {
        const isValid = dateUtil.isValidDate(date);
        let customDate;
        if (!isValid) {
            customDate=moment(date).format(localConstant.techSpec.common.DATE_FORMAT);
            if(!moment(customDate).isAfter((this.state.timeOffFrom),'day')){
                if(!moment(this.state.timeOffFrom).isSame(moment(customDate), 'day')){
                    IntertekToaster(localConstant.validationMessage.TIMEOFF_THROUGH_SHOULD_LESSTHAN_TIMEOFF_FROM, 'warningToast gdEndDateValCheck');
                    return false;
                }
            }
        }
        this.setState({
            timeOffThrough: moment(customDate,localConstant.techSpec.common.DATE_TIME_FORMAT).add(18,'h')
        }, () => {
            this.updatedData.timeOffThrough = this.state.timeOffThrough !== null ? this.state.timeOffThrough.format(localConstant.techSpec.common.DATE_TIME_FORMAT) : "";
        });
        this.props.actions.TechSpechUnSavedData(false);
    }

    handleOtherDetailTimeOffThroughDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isValidDate(e.target.value);
            if (!isValid) {
                // IntertekToaster("Invalid Start Date", 'warningToast gdStartDateVal');
            }
        }
    }

    dynamicsort(property) {
        return function (a, b) {
            if (a.hasOwnProperty(property) && b.hasOwnProperty(property)) {
                if (a[property] !== null && b[property] !== null)
                    return a[property].localeCompare(b[property]);
            }
        };
    }

    componentDidMount() {
        this.props.actions.FetchTimeOffRequestData().then(res => {
            if (Array.isArray(this.props.category) && this.props.category.length > 0) {
                const tempCategoryList = [];
                this.props.category.forEach(categoryData => {
                    const tempCategory = Object.assign({}, categoryData);
                    if (tempCategory.leaveTypeCategory.toLowerCase() === "sickleave")
                        tempCategory.categoryName = "Sick Leave";
                    else
                        tempCategory.categoryName = tempCategory.leaveTypeCategory;
                    tempCategoryList.push(tempCategory);
                });
                this.setState({
                    categoryList: tempCategoryList
                });
            }
        });
    }
    render() {
        const { resourceNameList, employmentTypeList, category,resourceDetails } = this.props;
        this.resourceList =[];
        resourceNameList.sort(this.dynamicsort("firstName")).forEach(iteratedValue =>{
            const resourceName={
               'fullName':`${ iteratedValue.firstName } ${ iteratedValue.lastName }`,
            };
            const value=Object.assign({},iteratedValue,resourceName);
            this.resourceList.push(value);
        });

        return (
            <div className="customCard">
               {this.state.errorList.length > 0 ?
                    <Modal title={ localConstant.commonConstants.CHECK_MANDATORY_FIELD }
                    titleClassName="chargeTypeOption"
                    modalContentClass="extranetModalContent"
                    modalClass="ApprovelModal"
                    modalId="errorListPopup"
                    formId="errorListForm"
                    buttons={this.errorListButton}
                    isShowModal={true}>
                         <ErrorList errors={this.state.errorList}/>
                    </Modal> : null
                }
                <TimeOffRequestDiv
                    inputHandleChange={(e) => this.inputHandleChange(e)}
                    category={this.state.categoryList}
                    // category={category}
                    employmentTypeList={employmentTypeList}
                    resourceNameList={this.resourceList.sort()}
                    saveRequest={this.saveRequest}
                    clearSearchData={this.clearSearchData}
                    fetchTimeOffFrom={this.fetchTimeOffFrom}
                    handleOtherDetailTimeOffFromDateBlur={this.handleOtherDetailTimeOffFromDateBlur}
                    timeOffFrom={this.state.timeOffFrom}
                    fetchTimeOffThrough={this.fetchTimeOffThrough}
                    handleOtherDetailTimeOffThroughDateBlur={this.handleOtherDetailTimeOffThroughDateBlur}
                    timeOffThrough={this.state.timeOffThrough}
                    updatedData={this.updatedData}
                    comments={this.state.comments}
                    userType={this.state.userType}
                    leaveCategoryType={this.state.leaveCategoryType}
                    selectedResource={this.state.selectedResource ? this.state.selectedResource : ""}
                    selectedempType={this.state.selectedempType}
                   // selectedempType={this.resourceList.length === 1 ? this.resourceList[0].employmentType : ""}
                />
            </div>
        );
    }
}

export default TimeOffRequest;
