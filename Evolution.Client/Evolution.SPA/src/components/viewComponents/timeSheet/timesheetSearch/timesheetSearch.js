import React, { Component } from 'react';
import { HeaderData } from './timesheetSearchHeader';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData,isEmpty } from '../../../../utils/commonUtils';
import SearchFilter from '../../../searchFilter';
import { searchFilterWithFunctionRefs,defaultFilterData } from './timesheetSearchFields';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import dateUtil from '../../../../utils/dateUtil';
import { validateObjectHasValue } from '../../../../utils/objectUtil';
import { applicationConstants } from '../../../../constants/appConstants';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { required } from '../../../../utils/validator';
import { moduleViewAllRights_Modified } from '../../../../utils/permissionUtil';
import { securitymodule } from '../../../../constants/securityConstant';

const localConstant = getlocalizeData();

class TimesheetSearch extends Component {
    constructor(props) {
        super(props);
        this.childSearchFilter = React.createRef();
        //this.timesheetSearchFilter = {};

        this.modelBtns = {
            errorListButton: [
              {
                type:'button',
                name: localConstant.commonConstants.OK,
                action: (e) => this.closeErrorList(e),
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
              }
            ]
        };

        //this.dropdownHandler = this.dropdownHandler.bind(this);
        this.handleLatestDateChange = this.handleLatestDateChange.bind(this);
        this.handleEariliestDateChange = this.handleEariliestDateChange.bind(this);
        this.changeHandler = this.changeHandler.bind(this);
        this.csvLink = React.createRef();

        const functionRefs = {};
        functionRefs["changeHandler"] = this.changeHandler;
        functionRefs["handleLatestDateChange"]=this.handleLatestDateChange;
        functionRefs["handleEariliestDateChange"]=this.handleEariliestDateChange;
        //functionRefs["dropdownHandler"] = this.dropdownHandler;
        functionRefs["resetAssignment"] = this.resetAssignment;
        functionRefs["csvExportClick"] = this.csvExportClick;
        this.searchFields = searchFilterWithFunctionRefs(functionRefs);
        this.state ={
            selectedEarliestDate:'',
            selectedLatestDate:'',
            errorList: [],
            timesheetSearchFilter:defaultFilterData,
            timesheetSearchList:[], //Search Optimization
            timesheetSearchTotalCount:0,
            searchFields:this.searchFields
        };

        this.csvLink = React.createRef();
    }

    componentDidMount=()=>{
        this.props.actions.FetchTaxonomyBusinessUnitForSearch();

        this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();
       this.props.actions.ClearData();

       this.props.actions.FetchCoordinatorForContractHoldingCompanyForSearch();
        this.props.actions.FetchCoordinatorForOperatingCompanyForSearch();

        let isChanged = false;
        this.searchFields.subComponents.forEach(componentProps => {
            if (componentProps.properties && componentProps.properties.name === 'timesheetSubCategory' || componentProps.properties.name === 'timesheetService') {
                componentProps.properties.disabled = true;
                isChanged = true;
                return true;
            }
        });
        if (isChanged) {
            this.setState({
                searchFields: this.searchFields
            });
        }
    }

    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    handleDateBlur = (e, type) => {
        const date = moment(e._isAMomentObject ? e : e.target.value);
        if (date.isValid()) {
            type && this[type](date);
        }
        else {
            IntertekToaster(localConstant.commonConstants.INVALID_DATE_FORMAT, 'warningToast PercentRange');
            return false;
        }
    }
    componentWillUnmount()
    {
        this.props.actions.ClearTimesheetSearchResults();
        this.props.actions.ClearData();
        this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();
    }

    validateDate = (e) => {
         
        // const mDate = moment(e);
        // if (mDate && mDate.isMoment() && !mDate.isValid()) {
        //     return false;
        // }
        if (e && e.target !== undefined) {
            if (!dateUtil.isValidDate(e.target.value)) {
                return false;
            }
        }
        return true;
    };
    
    handleEariliestDateChange = (date) => {
        const filterData = { ...this.state.timesheetSearchFilter }; 
        if (isEmpty(date)) {
            filterData.timesheetStartDate="";           
        }
        else {
            filterData.timesheetStartDate = moment(date);           
        }
        this.setState({
            timesheetSearchFilter: filterData
        });
    }

    handleLatestDateChange = (date) => {
        const filterData = { ...this.state.timesheetSearchFilter }; 
        if (isEmpty(date)) {
            filterData.timesheetEndDate="";           
        }
        else {
            filterData.timesheetEndDate = moment(date);           
        }
        this.setState({
            timesheetSearchFilter: filterData
        });
    }

