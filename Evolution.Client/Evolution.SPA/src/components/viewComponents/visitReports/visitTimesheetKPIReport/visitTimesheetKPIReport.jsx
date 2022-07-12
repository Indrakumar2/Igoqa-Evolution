import React, { Component, Fragment } from 'react';
import {
    getlocalizeData,
    GenerateReport,
    ObjectIntoQuerySting,
    isEmpty
} from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import moment from 'moment';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import TechSpecSearch from '../../../technicalSpecialistSearch';
import { HeaderData } from './headerData';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import { EvolutionReportsAPIConfig  } from '../../../../apiConfig/apiConfig';
import PropTypes from 'prop-types';

const localConstant = getlocalizeData();

const TechnicalSpecialistPopup = (props) => {
    return(
        <TechSpecSearch
            headerData={HeaderData.techSpecSearch}
            gridRef={props.gridRef}
            selectedLastName = {props.selectedLastName}
            selectedCompany = {props.selectedCompany}
            selectedCompanyName= {props.selectedCompanyName}
           />
    );
};

class VisitKPIReportModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
           isTechSpecModalOpen : false,
           selectedFromDate:'',
           selectedToDate:'',
        };
        this.selectedLastName = '';
        this.selectedCompany = '';
        this.selectedCompanyName = '';
        this.updatedData = {};
        this.reportParam= {};
        this.reportParam['format'] = localConstant.commonConstants.PDF;
        this.selectedResourceName = []; 
        this.contractValues = [];
        this.excelDatas = {

        };
        this.StartDateLabel='';
        this.EndDateLabel='';
    }

    openTechSpec() {
        this.setState({ isTechSpecModalOpen:true });
    }

    componentDidMount() {
        const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter( x=>x.companyCode===this.props.selectedCompany );
        const loggedInCompanyId = companyId[0].companyId;
        let isOperating = false;
        let _isVisit = false;
        if(this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC || this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_OC){
          isOperating = true;
        }

        if(this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC || this.props.title === localConstant.visit.VISITS_KPI_REPORT_CHC){
            _isVisit = true;
            this.StartDateLabel = localConstant.visit.EARILIEST_VISIST_DATE;
            this.EndDateLabel = localConstant.visit.LAST_VISIST_DATE;
        }
        else{
            this.StartDateLabel = localConstant.visit.EARILIEST_TIMESHEET_DATE;
            this.EndDateLabel = localConstant.visit.LATEST_TIMESHEET_DATE;
        }
  
        const data = {
            "ContractHolderCompanyId":loggedInCompanyId,
            "isVisit":_isVisit,
            "isOperating":isOperating
        };
        this.props.actions.ClearCustomerContracts();
        this.selectedCompany = this.props.selectedCompany;
        this.selectedCompanyName = companyId[0].companyName;
        this.props.actions.FetchKPICustomer(data);
    }

    componentWillUnmount() {
        this.props.actions.ClearCustomerContracts();
    }

    formatReportDate(formatedDate) {
        const finalFormatedDate = moment(formatedDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        const RDate = finalFormatedDate.split('-'); // this fix was given due to firefox date conversion issue
        const RYear = RDate[0] == '' ? RDate[1] : RDate[0];
        const RMonth = RDate[0] == '' ? RDate[2] : RDate[1];
        const RDay = RDate[0] == '' ? RDate[3] : RDate[2];
        return new Date(RYear + '-' + RMonth + '-' + RDay);
    }

    exportVisitKPIReportHandler = async () => {
       const _localStorage = localStorage.getItem('Username');
       this.reportParam['username'] = _localStorage;
       let reportFileName = '';
       let extension = '.xls';
       let content_type = 'application/vnd.ms-excel';

       const SpecialistId = this.reportParam['SpecialistId'];
       const CustomerId = this.reportParam['CustomerId'];
       const ContractId = this.reportParam['ContractId'];
       const ProjectId = this.reportParam['ProjectId'];
       const VisitStartDate = this.reportParam['VisitStartDate'];
       const VisitEndDate = this.reportParam['VisitEndDate'];
       
       if(CustomerId == undefined && ContractId == undefined && ProjectId == undefined && VisitStartDate == undefined && VisitEndDate == undefined && SpecialistId == undefined){
           IntertekToaster(localConstant.commonConstants.ERR_REPORT, 'warningToast ssrs_report_message');
       }
       else{
           
           if (VisitStartDate && VisitEndDate) {
               const date1 = this.formatReportDate(VisitStartDate);
               const date2 = this.formatReportDate(VisitEndDate);
               if (date1 > date2) {
                   IntertekToaster(localConstant.commonConstants.ERR_REPORT_DATE, 'warningToast ssrs_report_message');
                   return false;
               }
           }

           let ContractHolderCompany = false;
           const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter(x => x.companyCode === this.props.selectedCompany);
           const loggedInCompanyId = companyId[0].companyId;
           if (this.reportParam['format'] == localConstant.commonConstants.PDF) {
               extension = '.pdf';
               content_type = 'application/pdf';
           }
           if (this.props.title === localConstant.visit.VISITS_KPI_REPORT_CHC || this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_CHC) {
               this.reportParam['OperatingCompanyId'] = undefined;
               this.reportParam['ContractHolderCompanyId'] = loggedInCompanyId;
               ContractHolderCompany = true;
           }
           else if (this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC || this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_OC) {
               this.reportParam['OperatingCompanyId'] = loggedInCompanyId;
               this.reportParam['ContractHolderCompanyId'] = undefined;
               ContractHolderCompany = false;
           }

           if (this.props.title === localConstant.visit.VISITS_KPI_REPORT_CHC || this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC) {
               this.reportParam['reportname'] = EvolutionReportsAPIConfig.VisitsKPIReport;
               reportFileName = "VisitsKPIReport";
               this.reportParam['reptype'] = 2;
           }
           else if (this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_CHC || this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_OC) {
               this.reportParam['reportname'] = EvolutionReportsAPIConfig.TimesheetKPIReport;
               reportFileName = "TimesheetKPIReport";
               this.reportParam['reptype'] = 3;
           }
           this.reportParam['background'] = true;
           this.reportParam["ContractHolderCompany"] = ContractHolderCompany;
           DownloadReportFile(this.reportParam, content_type, reportFileName, extension, this.props);
       }
    }

    cancelTechSpecHandler = (e) => {
        e.preventDefault();
        this.props.actions.TechSpecClearSearch();
        this.setState({ isTechSpecModalOpen:false });
        this.selectedLastName = '';
        this.reportParam.SpecialistId = undefined;
        this.selectedResourceName = '';
        document.getElementById('resourceName').value = '';
    };

    getTechSpec = (e) => {
        e.preventDefault();
        const selectedResource = this.child.getSelectedRows();
        if(selectedResource.length> 0) {
                selectedResource.forEach(iteratedValue => {
                 this.selectedResourceName = `${ iteratedValue.lastName } ${ iteratedValue.firstName } ${ iteratedValue.epin }`;
                 this.reportParam['SpecialistId'] = (iteratedValue.epin).toString();
                }),
                document.getElementById('resourceName').value = this.selectedResourceName;
                this.props.actions.TechSpecClearSearch();
                this.setState({
                    isTechSpecModalOpen:false
                });
            }
        else{
            IntertekToaster(localConstant.visit.validationMessages.RESOURCE, 'warningToast resourceNameReq');
        }  
    }

    techSpecButtons = [
        {
            name: localConstant.commonConstants.CANCEL,
            action: this.cancelTechSpecHandler,
            btnID: "cancelTechSpec",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true,
            type:"button"
        },
        {
            name: localConstant.commonConstants.SUBMIT,
            action: this.getTechSpec,
            type: 'submit',
            btnID: "addTechSpec",
            btnClass: "waves-effect waves-teal btn-small ",
            showbtn: true
        }
    ];

     onFromDateChange =(date)=>{
        if (isEmpty(date)) {
            this.setState({
                selectedFromDate: ""
            }, () => {
                this.reportParam.VisitStartDate = undefined;
            });
        }
        else{ 
        this.setState({
            selectedFromDate: moment(date)
        }, () => {
            this.reportParam.VisitStartDate=moment(this.state.selectedFromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        });
       }
    }
    onToDateChange =(date)=>{
        if (isEmpty(date)) {
            this.setState({
                selectedToDate: ""
            }, () => {
                this.reportParam.VisitEndDate = undefined;
            });
        }
        else{ 
        this.setState({
            selectedToDate: moment(date)
        }, () => {
            this.reportParam.VisitEndDate=moment(this.state.selectedToDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        });
      }
    }

    resourceValueBlur = (e) => {
        //lastName
        if(e.target.value !== ""){
            this.selectedLastName = e.target.value + "*";
            this.setState({ isTechSpecModalOpen:true });
        }
        else if(e.target.value === "" && this.reportParam.SpecialistId !== undefined){
            this.reportParam.SpecialistId = undefined;
        }
    }

    handlerChange = (e) => {
        const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter(x => x.companyCode === this.props.selectedCompany);
        const loggedInCompanyId = companyId[0].companyId;
        let isOperating = false;
        let _isVisit = false;
        if (this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC || this.props.title === localConstant.timesheet.TIMESHEET_KPI_REPORT_OC) {
            isOperating = true;
        }

        if (this.props.title === localConstant.visit.VISITS_KPI_REPORT_OC || this.props.title === localConstant.visit.VISITS_KPI_REPORT_CHC) {
            _isVisit = true;
        }

        if (e.target.value != undefined && e.target.value != '') {
            if (e.target.name === "customers") {
                this.reportParam["CustomerId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                this.props.actions.ClearCustomerContracts();
                const data = {
                    "customerCode": e.target.value,
                    "ContractHolderCompanyId": loggedInCompanyId,
                    "isOperating": isOperating,
                    "isVisit": _isVisit,
                };
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearContractProjects();
                this.props.actions.ClearCustomerContracts();
                this.props.actions.FetchKPIVisitTimesheetContract(data);
            }
            else if (e.target.name === "contracts") {
                this.reportParam["ContractId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                const Params = {
                    "contractNumber": e.target.value,
                    "ContractHolderCompanyId": loggedInCompanyId,
                    "isVisit": _isVisit,
                    "isOperating": false
                };
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearContractProjects();
                this.props.actions.FetchKPIVisitTimesheetProject(Params);
            }
            else if (e.target.name === "projects") {
                this.reportParam["ProjectId"] = e.target.value;
            }
            else if (e.target.name === "Format") {
                this.reportParam["format"] = e.target.value;
            }
        }
        else {
            if (e.target.name === "customers") {
                this.reportParam["CustomerId"] = undefined;
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearContractProjects();
                this.props.actions.ClearCustomerContracts();
            }
            else if (e.target.name === "contracts") {
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearContractProjects();
            }
            else if (e.target.name === "projects") {
                this.reportParam["ProjectId"] = undefined;
            }
            else if (e.target.name === "Format") {
                this.reportParam["format"] = undefined;
            }
        }
    }

    mergedProjects = (projects) => { 
        const mergedData= Array.isArray(projects) && projects.map(items=>
            {
            items['mergedProjectData']=`${ items.projectNumber } ${ "|" } ${ items.customerProjectNumber } ${ "|" } ${ items.customerProjectName }`;
            return items;
            });
         return mergedData;
    }

    render() {
        const { contractList, projectList } = this.props;
        const projectValue = this.mergedProjects(projectList);
        return (
            <Fragment>
                <Modal id="reportId"
                    title={ this.props.title }
                    titleClassName='bold'
                    buttons={[
                        {
                            name: 'Cancel',
                            action: this.props.hidemodal,
                            type: "button",
                            btnClass: 'btn-small modal-close mr-2',
                            showbtn: true
                        },
                        {
                            name: 'View',
                            type: "button",
                            action: this.exportVisitKPIReportHandler,
                            btnClass: 'btn-small modal-close',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.props.showmodal}>
                    <div className="row">
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='customers'
                            divClassName='col'
                            label={localConstant.customer.CUSTOMER}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            customId='customerId'
                            optionName='customerName'
                            optionValue="customerCode"
                            optionsList={ this.props.customerData }
                            autocomplete="off"
                        />
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='contracts'
                            divClassName='col'
                            label={localConstant.contract.CONTRACTS}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            customId='id'
                            optionName= "mergedContractData"
                            optionValue= "contractNumber"
                            optionsList={ contractList }
                            autocomplete="off"
                        />
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='projects'
                            divClassName='col'
                            label={localConstant.project.PROJECTS}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            optionName='mergedProjectData'
                            optionValue="projectNumber"
                            optionsList={ projectValue  }
                            autocomplete="off"
                        />
                        <CustomInput
                            hasLabel={true}
                            colSize='s3'
                            isNonEditDateField={false}
                            name="visitStartDate"
                            label={this.StartDateLabel}
                            labelClass="customLabel"
                            type='date'
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            selectedDate={this.state.selectedFromDate}
                            onDateChange={this.onFromDateChange}
                            shouldCloseOnSelect={true}
                        />
                        <CustomInput
                            hasLabel={true}
                            colSize='s3'
                            isNonEditDateField={false}
                            name="visitEndDate"
                            label={this.EndDateLabel}
                            labelClass="customLabel"
                            type='date'
                            placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                            dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                            selectedDate={this.state.selectedToDate}
                            onDateChange={this.onToDateChange}
                            shouldCloseOnSelect={true}
                        />
                      <div className={this.props.colSize ? this.props.colSize :"col pl-0 "+this.props.divClassName} >
                        <CustomInput
                            //dataValType='valueText'
                            hasLabel={true}
                            divClassName='col customerSearchBox'
                            label={localConstant.assignments.TECHNICAL_SPECIALIST}
                            labelClass="customLabel"
                            htmlFor='resourceName'
                            type='text'
                            colSize='s11 pr-0'
                            name='resourceName'
                            inputClass="customInputs"
                            onValueBlur={(e) => this.resourceValueBlur(e)}
                            maxLength={200}
                            defaultValue={this.selectedResourceName? this.selectedResourceName:''}
                            />
                        <div className="customerSearchButton">
                        <button type="button" className="waves-effect waves-green btn p-2 btn-lineHeight mt-4x" onClick={(e) => this.openTechSpec(e)}>...</button>
                        </div>
                      </div> 
                    </div>
                       <label class='bold'>{localConstant.supplier.FORMAT}</label>
                        <label>
                            <input className="with-gap" onChange={this.handlerChange}
                                value='PDF' name="Format" type="radio" defaultChecked={true}/>
                            <span>{localConstant.commonConstants.PDF}</span>
                       </label>
                       <label>
                            <input className="with-gap"  onChange={this.handlerChange}
                                value='excel' name="Format" type="radio" />
                            <span>{localConstant.commonConstants.EXCEL}</span>
                        </label>
                </Modal>
                {this.state.isTechSpecModalOpen ?
                    <Modal title={localConstant.assignments.SEARCH_RESOURCES}
                     modalId="searchResourcePopup" 
                     formId="searchResourceForm"                    
                     modalClass="searchTechSpecModal" 
                     buttons={this.techSpecButtons} 
                     isShowModal={this.state.isTechSpecModalOpen}>
                        <TechnicalSpecialistPopup 
                           gridRef={ref => { this.child = ref; }}
                           selectedLastName= {this.selectedLastName}
                           selectedCompany= {this.selectedCompany}
                           selectedCompanyName = {this.selectedCompanyName}
                          />
                    </Modal>:null
                }

            </Fragment>
        );
    }
}

export default VisitKPIReportModal;

VisitKPIReportModal.propTypes = {
    contractList: PropTypes.array,
    projectList: PropTypes.array,
};

VisitKPIReportModal.defaultProps = {
    contractList: [],
    projectList: [],
};