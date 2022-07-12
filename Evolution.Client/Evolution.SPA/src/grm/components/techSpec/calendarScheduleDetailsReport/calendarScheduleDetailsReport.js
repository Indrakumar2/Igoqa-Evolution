import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import CustomerAndCountrySearch from '../../../../components/applicationComponents/customerAndCountrySearch';
import { HeaderData, supplierSearchHeader } from './headerData';
import { getlocalizeData, isEmpty, isEmptyReturnDefault, addElementToArrayParam } from '../../../../utils/commonUtils';
import moment from 'moment';
import PropTypes from 'prop-types';
import 'jspdf-autotable';
import arrayUtil from '../../../../utils/arrayUtil';
import InputWithPopUpSearch from '../../../../components/applicationComponents/inputWithPopUpSearch';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

/** Calendar Schedule SearchFilter starts */
const CalendarScheduleReportFilter = (props) => (
    <form id ="contractSearch"

        onSubmit={props.searchCalendarScheduleReports}
        onReset={props.clearSearchData} autoComplete="off">
        <div className="row mb-0">
        <CustomMultiSelect
                hasLabel={true}
                colSize='s3'
                name="companyList"
                label={localConstant.techSpec.common.COMPANY}
                type='multiSelect'
                labelClass="customLabel mandate"
                inputClass="customInputs"
                optionsList={props.companyList}
                optionLabelName={'companyName'}
                className="browser-default"
                defaultValue={props.selectedCompanies}
                multiSelectdValue={props.onCompanyChange}
                />

            <CustomInput
                hasLabel={true}
                name="year"
                colSize='s3'
                label={localConstant.techSpec.common.YEAR}
                type='select'
                labelClass="customLabel mandate"
                inputClass="customInputs"
                optionsList={props.year}
                optionName='name'
                optionValue="value"
                defaultValue={ props.defaultYear }
                onSelectChange={props.handlerChange}
               />
          
            <CustomInput
                hasLabel={true}
                name="month"
                colSize='s3'
                label={localConstant.techSpec.common.MONTH}
                type='select'
                labelClass="customLabel mandate"
                className="browser-default customInputs"
                optionsList={props.month}
                optionName='name'
                optionValue="value"
                onSelectChange={props.handlerChange}
                />
            </div>
            <div className="row mb-0">
            <CustomerAndCountrySearch divClassName="s3 pr-0" isSupplier={true} />

            <InputWithPopUpSearch
                colSize='col s3 pl-0'
                label={localConstant.techSpec.common.SUPPLIER_NAME}
                headerData={props.supplierHeader}
                name="supplierName"
                searchModalTitle={localConstant.supplierpo.SUPPLIER_LIST}
                gridRowData={props.modalRowData}
                defaultInputValue={props.selectedSupplier ? props.selectedSupplier : ''}
                onAddonBtnClick={props.supplierPopupOpen}
                onModalSelectChange={props.getMainSupplier}
                onInputBlur={props.getMainSupplier}
                onSubmitModalSearch={props.getSelectedSupplier}
                handleInputChange ={props.getSelectedSupplier}
                callBackFuncs={props.callBackFuncs}
                />

            <CustomInput
                hasLabel={true}
                colSize='s3'
                name="projectNumber"
                label={localConstant.techSpec.common.PROJECT_NO}
                type='text'
                dataType= 'numeric'
                valueType='value'
                inputClass="customInputs"
                onValueChange={props.handlerChange}
                />

            <CustomInput
                hasLabel={true}
                colSize='s3'
                name="assignmentNumber"
                label={localConstant.techSpec.common.ASSIGNMENT_NO}
                type='text'
                dataType= 'numeric'
                valueType='value'
                inputClass="customInputs"
                onValueChange={props.handlerChange}
                />
                </div>
            <div className="row mb-0">
            <CustomMultiSelect
                hasLabel={true}
                colSize='s3'
                name="chCoordinator"
                label={localConstant.techSpec.common.CH_COORDINATOR}
                type='multiSelect'
                labelClass="customLabel"
                inputClass="customInputs"
                optionsList={props.contractHoldingCompanyCoordinators}
                optionLabelName={'displayName'}
                className="browser-default"
                defaultValue={props.selectedCHCoordinators}
                multiSelectdValue={props.onCHCoordinatorsChange}
                disabled={props.selectedCompanies && props.selectedCompanies.length >0 ? false : true}
                />

            <CustomMultiSelect
                hasLabel={true}
                colSize='s3'
                name="ocCoordinator"
                label={localConstant.techSpec.common.OC_COORDINATOR}
                type='multiSelect'
                labelClass="customLabel"
                inputClass="customInputs"
                optionsList={props.contractHoldingCompanyCoordinators}
                optionLabelName={'displayName'}
                className="browser-default"
                defaultValue={props.selectedOCCoordinators}
                multiSelectdValue={props.onOCCoordinatorsChange}
                disabled={props.selectedCompanies && props.selectedCompanies.length >0 ? false : true}
                />

            <CustomMultiSelect
                hasLabel={true}
                colSize='s3'
                name="resourceName"
                label={localConstant.techSpec.common.RESOURCE_NAME}
                type='multiSelect'
                labelClass="customLabel"
                inputClass="customInputs"
                optionsList={props.resourceNameList}
                optionLabelName={'fullName'}
                className="browser-default"
                defaultValue={props.selectedResources}
                multiSelectdValue={props.onResourceChange}
                disabled={props.selectedCompanies && props.selectedCompanies.length >0 ? false : true}
                />
            
            <CustomMultiSelect
                hasLabel={true}
                colSize='s3'
                name="ePIN"
                label={localConstant.techSpec.common.EPIN}
                type='multiSelect'
                labelClass="customLabel"
                inputClass="customInputs"
                optionsList={props.ePinList}
                optionLabelName={'pin'}
                className="browser-default"
                defaultValue={props.selectedEpins}
                multiSelectdValue={props.onEPinChange}
                disabled={props.selectedCompanies && props.selectedCompanies.length >0 ? false : true}
                />
            </div>
            <div className="row mb-0 mr-2 right">
            <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">Search</button>
            <button type="reset" className="mt-4x modal-close waves-effect waves-green btn" >Reset</button>
            </div>
    </form>
);
/**Calendar Schedule Search Filter Ends */