    // dropdownHandler = (e) => {  
    //     this.timesheetSearchFilter[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
    //     this.timesheetSearchFilter[e.target.id] = e.nativeEvent.target.value;
    //     // const updateFilter = { ...defaultFilterData, ...this.timesheetSearchFilter };
    //     // this.setState({ timesheetSearchFilter:updateFilter });
    //     if(e.target.name === 'timesheetContractHolderCompany'){
    //         this.props.actions.FetchCoordinatorForContractHoldingCompany([ e.nativeEvent.target.value ]);
    //     }
    //     if(e.target.name === 'timesheetOperatingCompany'){
    //         this.props.actions.FetchCoordinatorForOperatingCompany([ e.nativeEvent.target.value ]);
    //     }
    // }

    changeHandler = (e) => { 
        let isChanged=false;    
        const updateFilter = { ...this.state.timesheetSearchFilter };       
        if(e.target.type === 'select'){
            updateFilter[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            updateFilter[e.target.id] = e.nativeEvent.target.value;
        }else{
            updateFilter[e.target.name] = e.target.value;
        }
        if(e.target.name === 'timesheetContractHolderCompany'){
            if(e.nativeEvent.target.value===""){
                this.props.actions.FetchCoordinatorForContractHoldingCompanyForSearch();
            }
            else
            this.props.actions.FetchCoordinatorForContractHoldingCompanyForSearch([ e.nativeEvent.target.value ]);
        }
        if(e.target.name === 'timesheetOperatingCompany'){
            if(e.nativeEvent.target.value===""){
                this.props.actions.FetchCoordinatorForOperatingCompanyForSearch();
            }
            else
            this.props.actions.FetchCoordinatorForOperatingCompanyForSearch([ e.nativeEvent.target.value ]);
        }
        if (e.target.name === "timesheetCategory") {
            this.props.actions.ClearSubCategory();
            if (e.target.value === ''){
                
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'timesheetService' || componentProps.properties.name === 'timesheetSubCategory') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            else{
                this.props.actions.FetchTechSpecSubCategory(e.target.value,true);
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'timesheetService') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                        return true;
                    }
                    if (componentProps.properties && componentProps.properties.name === 'timesheetSubCategory') {
                        componentProps.properties.disabled = false;
                        isChanged = true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            updateFilter['timesheetSubCategory'] = "";
            updateFilter['timesheetService'] = "";
            this.props.actions.ClearServices();
        }
        if (e.target.name === "timesheetSubCategory") {
            this.props.actions.ClearServices();
            if (e.target.value === ''){
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'timesheetService') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                        return true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            else{
                this.props.actions.FetchTechSpecServices(updateFilter.timesheetCategory, e.target.value,true);
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'timesheetService') {
                        componentProps.properties.disabled = false;
                        isChanged = true;
                        return true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            updateFilter['timesheetService'] = "";
        }
        this.setState({ timesheetSearchFilter:updateFilter });
        
    }

    getPanelSubTitle = () => {
        //D-138
        if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
            return localConstant.timesheet.CREATE_TIMESHEET;
        }         
        else {
            return localConstant.timesheet.EDIT_VIEW_TIMESHEET;
        }
    }
    
    //timesheet search
    searchTimesheet = (e,isLoadMore) => {
        e.preventDefault();      
        const searchFields = { ...this.state.timesheetSearchFilter };
        searchFields.timesheetCustomerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        searchFields.timesheetCustomerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';  
        if(!isEmpty(searchFields.timesheetStartDate)){
            searchFields.timesheetStartDate= moment(searchFields.timesheetStartDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }
        if(!isEmpty(searchFields.timesheetEndDate)){
        searchFields.timesheetEndDate= moment(searchFields.timesheetEndDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }

        const { timesheetStartDate, timesheetEndDate } = searchFields;
         
        if(timesheetStartDate &&   !this.validateDate(timesheetStartDate) ){
            IntertekToaster(localConstant.validationMessage.TIMESHEET_SEARCH_EARLIEST_DATE, 'warningToast gdStartDateVal');
            return false;
        }
        if(timesheetEndDate && !this.validateDate(timesheetEndDate)){
            IntertekToaster(localConstant.validationMessage.TIMESHEET_SEARCH_LATEST_DATE, 'warningToast gdStartDateVal');
            return false;
        }

        const isValid = validateObjectHasValue(searchFields,Object.keys(applicationConstants.timesheetSearchValidateFields));
        if (!isValid) {
            this.setState({
                errorList: Object.values(applicationConstants.timesheetSearchValidateFields)
            });
            return false;
        }

        searchFields.loggedInCompanyCode = this.props.selectedCompany;
        if (Array.isArray(this.props.companyList) && this.props.companyList.length > 0) {
            const tempCompany = this.props.companyList.filter(company => this.props.selectedCompany === company.companyCode);
            searchFields.loggedInCompanyId = Array.isArray(tempCompany) && tempCompany.length > 0  ? tempCompany[0].id : "";
        }
        if(!moduleViewAllRights_Modified(securitymodule.TIMESHEET, this.props.viewAllRightsCompanies)){
            if(required(searchFields.timesheetContractHolderCompany) && required(searchFields.timesheetOperatingCompany)){
                searchFields.loggedInCompanyCode = this.props.selectedCompany;
                searchFields.isOnlyViewTimesheet = true;              
            }
            // if(!required(searchFields.timesheetContractHolderCompany) && !required(searchFields.timesheetOperatingCompany) &&
            // searchFields.timesheetContractHolderCompany !== this.props.selectedCompany && searchFields.timesheetOperatingCompany !== this.props.selectedCompany){
            //     this.props.actions.ClearTimesheetSearchResults();
            //     return false;
            // }
            if(!required(searchFields.timesheetContractHolderCompany) && required(searchFields.timesheetOperatingCompany) && searchFields.timesheetContractHolderCompany !== this.props.selectedCompany){
                searchFields.timesheetOperatingCompany = this.props.selectedCompany;
            }
            if(required(searchFields.timesheetContractHolderCompany) && !required(searchFields.timesheetOperatingCompany) && searchFields.timesheetOperatingCompany !== this.props.selectedCompany){
                searchFields.timesheetContractHolderCompany = this.props.selectedCompany;
            }
        }

        if(!required(searchFields.timesheetCategory)){
            const categoryMandatoryValues=[];
            let isAlreadAdded = false;
            if(!required(searchFields.timesheetSubCategory)){
                if(required(searchFields.timesheetService)){
                    if (!isAlreadAdded) {
                    categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                    isAlreadAdded = true;
                    }
                }
            }
            else{
                if (!isAlreadAdded) {
                categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                isAlreadAdded = true;
                }
            }
            if(categoryMandatoryValues.length>0){
                this.setState({
                    errorList: categoryMandatoryValues
                });
                return false;
            }
        }

        //search optimization
        if(isLoadMore){
            searchFields.OffSet=this.state.timesheetSearchList?this.state.timesheetSearchList.length:0;
            searchFields.timesheetSearchTotalCount=this.state.visitSearchTotalCount;
        }

        const $this = this;
        this.props.actions.FetchTimesheetSearchResults(searchFields).then((res) => {
            if(res){
                if (!isLoadMore) {
                    this.setState({
                        timesheetSearchTotalCount: res.recordCount,
                        timesheetSearchList:[]
                    });
                    if($this.childSearchFilter && $this.childSearchFilter.current){
                        $this.childSearchFilter.current.panelClick();
                    }
                }
                const tempData = Object.assign([], this.state.timesheetSearchList);
                if (Array.isArray(this.props.timesheetList) && this.props.timesheetList.length > 0)
                    tempData.push(...this.props.timesheetList);
                this.setState({
                    timesheetSearchList: tempData
                });
            }
        });
    }

    csvExportClick = async (event, done) => {
        event.preventDefault();
        
        const searchFields = { ...this.state.timesheetSearchFilter };
        searchFields.isExport = true;
        searchFields.timesheetCustomerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        searchFields.timesheetCustomerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';  
        if(!isEmpty(searchFields.timesheetStartDate)){
            searchFields.timesheetStartDate= moment(searchFields.timesheetStartDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }
        if(!isEmpty(searchFields.timesheetEndDate)){
        searchFields.timesheetEndDate= moment(searchFields.timesheetEndDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }

        const { timesheetStartDate, timesheetEndDate } = searchFields;
         
        if(timesheetStartDate &&   !this.validateDate(timesheetStartDate) ){
            IntertekToaster(localConstant.validationMessage.TIMESHEET_SEARCH_EARLIEST_DATE, 'warningToast gdStartDateVal');
            return false;
        }
        if(timesheetEndDate && !this.validateDate(timesheetEndDate)){
            IntertekToaster(localConstant.validationMessage.TIMESHEET_SEARCH_LATEST_DATE, 'warningToast gdStartDateVal');
            return false;
        }

        const isValid = validateObjectHasValue(searchFields,Object.keys(applicationConstants.timesheetSearchValidateFields));
        if (!isValid) {
            this.setState({
                errorList: Object.values(applicationConstants.timesheetSearchValidateFields)
            });
            return false;
        }

        if(!moduleViewAllRights_Modified(securitymodule.TIMESHEET, this.props.viewAllRightsCompanies)){
            if(required(searchFields.timesheetContractHolderCompany) && required(searchFields.timesheetOperatingCompany)){
                searchFields.loggedInCompanyCode = this.props.selectedCompany;
                searchFields.isOnlyViewTimesheet = true;
            }
            if(!required(searchFields.timesheetContractHolderCompany) && !required(searchFields.timesheetOperatingCompany) &&
            searchFields.timesheetContractHolderCompany !== this.props.selectedCompany && searchFields.timesheetOperatingCompany !== this.props.selectedCompany){
                this.props.actions.ClearTimesheetSearchResults();
                return false;
            }
            if(!required(searchFields.timesheetContractHolderCompany) && required(searchFields.timesheetOperatingCompany) && searchFields.timesheetContractHolderCompany !== this.props.selectedCompany){
                searchFields.timesheetOperatingCompany = this.props.selectedCompany;
            }
            if(required(searchFields.timesheetContractHolderCompany) && !required(searchFields.timesheetOperatingCompany) && searchFields.timesheetOperatingCompany !== this.props.selectedCompany){
                searchFields.timesheetContractHolderCompany = this.props.selectedCompany;
            }
        }
        this.props.actions.FetchTimesheetSearchResults(searchFields).then((res) => {
            if (res && res.result && res.result.timesheetSearch && res.result.header) {
                // this.setState({
                //     headers:res.headers
                // });
                // this.headers=res.headers;
                this.csvLink.link.click();
            }
        });
    }

    resetAssignment = (e) => {
        const resetField = { ...defaultFilterData };
        this.setState({
            timesheetSearchFilter: resetField,
            timesheetSearchTotalCount: 0,
            timesheetSearchList: []
        });
        this.props.actions.ClearTimesheetSearchResults();
        this.props.actions.ClearData();
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearServices();

        this.props.actions.FetchCoordinatorForContractHoldingCompanyForSearch();
        this.props.actions.FetchCoordinatorForOperatingCompanyForSearch();

        let isChanged = false;
        this.searchFields.subComponents.forEach(componentProps => {
            if (componentProps.properties && componentProps.properties.name === 'timesheetSubCategory' || componentProps.properties.name === 'timesheetService') {
                componentProps.properties.disabled = true;
                isChanged = true;
                return true;
            }
        });
        if (isChanged) {
            this.setState({
                searchFields: this.searchFields
            });
        }

    }

    loadMoreClick=(data)=>{
        this.searchTimesheet(data,true);
    }
    
    render() {
        const subTitle = this.getPanelSubTitle();
        const { 
            companyList,
            contractHoldingCompanyCoordinators,
            operatingCompanyCoordinators,
            documentTypes,
            taxonomyCategory,
            techSpecServices,
            techSpecSubCategory } = this.props;
        return (
            <div className="customCard">
                <SearchFilter colSize="s12"
                    isArrow={true}
                    heading={`${ localConstant.timesheet.SEARCH_TIMESHEET } : `}
                    subtitle={subTitle}
                    searchFields={this.searchFields}
                    setData={
                        this.state.timesheetSearchFilter
                         } 
                    // setData={{
                    //     timesheetSearchField:this.state.timesheetSearchField,
                    //     timesheetEndDate: this.state.selectedLatestDate,
                    //     timesheetStartDate: this.state.selectedEarliestDate,
                    // }}
                    filterFieldsData={{ 
                        timesheetStatus: localConstant.commonConstants.timesheet_Status,
                        companyList,
                        contractHoldingCompanyCoordinators,
                        operatingCompanyCoordinators,
                        documentTypes,
                        taxonomyCategory,
                        techSpecServices,
                        techSpecSubCategory
                        // csvData:this.props.timesheetList&&this.props.timesheetList.timesheetSearch?this.props.timesheetList.timesheetSearch:[], 
                        // csvHeader:this.props.timesheetList&&this.props.timesheetList.header?this.props.timesheetList.header:[],  
                    }}
                    resetHandler={(e)=>this.resetAssignment(e)}
                    submitHandler={(e)=>this.searchTimesheet(e)}
                    ref={this.childSearchFilter}
                    csvRef={(r) => this.csvLink = r}
                />
                <ReactGrid
                    gridRowData={this.state.timesheetSearchList}
                    gridColData={HeaderData} 
                    loadMoreClick={this.loadMoreClick} 
                totalCount={this.state.timesheetSearchTotalCount}
                />
                <Modal title={localConstant.commonConstants.CHECK_ANY_MANDATORY_FIELD}
                    titleClassName="chargeTypeOption"
                    modalContentClass="extranetModalContent"
                    modalId="errorListPopup"
                    formId="errorListForm"
                    buttons={this.modelBtns.errorListButton}
                    isShowModal={this.state.errorList.length > 0}>
                    <ErrorList errors={this.state.errorList} />
                </Modal> 
            </div>
        );
    }
}

export default TimesheetSearch;

TimesheetSearch.propTypes = {
};

TimesheetSearch.propTypes = {
    
};