class CalendarScheduleDetailsReport extends Component {
    constructor(props) {
       
        super(props);
        this.state = {
            isPanelOpen: true,
            selectedCompanies:[],
            selectedCHCoordinators:[],
            selectedOCCoordinators:[],
            selectedResources:[],
            selectedEpins:[],
            defaultYear: '',
            supplierName: '',
            selectedSupplier: '',
            resourceList:[],
        };
        this.updatedData = {};
        this.yearObj={};
        this.headerData = HeaderData();
        this.supplierHeader = supplierSearchHeader;
  
        this.callBackFuncs ={
            onReset:()=>{}
          };
    }

    /**to do ComponentDidMount */
    componentDidMount() {
        this.findYearLOV();
    }

    /**panel click action */
    panelClick = (e) => {
        this.setState((state) => {
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }

    onCompanyChange = (e) => {
        this.setState({ selectedCompanies: e });
        const companies = e;
        const companyList = [];
        const companyIDs = [];
        const isActive = true;
        if(!isEmpty(companies)) {
            companies.forEach(x => {
                companyList.push(x.companyCode);
                companyIDs.push(x.id);
            });
        }
        const coordinatorParams = {
            companyCodes: companyList ,
            userTypes: [ "MICoordinator" ],
            isActiveCoordinators: true
        };
        this.props.actions.FetchContractHoldingCoordinator(coordinatorParams);
        this.props.actions.FetchResourceInfo(companyIDs, isActive);
    }

    onCHCoordinatorsChange = (e) => {
        this.setState({ selectedCHCoordinators:e });
    }

    onOCCoordinatorsChange = (e) => {
        this.setState({ selectedOCCoordinators:e });
    }

    onResourceChange = (e) => {
        this.setState({ selectedResources:e });
    }

    onEPinChange = (e) => {
        this.setState({ selectedEpins:e });
    }

    findYearLOV() {
        const month = moment().month();
        const yearLOV=[];
        let year = moment().year();
        this.setState({ defaultYear: year });
        this.updatedData.year = year;
        let j = 0;
        if(month>5){
            year = year + 1;
            for(let i=year;i>=year-6;i--){
                yearLOV[j] = { name:i, value:i };
                j++;
            }
        }
        else{
            for(let i=year;i>=year-5;i--){
                yearLOV[j] = { name:i, value:i };
                j++;
            }
        }
        this.yearObj = { year: yearLOV };
    }

    supplierPopupOpen = (data) => {
        this.props.actions.ClearSupplierSearchList();
    }

    getMainSupplier = (data) => { 
        const params = { 
            supplierName: data.serachInput,
            country: data.selectedCountry
        };
        this.setState({ supplierName: data.serachInput }); 
        if (data.serachInput || data.selectedCountry ) {
            this.props.actions.FetchSupplierSearchList(params);
        }
        else { 
            this.props.actions.ClearSupplierSearchList();
        }
    }
    getSelectedSupplier = (data) => {
        if (data) {
            this.setState({ selectedSupplier:data && Array.isArray(data) && data.length > 0 && data[0].supplierName });
        }
    }

    /**
     *  Schedule Details Search Action
     */
    searchCalendarScheduleReports =(e)=>{
        e.preventDefault();
        this.updatedData.selectedCompanies = this.state.selectedCompanies;
        this.updatedData.customerName = this.props.defaultCustomerName;
        this.updatedData.supplierName = this.state.selectedSupplier;
        if(isEmpty(this.updatedData.selectedCompanies)){
            IntertekToaster(localConstant.techSpec.common.REPORT_COMPANY_VALIDATION, 'warningToast companyNotSelected');
            return false;
        }
        if(this.updatedData.year === undefined || this.updatedData.year === ''){
            IntertekToaster(localConstant.techSpec.common.REPORT_YEAR_VALIDATION, 'warningToast yearNotSelected');
            return false;
        }
        if(this.updatedData.month === undefined || this.updatedData.month === ''){
            IntertekToaster(localConstant.techSpec.common.REPORT_MONTH_VALIDATION, 'warningToast monthNotSelected');
            return false;
        }
        this.updatedData["chCoordinator"]=[];
        if(!isEmpty(this.state.selectedCHCoordinators)){
            this.state.selectedCHCoordinators.forEach(x => {
                this.updatedData["chCoordinator"].push(x.displayName);
            });
        }
        this.updatedData["ocCoordinator"]=[];
        if(!isEmpty(this.state.selectedOCCoordinators)){
            this.state.selectedOCCoordinators.forEach(x => {
                this.updatedData["ocCoordinator"].push(x.displayName);
            });
        }
        this.updatedData["resourceEpins"]=[];
        if (!isEmpty(this.state.selectedResources)) {
            this.state.selectedResources.forEach(x => {
                this.updatedData["resourceEpins"].push(x.pin);
            });
        }
        this.updatedData["epinList"]=[];
        if (!isEmpty(this.state.selectedEpins)) {
            this.state.selectedEpins.forEach(x => {
                this.updatedData["epinList"].push(x.pin);
            });
        }
        this.props.actions.FetchCalendarScheduleDetails(this.updatedData);
    }

    clearSearchData=()=>{
        this.setState({ selectedCompanies: [], selectedSupplier: '', selectedCHCoordinators:[], selectedOCCoordinators:[],
                        selectedResources:[], selectedEpins:[] });
        this.props.actions.ClearSupplierSearchList();
        this.props.actions.ClearData();
        this.props.actions.ClearSearchData();
        this.updatedData = {};
        this.callBackFuncs.onReset();
    }

    /**Handler change */
    handlerChange =(e)=> {
        this.updatedData[e.target.name] = e.target.value;
    }

    render() {
     const { companyList, coordinators,  calendarSchedulesData, supplierList } = this.props;
        return (
            <div className="companyPageContainer customCard">
                <Panel colSize="s12"
                    isArrow={true}
                    heading={`${
                        localConstant.techSpec.common.CALENDAR_SCHEDULE_DETAILS_REPORT
                        }`}
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} >

                    <CalendarScheduleReportFilter                    
                        clearSearchData={this.clearSearchData}   
                        handlerChange={ (e) => this.handlerChange(e)}
                        companyList={!isEmpty(companyList) ? addElementToArrayParam(companyList,'companyName','companyName') :[] }
                        resourceNameList={!isEmpty(this.props.companyBasedTSData) ? addElementToArrayParam(arrayUtil.sort(this.props.companyBasedTSData,'fullName','asc'), 'fullName','fullName'):[]}
                        ePinList={!isEmpty(this.props.companyBasedTSData) ? addElementToArrayParam(this.props.companyBasedTSData,'pin','pin'):[]}
                        contractHoldingCompanyCoordinators={!isEmpty(coordinators)? addElementToArrayParam(coordinators,'displayName','displayName'):[]}
                        // operatingCompanyCoordinators={!isEmpty(coordinators) ? addElementToArrayParam(coordinators,'displayName','displayName'): []}
                        month={localConstant.commonConstants.month}
                        year={this.yearObj.year}
                        onCompanyChange={(e) => this.onCompanyChange(e)}
                        onCHCoordinatorsChange = {(e) => this.onCHCoordinatorsChange(e)}
                        onOCCoordinatorsChange = {(e) => this.onOCCoordinatorsChange(e)}
                        onResourceChange = {(e) => this.onResourceChange(e)}
                        onEPinChange = {(e) => this.onEPinChange(e)}
                        selectedCompanies ={this.state.selectedCompanies}
                        selectedCHCoordinators = {this.state.selectedCHCoordinators}
                        selectedOCCoordinators = {this.state.selectedOCCoordinators}
                        selectedResources = {this.state.selectedResources}
                        selectedEpins = {this.state.selectedEpins}
                        defaultYear = {this.state.defaultYear}
                        searchCalendarScheduleReports={(e)=> this.searchCalendarScheduleReports(e)}
                        supplierPopupOpen={data => this.supplierPopupOpen(data)}
                        supplierHeader={this.supplierHeader}
                        getMainSupplier={data => this.getMainSupplier(data)}
                        modalRowData={supplierList}
                        getSelectedSupplier={data => this.getSelectedSupplier(data)}
                        selectedSupplier={this.state.selectedSupplier}
                        callBackFuncs={this.callBackFuncs}
                    />

                </Panel>

                <div className="customCard"> 
   
                <ReactGrid
                    gridRowData={calendarSchedulesData}
                    gridColData={this.headerData} />
                    </div>
           
            </div>
           
        );
    };
};

export default CalendarScheduleDetailsReport;

CalendarScheduleDetailsReport.propTypes = {
    companyList: PropTypes.array,
};

CalendarScheduleDetailsReport.defaultProps = {
    companyList: [],
};